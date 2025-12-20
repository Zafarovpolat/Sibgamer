import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTimes, faUserPlus, faEye, faEyeSlash, faCheck, faXmark } from '@fortawesome/free-solid-svg-icons';
import toast from 'react-hot-toast';
import api from '../lib/axios';
import { getErrorMessage } from '../utils/errorUtils';
import { useAuthStore } from '../store/authStore';

interface RegisterFormData {
  username: string;
  email: string;
  password: string;
  confirmPassword: string;
}

interface RegisterModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSwitchToLogin: () => void;
}

const RegisterModal = ({ isOpen, onClose, onSwitchToLogin }: RegisterModalProps) => {
  const { register, handleSubmit, watch, reset, formState: { errors } } = useForm<RegisterFormData>();
  const [isLoading, setIsLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [usernameStatus, setUsernameStatus] = useState<'idle' | 'checking' | 'available' | 'taken'>('idle');
  const { login } = useAuthStore();

  const username = watch('username');

  useEffect(() => {
    if (!username || username.length < 3) {
      setUsernameStatus('idle');
      return;
    }

    const checkUsername = async () => {
      setUsernameStatus('checking');
      try {
        const response = await api.get(`/auth/check-username?username=${username}`);
        setUsernameStatus(response.data.available ? 'available' : 'taken');
      } catch {
        setUsernameStatus('idle');
      }
    };

    const timer = setTimeout(checkUsername, 500);
    return () => clearTimeout(timer);
  }, [username]);

  const onSubmit = async (data: RegisterFormData) => {
    if (data.password !== data.confirmPassword) {
      toast.error('Пароли не совпадают');
      return;
    }

    if (usernameStatus === 'taken') {
      toast.error('Это имя пользователя уже занято');
      return;
    }

    setIsLoading(true);
    try {
      const registerResponse = await api.post('/auth/register', {
        username: data.username,
        email: data.email,
        password: data.password,
      });
      
      login(registerResponse.data.user, registerResponse.data.token);
      toast.success('Регистрация успешна! Добро пожаловать!');
      reset();
      onClose();
    } catch (error: unknown) {
      const message = getErrorMessage(error);
      if (message?.includes('email')) {
        toast.error('Эта почта уже используется');
      } else if (message?.includes('username')) {
        toast.error('Это имя пользователя уже занято');
      } else {
        toast.error(message || 'Ошибка регистрации');
      }
    } finally {
      setIsLoading(false);
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
            Регистрация
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
            <div className="relative">
              <input
                {...register('username', { 
                  required: 'Обязательное поле',
                  minLength: { value: 3, message: 'Минимум 3 символа' }
                })}
                type="text"
                className="input-field pr-10"
                placeholder="Введите имя пользователя"
              />
              {usernameStatus === 'checking' && (
                <div className="absolute right-3 top-1/2 -translate-y-1/2">
                  <div className="animate-spin rounded-full h-5 w-5 border-t-2 border-b-2 border-highlight"></div>
                </div>
              )}
              {usernameStatus === 'available' && (
                <div className="absolute right-3 top-1/2 -translate-y-1/2 text-green-400">
                  <FontAwesomeIcon icon={faCheck} />
                </div>
              )}
              {usernameStatus === 'taken' && (
                <div className="absolute right-3 top-1/2 -translate-y-1/2 text-red-400">
                  <FontAwesomeIcon icon={faXmark} />
                </div>
              )}
            </div>
            {errors.username && (
              <p className="text-red-500 text-sm mt-1">{errors.username.message}</p>
            )}
            {usernameStatus === 'available' && (
              <p className="text-green-400 text-sm mt-1">Имя пользователя доступно</p>
            )}
            {usernameStatus === 'taken' && (
              <p className="text-red-400 text-sm mt-1">Имя пользователя уже занято</p>
            )}
          </div>

          <div>
            <label className="block text-sm font-medium mb-2 text-gray-300">Email</label>
            <input
              {...register('email', {
                required: 'Обязательное поле',
                pattern: {
                  value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                  message: 'Неверный формат email',
                },
              })}
              type="email"
              className="input-field"
              placeholder="your@email.com"
            />
            {errors.email && (
              <p className="text-red-500 text-sm mt-1">{errors.email.message}</p>
            )}
          </div>

          <div>
            <label className="block text-sm font-medium mb-2 text-gray-300">Пароль</label>
            <div className="relative">
              <input
                {...register('password', {
                  required: 'Обязательное поле',
                  minLength: { value: 6, message: 'Минимум 6 символов' },
                })}
                type={showPassword ? "text" : "password"}
                className="input-field pr-10"
                placeholder="••••••••"
              />
              <button
                type="button"
                onClick={() => setShowPassword(!showPassword)}
                className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-300 transition-colors"
              >
                <FontAwesomeIcon icon={showPassword ? faEyeSlash : faEye} />
              </button>
            </div>
            {errors.password && (
              <p className="text-red-500 text-sm mt-1">{errors.password.message}</p>
            )}
          </div>

          <div>
            <label className="block text-sm font-medium mb-2 text-gray-300">Подтверждение пароля</label>
            <div className="relative">
              <input
                {...register('confirmPassword', {
                  required: 'Обязательное поле',
                  validate: (value) =>
                    value === watch('password') || 'Пароли не совпадают',
                })}
                type={showConfirmPassword ? "text" : "password"}
                className="input-field pr-10"
                placeholder="••••••••"
              />
              <button
                type="button"
                onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-300 transition-colors"
              >
                <FontAwesomeIcon icon={showConfirmPassword ? faEyeSlash : faEye} />
              </button>
            </div>
            {errors.confirmPassword && (
              <p className="text-red-500 text-sm mt-1">{errors.confirmPassword.message}</p>
            )}
          </div>

          <button
            type="submit"
            disabled={isLoading}
            className="btn-primary w-full flex items-center justify-center space-x-2"
          >
            <FontAwesomeIcon icon={faUserPlus} />
            <span>{isLoading ? 'Регистрация...' : 'Зарегистрироваться'}</span>
          </button>
        </form>

        <div className="mt-6 text-center">
          <p className="text-gray-400">
            Уже есть аккаунт?{' '}
            <button
              onClick={() => {
                onClose();
                onSwitchToLogin();
              }}
              className="text-highlight hover:text-blue-400 transition-colors duration-300 font-medium"
            >
              Войти
            </button>
          </p>
        </div>
      </div>
    </div>
  );
};

export default RegisterModal;
