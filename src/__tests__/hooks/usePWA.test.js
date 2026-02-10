import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { renderHook, act } from '@testing-library/react';
import { usePWA } from '../../hooks/usePWA';

describe('usePWA', () => {
  let originalMatchMedia;
  let originalServiceWorker;

  beforeEach(() => {
    // Save originals
    originalMatchMedia = window.matchMedia;
    originalServiceWorker = navigator.serviceWorker;

    // Mock matchMedia
    window.matchMedia = vi.fn().mockImplementation(query => ({
      matches: query === '(display-mode: standalone)' ? false : true,
      media: query,
      addEventListener: vi.fn(),
      removeEventListener: vi.fn(),
    }));

    // Mock serviceWorker
    Object.defineProperty(navigator, 'serviceWorker', {
      value: {
        register: vi.fn().mockResolvedValue({ scope: '/' }),
      },
      configurable: true,
    });
  });

  afterEach(() => {
    window.matchMedia = originalMatchMedia;
    Object.defineProperty(navigator, 'serviceWorker', {
      value: originalServiceWorker,
      configurable: true,
    });
  });

  it('should return correct initial state', () => {
    const { result } = renderHook(() => usePWA());

    expect(result.current.isInstalled).toBe(false);
    expect(result.current.isInstallable).toBe(false);
    expect(result.current.isOnline).toBe(true);
  });

  it('should detect if already installed', () => {
    window.matchMedia = vi.fn().mockImplementation(query => ({
      matches: query === '(display-mode: standalone)' ? true : false,
      media: query,
      addEventListener: vi.fn(),
      removeEventListener: vi.fn(),
    }));

    const { result } = renderHook(() => usePWA());
    expect(result.current.isInstalled).toBe(true);
  });

  it('should handle online/offline status', async () => {
    const { result } = renderHook(() => usePWA());

    expect(result.current.isOnline).toBe(true);

    // Simulate going offline
    act(() => {
      window.dispatchEvent(new Event('offline'));
    });

    expect(result.current.isOnline).toBe(false);

    // Simulate coming online
    act(() => {
      window.dispatchEvent(new Event('online'));
    });

    expect(result.current.isOnline).toBe(true);
  });
});















