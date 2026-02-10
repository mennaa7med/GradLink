import React, { useState, useEffect, useCallback, useRef } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { FiMessageSquare, FiSend, FiSearch, FiMoreVertical, FiUser } from 'react-icons/fi';
import { getConversations, getConversation, getUnreadMessages } from '../../api/messages';
import './MessagesPage.css';

const MessagesPage = () => {
  const [conversations, setConversations] = useState([]);
  const [selectedConversation, setSelectedConversation] = useState(null);
  const [messages, setMessages] = useState([]);
  const [loading, setLoading] = useState(true);
  const [loadingMessages, setLoadingMessages] = useState(false);
  const [newMessage, setNewMessage] = useState('');
  const [searchQuery, setSearchQuery] = useState('');
  const messagesEndRef = useRef(null);

  const loadConversations = useCallback(async () => {
    setLoading(true);
    try {
      const data = await getConversations();
      setConversations(data || []);
    } catch (error) {
      console.error('Failed to load conversations:', error);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    loadConversations();
  }, [loadConversations]);

  const loadMessages = async (conversationId) => {
    setLoadingMessages(true);
    try {
      const data = await getConversation(conversationId);
      setMessages(data?.messages || []);
      setSelectedConversation(data);
    } catch (error) {
      console.error('Failed to load messages:', error);
    } finally {
      setLoadingMessages(false);
    }
  };

  const handleSelectConversation = (conversation) => {
    loadMessages(conversation.id);
  };

  const handleSendMessage = async (e) => {
    e.preventDefault();
    if (!newMessage.trim() || !selectedConversation) return;

    // For now, just show a placeholder - would need to implement backend send
    const tempMessage = {
      id: Date.now(),
      content: newMessage,
      sentAt: new Date().toISOString(),
      isRead: true,
      senderId: 'current-user',
      senderName: 'You'
    };
    
    setMessages(prev => [...prev, tempMessage]);
    setNewMessage('');
    
    // Scroll to bottom
    setTimeout(() => {
      messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
    }, 100);
  };

  const formatTime = (dateStr) => {
    const date = new Date(dateStr);
    const now = new Date();
    const diffMs = now - date;
    const diffDays = Math.floor(diffMs / 86400000);

    if (diffDays === 0) {
      return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    } else if (diffDays === 1) {
      return 'Yesterday';
    } else if (diffDays < 7) {
      return date.toLocaleDateString([], { weekday: 'short' });
    }
    return date.toLocaleDateString([], { month: 'short', day: 'numeric' });
  };

  const filteredConversations = conversations.filter(conv => {
    const otherUserName = conv.user1Name || conv.user2Name || '';
    return otherUserName.toLowerCase().includes(searchQuery.toLowerCase());
  });

  const currentUserId = localStorage.getItem('userId');

  return (
    <div className="messages-page">
      <div className="messages-container">
        {/* Conversations List */}
        <div className="conversations-panel">
          <div className="conversations-header">
            <h2>
              <FiMessageSquare className="header-icon" />
              Messages
            </h2>
          </div>
          
          <div className="conversations-search">
            <FiSearch className="search-icon" />
            <input
              type="text"
              placeholder="Search conversations..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
            />
          </div>
          
          <div className="conversations-list">
            {loading ? (
              <div className="conversations-loading">
                <div className="loading-spinner"></div>
                <span>Loading...</span>
              </div>
            ) : filteredConversations.length === 0 ? (
              <div className="conversations-empty">
                <FiMessageSquare className="empty-icon" />
                <p>No conversations yet</p>
                <span>Start chatting with mentors or team members</span>
              </div>
            ) : (
              filteredConversations.map((conv) => {
                const otherUserName = conv.user1Id === currentUserId ? conv.user2Name : conv.user1Name;
                return (
                  <motion.div
                    key={conv.id}
                    className={`conversation-item ${selectedConversation?.id === conv.id ? 'active' : ''}`}
                    onClick={() => handleSelectConversation(conv)}
                    whileHover={{ backgroundColor: '#f8f9fa' }}
                  >
                    <div className="conversation-avatar">
                      <span>{otherUserName?.charAt(0) || 'U'}</span>
                    </div>
                    <div className="conversation-info">
                      <div className="conversation-name">{otherUserName || 'Unknown User'}</div>
                      <div className="conversation-preview">
                        Click to view messages
                      </div>
                    </div>
                    <div className="conversation-meta">
                      <span className="conversation-time">
                        {conv.lastMessageAt ? formatTime(conv.lastMessageAt) : ''}
                      </span>
                    </div>
                  </motion.div>
                );
              })
            )}
          </div>
        </div>

        {/* Chat Panel */}
        <div className="chat-panel">
          {!selectedConversation ? (
            <div className="chat-placeholder">
              <FiMessageSquare className="placeholder-icon" />
              <h3>Select a conversation</h3>
              <p>Choose a conversation from the list to start messaging</p>
            </div>
          ) : (
            <>
              <div className="chat-header">
                <div className="chat-user-info">
                  <div className="chat-avatar">
                    <span>
                      {(selectedConversation.user1Id === currentUserId 
                        ? selectedConversation.user2Name 
                        : selectedConversation.user1Name)?.charAt(0) || 'U'}
                    </span>
                  </div>
                  <div>
                    <div className="chat-user-name">
                      {selectedConversation.user1Id === currentUserId 
                        ? selectedConversation.user2Name 
                        : selectedConversation.user1Name}
                    </div>
                    <div className="chat-user-status">Online</div>
                  </div>
                </div>
                <button className="chat-menu-btn">
                  <FiMoreVertical />
                </button>
              </div>

              <div className="chat-messages">
                {loadingMessages ? (
                  <div className="chat-loading">
                    <div className="loading-spinner"></div>
                    <span>Loading messages...</span>
                  </div>
                ) : messages.length === 0 ? (
                  <div className="chat-empty">
                    <p>No messages yet. Start the conversation!</p>
                  </div>
                ) : (
                  messages.map((msg, index) => {
                    const isOwn = msg.senderId === currentUserId || msg.senderName === 'You';
                    return (
                      <motion.div
                        key={msg.id}
                        className={`chat-message ${isOwn ? 'own' : 'other'}`}
                        initial={{ opacity: 0, y: 10 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ delay: index * 0.03 }}
                      >
                        {!isOwn && (
                          <div className="message-avatar">
                            <span>{msg.senderName?.charAt(0) || 'U'}</span>
                          </div>
                        )}
                        <div className="message-bubble">
                          <div className="message-content">{msg.content}</div>
                          <div className="message-time">{formatTime(msg.sentAt)}</div>
                        </div>
                      </motion.div>
                    );
                  })
                )}
                <div ref={messagesEndRef} />
              </div>

              <form className="chat-input-form" onSubmit={handleSendMessage}>
                <input
                  type="text"
                  placeholder="Type a message..."
                  value={newMessage}
                  onChange={(e) => setNewMessage(e.target.value)}
                />
                <button type="submit" disabled={!newMessage.trim()}>
                  <FiSend />
                </button>
              </form>
            </>
          )}
        </div>
      </div>
    </div>
  );
};

export default MessagesPage;















