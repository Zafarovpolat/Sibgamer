export const getAuthToken = (): string | null => {
  try {
    const authStorage = localStorage.getItem('auth-storage');
    if (authStorage) {
      const { state } = JSON.parse(authStorage);
      return state?.token || null;
    }
  } catch (error) {
    console.error('Error getting auth token:', error);
  }
  return null;
};
