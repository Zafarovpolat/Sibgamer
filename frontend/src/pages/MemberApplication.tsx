import { useQuery } from '@tanstack/react-query';
import { useState, useRef, useEffect } from 'react';
import { useAuthStore } from '../store/authStore';
import { getServersWithVipPublic, createVipApplication, getMyVipApplications, getMyVipPrivileges } from '../lib/donationApi';
import toast from 'react-hot-toast';
import { useQueryClient } from '@tanstack/react-query';
import { usePageTitle } from '../hooks/usePageTitle';

export default function MemberApplication() {
  usePageTitle('Заявка на участника');

  const { user, isAuthenticated } = useAuthStore();
  const queryClient = useQueryClient();

  const { data: vipServers } = useQuery({ queryKey: ['vipServerListWithVip'], queryFn: getServersWithVipPublic });
  const { data: myApplications } = useQuery({ queryKey: ['myVipApplications'], queryFn: getMyVipApplications, enabled: isAuthenticated });
  const { data: myVipPrivileges } = useQuery({ queryKey: ['myVipPrivileges'], queryFn: getMyVipPrivileges, enabled: isAuthenticated });

  const [selectedServerId, setSelectedServerId] = useState<number | null>(vipServers?.[0]?.serverId ?? null);
  const [hoursPerWeek, setHoursPerWeek] = useState<number | undefined>(undefined);
  const [reason, setReason] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async () => {
    if (!isAuthenticated) {
      toast.error('Требуется авторизация');
      return;
    }

    if (!selectedServerId) {
      toast.error('Выберите сервер');
      return;
    }
    const hasActiveVip = !!myVipPrivileges?.some(p => p.serverId === selectedServerId && p.isActive);
    if (hasActiveVip) {
      toast.error('Вы уже являетесь участником на выбранном сервере');
      return;
    }

    const hasPending = !!myApplications?.some(a => a.serverId === selectedServerId && a.status === 'pending');
    if (hasPending) {
      toast.error('Вы уже отправили заявку для этого сервера — ожидайте решения администратора');
      return;
    }
    if (!reason.trim()) {
      toast.error('Укажите причину');
      return;
    }

    setIsSubmitting(true);
    try {
      await createVipApplication({ serverId: selectedServerId, hoursPerWeek, reason: reason.trim() });
      toast.success('Заявка отправлена');
      queryClient.invalidateQueries({ queryKey: ['myVipApplications'] });
      setReason('');
      setHoursPerWeek(undefined);
    } catch (error: unknown) {
      toast.error('Ошибка отправки заявки');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="rounded-2xl border border-gray-700/60 p-8 mb-8 bg-secondary">
        <h1 className="text-3xl font-bold text-white mb-3">Заявка на участника</h1>
        <p className="text-gray-400 mb-6">Заполните форму — администраторы получат заявку и рассмотрят её.</p>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-4">
          <div>
            <label className="text-gray-300 mb-2 block">Сервер</label>
            <div className="relative">
              <ServerDropdown
                servers={vipServers ?? []}
                value={selectedServerId}
                onChange={(id) => setSelectedServerId(id)}
              />
            </div>
          </div>

          <div>
            <label className="text-gray-300 mb-2 block">Ник в проекте</label>
            <input type="text" value={user?.username || ''} disabled className="w-full bg-gray-800/30 border border-gray-600/50 rounded-xl p-3 text-white" />
          </div>

          <div>
            <label className="text-gray-300 mb-2 block">Steam ID</label>
            <input type="text" value={user?.steamId || ''} disabled className="w-full bg-gray-800/30 border border-gray-600/50 rounded-xl p-3 text-white" />
          </div>

          <div>
            <label className="text-gray-300 mb-2 block">Сколько часов в неделю вы играете (примерно)</label>
            <input type="number" value={hoursPerWeek ?? ''} onChange={(e) => setHoursPerWeek(e.target.value ? Number(e.target.value) : undefined)} min={0} max={168} className="w-full bg-gray-800/30 border border-gray-600/50 rounded-xl p-3 text-white" />
          </div>
        </div>

        <div className="mb-4">
          <label className="text-gray-300 mb-2 block">Почему вы считаете, что должны стать участником проекта?</label>
          <textarea value={reason} onChange={(e) => setReason(e.target.value)} rows={6} className="w-full bg-gray-800/30 border border-gray-600/50 rounded-xl p-3 text-white" />
        </div>

        {isAuthenticated && selectedServerId && (
          (() => {
            const hasActiveVip = !!myVipPrivileges?.some(p => p.serverId === selectedServerId && p.isActive);
            const hasPending = !!myApplications?.some(a => a.serverId === selectedServerId && a.status === 'pending');

            if (hasActiveVip) {
              return (
                <div className="p-4 rounded-lg bg-red-900/30 border border-red-600/30 text-red-100">
                  Вы уже являетесь участником на этом сервере — вы не можете отправлять заявку.
                </div>
              );
            }

            if (hasPending) {
              return (
                <div className="p-4 rounded-lg bg-yellow-900/20 border border-yellow-600/30 text-yellow-100">
                  Вы уже отправили заявку для этого сервера — она находится в обработке администратора.
                </div>
              );
            }

            return (
              <div className="flex items-center justify-between">
                <div />
                <button onClick={handleSubmit} disabled={isSubmitting} className="bg-gradient-to-r from-highlight to-blue-500 hover:from-blue-500 hover:to-highlight text-white font-bold py-3 px-6 rounded-xl transition-all duration-200 disabled:opacity-50">
                  {isSubmitting ? 'Отправка...' : 'Отправить заявку'}
                </button>
              </div>
            );
          })()
        )}
        {!isAuthenticated && (
          <div className="flex items-center justify-between">
            <div />
            <button onClick={handleSubmit} disabled={isSubmitting} className="bg-gradient-to-r from-highlight to-blue-500 hover:from-blue-500 hover:to-highlight text-white font-bold py-3 px-6 rounded-xl transition-all duration-200 disabled:opacity-50">
              {isSubmitting ? 'Отправка...' : 'Отправить заявку'}
            </button>
          </div>
        )}
      </div>

      {myApplications && myApplications.length > 0 && (
        <div className="rounded-2xl border border-gray-700/60 p-6 bg-secondary">
          <h2 className="text-xl font-bold mb-3 text-white">Мои заявки</h2>
          <div className="space-y-3">
            {myApplications.map(app => (
              <div key={app.id} className="p-4 border border-gray-700/30 rounded-lg bg-gray-800/20">
                <div className="flex justify-between items-start">
                  <div>
                    <div className="text-white font-semibold">{app.serverName}</div>
                    <div className="text-gray-400 text-sm mt-1">Отправлено: {new Date(app.createdAt).toLocaleString()}</div>
                  </div>
                  <div className="text-sm px-3 py-1 rounded-full bg-gray-700/40 text-gray-200">{(() => {
                    const map: Record<string, string> = { pending: 'В ожидании', approved: 'Одобрена', rejected: 'Отклонена' };
                    return map[app.status] || app.status;
                  })()}</div>
                </div>
                <div className="mt-2 text-gray-300">Часов в неделю: {app.hoursPerWeek ?? '—'}</div>
                <div className="mt-2 text-gray-200">{app.reason}</div>
                {app.status !== 'pending' && app.processedAt && (
                  <div className="mt-2 text-xs text-gray-400">Обработано: {new Date(app.processedAt).toLocaleString()}</div>
                )}
                {app.adminComment && <div className="mt-2 text-sm text-red-300">Комментарий: {app.adminComment}</div>}
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}

type ServerItem = { serverId: number; serverName: string };

function ServerDropdown({ servers, value, onChange }: { servers: ServerItem[]; value: number | null; onChange: (id: number | null) => void }) {
  const [open, setOpen] = useState(false);
  const wrapperRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    const handleDocClick = (e: MouseEvent) => {
      if (open && wrapperRef.current && !wrapperRef.current.contains(e.target as Node)) {
        setOpen(false);
      }
    };
    document.addEventListener('mousedown', handleDocClick);
    return () => document.removeEventListener('mousedown', handleDocClick);
  }, [open]);

  const selected = servers.find(s => s.serverId === value) ?? null;

  return (
    <div className="relative" ref={wrapperRef}>
      <button
        type="button"
        onClick={() => setOpen(v => !v)}
        className="w-full bg-gray-800/30 border border-gray-600/50 rounded-xl px-3 py-3 text-white flex items-center justify-between hover:bg-gray-800/40 transition-colors duration-150"
        aria-haspopup="listbox"
        aria-expanded={open}
      >
        <div className="truncate text-left">
          {selected ? selected.serverName : <span className="text-gray-400">-- Выберите сервер --</span>}
        </div>
        <svg className={`w-4 h-4 ml-3 transform transition-transform duration-150 ${open ? 'rotate-180' : ''}`} viewBox="0 0 20 20" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
          <path fillRule="evenodd" d="M5.23 7.21a.75.75 0 011.06.02L10 11.584l3.71-4.354a.75.75 0 011.14.975l-4.25 5a.75.75 0 01-1.14 0l-4.25-5a.75.75 0 01.02-1.06z" clipRule="evenodd" />
        </svg>
      </button>

      {open && (
        <div className="absolute left-0 right-0 mt-2 bg-secondary rounded-xl shadow-lg max-h-56 overflow-y-auto border border-gray-700/60 z-40">
          {servers.length === 0 ? (
            <div className="p-3 text-sm text-gray-400">Нет доступных серверов</div>
          ) : (
            servers.map(s => (
              <button
                key={s.serverId}
                type="button"
                onClick={() => { onChange(s.serverId); setOpen(false); }}
                className={`w-full text-left px-4 py-3 hover:bg-secondary/70 transition-colors duration-150 flex items-center justify-between ${value === s.serverId ? 'bg-secondary/30' : ''}`}
              >
                <span className="truncate">{s.serverName}</span>
                {value === s.serverId && <span className="text-xs text-gray-300">Выбран</span>}
              </button>
            ))
          )}
        </div>
      )}
    </div>
  );
}
