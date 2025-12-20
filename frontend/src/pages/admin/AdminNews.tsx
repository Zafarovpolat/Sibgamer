import { useState } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus, faEdit, faTrash, faSave, faTimes, faEye, faUpload, faHeart, faComment } from '@fortawesome/free-solid-svg-icons';
import RichTextEditor from '../../components/RichTextEditor';
import { API_URL } from '../../config/api';
import { resolveMediaUrl } from '../../lib/media';
import { getAuthToken } from '../../lib/auth';
import type { NewsListItem, CreateNewsData } from '../../types';

const AdminNews = () => {
  const queryClient = useQueryClient();
  const [isCreating, setIsCreating] = useState(false);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [formData, setFormData] = useState<CreateNewsData>({
    title: '',
    content: '',
    summary: '',
    slug: '',
    coverImage: '',
    isPublished: true,
    mediaUrls: []
  });

  const { data: newsList, isLoading } = useQuery<{ items: NewsListItem[] }>({
    queryKey: ['admin-news'],
    queryFn: async () => {
      const res = await fetch(`${API_URL}/news?pageSize=100`);
      if (!res.ok) throw new Error('Failed to fetch news');
      return res.json();
    }
  });

  const createMutation = useMutation({
    mutationFn: async (data: CreateNewsData) => {
      const token = getAuthToken();
      const res = await fetch(`${API_URL}/news`, {
        method: 'POST',
        headers: { 
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(data)
      });
      if (!res.ok) throw new Error('Failed to create news');
      return res.json();
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin-news'] });
      resetForm();
    }
  });

  const updateMutation = useMutation({
    mutationFn: async ({ id, data }: { id: number; data: CreateNewsData }) => {
      const token = getAuthToken();
      const res = await fetch(`${API_URL}/news/${id}`, {
        method: 'PUT',
        headers: { 
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(data)
      });
      if (!res.ok) throw new Error('Failed to update news');
      return res.json();
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin-news'] });
      resetForm();
    }
  });

  const deleteMutation = useMutation({
    mutationFn: async (id: number) => {
      const token = getAuthToken();
      const res = await fetch(`${API_URL}/news/${id}`, {
        method: 'DELETE',
        headers: { 
          'Authorization': `Bearer ${token}`
        }
      });
      if (!res.ok) throw new Error('Failed to delete news');
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin-news'] });
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
      mediaUrls: []
    });
    setIsCreating(false);
    setEditingId(null);
  };

  const handleEdit = async (news: NewsListItem) => {
    setEditingId(news.id);
    setIsCreating(true);
    
    try {
      const res = await fetch(`${API_URL}/news/${news.slug}`);
      if (!res.ok) throw new Error('Failed to fetch news');
      
      const data = await res.json();
      
      setFormData({
        title: data.title || '',
        content: data.content || '',
        summary: data.summary || '',
        slug: data.slug || '',
        coverImage: data.coverImage || '',
        isPublished: data.isPublished || false,
        mediaUrls: data.media?.map((m: { mediaUrl: string }) => m.mediaUrl) || []
      });
    } catch (error) {
      console.error('Failed to load news for editing:', error);
      alert('Ошибка загрузки новости для редактирования');
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

    const formData = new FormData();
    formData.append('file', file);

    try {
      const token = getAuthToken();
      const res = await fetch(`${API_URL}/upload/news`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`
        },
        body: formData
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
      const res = await fetch(`${API_URL}/upload/news`, {
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

  if (isLoading) {
    return <div className="text-center py-10">Загрузка...</div>;
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Управление новостями</h1>
        {!isCreating && (
          <button
            onClick={() => setIsCreating(true)}
            className="btn-primary"
          >
            <FontAwesomeIcon icon={faPlus} className="mr-2" />
            Создать новость
          </button>
        )}
      </div>

      {isCreating ? (
        <div className="glass-card p-6 mb-6">
          <h2 className="text-2xl font-bold mb-4">
            {editingId ? 'Редактировать новость' : 'Новая новость'}
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
                placeholder="Напишите содержание новости..."
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

            <div className="flex flex-col sm:flex-row gap-4">
              <button
                type="button"
                onClick={resetForm}
                className="btn-secondary"
              >
                <FontAwesomeIcon icon={faTimes} className="mr-2" />
                Отмена
              </button>
              <button
                type="submit"
                className="btn-primary sm:ml-auto shrink-0"
                disabled={createMutation.isPending || updateMutation.isPending}
              >
                <FontAwesomeIcon icon={faSave} className="mr-2" />
                {createMutation.isPending || updateMutation.isPending
                  ? (editingId ? 'Сохранение...' : 'Создание...')
                  : (editingId ? 'Сохранить' : 'Создать')}
              </button>
            </div>
          </form>
        </div>
      ) : (
        <div className="grid gap-4">
          {newsList?.items.map((news) => (
            <div key={news.id} className="glass-card p-4 flex flex-col sm:flex-row gap-4 items-start">
              {news.coverImage && (
                <img
                  src={resolveMediaUrl(news.coverImage)}
                  alt={news.title}
                  className="w-32 h-32 object-cover rounded"
                />
              )}
              <div className="flex-1">
                <h3 className="text-xl font-bold mb-2">{news.title}</h3>
                <p className="text-gray-400 mb-2">{news.summary}</p>
                <div className="flex gap-4 text-sm text-gray-500">
                  <span><FontAwesomeIcon icon={faEye} /> {news.viewCount}</span>
                  <span><FontAwesomeIcon icon={faHeart} className="mr-1" /> {news.likeCount}</span>
                  <span><FontAwesomeIcon icon={faComment} className="mr-1" /> {news.commentCount}</span>
                  <span className={news.isPublished ? 'text-green-500' : 'text-red-500'}>
                    {news.isPublished ? 'Опубликовано' : 'Черновик'}
                  </span>
                </div>
              </div>
              <div className="flex gap-2 sm:flex-col items-center sm:items-end">
                <button
                  onClick={() => handleEdit(news)}
                  className="btn-secondary"
                >
                  <FontAwesomeIcon icon={faEdit} />
                </button>
                <button
                  onClick={() => {
                    if (confirm('Удалить новость?')) {
                      deleteMutation.mutate(news.id);
                    }
                  }}
                  className="btn-secondary text-red-500"
                >
                  <FontAwesomeIcon icon={faTrash} />
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default AdminNews;
