import { useState, useEffect, useRef } from 'react';
import { Link } from 'react-router-dom';
import { Bell, Check } from 'lucide-react';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { getNotifications, getUnreadCount, markAsRead, markAllAsRead, deleteNotification, createNotification } from '../lib/notificationApi';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { useAuthStore } from '../store/authStore';
import { getServerLocalTime, formatServerDate, parseServerDate } from '../utils/dateUtils';
import toast from 'react-hot-toast';

const NotificationDropdown = () => {
  const { isAuthenticated } = useAuthStore();
  const [showDropdown, setShowDropdown] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);

  const { data: unreadCount } = useQuery({
    queryKey: ['notifications', 'unread-count'],
    queryFn: getUnreadCount,
    enabled: isAuthenticated,
    refetchInterval: 30000,
    staleTime: 2000, // Предотвращаем мгновенный запрос после входа (race condition с сохранением токена)
  });

  const queryClient = useQueryClient();

  const notificationsQuery = useQuery({
    queryKey: ['notifications', 'recent'],
    queryFn: () => getNotifications(1, 3, true),
    enabled: isAuthenticated && showDropdown,
  });

  const notifications = notificationsQuery.data;
  const unreadList = (notifications?.notifications || []).filter(n => !n.isRead);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
        setShowDropdown(false);
      }
    };

    if (showDropdown) {
      document.addEventListener('mousedown', handleClickOutside);
    }

    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [showDropdown]);

  const handleMarkAsRead = async (notificationId: number) => {
    try {
      await markAsRead(notificationId);
      queryClient.invalidateQueries({ queryKey: ['notifications', 'recent'] });
      queryClient.invalidateQueries({ queryKey: ['notifications', 'unread-count'] });
    } catch {
      toast.error('Ошибка при отметке уведомления как прочитанного');
    }
  };

  const handleMarkAllAsRead = async () => {
    try {
      await markAllAsRead();
      toast.success('Все уведомления отмечены как прочитанные');
      queryClient.invalidateQueries({ queryKey: ['notifications', 'recent'] });
      queryClient.invalidateQueries({ queryKey: ['notifications', 'unread-count'] });
      setShowDropdown(false);
    } catch {
      toast.error('Ошибка при отметке уведомлений как прочитанных');
    }
  };


  const handleDelete = async (notificationId: number, notificationData?: any) => {
    try {
      if (!window.confirm('Вы уверены, что хотите удалить это уведомление?')) return;
      await deleteNotification(notificationId);
      queryClient.invalidateQueries({ queryKey: ['notifications', 'recent'] });
      queryClient.invalidateQueries({ queryKey: ['notifications', 'unread-count'] });

      toast.custom((t) => (
        <div className="bg-secondary border border-gray-700 px-4 py-3 rounded shadow flex items-center justify-between gap-4">
          <div className="text-sm text-gray-200">Уведомление удалено</div>
          <div className="flex items-center gap-2">
            <button
              onClick={async () => {
                try {
                  await createNotification({ title: notificationData?.title || '', message: notificationData?.message || '', type: notificationData?.type, relatedEntityId: notificationData?.relatedEntityId });
                  queryClient.invalidateQueries({ queryKey: ['notifications', 'recent'] });
                  queryClient.invalidateQueries({ queryKey: ['notifications', 'unread-count'] });
                  toast.success('Уведомление восстановлено');
                  toast.dismiss(t.id);
                } catch {
                  toast.error('Не удалось восстановить уведомление');
                }
              }}
              className="text-sm bg-accent text-white px-3 py-1 rounded"
            >
              Отменить
            </button>
            <button onClick={() => toast.dismiss(t.id)} className="text-sm text-gray-400 px-2 py-1">Закрыть</button>
          </div>
        </div>
      ), { duration: 8000 });
    } catch {
      toast.error('Ошибка при удалении уведомления');
    }
  };

  const formatDate = (dateString: string) => {
    const date = parseServerDate(dateString);
    const now = getServerLocalTime();
    const diffInMinutes = Math.floor((now.getTime() - date.getTime()) / (1000 * 60));

    if (diffInMinutes < 1) return 'только что';
    if (diffInMinutes < 60) return `${diffInMinutes} мин назад`;

    const diffInHours = Math.floor(diffInMinutes / 60);
    if (diffInHours < 24) return `${diffInHours} ч назад`;

    const diffInDays = Math.floor(diffInHours / 24);
    if (diffInDays < 7) return `${diffInDays} д назад`;

    return formatServerDate(dateString, {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  if (!isAuthenticated) return null;

  return (
    <div className="relative" ref={dropdownRef}>
      <button
        onClick={() => setShowDropdown(!showDropdown)}
        className="relative text-gray-300 hover:text-highlight transition-colors duration-300 flex items-center space-x-2 px-3 py-2 rounded-lg hover:bg-highlight/10"
      >
        <Bell className="w-5 h-5" />
        {unreadCount && unreadCount.count > 0 && (
          <span className="absolute -top-1 -right-1 bg-red-500 text-white text-xs rounded-full h-5 w-5 flex items-center justify-center font-bold">
            {unreadCount.count > 99 ? '99+' : unreadCount.count}
          </span>
        )}
      </button>

      {showDropdown && (
        <div className="absolute right-0 mt-2 bg-secondary border border-gray-800 rounded-xl shadow-2xl py-2 animate-slide-in z-50 w-96 max-w-[calc(100vw-2rem)]">
          <div className="px-4 py-2 border-b border-gray-700/50">
            <div className="flex items-center justify-between">
              <h3 className="text-white font-semibold">Уведомления</h3>
              {unreadCount && unreadCount.count > 0 && (
                <button
                  onClick={handleMarkAllAsRead}
                  className="text-xs text-highlight hover:text-blue-400 transition-colors flex items-center gap-1"
                >
                  <Check className="w-3 h-3" />
                  Прочитать все
                </button>
              )}
            </div>
          </div>

          <div className="max-h-96 overflow-y-auto rounded-b-xl">
            {unreadList.length > 0 ? (
              unreadList.map((notification) => (
                <div
                  key={notification.id}
                  className={`flex items-start gap-3 px-4 py-3 hover:bg-secondary/80 transition-colors ${!notification.isRead ? 'bg-secondary/85' : 'bg-secondary/90'
                    }`}
                >
                  {!notification.isRead ? (
                    <div className="w-1.5 h-10 rounded-full bg-highlight mt-1 shrink-0" />
                  ) : (
                    <div className="w-1.5 h-10 mt-1 shrink-0" />
                  )}

                  <div className="flex-1 min-w-0 flex flex-col gap-2">
                    <div className="flex items-start justify-between gap-3">
                      <div className="flex items-center gap-2 min-w-0">
                        <h4 className="text-white font-semibold text-sm truncate">{notification.title}</h4>
                        {!notification.isRead && (
                          <span className="w-2 h-2 bg-highlight rounded-full flex-shrink-0" aria-hidden />
                        )}
                      </div>

                      <div className="flex items-center gap-2 flex-shrink-0">
                        <span className="text-gray-500 text-xs whitespace-nowrap">{formatDate(notification.createdAt)}</span>
                        {notification.type === 'admin_notification' && (
                          <span className="inline-block px-2 py-0.5 rounded-full bg-black/20 text-xs text-gray-300 whitespace-nowrap">Админ</span>
                        )}
                      </div>
                    </div>

                    <div className="text-gray-400 text-sm truncate whitespace-nowrap">{notification.message}</div>
                  </div>

                  <div className="flex flex-col items-center justify-start gap-1 ml-2">
                    {!notification.isRead && (
                      <button
                        onClick={() => handleMarkAsRead(notification.id)}
                        className="text-gray-400 hover:text-highlight transition-colors p-1"
                        title="Отметить как прочитанное"
                      >
                        <Check className="w-4 h-4" />
                      </button>
                    )}
                    <button
                      onClick={() => handleDelete(notification.id, notification)}
                      className="text-red-400 hover:text-red-300 transition-colors p-1"
                      title="Удалить уведомление"
                    >
                      <FontAwesomeIcon icon={faTrash} className="w-4 h-4" />
                    </button>
                  </div>
                </div>
              ))
            ) : (
              <div className="px-4 py-8 text-center">
                <Bell className="w-8 h-8 text-gray-600 mx-auto mb-2" />
                <p className="text-gray-500 text-sm">Непрочитанных уведомлений нет</p>
              </div>
            )}
          </div>

          <div className="px-4 py-2 border-t border-gray-700/50">
            <Link
              to="/notifications"
              className="block w-full text-center text-highlight hover:text-blue-400 transition-colors text-sm font-medium"
              onClick={() => setShowDropdown(false)}
            >
              Все уведомления
            </Link>
          </div>
        </div>
      )}
    </div>
  );
};

export default NotificationDropdown;