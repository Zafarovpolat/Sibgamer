import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTimes, faSignInAlt, faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { useMutation } from '@tanstack/react-query';
import toast from 'react-hot-toast';
import api from '../lib/axios';
import axios from 'axios';
import { getErrorMessage } from '../utils/errorUtils';
import { useAuthStore } from '../store/authStore';
import type { LoginCredentials, AuthResponse } from '../types';

interface LoginModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSwitchToRegister: () => void;
  onSwitchToForgotPassword: () => void;
}

const LoginModal = ({ isOpen, onClose, onSwitchToRegister, onSwitchToForgotPassword }: LoginModalProps) => {
  const { login } = useAuthStore();
  const [showPassword, setShowPassword] = useState(false);

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<LoginCredentials>();

  const loginMutation = useMutation({
    mutationFn: async (data: LoginCredentials) => {
      const response = await api.post<AuthResponse>('/auth/login', data);
      return response.data;
    },
    onSuccess: (data) => {
      login(data.user, data.token);
      toast.success(`Добро пожаловать, ${data.user.username}!`);
      reset();
      onClose();
    },
    onError: (error: unknown) => {
      if (axios.isAxiosError(error) && error.response?.status === 403) {
        const errorData = error.response?.data as Record<string, unknown> | undefined;
        const blocked = !!(errorData && typeof errorData.blocked === 'boolean' && (errorData.blocked as boolean) === true);
        const errName = errorData && typeof errorData.error === 'string' ? (errorData.error as string) : undefined;
        if (blocked || errName === 'account_blocked' || errName === 'access_denied') {
          const message = errorData && typeof errorData.message === 'string' ? (errorData.message as string) : undefined;
          toast.error(message || 'Ваш аккаунт заблокирован на данном ресурсе', {
            duration: 6000,
          });
          return;
        }
      }

      const message = getErrorMessage(error) || 'Неверное имя пользователя или пароль';
      toast.error(message);
    },
  });

  const onSubmit = async (data: LoginCredentials) => {
    try {
      await loginMutation.mutateAsync(data);
    } catch (error) {
      console.error('Login error:', error);
    }
  };

  if (!isOpen) return null;

  const handleOverlayClick = (e: React.MouseEvent<HTMLDivElement>) => {
    if (e.target === e.currentTarget) {
      onClose();
    }
  };

  return (
    <div className="modal-overlay" onMouseDown={handleOverlayClick}>
      <div className="modal-content" onMouseDown={(e) => e.stopPropagation()}>
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-3xl font-bold bg-gradient-to-r from-highlight to-blue-500 bg-clip-text text-transparent">
            Вход
          </h2>
          <button
            onClick={onClose}
            className="text-gray-400 hover:text-white transition-colors duration-300"
          >
            <FontAwesomeIcon icon={faTimes} className="text-2xl" />
          </button>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div>
            <label className="block text-sm font-medium mb-2 text-gray-300">Имя пользователя</label>
            <input
              {...register('username', { required: 'Обязательное поле' })}
              type="text"
              className="input-field"
              placeholder="Введите имя пользователя"
              autoComplete="username"
            />
            {errors.username && (
              <p className="text-red-500 text-sm mt-1">{errors.username.message}</p>
            )}
          </div>

          <div>
            <label className="block text-sm font-medium mb-2 text-gray-300">Пароль</label>
            <div className="relative">
              <input
                {...register('password', { required: 'Обязательное поле' })}
                type={showPassword ? 'text' : 'password'}
                className="input-field pr-12"
                placeholder="••••••••"
                autoComplete="current-password"
              />
              <button
                type="button"
                onClick={() => setShowPassword(!showPassword)}
                className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-white transition-colors"
              >
                <FontAwesomeIcon icon={showPassword ? faEyeSlash : faEye} />
              </button>
            </div>
            {errors.password && (
              <p className="text-red-500 text-sm mt-1">{errors.password.message}</p>
            )}
          </div>

          <button
            type="submit"
            disabled={loginMutation.isPending}
            className="btn-primary w-full flex items-center justify-center space-x-2"
          >
            <FontAwesomeIcon icon={faSignInAlt} />
            <span>{loginMutation.isPending ? 'Вход...' : 'Войти'}</span>
          </button>

          <button
            type="button"
            onClick={() => {
              onClose();
              onSwitchToForgotPassword();
            }}
            className="text-sm text-gray-400 hover:text-highlight transition-colors duration-300"
          >
            Забыли пароль?
          </button>
        </form>

        <div className="mt-6 text-center">
          <p className="text-gray-400">
            Нет аккаунта?{' '}
            <button
              onClick={() => {
                onClose();
                onSwitchToRegister();
              }}
              className="text-highlight hover:text-blue-400 transition-colors duration-300 font-medium"
            >
              Зарегистрироваться
            </button>
          </p>
        </div>
      </div>
    </div>
  );
};

export default LoginModal;
