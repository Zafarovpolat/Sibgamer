import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useState } from 'react';
import { getAllVipApplications, approveVipApplication, rejectVipApplication, getAllVipTariffs, getVipTariffOptions } from '../../lib/donationApi';
import toast from 'react-hot-toast';
import { formatServerDate } from '../../utils/dateUtils';

export default function AdminVipApplications() {
  const queryClient = useQueryClient();

  const { data: vipApplicationsPaged, isLoading } = useQuery({
    queryKey: ['vipApplicationsPage'],
    queryFn: () => getAllVipApplications(1, 1000),
  });

  const vipApplications = vipApplicationsPaged?.items || [];
  const { data: allVipTariffs } = useQuery({ queryKey: ['allVipTariffs'], queryFn: getAllVipTariffs });

  const [activeApp, setActiveApp] = useState<any | null>(null);
  const [modalMode, setModalMode] = useState<'view' | 'approve' | 'reject'>('view');
  const [selectedTariffId, setSelectedTariffId] = useState<number | null>(null);
  const [selectedOptionId, setSelectedOptionId] = useState<number | null>(null);
  const [rejectReason, setRejectReason] = useState('');

  const approveMutation = useMutation({
    mutationFn: ({ id, payload }: { id: number; payload: { vipGroup: string; durationDays: number; tariffId?: number | null; tariffOptionId?: number | null } }) => approveVipApplication(id, payload),
    onSuccess: () => {
      toast.success('Заявка одобрена и VIP выдан');
      queryClient.invalidateQueries({ queryKey: ['vipApplicationsPage'] });
      queryClient.invalidateQueries({ queryKey: ['adminVipPrivileges'] });
    },
    onError: () => toast.error('Ошибка при одобрении заявки'),
  });

  const rejectMutation = useMutation({
    mutationFn: ({ id, payload }: { id: number; payload: { reason: string } }) => rejectVipApplication(id, payload),
    onSuccess: () => {
      toast.success('Заявка отклонена');
      queryClient.invalidateQueries({ queryKey: ['vipApplicationsPage'] });
    },
    onError: () => toast.error('Ошибка при отклонении заявки'),
  });

  const serverTariffs = activeApp?.serverId ? (allVipTariffs || []).filter(t => t.serverId === activeApp.serverId) : [];

  const { data: tariffOptions = [], refetch: refetchTariffOptions } = useQuery({
    queryKey: ['vipTariffOptions', selectedTariffId],
    queryFn: () => selectedTariffId ? getVipTariffOptions(selectedTariffId) : Promise.resolve([]),
    enabled: !!selectedTariffId
  });

  const openModal = (app: any) => {
    setActiveApp(app);
    setModalMode('view');
    setSelectedTariffId(null);
    setSelectedOptionId(null);
    setRejectReason('');
  };

  const closeModal = () => {
    setActiveApp(null);
    setModalMode('view');
    setSelectedTariffId(null);
    setSelectedOptionId(null);
    setRejectReason('');
  };

  const formatDate = (dateString: string) => formatServerDate(dateString, { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' });

  return (
    <div className="p-6">
      <div className="mb-6 flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-white">Заявки на Участника</h1>
          <p className="text-gray-400 text-sm">Список заявок пользователей на получение Участника — одобрять/отклонять можно в деталях каждой заявки.</p>
        </div>
      </div>

      <div className="bg-secondary rounded-lg p-4 overflow-x-auto">
        {isLoading ? (
          <div className="text-gray-400 p-8">Загрузка...</div>
        ) : vipApplications.length === 0 ? (
          <div className="text-gray-400 p-8">Заявок нет</div>
        ) : (
          <table className="w-full text-sm text-left text-gray-200">
            <thead className="bg-primary text-xs uppercase">
              <tr>
                <th className="px-4 py-2">Пользователь</th>
                <th className="px-4 py-2">Steam ID</th>
                <th className="px-4 py-2">Сервер</th>
                <th className="px-4 py-2">Часы/нед</th>
                <th className="px-4 py-2">Причина</th>
                <th className="px-4 py-2">Статус</th>
                <th className="px-4 py-2">Дата</th>
                <th className="px-4 py-2">Действия</th>
              </tr>
            </thead>
            <tbody>
              {vipApplications.map(a => (
                <tr key={a.id} className="border-t border-gray-700/40 hover:bg-primary/10 transition-colors">
                  <td className="px-4 py-3 font-semibold text-white">{a.username}</td>
                  <td className="px-4 py-3"><code className="text-xs bg-primary px-2 py-1 rounded text-gray-300">{a.steamId}</code></td>
                  <td className="px-4 py-3">{a.serverName}</td>
                  <td className="px-4 py-3">{a.hoursPerWeek ?? '—'}</td>
                  <td className="px-4 py-3 text-gray-300 max-w-[320px] truncate">{a.reason}</td>
                  <td className="px-4 py-3">{(() => {
                    const map: Record<string, string> = { pending: 'В ожидании', approved: 'Одобрена', rejected: 'Отклонена' };
                    return map[a.status] || a.status;
                  })()}</td>
                  <td className="px-4 py-3">{a.createdAt ? formatDate(a.createdAt) : ''}</td>
                  <td className="px-4 py-3 flex gap-2 items-center">
                    <button className="bg-gray-700/30 hover:bg-gray-700/40 text-white px-3 py-1 rounded" onClick={() => openModal(a)}>Подробнее</button>
                    {a.status !== 'pending' && <div className="text-xs text-gray-400 py-1 px-2">Обработано</div>}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
      {activeApp && (
        <div className="modal-overlay" onMouseDown={(e) => { if (e.target === e.currentTarget) closeModal(); }}>
          <div className="modal-content max-w-3xl" onMouseDown={(e) => e.stopPropagation()}>
            <div className="flex justify-between items-center mb-4">
              <h2 className="text-2xl font-bold text-white">Заявка: {activeApp.username} — {activeApp.serverName}</h2>
              <button onClick={closeModal} className="text-gray-400 hover:text-white">✕</button>
            </div>

            {modalMode === 'view' && (
              <div className="space-y-4">
                <div className="grid grid-cols-2 gap-4">
                  <div>
                    <div className="text-sm text-gray-400">Пользователь</div>
                    <div className="text-white font-semibold">{activeApp.username}</div>
                  </div>
                  <div>
                    <div className="text-sm text-gray-400">Steam ID</div>
                    <div className="text-white">{activeApp.steamId}</div>
                  </div>
                  <div>
                    <div className="text-sm text-gray-400">Сервер</div>
                    <div className="text-white">{activeApp.serverName}</div>
                  </div>
                  <div>
                    <div className="text-sm text-gray-400">Часы/нед</div>
                    <div className="text-white">{activeApp.hoursPerWeek ?? '—'}</div>
                  </div>
                </div>

                <div>
                  <div className="text-sm text-gray-400">Причина</div>
                  <div className="text-gray-200 bg-gray-800/30 p-3 rounded mt-1">{activeApp.reason}</div>
                </div>

                <div className="flex justify-end gap-2 pt-4">
                  {activeApp.status === 'pending' && (
                    <>
                      <button onClick={() => setModalMode('reject')} className="bg-red-600 hover:bg-red-700 text-white px-4 py-2 rounded">Отклонить</button>
                      <button onClick={() => setModalMode('approve')} className="bg-green-600 hover:bg-green-700 text-white px-4 py-2 rounded">Одобрить</button>
                    </>
                  )}
                  <button onClick={closeModal} className="text-gray-400 px-4 py-2">Закрыть</button>
                </div>
              </div>
            )}

            {modalMode === 'reject' && (
              <div className="space-y-4">
                <div>
                  <div className="text-sm text-gray-400">Причина отказа</div>
                  <textarea value={rejectReason} onChange={(e) => setRejectReason(e.target.value)} rows={6} className="w-full bg-gray-800/30 border border-gray-600/50 rounded-xl p-3 text-white" />
                </div>
                <div className="flex justify-end gap-2 pt-4">
                  <button onClick={() => setModalMode('view')} className="text-gray-400 px-4 py-2">Отмена</button>
                  <button onClick={() => {
                    if (!rejectReason.trim()) { toast.error('Укажите причину отказа'); return; }
                    rejectMutation.mutate({ id: activeApp.id, payload: { reason: rejectReason.trim() } }, {
                      onSuccess: () => closeModal()
                    });
                  }} className="bg-red-600 hover:bg-red-700 text-white px-4 py-2 rounded">Подтвердить отказ</button>
                </div>
              </div>
            )}

            {modalMode === 'approve' && (
              <div className="space-y-4">
                <div>
                  <div className="text-sm text-gray-400">Доступные VIP-тарифы на сервере</div>
                  {serverTariffs.length === 0 ? (
                    <div className="text-yellow-400 p-4">На этом сервере пока нет VIP-тарифов — одобрить заявку можно только через существующие тарифы.</div>
                  ) : (
                    <div className="space-y-3 mt-3">
                      {serverTariffs.map(t => (
                        <div key={t.id} className={`p-3 rounded border ${selectedTariffId === t.id ? 'border-blue-500 bg-gray-700/40' : 'border-gray-700/30'}`}>
                          <div className="flex items-center justify-between">
                            <div>
                              <div className="flex items-center gap-3">
                                <div className="text-white font-semibold">{t.name}</div>
                                {!t.isActive && (
                                  <div className="text-xs inline-block bg-yellow-600/40 text-yellow-300 px-2 py-0.5 rounded">Неактивен</div>
                                )}
                              </div>
                              <div className="text-sm text-gray-400">Группа: {t.groupName || '—'}</div>
                            </div>
                            <div>
                              <button className="text-sm text-gray-300 px-3 py-1 rounded bg-primary/20" onClick={() => { setSelectedTariffId(t.id); setSelectedOptionId(null); refetchTariffOptions(); }}>Выбрать тариф</button>
                            </div>
                          </div>

                          {selectedTariffId === t.id && (
                            <div className="mt-3 space-y-2">
                              {tariffOptions.length === 0 ? (
                                <div className="text-gray-400 text-sm">В тарифе нет вариантов</div>
                              ) : (
                                <div className="space-y-2">
                                  {tariffOptions.map(opt => (
                                    <label key={opt.id} className={`w-full flex items-center justify-between p-2 rounded ${selectedOptionId === opt.id ? 'bg-blue-800/30' : 'bg-gray-800/10'}`}>
                                      <div>
                                        <div className="flex items-center gap-2">
                                          <div className="text-sm text-gray-200">{opt.durationDays === 0 ? 'Навсегда' : `${opt.durationDays} дней`}</div>
                                          {!opt.isActive && (
                                            <div className="text-xs inline-block bg-yellow-600/30 text-yellow-300 px-2 py-0.5 rounded">вариант неактивен</div>
                                          )}
                                        </div>
                                        <div className="text-xs text-gray-400">Цена: {opt.price}</div>
                                      </div>
                                      <input type="radio" name="vipOption" checked={selectedOptionId === opt.id} onChange={() => setSelectedOptionId(opt.id)} />
                                    </label>
                                  ))}
                                </div>
                              )}
                            </div>
                          )}
                        </div>
                      ))}
                    </div>
                  )}
                </div>

                <div className="text-sm text-gray-400">Выберите тариф и вариант (обязательное действие)</div>

                <div className="flex justify-end gap-2 pt-4">
                  <button onClick={() => setModalMode('view')} className="text-gray-400 px-4 py-2">Отмена</button>
                  <button onClick={async () => {
                    const opt = tariffOptions.find((o: any) => o.id === selectedOptionId);
                    const tariff = serverTariffs.find(t => t.id === selectedTariffId);
                    if (!tariff || !opt) { toast.error('Выберите тариф и вариант'); return; }
                    approveMutation.mutate({ id: activeApp.id, payload: { vipGroup: tariff.groupName || "", durationDays: opt.durationDays, tariffId: tariff.id, tariffOptionId: opt.id } }, { onSuccess: () => closeModal() });
                  }} className={`bg-green-600 hover:bg-green-700 text-white px-4 py-2 rounded ${!selectedOptionId ? 'opacity-60 cursor-not-allowed' : ''}`} disabled={!selectedOptionId}>Выдать</button>
                </div>
              </div>
            )}
          </div>
        </div>
      )}
    </div>
  );
}
