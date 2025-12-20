import axios from 'axios';

export function getErrorMessage(error: unknown): string {
  if (!error) return 'Неизвестная ошибка';
  if (typeof error === 'string') return error;
  if (error instanceof Error) return error.message;
  try {
    if (axios.isAxiosError(error)) {
      const data = (error as any).response?.data;
      if (data && typeof data === 'object') {
        if (typeof data.message === 'string') return data.message;
        if (typeof data.error === 'string') return data.error;
      }
      return error.message || 'Ошибка HTTP запроса';
    }
  } catch {
    // ignore
  }

  try {
    return JSON.stringify(error as object);
  } catch {
    return String(error);
  }
}
