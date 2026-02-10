import React, { useState, useEffect, useCallback } from 'react';
import { useSearchParams, useNavigate } from 'react-router-dom';
import { motion, AnimatePresence } from 'framer-motion';
import axios from 'axios';
import './MentorTest.css';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

const MentorTest = () => {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const token = searchParams.get('token');
  
  const [stage, setStage] = useState('loading'); // loading, instructions, test, submitting, result
  const [testInfo, setTestInfo] = useState(null);
  const [questions, setQuestions] = useState([]);
  const [currentQuestion, setCurrentQuestion] = useState(0);
  const [answers, setAnswers] = useState({});
  const [timeLeft, setTimeLeft] = useState(0);
  const [result, setResult] = useState(null);
  const [error, setError] = useState('');

  // Verify token on load
  useEffect(() => {
    if (!token) {
      setError('No test token provided');
      setStage('error');
      return;
    }
    verifyToken();
  }, [token]);

  // Timer
  useEffect(() => {
    if (stage !== 'test' || timeLeft <= 0) return;
    
    const timer = setInterval(() => {
      setTimeLeft(prev => {
        if (prev <= 1) {
          handleSubmit(true);
          return 0;
        }
        return prev - 1;
      });
    }, 1000);
    
    return () => clearInterval(timer);
  }, [stage, timeLeft]);

  const verifyToken = async () => {
    try {
      const response = await axios.post(`${API_URL}/mentorapplications/verify-token`, { token });
      
      if (!response.data.isValid) {
        setError(response.data.message);
        setStage('error');
        return;
      }
      
      setTestInfo(response.data);
      setStage('instructions');
    } catch (err) {
      setError('Failed to verify test token. Please try again.');
      setStage('error');
    }
  };

  const startTest = async () => {
    setStage('loading');
    
    try {
      const response = await axios.post(`${API_URL}/mentorapplications/start-test`, { token });
      
      setQuestions(response.data.questions);
      setTimeLeft(response.data.timeLimitMinutes * 60);
      setStage('test');
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to start test');
      setStage('error');
    }
  };

  const selectAnswer = (questionId, answer) => {
    setAnswers(prev => ({
      ...prev,
      [questionId]: answer
    }));
  };

  const handleSubmit = useCallback(async (timeExpired = false) => {
    if (stage === 'submitting') return;
    
    setStage('submitting');
    
    const answersList = Object.entries(answers).map(([questionId, answer]) => ({
      questionId: parseInt(questionId),
      answer
    }));
    
    try {
      const response = await axios.post(`${API_URL}/mentorapplications/submit-test`, {
        token,
        answers: answersList
      });
      
      setResult(response.data);
      setStage('result');
    } catch (err) {
      if (timeExpired) {
        setError('Time expired and test could not be submitted.');
      } else {
        setError(err.response?.data?.message || 'Failed to submit test');
      }
      setStage('error');
    }
  }, [answers, token, stage]);

  const formatTime = (seconds) => {
    const mins = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${mins.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;
  };

  const renderLoading = () => (
    <div className="test-loading">
      <div className="loading-spinner"></div>
      <p>Loading...</p>
    </div>
  );

  const renderError = () => (
    <motion.div 
      className="test-error"
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
    >
      <div className="error-icon">❌</div>
      <h2>Oops!</h2>
      <p>{error}</p>
      <button onClick={() => navigate('/')} className="btn-primary">
        Back to Home
      </button>
    </motion.div>
  );

  const renderInstructions = () => (
    <motion.div 
      className="test-instructions"
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
    >
      <div className="instructions-header">
        <h1>🎓 Mentor Assessment Test</h1>
        <p>Welcome, <strong>{testInfo?.applicantName}</strong>!</p>
      </div>
      
      <div className="instructions-info">
        <div className="info-card">
          <span className="info-icon">📚</span>
          <div>
            <strong>Specialization</strong>
            <p>{testInfo?.specialization}</p>
          </div>
        </div>
        
        <div className="info-card">
          <span className="info-icon">⏱️</span>
          <div>
            <strong>Time Limit</strong>
            <p>{testInfo?.timeLimitMinutes} minutes</p>
          </div>
        </div>
        
        <div className="info-card">
          <span className="info-icon">📝</span>
          <div>
            <strong>Questions</strong>
            <p>{testInfo?.totalQuestions} multiple choice</p>
          </div>
        </div>
        
        <div className="info-card">
          <span className="info-icon">🎯</span>
          <div>
            <strong>Passing Score</strong>
            <p>70% or higher</p>
          </div>
        </div>
      </div>
      
      <div className="instructions-rules">
        <h3>📋 Instructions</h3>
        <ul>
          <li>Read each question carefully before answering</li>
          <li>Select only one answer per question</li>
          <li>You can navigate between questions</li>
          <li>The timer starts when you click "Start Test"</li>
          <li>Test auto-submits when time expires</li>
          <li>Make sure you have a stable internet connection</li>
        </ul>
      </div>
      
      <div className="instructions-warning">
        <p>⚠️ <strong>Warning:</strong> Once you start, you cannot pause the test. Make sure you have {testInfo?.timeLimitMinutes} uninterrupted minutes.</p>
      </div>
      
      <button onClick={startTest} className="btn-start">
        Start Test 🚀
      </button>
    </motion.div>
  );

  const renderTest = () => {
    const question = questions[currentQuestion];
    const answeredCount = Object.keys(answers).length;
    const isWarning = timeLeft <= 60;
    const isCritical = timeLeft <= 30;
    
    return (
      <div className="test-container">
        {/* Header */}
        <div className="test-header">
          <div className="test-progress">
            <span>Question {currentQuestion + 1} of {questions.length}</span>
            <div className="progress-bar">
              <div 
                className="progress-fill"
                style={{ width: `${((currentQuestion + 1) / questions.length) * 100}%` }}
              />
            </div>
          </div>
          
          <div className={`test-timer ${isWarning ? 'warning' : ''} ${isCritical ? 'critical' : ''}`}>
            <span className="timer-icon">⏱️</span>
            <span className="timer-value">{formatTime(timeLeft)}</span>
          </div>
        </div>
        
        {/* Question */}
        <AnimatePresence mode="wait">
          <motion.div
            key={currentQuestion}
            className="question-card"
            initial={{ opacity: 0, x: 20 }}
            animate={{ opacity: 1, x: 0 }}
            exit={{ opacity: 0, x: -20 }}
          >
            <div className="question-meta">
              <span className="question-category">{question.category}</span>
              <span className={`question-difficulty ${question.difficulty.toLowerCase()}`}>
                {question.difficulty}
              </span>
            </div>
            
            <h2 className="question-text">{question.questionText}</h2>
            
            <div className="options-list">
              {['A', 'B', 'C', 'D'].map(option => (
                <motion.button
                  key={option}
                  className={`option-btn ${answers[question.id] === option ? 'selected' : ''}`}
                  onClick={() => selectAnswer(question.id, option)}
                  whileHover={{ scale: 1.01 }}
                  whileTap={{ scale: 0.99 }}
                >
                  <span className="option-letter">{option}</span>
                  <span className="option-text">{question[`option${option}`]}</span>
                </motion.button>
              ))}
            </div>
          </motion.div>
        </AnimatePresence>
        
        {/* Navigation */}
        <div className="test-navigation">
          <button 
            onClick={() => setCurrentQuestion(prev => Math.max(0, prev - 1))}
            disabled={currentQuestion === 0}
            className="nav-btn"
          >
            ← Previous
          </button>
          
          <div className="question-dots">
            {questions.map((q, idx) => (
              <button
                key={idx}
                className={`dot ${idx === currentQuestion ? 'current' : ''} ${answers[q.id] ? 'answered' : ''}`}
                onClick={() => setCurrentQuestion(idx)}
              >
                {idx + 1}
              </button>
            ))}
          </div>
          
          {currentQuestion === questions.length - 1 ? (
            <button 
              onClick={() => handleSubmit(false)}
              className="nav-btn submit-btn"
              disabled={answeredCount < questions.length}
            >
              Submit Test ({answeredCount}/{questions.length})
            </button>
          ) : (
            <button 
              onClick={() => setCurrentQuestion(prev => Math.min(questions.length - 1, prev + 1))}
              className="nav-btn"
            >
              Next →
            </button>
          )}
        </div>
      </div>
    );
  };

  const renderSubmitting = () => (
    <div className="test-loading">
      <div className="loading-spinner"></div>
      <p>Submitting your answers...</p>
    </div>
  );

  const renderResult = () => (
    <motion.div 
      className={`test-result ${result?.passed ? 'passed' : 'failed'}`}
      initial={{ opacity: 0, scale: 0.9 }}
      animate={{ opacity: 1, scale: 1 }}
    >
      <div className="result-icon">
        {result?.passed ? '🎉' : '😔'}
      </div>
      
      <h1>{result?.passed ? 'Congratulations!' : 'Not Quite There'}</h1>
      
      <div className="result-score">
        <div className="score-circle">
          <svg viewBox="0 0 100 100">
            <circle
              className="score-bg"
              cx="50"
              cy="50"
              r="45"
            />
            <circle
              className="score-fill"
              cx="50"
              cy="50"
              r="45"
              strokeDasharray={`${(result?.score / 100) * 283} 283`}
            />
          </svg>
          <div className="score-value">
            <span className="score-number">{result?.score}</span>
            <span className="score-percent">%</span>
          </div>
        </div>
        
        <div className="score-details">
          <p><strong>{result?.correctAnswers}</strong> out of <strong>{result?.totalQuestions}</strong> correct</p>
          <p className="passing-note">Passing score: 70%</p>
        </div>
      </div>
      
      <p className="result-message">{result?.message}</p>
      
      {!result?.passed && result?.retryAllowedAt && (
        <div className="retry-info">
          <p>You can retry after:</p>
          <strong>{new Date(result.retryAllowedAt).toLocaleDateString('en-US', { 
            weekday: 'long', 
            year: 'numeric', 
            month: 'long', 
            day: 'numeric' 
          })}</strong>
        </div>
      )}
      
      <div className="result-actions">
        <button onClick={() => navigate('/')} className="btn-primary">
          Back to Home
        </button>
        {result?.passed && (
          <button onClick={() => navigate('/signin')} className="btn-secondary">
            Go to Login
          </button>
        )}
      </div>
    </motion.div>
  );

  return (
    <div className="mentor-test-page">
      {stage === 'loading' && renderLoading()}
      {stage === 'error' && renderError()}
      {stage === 'instructions' && renderInstructions()}
      {stage === 'test' && renderTest()}
      {stage === 'submitting' && renderSubmitting()}
      {stage === 'result' && renderResult()}
    </div>
  );
};

export default MentorTest;















