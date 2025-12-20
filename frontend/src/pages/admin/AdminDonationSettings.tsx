import { useState, useEffect } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { API_URL } from '../../config/api';
import { 
  getYooMoneySettings, 
  updateYooMoneySettings,
  getAllSourceBansSettings,
  upsertSourceBansSettings,
  deleteSourceBansSettings,
  testSourceBansConnection,
  getDonationPackage,
  updateDonationPackage,
  getAllVipSettings,
  upsertVipSettings,
  deleteVipSettings,
  testVipConnection
} from '../../lib/donationApi';
import api from '../../lib/axios';
import type { UpdateYooMoneySettings, UpdateSourceBansSettings, UpdateDonationPackage, Server, SourceBansSettings, UpdateVipSettings, VipSettings } from '../../types';
import { Save, Wallet, Database, CheckCircle, Gift, Trash2, Plus, Server as ServerIcon, FileText } from 'lucide-react';
import { formatServerDate } from '../../utils/dateUtils';
import toast from 'react-hot-toast';
import { getErrorMessage } from '../../utils/errorUtils';

export default function AdminDonationSettings() {
  const queryClient = useQueryClient();
  const [activeTab, setActiveTab] = useState<'yoomoney' | 'sourcebans' | 'vip' | 'package'>('yoomoney');
  const [showSourceBansForm, setShowSourceBansForm] = useState(false);
  const [showVipForm, setShowVipForm] = useState(false);

  const { data: yooMoneySettings, isLoading: yooMoneyLoading } = useQuery({
    queryKey: ['yooMoneySettings'],
    queryFn: getYooMoneySettings,
  });

  const [yooMoneyForm, setYooMoneyForm] = useState<UpdateYooMoneySettings>({
    walletNumber: '',
    secretKey: '',
  });

  useEffect(() => {
    if (yooMoneySettings && yooMoneySettings.isConfigured) {
      setYooMoneyForm({
        walletNumber: yooMoneySettings.walletNumber,
        secretKey: yooMoneySettings.secretKey,
      });
    }
  }, [yooMoneySettings]);

  const updateYooMoneyMutation = useMutation({
    mutationFn: updateYooMoneySettings,
    onSuccess: () => {
      toast.success('Настройки ЮMoney сохранены');
      queryClient.invalidateQueries({ queryKey: ['yooMoneySettings'] });
    },
    onError: (error: unknown) => {
      toast.error(getErrorMessage(error) || 'Ошибка при сохранении настроек');
    },
  });

  const { data: allSourceBansSettings, isLoading: sourceBansLoading } = useQuery({
    queryKey: ['allSourceBansSettings'],
    queryFn: getAllSourceBansSettings,
  });

  const { data: servers } = useQuery<Server[]>({
    queryKey: ['servers'],
    queryFn: async () => {
      const response = await api.get<Server[]>('/servers');
      return response.data;
    },
  });

  const [sourceBansForm, setSourceBansForm] = useState<UpdateSourceBansSettings>({
    serverId: undefined,
    host: '',
    port: 3306,
    database: '',
    username: '',
    password: '',
  });

  const updateSourceBansMutation = useMutation({
    mutationFn: upsertSourceBansSettings,
    onSuccess: () => {
      toast.success('Настройки SourceBans сохранены');
      queryClient.invalidateQueries({ queryKey: ['allSourceBansSettings'] });
      setShowSourceBansForm(false);
      setSourceBansForm({
        serverId: undefined,
        host: '',
        port: 3306,
        database: '',
        username: '',
        password: '',
      });
    },
    onError: (error: unknown) => {
      toast.error(getErrorMessage(error) || 'Ошибка при сохранении настроек');
    },
  });

  const deleteSourceBansMutation = useMutation({
    mutationFn: deleteSourceBansSettings,
    onSuccess: () => {
      toast.success('Настройки SourceBans удалены');
      queryClient.invalidateQueries({ queryKey: ['allSourceBansSettings'] });
    },
    onError: (error: unknown) => {
      toast.error(getErrorMessage(error) || 'Ошибка при удалении настроек');
    },
  });

  const testSourceBansMutation = useMutation({
    mutationFn: (serverId: number) => testSourceBansConnection(serverId),
    onSuccess: (data) => {
      if (data.success) {
        toast.success(data.message);
      } else {
        toast.error(data.message);
      }
    },
    onError: () => {
      toast.error('Ошибка при проверке подключения');
    },
  });

  const { data: allVipSettings, isLoading: vipLoading } = useQuery({
    queryKey: ['allVipSettings'],
    queryFn: getAllVipSettings,
  });

  const [vipForm, setVipForm] = useState<UpdateVipSettings>({
    serverId: undefined,
    host: '',
    port: 3306,
    database: '',
    username: '',
    password: '',
  });

  const updateVipMutation = useMutation({
    mutationFn: upsertVipSettings,
    onSuccess: () => {
      toast.success('Настройки VIP сохранены');
      queryClient.invalidateQueries({ queryKey: ['allVipSettings'] });
      setShowVipForm(false);
      setVipForm({
        serverId: undefined,
        host: '',
        port: 3306,
        database: '',
        username: '',
        password: '',
      });
    },
    onError: (error: unknown) => {
      toast.error(getErrorMessage(error) || 'Ошибка при сохранении настроек');
    },
  });

  const deleteVipMutation = useMutation({
    mutationFn: deleteVipSettings,
    onSuccess: () => {
      toast.success('Настройки VIP удалены');
      queryClient.invalidateQueries({ queryKey: ['allVipSettings'] });
    },
    onError: (error: unknown) => {
      toast.error(getErrorMessage(error) || 'Ошибка при удалении настроек');
    },
  });

  const testVipMutation = useMutation({
    mutationFn: (serverId: number) => testVipConnection(serverId),
    onSuccess: (data) => {
      if (data.success) {
        toast.success(data.message);
      } else {
        toast.error(data.message);
      }
    },
    onError: () => {
      toast.error('Ошибка при проверке подключения');
    },
  });

  const { data: donationPackage, isLoading: packageLoading } = useQuery({
    queryKey: ['donationPackage'],
    queryFn: getDonationPackage,
  });

  const [packageForm, setPackageForm] = useState<UpdateDonationPackage>({
    title: '',
    description: '',
    suggestedAmounts: [],
    isActive: true,
  });

  const [suggestedAmountsInput, setSuggestedAmountsInput] = useState<string>('');

  useEffect(() => {
    if (donationPackage && donationPackage.id) {
      setPackageForm({
        title: donationPackage.title,
        description: donationPackage.description,
        suggestedAmounts: donationPackage.suggestedAmounts,
        isActive: donationPackage.isActive,
      });
      setSuggestedAmountsInput(donationPackage.suggestedAmounts?.join(', ') || '');
    }
  }, [donationPackage]);

  const updatePackageMutation = useMutation({
    mutationFn: updateDonationPackage,
    onSuccess: () => {
      toast.success('Настройки пакета доната сохранены');
      queryClient.invalidateQueries({ queryKey: ['donationPackage'] });
    },
    onError: (error: unknown) => {
      toast.error(getErrorMessage(error) || 'Ошибка при сохранении настроек');
    },
  });

  const handleYooMoneySave = () => {
    if (!yooMoneyForm.walletNumber || !yooMoneyForm.secretKey) {
      toast.error('Заполните все поля');
      return;
    }
    updateYooMoneyMutation.mutate(yooMoneyForm);
  };

  const handleSourceBansSave = () => {
    if (!sourceBansForm.serverId || !sourceBansForm.host || !sourceBansForm.database || !sourceBansForm.username || !sourceBansForm.password) {
      toast.error('Заполните все поля');
      return;
    }
    updateSourceBansMutation.mutate(sourceBansForm);
  };

  const handleEditSourceBans = (settings: SourceBansSettings) => {
    setSourceBansForm({
      serverId: settings.serverId,
      host: settings.host,
      port: settings.port,
      database: settings.database,
      username: settings.username,
      password: '', 
    });
    setShowSourceBansForm(true);
  };

  const handleAddNewSourceBans = () => {
    setSourceBansForm({
      serverId: undefined,
      host: '',
      port: 3306,
      database: '',
      username: '',
      password: '',
    });
    setShowSourceBansForm(true);
  };

  const handleDeleteSourceBans = (serverId: number) => {
    if (window.confirm('Вы уверены, что хотите удалить настройки SourceBans для этого сервера?')) {
      deleteSourceBansMutation.mutate(serverId);
    }
  };

  const handlePackageSave = () => {
    if (!packageForm.title || !packageForm.description) {
      toast.error('Заполните обязательные поля');
      return;
    }

    const amounts = suggestedAmountsInput
      .split(',')
      .map(s => parseInt(s.trim()))
      .filter(n => !isNaN(n) && n > 0);

    updatePackageMutation.mutate({
      ...packageForm,
      suggestedAmounts: amounts.length > 0 ? amounts : undefined,
    });
  };

  const handleVipSave = () => {
    if (!vipForm.serverId || !vipForm.host || !vipForm.database || !vipForm.username || !vipForm.password) {
      toast.error('Заполните все поля');
      return;
    }
    updateVipMutation.mutate(vipForm);
  };

  const handleEditVip = (settings: VipSettings) => {
    setVipForm({
      serverId: settings.serverId,
      host: settings.host,
      port: settings.port,
      database: settings.database,
      username: settings.username,
      password: '', 
    });
    setShowVipForm(true);
  };

  const handleAddNewVip = () => {
    setVipForm({
      serverId: undefined,
      host: '',
      port: 3306,
      database: '',
      username: '',
      password: '',
    });
    setShowVipForm(true);
  };

  const handleDeleteVip = (serverId: number) => {
    if (window.confirm('Вы уверены, что хотите удалить настройки VIP для этого сервера?')) {
      deleteVipMutation.mutate(serverId);
    }
  };

  const getAvailableServersForVip = () => {
    if (!servers) return [];
    const configuredServerIds = allVipSettings?.map(s => s.serverId) || [];
    return servers.filter(s => !configuredServerIds.includes(s.id));
  };

  if (yooMoneyLoading || sourceBansLoading || packageLoading || vipLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="text-gray-400">Загрузка...</div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-3xl font-bold text-white mb-2">Настройки системы доната</h1>
        <p className="text-gray-400">Управление настройками платежей и интеграций</p>
      </div>

      <div className="border-b border-gray-700">
        <nav className="flex space-x-8">
          <button
            onClick={() => setActiveTab('yoomoney')}
            className={`py-4 px-1 border-b-2 font-medium text-sm transition-colors ${
              activeTab === 'yoomoney'
                ? 'border-accent text-white'
                : 'border-transparent text-gray-400 hover:text-gray-300 hover:border-gray-600'
            }`}
          >
            <div className="flex items-center gap-2">
              <Wallet className="w-5 h-5" />
              <span>ЮMoney</span>
            </div>
          </button>
          <button
            onClick={() => setActiveTab('sourcebans')}
            className={`py-4 px-1 border-b-2 font-medium text-sm transition-colors ${
              activeTab === 'sourcebans'
                ? 'border-accent text-white'
                : 'border-transparent text-gray-400 hover:text-gray-300 hover:border-gray-600'
            }`}
          >
            <div className="flex items-center gap-2">
              <Database className="w-5 h-5" />
              <span>SourceBans</span>
            </div>
          </button>
          <button
            onClick={() => setActiveTab('vip')}
            className={`py-4 px-1 border-b-2 font-medium text-sm transition-colors ${
              activeTab === 'vip'
                ? 'border-accent text-white'
                : 'border-transparent text-gray-400 hover:text-gray-300 hover:border-gray-600'
            }`}
          >
            <div className="flex items-center gap-2">
              <ServerIcon className="w-5 h-5" />
              <span>VIP Настройки</span>
            </div>
          </button>
          <button
            onClick={() => setActiveTab('package')}
            className={`py-4 px-1 border-b-2 font-medium text-sm transition-colors ${
              activeTab === 'package'
                ? 'border-accent text-white'
                : 'border-transparent text-gray-400 hover:text-gray-300 hover:border-gray-600'
            }`}
          >
            <div className="flex items-center gap-2">
              <Gift className="w-5 h-5" />
              <span>Пакет доната</span>
            </div>
          </button>
        </nav>
      </div>

      {activeTab === 'yoomoney' && (
        <div className="bg-secondary rounded-lg p-6">
          <div className="mb-6">
            <div className="flex items-center gap-3 mb-4">
              <Wallet className="w-6 h-6 text-accent" />
              <h2 className="text-2xl font-bold text-white">Настройки ЮMoney</h2>
            </div>
            <p className="text-gray-400">
              Настройте кошелек ЮMoney для приема платежей. Получите секретный ключ в настройках уведомлений вашего кошелька.
            </p>
            {yooMoneySettings?.isConfigured && (
              <div className="mt-4 flex items-center gap-2 text-green-500">
                <CheckCircle className="w-5 h-5" />
                <span className="font-semibold">Настроено</span>
              </div>
            )}
          </div>

          <div className="space-y-6">
            <div>
              <label className="block text-gray-300 mb-2 font-semibold">
                Номер кошелька ЮMoney <span className="text-red-500">*</span>
              </label>
              <input
                type="text"
                value={yooMoneyForm.walletNumber}
                onChange={(e) => setYooMoneyForm({ ...yooMoneyForm, walletNumber: e.target.value })}
                placeholder="410012345678901"
                className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
              />
              <p className="text-sm text-gray-500 mt-1">Номер вашего кошелька для приема платежей</p>
            </div>

            <div>
              <label className="block text-gray-300 mb-2 font-semibold">
                Секретный ключ (Secret Key) <span className="text-red-500">*</span>
              </label>
              <input
                type="password"
                value={yooMoneyForm.secretKey}
                onChange={(e) => setYooMoneyForm({ ...yooMoneyForm, secretKey: e.target.value })}
                placeholder="Секретный ключ для проверки уведомлений"
                className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
              />
              <p className="text-sm text-gray-500 mt-1">
                Используется для проверки подлинности уведомлений от ЮMoney
              </p>
            </div>

            <div className="bg-blue-500/10 border border-blue-500/50 rounded-lg p-4">
              <h3 className="font-semibold text-blue-400 mb-2 flex items-center gap-2">
                <FileText className="w-4 h-4" /> URL для уведомлений
              </h3>
              <p className="text-sm text-gray-300 mb-2">
                Укажите этот URL в настройках HTTP-уведомлений вашего кошелька ЮMoney:
              </p>
              <code className="block bg-primary px-4 py-2 rounded text-sm text-green-400 break-all">
                {`${API_URL}/YooMoneyWebhook/notification`}
              </code>
            </div>

            <button
              onClick={handleYooMoneySave}
              disabled={updateYooMoneyMutation.isPending}
              className="w-full bg-accent hover:bg-accent-dark text-white font-bold py-3 px-6 rounded-lg transition-colors disabled:opacity-50 flex items-center justify-center gap-2"
            >
              <Save className="w-5 h-5" />
              {updateYooMoneyMutation.isPending ? 'Сохранение...' : 'Сохранить настройки'}
            </button>
          </div>
        </div>
      )}

      {activeTab === 'sourcebans' && (
        <div className="space-y-6">
          <div className="bg-secondary rounded-lg p-6">
            <div className="flex items-center justify-between mb-6">
              <div>
                <div className="flex items-center gap-3 mb-2">
                  <Database className="w-6 h-6 text-accent" />
                  <h2 className="text-2xl font-bold text-white">Настройки SourceBans</h2>
                </div>
                <p className="text-gray-400">
                  Настройте подключение к базам данных SourceBans для каждого сервера отдельно.
                </p>
              </div>
              {!showSourceBansForm && (
                <button
                  onClick={handleAddNewSourceBans}
                  className="bg-accent hover:bg-accent-dark text-white font-bold py-2 px-4 rounded-lg transition-colors flex items-center gap-2"
                >
                  <Plus className="w-5 h-5" />
                  Добавить сервер
                </button>
              )}
            </div>

            {!showSourceBansForm && allSourceBansSettings && allSourceBansSettings.length > 0 && (
              <div className="space-y-4">
                {allSourceBansSettings.map((settings) => (
                  <div
                    key={settings.serverId}
                    className="bg-primary border border-gray-700 rounded-lg p-6"
                  >
                    <div className="flex items-start justify-between mb-4">
                      <div className="flex items-center gap-3">
                        <ServerIcon className="w-6 h-6 text-accent" />
                        <div>
                          <h3 className="text-xl font-bold text-white">{settings.serverName}</h3>
                          <div className="flex items-center gap-2 mt-1">
                            <CheckCircle className="w-4 h-4 text-green-500" />
                            <span className="text-sm text-green-500">Настроено</span>
                          </div>
                        </div>
                      </div>
                      <div className="flex items-center gap-2">
                        <button
                          onClick={() => testSourceBansMutation.mutate(settings.serverId)}
                          disabled={testSourceBansMutation.isPending}
                          className="bg-blue-600 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-lg transition-colors disabled:opacity-50 flex items-center gap-2"
                        >
                          <CheckCircle className="w-4 h-4" />
                          Тест
                        </button>
                        <button
                          onClick={() => handleEditSourceBans(settings)}
                          className="bg-gray-600 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded-lg transition-colors"
                        >
                          Изменить
                        </button>
                        <button
                          onClick={() => handleDeleteSourceBans(settings.serverId)}
                          className="bg-red-600 hover:bg-red-700 text-white font-bold py-2 px-4 rounded-lg transition-colors flex items-center gap-2"
                        >
                          <Trash2 className="w-4 h-4" />
                          Удалить
                        </button>
                      </div>
                    </div>
                    <div className="grid grid-cols-2 md:grid-cols-4 gap-4 text-sm">
                      <div>
                        <span className="text-gray-400">Хост:</span>
                        <p className="text-white font-mono">{settings.host}:{settings.port}</p>
                      </div>
                      <div>
                        <span className="text-gray-400">База данных:</span>
                        <p className="text-white font-mono">{settings.database}</p>
                      </div>
                      <div>
                        <span className="text-gray-400">Пользователь:</span>
                        <p className="text-white font-mono">{settings.username}</p>
                      </div>
                      <div>
                        <span className="text-gray-400">Создано:</span>
                        <p className="text-white">{formatServerDate(settings.createdAt)}</p>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            )}

            {!showSourceBansForm && (!allSourceBansSettings || allSourceBansSettings.length === 0) && (
              <div className="text-center py-12 text-gray-500">
                <Database className="w-16 h-16 mx-auto mb-3 opacity-30" />
                <p>Ещё нет настроенных серверов SourceBans</p>
                <p className="text-sm mt-2">Нажмите "Добавить сервер" для начала</p>
              </div>
            )}

            {showSourceBansForm && (
              <div className="bg-primary border-2 border-accent rounded-lg p-6">
                <h3 className="text-xl font-bold text-white mb-6">
                  {sourceBansForm.serverId ? 'Редактировать' : 'Добавить'} настройки SourceBans
                </h3>

                <div className="space-y-6">
                  <div>
                    <label className="block text-gray-300 mb-2 font-semibold">
                      Сервер <span className="text-red-500">*</span>
                    </label>
                    <select
                      value={sourceBansForm.serverId || ''}
                      onChange={(e) => setSourceBansForm({ ...sourceBansForm, serverId: parseInt(e.target.value) })}
                      disabled={!!sourceBansForm.serverId && sourceBansForm.host !== '' && sourceBansForm.database !== ''}
                      className="w-full bg-secondary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent disabled:opacity-50"
                    >
                      <option value="">Выберите сервер</option>
                      {sourceBansForm.serverId && sourceBansForm.host !== '' ? (
                        <option value={sourceBansForm.serverId}>
                          {allSourceBansSettings?.find(s => s.serverId === sourceBansForm.serverId)?.serverName}
                        </option>
                      ) : (
                        getAvailableServersForVip().map((server: Server) => (
                          <option key={server.id} value={server.id}>
                            {server.name} ({server.ipAddress})
                          </option>
                        ))
                      )}
                    </select>
                    <p className="text-sm text-gray-500 mt-1">
                      {sourceBansForm.serverId && sourceBansForm.host !== '' 
                        ? 'Сервер нельзя изменить при редактировании' 
                        : 'Выберите сервер для настройки SourceBans'}
                    </p>
                  </div>

                  <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                    <div className="md:col-span-2">
                      <label className="block text-gray-300 mb-2 font-semibold">
                        Хост базы данных <span className="text-red-500">*</span>
                      </label>
                      <input
                        type="text"
                        value={sourceBansForm.host}
                        onChange={(e) => setSourceBansForm({ ...sourceBansForm, host: e.target.value })}
                        placeholder="localhost или IP адрес"
                        className="w-full bg-secondary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                      />
                    </div>

                    <div>
                      <label className="block text-gray-300 mb-2 font-semibold">
                        Порт <span className="text-red-500">*</span>
                      </label>
                      <input
                        type="number"
                        value={sourceBansForm.port}
                        onChange={(e) => setSourceBansForm({ ...sourceBansForm, port: parseInt(e.target.value) })}
                        placeholder="3306"
                        className="w-full bg-secondary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                      />
                    </div>
                  </div>

                  <div>
                    <label className="block text-gray-300 mb-2 font-semibold">
                      Название базы данных <span className="text-red-500">*</span>
                    </label>
                    <input
                      type="text"
                      value={sourceBansForm.database}
                      onChange={(e) => setSourceBansForm({ ...sourceBansForm, database: e.target.value })}
                      placeholder="sourcebans"
                      className="w-full bg-secondary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                    />
                  </div>

                  <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                    <div>
                      <label className="block text-gray-300 mb-2 font-semibold">
                        Имя пользователя <span className="text-red-500">*</span>
                      </label>
                      <input
                        type="text"
                        value={sourceBansForm.username}
                        onChange={(e) => setSourceBansForm({ ...sourceBansForm, username: e.target.value })}
                        placeholder="root"
                        className="w-full bg-secondary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                      />
                    </div>

                    <div>
                      <label className="block text-gray-300 mb-2 font-semibold">
                        Пароль <span className="text-red-500">*</span>
                      </label>
                      <input
                        type="password"
                        value={sourceBansForm.password}
                        onChange={(e) => setSourceBansForm({ ...sourceBansForm, password: e.target.value })}
                        placeholder="••••••••"
                        className="w-full bg-secondary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                      />
                    </div>
                  </div>

                  <div className="flex gap-4">
                    <button
                      onClick={handleSourceBansSave}
                      disabled={updateSourceBansMutation.isPending}
                      className="flex-1 bg-accent hover:bg-accent-dark text-white font-bold py-3 px-6 rounded-lg transition-colors disabled:opacity-50 flex items-center justify-center gap-2"
                    >
                      <Save className="w-5 h-5" />
                      {updateSourceBansMutation.isPending ? 'Сохранение...' : 'Сохранить'}
                    </button>
                    <button
                      onClick={() => {
                        setShowSourceBansForm(false);
                        setSourceBansForm({
                          serverId: undefined,
                          host: '',
                          port: 3306,
                          database: '',
                          username: '',
                          password: '',
                        });
                      }}
                      className="flex-1 bg-gray-600 hover:bg-gray-700 text-white font-bold py-3 px-6 rounded-lg transition-colors"
                    >
                      Отмена
                    </button>
                  </div>
                </div>
              </div>
            )}
          </div>
        </div>
      )}

      {activeTab === 'vip' && (
        <div className="space-y-6">
          <div className="bg-secondary rounded-lg p-6">
            <div className="flex items-center justify-between mb-6">
              <div>
                <div className="flex items-center gap-3 mb-2">
                  <ServerIcon className="w-6 h-6 text-accent" />
                  <h2 className="text-2xl font-bold text-white">Настройки VIP</h2>
                </div>
                <p className="text-gray-400">
                  Настройте подключение к базам данных VIP для каждого сервера отдельно.
                </p>
              </div>
              {!showVipForm && (
                <button
                  onClick={handleAddNewVip}
                  className="bg-accent hover:bg-accent-dark text-white font-bold py-2 px-4 rounded-lg transition-colors flex items-center gap-2"
                >
                  <Plus className="w-5 h-5" />
                  Добавить сервер
                </button>
              )}
            </div>

            {!showVipForm && allVipSettings && allVipSettings.length > 0 && (
              <div className="space-y-4">
                {allVipSettings.map((settings) => (
                  <div
                    key={settings.serverId}
                    className="bg-primary border border-gray-700 rounded-lg p-6"
                  >
                    <div className="flex items-start justify-between mb-4">
                      <div className="flex items-center gap-3">
                        <ServerIcon className="w-6 h-6 text-accent" />
                        <div>
                          <h3 className="text-xl font-bold text-white">{settings.serverName}</h3>
                          <div className="flex items-center gap-2 mt-1">
                            <CheckCircle className="w-4 h-4 text-green-500" />
                            <span className="text-sm text-green-500">Настроено</span>
                          </div>
                        </div>
                      </div>
                      <div className="flex items-center gap-2">
                        <button
                          onClick={() => testVipMutation.mutate(settings.serverId)}
                          disabled={testVipMutation.isPending}
                          className="bg-blue-600 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded-lg transition-colors disabled:opacity-50 flex items-center gap-2"
                        >
                          <CheckCircle className="w-4 h-4" />
                          Тест
                        </button>
                        <button
                          onClick={() => handleEditVip(settings)}
                          className="bg-gray-600 hover:bg-gray-700 text-white font-bold py-2 px-4 rounded-lg transition-colors"
                        >
                          Изменить
                        </button>
                        <button
                          onClick={() => handleDeleteVip(settings.serverId)}
                          className="bg-red-600 hover:bg-red-700 text-white font-bold py-2 px-4 rounded-lg transition-colors flex items-center gap-2"
                        >
                          <Trash2 className="w-4 h-4" />
                          Удалить
                        </button>
                      </div>
                    </div>
                    <div className="grid grid-cols-2 md:grid-cols-4 gap-4 text-sm">
                      <div>
                        <span className="text-gray-400">Хост:</span>
                        <p className="text-white font-mono">{settings.host}:{settings.port}</p>
                      </div>
                      <div>
                        <span className="text-gray-400">База данных:</span>
                        <p className="text-white font-mono">{settings.database}</p>
                      </div>
                      <div>
                        <span className="text-gray-400">Пользователь:</span>
                        <p className="text-white font-mono">{settings.username}</p>
                      </div>
                      <div>
                        <span className="text-gray-400">Создано:</span>
                        <p className="text-white">{formatServerDate(settings.createdAt)}</p>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            )}

            {!showVipForm && (!allVipSettings || allVipSettings.length === 0) && (
              <div className="text-center py-12 text-gray-500">
                <ServerIcon className="w-16 h-16 mx-auto mb-3 opacity-30" />
                <p>Ещё нет настроенных серверов VIP</p>
                <p className="text-sm mt-2">Нажмите "Добавить сервер" для начала</p>
              </div>
            )}

            {showVipForm && (
              <div className="bg-primary border-2 border-accent rounded-lg p-6">
                <h3 className="text-xl font-bold text-white mb-6">
                  {vipForm.serverId ? 'Редактировать' : 'Добавить'} настройки VIP
                </h3>

                <div className="space-y-6">
                  <div>
                    <label className="block text-gray-300 mb-2 font-semibold">
                      Сервер <span className="text-red-500">*</span>
                    </label>
                    <select
                      value={vipForm.serverId || ''}
                      onChange={(e) => setVipForm({ ...vipForm, serverId: parseInt(e.target.value) })}
                      disabled={!!vipForm.serverId && vipForm.host !== '' && vipForm.database !== ''}
                      className="w-full bg-secondary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent disabled:opacity-50"
                    >
                      <option value="">Выберите сервер</option>
                      {vipForm.serverId && vipForm.host !== '' ? (
                        <option value={vipForm.serverId}>
                          {allVipSettings?.find(s => s.serverId === vipForm.serverId)?.serverName}
                        </option>
                      ) : (
                        getAvailableServersForVip().map((server) => (
                          <option key={server.id} value={server.id}>
                            {server.name} ({server.ipAddress})
                          </option>
                        ))
                      )}
                    </select>
                    <p className="text-sm text-gray-500 mt-1">
                      {vipForm.serverId && vipForm.host !== '' 
                        ? 'Сервер нельзя изменить при редактировании' 
                        : 'Выберите сервер для настройки VIP'}
                    </p>
                  </div>

                  <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                    <div className="md:col-span-2">
                      <label className="block text-gray-300 mb-2 font-semibold">
                        Хост базы данных <span className="text-red-500">*</span>
                      </label>
                      <input
                        type="text"
                        value={vipForm.host}
                        onChange={(e) => setVipForm({ ...vipForm, host: e.target.value })}
                        placeholder="localhost или IP адрес"
                        className="w-full bg-secondary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                      />
                    </div>

                    <div>
                      <label className="block text-gray-300 mb-2 font-semibold">
                        Порт <span className="text-red-500">*</span>
                      </label>
                      <input
                        type="number"
                        value={vipForm.port}
                        onChange={(e) => setVipForm({ ...vipForm, port: parseInt(e.target.value) })}
                        placeholder="3306"
                        className="w-full bg-secondary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                      />
                    </div>
                  </div>

                  <div>
                    <label className="block text-gray-300 mb-2 font-semibold">
                      Название базы данных <span className="text-red-500">*</span>
                    </label>
                    <input
                      type="text"
                      value={vipForm.database}
                      onChange={(e) => setVipForm({ ...vipForm, database: e.target.value })}
                      placeholder="sourcebans"
                      className="w-full bg-secondary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                    />
                  </div>

                  <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                    <div>
                      <label className="block text-gray-300 mb-2 font-semibold">
                        Имя пользователя <span className="text-red-500">*</span>
                      </label>
                      <input
                        type="text"
                        value={vipForm.username}
                        onChange={(e) => setVipForm({ ...vipForm, username: e.target.value })}
                        placeholder="root"
                        className="w-full bg-secondary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                      />
                    </div>

                    <div>
                      <label className="block text-gray-300 mb-2 font-semibold">
                        Пароль <span className="text-red-500">*</span>
                      </label>
                      <input
                        type="password"
                        value={vipForm.password}
                        onChange={(e) => setVipForm({ ...vipForm, password: e.target.value })}
                        placeholder="••••••••"
                        className="w-full bg-secondary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                      />
                    </div>
                  </div>

                  <div className="flex gap-4">
                    <button
                      onClick={handleVipSave}
                      disabled={updateVipMutation.isPending}
                      className="flex-1 bg-accent hover:bg-accent-dark text-white font-bold py-3 px-6 rounded-lg transition-colors disabled:opacity-50 flex items-center justify-center gap-2"
                    >
                      <Save className="w-5 h-5" />
                      {updateVipMutation.isPending ? 'Сохранение...' : 'Сохранить'}
                    </button>
                    <button
                      onClick={() => {
                        setShowVipForm(false);
                        setVipForm({
                          serverId: undefined,
                          host: '',
                          port: 3306,
                          database: '',
                          username: '',
                          password: '',
                        });
                      }}
                      className="flex-1 bg-gray-600 hover:bg-gray-700 text-white font-bold py-3 px-6 rounded-lg transition-colors"
                    >
                      Отмена
                    </button>
                  </div>
                </div>
              </div>
            )}
          </div>
        </div>
      )}

      {activeTab === 'package' && (
        <div className="bg-secondary rounded-lg p-6">
          <div className="mb-6">
            <div className="flex items-center gap-3 mb-4">
              <Gift className="w-6 h-6 text-accent" />
              <h2 className="text-2xl font-bold text-white">Настройки пакета доната</h2>
            </div>
            <p className="text-gray-400">
              Настройте описание и предложенные суммы для пожертвований на проект.
            </p>
          </div>

          <div className="space-y-6">
            <div>
              <label className="block text-gray-300 mb-2 font-semibold">
                Заголовок <span className="text-red-500">*</span>
              </label>
              <input
                type="text"
                value={packageForm.title}
                onChange={(e) => setPackageForm({ ...packageForm, title: e.target.value })}
                placeholder="Поддержите наш проект"
                className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
              />
            </div>

            <div>
              <label className="block text-gray-300 mb-2 font-semibold">
                Описание <span className="text-red-500">*</span>
              </label>
              <textarea
                value={packageForm.description}
                onChange={(e) => setPackageForm({ ...packageForm, description: e.target.value })}
                placeholder="Ваша поддержка помогает нам развивать серверы..."
                rows={4}
                className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent resize-none"
              />
            </div>

            <div>
              <label className="block text-gray-300 mb-2 font-semibold">
                Предложенные суммы (через запятую)
              </label>
              <input
                type="text"
                value={suggestedAmountsInput}
                onChange={(e) => setSuggestedAmountsInput(e.target.value)}
                placeholder="100, 500, 1000, 5000"
                className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
              />
              <p className="text-sm text-gray-500 mt-1">
                Укажите суммы в рублях, которые будут предложены пользователям для быстрого выбора
              </p>
            </div>

            <div className="flex items-center gap-3">
              <input
                type="checkbox"
                id="isActive"
                checked={packageForm.isActive}
                onChange={(e) => setPackageForm({ ...packageForm, isActive: e.target.checked })}
                className="w-5 h-5 bg-primary border border-gray-700 rounded focus:ring-2 focus:ring-accent"
              />
              <label htmlFor="isActive" className="text-gray-300 font-semibold cursor-pointer">
                Активировать пакет доната
              </label>
            </div>

            <button
              onClick={handlePackageSave}
              disabled={updatePackageMutation.isPending}
              className="w-full bg-accent hover:bg-accent-dark text-white font-bold py-3 px-6 rounded-lg transition-colors disabled:opacity-50 flex items-center justify-center gap-2"
            >
              <Save className="w-5 h-5" />
              {updatePackageMutation.isPending ? 'Сохранение...' : 'Сохранить настройки'}
            </button>
          </div>
        </div>
      )}
    </div>
  );
}
