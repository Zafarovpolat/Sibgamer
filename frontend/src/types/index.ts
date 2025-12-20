export interface User {
  id: number;
  username: string;
  email: string;
  avatarUrl?: string;
  steamId?: string;
  steamProfileUrl?: string;
  lastIp?: string;
  isAdmin: boolean;
  isBlocked?: boolean;
  blockedAt?: string;
  blockReason?: string;
  createdAt: string;
}

export interface Server {
  id: number;
  name: string;
  ipAddress: string;
  port: number;
  mapName: string;
  currentPlayers: number;
  maxPlayers: number;
  isOnline: boolean;
  rconPasswordSet?: boolean;
  rconPassword?: string;
}

export interface SliderImage {
  id: number;
  imageUrl: string;
  title: string;
  description: string;
  order: number;
  buttons?: Array<{
    Text: string;
    Url: string;
  }>;
}

export interface LoginCredentials {
  username: string;
  password: string;
}

export interface RegisterData {
  username: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export interface AuthResponse {
  token: string;
  user: User;
}

export interface NewsMedia {
  id: number;
  mediaUrl: string;
  mediaType: string;
  order: number;
}

export interface News {
  id: number;
  title: string;
  content: string;
  summary?: string;
  slug: string;
  coverImage?: string;
  author: User;
  isPublished: boolean;
  createdAt: string;
  updatedAt: string;
  viewCount: number;
  likeCount: number;
  commentCount: number;
  media: NewsMedia[];
  isLikedByCurrentUser: boolean;
}

export interface NewsListItem {
  id: number;
  title: string;
  summary?: string;
  slug: string;
  coverImage?: string;
  authorName: string;
  createdAt: string;
  viewCount: number;
  likeCount: number;
  commentCount: number;
  isPublished: boolean;
}

export interface ParentCommentInfo {
  id: number;
  username: string;
  contentPreview: string;
}

export interface NewsComment {
  id: number;
  newsId: number;
  user: User;
  content: string;
  parentCommentId?: number;
  parentComment?: ParentCommentInfo;
  createdAt: string;
  updatedAt: string;
  replies: NewsComment[];
}

export interface CreateNewsData {
  title: string;
  content: string;
  summary?: string;
  slug?: string;
  coverImage?: string;
  isPublished: boolean;
  mediaUrls: string[];
}

export interface EventMedia {
  id: number;
  mediaUrl: string;
  mediaType: string;
  order: number;
}

export interface Event {
  id: number;
  title: string;
  content: string;
  summary?: string;
  slug: string;
  coverImage?: string;
  author: User;
  isPublished: boolean;
  startDate: string;
  endDate: string;
  createdAt: string;
  updatedAt: string;
  viewCount: number;
  likeCount: number;
  commentCount: number;
  media: EventMedia[];
  isLikedByCurrentUser: boolean;
}

export interface EventListItem {
  id: number;
  title: string;
  summary?: string;
  slug: string;
  coverImage?: string;
  author: { id: number; username: string; avatarUrl?: string };
  startDate: string;
  endDate: string;
  createdAt: string;
  viewCount: number;
  likeCount: number;
  commentCount: number;
}

export interface EventComment {
  id: number;
  eventId: number;
  user: User;
  content: string;
  parentCommentId?: number;
  createdAt: string;
  updatedAt: string;
  replies: EventComment[];
}

export interface CreateEventData {
  title: string;
  content: string;
  summary?: string;
  slug?: string;
  coverImage?: string;
  startDate: string;
  endDate: string;
  isPublished: boolean;
}

export interface SmtpSettings {
  id: number;
  host: string;
  port: number;
  username: string;
  password: string;
  enableSsl: boolean;
  fromEmail: string;
  fromName: string;
  isConfigured: boolean;
  updatedAt: string;
}

export interface UpdateSmtpSettings {
  host: string;
  port: number;
  username: string;
  password: string;
  enableSsl: boolean;
  fromEmail: string;
  fromName: string;
}

export interface BulkEmailRequest {
  subject: string;
  body: string;
}

export interface BulkEmailResponse {
  totalRecipients: number;
  successCount: number;
  failureCount: number;
  errors: string[];
}

export interface TestEmailResponse {
  success: boolean;
  message: string;
}

export interface ForgotPasswordRequest {
  email: string;
}

export interface ResetPasswordRequest {
  token: string;
  newPassword: string;
}

export interface YooMoneySettings {
  id: number;
  walletNumber: string;
  secretKey: string;
  isConfigured: boolean;
  updatedAt: string;
}

export interface UpdateYooMoneySettings {
  walletNumber: string;
  secretKey: string;
}

export interface SourceBansSettings {
  id: number;
  serverId: number;
  serverName: string;
  host: string;
  port: number;
  database: string;
  username: string;
  password: string;
  isConfigured: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface UpdateSourceBansSettings {
  serverId?: number;
  host: string;
  port: number;
  database: string;
  username: string;
  password: string;
}

export interface TopDonator {
  userId: number;
  username: string;
  avatarUrl?: string;
  totalAmount: number;
  donationCount: number;
}

export interface AdminTariffOption {
  id: number;
  tariffId: number;
  durationDays: number; 
  price: number;
  order: number;
  isActive: boolean;
  createdAt: string;
}

export interface AdminTariff {
  id: number;
  serverId: number;
  serverName: string;
  name: string;
  description: string;
  flags?: string;
  groupName?: string;
  immunity: number;
  isActive: boolean;
  order: number;
  createdAt: string;
  options?: AdminTariffOption[];
}

export interface CreateAdminTariff {
  serverId: number;
  name: string;
  description: string;
  flags?: string;
  groupName?: string;
  immunity: number;
  isActive: boolean;
  order: number;
}

export interface UpdateAdminTariff {
  name?: string;
  description?: string;
  flags?: string;
  groupName?: string;
  immunity?: number;
  isActive?: boolean;
  order?: number;
}

export interface CreateAdminTariffOption {
  durationDays: number;
  price: number;
  order: number;
  isActive: boolean;
}

export interface UpdateAdminTariffOption {
  durationDays?: number;
  price?: number;
  order?: number;
  isActive?: boolean;
}

export interface DonationPackage {
  id: number;
  title: string;
  description: string;
  suggestedAmounts?: number[];
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface UpdateDonationPackage {
  title: string;
  description: string;
  suggestedAmounts?: number[];
  isActive: boolean;
}

export interface CreateDonation {
  amount: number;
  message?: string;
}

export interface CreateAdminPurchase {
  tariffOptionId: number;
  serverId: number;
  adminPassword: string;
}

export interface DonationTransaction {
  id: number;
  transactionId: string;
  userId?: number;
  username?: string;
  steamId?: string;
  amount: number;
  type: string;
  tariffId?: number;
  tariffOptionId?: number;
  tariffName?: string;
  serverId?: number;
  serverName?: string;
  status: string;
  expiresAt?: string;
  createdAt: string;
  completedAt?: string;
}

export interface UserAdminPrivilege {
  id: number;
  userId?: number;
  username?: string;
  steamId: string;
  serverId: number;
  serverName: string;
  tariffName: string;
  tariffOptionId: number;
  flags?: string;
  groupName?: string;
  immunity: number;
  startsAt: string;
  expiresAt: string;
  isActive: boolean;
  isExpired: boolean;
  daysRemaining: number;
  adminPassword?: string;
  createdAt: string;
}

export interface ServerWithTariffs {
  serverId: number;
  serverName: string;
  serverIp: string;
  tariffs: AdminTariff[];
}

export interface DonationSettings {
  isConfigured: boolean;
  walletNumber?: string;
  donationPackage?: {
    id: number;
    title: string;
    description: string;
    suggestedAmounts?: number[];
  };
}

export interface PaymentResponse {
  transactionId: string;
  paymentUrl: string;
  amount: number;
  tariffName?: string;
  serverName?: string;
  pendingExpiresAt: string;
}

export interface TransactionStatus {
  transactionId: string;
  status: 'pending' | 'completed' | 'failed' | 'cancelled';
  amount: number;
  type: string;
  tariffName?: string;
  serverName?: string;
  createdAt: string;
  completedAt?: string;
  pendingExpiresAt?: string;
}

export interface UserVipPrivilege {
  id: number;
  userId?: number;
  username?: string;
  steamId?: string;
  serverId: number;
  serverName: string;
  tariffName: string;
  groupName: string;
  startsAt: string;
  expiresAt: string;
  isActive: boolean;
  isExpired: boolean;
  daysRemaining: number;
  createdAt?: string;
}

export interface VipApplicationDto {
  id: number;
  userId: number;
  username: string;
  steamId: string;
  serverId: number;
  serverName: string;
  hoursPerWeek?: number | null;
  reason: string;
  status: 'pending' | 'approved' | 'rejected' | string;
  adminId?: number | null;
  adminComment?: string | null;
  vipGroup?: string | null;
  durationDays?: number | null;
  createdAt: string;
  processedAt?: string | null;
}

export interface CreateVipPurchase {
  tariffOptionId: number;
  serverId: number;
}

export interface VipSettings {
  id: number;
  serverId: number;
  serverName: string;
  host: string;
  port: number;
  database: string;
  username: string;
  password: string;
  isConfigured: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface UpdateVipSettings {
  serverId?: number;
  host: string;
  port: number;
  database: string;
  username: string;
  password: string;
}

export interface VipTariff {
  id: number;
  serverId: number;
  serverName: string;
  name: string;
  description: string;
  groupName: string;
  isActive: boolean;
  order: number;
  createdAt: string;
  options?: VipTariffOption[];
}

export interface CreateVipTariff {
  serverId: number;
  name: string;
  description: string;
  groupName: string;
  isActive: boolean;
  order: number;
}

export interface UpdateVipTariff {
  name?: string;
  description?: string;
  groupName?: string;
  isActive?: boolean;
  order?: number;
}

export interface VipTariffOption {
  id: number;
  tariffId: number;
  durationDays: number;
  price: number;
  order: number;
  isActive: boolean;
  createdAt: string;
}

export interface CreateVipTariffOption {
  durationDays: number;
  price: number;
  order: number;
  isActive: boolean;
}

export interface UpdateVipTariffOption {
  durationDays?: number;
  price?: number;
  order?: number;
  isActive?: boolean;
}

export interface CreateCustomPageData {
  title: string;
  content: string;
  summary?: string;
  slug?: string;
  coverImage?: string;
  isPublished: boolean;
  mediaUrls: string[];
}

export interface CustomPageMedia {
  id: number;
  mediaUrl: string;
  mediaType: string;
  order: number;
}

export interface CustomPage {
  id: number;
  title: string;
  content: string;
  summary?: string;
  slug: string;
  coverImage?: string;
  author: User;
  isPublished: boolean;
  createdAt: string;
  updatedAt: string;
  viewCount: number;
  media: CustomPageMedia[];
}
