import { useState, useEffect } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCircle } from '@fortawesome/free-solid-svg-icons';
import { parseServerDate, getServerLocalTime } from '../utils/dateUtils';

interface CountdownTimerProps {
  targetDate: string;
  className?: string;
  showLabels?: boolean;
  compact?: boolean;
  statusLabel?: string;
  statusTextClass?: string;
  statusBgClass?: string;
}

const CountdownTimer = ({ targetDate, className = '', showLabels = true, compact = false, statusLabel, statusTextClass, statusBgClass }: CountdownTimerProps) => {
  const [timeLeft, setTimeLeft] = useState({
    days: 0,
    hours: 0,
    minutes: 0,
    seconds: 0,
    isExpired: false
  });

  useEffect(() => {
    const target = parseServerDate(targetDate).getTime();

    const updateTimer = () => {
      const now = getServerLocalTime().getTime();
      const difference = target - now;

      if (difference <= 0) {
        setTimeLeft({
          days: 0,
          hours: 0,
          minutes: 0,
          seconds: 0,
          isExpired: true
        });
        return;
      }

      const days = Math.floor(difference / (1000 * 60 * 60 * 24));
      const hours = Math.floor((difference % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
      const minutes = Math.floor((difference % (1000 * 60 * 60)) / (1000 * 60));
      const seconds = Math.floor((difference % (1000 * 60)) / 1000);

      setTimeLeft({
        days,
        hours,
        minutes,
        seconds,
        isExpired: false
      });
    };

    updateTimer();
    const interval = setInterval(updateTimer, 1000);

    return () => clearInterval(interval);
  }, [targetDate]);

  if (timeLeft.isExpired) {
    return null;
  }

  const formatNumber = (num: number) => num.toString().padStart(2, '0');

  const formatCountdownString = (useUnitDots = true) => {
    const parts: string[] = [];
    if (timeLeft.days > 0) parts.push(`${timeLeft.days}${useUnitDots ? 'д.' : 'д'}`);
    if (timeLeft.hours > 0 || parts.length > 0) {
      parts.push(`${formatNumber(timeLeft.hours)}${useUnitDots ? 'ч.' : 'ч'}`);
    }

    if (timeLeft.minutes > 0 || parts.length > 0) {
      parts.push(`${formatNumber(timeLeft.minutes)}${useUnitDots ? 'м.' : 'м'}`);
    }

    parts.push(`${formatNumber(timeLeft.seconds)}${useUnitDots ? 'с.' : 'с'}`);

    return parts.join(' ');
  };

  if (compact) {
    return (
      <div className={`${className} count-down-main inline-flex items-center gap-2`}>
        <div className={`flex items-center space-x-2 px-3 py-1 rounded-full ${statusBgClass ?? 'bg-black/40'} backdrop-blur-md border border-gray-700/40 ${className}`.trim()} role="group" aria-label={statusLabel ?? 'Countdown'}>
          <FontAwesomeIcon icon={faCircle} className={`${statusTextClass ?? 'text-green-400'} text-xs`} />
          {statusLabel && <span className={`text-sm font-medium ${statusTextClass ?? 'text-gray-200'}`.trim()}>{statusLabel}</span>}
          <span className={`text-sm font-medium ${statusTextClass ?? 'text-gray-200'}`.trim()}>{formatCountdownString(showLabels)}</span>
        </div>
      </div>
    );
  }

  return (
    <div className={`count-down-main ${className} flex items-start justify-center gap-1.5`}>
      <div className={`timer-pill lg rounded-full px-4 py-2 border border-gray-700/40 ${statusBgClass ?? 'bg-black/40'} ${className}`.trim()} role="group" aria-label={statusLabel ?? 'Countdown'}>
        <div className="flex items-center gap-3 whitespace-nowrap text-base">
          <FontAwesomeIcon icon={faCircle} className={`${statusTextClass ?? 'text-green-400'} text-xs`} />
          {statusLabel && <span className={`timer-label ${statusTextClass ?? 'text-gray-200'} text-sm`}>{statusLabel}</span>}
          <span className={`timer-text ${statusTextClass ?? 'text-gray-200'} text-lg font-semibold`}>{formatCountdownString(showLabels)}</span>
        </div>
      </div>
    </div>
  );
};

export default CountdownTimer;
