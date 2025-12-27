import { Node, mergeAttributes } from '@tiptap/core';
import { ReactNodeViewRenderer } from '@tiptap/react';
import AccordionComponent from '../AccordionComponent';

export interface AccordionOptions {
    HTMLAttributes: Record<string, any>;
}

declare module '@tiptap/core' {
    interface Commands<ReturnType> {
        accordion: {
            setAccordion: (attributes?: { title?: string; collapsedHeight?: number }) => ReturnType;
            toggleAccordion: () => ReturnType;
        };
    }
}

export const Accordion = Node.create<AccordionOptions>({
    name: 'accordion',

    group: 'block',

    content: 'block+',

    defining: true,

    addAttributes() {
        return {
            title: {
                default: 'Скрытый контент',
                parseHTML: element => element.getAttribute('data-title'),
                renderHTML: attributes => ({
                    'data-title': attributes.title,
                }),
            },
            collapsedHeight: {
                default: 150,
                parseHTML: element => parseInt(element.getAttribute('data-collapsed-height') || '150'),
                renderHTML: attributes => ({
                    'data-collapsed-height': attributes.collapsedHeight,
                }),
            },
            isExpanded: {
                default: false,
                parseHTML: element => element.getAttribute('data-expanded') === 'true',
                renderHTML: attributes => ({
                    'data-expanded': attributes.isExpanded,
                }),
            },
        };
    },

    parseHTML() {
        return [
            {
                tag: 'div[data-type="accordion"]',
            },
        ];
    },

    renderHTML({ HTMLAttributes }) {
        return ['div', mergeAttributes({ 'data-type': 'accordion' }, HTMLAttributes), 0];
    },

    addNodeView() {
        return ReactNodeViewRenderer(AccordionComponent);
    },

    addCommands() {
        return {
            setAccordion:
                (attributes) =>
                    ({ commands }) => {
                        return commands.insertContent({
                            type: this.name,
                            attrs: { ...attributes },
                            content: [
                                {
                                    type: 'paragraph',
                                    content: [{ type: 'text', text: 'Содержимое скрытого блока. Добавьте сюда текст, изображения или другой контент...' }],
                                },
                            ],
                        });
                    },
            toggleAccordion:
                () =>
                    ({ commands }) => {
                        return commands.toggleNode(this.name, 'paragraph');
                    },
        };
    },
});

export default Accordion;