import { describe, it, expect, vi } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import LanguageToggle from '../../components/LanguageToggle/LanguageToggle';
import { LanguageProvider } from '../../contexts/LanguageContext';

// Wrapper component with provider
const renderWithProvider = (component) => {
  return render(
    <LanguageProvider>
      {component}
    </LanguageProvider>
  );
};

describe('LanguageToggle', () => {
  it('should render language toggle button', () => {
    renderWithProvider(<LanguageToggle />);
    const button = screen.getByRole('button');
    expect(button).toBeInTheDocument();
  });

  it('should display Arabic option when in English', () => {
    renderWithProvider(<LanguageToggle />);
    expect(screen.getByText('العربية')).toBeInTheDocument();
  });

  it('should render compact version', () => {
    renderWithProvider(<LanguageToggle compact />);
    const button = screen.getByRole('button');
    expect(button).toHaveClass('compact');
  });

  it('should toggle language on click', () => {
    renderWithProvider(<LanguageToggle />);
    const button = screen.getByRole('button');
    
    // Initial state should show Arabic option (meaning we're in English)
    expect(screen.getByText('العربية')).toBeInTheDocument();
    
    // Click to toggle
    fireEvent.click(button);
    
    // After toggle, should show English option (meaning we're now in Arabic)
    expect(screen.getByText('English')).toBeInTheDocument();
  });
});















