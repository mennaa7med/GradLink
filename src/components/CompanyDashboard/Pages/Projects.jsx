import React, { useEffect, useMemo, useState, useCallback } from 'react';
import { motion } from 'framer-motion';
import './Projects.css';
import { createProject, deleteProject, listProjects, updateProject, listTasks, createTask, updateTask, deleteTask } from '../../../api/projects';

const Projects = () => {
  const [showAddModal, setShowAddModal] = useState(false);
  const [projects, setProjects] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [form, setForm] = useState({ 
    title: '', 
    description: '', 
    technologies: '', 
    status: 'Draft',
    companyName: '',
    duration: '3 months',
    budget: ''
  });
  const [selectedProject, setSelectedProject] = useState(null);
  const [tasks, setTasks] = useState([]);
  const [taskText, setTaskText] = useState('');

  const load = async () => {
    setLoading(true);
    try {
      const data = await listProjects();
      setProjects(data);
    } catch (e) {
      setError('Failed to load projects');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { load(); }, []);

  const openAdd = () => {
    // Get company name from localStorage
    const companyName = localStorage.getItem('companyName') || localStorage.getItem('userName') || 'My Company';
    
    setForm({ 
      title: '', 
      description: '', 
      technologies: '', 
      status: 'Draft',
      companyName: companyName, // Auto-fill company name
      duration: '3 months',
      budget: ''
    });
    setSelectedProject(null);
    setShowAddModal(true);
  };

  const submitAdd = async (e) => {
    e.preventDefault();
    try {
      const created = await createProject(form);
      setProjects((p) => [created, ...p]);
      closeModal();
    } catch {
      setError('Failed to create project');
    }
  };

  const onDelete = async (id) => {
    if (!confirm('Delete project?')) return;
    try {
      await deleteProject(id);
      setProjects((p) => p.filter(x => x.id !== id));
    } catch {
      setError('Failed to delete project');
    }
  };

  const onEdit = (p) => {
    // Get company name from localStorage
    const companyName = localStorage.getItem('companyName') || localStorage.getItem('userName') || 'My Company';
    
    setSelectedProject(p);
    setForm({ 
      title: p.title, 
      description: p.description, 
      technologies: p.technologies, 
      status: p.status,
      companyName: companyName, // Always use current company name
      duration: p.duration || '3 months',
      budget: p.budget || ''
    });
    setShowAddModal(true);
  };

  const submitEdit = async (e) => {
    e.preventDefault();
    try {
      const updated = await updateProject(selectedProject.id, form);
      setProjects((arr) => arr.map((x) => x.id === updated.id ? updated : x));
      closeModal();
    } catch {
      setError('Failed to update project');
    }
  };

  const openTasks = async (p) => {
    setSelectedProject(p);
    try {
      const t = await listTasks(p.id);
      setTasks(t);
    } catch {
      setTasks([]);
    }
  };

  const addTask = async () => {
    if (!taskText.trim() || !selectedProject) return;
    try {
      const t = await createTask(selectedProject.id, { title: taskText });
      setTasks((prev) => [...prev, t]);
      setTaskText('');
    } catch {}
  };

  const removeTask = async (taskId) => {
    if (!selectedProject) return;
    try {
      await deleteTask(selectedProject.id, taskId);
      setTasks((prev) => prev.filter(t => t.id !== taskId));
    } catch {}
  };

  const getStatusColor = (status) => {
    const colors = {
      active: 'active',
      completed: 'completed',
      paused: 'paused',
      cancelled: 'cancelled'
    };
    return colors[status] || colors.active;
  };

  const closeModal = useCallback(() => {
    setShowAddModal(false);
    setSelectedProject(null);
    setForm({
      title: '',
      description: '',
      technologies: '',
      status: 'Draft',
      companyName: '',
      duration: '3 months',
      budget: ''
    });
  }, []);

  const handleInputChange = useCallback((field, value) => {
    setForm(prev => ({ ...prev, [field]: value }));
  }, []);

  const handleKeyDown = useCallback((e) => {
    // Prevent Enter key from submitting form unless on submit button
    if (e.key === 'Enter' && e.target.type !== 'submit' && e.target.tagName !== 'BUTTON') {
      e.preventDefault();
    }
  }, []);

  const addProjectModal = (
    <motion.div
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
      className="projects-modal-overlay"
      onClick={closeModal}
    >
      <motion.div
        initial={{ scale: 0.9, opacity: 0 }}
        animate={{ scale: 1, opacity: 1 }}
        className="projects-modal-content"
        onClick={(e) => e.stopPropagation()}
      >
        <h2 className="projects-modal-title">{selectedProject ? 'Edit Project' : 'Add New Project'}</h2>
        <form 
          className="projects-modal-form" 
          onSubmit={selectedProject ? submitEdit : submitAdd}
          onKeyDown={handleKeyDown}
        >
          <div className="projects-modal-row">
            <div className="projects-modal-field">
              <label className="projects-modal-label">Project Title</label>
              <input
                type="text"
                className="projects-modal-input"
                placeholder="Enter project title"
                value={form.title}
                onChange={(e) => handleInputChange('title', e.target.value)}
              />
            </div>
            <div className="projects-modal-field">
              <label className="projects-modal-label">Duration</label>
              <select 
                className="projects-modal-select"
                value={form.duration}
                onChange={(e) => handleInputChange('duration', e.target.value)}
              >
                <option value="1 month">1 month</option>
                <option value="2 months">2 months</option>
                <option value="3 months">3 months</option>
                <option value="6 months">6 months</option>
                <option value="1 year">1 year</option>
              </select>
            </div>
          </div>
          
          <div className="projects-modal-field">
            <label className="projects-modal-label">Description</label>
            <textarea
              rows={4}
              className="projects-modal-textarea"
              placeholder="Describe the project requirements..."
              value={form.description}
              onChange={(e) => handleInputChange('description', e.target.value)}
            />
          </div>

          <div className="projects-modal-row">
            <div className="projects-modal-field">
              <label className="projects-modal-label">Budget</label>
              <input
                type="text"
                className="projects-modal-input"
                value={form.budget}
                onChange={(e) => handleInputChange('budget', e.target.value)}
                placeholder="$0"
              />
            </div>
            <div className="projects-modal-field">
              <label className="projects-modal-label">Required Skills</label>
              <input
                type="text"
                className="projects-modal-input"
                placeholder="React, Node.js, MongoDB"
                value={form.technologies}
                onChange={(e) => handleInputChange('technologies', e.target.value)}
              />
            </div>
          </div>

          <div className="projects-modal-buttons">
            <button
              type="button"
              onClick={closeModal}
              className="projects-modal-button cancel"
            >
              Cancel
            </button>
            <button
              type="submit"
              className="projects-modal-button submit"
            >
              {selectedProject ? 'Save Changes' : 'Create Project'}
            </button>
          </div>
        </form>
      </motion.div>
    </motion.div>
  );

  return (
    <div className="projects-page">
      {/* Page Header */}
      <div className="projects-page-header">
        <div>
          <h1 className="projects-page-title">Projects</h1>
          <p className="projects-page-description">Manage your company's projects and opportunities</p>
        </div>
        <motion.button
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.95 }}
          onClick={openAdd}
          className="projects-add-button"
        >
          <span className="projects-add-icon">+</span>
          <span>Add Project</span>
        </motion.button>
      </div>

      {/* Projects Table */}
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        className="projects-table-container"
      >
        <div className="overflow-x-auto">
          <table className="projects-table">
            <thead className="projects-table-header">
              <tr>
                <th>Project Title</th>
                <th>Status</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {loading && (
                <tr><td colSpan={4} style={{ padding: 16 }}>Loading...</td></tr>
              )}
              {error && !loading && (
                <tr><td colSpan={4} style={{ padding: 16, color: 'red' }}>{error}</td></tr>
              )}
              {!loading && !error && projects.map((project, index) => (
                <motion.tr
                  key={project.id}
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ delay: index * 0.1 }}
                  className="projects-table-row"
                >
                  <td className="projects-table-cell">
                    <div className="project-info">
                      <div className="project-title">{project.title}</div>
                      <div className="project-description">{project.description}</div>
                      {project.technologies && (
                        <div className="project-skills">
                          {project.technologies.split(',').map((skill, idx) => (
                            <span key={idx} className="project-skill-tag">
                              {skill.trim()}
                            </span>
                          ))}
                        </div>
                      )}
                    </div>
                  </td>
                  <td className="projects-table-cell">
                    <span className={`project-status ${getStatusColor((project.status || 'draft').toLowerCase())}`}>
                      {(project.status || 'Draft').charAt(0).toUpperCase() + (project.status || 'Draft').slice(1)}
                    </span>
                  </td>
                  <td className="projects-table-cell">
                    <div className="project-actions">
                      <motion.button
                        whileHover={{ scale: 1.1 }}
                        whileTap={{ scale: 0.9 }}
                        className="project-action-button view"
                        title="View"
                        onClick={() => openTasks(project)}
                      >
                        <span className="project-action-icon">👁️</span>
                      </motion.button>
                      <motion.button
                        whileHover={{ scale: 1.1 }}
                        whileTap={{ scale: 0.9 }}
                        className="project-action-button edit"
                        title="Edit"
                        onClick={() => onEdit(project)}
                      >
                        <span className="project-action-icon">✏️</span>
                      </motion.button>
                      <motion.button
                        whileHover={{ scale: 1.1 }}
                        whileTap={{ scale: 0.9 }}
                        className="project-action-button delete"
                        title="Delete"
                        onClick={() => onDelete(project.id)}
                      >
                        <span className="project-action-icon">🗑️</span>
                      </motion.button>
                    </div>
                  </td>
                </motion.tr>
              ))}
            </tbody>
          </table>
        </div>
      </motion.div>

      {/* Tasks Panel (optional) */}
      {selectedProject && (
        <div style={{ marginTop: 24, background: '#fff', borderRadius: 8, padding: 16 }}>
          <h3 style={{ marginBottom: 8 }}>Tasks for: {selectedProject.title}</h3>
          <div style={{ display: 'flex', gap: 8, marginBottom: 8 }}>
            <input placeholder="New task title" value={taskText} onChange={(e) => setTaskText(e.target.value)} />
            <button onClick={addTask}>Add Task</button>
          </div>
          <ul>
            {tasks.map(t => (
              <li key={t.id} style={{ display: 'flex', justifyContent: 'space-between', padding: '4px 0' }}>
                <span>{t.title}</span>
                <button onClick={() => removeTask(t.id)}>Delete</button>
              </li>
            ))}
            {tasks.length === 0 && <li style={{ color: '#666' }}>No tasks</li>}
          </ul>
        </div>
      )}

      {/* Add Project Modal */}
      {showAddModal && addProjectModal}
    </div>
  );
};

export default Projects;