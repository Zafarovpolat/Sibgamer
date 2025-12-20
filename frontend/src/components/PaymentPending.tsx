import { useEffect, useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { getTransactionStatus, cancelTransaction } from '../lib/donationApi';
import { Loader2, Clock, CheckCircle, XCircle, AlertCircle, CreditCard, DollarSign, FileText } from 'lucide-react';
import toast from 'react-hot-toast';
import { getErrorMessage } from '../utils/errorUtils';
import { parseServerDate, getServerLocalTime } from '../utils/dateUtils';

interface PaymentPendingProps {
  transactionId: string;
  onComplete: () => void;
  onCancel: () => void;
}

export default function PaymentPending({ transactionId, onComplete, onCancel }: PaymentPendingProps) {
  const queryClient = useQueryClient();
  const [timeRemaining, setTimeRemaining] = useState<number>(0);

  const { data: status, isLoading } = useQuery({
    queryKey: ['transactionStatus', transactionId],
    queryFn: () => getTransactionStatus(transactionId),
    refetchInterval: 5000, 
    enabled: !!transactionId,
  });

  const cancelMutation = useMutation({
    mutationFn: () => cancelTransaction(transactionId),
    onSuccess: () => {
      toast.success('Платёж отменён');
      queryClient.invalidateQueries({ queryKey: ['transactionStatus', transactionId] });
      onCancel();
    },
    onError: (error: unknown) => {
      const message = getErrorMessage(error) || '';
      if (!message.includes('ожидающие платежи')) {
        toast.error(message || 'Ошибка при отмене платежа');
      }
    },
  });

  useEffect(() => {
    if (!status?.pendingExpiresAt) {
      return;
    }

    const updateTimer = () => {
      const now = getServerLocalTime().getTime();
      const expiresAt = parseServerDate(status.pendingExpiresAt!).getTime();
      const remaining = Math.max(0, expiresAt - now);
      setTimeRemaining(remaining);
    };

    updateTimer();
    const interval = setInterval(updateTimer, 1000);

    return () => clearInterval(interval);
  }, [status?.pendingExpiresAt]);

  useEffect(() => {
    if (status?.status === 'completed') {
      toast.success('Платёж успешно завершён!');
      queryClient.invalidateQueries({ queryKey: ['myPrivileges'] });
      queryClient.invalidateQueries({ queryKey: ['topDonators'] });
      onComplete();
    } else if (status?.status === 'cancelled' || status?.status === 'failed') {
      onCancel();
    }
  }, [status?.status, onComplete, onCancel, queryClient]);

  const formatTime = (milliseconds: number): string => {
    const totalSeconds = Math.floor(milliseconds / 1000);
    const minutes = Math.floor(totalSeconds / 60);
    const seconds = totalSeconds % 60;
    return `${minutes}:${seconds.toString().padStart(2, '0')}`;
  };

  const getProgressPercentage = (): number => {
    if (!status?.pendingExpiresAt) return 100;
    const totalTime = 20 * 60 * 1000; 
    return (timeRemaining / totalTime) * 100;
  };

  if (isLoading) {
    return (
      <div className="rounded-xl bg-gradient-to-br from-gray-900/90 to-gray-800/90 backdrop-blur-md border border-gray-700/50 shadow-2xl p-8 text-center">
        <Loader2 className="w-12 h-12 text-accent mx-auto mb-4 animate-spin" />
        <p className="text-gray-400">Загрузка статуса платежа...</p>
      </div>
    );
  }

  return (
    <div className="rounded-xl bg-gradient-to-br from-gray-900/90 to-gray-800/90 backdrop-blur-md border border-accent/50 shadow-2xl overflow-hidden">
      <div className="bg-gradient-to-r from-accent/20 via-accent/10 to-transparent p-6 border-b border-gray-700/50">
        <div className="flex items-center justify-center gap-3 mb-2">
          <div className="relative">
            <div className="absolute inset-0 bg-accent/20 rounded-full blur-xl"></div>
            <Loader2 className="relative w-12 h-12 text-accent animate-spin" />
          </div>
          <h3 className="text-2xl font-bold text-white">Ожидание оплаты</h3>
        </div>
        <p className="text-gray-400 text-center">
          Завершите оплату в открывшейся вкладке ЮMoney
        </p>
      </div>

      <div className="p-6 space-y-6">
        <div className="flex items-center justify-center">
          {status?.status === 'pending' && (
            <div className="flex items-center gap-2 bg-yellow-500/10 border border-yellow-500/50 rounded-full px-4 py-2">
              <AlertCircle className="w-5 h-5 text-yellow-500" />
              <span className="font-semibold text-yellow-500">Ожидает оплаты</span>
            </div>
          )}
          {status?.status === 'completed' && (
            <div className="flex items-center gap-2 bg-green-500/10 border border-green-500/50 rounded-full px-4 py-2">
              <CheckCircle className="w-5 h-5 text-green-500" />
              <span className="font-semibold text-green-500">Оплачено</span>
            </div>
          )}
          {status?.status === 'cancelled' && (
            <div className="flex items-center gap-2 bg-gray-500/10 border border-gray-500/50 rounded-full px-4 py-2">
              <XCircle className="w-5 h-5 text-gray-500" />
              <span className="font-semibold text-gray-500">Отменено</span>
            </div>
          )}
          {status?.status === 'failed' && (
            <div className="flex items-center gap-2 bg-red-500/10 border border-red-500/50 rounded-full px-4 py-2">
              <XCircle className="w-5 h-5 text-red-500" />
              <span className="font-semibold text-red-500">Ошибка оплаты</span>
            </div>
          )}
        </div>

        {status?.status === 'pending' && (
          <>
            <div className="text-center">
              <div className="inline-flex items-center gap-3 bg-gradient-to-br from-accent/20 to-accent/5 rounded-2xl px-8 py-6 border border-accent/30">
                <Clock className="w-8 h-8 text-accent" />
                <div className="text-5xl font-bold text-accent tabular-nums">
                  {formatTime(timeRemaining)}
                </div>
              </div>
              <p className="text-sm text-gray-500 mt-3">до автоматической отмены</p>
            </div>

            <div className="relative">
              <div className="w-full bg-gray-800/50 rounded-full h-3 overflow-hidden border border-gray-700/50">
                <div 
                  className="bg-gradient-to-r from-accent to-blue-500 h-full transition-all duration-1000 ease-linear rounded-full"
                  style={{ width: `${getProgressPercentage()}%` }}
                />
              </div>
              <div className="absolute inset-0 bg-gradient-to-r from-transparent via-white/10 to-transparent rounded-full animate-pulse" />
            </div>

            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div className="bg-gray-800/50 rounded-xl p-4 border border-gray-700/50">
                <div className="flex items-center gap-2 mb-2">
                  <DollarSign className="w-5 h-5 text-accent" />
                  <span className="text-gray-400 text-sm">Сумма</span>
                </div>
                <p className="text-white font-bold text-2xl">{status.amount.toFixed(0)} ₽</p>
              </div>

              <div className="bg-gray-800/50 rounded-xl p-4 border border-gray-700/50">
                <div className="flex items-center gap-2 mb-2">
                  <CreditCard className="w-5 h-5 text-accent" />
                  <span className="text-gray-400 text-sm">Тип</span>
                </div>
                <p className="text-white font-bold text-lg">
                  {status.type === 'donation' ? 'Донат' : 'Админ-права'}
                </p>
              </div>

              {status.tariffName && (
                <div className="bg-gray-800/50 rounded-xl p-4 border border-gray-700/50">
                  <div className="flex items-center gap-2 mb-2">
                    <CheckCircle className="w-5 h-5 text-accent" />
                    <span className="text-gray-400 text-sm">Тариф</span>
                  </div>
                  <p className="text-white font-bold">{status.tariffName}</p>
                </div>
              )}

              {status.serverName && (
                <div className="bg-gray-800/50 rounded-xl p-4 border border-gray-700/50">
                  <div className="flex items-center gap-2 mb-2">
                    <AlertCircle className="w-5 h-5 text-accent" />
                    <span className="text-gray-400 text-sm">Сервер</span>
                  </div>
                  <p className="text-white font-bold">{status.serverName}</p>
                </div>
              )}
            </div>

            <button
              onClick={() => cancelMutation.mutate()}
              disabled={cancelMutation.isPending}
              className="w-full bg-red-600/90 hover:bg-red-600 text-white font-bold py-4 px-8 rounded-xl transition-all duration-300 disabled:opacity-50 disabled:cursor-not-allowed border border-red-500/50 shadow-lg hover:shadow-red-500/50"
            >
              {cancelMutation.isPending ? (
                <span className="flex items-center justify-center gap-2">
                  <Loader2 className="w-5 h-5 animate-spin" />
                  Отмена...
                </span>
              ) : (
                <span className="flex items-center justify-center gap-2">
                  <XCircle className="w-5 h-5" />
                  Отменить платёж
                </span>
              )}
            </button>
          </>
        )}

        <div className="bg-gradient-to-br from-blue-500/10 to-blue-600/5 border border-blue-500/30 rounded-xl p-5">
          <div className="flex items-start gap-3">
            <div className="flex-shrink-0 w-8 h-8 bg-blue-500/20 rounded-lg flex items-center justify-center">
              <FileText className="w-5 h-5 text-blue-400" />
            </div>
            <div className="flex-1">
              <p className="text-blue-400 font-semibold mb-3">Инструкция по оплате:</p>
              <ol className="text-gray-300 space-y-2 text-sm list-decimal list-inside">
                <li>В открывшейся вкладке завершите оплату через ЮMoney</li>
                <li>После оплаты вернитесь на эту страницу</li>
                <li>Статус автоматически обновится в течение 5-10 секунд</li>
                <li>У вас есть <strong className="text-accent">20 минут</strong> на завершение оплаты</li>
              </ol>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
