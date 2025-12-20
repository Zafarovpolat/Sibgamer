import { Link } from 'react-router-dom';
import { formatServerDate } from '../utils/dateUtils';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowRight, faComment, faEye } from '@fortawesome/free-solid-svg-icons';
import { resolveMediaUrl } from '../lib/media';
import type { NewsListItem } from '../types';

interface NewsSectionProps {
  news: NewsListItem[];
}

const NewsSection = ({ news }: NewsSectionProps) => {
  const formatDate = (dateString: string) => {
    return formatServerDate(dateString, { 
      year: 'numeric', 
      month: 'long', 
      day: 'numeric' 
    });
  };

  if (!news || news.length === 0) {
    return null;
  }

  return (
    <div className="mb-16">
      <div className="flex items-center justify-between mb-6">
        <div className="mb-6">
        <h2 className="text-3xl font-bold text-white mb-2">
          Новости
        </h2>
        <p className="text-gray-400">
         Последние новости и события нашего сообщества
        </p>
      </div>
        
        <Link 
          to="/news" 
          className="flex items-center gap-2 px-4 py-2 rounded-lg border border-gray-800/50 text-gray-300 hover:text-white hover:border-gray-700/50 transition-all duration-200 text-sm"
        >
          Перейти
          <FontAwesomeIcon icon={faArrowRight} className="text-xs" />
        </Link>
      </div>
      
      <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
        {news.map((item) => (
          <div
            key={item.id}
            className="group overflow-hidden rounded-xl border border-gray-800/50 hover:border-gray-700/50 transition-all duration-200"
          >
            <Link to={`/news/${item.slug}`}>
              {item.coverImage ? (
                <div className="relative h-44 overflow-hidden">
                  <img
                    src={resolveMediaUrl(item.coverImage)}
                    alt={item.title}
                    className="w-full h-full object-cover"
                  />
                  <div className="absolute inset-0 bg-gradient-to-t from-gray-900 to-transparent" />
                </div>
              ) : (
                <div className="h-44 bg-gray-800/50 flex items-center justify-center">
                  <FontAwesomeIcon icon={faComment} className="text-5xl text-gray-700" />
                </div>
              )}
            </Link>

            <div className="p-4">
              <Link to={`/news/${item.slug}`}>
                <h3 className="text-lg font-semibold mb-2 line-clamp-2 text-white group-hover:text-highlight transition-colors duration-200">
                  {item.title}
                </h3>
              </Link>
              
              {item.summary && (
                <p className="text-gray-400 text-sm mb-4 truncate">
                  {item.summary}
                </p>
              )}

              <div className="flex items-center justify-between gap-2">
                <div className="flex items-center gap-1 text-xs text-gray-500 flex-1">
                  <FontAwesomeIcon icon={faEye} />
                  <span>{item.viewCount}</span>
                </div>

                <div className="text-xs text-gray-500 flex-1 text-center">
                  {formatDate(item.createdAt)}
                </div>

                <div className="flex-1 flex justify-end">
                  <Link 
                    to={`/news/${item.slug}`}
                    className="flex items-center gap-1 px-3 py-1.5 rounded-lg border border-gray-800/50 text-gray-300 hover:text-white hover:border-highlight hover:bg-highlight/10 transition-all duration-200 text-xs whitespace-nowrap"
                  >
                    Посмотреть
                    <FontAwesomeIcon icon={faArrowRight} className="text-[10px]" />
                  </Link>
                </div>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default NewsSection;
