import { useEffect, useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSave, faCheck } from '@fortawesome/free-solid-svg-icons';
import { usePageTitle } from '../../hooks/usePageTitle';
import axios from '../../lib/axios';

const AdminDashboard = () => {
  usePageTitle('Панель управления');
  
  const queryClient = useQueryClient();
  const [newsPerPage, setNewsPerPage] = useState('9');
  const [siteName, setSiteName] = useState('SIBGamer');
  const [eventsPerPage, setEventsPerPage] = useState('6');
  const [savedNewsPerPage, setSavedNewsPerPage] = useState(false);
  const [savedSiteName, setSavedSiteName] = useState(false);
  const [savedEventsPerPage, setSavedEventsPerPage] = useState(false);

  const { data: settings } = useQuery<Record<string, string>>({
    queryKey: ['admin-settings'],
    queryFn: async () => {
      const res = await axios.get('/admin/settings');
      return res.data;
    }
  });

  useEffect(() => {
    if (settings) {
      setNewsPerPage(settings.news_per_page || '9');
      setSiteName(settings.site_name || 'SIBGamer');
      setEventsPerPage(settings.events_per_page || '6');
    }
  }, [settings]);

  const updateNewsPerPageMutation = useMutation({
    mutationFn: async (data: { key: string; value: string }[]) => {
      await axios.post('/admin/settings/batch', data);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin-settings'] });
      queryClient.invalidateQueries({ queryKey: ['settings'] });
      setSavedNewsPerPage(true);
      setTimeout(() => setSavedNewsPerPage(false), 2000);
    }
  });

  const updateSiteNameMutation = useMutation({
    mutationFn: async (data: { key: string; value: string }[]) => {
      await axios.post('/admin/settings/batch', data);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin-settings'] });
      queryClient.invalidateQueries({ queryKey: ['settings'] });
      setSavedSiteName(true);
      setTimeout(() => setSavedSiteName(false), 2000);
    }
  });

  const updateEventsPerPageMutation = useMutation({
    mutationFn: async (data: { key: string; value: string }[]) => {
      await axios.post('/admin/settings/batch', data);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['admin-settings'] });
      queryClient.invalidateQueries({ queryKey: ['settings'] });
      setSavedEventsPerPage(true);
      setTimeout(() => setSavedEventsPerPage(false), 2000);
    }
  });

  const handleSaveNewsPerPage = () => {
    updateNewsPerPageMutation.mutate([
      { key: 'news_per_page', value: newsPerPage }
    ]);
  };

  const handleSaveSiteName = () => {
    updateSiteNameMutation.mutate([
      { key: 'site_name', value: siteName }
    ]);
  };

  const handleSaveEventsPerPage = () => {
    updateEventsPerPageMutation.mutate([
      { key: 'events_per_page', value: eventsPerPage }
    ]);
  };

  return (
    <div>
      <h1 className="text-3xl font-bold mb-6">Панель управления</h1>
      
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-4">
        <div className="rounded-xl border border-gray-800/50 p-6">
          <div>
            <h3 className="text-lg font-semibold text-white mb-3">
              Название сайта
            </h3>

            <div className="flex w-full flex-col sm:flex-row items-start sm:items-center gap-4">
              <input
                type="text"
                value={siteName}
                onChange={(e) => setSiteName(e.target.value)}
                className="flex-1 px-4 py-2 bg-gray-800/50 border border-gray-700/50 rounded-lg text-white focus:outline-none focus:border-highlight transition-colors"
                placeholder="SIBGamer"
              />

              <button
                onClick={handleSaveSiteName}
                disabled={updateSiteNameMutation.isPending || savedSiteName}
                className="sm:ml-auto shrink-0 px-6 py-2 bg-highlight hover:bg-highlight/80 disabled:opacity-50 text-white font-medium rounded-lg transition-all duration-200 flex items-center gap-2 whitespace-nowrap"
              >
                <FontAwesomeIcon icon={savedSiteName ? faCheck : faSave} />
                {savedSiteName ? 'Сохранено' : 'Сохранить'}
              </button>
            </div>
          </div>
        </div>

        <div className="rounded-xl border border-gray-800/50 p-6">
          <div>
            <h3 className="text-lg font-semibold text-white mb-3">
              Количество новостей на странице
            </h3>

            <div className="flex w-full flex-col sm:flex-row items-start sm:items-center gap-4">
              <input
                type="number"
                min="1"
                value={newsPerPage}
                onChange={(e) => setNewsPerPage(e.target.value)}
                className="w-32 px-4 py-2 bg-gray-800/50 border border-gray-700/50 rounded-lg text-white focus:outline-none focus:border-highlight transition-colors"
              />

              <button
                onClick={handleSaveNewsPerPage}
                disabled={updateNewsPerPageMutation.isPending || savedNewsPerPage}
                className="sm:ml-auto shrink-0 px-6 py-2 bg-highlight hover:bg-highlight/80 disabled:opacity-50 text-white font-medium rounded-lg transition-all duration-200 flex items-center gap-2 whitespace-nowrap"
              >
                <FontAwesomeIcon icon={savedNewsPerPage ? faCheck : faSave} />
                {savedNewsPerPage ? 'Сохранено' : 'Сохранить'}
              </button>
            </div>
          </div>
        </div>

        <div className="rounded-xl border border-gray-800/50 p-6">
          <div>
            <h3 className="text-lg font-semibold text-white mb-3">
              Количество событий на странице
            </h3>

            <div className="flex w-full flex-col sm:flex-row items-start sm:items-center gap-4">
              <input
                type="number"
                min="1"
                value={eventsPerPage}
                onChange={(e) => setEventsPerPage(e.target.value)}
                className="w-32 px-4 py-2 bg-gray-800/50 border border-gray-700/50 rounded-lg text-white focus:outline-none focus:border-highlight transition-colors"
              />

              <button
                onClick={handleSaveEventsPerPage}
                disabled={updateEventsPerPageMutation.isPending || savedEventsPerPage}
                className="sm:ml-auto shrink-0 px-6 py-2 bg-highlight hover:bg-highlight/80 disabled:opacity-50 text-white font-medium rounded-lg transition-all duration-200 flex items-center gap-2 whitespace-nowrap"
              >
                <FontAwesomeIcon icon={savedEventsPerPage ? faCheck : faSave} />
                {savedEventsPerPage ? 'Сохранено' : 'Сохранить'}
              </button>
            </div>
          </div>
        </div>

      </div>
    </div>
  );
};

export default AdminDashboard;
