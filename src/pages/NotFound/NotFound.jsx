import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { motion } from 'framer-motion';
import { FiHome, FiArrowLeft, FiSearch } from 'react-icons/fi';
import './NotFound.css';

const NotFound = () => {
  const navigate = useNavigate();

  return (
    <div className="not-found-page">
      <div className="not-found-content">
        <motion.div
          className="error-code"
          initial={{ scale: 0, rotate: -180 }}
          animate={{ scale: 1, rotate: 0 }}
          transition={{ type: "spring", duration: 1 }}
        >
          <span className="four">4</span>
          <motion.span 
            className="zero"
            animate={{ 
              rotate: [0, 360],
              scale: [1, 1.1, 1]
            }}
            transition={{ 
              duration: 3,
              repeat: Infinity,
              repeatType: "reverse"
            }}
          >
            🎓
          </motion.span>
          <span className="four">4</span>
        </motion.div>

        <motion.h1
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.3 }}
        >
          Oops! Page Not Found
        </motion.h1>

        <motion.p
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.5 }}
        >
          The page you're looking for doesn't exist or has been moved.
          Let's get you back on track!
        </motion.p>

        <motion.div 
          className="action-buttons"
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.7 }}
        >
          <button className="btn-back" onClick={() => navigate(-1)}>
            <FiArrowLeft /> Go Back
          </button>
          <Link to="/" className="btn-home">
            <FiHome /> Home Page
          </Link>
          <Link to="/career" className="btn-explore">
            <FiSearch /> Explore Career
          </Link>
        </motion.div>

        <motion.div 
          className="helpful-links"
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          transition={{ delay: 0.9 }}
        >
          <h3>Helpful Links</h3>
          <div className="links-grid">
            <Link to="/dashboard">Dashboard</Link>
            <Link to="/career">Career Opportunities</Link>
            <Link to="/projectsbank">Projects Bank</Link>
            <Link to="/opportunities">Job Opportunities</Link>
            <Link to="/signin">Sign In</Link>
            <Link to="/signup">Create Account</Link>
          </div>
        </motion.div>
      </div>

      {/* Floating Elements */}
      <div className="floating-elements">
        {[...Array(6)].map((_, i) => (
          <motion.div
            key={i}
            className={`floating-shape shape-${i + 1}`}
            animate={{
              y: [0, -20, 0],
              x: [0, 10, 0],
              rotate: [0, 180, 360]
            }}
            transition={{
              duration: 5 + i,
              repeat: Infinity,
              ease: "easeInOut"
            }}
          />
        ))}
      </div>
    </div>
  );
};

export default NotFound;















