import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEnvelope } from '@fortawesome/free-solid-svg-icons';
import { faVk, faTelegram, faDiscord } from '@fortawesome/free-brands-svg-icons';
import { useSiteSettings } from '../hooks/useSiteSettings';
import { getServerLocalTime } from '../utils/dateUtils';

const Footer = () => {
  const currentYear = getServerLocalTime().getFullYear();
  const { data: settings } = useSiteSettings();
  const siteName = settings?.site_name || 'SIBGamer';

  return (
    <footer className="bg-secondary border-t border-gray-800 mt-auto">
      <div className="container mx-auto px-4 py-12">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-8 mb-8">
          <div className="col-span-1 md:col-span-2">
            <Link to="/" className="flex items-center mb-4">
              <span className="text-2xl font-bold bg-gradient-to-r from-highlight to-blue-500 bg-clip-text text-transparent">
                {siteName}
              </span>
            </Link>
            <p className="text-gray-400 mb-4 max-w-md">
              Игровой проект серверов Counter-Strike Source.
            </p>
            <div className="flex gap-4">
              <a
                href="https://vk.com"
                target="_blank"
                rel="noopener noreferrer"
                className="w-10 h-10 bg-gray-800 hover:bg-highlight rounded-full flex items-center justify-center transition-all duration-300"
              >
                <FontAwesomeIcon icon={faVk} className="text-xl" />
              </a>
              <a
                href="https://t.me"
                target="_blank"
                rel="noopener noreferrer"
                className="w-10 h-10 bg-gray-800 hover:bg-highlight rounded-full flex items-center justify-center transition-all duration-300"
              >
                <FontAwesomeIcon icon={faTelegram} className="text-xl" />
              </a>
              <a
                href="https://discord.com"
                target="_blank"
                rel="noopener noreferrer"
                className="w-10 h-10 bg-gray-800 hover:bg-highlight rounded-full flex items-center justify-center transition-all duration-300"
              >
                <FontAwesomeIcon icon={faDiscord} className="text-xl" />
              </a>
            </div>
          </div>

          <div>
            <h3 className="text-lg font-bold mb-4 text-white">Навигация</h3>
            <ul className="space-y-2">
              <li>
                <Link to="/" className="text-gray-400 hover:text-highlight transition-colors">
                  Главная
                </Link>
              </li>
              <li>
                <Link to="/events" className="text-gray-400 hover:text-highlight transition-colors">
                  События
                </Link>
              </li>
              <li>
                <Link to="/news" className="text-gray-400 hover:text-highlight transition-colors">
                  Новости
                </Link>
              </li>
              <li>
                <Link to="/donate" className="text-gray-400 hover:text-highlight transition-colors">
                  Донат
                </Link>
              </li>
            </ul>
          </div>

          <div>
            <h3 className="text-lg font-bold mb-4 text-white">Контакты</h3>
            <ul className="space-y-2">
              <li className="text-gray-400 flex items-center gap-2">
                <FontAwesomeIcon icon={faEnvelope} className="text-sm" />
                support@sibgamer.ru
              </li>
            </ul>
          </div>
        </div>

        <div className="border-t border-gray-800 pt-8">
          <div className="flex flex-col md:flex-row justify-between items-center text-gray-500 text-sm">
            <p>© {currentYear} {siteName}. Все права защищены.</p>
          </div>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
