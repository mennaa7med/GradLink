import './GeminiChatButton.css';
import { useState, useEffect, useRef } from 'react';
import { useLocation } from 'react-router-dom';
import { motion, AnimatePresence } from 'framer-motion';
import img12 from '../assets/images/ai-assistant.png';
import * as chatApi from '../api/chat';

const GeminiChatButton = () => {
  const [isOpen, setIsOpen] = useState(false);
  const [input, setInput] = useState('');
  const [messages, setMessages] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const messagesEndRef = useRef(null);
  const inputRef = useRef(null);
  const location = useLocation();

  // الصفحات التي لا يظهر فيها الـ Chatbot
  const hiddenPaths = ['/signin', '/signup', '/forgot-password', '/reset-password', '/oauth/callback'];
  const shouldHide = hiddenPaths.some(path => location.pathname.startsWith(path));

  // Load chat history from localStorage
  useEffect(() => {
    const savedMessages = localStorage.getItem('gemini-chat-history');
    if (savedMessages) {
      try {
        setMessages(JSON.parse(savedMessages));
      } catch (error) {
        console.error('Error loading chat history:', error);
        setMessages([{ 
          role: 'bot', 
          text: 'Hello! 👋 I\'m your AI Assistant powered by Gemini. How can I help you today?',
          timestamp: new Date().toISOString()
        }]);
      }
    } else {
      setMessages([{ 
        role: 'bot', 
        text: 'Hello! 👋 I\'m your AI Assistant powered by Gemini. How can I help you today?',
        timestamp: new Date().toISOString()
      }]);
    }
  }, []);

  // Save chat history to localStorage
  useEffect(() => {
    if (messages.length > 0) {
      localStorage.setItem('gemini-chat-history', JSON.stringify(messages));
    }
  }, [messages]);

  // Auto-scroll to bottom
  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, [messages]);

  // Focus input when chat opens
  useEffect(() => {
    if (isOpen && inputRef.current) {
      setTimeout(() => inputRef.current?.focus(), 100);
    }
  }, [isOpen]);

  const sendMessage = async () => {
    const messageText = input.trim();
    if (!messageText || isLoading) return;

    // Add user message
    const userMessage = { 
      role: 'user', 
      text: messageText,
      timestamp: new Date().toISOString()
    };
    setMessages(prev => [...prev, userMessage]);
    setInput('');
    setIsLoading(true);

    try {
      const data = await chatApi.sendChatMessage(messageText);
      const botMessage = { 
        role: 'bot', 
        text: data.text || 'Sorry, I could not process your request.',
        timestamp: new Date().toISOString()
      };
      setMessages(prev => [...prev, botMessage]);
    } catch (error) {
      console.error('Error sending message:', error);
      const errorMessage = { 
        role: 'bot', 
        text: '❌ Sorry, I encountered an error. Please make sure the backend is running and try again.',
        timestamp: new Date().toISOString()
      };
      setMessages(prev => [...prev, errorMessage]);
    } finally {
      setIsLoading(false);
    }
  };

  const handleKeyPress = (e) => {
    if (e.key === 'Enter' && !e.shiftKey) {
      e.preventDefault();
      sendMessage();
    }
  };

  const clearHistory = () => {
    setMessages([{ 
      role: 'bot', 
      text: 'Chat cleared! 🧹 How can I help you?',
      timestamp: new Date().toISOString()
    }]);
    localStorage.removeItem('gemini-chat-history');
  };

  const formatMessage = (text) => {
    // Simple markdown-like formatting
    return text
      .replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>')
      .replace(/\*(.*?)\*/g, '<em>$1</em>')
      .replace(/`(.*?)`/g, '<code>$1</code>')
      .replace(/\n/g, '<br/>');
  };

  // إذا كان يجب إخفاء الـ Chatbot، لا نعرض شيء
  if (shouldHide) return null;

  return (
    <>
      {/* Toggle Button */}
      <motion.button 
        className="chatbot-toggle" 
        onClick={() => setIsOpen(!isOpen)}
        aria-label="Toggle AI chat"
        whileHover={{ scale: 1.1 }}
        whileTap={{ scale: 0.9 }}
        animate={isOpen ? { rotate: 45 } : { rotate: 0 }}
        transition={{ type: "spring", stiffness: 300 }}
      >
        <img src={img12} alt="AI Assistant" />
      </motion.button>

      {/* Chat Container */}
      <AnimatePresence>
        {isOpen && (
          <motion.div 
            className="chat-container"
            initial={{ opacity: 0, y: 20, scale: 0.95 }}
            animate={{ opacity: 1, y: 0, scale: 1 }}
            exit={{ opacity: 0, y: 20, scale: 0.95 }}
            transition={{ type: "spring", damping: 25, stiffness: 300 }}
          >
            {/* Header */}
            <div className="chat-header">
              <h2>AI Assistant</h2>
              <motion.button 
                className="clear-btn" 
                onClick={clearHistory} 
                title="Clear history"
                whileHover={{ scale: 1.1, rotate: 10 }}
                whileTap={{ scale: 0.9 }}
              >
                🗑️
              </motion.button>
            </div>
            
            {/* Messages */}
            <div className="messages">
              {messages.map((msg, index) => (
                <motion.div 
                  key={index} 
                  className={`msg ${msg.role}`}
                  initial={{ opacity: 0, y: 10, scale: 0.95 }}
                  animate={{ opacity: 1, y: 0, scale: 1 }}
                  transition={{ 
                    type: "spring", 
                    damping: 20, 
                    stiffness: 300,
                    delay: index === messages.length - 1 ? 0.1 : 0
                  }}
                  dangerouslySetInnerHTML={{ __html: formatMessage(msg.text) }}
                />
              ))}
              
              {/* Typing Indicator */}
              <AnimatePresence>
                {isLoading && (
                  <motion.div 
                    className="msg bot typing"
                    initial={{ opacity: 0, y: 10 }}
                    animate={{ opacity: 1, y: 0 }}
                    exit={{ opacity: 0, y: -10 }}
                  >
                    Thinking
                  </motion.div>
                )}
              </AnimatePresence>
              
              <div ref={messagesEndRef} />
            </div>

            {/* Input Area */}
            <div className="input-area">
              <input
                ref={inputRef}
                type="text"
                value={input}
                onChange={(e) => setInput(e.target.value)}
                onKeyPress={handleKeyPress}
                placeholder="Message AI Assistant..."
                disabled={isLoading}
              />
              <motion.button 
                onClick={sendMessage} 
                disabled={isLoading || !input.trim()}
                whileHover={!isLoading && input.trim() ? { scale: 1.05 } : {}}
                whileTap={!isLoading && input.trim() ? { scale: 0.95 } : {}}
              >
                {isLoading ? (
                  <>Sending</>
                ) : (
                  <>Send ➤</>
                )}
              </motion.button>
            </div>
          </motion.div>
        )}
      </AnimatePresence>
    </>
  );
};

export default GeminiChatButton;
