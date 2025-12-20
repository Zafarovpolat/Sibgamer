import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faImage, faEye, faArrowRight, faCalendar, faCircle } from '@fortawesome/free-solid-svg-icons';
import { API_URL } from '../config/api';
import { resolveMediaUrl } from '../lib/media';
import { usePageTitle } from '../hooks/usePageTitle';
import type { EventListItem } from '../types';
import { formatServerDateOnly, getServerLocalTime, parseServerDate } from '../utils/dateUtils';
import CountdownTimer from '../components/CountdownTimer';

const Events = () => {
  usePageTitle('События');
  
  const [page, setPage] = useState(1);

  const { data: settings } = useQuery<Record<string, string>>({
    queryKey: ['settings'],
    queryFn: async () => {
      const res = await fetch(`${API_URL}/settings`);
      if (!res.ok) throw new Error('Failed to fetch settings');
      return res.json();
    }
  });

  const pageSize = parseInt(settings?.events_per_page || '6', 10); 

  const { data, isLoading } = useQuery<{ items: EventListItem[]; totalCount: number; totalPages: number }>({
    queryKey: ['events', page, pageSize],
    queryFn: async () => {
      const res = await fetch(`${API_URL}/events?page=${page}&pageSize=${pageSize}`);
      if (!res.ok) throw new Error('Failed to fetch events');
      return res.json();
    },
    enabled: !!settings
  });

  const getEventStatus = (startDate: string, endDate: string) => {
    const now = getServerLocalTime();
    const start = parseServerDate(startDate);
    const end = parseServerDate(endDate);

    if (now < start) {
      return { type: 'upcoming' as const, targetDate: startDate, color: 'text-white', bgColor: 'bg-black/40' };
    }
    
    if (now >= start && now <= end) {
      return { type: 'ongoing' as const, targetDate: endDate, color: 'text-green-400', bgColor: 'bg-black/40' };
    }
    
    return { type: 'finished' as const, text: 'Завершено', color: 'text-red-400', bgColor: 'bg-black/40' };
  };

  if (isLoading) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="text-center py-10">Загрузка событий...</div>
      </div>
    );
  }

  if (!data || !data.items) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="rounded-xl border border-gray-800/50 p-8 text-center">
          <p className="text-gray-400 text-lg">Не удалось загрузить события</p>
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="mb-8">
        <h1 className="text-4xl font-bold mb-2">События</h1>
        <p className="text-gray-400">Игровые события и турниры</p>
      </div>

      {data.items.length === 0 ? (
        <div className="rounded-xl border border-gray-800/50 p-8 text-center max-w-2xl mx-auto">
          <p className="text-gray-400 text-lg">Событий пока нет</p>
        </div>
      ) : (
        <>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-8">
            {(
              data?.items.slice().sort((a, b) => {
                const now = getServerLocalTime();
                const aStart = parseServerDate(a.startDate);
                const aEnd = parseServerDate(a.endDate);
                const bStart = parseServerDate(b.startDate);
                const bEnd = parseServerDate(b.endDate);

                const aStatus = (now >= aStart && now <= aEnd) ? 0 : (now < aStart ? 1 : 2);
                const bStatus = (now >= bStart && now <= bEnd) ? 0 : (now < bStart ? 1 : 2);

                if (aStatus !== bStatus) return aStatus - bStatus;

                if (aStatus === 0) {
                  return aEnd.getTime() - bEnd.getTime() > 0 ? 1 : -1;
                }
                if (aStatus === 1) {
                  return aStart.getTime() - bStart.getTime() > 0 ? 1 : -1;
                }
                return bEnd.getTime() - aEnd.getTime() > 0 ? 1 : -1;
              }) || []
            ).map((event) => {
              const status = getEventStatus(event.startDate, event.endDate);
              return (
                <div
                  key={event.id}
                  className="group overflow-hidden rounded-xl border border-gray-800/50 hover:border-gray-700/50 transition-all duration-200 flex flex-col"
                >
                  <Link to={`/events/${event.slug}`} className="relative">
                    {event.coverImage ? (
                      <div className="relative h-64 overflow-hidden">
                        <img
                          src={resolveMediaUrl(event.coverImage)}
                          alt={event.title}
                          className="w-full h-full object-cover"
                        />
                        <div className="absolute inset-0 bg-gradient-to-t from-gray-900 to-transparent" />
                        
                        <div className="absolute top-4 right-4">
                          {status.type === 'upcoming' || status.type === 'ongoing' ? (
                            <CountdownTimer
                              targetDate={status.targetDate}
                              className="text-sm"
                              showLabels={false}
                              compact={true}
                              statusLabel={status.type === 'upcoming' ? 'До начала' : 'До завершения'}
                              statusTextClass={status.color}
                              statusBgClass={status.bgColor}
                            />
                            ) : (
                            <div className={`flex items-center space-x-2 px-3 py-1 rounded-full ${status.bgColor} backdrop-blur-md border border-gray-700/40 text-sm font-semibold`.trim()}>
                              <FontAwesomeIcon icon={faCircle} className={`${status.color} text-xs`} />
                              <span className={`${status.color}`.trim()}>{status.text}</span>
                            </div>
                          )}
                        </div>
                      </div>
                    ) : (
                      <div className="h-64 bg-gray-800/50 flex items-center justify-center relative">
                        <FontAwesomeIcon icon={faImage} className="text-6xl text-gray-700" />
                        
                        <div className="absolute top-4 right-4">
                          {status.type === 'upcoming' || status.type === 'ongoing' ? (
                            <CountdownTimer
                              targetDate={status.targetDate}
                              className="text-sm"
                              showLabels={false}
                              compact={true}
                              statusLabel={status.type === 'upcoming' ? 'До начала' : 'До завершения'}
                              statusTextClass={status.color}
                              statusBgClass={status.bgColor}
                            />
                          ) : (
                            <div className={`flex items-center space-x-2 px-3 py-1 rounded-full ${status.bgColor} backdrop-blur-md border border-gray-700/40 text-sm font-semibold`.trim()}>
                              <FontAwesomeIcon icon={faCircle} className={`${status.color} text-xs`} />
                              <span className={`${status.color}`.trim()}>{status.text}</span>
                            </div>
                          )}
                        </div>
                      </div>
                    )}
                  </Link>

                  <div className="p-6 flex-1 flex flex-col">
                    <Link to={`/events/${event.slug}`}>
                      <h3 className="text-2xl font-bold mb-3 text-white group-hover:text-highlight transition-colors duration-200 line-clamp-2">
                        {event.title}
                      </h3>
                    </Link>

                    {event.summary && (
                      <p className="text-gray-400 text-base mb-4 line-clamp-3 flex-1">
                        {event.summary}
                      </p>
                    )}

                    <div className="flex items-center justify-between gap-4 pt-4 border-t border-gray-800/30">
                      <div className="flex items-center gap-4 text-sm text-gray-500">
                        <div className="flex items-center gap-1">
                          <FontAwesomeIcon icon={faCalendar} />
                          <span>{formatServerDateOnly(event.startDate)}</span>
                        </div>
                        <span>•</span>
                        <div className="flex items-center gap-1">
                          <FontAwesomeIcon icon={faEye} />
                          <span>{event.viewCount}</span>
                        </div>
                      </div>

                      <Link 
                        to={`/events/${event.slug}`}
                        className="flex items-center gap-2 px-4 py-2 rounded-lg border border-gray-800/50 text-gray-300 hover:text-white hover:border-highlight hover:bg-highlight/10 transition-all duration-200 text-sm whitespace-nowrap"
                      >
                        Подробнее
                        <FontAwesomeIcon icon={faArrowRight} className="text-xs" />
                      </Link>
                    </div>
                  </div>
                </div>
              );
            })}
          </div>

          {data.totalPages > 1 && (
            <div className="flex justify-center gap-2">
              <button
                onClick={() => setPage(p => Math.max(1, p - 1))}
                disabled={page === 1}
                className="px-4 py-2 rounded-lg border border-gray-800/50 text-gray-300 hover:text-white hover:border-highlight disabled:opacity-50 disabled:cursor-not-allowed transition-all duration-200"
              >
                Назад
              </button>

              {Array.from({ length: data.totalPages }, (_, i) => i + 1).map((pageNum) => (
                <button
                  key={pageNum}
                  onClick={() => setPage(pageNum)}
                  className={`px-4 py-2 rounded-lg border transition-all duration-200 ${
                    pageNum === page
                      ? 'bg-highlight border-highlight text-white'
                      : 'border-gray-800/50 text-gray-300 hover:text-white hover:border-highlight'
                  }`}
                >
                  {pageNum}
                </button>
              ))}

              <button
                onClick={() => setPage(p => Math.min(data.totalPages, p + 1))}
                disabled={page === data.totalPages}
                className="px-4 py-2 rounded-lg border border-gray-800/50 text-gray-300 hover:text-white hover:border-highlight disabled:opacity-50 disabled:cursor-not-allowed transition-all duration-200"
              >
                Вперёд
              </button>
            </div>
          )}
        </>
      )}
    </div>
  );
};

export default Events;
