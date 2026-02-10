import React, { useState, useEffect } from 'react';
import { useSearchParams, useNavigate, Link } from 'react-router-dom';
import { motion } from 'framer-motion';
import { FiCheck, FiX, FiMail, FiRefreshCw } from 'react-icons/fi';
import api from '../../api/client';
import './EmailVerification.css';

const EmailVerification = () => {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const [status, setStatus] = useState('verifying'); // verifying, success, error, expired
  const [resendLoading, setResendLoading] = useState(false);
  const [resendSuccess, setResendSuccess] = useState(false);

  const token = searchParams.get('token');
  const email = searchParams.get('email');

  useEffect(() => {
    if (token) {
      verifyEmail();
    } else {
      setStatus('pending');
    }
  }, [token]);

  const verifyEmail = async () => {
    try {
      await api.post('/api/auth/verify-email', { token });
      setStatus('success');
      setTimeout(() => {
        navigate('/signin?verified=true');
      }, 3000);
    } catch (error) {
      if (error.response?.status === 410) {
        setStatus('expired');
      } else {
        setStatus('error');
      }
    }
  };

  const resendVerification = async () => {
    if (!email) return;
    setResendLoading(true);
    try {
      await api.post('/api/auth/resend-verification', { email });
      setResendSuccess(true);
    } catch (error) {
      console.error('Failed to resend:', error);
    } finally {
      setResendLoading(false);
    }
  };

  return (
    <div className="email-verification-page">
      <div className="verification-container">
        {status === 'verifying' && (
          <motion.div 
            className="verification-content"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
          >
            <div className="loading-spinner large"></div>
            <h1>Verifying your email...</h1>
            <p>Please wait while we verify your email address.</p>
          </motion.div>
        )}

        {status === 'success' && (
          <motion.div 
            className="verification-content success"
            initial={{ opacity: 0, scale: 0.9 }}
            animate={{ opacity: 1, scale: 1 }}
          >
            <div className="status-icon success">
              <FiCheck />
            </div>
            <h1>Email Verified!</h1>
            <p>Your email has been successfully verified. Redirecting to sign in...</p>
            <Link to="/signin" className="btn-primary">Go to Sign In</Link>
          </motion.div>
        )}

        {status === 'error' && (
          <motion.div 
            className="verification-content error"
            initial={{ opacity: 0, scale: 0.9 }}
            animate={{ opacity: 1, scale: 1 }}
          >
            <div className="status-icon error">
              <FiX />
            </div>
            <h1>Verification Failed</h1>
            <p>We couldn't verify your email. The link may be invalid.</p>
            <div className="action-buttons">
              <button onClick={() => navigate('/signin')} className="btn-secondary">
                Go to Sign In
              </button>
              {email && (
                <button 
                  onClick={resendVerification} 
                  className="btn-primary"
                  disabled={resendLoading || resendSuccess}
                >
                  {resendLoading ? 'Sending...' : resendSuccess ? 'Email Sent!' : 'Resend Email'}
                </button>
              )}
            </div>
          </motion.div>
        )}

        {status === 'expired' && (
          <motion.div 
            className="verification-content expired"
            initial={{ opacity: 0, scale: 0.9 }}
            animate={{ opacity: 1, scale: 1 }}
          >
            <div className="status-icon expired">
              <FiRefreshCw />
            </div>
            <h1>Link Expired</h1>
            <p>This verification link has expired. Request a new one below.</p>
            {email && !resendSuccess ? (
              <button 
                onClick={resendVerification} 
                className="btn-primary"
                disabled={resendLoading}
              >
                {resendLoading ? 'Sending...' : 'Send New Link'}
              </button>
            ) : resendSuccess ? (
              <div className="resend-success">
                <FiCheck /> New verification link sent to {email}
              </div>
            ) : (
              <Link to="/signup" className="btn-primary">Sign Up Again</Link>
            )}
          </motion.div>
        )}

        {status === 'pending' && (
          <motion.div 
            className="verification-content pending"
            initial={{ opacity: 0, scale: 0.9 }}
            animate={{ opacity: 1, scale: 1 }}
          >
            <div className="status-icon pending">
              <FiMail />
            </div>
            <h1>Check Your Email</h1>
            <p>
              We've sent a verification link to your email address. 
              Click the link to verify your account.
            </p>
            <div className="email-tips">
              <h4>Didn't receive the email?</h4>
              <ul>
                <li>Check your spam or junk folder</li>
                <li>Make sure you entered the correct email</li>
                <li>Wait a few minutes and check again</li>
              </ul>
            </div>
            <Link to="/signin" className="btn-secondary">Back to Sign In</Link>
          </motion.div>
        )}
      </div>
    </div>
  );
};

export default EmailVerification;















