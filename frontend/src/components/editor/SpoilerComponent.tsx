import { NodeViewWrapper, NodeViewContent } from '@tiptap/react';
import { useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEye, faEyeSlash, faTrash, faPencil } from '@fortawesome/free-solid-svg-icons';

const SpoilerComponent = ({ node, updateAttributes, deleteNode }: any) => {
    const [isRevealed, setIsRevealed] = useState(false);
    const [isEditing, setIsEditing] = useState(false);
    const [label, setLabel] = useState(node.attrs.label);

    const handleLabelSave = () => {
        updateAttributes({ label });
        setIsEditing(false);
    };

    return (
        <NodeViewWrapper className="spoiler-wrapper my-4">
            <div className="spoiler-container border-2 border-purple-600 rounded-lg overflow-hidden bg-purple-900/20">
                {/* Header */}
                <div className="spoiler-header flex items-center justify-between p-3 bg-purple-800/50">
                    <div className="flex items-center gap-3">
                        <FontAwesomeIcon
                            icon={isRevealed ? faEye : faEyeSlash}
                            className="text-purple-400"
                        />
                        {isEditing ? (
                            <input
                                type="text"
                                value={label}
                                onChange={(e) => setLabel(e.target.value)}
                                onBlur={handleLabelSave}
                                onKeyDown={(e) => e.key === 'Enter' && handleLabelSave()}
                                className="bg-gray-800 border border-gray-500 rounded px-2 py-1 text-white text-sm focus:outline-none focus:border-purple-400"
                                autoFocus
                            />
                        ) : (
                            <span className="text-purple-300 text-sm font-medium">{node.attrs.label}</span>
                        )}
                    </div>
                    <div className="flex items-center gap-2">
                        <button
                            type="button"
                            onClick={() => setIsRevealed(!isRevealed)}
                            className="px-3 py-1 text-sm bg-purple-600 hover:bg-purple-500 text-white rounded transition-colors"
                        >
                            {isRevealed ? 'Скрыть' : 'Показать'}
                        </button>
                        <button
                            type="button"
                            onClick={() => setIsEditing(true)}
                            className="p-2 text-gray-400 hover:text-white transition-colors"
                            title="Редактировать метку"
                        >
                            <FontAwesomeIcon icon={faPencil} size="sm" />
                        </button>
                        <button
                            type="button"
                            onClick={deleteNode}
                            className="p-2 text-gray-400 hover:text-red-500 transition-colors"
                            title="Удалить блок"
                        >
                            <FontAwesomeIcon icon={faTrash} size="sm" />
                        </button>
                    </div>
                </div>

                {/* Content */}
                <div
                    className={`spoiler-content transition-all duration-300 ${isRevealed
                            ? 'max-h-[2000px] opacity-100 p-4'
                            : 'max-h-0 opacity-0 overflow-hidden'
                        }`}
                >
                    <NodeViewContent className="spoiler-inner-content" />
                </div>
            </div>
        </NodeViewWrapper>
    );
};

export default SpoilerComponent;