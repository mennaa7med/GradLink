import React from 'react';
import './Content.css';
import { FiArrowUpRight, FiPlus, FiPause, FiSquare, FiVideo } from 'react-icons/fi';
import { LineChart, Line, XAxis, YAxis, Tooltip, ResponsiveContainer, PieChart, Pie, Cell } from 'recharts';

const Content = () => {
  const lineChartData = [
    { name: 'Jan', completed: 4, inProgress: 6 },
    { name: 'Feb', completed: 3, inProgress: 7 },
    { name: 'Mar', completed: 5, inProgress: 5 },
    { name: 'Apr', completed: 7, inProgress: 4 },
    { name: 'May', completed: 6, inProgress: 5 },
    { name: 'Jun', completed: 8, inProgress: 6 },
    { name: 'Jul', completed: 9, inProgress: 7 },
    { name: 'Aug', completed: 8, inProgress: 8 },
    { name: 'Sep', completed: 10, inProgress: 7 },
    { name: 'Oct', completed: 11, inProgress: 5 },
    { name: 'Nov', completed: 12, inProgress: 6 },
    { name: 'Dec', completed: 14, inProgress: 8 },
  ];

  const pieData = [
    { name: 'Completed', value: 55, color: '#0c2737' },
    { name: 'In Progress', value: 25, color: '#6c8eaf' },
    { name: 'Pending', value: 20, color: '#f0ad4e' },
  ];

  return (
    <div className="content-wrapperr">
      {/* Header */}
      <div className="header">
        <div className="title-section">
          <h1 className="content-title">Dashboard</h1>
          <p className="content-subtitle">plan, prioritize, and accomplish your tasks with ease.</p>
        </div>
        <div className="button-group">
          
          <button className="import-bbtn">Import Data</button>
        </div>
      </div>

      {/* Stats Row */}
      <div className="stats-row">
        <div className="stat-card primary">
          <h3>Total Projects</h3>
          <div className="stat-value">25</div>
          <div className="stat-link">show your Projects</div>
          <div className="arrow-icon">
            <FiArrowUpRight />
          </div>
        </div>
        <div className="stat-card">
          <h3>Ended Projects</h3>
          <div className="stat-value">10</div>
          <div className="stat-link">show your Projects</div>
          <div className="arrow-icon">
            <FiArrowUpRight />
          </div>
        </div>
        <div className="stat-card">
          <h3>Running Projects</h3>
          <div className="stat-value">12</div>
          <div className="stat-link">show your Projects</div>
          <div className="arrow-icon">
            <FiArrowUpRight />
          </div>
        </div>
        <div className="stat-card">
          <h3>Pending Projects</h3>
          <div className="stat-value">2</div>
          <div className="stat-link">show your Projects</div>
          <div className="arrow-icon">
            <FiArrowUpRight />
          </div>
        </div>
      </div>

      {/* First Row: Analytics, Reminders, Projects */}
      <div className="dashboard-row">
        <div className="flex-item analytics">
          <h3>Project analytics</h3>
          <div className="chart-container">
            <div className="chart-label-top">Projects completed</div>
            <ResponsiveContainer width="100%" height={200}>
              <LineChart data={lineChartData} margin={{ top: 5, right: 20, left: 0, bottom: 5 }}>
                <XAxis dataKey="name" tick={{ fontSize: 10 }} axisLine={false} tickLine={false} />
                <YAxis hide={true} />
                <Tooltip />
                <Line type="monotone" dataKey="completed" stroke="#0c2737" strokeWidth={2} dot={false} />
                <Line type="monotone" dataKey="inProgress" stroke="#f0ad4e" strokeWidth={2} dot={false} />
              </LineChart>
            </ResponsiveContainer>
          </div>
        </div>

        <div className="flex-item reminders">
          <h3>Reminders</h3>
          <div className="meeting-info">
            <h4>Meeting with your team</h4>
            <p className="meeting-time">Time: 02:34 PM-04:00 PM</p>
            <button className="meeting-btn">
              <FiVideo className="camera-icon" /> Start Meeting
            </button>
          </div>
        </div>

        <div className="flex-item projects">
          <div className="projects-header">
            <h3>Projects</h3>
            <button className="new-btn">
              <FiPlus /> New
            </button>
          </div>
          <div className="project-items">
            <div className="project-item">
              <div className="tech-icon c">C</div>
              <div className="project-details">
                <p className="project-name">build Dashboard</p>
                <p className="due-date">Due Date: Nov 26, 2025</p>
              </div>
            </div>
            <div className="project-item">
              <div className="tech-icon ps">Ps</div>
              <div className="project-details">
                <p className="project-name">build Dashboard</p>
                <p className="due-date">Due Date: Nov 26, 2025</p>
              </div>
            </div>
            <div className="project-item">
              <div className="tech-icon js">JS</div>
              <div className="project-details">
                <p className="project-name">build Dashboard</p>
                <p className="due-date">Due Date: Nov 26, 2025</p>
              </div>
            </div>
            <div className="project-item">
              <div className="tech-icon css">CSS</div>
              <div className="project-details">
                <p className="project-name">build Dashboard</p>
                <p className="due-date">Due Date: Nov 26, 2025</p>
              </div>
            </div>
            <div className="project-item">
              <div className="tech-icon react">R</div>
              <div className="project-details">
                <p className="project-name">build Dashboard</p>
                <p className="due-date">Due Date: Nov 26, 2025</p>
              </div>
            </div>
             <div className="project-item">
              <div className="tech-icon react">R</div>
              <div className="project-details">
                <p className="project-name">build Dashboard</p>
                <p className="due-date">Due Date: Nov 26, 2025</p>
              </div>
            </div>
             <div className="project-item">
              <div className="tech-icon react">R</div>
              <div className="project-details">
                <p className="project-name">build Dashboard</p>
                <p className="due-date">Due Date: Nov 26, 2025</p>
              </div>
            </div>
             <div className="project-item">
              <div className="tech-icon react">R</div>
              <div className="project-details">
                <p className="project-name">build Dashboard</p>
                <p className="due-date">Due Date: Nov 26, 2025</p>
              </div>
            </div>
             <div className="project-item">
              <div className="tech-icon react">R</div>
              <div className="project-details">
                <p className="project-name">build Dashboard</p>
                <p className="due-date">Due Date: Nov 26, 2025</p>
              </div>
            </div>
             <div className="project-item">
              <div className="tech-icon react">R</div>
              <div className="project-details">
                <p className="project-name">build Dashboard</p>
                <p className="due-date">Due Date: Nov 26, 2025</p>
              </div>
            </div>
             <div className="project-item">
              <div className="tech-icon react">R</div>
              <div className="project-details">
                <p className="project-name">build Dashboard</p>
                <p className="due-date">Due Date: Nov 26, 2025</p>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Second Row: Teams, Progress Chart, Time Tracker */}
      <div className="dashboard-row">
        <div className="flex-item teams">
          <div className="teams-header">
            <h3>Project Teams</h3>
            <button className="add-member-btn">
              <FiPlus /> Add member
            </button>
          </div>
          <div className="team-members">
            <div className="team-member">
              <img 
                src="https://randomuser.me/api/portraits/women/44.jpg" 
                alt="Alexandra Deff" 
                className="member-avatar" 
              />
              <div className="member-info">
                <p className="member-name">Alexandra Deff</p>
                <p className="member-role">Working on GitHub Project Repository</p>
              </div>
              <div className="status-badge completed">completed</div>
            </div>
             <div className="team-member">
              <img 
                src="https://randomuser.me/api/portraits/women/44.jpg" 
                alt="Alexandra Deff" 
                className="member-avatar" 
              />
              <div className="member-info">
                <p className="member-name">Alexandra Deff</p>
                <p className="member-role">Working on GitHub Project Repository</p>
              </div>
              <div className="status-badge completed">completed</div>
            </div>
             <div className="team-member">
              <img 
                src="https://randomuser.me/api/portraits/women/44.jpg" 
                alt="Alexandra Deff" 
                className="member-avatar" 
              />
              <div className="member-info">
                <p className="member-name">Alexandra Deff</p>
                <p className="member-role">Working on GitHub Project Repository</p>
              </div>
              <div className="status-badge completed">completed</div>
            </div>
             <div className="team-member">
              <img 
                src="https://randomuser.me/api/portraits/women/44.jpg" 
                alt="Alexandra Deff" 
                className="member-avatar" 
              />
              <div className="member-info">
                <p className="member-name">Alexandra Deff</p>
                <p className="member-role">Working on GitHub Project Repository</p>
              </div>
              <div className="status-badge completed">completed</div>
            </div>
            <div className="team-member">
              <img 
                src="https://randomuser.me/api/portraits/women/44.jpg" 
                alt="Alexandra Deff" 
                className="member-avatar" 
              />
              <div className="member-info">
                <p className="member-name">Alexandra Deff</p>
                <p className="member-role">Working on GitHub Project Repository</p>
              </div>
              <div className="status-badge in-progress">in progress</div>
            </div>
            <div className="team-member">
              <img 
                src="https://randomuser.me/api/portraits/women/44.jpg" 
                alt="Alexandra Deff" 
                className="member-avatar" 
              />
              <div className="member-info">
                <p className="member-name">Alexandra Deff</p>
                <p className="member-role">Working on GitHub Project Repository</p>
              </div>
              <div className="status-badge pending">Pending</div>
            </div>
          </div>
        </div>

        <div className="flex-item progress-chart">
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
              <p className="percentage">55%</p>
              <p className="chart-label">Project Endend</p>
            </div>
          </div>
          <div className="chart-legend">
            <div className="legend-item">
              <div className="dot completed"></div>
              <span>Completed</span>
            </div>
            <div className="legend-item">
              <div className="dot in-progress"></div>
              <span>In Progress</span>
            </div>
            <div className="legend-item">
              <div className="dot pending"></div>
              <span>Pending</span>
            </div>
          </div>
        </div>

        <div className="flex-item time-tracker">
          <h3>Time Tracker</h3>
          <div className="timer">01:24:08</div>
          <div className="timer-controls">
            <button className="timer-btn">
              <FiPause />
            </button>
            <button className="timer-btn">
              <FiSquare />
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Content;
