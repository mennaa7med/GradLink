import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation, Link } from 'react-router-dom';
import { FcGoogle } from 'react-icons/fc';
import { FaGithub } from 'react-icons/fa';
import './Signin.css';
import backgroundImg from '../../assets/images/signin.jpg';
import logo from '../../assets/images/site.png';
import { useAuth } from '../../contexts/AuthContext';
import { getOAuthProviders, initiateGoogleLogin, initiateGitHubLogin } from '../../api/oauth';

function Signin() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [userType, setUserType] = useState('student');
  const [companyName, setCompanyName] = useState('');
  const [rememberMe, setRememberMe] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();
  const { login } = useAuth();

  // Load saved credentials on mount
  useEffect(() => {
    const savedEmail = localStorage.getItem('savedEmail');
    const savedUserType = localStorage.getItem('savedUserType');
    const savedCompanyName = localStorage.getItem('savedCompanyName');
    
    if (savedEmail) {
      setEmail(savedEmail);
      setRememberMe(true);
    }
    if (savedUserType) {
      setUserType(savedUserType);
    }
    if (savedCompanyName) {
      setCompanyName(savedCompanyName);
    }
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!email || !password) return alert('Please fill in all fields.');
    
    // Validate company name if user type is company or sponsor
    if ((userType === 'company' || userType === 'sponsor') && !companyName.trim()) {
      return alert('Please enter your company name.');
    }
    
    try {
      const response = await login(email, password);
      
      // Save credentials if Remember Me is checked
      if (rememberMe) {
        localStorage.setItem('savedEmail', email);
        localStorage.setItem('savedUserType', userType);
        if ((userType === 'company' || userType === 'sponsor') && companyName.trim()) {
          localStorage.setItem('savedCompanyName', companyName.trim());
        }
      } else {
        // Clear saved credentials if Remember Me is unchecked
        localStorage.removeItem('savedEmail');
        localStorage.removeItem('savedUserType');
        localStorage.removeItem('savedCompanyName');
      }
      
      // Save company name if user is a company or sponsor
      if ((userType === 'company' || userType === 'sponsor') && companyName.trim()) {
        localStorage.setItem('companyName', companyName.trim());
      }
      
      // Get user role from response or localStorage (set by AuthContext)
      const userRoles = response?.user?.roles || [];
      const userRole = localStorage.getItem('userRole');
      
      // Navigate based on selected user type FIRST, then check roles
      let destination = '/dashboard'; // Default for students
      
      // Priority: Selected userType > Roles from response > Stored role
      if (userType === 'sponsor' || userRoles.includes('Sponsor')) {
        destination = '/sponsor-dashboard';
      } else if (userType === 'mentor' || userRoles.includes('Mentor')) {
        destination = '/mentor-dashboard';
      } else if (userType === 'company' || userRoles.includes('Company') || userRole === 'Company') {
        destination = '/company-dashboard-new';
      } else {
        destination = '/dashboard';
      }
      
      // Always go to the correct dashboard based on type
      navigate(destination, { replace: true });
    } catch (error) {
      // Check if user needs email verification
      if (error.response?.data?.requiresVerification) {
        const email = error.response.data.email;
        alert('Please verify your email. A new code has been sent.');
        navigate('/verify-otp', { state: { email: email } });
      } else {
        alert(error.response?.data?.error || 'Invalid credentials');
      }
    }
  };

  const handleSocialLogin = async (provider) => {
    try {
      // Fetch OAuth provider configuration from backend
      const providers = await getOAuthProviders();
      const providerConfig = providers[provider.toLowerCase()];
      
      if (!providerConfig || !providerConfig.enabled) {
        alert(`${provider} login is not configured yet.\n\nTo enable:\n1. Set up OAuth credentials in ${provider === 'google' ? 'Google Cloud Console' : 'GitHub Developer Settings'}\n2. Add client ID and secret to backend appsettings.json`);
        return;
      }
      
      const redirectUri = providerConfig.redirectUri;
      
      // Initiate OAuth flow
      if (provider.toLowerCase() === 'google') {
        initiateGoogleLogin(providerConfig.clientId, redirectUri);
      } else if (provider.toLowerCase() === 'github') {
        initiateGitHubLogin(providerConfig.clientId, redirectUri);
      }
    } catch (error) {
      console.error('Social login error:', error);
      alert(`Failed to initialize ${provider} login. Please try again.`);
    }
  };

  return (
    <div className="signin-container" style={{ backgroundImage: `url(${backgroundImg})` }}>
      <div className="signin-overlay"></div>

      <div className="signin-box">
        <Link to="/" className="signin-logo-link">
          <img src={logo} alt="GradLink" className="signin-logo" />
        </Link>
        <h2 className="signin-title">Welcome Back 👋</h2>
        <p className="signin-subtitle">Sign in to continue your journey</p>

        <form onSubmit={handleSubmit}>
          <div className="input-group">
            <input
              type="email"
              placeholder="Email address"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>

          <div className="input-group">
            <input
              type="password"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>

          <div className="user-type-group">
            <label className={userType === 'student' ? 'selected' : ''}>
              <input
                type="radio"
                name="userType"
                value="student"
                checked={userType === 'student'}
                onChange={(e) => setUserType(e.target.value)}
              />
              Student
            </label>
            <label className={userType === 'company' ? 'selected' : ''}>
              <input
                type="radio"
                name="userType"
                value="company"
                checked={userType === 'company'}
                onChange={(e) => setUserType(e.target.value)}
              />
              Company
            </label>
            <label className={userType === 'sponsor' ? 'selected sponsor-option' : 'sponsor-option'}>
              <input
                type="radio"
                name="userType"
                value="sponsor"
                checked={userType === 'sponsor'}
                onChange={(e) => setUserType(e.target.value)}
              />
              💰 Sponsor
            </label>
            <label className={userType === 'mentor' ? 'selected mentor-option' : 'mentor-option'}>
              <input
                type="radio"
                name="userType"
                value="mentor"
                checked={userType === 'mentor'}
                onChange={(e) => setUserType(e.target.value)}
              />
              👨‍🏫 Mentor
            </label>
          </div>

          {(userType === 'company' || userType === 'sponsor') && (
            <div className="input-group company-name-input">
              <input
                type="text"
                placeholder={userType === 'sponsor' ? "Enter your company/sponsor name" : "Enter your company name"}
                value={companyName}
                onChange={(e) => setCompanyName(e.target.value)}
                required
                style={{
                  borderColor: userType === 'sponsor' ? '#6366f1' : '#FFCB66',
                  borderWidth: '2px'
                }}
              />
              <span className="input-icon">{userType === 'sponsor' ? '💰' : '🏢'}</span>
            </div>
          )}

          <div className="remember-me-section">
            <label className="remember-me-label">
              <input
                type="checkbox"
                checked={rememberMe}
                onChange={(e) => setRememberMe(e.target.checked)}
                className="remember-me-checkbox"
              />
              <span className="remember-me-text">Remember me</span>
            </label>
          </div>

          <button type="submit" className="signin-btn">Sign In</button>
        </form>

        <div className="social-divider">
          <span>or continue with</span>
        </div>

        <div className="social-buttons">
          <button 
            type="button" 
            className="social-btn google-btn"
            onClick={() => handleSocialLogin('google')}
          >
            <FcGoogle size={20} />
            <span>Google</span>
          </button>
          <button 
            type="button" 
            className="social-btn github-btn"
            onClick={() => handleSocialLogin('github')}
          >
            <FaGithub size={20} />
            <span>GitHub</span>
          </button>
        </div>

        <div className="signin-footer">
          <Link to="/forgot-password" className="forgot">Forgot Password?</Link>
          <p className="signup-prompt">
            Don't have an account? <Link to="/signup">Sign up</Link>
          </p>
        </div>
      </div>
    </div>
  );
}

export default Signin;
