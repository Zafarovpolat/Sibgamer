import { useEffect, useState, useCallback } from 'react';
import { useEditor, EditorContent } from '@tiptap/react';
import type { Editor } from '@tiptap/react';
import StarterKit from '@tiptap/starter-kit';
import Link from '@tiptap/extension-link';
import Image from '@tiptap/extension-image';
import Youtube from '@tiptap/extension-youtube';
import { Table } from '@tiptap/extension-table';
import TableRow from '@tiptap/extension-table-row';
import TableCell from '@tiptap/extension-table-cell';
import TableHeader from '@tiptap/extension-table-header';
import TextAlign from '@tiptap/extension-text-align';
import { TextStyle } from '@tiptap/extension-text-style';
import { Color } from '@tiptap/extension-color';
import Highlight from '@tiptap/extension-highlight';
import Underline from '@tiptap/extension-underline';
import CodeBlockLowlight from '@tiptap/extension-code-block-lowlight';
import Placeholder from '@tiptap/extension-placeholder';
import { common, createLowlight } from 'lowlight';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
  faBold, faItalic, faUnderline, faStrikethrough, faCode,
  faListUl, faListOl, faQuoteLeft, faLink, faImage,
  faTable, faAlignLeft, faAlignCenter, faAlignRight, faAlignJustify,
  faUndo, faRedo, faHighlighter, faTableCells, faChevronDown,
  faInfoCircle, faEyeSlash, faPalette
} from '@fortawesome/free-solid-svg-icons';
import { faYoutube } from '@fortawesome/free-brands-svg-icons';

// Импорт кастомных расширений
import Accordion from './editor/extensions/Accordion';
import Callout from './editor/extensions/Callout';
import Spoiler from './editor/extensions/Spoiler';

// Импорт модальных окон
import { LinkModal, ImageModal, YoutubeModal, ColorModal } from './editor/EditorModal';

import { uploadMedia } from '../lib/media';
import './RichTextEditor.css';

const lowlight = createLowlight(common);

interface RichTextEditorProps {
  content: string;
  onChange: (content: string) => void;
  placeholder?: string;
}

interface ModalState {
  link: boolean;
  image: boolean;
  youtube: boolean;
  color: boolean;
}

const MenuBar = ({
  editor,
  onOpenModal
}: {
  editor: Editor | null;
  onOpenModal: (modal: keyof ModalState) => void;
}) => {
  if (!editor) {
    return null;
  }

  const addTable = () => {
    editor.chain().focus().insertTable({ rows: 3, cols: 3, withHeaderRow: true }).run();
  };

  return (
    <div className="editor-menu-bar">
      {/* Undo/Redo */}
      <div className="menu-group">
        <button
          type="button"
          onClick={() => editor.chain().focus().undo().run()}
          disabled={!editor.can().undo()}
          title="Отменить (Ctrl+Z)"
          className="menu-button"
        >
          <FontAwesomeIcon icon={faUndo} />
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().redo().run()}
          disabled={!editor.can().redo()}
          title="Повторить (Ctrl+Y)"
          className="menu-button"
        >
          <FontAwesomeIcon icon={faRedo} />
        </button>
      </div>

      <div className="menu-divider" />

      {/* Заголовки */}
      <div className="menu-group">
        <select
          onChange={(e) => {
            const value = e.target.value;
            if (value === 'paragraph') {
              editor.chain().focus().setParagraph().run();
            } else if (value.startsWith('heading')) {
              const level = parseInt(value.replace('heading', ''));
              const lvl = Math.min(Math.max(level, 1), 6) as 1 | 2 | 3 | 4 | 5 | 6;
              editor.chain().focus().toggleHeading({ level: lvl }).run();
            }
          }}
          className="menu-select"
          value={
            editor.isActive('heading', { level: 1 }) ? 'heading1' :
              editor.isActive('heading', { level: 2 }) ? 'heading2' :
                editor.isActive('heading', { level: 3 }) ? 'heading3' :
                  editor.isActive('heading', { level: 4 }) ? 'heading4' :
                    editor.isActive('heading', { level: 5 }) ? 'heading5' :
                      editor.isActive('heading', { level: 6 }) ? 'heading6' :
                        'paragraph'
          }
        >
          <option value="paragraph">Параграф</option>
          <option value="heading1">Заголовок 1</option>
          <option value="heading2">Заголовок 2</option>
          <option value="heading3">Заголовок 3</option>
          <option value="heading4">Заголовок 4</option>
          <option value="heading5">Заголовок 5</option>
          <option value="heading6">Заголовок 6</option>
        </select>
      </div>

      <div className="menu-divider" />

      {/* Форматирование текста */}
      <div className="menu-group">
        <button
          type="button"
          onClick={() => editor.chain().focus().toggleBold().run()}
          className={`menu-button ${editor.isActive('bold') ? 'is-active' : ''}`}
          title="Жирный (Ctrl+B)"
        >
          <FontAwesomeIcon icon={faBold} />
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().toggleItalic().run()}
          className={`menu-button ${editor.isActive('italic') ? 'is-active' : ''}`}
          title="Курсив (Ctrl+I)"
        >
          <FontAwesomeIcon icon={faItalic} />
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().toggleUnderline().run()}
          className={`menu-button ${editor.isActive('underline') ? 'is-active' : ''}`}
          title="Подчеркнутый (Ctrl+U)"
        >
          <FontAwesomeIcon icon={faUnderline} />
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().toggleStrike().run()}
          className={`menu-button ${editor.isActive('strike') ? 'is-active' : ''}`}
          title="Зачеркнутый"
        >
          <FontAwesomeIcon icon={faStrikethrough} />
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().toggleHighlight().run()}
          className={`menu-button ${editor.isActive('highlight') ? 'is-active' : ''}`}
          title="Выделение"
        >
          <FontAwesomeIcon icon={faHighlighter} />
        </button>
        <button
          type="button"
          onClick={() => onOpenModal('color')}
          className="menu-button"
          title="Цвет текста"
        >
          <FontAwesomeIcon icon={faPalette} style={{ color: editor.getAttributes('textStyle').color || '#fff' }} />
        </button>
      </div>

      <div className="menu-divider" />

      {/* Выравнивание */}
      <div className="menu-group">
        <button
          type="button"
          onClick={() => editor.chain().focus().setTextAlign('left').run()}
          className={`menu-button ${editor.isActive({ textAlign: 'left' }) ? 'is-active' : ''}`}
          title="По левому краю"
        >
          <FontAwesomeIcon icon={faAlignLeft} />
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().setTextAlign('center').run()}
          className={`menu-button ${editor.isActive({ textAlign: 'center' }) ? 'is-active' : ''}`}
          title="По центру"
        >
          <FontAwesomeIcon icon={faAlignCenter} />
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().setTextAlign('right').run()}
          className={`menu-button ${editor.isActive({ textAlign: 'right' }) ? 'is-active' : ''}`}
          title="По правому краю"
        >
          <FontAwesomeIcon icon={faAlignRight} />
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().setTextAlign('justify').run()}
          className={`menu-button ${editor.isActive({ textAlign: 'justify' }) ? 'is-active' : ''}`}
          title="По ширине"
        >
          <FontAwesomeIcon icon={faAlignJustify} />
        </button>
      </div>

      <div className="menu-divider" />

      {/* Списки */}
      <div className="menu-group">
        <button
          type="button"
          onClick={() => editor.chain().focus().toggleBulletList().run()}
          className={`menu-button ${editor.isActive('bulletList') ? 'is-active' : ''}`}
          title="Маркированный список"
        >
          <FontAwesomeIcon icon={faListUl} />
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().toggleOrderedList().run()}
          className={`menu-button ${editor.isActive('orderedList') ? 'is-active' : ''}`}
          title="Нумерованный список"
        >
          <FontAwesomeIcon icon={faListOl} />
        </button>
      </div>

      <div className="menu-divider" />

      {/* Блоки */}
      <div className="menu-group">
        <button
          type="button"
          onClick={() => editor.chain().focus().toggleBlockquote().run()}
          className={`menu-button ${editor.isActive('blockquote') ? 'is-active' : ''}`}
          title="Цитата"
        >
          <FontAwesomeIcon icon={faQuoteLeft} />
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().toggleCodeBlock().run()}
          className={`menu-button ${editor.isActive('codeBlock') ? 'is-active' : ''}`}
          title="Блок кода"
        >
          <FontAwesomeIcon icon={faCode} />
        </button>
        <button
          type="button"
          onClick={() => onOpenModal('link')}
          className={`menu-button ${editor.isActive('link') ? 'is-active' : ''}`}
          title="Вставить ссылку"
        >
          <FontAwesomeIcon icon={faLink} />
        </button>
        <button
          type="button"
          onClick={() => onOpenModal('image')}
          className="menu-button"
          title="Вставить изображение"
        >
          <FontAwesomeIcon icon={faImage} />
        </button>
        <button
          type="button"
          onClick={() => onOpenModal('youtube')}
          className="menu-button"
          title="Вставить YouTube видео"
        >
          <FontAwesomeIcon icon={faYoutube} />
        </button>
        <button
          type="button"
          onClick={addTable}
          className="menu-button"
          title="Вставить таблицу"
        >
          <FontAwesomeIcon icon={faTable} />
        </button>
      </div>

      <div className="menu-divider" />

      {/* Специальные блоки */}
      <div className="menu-group">
        <button
          type="button"
          onClick={() => editor.chain().focus().setAccordion({ title: 'Раскрыть' }).run()}
          className={`menu-button ${editor.isActive('accordion') ? 'is-active' : ''}`}
          title="Раскрывающийся блок (Accordion)"
        >
          <FontAwesomeIcon icon={faChevronDown} />
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().setCallout({ type: 'info' }).run()}
          className={`menu-button ${editor.isActive('callout') ? 'is-active' : ''}`}
          title="Информационный блок (Callout)"
        >
          <FontAwesomeIcon icon={faInfoCircle} />
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().setSpoiler().run()}
          className={`menu-button ${editor.isActive('spoiler') ? 'is-active' : ''}`}
          title="Спойлер"
        >
          <FontAwesomeIcon icon={faEyeSlash} />
        </button>
      </div>

      {/* Управление таблицей */}
      {editor.isActive('table') && (
        <>
          <div className="menu-divider" />
          <div className="menu-group">
            <button
              type="button"
              onClick={() => editor.chain().focus().addColumnBefore().run()}
              className="menu-button"
              title="Добавить колонку слева"
            >
              ← Col
            </button>
            <button
              type="button"
              onClick={() => editor.chain().focus().addColumnAfter().run()}
              className="menu-button"
              title="Добавить колонку справа"
            >
              Col →
            </button>
            <button
              type="button"
              onClick={() => editor.chain().focus().deleteColumn().run()}
              className="menu-button"
              title="Удалить колонку"
            >
              Del Col
            </button>
            <button
              type="button"
              onClick={() => editor.chain().focus().addRowBefore().run()}
              className="menu-button"
              title="Добавить строку сверху"
            >
              ↑ Row
            </button>
            <button
              type="button"
              onClick={() => editor.chain().focus().addRowAfter().run()}
              className="menu-button"
              title="Добавить строку снизу"
            >
              Row ↓
            </button>
            <button
              type="button"
              onClick={() => editor.chain().focus().deleteRow().run()}
              className="menu-button"
              title="Удалить строку"
            >
              Del Row
            </button>
            <button
              type="button"
              onClick={() => editor.chain().focus().deleteTable().run()}
              className="menu-button text-red-500"
              title="Удалить таблицу"
            >
              <FontAwesomeIcon icon={faTableCells} /> ✕
            </button>
          </div>
        </>
      )}

      <div className="menu-divider" />

      {/* Разделители */}
      <div className="menu-group">
        <button
          type="button"
          onClick={() => editor.chain().focus().setHorizontalRule().run()}
          className="menu-button"
          title="Горизонтальная линия"
        >
          ―
        </button>
        <button
          type="button"
          onClick={() => editor.chain().focus().setHardBreak().run()}
          className="menu-button"
          title="Перенос строки"
        >
          ↵
        </button>
      </div>
    </div>
  );
};

const RichTextEditor = ({ content, onChange, placeholder = 'Начните писать свою статью...' }: RichTextEditorProps) => {
  const [modals, setModals] = useState<ModalState>({
    link: false,
    image: false,
    youtube: false,
    color: false,
  });

  const openModal = useCallback((modal: keyof ModalState) => {
    setModals(prev => ({ ...prev, [modal]: true }));
  }, []);

  const closeModal = useCallback((modal: keyof ModalState) => {
    setModals(prev => ({ ...prev, [modal]: false }));
  }, []);

  const editor = useEditor({
    extensions: [
      StarterKit.configure({
        codeBlock: false,
      }),
      Underline,
      Link.configure({
        openOnClick: false,
        HTMLAttributes: {
          class: 'text-highlight hover:underline cursor-pointer',
        },
      }),
      Image.configure({
        inline: true,
        allowBase64: true,
        HTMLAttributes: {
          class: 'rounded-lg max-w-full h-auto my-4',
        },
      }),
      Youtube.configure({
        width: 640,
        height: 360,
        HTMLAttributes: {
          class: 'rounded-lg my-4',
        },
      }),
      Table.configure({
        resizable: true,
        HTMLAttributes: {
          class: 'border-collapse table-auto w-full my-4',
        },
      }),
      TableRow,
      TableCell.configure({
        HTMLAttributes: {
          class: 'border border-gray-600 p-2',
        },
      }),
      TableHeader.configure({
        HTMLAttributes: {
          class: 'border border-gray-600 p-2 bg-gray-800 font-bold',
        },
      }),
      TextAlign.configure({
        types: ['heading', 'paragraph'],
      }),
      TextStyle,
      Color,
      Highlight.configure({
        multicolor: true,
      }),
      CodeBlockLowlight.configure({
        lowlight,
        HTMLAttributes: {
          class: 'bg-gray-900 text-green-400 p-4 rounded-lg my-4 overflow-x-auto',
        },
      }),
      Placeholder.configure({
        placeholder,
      }),
      // Кастомные расширения
      Accordion,
      Callout,
      Spoiler,
    ],
    content,
    onUpdate: ({ editor }) => {
      onChange(editor.getHTML());
    },
    editorProps: {
      attributes: {
        class: 'prose prose-invert max-w-none focus:outline-none min-h-[500px] p-6',
      },
      handleDrop: (view, event, slice, moved) => {
        if (!moved && event.dataTransfer?.files?.length) {
          const file = event.dataTransfer.files[0];
          if (file.type.startsWith('image/')) {
            event.preventDefault();
            handleImageUpload(file);
            return true;
          }
        }
        return false;
      },
      handlePaste: (view, event) => {
        const items = event.clipboardData?.items;
        if (items) {
          for (const item of items) {
            if (item.type.startsWith('image/')) {
              event.preventDefault();
              const file = item.getAsFile();
              if (file) handleImageUpload(file);
              return true;
            }
          }
        }
        return false;
      },
    },
  });

  const handleImageUpload = async (file: File) => {
    if (!editor) return;

    try {
      const url = await uploadMedia(file);
      editor.chain().focus().setImage({ src: url }).run();
    } catch (error) {
      console.error('Failed to upload image:', error);
      alert('Ошибка загрузки изображения');
    }
  };

  const handleLinkSubmit = useCallback((url: string) => {
    if (!editor) return;

    if (url === '') {
      editor.chain().focus().extendMarkRange('link').unsetLink().run();
    } else {
      editor.chain().focus().extendMarkRange('link').setLink({ href: url }).run();
    }
  }, [editor]);

  const handleImageSubmit = useCallback((url: string) => {
    if (!editor) return;
    editor.chain().focus().setImage({ src: url }).run();
  }, [editor]);

  const handleYoutubeSubmit = useCallback((url: string) => {
    if (!editor) return;
    editor.commands.setYoutubeVideo({ src: url, width: 640, height: 480 });
  }, [editor]);

  const handleColorSubmit = useCallback((color: string) => {
    if (!editor) return;
    editor.chain().focus().setColor(color).run();
  }, [editor]);

  useEffect(() => {
    if (editor && content !== editor.getHTML()) {
      editor.commands.setContent(content);
    }
  }, [content, editor]);

  return (
    <div className="rich-text-editor-wrapper">
      <MenuBar editor={editor} onOpenModal={openModal} />
      <EditorContent editor={editor} className="editor-content" />

      {/* Модальные окна */}
      <LinkModal
        isOpen={modals.link}
        onClose={() => closeModal('link')}
        onSubmit={handleLinkSubmit}
        initialUrl={editor?.getAttributes('link').href || ''}
      />

      <ImageModal
        isOpen={modals.image}
        onClose={() => closeModal('image')}
        onSubmit={handleImageSubmit}
        onUpload={uploadMedia}
      />

      <YoutubeModal
        isOpen={modals.youtube}
        onClose={() => closeModal('youtube')}
        onSubmit={handleYoutubeSubmit}
      />

      <ColorModal
        isOpen={modals.color}
        onClose={() => closeModal('color')}
        onSubmit={handleColorSubmit}
        currentColor={editor?.getAttributes('textStyle').color || '#ffffff'}
      />
    </div>
  );
};

export default RichTextEditor;