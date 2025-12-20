import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { getAllTariffs, createTariff, updateTariff, deleteTariff, getServersWithSourceBans, createTariffOption, updateTariffOption, deleteTariffOption, getTariffOptions, getAllVipTariffs, createVipTariff, updateVipTariff, deleteVipTariff, getServersWithVip, getVipTariffOptions, createVipTariffOption, updateVipTariffOption, deleteVipTariffOption } from '../../lib/donationApi';
import type { AdminTariff, CreateAdminTariff, AdminTariffOption, CreateAdminTariffOption, UpdateAdminTariffOption, UpdateAdminTariff, VipTariff, CreateVipTariff, VipTariffOption, CreateVipTariffOption, UpdateVipTariffOption, UpdateVipTariff } from '../../types';
import { Plus, Edit2, Trash2, Save, X, ShieldCheck, Settings, Crown } from 'lucide-react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faExclamationTriangle } from '@fortawesome/free-solid-svg-icons';
import toast from 'react-hot-toast';
import { getErrorMessage } from '../../utils/errorUtils';
import { formatServerDateOnly } from '../../utils/dateUtils';

interface TariffFormData extends Omit<CreateAdminTariff, 'serverId'> {
  serverId: number | '';
}

interface VipTariffFormData extends Omit<CreateVipTariff, 'serverId'> {
  serverId: number | '';
}

export default function AdminDonationTariffs() {
  const queryClient = useQueryClient();
  const [activeTab, setActiveTab] = useState<'admin' | 'vip'>('admin');
  const [isCreating, setIsCreating] = useState(false);
  const [editingTariff, setEditingTariff] = useState<AdminTariff | null>(null);
  const [showDeleteConfirm, setShowDeleteConfirm] = useState<number | null>(null);
  const [showDeleteVipConfirm, setShowDeleteVipConfirm] = useState<number | null>(null);
  const [isCreatingVip, setIsCreatingVip] = useState(false);
  const [editingVipTariff, setEditingVipTariff] = useState<VipTariff | null>(null);

  const [managingOptionsTariff, setManagingOptionsTariff] = useState<AdminTariff | null>(null);
  const [isCreatingOption, setIsCreatingOption] = useState(false);
  const [editingOption, setEditingOption] = useState<AdminTariffOption | null>(null);

  const [managingVipOptionsTariff, setManagingVipOptionsTariff] = useState<VipTariff | null>(null);
  const [isCreatingVipOption, setIsCreatingVipOption] = useState(false);
  const [editingVipOption, setEditingVipOption] = useState<VipTariffOption | null>(null);

  const emptyForm: TariffFormData = {
    serverId: '',
    name: '',
    description: '',
    flags: '',
    groupName: '',
    immunity: 0,
    isActive: true,
    order: 0,
  };

  const emptyVipForm: VipTariffFormData = {
    serverId: '',
    name: '',
    description: '',
    groupName: '',
    isActive: true,
    order: 0,
  };

  const emptyOptionForm: CreateAdminTariffOption = {
    durationDays: 30,
    price: 500,
    order: 0,
    isActive: true,
  };

  const emptyVipOptionForm: CreateVipTariffOption = {
    durationDays: 30,
    price: 500,
    order: 0,
    isActive: true,
  };

  const [formData, setFormData] = useState<TariffFormData>(emptyForm);
  const [vipFormData, setVipFormData] = useState<VipTariffFormData>(emptyVipForm);
  const [optionFormData, setOptionFormData] = useState<CreateAdminTariffOption>(emptyOptionForm);
  const [vipOptionFormData, setVipOptionFormData] = useState<CreateVipTariffOption>(emptyVipOptionForm);

  const { data: tariffs, isLoading: tariffsLoading } = useQuery({
    queryKey: ['adminTariffs'],
    queryFn: getAllTariffs,
  });

  const { data: serversWithSourceBans } = useQuery({
    queryKey: ['serversWithSourceBans'],
    queryFn: getServersWithSourceBans,
  });

  const { data: vipTariffs } = useQuery({
    queryKey: ['vipTariffs'],
    queryFn: getAllVipTariffs,
  });

  const { data: serversWithVip } = useQuery({
    queryKey: ['serversWithVip'],
    queryFn: getServersWithVip,
  });

  const { data: vipTariffOptions, isLoading: vipOptionsLoading } = useQuery({
    queryKey: ['vipTariffOptions', managingVipOptionsTariff?.id],
    queryFn: () => managingVipOptionsTariff ? getVipTariffOptions(managingVipOptionsTariff.id) : Promise.resolve([]),
    enabled: !!managingVipOptionsTariff,
  });

  const { data: tariffOptions, isLoading: optionsLoading } = useQuery({
    queryKey: ['tariffOptions', managingOptionsTariff?.id],
    queryFn: () => managingOptionsTariff ? getTariffOptions(managingOptionsTariff.id) : Promise.resolve([]),
    enabled: !!managingOptionsTariff,
  });

  const createMutation = useMutation({
    mutationFn: createTariff,
    onSuccess: () => {
      toast.success('Тариф создан');
      queryClient.invalidateQueries({ queryKey: ['adminTariffs'] });
      setIsCreating(false);
      setFormData(emptyForm);
    },
    onError: (error: unknown) => {
      toast.error(getErrorMessage(error) || 'Ошибка при создании тарифа');
    },
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, data }: { id: number; data: UpdateAdminTariff }) => updateTariff(id, data),
    onSuccess: () => {
      toast.success('Тариф обновлен');
      queryClient.invalidateQueries({ queryKey: ['adminTariffs'] });
      setEditingTariff(null);
      setFormData(emptyForm);
    },
    onError: (error: unknown) => {
      toast.error(getErrorMessage(error) || 'Ошибка при обновлении тарифа');
    },
  });

  const deleteMutation = useMutation({
    mutationFn: deleteTariff,
    onSuccess: () => {
      toast.success('Тариф удален');
      queryClient.invalidateQueries({ queryKey: ['adminTariffs'] });
      setShowDeleteConfirm(null);
    },
    onError: (error: unknown) => {
      toast.error(getErrorMessage(error) || 'Ошибка при удалении тарифа');
    },
  });

  const createOptionMutation = useMutation({
    mutationFn: ({ tariffId, data }: { tariffId: number; data: CreateAdminTariffOption }) =>
      createTariffOption(tariffId, data),
    onSuccess: () => {
      toast.success('Вариант тарифа создан');
      queryClient.invalidateQueries({ queryKey: ['tariffOptions', managingOptionsTariff?.id] });
      queryClient.invalidateQueries({ queryKey: ['adminTariffs'] });
      setIsCreatingOption(false);
      setOptionFormData(emptyOptionForm);
    },
    onError: (error: unknown) => {
      toast.error(getErrorMessage(error) || 'Ошибка при создании варианта');
    },
  });

  const updateOptionMutation = useMutation({
    mutationFn: ({ tariffId, optionId, data }: { tariffId: number; optionId: number; data: UpdateAdminTariffOption }) =>
      updateTariffOption(tariffId, optionId, data),
    onSuccess: () => {
      toast.success('Вариант тарифа обновлен');
      queryClient.invalidateQueries({ queryKey: ['tariffOptions', managingOptionsTariff?.id] });
      queryClient.invalidateQueries({ queryKey: ['adminTariffs'] });
      setEditingOption(null);
      setOptionFormData(emptyOptionForm);
    },
    onError: (error: unknown) => {
      toast.error(getErrorMessage(error) || 'Ошибка при обновлении варианта');
    },
  });

  const deleteOptionMutation = useMutation({
    mutationFn: ({ tariffId, optionId }: { tariffId: number; optionId: number }) =>
      deleteTariffOption(tariffId, optionId),
    onSuccess: () => {
      toast.success('Вариант тарифа удален');
      queryClient.invalidateQueries({ queryKey: ['tariffOptions', managingOptionsTariff?.id] });
      queryClient.invalidateQueries({ queryKey: ['adminTariffs'] });
    },
    onError: (error: unknown) => {
      toast.error(getErrorMessage(error) || 'Ошибка при удалении варианта');
    },
  });

  const createVipMutation = useMutation({
    mutationFn: createVipTariff,
    onSuccess: () => {
      toast.success('VIP тариф создан');
      queryClient.invalidateQueries({ queryKey: ['vipTariffs'] });
      setIsCreatingVip(false);
      setVipFormData(emptyVipForm);
    },
    onError: (error: unknown) => {
      toast.error(getErrorMessage(error) || 'Ошибка при создании VIP тарифа');
    },
  });

  const updateVipMutation = useMutation({
    mutationFn: ({ id, data }: { id: number; data: UpdateVipTariff }) => updateVipTariff(id, data),
    onSuccess: () => {
      toast.success('VIP тариф обновлен');
      queryClient.invalidateQueries({ queryKey: ['vipTariffs'] });
      setEditingVipTariff(null);
      setVipFormData(emptyVipForm);
    },
    onError: (error: unknown) => {
      toast.error(getErrorMessage(error) || 'Ошибка при обновлении VIP тарифа');
    },
  });

  const deleteVipMutation = useMutation({
    mutationFn: deleteVipTariff,
    onSuccess: () => {
      toast.success('VIP тариф удален');
      queryClient.invalidateQueries({ queryKey: ['vipTariffs'] });
      setShowDeleteVipConfirm(null);
    },
    onError: (error: unknown) => {
      toast.error(getErrorMessage(error) || 'Ошибка при удалении VIP тарифа');
    },
  });

  const createVipOptionMutation = useMutation({
    mutationFn: ({ tariffId, data }: { tariffId: number; data: CreateVipTariffOption }) =>
      createVipTariffOption(tariffId, data),
    onSuccess: () => {
      toast.success('Вариант VIP тарифа создан');
      queryClient.invalidateQueries({ queryKey: ['vipTariffOptions', managingVipOptionsTariff?.id] });
      queryClient.invalidateQueries({ queryKey: ['vipTariffs'] });
      setIsCreatingVipOption(false);
      setVipOptionFormData(emptyVipOptionForm);
    },
    onError: (error: unknown) => {
      toast.error(getErrorMessage(error) || 'Ошибка при создании варианта VIP тарифа');
    },
  });

  const updateVipOptionMutation = useMutation({
    mutationFn: ({ tariffId, optionId, data }: { tariffId: number; optionId: number; data: UpdateVipTariffOption }) =>
      updateVipTariffOption(tariffId, optionId, data),
    onSuccess: () => {
      toast.success('Вариант VIP тарифа обновлен');
      queryClient.invalidateQueries({ queryKey: ['vipTariffOptions', managingVipOptionsTariff?.id] });
      queryClient.invalidateQueries({ queryKey: ['vipTariffs'] });
      setEditingVipOption(null);
      setVipOptionFormData(emptyVipOptionForm);
    },
    onError: (error: unknown) => {
      toast.error(getErrorMessage(error) || 'Ошибка при обновлении варианта VIP тарифа');
    },
  });

  const deleteVipOptionMutation = useMutation({
    mutationFn: ({ tariffId, optionId }: { tariffId: number; optionId: number }) =>
      deleteVipTariffOption(tariffId, optionId),
    onSuccess: () => {
      toast.success('Вариант VIP тарифа удален');
      queryClient.invalidateQueries({ queryKey: ['vipTariffOptions', managingVipOptionsTariff?.id] });
      queryClient.invalidateQueries({ queryKey: ['vipTariffs'] });
    },
    onError: (error: unknown) => {
      toast.error(getErrorMessage(error) || 'Ошибка при удалении варианта VIP тарифа');
    },
  });

  const handleCreate = () => {
    if (formData.serverId === '' || !formData.name) {
      toast.error('Заполните обязательные поля корректно');
      return;
    }

    const hasFlags = formData.flags && formData.flags.trim() !== '';
    const hasGroup = formData.groupName && formData.groupName.trim() !== '';

    if (hasFlags && hasGroup) {
      toast.error('Укажите либо флаги, либо группу (не оба поля одновременно)');
      return;
    }

    if (!hasFlags && !hasGroup) {
      toast.error('Укажите либо флаги, либо группу (хотя бы одно поле)');
      return;
    }

    createMutation.mutate({
      ...formData,
      serverId: formData.serverId as number,
    });
  };

  const handleUpdate = () => {
    if (!editingTariff) return;

    const updateData: Partial<UpdateAdminTariff> = {};
    if (formData.name !== editingTariff.name) updateData.name = formData.name;
    if (formData.description !== editingTariff.description) updateData.description = formData.description;
    if (formData.flags !== editingTariff.flags) updateData.flags = formData.flags;
    if (formData.groupName !== editingTariff.groupName) updateData.groupName = formData.groupName;
    if (formData.immunity !== editingTariff.immunity) updateData.immunity = formData.immunity;
    if (formData.isActive !== editingTariff.isActive) updateData.isActive = formData.isActive;
    if (formData.order !== editingTariff.order) updateData.order = formData.order;

    const hasFlags = formData.flags && formData.flags.trim() !== '';
    const hasGroup = formData.groupName && formData.groupName.trim() !== '';

    if (hasFlags && hasGroup) {
      toast.error('Укажите либо флаги, либо группу (не оба поля одновременно)');
      return;
    }

    if (!hasFlags && !hasGroup) {
      toast.error('Укажите либо флаги, либо группу (хотя бы одно поле)');
      return;
    }

    if (Object.keys(updateData).length === 0) {
      toast.error('Нет изменений для сохранения');
      return;
    }

    updateMutation.mutate({ id: editingTariff.id, data: updateData });
  };

  const handleEdit = (tariff: AdminTariff) => {
    setEditingTariff(tariff);
    setFormData({
      serverId: tariff.serverId,
      name: tariff.name,
      description: tariff.description,
      flags: tariff.flags || '',
      groupName: tariff.groupName || '',
      immunity: tariff.immunity,
      isActive: tariff.isActive,
      order: tariff.order,
    });
    setIsCreating(false);
  };

  const handleCancel = () => {
    setIsCreating(false);
    setEditingTariff(null);
    setFormData(emptyForm);
  };

  const handleDelete = (id: number) => {
    deleteMutation.mutate(id);
  };

  const handleManageOptions = (tariff: AdminTariff) => {
    setManagingOptionsTariff(tariff);
    setIsCreatingOption(false);
    setEditingOption(null);
    setOptionFormData(emptyOptionForm);
  };

  const handleCreateOption = () => {
    if (!managingOptionsTariff) return;

    if (optionFormData.price <= 0) {
      toast.error('Цена должна быть больше 0');
      return;
    }

    if (optionFormData.durationDays < 0) {
      toast.error('Количество дней не может быть отрицательным');
      return;
    }

    createOptionMutation.mutate({
      tariffId: managingOptionsTariff.id,
      data: optionFormData,
    });
  };

  const handleUpdateOption = () => {
    if (!managingOptionsTariff || !editingOption) return;

    const updateData: UpdateAdminTariffOption = {};
    if (optionFormData.durationDays !== editingOption.durationDays) updateData.durationDays = optionFormData.durationDays;
    if (optionFormData.price !== editingOption.price) updateData.price = optionFormData.price;
    if (optionFormData.order !== editingOption.order) updateData.order = optionFormData.order;
    if (optionFormData.isActive !== editingOption.isActive) updateData.isActive = optionFormData.isActive;

    if (Object.keys(updateData).length === 0) {
      toast.error('Нет изменений для сохранения');
      return;
    }

    updateOptionMutation.mutate({
      tariffId: managingOptionsTariff.id,
      optionId: editingOption.id,
      data: updateData,
    });
  };

  const handleEditOption = (option: AdminTariffOption) => {
    setEditingOption(option);
    setOptionFormData({
      durationDays: option.durationDays,
      price: option.price,
      order: option.order,
      isActive: option.isActive,
    });
    setIsCreatingOption(false);
  };

  const handleCancelOptions = () => {
    setManagingOptionsTariff(null);
    setIsCreatingOption(false);
    setEditingOption(null);
    setOptionFormData(emptyOptionForm);
  };

  const handleCreateVip = () => {
    if (vipFormData.serverId === '' || !vipFormData.name) {
      toast.error('Заполните обязательные поля корректно');
      return;
    }

    createVipMutation.mutate({
      ...vipFormData,
      serverId: vipFormData.serverId as number,
    });
  };

  const handleUpdateVip = () => {
    if (!editingVipTariff) return;

    const updateData: Partial<UpdateVipTariff> = {};
    if (vipFormData.name !== editingVipTariff.name) updateData.name = vipFormData.name;
    if (vipFormData.description !== editingVipTariff.description) updateData.description = vipFormData.description;
    if (vipFormData.groupName !== editingVipTariff.groupName) updateData.groupName = vipFormData.groupName;
    if (vipFormData.isActive !== editingVipTariff.isActive) updateData.isActive = vipFormData.isActive;
    if (vipFormData.order !== editingVipTariff.order) updateData.order = vipFormData.order;

    if (Object.keys(updateData).length === 0) {
      toast.error('Нет изменений для сохранения');
      return;
    }

    updateVipMutation.mutate({ id: editingVipTariff.id, data: updateData });
  };

  const handleEditVipTariff = (tariff: VipTariff) => {
    setEditingVipTariff(tariff);
    setVipFormData({
      serverId: tariff.serverId,
      name: tariff.name,
      description: tariff.description,
      groupName: tariff.groupName || '',
      isActive: tariff.isActive,
      order: tariff.order,
    });
    setIsCreatingVip(false);
  };

  const handleCancelVip = () => {
    setIsCreatingVip(false);
    setEditingVipTariff(null);
    setVipFormData(emptyVipForm);
  };

  const handleDeleteVip = (id: number) => {
    deleteVipMutation.mutate(id);
  };

  const handleManageVipOptions = (tariff: VipTariff) => {
    setManagingVipOptionsTariff(tariff);
    setIsCreatingVipOption(false);
    setEditingVipOption(null);
    setVipOptionFormData(emptyVipOptionForm);
  };

  const handleCreateVipOption = () => {
    if (!managingVipOptionsTariff) return;

    if (vipOptionFormData.price <= 0) {
      toast.error('Цена должна быть больше 0');
      return;
    }

    if (vipOptionFormData.durationDays < 0) {
      toast.error('Количество дней не может быть отрицательным');
      return;
    }

    createVipOptionMutation.mutate({
      tariffId: managingVipOptionsTariff.id,
      data: vipOptionFormData,
    });
  };

  const handleUpdateVipOption = () => {
    if (!managingVipOptionsTariff || !editingVipOption) return;

    const updateData: UpdateVipTariffOption = {};
    if (vipOptionFormData.durationDays !== editingVipOption.durationDays) updateData.durationDays = vipOptionFormData.durationDays;
    if (vipOptionFormData.price !== editingVipOption.price) updateData.price = vipOptionFormData.price;
    if (vipOptionFormData.order !== editingVipOption.order) updateData.order = vipOptionFormData.order;
    if (vipOptionFormData.isActive !== editingVipOption.isActive) updateData.isActive = vipOptionFormData.isActive;

    if (Object.keys(updateData).length === 0) {
      toast.error('Нет изменений для сохранения');
      return;
    }

    updateVipOptionMutation.mutate({
      tariffId: managingVipOptionsTariff.id,
      optionId: editingVipOption.id,
      data: updateData,
    });
  };

  const handleEditVipOption = (option: VipTariffOption) => {
    setEditingVipOption(option);
    setVipOptionFormData({
      durationDays: option.durationDays,
      price: option.price,
      order: option.order,
      isActive: option.isActive,
    });
    setIsCreatingVipOption(false);
  };

  const handleCancelVipOptions = () => {
    setManagingVipOptionsTariff(null);
    setIsCreatingVipOption(false);
    setEditingVipOption(null);
    setVipOptionFormData(emptyVipOptionForm);
  };

  if (tariffsLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="text-gray-400">Загрузка...</div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold text-white mb-2">
            {activeTab === 'admin' ? 'Управление тарифами' : 'Управление VIP тарифами'}
          </h1>
          <p className="text-gray-400">
            {activeTab === 'admin' 
              ? 'Создавайте и редактируйте тарифы для покупки админ-прав' 
              : 'Создавайте и редактируйте VIP тарифы для покупки привилегий'
            }
          </p>
        </div>
        <button
          onClick={() => {
            if (activeTab === 'admin') {
              setIsCreating(true);
              setEditingTariff(null);
              setFormData(emptyForm);
            } else {
              setIsCreatingVip(true);
              setEditingVipTariff(null);
              setVipFormData(emptyVipForm);
            }
          }}
          className="bg-accent hover:bg-accent-dark text-white font-bold py-3 px-6 rounded-lg transition-colors flex items-center gap-2"
        >
          <Plus className="w-5 h-5" />
          {activeTab === 'admin' ? 'Создать тариф' : 'Создать VIP тариф'}
        </button>
      </div>

      <div className="flex space-x-1 bg-primary/50 p-1 rounded-lg">
        <button
          onClick={() => setActiveTab('admin')}
          className={`flex-1 py-2 px-4 rounded-md text-sm font-medium transition-colors flex items-center justify-center gap-2 ${
            activeTab === 'admin'
              ? 'bg-accent text-white'
              : 'text-gray-400 hover:text-white hover:bg-primary/30'
          }`}
        >
          <ShieldCheck className="w-4 h-4" />
          Админ тарифы
        </button>
        <button
          onClick={() => setActiveTab('vip')}
          className={`flex-1 py-2 px-4 rounded-md text-sm font-medium transition-colors flex items-center justify-center gap-2 ${
            activeTab === 'vip'
              ? 'bg-accent text-white'
              : 'text-gray-400 hover:text-white hover:bg-primary/30'
          }`}
        >
          <Crown className="w-4 h-4" />
          VIP тарифы
        </button>
      </div>

      {activeTab === 'admin' && (isCreating || editingTariff) && (
        <div className="bg-secondary rounded-lg p-6 border-2 border-accent">
          <div className="flex items-center justify-between mb-6">
            <h2 className="text-2xl font-bold text-white">
              {editingTariff ? 'Редактировать тариф' : 'Создать новый тариф'}
            </h2>
            <button
              onClick={handleCancel}
              className="text-gray-400 hover:text-white transition-colors"
            >
              <X className="w-6 h-6" />
            </button>
          </div>

          <div className="space-y-6">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label className="block text-gray-300 mb-2 font-semibold">
                  Сервер <span className="text-red-500">*</span>
                </label>
                <select
                  value={formData.serverId}
                  onChange={(e) => setFormData({ ...formData, serverId: e.target.value ? parseInt(e.target.value) : '' })}
                  disabled={!!editingTariff}
                  className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent disabled:opacity-50"
                >
                  <option value="">Выберите сервер</option>
                  {serversWithSourceBans?.map((server) => (
                    <option key={server.id} value={server.id}>
                      {server.name} ({server.ipAddress})
                    </option>
                  ))}
                </select>
              </div>

              <div>
                <label className="block text-gray-300 mb-2 font-semibold">
                  Название тарифа <span className="text-red-500">*</span>
                </label>
                <input
                  type="text"
                  value={formData.name}
                  onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                  placeholder="VIP на 30 дней"
                  className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                />
              </div>
            </div>

            <div>
              <label className="block text-gray-300 mb-2 font-semibold">
                Описание
              </label>
              <textarea
                value={formData.description}
                onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                placeholder="Полный доступ к админским командам..."
                rows={3}
                className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent resize-none"
              />
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label className="block text-gray-300 mb-2 font-semibold">
                  Порядок отображения
                </label>
                <input
                  type="number"
                  min="0"
                  value={formData.order}
                  onChange={(e) => setFormData({ ...formData, order: parseInt(e.target.value) || 0 })}
                  placeholder="0"
                  className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                />
              </div>
            </div>

            <div className="space-y-4">
              <div className="bg-primary/50 border border-yellow-500/30 rounded-lg p-4">
                <p className="text-yellow-400 text-sm font-semibold mb-2 flex items-center gap-2">
                  <FontAwesomeIcon icon={faExclamationTriangle} /> Правила SourceBans:
                </p>
                <p className="text-gray-300 text-sm">
                  Укажите <strong>либо флаги, либо группу</strong> (не оба поля одновременно). 
                  Это требование SourceBans для корректной работы админ-прав на сервере.
                </p>
              </div>

              <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                <div>
                  <label className="block text-gray-300 mb-2 font-semibold">
                    Флаги (flags) <span className="text-yellow-500">*</span>
                  </label>
                  <input
                    type="text"
                    value={formData.flags}
                    onChange={(e) => setFormData({ ...formData, flags: e.target.value })}
                    placeholder="abcdefghijklmnopqrst"
                    className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                  />
                  <p className="text-xs text-gray-500 mt-1">Флаги доступа SourceMod (например: z для Root)</p>
                </div>

                <div className="flex items-center justify-center">
                  <span className="text-gray-500 font-bold text-lg">ИЛИ</span>
                </div>

                <div>
                  <label className="block text-gray-300 mb-2 font-semibold">
                    Группа (group) <span className="text-yellow-500">*</span>
                  </label>
                  <input
                    type="text"
                    value={formData.groupName}
                    onChange={(e) => setFormData({ ...formData, groupName: e.target.value })}
                    placeholder="VIP"
                    className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                  />
                  <p className="text-xs text-gray-500 mt-1">Группа в SourceBans (например: VIP, Admin)</p>
                </div>
              </div>

              <div>
                <label className="block text-gray-300 mb-2 font-semibold">
                  Иммунитет
                </label>
                <div className="relative max-w-xs">
                  <ShieldCheck className="absolute left-3 top-1/2 transform -translate-y-1/2 w-5 h-5 text-gray-400" />
                  <input
                    type="number"
                    min="0"
                    max="100"
                    value={formData.immunity}
                    onChange={(e) => setFormData({ ...formData, immunity: parseInt(e.target.value) || 0 })}
                    placeholder="0"
                    className="w-full bg-primary border border-gray-700 rounded-lg pl-10 pr-4 py-3 text-white focus:outline-none focus:border-accent"
                  />
                </div>
                <p className="text-xs text-gray-500 mt-1">Уровень защиты от других админов (0-100)</p>
              </div>
            </div>

            <div className="flex items-center gap-3">
              <input
                type="checkbox"
                id="isActive"
                checked={formData.isActive}
                onChange={(e) => setFormData({ ...formData, isActive: e.target.checked })}
                className="w-5 h-5 bg-primary border border-gray-700 rounded focus:ring-2 focus:ring-accent"
              />
              <label htmlFor="isActive" className="text-gray-300 font-semibold cursor-pointer">
                Тариф активен (показывать на сайте)
              </label>
            </div>

            <div className="flex gap-4">
              <button
                onClick={editingTariff ? handleUpdate : handleCreate}
                disabled={createMutation.isPending || updateMutation.isPending}
                className="flex-1 bg-accent hover:bg-accent-dark text-white font-bold py-3 px-6 rounded-lg transition-colors disabled:opacity-50 flex items-center justify-center gap-2"
              >
                <Save className="w-5 h-5" />
                {createMutation.isPending || updateMutation.isPending ? 'Сохранение...' : 'Сохранить'}
              </button>
              <button
                onClick={handleCancel}
                className="px-6 py-3 bg-gray-700 hover:bg-gray-600 text-white font-bold rounded-lg transition-colors"
              >
                Отмена
              </button>
            </div>
          </div>
        </div>
      )}

      {(isCreatingVip || editingVipTariff) && activeTab === 'vip' && (
        <div className="bg-secondary rounded-lg p-6 border-2 border-accent">
          <div className="flex items-center justify-between mb-6">
            <h2 className="text-2xl font-bold text-white">
              {editingVipTariff ? 'Редактировать VIP тариф' : 'Создать новый VIP тариф'}
            </h2>
            <button
              onClick={() => {
                setIsCreatingVip(false);
                setEditingVipTariff(null);
                setVipFormData(emptyVipForm);
              }}
              className="text-gray-400 hover:text-white transition-colors"
            >
              <X className="w-6 h-6" />
            </button>
          </div>

          <div className="space-y-6">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label className="block text-gray-300 mb-2 font-semibold">
                  Сервер <span className="text-red-500">*</span>
                </label>
                <select
                  value={vipFormData.serverId}
                  onChange={(e) => setVipFormData({ ...vipFormData, serverId: e.target.value ? parseInt(e.target.value) : '' })}
                  disabled={!!editingVipTariff}
                  className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent disabled:opacity-50"
                >
                  <option value="">Выберите сервер</option>
                  {serversWithVip?.map((server) => (
                    <option key={server.id} value={server.id}>
                      {server.name} ({server.ipAddress})
                    </option>
                  ))}
                </select>
              </div>

              <div>
                <label className="block text-gray-300 mb-2 font-semibold">
                  Название VIP тарифа <span className="text-red-500">*</span>
                </label>
                <input
                  type="text"
                  value={vipFormData.name}
                  onChange={(e) => setVipFormData({ ...vipFormData, name: e.target.value })}
                  placeholder="VIP на 30 дней"
                  className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                />
              </div>
            </div>

            <div>
              <label className="block text-gray-300 mb-2 font-semibold">
                Описание
              </label>
              <textarea
                value={vipFormData.description}
                onChange={(e) => setVipFormData({ ...vipFormData, description: e.target.value })}
                placeholder="Полный доступ к VIP командам..."
                rows={3}
                className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent resize-none"
              />
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label className="block text-gray-300 mb-2 font-semibold">
                  Порядок отображения
                </label>
                <input
                  type="number"
                  min="0"
                  value={vipFormData.order}
                  onChange={(e) => setVipFormData({ ...vipFormData, order: parseInt(e.target.value) || 0 })}
                  placeholder="0"
                  className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                />
              </div>

              <div>
                <label className="block text-gray-300 mb-2 font-semibold">
                  Группа VIP <span className="text-red-500">*</span>
                </label>
                <input
                  type="text"
                  value={vipFormData.groupName}
                  onChange={(e) => setVipFormData({ ...vipFormData, groupName: e.target.value })}
                  placeholder="vip"
                  className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                />
                <p className="text-xs text-gray-500 mt-1">Название группы в VIP базе данных сервера</p>
              </div>
            </div>

            <div className="flex items-center gap-3">
              <input
                type="checkbox"
                id="vipIsActive"
                checked={vipFormData.isActive}
                onChange={(e) => setVipFormData({ ...vipFormData, isActive: e.target.checked })}
                className="w-5 h-5 bg-primary border border-gray-700 rounded focus:ring-2 focus:ring-accent"
              />
              <label htmlFor="vipIsActive" className="text-gray-300 font-semibold cursor-pointer">
                VIP тариф активен (показывать на сайте)
              </label>
            </div>

            <div className="flex gap-4">
              <button
                onClick={editingVipTariff ? handleUpdateVip : handleCreateVip}
                disabled={createVipMutation.isPending || updateVipMutation.isPending}
                className="flex-1 bg-accent hover:bg-accent-dark text-white font-bold py-3 px-6 rounded-lg transition-colors disabled:opacity-50 flex items-center justify-center gap-2"
              >
                <Save className="w-5 h-5" />
                {createVipMutation.isPending || updateVipMutation.isPending ? 'Сохранение...' : 'Сохранить'}
              </button>
              <button
                onClick={handleCancelVip}
                className="px-6 py-3 bg-gray-700 hover:bg-gray-600 text-white font-bold rounded-lg transition-colors"
              >
                Отмена
              </button>
            </div>
          </div>
        </div>
      )}

      {activeTab === 'admin' && (
        <div className="space-y-6">
          {tariffs && tariffs.length > 0 ? (
            tariffs
              .sort((a, b) => a.serverId - b.serverId || a.order - b.order)
              .map((tariff) => (
                <div
                  key={tariff.id}
                  className={`bg-secondary rounded-lg p-6 border-2 transition-all ${
                    tariff.isActive ? 'border-gray-700' : 'border-gray-800 opacity-60'
                  }`}
                >
                      <div className="flex items-start justify-between">
                        <div className="flex-1">
                          <div className="flex items-center gap-3 mb-2">
                            <h4 className="text-lg font-bold text-white">{tariff.name}</h4>
                            {!tariff.isActive && (
                              <span className="text-xs bg-gray-700 text-gray-400 px-2 py-1 rounded">
                                Неактивен
                              </span>
                            )}
                            <span className="text-sm text-gray-400">
                              {(tariff.options?.length || 0)} {(tariff.options?.length || 0) === 1 ? 'вариант' : (tariff.options?.length || 0) < 5 ? 'варианта' : 'вариантов'}
                            </span>
                          </div>
                          {tariff.description && (
                            <p className="text-gray-400 text-sm mb-2">{tariff.description}</p>
                          )}
                          
                          {tariff.options && tariff.options.length > 0 && (
                            <div className="mb-3">
                              <p className="text-xs text-gray-500 mb-2">Варианты:</p>
                              <div className="flex flex-wrap gap-2">
                                {tariff.options
                                  .filter(option => option.isActive)
                                  .sort((a, b) => a.order - b.order)
                                  .map(option => (
                                    <span key={option.id} className="text-xs bg-primary/50 px-2 py-1 rounded border border-gray-600">
                                      {option.durationDays === 0 
                                        ? 'Навсегда' 
                                        : `${option.durationDays} ${option.durationDays === 1 ? 'день' : option.durationDays < 5 ? 'дня' : 'дней'}`
                                      } - <span className="text-yellow-500 font-bold">{option.price} ₽</span>
                                    </span>
                                  ))}
                              </div>
                            </div>
                          )}

                          <div className="flex flex-wrap gap-4 text-sm text-gray-400">
                            <span>Сервер: <code className="text-xs bg-primary px-2 py-0.5 rounded">{tariff.serverName}</code></span>
                            {tariff.flags && (
                              <span>Флаги: <code className="text-xs bg-primary px-2 py-0.5 rounded">{tariff.flags}</code></span>
                            )}
                            {tariff.groupName && (
                              <span>Группа: <code className="text-xs bg-primary px-2 py-0.5 rounded">{tariff.groupName}</code></span>
                            )}
                            {tariff.immunity > 0 && (
                              <span className="flex items-center gap-1">
                                <ShieldCheck className="w-4 h-4" />
                                Иммунитет: {tariff.immunity}
                              </span>
                            )}
                          </div>
                        </div>
                        <div className="flex gap-2 ml-4">
                          <button
                            onClick={() => handleManageOptions(tariff)}
                            className="p-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg transition-colors"
                            title="Управление вариантами"
                          >
                            <Settings className="w-5 h-5" />
                          </button>
                          <button
                            onClick={() => handleEdit(tariff)}
                            className="p-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg transition-colors"
                            title="Редактировать"
                          >
                            <Edit2 className="w-5 h-5" />
                          </button>
                          <button
                            onClick={() => setShowDeleteConfirm(tariff.id)}
                            className="p-2 bg-red-600 hover:bg-red-700 text-white rounded-lg transition-colors"
                            title="Удалить"
                          >
                            <Trash2 className="w-5 h-5" />
                          </button>
                        </div>
                      </div>
                    </div>
                    ))
          ) : (
            <div className="bg-secondary rounded-lg p-12 text-center">
              <p className="text-gray-400 text-lg">Тарифов пока нет. Создайте первый тариф!</p>
            </div>
          )}
        </div>
      )}

      {activeTab === 'vip' && (
        <div className="space-y-6">
          {vipTariffs && vipTariffs.length > 0 ? (
            vipTariffs
              .sort((a, b) => a.order - b.order)
              .map((tariff) => (
                <div
                  key={tariff.id}
                  className={`bg-secondary rounded-lg p-6 border-2 transition-all ${
                    tariff.isActive ? 'border-gray-700' : 'border-gray-800 opacity-60'
                  }`}
                >
                  <div className="flex items-start justify-between">
                    <div className="flex-1">
                      <div className="flex items-center gap-3 mb-2">
                        <h4 className="text-lg font-bold text-white">{tariff.name}</h4>
                        {!tariff.isActive && (
                          <span className="text-xs bg-gray-700 text-gray-400 px-2 py-1 rounded">
                            Неактивен
                          </span>
                        )}
                        <span className="text-sm text-gray-400">
                          {(tariff.options?.length || 0)} {(tariff.options?.length || 0) === 1 ? 'вариант' : (tariff.options?.length || 0) < 5 ? 'варианта' : 'вариантов'}
                        </span>
                      </div>
                      {tariff.description && (
                        <p className="text-gray-400 text-sm mb-2">{tariff.description}</p>
                      )}
                      
                      {tariff.options && tariff.options.length > 0 && (
                        <div className="mb-3">
                          <p className="text-xs text-gray-500 mb-2">Варианты:</p>
                          <div className="flex flex-wrap gap-2">
                            {tariff.options
                              .filter(option => option.isActive)
                              .sort((a, b) => a.order - b.order)
                              .map(option => (
                                <span key={option.id} className="text-xs bg-primary/50 px-2 py-1 rounded border border-gray-600">
                                  {option.durationDays === 0 
                                    ? 'Навсегда' 
                                    : `${option.durationDays} ${option.durationDays === 1 ? 'день' : option.durationDays < 5 ? 'дня' : 'дней'}`
                                  } - <span className="text-yellow-500 font-bold">{option.price} ₽</span>
                                </span>
                              ))}
                          </div>
                        </div>
                      )}

                      <div className="flex flex-wrap gap-4 text-sm text-gray-400">
                        <span>Сервер: <code className="text-xs bg-primary px-2 py-0.5 rounded">{tariff.serverName}</code></span>
                        {tariff.groupName && (
                          <span>Группа: <code className="text-xs bg-primary px-2 py-0.5 rounded">{tariff.groupName}</code></span>
                        )}
                      </div>
                    </div>
                    <div className="flex gap-2 ml-4">
                      <button
                        onClick={() => handleManageVipOptions(tariff)}
                        className="p-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg transition-colors"
                        title="Управление вариантами"
                      >
                        <Settings className="w-5 h-5" />
                      </button>
                      <button
                        onClick={() => handleEditVipTariff(tariff)}
                        className="p-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg transition-colors"
                        title="Редактировать"
                      >
                        <Edit2 className="w-5 h-5" />
                      </button>
                      <button
                        onClick={() => setShowDeleteVipConfirm(tariff.id)}
                        className="p-2 bg-red-600 hover:bg-red-700 text-white rounded-lg transition-colors"
                        title="Удалить"
                      >
                        <Trash2 className="w-5 h-5" />
                      </button>
                    </div>
                  </div>
                </div>
              ))
          ) : (
            <div className="bg-secondary rounded-lg p-12 text-center">
              <p className="text-gray-400 text-lg">VIP тарифов пока нет. Создайте первый VIP тариф!</p>
            </div>
          )}
        </div>
      )}

      {showDeleteConfirm && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50 p-4">
          <div className="bg-secondary rounded-lg p-6 max-w-md w-full">
            <h3 className="text-xl font-bold text-white mb-4">Подтверждение удаления</h3>
            <p className="text-gray-400 mb-6">
              Вы уверены, что хотите удалить этот тариф? Это действие необратимо.
            </p>
            <div className="flex gap-4">
              <button
                onClick={() => handleDelete(showDeleteConfirm)}
                disabled={deleteMutation.isPending}
                className="flex-1 bg-red-600 hover:bg-red-700 text-white font-bold py-3 px-6 rounded-lg transition-colors disabled:opacity-50"
              >
                {deleteMutation.isPending ? 'Удаление...' : 'Удалить'}
              </button>
              <button
                onClick={() => setShowDeleteConfirm(null)}
                className="flex-1 bg-gray-700 hover:bg-gray-600 text-white font-bold py-3 px-6 rounded-lg transition-colors"
              >
                Отмена
              </button>
            </div>
          </div>
        </div>
      )}

      {managingOptionsTariff && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50 p-4">
          <div className="bg-secondary rounded-lg max-w-4xl w-full max-h-[90vh] overflow-hidden">
            <div className="flex items-center justify-between p-6 border-b border-gray-700">
              <div>
                <h3 className="text-2xl font-bold text-white">Управление вариантами тарифа</h3>
                <p className="text-gray-400 mt-1">{managingOptionsTariff.name}</p>
              </div>
              <button
                onClick={handleCancelOptions}
                className="text-gray-400 hover:text-white transition-colors"
              >
                <X className="w-6 h-6" />
              </button>
            </div>

            <div className="p-6 overflow-y-auto max-h-[calc(90vh-120px)]">
              {(isCreatingOption || editingOption) && (
                <div className="bg-primary/50 rounded-lg p-6 border-2 border-accent mb-6">
                  <div className="flex items-center justify-between mb-6">
                    <h4 className="text-xl font-bold text-white">
                      {editingOption ? 'Редактировать вариант' : 'Создать новый вариант'}
                    </h4>
                    <button
                      onClick={() => {
                        setIsCreatingOption(false);
                        setEditingOption(null);
                        setOptionFormData(emptyOptionForm);
                      }}
                      className="text-gray-400 hover:text-white transition-colors"
                    >
                      <X className="w-5 h-5" />
                    </button>
                  </div>

                  <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                    <div>
                      <label className="block text-gray-300 mb-2 font-semibold">
                        Длительность <span className="text-red-500">*</span>
                      </label>
                      <select
                        value={optionFormData.durationDays}
                        onChange={(e) => setOptionFormData({ ...optionFormData, durationDays: parseInt(e.target.value) })}
                        className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                      >
                        <option value={0}>Навсегда</option>
                        <option value={1}>1 день</option>
                        <option value={7}>7 дней</option>
                        <option value={14}>14 дней</option>
                        <option value={30}>30 дней</option>
                        <option value={60}>60 дней</option>
                        <option value={90}>90 дней</option>
                        <option value={180}>180 дней</option>
                        <option value={365}>365 дней</option>
                      </select>
                    </div>

                    <div>
                      <label className="block text-gray-300 mb-2 font-semibold">
                        Цена (₽) <span className="text-red-500">*</span>
                      </label>
                      <input
                        type="number"
                        min="1"
                        value={optionFormData.price}
                        onChange={(e) => setOptionFormData({ ...optionFormData, price: parseInt(e.target.value) || 0 })}
                        placeholder="500"
                        className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                      />
                    </div>

                    <div>
                      <label className="block text-gray-300 mb-2 font-semibold">
                        Порядок отображения
                      </label>
                      <input
                        type="number"
                        min="0"
                        value={optionFormData.order}
                        onChange={(e) => setOptionFormData({ ...optionFormData, order: parseInt(e.target.value) || 0 })}
                        placeholder="0"
                        className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                      />
                    </div>
                  </div>

                  <div className="flex items-center gap-3 mt-6">
                    <input
                      type="checkbox"
                      id="optionIsActive"
                      checked={optionFormData.isActive}
                      onChange={(e) => setOptionFormData({ ...optionFormData, isActive: e.target.checked })}
                      className="w-5 h-5 bg-primary border border-gray-700 rounded focus:ring-2 focus:ring-accent"
                    />
                    <label htmlFor="optionIsActive" className="text-gray-300 font-semibold cursor-pointer">
                      Вариант активен (показывать на сайте)
                    </label>
                  </div>

                  <div className="flex gap-4 mt-6">
                    <button
                      onClick={editingOption ? handleUpdateOption : handleCreateOption}
                      disabled={createOptionMutation.isPending || updateOptionMutation.isPending}
                      className="flex-1 bg-accent hover:bg-accent-dark text-white font-bold py-3 px-6 rounded-lg transition-colors disabled:opacity-50 flex items-center justify-center gap-2"
                    >
                      <Save className="w-5 h-5" />
                      {createOptionMutation.isPending || updateOptionMutation.isPending ? 'Сохранение...' : 'Сохранить'}
                    </button>
                    <button
                      onClick={() => {
                        setIsCreatingOption(false);
                        setEditingOption(null);
                        setOptionFormData(emptyOptionForm);
                      }}
                      className="px-6 py-3 bg-gray-700 hover:bg-gray-600 text-white font-bold rounded-lg transition-colors"
                    >
                      Отмена
                    </button>
                  </div>
                </div>
              )}

              <div className="space-y-4">
                <div className="flex items-center justify-between">
                  <h4 className="text-lg font-bold text-white">Варианты тарифа</h4>
                  <button
                    onClick={() => {
                      setIsCreatingOption(true);
                      setEditingOption(null);
                      setOptionFormData(emptyOptionForm);
                    }}
                    className="bg-accent hover:bg-accent-dark text-white font-bold py-2 px-4 rounded-lg transition-colors flex items-center gap-2"
                  >
                    <Plus className="w-4 h-4" />
                    Добавить вариант
                  </button>
                </div>

                {optionsLoading ? (
                  <div className="text-center py-8">
                    <div className="text-gray-400">Загрузка вариантов...</div>
                  </div>
                ) : tariffOptions && tariffOptions.length > 0 ? (
                  <div className="space-y-3">
                    {tariffOptions
                      .sort((a: AdminTariffOption, b: AdminTariffOption) => a.order - b.order)
                      .map((option: AdminTariffOption) => (
                        <div
                          key={option.id}
                          className={`border-2 rounded-lg p-4 transition-all ${
                            option.isActive ? 'border-gray-700' : 'border-gray-800 opacity-60'
                          }`}
                        >
                          <div className="flex items-center justify-between">
                            <div className="flex-1">
                              <div className="flex items-center gap-3 mb-2">
                                <span className="text-lg font-bold text-white">
                                  {option.durationDays === 0
                                    ? 'Навсегда'
                                    : `${option.durationDays} ${option.durationDays === 1 ? 'день' : option.durationDays < 5 ? 'дня' : 'дней'}`}
                                </span>
                                <span className="text-xl font-bold text-yellow-500">{option.price} ₽</span>
                                {!option.isActive && (
                                  <span className="text-xs bg-gray-700 text-gray-400 px-2 py-1 rounded">
                                    Неактивен
                                  </span>
                                )}
                              </div>
                              <div className="text-sm text-gray-400">
                                Порядок: {option.order} • Создан: {formatServerDateOnly(option.createdAt)}
                              </div>
                            </div>
                            <div className="flex gap-2">
                              <button
                                onClick={() => handleEditOption(option)}
                                className="p-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg transition-colors"
                                title="Редактировать"
                              >
                                <Edit2 className="w-4 h-4" />
                              </button>
                              <button
                                onClick={() => {
                                  if (managingOptionsTariff) {
                                    deleteOptionMutation.mutate({
                                      tariffId: managingOptionsTariff.id,
                                      optionId: option.id,
                                    });
                                  }
                                }}
                                className="p-2 bg-red-600 hover:bg-red-700 text-white rounded-lg transition-colors"
                                title="Удалить"
                              >
                                <Trash2 className="w-4 h-4" />
                              </button>
                            </div>
                          </div>
                        </div>
                      ))}
                  </div>
                ) : (
                  <div className="bg-primary/50 border-2 border-dashed border-gray-600 rounded-lg p-8 text-center">
                    <p className="text-gray-400 text-lg">Вариантов пока нет</p>
                    <p className="text-gray-500 text-sm mt-2">Добавьте первый вариант тарифа</p>
                  </div>
                )}
              </div>
            </div>
          </div>
        </div>
      )}

      {managingVipOptionsTariff && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50 p-4">
          <div className="bg-secondary rounded-lg max-w-4xl w-full max-h-[90vh] overflow-hidden">
            <div className="flex items-center justify-between p-6 border-b border-gray-700">
              <div>
                <h3 className="text-2xl font-bold text-white">Управление вариантами VIP тарифа</h3>
                <p className="text-gray-400 mt-1">{managingVipOptionsTariff.name}</p>
              </div>
              <button
                onClick={handleCancelVipOptions}
                className="text-gray-400 hover:text-white transition-colors"
              >
                <X className="w-6 h-6" />
              </button>
            </div>

            <div className="p-6 overflow-y-auto max-h-[calc(90vh-120px)]">
              {(isCreatingVipOption || editingVipOption) && (
                <div className="bg-primary/50 rounded-lg p-6 border-2 border-accent mb-6">
                  <div className="flex items-center justify-between mb-6">
                    <h4 className="text-xl font-bold text-white">
                      {editingVipOption ? 'Редактировать вариант VIP тарифа' : 'Создать новый вариант VIP тарифа'}
                    </h4>
                    <button
                      onClick={() => {
                        setIsCreatingVipOption(false);
                        setEditingVipOption(null);
                        setVipOptionFormData(emptyVipOptionForm);
                      }}
                      className="text-gray-400 hover:text-white transition-colors"
                    >
                      <X className="w-5 h-5" />
                    </button>
                  </div>

                  <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                    <div>
                      <label className="block text-gray-300 mb-2 font-semibold">
                        Длительность <span className="text-red-500">*</span>
                      </label>
                      <select
                        value={vipOptionFormData.durationDays}
                        onChange={(e) => setVipOptionFormData({ ...vipOptionFormData, durationDays: parseInt(e.target.value) })}
                        className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                      >
                        <option value={0}>Навсегда</option>
                        <option value={1}>1 день</option>
                        <option value={7}>7 дней</option>
                        <option value={14}>14 дней</option>
                        <option value={30}>30 дней</option>
                        <option value={60}>60 дней</option>
                        <option value={90}>90 дней</option>
                        <option value={180}>180 дней</option>
                        <option value={365}>365 дней</option>
                      </select>
                    </div>

                    <div>
                      <label className="block text-gray-300 mb-2 font-semibold">
                        Цена (₽) <span className="text-red-500">*</span>
                      </label>
                      <input
                        type="number"
                        min="1"
                        value={vipOptionFormData.price}
                        onChange={(e) => setVipOptionFormData({ ...vipOptionFormData, price: parseInt(e.target.value) || 0 })}
                        placeholder="500"
                        className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                      />
                    </div>

                    <div>
                      <label className="block text-gray-300 mb-2 font-semibold">
                        Порядок отображения
                      </label>
                      <input
                        type="number"
                        min="0"
                        value={vipOptionFormData.order}
                        onChange={(e) => setVipOptionFormData({ ...vipOptionFormData, order: parseInt(e.target.value) || 0 })}
                        placeholder="0"
                        className="w-full bg-primary border border-gray-700 rounded-lg px-4 py-3 text-white focus:outline-none focus:border-accent"
                      />
                    </div>
                  </div>

                  <div className="flex items-center gap-3 mt-6">
                    <input
                      type="checkbox"
                      id="vipOptionIsActive"
                      checked={vipOptionFormData.isActive}
                      onChange={(e) => setVipOptionFormData({ ...vipOptionFormData, isActive: e.target.checked })}
                      className="w-5 h-5 bg-primary border border-gray-700 rounded focus:ring-2 focus:ring-accent"
                    />
                    <label htmlFor="vipOptionIsActive" className="text-gray-300 font-semibold cursor-pointer">
                      Вариант активен (показывать на сайте)
                    </label>
                  </div>

                  <div className="flex gap-4 mt-6">
                    <button
                      onClick={editingVipOption ? handleUpdateVipOption : handleCreateVipOption}
                      disabled={createVipOptionMutation.isPending || updateVipOptionMutation.isPending}
                      className="flex-1 bg-accent hover:bg-accent-dark text-white font-bold py-3 px-6 rounded-lg transition-colors disabled:opacity-50 flex items-center justify-center gap-2"
                    >
                      <Save className="w-5 h-5" />
                      {createVipOptionMutation.isPending || updateVipOptionMutation.isPending ? 'Сохранение...' : 'Сохранить'}
                    </button>
                    <button
                      onClick={() => {
                        setIsCreatingVipOption(false);
                        setEditingVipOption(null);
                        setVipOptionFormData(emptyVipOptionForm);
                      }}
                      className="px-6 py-3 bg-gray-700 hover:bg-gray-600 text-white font-bold rounded-lg transition-colors"
                    >
                      Отмена
                    </button>
                  </div>
                </div>
              )}

              <div className="space-y-4">
                <div className="flex items-center justify-between">
                  <h4 className="text-lg font-bold text-white">Варианты VIP тарифа</h4>
                  <button
                    onClick={() => {
                      setIsCreatingVipOption(true);
                      setEditingVipOption(null);
                      setVipOptionFormData(emptyVipOptionForm);
                    }}
                    className="bg-accent hover:bg-accent-dark text-white font-bold py-2 px-4 rounded-lg transition-colors flex items-center gap-2"
                  >
                    <Plus className="w-4 h-4" />
                    Добавить вариант
                  </button>
                </div>

                {vipOptionsLoading ? (
                  <div className="text-center py-8">
                    <div className="text-gray-400">Загрузка вариантов...</div>
                  </div>
                ) : vipTariffOptions && vipTariffOptions.length > 0 ? (
                  <div className="space-y-3">
                    {vipTariffOptions
                      .sort((a: VipTariffOption, b: VipTariffOption) => a.order - b.order)
                      .map((option: VipTariffOption) => (
                        <div
                          key={option.id}
                          className={`border-2 rounded-lg p-4 transition-all ${
                            option.isActive ? 'border-gray-700' : 'border-gray-800 opacity-60'
                          }`}
                        >
                          <div className="flex items-center justify-between">
                            <div className="flex-1">
                              <div className="flex items-center gap-3 mb-2">
                                <span className="text-lg font-bold text-white">
                                  {option.durationDays === 0
                                    ? 'Навсегда'
                                    : `${option.durationDays} ${option.durationDays === 1 ? 'день' : option.durationDays < 5 ? 'дня' : 'дней'}`}
                                </span>
                                <span className="text-xl font-bold text-yellow-500">{option.price} ₽</span>
                                {!option.isActive && (
                                  <span className="text-xs bg-gray-700 text-gray-400 px-2 py-1 rounded">
                                    Неактивен
                                  </span>
                                )}
                              </div>
                              <div className="text-sm text-gray-400">
                                Порядок: {option.order} • Создан: {formatServerDateOnly(option.createdAt)}
                              </div>
                            </div>
                            <div className="flex gap-2">
                              <button
                                onClick={() => handleEditVipOption(option)}
                                className="p-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg transition-colors"
                                title="Редактировать"
                              >
                                <Edit2 className="w-4 h-4" />
                              </button>
                              <button
                                onClick={() => {
                                  if (managingVipOptionsTariff) {
                                    deleteVipOptionMutation.mutate({
                                      tariffId: managingVipOptionsTariff.id,
                                      optionId: option.id,
                                    });
                                  }
                                }}
                                className="p-2 bg-red-600 hover:bg-red-700 text-white rounded-lg transition-colors"
                                title="Удалить"
                              >
                                <Trash2 className="w-4 h-4" />
                              </button>
                            </div>
                          </div>
                        </div>
                      ))}
                  </div>
                ) : (
                  <div className="bg-primary/50 border-2 border-dashed border-gray-600 rounded-lg p-8 text-center">
                    <p className="text-gray-400 text-lg">Вариантов VIP тарифа пока нет</p>
                    <p className="text-gray-500 text-sm mt-2">Добавьте первый вариант VIP тарифа</p>
                  </div>
                )}
              </div>
            </div>
          </div>
        </div>
      )}

      {showDeleteVipConfirm && (
        <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50 p-4">
          <div className="bg-secondary rounded-lg p-6 max-w-md w-full">
            <h3 className="text-xl font-bold text-white mb-4">Подтверждение удаления</h3>
            <p className="text-gray-400 mb-6">
              Вы уверены, что хотите удалить этот VIP тариф? Это действие необратимо.
            </p>
            <div className="flex gap-4">
              <button
                onClick={() => handleDeleteVip(showDeleteVipConfirm)}
                disabled={deleteVipMutation.isPending}
                className="flex-1 bg-red-600 hover:bg-red-700 text-white font-bold py-3 px-6 rounded-lg transition-colors disabled:opacity-50"
              >
                {deleteVipMutation.isPending ? 'Удаление...' : 'Удалить'}
              </button>
              <button
                onClick={() => setShowDeleteVipConfirm(null)}
                className="flex-1 bg-gray-700 hover:bg-gray-600 text-white font-bold py-3 px-6 rounded-lg transition-colors"
              >
                Отмена
              </button>
            </div>
          </div>
        </div>
      )}

    </div>
  );
}
