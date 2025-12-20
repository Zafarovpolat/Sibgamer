import { useQuery } from '@tanstack/react-query';
import { useParams } from 'react-router-dom';
import { API_URL } from '../config/api';
import { resolveMediaUrl } from '../lib/media';
import type { CustomPage } from '../types';

const CustomPageDetail = () => {
  const { slug } = useParams<{ slug: string }>();

  const { data: page, isLoading, error } = useQuery<CustomPage>({
    queryKey: ['custom-page', slug],
    queryFn: async () => {
      const res = await fetch(`${API_URL}/custompages/${slug}`);
      if (!res.ok) throw new Error('Failed to fetch custom page');
      return res.json();
    },
    enabled: !!slug
  });

  if (isLoading) {
    return (
      <div className="min-h-screen bg-primary flex items-center justify-center">
        <div className="text-white text-xl">Загрузка...</div>
      </div>
    );
  }

  if (error || !page) {
    return (
      <div className="min-h-screen bg-primary flex items-center justify-center">
        <div className="text-white text-xl">Страница не найдена</div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-primary">
      <div className="container mx-auto px-4 py-8">
        {page.coverImage && (
          <div className="mb-8">
            <img
              src={resolveMediaUrl(page.coverImage)}
              alt={page.title}
              className="w-full h-64 md:h-96 object-cover rounded-lg"
            />
          </div>
        )}

        <div className="mb-8">
          <h1 className="text-4xl md:text-5xl font-bold text-white mb-4">
            {page.title}
          </h1>

          {page.summary && (
            <p className="text-xl text-gray-300 mb-6">
              {page.summary}
            </p>
          )}
        </div>

        <div>
          <div
            className="prose prose-invert max-w-none"
            dangerouslySetInnerHTML={{ __html: page.content }}
          />
        </div>

        {page.media && page.media.length > 0 && (
          <div className="mt-8">
            <h2 className="text-2xl font-bold text-white mb-6">Медиа</h2>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
              {page.media.map((media) => (
                <div key={media.id} className="glass-card p-4">
                  {media.mediaType.startsWith('image/') ? (
                    <img
                      src={resolveMediaUrl(media.mediaUrl)}
                      alt="Media"
                      className="w-full h-48 object-cover rounded"
                    />
                  ) : media.mediaType.startsWith('video/') ? (
                      <video
                        src={resolveMediaUrl(media.mediaUrl)}
                      controls
                      className="w-full h-48 object-cover rounded"
                    />
                  ) : null}
                </div>
              ))}
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default CustomPageDetail;
