import { useState } from 'react';
import { resolveMediaUrl } from '../lib/media';

interface AvatarProps {
  username: string;
  avatarUrl?: string;
  size?: 'sm' | 'md' | 'lg' | 'xl';
  className?: string;
}

const Avatar = ({ username, avatarUrl, size = 'md', className = '' }: AvatarProps) => {
  const [imageError, setImageError] = useState(false);

  const sizeClasses = {
    sm: 'w-8 h-8 text-sm',
    md: 'w-10 h-10 text-base',
    lg: 'w-16 h-16 text-2xl',
    xl: 'w-32 h-32 text-5xl',
  };

  const firstLetter = username.charAt(0).toUpperCase();

  if (avatarUrl && !imageError) {
    return (
      <div className={`${sizeClasses[size]} rounded-full overflow-hidden border-2 border-highlight ${className}`}>
        <img
          src={resolveMediaUrl(avatarUrl)}
          alt={username}
          className="w-full h-full object-cover"
          onError={() => setImageError(true)}
        />
      </div>
    );
  }

  return (
    <div
      className={`${sizeClasses[size]} rounded-full bg-gradient-to-br from-highlight to-blue-600 flex items-center justify-center font-bold text-white border-2 border-highlight ${className}`}
    >
      {firstLetter}
    </div>
  );
};

export default Avatar;
