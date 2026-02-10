import React, { useEffect, useState, useCallback } from 'react';
import { motion } from 'framer-motion';
import './Projects.css';
import { createJob, deleteJob, listMyJobs, updateJob } from '../../../api/jobs';

const Jobs = () => {
  const [showAddModal, setShowAddModal] = useState(false);
  const [jobs, setJobs] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [form, setForm] = useState({
    title: '',
    description: '',
    requirements: '',
    skills: '',
    location: '',
    employmentType: 'Full-time',
    salaryMin: '',
    salaryMax: '',
    companyName: '',
  });
  const [selectedJob, setSelectedJob] = useState(null);

  const load = async () => {
    setLoading(true);
    try {
      const data = await listMyJobs();
      setJobs(data);
    } catch (e) {
      setError('Failed to load jobs');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load();
  }, []);

  const closeModal = useCallback(() => {
    setShowAddModal(false);
    setSelectedJob(null);
    const companyName = localStorage.getItem('companyName') || localStorage.getItem('userName') || 'My Company';
    setForm({
      title: '',
      description: '',
      requirements: '',
      skills: '',
      location: '',
      employmentType: 'Full-time',
      salaryMin: '',
      salaryMax: '',
      companyName: companyName,
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

  const openAdd = () => {
    // Get company name from localStorage
    const companyName = localStorage.getItem('companyName') || localStorage.getItem('userName') || 'My Company';
    
    setSelectedJob(null);
    setForm({
      title: '',
      description: '',
      requirements: '',
      skills: '',
      location: '',
      employmentType: 'Full-time',
      salaryMin: '',
      salaryMax: '',
      companyName: companyName, // Auto-fill company name
    });
    setSelectedJob(null);
    setShowAddModal(true);
  };

  const submitAdd = async (e) => {
    e.preventDefault();
    try {
      const payload = {
        title: form.title,
        description: form.description,
        requirements: form.requirements ? form.requirements.split(',').map(r => r.trim()) : [],
        skills: form.skills ? form.skills.split(',').map(s => s.trim()) : [],
        location: form.location,
        employmentType: form.employmentType,
        salaryMin: form.salaryMin ? parseFloat(form.salaryMin) : null,
        salaryMax: form.salaryMax ? parseFloat(form.salaryMax) : null,
        companyName: form.companyName,
      };
      const created = await createJob(payload);
      setJobs((p) => [created, ...p]);
      closeModal();
    } catch {
      setError('Failed to create job');
    }
  };

  const onDelete = async (id) => {
    if (!confirm('Delete job?')) return;
    try {
      await deleteJob(id);
      setJobs((p) => p.filter((x) => x.id !== id));
    } catch {
      setError('Failed to delete job');
    }
  };

  const onEdit = (job) => {
    // Get company name from localStorage
    const companyName = localStorage.getItem('companyName') || localStorage.getItem('userName') || 'My Company';
    
    setSelectedJob(job);
    setForm({
      title: job.title,
      description: job.description,
      requirements: job.requirements ? job.requirements.join(', ') : '',
      skills: job.skills ? job.skills.join(', ') : '',
      location: job.location || '',
      employmentType: job.employmentType || 'Full-time',
      salaryMin: job.salaryMin || '',
      salaryMax: job.salaryMax || '',
      companyName: companyName, // Always use current company name
    });
    setShowAddModal(true);
  };

  const submitEdit = async (e) => {
    e.preventDefault();
    try {
      const payload = {
        title: form.title,
        description: form.description,
        requirements: form.requirements ? form.requirements.split(',').map(r => r.trim()) : [],
        skills: form.skills ? form.skills.split(',').map(s => s.trim()) : [],
        location: form.location,
        employmentType: form.employmentType,
        salaryMin: form.salaryMin ? parseFloat(form.salaryMin) : null,
        salaryMax: form.salaryMax ? parseFloat(form.salaryMax) : null,
        companyName: form.companyName,
      };
      const updated = await updateJob(selectedJob.id, payload);
      setJobs((arr) => arr.map((x) => (x.id === selectedJob.id ? { ...x, ...updated } : x)));
      closeModal();
    } catch {
      setError('Failed to update job');
    }
  };

  const getStatusColor = (status) => {
    const colors = {
      active: 'active',
      closed: 'cancelled',
      expired: 'paused',
    };
    return colors[(status || 'active').toLowerCase()] || colors.active;
  };

  const addJobModal = (
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
        <h2 className="projects-modal-title">{selectedJob ? 'Edit Job' : 'Add New Job'}</h2>
        <form 
          className="projects-modal-form" 
          onSubmit={selectedJob ? submitEdit : submitAdd}
          onKeyDown={handleKeyDown}
        >
          <div className="projects-modal-row">
            <div className="projects-modal-field">
              <label className="projects-modal-label">Job Title</label>
              <input
                type="text"
                className="projects-modal-input"
                placeholder="Enter job title"
                value={form.title}
                onChange={(e) => handleInputChange('title', e.target.value)}
                required
              />
            </div>
            <div className="projects-modal-field">
              <label className="projects-modal-label">Company Name</label>
              <input
                type="text"
                className="projects-modal-input"
                placeholder="Company name"
                value={form.companyName}
                readOnly
                disabled
                style={{ backgroundColor: '#f0f0f0', cursor: 'not-allowed' }}
              />
            </div>
          </div>

          <div className="projects-modal-field">
            <label className="projects-modal-label">Description</label>
            <textarea
              rows={4}
              className="projects-modal-textarea"
              placeholder="Describe the job requirements..."
              value={form.description}
              onChange={(e) => handleInputChange('description', e.target.value)}
              required
            />
          </div>

          <div className="projects-modal-row">
            <div className="projects-modal-field">
              <label className="projects-modal-label">Location</label>
              <input
                type="text"
                className="projects-modal-input"
                placeholder="e.g. Cairo, Egypt"
                value={form.location}
                onChange={(e) => handleInputChange('location', e.target.value)}
              />
            </div>
            <div className="projects-modal-field">
              <label className="projects-modal-label">Employment Type</label>
              <select
                className="projects-modal-select"
                value={form.employmentType}
                onChange={(e) => handleInputChange('employmentType', e.target.value)}
              >
                <option>Full-time</option>
                <option>Part-time</option>
                <option>Contract</option>
                <option>Remote</option>
              </select>
            </div>
          </div>

          <div className="projects-modal-row">
            <div className="projects-modal-field">
              <label className="projects-modal-label">Salary Min</label>
              <input
                type="number"
                className="projects-modal-input"
                placeholder="$0"
                value={form.salaryMin}
                onChange={(e) => handleInputChange('salaryMin', e.target.value)}
              />
            </div>
            <div className="projects-modal-field">
              <label className="projects-modal-label">Salary Max</label>
              <input
                type="number"
                className="projects-modal-input"
                placeholder="$0"
                value={form.salaryMax}
                onChange={(e) => handleInputChange('salaryMax', e.target.value)}
              />
            </div>
          </div>

          <div className="projects-modal-row">
            <div className="projects-modal-field">
              <label className="projects-modal-label">Requirements (comma-separated)</label>
              <input
                type="text"
                className="projects-modal-input"
                placeholder="Bachelor degree, 2+ years experience"
                value={form.requirements}
                onChange={(e) => handleInputChange('requirements', e.target.value)}
              />
            </div>
            <div className="projects-modal-field">
              <label className="projects-modal-label">Skills (comma-separated)</label>
              <input
                type="text"
                className="projects-modal-input"
                placeholder="React, Node.js, MongoDB"
                value={form.skills}
                onChange={(e) => handleInputChange('skills', e.target.value)}
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
            <button type="submit" className="projects-modal-button submit">
              {selectedJob ? 'Save Changes' : 'Create Job'}
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
          <h1 className="projects-page-title">Jobs</h1>
          <p className="projects-page-description">Manage your company's job postings</p>
        </div>
        <motion.button
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.95 }}
          onClick={openAdd}
          className="projects-add-button"
        >
          <span className="projects-add-icon">+</span>
          <span>Add Job</span>
        </motion.button>
      </div>

      {/* Jobs Table */}
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        className="projects-table-container"
      >
        <div className="overflow-x-auto">
          <table className="projects-table">
            <thead className="projects-table-header">
              <tr>
                <th>Job Title</th>
                <th>Location</th>
                <th>Type</th>
                <th>Status</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {loading && (
                <tr>
                  <td colSpan={5} style={{ padding: 16 }}>
                    Loading...
                  </td>
                </tr>
              )}
              {error && !loading && (
                <tr>
                  <td colSpan={5} style={{ padding: 16, color: 'red' }}>
                    {error}
                  </td>
                </tr>
              )}
              {!loading && !error && jobs.map((job, index) => (
                <motion.tr
                  key={job.id}
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ delay: index * 0.1 }}
                  className="projects-table-row"
                >
                  <td className="projects-table-cell">
                    <div className="project-info">
                      <div className="project-title">{job.title}</div>
                      <div className="project-description">{job.description}</div>
                      {job.skills && (
                        <div className="project-skills">
                          {job.skills.map((skill, idx) => (
                            <span key={idx} className="project-skill-tag">
                              {skill}
                            </span>
                          ))}
                        </div>
                      )}
                    </div>
                  </td>
                  <td className="projects-table-cell">{job.location || 'N/A'}</td>
                  <td className="projects-table-cell">{job.employmentType || 'N/A'}</td>
                  <td className="projects-table-cell">
                    <span className={`project-status ${getStatusColor(job.status)}`}>
                      {(job.status || 'Active').charAt(0).toUpperCase() + (job.status || 'Active').slice(1)}
                    </span>
                  </td>
                  <td className="projects-table-cell">
                    <div className="project-actions">
                      <motion.button
                        whileHover={{ scale: 1.1 }}
                        whileTap={{ scale: 0.9 }}
                        className="project-action-button edit"
                        title="Edit"
                        onClick={() => onEdit(job)}
                      >
                        <span className="project-action-icon">✏️</span>
                      </motion.button>
                      <motion.button
                        whileHover={{ scale: 1.1 }}
                        whileTap={{ scale: 0.9 }}
                        className="project-action-button delete"
                        title="Delete"
                        onClick={() => onDelete(job.id)}
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

      {/* Add Job Modal */}
      {showAddModal && addJobModal}
    </div>
  );
};

export default Jobs;


