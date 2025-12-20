import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { 
  faEye, faHeart, faComment, faCalendar, faUser, faReply, faTrash,
  faArrowLeft, faHeartBroken, faClock, faCircle
} from '@fortawesome/free-solid-svg-icons';
import toast from 'react-hot-toast';
import { API_URL } from '../config/api';
import { resolveMediaUrl } from '../lib/media';
import { useAuthStore } from '../store/authStore';
import { getServerLocalTime, formatServerDate, parseServerDate } from '../utils/dateUtils';
import CountdownTimer from '../components/CountdownTimer';
import { useSiteSettings } from '../hooks/useSiteSettings';

interface EventDetail {
  id: number;
  title: string;
  content: string;
  summary: string;
  slug: string;
  coverImage: string;
  author: {
    id: number;
    username: string;
    avatarUrl: string | null;
    isAdmin: boolean;
  };
  isPublished: boolean;
  startDate: string;
  endDate: string;
  createdAt: string;
  updatedAt: string;
  viewCount: number;
  likeCount: number;
  commentCount: number;
  isLikedByCurrentUser: boolean;
  media: Array<{
    id: number;
    mediaUrl: string;
    mediaType: string;
    caption: string | null;
  }>;
}

interface ParentCommentInfo {
  id: number;
  username: string;
  contentPreview: string;
}

interface EventComment {
  id: number;
  content: string;
  user: {
    id: number;
    username: string;
    avatarUrl: string | null;
    isAdmin: boolean;
  };
  createdAt: string;
  eventId: number;
  parentCommentId: number | null;
  parentComment?: ParentCommentInfo;
  replies?: EventComment[];
}

const getEventStatus = (startDate: string, endDate: string) => {
  const now = getServerLocalTime();
  const start = parseServerDate(startDate);
  const end = parseServerDate(endDate);

  if (now < start) {
    return { type: 'upcoming' as const, targetDate: startDate, color: 'text-white', bgColor: 'bg-black/40', readableText: 'Начнётся' };
  } else if (now >= start && now <= end) {
    return { type: 'ongoing' as const, targetDate: endDate, color: 'text-green-400', bgColor: 'bg-black/40', readableText: 'Проходит' };
  } else {
    return { type: 'finished' as const, text: 'Завершено', color: 'text-red-400', bgColor: 'bg-black/40' };
  }
};

const EventDetail = () => {
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

  const { data: event, isLoading } = useQuery<EventDetail>({
    queryKey: ['event', slug],
    queryFn: async () => {
      const res = await fetch(`${API_URL}/events/${slug}`, {
        headers: token ? { 'Authorization': `Bearer ${token}` } : {}
      });
      if (!res.ok) throw new Error('Failed to fetch event');
      return res.json();
    }
  });

  useEffect(() => {
    if (event) {
      document.title = `${event.title} - ${siteName}`;
    }
  }, [event, siteName]);

  const { data: comments } = useQuery<EventComment[]>({
    queryKey: ['event-comments', event?.id],
    queryFn: async () => {
      if (!event?.id) return [];
      const res = await fetch(`${API_URL}/events/${event.id}/comments`);
      if (!res.ok) throw new Error('Failed to fetch comments');
      return res.json();
    },
    enabled: !!event?.id
  });

  const likeMutation = useMutation({
    mutationFn: async () => {
      const res = await fetch(`${API_URL}/events/${event!.id}/like`, {
        method: 'POST',
        headers: { 'Authorization': `Bearer ${token}` }
      });
      if (!res.ok) throw new Error('Failed to like event');
      return res.json();
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['event', slug] });
      toast.success(event?.isLikedByCurrentUser ? 'Лайк убран' : 'Лайк поставлен!');
    }
  });

  const commentMutation = useMutation({
    mutationFn: async (data: { content: string; parentCommentId?: number }) => {
      const res = await fetch(`${API_URL}/events/${event!.id}/comments`, {
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
      queryClient.invalidateQueries({ queryKey: ['event-comments', event?.id] });
      queryClient.invalidateQueries({ queryKey: ['event', slug] });
      setCommentContent('');
      setReplyContent('');
      setReplyingTo(null);
      toast.success('Комментарий добавлен!');
    }
  });

  const deleteCommentMutation = useMutation({
    mutationFn: async (commentId: number) => {
      const res = await fetch(`${API_URL}/events/comments/${commentId}`, {
        method: 'DELETE',
        headers: { 'Authorization': `Bearer ${token}` }
      });
      if (!res.ok) throw new Error('Failed to delete comment');
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['event-comments', event?.id] });
      queryClient.invalidateQueries({ queryKey: ['event', slug] });
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

  const canDeleteComment = (comment: EventComment) => {
    if (!currentUser) return false;
    return currentUser.id === comment.user.id || currentUser.isAdmin;
  };

  const countAllComments = (comments: EventComment[]): number => {
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

  const renderComment = (comment: EventComment) => (
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
              <span>{formatServerDate(comment.createdAt)}</span>
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

  if (!event) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="glass-card p-8 text-center">
          <p className="text-xl mb-4">Событие не найдено</p>
          <button onClick={() => navigate('/events')} className="btn-primary">
            К списку событий
          </button>
        </div>
      </div>
    );
  }

  const status = getEventStatus(event.startDate, event.endDate);

  return (
    <div className="container mx-auto px-4 py-8">
      <button
        onClick={() => navigate('/events')}
        className="btn-secondary mb-6"
      >
        <FontAwesomeIcon icon={faArrowLeft} className="mr-2" />
        Назад к событиям
      </button>

      <article className="glass-card overflow-hidden mb-8">
        {event.coverImage && (
          <div className="relative h-96 overflow-hidden">
            <img
              src={resolveMediaUrl(event.coverImage)}
              alt={event.title}
              className="w-full h-full object-cover"
            />
            <div className="absolute inset-0 bg-gradient-to-t from-gray-900 via-transparent to-transparent" />
            <div className="absolute top-4 right-4">
              {(() => {
                const now = getServerLocalTime();
                const start = parseServerDate(event.startDate);
                const end = parseServerDate(event.endDate);
                if (now < start) {
                  return (
                    <CountdownTimer
                      targetDate={event.startDate}
                      showLabels={true}
                      statusLabel="До начала"
                      statusTextClass="text-white"
                      statusBgClass="bg-black/40"
                      className="text-sm"
                    />
                  );
                }
                if (now >= start && now <= end) {
                  return (
                    <CountdownTimer
                      targetDate={event.endDate}
                      showLabels={true}
                      statusLabel="До завершения"
                      statusTextClass="text-green-400"
                      statusBgClass="bg-black/40"
                      className="text-sm"
                    />
                  );
                }
                return (
                  <div className={`timer-pill lg ${status.bgColor} ${status.color} text-sm font-semibold`.trim()} role="status">
                    <FontAwesomeIcon icon={faCircle} className={`${status.color} text-xs mr-2`} />
                    <span className={`${status.color} text-sm font-semibold`}>{status.text}</span>
                  </div>
                );
              })()}
            </div>
          </div>
        )}

        <div className="p-8">
          <h1 className="text-4xl font-bold mb-4 bg-gradient-to-r from-highlight to-blue-500 bg-clip-text text-transparent">
            {event.title}
          </h1>

          <div className="flex items-center gap-6 text-sm text-gray-400 mb-6 pb-6 border-b border-gray-700">
            <div className="flex items-center gap-2">
              <FontAwesomeIcon icon={faUser} />
              <span>{event.author.username}</span>
            </div>
            <div className="flex items-center gap-2">
              <FontAwesomeIcon icon={faCalendar} />
              <span>Начало: {formatServerDate(event.startDate)}</span>
            </div>
            <div className="flex items-center gap-2">
              <FontAwesomeIcon icon={faClock} />
              <span>Конец: {formatServerDate(event.endDate)}</span>
            </div>
            <div className="flex items-center gap-2">
              <FontAwesomeIcon icon={faEye} />
              <span>{event.viewCount} просмотров</span>
            </div>
          </div>

          {event.summary && (
            <div className="bg-gray-800/50 p-4 rounded-lg mb-6">
              <p className="text-lg text-gray-300">{event.summary}</p>
            </div>
          )}

          <div 
            className="prose prose-invert max-w-none mb-8"
            dangerouslySetInnerHTML={{ __html: event.content }}
          />

          {event.media && event.media.length > 0 && (
            <div className="grid grid-cols-2 md:grid-cols-3 gap-4 mb-8">
              {event.media.map((media) => (
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
                event.isLikedByCurrentUser
                  ? 'bg-red-500 text-white'
                  : 'bg-gray-800 text-gray-300 hover:bg-gray-700'
              }`}
            >
              <FontAwesomeIcon icon={event.isLikedByCurrentUser ? faHeartBroken : faHeart} />
              <span>{event.likeCount}</span>
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

export default EventDetail;
