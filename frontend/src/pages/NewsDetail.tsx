import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { 
  faEye, faHeart, faComment, faCalendar, faUser, faReply, faTrash,
  faArrowLeft, faHeartBroken
} from '@fortawesome/free-solid-svg-icons';
import toast from 'react-hot-toast';
import { API_URL } from '../config/api';
import { resolveMediaUrl } from '../lib/media';
import { useAuthStore } from '../store/authStore';
import { useSiteSettings } from '../hooks/useSiteSettings';
import { formatServerDate } from '../utils/dateUtils';
import type { News, NewsComment } from '../types';

const NewsDetail = () => {
  const { slug } = useParams<{ slug: string }>();
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const [commentContent, setCommentContent] = useState('');
  const [replyingTo, setReplyingTo] = useState<number | null>(null);
  const [replyContent, setReplyContent] = useState('');
  const [replyingToUser, setReplyingToUser] = useState<string>('');

  const { token, user: currentUser } = useAuthStore();
  const { data: settings } = useSiteSettings();
  const siteName = settings?.site_name || 'SIBGamer';

  const { data: news, isLoading } = useQuery<News>({
    queryKey: ['news', slug],
    queryFn: async () => {
      const res = await fetch(`${API_URL}/news/${slug}`, {
        headers: token ? { 'Authorization': `Bearer ${token}` } : {}
      });
      if (!res.ok) throw new Error('Failed to fetch news');
      return res.json();
    }
  });

  useEffect(() => {
    if (news) {
      document.title = `${news.title} - ${siteName}`;
    }
  }, [news, siteName]);

  const { data: comments } = useQuery<NewsComment[]>({
    queryKey: ['news-comments', news?.id],
    queryFn: async () => {
      if (!news?.id) return [];
      const res = await fetch(`${API_URL}/news/${news.id}/comments`);
      if (!res.ok) throw new Error('Failed to fetch comments');
      return res.json();
    },
    enabled: !!news?.id
  });

  const likeMutation = useMutation({
    mutationFn: async () => {
      const res = await fetch(`${API_URL}/news/${news!.id}/like`, {
        method: 'POST',
        headers: { 'Authorization': `Bearer ${token}` }
      });
      if (!res.ok) throw new Error('Failed to like news');
      return res.json();
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['news', slug] });
      toast.success(news?.isLikedByCurrentUser ? 'Лайк убран' : 'Лайк поставлен!');
    }
  });

  const commentMutation = useMutation({
    mutationFn: async (data: { content: string; parentCommentId?: number }) => {
      const res = await fetch(`${API_URL}/news/${news!.id}/comments`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(data)
      });
      if (!res.ok) throw new Error('Failed to post comment');
      return res.json();
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['news-comments', news?.id] });
      queryClient.invalidateQueries({ queryKey: ['news', slug] });
      setCommentContent('');
      setReplyContent('');
      setReplyingTo(null);
      toast.success('Комментарий добавлен!');
    }
  });

  const deleteCommentMutation = useMutation({
    mutationFn: async (commentId: number) => {
      const res = await fetch(`${API_URL}/news/comments/${commentId}`, {
        method: 'DELETE',
        headers: { 'Authorization': `Bearer ${token}` }
      });
      if (!res.ok) throw new Error('Failed to delete comment');
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['news-comments', news?.id] });
      queryClient.invalidateQueries({ queryKey: ['news', slug] });
      toast.success('Комментарий удалён');
    }
  });

  const handleLike = () => {
    if (!token) {
      toast.error('Войдите, чтобы поставить лайк');
      return;
    }
    likeMutation.mutate();
  };

  const handleComment = (e: React.FormEvent) => {
    e.preventDefault();
    if (!token) {
      toast.error('Войдите, чтобы оставить комментарий');
      return;
    }
    if (!commentContent.trim()) return;
    commentMutation.mutate({ content: commentContent });
  };

  const handleReply = (parentId: number) => {
    if (!token) {
      toast.error('Войдите, чтобы ответить');
      return;
    }
    if (!replyContent.trim()) return;
    commentMutation.mutate({ content: replyContent, parentCommentId: parentId });
  };

  const handleDeleteComment = (commentId: number) => {
    if (confirm('Удалить комментарий?')) {
      deleteCommentMutation.mutate(commentId);
    }
  };

  const canDeleteComment = (comment: NewsComment) => {
    if (!currentUser) return false;
    return currentUser.id === comment.user.id || currentUser.isAdmin;
  };

  const countAllComments = (comments: NewsComment[]): number => {
    return comments.reduce((total, comment) => {
      return total + 1 + countAllComments(comment.replies || []);
    }, 0);
  };

  const totalCommentsCount = comments ? countAllComments(comments) : 0;

  const [highlightedCommentId, setHighlightedCommentId] = useState<number | null>(null);

  const scrollToComment = (commentId: number) => {
    const element = document.getElementById(`comment-${commentId}`);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth', block: 'center' });
      setHighlightedCommentId(commentId);
      setTimeout(() => setHighlightedCommentId(null), 3000);
    }
  };

  const renderComment = (comment: NewsComment) => (
    <div 
      key={comment.id} 
      id={`comment-${comment.id}`}
      className="mb-4"
    >
      <div className={`glass-card p-4 transition-all duration-300 ${
        highlightedCommentId === comment.id ? 'ring-2 ring-highlight' : ''
      }`}>
        <div className="flex items-start gap-3">
          <img
            src={comment.user.avatarUrl 
              ? resolveMediaUrl(comment.user.avatarUrl)
              : `https://ui-avatars.com/api/?name=${encodeURIComponent(comment.user.username)}&background=6366f1&color=fff`
            }
            alt={comment.user.username}
            className="w-10 h-10 rounded-full flex-shrink-0"
          />

          <div className="flex-1 min-w-0">
            <div className="flex items-center gap-2 mb-2 flex-wrap">
              <span className="font-bold text-white">{comment.user.username}</span>
              {comment.user.isAdmin && (
                <span className="px-2 py-0.5 bg-gradient-to-r from-highlight to-blue-500 text-white text-xs rounded font-semibold">
                  ADMIN
                </span>
              )}
              <span className="text-xs text-gray-500">{formatServerDate(comment.createdAt)}</span>
            </div>

            {comment.parentComment && (
              <div 
                onClick={() => scrollToComment(comment.parentComment!.id)}
                className="mb-3 p-2 bg-gray-800/50 rounded border-l-2 border-highlight cursor-pointer hover:bg-gray-800/70 transition-colors"
              >
                <div className="flex items-center gap-1 text-xs text-gray-400 mb-1">
                  <FontAwesomeIcon icon={faReply} className="text-highlight" />
                  <span>Ответ для <span className="text-highlight font-semibold">{comment.parentComment.username}</span></span>
                </div>
                <div className="text-sm text-gray-400 italic line-clamp-2">
                  {comment.parentComment.contentPreview}
                </div>
              </div>
            )}

            <div className="text-gray-300 mb-3 break-words whitespace-pre-wrap">
              {comment.content}
            </div>

            <div className="flex items-center gap-4">
              <button
                onClick={() => {
                  if (replyingTo === comment.id) {
                    setReplyingTo(null);
                    setReplyingToUser('');
                  } else {
                    setReplyingTo(comment.id);
                    setReplyingToUser(comment.user.username);
                  }
                }}
                className="text-sm text-gray-400 hover:text-highlight transition-colors"
              >
                <FontAwesomeIcon icon={faReply} className="mr-1" />
                Ответить
              </button>
              {canDeleteComment(comment) && (
                <button
                  onClick={() => handleDeleteComment(comment.id)}
                  className="text-sm text-red-500 hover:text-red-400 transition-colors"
                >
                  <FontAwesomeIcon icon={faTrash} className="mr-1" />
                  Удалить
                </button>
              )}
            </div>

            {replyingTo === comment.id && (
              <div className="mt-4">
                <textarea
                  value={replyContent}
                  onChange={(e) => setReplyContent(e.target.value)}
                  placeholder={`Ответить ${replyingToUser}...`}
                  className="input-field mb-2"
                  rows={3}
                />
                <div className="flex gap-2">
                  <button
                    onClick={() => handleReply(comment.id)}
                    className="btn-primary text-sm"
                  >
                    Отправить
                  </button>
                  <button
                    onClick={() => {
                      setReplyingTo(null);
                      setReplyContent('');
                      setReplyingToUser('');
                    }}
                    className="btn-secondary text-sm"
                  >
                    Отмена
                  </button>
                </div>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );

  if (isLoading) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="text-center py-10">Загрузка...</div>
      </div>
    );
  }

  if (!news) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="glass-card p-8 text-center">
          <p className="text-xl mb-4">Новость не найдена</p>
          <button onClick={() => navigate('/news')} className="btn-primary">
            К списку новостей
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <button
        onClick={() => navigate('/news')}
        className="btn-secondary mb-6"
      >
        <FontAwesomeIcon icon={faArrowLeft} className="mr-2" />
        Назад к новостям
      </button>

      <article className="glass-card overflow-hidden mb-8">
        {news.coverImage && (
          <div className="relative h-96 overflow-hidden">
            <img
              src={resolveMediaUrl(news.coverImage)}
              alt={news.title}
              className="w-full h-full object-cover"
            />
            <div className="absolute inset-0 bg-gradient-to-t from-gray-900 via-transparent to-transparent" />
          </div>
        )}

        <div className="p-8">
          <h1 className="text-4xl font-bold mb-4 bg-gradient-to-r from-highlight to-blue-500 bg-clip-text text-transparent">
            {news.title}
          </h1>

          <div className="flex items-center gap-6 text-sm text-gray-400 mb-6 pb-6 border-b border-gray-700">
            <div className="flex items-center gap-2">
              <FontAwesomeIcon icon={faUser} />
              <span>{news.author.username}</span>
            </div>
            <div className="flex items-center gap-2">
              <FontAwesomeIcon icon={faCalendar} />
              <span>{formatServerDate(news.createdAt)}</span>
            </div>
            <div className="flex items-center gap-2">
              <FontAwesomeIcon icon={faEye} />
              <span>{news.viewCount} просмотров</span>
            </div>
          </div>

          {news.summary && (
            <div className="bg-gray-800/50 p-4 rounded-lg mb-6">
              <p className="text-lg text-gray-300">{news.summary}</p>
            </div>
          )}

          <div 
            className="prose prose-invert max-w-none mb-8"
            dangerouslySetInnerHTML={{ __html: news.content }}
          />

          {news.media && news.media.length > 0 && (
            <div className="grid grid-cols-2 md:grid-cols-3 gap-4 mb-8">
              {news.media.map((media) => (
                <div key={media.id} className="rounded-lg overflow-hidden">
                  {media.mediaType.startsWith('image/') ? (
                    <img
                      src={resolveMediaUrl(media.mediaUrl)}
                      alt="Media"
                      className="w-full h-full object-cover"
                    />
                  ) : (
                    <video
                      src={resolveMediaUrl(media.mediaUrl)}
                      controls
                      className="w-full h-full"
                    />
                  )}
                </div>
              ))}
            </div>
          )}

          <div className="flex items-center gap-6 pt-6 border-t border-gray-700">
            <button
              onClick={handleLike}
              className={`flex items-center gap-2 px-4 py-2 rounded-lg transition-all ${
                news.isLikedByCurrentUser
                  ? 'bg-red-500 text-white'
                  : 'bg-gray-800 text-gray-300 hover:bg-gray-700'
              }`}
            >
              <FontAwesomeIcon icon={news.isLikedByCurrentUser ? faHeartBroken : faHeart} />
              <span>{news.likeCount}</span>
            </button>
            <div className="flex items-center gap-2 text-gray-400">
              <FontAwesomeIcon icon={faComment} />
              <span>{totalCommentsCount} {totalCommentsCount === 1 ? 'комментарий' : totalCommentsCount >= 2 && totalCommentsCount <= 4 ? 'комментария' : 'комментариев'}</span>
            </div>
          </div>
        </div>
      </article>

      <div className="glass-card p-8">
        <h2 className="text-2xl font-bold mb-6">Комментарии ({totalCommentsCount})</h2>

        {token ? (
          <form onSubmit={handleComment} className="mb-8">
            <textarea
              value={commentContent}
              onChange={(e) => setCommentContent(e.target.value)}
              placeholder="Оставьте комментарий..."
              className="input-field mb-4"
              rows={4}
            />
            <button type="submit" className="btn-primary">
              Отправить комментарий
            </button>
          </form>
        ) : (
          <div className="bg-gray-800/50 p-4 rounded-lg mb-8 text-center">
            <p className="text-gray-400">Войдите, чтобы оставить комментарий</p>
          </div>
        )}

        <div>
          {comments && comments.length > 0 ? (
            comments.map(comment => renderComment(comment))
          ) : (
            <p className="text-gray-400 text-center py-8">Комментариев пока нет. Будьте первым!</p>
          )}
        </div>
      </div>
    </div>
  );
};

export default NewsDetail;
