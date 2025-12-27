import { useState, useEffect, useRef } from 'react';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
  faHome, faSignOutAlt, faSignInAlt, faBars, faTimes, faUserPlus,
  faNewspaper, faCalendarAlt, faHeart, faBell, faUser, faCog,
  faBan, faChartBar, faChevronDown, faInfoCircle, faUsers, faLink,
  faFileAlt, faQuestion, faExternalLinkAlt
} from '@fortawesome/free-solid-svg-icons';
import { useQuery } from '@tanstack/react-query';
import { useAuthStore } from '../store/authStore';
import { useSiteSettings } from '../hooks/useSiteSettings';
import { getUnreadCount } from '../lib/notificationApi';
import { getNavSections } from '../lib/navApi';
import LoginModal from './LoginModal';
import RegisterModal from './RegisterModal';
import ForgotPasswordModal from './ForgotPasswordModal';
import Avatar from './Avatar';
import NotificationDropdown from './NotificationDropdown';
import type { NavSection } from '../types/nav';

// Маппинг иконок
const ICON_MAP: Record<string, typeof faHome> = {
  faHome, faNewspaper, faCalendarAlt, faHeart, faInfoCircle,
  faUsers, faBan, faChartBar, faCog, faLink, faFileAlt, faQuestion
};

const getIcon = (iconName?: string) => {
  if (!iconName) return null;
  return ICON_MAP[iconName] || faLink;
};

const Navbar = () => {
  const { user, isAuthenticated, logout } = useAuthStore();
  const { data: settings } = useSiteSettings();
  const siteName = settings?.site_name || 'SIBGamer';

  const [showUserMenu, setShowUserMenu] = useState(false);
  const [activeDropdown, setActiveDropdown] = useState<number | null>(null);
  const [showLoginModal, setShowLoginModal] = useState(false);
  const [showRegisterModal, setShowRegisterModal] = useState(false);
  const [showForgotPasswordModal, setShowForgotPasswordModal] = useState(false);
  const [showMobileMenu, setShowMobileMenu] = useState(false);
  const [isMenuAnimating, setIsMenuAnimating] = useState(false);
  const [mobileExpandedSections, setMobileExpandedSections] = useState<number[]>([]);

  const userMenuRef = useRef<HTMLDivElement>(null);
  const dropdownRefs = useRef<Map<number, HTMLDivElement>>(new Map());

  // Fetch navigation sections
  const { data: navSections = [] } = useQuery<NavSection[]>({
    queryKey: ['nav-sections'],
    queryFn: getNavSections,
    staleTime: 5 * 60 * 1000, // 5 минут кэш
  });

  const { data: unreadCount } = useQuery({
    queryKey: ['notifications', 'unread-count'],
    queryFn: getUnreadCount,
    enabled: isAuthenticated,
    refetchInterval: 30000,
    staleTime: 2000,
  });

  // Close dropdowns on outside click
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (userMenuRef.current && !userMenuRef.current.contains(event.target as Node)) {
        setShowUserMenu(false);
      }

      if (activeDropdown !== null) {
        const ref = dropdownRefs.current.get(activeDropdown);
        if (ref && !ref.contains(event.target as Node)) {
          setActiveDropdown(null);
        }
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, [activeDropdown]);

  const closeMobileMenu = () => {
    setIsMenuAnimating(true);
    setTimeout(() => {
      setShowMobileMenu(false);
      setIsMenuAnimating(false);
      setMobileExpandedSections([]);
    }, 300);
  };

  const toggleMobileMenu = () => {
    if (showMobileMenu) {
      closeMobileMenu();
    } else {
      setShowMobileMenu(true);
    }
  };

  const toggleMobileSection = (sectionId: number) => {
    setMobileExpandedSections(prev =>
      prev.includes(sectionId)
        ? prev.filter(id => id !== sectionId)
        : [...prev, sectionId]
    );
  };

  const handleLogout = () => {
    logout();
    setShowUserMenu(false);
    closeMobileMenu();
  };

  // Рендер ссылки (внутренняя/внешняя)
  const renderLink = (
    url: string,
    isExternal: boolean,
    openInNewTab: boolean,
    children: React.ReactNode,
    className: string,
    onClick?: () => void
  ) => {
    if (isExternal || openInNewTab) {
      return (
        <a
          href={url}
          target={openInNewTab ? '_blank' : undefined}
          rel={openInNewTab ? 'noopener noreferrer' : undefined}
          className={className}
          onClick={onClick}
        >
          {children}
        </a>
      );
    }

    return (
      <Link to={url} className={className} onClick={onClick}>
        {children}
      </Link>
    );
  };

  return (
    <>
      <nav className="bg-glass backdrop-blur-2xl border-b border-gray-800 sticky top-0 z-40">
        <div className="container mx-auto px-4">
          <div className="flex justify-between items-center h-16">
            {/* Logo */}
            <Link to="/" className="text-2xl font-bold bg-gradient-to-r from-highlight to-blue-500 bg-clip-text text-transparent">
              {siteName}
            </Link>

            {/* Desktop Navigation */}
            <div className="hidden lg:flex items-center space-x-6 absolute left-1/2 transform -translate-x-1/2">
              {navSections.map((section) => (
                <div key={section.id} className="relative" ref={el => { if (el) dropdownRefs.current.set(section.id, el); }}
                >
                  {section.type === 'link' ? (
                    // Simple link
                    renderLink(
                      section.url || '/',
                      section.isExternal,
                      section.openInNewTab,
                      <>
                        {section.icon && getIcon(section.icon) && (
                          <FontAwesomeIcon icon={getIcon(section.icon)!} className="mr-2" />
                        )}
                        <span>{section.name}</span>
                        {section.openInNewTab && (
                          <FontAwesomeIcon icon={faExternalLinkAlt} className="ml-1 text-xs opacity-50" />
                        )}
                      </>,
                      "text-gray-300 hover:text-highlight transition-colors duration-300 flex items-center space-x-2 px-4 py-2 rounded-lg hover:bg-highlight/10"
                    )
                  ) : (
                    // Dropdown
                    <>
                      <button
                        onClick={() => setActiveDropdown(activeDropdown === section.id ? null : section.id)}
                        className="flex items-center space-x-2 text-gray-300 hover:text-highlight transition-colors duration-300 px-4 py-2 rounded-lg hover:bg-highlight/10"
                      >
                        {section.icon && getIcon(section.icon) && (
                          <FontAwesomeIcon icon={getIcon(section.icon)!} className="mr-1" />
                        )}
                        <span>{section.name}</span>
                        <FontAwesomeIcon
                          icon={faChevronDown}
                          className={`transition-transform duration-200 ${activeDropdown === section.id ? 'rotate-180' : ''}`}
                        />
                      </button>

                      {activeDropdown === section.id && section.items.length > 0 && (
                        <div className="absolute left-0 top-full w-auto min-w-[12rem] whitespace-nowrap bg-secondary rounded-xl py-2 animate-slide-in z-50 overflow-hidden shadow-lg">
                          {section.items.map((item) => (
                            renderLink(
                              item.url || '/',
                              item.type === 'externallink',
                              item.openInNewTab,
                              <>
                                {item.icon && getIcon(item.icon) && (
                                  <FontAwesomeIcon icon={getIcon(item.icon)!} className="mr-2 w-4" />
                                )}
                                <span>{item.name}</span>
                                {item.openInNewTab && (
                                  <FontAwesomeIcon icon={faExternalLinkAlt} className="ml-auto text-xs opacity-50" />
                                )}
                              </>,
                              "flex items-center px-4 py-2 text-gray-300 hover:text-highlight hover:bg-secondary/70 transition-colors duration-200",
                              () => setActiveDropdown(null)
                            )
                          ))}
                        </div>
                      )}
                    </>
                  )}
                </div>
              ))}
            </div>

            {/* User menu (desktop) */}
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
                          className="block px-4 py-2 text-gray-300 hover:text-highlight hover:bg-secondary/70 transition-colors duration-200"
                          onClick={() => setShowUserMenu(false)}
                        >
                          Личный кабинет
                        </Link>
                        {user.isAdmin && (
                          <Link
                            to="/admin"
                            className="block px-4 py-2 text-gray-300 hover:text-highlight hover:bg-secondary/70 transition-colors duration-200"
                            onClick={() => setShowUserMenu(false)}
                          >
                            Админ-панель
                          </Link>
                        )}
                        <button
                          onClick={handleLogout}
                          className="w-full text-left px-4 py-2 text-gray-300 hover:text-highlight hover:bg-secondary/70 transition-colors duration-200 flex items-center space-x-2"
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

            {/* Mobile menu button */}
            <button
              onClick={toggleMobileMenu}
              className="lg:hidden text-2xl text-gray-300"
            >
              <FontAwesomeIcon icon={showMobileMenu ? faTimes : faBars} />
            </button>
          </div>
        </div>

        {/* Mobile Menu */}
        {(showMobileMenu || isMenuAnimating) && (
          <div className={`lg:hidden border-t border-gray-800 max-h-[calc(100vh-4rem)] overflow-y-auto ${showMobileMenu && !isMenuAnimating ? 'animate-slide-in' : 'animate-slide-out'}`}>
            <div className="px-4 py-4 space-y-3">
              {isAuthenticated && user && (
                <div className="flex items-center space-x-3 py-3 border-b border-gray-800 mb-4">
                  <Avatar username={user.username} avatarUrl={user.avatarUrl} size="md" />
                  <span className="text-gray-300 font-medium">{user.username}</span>
                </div>
              )}

              {/* Dynamic nav sections */}
              {navSections.map((section) => (
                <div key={section.id}>
                  {section.type === 'link' ? (
                    renderLink(
                      section.url || '/',
                      section.isExternal,
                      section.openInNewTab,
                      <>
                        {section.icon && getIcon(section.icon) && (
                          <FontAwesomeIcon icon={getIcon(section.icon)!} className="mr-2" />
                        )}
                        {section.name}
                      </>,
                      "block py-2 text-gray-300 hover:text-highlight transition-colors duration-300",
                      closeMobileMenu
                    )
                  ) : (
                    <>
                      <button
                        onClick={() => toggleMobileSection(section.id)}
                        className="w-full flex items-center justify-between py-2 text-gray-300 hover:text-highlight transition-colors duration-300"
                      >
                        <div className="flex items-center">
                          {section.icon && getIcon(section.icon) && (
                            <FontAwesomeIcon icon={getIcon(section.icon)!} className="mr-2" />
                          )}
                          <span>{section.name}</span>
                        </div>
                        <FontAwesomeIcon
                          icon={faChevronDown}
                          className={`text-gray-400 transition-transform duration-200 ${mobileExpandedSections.includes(section.id) ? 'rotate-180' : ''}`}
                        />
                      </button>

                      {mobileExpandedSections.includes(section.id) && (
                        <div className="pl-6 pt-2 space-y-1">
                          {section.items.map((item) => (
                            renderLink(
                              item.url || '/',
                              item.type === 'externallink',
                              item.openInNewTab,
                              <>
                                {item.icon && getIcon(item.icon) && (
                                  <FontAwesomeIcon icon={getIcon(item.icon)!} className="mr-2" />
                                )}
                                {item.name}
                              </>,
                              "block py-2 text-gray-300 hover:text-highlight transition-colors duration-300",
                              closeMobileMenu
                            )
                          ))}
                        </div>
                      )}
                    </>
                  )}
                </div>
              ))}

              {/* User options for mobile */}
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

      {/* Modals */}
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