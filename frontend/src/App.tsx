import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { Toaster } from 'react-hot-toast';
import { useEffect, useState } from 'react';
import Navbar from './components/Navbar';
import Footer from './components/Footer';
import SteamModal from './components/SteamModal';
import Home from './pages/Home';
import Profile from './pages/Profile';
import News from './pages/News';
import NewsDetail from './pages/NewsDetail';
import Events from './pages/Events';
import EventDetail from './pages/EventDetail';
import CustomPageDetail from './pages/CustomPageDetail';
import AdminLayout from './components/AdminLayout';
import AdminDashboard from './pages/admin/AdminDashboard';
import AdminServers from './pages/admin/AdminServers';
import AdminUsers from './pages/admin/AdminUsers';
import AdminSlider from './pages/admin/AdminSlider';
import AdminNews from './pages/admin/AdminNews';
import AdminEvents from './pages/admin/AdminEvents';
import AdminEmail from './pages/admin/AdminEmail';
import AdminDonationSettings from './pages/admin/AdminDonationSettings';
import AdminDonationTariffs from './pages/admin/AdminDonationTariffs';
import AdminDonationMonitoring from './pages/admin/AdminDonationMonitoring';
import AdminVipApplications from './pages/admin/AdminVipApplications';
import AdminNotifications from './pages/admin/AdminNotifications';
import AdminTgNotifications from './pages/admin/AdminTgNotifications';
import AdminCustomPages from './pages/admin/AdminCustomPages';
import ResetPassword from './pages/ResetPassword';
import Donate from './pages/Donate';
import DonationSuccess from './pages/DonationSuccess';
import MemberApplication from './pages/MemberApplication';
import Notifications from './pages/Notifications';
import ProtectedRoute from './components/ProtectedRoute';
import AdminNavSections from './pages/admin/AdminNavSections';
import { useAuthStore } from './store/authStore';
import { initServerTime } from './utils/dateUtils';
import api from './lib/axios';
import axios from 'axios';

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      refetchOnWindowFocus: false,
      retry: 1,
    },
  },
});

function App() {
  const { user, isAuthenticated, logout } = useAuthStore();
  const [showSteamModal, setShowSteamModal] = useState(false);
  const [isInitializing, setIsInitializing] = useState(true);

  useEffect(() => {
    const checkTokenValidity = async () => {
      const authStorage = localStorage.getItem('auth-storage');
      if (authStorage) {
        try {
          const { state } = JSON.parse(authStorage);
          if (state?.token && state?.user) {
            try {
              await api.get('/profile');
            } catch (error: unknown) {
              if (axios.isAxiosError(error) && error.response?.status === 401) {
                console.log('Token expired or invalid, logging out');
                logout();
              }
            }
          }
        } catch (error) {
          console.error('Error parsing auth storage:', error);
          logout();
        }
      }
      initServerTime().catch(() => { });
      setIsInitializing(false);
    };

    checkTokenValidity();
  }, [logout]);

  useEffect(() => {
    if (isAuthenticated && user && !user.steamId && !isInitializing) {
      const timer = setTimeout(() => {
        setShowSteamModal(true);
      }, 1000);
      return () => clearTimeout(timer);
    }
  }, [isAuthenticated, user, isInitializing]);

  const handleSteamModalClose = () => {
    setShowSteamModal(false);
  };

  const handleSteamSuccess = () => {
    setShowSteamModal(false);
  };

  if (isInitializing) {
    return (
      <div className="min-h-screen bg-primary flex items-center justify-center">
        <div className="text-white text-xl">Загрузка...</div>
      </div>
    );
  }

  return (
    <QueryClientProvider client={queryClient}>
      <BrowserRouter>
        <div className="min-h-screen bg-primary flex flex-col">
          <Routes>
            <Route
              path="/"
              element={
                <>
                  <Navbar />
                  <Home />
                  <Footer />
                </>
              }
            />
            <Route path="/reset-password" element={<ResetPassword />} />
            <Route
              path="/profile"
              element={
                <ProtectedRoute>
                  <Navbar />
                  <Profile />
                  <Footer />
                </ProtectedRoute>
              }
            />
            <Route
              path="/news"
              element={
                <>
                  <Navbar />
                  <News />
                  <Footer />
                </>
              }
            />
            <Route
              path="/news/:slug"
              element={
                <>
                  <Navbar />
                  <NewsDetail />
                  <Footer />
                </>
              }
            />
            <Route
              path="/events"
              element={
                <>
                  <Navbar />
                  <Events />
                  <Footer />
                </>
              }
            />
            <Route
              path="/events/:slug"
              element={
                <>
                  <Navbar />
                  <EventDetail />
                  <Footer />
                </>
              }
            />
            <Route
              path="/:slug"
              element={
                <>
                  <Navbar />
                  <CustomPageDetail />
                  <Footer />
                </>
              }
            />
            <Route
              path="/donate"
              element={
                <>
                  <Navbar />
                  <Donate />
                  <Footer />
                </>
              }
            />
            <Route
              path="/member"
              element={
                <ProtectedRoute>
                  <Navbar />
                  <MemberApplication />
                  <Footer />
                </ProtectedRoute>
              }
            />
            <Route
              path="/donation-success"
              element={
                <>
                  <DonationSuccess />
                </>
              }
            />
            <Route
              path="/notifications"
              element={
                <ProtectedRoute>
                  <Navbar />
                  <Notifications />
                  <Footer />
                </ProtectedRoute>
              }
            />
            <Route path="/admin" element={<AdminLayout />}>
              <Route index element={<AdminDashboard />} />
              <Route path="servers" element={<AdminServers />} />
              <Route path="users" element={<AdminUsers />} />
              <Route path="slider" element={<AdminSlider />} />
              <Route path="news" element={<AdminNews />} />
              <Route path="events" element={<AdminEvents />} />
              <Route path="custompages" element={<AdminCustomPages />} />
              <Route path="email" element={<AdminEmail />} />
              <Route path="nav-sections" element={<AdminNavSections />} />
              <Route path="donation-settings" element={<AdminDonationSettings />} />
              <Route path="donation-tariffs" element={<AdminDonationTariffs />} />
              <Route path="donation-monitoring" element={<AdminDonationMonitoring />} />
              <Route path="vip-applications" element={<AdminVipApplications />} />
              <Route path="notifications" element={<AdminNotifications />} />
              <Route path="tg-notifications" element={<AdminTgNotifications />} />
            </Route>
          </Routes>
        </div>
        <Toaster position="top-right" />
        <SteamModal
          isOpen={showSteamModal}
          onClose={handleSteamModalClose}
          onSuccess={handleSteamSuccess}
        />
      </BrowserRouter>
    </QueryClientProvider>
  );
}

export default App;
