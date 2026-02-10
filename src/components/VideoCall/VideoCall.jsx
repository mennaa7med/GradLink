import React, { useState, useEffect, useRef } from 'react';
import { FiVideo, FiVideoOff, FiMic, FiMicOff, FiMonitor, FiPhoneOff, FiUsers, FiCopy, FiCheck } from 'react-icons/fi';
import { motion } from 'framer-motion';
import './VideoCall.css';

// Using Jitsi Meet API (free, open-source)
const JITSI_DOMAIN = 'meet.jit.si';

const VideoCall = ({ roomName, displayName, onClose, isHost = false }) => {
  const [isVideoOn, setIsVideoOn] = useState(true);
  const [isMicOn, setIsMicOn] = useState(true);
  const [isScreenSharing, setIsScreenSharing] = useState(false);
  const [participantCount, setParticipantCount] = useState(1);
  const [copied, setCopied] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const containerRef = useRef(null);
  const jitsiApiRef = useRef(null);

  useEffect(() => {
    // Load Jitsi API script
    const script = document.createElement('script');
    script.src = 'https://meet.jit.si/external_api.js';
    script.async = true;
    script.onload = initJitsi;
    document.body.appendChild(script);

    return () => {
      if (jitsiApiRef.current) {
        jitsiApiRef.current.dispose();
      }
      document.body.removeChild(script);
    };
  }, []);

  const initJitsi = () => {
    if (!containerRef.current) return;

    const domain = JITSI_DOMAIN;
    const options = {
      roomName: `GradLink_${roomName}`,
      width: '100%',
      height: '100%',
      parentNode: containerRef.current,
      userInfo: {
        displayName: displayName || 'Guest'
      },
      configOverwrite: {
        startWithAudioMuted: !isMicOn,
        startWithVideoMuted: !isVideoOn,
        prejoinPageEnabled: false,
        disableDeepLinking: true
      },
      interfaceConfigOverwrite: {
        TOOLBAR_BUTTONS: [
          'microphone', 'camera', 'desktop', 'chat', 
          'raisehand', 'participants-pane', 'tileview'
        ],
        SHOW_JITSI_WATERMARK: false,
        SHOW_WATERMARK_FOR_GUESTS: false,
        DEFAULT_BACKGROUND: '#0c2737',
        DISABLE_JOIN_LEAVE_NOTIFICATIONS: false,
        MOBILE_APP_PROMO: false
      }
    };

    const api = new window.JitsiMeetExternalAPI(domain, options);
    jitsiApiRef.current = api;

    api.addListener('readyToClose', () => {
      if (onClose) onClose();
    });

    api.addListener('participantJoined', () => {
      setParticipantCount(prev => prev + 1);
    });

    api.addListener('participantLeft', () => {
      setParticipantCount(prev => Math.max(1, prev - 1));
    });

    api.addListener('videoConferenceJoined', () => {
      setIsLoading(false);
    });

    api.addListener('audioMuteStatusChanged', ({ muted }) => {
      setIsMicOn(!muted);
    });

    api.addListener('videoMuteStatusChanged', ({ muted }) => {
      setIsVideoOn(!muted);
    });

    api.addListener('screenSharingStatusChanged', ({ on }) => {
      setIsScreenSharing(on);
    });
  };

  const toggleVideo = () => {
    if (jitsiApiRef.current) {
      jitsiApiRef.current.executeCommand('toggleVideo');
    }
  };

  const toggleMic = () => {
    if (jitsiApiRef.current) {
      jitsiApiRef.current.executeCommand('toggleAudio');
    }
  };

  const toggleScreenShare = () => {
    if (jitsiApiRef.current) {
      jitsiApiRef.current.executeCommand('toggleShareScreen');
    }
  };

  const hangUp = () => {
    if (jitsiApiRef.current) {
      jitsiApiRef.current.executeCommand('hangup');
    }
    if (onClose) onClose();
  };

  const copyMeetingLink = () => {
    const link = `https://${JITSI_DOMAIN}/GradLink_${roomName}`;
    navigator.clipboard.writeText(link);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  return (
    <div className="video-call-container">
      {isLoading && (
        <div className="video-loading">
          <div className="spinner"></div>
          <p>Connecting to meeting...</p>
        </div>
      )}
      
      <div className="video-main" ref={containerRef}></div>

      <div className="video-controls">
        <div className="controls-left">
          <span className="participant-count">
            <FiUsers /> {participantCount}
          </span>
        </div>

        <div className="controls-center">
          <motion.button
            className={`control-btn ${!isMicOn ? 'off' : ''}`}
            onClick={toggleMic}
            whileHover={{ scale: 1.1 }}
            whileTap={{ scale: 0.9 }}
            title={isMicOn ? 'Mute' : 'Unmute'}
          >
            {isMicOn ? <FiMic /> : <FiMicOff />}
          </motion.button>

          <motion.button
            className={`control-btn ${!isVideoOn ? 'off' : ''}`}
            onClick={toggleVideo}
            whileHover={{ scale: 1.1 }}
            whileTap={{ scale: 0.9 }}
            title={isVideoOn ? 'Turn off camera' : 'Turn on camera'}
          >
            {isVideoOn ? <FiVideo /> : <FiVideoOff />}
          </motion.button>

          <motion.button
            className={`control-btn ${isScreenSharing ? 'active' : ''}`}
            onClick={toggleScreenShare}
            whileHover={{ scale: 1.1 }}
            whileTap={{ scale: 0.9 }}
            title="Share screen"
          >
            <FiMonitor />
          </motion.button>

          <motion.button
            className="control-btn end-call"
            onClick={hangUp}
            whileHover={{ scale: 1.1 }}
            whileTap={{ scale: 0.9 }}
            title="End call"
          >
            <FiPhoneOff />
          </motion.button>
        </div>

        <div className="controls-right">
          <motion.button
            className="copy-link-btn"
            onClick={copyMeetingLink}
            whileHover={{ scale: 1.05 }}
            whileTap={{ scale: 0.95 }}
          >
            {copied ? <FiCheck /> : <FiCopy />}
            {copied ? 'Copied!' : 'Copy Link'}
          </motion.button>
        </div>
      </div>
    </div>
  );
};

// Utility function to generate a meeting room
export const generateMeetingRoom = () => {
  return `meeting_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`;
};

// Quick meeting links generator
export const getMeetingLinks = (roomName) => {
  const fullRoomName = `GradLink_${roomName}`;
  return {
    jitsi: `https://meet.jit.si/${fullRoomName}`,
    googleMeet: `https://meet.google.com/new`,
    zoom: `https://zoom.us/start/videomeeting`,
    teams: `https://teams.microsoft.com/l/meeting/new`
  };
};

export default VideoCall;















