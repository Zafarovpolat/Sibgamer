import { useQuery } from '@tanstack/react-query';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowRight, faComment, faEye, faImage, faTrophy, faCircle } from '@fortawesome/free-solid-svg-icons';
import ImageSlider from '../components/ImageSlider';
import ServerCard from '../components/ServerCard';
import CountdownTimer from '../components/CountdownTimer';
import api from '../lib/axios';
import { API_URL } from '../config/api';
import { resolveMediaUrl } from '../lib/media';
import { usePageTitle } from '../hooks/usePageTitle';
import type { Server, SliderImage, NewsListItem, EventListItem, TopDonator } from '../types';
import { getServerLocalTime, formatServerDateOnly, parseServerDate } from '../utils/dateUtils';

interface SliderImageApi extends Omit<SliderImage, 'buttons'> {
  buttons?: string;
}

const Home = () => {
  usePageTitle('Главная');
  
  const { data: servers, isLoading: serversLoading } = useQuery({
    queryKey: ['servers'],
    queryFn: async () => {
      const response = await api.get<Server[]>('/servers');
      return response.data;
    },
    refetchInterval: 30000,
    refetchIntervalInBackground: true,
  });

  const { data: sliderImages } = useQuery({
    queryKey: ['sliderImages'],
    queryFn: async () => {
      const response = await api.get<SliderImageApi[]>('/slider');
      return response.data.map(image => {
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
      }) as SliderImage[];
    },
    staleTime: 0,
  });

  const { data: latestNews } = useQuery<NewsListItem[]>({
    queryKey: ['latestNews'],
    queryFn: async () => {
      const response = await fetch(`${API_URL}/news/latest/5`);
      if (!response.ok) throw new Error('Failed to fetch latest news');
      return response.json();
    },
  });

  const { data: topDonators } = useQuery<TopDonator[]>({
    queryKey: ['topDonators'],
    queryFn: async () => {
      const response = await api.get('/donation/top-donators?limit=3');
      return response.data;
    },
  });

  const { data: upcomingEvents } = useQuery<EventListItem[]>({
    queryKey: ['upcoming-events'],
    queryFn: async () => {
      const res = await fetch(`${API_URL}/events/upcoming?count=2`);
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

  return (
    <div className="min-h-screen flex flex-col">
      <div className="flex-1">
        <div className="container mx-auto px-4 py-8">
          <div className="mb-8">
            <ImageSlider images={sliderImages || []} />
          </div>

          <div className="grid grid-cols-1 lg:grid-cols-12 gap-6">
            <div className="lg:col-span-8">
              <div className="mb-6">
                <div className="flex items-center justify-between mb-4">
                  <h2 className="text-2xl font-bold text-white">
                    Свежие новости
                  </h2>
                  <Link 
                    to="/news" 
                    className="flex items-center gap-2 px-3 py-1.5 rounded-lg border border-gray-800/50 text-gray-300 hover:text-white hover:border-gray-700/50 transition-all duration-200 text-sm"
                  >
                    Все новости
                    <FontAwesomeIcon icon={faArrowRight} className="text-xs" />
                  </Link>
                </div>
              </div>

              <div className="space-y-4">
                {latestNews && latestNews.length > 0 ? (
                  latestNews.map((item) => (
                    <div
                      key={item.id}
                      className="group overflow-hidden rounded-xl border border-gray-800/50 hover:border-gray-700/50 transition-all duration-200 bg-gray-900/40 backdrop-blur-md sm:h-52"
                    >
                      <div className="flex flex-col sm:flex-row sm:h-full">
                        <Link to={`/news/${item.slug}`} className="sm:w-64 flex-shrink-0 h-40 sm:h-full">
                          {item.coverImage ? (
                            <div className="relative h-full overflow-hidden">
                              <img
                                src={resolveMediaUrl(item.coverImage)}
                                alt={item.title}
                                className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
                              />
                              <div className="absolute inset-0 bg-gradient-to-r from-transparent to-gray-900/50" />
                            </div>
                          ) : (
                            <div className="h-full bg-gray-800/50 flex items-center justify-center">
                              <FontAwesomeIcon icon={faComment} className="text-4xl text-gray-700" />
                            </div>
                          )}
                        </Link>

                        <div className="flex-1 p-4 flex flex-col justify-between sm:h-full">
                          <div>
                            <Link to={`/news/${item.slug}`}>
                              <h3 className="text-lg font-semibold mb-2 line-clamp-2 text-white group-hover:text-highlight transition-colors duration-200">
                                {item.title}
                              </h3>
                            </Link>
                            
                            {item.summary && (
                              <p className="text-gray-400 text-sm mb-3 line-clamp-2">
                                {item.summary}
                              </p>
                            )}
                          </div>

                          <div className="flex items-center justify-between text-xs text-gray-500">
                            <div className="flex items-center gap-3">
                              <div className="flex items-center gap-1">
                                <FontAwesomeIcon icon={faEye} />
                                <span>{item.viewCount}</span>
                              </div>
                              <span>{formatServerDateOnly(item.createdAt)}</span>
                            </div>
                            <Link 
                              to={`/news/${item.slug}`}
                              className="flex items-center gap-1 px-3 py-1.5 rounded-lg border border-gray-800/50 text-gray-300 hover:text-white hover:border-highlight hover:bg-highlight/10 transition-all duration-200"
                            >
                              Читать
                              <FontAwesomeIcon icon={faArrowRight} className="text-[10px]" />
                            </Link>
                          </div>
                        </div>
                      </div>
                    </div>
                  ))
                ) : (
                  <div className="text-center py-12 rounded-xl bg-gray-900/40 backdrop-blur-md border border-gray-800/50">
                    <FontAwesomeIcon icon={faComment} className="text-5xl text-gray-700 mb-3" />
                    <p className="text-gray-400">Новостей пока нет</p>
                  </div>
                )}
              </div>
            </div>

            <div className="lg:col-span-4 space-y-6">
              <div className="glass-effect rounded-xl p-4 border border-gray-800/50">
                <div className="flex items-center justify-between mb-4">
                  <h3 className="text-lg font-bold text-white flex items-center gap-2">
                    <span>💎</span>
                    Топ донатеры
                  </h3>
                  <Link 
                    to="/donate" 
                    className="text-xs text-gray-400 hover:text-white transition-colors"
                  >
                    Помочь →
                  </Link>
                </div>

                <div className="space-y-2">
                  {topDonators && topDonators.length > 0 ? (
                    topDonators.map((donator: TopDonator, index: number) => (
                      <div
                        key={donator.userId}
                        className="flex items-center gap-3 p-3 rounded-lg bg-gray-800/30 hover:bg-gray-800/50 transition-all duration-200 border border-gray-700/30"
                      >
                        <div className={`flex-shrink-0 w-8 h-8 rounded-full flex items-center justify-center font-bold text-sm ${
                          index === 0 ? 'bg-yellow-500/20 text-yellow-400' :
                          index === 1 ? 'bg-gray-400/20 text-gray-300' :
                          index === 2 ? 'bg-orange-500/20 text-orange-400' :
                          'bg-gray-700/50 text-gray-400'
                        }`}>
                          {index + 1}
                        </div>

                        <div className="flex-shrink-0">
                          {donator.avatarUrl ? (
                            <img
                              src={resolveMediaUrl(donator.avatarUrl)}
                              alt={donator.username}
                              className="w-10 h-10 rounded-full border-2 border-gray-700/50"
                            />
                          ) : (
                            <div className="w-10 h-10 rounded-full bg-gray-700/50 flex items-center justify-center text-gray-400 font-semibold border-2 border-gray-700/50">
                              {donator.username?.charAt(0).toUpperCase() || '?'}
                            </div>
                          )}
                        </div>

                        <div className="flex-1 min-w-0">
                          <div className="font-semibold text-white text-sm truncate">
                            {donator.username}
                          </div>
                          <div className="text-xs text-gray-400">
                            {donator.donationCount} {donator.donationCount === 1 ? 'донат' : 'донатов'}
                          </div>
                        </div>

                        <div className="flex-shrink-0 text-right">
                          <div className="font-bold text-highlight">
                            {donator.totalAmount.toFixed(0)} ₽
                          </div>
                        </div>
                      </div>
                    ))
                  ) : (
                    <div className="text-center py-6 text-gray-500 text-sm">
                      <p>Пока нет донатов</p>
                      <p className="text-xs mt-1 flex items-center justify-center gap-1">
                        Станьте первым! <FontAwesomeIcon icon={faTrophy} className="text-yellow-400" />
                      </p>
                    </div>
                  )}
                </div>
              </div>

              <div className="glass-effect rounded-xl p-4 border border-gray-800/50">
                <div className="flex items-center justify-between mb-4">
                  <h3 className="text-lg font-bold text-white">
                    События
                  </h3>
                  <Link 
                    to="/events" 
                    className="text-xs text-gray-400 hover:text-white transition-colors"
                  >
                    Все →
                  </Link>
                </div>

                <div className="grid gap-4 grid-cols-1">
                  {upcomingEvents && upcomingEvents.length > 0 ? (
                    upcomingEvents.map((event) => {
                      const status = getEventStatus(event.startDate, event.endDate);
                      return (
                        <Link
                          key={event.id}
                          to={`/events/${event.slug}`}
                          className="block group"
                        >
                          <div className="rounded-lg overflow-hidden border border-gray-800/50 bg-gray-800/30">
                            {event.coverImage ? (
                              <div className="relative h-56 lg:h-64 overflow-hidden">
                                <img
                                  src={resolveMediaUrl(event.coverImage)}
                                  alt={event.title}
                                  className="w-full h-full object-cover"
                                />
                                <div className="absolute inset-0 bg-gradient-to-t from-gray-900 to-transparent" />
                                <div className="absolute top-2 right-2">
                                  {status.type === 'upcoming' || status.type === 'ongoing' ? (
                                    <CountdownTimer
                                      targetDate={status.targetDate}
                                      className="text-[10px]"
                                      showLabels={false}
                                      compact={true}
                                      statusLabel={status.type === 'upcoming' ? 'До начала' : 'До завершения'}
                                      statusTextClass={status.color}
                                      statusBgClass={status.bgColor}
                                    />
                                    ) : (
                                    <div className={`flex items-center space-x-2 px-3 py-1 rounded-full ${status.bgColor} backdrop-blur-md border border-gray-700/40`.trim()}>
                                      <FontAwesomeIcon icon={faCircle} className={`${status.color} text-xs`} />
                                      <span className={`text-sm font-medium ${status.color}`.trim()}>{status.text}</span>
                                    </div>
                                  )}
                                </div>
                              </div>
                            ) : (
                              <div className="h-56 lg:h-64 bg-gray-800/50 flex items-center justify-center relative">
                                <FontAwesomeIcon icon={faImage} className="text-3xl text-gray-700" />
                                <div className="absolute top-2 right-2">
                                  {status.type === 'upcoming' || status.type === 'ongoing' ? (
                                    <CountdownTimer
                                      targetDate={status.targetDate}
                                      className="text-[10px]"
                                      showLabels={false}
                                      compact={true}
                                      statusLabel={status.type === 'upcoming' ? 'До начала' : 'До завершения'}
                                      statusTextClass={status.color}
                                      statusBgClass={status.bgColor}
                                    />
                                  ) : (
                                    <div className={`flex items-center space-x-2 px-3 py-1 rounded-full ${status.bgColor} backdrop-blur-md border border-gray-700/40`.trim()}>
                                      <FontAwesomeIcon icon={faCircle} className={`${status.color} text-xs`} />
                                      <span className={`text-sm font-medium ${status.color}`.trim()}>{status.text}</span>
                                    </div>
                                  )}
                                </div>
                              </div>
                            )}
                            <div className="p-3">
                              <h4 className="text-sm font-semibold text-white line-clamp-2">
                                {event.title}
                              </h4>
                              {event.summary && (
                                <p className="text-xs text-gray-400 mt-1 line-clamp-1">
                                  {event.summary}
                                </p>
                              )}
                            </div>
                          </div>
                        </Link>
                      );
                    })
                  ) : (
                    <div className="text-center py-6 text-gray-500 text-sm">
                      <FontAwesomeIcon icon={faImage} className="text-3xl mb-2" />
                      <p>Событий пока нет</p>
                    </div>
                  )}
                </div>
              </div>

              <div className="glass-effect rounded-xl p-4 border border-gray-800/50">
                <div className="flex items-center justify-between mb-4">
                  <h3 className="text-lg font-bold text-white">
                    Серверы
                  </h3>
                </div>

                <div className="space-y-3">
                  {serversLoading ? (
                    <div className="text-center py-6">
                      <div className="inline-block animate-spin rounded-full h-8 w-8 border-t-2 border-b-2 border-highlight mb-2"></div>
                      <p className="text-xs text-gray-400">Загрузка...</p>
                    </div>
                  ) : servers && servers.length > 0 ? (
                    servers.map((server) => (
                      <div key={server.id}>
                        <ServerCard server={server} />
                      </div>
                    ))
                  ) : (
                    <div className="text-center py-6 text-gray-500 text-sm">
                      <p>Нет серверов</p>
                    </div>
                  )}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Home;
