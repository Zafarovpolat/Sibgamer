import { useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTimes, faEnvelope, faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import toast from 'react-hot-toast';
import api from '../lib/axios';
import { getErrorMessage } from '../utils/errorUtils';

interface ForgotPasswordModalProps {
  isOpen: boolean;
  onClose: () => void;
}

const ForgotPasswordModal = ({ isOpen, onClose }: ForgotPasswordModalProps) => {
  const [email, setEmail] = useState('');
  const [loading, setLoading] = useState(false);

  if (!isOpen) return null;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);

    try {
      await api.post('/auth/forgot-password', { email });
      toast.success('Если email существует, мы отправили инструкции по восстановлению пароля');
      setEmail('');
      onClose();
    } catch (error: unknown) {
      toast.error(getErrorMessage(error) || 'Ошибка отправки письма');
    } finally {
      setLoading(false);
    }
  };

  const handleOverlayClick = (e: React.MouseEvent<HTMLDivElement>) => {
    if (e.target === e.currentTarget) {
      onClose();
    }
  };

  return (
    <div 
      className="modal-overlay" 
      onMouseDown={handleOverlayClick}
    >
      <div className="modal-content" onMouseDown={(e) => e.stopPropagation()}>
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-3xl font-bold bg-gradient-to-r from-highlight to-blue-500 bg-clip-text text-transparent">
            Восстановление пароля
          </h2>
          <button
            onClick={onClose}
            className="text-gray-400 hover:text-white transition-colors duration-300"
          >
            <FontAwesomeIcon icon={faTimes} className="text-2xl" />
          </button>
        </div>

        <div className="flex items-center justify-center mb-6">
          <div className="w-20 h-20 bg-gradient-to-br from-highlight/20 to-blue-500/20 rounded-full flex items-center justify-center border-2 border-highlight/30">
            <FontAwesomeIcon icon={faEnvelope} className="text-4xl text-highlight" />
          </div>
        </div>

        <p className="text-gray-400 text-center mb-6">
          Введите ваш email и мы отправим инструкции по восстановлению пароля
        </p>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label htmlFor="email" className="block text-sm font-medium mb-2 text-gray-300">
              Email адрес
            </label>
            <input
              id="email"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="input-field"
              placeholder="your-email@example.com"
              required
              autoFocus
            />
          </div>

          <button
            type="submit"
            disabled={loading}
            className="btn-primary w-full flex items-center justify-center space-x-2"
          >
            <FontAwesomeIcon icon={faEnvelope} />
            <span>{loading ? 'Отправка...' : 'Отправить инструкции'}</span>
          </button>

          <button
            type="button"
            onClick={onClose}
            className="btn-secondary w-full flex items-center justify-center space-x-2"
          >
            <FontAwesomeIcon icon={faArrowLeft} />
            <span>Вернуться к входу</span>
          </button>
        </form>
      </div>
    </div>
  );
};

export default ForgotPasswordModal;
