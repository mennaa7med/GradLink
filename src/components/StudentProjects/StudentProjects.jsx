import React, { useState, useEffect } from 'react';
import { FiPlus, FiEdit2, FiTrash2, FiUsers, FiCalendar, FiFolder, FiX, FiCheck } from 'react-icons/fi';
import { motion, AnimatePresence } from 'framer-motion';
import api from '../../api/client';
import './StudentProjects.css';

const StudentProjects = () => {
  const [projects, setProjects] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [editingProject, setEditingProject] = useState(null);
  const [formData, setFormData] = useState({
    title: '',
    description: '',
    category: 'Web Development',
    status: 'Planning'
  });

  const categories = [
    'Web Development',
    'Mobile Development',
    'AI/Machine Learning',
    'Data Science',
    'IoT',
    'Cybersecurity',
    'Game Development',
    'Other'
  ];

  const statuses = [
    'Planning',
    'In Progress',
    'Testing',
    'Completed',
    'On Hold'
  ];

  useEffect(() => {
    loadProjects();
  }, []);

  const loadProjects = async () => {
    try {
      setLoading(true);
      const { data } = await api.get('/api/projects');
      setProjects(data);
    } catch (error) {
      console.error('Failed to load projects:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (editingProject) {
        await api.put(`/api/projects/${editingProject.id}`, formData);
      } else {
        await api.post('/api/projects', formData);
      }
      await loadProjects();
      closeModal();
      // Dispatch event for dashboard update
      window.dispatchEvent(new Event('projectUpdated'));
    } catch (error) {
      console.error('Failed to save project:', error);
      alert('Failed to save project. Please try again.');
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Are you sure you want to delete this project?')) return;
    try {
      await api.delete(`/api/projects/${id}`);
      await loadProjects();
      window.dispatchEvent(new Event('projectUpdated'));
    } catch (error) {
      console.error('Failed to delete project:', error);
      alert('Failed to delete project. Please try again.');
    }
  };

  const openEditModal = (project) => {
    setEditingProject(project);
    setFormData({
      title: project.title,
      description: project.description || '',
      category: project.category,
      status: project.status
    });
    setShowModal(true);
  };

  const openNewModal = () => {
    setEditingProject(null);
    setFormData({
      title: '',
      description: '',
      category: 'Web Development',
      status: 'Planning'
    });
    setShowModal(true);
  };

  const closeModal = () => {
    setShowModal(false);
    setEditingProject(null);
    setFormData({
      title: '',
      description: '',
      category: 'Web Development',
      status: 'Planning'
    });
  };

  const getStatusColor = (status) => {
    switch (status) {
      case 'Completed': return '#10b981';
      case 'In Progress': return '#3b82f6';
      case 'Testing': return '#f59e0b';
      case 'On Hold': return '#ef4444';
      default: return '#6b7280';
    }
  };

  const getCategoryIcon = (category) => {
    switch (category) {
      case 'Web Development': return '🌐';
      case 'Mobile Development': return '📱';
      case 'AI/Machine Learning': return '🤖';
      case 'Data Science': return '📊';
      case 'IoT': return '🔌';
      case 'Cybersecurity': return '🔒';
      case 'Game Development': return '🎮';
      default: return '📁';
    }
  };

  return (
    <div className="student-projects">
      <div className="projects-header">
        <div className="header-info">
          <h1>My Projects</h1>
          <p>Manage your graduation and personal projects</p>
        </div>
        <button className="create-project-btn" onClick={openNewModal}>
          <FiPlus /> New Project
        </button>
      </div>

      {loading ? (
        <div className="loading-state">
          <div className="spinner"></div>
          <p>Loading projects...</p>
        </div>
      ) : projects.length === 0 ? (
        <div className="empty-state">
          <FiFolder className="empty-icon" />
          <h3>No Projects Yet</h3>
          <p>Create your first project to get started!</p>
          <button className="create-first-btn" onClick={openNewModal}>
            <FiPlus /> Create Project
          </button>
        </div>
      ) : (
        <div className="projects-grid">
          {projects.map((project, index) => (
            <motion.div
              key={project.id}
              className="project-card"
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ delay: index * 0.1 }}
            >
              <div className="project-header">
                <span className="category-icon">{getCategoryIcon(project.category)}</span>
                <span 
                  className="status-badge"
                  style={{ backgroundColor: getStatusColor(project.status) }}
                >
                  {project.status}
                </span>
              </div>
              
              <h3 className="project-title">{project.title}</h3>
              <p className="project-description">
                {project.description || 'No description provided'}
              </p>
              
              <div className="project-meta">
                <span className="meta-item">
                  <FiFolder /> {project.category}
                </span>
                <span className="meta-item">
                  <FiCalendar /> {new Date(project.createdAt).toLocaleDateString()}
                </span>
              </div>

              <div className="project-actions">
                <button 
                  className="action-btn edit"
                  onClick={() => openEditModal(project)}
                >
                  <FiEdit2 /> Edit
                </button>
                <button 
                  className="action-btn delete"
                  onClick={() => handleDelete(project.id)}
                >
                  <FiTrash2 /> Delete
                </button>
              </div>
            </motion.div>
          ))}
        </div>
      )}

      {/* Create/Edit Modal */}
      <AnimatePresence>
        {showModal && (
          <motion.div 
            className="modal-overlay"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            onClick={closeModal}
          >
            <motion.div 
              className="modal-content"
              initial={{ scale: 0.9, opacity: 0 }}
              animate={{ scale: 1, opacity: 1 }}
              exit={{ scale: 0.9, opacity: 0 }}
              onClick={e => e.stopPropagation()}
            >
              <div className="modal-header">
                <h2>{editingProject ? 'Edit Project' : 'Create New Project'}</h2>
                <button className="close-btn" onClick={closeModal}>
                  <FiX />
                </button>
              </div>

              <form onSubmit={handleSubmit}>
                <div className="form-group">
                  <label>Project Title *</label>
                  <input
                    type="text"
                    value={formData.title}
                    onChange={(e) => setFormData({ ...formData, title: e.target.value })}
                    placeholder="Enter project title"
                    required
                  />
                </div>

                <div className="form-group">
                  <label>Description</label>
                  <textarea
                    value={formData.description}
                    onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                    placeholder="Describe your project..."
                    rows={4}
                  />
                </div>

                <div className="form-row">
                  <div className="form-group">
                    <label>Category</label>
                    <select
                      value={formData.category}
                      onChange={(e) => setFormData({ ...formData, category: e.target.value })}
                    >
                      {categories.map(cat => (
                        <option key={cat} value={cat}>{cat}</option>
                      ))}
                    </select>
                  </div>

                  <div className="form-group">
                    <label>Status</label>
                    <select
                      value={formData.status}
                      onChange={(e) => setFormData({ ...formData, status: e.target.value })}
                    >
                      {statuses.map(status => (
                        <option key={status} value={status}>{status}</option>
                      ))}
                    </select>
                  </div>
                </div>

                <div className="modal-actions">
                  <button type="button" className="cancel-btn" onClick={closeModal}>
                    Cancel
                  </button>
                  <button type="submit" className="submit-btn">
                    <FiCheck /> {editingProject ? 'Save Changes' : 'Create Project'}
                  </button>
                </div>
              </form>
            </motion.div>
          </motion.div>
        )}
      </AnimatePresence>
    </div>
  );
};

export default StudentProjects;

