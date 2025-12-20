import { useQuery } from '@tanstack/react-query';
import { useState, useRef, useMemo } from 'react';
import { getMyPrivileges, getMyVipPrivileges } from '../lib/donationApi';
import { getErrorMessage } from '../utils/errorUtils';
import type { UserAdminPrivilege, UserVipPrivilege } from '../types';
import { useForm } from 'react-hook-form';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCamera, faUser, faLock, faEye, faEyeSlash, faCog, faHome, faShield } from '@fortawesome/free-solid-svg-icons';
import { faSteam } from '@fortawesome/free-brands-svg-icons';
import toast from 'react-hot-toast';
import api from '../lib/axios';
import { useAuthStore } from '../store/authStore';
import { usePageTitle } from '../hooks/usePageTitle';
import Avatar from '../components/Avatar';
import { formatServerDate, getServerLocalTime, parseServerDate, isForeverDate } from '../utils/dateUtils';

interface UsernameForm {
  newUsername: string;
}

interface PasswordForm {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

interface SteamForm {
  steamInput: string;
}

type Tab = 'overview' | 'services' | 'settings';

const Profile = () => {
  usePageTitle('Личный кабинет');

  const { user, updateUser, isAuthenticated } = useAuthStore();
  const [activeTab, setActiveTab] = useState<Tab>('overview');
  const [isUploadingAvatar, setIsUploadingAvatar] = useState(false);
  const [showCurrentPassword, setShowCurrentPassword] = useState(false);
  const [showNewPassword, setShowNewPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [changingPasswordFor, setChangingPasswordFor] = useState<number | null>(null);
  const [newAdminPassword, setNewAdminPassword] = useState('');
  const fileInputRef = useRef<HTMLInputElement>(null);

  const {
    register: registerUsername,
    handleSubmit: handleSubmitUsername,
    formState: { errors: usernameErrors },
  } = useForm<UsernameForm>();

  const {
    register: registerPassword,
    handleSubmit: handleSubmitPassword,
    watch,
    reset: resetPassword,
    formState: { errors: passwordErrors },
  } = useForm<PasswordForm>();

  const {
    register: registerSteam,
    handleSubmit: handleSubmitSteam,
    reset: resetSteam,
    formState: { errors: steamErrors },
  } = useForm<SteamForm>();

  const { data: myPrivileges } = useQuery({
    queryKey: ['myPrivileges'],
    queryFn: getMyPrivileges,
    enabled: isAuthenticated,
  });

  const { data: myVipPrivileges } = useQuery({
    queryKey: ['myVipPrivileges'],
    queryFn: getMyVipPrivileges,
    enabled: isAuthenticated,
  });

  const { data: adminStatuses } = useQuery({
    queryKey: ['myAdminStatuses'],
    queryFn: async () => {
      const response = await api.get('/user/admin-status');
      return response.data;
    },
    enabled: isAuthenticated,
  });

  const combinedServices = useMemo(() => {
    type AdminPurchased = UserAdminPrivilege & { source: 'purchased'; canExtend: true; type: 'admin'; id: number };
    type AdminSourcebans = Partial<UserAdminPrivilege> & { serverId: number; serverName: string; source: 'sourcebans'; canExtend: false; id: string; type: 'admin' };
    type VipCombined = UserVipPrivilege & { source: 'purchased'; canExtend: false; type: 'vip'; isActive: boolean; daysRemaining: number };
    type CombinedService = AdminPurchased | AdminSourcebans | VipCombined;
    const services: CombinedService[] = [];

    if (myPrivileges) {
      myPrivileges.forEach((privilege: UserAdminPrivilege) => {
        services.push({
          ...privilege,
          source: 'purchased', 
          canExtend: true, 
          type: 'admin',
        });
      });
    }

    if (myVipPrivileges) {
      myVipPrivileges.forEach((privilege: UserVipPrivilege) => {
        const now = getServerLocalTime();
        const isActive = privilege.isActive && (!privilege.expiresAt || parseServerDate(privilege.expiresAt).getTime() > now.getTime());
        const daysRemaining = privilege.expiresAt
          ? (isForeverDate(privilege.expiresAt) ? 0 : Math.ceil((parseServerDate(privilege.expiresAt).getTime() - now.getTime()) / (1000 * 60 * 60 * 24)))
          : 0;
        
        services.push({
          ...privilege,
          source: 'purchased',
          canExtend: false,
          type: 'vip',
          isActive,
          daysRemaining,
        });
      });
    }

    if (adminStatuses) {
      adminStatuses.forEach((status: { serverId: number; serverName: string; tariffName?: string; flags?: string; groupName?: string; immunity?: number }) => {
        const existingService = services.find(s => s.serverId === status.serverId);
        if (!existingService) {
          services.push({
            ...status,
            source: 'sourcebans',
            canExtend: false, 
            id: `sourcebans-${status.serverId}`, 
            type: 'admin',
          } as AdminSourcebans);
        }
      });
    }

    return services;
  }, [myPrivileges, myVipPrivileges, adminStatuses]);

  const handleUsernameSubmit = async (data: UsernameForm) => {
    try {
      const response = await api.put('/profile/username', data);
      updateUser(response.data);
      toast.success('Имя пользователя успешно изменено');
    } catch (err: unknown) {
      const message = getErrorMessage(err) || 'Ошибка при изменении имени';
      toast.error(message);
    }
  };

  const handlePasswordSubmit = async (data: PasswordForm) => {
    if (data.newPassword !== data.confirmPassword) {
      toast.error('Пароли не совпадают');
      return;
    }

    try {
      await api.put('/profile/password', {
        currentPassword: data.currentPassword,
        newPassword: data.newPassword,
      });
      toast.success('Пароль успешно изменен');
      resetPassword();
    } catch (err: unknown) {
      const message = getErrorMessage(err) || 'Ошибка при изменении пароля';
      toast.error(message);
    }
  };

  const handleSteamSubmit = async (data: SteamForm) => {
    try {
      const response = await api.put('/profile/steam', data);
      updateUser(response.data);
      toast.success('Steam ID успешно обновлен');
      resetSteam();
    } catch (err: unknown) {
      const message = getErrorMessage(err) || 'Ошибка при обновлении Steam ID';
      toast.error(message);
    }
  };

  const handleAvatarClick = () => {
    fileInputRef.current?.click();
  };

  const handleAdminPasswordChange = async (privilegeId: number, newPassword: string) => {
    try {
      await api.put('/profile/admin-password', {
        privilegeId,
        newPassword
      });
      toast.success('Пароль админа успешно изменен');
      setChangingPasswordFor(null);
      setNewAdminPassword('');
    } catch (err: unknown) {
      const message = getErrorMessage(err) || 'Ошибка при изменении пароля админа';
      toast.error(message);
    }
  };

  const handleAvatarChange = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (!file) return;

    if (file.size > 2 * 1024 * 1024) {
      toast.error('Размер файла не должен превышать 2 МБ');
      return;
    }

    const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif', 'image/webp'];
    if (!allowedTypes.includes(file.type)) {
      toast.error('Разрешены только изображения (jpg, png, gif, webp)');
      return;
    }

    setIsUploadingAvatar(true);
    try {
      const formData = new FormData();
      formData.append('file', file);

      const uploadResponse = await api.post('/upload/avatar', formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
      });

      const avatarUrl = uploadResponse.data.url;
      const response = await api.put('/profile/avatar', { avatarUrl });

      updateUser(response.data);
      toast.success('Аватар успешно обновлен');
    } catch (err: unknown) {
      const message = getErrorMessage(err) || 'Ошибка при загрузке аватара';
      toast.error(message);
    } finally {
      setIsUploadingAvatar(false);
    }
  };

  if (!user) return null;

  return (
    <div className="min-h-screen py-8 px-4">
      <div className="container mx-auto max-w-6xl">
        <h1 className="text-3xl font-bold text-white mb-8">Личный кабинет</h1>

        <div className="grid grid-cols-1 lg:grid-cols-[280px_1fr] gap-6">
          <div className="space-y-2">
            <button
              onClick={() => setActiveTab('overview')}
              className={`w-full flex items-center gap-3 px-4 py-3 rounded-xl transition-colors duration-200 ${
                activeTab === 'overview'
                  ? 'bg-highlight text-white'
                  : 'border border-gray-800/50 text-gray-300 hover:border-gray-700/50'
              }`}
            >
              <FontAwesomeIcon icon={faHome} />
              <span className="font-medium">Главная</span>
            </button>
            
            <button
              onClick={() => setActiveTab('services')}
              className={`w-full flex items-center gap-3 px-4 py-3 rounded-xl transition-colors duration-200 ${
                activeTab === 'services'
                  ? 'bg-highlight text-white'
                  : 'border border-gray-800/50 text-gray-300 hover:border-gray-700/50'
              }`}
            >
              <FontAwesomeIcon icon={faShield} />
              <span className="font-medium">Мои услуги</span>
            </button>
            
            <button
              onClick={() => setActiveTab('settings')}
              className={`w-full flex items-center gap-3 px-4 py-3 rounded-xl transition-colors duration-200 ${
                activeTab === 'settings'
                  ? 'bg-highlight text-white'
                  : 'border border-gray-800/50 text-gray-300 hover:border-gray-700/50'
              }`}
            >
              <FontAwesomeIcon icon={faCog} />
              <span className="font-medium">Настройки профиля</span>
            </button>
          </div>

          <div>
            {activeTab === 'overview' && (
              <div className="space-y-6">
                <div className="rounded-xl border border-gray-800/50 p-8">
                  <h2 className="text-2xl font-bold text-white mb-2">
                    Привет, <span className="text-highlight">{user.username}</span>!
                  </h2>
                  <p className="text-gray-400 mb-6">
                    Добро пожаловать в ваш личный кабинет. Здесь вы можете управлять своим профилем.
                  </p>
                  
                  <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mt-6">
                    <div className="rounded-lg bg-gray-800/50 p-4">
                      <div className="text-sm text-gray-400 mb-1">Имя пользователя</div>
                      <div className="text-lg font-semibold text-white">{user.username}</div>
                    </div>
                    <div className="rounded-lg bg-gray-800/50 p-4">
                      <div className="text-sm text-gray-400 mb-1">Email</div>
                      <div className="text-lg font-semibold text-white">{user.email}</div>
                    </div>
                    {user.steamId && (
                      <div className="rounded-lg bg-gray-800/50 p-4 md:col-span-2">
                        <div className="text-sm text-gray-400 mb-1">Steam ID</div>
                        <div className="flex items-center justify-between">
                          <div className="text-lg font-semibold text-white font-mono">{user.steamId}</div>
                          {user.steamProfileUrl && (
                            <a
                              href={user.steamProfileUrl}
                              target="_blank"
                              rel="noopener noreferrer"
                              className="text-highlight hover:underline text-sm flex items-center gap-1"
                            >
                              <FontAwesomeIcon icon={faSteam} />
                              Профиль Steam
                            </a>
                          )}
                        </div>
                      </div>
                    )}
                  </div>
                </div>

                <div className="rounded-xl border border-gray-800/50 p-8">
                  <h3 className="text-xl font-bold text-white mb-6">Аватар профиля</h3>
                  <div className="flex flex-col items-center">
                    <div className="relative group">
                      <Avatar username={user.username} avatarUrl={user.avatarUrl} size="xl" />
                      <button
                        onClick={handleAvatarClick}
                        disabled={isUploadingAvatar}
                        className="absolute inset-0 rounded-full bg-black/50 opacity-0 group-hover:opacity-100 transition-opacity duration-200 flex items-center justify-center cursor-pointer"
                      >
                        <FontAwesomeIcon icon={faCamera} className="text-white text-2xl" />
                      </button>
                      {isUploadingAvatar && (
                        <div className="absolute inset-0 rounded-full bg-black/70 flex items-center justify-center">
                          <div className="animate-spin rounded-full h-8 w-8 border-t-2 border-b-2 border-highlight"></div>
                        </div>
                      )}
                    </div>
                    <input
                      ref={fileInputRef}
                      type="file"
                      accept="image/*"
                      onChange={handleAvatarChange}
                      className="hidden"
                    />
                    <p className="text-gray-400 text-sm mt-4">Нажмите на аватар, чтобы изменить</p>
                    <p className="text-gray-500 text-xs mt-1">Максимум 2 МБ (JPG, PNG, GIF, WEBP)</p>
                  </div>
                </div>
              </div>
            )}

            {activeTab === 'services' && (
              <div className="space-y-6">
                <div className="rounded-xl border border-gray-800/50 p-8">
                  <h2 className="text-2xl font-bold text-white mb-6 flex items-center gap-3">
                    <FontAwesomeIcon icon={faShield} className="text-purple-500" />
                    Мои услуги
                  </h2>
                  
                  {combinedServices && combinedServices.length > 0 ? (
                    <div className="space-y-4">
                      {combinedServices.map((service) => (
                        <div
                          key={service.id || service.source + '-' + service.serverId}
                          className="group relative overflow-hidden rounded-xl border border-gray-800/40 p-5 bg-gradient-to-br from-[#0b0f12]/60 via-[#071018]/40 to-[#071018]/20"
                        >
                          <div className="absolute left-0 top-0 bottom-0 w-1 bg-gradient-to-b from-purple-500 to-indigo-500 opacity-80" />
                          <div className="flex items-start justify-between mb-4">
                            <div>
                              <h3 className="text-xl font-semibold text-white mb-1">
                                {service.type === 'vip' ? `VIP ${service.tariffName || `на ${service.serverName}`}` : (service.tariffName || `Админ на ${service.serverName}`)}
                              </h3>
                              <p className="text-gray-400">{service.serverName}</p>
                              {service.source === 'sourcebans' && (
                                <span className="inline-block mt-1 px-2 py-1 text-xs bg-blue-500/10 text-blue-300 border border-blue-700/20 rounded">
                                  Из SourceBans
                                </span>
                              )}
                              {service.type === 'vip' && (
                                <span className="inline-block mt-1 px-2 py-1 text-xs bg-blue-500/20 text-blue-400 rounded">
                                  VIP статус
                                </span>
                              )}
                            </div>
                            <div className={`px-3 py-1 rounded-full text-sm font-medium backdrop-blur-sm ${
                              service.isActive
                                ? 'bg-green-500/20 text-green-400 border border-green-500/30'
                                : service.isExpired
                                ? 'bg-red-500/20 text-red-400 border border-red-500/30'
                                : 'bg-blue-500/20 text-blue-400 border border-blue-500/30'
                            }`}>
                              {service.isActive ? 'Активна' : service.isExpired ? 'Истекла' : 'Неактивна'}
                            </div>
                          </div>

                          <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4 mt-2">
                            <div className="space-y-2">
                              <div className="flex items-center gap-2 text-gray-300">
                                <FontAwesomeIcon icon={faHome} className="w-4 h-4 text-gray-500" />
                                <span className="text-sm">
                                  {service.expiresAt
                                    ? (isForeverDate(service.expiresAt)
                                        ? 'Навсегда'
                                        : service.isActive
                                          ? `Истекает: ${formatServerDate(service.expiresAt, { year: 'numeric', month: 'short', day: 'numeric' })}`
                                          : `Истекла: ${formatServerDate(service.expiresAt, { year: 'numeric', month: 'short', day: 'numeric' })}`)
                                    : '—'
                                  }
                                </span>
                              </div>
                              
                              {service.type === 'vip' && service.daysRemaining > 0 && (
                                <div className="flex items-center gap-2 text-gray-300">
                                  <FontAwesomeIcon icon={faHome} className="w-4 h-4 text-gray-500" />
                                  <span className="text-sm">
                                    Осталось: {service.daysRemaining} {service.daysRemaining === 1 ? 'день' : service.daysRemaining < 5 ? 'дня' : 'дней'}
                                  </span>
                                </div>
                              )}
                            </div>

                            <div className="space-y-2">
                              {service.type === 'admin' && service.flags && (
                                <div className="flex items-start gap-2 text-gray-300">
                                  <FontAwesomeIcon icon={faShield} className="w-4 h-4 text-purple-500 mt-0.5" />
                                  <div>
                                    <div className="text-xs text-gray-500 mb-1">Флаги</div>
                                    <code className="text-xs bg-gray-700/50 px-2 py-1 rounded">{service.flags}</code>
                                  </div>
                                </div>
                              )}
                              
                              {service.type === 'admin' && service.groupName && (
                                <div className="flex items-start gap-2 text-gray-300">
                                  <FontAwesomeIcon icon={faShield} className="w-4 h-4 text-blue-500 mt-0.5" />
                                  <div>
                                    <div className="text-xs text-gray-500 mb-1">Группа</div>
                                    <code className="text-xs bg-gray-700/50 px-2 py-1 rounded">{service.groupName}</code>
                                  </div>
                                </div>
                              )}
                              
                              {service.type === 'admin' && (service.immunity ?? 0) > 0 && (
                                <div className="flex items-center gap-2 text-gray-300">
                                  <FontAwesomeIcon icon={faShield} className="w-4 h-4 text-red-500" />
                                  <span className="text-sm">Иммунитет: {service.immunity}</span>
                                </div>
                              )}

                              {service.type === 'vip' && (
                                <div className="flex items-center gap-2 text-gray-300">
                                  <FontAwesomeIcon icon={faShield} className="w-4 h-4 text-yellow-500" />
                                  <span className="text-sm">VIP привилегии активны</span>
                                </div>
                              )}
                            </div>
                          </div>

                              {service.isActive && service.type === 'admin' && service.source === 'purchased' && (
                            <div className="bg-highlight/10 border border-highlight/30 rounded-lg p-4 backdrop-blur-sm">
                              <h4 className="text-highlight font-medium mb-2 flex items-center gap-2">
                                <FontAwesomeIcon icon={faShield} className="w-4 h-4" />
                                Инструкция по подключению:
                              </h4>
                              <div className="text-sm text-gray-300 space-y-2">
                                <p>1. Запустите Counter-Strike Source</p>
                                <p>2. Подключитесь к серверу <strong>{service.serverName}</strong></p>
                                <p>3. В консоли введите: <code className="bg-gray-700/50 px-2 py-1 rounded text-xs">setinfo _pw "{service.adminPassword || 'ВАШ_ПАРОЛЬ'}"</code></p>
                                <p>4. Переподключитесь к серверу</p>
                              </div>
                              {service.adminPassword && (
                                <div className="mt-3 p-3 bg-gray-800/50 rounded-lg border border-gray-700/50">
                                  <div className="flex items-center justify-between">
                                    <span className="text-sm text-gray-400">Ваш пароль админа:</span>
                                    <div className="flex items-center gap-2">
                                      <code className="bg-gray-700/50 px-3 py-1 rounded text-white font-mono text-sm select-all">
                                        {service.adminPassword}
                                      </code>
                                      <button
                                        onClick={() => navigator.clipboard.writeText(service.adminPassword!)}
                                        className="text-gray-400 hover:text-white transition-colors"
                                        title="Копировать пароль"
                                      >
                                        <FontAwesomeIcon icon={faEye} className="w-4 h-4" />
                                      </button>
                                    </div>
                                  </div>
                                </div>
                              )}
                              <div className="mt-3 text-xs text-gray-500">
                                Пароль можно изменить ниже. Он чувствителен к регистру.
                              </div>
                            </div>
                          )}

                          {service.isActive && service.type === 'vip' && service.source === 'purchased' && (
                            <div className="bg-blue-500/10 border border-blue-500/30 rounded-lg p-4 backdrop-blur-sm">
                              <h4 className="text-blue-400 font-medium mb-2 flex items-center gap-2">
                                <FontAwesomeIcon icon={faShield} className="w-4 h-4" />
                                VIP статус активен:
                              </h4>
                              <div className="text-sm text-gray-300 space-y-2">
                                <p>Ваши VIP привилегии активны на сервере <strong>{service.serverName}</strong></p>
                                <p>VIP статус автоматически применяется при подключении к серверу</p>
                                <p>Наслаждайтесь расширенными возможностями и привилегиями!</p>
                              </div>
                            </div>
                          )}

                          {service.isActive && service.type === 'admin' && service.source === 'purchased' && (
                            <div className="border-t border-gray-700/50 pt-4">
                              <h4 className="text-white font-medium mb-3">Изменить пароль админа</h4>
                              
                              {typeof service.id === 'number' && changingPasswordFor === service.id ? (
                                <div className="space-y-3">
                                  <input
                                    type="password"
                                    value={newAdminPassword}
                                    onChange={(e) => setNewAdminPassword(e.target.value)}
                                    placeholder="Введите новый пароль"
                                    className="w-full bg-gray-700/50 border border-gray-600/50 rounded-lg px-4 py-3 text-white placeholder-gray-500 focus:outline-none focus:border-purple-500/60"
                                    autoFocus
                                  />
                                  <div className="flex gap-2">
                                    <button
                                      onClick={() => typeof service.id === 'number' && handleAdminPasswordChange(service.id, newAdminPassword)}
                                      disabled={!newAdminPassword.trim() || newAdminPassword.length < 4}
                                      className="flex-1 bg-purple-500 hover:bg-purple-600 text-white font-medium py-2 px-4 rounded-lg transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
                                    >
                                      Сохранить
                                    </button>
                                    <button
                                      onClick={() => {
                                        setChangingPasswordFor(null);
                                        setNewAdminPassword('');
                                      }}
                                      className="bg-gray-600 hover:bg-gray-700 text-white font-medium py-2 px-4 rounded-lg transition-colors"
                                    >
                                      Отмена
                                    </button>
                                  </div>
                                </div>
                              ) : (
                                <button
                                  onClick={() => typeof service.id === 'number' && setChangingPasswordFor(service.id)}
                                  className="bg-highlight/20 hover:bg-highlight/30 text-highlight font-medium py-2 px-4 rounded-lg transition-colors border border-highlight/30 hover:border-highlight/50"
                                >
                                  Изменить пароль
                                </button>
                              )}
                            </div>
                          )}

                          {service.source === 'sourcebans' && (
                            <div className="border-t border-gray-700/50 pt-4">
                              <div className="text-sm text-gray-400">
                                Эта привилегия получена не через наш сайт. Для управления обратитесь к администраторам сервера.
                              </div>
                            </div>
                          )}

                        </div>
                      ))}
                    </div>
                  ) : (
                    <div className="text-center py-12">
                      <FontAwesomeIcon icon={faShield} className="w-16 h-16 mx-auto mb-4 text-gray-600" />
                      <p className="text-gray-400 text-lg mb-2">У вас пока нет активных услуг</p>
                      <p className="text-gray-500 text-sm">Приобретите админ-права в разделе донатов</p>
                    </div>
                  )}
                </div>
              </div>
            )}

            {activeTab === 'settings' && (
              <div className="space-y-6">
                <div className="rounded-xl border border-gray-800/50 p-6">
                  <div className="flex items-center mb-4">
                    <FontAwesomeIcon icon={faUser} className="text-highlight text-xl mr-3" />
                    <h2 className="text-xl font-bold text-white">Изменить имя пользователя</h2>
                  </div>
                  <form onSubmit={handleSubmitUsername(handleUsernameSubmit)} className="space-y-4">
                    <div>
                      <label className="block text-sm font-medium mb-2 text-gray-300">Текущее имя</label>
                      <input
                        type="text"
                        value={user.username}
                        disabled
                        className="input-field bg-gray-800/50 text-gray-500 cursor-not-allowed"
                      />
                    </div>
                    <div>
                      <label className="block text-sm font-medium mb-2 text-gray-300">Новое имя пользователя</label>
                      <input
                        {...registerUsername('newUsername', {
                          required: 'Обязательное поле',
                          minLength: { value: 3, message: 'Минимум 3 символа' },
                        })}
                        type="text"
                        className="input-field"
                        placeholder="Введите новое имя"
                      />
                      {usernameErrors.newUsername && (
                        <p className="text-red-500 text-sm mt-1">{usernameErrors.newUsername.message}</p>
                      )}
                    </div>
                    <button type="submit" className="btn-primary">
                      Сохранить имя
                    </button>
                  </form>
                </div>

                <div className="rounded-xl border border-gray-800/50 p-6">
                  <div className="flex items-center mb-4">
                    <FontAwesomeIcon icon={faLock} className="text-highlight text-xl mr-3" />
                    <h2 className="text-xl font-bold text-white">Изменить пароль</h2>
                  </div>
                  <form onSubmit={handleSubmitPassword(handlePasswordSubmit)} className="space-y-4">
                    <div>
                      <label className="block text-sm font-medium mb-2 text-gray-300">Текущий пароль</label>
                      <div className="relative">
                        <input
                          {...registerPassword('currentPassword', {
                            required: 'Обязательное поле',
                          })}
                          type={showCurrentPassword ? 'text' : 'password'}
                          className="input-field pr-12"
                          placeholder="Введите текущий пароль"
                        />
                        <button
                          type="button"
                          onClick={() => setShowCurrentPassword(!showCurrentPassword)}
                          className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-300 transition-colors"
                        >
                          <FontAwesomeIcon icon={showCurrentPassword ? faEyeSlash : faEye} />
                        </button>
                      </div>
                      {passwordErrors.currentPassword && (
                        <p className="text-red-500 text-sm mt-1">{passwordErrors.currentPassword.message}</p>
                      )}
                    </div>

                    <div>
                      <label className="block text-sm font-medium mb-2 text-gray-300">Новый пароль</label>
                      <div className="relative">
                        <input
                          {...registerPassword('newPassword', {
                            required: 'Обязательное поле',
                            minLength: { value: 6, message: 'Минимум 6 символов' },
                          })}
                          type={showNewPassword ? 'text' : 'password'}
                          className="input-field pr-12"
                          placeholder="Введите новый пароль"
                        />
                        <button
                          type="button"
                          onClick={() => setShowNewPassword(!showNewPassword)}
                          className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-300 transition-colors"
                        >
                          <FontAwesomeIcon icon={showNewPassword ? faEyeSlash : faEye} />
                        </button>
                      </div>
                      {passwordErrors.newPassword && (
                        <p className="text-red-500 text-sm mt-1">{passwordErrors.newPassword.message}</p>
                      )}
                    </div>

                    <div>
                      <label className="block text-sm font-medium mb-2 text-gray-300">Подтвердите новый пароль</label>
                      <div className="relative">
                        <input
                          {...registerPassword('confirmPassword', {
                            required: 'Обязательное поле',
                            validate: (value) => value === watch('newPassword') || 'Пароли не совпадают',
                          })}
                          type={showConfirmPassword ? 'text' : 'password'}
                          className="input-field pr-12"
                          placeholder="Повторите новый пароль"
                        />
                        <button
                          type="button"
                          onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                          className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-300 transition-colors"
                        >
                          <FontAwesomeIcon icon={showConfirmPassword ? faEyeSlash : faEye} />
                        </button>
                      </div>
                      {passwordErrors.confirmPassword && (
                        <p className="text-red-500 text-sm mt-1">{passwordErrors.confirmPassword.message}</p>
                      )}
                    </div>

                    <button type="submit" className="btn-primary">
                      Изменить пароль
                    </button>
                  </form>
                </div>

                <div className="rounded-xl border border-gray-800/50 p-6">
                  <div className="flex items-center mb-4">
                    <FontAwesomeIcon icon={faSteam} className="text-highlight text-xl mr-3" />
                    <h2 className="text-xl font-bold text-white">Steam ID</h2>
                  </div>
                  
                  {user.steamId && user.steamProfileUrl && (
                    <div className="mb-4 p-4 bg-gray-800/50 rounded-lg">
                      <div className="flex items-center justify-between">
                        <div>
                          <p className="text-sm text-gray-400 mb-1">Текущий Steam ID</p>
                          <p className="text-white font-mono text-sm">{user.steamId}</p>
                          <a
                            href={user.steamProfileUrl}
                            target="_blank"
                            rel="noopener noreferrer"
                            className="text-highlight hover:underline text-sm mt-1 inline-block"
                          >
                            Открыть профиль Steam →
                          </a>
                        </div>
                      </div>
                    </div>
                  )}

                  <form onSubmit={handleSubmitSteam(handleSteamSubmit)} className="space-y-4">
                    <div>
                      <label className="block text-sm font-medium mb-2 text-gray-300">
                        {user.steamId ? 'Обновить Steam ID' : 'Добавить Steam ID'}
                      </label>
                      <input
                        {...registerSteam('steamInput', {
                          required: 'Обязательное поле',
                        })}
                        type="text"
                        className="input-field"
                        placeholder="STEAM_0:0:X, [U:1:X], 76561198... или ссылка"
                      />
                      {steamErrors.steamInput && (
                        <p className="text-red-500 text-sm mt-1">{steamErrors.steamInput.message}</p>
                      )}
                      <p className="text-xs text-gray-500 mt-2">
                        Поддерживаемые форматы: STEAM_0:0:X, [U:1:X], 76561198XXXXXXXXX или ссылка на профиль Steam
                      </p>
                    </div>
                    <button type="submit" className="btn-primary">
                      {user.steamId ? 'Обновить Steam ID' : 'Добавить Steam ID'}
                    </button>
                  </form>
                </div>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default Profile;
