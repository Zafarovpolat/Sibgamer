import { useEffect } from 'react';
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
  faUndo, faRedo, faHighlighter, faTableCells
} from '@fortawesome/free-solid-svg-icons';
import { faYoutube } from '@fortawesome/free-brands-svg-icons';
import './RichTextEditor.css';

const lowlight = createLowlight(common);

interface RichTextEditorProps {
  content: string;
  onChange: (content: string) => void;
  placeholder?: string;
}

const MenuBar = ({ editor }: { editor: Editor | null }) => {
  if (!editor) {
    return null;
  }

  const addYoutubeVideo = () => {
    const url = prompt('Введите YouTube URL:');
    if (url) {
      editor.commands.setYoutubeVideo({ src: url, width: 640, height: 480 });
    }
  };

  const addImage = () => {
    const url = prompt('Введите URL изображения:');
    if (url) {
      editor.chain().focus().setImage({ src: url }).run();
    }
  };

  const setLink = () => {
    const previousUrl = editor.getAttributes('link').href;
    const url = prompt('Введите URL:', previousUrl);

    if (url === null) {
      return;
    }

    if (url === '') {
      editor.chain().focus().extendMarkRange('link').unsetLink().run();
      return;
    }

    editor.chain().focus().extendMarkRange('link').setLink({ href: url }).run();
  };

  const addTable = () => {
    editor.chain().focus().insertTable({ rows: 3, cols: 3, withHeaderRow: true }).run();
  };

  const setColor = () => {
    const color = prompt('Введите цвет (например, #ff0000):');
    if (color) {
      editor.chain().focus().setColor(color).run();
    }
  };

  return (
    <div className="editor-menu-bar">
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
          onClick={setColor}
          className="menu-button"
          title="Цвет текста"
        >
          <span style={{ color: editor.getAttributes('textStyle').color || '#fff' }}>A</span>
        </button>
      </div>

      <div className="menu-divider" />

      <div className="menu-group">
        <button
          type="button"
          onClick={() => editor.chain().focus().setTextAlign('left').run()}
          className={`menu-button ${editor.isActive({ textAlign: 'left' }) || (!editor.isActive({ textAlign: 'center' }) && !editor.isActive({ textAlign: 'right' }) && !editor.isActive({ textAlign: 'justify' })) ? 'is-active' : ''}`}
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
          onClick={setLink}
          className={`menu-button ${editor.isActive('link') ? 'is-active' : ''}`}
          title="Вставить ссылку"
        >
          <FontAwesomeIcon icon={faLink} />
        </button>
        <button
          type="button"
          onClick={addImage}
          className="menu-button"
          title="Вставить изображение"
        >
          <FontAwesomeIcon icon={faImage} />
        </button>
        <button
          type="button"
          onClick={addYoutubeVideo}
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
    ],
    content,
    onUpdate: ({ editor }) => {
      onChange(editor.getHTML());
    },
    editorProps: {
      attributes: {
        class: 'prose prose-invert max-w-none focus:outline-none min-h-[500px] p-6',
      },
    },
  });

  useEffect(() => {
    if (editor && content !== editor.getHTML()) {
      editor.commands.setContent(content);
    }
  }, [content, editor]);

  return (
    <div className="rich-text-editor-wrapper">
      <MenuBar editor={editor} />
      <EditorContent editor={editor} className="editor-content" />
    </div>
  );
};

export default RichTextEditor;
