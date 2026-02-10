import React, { useState, useEffect } from 'react';
import { Link, useSearchParams, useNavigate } from 'react-router-dom';
import { resetPassword, verifyResetToken } from '../../api/auth';
import './ResetPassword.css';

function ResetPassword() {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const token = searchParams.get('token');
  
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [verifying, setVerifying] = useState(true);
  const [validToken, setValidToken] = useState(false);
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    const verify = async () => {
      if (!token) {
        setVerifying(false);
        return;
      }
      
      try {
        const result = await verifyResetToken(token);
        setValidToken(result.valid);
      } catch {
        setValidToken(false);
      } finally {
        setVerifying(false);
      }
    };
    
    verify();
  }, [token]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    
    if (password !== confirmPassword) {
      setError('Passwords do not match');
      return;
    }
    
    if (password.length < 6) {
      setError('Password must be at least 6 characters');
      return;
    }
    
    setLoading(true);
    
    try {
      await resetPassword(token, password);
      setSuccess(true);
      setTimeout(() => navigate('/signin'), 3000);
    } catch (err) {
      setError(err.response?.data?.error || 'Failed to reset password. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  if (verifying) {
    return (
      <div className="reset-password-container">
        <div className="reset-password-box">
          <div className="loading-spinner"></div>
          <p>Verifying reset link...</p>
        </div>
      </div>
    );
  }

  if (!token || !validToken) {
    return (
      <div className="reset-password-container">
        <div className="reset-password-box">
          <div className="error-icon">❌</div>
          <h2>Invalid or Expired Link</h2>
          <p>This password reset link is invalid or has expired.</p>
          <Link to="/forgot-password" className="action-link">Request a new link</Link>
        </div>
      </div>
    );
  }

  if (success) {
    return (
      <div className="reset-password-container">
        <div className="reset-password-box">
          <div className="success-icon">✅</div>
          <h2>Password Reset Successful!</h2>
          <p>Your password has been reset. Redirecting to sign in...</p>
          <Link to="/signin" className="action-link">Go to Sign In</Link>
        </div>
      </div>
    );
  }

  return (
    <div className="reset-password-container">
      <div className="reset-password-box">
        <div className="icon">🔑</div>
        <h2>Reset Your Password</h2>
        <p>Enter your new password below.</p>
        
        {error && <div className="error-message">{error}</div>}
        
        <form onSubmit={handleSubmit}>
          <div className="input-group">
            <input
              type="password"
              placeholder="New password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              minLength={6}
            />
          </div>
          
          <div className="input-group">
            <input
              type="password"
              placeholder="Confirm new password"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
            />
          </div>
          
          <button type="submit" className="submit-btn" disabled={loading}>
            {loading ? 'Resetting...' : 'Reset Password'}
          </button>
        </form>
        
        <Link to="/signin" className="back-link">← Back to Sign In</Link>
      </div>
    </div>
  );
}

export default ResetPassword;

















