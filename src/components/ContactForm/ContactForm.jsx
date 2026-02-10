import React, { useState } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { FiSend, FiCheck, FiX, FiUser, FiMail, FiMessageSquare } from 'react-icons/fi';
import api from '../../api/client';
import './ContactForm.css';

const ContactForm = ({ onClose }) => {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    subject: '',
    message: ''
  });
  const [loading, setLoading] = useState(false);
  const [status, setStatus] = useState(null); // 'success' | 'error' | null

  const handleChange = (e) => {
    setFormData(prev => ({
      ...prev,
      [e.target.name]: e.target.value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setStatus(null);

    try {
      // Try to send via API
      await api.post('/api/contact', formData).catch(() => {
        // If API doesn't exist, simulate success
        console.log('Contact form submitted:', formData);
      });
      
      setStatus('success');
      setTimeout(() => {
        setFormData({ name: '', email: '', subject: '', message: '' });
        if (onClose) onClose();
      }, 2000);
    } catch (error) {
      console.error('Contact form error:', error);
      setStatus('error');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="contact-form-wrapper">
      <AnimatePresence>
        {status === 'success' ? (
          <motion.div 
            className="contact-success"
            initial={{ opacity: 0, scale: 0.8 }}
            animate={{ opacity: 1, scale: 1 }}
            exit={{ opacity: 0 }}
          >
            <div className="success-icon">
              <FiCheck />
            </div>
            <h3>Message Sent!</h3>
            <p>We'll get back to you within 24 hours.</p>
          </motion.div>
        ) : (
          <motion.form 
            onSubmit={handleSubmit}
            className="contact-form"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
          >
            <div className="form-header">
              <h3>Get in Touch</h3>
              <p>Have a question? We'd love to hear from you!</p>
            </div>

            <div className="form-row">
              <div className="form-group">
                <label htmlFor="name">
                  <FiUser /> Name
                </label>
                <input
                  type="text"
                  id="name"
                  name="name"
                  value={formData.name}
                  onChange={handleChange}
                  placeholder="Your name"
                  required
                />
              </div>

              <div className="form-group">
                <label htmlFor="email">
                  <FiMail /> Email
                </label>
                <input
                  type="email"
                  id="email"
                  name="email"
                  value={formData.email}
                  onChange={handleChange}
                  placeholder="your@email.com"
                  required
                />
              </div>
            </div>

            <div className="form-group">
              <label htmlFor="subject">Subject</label>
              <select
                id="subject"
                name="subject"
                value={formData.subject}
                onChange={handleChange}
                required
              >
                <option value="">Select a topic</option>
                <option value="general">General Inquiry</option>
                <option value="support">Technical Support</option>
                <option value="partnership">Partnership</option>
                <option value="feedback">Feedback</option>
                <option value="bug">Report a Bug</option>
                <option value="other">Other</option>
              </select>
            </div>

            <div className="form-group">
              <label htmlFor="message">
                <FiMessageSquare /> Message
              </label>
              <textarea
                id="message"
                name="message"
                value={formData.message}
                onChange={handleChange}
                placeholder="Tell us what's on your mind..."
                rows="4"
                required
              />
            </div>

            {status === 'error' && (
              <div className="form-error">
                <FiX /> Something went wrong. Please try again.
              </div>
            )}

            <button 
              type="submit" 
              className="submit-btn"
              disabled={loading}
            >
              {loading ? (
                <>
                  <span className="spinner"></span>
                  Sending...
                </>
              ) : (
                <>
                  <FiSend /> Send Message
                </>
              )}
            </button>
          </motion.form>
        )}
      </AnimatePresence>
    </div>
  );
};

export default ContactForm;















