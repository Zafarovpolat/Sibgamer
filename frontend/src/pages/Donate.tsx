import { useQuery } from '@tanstack/react-query';
import { useState } from 'react';
import { usePageTitle } from '../hooks/usePageTitle';
import { getDonationSettings, getTariffsByServers, getMyPrivileges, createDonation, createAdminPurchase, getTopDonators, getVipTariffsByServers, getMyVipPrivileges, createVipPurchase } from '../lib/donationApi';
import { useAuthStore } from '../store/authStore';
import toast from 'react-hot-toast';
import { getErrorMessage } from '../utils/errorUtils';
import { Trophy, Crown, Server as ServerIcon, Clock, DollarSign, Shield, Gift, Heart, Key, X, Eye, EyeOff } from 'lucide-react';
import { isForeverDate } from '../utils/dateUtils';

export default function Donate() {
  usePageTitle('Донат');
  
  const { isAuthenticated } = useAuthStore();
  const [selectedServerId, setSelectedServerId] = useState<number | null>(null);
  const [customAmount, setCustomAmount] = useState('');
  const [isDonating, setIsDonating] = useState(false);
  const [selectedTariffOptions, setSelectedTariffOptions] = useState<Record<number, number>>({});
  const [showPasswordModal, setShowPasswordModal] = useState(false);
  const [selectedTariffForPurchase, setSelectedTariffForPurchase] = useState<{ optionId: number; tariffName: string; serverName: string } | null>(null);
  const [adminPassword, setAdminPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [selectedVipServerId, setSelectedVipServerId] = useState<number | null>(null);
  const [selectedVipTariffOptions, setSelectedVipTariffOptions] = useState<Record<number, number>>({});

  const { data: settings } = useQuery({
    queryKey: ['donationSettings'],
    queryFn: getDonationSettings,
  });

  const { data: serversWithTariffs } = useQuery({
    queryKey: ['serverTariffs'],
    queryFn: getTariffsByServers,
  });

  const { data: myPrivileges } = useQuery({
    queryKey: ['myPrivileges'],
    queryFn: getMyPrivileges,
    enabled: isAuthenticated,
  });

  const { data: vipServersWithTariffs } = useQuery({
    queryKey: ['vipServerTariffs'],
    queryFn: getVipTariffsByServers,
  });

  const { data: myVipPrivileges } = useQuery({
    queryKey: ['myVipPrivileges'],
    queryFn: getMyVipPrivileges,
    enabled: isAuthenticated,
  });

  const { data: topDonators } = useQuery({
    queryKey: ['topDonators'],
    queryFn: () => getTopDonators(3),
  });

  const selectedServer = serversWithTariffs?.find(s => s.serverId === selectedServerId);
  const selectedVipServer = vipServersWithTariffs?.find(s => s.serverId === selectedVipServerId);

  const handleDonation = async (amount: number) => {
    if (!isAuthenticated) {
      toast.error('Необходимо авторизоваться');
      return;
    }
    setIsDonating(true);
    try {
      const response = await createDonation({ amount });
      window.open(response.paymentUrl, '_blank');
      toast.success('Платеж создан! Завершите оплату в новой вкладке.');
    } catch (error: unknown) {
      toast.error(getErrorMessage(error) || 'Ошибка при создании платежа');
    } finally {
      setIsDonating(false);
    }
  };

  const handleAdminPurchase = (tariffOptionId: number) => {
    if (!selectedServerId) {
      toast.error('Выберите сервер');
      return;
    }

    const selectedServer = serversWithTariffs?.find(s => s.serverId === selectedServerId);
    if (!selectedServer) {
      toast.error('Сервер не найден');
      return;
    }

    const tariff = selectedServer.tariffs.find(t => t.options?.some(o => o.id === tariffOptionId));
    if (!tariff) {
      toast.error('Тариф не найден');
      return;
    }

    setSelectedTariffForPurchase({
      optionId: tariffOptionId,
      tariffName: tariff.name,
      serverName: selectedServer.serverName
    });
    setAdminPassword('');
    setShowPasswordModal(true);
  };

  const attemptAdminPurchase = (tariffOptionId: number) => {
    if (selectedServerId) {
      const hasActiveAdmin = myPrivileges?.some(p => p.serverId === selectedServerId && p.isActive);
      if (hasActiveAdmin) {
        toast.error('У вас уже есть активная админ-привилегия на этом сервере. Вы сможете купить новую только после окончания текущей услуги.');
        return;
      }
    }

    handleAdminPurchase(tariffOptionId);
  };

  const handleConfirmAdminPurchase = async () => {
    if (!selectedTariffForPurchase || !adminPassword.trim()) {
      toast.error('Введите пароль');
      return;
    }

    if (adminPassword.length < 4) {
      toast.error('Пароль должен содержать минимум 4 символа');
      return;
    }

    setIsDonating(true);
    setShowPasswordModal(false);

    try {
      const response = await createAdminPurchase({ 
        tariffOptionId: selectedTariffForPurchase.optionId,
        serverId: selectedServerId!,
        adminPassword: adminPassword.trim()
      });
      window.open(response.paymentUrl, '_blank');
      toast.success('Платеж создан! Завершите оплату в новой вкладке.');
    } catch (error: unknown) {
      toast.error(getErrorMessage(error) || 'Ошибка при создании платежа');
    } finally {
      setIsDonating(false);
      setSelectedTariffForPurchase(null);
    }
  };

  const handleVipPurchase = async (tariffOptionId: number) => {
    if (!selectedVipServerId) {
      toast.error('Выберите сервер');
      return;
    }

    const selectedServer = vipServersWithTariffs?.find(s => s.serverId === selectedVipServerId);
    if (!selectedServer) {
      toast.error('Сервер не найден');
      return;
    }

    const tariff = selectedServer.tariffs.find(t => t.options?.some(o => o.id === tariffOptionId));
    if (!tariff) {
      toast.error('Тариф не найден');
      return;
    }

    setIsDonating(true);

    try {
      const response = await createVipPurchase({ 
        tariffOptionId: tariffOptionId,
        serverId: selectedVipServerId!
      });
      window.open(response.paymentUrl, '_blank');
      toast.success('Платеж создан! Завершите оплату в новой вкладке.');
    } catch (error: unknown) {
      toast.error(getErrorMessage(error) || 'Ошибка при создании платежа');
    } finally {
      setIsDonating(false);
    }
  };

  const attemptVipPurchase = (tariffOptionId: number) => {
    if (selectedVipServerId) {
      const hasActiveVip = myVipPrivileges?.some(p => p.serverId === selectedVipServerId && p.isActive);
      if (hasActiveVip) {
        toast.error('У вас уже есть активная VIP-привилегия на этом сервере. Вы сможете купить новую только после окончания текущей услуги.');
        return;
      }
    }

    void handleVipPurchase(tariffOptionId);
  };

  const suggestedAmounts = settings?.donationPackage?.suggestedAmounts || [];

  return (
    <div className="min-h-screen flex flex-col">
      <div className="flex-1">
        <div className="container mx-auto px-4 py-8">
          
          <div>
            <div className="block lg:hidden text-center px-4">
              <h1 className="text-3xl md:text-4xl font-bold mb-4 text-white flex items-center justify-center gap-3">
                <Heart className="w-10 h-10 text-red-500" />
                Поддержать проект
              </h1>
              <p className="text-base md:text-lg text-gray-300 max-w-md mx-auto">
                {settings?.donationPackage?.description || 'Ваша поддержка помогает развивать серверы и делать их лучше!'}
              </p>
            </div>

            <div className="hidden lg:flex relative min-h-[400px] items-center justify-center">
              <div className="relative z-10 text-center">
                <h1 className="text-6xl md:text-7xl font-bold mb-6 text-white drop-shadow-2xl flex items-center justify-center gap-4">
                  <Heart className="w-20 h-20 text-red-500" />
                  Поддержать проект
                </h1>
                <p className="text-xl md:text-2xl text-gray-200 drop-shadow-lg leading-relaxed">
                  {settings?.donationPackage?.description || 'Ваша поддержка помогает развивать серверы и делать их лучше!'}
                </p>
              </div>
            </div>
          </div>

          <div className="rounded-2xl border border-gray-700/60 p-8 mb-16 relative overflow-hidden">
            <div className="absolute inset-0 bg-gradient-to-br from-gray-900/20 via-transparent to-gray-800/10 rounded-2xl"></div>

            <div className="relative grid grid-cols-1 lg:grid-cols-3 gap-8 lg:gap-12">
              <div className="lg:col-span-2">
                <div className="mb-8">
                  <h2 className="text-3xl font-bold text-white mb-3 flex items-center gap-3">
                    <div className="p-2 rounded-xl bg-highlight/10 border border-highlight/20">
                      <Gift className="w-6 h-6 text-highlight" />
                    </div>
                    Сделать пожертвование
                  </h2>
                 
                </div>

                <div className="space-y-6">
                  <div>
                    <p className="text-gray-300 mb-4 font-medium">Выберите сумму или введите свою:</p>
                    <div className="grid grid-cols-2 md:grid-cols-4 gap-3">
                      {suggestedAmounts.map((amount) => (
                        <button
                          key={amount}
                          onClick={() => handleDonation(amount)}
                          disabled={isDonating}
                          className="group bg-gray-800/30 hover:bg-highlight/20 border border-gray-600/50 hover:border-highlight/60 text-white font-semibold py-4 px-6 rounded-xl transition-all duration-200 hover:shadow-lg hover:shadow-highlight/10 disabled:opacity-50 disabled:cursor-not-allowed"
                        >
                          <span className="text-lg">{amount} ₽</span>
                        </button>
                      ))}
                    </div>
                  </div>

                  <div className="space-y-3">
                    <label className="block text-gray-300 font-medium">Другая сумма:</label>
                    <div className="flex gap-4">
                      <div className="relative flex-1">
                        <div className="absolute left-4 top-1/2 transform -translate-y-1/2 p-1 rounded-lg bg-gray-700/50">
                          <DollarSign className="w-4 h-4 text-gray-400" />
                        </div>
                        <input
                          type="number"
                          min="1"
                          step="1"
                          value={customAmount}
                          onChange={(e) => setCustomAmount(e.target.value)}
                          placeholder="Введите сумму"
                          className="w-full bg-gray-800/30 border border-gray-600/50 rounded-xl pl-12 pr-4 py-4 text-white placeholder-gray-500 focus:outline-none focus:border-highlight/60 focus:bg-gray-800/50 transition-all duration-200 text-lg"
                        />
                      </div>
                      <button
                        onClick={() => handleDonation(parseFloat(customAmount))}
                        disabled={isDonating || !customAmount || parseFloat(customAmount) < 1}
                        className="bg-gradient-to-r from-highlight to-blue-500 hover:from-blue-500 hover:to-highlight text-white font-bold py-4 px-8 rounded-xl transition-all duration-200 hover:shadow-lg hover:shadow-highlight/20 disabled:opacity-50 disabled:cursor-not-allowed disabled:transform-none"
                      >
                        Отправить
                      </button>
                    </div>
                  </div>
                </div>
              </div>

              <div className="lg:col-span-1">
                <div className="mb-8">
                  <h2 className="text-2xl font-bold text-white mb-3 flex items-center gap-3">
                    <div className="p-2 rounded-xl bg-blue-500/10 border border-blue-500/20">
                      <Trophy className="w-5 h-5 text-blue-500" />
                    </div>
                    Топ донатеры
                  </h2>
                  <p className="text-gray-400 text-sm leading-relaxed">
                    Самые щедрые поддержатели проекта
                  </p>
                </div>

                {topDonators && topDonators.length > 0 ? (
                  <div className="space-y-3">
                    {topDonators.map((donator, index) => (
                      <div
                        key={donator.userId}
                        className={`flex items-center gap-3 p-4 rounded-xl border backdrop-blur-sm transition-all duration-300 ${
                          index === 0 ? 'bg-gradient-to-r from-blue-500/10 via-blue-500/5 to-transparent border-blue-500/30 shadow-lg shadow-blue-500/10' :
                          index === 1 ? 'bg-gradient-to-r from-gray-400/10 via-gray-400/5 to-transparent border-gray-400/30 shadow-lg shadow-gray-400/10' :
                          index === 2 ? 'bg-gradient-to-r from-blue-600/10 via-blue-600/5 to-transparent border-blue-600/30 shadow-lg shadow-blue-600/10' :
                          'bg-gray-800/40 border-gray-700/50'
                        }`}
                      >
                        <div className="flex-shrink-0">
                          <div className={`w-10 h-10 rounded-xl flex items-center justify-center font-bold text-sm transition-all duration-300 ${
                            index === 0 ? 'bg-gradient-to-br from-blue-400 via-blue-500 to-blue-600 text-white shadow-lg shadow-blue-500/30 border border-blue-400/50' :
                            index === 1 ? 'bg-gradient-to-br from-gray-300 via-gray-400 to-gray-500 text-black shadow-lg shadow-gray-400/30 border border-gray-300/50' :
                            index === 2 ? 'bg-gradient-to-br from-blue-500 via-blue-600 to-blue-700 text-white shadow-lg shadow-blue-600/30 border border-blue-500/50' :
                            'bg-gradient-to-br from-gray-600 to-gray-700 text-white shadow-lg shadow-gray-600/20 border border-gray-500/30'
                          }`}>
                            {index + 1}
                          </div>
                        </div>
                        <div className="flex-1 min-w-0">
                          <div className="flex items-center gap-2">
                            <span className="font-semibold text-white truncate">{donator.username}</span>
                            {index === 0 && <Crown className="w-3.5 h-3.5 text-blue-500 flex-shrink-0" />}
                          </div>
                          <div className="text-xs text-gray-400">
                            {donator.donationCount} {donator.donationCount === 1 ? 'донат' : 'донатов'}
                          </div>
                        </div>
                        <div className="text-right flex-shrink-0">
                          <div className="text-highlight font-bold">{donator.totalAmount.toFixed(0)} ₽</div>
                        </div>
                      </div>
                    ))}
                  </div>
                ) : (
                  <div className="text-center py-12">
                    <div className="w-16 h-16 mx-auto mb-4 rounded-2xl bg-gray-800/30 flex items-center justify-center">
                      <Trophy className="w-8 h-8 text-gray-600" />
                    </div>
                    <p className="text-gray-500 text-sm">Пока нет донатов</p>
                  </div>
                )}
              </div>
            </div>
          </div>

          <div className="mb-16">
            <div className="h-px bg-gradient-to-r from-transparent via-gray-800 to-transparent"></div>
          </div>

          {serversWithTariffs && serversWithTariffs.length > 0 && (
            <div className="rounded-2xl border border-gray-700/60 p-8 mb-16 relative overflow-hidden">
              <div className="absolute inset-0 bg-gradient-to-br from-gray-900/20 via-transparent to-gray-800/10 rounded-2xl"></div>

              <div className="relative">
                <div className="mb-8">
                  <h2 className="text-3xl font-bold text-white mb-3 flex items-center gap-3">
                    <div className="p-2 rounded-xl bg-purple-500/10 border border-purple-500/20">
                      <Shield className="w-6 h-6 text-purple-500" />
                    </div>
                    Админские права
                  </h2>
                  <p className="text-gray-400 text-lg leading-relaxed">
                    Получите доступ к админским командам и управлению сервером
                  </p>
                </div>

              <div className="mb-8">
                <h3 className="text-2xl font-bold text-white mb-6 flex items-center gap-3">
                  <ServerIcon className="w-5 h-5 text-highlight" />
                  Выберите сервер
                </h3>
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                  {serversWithTariffs?.map((server) => (
                    <button
                      key={server.serverId}
                      onClick={() => setSelectedServerId(server.serverId)}
                      className={`group p-6 rounded-xl border transition-all duration-200 text-left hover:shadow-lg ${
                        selectedServerId === server.serverId
                          ? 'border-highlight bg-highlight/10 shadow-lg shadow-highlight/10'
                          : 'border-gray-700/50 bg-gray-800/30 hover:border-highlight/60 hover:bg-highlight/5 hover:shadow-lg hover:shadow-highlight/5'
                      }`}
                    >
                      <div className="flex items-start gap-3">
                        <div className={`p-2 rounded-lg transition-all duration-200 ${
                          selectedServerId === server.serverId
                            ? 'bg-highlight/20'
                            : 'bg-gray-700/30 group-hover:bg-highlight/10'
                        }`}>
                          <ServerIcon className={`w-5 h-5 ${
                            selectedServerId === server.serverId ? 'text-highlight' : 'text-gray-400 group-hover:text-highlight'
                          }`} />
                        </div>
                        <div className="flex-1">
                          <h4 className="text-white font-bold text-lg mb-1">{server.serverName}</h4>
                          <p className="text-gray-400 text-sm mb-2">{server.serverIp}</p>
                          <div className="flex items-center gap-2">
                            <div className={`px-2 py-1 rounded-md text-xs font-medium ${
                              selectedServerId === server.serverId
                                ? 'bg-highlight/20 text-highlight'
                                : 'bg-gray-700/50 text-gray-300'
                            }`}>
                              {server.tariffs.length} {server.tariffs.length === 1 ? 'тариф' : server.tariffs.length < 5 ? 'тарифа' : 'тарифов'}
                            </div>
                          </div>
                        </div>
                      </div>
                    </button>
                  ))}
                </div>
              </div>

              {selectedServer && (
                <div>
                  <div className="mb-8">
                    <h3 className="text-2xl font-bold text-white mb-3 flex items-center gap-3">
                      <Crown className="w-5 h-5 text-blue-500" />
                      Тарифы для {selectedServer.serverName}
                    </h3>
                    <div className="h-px bg-gradient-to-r from-transparent via-gray-700/50 to-transparent"></div>
                  </div>

                  {myPrivileges && myPrivileges.filter(p => p.serverId === selectedServerId && p.isActive).length > 0 && (
                    <div className="bg-gradient-to-r from-green-500/10 to-emerald-500/10 border border-green-500/30 rounded-xl p-6 mb-8 relative overflow-hidden">
                      <div className="absolute inset-0 bg-gradient-to-r from-green-500/5 to-transparent"></div>
                      <div className="relative flex items-start gap-3">
                        <div className="p-2 rounded-lg bg-green-500/20">
                          <Shield className="w-6 h-6 text-green-500" />
                        </div>
                        <div className="flex-1">
                          <h4 className="text-green-400 font-bold mb-3 flex items-center gap-2">
                            <div className="w-2 h-2 rounded-full bg-green-500"></div>
                            Ваши активные права
                          </h4>
                          <div className="space-y-2">
                            {myPrivileges
                              .filter(p => p.serverId === selectedServerId && p.isActive)
                              .map((priv) => (
                                <div key={priv.id} className="flex items-center justify-between p-3 bg-gray-800/30 rounded-lg border border-gray-700/30">
                                  <span className="text-gray-200 font-medium">{priv.tariffName}</span>
                                  <div className="flex items-center gap-2 text-sm text-gray-400">
                                    <Clock className="w-4 h-4" />
                                    {priv.expiresAt && isForeverDate(priv.expiresAt)
                                      ? 'Навсегда'
                                      : (priv.daysRemaining > 0
                                          ? `${priv.daysRemaining} ${priv.daysRemaining === 1 ? 'день' : priv.daysRemaining < 5 ? 'дня' : 'дней'}`
                                          : 'Истекла')
                                    }
                                  </div>
                                </div>
                              ))
                            }
                          </div>
                        </div>
                      </div>
                    </div>
                  )}

                  <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    {selectedServer.tariffs
                      .filter(tariff => tariff.isActive && tariff.options?.some(o => o.isActive))
                      .sort((a, b) => a.order - b.order)
                      .map((tariff) => {
                        const selectedOptionId = selectedTariffOptions[tariff.id] || tariff.options?.[0]?.id;
                        const selectedOption = tariff.options?.find(o => o.id === selectedOptionId);

                        return (
                          <div
                            key={tariff.id}
                            className="group rounded-xl bg-gray-800/30 backdrop-blur-md border border-gray-700/50 hover:border-highlight/60 p-6 transition-all duration-200 hover:shadow-lg hover:shadow-highlight/10 relative overflow-hidden flex flex-col min-h-[400px]"
                          >
                            <div className="absolute inset-0 bg-gradient-to-br from-gray-900/40 via-transparent to-gray-800/20 opacity-0 group-hover:opacity-100 transition-opacity duration-200"></div>

                            <div className="relative flex flex-col flex-1">
                              <div className="flex items-start justify-between mb-4">
                                <h4 className="text-xl font-bold text-white">{tariff.name}</h4>
                                {selectedOption && (
                                  <div className="text-right">
                                    <div className="text-2xl font-bold text-highlight">{selectedOption.price} ₽</div>
                                  </div>
                                )}
                              </div>

                              {tariff.description && (
                                <p className="text-gray-400 mb-4 text-sm leading-relaxed">{tariff.description}</p>
                              )}

                              <div className="mb-4">
                                <label className="text-gray-300 text-sm font-medium mb-3 flex items-center gap-2">
                                  <div className="p-1.5 rounded-md bg-highlight/10">
                                    <Clock className="w-4 h-4 text-highlight" />
                                  </div>
                                  Выберите срок:
                                </label>
                                <div className="grid grid-cols-2 gap-2">
                                  {tariff.options
                                    ?.filter(option => option.isActive)
                                    .sort((a, b) => a.order - b.order)
                                    .map(option => (
                                      <button
                                        key={option.id}
                                        onClick={() => setSelectedTariffOptions(prev => ({
                                          ...prev,
                                          [tariff.id]: option.id
                                        }))}
                                        className={`group p-3 rounded-lg border transition-all duration-200 text-left hover:shadow-md ${
                                          selectedOptionId === option.id
                                               ? 'text-blue-500' 
                                            : 'border-gray-600/50 bg-gray-700/30 hover:border-highlight/60 hover:bg-highlight/5 hover:shadow-lg hover:shadow-highlight/5'
                                        }`}
                                      >
                                        <div className="flex items-center justify-between">
                                          <div className="flex items-center gap-2">
                                            <div className={`p-1.5 rounded-md transition-all duration-200 ${
                                              selectedOptionId === option.id
                                                   ? 'text-blue-500' 
                                                : 'bg-gray-600/30 group-hover:bg-highlight/10'
                                            }`}>
                                              <Clock className={`w-3.5 h-3.5 ${
                                                selectedOptionId === option.id ? 'text-highlight' : 'text-gray-400 group-hover:text-highlight'
                                              }`} />
                                            </div>
                                            <span className={`text-sm font-medium transition-colors ${
                                              selectedOptionId === option.id ? 'text-white' : 'text-gray-300 group-hover:text-white'
                                            }`}>
                                              {option.durationDays === 0 
                                                ? 'Навсегда' 
                                                : `${option.durationDays} ${option.durationDays === 1 ? 'день' : option.durationDays < 5 ? 'дня' : 'дней'}`
                                              }
                                            </span>
                                          </div>
                                          <span className={`text-sm font-bold transition-colors ${
                                            selectedOptionId === option.id ? 'text-highlight' : 'text-gray-400 group-hover:text-highlight'
                                          }`}>
                                            {option.price} ₽
                                          </span>
                                        </div>
                                      </button>
                                    ))}
                                </div>
                              </div>

                              <div className="space-y-3 flex-1">
                                {selectedOption && (
                                  <div className="flex items-center gap-3 text-gray-300">
                                    <div className="p-1.5 rounded-md bg-highlight/10">
                                      <Clock className="w-4 h-4 text-highlight" />
                                    </div>
                                    <span className="text-sm">
                                      {selectedOption.durationDays === 0 
                                        ? 'Навсегда' 
                                        : `${selectedOption.durationDays} ${selectedOption.durationDays === 1 ? 'день' : selectedOption.durationDays < 5 ? 'дня' : 'дней'}`
                                    }
                                    </span>
                                  </div>
                                )}

                                {tariff.flags && (
                                  <div className="flex items-start gap-3 text-gray-300">
                                    <div className="p-1.5 rounded-md bg-purple-500/10">
                                      <Shield className="w-4 h-4 text-purple-500" />
                                    </div>
                                    <div className="flex-1">
                                      <div className="text-xs text-gray-500 mb-1">Флаги</div>
                                      <code className="text-xs bg-gray-700/50 px-2 py-1 rounded break-all">{tariff.flags}</code>
                                    </div>
                                  </div>
                                )}

                                {tariff.groupName && (
                                  <div className="flex items-center gap-3 text-gray-300">
                                    <div className="p-1.5 rounded-md bg-blue-500/10">
                                      <Crown className="w-4 h-4 text-blue-500" />
                                    </div>
                                    <div className="flex-1">
                                      <div className="text-xs text-gray-500 mb-1">Группа</div>
                                      <code className="text-xs bg-gray-700/50 px-2 py-1 rounded">{tariff.groupName}</code>
                                    </div>
                                  </div>
                                )}

                                {tariff.immunity > 0 && (
                                  <div className="flex items-center gap-3 text-gray-300">
                                    <div className="p-1.5 rounded-md bg-red-500/10">
                                      <Shield className="w-4 h-4 text-red-500" />
                                    </div>
                                    <span className="text-sm">Иммунитет: {tariff.immunity}</span>
                                  </div>
                                )}
                              </div>

                              <div className="mt-auto pt-4">
                                <button
                                  onClick={() => selectedOptionId && attemptAdminPurchase(selectedOptionId)}
                                  disabled={isDonating || !selectedOptionId}
                                  className="w-full bg-gradient-to-r from-highlight to-blue-500 hover:from-blue-500 hover:to-highlight text-white font-bold py-3 px-6 rounded-lg transition-all duration-200 hover:shadow-lg hover:shadow-highlight/20 disabled:opacity-50 disabled:cursor-not-allowed disabled:transform-none"
                                >
                                  Купить
                                </button>
                              </div>
                            </div>
                          </div>
                        );
                      })}
                  </div>

                  {selectedServer.tariffs.filter(t => t.isActive && t.options?.some(o => o.isActive)).length === 0 && (
                    <div className="text-center py-12 rounded-xl bg-gray-800/30 border border-gray-700/50">
                      <Shield className="w-16 h-16 mx-auto mb-4 text-gray-600" />
                      <p className="text-gray-400">Для этого сервера пока нет доступных тарифов</p>
                    </div>
                  )}
                </div>
              )}

              {(!serversWithTariffs || serversWithTariffs.length === 0) && (
                <div className="text-center py-12 rounded-xl bg-gray-800/30 border border-gray-700/50">
                  <ServerIcon className="w-16 h-16 mx-auto mb-4 text-gray-600" />
                  <p className="text-gray-400">Серверов с тарифами пока нет</p>
                </div>
              )}
            </div>
          </div>
          )}

          <div className="mb-16">
            <div className="h-px bg-gradient-to-r from-transparent via-gray-800 to-transparent"></div>
          </div>

          {vipServersWithTariffs && vipServersWithTariffs.length > 0 && (
            <div className="rounded-2xl border border-gray-700/60 p-8 mb-16 relative overflow-hidden">
              <div className="absolute inset-0 bg-gradient-to-br from-gray-900/20 via-transparent to-gray-800/10 rounded-2xl"></div>

              <div className="relative">
                <div className="mb-8">
                  <h2 className="text-3xl font-bold text-white mb-3 flex items-center gap-3">
                    <div className="p-2 rounded-xl bg-blue-500/10 border border-blue-500/20">
                      <Crown className="w-6 h-6 text-blue-500" />
                    </div>
                    VIP статус
                  </h2>
                  <p className="text-gray-400 text-lg leading-relaxed">
                    Получите VIP статус и дополнительные преимущества на сервере
                  </p>
                </div>

              <div className="mb-8">
                <h3 className="text-2xl font-bold text-white mb-6 flex items-center gap-3">
                  <ServerIcon className="w-5 h-5 text-highlight" />
                  Выберите сервер
                </h3>
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                  {vipServersWithTariffs?.map((server) => (
                    <button
                      key={server.serverId}
                      onClick={() => setSelectedVipServerId(server.serverId)}
                      className={`group p-6 rounded-xl border transition-all duration-200 text-left hover:shadow-lg ${
                        selectedVipServerId === server.serverId
                          ? 'border-highlight bg-highlight/10 shadow-lg shadow-highlight/10'
                          : 'border-gray-700/50 bg-gray-800/30 hover:border-highlight/60 hover:bg-highlight/5 hover:shadow-lg hover:shadow-highlight/5'
                      }`}
                    >
                      <div className="flex items-start gap-3">
                        <div className={`p-2 rounded-lg transition-all duration-200 ${
                          selectedVipServerId === server.serverId
                            ? 'bg-highlight/20'
                            : 'bg-gray-700/30 group-hover:bg-highlight/10'
                        }`}>
                          <ServerIcon className={`w-5 h-5 ${
                            selectedVipServerId === server.serverId ? 'text-highlight' : 'text-gray-400 group-hover:text-highlight'
                          }`} />
                        </div>
                        <div className="flex-1">
                          <h4 className="text-white font-bold text-lg mb-1">{server.serverName}</h4>
                          <p className="text-gray-400 text-sm mb-2">{server.serverIp}</p>
                          <div className="flex items-center gap-2">
                            <div className={`px-2 py-1 rounded-md text-xs font-medium ${
                              selectedVipServerId === server.serverId
                                ? 'bg-highlight/20 text-highlight'
                                : 'bg-gray-700/50 text-gray-300'
                            }`}>
                              {server.tariffs.length} {server.tariffs.length === 1 ? 'тариф' : server.tariffs.length < 5 ? 'тарифа' : 'тарифов'}
                            </div>
                          </div>
                        </div>
                      </div>
                    </button>
                  ))}
                </div>
              </div>

              {selectedVipServer && (
                <div>
                  <div className="mb-8">
                    <h3 className="text-2xl font-bold text-white mb-3 flex items-center gap-3">
                      <Crown className="w-5 h-5 text-blue-500" />
                      VIP тарифы для {selectedVipServer.serverName}
                    </h3>
                    <div className="h-px bg-gradient-to-r from-transparent via-gray-700/50 to-transparent"></div>
                  </div>

                  {myVipPrivileges && myVipPrivileges.filter(p => p.serverId === selectedVipServerId && p.isActive).length > 0 && (
                    <div className="bg-gradient-to-r from-blue-500/10 to-blue-500/10 border border-blue-500/30 rounded-xl p-6 mb-8 relative overflow-hidden">
                      <div className="absolute inset-0 bg-gradient-to-r from-blue-500/5 to-transparent"></div>
                      <div className="relative flex items-start gap-3">
                        <div className="p-2 rounded-lg bg-blue-500/20">
                          <Crown className="w-6 h-6 text-blue-500" />
                        </div>
                        <div className="flex-1">
                          <h4 className="text-blue-400 font-bold mb-3 flex items-center gap-2">
                            <div className="w-2 h-2 rounded-full bg-blue-500"></div>
                            Ваши активные VIP права
                          </h4>
                          <div className="space-y-2">
                            {myVipPrivileges
                              .filter(p => p.serverId === selectedVipServerId && p.isActive)
                              .map((priv) => (
                                <div key={priv.id} className="flex items-center justify-between p-3 bg-gray-800/30 rounded-lg border border-gray-700/30">
                                  <span className="text-gray-200 font-medium">{priv.tariffName}</span>
                                  <div className="flex items-center gap-2 text-sm text-gray-400">
                                    <Clock className="w-4 h-4" />
                                    {priv.expiresAt && isForeverDate(priv.expiresAt)
                                      ? 'Навсегда'
                                      : (priv.daysRemaining > 0
                                          ? `${priv.daysRemaining} ${priv.daysRemaining === 1 ? 'день' : priv.daysRemaining < 5 ? 'дня' : 'дней'}`
                                          : 'Истек')
                                    }
                                  </div>
                                </div>
                              ))
                            }
                          </div>
                        </div>
                      </div>
                    </div>
                  )}

                  <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    {selectedVipServer.tariffs
                      .filter(tariff => tariff.isActive && tariff.options?.some(o => o.isActive))
                      .sort((a, b) => a.order - b.order)
                      .map((tariff) => {
                        const selectedOptionId = selectedVipTariffOptions[tariff.id] || tariff.options?.[0]?.id;
                        const selectedOption = tariff.options?.find(o => o.id === selectedOptionId);

                        return (
                          <div
                            key={tariff.id}
                            className="group rounded-xl bg-gray-800/30 backdrop-blur-md border border-gray-700/50 hover:border-blue-500/60 p-6 transition-all duration-200 hover:shadow-lg hover:shadow-blue-500/10 relative overflow-hidden flex flex-col min-h-[400px]"
                          >
                            <div className="absolute inset-0 bg-gradient-to-br from-gray-900/40 via-transparent to-gray-800/20 opacity-0 group-hover:opacity-100 transition-opacity duration-200"></div>

                            <div className="relative flex flex-col flex-1">
                              <div className="flex items-start justify-between mb-4">
                                <h4 className="text-xl font-bold text-white">{tariff.name}</h4>
                                {selectedOption && (
                                  <div className="text-right">
                                    <div className="text-2xl font-bold text-blue-500">{selectedOption.price} ₽</div>
                                  </div>
                                )}
                              </div>

                              {tariff.description && (
                                <p className="text-gray-400 mb-4 text-sm leading-relaxed">{tariff.description}</p>
                              )}

                              <div className="mb-4">
                                <label className="text-gray-300 text-sm font-medium mb-3 flex items-center gap-2">
                                  <div className="p-1.5 rounded-md bg-blue-500/10">
                                    <Clock className="w-4 h-4 text-blue-500" />
                                  </div>
                                  Выберите срок:
                                </label>
                                <div className="grid grid-cols-2 gap-2">
                                  {tariff.options
                                    ?.filter(option => option.isActive)
                                    .sort((a, b) => a.order - b.order)
                                    .map(option => (
                                      <button
                                        key={option.id}
                                        onClick={() => setSelectedVipTariffOptions(prev => ({
                                          ...prev,
                                          [tariff.id]: option.id
                                        }))}
                                        className={`group p-3 rounded-lg border transition-all duration-200 text-left hover:shadow-md ${
                                          selectedOptionId === option.id
                                            ? 'border-blue-500 bg-blue-500/15 shadow-lg shadow-blue-500/10'
                                            : 'border-gray-600/50 bg-gray-700/30 hover:border-blue-500/60 hover:bg-blue-500/5 hover:shadow-lg hover:shadow-blue-500/5'
                                        }`}
                                      >
                                        <div className="flex items-center justify-between">
                                          <div className="flex items-center gap-2">
                                            <div className={`p-1.5 rounded-md transition-all duration-200 ${
                                              selectedOptionId === option.id
                                                ? 'bg-blue-500/20'
                                                : 'bg-gray-600/30 group-hover:bg-blue-500/10'
                                            }`}>
                                              <Clock className={`w-3.5 h-3.5 ${
                                                selectedOptionId === option.id ? 'text-blue-500' : 'text-gray-400 group-hover:text-blue-500'
                                              }`} />
                                            </div>
                                            <span className={`text-sm font-medium transition-colors ${
                                              selectedOptionId === option.id ? 'text-white' : 'text-gray-300 group-hover:text-white'
                                            }`}>
                                              {option.durationDays === 0 
                                                ? 'Навсегда' 
                                                : `${option.durationDays} ${option.durationDays === 1 ? 'день' : option.durationDays < 5 ? 'дня' : 'дней'}`
                                              }
                                            </span>
                                          </div>
                                          <span className={`text-sm font-bold transition-colors ${
                                                selectedOptionId === option.id ? 'text-blue-500' : 'text-gray-400 group-hover:text-blue-500'
                                          }`}>
                                            {option.price} ₽
                                          </span>
                                        </div>
                                      </button>
                                    ))}
                                </div>
                              </div>

                              <div className="space-y-3 flex-1">
                                {selectedOption && (
                                  <div className="flex items-center gap-3 text-gray-300">
                                    <div className="p-1.5 rounded-md bg-blue-500/10">
                                      <Clock className="w-4 h-4 text-blue-500" />
                                    </div>
                                    <span className="text-sm">
                                      {selectedOption.durationDays === 0 
                                        ? 'Навсегда' 
                                        : `${selectedOption.durationDays} ${selectedOption.durationDays === 1 ? 'день' : selectedOption.durationDays < 5 ? 'дня' : 'дней'}`
                                    }
                                    </span>
                                  </div>
                                )}

                                {tariff.groupName && (
                                  <div className="flex items-center gap-3 text-gray-300">
                                    <div className="p-1.5 rounded-md bg-blue-500/10">
                                      <Crown className="w-4 h-4 text-blue-500" />
                                    </div>
                                    <div className="flex-1">
                                      <div className="text-xs text-gray-500 mb-1">VIP группа</div>
                                      <code className="text-xs bg-gray-700/50 px-2 py-1 rounded">{tariff.groupName}</code>
                                    </div>
                                  </div>
                                )}
                              </div>

                              <div className="mt-auto pt-4">
                                <button
                                  onClick={() => selectedOptionId && attemptVipPurchase(selectedOptionId)}
                                  disabled={isDonating || !selectedOptionId}
                                  className="w-full bg-gradient-to-r from-blue-500 to-highlight hover:from-highlight hover:to-blue-500 text-white font-bold py-3 px-6 rounded-lg transition-all duration-200 hover:shadow-lg hover:shadow-blue-500/20 disabled:opacity-50 disabled:cursor-not-allowed disabled:transform-none"
                                >
                                  Купить VIP
                                </button>
                              </div>
                            </div>
                          </div>
                        );
                      })}
                  </div>

                  {selectedVipServer.tariffs.filter(t => t.isActive && t.options?.some(o => o.isActive)).length === 0 && (
                    <div className="text-center py-12 rounded-xl bg-gray-800/30 border border-gray-700/50">
                      <Crown className="w-16 h-16 mx-auto mb-4 text-gray-600" />
                      <p className="text-gray-400">Для этого сервера пока нет доступных VIP тарифов</p>
                    </div>
                  )}
                </div>
              )}

              {(!vipServersWithTariffs || vipServersWithTariffs.length === 0) && (
                <div className="text-center py-12 rounded-xl bg-gray-800/30 border border-gray-700/50">
                  <ServerIcon className="w-16 h-16 mx-auto mb-4 text-gray-600" />
                  <p className="text-gray-400">Серверов с VIP тарифами пока нет</p>
                </div>
              )}
            </div>
          </div>
          )}
        </div>
      </div>

      {showPasswordModal && selectedTariffForPurchase && (
        <div className="fixed inset-0 bg-black/50 backdrop-blur-sm z-50 flex items-center justify-center p-4">
          <div className="rounded-2xl border border-gray-700/60 p-8 max-w-lg w-full relative overflow-hidden">
            <div className="absolute inset-0 bg-gradient-to-br from-gray-900/20 via-transparent to-gray-800/10 rounded-2xl"></div>

            <div className="relative">
              <div className="flex items-center justify-between mb-8">
                <h3 className="text-2xl font-bold text-white flex items-center gap-3">
                  <div className="p-2 rounded-xl bg-highlight/10 border border-highlight/20">
                    <Key className="w-6 h-6 text-highlight" />
                  </div>
                  Введите пароль
                </h3>
                <button
                  onClick={() => setShowPasswordModal(false)}
                  className="p-2 rounded-lg bg-gray-700/50 hover:bg-gray-600/50 text-gray-400 hover:text-white transition-colors"
                >
                  <X className="w-5 h-5" />
                </button>
              </div>

              <div className="space-y-6">
                <div className="bg-gray-800/30 backdrop-blur-md border border-gray-700/50 rounded-xl p-6 transition-all duration-200">
                  <div className="text-sm text-gray-400 mb-3 flex items-center gap-2">
                    <div className="p-1.5 rounded-md bg-highlight/10">
                      <Shield className="w-4 h-4 text-highlight" />
                    </div>
                    Вы покупаете:
                  </div>
                  <div className="text-white font-bold text-lg mb-1">{selectedTariffForPurchase.tariffName}</div>
                  <div className="text-gray-400 text-sm flex items-center gap-2">
                    <ServerIcon className="w-4 h-4" />
                    Сервер: {selectedTariffForPurchase.serverName}
                  </div>
                </div>

                <div className="space-y-4">
                  <div>
                    <label className="text-gray-300 font-medium mb-3 flex items-center gap-2">
                      <div className="p-1.5 rounded-md bg-highlight/10">
                        <Key className="w-4 h-4 text-highlight" />
                      </div>
                      Пароль для админ-прав <span className="text-red-400">*</span>
                    </label>
                    <div className="relative">
                      <div className="absolute left-4 top-1/2 transform -translate-y-1/2 p-1 rounded-lg bg-gray-700/50">
                        <Key className="w-4 h-4 text-gray-400" />
                      </div>
                      <input
                        type={showPassword ? "text" : "password"}
                        value={adminPassword}
                        onChange={(e) => setAdminPassword(e.target.value)}
                        placeholder="Введите пароль (минимум 4 символа)"
                        className="w-full bg-gray-700/50 border border-gray-600/50 rounded-xl pl-12 pr-12 py-4 text-white placeholder-gray-500 focus:outline-none focus:border-highlight/60 focus:bg-gray-700/70 transition-all duration-200 text-base"
                        autoFocus
                      />
                      <button
                        type="button"
                        onClick={() => setShowPassword(!showPassword)}
                        className="absolute right-4 top-1/2 transform -translate-y-1/2 p-1 rounded-lg bg-gray-700/50 hover:bg-gray-600/50 text-gray-400 hover:text-gray-300 transition-colors"
                      >
                        {showPassword ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                      </button>
                    </div>
                    <div className="text-xs text-gray-500 mt-3 flex items-start gap-2">
                      <div className="w-1 h-1 rounded-full bg-gray-500 mt-2 flex-shrink-0"></div>
                      Этот пароль будет использоваться для доступа к админ-командам на сервере
                    </div>
                  </div>

                  <div className="flex gap-4 pt-2">
                    <button
                      onClick={() => setShowPasswordModal(false)}
                      className="flex-1 bg-gray-700/50 hover:bg-gray-600/50 text-gray-300 font-medium py-4 px-6 rounded-xl transition-all duration-200 border border-gray-600/30 hover:border-gray-500/50"
                    >
                      Отмена
                    </button>
                    <button
                      onClick={handleConfirmAdminPurchase}
                      disabled={!adminPassword.trim() || adminPassword.length < 4 || isDonating}
                      className="flex-1 bg-gradient-to-r from-highlight to-blue-500 hover:from-blue-500 hover:to-highlight text-white font-bold py-4 px-6 rounded-xl transition-all duration-200 hover:shadow-lg hover:shadow-highlight/20 disabled:opacity-50 disabled:cursor-not-allowed disabled:transform-none"
                    >
                      {isDonating ? 'Покупка...' : 'Купить'}
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
