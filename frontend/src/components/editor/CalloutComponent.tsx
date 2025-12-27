import { NodeViewWrapper, NodeViewContent } from '@tiptap/react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faInfoCircle,
    faExclamationTriangle,
    faCheckCircle,
    faTimesCircle,
    faTrash
} from '@fortawesome/free-solid-svg-icons';

type CalloutType = 'info' | 'warning' | 'success' | 'error';

const calloutStyles: Record<CalloutType, { bg: string; border: string; icon: any; iconColor: string }> = {
    info: {
        bg: 'bg-blue-900/30',
        border: 'border-blue-500',
        icon: faInfoCircle,
        iconColor: 'text-blue-400',
    },
    warning: {
        bg: 'bg-yellow-900/30',
        border: 'border-yellow-500',
        icon: faExclamationTriangle,
        iconColor: 'text-yellow-400',
    },
    success: {
        bg: 'bg-green-900/30',
        border: 'border-green-500',
        icon: faCheckCircle,
        iconColor: 'text-green-400',
    },
    error: {
        bg: 'bg-red-900/30',
        border: 'border-red-500',
        icon: faTimesCircle,
        iconColor: 'text-red-400',
    },
};

const CalloutComponent = ({ node, updateAttributes, deleteNode }: any) => {
    const type: CalloutType = node.attrs.type || 'info';
    const style = calloutStyles[type];

    const cycleType = () => {
        const types: CalloutType[] = ['info', 'warning', 'success', 'error'];
        const currentIndex = types.indexOf(type);
        const nextType = types[(currentIndex + 1) % types.length];
        updateAttributes({ type: nextType });
    };

    return (
        <NodeViewWrapper className="callout-wrapper my-4">
            <div className={`callout-container ${style.bg} ${style.border} border-l-4 rounded-r-lg p-4`}>
                <div className="flex items-start gap-3">
                    <button
                        type="button"
                        onClick={cycleType}
                        className={`${style.iconColor} hover:opacity-80 transition-opacity mt-1`}
                        title="Изменить тип (клик для переключения)"
                    >
                        <FontAwesomeIcon icon={style.icon} size="lg" />
                    </button>
                    <div className="flex-1">
                        <NodeViewContent className="callout-content" />
                    </div>
                    <button
                        type="button"
                        onClick={deleteNode}
                        className="text-gray-400 hover:text-red-500 transition-colors"
                        title="Удалить блок"
                    >
                        <FontAwesomeIcon icon={faTrash} size="sm" />
                    </button>
                </div>
            </div>
        </NodeViewWrapper>
    );
};

export default CalloutComponent;