import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash, faShield, faBan, faInfoCircle, faTimes } from '@fortawesome/free-solid-svg-icons';
import toast from 'react-hot-toast';
import { usePageTitle } from '../../hooks/usePageTitle';
import api from '../../lib/axios';
import Avatar from '../../components/Avatar';
import type { User } from '../../types';
import { formatServerDate } from '../../utils/dateUtils';

const AdminUsers = () => {
  usePageTitle('Пользователи - Админ панель');

  const [selectedUser, setSelectedUser] = useState<User | null>(null);

  const queryClient = useQueryClient();

  const { data: users, isLoading } = useQuery({
    queryKey: ['admin-users'],
    queryFn: async () => {
      const response = await api.get<User[]>('/admin/users');
      return response.data;
    },
  });

  const toggleAdminMutation = useMutation({
    mutationFn: async (userId: number) => {
      await api.patch(`/admin/users/${userId}/toggle-admin`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin-users'] });
      toast.success('Права обновлены');
    },
    onError: () => {
      toast.error('Ошибка при обновлении прав');
    },
  });

  const deleteMutation = useMutation({
    mutationFn: async (userId: number) => {
      await api.delete(`/admin/users/${userId}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin-users'] });
      toast.success('Пользователь удален');
    },
    onError: () => {
      toast.error('Ошибка при удалении пользователя');
    },
  });

  const blockMutation = useMutation({
    mutationFn: async (userId: number) => {
      await api.post(`/admin/users/${userId}/block`, { reason: 'Заблокирован администратором' });
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin-users'] });
      toast.success('Пользователь заблокирован');
    },
    onError: () => {
      toast.error('Ошибка при блокировке пользователя');
    },
  });

  const unblockMutation = useMutation({
    mutationFn: async (userId: number) => {
      await api.post(`/admin/users/${userId}/unblock`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin-users'] });
      toast.success('Пользователь разблокирован');
    },
    onError: () => {
      toast.error('Ошибка при разблокировке пользователя');
    },
  });

  return (
    <div>
      <h1 className="text-3xl font-bold mb-6">Управление пользователями</h1>

      {isLoading ? (
        <div className="text-center py-12">
          <div className="inline-block animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-highlight"></div>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {users?.map((user) => (
            <div key={user.id} className="glass-card p-6 animate-fade-in hover:border-highlight/50 transition-colors duration-300">
              <div className="flex flex-col items-center text-center">
                <Avatar username={user.username} avatarUrl={user.avatarUrl} size="lg" className="mb-4" />
                
                <div className="flex items-center justify-center space-x-2 mb-2">
                  <h3 className="text-xl font-bold text-white">{user.username}</h3>
                  {user.isAdmin && (
                    <span className="bg-highlight text-white px-2 py-1 rounded-full text-xs flex items-center space-x-1">
                      <FontAwesomeIcon icon={faShield} />
                    </span>
                  )}
                  {user.isBlocked && (
                    <span className="bg-red-600 text-white px-2 py-1 rounded-full text-xs flex items-center space-x-1">
                      <FontAwesomeIcon icon={faBan} />
                    </span>
                  )}
                </div>
                
                <p className="text-gray-400 text-sm mb-1">{user.email}</p>
                <p className="text-xs text-gray-500 mb-4">
                  {formatServerDate(user.createdAt)}
                </p>

                <div className="flex flex-col w-full space-y-2">
                  <div className="flex space-x-2">
                    <button
                      onClick={() => toggleAdminMutation.mutate(user.id)}
                      className={`${
                        user.isAdmin ? 'bg-yellow-600 hover:bg-yellow-700' : 'bg-green-600 hover:bg-green-700'
                      } text-white font-medium py-2 px-4 rounded transition-colors duration-200 flex items-center justify-center space-x-2 flex-1`}
                    >
                      <FontAwesomeIcon icon={faShield} />
                      <span>{user.isAdmin ? 'Снять админа' : 'Сделать админом'}</span>
                    </button>

                    <button
                      onClick={() => setSelectedUser(user)}
                      className="bg-blue-600 hover:bg-blue-700 text-white font-medium py-2 px-4 rounded transition-colors duration-200 flex items-center justify-center"
                    >
                      <FontAwesomeIcon icon={faInfoCircle} />
                    </button>
                  </div>

                  <div className="flex space-x-2">
                    {user.isBlocked ? (
                      <button
                        onClick={() => {
                          if (window.confirm(`Вы уверены, что хотите разблокировать пользователя ${user.username}?`)) {
                            unblockMutation.mutate(user.id);
                          }
                        }}
                        className="bg-green-600 hover:bg-green-700 text-white font-medium py-2 px-4 rounded transition-colors duration-200 flex items-center justify-center space-x-2 flex-1"
                      >
                        <FontAwesomeIcon icon={faBan} />
                        <span>Разблокировать</span>
                      </button>
                    ) : (
                      <button
                        onClick={() => {
                          if (window.confirm(`Вы уверены, что хотите заблокировать пользователя ${user.username}? Это удалит все его комментарии и донаты, и заблокирует его IP.`)) {
                            blockMutation.mutate(user.id);
                          }
                        }}
                        className="bg-red-600 hover:bg-red-700 text-white font-medium py-2 px-4 rounded transition-colors duration-200 flex items-center justify-center space-x-2 flex-1"
                      >
                        <FontAwesomeIcon icon={faBan} />
                        <span>Заблокировать</span>
                      </button>
                    )}

                    <button
                      onClick={() => {
                        if (window.confirm(`Вы уверены, что хотите удалить пользователя ${user.username}?`)) {
                          deleteMutation.mutate(user.id);
                        }
                      }}
                      className="bg-gray-600 hover:bg-gray-700 text-white font-medium py-2 px-4 rounded transition-colors duration-200 flex items-center justify-center space-x-2 flex-1"
                    >
                      <FontAwesomeIcon icon={faTrash} />
                      <span>Удалить</span>
                    </button>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      {selectedUser && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/80 backdrop-blur-sm">
          <div className="bg-secondary border border-accent rounded-lg p-8 max-w-md w-full mx-4 animate-scale-in">
            <div className="flex items-center justify-between mb-6">
              <h2 className="text-2xl font-bold text-white">Детали пользователя</h2>
              <button
                onClick={() => setSelectedUser(null)}
                className="text-gray-400 hover:text-white transition-colors"
              >
                <FontAwesomeIcon icon={faTimes} />
              </button>
            </div>
            <div className="space-y-3 text-gray-300">
              <p><strong className="text-white">Имя:</strong> {selectedUser.username}</p>
              <p><strong className="text-white">Email:</strong> {selectedUser.email}</p>
              <p><strong className="text-white">Steam ID:</strong> {selectedUser.steamId || 'Не указан'}</p>
              <p><strong className="text-white">Дата регистрации:</strong> {formatServerDate(selectedUser.createdAt)}</p>
              <p><strong className="text-white">IP адрес:</strong> {selectedUser.lastIp || 'Неизвестен'}</p>
              <p><strong className="text-white">Админ:</strong> {selectedUser.isAdmin ? 'Да' : 'Нет'}</p>
              {selectedUser.isBlocked && (
                <>
                  <p className="text-red-400"><strong className="text-white">Статус:</strong> Заблокирован</p>
                  {selectedUser.blockedAt && (
                    <p className="text-red-400"><strong className="text-white">Дата блокировки:</strong> {formatServerDate(selectedUser.blockedAt)}</p>
                  )}
                  {selectedUser.blockReason && (
                    <p className="text-red-400"><strong className="text-white">Причина:</strong> {selectedUser.blockReason}</p>
                  )}
                </>
              )}
            </div>
            <button
              onClick={() => setSelectedUser(null)}
              className="mt-6 w-full bg-accent hover:bg-accent/80 text-white px-6 py-3 rounded-lg font-medium transition-all"
            >
              Закрыть
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default AdminUsers;

