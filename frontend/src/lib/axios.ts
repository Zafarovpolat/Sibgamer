import axios from 'axios';
import type { InternalAxiosRequestConfig, AxiosResponse, AxiosError } from 'axios';
import { useAuthStore } from '../store/authStore';
import toast from 'react-hot-toast';
import { API_URL } from '../config/api';

const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

api.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    const { token } = useAuthStore.getState();

    if (token) {
      config.headers = config.headers || {};
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error: AxiosError) => {
    return Promise.reject(error);
  }
);

// Защита от множественных logout
let isLoggingOut = false;
let lastLogoutTime = 0;

api.interceptors.response.use(
  (response: AxiosResponse) => response,
  (error: AxiosError<unknown>) => {
    const isAuthRequest = error.config?.url?.includes('/auth/');
    const isNotificationRequest = error.config?.url?.includes('/notifications/');

    // Игнорируем 401 для запросов авторизации и уведомлений
    if (error.response?.status === 401 && !isAuthRequest) {
      const now = Date.now();
      const { token, isAuthenticated, logout } = useAuthStore.getState();

      // Только если:
      // 1. Был токен И пользователь считался авторизованным
      // 2. Прошло больше 2 секунд с последнего logout
      // 3. Это не запрос уведомлений (они могут быть отправлены до сохранения токена)
      if (token && isAuthenticated && !isLoggingOut && (now - lastLogoutTime > 2000) && !isNotificationRequest) {
        isLoggingOut = true;
        lastLogoutTime = now;
        logout();

        toast.error('Сессия истекла. Пожалуйста, войдите снова.');

        setTimeout(() => {
          isLoggingOut = false;
        }, 2000);
      }
    }

    if (error.response?.status === 403) {
      const errorData = error.response?.data as Record<string, unknown> | undefined;
      const blocked = errorData && typeof errorData['blocked'] === 'boolean' ? (errorData['blocked'] as boolean) : false;
      const errorCode = errorData && typeof errorData['error'] === 'string' ? (errorData['error'] as string) : undefined;
      const errorMessage = errorData && typeof errorData['message'] === 'string' ? (errorData['message'] as string) : undefined;

      if (blocked || errorCode === 'account_blocked' || errorCode === 'access_denied') {
        const { logout } = useAuthStore.getState();
        logout();

        toast.error(errorMessage || 'Ваш аккаунт заблокирован на данном ресурсе', {
          duration: 8000,
        });

        if (window.location.pathname !== '/') {
          setTimeout(() => {
            window.location.href = '/';
          }, 1000);
        }
      }
    }

    return Promise.reject(error);
  }
);

export default api;