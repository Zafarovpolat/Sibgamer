import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faServer } from '@fortawesome/free-solid-svg-icons';
import ServerCard from './ServerCard';
import type { Server } from '../types';

interface ServersSectionProps {
  servers?: Server[];
  isLoading?: boolean;
}

const ServersSection = ({ servers, isLoading }: ServersSectionProps) => {
  return (
    <div>
      <div className="mb-6">
        <h2 className="text-3xl font-bold text-white mb-2">
          Наши серверы
        </h2>
        <p className="text-gray-400">
          Профессионально настроенные серверы с лучшими модами и плагинами
        </p>
      </div>

      {isLoading ? (
        <div className="text-center py-16">
          <div className="inline-block animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-highlight mb-3"></div>
          <p className="text-gray-400">Загрузка серверов...</p>
        </div>
      ) : servers && servers.length > 0 ? (
        <div className="flex flex-wrap justify-center gap-6">
          {servers.map((server) => (
            <div 
              key={server.id} 
              className="w-full sm:w-[calc(50%-0.75rem)] lg:w-[calc(33.333%-1rem)] xl:w-[calc(25%-1.125rem)] 2xl:w-[calc(20%-1.2rem)]"
            >
              <ServerCard server={server} />
            </div>
          ))}
        </div>
      ) : (
        <div className="text-center py-12 rounded-xl bg-gray-900/40 backdrop-blur-md border border-gray-800/50">
          <FontAwesomeIcon icon={faServer} className="text-5xl text-gray-700 mb-3" />
          <p className="text-gray-400">Нет доступных серверов</p>
        </div>
      )}
    </div>
  );
};

export default ServersSection;
