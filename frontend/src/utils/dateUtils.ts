import { SERVER_TIMEZONE_OFFSET_MINUTES } from '../config/api';
import api from '../lib/axios';

/**
 * Маппинг Windows timezone -> IANA timezone
 */
const WINDOWS_TO_IANA: Record<string, string> = {
  'Russian Standard Time': 'Europe/Moscow',
  'Russia Time Zone 3': 'Europe/Samara',
  'Ekaterinburg Standard Time': 'Asia/Yekaterinburg',
  'N. Central Asia Standard Time': 'Asia/Novosibirsk',
  'North Asia Standard Time': 'Asia/Krasnoyarsk',
  'North Asia East Standard Time': 'Asia/Irkutsk',
  'Yakutsk Standard Time': 'Asia/Yakutsk',
  'Vladivostok Standard Time': 'Asia/Vladivostok',
  'Russia Time Zone 10': 'Asia/Srednekolymsk',
  'Russia Time Zone 11': 'Asia/Kamchatka',
  'Kaliningrad Standard Time': 'Europe/Kaliningrad',
  'UTC': 'UTC',
  'Coordinated Universal Time': 'UTC',
  // Добавьте другие при необходимости
};

/**
 * Конвертация Windows timezone в IANA формат
 */
function toIanaTimezone(timezone: string): string {
  // Если уже в формате IANA (содержит /)
  if (timezone.includes('/')) {
    return timezone;
  }

  // Попробовать найти в маппинге
  const mapped = WINDOWS_TO_IANA[timezone];
  if (mapped) {
    return mapped;
  }

  // Проверить, валидный ли timezone
  try {
    Intl.DateTimeFormat('ru-RU', { timeZone: timezone });
    return timezone;
  } catch {
    console.warn(`Unknown timezone "${timezone}", falling back to Europe/Moscow`);
    return 'Europe/Moscow';
  }
}

/**
 * Проверка валидности timezone
 */
function isValidTimezone(timezone: string): boolean {
  try {
    Intl.DateTimeFormat('ru-RU', { timeZone: timezone });
    return true;
  } catch {
    return false;
  }
}

export function parseServerDate(dateString: string): Date {
  if (!dateString) return new Date(NaN);

  // Если дата уже содержит timezone информацию
  if (/[zZ]|[+-]\d{2}:?\d{2}$/.test(dateString)) {
    return new Date(dateString);
  }

  const parts = dateString.match(/(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2})(?::(\d{2})(?:\.(\d+))?)?/);
  if (!parts) {
    return new Date(dateString);
  }

  const year = Number(parts[1]);
  const month = Number(parts[2]);
  const day = Number(parts[3]);
  const hour = Number(parts[4]);
  const minute = Number(parts[5]);
  const second = Number(parts[6] || '0');
  const ms = Number((parts[7] || '0').slice(0, 3).padEnd(3, '0'));
  const utcMs = Date.UTC(year, month - 1, day, hour, minute, second, ms);
  const serverOffsetMs = _serverOffsetMinutes * 60 * 1000;
  return new Date(utcMs - serverOffsetMs);
}

let _serverOffsetMinutes: number = SERVER_TIMEZONE_OFFSET_MINUTES;
let _clientServerDeltaMs: number = 0;
let _serverTimeZoneId: string = 'Europe/Moscow';

export async function initServerTime(): Promise<void> {
  try {
    const res = await api.get('/system/time');
    const data = res.data as {
      serverTime?: string;
      timezoneOffsetMinutes?: number;
      timezoneId?: string
    };

    if (data?.timezoneOffsetMinutes != null) {
      _serverOffsetMinutes = Number(data.timezoneOffsetMinutes);
    }

    if (data?.timezoneId) {
      // Конвертируем Windows timezone в IANA
      _serverTimeZoneId = toIanaTimezone(String(data.timezoneId));
      console.log(`Server timezone: ${data.timezoneId} -> ${_serverTimeZoneId}`);
    }

    if (data?.serverTime) {
      const serverMs = new Date(data.serverTime).getTime();
      const clientMs = Date.now();
      _clientServerDeltaMs = serverMs - clientMs;
    }
  } catch (e) {
    console.warn('Failed to sync server time, using defaults', e);
    // Убедимся что используется валидный timezone
    if (!isValidTimezone(_serverTimeZoneId)) {
      _serverTimeZoneId = 'Europe/Moscow';
    }
  }
}

export function getServerOffsetMinutes(): number {
  return _serverOffsetMinutes;
}

export function getClientServerDeltaMs(): number {
  return _clientServerDeltaMs;
}

export function getServerTimezone(): string {
  return _serverTimeZoneId;
}

export function formatServerDate(
  date: string | Date | null | undefined,
  options?: Intl.DateTimeFormatOptions
): string {
  if (!date) return '';

  try {
    const dateObj = typeof date === 'string' ? parseServerDate(date) : new Date(date);

    if (isNaN(dateObj.getTime())) {
      return '';
    }

    const defaultOptions: Intl.DateTimeFormatOptions = {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
      timeZone: _serverTimeZoneId
    };

    const mergedOptions = { ...defaultOptions, ...options };

    return new Intl.DateTimeFormat('ru-RU', mergedOptions).format(dateObj);
  } catch (error) {
    console.error('Error formatting date:', error, { date, timezone: _serverTimeZoneId });
    return '';
  }
}

export function formatServerDateOnly(date: string | Date | null | undefined): string {
  if (!date) return '';

  try {
    const dateObj = typeof date === 'string' ? parseServerDate(date) : new Date(date);
    return formatServerDate(dateObj, {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: undefined,
      minute: undefined,
      second: undefined,
    });
  } catch (error) {
    console.error('Error formatting date only:', error);
    return '';
  }
}

export function formatServerTimeOnly(date: string | Date | null | undefined): string {
  if (!date) return '';

  return formatServerDate(date, {
    year: undefined,
    month: undefined,
    day: undefined,
    hour: '2-digit',
    minute: '2-digit',
    second: undefined,
  });
}

export function formatServerDateShort(date: string | Date | null | undefined): string {
  if (!date) return '';

  return formatServerDate(date, {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: undefined,
    minute: undefined,
    second: undefined,
  });
}

export function isForeverDate(date: string | Date | null | undefined): boolean {
  if (!date) return false;

  try {
    const dateObj = typeof date === 'string' ? parseServerDate(date) : new Date(date);
    const year = dateObj.getUTCFullYear();
    return year >= 9999;
  } catch {
    return false;
  }
}

export function formatForDatetimeLocal(date: string | Date | null | undefined): string {
  if (!date) return '';

  try {
    const dateObj = typeof date === 'string' ? parseServerDate(date) : new Date(date);

    if (isNaN(dateObj.getTime())) {
      return '';
    }

    const dtf = new Intl.DateTimeFormat('sv-SE', {
      timeZone: _serverTimeZoneId,
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      hour12: false,
    });

    const parts = dtf.formatToParts(dateObj);
    const out: Record<string, string> = {};
    parts.forEach(p => { if (p.type !== 'literal') out[p.type] = p.value; });

    return `${out.year}-${out.month}-${out.day}T${out.hour}:${out.minute}`;
  } catch (error) {
    console.error('Error formatting for datetime-local:', error);
    return '';
  }
}

export function getServerLocalTime(): Date {
  return new Date(Date.now() + _clientServerDeltaMs);
}

/**
 * Относительное время (X минут назад, вчера и т.д.)
 */
export function formatRelativeTime(date: string | Date | null | undefined): string {
  if (!date) return '';

  try {
    const dateObj = typeof date === 'string' ? parseServerDate(date) : new Date(date);

    if (isNaN(dateObj.getTime())) return '';

    const now = new Date();
    const diffMs = now.getTime() - dateObj.getTime();
    const diffSeconds = Math.floor(diffMs / 1000);
    const diffMinutes = Math.floor(diffSeconds / 60);
    const diffHours = Math.floor(diffMinutes / 60);
    const diffDays = Math.floor(diffHours / 24);

    if (diffSeconds < 0) {
      // Дата в будущем
      return formatServerDateOnly(date);
    } else if (diffSeconds < 60) {
      return 'только что';
    } else if (diffMinutes < 60) {
      return `${diffMinutes} ${pluralize(diffMinutes, 'минуту', 'минуты', 'минут')} назад`;
    } else if (diffHours < 24) {
      return `${diffHours} ${pluralize(diffHours, 'час', 'часа', 'часов')} назад`;
    } else if (diffDays === 1) {
      return 'вчера';
    } else if (diffDays < 7) {
      return `${diffDays} ${pluralize(diffDays, 'день', 'дня', 'дней')} назад`;
    } else {
      return formatServerDateOnly(date);
    }
  } catch (error) {
    console.error('Error formatting relative time:', error);
    return '';
  }
}

/**
 * Склонение слов
 */
function pluralize(n: number, one: string, few: string, many: string): string {
  const mod10 = n % 10;
  const mod100 = n % 100;

  if (mod100 >= 11 && mod100 <= 19) {
    return many;
  } else if (mod10 === 1) {
    return one;
  } else if (mod10 >= 2 && mod10 <= 4) {
    return few;
  } else {
    return many;
  }
}

/**
 * Проверка, прошла ли дата
 */
export function isPastDate(date: string | Date | null | undefined): boolean {
  if (!date) return false;

  try {
    const dateObj = typeof date === 'string' ? parseServerDate(date) : new Date(date);
    return dateObj.getTime() < Date.now();
  } catch {
    return false;
  }
}

/**
 * Проверка, сегодня ли дата
 */
export function isToday(date: string | Date | null | undefined): boolean {
  if (!date) return false;

  try {
    const dateObj = typeof date === 'string' ? parseServerDate(date) : new Date(date);
    const today = new Date();

    return (
      dateObj.getDate() === today.getDate() &&
      dateObj.getMonth() === today.getMonth() &&
      dateObj.getFullYear() === today.getFullYear()
    );
  } catch {
    return false;
  }
}

export default {
  parseServerDate,
  initServerTime,
  formatServerDate,
  formatServerDateOnly,
  formatServerTimeOnly,
  formatServerDateShort,
  formatForDatetimeLocal,
  formatRelativeTime,
  isForeverDate,
  isPastDate,
  isToday,
  getServerLocalTime,
  getServerOffsetMinutes,
  getClientServerDeltaMs,
  getServerTimezone,
};