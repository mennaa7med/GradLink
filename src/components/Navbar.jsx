import React from 'react';
import './Navbar.css';
import { Link } from 'react-router-dom';
import img from '../assets/images/logo.png';

const Navbar = () => {
  const scrollToSection = (id) => {
    const element = document.getElementById(id);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth' });
    }
  };

  return (
    <header className="navbar">
      <div className="navbar-left">
        <img src={img} alt="Site Logo" className="logo" />
        <div className="site-info">
          <span className="site-name">GradLink</span>
        </div>
      </div>

      <nav className="navbar-right">
        <ul className="nav-links">
          <li>
            <button onClick={() => scrollToSection('home')} className="nav-button">
              Home
            </button>
          </li>
          <li>
            <button onClick={() => scrollToSection('services')} className="nav-button">
              Services
            </button>
          </li>
          <li>
            <button onClick={() => scrollToSection('features')} className="nav-button">
              Features
            </button>
          </li>
          <li>
            <button onClick={() => scrollToSection('about')} className="nav-button">
              About Us
            </button>
          </li>
          <li>
            <Link to="/projectsbank">Projects Bank</Link>
          </li>
          <li>
            <Link to="/Opportunities">Opportunities</Link>
          </li>
          
        </ul>

        <div className="nav-buttons">
          <Link to="/signin" className="signin-btn">Sign In</Link>
          <Link to="/signup" className="signup-btnn">Sign Up</Link>
        </div>
      </nav>
    </header>
  );
};

export default Navbar;
