import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUsers, faCircle, faMap, faGamepad, faCopy } from '@fortawesome/free-solid-svg-icons';
import { useState } from 'react';
import type { Server } from '../types';

interface ServerCardProps {
  server: Server;
}

const ServerCard = ({ server }: ServerCardProps) => {
  const [copied, setCopied] = useState(false);

  const getMapImageUrl = (mapName: string) => {
    return `/maps/${mapName}.jpg`;
  };

  const handleConnect = () => {
    const steamConnectUrl = `steam://connect/${server.ipAddress}:${server.port}`;
    window.location.href = steamConnectUrl;
  };

  const handleCopyIp = (e: React.MouseEvent) => {
    e.stopPropagation();
    const text = `${server.ipAddress}:${server.port}`;
    navigator.clipboard?.writeText(text).then(() => {
      setCopied(true);
      setTimeout(() => setCopied(false), 1500);
    }).catch(() => {
      // ignore
    });
  };

  return (
    <div className="server-card bg-secondary/30 border border-gray-800/60 rounded-2xl overflow-hidden shadow-md">
      <div className="relative h-48 overflow-hidden">
        <img
          src={getMapImageUrl(server.mapName)}
          alt={server.mapName}
          className="w-full h-full object-cover"
          onError={(e) => {
            const target = e.target as HTMLImageElement;
            target.src = '/maps/default.svg';
          }}
        />
        <div className="absolute inset-0 bg-gradient-to-t from-black/90 via-black/50 to-transparent" />
        
        <div className="absolute top-3 right-3 flex items-center space-x-2 px-3 py-1 rounded-full bg-black/40 backdrop-blur-md border border-gray-700/40">
          <FontAwesomeIcon
            icon={faCircle}
            className={server.isOnline ? 'text-green-400 animate-pulse' : 'text-red-400'}
            size="xs"
          />
          <span className={`text-sm font-medium ${server.isOnline ? 'text-green-400' : 'text-red-400'}`}>
            {server.isOnline ? 'Online' : 'Offline'}
          </span>
        </div>

        <div className="absolute bottom-3 left-3 flex items-center space-x-2 bg-black/30 px-3 py-1 rounded-lg backdrop-blur-sm border border-gray-700/40">
          <FontAwesomeIcon icon={faMap} className="text-highlight" />
          <span className="text-white font-medium">{server.mapName}</span>
        </div>
      </div>

      <div className="p-4 flex-1 flex flex-col justify-between">
        <div>
          <h3 className="text-xl font-extrabold mb-3 truncate text-white">
            {server.name}
          </h3>

          <div className="space-y-3 mb-4">
            <div className="flex items-center justify-between text-sm">
              <div className="flex items-center space-x-2">
                <FontAwesomeIcon icon={faUsers} className="text-highlight" />
                <span className="text-gray-300">Игроков:</span>
              </div>
              <span className="text-white font-bold">
                {server.currentPlayers}/{server.maxPlayers}
              </span>
            </div>
            <div className="bg-secondary/50 rounded-full h-2 overflow-hidden">
              <div
                className="bg-gradient-to-r from-highlight to-blue-500 h-2 rounded-full transition-all duration-500"
                style={{
                  width: `${(server.currentPlayers / server.maxPlayers) * 100}%`,
                }}
              />
            </div>
          </div>

          <div className="flex items-center justify-between mb-3">
            <div className="text-xs text-gray-400 font-mono break-all">{server.ipAddress}:{server.port}</div>
            <button
              onClick={handleCopyIp}
              className="text-gray-300 hover:text-white text-sm flex items-center space-x-2 px-3 py-1 rounded-lg bg-black/20 border border-gray-700/30"
              title="Копировать IP"
            >
              <FontAwesomeIcon icon={faCopy} />
              <span>{copied ? 'Скопировано' : 'Копировать'}</span>
            </button>
          </div>
        </div>

        <div className="flex items-center space-x-3">
          {server.isOnline ? (
            <button
              onClick={handleConnect}
              className="btn-primary flex-1 flex items-center justify-center space-x-3 text-sm py-2"
            >
              <FontAwesomeIcon icon={faGamepad} />
              <span>Подключиться</span>
            </button>
          ) : (
            <div className="flex-1 text-center text-xs text-gray-400 py-2">Сервер офлайн</div>
          )}
        </div>
      </div>
    </div>
  );
};

export default ServerCard;
