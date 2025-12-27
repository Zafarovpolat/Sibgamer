import { Node, mergeAttributes } from '@tiptap/core';
import { ReactNodeViewRenderer } from '@tiptap/react';
import SpoilerComponent from '../SpoilerComponent';

declare module '@tiptap/core' {
    interface Commands<ReturnType> {
        spoiler: {
            setSpoiler: () => ReturnType;
        };
    }
}

export const Spoiler = Node.create({
    name: 'spoiler',

    group: 'block',

    content: 'block+',

    defining: true,

    addAttributes() {
        return {
            label: {
                default: 'Спойлер',
                parseHTML: element => element.getAttribute('data-label'),
                renderHTML: attributes => ({
                    'data-label': attributes.label,
                }),
            },
        };
    },

    parseHTML() {
        return [
            {
                tag: 'div[data-type="spoiler"]',
            },
        ];
    },

    renderHTML({ HTMLAttributes }) {
        return ['div', mergeAttributes({ 'data-type': 'spoiler' }, HTMLAttributes), 0];
    },

    addNodeView() {
        return ReactNodeViewRenderer(SpoilerComponent);
    },

    addCommands() {
        return {
            setSpoiler:
                () =>
                    ({ commands }) => {
                        return commands.insertContent({
                            type: this.name,
                            content: [
                                {
                                    type: 'paragraph',
                                    content: [{ type: 'text', text: 'Скрытый контент...' }],
                                },
                            ],
                        });
                    },
        };
    },
});

export default Spoiler;