import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faPlus, faEdit, faTrash, faSave, faTimes, faGripVertical,
    faChevronDown, faChevronUp, faLink, faExternalLinkAlt, faFileAlt,
    faEye, faEyeSlash, faHome, faNewspaper, faCalendarAlt, faHeart,
    faInfoCircle, faUsers, faBan, faChartBar, faCog, faQuestion
} from '@fortawesome/free-solid-svg-icons';
import type { NavSection, NavSectionItem, CustomPageOption } from '../../types/nav';
import {
    getAdminNavSections,
    createNavSection,
    updateNavSection,
    deleteNavSection,
    reorderNavSections,
    addNavItem,
    updateNavItem,
    deleteNavItem,
    getCustomPagesForNav
} from '../../lib/navApi';

// Доступные иконки
const AVAILABLE_ICONS = [
    { value: 'faHome', label: 'Дом', icon: faHome },
    { value: 'faNewspaper', label: 'Новости', icon: faNewspaper },
    { value: 'faCalendarAlt', label: 'События', icon: faCalendarAlt },
    { value: 'faHeart', label: 'Сердце', icon: faHeart },
    { value: 'faInfoCircle', label: 'Информация', icon: faInfoCircle },
    { value: 'faUsers', label: 'Пользователи', icon: faUsers },
    { value: 'faBan', label: 'Бан', icon: faBan },
    { value: 'faChartBar', label: 'Статистика', icon: faChartBar },
    { value: 'faCog', label: 'Настройки', icon: faCog },
    { value: 'faLink', label: 'Ссылка', icon: faLink },
    { value: 'faFileAlt', label: 'Файл', icon: faFileAlt },
    { value: 'faQuestion', label: 'Вопрос', icon: faQuestion },
];

const getIconByName = (name?: string) => {
    const found = AVAILABLE_ICONS.find(i => i.value === name);
    return found?.icon || faLink;
};

const AdminNavSections = () => {
    const queryClient = useQueryClient();
    const [editingSection, setEditingSection] = useState<NavSection | null>(null);
    const [editingItem, setEditingItem] = useState<{ sectionId: number; item: NavSectionItem | null } | null>(null);
    const [expandedSections, setExpandedSections] = useState<number[]>([]);
    const [isCreatingSection, setIsCreatingSection] = useState(false);

    // Fetch data
    const { data: sections = [], isLoading } = useQuery({
        queryKey: ['admin-nav-sections'],
        queryFn: getAdminNavSections
    });

    const { data: customPages = [] } = useQuery({
        queryKey: ['nav-custom-pages'],
        queryFn: getCustomPagesForNav
    });

    // Mutations
    const createSectionMutation = useMutation({
        mutationFn: createNavSection,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['admin-nav-sections'] });
            setIsCreatingSection(false);
            setEditingSection(null);
        }
    });

    const updateSectionMutation = useMutation({
        mutationFn: ({ id, data }: { id: number; data: Partial<NavSection> }) => updateNavSection(id, data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['admin-nav-sections'] });
            setEditingSection(null);
        }
    });

    const deleteSectionMutation = useMutation({
        mutationFn: deleteNavSection,
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['admin-nav-sections'] })
    });

    const addItemMutation = useMutation({
        mutationFn: ({ sectionId, data }: { sectionId: number; data: Partial<NavSectionItem> }) =>
            addNavItem(sectionId, data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['admin-nav-sections'] });
            setEditingItem(null);
        }
    });

    const updateItemMutation = useMutation({
        mutationFn: ({ itemId, data }: { itemId: number; data: Partial<NavSectionItem> }) =>
            updateNavItem(itemId, data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['admin-nav-sections'] });
            setEditingItem(null);
        }
    });

    const deleteItemMutation = useMutation({
        mutationFn: deleteNavItem,
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['admin-nav-sections'] })
    });

    const reorderMutation = useMutation({
        mutationFn: reorderNavSections,
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['admin-nav-sections'] })
    });

    // Handlers
    const toggleExpanded = (id: number) => {
        setExpandedSections(prev =>
            prev.includes(id) ? prev.filter(x => x !== id) : [...prev, id]
        );
    };

    const moveSection = (index: number, direction: 'up' | 'down') => {
        const newSections = [...sections];
        const targetIndex = direction === 'up' ? index - 1 : index + 1;

        if (targetIndex < 0 || targetIndex >= newSections.length) return;

        [newSections[index], newSections[targetIndex]] = [newSections[targetIndex], newSections[index]];

        const reorderData = newSections.map((s, i) => ({ id: s.id, order: i }));
        reorderMutation.mutate(reorderData);
    };

    const handleSaveSection = (data: Partial<NavSection>) => {
        if (editingSection?.id) {
            updateSectionMutation.mutate({ id: editingSection.id, data });
        } else {
            createSectionMutation.mutate(data);
        }
    };

    const handleSaveItem = (sectionId: number, data: Partial<NavSectionItem>) => {
        if (editingItem?.item?.id) {
            updateItemMutation.mutate({ itemId: editingItem.item.id, data });
        } else {
            addItemMutation.mutate({ sectionId, data });
        }
    };

    if (isLoading) {
        return <div className="p-8 text-center">Загрузка...</div>;
    }

    return (
        <div className="container mx-auto px-4 py-8">
            <div className="flex justify-between items-center mb-6">
                <h1 className="text-3xl font-bold">Управление меню навигации</h1>
                {!isCreatingSection && !editingSection && (
                    <button
                        onClick={() => {
                            setIsCreatingSection(true);
                            setEditingSection({
                                id: 0,
                                name: '',
                                icon: '',
                                order: sections.length,
                                isPublished: true,
                                type: 'link',
                                url: '',
                                isExternal: false,
                                openInNewTab: false,
                                items: []
                            });
                        }}
                        className="btn-primary"
                    >
                        <FontAwesomeIcon icon={faPlus} className="mr-2" />
                        Добавить раздел
                    </button>
                )}
            </div>

            {/* Section Form */}
            {(isCreatingSection || editingSection) && (
                <SectionForm
                    section={editingSection!}
                    onSave={handleSaveSection}
                    onCancel={() => {
                        setIsCreatingSection(false);
                        setEditingSection(null);
                    }}
                    isLoading={createSectionMutation.isPending || updateSectionMutation.isPending}
                />
            )}

            {/* Sections List */}
            {!isCreatingSection && !editingSection && (
                <div className="space-y-4">
                    {sections.map((section, index) => (
                        <div key={section.id} className="glass-card overflow-hidden">
                            {/* Section Header */}
                            <div className="p-4 flex items-center gap-4">
                                <div className="flex flex-col gap-1">
                                    <button
                                        onClick={() => moveSection(index, 'up')}
                                        disabled={index === 0}
                                        className="text-gray-400 hover:text-white disabled:opacity-30"
                                    >
                                        <FontAwesomeIcon icon={faChevronUp} size="sm" />
                                    </button>
                                    <button
                                        onClick={() => moveSection(index, 'down')}
                                        disabled={index === sections.length - 1}
                                        className="text-gray-400 hover:text-white disabled:opacity-30"
                                    >
                                        <FontAwesomeIcon icon={faChevronDown} size="sm" />
                                    </button>
                                </div>

                                <FontAwesomeIcon icon={faGripVertical} className="text-gray-500" />

                                {section.icon && (
                                    <FontAwesomeIcon icon={getIconByName(section.icon)} className="text-highlight" />
                                )}

                                <div className="flex-1">
                                    <h3 className="font-bold text-lg">{section.name}</h3>
                                    <div className="text-sm text-gray-400 flex items-center gap-3">
                                        <span className={`px-2 py-0.5 rounded text-xs ${section.type === 'dropdown' ? 'bg-blue-500/20 text-blue-400' : 'bg-green-500/20 text-green-400'
                                            }`}>
                                            {section.type === 'dropdown' ? 'Выпадающее меню' : 'Ссылка'}
                                        </span>
                                        {section.type === 'link' && section.url && (
                                            <span className="flex items-center gap-1">
                                                <FontAwesomeIcon icon={section.isExternal ? faExternalLinkAlt : faLink} size="xs" />
                                                {section.url}
                                            </span>
                                        )}
                                        {section.type === 'dropdown' && (
                                            <span>{section.items.length} пунктов</span>
                                        )}
                                    </div>
                                </div>

                                <div className="flex items-center gap-2">
                                    <span className={`flex items-center gap-1 text-sm ${section.isPublished ? 'text-green-400' : 'text-gray-500'}`}>
                                        <FontAwesomeIcon icon={section.isPublished ? faEye : faEyeSlash} />
                                    </span>

                                    {section.type === 'dropdown' && (
                                        <button
                                            onClick={() => toggleExpanded(section.id)}
                                            className="btn-secondary px-3 py-1"
                                        >
                                            <FontAwesomeIcon icon={expandedSections.includes(section.id) ? faChevronUp : faChevronDown} />
                                        </button>
                                    )}

                                    <button
                                        onClick={() => setEditingSection(section)}
                                        className="btn-secondary px-3 py-1"
                                    >
                                        <FontAwesomeIcon icon={faEdit} />
                                    </button>

                                    <button
                                        onClick={() => {
                                            if (confirm(`Удалить раздел "${section.name}"?`)) {
                                                deleteSectionMutation.mutate(section.id);
                                            }
                                        }}
                                        className="btn-secondary px-3 py-1 text-red-400 hover:text-red-300"
                                    >
                                        <FontAwesomeIcon icon={faTrash} />
                                    </button>
                                </div>
                            </div>

                            {/* Section Items (for dropdowns) */}
                            {section.type === 'dropdown' && expandedSections.includes(section.id) && (
                                <div className="border-t border-gray-700 bg-black/20 p-4">
                                    <div className="space-y-2">
                                        {section.items.map((item) => (
                                            <div key={item.id} className="flex items-center gap-3 p-3 bg-gray-800/50 rounded-lg">
                                                {item.icon && (
                                                    <FontAwesomeIcon icon={getIconByName(item.icon)} className="text-gray-400" />
                                                )}
                                                <div className="flex-1">
                                                    <span className="font-medium">{item.name}</span>
                                                    <span className="ml-2 text-sm text-gray-400">
                                                        {item.type === 'custompage' ? (
                                                            <span className="text-purple-400">📄 {item.customPageTitle || item.customPageSlug}</span>
                                                        ) : item.type === 'externallink' ? (
                                                            <span className="text-blue-400">🔗 {item.url}</span>
                                                        ) : (
                                                            <span className="text-green-400">→ {item.url}</span>
                                                        )}
                                                    </span>
                                                </div>

                                                <span className={`text-sm ${item.isPublished ? 'text-green-400' : 'text-gray-500'}`}>
                                                    <FontAwesomeIcon icon={item.isPublished ? faEye : faEyeSlash} />
                                                </span>

                                                <button
                                                    onClick={() => setEditingItem({ sectionId: section.id, item })}
                                                    className="text-gray-400 hover:text-white"
                                                >
                                                    <FontAwesomeIcon icon={faEdit} />
                                                </button>

                                                <button
                                                    onClick={() => {
                                                        if (confirm(`Удалить пункт "${item.name}"?`)) {
                                                            deleteItemMutation.mutate(item.id);
                                                        }
                                                    }}
                                                    className="text-red-400 hover:text-red-300"
                                                >
                                                    <FontAwesomeIcon icon={faTrash} />
                                                </button>
                                            </div>
                                        ))}
                                    </div>

                                    <button
                                        onClick={() => setEditingItem({
                                            sectionId: section.id,
                                            item: null
                                        })}
                                        className="mt-3 btn-secondary w-full"
                                    >
                                        <FontAwesomeIcon icon={faPlus} className="mr-2" />
                                        Добавить пункт
                                    </button>
                                </div>
                            )}
                        </div>
                    ))}

                    {sections.length === 0 && (
                        <div className="text-center py-12 text-gray-400">
                            <p>Нет разделов навигации</p>
                            <p className="text-sm mt-2">Создайте первый раздел для меню сайта</p>
                        </div>
                    )}
                </div>
            )}

            {/* Item Modal */}
            {editingItem && (
                <ItemModal
                    sectionId={editingItem.sectionId}
                    item={editingItem.item}
                    customPages={customPages}
                    onSave={(data) => handleSaveItem(editingItem.sectionId, data)}
                    onClose={() => setEditingItem(null)}
                    isLoading={addItemMutation.isPending || updateItemMutation.isPending}
                />
            )}
        </div>
    );
};

// ==================== Section Form ====================

interface SectionFormProps {
    section: NavSection;
    onSave: (data: Partial<NavSection>) => void;
    onCancel: () => void;
    isLoading: boolean;
}

const SectionForm = ({ section, onSave, onCancel, isLoading }: SectionFormProps) => {
    const [formData, setFormData] = useState({
        name: section.name,
        icon: section.icon || '',
        type: section.type,
        url: section.url || '',
        isExternal: section.isExternal,
        openInNewTab: section.openInNewTab,
        isPublished: section.isPublished,
        order: section.order
    });

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        onSave(formData);
    };

    return (
        <div className="glass-card p-6 mb-6">
            <h2 className="text-xl font-bold mb-4">
                {section.id ? 'Редактировать раздел' : 'Новый раздел'}
            </h2>

            <form onSubmit={handleSubmit} className="space-y-4">
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <div>
                        <label className="block text-sm font-medium mb-2">Название *</label>
                        <input
                            type="text"
                            value={formData.name}
                            onChange={e => setFormData(prev => ({ ...prev, name: e.target.value }))}
                            className="input-field"
                            required
                        />
                    </div>

                    <div>
                        <label className="block text-sm font-medium mb-2">Иконка</label>
                        <select
                            value={formData.icon}
                            onChange={e => setFormData(prev => ({ ...prev, icon: e.target.value }))}
                            className="input-field"
                        >
                            <option value="">Без иконки</option>
                            {AVAILABLE_ICONS.map(icon => (
                                <option key={icon.value} value={icon.value}>{icon.label}</option>
                            ))}
                        </select>
                    </div>
                </div>

                <div>
                    <label className="block text-sm font-medium mb-2">Тип раздела *</label>
                    <div className="flex gap-4">
                        <label className="flex items-center gap-2 cursor-pointer">
                            <input
                                type="radio"
                                checked={formData.type === 'link'}
                                onChange={() => setFormData(prev => ({ ...prev, type: 'link' }))}
                                className="text-highlight"
                            />
                            <span>Простая ссылка</span>
                        </label>
                        <label className="flex items-center gap-2 cursor-pointer">
                            <input
                                type="radio"
                                checked={formData.type === 'dropdown'}
                                onChange={() => setFormData(prev => ({ ...prev, type: 'dropdown' }))}
                                className="text-highlight"
                            />
                            <span>Выпадающее меню</span>
                        </label>
                    </div>
                </div>

                {formData.type === 'link' && (
                    <div className="space-y-4 p-4 bg-gray-800/50 rounded-lg">
                        <div>
                            <label className="block text-sm font-medium mb-2">URL ссылки *</label>
                            <input
                                type="text"
                                value={formData.url}
                                onChange={e => setFormData(prev => ({ ...prev, url: e.target.value }))}
                                className="input-field"
                                placeholder="/news или https://example.com"
                                required={formData.type === 'link'}
                            />
                        </div>

                        <div className="flex gap-6">
                            <label className="flex items-center gap-2 cursor-pointer">
                                <input
                                    type="checkbox"
                                    checked={formData.isExternal}
                                    onChange={e => setFormData(prev => ({ ...prev, isExternal: e.target.checked }))}
                                />
                                <span>Внешняя ссылка</span>
                            </label>

                            <label className="flex items-center gap-2 cursor-pointer">
                                <input
                                    type="checkbox"
                                    checked={formData.openInNewTab}
                                    onChange={e => setFormData(prev => ({ ...prev, openInNewTab: e.target.checked }))}
                                />
                                <span>Открывать в новой вкладке</span>
                            </label>
                        </div>
                    </div>
                )}

                <div>
                    <label className="flex items-center gap-2 cursor-pointer">
                        <input
                            type="checkbox"
                            checked={formData.isPublished}
                            onChange={e => setFormData(prev => ({ ...prev, isPublished: e.target.checked }))}
                        />
                        <span>Опубликовать</span>
                    </label>
                </div>

                <div className="flex gap-4">
                    <button type="button" onClick={onCancel} className="btn-secondary">
                        <FontAwesomeIcon icon={faTimes} className="mr-2" />
                        Отмена
                    </button>
                    <button type="submit" className="btn-primary" disabled={isLoading}>
                        <FontAwesomeIcon icon={faSave} className="mr-2" />
                        {isLoading ? 'Сохранение...' : 'Сохранить'}
                    </button>
                </div>
            </form>
        </div>
    );
};

// ==================== Item Modal ====================

interface ItemModalProps {
    sectionId: number;
    item: NavSectionItem | null;
    customPages: CustomPageOption[];
    onSave: (data: Partial<NavSectionItem>) => void;
    onClose: () => void;
    isLoading: boolean;
}

const ItemModal = ({ item, customPages, onSave, onClose, isLoading }: ItemModalProps) => {
    const [formData, setFormData] = useState({
        name: item?.name || '',
        icon: item?.icon || '',
        type: item?.type || 'internallink',
        url: item?.url || '',
        customPageId: item?.customPageId || undefined,
        openInNewTab: item?.openInNewTab || false,
        isPublished: item?.isPublished ?? true,
        order: item?.order || 0
    });

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        onSave(formData);
    };

    return (
        <div className="fixed inset-0 bg-black/70 flex items-center justify-center z-50 p-4">
            <div className="glass-card p-6 w-full max-w-lg">
                <h2 className="text-xl font-bold mb-4">
                    {item ? 'Редактировать пункт' : 'Добавить пункт'}
                </h2>

                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label className="block text-sm font-medium mb-2">Название *</label>
                        <input
                            type="text"
                            value={formData.name}
                            onChange={e => setFormData(prev => ({ ...prev, name: e.target.value }))}
                            className="input-field"
                            required
                        />
                    </div>

                    <div>
                        <label className="block text-sm font-medium mb-2">Иконка</label>
                        <select
                            value={formData.icon}
                            onChange={e => setFormData(prev => ({ ...prev, icon: e.target.value }))}
                            className="input-field"
                        >
                            <option value="">Без иконки</option>
                            {AVAILABLE_ICONS.map(icon => (
                                <option key={icon.value} value={icon.value}>{icon.label}</option>
                            ))}
                        </select>
                    </div>

                    <div>
                        <label className="block text-sm font-medium mb-2">Тип ссылки *</label>
                        <select
                            value={formData.type}
                            onChange={e => setFormData(prev => ({
                                ...prev,
                                type: e.target.value as 'internallink' | 'externallink' | 'custompage'
                            }))}
                            className="input-field"
                        >
                            <option value="internallink">Внутренняя ссылка</option>
                            <option value="externallink">Внешняя ссылка</option>
                            <option value="custompage">Кастомная страница</option>
                        </select>
                    </div>

                    {formData.type === 'custompage' ? (
                        <div>
                            <label className="block text-sm font-medium mb-2">Страница *</label>
                            <select
                                value={formData.customPageId || ''}
                                onChange={e => setFormData(prev => ({
                                    ...prev,
                                    customPageId: e.target.value ? Number(e.target.value) : undefined
                                }))}
                                className="input-field"
                                required
                            >
                                <option value="">Выберите страницу</option>
                                {customPages.map(page => (
                                    <option key={page.id} value={page.id}>{page.title}</option>
                                ))}
                            </select>
                        </div>
                    ) : (
                        <div>
                            <label className="block text-sm font-medium mb-2">URL *</label>
                            <input
                                type="text"
                                value={formData.url}
                                onChange={e => setFormData(prev => ({ ...prev, url: e.target.value }))}
                                className="input-field"
                                placeholder={formData.type === 'externallink' ? 'https://example.com' : '/news'}
                                required
                            />
                        </div>
                    )}

                    <div className="flex gap-6">
                        <label className="flex items-center gap-2 cursor-pointer">
                            <input
                                type="checkbox"
                                checked={formData.openInNewTab}
                                onChange={e => setFormData(prev => ({ ...prev, openInNewTab: e.target.checked }))}
                            />
                            <span>Открывать в новой вкладке</span>
                        </label>

                        <label className="flex items-center gap-2 cursor-pointer">
                            <input
                                type="checkbox"
                                checked={formData.isPublished}
                                onChange={e => setFormData(prev => ({ ...prev, isPublished: e.target.checked }))}
                            />
                            <span>Опубликовать</span>
                        </label>
                    </div>

                    <div className="flex gap-4 pt-4">
                        <button type="button" onClick={onClose} className="btn-secondary flex-1">
                            Отмена
                        </button>
                        <button type="submit" className="btn-primary flex-1" disabled={isLoading}>
                            {isLoading ? 'Сохранение...' : 'Сохранить'}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default AdminNavSections;