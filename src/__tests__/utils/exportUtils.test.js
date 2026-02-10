import { describe, it, expect, vi, beforeEach } from 'vitest';
import { exportToCSV, exportToPDF } from '../../utils/exportUtils';

describe('exportUtils', () => {
  beforeEach(() => {
    // Mock URL.createObjectURL
    global.URL.createObjectURL = vi.fn(() => 'mock-url');
    global.URL.revokeObjectURL = vi.fn();
  });

  describe('exportToCSV', () => {
    it('should not export when data is empty', () => {
      const consoleWarn = vi.spyOn(console, 'warn').mockImplementation(() => {});
      exportToCSV([], 'test');
      expect(consoleWarn).toHaveBeenCalledWith('No data to export');
      consoleWarn.mockRestore();
    });

    it('should create CSV with headers from data keys', () => {
      const mockClick = vi.fn();
      const mockAppendChild = vi.fn();
      const mockRemoveChild = vi.fn();
      
      document.createElement = vi.fn(() => ({
        click: mockClick,
        href: '',
        download: ''
      }));
      document.body.appendChild = mockAppendChild;
      document.body.removeChild = mockRemoveChild;

      const data = [
        { name: 'Task 1', status: 'Completed' },
        { name: 'Task 2', status: 'Pending' }
      ];

      exportToCSV(data, 'tasks');

      expect(global.URL.createObjectURL).toHaveBeenCalled();
      expect(mockClick).toHaveBeenCalled();
    });

    it('should handle custom columns', () => {
      const mockClick = vi.fn();
      document.createElement = vi.fn(() => ({
        click: mockClick,
        href: '',
        download: ''
      }));
      document.body.appendChild = vi.fn();
      document.body.removeChild = vi.fn();

      const data = [{ name: 'Test', value: 123 }];
      const columns = [{ key: 'name', header: 'Task Name' }];

      exportToCSV(data, 'test', columns);

      expect(mockClick).toHaveBeenCalled();
    });

    it('should handle null values', () => {
      const mockClick = vi.fn();
      document.createElement = vi.fn(() => ({
        click: mockClick,
        href: '',
        download: ''
      }));
      document.body.appendChild = vi.fn();
      document.body.removeChild = vi.fn();

      const data = [{ name: null, value: undefined }];
      exportToCSV(data, 'test');

      expect(mockClick).toHaveBeenCalled();
    });
  });

  describe('exportToPDF', () => {
    it('should not export when data is empty', () => {
      const consoleWarn = vi.spyOn(console, 'warn').mockImplementation(() => {});
      exportToPDF('Test', []);
      expect(consoleWarn).toHaveBeenCalledWith('No data to export');
      consoleWarn.mockRestore();
    });

    it('should open print window', () => {
      const mockWrite = vi.fn();
      const mockClose = vi.fn();
      const mockFocus = vi.fn();
      const mockPrint = vi.fn();

      window.open = vi.fn(() => ({
        document: {
          write: mockWrite,
          close: mockClose
        },
        focus: mockFocus,
        print: mockPrint
      }));

      const data = [{ name: 'Test', value: 123 }];
      exportToPDF('Test Report', data);

      expect(window.open).toHaveBeenCalledWith('', '_blank');
      expect(mockWrite).toHaveBeenCalled();
      expect(mockClose).toHaveBeenCalled();
      expect(mockFocus).toHaveBeenCalled();
    });
  });
});















