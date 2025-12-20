import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { getAllTransactions, getAllPrivileges, getAllVipPrivileges, approveTransaction, rejectTransaction, removeVipPrivilege, removeAdminPrivilege } from '../../lib/donationApi';
import { Search, Calendar, DollarSign, User, Server, Clock, CheckCircle, XCircle, AlertCircle, Shield, Check, X } from 'lucide-react';
import toast from 'react-hot-toast';
import { formatServerDate, getServerLocalTime, parseServerDate, isForeverDate } from '../../utils/dateUtils';

type Tab = 'transactions' | 'privileges' | 'vip-privileges';
type TransactionStatus = 'pending' | 'completed' | 'failed' | 'cancelled';
type TransactionType = 'donation' | 'admin_purchase' | 'vip_purchase';

export default function AdminDonationMonitoring() {
  const [activeTab, setActiveTab] = useState<Tab>('transactions');
  const [searchQuery, setSearchQuery] = useState('');
  const [statusFilter, setStatusFilter] = useState<TransactionStatus | 'all'>('all');
  const [typeFilter, setTypeFilter] = useState<TransactionType | 'all'>('all');
  const [privilegeStatusFilter, setPrivilegeStatusFilter] = useState<'active' | 'expired' | 'all'>('all');
  const [vipPrivilegeStatusFilter, setVipPrivilegeStatusFilter] = useState<'active' | 'expired' | 'all'>('all');

  const queryClient = useQueryClient();

  const approveMutation = useMutation({
    mutationFn: approveTransaction,
    onSuccess: (data) => {
      const isVipPurchase = Boolean((data as any).vipGroup) || data.tariffName?.toLowerCase().includes('vip') || data.message?.toLowerCase().includes('vip');
      toast.success(`${isVipPurchase ? 'VIP статус выдан' : 'Админка выдана'} на сервере ${data.serverName}`);
      queryClient.invalidateQueries({ queryKey: ['adminTransactions'] });
      queryClient.invalidateQueries({ queryKey: ['adminPrivileges'] });
      queryClient.invalidateQueries({ queryKey: ['adminVipPrivileges'] });
    },
    onError: (error: unknown) => {
      const err = error as { response?: { data?: { message?: string } } };
      toast.error(err?.response?.data?.message || 'Ошибка при подтверждении платежа');
    },
  });

  const rejectMutation = useMutation({
    mutationFn: rejectTransaction,
    onSuccess: () => {
      toast.success('Платеж отклонен');
      queryClient.invalidateQueries({ queryKey: ['adminTransactions'] });
    },
    onError: (error: unknown) => {
      const err = error as { response?: { data?: { message?: string } } };
      toast.error(err?.response?.data?.message || 'Ошибка при отклонении платежа');
    },
  });

  const removeVipMutation = useMutation({
    mutationFn: removeVipPrivilege,
    onSuccess: () => {
      toast.success('VIP статус удален');
      queryClient.invalidateQueries({ queryKey: ['adminVipPrivileges'] });
    },
    onError: (error: unknown) => {
      const err = error as { response?: { data?: { message?: string } } };
      toast.error(err?.response?.data?.message || 'Ошибка при удалении VIP статуса');
    },
  });

  const removeAdminMutation = useMutation({
    mutationFn: removeAdminPrivilege,
    onSuccess: () => {
      toast.success('Admin привилегия удалена');
      queryClient.invalidateQueries({ queryKey: ['adminPrivileges'] });
    },
    onError: (error: unknown) => {
      const err = error as { response?: { data?: { message?: string } } };
      toast.error(err?.response?.data?.message || 'Ошибка при удалении админ привилегии');
    },
  });

  const { data: transactions, isLoading: transactionsLoading } = useQuery({
    queryKey: ['adminTransactions'],
    queryFn: () => getAllTransactions(1, 1000),
    enabled: activeTab === 'transactions',
  });

  const { data: privileges, isLoading: privilegesLoading } = useQuery({
    queryKey: ['adminPrivileges'],
    queryFn: () => getAllPrivileges(1, 1000),
    enabled: activeTab === 'privileges',
  });

  const { data: vipPrivileges, isLoading: vipPrivilegesLoading } = useQuery({
    queryKey: ['adminVipPrivileges'],
    queryFn: () => getAllVipPrivileges(1, 1000),
    enabled: activeTab === 'vip-privileges',
  });

  const filteredTransactions = transactions?.filter((tx) => {
    const matchesSearch = searchQuery
      ? (tx.username?.toLowerCase().includes(searchQuery.toLowerCase()) || false) ||
        (tx.steamId?.toLowerCase().includes(searchQuery.toLowerCase()) || false) ||
        tx.transactionId.toLowerCase().includes(searchQuery.toLowerCase())
      : true;
    const matchesStatus = statusFilter === 'all' || tx.status === statusFilter;
    const matchesType = typeFilter === 'all' || tx.type === typeFilter;
    return matchesSearch && matchesStatus && matchesType;
  });

  const filteredPrivileges = privileges?.filter((priv) => {
    const matchesSearch = searchQuery
      ? (priv.username?.toLowerCase().includes(searchQuery.toLowerCase()) || false) ||
        (priv.steamId?.toLowerCase().includes(searchQuery.toLowerCase()) || false) ||
        (priv.serverName?.toLowerCase().includes(searchQuery.toLowerCase()) || false)
      : true;
    
    const now = getServerLocalTime();
    const isActive = priv.isActive && (!priv.expiresAt || (parseServerDate(priv.expiresAt).getTime()) > now.getTime());
    
    const matchesStatus = 
      privilegeStatusFilter === 'all' ||
      (privilegeStatusFilter === 'active' && isActive) ||
      (privilegeStatusFilter === 'expired' && !isActive);
    
    return matchesSearch && matchesStatus;
  });

  const filteredVipPrivileges = vipPrivileges?.filter((priv) => {
    const matchesSearch = searchQuery
      ? (priv.username?.toLowerCase().includes(searchQuery.toLowerCase()) || false) ||
        (priv.steamId?.toLowerCase().includes(searchQuery.toLowerCase()) || false) ||
        (priv.serverName?.toLowerCase().includes(searchQuery.toLowerCase()) || false)
      : true;
    
    const now = getServerLocalTime();
    const isActive = priv.isActive && (!priv.expiresAt || (parseServerDate(priv.expiresAt).getTime()) > now.getTime());
    
    const matchesStatus = 
      vipPrivilegeStatusFilter === 'all' ||
      (vipPrivilegeStatusFilter === 'active' && isActive) ||
      (vipPrivilegeStatusFilter === 'expired' && !isActive);
    
    return matchesSearch && matchesStatus;
  });

  const getStatusBadge = (status: TransactionStatus) => {
    switch (status) {
      case 'completed':
        return (
          <span className="flex items-center gap-1 text-green-500 bg-green-500/10 px-3 py-1 rounded-full text-sm font-semibold">
            <CheckCircle className="w-4 h-4" />
            Завершена
          </span>
        );
      case 'pending':
        return (
          <span className="flex items-center gap-1 text-yellow-500 bg-yellow-500/10 px-3 py-1 rounded-full text-sm font-semibold">
            <AlertCircle className="w-4 h-4" />
            Ожидание
          </span>
        );
      case 'cancelled':
        return (
          <span className="flex items-center gap-1 text-gray-500 bg-gray-500/10 px-3 py-1 rounded-full text-sm font-semibold">
            <XCircle className="w-4 h-4" />
            Отменена
          </span>
        );
      case 'failed':
        return (
          <span className="flex items-center gap-1 text-red-500 bg-red-500/10 px-3 py-1 rounded-full text-sm font-semibold">
            <XCircle className="w-4 h-4" />
            Ошибка
          </span>
        );
    }
  };

  const getTypeBadge = (type: TransactionType) => {
    if (type === 'donation') {
      return (
        <span className="text-blue-400 bg-blue-400/10 px-3 py-1 rounded-full text-sm font-semibold">
          Донат
        </span>
      );
    } else if (type === 'vip_purchase') {
      return (
        <span className="text-yellow-400 bg-yellow-400/10 px-3 py-1 rounded-full text-sm font-semibold">
          VIP
        </span>
      );
    } else {
      return (
        <span className="text-purple-400 bg-purple-400/10 px-3 py-1 rounded-full text-sm font-semibold">
          Админ
        </span>
      );
    }
  };

  const formatDate = (dateString: string) => {
    return formatServerDate(dateString, {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  const getDaysRemaining = (expiresAt: string | null) => {
    if (!expiresAt) return null;
    if (isForeverDate(expiresAt)) return null;
    const now = getServerLocalTime();
    const expiry = parseServerDate(expiresAt);
    const expiryMSK = expiry.getTime();
    const diff = expiryMSK - now.getTime();
    const days = Math.ceil(diff / (1000 * 60 * 60 * 24));
    return days;
  };

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-3xl font-bold text-white mb-2">Мониторинг донатов</h1>
        <p className="text-gray-400">Отслеживайте транзакции и привилегии пользователей</p>
      </div>

      <div className="flex gap-2 bg-secondary rounded-lg p-2">
        <button
          onClick={() => setActiveTab('transactions')}
          className={`flex-1 py-3 px-6 rounded-lg font-bold transition-all ${
            activeTab === 'transactions'
              ? 'bg-accent text-white'
              : 'text-gray-400 hover:text-white'
          }`}
        >
          <div className="flex items-center justify-center gap-2">
            <DollarSign className="w-5 h-5" />
            Транзакции
          </div>
        </button>
        <button
          onClick={() => setActiveTab('privileges')}
          className={`flex-1 py-3 px-6 rounded-lg font-bold transition-all ${
            activeTab === 'privileges'
              ? 'bg-accent text-white'
              : 'text-gray-400 hover:text-white'
          }`}
        >
          <div className="flex items-center justify-center gap-2">
            <Shield className="w-5 h-5" />
            Админ привилегии
          </div>
        </button>
        <button
          onClick={() => setActiveTab('vip-privileges')}
          className={`flex-1 py-3 px-6 rounded-lg font-bold transition-all ${
            activeTab === 'vip-privileges'
              ? 'bg-accent text-white'
              : 'text-gray-400 hover:text-white'
          }`}
        >
          <div className="flex items-center justify-center gap-2">
            <Shield className="w-5 h-5" />
            VIP привилегии
          </div>
        </button>
      </div>

      <div className="bg-secondary rounded-lg p-6">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
          <div className="md:col-span-2">
            <label className="block text-gray-300 mb-2 font-semibold">
              Поиск
            </label>
            <div className="relative">
              <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 w-5 h-5 text-gray-400" />
              <input
                type="text"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                placeholder={
                  activeTab === 'transactions'
                    ? 'Имя, Email, ID транзакции...'
                    : 'Имя, Steam ID, Сервер...'
                }
                className="w-full bg-primary border border-gray-700 rounded-lg pl-10 pr-4 py-3 text-white focus:outline-none focus:border-accent"
              />
            </div>
          </div>

          {activeTab === 'transactions' ? (
            <>
              <div>
                <label className="block text-gray-300 mb-2 font-semibold">
                  Статус
                </label>
                <select
                  value={statusFilter}
                  onChange={(e) => setStatusFilter(e.target.value as TransactionStatus | 'all')}
                  className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                >
                  <option value="all">Все</option>
                  <option value="completed">Завершена</option>
                  <option value="pending">Ожидание</option>
                  <option value="cancelled">Отменена</option>
                  <option value="failed">Ошибка</option>
                </select>
              </div>
              <div>
                <label className="block text-gray-300 mb-2 font-semibold">
                  Тип
                </label>
                <select
                  value={typeFilter}
                  onChange={(e) => setTypeFilter(e.target.value as TransactionType | 'all')}
                  className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                >
                  <option value="all">Все</option>
                  <option value="donation">Донат</option>
                  <option value="admin_purchase">Админ</option>
                  <option value="vip_purchase">VIP</option>
                </select>
              </div>
            </>
          ) : activeTab === 'privileges' ? (
            <div className="md:col-span-2">
              <label className="block text-gray-300 mb-2 font-semibold">
                Статус
              </label>
              <select
                value={privilegeStatusFilter}
                onChange={(e) => setPrivilegeStatusFilter(e.target.value as 'active' | 'expired' | 'all')}
                className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
              >
                <option value="all">Все</option>
                <option value="active">Активные</option>
                <option value="expired">Истекшие</option>
              </select>
            </div>
          ) : (
            <div className="md:col-span-2">
              <label className="block text-gray-300 mb-2 font-semibold">
                Статус VIP
              </label>
              <select
                value={vipPrivilegeStatusFilter}
                onChange={(e) => setVipPrivilegeStatusFilter(e.target.value as 'active' | 'expired' | 'all')}
                className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
              >
                <option value="all">Все</option>
                <option value="active">Активные</option>
                <option value="expired">Истекшие</option>
              </select>
            </div>
          )}
        </div>
      </div>

      {activeTab === 'transactions' && (
        <div className="bg-secondary rounded-lg overflow-hidden">
          {transactionsLoading ? (
            <div className="flex items-center justify-center h-64">
              <div className="text-gray-400">Загрузка...</div>
            </div>
          ) : filteredTransactions && filteredTransactions.length > 0 ? (
            <div className="overflow-x-auto">
              <table className="w-full">
                <thead className="bg-primary">
                  <tr>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">ID</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Пользователь</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Тип</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Сумма</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Статус</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Дата</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Сервер</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Действия</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-700">
                  {filteredTransactions.map((tx) => (
                    <tr key={tx.id} className="hover:bg-primary/50 transition-colors">
                      <td className="px-6 py-4">
                        <code className="text-xs bg-primary px-2 py-1 rounded text-gray-300">
                          {tx.transactionId.substring(0, 12)}...
                        </code>
                      </td>
                      <td className="px-6 py-4">
                        <div className="flex items-center gap-2">
                          <User className="w-4 h-4 text-gray-400" />
                          <div>
                            <div className="text-white font-semibold">{tx.username || 'Гость'}</div>
                            <div className="text-xs text-gray-400">{tx.steamId || '—'}</div>
                          </div>
                        </div>
                      </td>
                      <td className="px-6 py-4">
                        {getTypeBadge(tx.type as TransactionType)}
                      </td>
                      <td className="px-6 py-4">
                        <div className="flex items-center gap-1 text-white font-bold">
                          <DollarSign className="w-4 h-4 text-accent" />
                          {tx.amount} ₽
                        </div>
                      </td>
                      <td className="px-6 py-4">
                        {getStatusBadge(tx.status as TransactionStatus)}
                      </td>
                      <td className="px-6 py-4">
                        <div className="flex items-center gap-2 text-gray-300 text-sm">
                          <Calendar className="w-4 h-4 text-gray-400" />
                          {formatDate(tx.createdAt)}
                        </div>
                      </td>
                      <td className="px-6 py-4">
                        {tx.serverName ? (
                          <div className="flex items-center gap-1 text-gray-300 text-sm">
                            <Server className="w-4 h-4 text-gray-400" />
                            {tx.serverName}
                          </div>
                        ) : (
                          <span className="text-gray-500 text-sm">—</span>
                        )}
                      </td>
                      <td className="px-6 py-4">
                        {tx.status === 'pending' && (tx.type === 'admin_purchase' || tx.type === 'vip_purchase') ? (
                          <div className="flex gap-2">
                            <button
                              onClick={() => approveMutation.mutate(tx.transactionId)}
                              disabled={approveMutation.isPending}
                              className="flex items-center gap-1 bg-green-600 hover:bg-green-700 disabled:bg-green-800 text-white px-3 py-1 rounded text-sm font-semibold transition-colors"
                            >
                              <Check className="w-4 h-4" />
                              {approveMutation.isPending ? 'Подтверждаю...' : 'Подтвердить'}
                            </button>
                            <button
                              onClick={() => rejectMutation.mutate(tx.transactionId)}
                              disabled={rejectMutation.isPending}
                              className="flex items-center gap-1 bg-red-600 hover:bg-red-700 disabled:bg-red-800 text-white px-3 py-1 rounded text-sm font-semibold transition-colors"
                            >
                              <X className="w-4 h-4" />
                              {rejectMutation.isPending ? 'Отклоняю...' : 'Отклонить'}
                            </button>
                          </div>
                        ) : (
                          <span className="text-gray-500 text-sm">—</span>
                        )}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          ) : (
            <div className="flex flex-col items-center justify-center h-64 text-gray-400">
              <DollarSign className="w-16 h-16 mb-4 opacity-50" />
              <p className="text-lg">Транзакции не найдены</p>
            </div>
          )}
        </div>
      )}

      {activeTab === 'privileges' && (
        <div className="bg-secondary rounded-lg overflow-hidden">
          {privilegesLoading ? (
            <div className="flex items-center justify-center h-64">
              <div className="text-gray-400">Загрузка...</div>
            </div>
          ) : filteredPrivileges && filteredPrivileges.length > 0 ? (
            <div className="overflow-x-auto">
              <table className="w-full">
                <thead className="bg-primary">
                  <tr>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Пользователь</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Steam ID</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Сервер</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Тариф</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Статус</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Истекает</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Куплено</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Действия</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-700">
                  {filteredPrivileges.map((priv) => {
                    const now = getServerLocalTime();
                    const isActive = priv.isActive && (!priv.expiresAt || (parseServerDate(priv.expiresAt).getTime()) > now.getTime());
                    const daysRemaining = priv.expiresAt ? getDaysRemaining(priv.expiresAt) : null;

                    return (
                      <tr key={priv.id} className="hover:bg-primary/50 transition-colors">
                        <td className="px-6 py-4">
                          <div className="flex items-center gap-2">
                            <User className="w-4 h-4 text-gray-400" />
                            <span className="text-white font-semibold">{priv.username || 'Пользователь'}</span>
                          </div>
                        </td>
                        <td className="px-6 py-4">
                          <code className="text-xs bg-primary px-2 py-1 rounded text-gray-300">
                            {priv.steamId}
                          </code>
                        </td>
                        <td className="px-6 py-4">
                          <div className="flex items-center gap-1 text-gray-300 text-sm">
                            <Server className="w-4 h-4 text-gray-400" />
                            {priv.serverName}
                          </div>
                        </td>
                        <td className="px-6 py-4">
                          <span className="text-white font-semibold">{priv.tariffName}</span>
                        </td>
                        <td className="px-6 py-4">
                          {isActive ? (
                            <span className="flex items-center gap-1 text-green-500 bg-green-500/10 px-3 py-1 rounded-full text-sm font-semibold w-fit">
                              <CheckCircle className="w-4 h-4" />
                              Активна
                            </span>
                          ) : (
                            <span className="flex items-center gap-1 text-gray-500 bg-gray-500/10 px-3 py-1 rounded-full text-sm font-semibold w-fit">
                              <XCircle className="w-4 h-4" />
                              Истекла
                            </span>
                          )}
                        </td>
                        <td className="px-6 py-4">
                          {priv.expiresAt ? (
                            <div>
                              <div className="flex items-center gap-2 text-gray-300 text-sm">
                                <Clock className="w-4 h-4 text-gray-400" />
                                {isForeverDate(priv.expiresAt) ? 'Навсегда' : formatDate(priv.expiresAt)}
                              </div>
                              {isActive && daysRemaining !== null && (
                                <div className={`text-xs mt-1 ${
                                  daysRemaining <= 3 ? 'text-red-400' :
                                  daysRemaining <= 7 ? 'text-yellow-400' :
                                  'text-gray-400'
                                }`}>
                                  Осталось {daysRemaining} дн.
                                </div>
                              )}
                            </div>
                          ) : (
                            <span className="text-gray-400 text-sm">Бессрочно</span>
                          )}
                        </td>
                        <td className="px-6 py-4">
                          <div className="flex items-center gap-2 text-gray-300 text-sm">
                            <Calendar className="w-4 h-4 text-gray-400" />
                            {priv.createdAt ? formatDate(priv.createdAt) : '—'}
                          </div>
                        </td>
                        <td className="px-6 py-4">
                          <button
                            onClick={() => {
                              if (window.confirm(`Вы уверены, что хотите удалить админ-привилегию у пользователя ${priv.username || 'Пользователь'}?`)) {
                                removeAdminMutation.mutate(priv.id);
                              }
                            }}
                            disabled={removeAdminMutation.isPending}
                            className="flex items-center gap-1 bg-red-600 hover:bg-red-700 disabled:bg-red-800 text-white px-3 py-1 rounded text-sm font-semibold transition-colors"
                          >
                            <X className="w-4 h-4" />
                            {removeAdminMutation.isPending ? 'Удаляю...' : 'Удалить админ'}
                          </button>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          ) : (
            <div className="flex flex-col items-center justify-center h-64 text-gray-400">
              <Shield className="w-16 h-16 mb-4 opacity-50" />
              <p className="text-lg">Привилегии не найдены</p>
            </div>
          )}
        </div>
      )}

      {activeTab === 'vip-privileges' && (
        <div className="bg-secondary rounded-lg overflow-hidden">
          {vipPrivilegesLoading ? (
            <div className="flex items-center justify-center h-64">
              <div className="text-gray-400">Загрузка...</div>
            </div>
          ) : filteredVipPrivileges && filteredVipPrivileges.length > 0 ? (
            <div className="overflow-x-auto">
              <table className="w-full">
                <thead className="bg-primary">
                  <tr>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Пользователь</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Steam ID</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Сервер</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Тариф</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Статус</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Истекает</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Куплено</th>
                    <th className="px-6 py-4 text-left text-sm font-bold text-gray-300">Действия</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-700">
                  {filteredVipPrivileges.map((priv) => {
                    const now = getServerLocalTime();
                    const isActive = priv.isActive && (!priv.expiresAt || (parseServerDate(priv.expiresAt).getTime()) > now.getTime());
                    const daysRemaining = priv.expiresAt ? getDaysRemaining(priv.expiresAt) : null;

                    return (
                      <tr key={priv.id} className="hover:bg-primary/50 transition-colors">
                        <td className="px-6 py-4">
                          <div className="flex items-center gap-2">
                            <User className="w-4 h-4 text-gray-400" />
                            <span className="text-white font-semibold">{priv.username || 'Пользователь'}</span>
                          </div>
                        </td>
                        <td className="px-6 py-4">
                          <code className="text-xs bg-primary px-2 py-1 rounded text-gray-300">
                            {priv.steamId}
                          </code>
                        </td>
                        <td className="px-6 py-4">
                          <div className="flex items-center gap-1 text-gray-300 text-sm">
                            <Server className="w-4 h-4 text-gray-400" />
                            {priv.serverName}
                          </div>
                        </td>
                        <td className="px-6 py-4">
                          <span className="text-white font-semibold">{priv.tariffName}</span>
                        </td>
                        <td className="px-6 py-4">
                          {isActive ? (
                            <span className="flex items-center gap-1 text-green-500 bg-green-500/10 px-3 py-1 rounded-full text-sm font-semibold w-fit">
                              <CheckCircle className="w-4 h-4" />
                              Активна
                            </span>
                          ) : (
                            <span className="flex items-center gap-1 text-gray-500 bg-gray-500/10 px-3 py-1 rounded-full text-sm font-semibold w-fit">
                              <XCircle className="w-4 h-4" />
                              Истекла
                            </span>
                          )}
                        </td>
                        <td className="px-6 py-4">
                          {priv.expiresAt ? (
                            <div>
                              <div className="flex items-center gap-2 text-gray-300 text-sm">
                                <Clock className="w-4 h-4 text-gray-400" />
                                {isForeverDate(priv.expiresAt) ? 'Навсегда' : formatDate(priv.expiresAt)}
                              </div>
                              {isActive && daysRemaining !== null && (
                                <div className={`text-xs mt-1 ${
                                  daysRemaining <= 3 ? 'text-red-400' :
                                  daysRemaining <= 7 ? 'text-yellow-400' :
                                  'text-gray-400'
                                }`}>
                                  Осталось {daysRemaining} дн.
                                </div>
                              )}
                            </div>
                          ) : (
                            <span className="text-gray-400 text-sm">Бессрочно</span>
                          )}
                        </td>
                        <td className="px-6 py-4">
                          <div className="flex items-center gap-2 text-gray-300 text-sm">
                            <Calendar className="w-4 h-4 text-gray-400" />
                            {priv.createdAt ? formatDate(priv.createdAt) : '—'}
                          </div>
                        </td>
                        <td className="px-6 py-4">
                          <button
                            onClick={() => {
                              if (window.confirm(`Вы уверены, что хотите удалить VIP статус у пользователя ${priv.username || 'Пользователь'}?`)) {
                                removeVipMutation.mutate(priv.id);
                              }
                            }}
                            disabled={removeVipMutation.isPending}
                            className="flex items-center gap-1 bg-red-600 hover:bg-red-700 disabled:bg-red-800 text-white px-3 py-1 rounded text-sm font-semibold transition-colors"
                          >
                            <X className="w-4 h-4" />
                            {removeVipMutation.isPending ? 'Удаляю...' : 'Удалить VIP'}
                          </button>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          ) : (
            <div className="flex flex-col items-center justify-center h-64 text-gray-400">
              <Shield className="w-16 h-16 mb-4 opacity-50" />
              <p className="text-lg">VIP привилегии не найдены</p>
            </div>
          )}
        </div>
      )}

      {activeTab === 'transactions' && filteredTransactions && (
        <div className="grid grid-cols-1 md:grid-cols-5 gap-4">
          <div className="bg-secondary rounded-lg p-6">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-gray-400 text-sm mb-1">Всего транзакций</p>
                <p className="text-3xl font-bold text-white">{filteredTransactions.length}</p>
              </div>
              <DollarSign className="w-12 h-12 text-white opacity-75" />
            </div>
          </div>
          <div className="bg-secondary rounded-lg p-6">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-gray-400 text-sm mb-1">Завершено</p>
                <p className="text-3xl font-bold text-green-500">
                  {filteredTransactions.filter(tx => tx.status === 'completed').length}
                </p>
              </div>
              <CheckCircle className="w-12 h-12 text-green-500 opacity-75" />
            </div>
          </div>
          <div className="bg-secondary rounded-lg p-6">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-gray-400 text-sm mb-1">В ожидании</p>
                <p className="text-3xl font-bold text-yellow-500">
                  {filteredTransactions.filter(tx => tx.status === 'pending').length}
                </p>
              </div>
              <AlertCircle className="w-12 h-12 text-yellow-500 opacity-75" />
            </div>
          </div>
          <div className="bg-secondary rounded-lg p-6">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-gray-400 text-sm mb-1">Отменено</p>
                <p className="text-3xl font-bold text-gray-400">
                  {filteredTransactions.filter(tx => tx.status === 'cancelled').length}
                </p>
              </div>
              <XCircle className="w-12 h-12 text-gray-400 opacity-75" />
            </div>
          </div>
          <div className="bg-secondary rounded-lg p-6">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-gray-400 text-sm mb-1">Общая сумма</p>
                <p className="text-3xl font-bold text-highlight">
                  {filteredTransactions
                    .filter(tx => tx.status === 'completed')
                    .reduce((sum, tx) => sum + tx.amount, 0)
                    .toFixed(2)} ₽
                </p>
              </div>
              <DollarSign className="w-12 h-12 text-highlight opacity-75" />
            </div>
          </div>
        </div>
      )}

      {activeTab === 'privileges' && filteredPrivileges && (
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <div className="bg-secondary rounded-lg p-6">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-gray-400 text-sm mb-1">Всего привилегий</p>
                <p className="text-3xl font-bold text-white">{filteredPrivileges.length}</p>
              </div>
              <Shield className="w-12 h-12 text-white opacity-75" />
            </div>
          </div>
          <div className="bg-secondary rounded-lg p-6">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-gray-400 text-sm mb-1">Активных</p>
                <p className="text-3xl font-bold text-green-500">
                  {filteredPrivileges.filter(priv => {
                    const now = getServerLocalTime();
                    return priv.isActive && (!priv.expiresAt || (parseServerDate(priv.expiresAt).getTime()) > now.getTime());
                  }).length}
                </p>
              </div>
              <CheckCircle className="w-12 h-12 text-green-500 opacity-75" />
            </div>
          </div>
          <div className="bg-secondary rounded-lg p-6">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-gray-400 text-sm mb-1">Истекших</p>
                <p className="text-3xl font-bold text-gray-400">
                  {filteredPrivileges.filter(priv => {
                    const now = getServerLocalTime();
                    return !priv.isActive || (priv.expiresAt && (parseServerDate(priv.expiresAt).getTime()) <= now.getTime());
                  }).length}
                </p>
              </div>
              <XCircle className="w-12 h-12 text-gray-400 opacity-75" />
            </div>
          </div>
        </div>
      )}

      {activeTab === 'vip-privileges' && filteredVipPrivileges && (
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <div className="bg-secondary rounded-lg p-6">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-gray-400 text-sm mb-1">Всего VIP привилегий</p>
                <p className="text-3xl font-bold text-white">{filteredVipPrivileges.length}</p>
              </div>
              <Shield className="w-12 h-12 text-white opacity-75" />
            </div>
          </div>
          <div className="bg-secondary rounded-lg p-6">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-gray-400 text-sm mb-1">Активных VIP</p>
                <p className="text-3xl font-bold text-green-500">
                  {filteredVipPrivileges.filter(priv => {
                    const now = getServerLocalTime();
                    return priv.isActive && (!priv.expiresAt || (parseServerDate(priv.expiresAt).getTime()) > now.getTime());
                  }).length}
                </p>
              </div>
              <CheckCircle className="w-12 h-12 text-green-500 opacity-75" />
            </div>
          </div>
          <div className="bg-secondary rounded-lg p-6">
            <div className="flex items-center justify-between">
              <div>
                <p className="text-gray-400 text-sm mb-1">Истекших VIP</p>
                <p className="text-3xl font-bold text-gray-400">
                  {filteredVipPrivileges.filter(priv => {
                    const now = getServerLocalTime();
                    return !priv.isActive || (priv.expiresAt && (parseServerDate(priv.expiresAt).getTime()) <= now.getTime());
                  }).length}
                </p>
              </div>
              <XCircle className="w-12 h-12 text-gray-400 opacity-75" />
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
