import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom'; // For navigation after signup
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faGoogle, faApple } from '@fortawesome/free-brands-svg-icons';
import img7 from '../../assets/images/vidd.mp4';

import './Signup.css';

function Signup() {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [agreeTerms, setAgreeTerms] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log('Signup Attempt:', { name, email, password, agreeTerms });
    // Simulate a successful signup (replace with actual API call)
    if (name && email && password && agreeTerms) {
      navigate('/dashboard'); // Redirect to dashboard after signup
    } else {
      alert('Please fill all fields and agree to the terms.');
    }
    // Add your authentication logic here (e.g., API call to .NET backend)
  };

  const handleGoogleSignup = () => {
    console.log('Google Signup Attempt');
    // Add Google OAuth logic here
    navigate('/dashboard'); // Redirect after successful Google signup (simulated)
  };

  const handleAppleSignup = () => {
    console.log('Apple Signup Attempt');
    // Add Apple OAuth logic here
    navigate('/dashboard'); // Redirect after successful Apple signup (simulated)
  };

  return (
    <div className="login-container">
      <video autoPlay muted loop className="background-video">
              <source src={img7} type="video/mp4" />
            </video>
      <div className="overlay"></div>
      <div className="login-box">
        <div className="header-section">
          <h2>Welcome to GradLink</h2>
        </div>
        <p>Sign up to start your graduation journey</p>
        <form onSubmit={handleSubmit}>
          <div className="input-group">
            <input
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
              placeholder="Full Name"
              required
            />
          </div>
          <div className="input-group">
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="Email"
              required
            />
          </div>
          <div className="input-group">
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Password"
              required
            />
          </div>
          <div className="terms-section">
            <input
              type="checkbox"
              checked={agreeTerms}
              onChange={(e) => setAgreeTerms(e.target.checked)}
              required
            />
            <label>
              I agree to the <a href="#">terms and policy</a>
            </label>
          </div>
          <button type="submit" className="login-btn">Sign Up</button>
        </form>
        <div className="divider">
          <hr />
          <span>or</span>
          <hr />
        </div>
        <div className="social-buttons">
          <button className="google-btn" onClick={handleGoogleSignup}>
            <FontAwesomeIcon icon={faGoogle} /> Sign up with Google
          </button>
          <button className="apple-btn" onClick={handleAppleSignup}>
            <FontAwesomeIcon icon={faApple} /> Sign up with Apple
          </button>
        </div>
        <div className="signin-link">
          Already have an account? <a href="/signin">Sign In</a>
        </div>
      </div>
    </div>
  );
}

export default Signup;