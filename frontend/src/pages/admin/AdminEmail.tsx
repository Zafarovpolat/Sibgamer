import { useState, useEffect } from 'react';
import { getErrorMessage } from '../../utils/errorUtils';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { 
  faEnvelope, 
  faServer, 
  faPaperPlane,
  faCheckCircle,
  faExclamationCircle 
} from '@fortawesome/free-solid-svg-icons';
import { API_URL } from '../../config/api';
import { useAuthStore } from '../../store/authStore';
import type { SmtpSettings, UpdateSmtpSettings, BulkEmailResponse, TestEmailResponse } from '../../types';

const AdminEmail = () => {
  const token = useAuthStore((state) => state.token);
  const [settings, setSettings] = useState<SmtpSettings | null>(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [testing, setTesting] = useState(false);
  const [sending, setSending] = useState(false);
  
  const [formData, setFormData] = useState<UpdateSmtpSettings>({
    host: '',
    port: 587,
    username: '',
    password: '',
    enableSsl: true,
    fromEmail: '',
    fromName: ''
  });

  const [testEmail, setTestEmail] = useState('');

  useEffect(() => {
    setFormData(prev => ({ ...prev, fromEmail: prev.username }));
  }, [formData.username]);

  const [bulkEmail, setBulkEmail] = useState({
    subject: '',
    body: ''
  });

  const [message, setMessage] = useState<{ type: 'success' | 'error', text: string } | null>(null);
  const [bulkResult, setBulkResult] = useState<BulkEmailResponse | null>(null);

  useEffect(() => {
    const fetchSettings = async () => {
      try {
        const response = await fetch(`${API_URL}/admin/email/settings`, {
          headers: { Authorization: `Bearer ${token}` }
        });
        
        if (!response.ok) throw new Error('Failed to fetch settings');
        
        const data = await response.json();
        setSettings(data);
        setFormData({
          host: data.host,
          port: data.port,
          username: data.username,
          password: data.password,
          enableSsl: data.enableSsl,
          fromEmail: data.fromEmail,
          fromName: data.fromName
        });
      } catch (err: unknown) {
        console.error('Failed to fetch SMTP settings:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchSettings();
  }, [token]);

  

  const handleSaveSettings = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);
    setMessage(null);

    try {
      const response = await fetch(`${API_URL}/admin/email/settings`, {
        method: 'PUT',
        headers: { 
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}` 
        },
        body: JSON.stringify(formData)
      });

      if (!response.ok) {
        if (response.status === 403) {
          throw new Error('Доступ запрещен. Требуются права администратора.');
        }
        
        try {
          const error = await response.json();
          throw new Error(error.message || 'Failed to save settings');
        } catch {
          throw new Error(`Ошибка сервера: ${response.status}`);
        }
      }

      const data = await response.json();
      setSettings(data);
      setMessage({ type: 'success', text: 'Настройки SMTP успешно сохранены!' });
    } catch (err: unknown) {
      setMessage({ 
        type: 'error', 
        text: getErrorMessage(err) || 'Ошибка сохранения настроек' 
      });
    } finally {
      setSaving(false);
    }
  };

  const handleTestConnection = async () => {
    if (!testEmail || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(testEmail)) {
      setMessage({ 
        type: 'error', 
        text: 'Пожалуйста, введите корректный email адрес для теста' 
      });
      return;
    }

    setTesting(true);
    setMessage(null);

    try {
      const response = await fetch(`${API_URL}/admin/email/test`, {
        method: 'POST',
        headers: { 
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}` 
        },
        body: JSON.stringify({ testEmailAddress: testEmail })
      });

      if (!response.ok) {
        if (response.status === 403) {
          throw new Error('Доступ запрещен');
        }
        throw new Error('Ошибка тестирования');
      }

      const data: TestEmailResponse = await response.json();
      setMessage({ 
        type: data.success ? 'success' : 'error', 
        text: data.message 
      });
    } catch (err: unknown) {
      setMessage({ 
        type: 'error', 
        text: getErrorMessage(err) || 'Ошибка тестирования подключения' 
      });
    } finally {
      setTesting(false);
    }
  };

  const handleSendBulkEmail = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!window.confirm('Вы уверены, что хотите отправить письмо всем пользователям?')) {
      return;
    }

    setSending(true);
    setMessage(null);
    setBulkResult(null);

    try {
      const response = await fetch(`${API_URL}/admin/email/bulk`, {
        method: 'POST',
        headers: { 
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}` 
        },
        body: JSON.stringify(bulkEmail)
      });

      if (!response.ok) {
        if (response.status === 403) {
          throw new Error('Доступ запрещен');
        }
        throw new Error('Ошибка массовой рассылки');
      }

      const data: BulkEmailResponse = await response.json();
      setBulkResult(data);
      
      if (data.failureCount === 0) {
        setMessage({ 
          type: 'success', 
          text: `Письмо успешно отправлено ${data.successCount} пользователям!` 
        });
        setBulkEmail({ subject: '', body: '' });
      } else {
        setMessage({ 
          type: 'error', 
          text: `Отправлено: ${data.successCount}, Ошибок: ${data.failureCount}` 
        });
      }
    } catch (err: unknown) {
      setMessage({ 
        type: 'error', 
        text: getErrorMessage(err) || 'Ошибка массовой рассылки' 
      });
    } finally {
      setSending(false);
    }
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-highlight"></div>
      </div>
    );
  }

  return (
    <div className="space-y-8">
      <div className="flex items-center space-x-4">
        <div className="w-16 h-16 bg-gradient-to-br from-highlight to-blue-500 rounded-2xl flex items-center justify-center shadow-lg shadow-highlight/30">
          <FontAwesomeIcon icon={faEnvelope} className="text-3xl text-white" />
        </div>
        <div>
          <h1 className="text-4xl font-bold bg-gradient-to-r from-white to-gray-300 bg-clip-text text-transparent">
            Настройки Email
          </h1>
          <p className="text-gray-400 mt-1">Управление SMTP сервером и рассылками</p>
        </div>
      </div>

      {message && (
        <div className={`glass-card border-l-4 p-4 ${
          message.type === 'success' 
            ? 'border-green-500 bg-green-500/10' 
            : 'border-red-500 bg-red-500/10'
        }`}>
          <div className="flex items-center space-x-3">
            <FontAwesomeIcon 
              icon={message.type === 'success' ? faCheckCircle : faExclamationCircle} 
              className={message.type === 'success' ? 'text-green-500' : 'text-red-500'}
              size="lg"
            />
            <p className="text-white">{message.text}</p>
          </div>
        </div>
      )}

      <div className="glass-card p-8 border border-gray-800/50 hover:border-highlight/30 transition-all duration-300">
        <div className="flex items-center space-x-3 mb-6 pb-4 border-b border-gray-800">
          <div className="w-10 h-10 bg-highlight/20 rounded-lg flex items-center justify-center">
            <FontAwesomeIcon icon={faServer} className="text-highlight" />
          </div>
          <h2 className="text-2xl font-bold">SMTP Сервер</h2>
        </div>

        <form onSubmit={handleSaveSettings} className="space-y-6">
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            <div className="space-y-2">
              <label className="block text-sm font-semibold text-gray-300">
                SMTP Хост
              </label>
              <input
                type="text"
                value={formData.host}
                onChange={(e) => setFormData({ ...formData, host: e.target.value })}
                className="w-full px-4 py-3 bg-secondary/50 border border-gray-700 rounded-lg focus:outline-none focus:ring-2 focus:ring-highlight focus:border-transparent transition-all duration-300"
                placeholder="smtp.gmail.com"
                required
              />
            </div>

            <div className="space-y-2">
              <label className="block text-sm font-semibold text-gray-300">
                Порт
              </label>
              <input
                type="number"
                value={formData.port}
                onChange={(e) => setFormData({ ...formData, port: parseInt(e.target.value) })}
                className="w-full px-4 py-3 bg-secondary/50 border border-gray-700 rounded-lg focus:outline-none focus:ring-2 focus:ring-highlight focus:border-transparent transition-all duration-300"
                required
              />
            </div>

            <div className="space-y-2">
              <label className="block text-sm font-semibold text-gray-300">
                Email <span className="text-xs text-gray-500">(используется для входа и отправки)</span>
              </label>
              <input
                type="email"
                value={formData.username}
                onChange={(e) => setFormData({ ...formData, username: e.target.value, fromEmail: e.target.value })}
                className="w-full px-4 py-3 bg-secondary/50 border border-gray-700 rounded-lg focus:outline-none focus:ring-2 focus:ring-highlight focus:border-transparent transition-all duration-300"
                placeholder="your-email@gmail.com"
                required
              />
            </div>

            <div className="space-y-2">
              <label className="block text-sm font-semibold text-gray-300">
                Пароль приложения
              </label>
              <input
                type="password"
                value={formData.password}
                onChange={(e) => setFormData({ ...formData, password: e.target.value })}
                className="w-full px-4 py-3 bg-secondary/50 border border-gray-700 rounded-lg focus:outline-none focus:ring-2 focus:ring-highlight focus:border-transparent transition-all duration-300"
                placeholder="••••••••••••••••"
                required
              />
            </div>

            <div className="space-y-2 lg:col-span-2">
              <label className="block text-sm font-semibold text-gray-300">
                Имя отправителя
              </label>
              <input
                type="text"
                value={formData.fromName}
                onChange={(e) => setFormData({ ...formData, fromName: e.target.value })}
                className="w-full px-4 py-3 bg-secondary/50 border border-gray-700 rounded-lg focus:outline-none focus:ring-2 focus:ring-highlight focus:border-transparent transition-all duration-300"
                placeholder="SibGamer Community"
                required
              />
            </div>
          </div>

          <div className="flex items-center space-x-3 p-4 bg-secondary/30 border border-gray-700/50 rounded-lg">
            <input
              type="checkbox"
              id="enableSsl"
              checked={formData.enableSsl}
              onChange={(e) => setFormData({ ...formData, enableSsl: e.target.checked })}
              className="w-5 h-5 text-highlight bg-secondary border-gray-600 rounded focus:ring-2 focus:ring-highlight"
            />
            <label htmlFor="enableSsl" className="text-sm font-medium cursor-pointer">
              Использовать SSL/TLS (рекомендуется)
            </label>
          </div>

          <div className="space-y-2">
            <label className="block text-sm font-semibold text-gray-300">
              Email для тестирования подключения
            </label>
            <input
              type="email"
              value={testEmail}
              onChange={(e) => setTestEmail(e.target.value)}
              className="w-full px-4 py-3 bg-secondary/50 border border-gray-700 rounded-lg focus:outline-none focus:ring-2 focus:ring-highlight focus:border-transparent transition-all duration-300"
              placeholder="test@example.com"
            />
            <p className="text-sm text-gray-400">
              Укажите email адрес, на который будет отправлено тестовое письмо
            </p>
          </div>

          <div className="flex flex-wrap gap-4 pt-4">
            <button
              type="submit"
              disabled={saving}
              className="flex-1 min-w-[200px] px-6 py-3 bg-gradient-to-r from-highlight to-blue-500 text-white font-semibold rounded-lg hover:shadow-lg hover:shadow-highlight/50 transition-all duration-300 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {saving ? (
                <span className="flex items-center justify-center space-x-2">
                  <div className="w-5 h-5 border-2 border-white border-t-transparent rounded-full animate-spin"></div>
                  <span>Сохранение...</span>
                </span>
              ) : (
                'Сохранить настройки'
              )}
            </button>

            <button
              type="button"
              onClick={handleTestConnection}
              disabled={testing || !testEmail}
              className="flex-1 min-w-[200px] px-6 py-3 bg-secondary/50 border border-gray-700 text-white font-semibold rounded-lg hover:bg-secondary hover:border-highlight transition-all duration-300 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {testing ? (
                <span className="flex items-center justify-center space-x-2">
                  <div className="w-5 h-5 border-2 border-white border-t-transparent rounded-full animate-spin"></div>
                  <span>Тестирование...</span>
                </span>
              ) : (
                'Тестировать подключение'
              )}
            </button>
          </div>

        </form>
      </div>

      {settings?.isConfigured ? (
        <div className="glass-card p-8 border border-gray-800/50 hover:border-highlight/30 transition-all duration-300">
          <div className="flex items-center space-x-3 mb-6 pb-4 border-b border-gray-800">
            <div className="w-10 h-10 bg-highlight/20 rounded-lg flex items-center justify-center">
              <FontAwesomeIcon icon={faPaperPlane} className="text-highlight" />
            </div>
            <h2 className="text-2xl font-bold">Массовая рассылка</h2>
          </div>

          <form onSubmit={handleSendBulkEmail} className="space-y-6">
            <div className="space-y-2">
              <label className="block text-sm font-semibold text-gray-300">
                Тема письма
              </label>
              <input
                type="text"
                value={bulkEmail.subject}
                onChange={(e) => setBulkEmail({ ...bulkEmail, subject: e.target.value })}
                className="w-full px-4 py-3 bg-secondary/50 border border-gray-700 rounded-lg focus:outline-none focus:ring-2 focus:ring-highlight focus:border-transparent transition-all duration-300"
                placeholder="Важное объявление для игроков"
                required
              />
            </div>

            <div className="space-y-2">
              <label className="block text-sm font-semibold text-gray-300">
                Текст письма <span className="text-xs text-gray-500">(HTML поддерживается)</span>
              </label>
              <textarea
                value={bulkEmail.body}
                onChange={(e) => setBulkEmail({ ...bulkEmail, body: e.target.value })}
                className="w-full px-4 py-3 bg-secondary/50 border border-gray-700 rounded-lg focus:outline-none focus:ring-2 focus:ring-highlight focus:border-transparent transition-all duration-300 font-mono text-sm"
                rows={12}
                placeholder="<p>Привет, {username}!</p><p>Ваше сообщение...</p>"
                required
              />
              <p className="text-sm text-gray-400">
                💡 Используйте <code className="px-2 py-1 bg-secondary/50 rounded text-highlight">{`{username}`}</code> для вставки имени пользователя
              </p>
            </div>

            <button
              type="submit"
              disabled={sending}
              className="w-full px-6 py-3 bg-gradient-to-r from-highlight to-blue-500 text-white font-semibold rounded-lg hover:shadow-lg hover:shadow-highlight/50 transition-all duration-300 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {sending ? (
                <span className="flex items-center justify-center space-x-2">
                  <div className="w-5 h-5 border-2 border-white border-t-transparent rounded-full animate-spin"></div>
                  <span>Отправка...</span>
                </span>
              ) : (
                <span className="flex items-center justify-center space-x-2">
                  <FontAwesomeIcon icon={faEnvelope} />
                  <span>Отправить всем пользователям</span>
                </span>
              )}
            </button>
          </form>

          {bulkResult && (
            <div className="mt-6 p-6 bg-secondary/30 border border-gray-700 rounded-lg">
              <h3 className="font-bold text-lg mb-4 flex items-center space-x-2">
                <FontAwesomeIcon icon={faCheckCircle} className="text-green-500" />
                <span>Результаты рассылки</span>
              </h3>
              <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-4">
                <div className="p-4 bg-secondary/50 rounded-lg border border-gray-700">
                  <p className="text-sm text-gray-400">Всего получателей</p>
                  <p className="text-2xl font-bold text-white">{bulkResult.totalRecipients}</p>
                </div>
                <div className="p-4 bg-green-500/10 rounded-lg border border-green-500/30">
                  <p className="text-sm text-green-400">Успешно</p>
                  <p className="text-2xl font-bold text-green-500">{bulkResult.successCount}</p>
                </div>
                <div className="p-4 bg-red-500/10 rounded-lg border border-red-500/30">
                  <p className="text-sm text-red-400">Ошибок</p>
                  <p className="text-2xl font-bold text-red-500">{bulkResult.failureCount}</p>
                </div>
              </div>
              
              {bulkResult.errors.length > 0 && (
                <div className="mt-4 p-4 bg-red-500/10 border border-red-500/30 rounded-lg">
                  <p className="font-semibold text-red-400 mb-2">Список ошибок:</p>
                  <ul className="text-sm text-gray-400 max-h-40 overflow-y-auto space-y-1">
                    {bulkResult.errors.map((error, idx) => (
                      <li key={idx} className="flex items-start space-x-2">
                        <span className="text-red-500">•</span>
                        <span>{error}</span>
                      </li>
                    ))}
                  </ul>
                </div>
              )}
            </div>
          )}
        </div>
      ) : (
        <div className="glass-card p-8 border border-yellow-500/50 bg-yellow-500/5">
          <div className="flex items-center space-x-4">
            <div className="w-12 h-12 bg-yellow-500/20 rounded-lg flex items-center justify-center">
              <FontAwesomeIcon icon={faExclamationCircle} className="text-yellow-500 text-2xl" />
            </div>
            <div>
              <h3 className="font-bold text-yellow-500 text-lg">SMTP не настроен</h3>
              <p className="text-gray-400 mt-1">
                Сначала настройте и сохраните параметры SMTP сервера для отправки массовых рассылок
              </p>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default AdminEmail;
