import { useState, useEffect, useCallback, useRef } from 'react';
import * as signalR from '@microsoft/signalr';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5080';

/**
 * Custom hook for SignalR connection
 * @param {string} hubPath - The hub endpoint path (e.g., '/hubs/chat')
 * @returns {Object} SignalR connection state and methods
 */
export const useSignalR = (hubPath = '/hubs/chat') => {
  const [connection, setConnection] = useState(null);
  const [connectionState, setConnectionState] = useState('disconnected');
  const [error, setError] = useState(null);
  const reconnectTimeoutRef = useRef(null);

  // Initialize connection
  useEffect(() => {
    const token = localStorage.getItem('accessToken');
    if (!token) {
      setConnectionState('disconnected');
      return;
    }

    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${API_BASE_URL}${hubPath}`, {
        accessTokenFactory: () => token,
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
      .configureLogging(signalR.LogLevel.Information)
      .build();

    newConnection.onreconnecting((error) => {
      console.log('SignalR reconnecting...', error);
      setConnectionState('reconnecting');
    });

    newConnection.onreconnected((connectionId) => {
      console.log('SignalR reconnected:', connectionId);
      setConnectionState('connected');
      setError(null);
    });

    newConnection.onclose((error) => {
      console.log('SignalR closed:', error);
      setConnectionState('disconnected');
      if (error) {
        setError(error.message);
      }
    });

    setConnection(newConnection);

    return () => {
      if (reconnectTimeoutRef.current) {
        clearTimeout(reconnectTimeoutRef.current);
      }
      newConnection.stop();
    };
  }, [hubPath]);

  // Start connection
  const startConnection = useCallback(async () => {
    if (!connection) return;
    
    if (connection.state === signalR.HubConnectionState.Connected) {
      setConnectionState('connected');
      return;
    }

    try {
      setConnectionState('connecting');
      await connection.start();
      setConnectionState('connected');
      setError(null);
      console.log('SignalR Connected');
    } catch (err) {
      console.error('SignalR Connection Error:', err);
      setError(err.message);
      setConnectionState('disconnected');
      
      // Retry after delay
      reconnectTimeoutRef.current = setTimeout(() => {
        startConnection();
      }, 5000);
    }
  }, [connection]);

  // Stop connection
  const stopConnection = useCallback(async () => {
    if (connection) {
      await connection.stop();
      setConnectionState('disconnected');
    }
  }, [connection]);

  // Invoke hub method
  const invoke = useCallback(async (method, ...args) => {
    if (!connection || connection.state !== signalR.HubConnectionState.Connected) {
      throw new Error('Not connected to SignalR hub');
    }
    return connection.invoke(method, ...args);
  }, [connection]);

  // Register event handler
  const on = useCallback((event, handler) => {
    if (!connection) return () => {};
    
    connection.on(event, handler);
    return () => connection.off(event, handler);
  }, [connection]);

  // Unregister event handler
  const off = useCallback((event, handler) => {
    if (connection) {
      connection.off(event, handler);
    }
  }, [connection]);

  return {
    connection,
    connectionState,
    error,
    startConnection,
    stopConnection,
    invoke,
    on,
    off,
    isConnected: connectionState === 'connected'
  };
};

/**
 * Custom hook specifically for chat functionality
 */
export const useChatHub = () => {
  const {
    connection,
    connectionState,
    error,
    startConnection,
    stopConnection,
    invoke,
    on,
    off,
    isConnected
  } = useSignalR('/hubs/chat');

  const [messages, setMessages] = useState([]);

  // Auto-start connection
  useEffect(() => {
    if (connection && connectionState === 'disconnected') {
      startConnection();
    }
  }, [connection, connectionState, startConnection]);

  // Handle incoming messages
  useEffect(() => {
    if (!connection) return;

    const handleReceiveMessage = (message) => {
      console.log('Received message:', message);
      setMessages(prev => [...prev, { ...message, isOwn: false }]);
    };

    const handleMessageSent = (message) => {
      console.log('Message sent:', message);
      setMessages(prev => [...prev, { ...message, isOwn: true }]);
    };

    const unsubReceive = on('ReceiveMessage', handleReceiveMessage);
    const unsubSent = on('MessageSent', handleMessageSent);

    return () => {
      unsubReceive();
      unsubSent();
    };
  }, [connection, on]);

  // Send message
  const sendMessage = useCallback(async (recipientId, content) => {
    if (!isConnected) {
      throw new Error('Not connected to chat');
    }
    await invoke('SendMessage', recipientId, content);
  }, [isConnected, invoke]);

  // Mark messages as read
  const markAsRead = useCallback(async (conversationId) => {
    if (!isConnected) return;
    await invoke('MarkAsRead', conversationId);
  }, [isConnected, invoke]);

  // Clear messages (e.g., when switching conversations)
  const clearMessages = useCallback(() => {
    setMessages([]);
  }, []);

  return {
    connectionState,
    error,
    isConnected,
    messages,
    sendMessage,
    markAsRead,
    clearMessages,
    startConnection,
    stopConnection
  };
};

export default useSignalR;















