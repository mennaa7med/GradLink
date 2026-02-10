import React, { useEffect, useRef, useState } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { FiSend, FiPaperclip, FiSmile, FiMoreVertical, FiSearch, FiPhone, FiVideo, FiArrowLeft } from 'react-icons/fi';
import { createChatConnection } from '../api/signalr';
import { useAuth } from '../contexts/AuthContext';
import api from '../api/client';
import './Chat.css';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000';

export default function Chat() {
  const { user } = useAuth();
  const [conversations, setConversations] = useState([]);
  const [selectedConversation, setSelectedConversation] = useState(null);
  const [connected, setConnected] = useState(false);
  const [message, setMessage] = useState('');
  const [messages, setMessages] = useState([]);
  const [searchQuery, setSearchQuery] = useState('');
  const [loading, setLoading] = useState(true);
  const [isTyping, setIsTyping] = useState(false);
  const connRef = useRef(null);
  const messagesEndRef = useRef(null);
  const inputRef = useRef(null);

  // Initialize SignalR connection
  useEffect(() => {
    const connection = createChatConnection(API_BASE_URL);
    connRef.current = connection;

    connection.on('ReceiveMessage', (userId, text, sentAt, senderName, senderAvatar) => {
      setMessages(prev => [...prev, {
        id: Date.now(),
        senderId: userId,
        text,
        sentAt,
        senderName,
        senderAvatar,
        isOwn: userId === user?.id
      }]);
    });

    connection.on('UserTyping', (userId) => {
      if (userId !== user?.id) {
        setIsTyping(true);
        setTimeout(() => setIsTyping(false), 3000);
      }
    });

    connection.start()
      .then(() => {
        setConnected(true);
        loadConversations();
      })
      .catch(() => setConnected(false));

    return () => {
      connection.stop();
    };
  }, []);

  // Scroll to bottom when new messages arrive
  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages]);

  const loadConversations = async () => {
    try {
      const { data } = await api.get('/api/conversations');
      setConversations(data || []);
    } catch (error) {
      console.error('Failed to load conversations:', error);
      // Mock data for demo
      setConversations([
        {
          id: '1',
          participantName: 'Ahmed Mohamed',
          participantAvatar: null,
          lastMessage: 'Hey, how\'s the project going?',
          lastMessageAt: new Date().toISOString(),
          unreadCount: 2
        },
        {
          id: '2',
          participantName: 'Sara Ali',
          participantAvatar: null,
          lastMessage: 'Meeting at 3pm?',
          lastMessageAt: new Date(Date.now() - 3600000).toISOString(),
          unreadCount: 0
        },
        {
          id: '3',
          participantName: 'Team Project',
          participantAvatar: null,
          lastMessage: 'Great work everyone!',
          lastMessageAt: new Date(Date.now() - 86400000).toISOString(),
          unreadCount: 5
        }
      ]);
    } finally {
      setLoading(false);
    }
  };

  const loadMessages = async (conversationId) => {
    try {
      const { data } = await api.get(`/api/conversations/${conversationId}/messages`);
      setMessages(data || []);
    } catch (error) {
      console.error('Failed to load messages:', error);
      // Mock messages for demo
      setMessages([
        { id: 1, senderId: 'other', text: 'Hey! How are you?', sentAt: new Date(Date.now() - 3600000).toISOString(), isOwn: false },
        { id: 2, senderId: user?.id, text: 'I\'m good! Working on the project.', sentAt: new Date(Date.now() - 3000000).toISOString(), isOwn: true },
        { id: 3, senderId: 'other', text: 'That\'s great! Need any help?', sentAt: new Date(Date.now() - 1800000).toISOString(), isOwn: false },
      ]);
    }
  };

  const selectConversation = async (conversation) => {
    setSelectedConversation(conversation);
    await loadMessages(conversation.id);
    
    if (connRef.current && connected) {
      try {
        await connRef.current.invoke('Join', conversation.id);
      } catch (error) {
        console.error('Failed to join conversation:', error);
      }
    }
  };

  const sendMessage = async () => {
    if (!message.trim() || !selectedConversation) return;

    const newMessage = {
      id: Date.now(),
      senderId: user?.id,
      text: message,
      sentAt: new Date().toISOString(),
      isOwn: true
    };

    // Optimistically add message
    setMessages(prev => [...prev, newMessage]);
    setMessage('');
    inputRef.current?.focus();

    if (connRef.current && connected) {
      try {
        await connRef.current.invoke('SendMessage', selectedConversation.id, message);
      } catch (error) {
        console.error('Failed to send message:', error);
      }
    }
  };

  const handleTyping = () => {
    if (connRef.current && connected && selectedConversation) {
      connRef.current.invoke('SendTyping', selectedConversation.id).catch(() => {});
    }
  };

  const formatTime = (dateString) => {
    const date = new Date(dateString);
    const now = new Date();
    const diff = now - date;
    
    if (diff < 86400000) { // Less than 24 hours
      return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    } else if (diff < 604800000) { // Less than 7 days
      return date.toLocaleDateString([], { weekday: 'short' });
    } else {
      return date.toLocaleDateString([], { month: 'short', day: 'numeric' });
    }
  };

  const filteredConversations = conversations.filter(conv =>
    conv.participantName?.toLowerCase().includes(searchQuery.toLowerCase())
  );

  const getInitials = (name) => {
    return name?.split(' ').map(n => n[0]).join('').toUpperCase().slice(0, 2) || '?';
  };

  return (
    <div className="chat-container">
      {/* Conversations List */}
      <motion.div 
        className={`conversations-panel ${selectedConversation ? 'hidden-mobile' : ''}`}
        initial={{ opacity: 0, x: -20 }}
        animate={{ opacity: 1, x: 0 }}
      >
        <div className="conversations-header">
          <h2>Messages</h2>
          <span className={`connection-status ${connected ? 'online' : 'offline'}`}>
            {connected ? 'Connected' : 'Connecting...'}
          </span>
        </div>

        <div className="search-conversations">
          <FiSearch />
          <input
            type="text"
            placeholder="Search conversations..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
          />
        </div>

        <div className="conversations-list">
          {loading ? (
            <div className="loading-conversations">Loading...</div>
          ) : filteredConversations.length > 0 ? (
            filteredConversations.map(conv => (
              <motion.div
                key={conv.id}
                className={`conversation-item ${selectedConversation?.id === conv.id ? 'active' : ''}`}
                onClick={() => selectConversation(conv)}
                whileHover={{ backgroundColor: 'rgba(12, 39, 55, 0.05)' }}
              >
                <div className="conv-avatar">
                  {conv.participantAvatar ? (
                    <img src={conv.participantAvatar} alt={conv.participantName} />
                  ) : (
                    <span>{getInitials(conv.participantName)}</span>
                  )}
                  {conv.isOnline && <span className="online-indicator" />}
                </div>
                <div className="conv-info">
                  <div className="conv-header">
                    <span className="conv-name">{conv.participantName}</span>
                    <span className="conv-time">{formatTime(conv.lastMessageAt)}</span>
                  </div>
                  <div className="conv-preview">
                    <span className="conv-message">{conv.lastMessage}</span>
                    {conv.unreadCount > 0 && (
                      <span className="unread-badge">{conv.unreadCount}</span>
                    )}
                  </div>
                </div>
              </motion.div>
            ))
          ) : (
            <div className="no-conversations">
              <p>No conversations yet</p>
            </div>
          )}
        </div>
      </motion.div>

      {/* Chat Area */}
      <div className={`chat-area ${selectedConversation ? 'visible' : ''}`}>
        {selectedConversation ? (
          <>
            <div className="chat-header">
              <button className="back-btn mobile-only" onClick={() => setSelectedConversation(null)}>
                <FiArrowLeft />
              </button>
              <div className="chat-user-info">
                <div className="chat-avatar">
                  {selectedConversation.participantAvatar ? (
                    <img src={selectedConversation.participantAvatar} alt={selectedConversation.participantName} />
                  ) : (
                    <span>{getInitials(selectedConversation.participantName)}</span>
                  )}
                </div>
                <div className="chat-user-details">
                  <span className="chat-user-name">{selectedConversation.participantName}</span>
                  {isTyping && <span className="typing-indicator">typing...</span>}
                </div>
              </div>
              <div className="chat-actions">
                <button className="action-btn"><FiPhone /></button>
                <button className="action-btn"><FiVideo /></button>
                <button className="action-btn"><FiMoreVertical /></button>
              </div>
            </div>

            <div className="messages-area">
              <AnimatePresence>
                {messages.map((msg, index) => (
                  <motion.div
                    key={msg.id}
                    className={`message ${msg.isOwn ? 'own' : 'other'}`}
                    initial={{ opacity: 0, y: 20 }}
                    animate={{ opacity: 1, y: 0 }}
                    exit={{ opacity: 0 }}
                    transition={{ delay: index * 0.05 }}
                  >
                    <div className="message-content">
                      <p>{msg.text}</p>
                      <span className="message-time">{formatTime(msg.sentAt)}</span>
                    </div>
                  </motion.div>
                ))}
              </AnimatePresence>
              <div ref={messagesEndRef} />
            </div>

            <div className="message-input-area">
              <button className="input-action"><FiPaperclip /></button>
              <input
                ref={inputRef}
                type="text"
                placeholder="Type a message..."
                value={message}
                onChange={(e) => {
                  setMessage(e.target.value);
                  handleTyping();
                }}
                onKeyPress={(e) => e.key === 'Enter' && sendMessage()}
              />
              <button className="input-action"><FiSmile /></button>
              <button 
                className="send-btn" 
                onClick={sendMessage}
                disabled={!message.trim()}
              >
                <FiSend />
              </button>
            </div>
          </>
        ) : (
          <div className="no-chat-selected">
            <div className="no-chat-content">
              <div className="no-chat-icon">💬</div>
              <h3>Select a conversation</h3>
              <p>Choose a conversation from the list to start chatting</p>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
