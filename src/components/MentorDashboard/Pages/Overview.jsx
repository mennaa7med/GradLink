import React, { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import { 
  FiUsers, 
  FiCalendar, 
  FiTrendingUp, 
  FiMessageSquare,
  FiCheckCircle,
  FiClock,
  FiUserPlus,
  FiFileText,
  FiStar,
  FiAward,
  FiTarget
} from 'react-icons/fi';
import { AreaChart, Area, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';
import './Overview.css';

const Overview = () => {
  // Get logged-in user's name
  const [userName, setUserName] = useState('Mentor');
  
  useEffect(() => {
    const userStr = localStorage.getItem('user');
    if (userStr) {
      try {
        const userData = JSON.parse(userStr);
        if (userData.fullName) {
          setUserName(userData.fullName);
        }
      } catch (e) {
        console.error('Error parsing user data:', e);
      }
    }
  }, []);
  // Mock data for KPIs
  const kpiData = [
    { 
      id: 1, 
      label: 'Total Mentees', 
      value: 12, 
      icon: <FiUsers />, 
      color: 'green',
      trend: '+2 this month',
      trendUp: true
    },
    { 
      id: 2, 
      label: 'Upcoming Sessions', 
      value: 5, 
      icon: <FiCalendar />, 
      color: 'blue',
      trend: 'Next 7 days',
      trendUp: true
    },
    { 
      id: 3, 
      label: 'Avg. Progress', 
      value: '78%', 
      icon: <FiTrendingUp />, 
      color: 'purple',
      trend: '+5% from last month',
      trendUp: true
    },
    { 
      id: 4, 
      label: 'Unread Messages', 
      value: 8, 
      icon: <FiMessageSquare />, 
      color: 'orange',
      trend: '3 new today',
      trendUp: false
    },
  ];

  // Mock data for progress chart
  const progressData = [
    { name: 'Week 1', progress: 45, sessions: 3 },
    { name: 'Week 2', progress: 52, sessions: 4 },
    { name: 'Week 3', progress: 58, sessions: 2 },
    { name: 'Week 4', progress: 65, sessions: 5 },
    { name: 'Week 5', progress: 70, sessions: 3 },
    { name: 'Week 6', progress: 78, sessions: 4 },
  ];

  // Mock recent activities
  const recentActivities = [
    { 
      id: 1, 
      type: 'message', 
      text: 'Ahmed Hassan sent you a message', 
      time: '10 min ago',
      icon: <FiMessageSquare />
    },
    { 
      id: 2, 
      type: 'session', 
      text: 'Session with Sarah Mohamed completed', 
      time: '2 hours ago',
      icon: <FiCheckCircle />
    },
    { 
      id: 3, 
      type: 'mentee', 
      text: 'New mentee request from ahmed ali', 
      time: '5 hours ago',
      icon: <FiUserPlus />
    },
    { 
      id: 4, 
      type: 'task', 
      text: 'Fatma submitted the project proposal', 
      time: 'Yesterday',
      icon: <FiFileText />
    },
    { 
      id: 5, 
      type: 'reminder', 
      text: 'Upcoming session with Mahmoud in 2 hours', 
      time: '2 hours left',
      icon: <FiClock />
    },
  ];

  // Mock upcoming sessions
  const upcomingSessions = [
    { id: 1, student: 'Ahmed Hassan', topic: 'Project Review', date: 'Today, 3:00 PM', type: 'Online' },
    { id: 2, student: 'Sarah Mohamed', topic: 'Career Guidance', date: 'Tomorrow, 10:00 AM', type: 'Online' },
    { id: 3, student: 'ahmed ali', topic: 'Technical Discussion', date: 'Wed, 2:00 PM', type: 'In-person' },
  ];

  return (
    <div className="mentor-overview">
      {/* Enhanced Header with Stats */}
      <motion.div 
        className="overview-header"
        initial={{ opacity: 0, y: -20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5 }}
      >
        <div className="overview-header-content">
          <div className="overview-header-text">
            <h1>Welcome back, {userName}! 👋</h1>
            <p>Here's what's happening with your mentees today.</p>
          </div>
          <div className="overview-header-stats">
            <div className="header-stat">
              <span className="header-stat-value">4.9</span>
              <span className="header-stat-label">Rating</span>
            </div>
            <div className="header-stat">
              <span className="header-stat-value">156</span>
              <span className="header-stat-label">Sessions</span>
            </div>
            <div className="header-stat">
              <span className="header-stat-value">42</span>
              <span className="header-stat-label">Reviews</span>
            </div>
          </div>
        </div>
      </motion.div>

      {/* KPI Cards */}
      <div className="mentor-kpi-grid">
        {kpiData.map((kpi, index) => (
          <motion.div
            key={kpi.id}
            className={`mentor-kpi-card ${kpi.color}`}
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ delay: index * 0.1 }}
          >
            <div className="mentor-kpi-header">
              <div className={`mentor-kpi-icon ${kpi.color}`}>
                {kpi.icon}
              </div>
            </div>
            <div className="mentor-kpi-content">
              <div className="mentor-kpi-value">{kpi.value}</div>
              <div className="mentor-kpi-label">{kpi.label}</div>
              <div className={`mentor-kpi-trend ${kpi.trendUp ? 'up' : 'down'}`}>
                <FiTrendingUp />
                <span>{kpi.trend}</span>
              </div>
            </div>
          </motion.div>
        ))}
      </div>

      {/* Charts and Activity Grid */}
      <div className="overview-grid">
        {/* Progress Chart */}
        <motion.div 
          className="overview-chart"
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.4 }}
        >
          <div className="mentor-card-header">
            <h3 className="mentor-card-title">
              <FiTrendingUp className="icon" />
              Mentee Progress Overview
            </h3>
            <select className="chart-filter">
              <option value="30">Last 30 days</option>
              <option value="60">Last 60 days</option>
              <option value="90">Last 90 days</option>
            </select>
          </div>
          <div className="chart-container">
            <ResponsiveContainer width="100%" height={280}>
              <AreaChart data={progressData}>
                <defs>
                  <linearGradient id="progressGradient" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="0%" stopColor="#10b981" stopOpacity={0.4}/>
                    <stop offset="100%" stopColor="#10b981" stopOpacity={0.05}/>
                  </linearGradient>
                </defs>
                <CartesianGrid strokeDasharray="3 3" stroke="#e2e8f0" vertical={false} />
                <XAxis 
                  dataKey="name" 
                  stroke="#94a3b8" 
                  fontSize={12} 
                  tickLine={false}
                  axisLine={false}
                />
                <YAxis 
                  stroke="#94a3b8" 
                  fontSize={12} 
                  tickLine={false}
                  axisLine={false}
                  tickFormatter={(value) => `${value}%`}
                />
                <Tooltip 
                  contentStyle={{ 
                    background: '#fff', 
                    border: 'none',
                    borderRadius: '12px',
                    boxShadow: '0 10px 40px rgba(0,0,0,0.12)',
                    padding: '12px 16px'
                  }}
                  formatter={(value) => [`${value}%`, 'Progress']}
                  labelStyle={{ color: '#64748b', fontWeight: 500, marginBottom: '4px' }}
                />
                <Area 
                  type="monotone" 
                  dataKey="progress" 
                  stroke="#10b981" 
                  strokeWidth={3}
                  fill="url(#progressGradient)" 
                />
              </AreaChart>
            </ResponsiveContainer>
          </div>
        </motion.div>

        {/* Recent Activity */}
        <motion.div 
          className="overview-activity"
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.5 }}
        >
          <div className="mentor-card-header">
            <h3 className="mentor-card-title">
              <FiClock className="icon" />
              Recent Activity
            </h3>
            <button className="view-all-btn">View All</button>
          </div>
          <div className="activity-list">
            {recentActivities.map((activity, index) => (
              <motion.div 
                key={activity.id}
                className={`activity-item ${activity.type}`}
                initial={{ opacity: 0, x: -20 }}
                animate={{ opacity: 1, x: 0 }}
                transition={{ delay: 0.5 + index * 0.08 }}
              >
                <div className={`activity-icon ${activity.type}`}>
                  {activity.icon}
                </div>
                <div className="activity-content">
                  <p className="activity-text">{activity.text}</p>
                  <span className="activity-time">{activity.time}</span>
                </div>
              </motion.div>
            ))}
          </div>
        </motion.div>
      </div>

      {/* Quick Stats Row */}
      <div className="quick-stats-row">
        <motion.div 
          className="quick-stat-card"
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.55 }}
        >
          <div className="quick-stat-icon success">
            <FiAward />
          </div>
          <div className="quick-stat-content">
            <h4>28</h4>
            <p>Completed Sessions</p>
          </div>
        </motion.div>

        <motion.div 
          className="quick-stat-card"
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.6 }}
        >
          <div className="quick-stat-icon info">
            <FiTarget />
          </div>
          <div className="quick-stat-content">
            <h4>85%</h4>
            <p>Goals Achieved</p>
          </div>
        </motion.div>

        <motion.div 
          className="quick-stat-card"
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.65 }}
        >
          <div className="quick-stat-icon warning">
            <FiStar />
          </div>
          <div className="quick-stat-content">
            <h4>4.9</h4>
            <p>Average Rating</p>
          </div>
        </motion.div>
      </div>

      {/* Upcoming Sessions */}
      <motion.div 
        className="upcoming-sessions-card"
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ delay: 0.7 }}
      >
        <div className="mentor-card-header">
          <h3 className="mentor-card-title">
            <FiCalendar className="icon" />
            Upcoming Sessions
          </h3>
          <button className="mentor-btn mentor-btn-primary">
            <FiCalendar /> Schedule New
          </button>
        </div>
        <div className="sessions-grid">
          {upcomingSessions.map((session, index) => (
            <motion.div 
              key={session.id}
              className="session-card"
              initial={{ opacity: 0, scale: 0.95 }}
              animate={{ opacity: 1, scale: 1 }}
              transition={{ delay: 0.7 + index * 0.1 }}
            >
              <div className="session-avatar">
                {session.student.charAt(0)}
              </div>
              <div className="session-info">
                <h4 className="session-student">{session.student}</h4>
                <p className="session-topic">{session.topic}</p>
                <div className="session-meta">
                  <span className="session-date">
                    <FiCalendar /> {session.date}
                  </span>
                  <span className={`session-type ${session.type.toLowerCase().replace(' ', '-')}`}>
                    {session.type}
                  </span>
                </div>
              </div>
              <button className="session-action-btn">Join</button>
            </motion.div>
          ))}
        </div>
      </motion.div>
    </div>
  );
};

export default Overview;
