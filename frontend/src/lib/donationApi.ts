import api from './axios';
import type {
  DonationSettings,
  ServerWithTariffs,
  UserAdminPrivilege,
  CreateDonation,
  CreateAdminPurchase,
  PaymentResponse,
  TransactionStatus,
  DonationTransaction,
  YooMoneySettings,
  UpdateYooMoneySettings,
  SourceBansSettings,
  UpdateSourceBansSettings,
  AdminTariff,
  CreateAdminTariff,
  UpdateAdminTariff,
  AdminTariffOption,
  CreateAdminTariffOption,
  UpdateAdminTariffOption,
  DonationPackage,
  UpdateDonationPackage,
  TopDonator,
  UserVipPrivilege,
  CreateVipPurchase,
  VipSettings,
  UpdateVipSettings,
  VipTariff,
  CreateVipTariff,
  UpdateVipTariff,
  VipTariffOption,
  CreateVipTariffOption,
  VipApplicationDto,
  UpdateVipTariffOption,
} from '../types';

export const getDonationSettings = async (): Promise<DonationSettings> => {
  const response = await api.get('/donation/settings');
  return response.data;
};

export const getTopDonators = async (limit: number = 3): Promise<TopDonator[]> => {
  const response = await api.get(`/donation/top-donators?limit=${limit}`);
  return response.data;
};

export const getTariffsByServers = async (): Promise<ServerWithTariffs[]> => {
  const response = await api.get('/donation/tariffs');
  return response.data;
};

export const getMyPrivileges = async (): Promise<UserAdminPrivilege[]> => {
  const response = await api.get('/donation/my-privileges');
  return response.data;
};

export const createDonation = async (data: CreateDonation): Promise<PaymentResponse> => {
  const response = await api.post('/donation/create-donation', data);
  return response.data;
};

export const createAdminPurchase = async (data: CreateAdminPurchase): Promise<PaymentResponse> => {
  const response = await api.post('/donation/create-admin-purchase', data);
  return response.data;
};

export const getTransactionStatus = async (transactionId: string): Promise<TransactionStatus> => {
  const response = await api.get(`/donation/transaction/${transactionId}`);
  return response.data;
};

export const cancelTransaction = async (transactionId: string): Promise<{ message: string }> => {
  const response = await api.post(`/donation/cancel-transaction/${transactionId}`);
  return response.data;
};

export const getYooMoneySettings = async (): Promise<YooMoneySettings> => {
  const response = await api.get('/admin/donation/yoomoney-settings');
  return response.data;
};

export const updateYooMoneySettings = async (data: UpdateYooMoneySettings): Promise<YooMoneySettings> => {
  const response = await api.post('/admin/donation/yoomoney-settings', data);
  return response.data;
};

export const getAllSourceBansSettings = async (): Promise<SourceBansSettings[]> => {
  const response = await api.get('/admin/donation/sourcebans-settings');
  return response.data;
};

export const getSourceBansSettingsByServer = async (serverId: number): Promise<SourceBansSettings | { isConfigured: false; serverId: number }> => {
  const response = await api.get(`/admin/donation/sourcebans-settings/${serverId}`);
  return response.data;
};

export const upsertSourceBansSettings = async (data: UpdateSourceBansSettings): Promise<SourceBansSettings> => {
  const response = await api.post('/admin/donation/sourcebans-settings', data);
  return response.data;
};

export const deleteSourceBansSettings = async (serverId: number): Promise<void> => {
  await api.delete(`/admin/donation/sourcebans-settings/${serverId}`);
};

export const testSourceBansConnection = async (serverId: number): Promise<{ success: boolean; message: string }> => {
  const response = await api.post(`/admin/donation/sourcebans-settings/${serverId}/test`);
  return response.data;
};

export const getAllVipSettings = async (): Promise<VipSettings[]> => {
  const response = await api.get('/admin/donation/vip-settings');
  return response.data;
};

export const getVipSettingsByServer = async (serverId: number): Promise<VipSettings | { isConfigured: false; serverId: number }> => {
  const response = await api.get(`/admin/donation/vip-settings/${serverId}`);
  return response.data;
};

export const upsertVipSettings = async (data: UpdateVipSettings): Promise<VipSettings> => {
  const response = await api.post('/admin/donation/vip-settings', data);
  return response.data;
};

export const deleteVipSettings = async (serverId: number): Promise<void> => {
  await api.delete(`/admin/donation/vip-settings/${serverId}`);
};

export const testVipConnection = async (serverId: number): Promise<{ success: boolean; message: string }> => {
  const response = await api.post(`/admin/donation/vip-settings/${serverId}/test`);
  return response.data;
};

export const getServersWithSourceBans = async (): Promise<{ id: number; name: string; ipAddress: string }[]> => {
  const response = await api.get('/admin/donation/servers-with-sourcebans');
  return response.data;
};

export const getAllTariffs = async (): Promise<AdminTariff[]> => {
  const response = await api.get('/admin/donation/tariffs');
  return response.data;
};

export const createTariff = async (data: CreateAdminTariff): Promise<AdminTariff> => {
  const response = await api.post('/admin/donation/tariffs', data);
  return response.data;
};

export const updateTariff = async (id: number, data: UpdateAdminTariff): Promise<AdminTariff> => {
  const response = await api.put(`/admin/donation/tariffs/${id}`, data);
  return response.data;
};

export const deleteTariff = async (id: number): Promise<void> => {
  await api.delete(`/admin/donation/tariffs/${id}`);
};

export const getTariffOptions = async (tariffId: number): Promise<AdminTariffOption[]> => {
  const response = await api.get(`/admin/donation/tariffs/${tariffId}/options`);
  return response.data;
};

export const createTariffOption = async (tariffId: number, data: CreateAdminTariffOption): Promise<AdminTariffOption> => {
  const response = await api.post(`/admin/donation/tariffs/${tariffId}/options`, data);
  return response.data;
};

export const updateTariffOption = async (tariffId: number, optionId: number, data: UpdateAdminTariffOption): Promise<AdminTariffOption> => {
  const response = await api.put(`/admin/donation/tariffs/${tariffId}/options/${optionId}`, data);
  return response.data;
};

export const deleteTariffOption = async (tariffId: number, optionId: number): Promise<void> => {
  await api.delete(`/admin/donation/tariffs/${tariffId}/options/${optionId}`);
};

export const getDonationPackage = async (): Promise<DonationPackage> => {
  const response = await api.get('/admin/donation/package');
  return response.data;
};

export const updateDonationPackage = async (data: UpdateDonationPackage): Promise<DonationPackage> => {
  const response = await api.post('/admin/donation/package', data);
  return response.data;
};

export const getAllTransactions = async (page: number = 1, pageSize: number = 50): Promise<DonationTransaction[]> => {
  const response = await api.get(`/admin/donation/transactions?page=${page}&pageSize=${pageSize}`);
  return response.data;
};

export const approveTransaction = async (transactionId: string): Promise<{ message: string; transactionId: string; adminId?: number; privilegeId?: number; serverName?: string; tariffName?: string; vipGroup?: string; expiresAt?: string }> => {
  const response = await api.post(`/admin/donation/transactions/${transactionId}/approve`);
  return response.data;
};

export const rejectTransaction = async (transactionId: string): Promise<{ message: string; transactionId: string }> => {
  const response = await api.post(`/admin/donation/transactions/${transactionId}/reject`);
  return response.data;
};

export const getAllPrivileges = async (page: number = 1, pageSize: number = 50): Promise<UserAdminPrivilege[]> => {
  const response = await api.get(`/admin/donation/privileges?page=${page}&pageSize=${pageSize}`);
  return response.data;
};

export const getExtendOptions = async (privilegeId: number): Promise<{ privilegeId: number; availableOptions: AdminTariffOption[] }> => {
  const response = await api.get(`/donation/extend-options/${privilegeId}`);
  return response.data;
};

export const extendAdminPrivilege = async (data: { privilegeId: number; tariffOptionId: number }): Promise<PaymentResponse> => {
  const response = await api.post('/donation/extend-admin-privilege', data);
  return response.data;
};

export const getVipTariffsByServers = async (): Promise<ServerWithTariffs[]> => {
  const response = await api.get('/donation/vip-tariffs');
  return response.data;
};

export const getServersWithVipPublic = async (): Promise<{ serverId: number; serverName: string; ipAddress: string }[]> => {
  const response = await api.get('/donation/servers-with-vip');
  return response.data;
};

export const getMyVipPrivileges = async (): Promise<UserVipPrivilege[]> => {
  const response = await api.get('/donation/my-vip-privileges');
  return response.data;
};

export const createVipPurchase = async (data: CreateVipPurchase): Promise<PaymentResponse> => {
  const response = await api.post('/donation/create-vip-purchase', data);
  return response.data;
};

export const createVipApplication = async (data: { serverId: number; hoursPerWeek?: number; reason: string }): Promise<{ message: string; applicationId: number }> => {
  const response = await api.post('/donation/vip-application', data);
  return response.data;
};

export const getMyVipApplications = async (): Promise<VipApplicationDto[]> => {
  const response = await api.get('/donation/my-vip-applications');
  return response.data;
};

export const getMyVipStatuses = async (): Promise<Record<string, unknown>[]> => {
  const response = await api.get('/user/vip-status');
  return response.data;
};

export const syncMyVipStatus = async (): Promise<{ message: string }> => {
  const response = await api.post('/user/sync-vip-status');
  return response.data;
};

export const getAllVipTariffs = async (): Promise<VipTariff[]> => {
  const response = await api.get('/admin/donation/vip-tariffs');
  return response.data;
};

export const createVipTariff = async (data: CreateVipTariff): Promise<VipTariff> => {
  const response = await api.post('/admin/donation/vip-tariffs', data);
  return response.data;
};

export const updateVipTariff = async (id: number, data: UpdateVipTariff): Promise<VipTariff> => {
  const response = await api.put(`/admin/donation/vip-tariffs/${id}`, data);
  return response.data;
};

export const deleteVipTariff = async (id: number): Promise<void> => {
  await api.delete(`/admin/donation/vip-tariffs/${id}`);
};

export const getVipTariffOptions = async (tariffId: number): Promise<VipTariffOption[]> => {
  const response = await api.get(`/admin/donation/vip-tariffs/${tariffId}/options`);
  return response.data;
};

export const createVipTariffOption = async (tariffId: number, data: CreateVipTariffOption): Promise<VipTariffOption> => {
  const response = await api.post(`/admin/donation/vip-tariffs/${tariffId}/options`, data);
  return response.data;
};

export const updateVipTariffOption = async (tariffId: number, optionId: number, data: UpdateVipTariffOption): Promise<VipTariffOption> => {
  const response = await api.put(`/admin/donation/vip-tariffs/${tariffId}/options/${optionId}`, data);
  return response.data;
};

export const deleteVipTariffOption = async (tariffId: number, optionId: number): Promise<void> => {
  await api.delete(`/admin/donation/vip-tariffs/${tariffId}/options/${optionId}`);
};

export const getServersWithVip = async (): Promise<{ id: number; name: string; ipAddress: string }[]> => {
  const response = await api.get('/admin/donation/servers-with-vip');
  return response.data;
};

export const getAllVipPrivileges = async (page: number = 1, pageSize: number = 50): Promise<UserVipPrivilege[]> => {
  const response = await api.get(`/admin/donation/vip-privileges?page=${page}&pageSize=${pageSize}`);
  return response.data;
};

export const removeVipPrivilege = async (privilegeId: number): Promise<void> => {
  await api.delete(`/admin/donation/vip-privileges/${privilegeId}`);
};

export const getAllVipApplications = async (page: number = 1, pageSize: number = 50): Promise<{ items: VipApplicationDto[]; total: number; page: number; pageSize: number }> => {
  const response = await api.get(`/admin/donation/vip-applications?page=${page}&pageSize=${pageSize}`);
  return response.data;
};

export const getVipGroups = async (): Promise<string[]> => {
  const response = await api.get(`/admin/donation/vip-groups`);
  return response.data;
};

export const approveVipApplication = async (id: number, data: { vipGroup: string; durationDays: number; tariffId?: number | null; tariffOptionId?: number | null }): Promise<void> => {
  await api.post(`/admin/donation/vip-applications/${id}/approve`, data);
};

export const rejectVipApplication = async (id: number, data: { reason: string }): Promise<void> => {
  await api.post(`/admin/donation/vip-applications/${id}/reject`, data);
};

export const removeAdminPrivilege = async (privilegeId: number): Promise<void> => {
  await api.delete(`/admin/donation/admin-privileges/${privilegeId}`);
};

