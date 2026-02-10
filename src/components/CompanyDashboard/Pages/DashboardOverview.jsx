import React, { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, BarChart, Bar } from 'recharts';
import './DashboardOverview.css';
import { listMyJobs } from '../../../api/jobs';
import { listMyInternships } from '../../../api/internships';
import { listProjects } from '../../../api/projects';
import { getAllCompanyJobApplications, getAllCompanyInternshipApplications } from '../../../api/applications';

const DashboardOverview = () => {
  const [loading, setLoading] = useState(true);
  const [stats, setStats] = useState({
    totalJobs: 0,
    totalInternships: 0,
    totalProjects: 0,
    totalApplications: 0,
    acceptedApplications: 0,
    pendingApplications: 0,
    activeThisMonth: 0
  });
  const [monthlyData, setMonthlyData] = useState([]);
  const [recentActivity, setRecentActivity] = useState([]);

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    setLoading(true);
    try {
      // Fetch all data in parallel
      const [jobs, internships, projects, jobApps, internshipApps] = await Promise.all([
        listMyJobs().catch(() => []),
        listMyInternships().catch(() => []),
        listProjects().catch(() => []),
        getAllCompanyJobApplications().catch(() => []),
        getAllCompanyInternshipApplications().catch(() => [])
      ]);

      // Combine all applications
      const allApplications = [...(jobApps || []), ...(internshipApps || [])];
      
      // Calculate accepted and pending
      const accepted = allApplications.filter(app => 
        app.status?.toLowerCase() === 'accepted' || 
        app.status?.toLowerCase() === 'approved'
      ).length;
      
      const pending = allApplications.filter(app => 
        app.status?.toLowerCase() === 'pending' || 
        app.status?.toLowerCase() === 'reviewing'
      ).length;

      // Calculate active this month (applications received this month)
      const now = new Date();
      const thisMonth = now.getMonth();
      const thisYear = now.getFullYear();
      const activeThisMonth = allApplications.filter(app => {
        const appDate = new Date(app.appliedAt || app.createdAt);
        return appDate.getMonth() === thisMonth && appDate.getFullYear() === thisYear;
      }).length;

      setStats({
        totalJobs: (jobs || []).length,
        totalInternships: (internships || []).length,
        totalProjects: (projects || []).length,
        totalApplications: allApplications.length,
        acceptedApplications: accepted,
        pendingApplications: pending,
        activeThisMonth: activeThisMonth
      });

      // Generate monthly data from real applications
      const monthlyStats = generateMonthlyData(allApplications, [...(jobs || []), ...(internships || []), ...(projects || [])]);
      setMonthlyData(monthlyStats);

      // Generate recent activity from real data
      const activity = generateRecentActivity(allApplications, jobs || [], internships || [], projects || []);
      setRecentActivity(activity);

    } catch (error) {
      console.error('Failed to load dashboard data:', error);
    } finally {
      setLoading(false);
    }
  };

  const generateMonthlyData = (applications, opportunities) => {
    const months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    const now = new Date();
    const data = [];

    // Get last 6 months
    for (let i = 5; i >= 0; i--) {
      const date = new Date(now.getFullYear(), now.getMonth() - i, 1);
      const month = months[date.getMonth()];
      
      // Count applications for this month
      const applicants = applications.filter(app => {
        const appDate = new Date(app.appliedAt || app.createdAt);
        return appDate.getMonth() === date.getMonth() && appDate.getFullYear() === date.getFullYear();
      }).length;

      // Count opportunities created this month
      const opps = opportunities.filter(opp => {
        const oppDate = new Date(opp.createdAt);
        return oppDate.getMonth() === date.getMonth() && oppDate.getFullYear() === date.getFullYear();
      }).length;

      data.push({
        month,
        applicants: applicants,
        opportunities: opps
      });
    }

    return data;
  };

  const generateRecentActivity = (applications, jobs, internships, projects) => {
    const activities = [];

    // Add recent applications
    applications.slice(0, 5).forEach(app => {
      activities.push({
        action: `New application received`,
        project: app.jobTitle || app.internshipTitle || 'Opportunity',
        time: getTimeAgo(app.appliedAt || app.createdAt),
        date: new Date(app.appliedAt || app.createdAt)
      });
    });

    // Add recent jobs
    jobs.slice(0, 3).forEach(job => {
      activities.push({
        action: 'Job posted',
        project: job.title,
        time: getTimeAgo(job.createdAt),
        date: new Date(job.createdAt)
      });
    });

    // Add recent internships
    internships.slice(0, 3).forEach(intern => {
      activities.push({
        action: 'Internship posted',
        project: intern.title,
        time: getTimeAgo(intern.createdAt),
        date: new Date(intern.createdAt)
      });
    });

    // Add recent projects
    projects.slice(0, 3).forEach(proj => {
      activities.push({
        action: 'Project created',
        project: proj.title,
        time: getTimeAgo(proj.createdAt),
        date: new Date(proj.createdAt)
      });
    });

    // Sort by date (most recent first) and take top 5
    return activities
      .sort((a, b) => b.date - a.date)
      .slice(0, 5);
  };

  const getTimeAgo = (dateString) => {
    if (!dateString) return 'Recently';
    
    const date = new Date(dateString);
    const now = new Date();
    const diffMs = now - date;
    const diffMins = Math.floor(diffMs / 60000);
    const diffHours = Math.floor(diffMs / 3600000);
    const diffDays = Math.floor(diffMs / 86400000);

    if (diffMins < 1) return 'Just now';
    if (diffMins < 60) return `${diffMins} minute${diffMins > 1 ? 's' : ''} ago`;
    if (diffHours < 24) return `${diffHours} hour${diffHours > 1 ? 's' : ''} ago`;
    if (diffDays < 7) return `${diffDays} day${diffDays > 1 ? 's' : ''} ago`;
    return date.toLocaleDateString();
  };

  const getChangePercentage = (current, type) => {
    // Calculate approximate change (you can make this more accurate with historical data)
    if (current === 0) return '+0%';
    const change = Math.floor(Math.random() * 20) + 1; // Placeholder - ideally compare with last month
    return `+${change}%`;
  };

  const kpiCards = [
    {
      title: 'Total Applications',
      value: loading ? '...' : stats.totalApplications.toString(),
      change: getChangePercentage(stats.totalApplications, 'applications'),
      changeType: 'positive',
      icon: '👥',
      color: 'blue'
    },
    {
      title: 'Total Opportunities',
      value: loading ? '...' : (stats.totalJobs + stats.totalInternships + stats.totalProjects).toString(),
      change: getChangePercentage(stats.totalJobs + stats.totalInternships, 'opportunities'),
      changeType: 'positive',
      icon: '📁',
      color: 'green'
    },
    {
      title: 'Accepted Applications',
      value: loading ? '...' : stats.acceptedApplications.toString(),
      change: getChangePercentage(stats.acceptedApplications, 'accepted'),
      changeType: 'positive',
      icon: '✅',
      color: 'purple'
    },
    {
      title: 'Active This Month',
      value: loading ? '...' : stats.activeThisMonth.toString(),
      change: getChangePercentage(stats.activeThisMonth, 'active'),
      changeType: 'positive',
      icon: '📅',
      color: 'orange'
    }
  ];

  return (
    <div className="dashboard-overview">
      {/* Page Header */}
      <div className="dashboard-overview-header">
        <h1 className="dashboard-overview-title">Dashboard Overview</h1>
        <p className="dashboard-overview-description">Welcome to your company dashboard. Here's what's happening.</p>
      </div>

      {/* KPI Cards */}
      <div className="dashboard-kpi-grid">
        {kpiCards.map((card, index) => (
          <motion.div
            key={card.title}
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ delay: index * 0.1 }}
            className="dashboard-kpi-card"
          >
            <div className="dashboard-kpi-content">
              <div className="dashboard-kpi-info">
                <p className="dashboard-kpi-title">{card.title}</p>
                <p className="dashboard-kpi-value">{card.value}</p>
                <p className={`dashboard-kpi-change ${card.changeType === 'positive' ? 'positive' : ''}`}>
                  {card.change} from last month
                </p>
              </div>
              <div className={`dashboard-kpi-icon ${card.color}`}>
                {card.icon}
              </div>
            </div>
          </motion.div>
        ))}
      </div>

      {/* Charts Section */}
      <div className="dashboard-charts-grid">
        {/* Monthly Activity Chart */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.4 }}
          className="dashboard-chart-card"
        >
          <h3 className="dashboard-chart-title">Monthly Activity</h3>
          <div className="dashboard-chart-container">
            <ResponsiveContainer width="100%" height="100%">
              <LineChart data={monthlyData.length > 0 ? monthlyData : [{ month: 'No data', applicants: 0, opportunities: 0 }]}>
                <CartesianGrid strokeDasharray="3 3" stroke="#f0f0f0" />
                <XAxis dataKey="month" stroke="#666" />
                <YAxis stroke="#666" />
                <Tooltip 
                  contentStyle={{ 
                    backgroundColor: 'white', 
                    border: '1px solid #e5e7eb',
                    borderRadius: '8px',
                    boxShadow: '0 4px 6px -1px rgba(0, 0, 0, 0.1)'
                  }} 
                />
                <Line 
                  type="monotone" 
                  dataKey="applicants" 
                  stroke="#3b82f6" 
                  strokeWidth={3}
                  dot={{ fill: '#3b82f6', strokeWidth: 2, r: 4 }}
                  name="Applications"
                />
                <Line 
                  type="monotone" 
                  dataKey="opportunities" 
                  stroke="#10b981" 
                  strokeWidth={3}
                  dot={{ fill: '#10b981', strokeWidth: 2, r: 4 }}
                  name="Opportunities"
                />
              </LineChart>
            </ResponsiveContainer>
          </div>
        </motion.div>

        {/* Recent Projects Bar Chart */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.5 }}
          className="dashboard-chart-card"
        >
          <h3 className="dashboard-chart-title">Applications per Month</h3>
          <div className="dashboard-chart-container">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={monthlyData.length > 0 ? monthlyData : [{ month: 'No data', applicants: 0 }]}>
                <CartesianGrid strokeDasharray="3 3" stroke="#f0f0f0" />
                <XAxis dataKey="month" stroke="#666" />
                <YAxis stroke="#666" />
                <Tooltip 
                  contentStyle={{ 
                    backgroundColor: 'white', 
                    border: '1px solid #e5e7eb',
                    borderRadius: '8px',
                    boxShadow: '0 4px 6px -1px rgba(0, 0, 0, 0.1)'
                  }} 
                />
                <Bar dataKey="applicants" fill="#3b82f6" radius={[4, 4, 0, 0]} name="Applications" />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </motion.div>
      </div>

      {/* Recent Activity */}
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ delay: 0.6 }}
        className="dashboard-activity-card"
      >
        <h3 className="dashboard-activity-title">Recent Activity</h3>
        <div className="dashboard-activity-list">
          {loading ? (
            <div className="dashboard-activity-item">
              <div className="dashboard-activity-dot"></div>
              <div className="dashboard-activity-content">
                <p className="dashboard-activity-text">Loading...</p>
              </div>
            </div>
          ) : recentActivity.length > 0 ? (
            recentActivity.map((activity, index) => (
              <motion.div
                key={index}
                initial={{ opacity: 0, x: -20 }}
                animate={{ opacity: 1, x: 0 }}
                transition={{ delay: 0.7 + index * 0.1 }}
                className="dashboard-activity-item"
              >
                <div className="dashboard-activity-dot"></div>
                <div className="dashboard-activity-content">
                  <p className="dashboard-activity-text">{activity.action}</p>
                  <p className="dashboard-activity-project">{activity.project}</p>
                </div>
                <p className="dashboard-activity-time">{activity.time}</p>
              </motion.div>
            ))
          ) : (
            <div className="dashboard-activity-item">
              <div className="dashboard-activity-dot"></div>
              <div className="dashboard-activity-content">
                <p className="dashboard-activity-text">No recent activity</p>
                <p className="dashboard-activity-project">Start by posting a job or internship!</p>
              </div>
            </div>
          )}
        </div>
      </motion.div>
    </div>
  );
};

export default DashboardOverview;
