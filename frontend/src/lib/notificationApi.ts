import api from './axios';

export interface Notification {
  id: number;
  title: string;
  message: string;
  type: string;
  isRead: boolean;
  createdAt: string;
  relatedEntityId?: number;
}

export interface UnreadCount {
  count: number;
}

export interface NotificationsResponse {
  notifications: Notification[];
  pagination: {
    currentPage: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
  };
}

export const getNotifications = async (page: number = 1, pageSize: number = 20, onlyUnread: boolean = false): Promise<NotificationsResponse> => {
  const q = `/notifications?page=${page}&pageSize=${pageSize}${onlyUnread ? '&onlyUnread=true' : ''}`;
  const response = await api.get(q);
  return response.data;
};

export const getUnreadCount = async (): Promise<UnreadCount> => {
  const response = await api.get('/notifications/unread-count');
  return response.data;
};

export const markAsRead = async (notificationId: number): Promise<void> => {
  await api.put(`/notifications/${notificationId}/read`);
};

export const markAllAsRead = async (): Promise<void> => {
  await api.put('/notifications/mark-all-read');
};

export const deleteNotification = async (notificationId: number): Promise<void> => {
  if (notificationId === undefined || notificationId === null || Number.isNaN(notificationId)) {
    throw new Error('deleteNotification: invalid notificationId');
  }

  await api.delete(`/notifications/${notificationId}`);
};

export const createNotification = async (data: { title: string; message: string; type?: string; relatedEntityId?: number }): Promise<Notification> => {
  const response = await api.post('/notifications', data);
  return response.data;
};
