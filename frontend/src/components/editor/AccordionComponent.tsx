import { NodeViewWrapper, NodeViewContent } from '@tiptap/react';
import { useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faChevronDown, faTrash, faGear } from '@fortawesome/free-solid-svg-icons';

const AccordionComponent = ({ node, updateAttributes, deleteNode }: any) => {
    const [showSettings, setShowSettings] = useState(false);
    const [collapsedHeight, setCollapsedHeight] = useState(node.attrs.collapsedHeight || 150);
    const [title, setTitle] = useState(node.attrs.title || 'Раскрыть');

    const handleSettingsSave = (e: React.MouseEvent) => {
        e.preventDefault();
        e.stopPropagation();
        updateAttributes({ title, collapsedHeight });
        setShowSettings(false);
    };

    return (
        <NodeViewWrapper className="accordion-wrapper my-4">
            <div className="accordion-container relative border-2 border-dashed border-orange-500/50 rounded-lg overflow-hidden bg-gray-800/50">
                {/* Панель управления */}
                <div className="accordion-controls flex items-center justify-between px-3 py-2 bg-orange-500/20 border-b border-orange-500/30">
                    <div className="flex items-center gap-2">
                        <FontAwesomeIcon icon={faChevronDown} className="text-orange-500" />
                        <span className="text-sm text-orange-400 font-medium">Раскрывающийся блок</span>
                        <span className="text-xs text-gray-400">• Кнопка: "{title}" • Высота: {collapsedHeight}px</span>
                    </div>
                    <div className="flex items-center gap-1">
                        <button
                            type="button"
                            onClick={(e) => {
                                e.preventDefault();
                                e.stopPropagation();
                                setShowSettings(!showSettings);
                            }}
                            className={`p-1.5 transition-colors rounded ${showSettings ? 'text-orange-500 bg-orange-500/20' : 'text-gray-400 hover:text-white hover:bg-gray-600'}`}
                            title="Настройки блока"
                        >
                            <FontAwesomeIcon icon={faGear} size="sm" />
                        </button>
                        <button
                            type="button"
                            onClick={(e) => {
                                e.preventDefault();
                                e.stopPropagation();
                                deleteNode();
                            }}
                            className="p-1.5 text-gray-400 hover:text-red-500 transition-colors rounded hover:bg-gray-600"
                            title="Удалить блок"
                        >
                            <FontAwesomeIcon icon={faTrash} size="sm" />
                        </button>
                    </div>
                </div>

                {/* Настройки */}
                {showSettings && (
                    <div
                        className="accordion-settings p-4 bg-gray-700 border-b border-gray-600 space-y-4"
                        onClick={(e) => e.stopPropagation()}
                    >
                        <div className="grid grid-cols-2 gap-4">
                            <div>
                                <label className="block text-xs text-gray-400 mb-1.5">Текст кнопки "Раскрыть"</label>
                                <input
                                    type="text"
                                    value={title}
                                    onChange={(e) => setTitle(e.target.value)}
                                    placeholder="Раскрыть, Показать больше..."
                                    className="w-full bg-gray-800 border border-gray-600 rounded px-3 py-2 text-sm text-white focus:outline-none focus:border-orange-500"
                                    onClick={(e) => e.stopPropagation()}
                                />
                            </div>
                            <div>
                                <label className="block text-xs text-gray-400 mb-1.5">Высота в свёрнутом виде (px)</label>
                                <input
                                    type="number"
                                    value={collapsedHeight}
                                    onChange={(e) => setCollapsedHeight(parseInt(e.target.value) || 150)}
                                    min={50}
                                    max={500}
                                    className="w-full bg-gray-800 border border-gray-600 rounded px-3 py-2 text-sm text-white focus:outline-none focus:border-orange-500"
                                    onClick={(e) => e.stopPropagation()}
                                />
                            </div>
                        </div>
                        <div className="flex items-center gap-3">
                            <button
                                type="button"
                                onClick={handleSettingsSave}
                                className="px-4 py-2 bg-orange-500 hover:bg-orange-600 text-white text-sm rounded transition-colors"
                            >
                                Сохранить настройки
                            </button>
                            <span className="text-xs text-gray-500">
                                При просмотре контент будет обрезан до {collapsedHeight}px с кнопкой "{title}"
                            </span>
                        </div>
                    </div>
                )}

                {/* Контент - в редакторе всегда полностью виден */}
                <div className="accordion-content p-4">
                    <NodeViewContent className="accordion-inner-content" />
                </div>

                {/* Превью как будет выглядеть */}
                <div className="accordion-preview px-4 py-3 bg-gray-900/50 border-t border-gray-600">
                    <div className="flex items-center justify-center gap-2 text-sm text-gray-400">
                        <span>При просмотре появится кнопка:</span>
                        <span className="px-3 py-1 bg-gray-700 rounded text-gray-300 flex items-center gap-2">
                            {title}
                            <FontAwesomeIcon icon={faChevronDown} className="text-orange-500" size="sm" />
                        </span>
                    </div>
                </div>
            </div>
        </NodeViewWrapper>
    );
};

export default AccordionComponent;