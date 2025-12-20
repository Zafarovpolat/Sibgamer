import { API_BASE_URL, API_URL, IMAGE_BASE_URL } from '../config/api';

export function resolveMediaUrl(url?: string | null): string {
  if (!url) return '';
  const trimmed = String(url).trim();
  if (/^(https?:)?\/\//i.test(trimmed)) return trimmed;

  const prefix = IMAGE_BASE_URL || API_URL.replace(/\/api\/?$/, '') || API_BASE_URL;
  return `${prefix}${trimmed}`;
}
