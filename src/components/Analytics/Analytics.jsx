import React, { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import { FiTrendingUp, FiTrendingDown, FiActivity, FiUsers, FiFolder, FiCheckCircle, FiClock, FiCalendar } from 'react-icons/fi';
import {
  LineChart, Line, BarChart, Bar, PieChart, Pie, Cell,
  XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer, AreaChart, Area
} from 'recharts';
import api from '../../api/client';
import './Analytics.css';

const Analytics = () => {
  const [period, setPeriod] = useState('week');
  const [loading, setLoading] = useState(true);
  const [stats, setStats] = useState({
    overview: {},
    taskTrend: [],
    projectStatus: [],
    activityByDay: [],
    productivity: []
  });

  useEffect(() => {
    loadAnalytics();
  }, [period]);

  const loadAnalytics = async () => {
    setLoading(true);
    try {
      // Load tasks for analytics
      const { data: tasks } = await api.get('/api/tasks');
      const { data: projects } = await api.get('/api/projects');

      // Calculate overview stats
      const completedTasks = tasks.filter(t => t.status === 'Completed').length;
      const pendingTasks = tasks.filter(t => t.status !== 'Completed' && t.status !== 'Cancelled').length;
      const totalProjects = projects.length;
      const activeProjects = projects.filter(p => p.status === 'In Progress').length;

      // Calculate task trend (last 7 days)
      const taskTrend = calculateTaskTrend(tasks);
      
      // Project status distribution
      const projectStatus = [
        { name: 'Planning', value: projects.filter(p => p.status === 'Planning').length, color: '#6b7280' },
        { name: 'In Progress', value: projects.filter(p => p.status === 'In Progress').length, color: '#3b82f6' },
        { name: 'Testing', value: projects.filter(p => p.status === 'Testing').length, color: '#f59e0b' },
        { name: 'Completed', value: projects.filter(p => p.status === 'Completed').length, color: '#10b981' }
      ].filter(s => s.value > 0);

      // Activity by day of week
      const activityByDay = calculateActivityByDay(tasks);

      // Productivity score
      const productivity = calculateProductivity(tasks);

      setStats({
        overview: {
          totalTasks: tasks.length,
          completedTasks,
          pendingTasks,
          completionRate: tasks.length ? Math.round((completedTasks / tasks.length) * 100) : 0,
          totalProjects,
          activeProjects
        },
        taskTrend,
        projectStatus,
        activityByDay,
        productivity
      });
    } catch (error) {
      console.error('Failed to load analytics:', error);
    } finally {
      setLoading(false);
    }
  };

  const calculateTaskTrend = (tasks) => {
    const days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
    const today = new Date();
    const trend = [];

    for (let i = 6; i >= 0; i--) {
      const date = new Date(today);
      date.setDate(date.getDate() - i);
      const dayStart = new Date(date.setHours(0, 0, 0, 0));
      const dayEnd = new Date(date.setHours(23, 59, 59, 999));

      const created = tasks.filter(t => {
        const createdAt = new Date(t.createdAt);
        return createdAt >= dayStart && createdAt <= dayEnd;
      }).length;

      const completed = tasks.filter(t => {
        if (t.status !== 'Completed' || !t.updatedAt) return false;
        const completedAt = new Date(t.updatedAt);
        return completedAt >= dayStart && completedAt <= dayEnd;
      }).length;

      trend.push({
        day: days[new Date(dayStart).getDay()],
        created,
        completed
      });
    }

    return trend;
  };

  const calculateActivityByDay = (tasks) => {
    const days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    const activity = days.map(day => ({ day: day.substring(0, 3), tasks: 0 }));

    tasks.forEach(task => {
      const createdAt = new Date(task.createdAt);
      const dayIndex = createdAt.getDay();
      activity[dayIndex].tasks++;
    });

    return activity;
  };

  const calculateProductivity = (tasks) => {
    const weeks = [];
    const today = new Date();

    for (let i = 3; i >= 0; i--) {
      const weekStart = new Date(today);
      weekStart.setDate(weekStart.getDate() - (i * 7) - 6);
      const weekEnd = new Date(weekStart);
      weekEnd.setDate(weekEnd.getDate() + 6);

      const weekTasks = tasks.filter(t => {
        const createdAt = new Date(t.createdAt);
        return createdAt >= weekStart && createdAt <= weekEnd;
      });

      const completed = weekTasks.filter(t => t.status === 'Completed').length;
      const total = weekTasks.length;

      weeks.push({
        week: `Week ${4 - i}`,
        productivity: total ? Math.round((completed / total) * 100) : 0,
        tasks: total
      });
    }

    return weeks;
  };

  const COLORS = ['#6b7280', '#3b82f6', '#f59e0b', '#10b981'];

  if (loading) {
    return (
      <div className="analytics-loading">
        <div className="spinner"></div>
        <p>Loading analytics...</p>
      </div>
    );
  }

  return (
    <div className="analytics-container">
      <div className="analytics-header">
        <div>
          <h1>Analytics Dashboard</h1>
          <p>Track your performance and productivity</p>
        </div>
        <div className="period-selector">
          <button className={period === 'week' ? 'active' : ''} onClick={() => setPeriod('week')}>Week</button>
          <button className={period === 'month' ? 'active' : ''} onClick={() => setPeriod('month')}>Month</button>
          <button className={period === 'year' ? 'active' : ''} onClick={() => setPeriod('year')}>Year</button>
        </div>
      </div>

      {/* Overview Cards */}
      <div className="stats-overview">
        <motion.div className="stat-card" initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }}>
          <div className="stat-icon tasks"><FiCheckCircle /></div>
          <div className="stat-details">
            <span className="stat-value">{stats.overview.completedTasks}</span>
            <span className="stat-label">Completed Tasks</span>
          </div>
          <span className={`stat-trend ${stats.overview.completionRate > 50 ? 'up' : 'down'}`}>
            {stats.overview.completionRate > 50 ? <FiTrendingUp /> : <FiTrendingDown />}
            {stats.overview.completionRate}%
          </span>
        </motion.div>

        <motion.div className="stat-card" initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: 0.1 }}>
          <div className="stat-icon pending"><FiClock /></div>
          <div className="stat-details">
            <span className="stat-value">{stats.overview.pendingTasks}</span>
            <span className="stat-label">Pending Tasks</span>
          </div>
        </motion.div>

        <motion.div className="stat-card" initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: 0.2 }}>
          <div className="stat-icon projects"><FiFolder /></div>
          <div className="stat-details">
            <span className="stat-value">{stats.overview.activeProjects}</span>
            <span className="stat-label">Active Projects</span>
          </div>
        </motion.div>

        <motion.div className="stat-card" initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: 0.3 }}>
          <div className="stat-icon activity"><FiActivity /></div>
          <div className="stat-details">
            <span className="stat-value">{stats.overview.totalTasks}</span>
            <span className="stat-label">Total Tasks</span>
          </div>
        </motion.div>
      </div>

      {/* Charts */}
      <div className="charts-grid">
        {/* Task Trend Chart */}
        <motion.div className="chart-card" initial={{ opacity: 0 }} animate={{ opacity: 1 }} transition={{ delay: 0.4 }}>
          <h3>Task Trend (Last 7 Days)</h3>
          <ResponsiveContainer width="100%" height={250}>
            <AreaChart data={stats.taskTrend}>
              <CartesianGrid strokeDasharray="3 3" stroke="#eee" />
              <XAxis dataKey="day" stroke="#888" />
              <YAxis stroke="#888" />
              <Tooltip />
              <Legend />
              <Area type="monotone" dataKey="created" stroke="#3b82f6" fill="#3b82f6" fillOpacity={0.2} name="Created" />
              <Area type="monotone" dataKey="completed" stroke="#10b981" fill="#10b981" fillOpacity={0.2} name="Completed" />
            </AreaChart>
          </ResponsiveContainer>
        </motion.div>

        {/* Project Status Pie Chart */}
        <motion.div className="chart-card" initial={{ opacity: 0 }} animate={{ opacity: 1 }} transition={{ delay: 0.5 }}>
          <h3>Project Status</h3>
          <ResponsiveContainer width="100%" height={250}>
            <PieChart>
              <Pie
                data={stats.projectStatus}
                cx="50%"
                cy="50%"
                innerRadius={60}
                outerRadius={80}
                paddingAngle={5}
                dataKey="value"
                label={({ name, value }) => `${name}: ${value}`}
              >
                {stats.projectStatus.map((entry, index) => (
                  <Cell key={`cell-${index}`} fill={entry.color} />
                ))}
              </Pie>
              <Tooltip />
            </PieChart>
          </ResponsiveContainer>
        </motion.div>

        {/* Activity by Day Bar Chart */}
        <motion.div className="chart-card" initial={{ opacity: 0 }} animate={{ opacity: 1 }} transition={{ delay: 0.6 }}>
          <h3>Activity by Day</h3>
          <ResponsiveContainer width="100%" height={250}>
            <BarChart data={stats.activityByDay}>
              <CartesianGrid strokeDasharray="3 3" stroke="#eee" />
              <XAxis dataKey="day" stroke="#888" />
              <YAxis stroke="#888" />
              <Tooltip />
              <Bar dataKey="tasks" fill="#0c2737" radius={[4, 4, 0, 0]} />
            </BarChart>
          </ResponsiveContainer>
        </motion.div>

        {/* Productivity Trend */}
        <motion.div className="chart-card" initial={{ opacity: 0 }} animate={{ opacity: 1 }} transition={{ delay: 0.7 }}>
          <h3>Weekly Productivity</h3>
          <ResponsiveContainer width="100%" height={250}>
            <LineChart data={stats.productivity}>
              <CartesianGrid strokeDasharray="3 3" stroke="#eee" />
              <XAxis dataKey="week" stroke="#888" />
              <YAxis stroke="#888" unit="%" />
              <Tooltip />
              <Line type="monotone" dataKey="productivity" stroke="#0c2737" strokeWidth={2} dot={{ r: 4 }} />
            </LineChart>
          </ResponsiveContainer>
        </motion.div>
      </div>
    </div>
  );
};

export default Analytics;















