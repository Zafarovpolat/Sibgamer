export interface NavSectionItem {
    id: number;
    name: string;
    icon?: string;
    order: number;
    isPublished: boolean;
    type: 'internallink' | 'externallink' | 'custompage';
    url?: string;
    customPageId?: number;
    customPageTitle?: string;
    customPageSlug?: string;
    openInNewTab: boolean;
}

export interface NavSection {
    id: number;
    name: string;
    icon?: string;
    order: number;
    isPublished: boolean;
    type: 'link' | 'dropdown';
    url?: string;
    isExternal: boolean;
    openInNewTab: boolean;
    itemsCount?: number;
    items: NavSectionItem[];
    createdAt?: string;
    updatedAt?: string;
}

export interface CustomPageOption {
    id: number;
    title: string;
    slug: string;
}