import { useEffect, useState } from 'react';
import { useSearchParams, Link, useNavigate } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCheckCircle, faSpinner, faExclamationTriangle, faHome, faEnvelope, faGift, faShield, faStar, faClock, faTimes, faCopy } from '@fortawesome/free-solid-svg-icons';
import { API_URL } from '../config/api';
import { getAuthToken } from '../lib/auth';

const DonationSuccess = () => {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const [status, setStatus] = useState<'checking' | 'success' | 'pending' | 'error'>('checking');
  interface TransactionData {
    amount?: number;
    adminPassword?: string;
    status?: string;
    transactionId?: string; 
    [key: string]: unknown;
  }

  const [transactionData, setTransactionData] = useState<TransactionData | null>(null);
  const [copied, setCopied] = useState(false);
  
  const type = searchParams.get('type');
  const transactionId = searchParams.get('transactionId');

  const copyToClipboard = async (text: string) => {
    try {
      await navigator.clipboard.writeText(text);
      setCopied(true);
      setTimeout(() => setCopied(false), 2000);
    } catch (err) {
      console.error('Failed to copy: ', err);
    }
  };

  useEffect(() => {
    const checkTransaction = async () => {
      if (!transactionId) {
        setStatus('error');
        return;
      }

      try {
        const token = getAuthToken();
        if (!token) {
          setStatus('pending');
          return;
        }

        let attempts = 0;
        const maxAttempts = 10;
        const checkInterval = 2000; 
        let foundTerminal = false;
        const checkStatus = async (): Promise<boolean> => {
          try {
            const res = await fetch(`${API_URL}/donation/transaction/${transactionId}`, {
              headers: {
                'Authorization': `Bearer ${token}`
              }
            });

            if (!res.ok) {
              return false;
            }

            const transaction = await res.json();

            if (transaction) {
              setTransactionData(transaction);
              
              if (transaction.status === 'completed') {
                setStatus('success');
                foundTerminal = true;
                return true;
              } else if (transaction.status === 'pending') {
                setStatus('pending');
                return false;
              } else {
                setStatus('error');
                foundTerminal = true;
                return true;
              }
            }
            
            return false;
          } catch (error: unknown) {
            console.error('Error checking transaction:', error);
            return false;
          }
        };

        while (attempts < maxAttempts) {
          const done = await checkStatus();
          if (done) break;
          
          attempts++;
          if (attempts < maxAttempts) {
            await new Promise(resolve => setTimeout(resolve, checkInterval));
          }
        }

        if (!foundTerminal) {
          setStatus('pending');
        }

      } catch (error: unknown) {
        console.error('Error:', error);
        setStatus('error');
      }
    };

    checkTransaction();
  }, [transactionId]);

  const getTitle = () => {
    if (status === 'checking') {
      return 'Проверяем статус платежа...';
    }
    
    if (status === 'success') {
      if (type === 'donation') {
        return 'Спасибо за поддержку!';
      } else if (type === 'admin') {
        return 'Админ-права активированы!';
      } else if (type === 'vip') {
        return 'VIP статус активирован!';
      } else if (type === 'extend') {
        return 'Привилегия продлена!';
      }
    }
    
    if (status === 'pending') {
      return 'Ожидаем подтверждение платежа';
    }
    
    return 'Произошла ошибка';
  };

  const getMessage = () => {
    if (status === 'checking') {
      return 'Пожалуйста, подождите. Мы проверяем статус вашего платежа в системе ЮMoney...';
    }
    
    if (status === 'success') {
      if (type === 'donation') {
        return `Ваш донат на сумму ${transactionData?.amount} ₽ успешно получен! Спасибо за вашу щедрость и поддержку нашего проекта. Благодаря вам мы можем продолжать развиваться!`;
      } else if (type === 'admin') {
        return `Ваши админ-права успешно активированы! Проверьте вашу электронную почту для получения инструкций по подключению. ${transactionData?.adminPassword ? `Ваш пароль: ${transactionData.adminPassword}` : ''}`;
      } else if (type === 'vip') {
        return `Ваш VIP статус успешно активирован! Проверьте вашу электронную почту для получения информации о преимуществах. Наслаждайтесь игрой с привилегиями!`;
      } else if (type === 'extend') {
        return `Ваша привилегия успешно продлена! Проверьте вашу электронную почту для получения подтверждения. ${transactionData?.adminPassword ? `Новый пароль: ${transactionData.adminPassword}` : ''}`;
      }
    }
    
    if (status === 'pending') {
      return 'Платёж обрабатывается. Обычно это занимает несколько минут. Вы получите уведомление на сайте и email, когда платёж будет подтверждён. Вы можете закрыть эту страницу.';
    }
    
    return 'К сожалению, произошла ошибка при обработке платежа. Пожалуйста, свяжитесь с администрацией, если деньги были списаны.';
  };

  const getIcon = () => {
    if (status === 'checking') {
      return <FontAwesomeIcon icon={faSpinner} className="text-6xl text-accent animate-spin" />;
    }
    
    if (status === 'success') {
      return <FontAwesomeIcon icon={faCheckCircle} className="text-6xl text-green-500" />;
    }
    
    if (status === 'pending') {
      return <FontAwesomeIcon icon={faSpinner} className="text-6xl text-yellow-500 animate-pulse" />;
    }
    
    return <FontAwesomeIcon icon={faExclamationTriangle} className="text-6xl text-red-500" />;
  };

  return (
    <div className="min-h-screen bg-primary flex items-center justify-center px-4 py-12">
      <div className="max-w-2xl w-full">
        <div className="glass-effect p-8 md:p-12 rounded-lg text-center">
          <div className="mb-6">
            {getIcon()}
          </div>
          
          <h1 className="text-3xl md:text-4xl font-bold text-white mb-4 flex items-center justify-center gap-3">
            {getTitle()}
            {status === 'success' && type === 'donation' && <FontAwesomeIcon icon={faGift} className="text-accent" />}
            {status === 'success' && type === 'admin' && <FontAwesomeIcon icon={faShield} className="text-accent" />}
            {status === 'success' && type === 'vip' && <FontAwesomeIcon icon={faStar} className="text-yellow-400" />}
            {status === 'success' && type === 'extend' && <FontAwesomeIcon icon={faGift} className="text-accent" />}
            {status === 'pending' && <FontAwesomeIcon icon={faClock} className="text-yellow-500" />}
            {status === 'error' && <FontAwesomeIcon icon={faTimes} className="text-red-500" />}
          </h1>
          
          <p className="text-gray-300 text-lg mb-8">
            {getMessage()}
          </p>

          {status === 'success' && (
            <div className="bg-secondary/30 rounded-lg p-6 mb-8">
              <div className="flex items-center justify-center gap-3 text-accent mb-3">
                <FontAwesomeIcon icon={faEnvelope} className="text-2xl" />
                <span className="text-lg font-semibold">Проверьте вашу почту</span>
              </div>
              <p className="text-gray-400 text-sm">
                Мы отправили подробную информацию на вашу электронную почту
              </p>
            </div>
          )}

          {transactionData && (
            <div className="bg-secondary/20 rounded-lg p-6 mb-8 text-left">
              <h3 className="text-white font-semibold mb-4">Детали транзакции:</h3>
              <div className="space-y-2 text-sm">
                <div className="flex justify-between items-center">
                  <span className="text-gray-400">ID транзакции:</span>
                  <div className="flex items-center gap-2">
                    <span className="text-white font-mono">{transactionData.transactionId?.substring(0, 8)}...</span>
                    <button
                      onClick={() => copyToClipboard(transactionData.transactionId ?? '')}
                      className="text-green-500 hover:text-green-400 transition-colors"
                      title="Копировать полный ID"
                    >
                      <FontAwesomeIcon icon={faCopy} className="text-sm" />
                    </button>
                    {copied && <span className="text-green-500 text-xs">Скопировано!</span>}
                  </div>
                </div>
                <div className="flex justify-between">
                  <span className="text-gray-400">Сумма:</span>
                  <span className="text-white font-semibold">{transactionData.amount} ₽</span>
                </div>
                <div className="flex justify-between">
                  <span className="text-gray-400">Статус:</span>
                  <span className={`font-semibold ${
                    transactionData.status === 'completed' ? 'text-green-500' :
                    transactionData.status === 'pending' ? 'text-yellow-500' :
                    'text-red-500'
                  }`}>
                    {transactionData.status === 'completed' ? 'Завершён' :
                     transactionData.status === 'pending' ? 'Ожидание' :
                     'Ошибка'}
                  </span>
                </div>
              </div>
            </div>
          )}

          <div className="flex flex-col sm:flex-row gap-4 justify-center">
            <Link
              to="/"
              className="btn-primary flex items-center justify-center gap-2"
            >
              <FontAwesomeIcon icon={faHome} />
              На главную
            </Link>
            
            {status === 'success' && (
              <button
                onClick={() => navigate('/profile')}
                className="btn-secondary flex items-center justify-center gap-2"
              >
                Мой профиль
              </button>
            )}
            
            {status === 'pending' && (
              <button
                onClick={() => navigate('/notifications')}
                className="btn-secondary flex items-center justify-center gap-2"
              >
                Мои уведомления
              </button>
            )}
          </div>

          {status === 'pending' && (
            <p className="text-gray-500 text-xs mt-6">
              💡 Совет: Добавьте эту страницу в закладки или следите за уведомлениями в профиле
            </p>
          )}
        </div>
      </div>
    </div>
  );
};

export default DonationSuccess;
