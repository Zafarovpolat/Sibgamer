import { API_BASE_URL, API_URL, IMAGE_BASE_URL } from '../config/api';
import api from './axios';

export function resolveMediaUrl(url?: string | null): string {
  if (!url) return '';
  const trimmed = String(url).trim();
  if (/^(https?:)?\/\//i.test(trimmed)) return trimmed;

  const prefix = IMAGE_BASE_URL || API_URL.replace(/\/api\/?$/, '') || API_BASE_URL;
  return `${prefix}${trimmed}`;
}

/**
 * Загрузка медиа-файла на сервер
 */
export const uploadMedia = async (file: File): Promise<string> => {
  const formData = new FormData();
  formData.append('file', file);

  const response = await api.post('/upload/image', formData, {
    headers: {
      'Content-Type': 'multipart/form-data',
    },
  });

  return response.data.url;
};

/**
 * Загрузка нескольких файлов
 */
export const uploadMultipleMedia = async (files: File[]): Promise<string[]> => {
  const urls: string[] = [];

  for (const file of files) {
    const url = await uploadMedia(file);
    urls.push(url);
  }

  return urls;
};

/**
 * Проверка типа файла
 */
export const isImageFile = (file: File): boolean => {
  return file.type.startsWith('image/');
};

/**
 * Проверка размера файла (в МБ)
 */
export const checkFileSize = (file: File, maxSizeMB: number = 10): boolean => {
  const maxBytes = maxSizeMB * 1024 * 1024;
  return file.size <= maxBytes;
};

/**
 * Получить превью изображения как base64
 */
export const getImagePreview = (file: File): Promise<string> => {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.onload = (e) => resolve(e.target?.result as string);
    reader.onerror = reject;
    reader.readAsDataURL(file);
  });
};