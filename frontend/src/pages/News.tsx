import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faComment, faEye, faArrowRight } from '@fortawesome/free-solid-svg-icons';
import { API_URL } from '../config/api';
import { resolveMediaUrl } from '../lib/media';
import { usePageTitle } from '../hooks/usePageTitle';
import { formatServerDateOnly } from '../utils/dateUtils';
import type { NewsListItem } from '../types';

const News = () => {
  usePageTitle('Новости');
  
  const [page, setPage] = useState(1);

  const { data: settings } = useQuery<Record<string, string>>({
    queryKey: ['settings'],
    queryFn: async () => {
      const res = await fetch(`${API_URL}/settings`);
      if (!res.ok) throw new Error('Failed to fetch settings');
      return res.json();
    }
  });

  const pageSize = settings?.news_per_page ? parseInt(settings.news_per_page) : 9;

  const { data, isLoading } = useQuery<{ items: NewsListItem[]; totalCount: number; totalPages: number }>({
    queryKey: ['news', page, pageSize],
    queryFn: async () => {
      const res = await fetch(`${API_URL}/news?page=${page}&pageSize=${pageSize}`);
      if (!res.ok) throw new Error('Failed to fetch news');
      return res.json();
    },
    enabled: !!settings 
  });

  if (isLoading) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="text-center py-10">Загрузка новостей...</div>
      </div>
    );
  }

  if (!data || !data.items) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="glass-card p-8 text-center">
          <p className="text-gray-400 text-lg">Не удалось загрузить новости</p>
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-white mb-2">Все новости</h1>
        <p className="text-gray-400">Последние обновления и события нашего сообщества</p>
      </div>

      {data.items.length === 0 ? (
        <div className="rounded-xl border border-gray-800/50 p-8 text-center max-w-2xl mx-auto">
          <p className="text-gray-400 text-lg">Новостей пока нет</p>
        </div>
      ) : (
        <>
          <div className="max-w-5xl mx-auto space-y-4 mb-8">
            {data?.items.map((news) => (
              <div
                key={news.id}
                className="group overflow-hidden rounded-xl border border-gray-800/50 hover:border-gray-700/50 transition-all duration-200 flex flex-col md:flex-row"
              >
                <Link to={`/news/${news.slug}`} className="flex-shrink-0">
                  {news.coverImage ? (
                    <div className="relative w-full md:w-80 h-56 overflow-hidden">
                      <img
                        src={resolveMediaUrl(news.coverImage)}
                        alt={news.title}
                        className="w-full h-full object-cover"
                      />
                      <div className="absolute inset-0 bg-gradient-to-t from-gray-900/50 to-transparent" />
                    </div>
                  ) : (
                    <div className="w-full md:w-80 h-56 bg-gray-800/50 flex items-center justify-center">
                      <FontAwesomeIcon icon={faComment} className="text-6xl text-gray-700" />
                    </div>
                  )}
                </Link>

                <div className="flex-1 p-6 flex flex-col justify-between">
                  <div>
                    <Link to={`/news/${news.slug}`}>
                      <h3 className="text-2xl font-bold mb-3 text-white group-hover:text-highlight transition-colors duration-200 line-clamp-2">
                        {news.title}
                      </h3>
                    </Link>

                    {news.summary && (
                      <p className="text-gray-400 text-base mb-4 line-clamp-3">
                        {news.summary}
                      </p>
                    )}
                  </div>

                  <div className="flex items-center justify-between gap-4 pt-4 border-t border-gray-800/30">
                    <div className="flex items-center gap-4 text-sm text-gray-500">
                      <div className="flex items-center gap-1">
                        <FontAwesomeIcon icon={faEye} />
                        <span>{news.viewCount}</span>
                      </div>
                      <span>•</span>
                      <div>
                        {formatServerDateOnly(news.createdAt)}
                      </div>
                    </div>

                    <Link 
                      to={`/news/${news.slug}`}
                      className="flex items-center gap-2 px-4 py-2 rounded-lg border border-gray-800/50 text-gray-300 hover:text-white hover:border-highlight hover:bg-highlight/10 transition-all duration-200 text-sm whitespace-nowrap"
                    >
                      Читать далее
                      <FontAwesomeIcon icon={faArrowRight} className="text-xs" />
                    </Link>
                  </div>
                </div>
              </div>
            ))}
          </div>

          {data && data.totalPages > 1 && (
            <div className="flex justify-center items-center gap-3">
              <button
                onClick={() => setPage(p => Math.max(1, p - 1))}
                disabled={page === 1}
                className="px-4 py-2 rounded-lg border border-gray-800/50 text-gray-300 hover:text-white hover:border-gray-700/50 disabled:opacity-50 disabled:cursor-not-allowed transition-colors duration-200"
              >
                ← Назад
              </button>
              
              <div className="flex gap-2">
                {Array.from({ length: Math.min(data.totalPages, 7) }, (_, i) => {
                  let pageNum;
                  if (data.totalPages <= 7) {
                    pageNum = i + 1;
                  } else if (page <= 4) {
                    pageNum = i + 1;
                  } else if (page >= data.totalPages - 3) {
                    pageNum = data.totalPages - 6 + i;
                  } else {
                    pageNum = page - 3 + i;
                  }
                  
                  return (
                    <button
                      key={pageNum}
                      onClick={() => setPage(pageNum)}
                      className={`px-4 py-2 rounded-lg font-semibold transition-colors duration-200 ${
                        page === pageNum
                          ? 'bg-highlight text-white'
                          : 'border border-gray-800/50 text-gray-400 hover:text-white hover:border-gray-700/50'
                      }`}
                    >
                      {pageNum}
                    </button>
                  );
                })}
              </div>

              <button
                onClick={() => setPage(p => Math.min(data.totalPages, p + 1))}
                disabled={page === data.totalPages}
                className="px-4 py-2 rounded-lg border border-gray-800/50 text-gray-300 hover:text-white hover:border-gray-700/50 disabled:opacity-50 disabled:cursor-not-allowed transition-colors duration-200"
              >
                Далее →
              </button>
            </div>
          )}
        </>
      )}
    </div>
  );
};

export default News;
