import { useState, useEffect, useRef } from 'react';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faHome, faSignOutAlt, faSignInAlt, faBars, faTimes, faUserPlus, faNewspaper, faCalendarAlt, faHeart, faBell, faUser, faCog, faBan, faChartBar, faChevronDown, faInfoCircle, faUsers } from '@fortawesome/free-solid-svg-icons';
import { useQuery } from '@tanstack/react-query';
import { useAuthStore } from '../store/authStore';
import { useSiteSettings } from '../hooks/useSiteSettings';
import { getUnreadCount } from '../lib/notificationApi';
import LoginModal from './LoginModal';
import RegisterModal from './RegisterModal';
import ForgotPasswordModal from './ForgotPasswordModal';
import Avatar from './Avatar';
import NotificationDropdown from './NotificationDropdown';

const Navbar = () => {
  const { user, isAuthenticated, logout } = useAuthStore();
  const { data: settings } = useSiteSettings();
  const siteName = settings?.site_name || 'SIBGamer';

  const [showUserMenu, setShowUserMenu] = useState(false);
  const [showInfoDropdown, setShowInfoDropdown] = useState(false);
  const [showParticipationDropdown, setShowParticipationDropdown] = useState(false);
  const infoRef = useRef<HTMLDivElement | null>(null);
  const participationRef = useRef<HTMLDivElement | null>(null);
  const [showLoginModal, setShowLoginModal] = useState(false);
  const [showRegisterModal, setShowRegisterModal] = useState(false);
  const [showForgotPasswordModal, setShowForgotPasswordModal] = useState(false);
  const [showMobileMenu, setShowMobileMenu] = useState(false);
  const [isMenuAnimating, setIsMenuAnimating] = useState(false);
  const userMenuRef = useRef<HTMLDivElement>(null);

  const { data: unreadCount } = useQuery({
    queryKey: ['notifications', 'unread-count'],
    queryFn: getUnreadCount,
    enabled: isAuthenticated,
    refetchInterval: 30000,
    staleTime: 2000, // Предотвращаем мгновенный запрос после входа (race condition с сохранением токена)
  });

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (userMenuRef.current && !userMenuRef.current.contains(event.target as Node)) {
        setShowUserMenu(false);
      }
    };

    if (showUserMenu) {
      document.addEventListener('mousedown', handleClickOutside);
    }

    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [showUserMenu]);

  useEffect(() => {
    const handleDocClick = (e: MouseEvent) => {
      if (showInfoDropdown && infoRef.current && !infoRef.current.contains(e.target as Node)) {
        setShowInfoDropdown(false);
      }
    };

    if (showInfoDropdown) {
      document.addEventListener('mousedown', handleDocClick);
    }

    return () => document.removeEventListener('mousedown', handleDocClick);
  }, [showInfoDropdown]);

  useEffect(() => {
    const handleDocClick = (e: MouseEvent) => {
      if (showParticipationDropdown && participationRef.current && !participationRef.current.contains(e.target as Node)) {
        setShowParticipationDropdown(false);
      }
    };

    if (showParticipationDropdown) {
      document.addEventListener('mousedown', handleDocClick);
    }

    return () => document.removeEventListener('mousedown', handleDocClick);
  }, [showParticipationDropdown]);

  const closeMobileMenu = () => {
    setIsMenuAnimating(true);
    setTimeout(() => {
      setShowMobileMenu(false);
      setIsMenuAnimating(false);
    }, 300);
  };

  const toggleMobileMenu = () => {
    if (showMobileMenu) {
      closeMobileMenu();
    } else {
      setShowMobileMenu(true);
    }
  };

  const [showMobileInfo, setShowMobileInfo] = useState(false);
  const [showMobileParticipation, setShowMobileParticipation] = useState(false);

  const handleLogout = () => {
    logout();
    setShowUserMenu(false);
    closeMobileMenu();
  };

  return (
    <>
      <nav className="bg-glass backdrop-blur-2xl border-b border-gray-800 sticky top-0 z-40">
        <div className="container mx-auto px-4">
          <div className="flex justify-between items-center h-16">
            <Link to="/" className="text-2xl font-bold bg-gradient-to-r from-highlight to-blue-500 bg-clip-text text-transparent">
              {siteName}
            </Link>
            <div className="hidden lg:flex items-center space-x-6 absolute left-1/2 transform -translate-x-1/2">
              <Link
                to="/"
                className="text-gray-300 hover:text-highlight transition-colors duration-300 flex items-center space-x-2 px-4 py-2 rounded-lg hover:bg-highlight/10"
              >
                <span>Главная</span>
              </Link>
              <Link
                to="/news"
                className="text-gray-300 hover:text-highlight transition-colors duration-300 flex items-center space-x-2 px-4 py-2 rounded-lg hover:bg-highlight/10"
              >
                <span>Новости</span>
              </Link>
              <Link
                to="/events"
                className="text-gray-300 hover:text-highlight transition-colors duration-300 flex items-center space-x-2 px-4 py-2 rounded-lg hover:bg-highlight/10"
              >
                <span>События</span>
              </Link>
              <Link
                to="/donate"
                className="text-gray-300 hover:text-highlight transition-colors duration-300 flex items-center space-x-2 px-4 py-2 rounded-lg hover:bg-highlight/10"
              >
                <span>Донат</span>
              </Link>
              <div className="relative" ref={participationRef}>
                <button
                  onClick={() => setShowParticipationDropdown(!showParticipationDropdown)}
                  className="flex items-center space-x-2 text-gray-300 hover:text-highlight transition-colors duration-300 px-4 py-2 rounded-lg hover:bg-highlight/10"
                >
                  <span>Участие</span>
                  <FontAwesomeIcon
                    icon={faChevronDown}
                    className={`transition-transform duration-200 ${showParticipationDropdown ? 'rotate-180' : ''}`}
                  />
                </button>
                {showParticipationDropdown && (
                  <div className="absolute left-0 top-full w-auto min-w-[11rem] whitespace-nowrap bg-secondary rounded-xl py-2 animate-slide-in z-50 overflow-hidden shadow-lg">
                    <Link
                      to="/member"
                      className="block px-4 py-2 text-gray-300 hover:text-highlight hover:bg-secondary/70 transition-colors duration-200"
                      onClick={() => setShowParticipationDropdown(false)}
                    >
                      Стать участником
                    </Link>
                  </div>
                )}
              </div>
              <div className="relative" ref={infoRef}>
                <button
                  onClick={() => setShowInfoDropdown(!showInfoDropdown)}
                  className="flex items-center space-x-2 text-gray-300 hover:text-highlight transition-colors duration-300 px-4 py-2 rounded-lg hover:bg-highlight/10"
                >
                  <span>Информация</span>
                  <FontAwesomeIcon
                    icon={faChevronDown}
                    className={`transition-transform duration-200 ${showInfoDropdown ? 'rotate-180' : ''}`}
                  />
                </button>
                {showInfoDropdown && (
                  <div className="absolute left-0 top-full w-auto min-w-[11rem] bg-secondary rounded-xl py-2 animate-slide-in z-50 overflow-hidden shadow-lg">
                    <a
                      href="https://sibgamer.myarena.site/bans"
                      target="_blank"
                      rel="noopener noreferrer"
                      className="block px-4 py-2 text-gray-300 hover:text-highlight hover:bg-secondary/70 transition-colors duration-200"
                    >
                      Банлист
                    </a>
                    <a
                      href="https://sibgamer.myarena.site/"
                      target="_blank"
                      rel="noopener noreferrer"
                      className="block px-4 py-2 text-gray-300 hover:text-highlight hover:bg-secondary/70 transition-colors duration-200"
                    >
                      Статистика
                    </a>
                  </div>
                )}
              </div>


            </div>

            <div className="hidden lg:flex items-center space-x-6">
              {isAuthenticated && user ? (
                <>
                  <NotificationDropdown />
                  <div className="relative" ref={userMenuRef}>
                    <button
                      onClick={() => setShowUserMenu(!showUserMenu)}
                      className="text-gray-300 hover:text-highlight transition-colors duration-300 flex items-center space-x-3 px-4 py-2 rounded-lg hover:bg-highlight/10"
                    >
                      <Avatar username={user.username} avatarUrl={user.avatarUrl} size="sm" />
                      <span>{user.username}</span>
                    </button>
                    {showUserMenu && (
                      <div className="absolute right-0 mt-2 w-48 bg-secondary rounded-xl py-2 animate-slide-in z-50 overflow-hidden shadow-lg">
                        <Link
                          to="/profile"
                          className="block px-4 py-2 text-gray-300 hover:text-highlight hover:bg-secondary/70 transition-colors duration-200 lg:px-3 lg:py-2 lg:my-0"
                          onClick={() => setShowUserMenu(false)}
                        >
                          Личный кабинет
                        </Link>
                        {user.isAdmin && (
                          <Link
                            to="/admin"
                            className="block px-4 py-2 text-gray-300 hover:text-highlight hover:bg-secondary/70 transition-colors duration-200 lg:px-3 lg:py-2 lg:my-0"
                            onClick={() => setShowUserMenu(false)}
                          >
                            Админ-панель
                          </Link>
                        )}
                        <button
                          onClick={handleLogout}
                          className="w-full text-left px-4 py-2 text-gray-300 hover:text-highlight hover:bg-secondary/70 transition-colors duration-200 flex items-center space-x-2 lg:px-3 lg:py-2 lg:my-0"
                        >
                          <FontAwesomeIcon icon={faSignOutAlt} />
                          <span>Выход</span>
                        </button>
                      </div>
                    )}
                  </div>
                </>
              ) : (
                <div className="flex items-center space-x-3">
                  <button
                    onClick={() => setShowLoginModal(true)}
                    className="btn-secondary flex items-center space-x-2"
                  >
                    <FontAwesomeIcon icon={faSignInAlt} />
                    <span>Вход</span>
                  </button>
                  <button
                    onClick={() => setShowRegisterModal(true)}
                    className="btn-primary flex items-center space-x-2"
                  >
                    <FontAwesomeIcon icon={faUserPlus} />
                    <span>Регистрация</span>
                  </button>
                </div>
              )}
            </div>

            <button
              onClick={toggleMobileMenu}
              className="lg:hidden text-2xl text-gray-300"
            >
              <FontAwesomeIcon icon={showMobileMenu ? faTimes : faBars} />
            </button>
          </div>
        </div>

        {(showMobileMenu || isMenuAnimating) && (
          <div className={`lg:hidden border-t border-gray-800 max-h-[calc(100vh-4rem)] overflow-y-auto ${showMobileMenu && !isMenuAnimating ? 'animate-slide-in' : 'animate-slide-out'}`}>
            <div className="px-4 py-4 space-y-3">
              {isAuthenticated && user && (
                <div className="flex items-center space-x-3 py-3 border-b border-gray-800 mb-4">
                  <Avatar username={user.username} avatarUrl={user.avatarUrl} size="md" />
                  <span className="text-gray-300 font-medium">{user.username}</span>
                </div>
              )}

              <Link
                to="/"
                className="block py-2 text-gray-300 hover:text-highlight transition-colors duration-300"
                onClick={closeMobileMenu}
              >
                <FontAwesomeIcon icon={faHome} className="mr-2" />
                Главная
              </Link>
              <Link
                to="/events"
                className="block py-2 text-gray-300 hover:text-highlight transition-colors duration-300"
                onClick={closeMobileMenu}
              >
                <FontAwesomeIcon icon={faCalendarAlt} className="mr-2" />
                События
              </Link>
              <Link
                to="/news"
                className="block py-2 text-gray-300 hover:text-highlight transition-colors duration-300"
                onClick={closeMobileMenu}
              >
                <FontAwesomeIcon icon={faNewspaper} className="mr-2" />
                Новости
              </Link>
              <Link
                to="/donate"
                className="block py-2 text-gray-300 hover:text-highlight transition-colors duration-300"
                onClick={closeMobileMenu}
              >
                <FontAwesomeIcon icon={faHeart} className="mr-2" />
                Донат
              </Link>
              <div>
                <button
                  onClick={() => setShowMobileParticipation(!showMobileParticipation)}
                  className="w-full flex items-center justify-between py-2 text-gray-300 hover:text-highlight transition-colors duration-300"
                >
                  <div className="flex items-center">
                    <FontAwesomeIcon icon={faUsers} className="mr-2" />
                    <span className="mr-2">Участие</span>
                  </div>
                  <FontAwesomeIcon
                    icon={faChevronDown}
                    className={`text-gray-400 transition-transform duration-200 ${showMobileParticipation ? 'rotate-180' : ''}`}
                  />
                </button>
                {showMobileParticipation && (
                  <div className="pl-6 pt-2 space-y-1">
                    <Link
                      to="/member"
                      className="block py-2 text-gray-300 hover:text-highlight transition-colors duration-300"
                      onClick={closeMobileMenu}
                    >
                      <FontAwesomeIcon icon={faUsers} className="mr-2" />
                      Стать участником
                    </Link>
                  </div>
                )}
              </div>
              <div>
                <button
                  onClick={() => setShowMobileInfo(!showMobileInfo)}
                  className="w-full flex items-center justify-between py-2 text-gray-300 hover:text-highlight transition-colors duration-300"
                >
                  <div className="flex items-center">
                    <FontAwesomeIcon icon={faInfoCircle} className="mr-2" />
                    <span className="mr-2">Информация</span>
                  </div>
                  <FontAwesomeIcon
                    icon={faChevronDown}
                    className={`text-gray-400 transition-transform duration-200 ${showMobileInfo ? 'rotate-180' : ''}`}
                  />
                </button>
                {showMobileInfo && (
                  <div className="pl-6 pt-2 space-y-1">
                    <a
                      href="https://sibgamer.myarena.site/bans"
                      target="_blank"
                      rel="noopener noreferrer"
                      className="block py-2 text-gray-300 hover:text-highlight transition-colors duration-300"
                      onClick={closeMobileMenu}
                    >
                      <FontAwesomeIcon icon={faBan} className="mr-2" />
                      Банлист
                    </a>
                    <a
                      href="https://sibgamer.myarena.site/"
                      target="_blank"
                      rel="noopener noreferrer"
                      className="block py-2 text-gray-300 hover:text-highlight transition-colors duration-300"
                      onClick={closeMobileMenu}
                    >
                      <FontAwesomeIcon icon={faChartBar} className="mr-2" />
                      Статистика
                    </a>
                  </div>
                )}
              </div>

              {isAuthenticated && user ? (
                <>
                  <Link
                    to="/notifications"
                    className="block py-2 text-gray-300 hover:text-highlight transition-colors duration-300 relative"
                    onClick={closeMobileMenu}
                  >
                    <div className="flex items-center">
                      <FontAwesomeIcon icon={faBell} className="mr-2" />
                      <span>Уведомления</span>
                      {unreadCount && unreadCount.count > 0 && (
                        <span className="ml-2 bg-red-500 text-white text-xs rounded-full h-5 w-5 flex items-center justify-center font-bold">
                          {unreadCount.count > 99 ? '99+' : unreadCount.count}
                        </span>
                      )}
                    </div>
                  </Link>
                  <Link
                    to="/profile"
                    className="block py-2 text-gray-300 hover:text-highlight transition-colors duration-300"
                    onClick={closeMobileMenu}
                  >
                    <FontAwesomeIcon icon={faUser} className="mr-2" />
                    Личный кабинет
                  </Link>
                  {user.isAdmin && (
                    <Link
                      to="/admin"
                      className="block py-2 text-gray-300 hover:text-highlight transition-colors duration-300"
                      onClick={closeMobileMenu}
                    >
                      <FontAwesomeIcon icon={faCog} className="mr-2" />
                      Админ-панель
                    </Link>
                  )}
                  <button
                    onClick={handleLogout}
                    className="w-full text-left py-2 text-gray-300 hover:text-highlight transition-colors duration-300"
                  >
                    <FontAwesomeIcon icon={faSignOutAlt} className="mr-2" />
                    Выход
                  </button>
                </>
              ) : (
                <div className="space-y-2 pt-2">
                  <button
                    onClick={() => {
                      closeMobileMenu();
                      setShowLoginModal(true);
                    }}
                    className="btn-secondary w-full flex items-center justify-center space-x-2"
                  >
                    <FontAwesomeIcon icon={faSignInAlt} />
                    <span>Вход</span>
                  </button>
                  <button
                    onClick={() => {
                      closeMobileMenu();
                      setShowRegisterModal(true);
                    }}
                    className="btn-primary w-full flex items-center justify-center space-x-2"
                  >
                    <FontAwesomeIcon icon={faUserPlus} />
                    <span>Регистрация</span>
                  </button>
                </div>
              )}
            </div>
          </div>
        )}
      </nav>

      <LoginModal
        isOpen={showLoginModal}
        onClose={() => setShowLoginModal(false)}
        onSwitchToRegister={() => {
          setShowLoginModal(false);
          setShowRegisterModal(true);
        }}
        onSwitchToForgotPassword={() => {
          setShowLoginModal(false);
          setShowForgotPasswordModal(true);
        }}
      />
      <RegisterModal
        isOpen={showRegisterModal}
        onClose={() => setShowRegisterModal(false)}
        onSwitchToLogin={() => {
          setShowRegisterModal(false);
          setShowLoginModal(true);
        }}
      />
      <ForgotPasswordModal
        isOpen={showForgotPasswordModal}
        onClose={() => setShowForgotPasswordModal(false)}
      />
    </>
  );
};

export default Navbar;
