import React, { useState, useEffect } from 'react';
import './Navbar.css';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import img from '../assets/images/logo.png';
import { FiGrid } from 'react-icons/fi';

const Navbar = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  // Check login status
  useEffect(() => {
    const checkAuth = () => {
      const token = localStorage.getItem('accessToken');
      setIsLoggedIn(!!token);
    };
    
    checkAuth();
    // Listen for storage changes (login/logout from other tabs)
    window.addEventListener('storage', checkAuth);
    return () => window.removeEventListener('storage', checkAuth);
  }, []);

  // Get dashboard path based on user role
  const getDashboardPath = () => {
    const role = localStorage.getItem('userRole')?.toLowerCase();
    
    switch (role) {
      case 'sponsor':
        return '/sponsor-dashboard';
      case 'mentor':
        return '/mentor-dashboard';
      case 'company':
        return '/company-dashboard-new';
      case 'admin':
        return '/admin-dashboard';
      default:
        return '/dashboard'; // Student dashboard
    }
  };

  const scrollToSection = (id) => {
    // إذا كنا في الصفحة الرئيسية، نقوم بالـ scroll مباشرة
    if (location.pathname === '/') {
      const element = document.getElementById(id);
      if (element) {
        element.scrollIntoView({ behavior: 'smooth' });
      }
    } else {
      // إذا كنا في صفحة أخرى، نذهب للصفحة الرئيسية ثم نقوم بالـ scroll
      navigate('/', { state: { scrollTo: id } });
    }
  };

  // التعامل مع الـ scroll بعد التنقل من صفحة أخرى
  React.useEffect(() => {
    if (location.state?.scrollTo) {
      const id = location.state.scrollTo;
      setTimeout(() => {
        const element = document.getElementById(id);
        if (element) {
          element.scrollIntoView({ behavior: 'smooth' });
        }
      }, 100);
      // مسح الـ state بعد الاستخدام
      window.history.replaceState({}, document.title);
    }
  }, [location]);

  return (
    <header className="navbar">
      <div className="navbar-left">
        <Link to="/" className="logo-link">
          <img src={img} alt="Site Logo" className="logo" />
          <div className="site-info">
            <span className="site-name">GradLink</span>
          </div>
        </Link>
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
            <Link to="/career">Career</Link>
          </li>
          <li>
            <Link to="/Opportunities">Opportunities</Link>
          </li>
          <li>
            <Link to="/screening-resume">Screening Resume</Link>
          </li>
        </ul>

        <div className="nav-buttons">
          {isLoggedIn ? (
            <Link to={getDashboardPath()} className="dashboard-btn">
              <FiGrid /> My Dashboard
            </Link>
          ) : (
            <>
              <Link to="/signin" className="signin-btn">Sign In</Link>
              <Link to="/signup" className="signup-btnn">Sign Up</Link>
            </>
          )}
        </div>
      </nav>
    </header>
  );
};

export default Navbar;
