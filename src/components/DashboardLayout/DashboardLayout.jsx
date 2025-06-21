import React from "react";
import { Outlet } from "react-router-dom";
import Sidebar from "../Sidebar/Sidebar";
import Topbar from "../Topbar";
import "./DashboardLayout.css";

const DashboardLayout = () => {
  return (
    <div className="dashboard-container">
      <Sidebar />
      <div className="main-content">
        <Topbar />
        <div className="outlet-container">
          <Outlet />
        </div>
      </div>
    </div>
  );
};

export default DashboardLayout; 