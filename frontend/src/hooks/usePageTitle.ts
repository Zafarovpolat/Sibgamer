import { useEffect } from 'react';
import { useSiteSettings } from './useSiteSettings';

export const usePageTitle = (pageTitle?: string) => {
  const { data: settings } = useSiteSettings();
  const siteName = settings?.site_name || 'SIBGamer';

  useEffect(() => {
    if (pageTitle) {
      document.title = `${pageTitle} - ${siteName}`;
    } else {
      document.title = siteName;
    }
  }, [pageTitle, siteName]);
};
