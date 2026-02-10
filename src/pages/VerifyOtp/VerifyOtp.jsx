import React, { useState, useRef, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import './VerifyOtp.css';
import logo from '../../assets/images/site.png';
import videoBg from '../../assets/images/vid.mp4';
import videoPoster from '../../assets/images/signin.jpg';
import api from '../../api/client';
import { useAuth } from '../../contexts/AuthContext';

function VerifyOtp() {
  const [otp, setOtp] = useState(['', '', '', '', '', '']);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [resendTimer, setResendTimer] = useState(60);
  const [canResend, setCanResend] = useState(false);
  
  const inputRefs = useRef([]);
  const navigate = useNavigate();
  const location = useLocation();
  const { loginWithToken } = useAuth();
  
  // Get email from location state or redirect to signup
  const email = location.state?.email || '';
  
  useEffect(() => {
    if (!email) {
      navigate('/signup');
      return;
    }
    
    // Focus first input
    inputRefs.current[0]?.focus();
    
    // Start countdown timer
    const timer = setInterval(() => {
      setResendTimer((prev) => {
        if (prev <= 1) {
          setCanResend(true);
          clearInterval(timer);
          return 0;
        }
        return prev - 1;
      });
    }, 1000);
    
    return () => clearInterval(timer);
  }, [email, navigate]);

  const handleChange = (index, value) => {
    // Only allow numbers
    if (value && !/^\d$/.test(value)) return;
    
    const newOtp = [...otp];
    newOtp[index] = value;
    setOtp(newOtp);
    setError('');
    
    // Auto-focus next input
    if (value && index < 5) {
      inputRefs.current[index + 1]?.focus();
    }
    
    // Auto-submit when all digits are entered
    if (index === 5 && value) {
      const fullOtp = newOtp.join('');
      if (fullOtp.length === 6) {
        handleVerify(fullOtp);
      }
    }
  };

  const handleKeyDown = (index, e) => {
    // Handle backspace
    if (e.key === 'Backspace' && !otp[index] && index > 0) {
      inputRefs.current[index - 1]?.focus();
    }
    
    // Handle paste
    if (e.key === 'v' && (e.ctrlKey || e.metaKey)) {
      e.preventDefault();
      navigator.clipboard.readText().then((text) => {
        const digits = text.replace(/\D/g, '').slice(0, 6).split('');
        const newOtp = [...otp];
        digits.forEach((digit, i) => {
          if (i < 6) newOtp[i] = digit;
        });
        setOtp(newOtp);
        if (digits.length === 6) {
          handleVerify(digits.join(''));
        }
      });
    }
  };

  const handlePaste = (e) => {
    e.preventDefault();
    const text = e.clipboardData.getData('text');
    const digits = text.replace(/\D/g, '').slice(0, 6).split('');
    const newOtp = [...otp];
    digits.forEach((digit, i) => {
      if (i < 6) newOtp[i] = digit;
    });
    setOtp(newOtp);
    if (digits.length === 6) {
      handleVerify(digits.join(''));
    }
  };

  const handleVerify = async (otpCode) => {
    if (otpCode.length !== 6) {
      setError('Please enter the complete 6-digit code');
      return;
    }
    
    setIsLoading(true);
    setError('');
    
    try {
      const response = await api.post('/api/auth/verify-otp', {
        email: email,
        otpCode: otpCode
      });
      
      if (response.data.accessToken) {
        setSuccess('Email verified successfully! 🎉');
        
        // Store tokens and login
        if (loginWithToken) {
          loginWithToken(
            response.data.accessToken,
            response.data.refreshToken,
            response.data.user
          );
        }
        
        // Redirect based on role
        setTimeout(() => {
          const roles = response.data.user?.roles || [];
          if (roles.includes('Company')) {
            navigate('/company-dashboard-new');
          } else if (roles.includes('Mentor')) {
            navigate('/mentor-dashboard');
          } else if (roles.includes('Sponsor')) {
            navigate('/sponsor-dashboard');
          } else {
            navigate('/dashboard');
          }
        }, 1500);
      }
    } catch (err) {
      const errorMsg = err.response?.data?.error || 'Verification failed. Please try again.';
      setError(errorMsg);
      setOtp(['', '', '', '', '', '']);
      inputRefs.current[0]?.focus();
    } finally {
      setIsLoading(false);
    }
  };

  const handleResend = async () => {
    if (!canResend) return;
    
    setIsLoading(true);
    setError('');
    
    try {
      await api.post('/api/auth/resend-otp', { email });
      setSuccess('A new code has been sent to your email! 📧');
      setCanResend(false);
      setResendTimer(60);
      setOtp(['', '', '', '', '', '']);
      inputRefs.current[0]?.focus();
      
      // Restart timer
      const timer = setInterval(() => {
        setResendTimer((prev) => {
          if (prev <= 1) {
            setCanResend(true);
            clearInterval(timer);
            return 0;
          }
          return prev - 1;
        });
      }, 1000);
      
      setTimeout(() => setSuccess(''), 3000);
    } catch (err) {
      setError('Failed to resend code. Please try again.');
    } finally {
      setIsLoading(false);
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    handleVerify(otp.join(''));
  };

  return (
    <div className="verify-otp-container">
      <video autoPlay muted loop playsInline preload="metadata" poster={videoPoster} className="verify-otp-video">
        <source src={videoBg} type="video/mp4" />
      </video>
      <div className="verify-otp-overlay"></div>
      
      <div className="verify-otp-box">
        <img src={logo} alt="GradLink" className="verify-otp-logo" />
        
        <div className="verify-otp-icon">📧</div>
        <h2 className="verify-otp-title">Verify Your Email</h2>
        <p className="verify-otp-subtitle">
          We've sent a 6-digit code to<br />
          <strong>{email}</strong>
        </p>

        {error && <div className="error-message">{error}</div>}
        {success && <div className="success-message">{success}</div>}

        <form onSubmit={handleSubmit}>
          <div className="otp-inputs">
            {otp.map((digit, index) => (
              <input
                key={index}
                ref={(el) => (inputRefs.current[index] = el)}
                type="text"
                inputMode="numeric"
                maxLength={1}
                value={digit}
                onChange={(e) => handleChange(index, e.target.value)}
                onKeyDown={(e) => handleKeyDown(index, e)}
                onPaste={handlePaste}
                className={`otp-input ${digit ? 'filled' : ''}`}
                disabled={isLoading}
                autoComplete="off"
              />
            ))}
          </div>

          <button 
            type="submit" 
            className="verify-btn"
            disabled={isLoading || otp.join('').length !== 6}
          >
            {isLoading ? (
              <span className="loading-spinner"></span>
            ) : (
              'Verify Email ✓'
            )}
          </button>
        </form>

        <div className="resend-section">
          <p>Didn't receive the code?</p>
          {canResend ? (
            <button 
              onClick={handleResend} 
              className="resend-btn"
              disabled={isLoading}
            >
              Resend Code
            </button>
          ) : (
            <span className="resend-timer">
              Resend in {resendTimer}s
            </span>
          )}
        </div>

        <button 
          onClick={() => navigate('/signup')} 
          className="back-link"
        >
          ← Back to Sign Up
        </button>
      </div>
    </div>
  );
}

export default VerifyOtp;




