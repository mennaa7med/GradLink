import React, { useState, useEffect } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { FiAward, FiStar, FiX, FiLock } from 'react-icons/fi';
import api from '../../api/client';
import './AchievementBadges.css';

const allBadges = [
  // Starter Badges
  { 
    id: 'first_login', 
    name: 'First Steps', 
    description: 'Logged in for the first time', 
    icon: '🎯',
    category: 'starter',
    points: 10
  },
  { 
    id: 'profile_complete', 
    name: 'Identity Revealed', 
    description: 'Completed your profile 100%', 
    icon: '👤',
    category: 'starter',
    points: 20
  },
  { 
    id: 'first_task', 
    name: 'Task Master', 
    description: 'Created your first task', 
    icon: '✅',
    category: 'starter',
    points: 15
  },

  // Achievement Badges
  { 
    id: 'tasks_10', 
    name: 'Getting Things Done', 
    description: 'Completed 10 tasks', 
    icon: '🏆',
    category: 'achievement',
    points: 50
  },
  { 
    id: 'tasks_50', 
    name: 'Productivity Pro', 
    description: 'Completed 50 tasks', 
    icon: '💪',
    category: 'achievement',
    points: 100
  },
  { 
    id: 'first_project', 
    name: 'Project Pioneer', 
    description: 'Started your first project', 
    icon: '🚀',
    category: 'achievement',
    points: 30
  },
  { 
    id: 'project_complete', 
    name: 'Mission Accomplished', 
    description: 'Completed a project', 
    icon: '🎉',
    category: 'achievement',
    points: 75
  },

  // Social Badges
  { 
    id: 'first_team', 
    name: 'Team Player', 
    description: 'Joined your first team', 
    icon: '👥',
    category: 'social',
    points: 25
  },
  { 
    id: 'mentor_connect', 
    name: 'Knowledge Seeker', 
    description: 'Connected with a mentor', 
    icon: '🎓',
    category: 'social',
    points: 40
  },
  { 
    id: 'first_message', 
    name: 'Communicator', 
    description: 'Sent your first message', 
    icon: '💬',
    category: 'social',
    points: 15
  },

  // Learning Badges
  { 
    id: 'materials_5', 
    name: 'Curious Mind', 
    description: 'Viewed 5 learning materials', 
    icon: '📚',
    category: 'learning',
    points: 20
  },
  { 
    id: 'materials_complete', 
    name: 'Scholar', 
    description: 'Completed all materials in a collection', 
    icon: '🎖️',
    category: 'learning',
    points: 60
  },

  // Career Badges
  { 
    id: 'first_application', 
    name: 'Opportunity Seeker', 
    description: 'Applied to your first opportunity', 
    icon: '📝',
    category: 'career',
    points: 25
  },
  { 
    id: 'resume_upload', 
    name: 'Ready to Hire', 
    description: 'Uploaded your resume', 
    icon: '📄',
    category: 'career',
    points: 20
  },

  // Special Badges
  { 
    id: 'early_adopter', 
    name: 'Early Adopter', 
    description: 'Joined in the first month', 
    icon: '⭐',
    category: 'special',
    points: 100
  },
  { 
    id: 'streak_7', 
    name: 'Dedicated', 
    description: '7-day login streak', 
    icon: '🔥',
    category: 'special',
    points: 50
  },
  { 
    id: 'top_contributor', 
    name: 'Top Contributor', 
    description: 'Added valuable resources to the community', 
    icon: '🌟',
    category: 'special',
    points: 150
  }
];

const AchievementBadges = ({ compact = false }) => {
  const [earnedBadges, setEarnedBadges] = useState([]);
  const [selectedBadge, setSelectedBadge] = useState(null);
  const [loading, setLoading] = useState(true);
  const [totalPoints, setTotalPoints] = useState(0);
  const [filter, setFilter] = useState('all');

  useEffect(() => {
    loadBadges();
  }, []);

  const loadBadges = async () => {
    try {
      // Try to get badges from API
      const { data } = await api.get('/api/users/me/badges').catch(() => ({ data: null }));
      
      if (data) {
        setEarnedBadges(data.earnedBadges || []);
        setTotalPoints(data.totalPoints || 0);
      } else {
        // Fallback to localStorage
        const saved = localStorage.getItem('earnedBadges');
        if (saved) {
          const badges = JSON.parse(saved);
          setEarnedBadges(badges);
          const points = badges.reduce((sum, id) => {
            const badge = allBadges.find(b => b.id === id);
            return sum + (badge?.points || 0);
          }, 0);
          setTotalPoints(points);
        } else {
          // Default: first_login badge
          setEarnedBadges(['first_login']);
          localStorage.setItem('earnedBadges', JSON.stringify(['first_login']));
          setTotalPoints(10);
        }
      }
    } catch (error) {
      console.error('Failed to load badges:', error);
    } finally {
      setLoading(false);
    }
  };

  const categories = [
    { id: 'all', name: 'All Badges' },
    { id: 'starter', name: 'Starter' },
    { id: 'achievement', name: 'Achievement' },
    { id: 'social', name: 'Social' },
    { id: 'learning', name: 'Learning' },
    { id: 'career', name: 'Career' },
    { id: 'special', name: 'Special' }
  ];

  const filteredBadges = filter === 'all' 
    ? allBadges 
    : allBadges.filter(b => b.category === filter);

  const earnedCount = earnedBadges.length;
  const totalCount = allBadges.length;
  const progress = (earnedCount / totalCount) * 100;

  if (compact) {
    // Compact view for sidebar/dashboard
    return (
      <div className="badges-compact">
        <div className="badges-summary">
          <FiAward className="summary-icon" />
          <div className="summary-info">
            <span className="badge-count">{earnedCount} / {totalCount}</span>
            <span className="badge-points">{totalPoints} points</span>
          </div>
        </div>
        <div className="recent-badges">
          {earnedBadges.slice(-3).map(id => {
            const badge = allBadges.find(b => b.id === id);
            return badge && (
              <span key={id} className="mini-badge" title={badge.name}>
                {badge.icon}
              </span>
            );
          })}
        </div>
      </div>
    );
  }

  return (
    <div className="achievement-badges">
      <div className="badges-header">
        <div className="header-info">
          <h2><FiAward /> Achievements</h2>
          <p>Earn badges by completing activities on GradLink</p>
        </div>
        <div className="header-stats">
          <div className="stat-box">
            <span className="stat-value">{earnedCount}</span>
            <span className="stat-label">Earned</span>
          </div>
          <div className="stat-box">
            <span className="stat-value">{totalPoints}</span>
            <span className="stat-label">Points</span>
          </div>
          <div className="progress-ring">
            <svg viewBox="0 0 100 100">
              <circle className="ring-bg" cx="50" cy="50" r="45" />
              <circle 
                className="ring-fill" 
                cx="50" cy="50" r="45"
                strokeDasharray={`${progress * 2.83} 283`}
              />
            </svg>
            <span className="ring-text">{Math.round(progress)}%</span>
          </div>
        </div>
      </div>

      <div className="category-tabs">
        {categories.map(cat => (
          <button
            key={cat.id}
            className={`category-tab ${filter === cat.id ? 'active' : ''}`}
            onClick={() => setFilter(cat.id)}
          >
            {cat.name}
          </button>
        ))}
      </div>

      {loading ? (
        <div className="badges-loading">Loading badges...</div>
      ) : (
        <div className="badges-grid">
          {filteredBadges.map(badge => {
            const isEarned = earnedBadges.includes(badge.id);
            return (
              <motion.div
                key={badge.id}
                className={`badge-card ${isEarned ? 'earned' : 'locked'}`}
                onClick={() => setSelectedBadge(badge)}
                whileHover={{ scale: 1.05 }}
                whileTap={{ scale: 0.95 }}
              >
                <div className="badge-icon">
                  {isEarned ? badge.icon : <FiLock />}
                </div>
                <div className="badge-info">
                  <span className="badge-name">{badge.name}</span>
                  <span className="badge-points">+{badge.points} pts</span>
                </div>
                {isEarned && <span className="earned-check">✓</span>}
              </motion.div>
            );
          })}
        </div>
      )}

      {/* Badge Detail Modal */}
      <AnimatePresence>
        {selectedBadge && (
          <motion.div 
            className="badge-modal-overlay"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            onClick={() => setSelectedBadge(null)}
          >
            <motion.div 
              className="badge-modal"
              initial={{ scale: 0.8, opacity: 0 }}
              animate={{ scale: 1, opacity: 1 }}
              exit={{ scale: 0.8, opacity: 0 }}
              onClick={e => e.stopPropagation()}
            >
              <button className="close-modal" onClick={() => setSelectedBadge(null)}>
                <FiX />
              </button>
              <div className={`modal-badge-icon ${earnedBadges.includes(selectedBadge.id) ? 'earned' : ''}`}>
                {selectedBadge.icon}
              </div>
              <h3>{selectedBadge.name}</h3>
              <p className="modal-description">{selectedBadge.description}</p>
              <div className="modal-stats">
                <span className="modal-points">+{selectedBadge.points} points</span>
                <span className="modal-category">{selectedBadge.category}</span>
              </div>
              {earnedBadges.includes(selectedBadge.id) ? (
                <span className="earned-badge-text">🎉 You've earned this badge!</span>
              ) : (
                <span className="locked-badge-text">🔒 Complete the challenge to unlock</span>
              )}
            </motion.div>
          </motion.div>
        )}
      </AnimatePresence>
    </div>
  );
};

export default AchievementBadges;















