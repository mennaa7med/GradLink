import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom'; // Import useNavigate
import './Signin.css';
import img10 from '../../assets/images/vid.mp4';
import logo from '../../assets/images/site.png'; // Ensure this path is correct

function Signin() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate(); // Initialize navigate

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log('Login Attempt:', { email, password });
    // Simulate a successful login (replace with actual authentication logic)
    if (email && password) {
      // Redirect to the dashboard page
      navigate('/Dashboard'); // Change '/dashboard' to your desired page
    } else {
      alert('Please fill in both fields.');
    }
    // Add your authentication logic here (e.g., API call to .NET backend)
  };

  return (
    <div className="login-container">
      <video autoPlay muted loop className="background-video">
        <source src={img10} type="video/mp4" />
      </video>
      <div className="overlay"></div>
      <div className="login-box">
        <div className="header-section">
          <img src={logo} alt="GradLink Logo" className="logo" />
          <h2>Welcome to GradLink</h2>
        </div>
        <p>Sign in to manage your graduation journey</p>
        <form onSubmit={handleSubmit}>
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
          <button type="submit" className="login-btn">Sign In</button>
        </form>
        <div className="forgot-password">
          <a href="#">Forgot Password?</a>
        </div>
      </div>
    </div>
  );
}

export default Signin;