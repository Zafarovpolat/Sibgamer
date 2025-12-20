export const API_URL = import.meta.env.VITE_API_URL || 'https://api.sibgamer.com/api';
export const API_BASE_URL = import.meta.env.VITE_BASE_URL || 'https://sibgamer.com';
export const IMAGE_BASE_URL = import.meta.env.VITE_IMAGE_BASE_URL || API_URL.replace(/\/api\/?$/, '') || API_BASE_URL;
export const SERVER_TIMEZONE_OFFSET_MINUTES = Number(import.meta.env.VITE_SERVER_TZ_OFFSET) || 180;
