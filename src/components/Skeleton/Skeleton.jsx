import React from 'react';
import './Skeleton.css';

// Base Skeleton Component
export const Skeleton = ({ width, height, borderRadius = '8px', className = '' }) => (
  <div 
    className={`skeleton ${className}`}
    style={{ width, height, borderRadius }}
  />
);

// Text Line Skeleton
export const SkeletonText = ({ lines = 1, width = '100%', lastLineWidth = '70%' }) => (
  <div className="skeleton-text">
    {[...Array(lines)].map((_, i) => (
      <div 
        key={i}
        className="skeleton skeleton-line"
        style={{ width: i === lines - 1 ? lastLineWidth : width }}
      />
    ))}
  </div>
);

// Avatar Skeleton
export const SkeletonAvatar = ({ size = 48 }) => (
  <div 
    className="skeleton skeleton-avatar"
    style={{ width: size, height: size }}
  />
);

// Card Skeleton
export const SkeletonCard = ({ height = 200 }) => (
  <div className="skeleton-card">
    <div className="skeleton" style={{ height: '60%', borderRadius: '12px 12px 0 0' }} />
    <div className="skeleton-card-content">
      <SkeletonText lines={2} />
      <div className="skeleton-card-meta">
        <SkeletonAvatar size={32} />
        <Skeleton width="60%" height="16px" />
      </div>
    </div>
  </div>
);

// Table Row Skeleton
export const SkeletonTableRow = ({ columns = 4 }) => (
  <div className="skeleton-table-row">
    {[...Array(columns)].map((_, i) => (
      <div key={i} className="skeleton-cell">
        <Skeleton width={i === 0 ? '80%' : '60%'} height="16px" />
      </div>
    ))}
  </div>
);

// List Item Skeleton
export const SkeletonListItem = () => (
  <div className="skeleton-list-item">
    <SkeletonAvatar size={48} />
    <div className="skeleton-list-content">
      <Skeleton width="70%" height="18px" />
      <Skeleton width="90%" height="14px" />
    </div>
  </div>
);

// Dashboard Stats Skeleton
export const SkeletonStats = ({ count = 4 }) => (
  <div className="skeleton-stats">
    {[...Array(count)].map((_, i) => (
      <div key={i} className="skeleton-stat-card">
        <Skeleton width="40%" height="14px" />
        <Skeleton width="60px" height="36px" borderRadius="4px" />
        <Skeleton width="80%" height="12px" />
      </div>
    ))}
  </div>
);

// Notification Skeleton
export const SkeletonNotification = () => (
  <div className="skeleton-notification">
    <Skeleton width="40px" height="40px" borderRadius="10px" />
    <div className="skeleton-notification-content">
      <Skeleton width="60%" height="16px" />
      <Skeleton width="40%" height="12px" />
    </div>
  </div>
);

// Profile Header Skeleton
export const SkeletonProfileHeader = () => (
  <div className="skeleton-profile-header">
    <div className="skeleton skeleton-cover" />
    <div className="skeleton-profile-info">
      <SkeletonAvatar size={100} />
      <Skeleton width="200px" height="28px" />
      <Skeleton width="150px" height="16px" />
    </div>
  </div>
);

// Full Page Loading Skeleton
export const PageLoadingSkeleton = () => (
  <div className="page-loading-skeleton">
    <div className="loading-spinner-large"></div>
    <p>Loading...</p>
  </div>
);

export default Skeleton;















