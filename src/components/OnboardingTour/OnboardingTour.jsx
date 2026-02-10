import React, { useState, useEffect } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { FiX, FiArrowRight, FiArrowLeft, FiCheck, FiPlay } from 'react-icons/fi';
import './OnboardingTour.css';

const tourSteps = [
  {
    id: 1,
    title: 'Welcome to GradLink! 🎉',
    description: 'Your all-in-one platform for graduation projects, career opportunities, and professional growth.',
    icon: '👋',
    target: null
  },
  {
    id: 2,
    title: 'Dashboard Overview',
    description: 'Track your progress, view statistics, and stay on top of your tasks all in one place.',
    icon: '📊',
    target: '.content-wrapperr'
  },
  {
    id: 3,
    title: 'Task Management',
    description: 'Create, organize, and track your tasks with our intuitive task manager. Drag and drop to prioritize!',
    icon: '✅',
    target: '[href="/dashboard/tasks"]'
  },
  {
    id: 4,
    title: 'Learning Materials',
    description: 'Access curated resources, tutorials, and templates to boost your skills and projects.',
    icon: '📚',
    target: '[href="/dashboard/materials"]'
  },
  {
    id: 5,
    title: 'Team Collaboration',
    description: 'Build your dream team, assign tasks, and collaborate effectively on projects.',
    icon: '👥',
    target: '[href="/dashboard/teams"]'
  },
  {
    id: 6,
    title: 'Find Mentors',
    description: 'Connect with experienced mentors who can guide you through your career journey.',
    icon: '🎓',
    target: '[href="/dashboard/mentors"]'
  },
  {
    id: 7,
    title: 'Stay Connected',
    description: 'Check your notifications and messages to never miss important updates.',
    icon: '🔔',
    target: '.icon-container'
  },
  {
    id: 8,
    title: "You're All Set! 🎉",
    description: 'You are now ready to explore GradLink and begin your journey. Good luck with your projects!',
    icon: '🚀',
    target: null,
    isFinal: true
  }
];

const OnboardingTour = ({ onComplete }) => {
  const [currentStep, setCurrentStep] = useState(0);
  const [isVisible, setIsVisible] = useState(true);
  const [showWelcome, setShowWelcome] = useState(true);

  useEffect(() => {
    // Check if user has seen the tour
    const hasSeenTour = localStorage.getItem('hasSeenOnboardingTour');
    if (hasSeenTour) {
      setIsVisible(false);
    }
  }, []);

  const handleNext = () => {
    if (currentStep < tourSteps.length - 1) {
      setCurrentStep(prev => prev + 1);
    } else {
      completeTour();
    }
  };

  const handlePrev = () => {
    if (currentStep > 0) {
      setCurrentStep(prev => prev - 1);
    }
  };

  const startTour = () => {
    setShowWelcome(false);
  };

  const completeTour = () => {
    localStorage.setItem('hasSeenOnboardingTour', 'true');
    setIsVisible(false);
    if (onComplete) onComplete();
  };

  const skipTour = () => {
    localStorage.setItem('hasSeenOnboardingTour', 'true');
    setIsVisible(false);
    if (onComplete) onComplete();
  };

  if (!isVisible) return null;

  const step = tourSteps[currentStep];
  const progress = ((currentStep + 1) / tourSteps.length) * 100;

  return (
    <AnimatePresence>
      <div className="onboarding-overlay">
        {showWelcome ? (
          // Welcome Screen with Skip & Start
          <motion.div
            className="onboarding-modal welcome-screen"
            initial={{ opacity: 0, scale: 0.9, y: 20 }}
            animate={{ opacity: 1, scale: 1, y: 0 }}
            exit={{ opacity: 0, scale: 0.9, y: 20 }}
          >
            <div className="welcome-content">
              <motion.div 
                className="welcome-icon"
                animate={{ 
                  scale: [1, 1.1, 1],
                  rotate: [0, 5, -5, 0]
                }}
                transition={{ 
                  duration: 2,
                  repeat: Infinity,
                  repeatType: "reverse"
                }}
              >
                🎓
              </motion.div>
              <h1>Welcome to GradLink!</h1>
              <p>Let us show you around and help you get started with your journey.</p>
              
              <div className="welcome-features">
                <div className="feature-item">
                  <span className="feature-icon">✅</span>
                  <span>Manage Tasks</span>
                </div>
                <div className="feature-item">
                  <span className="feature-icon">👥</span>
                  <span>Build Teams</span>
                </div>
                <div className="feature-item">
                  <span className="feature-icon">🎓</span>
                  <span>Find Mentors</span>
                </div>
                <div className="feature-item">
                  <span className="feature-icon">💼</span>
                  <span>Get Opportunities</span>
                </div>
              </div>
            </div>

            <div className="welcome-actions">
              <button className="skip-tour-btn" onClick={skipTour}>
                Skip Tour
              </button>
              <button className="start-tour-btn" onClick={startTour}>
                <FiPlay /> Start Tour
              </button>
            </div>
          </motion.div>
        ) : (
          // Tour Steps
          <motion.div
            className="onboarding-modal"
            initial={{ opacity: 0, scale: 0.9, y: 20 }}
            animate={{ opacity: 1, scale: 1, y: 0 }}
            exit={{ opacity: 0, scale: 0.9, y: 20 }}
          >
            <button className="skip-btn" onClick={skipTour} title="Skip Tour">
              <FiX />
            </button>

            <div className="tour-progress">
              <div className="progress-bar">
                <motion.div 
                  className="progress-fill"
                  initial={{ width: 0 }}
                  animate={{ width: `${progress}%` }}
                />
              </div>
              <span className="progress-text">{currentStep + 1} / {tourSteps.length}</span>
            </div>

            <motion.div 
              className="tour-content"
              key={step.id}
              initial={{ opacity: 0, x: 20 }}
              animate={{ opacity: 1, x: 0 }}
              exit={{ opacity: 0, x: -20 }}
            >
              <div className="tour-icon">{step.icon}</div>
              <h2>{step.title}</h2>
              <p>{step.description}</p>
              
              {/* Final Step - Start Journey Button */}
              {step.isFinal && (
                <motion.div 
                  className="final-step-actions"
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ delay: 0.3 }}
                >
                  <div className="final-features">
                    <span>🎯 Tasks</span>
                    <span>📚 Materials</span>
                    <span>👥 Teams</span>
                    <span>💼 Career</span>
                  </div>
                  <button className="start-journey-btn" onClick={completeTour}>
                    <FiPlay /> Start My Journey
                  </button>
                </motion.div>
              )}
            </motion.div>

            {/* Navigation - Hide on final step */}
            {!step.isFinal ? (
              <>
                <div className="tour-navigation">
                  <button 
                    className="nav-btn prev"
                    onClick={handlePrev}
                    disabled={currentStep === 0}
                  >
                    <FiArrowLeft /> Back
                  </button>
                  
                  <div className="tour-dots">
                    {tourSteps.map((_, index) => (
                      <button
                        key={index}
                        className={`dot ${index === currentStep ? 'active' : ''} ${index < currentStep ? 'completed' : ''}`}
                        onClick={() => setCurrentStep(index)}
                      />
                    ))}
                  </div>

                  <button 
                    className="nav-btn next"
                    onClick={handleNext}
                  >
                    Next <FiArrowRight />
                  </button>
                </div>

                <button className="skip-text-btn" onClick={skipTour}>
                  Skip Tour
                </button>
              </>
            ) : (
              <div className="final-navigation">
                <button 
                  className="nav-btn prev"
                  onClick={handlePrev}
                >
                  <FiArrowLeft /> Back
                </button>
                <div className="tour-dots">
                  {tourSteps.map((_, index) => (
                    <button
                      key={index}
                      className={`dot ${index === currentStep ? 'active' : ''} ${index < currentStep ? 'completed' : ''}`}
                      onClick={() => setCurrentStep(index)}
                    />
                  ))}
                </div>
              </div>
            )}
          </motion.div>
        )}
      </div>
    </AnimatePresence>
  );
};

export default OnboardingTour;
