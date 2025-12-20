import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useForm } from 'react-hook-form';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus, faEdit, faTrash, faSave, faTimes } from '@fortawesome/free-solid-svg-icons';
import toast from 'react-hot-toast';
import { usePageTitle } from '../../hooks/usePageTitle';
import api from '../../lib/axios';
import type { Server } from '../../types';

const AdminServers = () => {
  usePageTitle('Серверы - Админ панель');

  const queryClient = useQueryClient();
  const [editingId, setEditingId] = useState<number | null>(null);
  const [isAdding, setIsAdding] = useState(false);

  const { register, handleSubmit, reset, setValue } = useForm<Omit<Server, 'id' | 'isOnline' | 'currentPlayers'>>();

  const { data: servers, isLoading } = useQuery({
    queryKey: ['admin-servers'],
    queryFn: async () => {
      const response = await api.get<Server[]>('/admin/servers');
      return response.data;
    },
  });

  const createMutation = useMutation({
    mutationFn: async (data: Omit<Server, 'id' | 'isOnline' | 'currentPlayers'>) => {
      await api.post('/admin/servers', data);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin-servers'] });
      toast.success('Сервер добавлен');
      setIsAdding(false);
      reset();
    },
    onError: () => {
      toast.error('Ошибка при добавлении сервера');
    },
  });

  const updateMutation = useMutation({
    mutationFn: async ({ id, data }: { id: number; data: Partial<Server> }) => {
      await api.put(`/admin/servers/${id}`, data);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin-servers'] });
      toast.success('Сервер обновлен');
      setEditingId(null);
    },
    onError: () => {
      toast.error('Ошибка при обновлении сервера');
    },
  });

  const deleteMutation = useMutation({
    mutationFn: async (id: number) => {
      await api.delete(`/admin/servers/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin-servers'] });
      toast.success('Сервер удален');
    },
    onError: () => {
      toast.error('Ошибка при удалении сервера');
    },
  });

  const onSubmit = (data: Omit<Server, 'id' | 'isOnline' | 'currentPlayers'>) => {
    if (editingId) {
      updateMutation.mutate({ id: editingId, data });
    } else {
      createMutation.mutate(data);
    }
  };

  const handleEdit = (server: Server) => {
    setEditingId(server.id);
    setValue('ipAddress', server.ipAddress);
    setValue('port', server.port);
  };

  const handleCancelEdit = () => {
    setEditingId(null);
    setIsAdding(false);
    reset();
  };

  return (
    <div>
      <div className="flex flex-col sm:flex-row sm:justify-between sm:items-center mb-6 space-y-4 sm:space-y-0">
        <h1 className="text-3xl font-bold">Управление серверами</h1>
        <button
          onClick={() => setIsAdding(true)}
          className="btn-primary flex items-center space-x-2"
        >
          <FontAwesomeIcon icon={faPlus} />
          <span>Добавить сервер</span>
        </button>
      </div>

      {isAdding && (
        <div className="card mb-6">
          <h2 className="text-xl font-bold mb-4">Новый сервер</h2>
          <p className="text-gray-400 mb-4">
            Введите IP адрес и порт сервера. Информация о названии, карте и количестве игроков будет получена автоматически.
          </p>
          <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div>
                <label className="block text-sm font-medium mb-2">IP адрес *</label>
                <input
                  {...register('ipAddress', { required: true })}
                  type="text"
                  placeholder="127.0.0.1"
                  className="w-full px-4 py-2 rounded bg-accent text-white border border-gray-600 focus:border-highlight focus:outline-none"
                />
              </div>
              <div>
                <label className="block text-sm font-medium mb-2">RCON пароль (опционально)</label>
                <input
                  {...register('rconPassword')}
                  type="text"
                  placeholder="rcon_password_here"
                  className="w-full px-4 py-2 rounded bg-accent text-white border border-gray-600 focus:border-highlight focus:outline-none"
                />
              </div>
              <div>
                <label className="block text-sm font-medium mb-2">Порт *</label>
                <input
                  {...register('port', { required: true, valueAsNumber: true })}
                  type="number"
                  placeholder="27015"
                  className="w-full px-4 py-2 rounded bg-accent text-white border border-gray-600 focus:border-highlight focus:outline-none"
                />
              </div>
            </div>
            <div className="flex space-x-2">
              <button type="submit" className="btn-primary flex items-center space-x-2">
                <FontAwesomeIcon icon={faSave} />
                <span>Добавить и получить данные</span>
              </button>
              <button
                type="button"
                onClick={handleCancelEdit}
                className="btn-secondary flex items-center space-x-2"
              >
                <FontAwesomeIcon icon={faTimes} />
                <span>Отмена</span>
              </button>
            </div>
          </form>
        </div>
      )}

      {isLoading ? (
        <div className="text-center py-12">
          <div className="inline-block animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-highlight"></div>
        </div>
      ) : (
        <div className="space-y-4">
          {servers?.map((server) => (
            <div key={server.id} className="card">
              {editingId === server.id ? (
                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                  <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                    <div>
                      <label className="block text-sm font-medium mb-2">IP адрес</label>
                      <input
                        {...register('ipAddress', { required: true })}
                        type="text"
                        className="w-full px-4 py-2 rounded bg-accent text-white border border-gray-600 focus:border-highlight focus:outline-none"
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium mb-2">Порт</label>
                      <input
                        {...register('port', { required: true, valueAsNumber: true })}
                        type="number"
                        className="w-full px-4 py-2 rounded bg-accent text-white border border-gray-600 focus:border-highlight focus:outline-none"
                      />
                    </div>
                  </div>
                  <div>
                    <label className="block text-sm font-medium mb-2">RCON пароль (опционально)</label>
                    <input
                      {...register('rconPassword')}
                      type="text"
                      className="w-full px-4 py-2 rounded bg-accent text-white border border-gray-600 focus:border-highlight focus:outline-none"
                    />
                  </div>
                  <div className="flex space-x-2">
                    <button type="submit" className="btn-primary flex items-center space-x-2">
                      <FontAwesomeIcon icon={faSave} />
                      <span>Сохранить</span>
                    </button>
                    <button
                      type="button"
                      onClick={handleCancelEdit}
                      className="btn-secondary flex items-center space-x-2"
                    >
                      <FontAwesomeIcon icon={faTimes} />
                      <span>Отмена</span>
                    </button>
                  </div>
                </form>
              ) : (
                <div className="flex flex-col md:flex-row md:justify-between md:items-center space-y-4 md:space-y-0">
                  <div>
                    <h3 className="text-xl font-bold">{server.name}</h3>
                    <p className="text-gray-400">
                      {server.ipAddress}:{server.port}
                    </p>
                    <p className="text-sm text-gray-500">
                      {server.mapName} | {server.currentPlayers}/{server.maxPlayers} игроков
                      {server.isOnline ? (
                        <span className="text-green-500 ml-2">● Онлайн</span>
                      ) : (
                        <span className="text-red-500 ml-2">● Оффлайн</span>
                      )}
                    </p>
                  </div>
                  <div className="flex space-x-2">
                    <button
                      onClick={() => handleEdit(server)}
                      className="btn-secondary flex items-center space-x-2"
                    >
                      <FontAwesomeIcon icon={faEdit} />
                      <span>Изменить</span>
                    </button>
                    <button
                      onClick={() => deleteMutation.mutate(server.id)}
                      className="bg-red-600 hover:bg-red-700 text-white font-bold py-2 px-4 rounded transition-colors duration-200 flex items-center space-x-2"
                    >
                      <FontAwesomeIcon icon={faTrash} />
                      <span>Удалить</span>
                    </button>
                  </div>
                </div>
              )}
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default AdminServers;
