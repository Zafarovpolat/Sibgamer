import { API_URL } from '../config/api';
import { getAuthToken } from './auth';
import type { NavSection, CustomPageOption } from '../types/nav';

// ==================== PUBLIC ====================

export async function getNavSections(): Promise<NavSection[]> {
    const res = await fetch(`${API_URL}/nav-sections`);
    if (!res.ok) throw new Error('Failed to fetch nav sections');
    return res.json();
}

// ==================== ADMIN ====================

export async function getAdminNavSections(): Promise<NavSection[]> {
    const token = getAuthToken();
    const res = await fetch(`${API_URL}/admin/nav-sections`, {
        headers: { Authorization: `Bearer ${token}` }
    });
    if (!res.ok) throw new Error('Failed to fetch nav sections');
    return res.json();
}

export async function createNavSection(data: Partial<NavSection>): Promise<{ id: number }> {
    const token = getAuthToken();
    const res = await fetch(`${API_URL}/admin/nav-sections`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${token}`
        },
        body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error('Failed to create nav section');
    return res.json();
}

export async function updateNavSection(id: number, data: Partial<NavSection>): Promise<void> {
    const token = getAuthToken();
    const res = await fetch(`${API_URL}/admin/nav-sections/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${token}`
        },
        body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error('Failed to update nav section');
}

export async function deleteNavSection(id: number): Promise<void> {
    const token = getAuthToken();
    const res = await fetch(`${API_URL}/admin/nav-sections/${id}`, {
        method: 'DELETE',
        headers: { Authorization: `Bearer ${token}` }
    });
    if (!res.ok) throw new Error('Failed to delete nav section');
}

export async function reorderNavSections(items: { id: number; order: number }[]): Promise<void> {
    const token = getAuthToken();
    const res = await fetch(`${API_URL}/admin/nav-sections/reorder`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${token}`
        },
        body: JSON.stringify(items)
    });
    if (!res.ok) throw new Error('Failed to reorder sections');
}

// ==================== ITEMS ====================

export async function addNavItem(sectionId: number, data: Partial<NavSection['items'][0]>): Promise<{ id: number }> {
    const token = getAuthToken();
    const res = await fetch(`${API_URL}/admin/nav-sections/${sectionId}/items`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${token}`
        },
        body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error('Failed to add nav item');
    return res.json();
}

export async function updateNavItem(itemId: number, data: Partial<NavSection['items'][0]>): Promise<void> {
    const token = getAuthToken();
    const res = await fetch(`${API_URL}/admin/nav-sections/items/${itemId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${token}`
        },
        body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error('Failed to update nav item');
}

export async function deleteNavItem(itemId: number): Promise<void> {
    const token = getAuthToken();
    const res = await fetch(`${API_URL}/admin/nav-sections/items/${itemId}`, {
        method: 'DELETE',
        headers: { Authorization: `Bearer ${token}` }
    });
    if (!res.ok) throw new Error('Failed to delete nav item');
}

export async function getCustomPagesForNav(): Promise<CustomPageOption[]> {
    const token = getAuthToken();
    const res = await fetch(`${API_URL}/admin/nav-sections/custom-pages`, {
        headers: { Authorization: `Bearer ${token}` }
    });
    if (!res.ok) throw new Error('Failed to fetch custom pages');
    return res.json();
}