import { useQuery } from '@tanstack/react-query';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEye, faArrowRight, faImage, faCircle } from '@fortawesome/free-solid-svg-icons';
import { API_URL } from '../config/api';
import { resolveMediaUrl } from '../lib/media';
import type { EventListItem } from '../types';
import CountdownTimer from './CountdownTimer';
import { parseServerDate, getServerLocalTime } from '../utils/dateUtils';

const EventsSection = () => {
  const { data: events, isLoading } = useQuery<EventListItem[]>({
    queryKey: ['upcoming-events'],
    queryFn: async () => {
      const res = await fetch(`${API_URL}/events/upcoming?count=3`);
      if (!res.ok) throw new Error('Failed to fetch events');
      return res.json();
    },
    refetchInterval: 60000, 
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
      <div className="mb-16">
        <div className="text-center">Загрузка событий...</div>
      </div>
    );
  }

  if (!events || events.length === 0) {
    return null;
  }

  return (
    <div className="mb-16">
      <div className="flex items-center justify-between mb-6">
        <div className="mb-6">
        <h2 className="text-3xl font-bold text-white mb-2">
          События
        </h2>
        <p className="text-gray-400">
          Ближайшие события и турниры нашего сообщества
        </p>
      </div>
        <Link
          to="/events"
          className="flex items-center gap-2 px-4 py-2 rounded-lg border border-gray-800/50 text-gray-300 hover:text-white hover:border-gray-700/50 transition-all duration-200 text-sm"
        >
          Перейти
          <FontAwesomeIcon icon={faArrowRight} className="text-xs" />
        </Link>
      </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {events.map((event) => {
            const status = getEventStatus(event.startDate, event.endDate);
            return (
              <div
                key={event.id}
                className="group overflow-hidden rounded-xl border border-gray-800/50 hover:border-gray-700/50 transition-all duration-200"
              >
                <Link to={`/events/${event.slug}`}>
                    {event.coverImage ? (
                    <div className="relative h-44 overflow-hidden">
                      <img
                        src={resolveMediaUrl(event.coverImage)}
                        alt={event.title}
                        className="w-full h-full object-cover"
                      />
                      <div className="absolute inset-0 bg-gradient-to-t from-gray-900 to-transparent" />

                      <div className="absolute top-3 right-3">
                        {status.type === 'upcoming' || status.type === 'ongoing' ? (
                          <CountdownTimer
                            targetDate={status.targetDate}
                            className="text-xs"
                            showLabels={false}
                            compact={true}
                            statusLabel={status.type === 'upcoming' ? 'До начала' : 'До завершения'}
                            statusTextClass={status.color}
                            statusBgClass={status.bgColor}
                          />
                        ) : (
                          <div className={`flex items-center space-x-2 px-3 py-1 rounded-full ${status.bgColor} backdrop-blur-md border border-gray-700/40 text-xs font-semibold`}>
                            <FontAwesomeIcon icon={faCircle} className={`${status.color} text-xs`} />
                            <span className={`${status.color}`.trim()}>{status.text}</span>
                          </div>
                        )}
                      </div>
                    </div>
                  ) : (
                    <div className="h-44 bg-gray-800/50 flex items-center justify-center relative">
                      <FontAwesomeIcon icon={faImage} className="text-5xl text-gray-700" />
                      
                      <div className="absolute top-3 right-3">
                        {status.type === 'upcoming' || status.type === 'ongoing' ? (
                          <CountdownTimer
                            targetDate={status.targetDate}
                            className="text-xs"
                            showLabels={false}
                            compact={true}
                            statusLabel={status.type === 'upcoming' ? 'До начала' : 'До завершения'}
                            statusTextClass={status.color}
                            statusBgClass={status.bgColor}
                          />
                        ) : (
                          <div className={`px-3 py-1 rounded-full text-xs font-semibold ${status.color} bg-gray-900/80 backdrop-blur-sm flex items-center gap-2`}>
                            <FontAwesomeIcon icon={faCircle} className={`${status.color} text-xs`} />
                            <span>{status.text}</span>
                          </div>
                        )}
                      </div>
                    </div>
                  )}
                </Link>

                <div className="p-4">
                  <Link to={`/events/${event.slug}`}>
                    <h3 className="text-lg font-semibold mb-2 line-clamp-2 text-white group-hover:text-highlight transition-colors duration-200">
                      {event.title}
                    </h3>
                  </Link>

                  {event.summary && (
                    <p className="text-gray-400 text-sm mb-4 truncate">
                      {event.summary}
                    </p>
                  )}

                  <div className="flex items-center justify-between gap-2">
                    <div className="flex items-center gap-1 text-xs text-gray-500 flex-1">
                      <FontAwesomeIcon icon={faEye} />
                      <span>{event.viewCount}</span>
                    </div>

                    <div className="flex-1 flex justify-end">
                      <Link 
                        to={`/events/${event.slug}`}
                        className="flex items-center gap-1 px-3 py-1.5 rounded-lg border border-gray-800/50 text-gray-300 hover:text-white hover:border-highlight hover:bg-highlight/10 transition-all duration-200 text-xs whitespace-nowrap"
                      >
                        Посмотреть
                        <FontAwesomeIcon icon={faArrowRight} className="text-[10px]" />
                      </Link>
                    </div>
                  </div>
                </div>
              </div>
            );
          })}
        </div>
    </div>
  );
};

export default EventsSection;
