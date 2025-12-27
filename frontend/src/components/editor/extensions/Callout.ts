import { Node, mergeAttributes } from '@tiptap/core';
import { ReactNodeViewRenderer } from '@tiptap/react';
import CalloutComponent from '../CalloutComponent';

export type CalloutType = 'info' | 'warning' | 'success' | 'error';

export interface CalloutOptions {
    HTMLAttributes: Record<string, any>;
}

declare module '@tiptap/core' {
    interface Commands<ReturnType> {
        callout: {
            setCallout: (attributes?: { type?: CalloutType }) => ReturnType;
        };
    }
}

export const Callout = Node.create<CalloutOptions>({
    name: 'callout',

    group: 'block',

    content: 'block+',

    defining: true,

    addAttributes() {
        return {
            type: {
                default: 'info',
                parseHTML: element => element.getAttribute('data-callout-type') || 'info',
                renderHTML: attributes => ({
                    'data-callout-type': attributes.type,
                }),
            },
        };
    },

    parseHTML() {
        return [
            {
                tag: 'div[data-type="callout"]',
            },
        ];
    },

    renderHTML({ HTMLAttributes }) {
        return ['div', mergeAttributes({ 'data-type': 'callout' }, HTMLAttributes), 0];
    },

    addNodeView() {
        return ReactNodeViewRenderer(CalloutComponent);
    },

    addCommands() {
        return {
            setCallout:
                (attributes) =>
                    ({ commands }) => {
                        return commands.insertContent({
                            type: this.name,
                            attrs: attributes,
                            content: [
                                {
                                    type: 'paragraph',
                                    content: [{ type: 'text', text: 'Текст уведомления...' }],
                                },
                            ],
                        });
                    },
        };
    },
});

export default Callout;