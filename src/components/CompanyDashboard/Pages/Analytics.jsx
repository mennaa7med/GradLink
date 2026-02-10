import React from 'react';
import { motion } from 'framer-motion';
import {
  LineChart,
  Line,
  BarChart,
  Bar,
  PieChart,
  Pie,
  Cell,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer
} from 'recharts';
import './Analytics.css';

const Analytics = () => {
  // Sample data for charts
  const applicantsOverTime = [
    { month: 'Jan', applicants: 45, accepted: 12, rejected: 8 },
    { month: 'Feb', applicants: 52, accepted: 15, rejected: 10 },
    { month: 'Mar', applicants: 38, accepted: 8, rejected: 6 },
    { month: 'Apr', applicants: 67, accepted: 20, rejected: 12 },
    { month: 'May', applicants: 73, accepted: 22, rejected: 15 },
    { month: 'Jun', applicants: 58, accepted: 18, rejected: 9 },
  ];

  const acceptanceData = [
    { name: 'Accepted', value: 95, color: '#059669' },
    { name: 'Rejected', value: 60, color: '#dc2626' },
    { name: 'Pending', value: 25, color: '#f59e0b' },
  ];

  const skillsData = [
    { skill: 'React', count: 45 },
    { skill: 'JavaScript', count: 38 },
    { skill: 'Python', count: 32 },
    { skill: 'Node.js', count: 28 },
    { skill: 'CSS', count: 25 },
    { skill: 'SQL', count: 22 },
  ];

  const projectPerformance = [
    { project: 'Mobile App Dev', applicants: 24, accepted: 8, successRate: 33 },
    { project: 'Web Development', applicants: 18, accepted: 6, successRate: 33 },
    { project: 'Data Science', applicants: 12, accepted: 4, successRate: 33 },
    { project: 'UI/UX Design', applicants: 8, accepted: 2, successRate: 25 },
  ];

  const metrics = [
    {
      title: 'Total Applications',
      value: '342',
      change: '+12%',
      changeType: 'positive'
    },
    {
      title: 'Acceptance Rate',
      value: '28%',
      change: '+5%',
      changeType: 'positive'
    },
    {
      title: 'Average Response Time',
      value: '2.3 days',
      change: '-0.5 days',
      changeType: 'positive'
    },
    {
      title: 'Top Skill Match',
      value: 'React',
      change: 'Most Popular',
      changeType: 'neutral'
    }
  ];

  const insights = [
    {
      text: "Your React positions receive 40% more applications than average",
      type: "blue"
    },
    {
      text: "Consider expanding your Python and Data Science opportunities",
      type: "green"
    },
    {
      text: "Mobile development roles show highest success rates",
      type: "purple"
    }
  ];

  return (
    <div className="analytics-page">
      {/* Page Header */}
      <div className="analytics-page-header">
        <h1 className="analytics-page-title">Analytics</h1>
        <p className="analytics-page-description">Insights and performance metrics for your hiring process</p>
      </div>

      {/* Key Metrics */}
      <div className="analytics-metrics-grid">
        {metrics.map((metric, index) => (
          <motion.div
            key={metric.title}
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ delay: index * 0.1 }}
            className="analytics-metric-card"
          >
            <div className="analytics-metric-value">{metric.value}</div>
            <div className="analytics-metric-title">{metric.title}</div>
            <div className={`analytics-metric-change ${metric.changeType === 'positive' ? 'positive' : metric.changeType === 'negative' ? 'negative' : ''}`}>
              {metric.change} from last month
            </div>
          </motion.div>
        ))}
      </div>

      {/* Charts Grid */}
      <div className="analytics-charts-grid">
        {/* Applicants Over Time */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.4 }}
          className="analytics-chart-card"
        >
          <h3 className="analytics-chart-title">Applicants Over Time</h3>
          <div className="analytics-chart-container">
            <ResponsiveContainer width="100%" height="100%">
              <LineChart data={applicantsOverTime}>
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
                />
                <Line 
                  type="monotone" 
                  dataKey="accepted" 
                  stroke="#059669" 
                  strokeWidth={3}
                  dot={{ fill: '#059669', strokeWidth: 2, r: 4 }}
                />
              </LineChart>
            </ResponsiveContainer>
          </div>
          <div className="analytics-chart-legend">
            <div className="analytics-legend-item">
              <div className="analytics-legend-color" style={{ backgroundColor: '#3b82f6' }}></div>
              <span>Total Applicants</span>
            </div>
            <div className="analytics-legend-item">
              <div className="analytics-legend-color" style={{ backgroundColor: '#059669' }}></div>
              <span>Accepted</span>
            </div>
          </div>
        </motion.div>

        {/* Acceptance Rate Pie Chart */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.5 }}
          className="analytics-chart-card"
        >
          <h3 className="analytics-chart-title">Application Status</h3>
          <div className="analytics-chart-container">
            <ResponsiveContainer width="100%" height="100%">
              <PieChart>
                <Pie
                  data={acceptanceData}
                  cx="50%"
                  cy="50%"
                  innerRadius={60}
                  outerRadius={120}
                  paddingAngle={5}
                  dataKey="value"
                >
                  {acceptanceData.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={entry.color} />
                  ))}
                </Pie>
                <Tooltip 
                  contentStyle={{ 
                    backgroundColor: 'white', 
                    border: '1px solid #e5e7eb',
                    borderRadius: '8px',
                    boxShadow: '0 4px 6px -1px rgba(0, 0, 0, 0.1)'
                  }} 
                />
              </PieChart>
            </ResponsiveContainer>
          </div>
          <div className="analytics-chart-legend">
            {acceptanceData.map((item, index) => (
              <div key={index} className="analytics-legend-item">
                <div className="analytics-legend-color" style={{ backgroundColor: item.color }}></div>
                <span>{item.name}: {item.value}</span>
              </div>
            ))}
          </div>
        </motion.div>

        {/* Skills Distribution */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.6 }}
          className="analytics-chart-card"
        >
          <h3 className="analytics-chart-title">Most Common Skills</h3>
          <div className="analytics-chart-container">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={skillsData} layout="horizontal">
                <CartesianGrid strokeDasharray="3 3" stroke="#f0f0f0" />
                <XAxis type="number" stroke="#666" />
                <YAxis dataKey="skill" type="category" stroke="#666" width={80} />
                <Tooltip 
                  contentStyle={{ 
                    backgroundColor: 'white', 
                    border: '1px solid #e5e7eb',
                    borderRadius: '8px',
                    boxShadow: '0 4px 6px -1px rgba(0, 0, 0, 0.1)'
                  }} 
                />
                <Bar dataKey="count" fill="#8b5cf6" radius={[0, 4, 4, 0]} />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </motion.div>

        {/* AI Insights */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.7 }}
          className="analytics-insights-card"
        >
          <h3 className="analytics-insights-title">AI Insights</h3>
          <div className="analytics-insights-list">
            {insights.map((insight, index) => (
              <motion.div
                key={index}
                initial={{ opacity: 0, x: -20 }}
                animate={{ opacity: 1, x: 0 }}
                transition={{ delay: 0.8 + index * 0.1 }}
                className="analytics-insight-item"
              >
                <div className={`analytics-insight-dot ${insight.type}`}></div>
                <p className="analytics-insight-text">
                  <strong>Insight:</strong> {insight.text}
                </p>
              </motion.div>
            ))}
          </div>
        </motion.div>
      </div>

      {/* Project Performance Table */}
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ delay: 0.8 }}
        className="analytics-performance-card"
      >
        <h3 className="analytics-performance-title">Project Performance</h3>
        <div className="overflow-x-auto">
          <table className="analytics-performance-table">
            <thead className="analytics-performance-header">
              <tr>
                <th>Project</th>
                <th>Applicants</th>
                <th>Accepted</th>
                <th>Success Rate</th>
              </tr>
            </thead>
            <tbody>
              {projectPerformance.map((project, index) => (
                <tr key={index} className="analytics-performance-row">
                  <td className="analytics-performance-cell">
                    <div className="analytics-performance-project">{project.project}</div>
                  </td>
                  <td className="analytics-performance-cell">{project.applicants}</td>
                  <td className="analytics-performance-cell">{project.accepted}</td>
                  <td className="analytics-performance-cell">
                    <div className="analytics-success-rate">
                      <div className="analytics-success-bar">
                        <div 
                          className="analytics-success-progress"
                          style={{ width: `${project.successRate}%` }}
                        />
                      </div>
                      <span className="analytics-success-percentage">{project.successRate}%</span>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </motion.div>
    </div>
  );
};

export default Analytics;