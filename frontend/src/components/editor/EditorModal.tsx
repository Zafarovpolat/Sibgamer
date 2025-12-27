import { useState, useEffect, useRef } from 'react';
import { createPortal } from 'react-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTimes, faUpload, faLink, faImage } from '@fortawesome/free-solid-svg-icons';

interface EditorModalProps {
    isOpen: boolean;
    onClose: () => void;
    title: string;
    children: React.ReactNode;
}

export const EditorModal = ({ isOpen, onClose, title, children }: EditorModalProps) => {
    useEffect(() => {
        const handleEscape = (e: KeyboardEvent) => {
            if (e.key === 'Escape') onClose();
        };

        if (isOpen) {
            document.addEventListener('keydown', handleEscape);
            document.body.style.overflow = 'hidden';
        }

        return () => {
            document.removeEventListener('keydown', handleEscape);
            document.body.style.overflow = 'unset';
        };
    }, [isOpen, onClose]);

    if (!isOpen) return null;

    // Используем Portal чтобы рендерить модалку вне формы
    return createPortal(
        <div
            className="fixed inset-0 z-[9999] flex items-center justify-center bg-black/70 backdrop-blur-sm"
            onClick={(e) => {
                e.preventDefault();
                e.stopPropagation();
                if (e.target === e.currentTarget) onClose();
            }}
            onMouseDown={(e) => e.stopPropagation()}
        >
            <div
                className="bg-gray-800 rounded-xl shadow-2xl border border-gray-700 w-full max-w-lg mx-4 overflow-hidden"
                onClick={(e) => e.stopPropagation()}
                onMouseDown={(e) => e.stopPropagation()}
            >
                <div className="flex items-center justify-between p-4 border-b border-gray-700">
                    <h3 className="text-lg font-semibold text-white">{title}</h3>
                    <button
                        type="button"
                        onClick={(e) => {
                            e.preventDefault();
                            e.stopPropagation();
                            onClose();
                        }}
                        className="text-gray-400 hover:text-white transition-colors"
                    >
                        <FontAwesomeIcon icon={faTimes} />
                    </button>
                </div>
                <div className="p-4" onClick={(e) => e.stopPropagation()}>
                    {children}
                </div>
            </div>
        </div>,
        document.body
    );
};

interface LinkModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSubmit: (url: string, text?: string) => void;
    initialUrl?: string;
    initialText?: string;
}

export const LinkModal = ({ isOpen, onClose, onSubmit, initialUrl = '', initialText = '' }: LinkModalProps) => {
    const [url, setUrl] = useState(initialUrl);
    const inputRef = useRef<HTMLInputElement>(null);

    useEffect(() => {
        if (isOpen) {
            setUrl(initialUrl);
            setTimeout(() => inputRef.current?.focus(), 100);
        }
    }, [isOpen, initialUrl]);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        e.stopPropagation();
        onSubmit(url.trim());
        onClose();
    };

    return (
        <EditorModal isOpen={isOpen} onClose={onClose} title="Вставить ссылку">
            <form onSubmit={handleSubmit} className="space-y-4" onClick={(e) => e.stopPropagation()}>
                <div>
                    <label className="block text-sm font-medium text-gray-300 mb-2">
                        URL ссылки *
                    </label>
                    <div className="relative">
                        <FontAwesomeIcon icon={faLink} className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-500" />
                        <input
                            ref={inputRef}
                            type="url"
                            value={url}
                            onChange={(e) => setUrl(e.target.value)}
                            placeholder="https://example.com"
                            className="w-full pl-10 pr-4 py-2 bg-gray-900 border border-gray-600 rounded-lg text-white placeholder-gray-500 focus:outline-none focus:border-orange-500"
                            onClick={(e) => e.stopPropagation()}
                        />
                    </div>
                </div>
                <div className="flex gap-3 pt-2">
                    <button
                        type="button"
                        onClick={(e) => {
                            e.preventDefault();
                            e.stopPropagation();
                            onClose();
                        }}
                        className="flex-1 px-4 py-2 bg-gray-700 hover:bg-gray-600 text-white rounded-lg transition-colors"
                    >
                        Отмена
                    </button>
                    <button
                        type="submit"
                        className="flex-1 px-4 py-2 bg-orange-500 hover:bg-orange-600 text-white rounded-lg transition-colors"
                        onClick={(e) => e.stopPropagation()}
                    >
                        Вставить
                    </button>
                </div>
            </form>
        </EditorModal>
    );
};

interface ImageModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSubmit: (url: string) => void;
    onUpload?: (file: File) => Promise<string>;
}

export const ImageModal = ({ isOpen, onClose, onSubmit, onUpload }: ImageModalProps) => {
    const [url, setUrl] = useState('');
    const [isUploading, setIsUploading] = useState(false);
    const [dragOver, setDragOver] = useState(false);
    const [preview, setPreview] = useState<string | null>(null);
    const fileInputRef = useRef<HTMLInputElement>(null);
    const inputRef = useRef<HTMLInputElement>(null);

    useEffect(() => {
        if (isOpen) {
            setUrl('');
            setPreview(null);
            setTimeout(() => inputRef.current?.focus(), 100);
        }
    }, [isOpen]);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        e.stopPropagation();
        if (url.trim()) {
            onSubmit(url.trim());
            onClose();
        }
    };

    const handleFileSelect = async (file: File) => {
        if (!file.type.startsWith('image/')) {
            alert('Пожалуйста, выберите изображение');
            return;
        }

        const reader = new FileReader();
        reader.onload = (e) => setPreview(e.target?.result as string);
        reader.readAsDataURL(file);

        if (onUpload) {
            setIsUploading(true);
            try {
                const uploadedUrl = await onUpload(file);
                setUrl(uploadedUrl);
                onSubmit(uploadedUrl);
                onClose();
            } catch (error) {
                console.error('Upload failed:', error);
                alert('Ошибка загрузки изображения');
            } finally {
                setIsUploading(false);
            }
        }
    };

    const handleDrop = (e: React.DragEvent) => {
        e.preventDefault();
        e.stopPropagation();
        setDragOver(false);
        const file = e.dataTransfer.files[0];
        if (file) handleFileSelect(file);
    };

    return (
        <EditorModal isOpen={isOpen} onClose={onClose} title="Вставить изображение">
            <div className="space-y-4" onClick={(e) => e.stopPropagation()}>
                {onUpload && (
                    <div
                        className={`border-2 border-dashed rounded-lg p-6 text-center transition-colors cursor-pointer ${dragOver
                                ? 'border-orange-500 bg-orange-500/10'
                                : 'border-gray-600 hover:border-gray-500'
                            }`}
                        onDragOver={(e) => { e.preventDefault(); e.stopPropagation(); setDragOver(true); }}
                        onDragLeave={(e) => { e.stopPropagation(); setDragOver(false); }}
                        onDrop={handleDrop}
                        onClick={(e) => { e.stopPropagation(); fileInputRef.current?.click(); }}
                    >
                        {preview ? (
                            <img src={preview} alt="Preview" className="max-h-40 mx-auto rounded-lg" />
                        ) : (
                            <>
                                <FontAwesomeIcon icon={faUpload} className="text-3xl text-gray-500 mb-2" />
                                <p className="text-gray-400">
                                    Перетащите изображение сюда или <span className="text-orange-500">нажмите для выбора</span>
                                </p>
                            </>
                        )}
                        {isUploading && (
                            <div className="mt-2">
                                <div className="animate-spin w-6 h-6 border-2 border-orange-500 border-t-transparent rounded-full mx-auto" />
                                <p className="text-gray-400 mt-2">Загрузка...</p>
                            </div>
                        )}
                        <input
                            ref={fileInputRef}
                            type="file"
                            accept="image/*"
                            onChange={(e) => {
                                e.stopPropagation();
                                e.target.files?.[0] && handleFileSelect(e.target.files[0]);
                            }}
                            className="hidden"
                            onClick={(e) => e.stopPropagation()}
                        />
                    </div>
                )}

                <div className="flex items-center gap-3">
                    <div className="flex-1 h-px bg-gray-600" />
                    <span className="text-gray-500 text-sm">или</span>
                    <div className="flex-1 h-px bg-gray-600" />
                </div>

                <form onSubmit={handleSubmit} onClick={(e) => e.stopPropagation()}>
                    <div>
                        <label className="block text-sm font-medium text-gray-300 mb-2">
                            URL изображения
                        </label>
                        <div className="relative">
                            <FontAwesomeIcon icon={faImage} className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-500" />
                            <input
                                ref={inputRef}
                                type="url"
                                value={url}
                                onChange={(e) => setUrl(e.target.value)}
                                placeholder="https://example.com/image.jpg"
                                className="w-full pl-10 pr-4 py-2 bg-gray-900 border border-gray-600 rounded-lg text-white placeholder-gray-500 focus:outline-none focus:border-orange-500"
                                onClick={(e) => e.stopPropagation()}
                            />
                        </div>
                    </div>
                    <div className="flex gap-3 pt-4">
                        <button
                            type="button"
                            onClick={(e) => {
                                e.preventDefault();
                                e.stopPropagation();
                                onClose();
                            }}
                            className="flex-1 px-4 py-2 bg-gray-700 hover:bg-gray-600 text-white rounded-lg transition-colors"
                        >
                            Отмена
                        </button>
                        <button
                            type="submit"
                            disabled={!url.trim()}
                            className="flex-1 px-4 py-2 bg-orange-500 hover:bg-orange-600 text-white rounded-lg transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
                            onClick={(e) => e.stopPropagation()}
                        >
                            Вставить
                        </button>
                    </div>
                </form>
            </div>
        </EditorModal>
    );
};

interface YoutubeModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSubmit: (url: string) => void;
}

export const YoutubeModal = ({ isOpen, onClose, onSubmit }: YoutubeModalProps) => {
    const [url, setUrl] = useState('');
    const [preview, setPreview] = useState<string | null>(null);
    const inputRef = useRef<HTMLInputElement>(null);

    useEffect(() => {
        if (isOpen) {
            setUrl('');
            setPreview(null);
            setTimeout(() => inputRef.current?.focus(), 100);
        }
    }, [isOpen]);

    useEffect(() => {
        const match = url.match(/(?:youtube\.com\/watch\?v=|youtu\.be\/)([a-zA-Z0-9_-]{11})/);
        if (match) {
            setPreview(`https://img.youtube.com/vi/${match[1]}/hqdefault.jpg`);
        } else {
            setPreview(null);
        }
    }, [url]);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        e.stopPropagation();
        if (url.trim()) {
            onSubmit(url.trim());
            onClose();
        }
    };

    return (
        <EditorModal isOpen={isOpen} onClose={onClose} title="Вставить YouTube видео">
            <form onSubmit={handleSubmit} className="space-y-4" onClick={(e) => e.stopPropagation()}>
                <div>
                    <label className="block text-sm font-medium text-gray-300 mb-2">
                        YouTube URL
                    </label>
                    <input
                        ref={inputRef}
                        type="url"
                        value={url}
                        onChange={(e) => setUrl(e.target.value)}
                        placeholder="https://www.youtube.com/watch?v=..."
                        className="w-full px-4 py-2 bg-gray-900 border border-gray-600 rounded-lg text-white placeholder-gray-500 focus:outline-none focus:border-orange-500"
                        required
                        onClick={(e) => e.stopPropagation()}
                    />
                </div>

                {preview && (
                    <div className="rounded-lg overflow-hidden">
                        <img src={preview} alt="Video preview" className="w-full" />
                    </div>
                )}

                <div className="flex gap-3 pt-2">
                    <button
                        type="button"
                        onClick={(e) => {
                            e.preventDefault();
                            e.stopPropagation();
                            onClose();
                        }}
                        className="flex-1 px-4 py-2 bg-gray-700 hover:bg-gray-600 text-white rounded-lg transition-colors"
                    >
                        Отмена
                    </button>
                    <button
                        type="submit"
                        className="flex-1 px-4 py-2 bg-red-600 hover:bg-red-500 text-white rounded-lg transition-colors"
                        onClick={(e) => e.stopPropagation()}
                    >
                        Вставить
                    </button>
                </div>
            </form>
        </EditorModal>
    );
};

interface ColorModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSubmit: (color: string) => void;
    currentColor?: string;
}

export const ColorModal = ({ isOpen, onClose, onSubmit, currentColor = '#ffffff' }: ColorModalProps) => {
    const [color, setColor] = useState(currentColor);

    const presetColors = [
        '#ffffff', '#f87171', '#fb923c', '#facc15', '#4ade80',
        '#22d3ee', '#60a5fa', '#a78bfa', '#f472b6', '#94a3b8',
        '#ef4444', '#f97316', '#eab308', '#22c55e', '#06b6d4',
        '#3b82f6', '#8b5cf6', '#ec4899', '#64748b', '#000000',
    ];

    useEffect(() => {
        if (isOpen) setColor(currentColor);
    }, [isOpen, currentColor]);

    const handleSubmit = (e?: React.MouseEvent) => {
        e?.preventDefault();
        e?.stopPropagation();
        onSubmit(color);
        onClose();
    };

    return (
        <EditorModal isOpen={isOpen} onClose={onClose} title="Выбрать цвет текста">
            <div className="space-y-4" onClick={(e) => e.stopPropagation()}>
                <div className="grid grid-cols-10 gap-2">
                    {presetColors.map((c) => (
                        <button
                            key={c}
                            type="button"
                            onClick={(e) => {
                                e.preventDefault();
                                e.stopPropagation();
                                setColor(c);
                            }}
                            className={`w-8 h-8 rounded-lg border-2 transition-transform hover:scale-110 ${color === c ? 'border-orange-500 scale-110' : 'border-gray-600'
                                }`}
                            style={{ backgroundColor: c }}
                        />
                    ))}
                </div>

                <div>
                    <label className="block text-sm font-medium text-gray-300 mb-2">
                        Свой цвет
                    </label>
                    <div className="flex gap-3">
                        <input
                            type="color"
                            value={color}
                            onChange={(e) => setColor(e.target.value)}
                            className="w-12 h-10 rounded cursor-pointer"
                            onClick={(e) => e.stopPropagation()}
                        />
                        <input
                            type="text"
                            value={color}
                            onChange={(e) => setColor(e.target.value)}
                            placeholder="#ff0000"
                            className="flex-1 px-4 py-2 bg-gray-900 border border-gray-600 rounded-lg text-white focus:outline-none focus:border-orange-500"
                            onClick={(e) => e.stopPropagation()}
                        />
                    </div>
                </div>

                <div className="p-4 bg-gray-900 rounded-lg">
                    <span style={{ color }}>Пример текста с выбранным цветом</span>
                </div>

                <div className="flex gap-3">
                    <button
                        type="button"
                        onClick={(e) => {
                            e.preventDefault();
                            e.stopPropagation();
                            onClose();
                        }}
                        className="flex-1 px-4 py-2 bg-gray-700 hover:bg-gray-600 text-white rounded-lg transition-colors"
                    >
                        Отмена
                    </button>
                    <button
                        type="button"
                        onClick={handleSubmit}
                        className="flex-1 px-4 py-2 bg-orange-500 hover:bg-orange-600 text-white rounded-lg transition-colors"
                    >
                        Применить
                    </button>
                </div>
            </div>
        </EditorModal>
    );
};