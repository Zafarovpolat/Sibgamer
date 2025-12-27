import { useEffect, useRef } from 'react';

interface ContentRendererProps {
    content: string;
    className?: string;
}

const ContentRenderer = ({ content, className = '' }: ContentRendererProps) => {
    const containerRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        if (!containerRef.current) return;

        // Цвета из темы сайта
        const colors = {
            highlight: '#ff6b35',
            highlightHover: '#ff8555',
            glass: 'rgba(17, 24, 39, 0.8)',
            border: 'rgba(75, 85, 99, 0.5)',
            borderHover: 'rgba(107, 114, 128, 0.6)',
            text: '#d1d5db',
            textHover: '#ffffff',
            bg: 'rgba(31, 41, 55, 0.6)',
            bgHover: 'rgba(55, 65, 81, 0.6)',
        };

        // Обработка Accordion блоков
        const accordions = containerRef.current.querySelectorAll('[data-type="accordion"]');

        accordions.forEach((accordion) => {
            const element = accordion as HTMLElement;

            if (element.dataset.processed === 'true') return;
            element.dataset.processed = 'true';

            const collapsedHeight = parseInt(element.getAttribute('data-collapsed-height') || '150');
            const title = element.getAttribute('data-title') || 'Раскрыть';

            const originalContent = element.innerHTML;

            element.innerHTML = '';
            element.style.cssText = `
        background: ${colors.glass};
        backdrop-filter: blur(12px);
        border: 1px solid ${colors.border};
        border-radius: 12px;
        overflow: hidden;
        margin: 20px 0;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
      `;

            // Контейнер для контента
            const contentWrapper = document.createElement('div');
            contentWrapper.className = 'accordion-content-wrapper';
            contentWrapper.innerHTML = originalContent;
            contentWrapper.style.cssText = `
        padding: 20px;
        max-height: ${collapsedHeight}px;
        overflow: hidden;
        position: relative;
        transition: max-height 0.4s ease-in-out;
      `;

            // Градиент
            const gradient = document.createElement('div');
            gradient.className = 'accordion-gradient';
            gradient.style.cssText = `
        position: absolute;
        bottom: 0;
        left: 0;
        right: 0;
        height: 80px;
        background: linear-gradient(to bottom, transparent, rgba(17, 24, 39, 0.95));
        pointer-events: none;
        transition: opacity 0.3s ease;
      `;
            contentWrapper.appendChild(gradient);

            // Кнопка
            const button = document.createElement('button');
            button.className = 'accordion-toggle-btn';
            button.style.cssText = `
        width: 100%;
        padding: 14px 20px;
        background: ${colors.bg};
        border-top: 1px solid ${colors.border};
        color: ${colors.text};
        font-size: 14px;
        font-weight: 600;
        cursor: pointer;
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 10px;
        transition: all 0.3s ease;
        border: none;
        outline: none;
        font-family: inherit;
      `;

            const updateButton = (isExpanded: boolean) => {
                button.innerHTML = `
          <span>${isExpanded ? 'Свернуть' : title}</span>
          <svg width="14" height="14" viewBox="0 0 14 14" fill="none" style="transform: rotate(${isExpanded ? '180deg' : '0deg'}); transition: transform 0.3s ease;">
            <path d="M2 5L7 10L12 5" stroke="${colors.highlight}" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"/>
          </svg>
        `;
            };

            let isExpanded = false;
            updateButton(false);

            element.appendChild(contentWrapper);

            // Даём время на рендер контента
            requestAnimationFrame(() => {
                const needsExpand = contentWrapper.scrollHeight > collapsedHeight;

                if (needsExpand) {
                    element.appendChild(button);

                    button.addEventListener('click', () => {
                        isExpanded = !isExpanded;

                        if (isExpanded) {
                            contentWrapper.style.maxHeight = `${contentWrapper.scrollHeight + 100}px`;
                            gradient.style.opacity = '0';
                        } else {
                            contentWrapper.style.maxHeight = `${collapsedHeight}px`;
                            gradient.style.opacity = '1';
                        }

                        updateButton(isExpanded);
                    });

                    button.addEventListener('mouseenter', () => {
                        button.style.background = colors.bgHover;
                        button.style.color = colors.textHover;
                    });

                    button.addEventListener('mouseleave', () => {
                        button.style.background = colors.bg;
                        button.style.color = colors.text;
                    });
                } else {
                    contentWrapper.style.maxHeight = 'none';
                    gradient.style.display = 'none';
                }
            });
        });

        // Обработка Spoiler блоков
        const spoilers = containerRef.current.querySelectorAll('[data-type="spoiler"]');

        spoilers.forEach((spoiler) => {
            const element = spoiler as HTMLElement;

            if (element.dataset.processed === 'true') return;
            element.dataset.processed = 'true';

            const label = element.getAttribute('data-label') || 'Спойлер';
            const originalContent = element.innerHTML;

            element.innerHTML = '';
            element.style.cssText = `
        background: ${colors.glass};
        backdrop-filter: blur(12px);
        border: 2px solid ${colors.highlight}40;
        border-radius: 12px;
        overflow: hidden;
        margin: 20px 0;
        box-shadow: 0 4px 20px rgba(255, 107, 53, 0.1);
      `;

            // Заголовок
            const header = document.createElement('div');
            header.style.cssText = `
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 14px 20px;
        background: ${colors.highlight}15;
        cursor: pointer;
        transition: background 0.3s ease;
      `;

            const labelSpan = document.createElement('span');
            labelSpan.innerHTML = `
        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" style="display: inline-block; margin-right: 10px; vertical-align: middle;">
          <path d="M1 12C1 12 5 4 12 4C19 4 23 12 23 12C23 12 19 20 12 20C5 20 1 12 1 12Z" stroke="${colors.highlight}" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
          <circle cx="12" cy="12" r="3" stroke="${colors.highlight}" stroke-width="2"/>
          <path d="M3 3L21 21" stroke="${colors.highlight}" stroke-width="2" stroke-linecap="round"/>
        </svg>
        <span style="color: ${colors.highlight}; font-size: 14px; font-weight: 600;">${label}</span>
      `;

            const toggleBtn = document.createElement('button');
            toggleBtn.style.cssText = `
        padding: 8px 20px;
        background: ${colors.highlight};
        color: white;
        border: none;
        border-radius: 8px;
        font-size: 13px;
        font-weight: 600;
        cursor: pointer;
        transition: all 0.3s ease;
        font-family: inherit;
        box-shadow: 0 2px 10px ${colors.highlight}40;
      `;
            toggleBtn.textContent = 'Показать';

            header.appendChild(labelSpan);
            header.appendChild(toggleBtn);

            // Контент
            const contentDiv = document.createElement('div');
            contentDiv.innerHTML = originalContent;
            contentDiv.style.cssText = `
        max-height: 0;
        overflow: hidden;
        transition: max-height 0.4s ease, padding 0.4s ease;
        padding: 0 20px;
      `;

            let isRevealed = false;

            const toggle = () => {
                isRevealed = !isRevealed;
                if (isRevealed) {
                    contentDiv.style.maxHeight = `${contentDiv.scrollHeight + 40}px`;
                    contentDiv.style.padding = '20px';
                    toggleBtn.textContent = 'Скрыть';
                } else {
                    contentDiv.style.maxHeight = '0';
                    contentDiv.style.padding = '0 20px';
                    toggleBtn.textContent = 'Показать';
                }
            };

            header.addEventListener('click', toggle);

            header.addEventListener('mouseenter', () => {
                header.style.background = `${colors.highlight}25`;
            });
            header.addEventListener('mouseleave', () => {
                header.style.background = `${colors.highlight}15`;
            });

            toggleBtn.addEventListener('mouseenter', () => {
                toggleBtn.style.background = colors.highlightHover;
                toggleBtn.style.transform = 'translateY(-2px)';
                toggleBtn.style.boxShadow = `0 4px 15px ${colors.highlight}50`;
            });
            toggleBtn.addEventListener('mouseleave', () => {
                toggleBtn.style.background = colors.highlight;
                toggleBtn.style.transform = 'translateY(0)';
                toggleBtn.style.boxShadow = `0 2px 10px ${colors.highlight}40`;
            });

            element.appendChild(header);
            element.appendChild(contentDiv);
        });

        // Обработка Callout блоков
        const callouts = containerRef.current.querySelectorAll('[data-type="callout"]');

        callouts.forEach((callout) => {
            const element = callout as HTMLElement;

            if (element.dataset.processed === 'true') return;
            element.dataset.processed = 'true';

            const type = element.getAttribute('data-callout-type') || 'info';

            const styles: Record<string, { bg: string; border: string; icon: string; iconBg: string }> = {
                info: {
                    bg: 'rgba(59, 130, 246, 0.1)',
                    border: '#3b82f6',
                    icon: `<svg width="20" height="20" viewBox="0 0 24 24" fill="none"><circle cx="12" cy="12" r="10" stroke="#3b82f6" stroke-width="2"/><path d="M12 16V12M12 8H12.01" stroke="#3b82f6" stroke-width="2" stroke-linecap="round"/></svg>`,
                    iconBg: 'rgba(59, 130, 246, 0.2)'
                },
                warning: {
                    bg: 'rgba(245, 158, 11, 0.1)',
                    border: '#f59e0b',
                    icon: `<svg width="20" height="20" viewBox="0 0 24 24" fill="none"><path d="M12 9V13M12 17H12.01M10.29 3.86L1.82 18C1.64 18.3 1.55 18.64 1.55 19C1.55 19.36 1.64 19.7 1.82 20C2 20.3 2.26 20.56 2.58 20.73C2.9 20.9 3.26 21 3.64 21H20.36C20.74 21 21.1 20.9 21.42 20.73C21.74 20.56 22 20.3 22.18 20C22.36 19.7 22.45 19.36 22.45 19C22.45 18.64 22.36 18.3 22.18 18L13.71 3.86C13.53 3.56 13.27 3.32 12.95 3.15C12.63 2.98 12.27 2.89 11.91 2.89C11.55 2.89 11.19 2.98 10.87 3.15C10.55 3.32 10.29 3.56 10.11 3.86H10.29Z" stroke="#f59e0b" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/></svg>`,
                    iconBg: 'rgba(245, 158, 11, 0.2)'
                },
                success: {
                    bg: 'rgba(34, 197, 94, 0.1)',
                    border: '#22c55e',
                    icon: `<svg width="20" height="20" viewBox="0 0 24 24" fill="none"><path d="M22 11.08V12C21.9988 14.1564 21.3005 16.2547 20.0093 17.9818C18.7182 19.709 16.9033 20.9725 14.8354 21.5839C12.7674 22.1953 10.5573 22.1219 8.53447 21.3746C6.51168 20.6273 4.78465 19.2461 3.61096 17.4371C2.43727 15.628 1.87979 13.4881 2.02168 11.3363C2.16356 9.18455 2.99721 7.13631 4.39828 5.49706C5.79935 3.85781 7.69279 2.71537 9.79619 2.24013C11.8996 1.7649 14.1003 1.98232 16.07 2.85999" stroke="#22c55e" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/><path d="M22 4L12 14.01L9 11.01" stroke="#22c55e" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/></svg>`,
                    iconBg: 'rgba(34, 197, 94, 0.2)'
                },
                error: {
                    bg: 'rgba(239, 68, 68, 0.1)',
                    border: '#ef4444',
                    icon: `<svg width="20" height="20" viewBox="0 0 24 24" fill="none"><circle cx="12" cy="12" r="10" stroke="#ef4444" stroke-width="2"/><path d="M15 9L9 15M9 9L15 15" stroke="#ef4444" stroke-width="2" stroke-linecap="round"/></svg>`,
                    iconBg: 'rgba(239, 68, 68, 0.2)'
                },
            };

            const style = styles[type] || styles.info;

            const originalContent = element.innerHTML;
            element.innerHTML = `
        <div style="display: flex; align-items: flex-start; gap: 16px;">
          <div style="flex-shrink: 0; width: 36px; height: 36px; border-radius: 10px; background: ${style.iconBg}; display: flex; align-items: center; justify-content: center;">
            ${style.icon}
          </div>
          <div style="flex: 1; padding-top: 6px;">${originalContent}</div>
        </div>
      `;
            element.style.cssText = `
        background: ${style.bg};
        backdrop-filter: blur(8px);
        border-left: 4px solid ${style.border};
        border-radius: 0 12px 12px 0;
        padding: 20px;
        margin: 20px 0;
        box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
      `;
        });

    }, [content]);

    return (
        <div
            ref={containerRef}
            className={`prose prose-invert max-w-none ${className}`}
            dangerouslySetInnerHTML={{ __html: content }}
        />
    );
};

export default ContentRenderer;