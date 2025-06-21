import React from 'react';
import Dashboard from '../pages/dashboard';
import Topbar from './Topbar';   // مسار ملف الـ Topbar
import { Outlet } from 'react-router-dom';
import './DashboardLayout.css'; // هننسقه كمان شوية

const DashboardLayout = () => {
  return (
    <div className="layout-container">
      <Sidebar />
      <div className="main-section">
        <Topbar />
        <div className="content-area">
          <Outlet /> {Dashboard}
        </div>
      </div>
    </div>
  );
};

export default DashboardLayout;
