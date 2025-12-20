import { useQuery } from '@tanstack/react-query';
import { API_URL } from '../config/api';

export const useSiteSettings = () => {
  return useQuery<Record<string, string>>({
    queryKey: ['settings'],
    queryFn: async () => {
      const res = await fetch(`${API_URL}/settings`);
      if (!res.ok) throw new Error('Failed to fetch settings');
      return res.json();
    },
    staleTime: 5 * 60 * 1000, 
  });
};
