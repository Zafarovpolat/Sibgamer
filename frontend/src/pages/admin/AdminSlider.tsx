import { useState, useRef } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useForm } from 'react-hook-form';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus, faEdit, faTrash, faSave, faTimes, faUpload, faImage } from '@fortawesome/free-solid-svg-icons';
import toast from 'react-hot-toast';
import { usePageTitle } from '../../hooks/usePageTitle';
import api from '../../lib/axios';
import { resolveMediaUrl } from '../../lib/media';

interface SliderImageApi {
  id: number;
  imageUrl: string;
  title: string;
  description: string;
  buttons?: string;
}

interface SliderImage {
  id: number;
  imageUrl: string;
  title: string;
  description: string;
  buttons?: Array<{ Text: string; Url: string }>;
}

interface SliderImageDto {
  imageUrl: string;
  title: string;
  description: string;
  buttons?: Array<{ Text: string; Url: string }>;
}

const AdminSlider = () => {
  usePageTitle('Слайдер - Админ панель');

  const queryClient = useQueryClient();
  const [editingId, setEditingId] = useState<number | null>(null);
  const [isAdding, setIsAdding] = useState(false);
  const [uploading, setUploading] = useState(false);
  const [previewUrl, setPreviewUrl] = useState<string>('');
  const [buttons, setButtons] = useState<Array<{ Text: string; Url: string }>>([]);
  const fileInputRef = useRef<HTMLInputElement>(null);

  const { register, handleSubmit, reset, setValue, watch } = useForm<SliderImageDto>();
  const imageUrlValue = watch('imageUrl');

  const { data: images, isLoading } = useQuery({
    queryKey: ['admin-slider'],
    queryFn: async () => {
      const response = await api.get<SliderImageApi[]>('/admin/slider');
      const mapped = response.data.map(image => {
        let parsedButtons: Array<{ Text: string; Url: string }> | undefined = undefined;
        if (image.buttons) {
          try {
            parsedButtons = JSON.parse(image.buttons) as Array<{ Text: string; Url: string }>;
            console.log('Parsed buttons for slider', image.id, parsedButtons);
          } catch (e) {
            console.error('Error parsing buttons for slider', image.id, e);
            parsedButtons = undefined;
          }
        }
        return {
          ...image,
          buttons: parsedButtons
        };
      });
      console.log('Loaded images:', mapped);
      return mapped as SliderImage[];
    },
    staleTime: 0,
  });

  const createMutation = useMutation({
    mutationFn: async (data: SliderImageDto) => {
      await api.post('/admin/slider', { ...data, order: 0 });
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin-slider'] });
      queryClient.invalidateQueries({ queryKey: ['sliderImages'] });
      toast.success('Слайд добавлен');
      setIsAdding(false);
      reset();
      setPreviewUrl('');
    },
    onError: () => {
      toast.error('Ошибка при добавлении слайда');
    },
  });

  const updateMutation = useMutation({
    mutationFn: async ({ id, data }: { id: number; data: SliderImageDto }) => {
      await api.put(`/admin/slider/${id}`, { ...data, order: 0 });
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin-slider'] });
      queryClient.invalidateQueries({ queryKey: ['sliderImages'] });
      toast.success('Слайд обновлён');
      setEditingId(null);
      setPreviewUrl('');
    },
    onError: () => {
      toast.error('Ошибка при обновлении слайда');
    },
  });

  const deleteMutation = useMutation({
    mutationFn: async (id: number) => {
      await api.delete(`/admin/slider/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin-slider'] });
      queryClient.invalidateQueries({ queryKey: ['sliderImages'] });
      toast.success('Слайд удалён');
    },
    onError: () => {
      toast.error('Ошибка при удалении слайда');
    },
  });

  const handleFileSelect = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (!file) return;

    if (!file.type.startsWith('image/')) {
      toast.error('Выберите файл изображения');
      return;
    }

    if (file.size > 5 * 1024 * 1024) {
      toast.error('Размер файла не должен превышать 5 МБ');
      return;
    }

    const reader = new FileReader();
    reader.onloadend = () => {
      setPreviewUrl(reader.result as string);
    };
    reader.readAsDataURL(file);

    setUploading(true);
    const formData = new FormData();
    formData.append('file', file);

    try {
      const response = await api.post<{ url: string }>('/upload/slider', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });
      
      setValue('imageUrl', response.data.url);
      toast.success('Изображение загружено');
    } catch {
      toast.error('Ошибка при загрузке изображения');
      setPreviewUrl('');
    } finally {
      setUploading(false);
    }
  };

  const onSubmit = (data: SliderImageDto) => {
    if (!data.imageUrl) {
      toast.error('Загрузите изображение');
      return;
    }

    const filteredButtons = buttons.filter(btn => btn.Text && btn.Url);
    const submitData = { ...data, buttons: filteredButtons.length > 0 ? filteredButtons : undefined };

    if (editingId) {
      updateMutation.mutate({ id: editingId, data: submitData });
    } else {
      createMutation.mutate(submitData);
    }
  };

  const handleEdit = (image: SliderImage) => {
    setEditingId(image.id);
    setValue('imageUrl', image.imageUrl);
    setValue('title', image.title);
    setValue('description', image.description);
    setButtons(image.buttons || []);
    setPreviewUrl('');
  };

  const handleCancelEdit = () => {
    setEditingId(null);
    setIsAdding(false);
    reset();
    setButtons([]);
    setPreviewUrl('');
  };

  const addButton = () => {
    setButtons([...buttons, { Text: '', Url: '' }]);
  };

  const removeButton = (index: number) => {
    setButtons(buttons.filter((_, i) => i !== index));
  };

  const updateButton = (index: number, field: 'Text' | 'Url', value: string) => {
    const newButtons = [...buttons];
    newButtons[index][field] = value;
    setButtons(newButtons);
  };

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold bg-gradient-to-r from-highlight to-blue-500 bg-clip-text text-transparent">
          Управление слайдером
        </h1>
        {!isAdding && !editingId && (
          <button
            onClick={() => setIsAdding(true)}
            className="btn-primary flex items-center space-x-2"
          >
            <FontAwesomeIcon icon={faPlus} />
            <span>Добавить слайд</span>
          </button>
        )}
      </div>

      {(isAdding || editingId) && (
        <div className="glass-card p-6 mb-6 animate-scale-in">
          <h2 className="text-xl font-bold mb-6">
            {editingId ? 'Редактировать слайд' : 'Новый слайд'}
          </h2>
          <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
            <div>
              <label className="block text-sm font-medium mb-3 text-gray-300">
                Изображение слайда
              </label>
              <div className="space-y-4">
                <input
                  ref={fileInputRef}
                  type="file"
                  accept="image/*"
                  onChange={handleFileSelect}
                  className="hidden"
                />
                
                <div className="flex items-center space-x-4">
                  <button
                    type="button"
                    onClick={() => fileInputRef.current?.click()}
                    disabled={uploading}
                    className="btn-secondary flex items-center space-x-2"
                  >
                    <FontAwesomeIcon icon={faUpload} />
                    <span>{uploading ? 'Загрузка...' : 'Загрузить изображение'}</span>
                  </button>
                  
                  {previewUrl && (
                    <span className="text-green-400 text-sm">✓ Изображение выбрано</span>
                  )}
                </div>

                {(previewUrl || (editingId && imageUrlValue)) && (
                  <div className="glass-card p-4">
                    <img
                      src={previewUrl || resolveMediaUrl(imageUrlValue)}
                      alt="Превью"
                      className="w-full h-64 object-cover rounded-lg"
                    />
                  </div>
                )}

                <input
                  {...register('imageUrl', { required: true })}
                  type="hidden"
                />
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium mb-2 text-gray-300">Заголовок</label>
              <input
                {...register('title', { required: 'Обязательное поле' })}
                type="text"
                className="input-field"
                placeholder="Введите заголовок"
              />
            </div>

            <div>
              <label className="block text-sm font-medium mb-2 text-gray-300">Описание</label>
              <textarea
                {...register('description', { required: 'Обязательное поле' })}
                rows={3}
                className="input-field"
                placeholder="Введите описание"
              />
            </div>

            <div>
              <div className="flex items-center justify-between mb-2">
                <label className="block text-sm font-medium text-gray-300">Кнопки (опционально)</label>
                <button
                  type="button"
                  onClick={addButton}
                  className="btn-secondary text-xs px-2 py-1"
                >
                  + Добавить кнопку
                </button>
              </div>
              <div className="space-y-2">
                {buttons.map((button, index) => (
                  <div key={index} className="flex items-center space-x-2">
                    <input
                      type="text"
                      value={button.Text}
                      onChange={(e) => updateButton(index, 'Text', e.target.value)}
                      placeholder="Текст кнопки"
                      className="input-field flex-1"
                    />
                    <input
                      type="url"
                      value={button.Url}
                      onChange={(e) => updateButton(index, 'Url', e.target.value)}
                      placeholder="URL"
                      className="input-field flex-1"
                    />
                    <button
                      type="button"
                      onClick={() => removeButton(index)}
                      className="btn-secondary text-red-400 hover:text-red-300 px-2 py-2"
                    >
                      <FontAwesomeIcon icon={faTimes} />
                    </button>
                  </div>
                ))}
              </div>
            </div>

            <div className="flex space-x-3">
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
        </div>
      )}

      {isLoading ? (
        <div className="text-center py-20">
          <div className="inline-block animate-spin rounded-full h-16 w-16 border-t-4 border-b-4 border-highlight"></div>
          <p className="mt-4 text-gray-400">Загрузка...</p>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {images?.map((image) => (
            <div key={image.id} className="glass-card p-0 overflow-hidden group animate-scale-in">
              <div className="relative h-48 overflow-hidden">
                <img
                  src={resolveMediaUrl(image.imageUrl)}
                  alt={image.title}
                  className="w-full h-full object-cover transition-transform duration-500 group-hover:scale-110"
                  onError={(e) => {
                    const target = e.target as HTMLImageElement;
                    target.src = '/maps/default.svg';
                  }}
                />
                <div className="absolute inset-0 bg-gradient-to-t from-black/90 via-black/50 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300" />
                <div className="absolute bottom-3 left-3 right-3 opacity-0 group-hover:opacity-100 transition-opacity duration-300">
                  <FontAwesomeIcon icon={faImage} className="text-highlight mr-2" />
                  <span className="text-white text-sm">Превью слайда</span>
                </div>
              </div>

              <div className="p-4">
                <h3 className="text-lg font-bold mb-2 truncate group-hover:text-highlight transition-colors">
                  {image.title}
                </h3>
                <p className="text-gray-400 text-sm mb-4 line-clamp-2">
                  {image.description}
                </p>
                {image.buttons && image.buttons.length > 0 && (
                  <div className="mb-4">
                    <p className="text-xs text-gray-500 mb-1">Кнопки:</p>
                    <div className="flex flex-wrap gap-1">
                      {image.buttons.map((btn, idx) => (
                        <span key={idx} className="text-xs bg-gray-700 px-2 py-1 rounded">
                          {btn.Text}
                        </span>
                      ))}
                    </div>
                  </div>
                )}

                <div className="flex space-x-2">
                  <button
                    onClick={() => handleEdit(image)}
                    className="flex-1 btn-secondary flex items-center justify-center space-x-2 text-sm"
                  >
                    <FontAwesomeIcon icon={faEdit} />
                    <span>Изменить</span>
                  </button>
                  <button
                    onClick={() => {
                      if (confirm('Удалить этот слайд?')) {
                        deleteMutation.mutate(image.id);
                      }
                    }}
                    className="flex-1 bg-red-600/20 hover:bg-red-600 text-red-400 hover:text-white border border-red-600/50 hover:border-red-600 font-bold py-2 px-4 rounded-lg transition-all duration-300 flex items-center justify-center space-x-2 text-sm"
                  >
                    <FontAwesomeIcon icon={faTrash} />
                    <span>Удалить</span>
                  </button>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      {!isLoading && images?.length === 0 && (
        <div className="glass-card text-center py-12">
          <FontAwesomeIcon icon={faImage} className="text-6xl text-gray-600 mb-4" />
          <p className="text-gray-400 text-lg mb-4">Нет слайдов</p>
          <button
            onClick={() => setIsAdding(true)}
            className="btn-primary"
          >
            Добавить первый слайд
          </button>
        </div>
      )}
    </div>
  );
};

export default AdminSlider;
