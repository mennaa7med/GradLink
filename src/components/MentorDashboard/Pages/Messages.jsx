import React, { useState, useRef, useEffect } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { 
  FiSearch, 
  FiSend, 
  FiPaperclip,
  FiSmile,
  FiMoreVertical,
  FiCheck,
  FiCheckCircle,
  FiImage
} from 'react-icons/fi';
import './Messages.css';

const Messages = () => {
  const [selectedConversation, setSelectedConversation] = useState(null);
  const [messageText, setMessageText] = useState('');
  const [searchQuery, setSearchQuery] = useState('');
  const messagesEndRef = useRef(null);

  // Mock conversations data
  const [conversations, setConversations] = useState([
    {
      id: 1,
      student: 'Ahmed Hassan',
      avatar: null,
      lastMessage: 'Thank you for the feedback on my project!',
      time: '10:30 AM',
      unread: 2,
      online: true,
      messages: [
        { id: 1, text: 'Hello mentor, I have a question about the AI model.', sender: 'student', time: '10:00 AM', read: true },
        { id: 2, text: 'Of course! What would you like to know?', sender: 'mentor', time: '10:05 AM', read: true },
        { id: 3, text: 'I\'m having trouble with the training accuracy. It\'s stuck at 75%.', sender: 'student', time: '10:10 AM', read: true },
        { id: 4, text: 'Try adjusting the learning rate and adding more training data. Also, consider using data augmentation.', sender: 'mentor', time: '10:15 AM', read: true },
        { id: 5, text: 'Thank you for the feedback on my project!', sender: 'student', time: '10:30 AM', read: false },
        { id: 6, text: 'I\'ll try those suggestions right away.', sender: 'student', time: '10:31 AM', read: false }
      ]
    },
    {
      id: 2,
      student: 'Sarah Mohamed',
      avatar: null,
      lastMessage: 'Can we schedule a meeting for tomorrow?',
      time: '9:45 AM',
      unread: 1,
      online: true,
      messages: [
        { id: 1, text: 'Hi! I wanted to discuss my career plans.', sender: 'student', time: '9:30 AM', read: true },
        { id: 2, text: 'Can we schedule a meeting for tomorrow?', sender: 'student', time: '9:45 AM', read: false }
      ]
    },
    {
      id: 3,
      student: 'ahmed ali',
      avatar: null,
      lastMessage: 'The database design is ready for review.',
      time: 'Yesterday',
      unread: 0,
      online: false,
      messages: [
        { id: 1, text: 'I\'ve finished the database design.', sender: 'student', time: 'Yesterday, 4:00 PM', read: true },
        { id: 2, text: 'The database design is ready for review.', sender: 'student', time: 'Yesterday, 4:05 PM', read: true },
        { id: 3, text: 'Great! I\'ll review it and get back to you.', sender: 'mentor', time: 'Yesterday, 5:00 PM', read: true }
      ]
    },
    {
      id: 4,
      student: 'Fatma Ibrahim',
      avatar: null,
      lastMessage: 'Thanks! See you on Monday.',
      time: 'Monday',
      unread: 0,
      online: false,
      messages: [
        { id: 1, text: 'Our session was very helpful!', sender: 'student', time: 'Monday, 3:00 PM', read: true },
        { id: 2, text: 'Thanks! See you on Monday.', sender: 'student', time: 'Monday, 3:05 PM', read: true }
      ]
    },
    {
      id: 5,
      student: 'Mahmoud Khalil',
      avatar: null,
      lastMessage: 'The IoT sensors are working perfectly now.',
      time: 'Last week',
      unread: 0,
      online: true,
      messages: [
        { id: 1, text: 'I fixed the sensor connectivity issue.', sender: 'student', time: 'Last week', read: true },
        { id: 2, text: 'The IoT sensors are working perfectly now.', sender: 'student', time: 'Last week', read: true }
      ]
    }
  ]);

  // Filter conversations
  const filteredConversations = conversations.filter(conv =>
    conv.student.toLowerCase().includes(searchQuery.toLowerCase())
  );

  // Scroll to bottom of messages
  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  };

  useEffect(() => {
    scrollToBottom();
  }, [selectedConversation?.messages]);

  // Send message
  const sendMessage = (e) => {
    e.preventDefault();
    if (!messageText.trim() || !selectedConversation) return;

    const newMessage = {
      id: Date.now(),
      text: messageText,
      sender: 'mentor',
      time: new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
      read: true
    };

    setConversations(convs => 
      convs.map(conv => 
        conv.id === selectedConversation.id
          ? { 
              ...conv, 
              messages: [...conv.messages, newMessage],
              lastMessage: messageText,
              time: 'Just now'
            }
          : conv
      )
    );

    setSelectedConversation(prev => ({
      ...prev,
      messages: [...prev.messages, newMessage]
    }));

    setMessageText('');
  };

  // Mark as read
  const markAsRead = (convId) => {
    setConversations(convs =>
      convs.map(conv =>
        conv.id === convId
          ? { 
              ...conv, 
              unread: 0,
              messages: conv.messages.map(m => ({ ...m, read: true }))
            }
          : conv
      )
    );
  };

  const selectConversation = (conv) => {
    setSelectedConversation(conv);
    markAsRead(conv.id);
  };

  return (
    <div className="messages-page">
      {/* Conversations List */}
      <div className="conversations-panel">
        <div className="panel-header">
          <h2>Messages</h2>
          <span className="unread-total">
            {conversations.reduce((sum, c) => sum + c.unread, 0)} unread
          </span>
        </div>

        <div className="search-box">
          <FiSearch className="search-icon" />
          <input
            type="text"
            placeholder="Search conversations..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
          />
        </div>

        <div className="conversations-list">
          {filteredConversations.map((conv) => (
            <motion.div
              key={conv.id}
              className={`conversation-item ${selectedConversation?.id === conv.id ? 'active' : ''} ${conv.unread > 0 ? 'unread' : ''}`}
              onClick={() => selectConversation(conv)}
              whileHover={{ x: 4 }}
            >
              <div className="conv-avatar">
                {conv.student.charAt(0)}
                {conv.online && <span className="online-indicator"></span>}
              </div>
              <div className="conv-info">
                <div className="conv-top">
                  <span className="conv-name">{conv.student}</span>
                  <span className="conv-time">{conv.time}</span>
                </div>
                <div className="conv-bottom">
                  <span className="conv-message">{conv.lastMessage}</span>
                  {conv.unread > 0 && (
                    <span className="unread-badge">{conv.unread}</span>
                  )}
                </div>
              </div>
            </motion.div>
          ))}
        </div>
      </div>

      {/* Chat Panel */}
      <div className="chat-panel">
        {selectedConversation ? (
          <>
            {/* Chat Header */}
            <div className="chat-header">
              <div className="chat-user">
                <div className="chat-avatar">
                  {selectedConversation.student.charAt(0)}
                  {selectedConversation.online && <span className="online-indicator"></span>}
                </div>
                <div className="chat-user-info">
                  <span className="chat-user-name">{selectedConversation.student}</span>
                  <span className="chat-user-status">
                    {selectedConversation.online ? 'Online' : 'Offline'}
                  </span>
                </div>
              </div>
              <button className="more-btn">
                <FiMoreVertical />
              </button>
            </div>

            {/* Messages */}
            <div className="messages-container">
              {selectedConversation.messages.map((message, index) => (
                <motion.div
                  key={message.id}
                  className={`message ${message.sender}`}
                  initial={{ opacity: 0, y: 10 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ delay: index * 0.02 }}
                >
                  <div className="message-bubble">
                    <p className="message-text">{message.text}</p>
                    <div className="message-meta">
                      <span className="message-time">{message.time}</span>
                      {message.sender === 'mentor' && (
                        <span className="message-status">
                          {message.read ? <FiCheckCircle /> : <FiCheck />}
                        </span>
                      )}
                    </div>
                  </div>
                </motion.div>
              ))}
              <div ref={messagesEndRef} />
            </div>

            {/* Message Input */}
            <form className="message-input-container" onSubmit={sendMessage}>
              <button type="button" className="input-action" title="Attach file">
                <FiPaperclip />
              </button>
              <button type="button" className="input-action" title="Send image">
                <FiImage />
              </button>
              <input
                type="text"
                placeholder="Type a message..."
                value={messageText}
                onChange={(e) => setMessageText(e.target.value)}
              />
              <button type="button" className="input-action" title="Emoji">
                <FiSmile />
              </button>
              <button 
                type="submit" 
                className={`send-btn ${messageText.trim() ? 'active' : ''}`}
                disabled={!messageText.trim()}
              >
                <FiSend />
              </button>
            </form>
          </>
        ) : (
          <div className="no-chat-selected">
            <div className="no-chat-icon">💬</div>
            <h3>Select a conversation</h3>
            <p>Choose a mentee from the list to start chatting</p>
          </div>
        )}
      </div>
    </div>
  );
};

export default Messages;






