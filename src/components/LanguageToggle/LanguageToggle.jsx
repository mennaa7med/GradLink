import React from 'react';
import { useLanguage } from '../../contexts/LanguageContext';
import './LanguageToggle.css';

const LanguageToggle = ({ compact = false }) => {
  const { language, toggleLanguage, isRTL } = useLanguage();

  return (
    <button 
      className={`language-toggle ${compact ? 'compact' : ''}`}
      onClick={toggleLanguage}
      title={isRTL ? 'Switch to English' : 'التبديل إلى العربية'}
    >
      {compact ? (
        <span className="lang-abbr">{language.toUpperCase()}</span>
      ) : (
        <>
          <span className="lang-icon">{isRTL ? '🇺🇸' : '🇸🇦'}</span>
          <span className="lang-text">{isRTL ? 'English' : 'العربية'}</span>
        </>
      )}
    </button>
  );
};

export default LanguageToggle;















