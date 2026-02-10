import React, { useState } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { FiDownload, FiX, FiSmartphone } from 'react-icons/fi';
import { usePWA } from '../../hooks/usePWA';
import './InstallPrompt.css';

const InstallPrompt = () => {
  const { isInstallable, isInstalled, installApp } = usePWA();
  const [dismissed, setDismissed] = useState(false);

  // Check if dismissed in this session
  if (dismissed || isInstalled || !isInstallable) {
    return null;
  }

  const handleInstall = async () => {
    const success = await installApp();
    if (!success) {
      setDismissed(true);
    }
  };

  return (
    <AnimatePresence>
      <motion.div
        className="install-prompt"
        initial={{ y: 100, opacity: 0 }}
        animate={{ y: 0, opacity: 1 }}
        exit={{ y: 100, opacity: 0 }}
        transition={{ type: 'spring', stiffness: 300, damping: 30 }}
      >
        <div className="install-content">
          <div className="install-icon">
            <FiSmartphone />
          </div>
          <div className="install-text">
            <h4>Install GradLink</h4>
            <p>Add to your home screen for quick access</p>
          </div>
        </div>
        <div className="install-actions">
          <button className="install-btn" onClick={handleInstall}>
            <FiDownload /> Install
          </button>
          <button className="dismiss-btn" onClick={() => setDismissed(true)}>
            <FiX />
          </button>
        </div>
      </motion.div>
    </AnimatePresence>
  );
};

export default InstallPrompt;















