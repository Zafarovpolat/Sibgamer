import { useState } from 'react';
import { X } from 'lucide-react';
import axios from 'axios';
import { API_URL } from '../config/api';
import { useAuthStore } from '../store/authStore';
import toast from 'react-hot-toast';
import { getErrorMessage } from '../utils/errorUtils';

interface SteamModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

const SteamModal = ({ isOpen, onClose, onSuccess }: SteamModalProps) => {
  const [steamInput, setSteamInput] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);
  const { token, updateUser } = useAuthStore();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!steamInput.trim()) {
      toast.error('Пожалуйста, введите Steam ID');
      return;
    }

    setIsSubmitting(true);

    try {
      const response = await axios.put(
        `${API_URL}/profile/steam`,
        { steamInput: steamInput.trim() },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      updateUser(response.data);
      toast.success('Steam ID успешно добавлен!');
      setSteamInput('');
      onSuccess();
      onClose();
    } catch (error: unknown) {
      const errorMessage = getErrorMessage(error) || 'Ошибка при сохранении Steam ID';
      toast.error(errorMessage);
    } finally {
      setIsSubmitting(false);
    }
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/80 backdrop-blur-sm">
      <div className="bg-secondary border border-accent rounded-lg p-8 max-w-md w-full mx-4 animate-scale-in">
        <div className="flex items-center justify-between mb-6">
          <h2 className="text-2xl font-bold text-white">Привязка Steam ID</h2>
          <button
            onClick={onClose}
            className="text-gray-400 hover:text-white transition-colors"
          >
            <X size={24} />
          </button>
        </div>

        <div className="mb-6 text-gray-300 space-y-2">
          <p>Пожалуйста, укажите ваш Steam ID для продолжения.</p>
          <p className="text-sm text-gray-400">
            Вы можете использовать любой из следующих форматов:
          </p>
          <ul className="text-sm text-gray-400 list-disc list-inside space-y-1 ml-2">
            <li>STEAM_0:0:123456789</li>
            <li>[U:1:1234567890]</li>
            <li>123456789987654</li>
            <li>https://steamcommunity.com/profiles/12345</li>
          </ul>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label htmlFor="steamInput" className="block text-sm font-medium text-gray-300 mb-2">
              Steam ID *
            </label>
            <input
              type="text"
              id="steamInput"
              value={steamInput}
              onChange={(e) => setSteamInput(e.target.value)}
              placeholder="Введите Steam ID или ссылку на профиль"
              className="w-full px-4 py-3 bg-primary border border-accent rounded-lg text-white placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-highlight transition-all"
              required
            />
          </div>

          <div className="flex gap-3 pt-2">
            <button
              type="submit"
              disabled={isSubmitting}
              className="flex-1 bg-highlight hover:bg-highlight/90 text-white px-6 py-3 rounded-lg font-medium transition-all disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {isSubmitting ? 'Сохранение...' : 'Сохранить'}
            </button>
            <button
              type="button"
              onClick={onClose}
              className="flex-1 bg-accent hover:bg-accent/80 text-white px-6 py-3 rounded-lg font-medium transition-all"
            >
              Отмена
            </button>
          </div>
        </form>

        <div className="mt-6 pt-6 border-t border-accent">
          <p className="text-xs text-gray-500 text-center">
            Ваш Steam ID необходим для доступа ко всем функциям сайта
          </p>
        </div>
      </div>
    </div>
  );
};

export default SteamModal;
