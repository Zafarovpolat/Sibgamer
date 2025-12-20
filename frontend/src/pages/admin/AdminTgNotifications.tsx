import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { Bell, Send, Bot, Save } from 'lucide-react';
import toast from 'react-hot-toast';
import { getErrorMessage } from '../../utils/errorUtils';
import api from '../../lib/axios';

interface TelegramNotificationForm {
  title: string;
  message: string;
}

const AdminTgNotifications = () => {
  const [isSending, setIsSending] = useState(false);
  const [botToken, setBotToken] = useState('');
  const [isSavingToken, setIsSavingToken] = useState(false);

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<TelegramNotificationForm>();

  useEffect(() => {
    loadBotToken();
  }, []);

  const loadBotToken = async () => {
    try {
      const response = await api.get('/admin/telegram/bot-token');
      setBotToken(response.data.token || '');
    } catch (error) {
      console.error('Error loading bot token:', error);
    }
  };

  const handleSaveToken = async () => {
    if (!botToken.trim()) {
      toast.error('Введите токен бота');
      return;
    }

    setIsSavingToken(true);
    try {
      await api.post('/admin/telegram/bot-token', { token: botToken.trim() });
      toast.success('Токен бота сохранен');
    } catch (err: unknown) {
      const message = getErrorMessage(err) || 'Ошибка при сохранении токена';
      toast.error(message);
    } finally {
      setIsSavingToken(false);
    }
  };

  const onSubmit = async (data: TelegramNotificationForm) => {
    setIsSending(true);
    try {
      await api.post('/admin/telegram/send-notification', {
        title: data.title,
        message: data.message,
        type: 'telegram_admin_announcement'
      });

      toast.success('Уведомление отправлено всем подписчикам Telegram');
      reset();
    } catch (err: unknown) {
      const message = getErrorMessage(err) || 'Ошибка при отправке уведомления';
      toast.error(message);
    } finally {
      setIsSending(false);
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center gap-3 mb-6">
        <Bot className="w-8 h-8 text-highlight" />
        <div>
          <h1 className="text-2xl font-bold text-white">Уведомления TG</h1>
          <p className="text-gray-400">Управление Telegram ботом и отправка уведомлений</p>
        </div>
      </div>

      <div className="glass-card p-6">
        <div className="flex items-center gap-3 mb-6">
          <Bot className="w-6 h-6 text-highlight" />
          <h2 className="text-xl font-bold text-white">Настройки Telegram бота</h2>
        </div>

        <div className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-300 mb-2">
              Токен бота *
            </label>
            <div className="flex gap-4">
              <input
                type="password"
                value={botToken}
                onChange={(e) => setBotToken(e.target.value)}
                className="flex-1 bg-gray-800/50 border border-gray-600/50 rounded-lg px-4 py-3 text-white placeholder-gray-500 focus:outline-none focus:border-highlight/60"
                placeholder="Введите токен Telegram бота"
              />
              <button
                onClick={handleSaveToken}
                disabled={isSavingToken}
                className="btn-primary flex items-center gap-2 disabled:opacity-50 disabled:cursor-not-allowed"
              >
                {isSavingToken ? (
                  <>
                    <div className="animate-spin rounded-full h-4 w-4 border-t-2 border-b-2 border-white"></div>
                    Сохранение...
                  </>
                ) : (
                  <>
                    <Save className="w-4 h-4" />
                    Сохранить
                  </>
                )}
              </button>
            </div>
            <p className="text-xs text-gray-500 mt-2">
              Получить токен можно у @BotFather в Telegram
            </p>
          </div>
        </div>
      </div>

      <div className="glass-card p-6">
        <div className="flex items-center gap-3 mb-6">
          <Send className="w-6 h-6 text-highlight" />
          <h2 className="text-xl font-bold text-white">Отправить уведомление всем подписчикам</h2>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
          <div>
            <label className="block text-sm font-medium text-gray-300 mb-2">
              Заголовок уведомления *
            </label>
            <input
              {...register('title', {
                required: 'Заголовок обязателен',
                minLength: { value: 3, message: 'Минимум 3 символа' },
                maxLength: { value: 100, message: 'Максимум 100 символов' }
              })}
              type="text"
              className="w-full bg-gray-800/50 border border-gray-600/50 rounded-lg px-4 py-3 text-white placeholder-gray-500 focus:outline-none focus:border-highlight/60"
              placeholder="Введите заголовок уведомления"
            />
            {errors.title && (
              <p className="text-red-500 text-sm mt-1">{errors.title.message}</p>
            )}
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-300 mb-2">
              Текст уведомления *
            </label>
            <textarea
              {...register('message', {
                required: 'Текст уведомления обязателен',
                minLength: { value: 10, message: 'Минимум 10 символов' },
                maxLength: { value: 1000, message: 'Максимум 1000 символов' }
              })}
              rows={6}
              className="w-full bg-gray-800/50 border border-gray-600/50 rounded-lg px-4 py-3 text-white placeholder-gray-500 focus:outline-none focus:border-highlight/60 resize-vertical"
              placeholder="Введите текст уведомления"
            />
            {errors.message && (
              <p className="text-red-500 text-sm mt-1">{errors.message.message}</p>
            )}
            <p className="text-xs text-gray-500 mt-2">
              Уведомление будет отправлено в формате: эмодзи + заголовок + текст
            </p>
          </div>

          <div className="flex items-center gap-4 p-4 bg-blue-500/10 border border-blue-500/30 rounded-lg">
            <Bell className="w-5 h-5 text-blue-400 flex-shrink-0" />
            <div className="text-sm text-blue-300">
              <strong>Внимание:</strong> Это уведомление будет отправлено всем подписчикам Telegram бота.
              Убедитесь, что токен бота настроен правильно и бот запущен.
            </div>
          </div>

          <button
            type="submit"
            disabled={isSending}
            className="btn-primary flex items-center gap-2 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            {isSending ? (
              <>
                <div className="animate-spin rounded-full h-4 w-4 border-t-2 border-b-2 border-white"></div>
                Отправка...
              </>
            ) : (
              <>
                <Send className="w-4 h-4" />
                Отправить уведомление
              </>
            )}
          </button>
        </form>
      </div>
    </div>
  );
};

export default AdminTgNotifications;
