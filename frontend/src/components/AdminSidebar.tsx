import { Link, useLocation } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faHome, faServer, faUsers, faImage, faTimes, faNewspaper, faCalendarAlt, faEnvelope, faDollarSign, faTags, faChartLine, faBell, faRobot, faFileAlt, faArrowLeft, faClipboardList } from '@fortawesome/free-solid-svg-icons';

interface AdminSidebarProps {
  isOpen: boolean;
  onClose: () => void;
}

const AdminSidebar = ({ isOpen, onClose }: AdminSidebarProps) => {
  const location = useLocation();

  const menuItems = [
    { path: '/admin', icon: faHome, label: 'Главная' },
    { path: '/admin/servers', icon: faServer, label: 'Серверы' },
    { path: '/admin/users', icon: faUsers, label: 'Пользователи' },
    { path: '/admin/slider', icon: faImage, label: 'Слайдер' },
    { path: '/admin/events', icon: faCalendarAlt, label: 'События' },
    { path: '/admin/news', icon: faNewspaper, label: 'Новости' },
    { path: '/admin/custompages', icon: faFileAlt, label: 'Страницы' },
    { path: '/admin/notifications', icon: faBell, label: 'Уведомления' },
    { path: '/admin/tg-notifications', icon: faRobot, label: 'Telegram' },
    { path: '/admin/email', icon: faEnvelope, label: 'Почта' },
    { path: '/admin/donation-settings', icon: faDollarSign, label: 'Донат' },
    { path: '/admin/donation-tariffs', icon: faTags, label: 'Тарифы' },
    { path: '/admin/vip-applications', icon: faClipboardList, label: 'Заявки участника' },
    { path: '/admin/donation-monitoring', icon: faChartLine, label: 'Мониторинг' },
  ];

  return (
    <>
      <div className="hidden md:block w-64 glass-card sticky top-0 h-screen p-6 border-r border-gray-800 overflow-y-auto sidebar-scroll">
        <h2 className="text-2xl font-bold mb-8 bg-gradient-to-r from-highlight to-blue-500 bg-clip-text text-transparent">
          Админ-панель
        </h2>
        <nav className="space-y-2">
          {menuItems.map((item) => (
            <Link
              key={item.path}
              to={item.path}
              className={`flex items-center space-x-3 px-4 py-3 rounded-lg ${
                location.pathname === item.path
                  ? 'bg-highlight text-white shadow-lg shadow-highlight/30'
                  : 'text-gray-300'
              }`}
            >
              <FontAwesomeIcon icon={item.icon} />
              <span>{item.label}</span>
            </Link>
          ))}
        </nav>
        <div className="mt-6 border-t border-gray-800/40 pt-6">
          <Link
            to="/"
            className="flex items-center gap-2 px-4 py-2 rounded-full bg-black/20 border border-gray-700/30 text-gray-200 font-semibold hover:bg-black/25 transition-colors duration-150"
          >
            <FontAwesomeIcon icon={faArrowLeft} />
            <span>На сайт</span>
          </Link>
        </div>
      </div>

      <div
        className={`fixed top-0 left-0 h-full w-64 glass-card z-50 transform transition-transform duration-300 md:hidden ${
          isOpen ? 'translate-x-0' : '-translate-x-full'
        } overflow-y-auto sidebar-scroll`}
      >
        <div className="p-6">
          <div className="flex items-center justify-between mb-8">
            <h2 className="text-xl font-bold bg-gradient-to-r from-highlight to-blue-500 bg-clip-text text-transparent">
              Админ-панель
            </h2>
            <button
              onClick={onClose}
              className="text-gray-400 hover:text-white transition-colors"
            >
              <FontAwesomeIcon icon={faTimes} className="text-xl" />
            </button>
          </div>

          <nav className="space-y-2">
            {menuItems.map((item) => (
              <Link
                key={item.path}
                to={item.path}
                onClick={onClose}
                className={`flex items-center space-x-3 px-4 py-3 rounded-lg ${
                  location.pathname === item.path
                    ? 'bg-highlight text-white shadow-lg shadow-highlight/30'
                    : 'text-gray-300'
                }`}
              >
                <FontAwesomeIcon icon={item.icon} />
                <span>{item.label}</span>
              </Link>
            ))}
          </nav>
          <div className="mt-6 border-t border-gray-800/40 pt-6">
            <Link
              to="/"
              onClick={onClose}
              className="flex items-center gap-2 px-4 py-2 rounded-full bg-black/20 border border-gray-700/30 text-gray-200 font-semibold hover:bg-black/25 transition-colors duration-150"
            >
              <FontAwesomeIcon icon={faArrowLeft} />
              <span>На сайт</span>
            </Link>
          </div>
        </div>
      </div>
    </>
  );
};

export default AdminSidebar;
