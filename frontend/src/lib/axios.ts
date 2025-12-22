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
    // Получаем токен напрямую из store (более надёжно)
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

// Флаг чтобы не вызывать logout много раз
let isLoggingOut = false;

api.interceptors.response.use(
  (response: AxiosResponse) => response,
  (error: AxiosError<unknown>) => {
    const isAuthRequest = error.config?.url?.includes('/auth/');

    if (error.response?.status === 401 && !isAuthRequest && !isLoggingOut) {
      const { token, logout } = useAuthStore.getState();

      // Только если был токен - значит сессия истекла
      if (token) {
        isLoggingOut = true;
        logout();

        if (window.location.pathname !== '/') {
          toast.error('Сессия истекла. Пожалуйста, войдите снова.');
        }

        setTimeout(() => {
          isLoggingOut = false;
        }, 1000);
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