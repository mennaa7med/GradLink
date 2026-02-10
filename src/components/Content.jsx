import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './Content.css';
import { FiArrowUpRight, FiPlus, FiPause, FiSquare, FiVideo, FiPlay, FiUsers } from 'react-icons/fi';
import { LineChart, Line, XAxis, YAxis, Tooltip, ResponsiveContainer, PieChart, Pie, Cell } from 'recharts';
import api from '../api/client';
import { SkeletonStats, SkeletonListItem } from './Skeleton/Skeleton';
import OnboardingTour from './OnboardingTour/OnboardingTour';

const Content = () => {
  const [loading, setLoading] = useState(true);
  const [stats, setStats] = useState({
    totalProjects: 0,
    completedProjects: 0,
    runningProjects: 0,
    pendingProjects: 0
  });
  const [tasks, setTasks] = useState([]);
  const [teamMembers, setTeamMembers] = useState([]);
  const [chartData, setChartData] = useState([]);
  const [timer, setTimer] = useState({ hours: 0, minutes: 0, seconds: 0, isRunning: false });
  const [showTour, setShowTour] = useState(false);
  const [showMeetingOptions, setShowMeetingOptions] = useState(false);

  useEffect(() => {
    loadDashboardData();
    checkOnboarding();
    
    // Listen for task updates to refresh Recent Tasks
    const handleTaskUpdate = () => {
      loadDashboardData();
    };
    
    // Listen for team member updates
    const handleTeamUpdate = () => {
      loadDashboardData();
    };
    
    window.addEventListener('taskUpdated', handleTaskUpdate);
    window.addEventListener('teamMemberUpdated', handleTeamUpdate);
    
    return () => {
      window.removeEventListener('taskUpdated', handleTaskUpdate);
      window.removeEventListener('teamMemberUpdated', handleTeamUpdate);
    };
  }, []);

  // Timer effect
  useEffect(() => {
    let interval;
    if (timer.isRunning) {
      interval = setInterval(() => {
        setTimer(prev => {
          let { hours, minutes, seconds } = prev;
          seconds++;
          if (seconds >= 60) {
            seconds = 0;
            minutes++;
          }
          if (minutes >= 60) {
            minutes = 0;
            hours++;
          }
          return { ...prev, hours, minutes, seconds };
        });
      }, 1000);
    }
    return () => clearInterval(interval);
  }, [timer.isRunning]);

  const checkOnboarding = () => {
    const hasSeenTour = localStorage.getItem('hasSeenOnboardingTour');
    if (!hasSeenTour) {
      setShowTour(true);
    }
  };

  const loadDashboardData = async () => {
    try {
      // Load tasks stats
      const tasksRes = await api.get('/api/tasks').catch(() => ({ data: [] }));

      const allTasks = tasksRes.data || [];

      // Calculate stats from tasks
      const completed = allTasks.filter(t => t.status === 'Completed' || t.status === 'Done').length;
      const inProgress = allTasks.filter(t => t.status === 'InProgress' || t.status === 'In Progress').length;
      const pending = allTasks.filter(t => t.status === 'Pending' || t.status === 'Todo').length;
      const total = allTasks.length;

      setStats({
        totalProjects: total,
        completedProjects: completed,
        runningProjects: inProgress,
        pendingProjects: pending
      });

      // Set recent tasks for projects section
      setTasks(allTasks.slice(0, 8));

      // Load team members from localStorage (shared with Teams component)
      const savedMembers = localStorage.getItem('dashboardTeamMembers');
      if (savedMembers) {
        try {
          const localMembers = JSON.parse(savedMembers);
          setTeamMembers(localMembers.slice(0, 6));
        } catch (e) {
          console.error('Failed to parse saved members:', e);
          setTeamMembers([]);
        }
      } else {
        setTeamMembers([]);
      }

      // Generate chart data based on real data or month progression
      const months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
      const currentMonth = new Date().getMonth();
      const generatedChartData = months.map((month, index) => {
        const factor = index <= currentMonth ? (index + 1) / (currentMonth + 1) : 0;
        return {
          name: month,
          completed: Math.round(completed * factor * (0.8 + Math.random() * 0.4)),
          inProgress: Math.round(inProgress * factor * (0.8 + Math.random() * 0.4))
        };
      });
      setChartData(generatedChartData);

    } catch (error) {
      console.error('Failed to load dashboard data:', error);
    } finally {
      setLoading(false);
    }
  };

  const pieData = [
    { name: 'Completed', value: stats.completedProjects || 1, color: '#0c2737' },
    { name: 'In Progress', value: stats.runningProjects || 1, color: '#6c8eaf' },
    { name: 'Pending', value: stats.pendingProjects || 1, color: '#f0ad4e' },
  ];

  const completionPercentage = stats.totalProjects > 0 
    ? Math.round((stats.completedProjects / stats.totalProjects) * 100) 
    : 0;

  const formatTime = (num) => num.toString().padStart(2, '0');

  const getTaskIcon = (name) => {
    const lower = name?.toLowerCase() || '';
    if (lower.includes('react') || lower.includes('frontend')) return { icon: 'R', class: 'react' };
    if (lower.includes('css') || lower.includes('style')) return { icon: 'CSS', class: 'css' };
    if (lower.includes('js') || lower.includes('javascript')) return { icon: 'JS', class: 'js' };
    if (lower.includes('design') || lower.includes('ui')) return { icon: 'Ps', class: 'ps' };
    if (lower.includes('api') || lower.includes('backend')) return { icon: 'C#', class: 'c' };
    return { icon: '📋', class: 'default' };
  };

  return (
    <div className="content-wrapperr">
      {/* Onboarding Tour */}
      {showTour && <OnboardingTour onComplete={() => setShowTour(false)} />}

      {/* Header */}
      <div className="header">
        <div className="title-section">
          <h1 className="content-title">Dashboard</h1>
          <p className="content-subtitle">plan, prioritize, and accomplish your tasks with ease.</p>
        </div>
        <div className="button-group">
          <Link to="/dashboard/tasks" className="import-bbtn">View All Tasks</Link>
        </div>
      </div>

      {/* Stats Row */}
      {loading ? (
        <SkeletonStats count={4} />
      ) : (
        <div className="stats-row">
          <Link to="/dashboard/tasks" className="stat-card primary">
            <h3>Total Tasks</h3>
            <div className="stat-value">{stats.totalProjects}</div>
            <div className="stat-link">view all tasks</div>
            <div className="arrow-icon">
              <FiArrowUpRight />
            </div>
          </Link>
          <Link to="/dashboard/tasks?status=completed" className="stat-card">
            <h3>Completed</h3>
            <div className="stat-value">{stats.completedProjects}</div>
            <div className="stat-link">view completed</div>
            <div className="arrow-icon">
              <FiArrowUpRight />
            </div>
          </Link>
          <Link to="/dashboard/tasks?status=in-progress" className="stat-card">
            <h3>In Progress</h3>
            <div className="stat-value">{stats.runningProjects}</div>
            <div className="stat-link">view in progress</div>
            <div className="arrow-icon">
              <FiArrowUpRight />
            </div>
          </Link>
          <Link to="/dashboard/tasks?status=pending" className="stat-card">
            <h3>Pending</h3>
            <div className="stat-value">{stats.pendingProjects}</div>
            <div className="stat-link">view pending</div>
            <div className="arrow-icon">
              <FiArrowUpRight />
            </div>
          </Link>
        </div>
      )}

      {/* First Row: Analytics, Reminders, Projects */}
      <div className="dashboard-row">
        <div className="flex-item analytics">
          <h3>Task Analytics</h3>
          <div className="chart-container">
            <div className="chart-label-top">Tasks completed over time</div>
            <ResponsiveContainer width="100%" height={200}>
              <LineChart data={chartData} margin={{ top: 5, right: 20, left: 0, bottom: 5 }}>
                <XAxis dataKey="name" tick={{ fontSize: 10 }} axisLine={false} tickLine={false} />
                <YAxis hide={true} />
                <Tooltip />
                <Line type="monotone" dataKey="completed" stroke="#0c2737" strokeWidth={2} dot={false} name="Completed" />
                <Line type="monotone" dataKey="inProgress" stroke="#f0ad4e" strokeWidth={2} dot={false} name="In Progress" />
              </LineChart>
            </ResponsiveContainer>
          </div>
        </div>

        <div className="flex-item reminders">
          <h3>Team Workspace</h3>
          <div className="meeting-info">
            <div className="workspace-icon">
              <FiUsers />
            </div>
            <h4>Start Team Meeting</h4>
            <p className="meeting-time">Collaborate with your team in real-time</p>
            
            {!showMeetingOptions ? (
              <button className="meeting-btn" onClick={() => setShowMeetingOptions(true)}>
                <FiVideo className="camera-icon" /> Join Meeting
              </button>
            ) : (
              <div className="meeting-options">
                <a 
                  href="https://meet.google.com/new" 
                  target="_blank" 
                  rel="noopener noreferrer"
                  className="meeting-option google-meet"
                >
                  <img src="https://fonts.gstatic.com/s/i/productlogos/meet_2020q4/v1/web-96dp/logo_meet_2020q4_color_2x_web_96dp.png" alt="Google Meet" />
                  <span>Google Meet</span>
                </a>
                <a 
                  href="https://teams.microsoft.com/l/meetup-join/new" 
                  target="_blank" 
                  rel="noopener noreferrer"
                  className="meeting-option ms-teams"
                >
                  <img src="https://upload.wikimedia.org/wikipedia/commons/c/c9/Microsoft_Office_Teams_%282018%E2%80%93present%29.svg" alt="Microsoft Teams" />
                  <span>MS Teams</span>
                </a>
                <a 
                  href="https://zoom.us/start/videomeeting" 
                  target="_blank" 
                  rel="noopener noreferrer"
                  className="meeting-option zoom"
                >
                  <img src="https://upload.wikimedia.org/wikipedia/commons/7/7b/Zoom_Communications_Logo.svg" alt="Zoom" />
                  <span>Zoom</span>
                </a>
                <button className="back-btn" onClick={() => setShowMeetingOptions(false)}>
                  ← Back
                </button>
              </div>
            )}
          </div>
        </div>

        <div className="flex-item projects">
          <div className="projects-header">
            <h3>Recent Tasks</h3>
            <Link to="/dashboard/tasks" className="new-btn">
              <FiPlus /> New
            </Link>
          </div>
          <div className="project-items">
            {loading ? (
              [...Array(5)].map((_, i) => <SkeletonListItem key={i} />)
            ) : tasks.length > 0 ? (
              tasks.map(task => {
                const iconData = getTaskIcon(task.name || task.title);
                return (
                  <div key={task.id} className="project-item">
                    <div className={`tech-icon ${iconData.class}`}>{iconData.icon}</div>
                    <div className="project-details">
                      <p className="project-name">{task.name || task.title}</p>
                      <p className="due-date">
                        {task.dueDate ? `Due: ${new Date(task.dueDate).toLocaleDateString()}` : 'No due date'}
                      </p>
                    </div>
                  </div>
                );
              })
            ) : (
              <div className="empty-state">
                <p>No tasks yet. Create your first task!</p>
                <Link to="/dashboard/tasks" className="create-task-btn">Create Task</Link>
              </div>
            )}
          </div>
        </div>
      </div>

      {/* Second Row: Teams, Progress Chart, Time Tracker */}
      <div className="dashboard-row">
        <div className="flex-item teams">
          <div className="teams-header">
            <h3>Team Members</h3>
            <Link to="/dashboard/teams" className="add-member-btn">
              <FiPlus /> Add member
            </Link>
          </div>
          <div className="team-members">
            {loading ? (
              [...Array(4)].map((_, i) => <SkeletonListItem key={i} />)
            ) : teamMembers.length > 0 ? (
              teamMembers.map((member, index) => (
                <div key={member.id || index} className="team-member">
                  <img 
                    src={member.profileImageUrl || member.avatar || member.profilePicture || `https://ui-avatars.com/api/?name=${encodeURIComponent(member.fullName || member.name || 'User')}&background=random`}
                    alt={member.fullName || member.name}
                    className="member-avatar" 
                  />
                  <div className="member-info">
                    <p className="member-name">{member.fullName || member.name}</p>
                    <p className="member-role">{member.role || 'Team Member'}</p>
                  </div>
                  <div className={`status-badge ${(member.availability || member.status || 'active').toLowerCase().replace(' ', '-')}`}>
                    {member.availability || member.status || 'Active'}
                  </div>
                </div>
              ))
            ) : (
              <div className="empty-state">
                <p>No team members yet.</p>
                <Link to="/dashboard/teams">Add Team Members</Link>
              </div>
            )}
          </div>
        </div>

        <div className="flex-item progress-chart">
          <h3>Task Progress</h3>
          <div style={{ position: 'relative', height: '200px' }}>
            <ResponsiveContainer width="100%" height="100%">
              <PieChart>
                <Pie
                  data={pieData}
                  cx="50%"
                  cy="50%"
                  innerRadius={60}
                  outerRadius={80}
                  dataKey="value"
                  startAngle={90}
                  endAngle={-260}
                >
                  {pieData.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={entry.color} />
                  ))}
                </Pie>
              </PieChart>
            </ResponsiveContainer>
            <div className="chart-center">
              <p className="percentage">{completionPercentage}%</p>
              <p className="chart-label">Completed</p>
            </div>
          </div>
          <div className="chart-legend">
            <div className="legend-item">
              <div className="dot completed"></div>
              <span>Completed ({stats.completedProjects})</span>
            </div>
            <div className="legend-item">
              <div className="dot in-progress"></div>
              <span>In Progress ({stats.runningProjects})</span>
            </div>
            <div className="legend-item">
              <div className="dot pending"></div>
              <span>Pending ({stats.pendingProjects})</span>
            </div>
          </div>
        </div>

        <div className="flex-item time-tracker">
          <h3>Time Tracker</h3>
          <div className="timer">
            {formatTime(timer.hours)}:{formatTime(timer.minutes)}:{formatTime(timer.seconds)}
          </div>
          <div className="timer-controls">
            <button 
              className="timer-btn"
              onClick={() => setTimer(prev => ({ ...prev, isRunning: !prev.isRunning }))}
            >
              {timer.isRunning ? <FiPause /> : <FiPlay />}
            </button>
            <button 
              className="timer-btn"
              onClick={() => setTimer({ hours: 0, minutes: 0, seconds: 0, isRunning: false })}
            >
              <FiSquare />
            </button>
          </div>
          <p className="timer-hint">Track time spent on your tasks</p>
        </div>
      </div>
    </div>
  );
};

export default Content;
