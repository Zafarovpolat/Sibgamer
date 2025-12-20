import { useState } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus, faEdit, faTrash, faSave, faTimes, faEye, faUpload, faCalendar, faHeart, faComment } from '@fortawesome/free-solid-svg-icons';
import { formatServerDate } from '../../utils/dateUtils';
import RichTextEditor from '../../components/RichTextEditor';
import { API_URL } from '../../config/api';
import { resolveMediaUrl } from '../../lib/media';
import { getAuthToken } from '../../lib/auth';
import { getServerLocalTime, parseServerDate, formatForDatetimeLocal } from '../../utils/dateUtils';
import CountdownTimer from '../../components/CountdownTimer';
import type { EventListItem } from '../../types';

interface CreateEventData {
  title: string;
  content: string;
  summary: string;
  slug: string;
  coverImage: string;
  isPublished: boolean;
  startDate: string;
  endDate: string;
  mediaUrls: string[];
}

const AdminEvents = () => {
  const queryClient = useQueryClient();
  const [isCreating, setIsCreating] = useState(false);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [formData, setFormData] = useState<CreateEventData>({
    title: '',
    content: '',
    summary: '',
    slug: '',
    coverImage: '',
    isPublished: true,
    startDate: '',
    endDate: '',
    mediaUrls: []
  });

  const { data: eventsList, isLoading } = useQuery<{ items: EventListItem[] }>({
    queryKey: ['admin-events'],
    queryFn: async () => {
      const res = await fetch(`${API_URL}/events?pageSize=100`);
      if (!res.ok) throw new Error('Failed to fetch events');
      return res.json();
    }
  });

  const createMutation = useMutation({
    mutationFn: async (data: CreateEventData) => {
      const token = getAuthToken();
      const res = await fetch(`${API_URL}/admin/events`, {
        method: 'POST',
        headers: { 
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(data)
      });
      if (!res.ok) throw new Error('Failed to create event');
      return res.json();
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin-events'] });
      queryClient.invalidateQueries({ queryKey: ['events'] });
      queryClient.invalidateQueries({ queryKey: ['upcoming-events'] });
      resetForm();
    }
  });

  const updateMutation = useMutation({
    mutationFn: async ({ id, data }: { id: number; data: CreateEventData }) => {
      const token = getAuthToken();
      const res = await fetch(`${API_URL}/admin/events/${id}`, {
        method: 'PUT',
        headers: { 
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(data)
      });
      if (!res.ok) throw new Error('Failed to update event');
      return res.json();
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin-events'] });
      queryClient.invalidateQueries({ queryKey: ['events'] });
      queryClient.invalidateQueries({ queryKey: ['upcoming-events'] });
      resetForm();
    }
  });

  const deleteMutation = useMutation({
    mutationFn: async (id: number) => {
      const token = getAuthToken();
      const res = await fetch(`${API_URL}/admin/events/${id}`, {
        method: 'DELETE',
        headers: { 
          'Authorization': `Bearer ${token}`
        }
      });
      if (!res.ok) throw new Error('Failed to delete event');
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin-events'] });
      queryClient.invalidateQueries({ queryKey: ['events'] });
      queryClient.invalidateQueries({ queryKey: ['upcoming-events'] });
    }
  });

  const resetForm = () => {
    setFormData({
      title: '',
      content: '',
      summary: '',
      slug: '',
      coverImage: '',
      isPublished: true,
      startDate: '',
      endDate: '',
      mediaUrls: []
    });
    setIsCreating(false);
    setEditingId(null);
  };

  const handleEdit = async (event: EventListItem) => {
    setEditingId(event.id);
    setIsCreating(true);
    
    try {
      const res = await fetch(`${API_URL}/events/${event.slug}`);
      if (!res.ok) throw new Error('Failed to fetch event');
      
      const data = await res.json();
      
      setFormData({
        title: data.title || '',
        content: data.content || '',
        summary: data.summary || '',
        slug: data.slug || '',
        coverImage: data.coverImage || '',
        isPublished: data.isPublished || false,
          startDate: data.startDate ? formatForDatetimeLocal(data.startDate) : '',
          endDate: data.endDate ? formatForDatetimeLocal(data.endDate) : '',
        mediaUrls: data.media?.map((m: { mediaUrl: string }) => m.mediaUrl) || []
      });
    } catch (error) {
      console.error('Failed to load event for editing:', error);
      alert('Ошибка загрузки события для редактирования');
      resetForm();
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (editingId) {
      await updateMutation.mutateAsync({ id: editingId, data: formData });
    } else {
      await createMutation.mutateAsync(formData);
    }
  };

  const handleCoverImageUpload = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;

    const formDataUpload = new FormData();
    formDataUpload.append('file', file);

    try {
      const token = getAuthToken();
      const res = await fetch(`${API_URL}/upload/events`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`
        },
        body: formDataUpload
      });

      if (!res.ok) throw new Error('Upload failed');
      const data = await res.json();
      setFormData(prev => ({ ...prev, coverImage: data.url }));
    } catch (error) {
      console.error('Upload error:', error);
    }
  };

  const handleMediaUpload = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const files = e.target.files;
    if (!files) return;

    const uploadPromises = Array.from(files).map(async (file) => {
      const formDataUpload = new FormData();
      formDataUpload.append('file', file);

      const token = getAuthToken();
      const res = await fetch(`${API_URL}/upload/events`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`
        },
        body: formDataUpload
      });

      if (!res.ok) throw new Error('Upload failed');
      const data = await res.json();
      return data.url;
    });

    try {
      const urls = await Promise.all(uploadPromises);
      setFormData(prev => ({
        ...prev,
        mediaUrls: [...prev.mediaUrls, ...urls]
      }));
    } catch (error) {
      console.error('Upload error:', error);
    }
  };

  const removeMedia = (index: number) => {
    setFormData(prev => ({
      ...prev,
      mediaUrls: prev.mediaUrls.filter((_: string, i: number) => i !== index)
    }));
  };

  const getEventStatus = (startDate: string, endDate: string) => {
    const now = getServerLocalTime();
    const start = parseServerDate(startDate);
    const end = parseServerDate(endDate);

    if (now < start) {
      return { type: 'upcoming' as const, targetDate: startDate, text: 'Предстоящее', color: 'text-white', bgColor: 'bg-black/40' };
    } else if (now >= start && now <= end) {
      return { type: 'ongoing' as const, targetDate: endDate, text: 'В процессе', color: 'text-green-400', bgColor: 'bg-black/40' };
    } else {
      return { type: 'finished' as const, text: 'Завершено', color: 'text-red-400', bgColor: 'bg-black/40' };
    }
  };

  if (isLoading) {
    return <div className="text-center py-10">Загрузка...</div>;
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="flex flex-col sm:flex-row sm:justify-between sm:items-center mb-6 space-y-4 sm:space-y-0">
        <h1 className="text-3xl font-bold">Управление событиями</h1>
        {!isCreating && (
          <button
            onClick={() => setIsCreating(true)}
            className="btn-primary"
          >
            <FontAwesomeIcon icon={faPlus} className="mr-2" />
            Создать событие
          </button>
        )}
      </div>

      {isCreating ? (
        <div className="glass-card p-6 mb-6">
          <h2 className="text-2xl font-bold mb-4">
            {editingId ? 'Редактировать событие' : 'Новое событие'}
          </h2>
          <form onSubmit={handleSubmit} className="space-y-4">
            <div>
              <label className="block text-sm font-medium mb-2">
                Заголовок <span className="text-red-500">*</span>
              </label>
              <input
                type="text"
                value={formData.title}
                onChange={(e) => setFormData({ ...formData, title: e.target.value })}
                className="input-field"
                required
              />
            </div>

            <div>
              <label className="block text-sm font-medium mb-2">
                URL (slug) <span className="text-gray-400 text-xs">- оставьте пустым для автогенерации</span>
              </label>
              <input
                type="text"
                value={formData.slug}
                onChange={(e) => setFormData({ ...formData, slug: e.target.value })}
                className="input-field"
                placeholder="Автоматически создастся из заголовка"
              />
            </div>

            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div>
                <label className="block text-sm font-medium mb-2">
                  Дата начала <span className="text-red-500">*</span>
                </label>
                <input
                  type="datetime-local"
                  value={formData.startDate}
                  onChange={(e) => setFormData({ ...formData, startDate: e.target.value })}
                  className="input-field"
                  required
                />
              </div>
              <div>
                <label className="block text-sm font-medium mb-2">
                  Дата окончания <span className="text-red-500">*</span>
                </label>
                <input
                  type="datetime-local"
                  value={formData.endDate}
                  onChange={(e) => setFormData({ ...formData, endDate: e.target.value })}
                  className="input-field"
                  required
                />
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium mb-2">
                Краткое описание <span className="text-gray-400 text-xs">(необязательно)</span>
              </label>
              <textarea
                value={formData.summary}
                onChange={(e) => setFormData({ ...formData, summary: e.target.value })}
                className="input-field"
                rows={3}
                placeholder="Короткое описание для превью"
              />
            </div>

            <div>
              <label className="block text-sm font-medium mb-2">
                Обложка
              </label>
              <div className="flex gap-4 items-start">
                {formData.coverImage && (
                  <img
                    src={resolveMediaUrl(formData.coverImage)}
                    alt="Cover"
                    className="w-32 h-32 object-cover rounded"
                  />
                )}
                <div>
                  <label className="btn-secondary cursor-pointer">
                    <FontAwesomeIcon icon={faUpload} className="mr-2" />
                    Загрузить обложку
                    <input
                      type="file"
                      accept="image/*"
                      onChange={handleCoverImageUpload}
                      className="hidden"
                    />
                  </label>
                </div>
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium mb-2">
                Содержание <span className="text-red-500">*</span>
              </label>
              <RichTextEditor
                content={formData.content}
                onChange={(content) => setFormData({ ...formData, content })}
                placeholder="Напишите описание события..."
              />
            </div>

            <div>
              <label className="block text-sm font-medium mb-2">
                Дополнительные медиа
              </label>
              <div className="space-y-2">
                {formData.mediaUrls.map((url: string, index: number) => (
                  <div key={index} className="flex items-center gap-2">
                    <img
                      src={resolveMediaUrl(url)}
                      alt={`Media ${index + 1}`}
                      className="w-20 h-20 object-cover rounded"
                    />
                    <button
                      type="button"
                      onClick={() => removeMedia(index)}
                      className="btn-secondary text-red-500"
                    >
                      <FontAwesomeIcon icon={faTrash} />
                    </button>
                  </div>
                ))}
                <label className="btn-secondary cursor-pointer">
                  <FontAwesomeIcon icon={faUpload} className="mr-2" />
                  Добавить медиа
                  <input
                    type="file"
                    accept="image/*,video/*"
                    multiple
                    onChange={handleMediaUpload}
                    className="hidden"
                  />
                </label>
              </div>
            </div>

            <div className="flex gap-4">
              <button
                type="submit"
                className="btn-primary"
                disabled={createMutation.isPending || updateMutation.isPending}
              >
                <FontAwesomeIcon icon={faSave} className="mr-2" />
                {createMutation.isPending || updateMutation.isPending
                  ? (editingId ? 'Сохранение...' : 'Создание...')
                  : (editingId ? 'Сохранить' : 'Создать и опубликовать')}
              </button>
              <button
                type="button"
                onClick={resetForm}
                className="btn-secondary"
              >
                <FontAwesomeIcon icon={faTimes} className="mr-2" />
                Отмена
              </button>
            </div>
          </form>
        </div>
      ) : (
        <div className="grid gap-4">
          {eventsList?.items.map((event) => {
            const status = getEventStatus(event.startDate, event.endDate);
            return (
              <div key={event.id} className="glass-card p-4 flex flex-col md:flex-row gap-4">
                {event.coverImage && (
                  <img
                    src={resolveMediaUrl(event.coverImage)}
                    alt={event.title}
                    className="w-32 h-32 object-cover rounded flex-shrink-0"
                  />
                )}
                <div className="flex-1">
                    <div className="flex flex-col sm:flex-row sm:items-start sm:justify-between mb-2 space-y-2 sm:space-y-0">
                    <h3 className="text-xl font-bold">{event.title}</h3>
                    {status.type === 'upcoming' || status.type === 'ongoing' ? (
                      <div className="self-start sm:self-auto">
                        <CountdownTimer
                          targetDate={status.targetDate}
                          compact={true}
                          showLabels={false}
                          statusLabel={status.type === 'upcoming' ? 'До начала' : 'До завершения'}
                          statusTextClass={status.color}
                          statusBgClass={status.bgColor}
                        />
                      </div>
                    ) : (
                      <span className={`px-2 py-1 rounded text-xs font-semibold ${status.color} ${status.bgColor}`}>
                        {status.text}
                      </span>
                    )}
                  </div>
                  <p className="text-gray-400 mb-2">{event.summary}</p>
                  <div className="flex flex-wrap gap-4 text-sm text-gray-500">
                    <span>
                      <FontAwesomeIcon icon={faCalendar} className="mr-1" />
                        {formatServerDate(event.startDate, { year: 'numeric', month: 'short', day: 'numeric', hour: '2-digit', minute: '2-digit' })}
                    </span>
                    <span>
                      <FontAwesomeIcon icon={faCalendar} className="mr-1" />
                      {formatServerDate(event.endDate, { year: 'numeric', month: 'short', day: 'numeric', hour: '2-digit', minute: '2-digit' })}
                    </span>
                    <span><FontAwesomeIcon icon={faEye} /> {event.viewCount}</span>
                    <span><FontAwesomeIcon icon={faHeart} className="mr-1" /> {event.likeCount}</span>
                    <span><FontAwesomeIcon icon={faComment} className="mr-1" /> {event.commentCount}</span>
                  </div>
                </div>
                <div className="flex flex-col gap-2 flex-shrink-0">
                  <button
                    onClick={() => handleEdit(event)}
                    className="btn-secondary"
                  >
                    <FontAwesomeIcon icon={faEdit} />
                  </button>
                  <button
                    onClick={() => {
                      if (confirm('Удалить событие?')) {
                        deleteMutation.mutate(event.id);
                      }
                    }}
                    className="btn-secondary text-red-500"
                  >
                    <FontAwesomeIcon icon={faTrash} />
                  </button>
                </div>
              </div>
            );
          })}
        </div>
      )}
    </div>
  );
};

export default AdminEvents;
