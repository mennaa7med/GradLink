import React, { useState, useEffect, useRef, useCallback } from 'react';
import './Matrials.css';
import { 
  FiDownload, FiExternalLink, FiBook, FiDatabase, FiVideo, 
  FiSearch, FiFilter, FiPlus, FiStar, FiBookmark, FiAward, 
  FiTrendingUp, FiHeart, FiShare2, FiClock, FiCalendar, 
  FiTag, FiX, FiChevronDown, FiChevronUp, FiGrid, FiList,
  FiFolder, FiPlay, FiCheck, FiEdit3, FiMessageSquare,
  FiThumbsUp, FiLoader, FiChevronLeft, FiChevronRight,
  FiLayers, FiTarget, FiZap
} from 'react-icons/fi';
import * as materialsApi from '../api/materials';

const Matrials = () => {
  const [activeSection, setActiveSection] = useState('all');
  const [searchTerm, setSearchTerm] = useState('');
  const [showFilters, setShowFilters] = useState(false);
  const [selectedField, setSelectedField] = useState('all');
  const [showAddModal, setShowAddModal] = useState(false);
  const [viewMode, setViewMode] = useState('grid');
  const [sortBy, setSortBy] = useState('popular');
  const [showToast, setShowToast] = useState(false);
  const [toastMessage, setToastMessage] = useState('');
  const [showRecentlyViewed, setShowRecentlyViewed] = useState(false);
  const searchInputRef = useRef(null);
  
  // API data states
  const [materials, setMaterials] = useState([]);
  const [loading, setLoading] = useState(true);
  const [stats, setStats] = useState(null);
  const [userStats, setUserStats] = useState(null);
  const [recommendations, setRecommendations] = useState([]);
  const [topRated, setTopRated] = useState([]);
  const [recentlyViewed, setRecentlyViewed] = useState([]);
  const [learningPaths, setLearningPaths] = useState([]);
  const [myCollections, setMyCollections] = useState([]);
  
  // Pagination
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const pageSize = 12;
  
  // Selected material for details
  const [selectedMaterial, setSelectedMaterial] = useState(null);
  const [showRatingModal, setShowRatingModal] = useState(false);
  const [showNotesModal, setShowNotesModal] = useState(false);
  const [showCollectionModal, setShowCollectionModal] = useState(false);
  
  // Check if user is logged in
  const isLoggedIn = !!localStorage.getItem('token');

  const fields = ['all', 'computer science', 'engineering', 'business', 'medicine', 'arts'];

  // Fetch materials from API
  const fetchMaterials = useCallback(async () => {
    setLoading(true);
    try {
      const filters = {
        type: activeSection !== 'all' && activeSection !== 'favorites' && activeSection !== 'collections' ? activeSection : undefined,
        field: selectedField !== 'all' ? selectedField : undefined,
        searchTerm: searchTerm || undefined,
        sortBy: sortBy,
        page: currentPage,
        pageSize: pageSize
      };
      
      if (activeSection === 'favorites' && isLoggedIn) {
        const data = await materialsApi.getBookmarkedMaterials();
        setMaterials(data);
        setTotalCount(data.length);
        setTotalPages(1);
      } else if (activeSection !== 'collections') {
        const data = await materialsApi.getMaterials(filters);
        setMaterials(data.items || []);
        setTotalCount(data.totalCount);
        setTotalPages(data.totalPages);
      }
    } catch (error) {
      console.error('Error fetching materials:', error);
      showToastMessage('Failed to load materials');
    } finally {
      setLoading(false);
    }
  }, [activeSection, selectedField, searchTerm, sortBy, currentPage, isLoggedIn]);

  // Initial load
  useEffect(() => {
    fetchMaterials();
    fetchStats();
    fetchTopRated();
    fetchLearningPaths();
    
    if (isLoggedIn) {
      fetchUserStats();
      fetchRecommendations();
      fetchRecentlyViewed();
      fetchMyCollections();
    }
  }, [fetchMaterials, isLoggedIn]);

  // Keyboard shortcut for search
  useEffect(() => {
    const handleKeyDown = (e) => {
      if ((e.ctrlKey || e.metaKey) && e.key === 'k') {
        e.preventDefault();
        searchInputRef.current?.focus();
      }
    };
    
    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, []);

  const fetchStats = async () => {
    try {
      const data = await materialsApi.getMaterialStats();
      setStats(data);
    } catch (error) {
      console.error('Error fetching stats:', error);
    }
  };

  const fetchUserStats = async () => {
    try {
      const data = await materialsApi.getMyMaterialStats();
      setUserStats(data);
    } catch (error) {
      console.error('Error fetching user stats:', error);
    }
  };

  const fetchRecommendations = async () => {
    try {
      const data = await materialsApi.getRecommendations();
      setRecommendations(data);
    } catch (error) {
      console.error('Error fetching recommendations:', error);
    }
  };

  const fetchTopRated = async () => {
    try {
      const data = await materialsApi.getTopRated(4);
      setTopRated(data);
    } catch (error) {
      console.error('Error fetching top rated:', error);
    }
  };

  const fetchRecentlyViewed = async () => {
    try {
      const data = await materialsApi.getRecentlyViewedMaterials(10);
      setRecentlyViewed(data);
    } catch (error) {
      console.error('Error fetching recently viewed:', error);
    }
  };

  const fetchLearningPaths = async () => {
    try {
      const data = await materialsApi.getLearningPaths();
      setLearningPaths(data);
    } catch (error) {
      console.error('Error fetching learning paths:', error);
    }
  };

  const fetchMyCollections = async () => {
    try {
      const data = await materialsApi.getMyCollections();
      setMyCollections(data);
    } catch (error) {
      console.error('Error fetching collections:', error);
    }
  };

  const showToastMessage = (message) => {
    setToastMessage(message);
    setShowToast(true);
    setTimeout(() => setShowToast(false), 3000);
  };

  const handleToggleBookmark = async (material) => {
    if (!isLoggedIn) {
      showToastMessage('Please login to bookmark materials');
      return;
    }
    
    try {
      const result = await materialsApi.toggleBookmark(material.id);
      showToastMessage(result.message);
      
      // Update local state
      setMaterials(prev => prev.map(m => 
        m.id === material.id ? { ...m, isBookmarked: result.isBookmarked } : m
      ));
      
      if (activeSection === 'favorites') {
        fetchMaterials();
      }
    } catch (error) {
      console.error('Error toggling bookmark:', error);
      showToastMessage('Failed to update bookmark');
    }
  };

  const handleViewResource = async (item) => {
    // Track view
    if (isLoggedIn) {
      try {
        await materialsApi.updateMaterialProgress(item.id, {
          status: 'in_progress'
        });
        fetchRecentlyViewed();
      } catch (error) {
        console.error('Error tracking view:', error);
      }
    }
    
    // Open link
    if (item.link) {
      window.open(item.link, '_blank');
    }
  };

  const handleShareResource = async (item) => {
    if (navigator.share) {
      try {
        await navigator.share({
          title: item.title,
          text: item.description,
          url: item.link || window.location.href,
        });
        showToastMessage('Resource shared successfully');
      } catch (error) {
        console.error('Error sharing:', error);
      }
    } else {
      navigator.clipboard.writeText(item.link || window.location.href);
      showToastMessage('Link copied to clipboard');
    }
  };

  const handleMarkComplete = async (material) => {
    if (!isLoggedIn) {
      showToastMessage('Please login to track progress');
      return;
    }
    
    try {
      await materialsApi.updateMaterialProgress(material.id, {
        status: 'completed',
        progressPercent: 100
      });
      showToastMessage('Marked as complete!');
      fetchMaterials();
      fetchUserStats();
    } catch (error) {
      console.error('Error marking complete:', error);
      showToastMessage('Failed to update progress');
    }
  };

  // Render star rating
  const renderStars = (rating) => {
    return (
      <div className="star-rating">
        {[1, 2, 3, 4, 5].map((star) => (
          <FiStar 
            key={star}
            className={`star ${star <= rating ? 'filled' : ''}`}
          />
        ))}
        <span className="rating-value">{rating?.toFixed(1) || '0.0'}</span>
      </div>
    );
  };

  // Render progress bar
  const renderProgressBar = (progress) => {
    return (
      <div className="progress-container">
        <div className="progress-bar">
          <div 
            className="progress-fill" 
            style={{ width: `${progress || 0}%` }}
          />
        </div>
        <span className="progress-text">{progress || 0}%</span>
      </div>
    );
  };

  // Pagination component
  const renderPagination = () => {
    if (totalPages <= 1) return null;
    
    return (
      <div className="pagination">
        <button 
          className="page-btn"
          disabled={currentPage === 1}
          onClick={() => setCurrentPage(p => p - 1)}
        >
          <FiChevronLeft />
        </button>
        
        <span className="page-info">
          Page {currentPage} of {totalPages} ({totalCount} items)
        </span>
        
        <button 
          className="page-btn"
          disabled={currentPage === totalPages}
          onClick={() => setCurrentPage(p => p + 1)}
        >
          <FiChevronRight />
        </button>
      </div>
    );
  };

  // Learning paths section
  const renderLearningPaths = () => {
    if (activeSection !== 'collections' || learningPaths.length === 0) return null;
    
    return (
      <div className="learning-paths-section">
        <h2 className="section-title">
          <FiTarget className="section-icon" /> Learning Paths
        </h2>
        <div className="learning-paths-grid">
          {learningPaths.map((path) => (
            <div className="learning-path-card" key={path.id}>
              <div className="path-header">
                <div className="path-icon">
                  <FiLayers />
                </div>
                <div className="path-badge">{path.difficultyLevel || 'Beginner'}</div>
              </div>
              <h3>{path.title}</h3>
              <p>{path.description}</p>
              <div className="path-meta">
                <span><FiBook /> {path.materialCount} materials</span>
                <span><FiClock /> {path.estimatedTimeMinutes ? Math.round(path.estimatedTimeMinutes / 60) : 0}h</span>
              </div>
              {isLoggedIn && path.isFollowing && (
                <div className="path-progress">
                  {renderProgressBar(path.userProgress)}
                </div>
              )}
              <button 
                className={`path-btn ${path.isFollowing ? 'following' : ''}`}
                onClick={async () => {
                  if (!isLoggedIn) {
                    showToastMessage('Please login to follow learning paths');
                    return;
                  }
                  try {
                    if (path.isFollowing) {
                      await materialsApi.unfollowCollection(path.id);
                    } else {
                      await materialsApi.followCollection(path.id);
                    }
                    fetchLearningPaths();
                  } catch (error) {
                    showToastMessage('Failed to update follow status');
                  }
                }}
              >
                {path.isFollowing ? <><FiCheck /> Following</> : <><FiPlay /> Start Learning</>}
              </button>
            </div>
          ))}
        </div>
      </div>
    );
  };

  // User collections section
  const renderMyCollections = () => {
    if (activeSection !== 'collections' || !isLoggedIn) return null;
    
    return (
      <div className="my-collections-section">
        <div className="section-header">
          <h2 className="section-title">
            <FiFolder className="section-icon" /> My Collections
          </h2>
          <button 
            className="add-collection-btn"
            onClick={() => setShowCollectionModal(true)}
          >
            <FiPlus /> Create Collection
          </button>
        </div>
        
        {myCollections.length === 0 ? (
          <div className="empty-state">
            <FiFolder size={48} />
            <p>You haven't created any collections yet</p>
            <button 
              className="create-btn"
              onClick={() => setShowCollectionModal(true)}
            >
              Create Your First Collection
            </button>
          </div>
        ) : (
          <div className="collections-grid">
            {myCollections.map((collection) => (
              <div className="collection-card" key={collection.id}>
                <h3>{collection.title}</h3>
                <p>{collection.description}</p>
                <div className="collection-meta">
                  <span><FiBook /> {collection.materialCount} items</span>
                  <span><FiUsers /> {collection.followerCount} followers</span>
                </div>
                <div className="collection-actions">
                  <button className="action-btn" title="Edit">
                    <FiEdit3 />
                  </button>
                  <button 
                    className="action-btn"
                    onClick={async () => {
                      if (window.confirm('Delete this collection?')) {
                        try {
                          await materialsApi.deleteCollection(collection.id);
                          fetchMyCollections();
                          showToastMessage('Collection deleted');
                        } catch (error) {
                          showToastMessage('Failed to delete collection');
                        }
                      }
                    }}
                    title="Delete"
                  >
                    <FiX />
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    );
  };

  // Recommendations section
  const renderRecommendations = () => {
    if (!isLoggedIn || recommendations.length === 0 || activeSection === 'collections') return null;
    
    return (
      <div className="recommendations-section">
        <h2 className="section-title">
          <FiZap className="section-icon" /> Recommended For You
        </h2>
        <div className="recommendations-grid">
          {recommendations.slice(0, 4).map((rec, index) => (
            <div className="recommendation-card" key={index}>
              <div className="rec-reason">{rec.reason}</div>
              <h3>{rec.material.title}</h3>
              <p>{rec.material.description?.substring(0, 100)}...</p>
              <div className="rec-meta">
                <span className="type-badge">{rec.material.type}</span>
                {renderStars(rec.material.averageRating)}
              </div>
              <button 
                className="view-btn"
                onClick={() => handleViewResource(rec.material)}
              >
                <FiExternalLink /> View
              </button>
            </div>
          ))}
        </div>
      </div>
    );
  };

  // Top rated section
  const renderTopRated = () => {
    if (activeSection === 'collections' || topRated.length === 0) return null;
    
    return (
      <div className="top-rated-section">
        <h2 className="section-title"><FiStar className="section-icon" /> Top Rated Resources</h2>
        <div className="materials-grid">
          {topRated.map((item, index) => (
            <div className="material-card top-rated" key={index}>
              <div className="material-content">
                <div className="downloads-badge">{item.downloadCount} downloads</div>
                <h3 className="material-title">{item.title}</h3>
                {item.author && <p className="material-author">by {item.author}</p>}
                <p className="material-description">{item.description}</p>
                <div className="card-footer">
                  {item.fileType && <span className="file-type">{item.fileType}</span>}
                  <span className="field-tag">{item.field}</span>
                  {renderStars(item.averageRating)}
                </div>
              </div>
              <button 
                className="external-link-btn"
                onClick={() => handleViewResource(item)}
              >
                <FiExternalLink /> View Resource
              </button>
            </div>
          ))}
        </div>
      </div>
    );
  };

  // Recently viewed section
  const renderRecentlyViewed = () => {
    if (!isLoggedIn || recentlyViewed.length === 0 || activeSection === 'collections') return null;
    
    return (
      <div className="recently-viewed-section">
        <div className="section-header">
          <h2 className="section-title">
            <FiClock className="section-icon" /> Recently Viewed
          </h2>
          <button 
            className="toggle-btn"
            onClick={() => setShowRecentlyViewed(!showRecentlyViewed)}
          >
            {showRecentlyViewed ? <FiChevronUp /> : <FiChevronDown />}
          </button>
        </div>
        
        {showRecentlyViewed && (
          <div className="recently-viewed-grid">
            {recentlyViewed.map((item, index) => (
              <div className="recent-item" key={index}>
                <div className="recent-content">
                  <h4>{item.materialTitle}</h4>
                  <p className="recent-meta">
                    <span className="recent-section">{item.materialType}</span>
                    <span className="recent-time">
                      {new Date(item.lastAccessedAt).toLocaleDateString()}
                    </span>
                  </p>
                  {item.status !== 'not_started' && (
                    <div className="mini-progress">
                      {renderProgressBar(item.progressPercent)}
                    </div>
                  )}
                </div>
                <div className="recent-actions">
                  <button 
                    className="action-btn"
                    onClick={() => {
                      const material = { id: item.materialId, title: item.materialTitle };
                      handleViewResource(material);
                    }}
                  >
                    <FiExternalLink />
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    );
  };

  // Main content renderer
  const renderContent = () => {
    if (loading) {
      return (
        <div className="loading-state">
          <FiLoader className="spinner" />
          <p>Loading materials...</p>
        </div>
      );
    }

    if (activeSection === 'collections') {
      return (
        <>
          {renderLearningPaths()}
          {renderMyCollections()}
        </>
      );
    }
    
    if (materials.length === 0) {
      return (
        <div className="no-results">
          <FiSearch size={48} />
          <p>No resources match your search criteria.</p>
        </div>
      );
    }
    
    return (
      <>
        <div className={`materials-${viewMode}`}>
          {materials.map((item, index) => (
            <div className={`material-card ${viewMode === 'list' ? 'list-view' : ''}`} key={index}>
              <div className="material-content">
                {item.isFeatured && (
                  <div className="featured-badge">
                    <FiAward /> Featured
                  </div>
                )}
                <h3 className="material-title">{item.title}</h3>
                {item.author && <p className="material-author">by {item.author}</p>}
                <p className="material-description">{item.description}</p>
                <div className="card-footer">
                  {item.fileType && <span className="file-type">{item.fileType}</span>}
                  <span className="field-tag">{item.field}</span>
                  <span className="downloads-count"><FiDownload /> {item.downloadCount}</span>
                  {item.ratingCount > 0 && renderStars(item.averageRating)}
                </div>
                {isLoggedIn && item.userProgress > 0 && (
                  <div className="user-progress">
                    {renderProgressBar(item.userProgress)}
                  </div>
                )}
              </div>
              <div className="card-actions">
                <button 
                  className={`action-btn ${item.isBookmarked ? 'active' : ''}`}
                  onClick={() => handleToggleBookmark(item)}
                  title={item.isBookmarked ? "Remove from bookmarks" : "Add to bookmarks"}
                >
                  <FiHeart className={item.isBookmarked ? "filled" : ""} />
                </button>
                <button 
                  className="action-btn"
                  onClick={() => handleShareResource(item)}
                  title="Share"
                >
                  <FiShare2 />
                </button>
                {isLoggedIn && item.userProgress < 100 && (
                  <button 
                    className="action-btn"
                    onClick={() => handleMarkComplete(item)}
                    title="Mark as complete"
                  >
                    <FiCheck />
                  </button>
                )}
                <button 
                  className="external-link-btn"
                  onClick={() => handleViewResource(item)}
                >
                  {item.fileType ? (
                    <><FiDownload /> Download</>
                  ) : (
                    <><FiExternalLink /> View Resource</>
                  )}
                </button>
              </div>
            </div>
          ))}
        </div>
        {renderPagination()}
      </>
    );
  };

  // Add Resource Modal
  const AddResourceModal = () => {
    const [newResource, setNewResource] = useState({
      title: '',
      description: '',
      field: 'computer science',
      link: '',
      type: 'tool',
      author: '',
      fileType: '',
      difficultyLevel: 'beginner',
      estimatedTimeMinutes: ''
    });
    const [submitting, setSubmitting] = useState(false);
    
    const handleChange = (e) => {
      const { name, value } = e.target;
      setNewResource(prev => ({
        ...prev,
        [name]: value
      }));
    };

    const handleSubmit = async (e) => {
      e.preventDefault();
      setSubmitting(true);
      
      try {
        await materialsApi.createMaterial({
          ...newResource,
          estimatedTimeMinutes: newResource.estimatedTimeMinutes 
            ? parseInt(newResource.estimatedTimeMinutes) 
            : null
        });
        setShowAddModal(false);
        showToastMessage('Resource submitted successfully! It will be reviewed before being added.');
        fetchMaterials();
      } catch (error) {
        console.error('Error submitting resource:', error);
        showToastMessage('Failed to submit resource. Please try again.');
      } finally {
        setSubmitting(false);
      }
    };
    
    return (
      <div className="modal-overlay" onClick={() => setShowAddModal(false)}>
        <div className="modal-content" onClick={e => e.stopPropagation()}>
          <h2>Add New Resource</h2>
          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <label htmlFor="type">Resource Type</label>
              <select 
                id="type" 
                name="type" 
                value={newResource.type}
                onChange={handleChange}
                required
              >
                <option value="tool">Tool</option>
                <option value="book">Book</option>
                <option value="template">Template</option>
                <option value="dataset">Dataset</option>
                <option value="video">Video</option>
              </select>
            </div>
            
            <div className="form-group">
              <label htmlFor="title">Title</label>
              <input 
                type="text" 
                id="title" 
                name="title" 
                value={newResource.title}
                onChange={handleChange}
                required
                placeholder="Enter resource title"
              />
            </div>
            
            <div className="form-group">
              <label htmlFor="description">Description</label>
              <textarea 
                id="description" 
                name="description" 
                value={newResource.description}
                onChange={handleChange}
                required
                placeholder="Describe this resource"
              />
            </div>
            
            <div className="form-group">
              <label htmlFor="field">Field</label>
              <select 
                id="field" 
                name="field" 
                value={newResource.field}
                onChange={handleChange}
                required
              >
                {fields.filter(f => f !== 'all').map(field => (
                  <option key={field} value={field}>
                    {field.charAt(0).toUpperCase() + field.slice(1)}
                  </option>
                ))}
              </select>
            </div>
            
            <div className="form-group">
              <label htmlFor="link">Resource Link</label>
              <input 
                type="url" 
                id="link" 
                name="link" 
                value={newResource.link}
                onChange={handleChange}
                required
                placeholder="https://example.com"
              />
            </div>
            
            <div className="form-group">
              <label htmlFor="difficultyLevel">Difficulty Level</label>
              <select 
                id="difficultyLevel" 
                name="difficultyLevel" 
                value={newResource.difficultyLevel}
                onChange={handleChange}
              >
                <option value="beginner">Beginner</option>
                <option value="intermediate">Intermediate</option>
                <option value="advanced">Advanced</option>
              </select>
            </div>
            
            {newResource.type === 'template' && (
              <div className="form-group">
                <label htmlFor="fileType">File Type</label>
                <input 
                  type="text" 
                  id="fileType" 
                  name="fileType" 
                  value={newResource.fileType}
                  onChange={handleChange}
                  placeholder="e.g., DOCX, PDF, PPTX"
                />
              </div>
            )}
            
            {newResource.type === 'book' && (
              <div className="form-group">
                <label htmlFor="author">Author</label>
                <input 
                  type="text" 
                  id="author" 
                  name="author" 
                  value={newResource.author}
                  onChange={handleChange}
                  placeholder="Author name"
                />
              </div>
            )}
            
            {(newResource.type === 'video' || newResource.type === 'book') && (
              <div className="form-group">
                <label htmlFor="estimatedTimeMinutes">Estimated Time (minutes)</label>
                <input 
                  type="number" 
                  id="estimatedTimeMinutes" 
                  name="estimatedTimeMinutes"
                  value={newResource.estimatedTimeMinutes}
                  onChange={handleChange}
                  placeholder="e.g., 120"
                  min="1"
                />
              </div>
            )}
            
            <div className="modal-buttons">
              <button 
                type="button" 
                className="cancel-btn"
                onClick={() => setShowAddModal(false)}
                disabled={submitting}
              >
                Cancel
              </button>
              <button type="submit" className="submit-btn" disabled={submitting}>
                {submitting ? <><FiLoader className="spinner" /> Submitting...</> : 'Submit Resource'}
              </button>
            </div>
          </form>
        </div>
      </div>
    );
  };

  // User stats widget
  const renderUserStats = () => {
    if (!isLoggedIn || !userStats) return null;
    
    return (
      <div className="user-stats-widget">
        <div className="stat-item">
          <FiBookmark />
          <span>{userStats.bookmarkedCount} Bookmarked</span>
        </div>
        <div className="stat-item">
          <FiCheck />
          <span>{userStats.completedCount} Completed</span>
        </div>
        <div className="stat-item">
          <FiPlay />
          <span>{userStats.inProgressCount} In Progress</span>
        </div>
        <div className="stat-item">
          <FiClock />
          <span>{Math.round((userStats.totalTimeSpentMinutes || 0) / 60)}h Learning</span>
        </div>
      </div>
    );
  };

  return (
    <div className="materials-container">
      <h1 className="materials-title">Materials & Resources</h1>
      
      {renderUserStats()}
      
      {/* Navigation */}
      <div className="materials-nav">
        <button 
          className={`nav-item ${activeSection === 'all' ? 'active' : ''}`}
          onClick={() => { setActiveSection('all'); setCurrentPage(1); }}
        >
          <FiTrendingUp className="nav-icon" /> All
        </button>
        <button 
          className={`nav-item ${activeSection === 'template' ? 'active' : ''}`}
          onClick={() => { setActiveSection('template'); setCurrentPage(1); }}
        >
          <FiDownload className="nav-icon" /> Templates
        </button>
        <button 
          className={`nav-item ${activeSection === 'tool' ? 'active' : ''}`}
          onClick={() => { setActiveSection('tool'); setCurrentPage(1); }}
        >
          <FiExternalLink className="nav-icon" /> Tools
        </button>
        <button 
          className={`nav-item ${activeSection === 'book' ? 'active' : ''}`}
          onClick={() => { setActiveSection('book'); setCurrentPage(1); }}
        >
          <FiBook className="nav-icon" /> Books
        </button>
        <button 
          className={`nav-item ${activeSection === 'dataset' ? 'active' : ''}`}
          onClick={() => { setActiveSection('dataset'); setCurrentPage(1); }}
        >
          <FiDatabase className="nav-icon" /> Datasets
        </button>
        <button 
          className={`nav-item ${activeSection === 'video' ? 'active' : ''}`}
          onClick={() => { setActiveSection('video'); setCurrentPage(1); }}
        >
          <FiVideo className="nav-icon" /> Videos
        </button>
        <button 
          className={`nav-item ${activeSection === 'favorites' ? 'active' : ''}`}
          onClick={() => { setActiveSection('favorites'); setCurrentPage(1); }}
        >
          <FiHeart className="nav-icon" /> Favorites
        </button>
        <button 
          className={`nav-item ${activeSection === 'collections' ? 'active' : ''}`}
          onClick={() => { setActiveSection('collections'); setCurrentPage(1); }}
        >
          <FiFolder className="nav-icon" /> Collections
        </button>
      </div>
      
      {activeSection !== 'collections' && (
        <div className="search-filter-container">
          <div className="search--box">
            <FiSearch className="search-iconn" />
            <input 
              type="text" 
              placeholder="Search resources... (Ctrl+K)"
              value={searchTerm}
              onChange={(e) => {
                setSearchTerm(e.target.value);
                setCurrentPage(1);
              }}
              ref={searchInputRef}
            />
            {searchTerm && (
              <button 
                className="clear-search" 
                onClick={() => { setSearchTerm(''); setCurrentPage(1); }}
                title="Clear search"
              >
                <FiX />
              </button>
            )}
          </div>
          
          <div className="view-controls">
            <button 
              className={`view-btn ${viewMode === 'grid' ? 'active' : ''}`}
              onClick={() => setViewMode('grid')}
              title="Grid view"
            >
              <FiGrid />
            </button>
            <button 
              className={`view-btn ${viewMode === 'list' ? 'active' : ''}`}
              onClick={() => setViewMode('list')}
              title="List view"
            >
              <FiList />
            </button>
          </div>
          
          <div className="sort-control">
            <select 
              value={sortBy}
              onChange={(e) => { setSortBy(e.target.value); setCurrentPage(1); }}
            >
              <option value="popular">Most Popular</option>
              <option value="newest">Newest</option>
              <option value="rating">Top Rated</option>
              <option value="name">Name</option>
            </select>
          </div>
          
          <button 
            className="filter-btn"
            onClick={() => setShowFilters(!showFilters)}
          >
            <FiFilter /> Filter
          </button>
          
          {isLoggedIn && (
            <button 
              className="add-resource-btn"
              onClick={() => setShowAddModal(true)}
            >
              <FiPlus /> Add Resource
            </button>
          )}
        </div>
      )}
      
      {showFilters && activeSection !== 'collections' && (
        <div className="filters-container">
          <div className="filter-group">
            <label>Field:</label>
            <div className="field-options">
              {fields.map(field => (
                <button 
                  key={field}
                  className={`field-option ${selectedField === field ? 'active' : ''}`}
                  onClick={() => { setSelectedField(field); setCurrentPage(1); }}
                >
                  {field.charAt(0).toUpperCase() + field.slice(1)}
                </button>
              ))}
            </div>
          </div>
        </div>
      )}
      
      {/* Recommendations */}
      {activeSection === 'all' && !searchTerm && renderRecommendations()}
      
      {/* Main Content */}
      {renderContent()}
      
      {/* Top Rated */}
      {!searchTerm && selectedField === 'all' && activeSection === 'all' && renderTopRated()}
      
      {/* Modals */}
      {showAddModal && <AddResourceModal />}
      
      {/* Toast */}
      {showToast && (
        <div className={`toast-notification show`}>
          {toastMessage}
        </div>
      )}
      
      {/* Recently Viewed */}
      {renderRecentlyViewed()}
    </div>
  );
};

export default Matrials;
