import { Navigate, Outlet, Link } from 'react-router-dom';
import { useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBars, faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { useAuthStore } from '../store/authStore';
import AdminSidebar from '../components/AdminSidebar';

const AdminLayout = () => {
  const { user, isAuthenticated } = useAuthStore();
  const [sidebarOpen, setSidebarOpen] = useState(false);

  if (!isAuthenticated || !user?.isAdmin) {
    return <Navigate to="/" replace />;
  }

  return (
    <div className="flex min-h-screen bg-primary">
      {sidebarOpen && (
        <div
          className="fixed inset-0 bg-black/70 backdrop-blur-sm z-40 md:hidden"
          onClick={() => setSidebarOpen(false)}
        />
      )}

      <AdminSidebar isOpen={sidebarOpen} onClose={() => setSidebarOpen(false)} />

      <div className="flex-1 flex flex-col overflow-hidden">
        <div className="md:hidden glass-card p-4 flex items-center justify-between sticky top-0 z-30">
          <button
            onClick={() => setSidebarOpen(true)}
            className="text-2xl text-gray-300 hover:text-white transition-colors"
          >
            <FontAwesomeIcon icon={faBars} />
          </button>
          <h1 className="text-xl font-bold bg-gradient-to-r from-highlight to-blue-500 bg-clip-text text-transparent">
            Админ-панель
          </h1>
          <Link to="/" className="flex items-center gap-2 text-sm text-gray-300 hover:text-white transition-colors">
            <FontAwesomeIcon icon={faArrowLeft} />
            <span className="hidden sm:inline">Вернуться на сайт</span>
          </Link>
        </div>

        <div className="flex-1 p-4 md:p-8 overflow-y-auto">
          <Outlet />
        </div>
      </div>
    </div>
  );
};

export default AdminLayout;
