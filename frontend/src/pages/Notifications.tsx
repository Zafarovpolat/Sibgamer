import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { Bell, Check, CheckCheck, Trash2, ChevronLeft, ChevronRight } from 'lucide-react';
import { getNotifications, markAsRead, markAllAsRead, deleteNotification, createNotification } from '../lib/notificationApi';
import { useAuthStore } from '../store/authStore';
import toast from 'react-hot-toast';
import { usePageTitle } from '../hooks/usePageTitle';
import { formatServerDate, getServerLocalTime, parseServerDate } from '../utils/dateUtils';

const Notifications = () => {
  usePageTitle('Уведомления');

  const { isAuthenticated } = useAuthStore();
  const queryClient = useQueryClient();
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 10;

  const { data: notificationsData, isLoading } = useQuery({
    queryKey: ['notifications', 'all', currentPage],
    queryFn: () => getNotifications(currentPage, pageSize),
    enabled: isAuthenticated,
  });

  const markAsReadMutation = useMutation({
    mutationFn: markAsRead,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['notifications'] });
      toast.success('Уведомление отмечено как прочитанное');
    },
    onError: () => {
      toast.error('Ошибка при отметке уведомления как прочитанного');
    },
  });

  const markAllAsReadMutation = useMutation({
    mutationFn: markAllAsRead,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['notifications'] });
      toast.success('Все уведомления отмечены как прочитанные');
    },
    onError: () => {
      toast.error('Ошибка при отметке уведомлений как прочитанных');
    },
  });

  const deleteNotificationMutation = useMutation({
    mutationFn: deleteNotification,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['notifications'] });
    },
    onError: () => {
      toast.error('Ошибка при удалении уведомления');
    },
  });

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

  const handleMarkAsRead = (notificationId: number) => {
    markAsReadMutation.mutate(notificationId);
  };

  const handleMarkAllAsRead = () => {
    markAllAsReadMutation.mutate();
  };

  const handleDelete = (notification: any) => {
    if (!notification || typeof notification.id !== 'number') {
      console.error('handleDelete: invalid notification object', notification);
      toast.error('Не удалось удалить уведомление — неверные данные');
      return;
    }

    if (!window.confirm('Вы уверены, что хотите удалить это уведомление?')) return;

    const dataCopy = { ...notification };

    deleteNotificationMutation.mutate(notification.id, {
      onSuccess: async () => {
        toast.custom(() => (
          <div className="bg-secondary border border-gray-700 px-4 py-3 rounded shadow flex items-center justify-between gap-4">
            <div className="text-sm text-gray-200">Уведомление удалено</div>
            <div className="flex items-center gap-2">
              <button
                onClick={async () => {
                  try {
                          await createNotification({ title: dataCopy.title, message: dataCopy.message, type: dataCopy.type, relatedEntityId: dataCopy.relatedEntityId });
                    queryClient.invalidateQueries({ queryKey: ['notifications'] });
                    queryClient.invalidateQueries({ queryKey: ['notifications', 'all', currentPage] });
                    queryClient.invalidateQueries({ queryKey: ['notifications', 'unread-count'] });
                    toast.success('Уведомление восстановлено');
                  } catch {
                    toast.error('Не удалось восстановить уведомление');
                  }
                }}
                className="text-sm bg-accent text-white px-3 py-1 rounded"
              >
                Отменить
              </button>
              <button onClick={() => toast.dismiss()} className="text-sm text-gray-400 px-2 py-1">Закрыть</button>
            </div>
          </div>
        ), { duration: 8000 });
      }
    });
  };

  if (!isAuthenticated) {
    return (
      <div className="min-h-screen bg-dark text-white pt-20">
        <div className="container mx-auto px-4 py-8">
          <div className="text-center">
            <h1 className="text-3xl font-bold mb-4">Уведомления</h1>
            <p className="text-gray-400">Войдите в аккаунт, чтобы просмотреть уведомления</p>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-dark text-white pt-20">
      <div className="container mx-auto px-4 py-8">
        <div className="max-w-4xl mx-auto">
          <div className="flex items-center justify-between mb-8">
            <div className="flex items-center space-x-3">
              
            </div>
            {notificationsData && notificationsData.notifications.length > 0 && (
              <button
                onClick={handleMarkAllAsRead}
                className="btn-secondary flex items-center space-x-2"
                disabled={markAllAsReadMutation.isPending}
              >
                <CheckCheck className="w-4 h-4" />
                <span>Отметить все как прочитанные</span>
              </button>
            )}
          </div>

          {isLoading ? (
            <div className="text-center py-12">
              <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-highlight mx-auto mb-4"></div>
              <p className="text-gray-400">Загрузка уведомлений...</p>
            </div>
          ) : notificationsData && notificationsData.notifications.length > 0 ? (
            <div className="space-y-4">
              {notificationsData.notifications.map((notification) => (
                  <div
                    key={notification.id}
                    className={`glass-card p-6 transition-all duration-300 hover:shadow-lg ${
                      !notification.isRead ? 'border-l-4 border-highlight bg-highlight/5' : 'bg-secondary/40'
                    }`}
                  >
                  <div className="flex items-start justify-between gap-4">
                    <div className="flex-1 min-w-0">
                      <div className="flex items-center gap-3 mb-2">
                        <h3 className="text-lg font-semibold text-white truncate">
                          {notification.title}
                        </h3>
                        {!notification.isRead && (
                          <div className="w-3 h-3 bg-highlight rounded-full flex-shrink-0"></div>
                        )}
                      </div>
                      <div className="text-gray-300 leading-relaxed mb-3 break-words whitespace-pre-wrap">
                        {notification.message}
                      </div>
                      <div className="flex items-center justify-between">
                        <span className="text-sm text-gray-500">
                          {formatDate(notification.createdAt)}
                        </span>
                        {notification.type === 'admin_notification' && (
                          <div className="text-xs text-gray-400 ml-2">
                            <span className="inline-block px-3 py-1 rounded-full bg-black/20 text-xs text-gray-300">Сообщение от администрации</span>
                          </div>
                        )}
                        <div className="flex items-center space-x-2">
                          {!notification.isRead && (
                            <button
                              onClick={() => handleMarkAsRead(notification.id)}
                              className="text-highlight hover:text-blue-400 transition-colors p-2 rounded-lg hover:bg-highlight/10"
                              title="Отметить как прочитанное"
                              disabled={markAsReadMutation.isPending}
                            >
                              <Check className="w-4 h-4" />
                            </button>
                          )}
                          <button
                            onClick={() => handleDelete(notification)}
                            className="text-red-400 hover:text-red-300 transition-colors p-2 rounded-lg hover:bg-red-500/10"
                            title="Удалить уведомление"
                            disabled={deleteNotificationMutation.isPending}
                          >
                            <Trash2 className="w-4 h-4" />
                          </button>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          ) : (
            <div className="text-center py-16">
              <Bell className="w-16 h-16 text-gray-600 mx-auto mb-4" />
              <h3 className="text-xl font-semibold text-gray-400 mb-2">У вас нет уведомлений</h3>
              <p className="text-gray-500">Когда появятся новые уведомления, они отобразятся здесь</p>
            </div>
          )}

          {notificationsData && notificationsData.pagination.totalPages > 1 && (
            <div className="flex items-center justify-center space-x-4 mt-8">
              <button
                onClick={() => setCurrentPage(prev => Math.max(1, prev - 1))}
                disabled={currentPage === 1}
                className="btn-secondary disabled:opacity-50 disabled:cursor-not-allowed flex items-center space-x-2"
              >
                <ChevronLeft className="w-4 h-4" />
                <span>Предыдущая</span>
              </button>

              <div className="flex items-center space-x-2">
                <span className="text-gray-400">Страница</span>
                <span className="text-white font-semibold">{currentPage}</span>
                <span className="text-gray-400">из</span>
                <span className="text-white font-semibold">{notificationsData.pagination.totalPages}</span>
              </div>

              <button
                onClick={() => setCurrentPage(prev => Math.min(notificationsData.pagination.totalPages, prev + 1))}
                disabled={currentPage === notificationsData.pagination.totalPages}
                className="btn-secondary disabled:opacity-50 disabled:cursor-not-allowed flex items-center space-x-2"
              >
                <span>Следующая</span>
                <ChevronRight className="w-4 h-4" />
              </button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default Notifications;
