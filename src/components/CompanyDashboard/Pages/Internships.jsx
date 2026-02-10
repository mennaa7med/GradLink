import React, { useEffect, useState, useCallback } from 'react';
import { motion } from 'framer-motion';
import './Projects.css';
import { createInternship, deleteInternship, listMyInternships, updateInternship } from '../../../api/internships';

const Internships = () => {
  const [showAddModal, setShowAddModal] = useState(false);
  const [internships, setInternships] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [form, setForm] = useState({
    title: '',
    description: '',
    requirements: '',
    skills: '',
    location: '',
    duration: '3 months',
    isPaid: false,
    stipend: '',
    companyName: '',
  });
  const [selectedInternship, setSelectedInternship] = useState(null);

  const load = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await listMyInternships();
      setInternships(data);
    } catch (e) {
      console.error('Failed to load internships:', e);
      console.error('Error response:', e.response);
      
      if (e.response?.status === 404) {
        setError('Internships endpoint not found. Please make sure backend migration is complete.');
      } else if (e.response?.status === 401) {
        setError('Not authorized. Please login again.');
      } else if (e.response?.status === 500) {
        setError('Server error. The Internships table might not exist in database.');
      } else {
        setError('Failed to load internships: ' + (e.response?.data?.message || e.message));
      }
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load();
  }, []);

  const closeModal = useCallback(() => {
    setShowAddModal(false);
    setSelectedInternship(null);
    const companyName = localStorage.getItem('companyName') || localStorage.getItem('userName') || 'My Company';
    setForm({
      title: '',
      description: '',
      requirements: '',
      skills: '',
      location: '',
      duration: '3 months',
      isPaid: false,
      stipend: '',
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
    
    setForm({
      title: '',
      description: '',
      requirements: '',
      skills: '',
      location: '',
      duration: '3 months',
      isPaid: false,
      stipend: '',
      companyName: companyName, // Auto-fill company name
    });
    setSelectedInternship(null);
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
        duration: form.duration,
        isPaid: form.isPaid,
        stipend: form.isPaid && form.stipend ? parseFloat(form.stipend) : null,
        companyName: form.companyName,
      };
      
      const created = await createInternship(payload);
      
      setInternships((p) => [created, ...p]);
      closeModal();
      setError(null);
      alert('Internship created successfully!');
    } catch (err) {
      console.error('Failed to create internship:', err);
      console.error('Error response:', err.response);
      
      let errorMessage = 'Failed to create internship';
      
      if (err.response?.status === 401) {
        errorMessage = 'Not authorized. Please login again.';
      } else if (err.response?.status === 400) {
        errorMessage = 'Invalid data. Please check all required fields: ' + JSON.stringify(err.response.data);
      } else if (err.response?.data?.errors) {
        errorMessage = 'Validation errors: ' + JSON.stringify(err.response.data.errors);
      } else if (err.response?.data?.message) {
        errorMessage = err.response.data.message;
      } else if (err.message) {
        errorMessage = err.message;
      }
      
      setError(errorMessage);
      alert('Error: ' + errorMessage);
    }
  };

  const onDelete = async (id) => {
    if (!confirm('Delete internship?')) return;
    try {
      await deleteInternship(id);
      setInternships((p) => p.filter((x) => x.id !== id));
    } catch {
      setError('Failed to delete internship');
    }
  };

  const onEdit = (internship) => {
    // Get company name from localStorage
    const companyName = localStorage.getItem('companyName') || localStorage.getItem('userName') || 'My Company';
    
    setSelectedInternship(internship);
    setForm({
      title: internship.title,
      description: internship.description,
      requirements: internship.requirements ? internship.requirements.join(', ') : '',
      skills: internship.skills ? internship.skills.join(', ') : '',
      location: internship.location || '',
      duration: internship.duration || '3 months',
      isPaid: internship.isPaid || false,
      stipend: internship.stipend || '',
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
        duration: form.duration,
        isPaid: form.isPaid,
        stipend: form.isPaid && form.stipend ? parseFloat(form.stipend) : null,
        companyName: form.companyName,
      };
      const updated = await updateInternship(selectedInternship.id, payload);
      setInternships((arr) => arr.map((x) => (x.id === selectedInternship.id ? { ...x, ...updated } : x)));
      closeModal();
    } catch {
      setError('Failed to update internship');
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

  const addInternshipModal = (
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
        <h2 className="projects-modal-title">{selectedInternship ? 'Edit Internship' : 'Add New Internship'}</h2>
        <form 
          className="projects-modal-form" 
          onSubmit={selectedInternship ? submitEdit : submitAdd}
          onKeyDown={handleKeyDown}
        >
          <div className="projects-modal-row">
            <div className="projects-modal-field">
              <label className="projects-modal-label">Internship Title</label>
              <input
                type="text"
                className="projects-modal-input"
                placeholder="Enter internship title"
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
              placeholder="Describe the internship requirements..."
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
              <label className="projects-modal-label">Duration</label>
              <select
                className="projects-modal-select"
                value={form.duration}
                onChange={(e) => handleInputChange('duration', e.target.value)}
              >
                <option>1 month</option>
                <option>2 months</option>
                <option>3 months</option>
                <option>6 months</option>
                <option>1 year</option>
              </select>
            </div>
          </div>

          <div className="projects-modal-row">
            <div className="projects-modal-field">
              <label className="projects-modal-label">
                <input
                  type="checkbox"
                  checked={form.isPaid}
                  onChange={(e) => handleInputChange('isPaid', e.target.checked)}
                  style={{ marginRight: '8px' }}
                />
                Paid Internship
              </label>
            </div>
            {form.isPaid && (
              <div className="projects-modal-field">
                <label className="projects-modal-label">Stipend</label>
                <input
                  type="number"
                  className="projects-modal-input"
                  placeholder="$0"
                  value={form.stipend}
                  onChange={(e) => handleInputChange('stipend', e.target.value)}
                />
              </div>
            )}
          </div>

          <div className="projects-modal-row">
            <div className="projects-modal-field">
              <label className="projects-modal-label">Requirements (comma-separated)</label>
              <input
                type="text"
                className="projects-modal-input"
                placeholder="Bachelor student, Good communication"
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
              {selectedInternship ? 'Save Changes' : 'Create Internship'}
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
          <h1 className="projects-page-title">Internships</h1>
          <p className="projects-page-description">Manage your company's internship programs</p>
        </div>
        <motion.button
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.95 }}
          onClick={openAdd}
          className="projects-add-button"
        >
          <span className="projects-add-icon">+</span>
          <span>Add Internship</span>
        </motion.button>
      </div>

      {/* Internships Table */}
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        className="projects-table-container"
      >
        <div className="overflow-x-auto">
          <table className="projects-table">
            <thead className="projects-table-header">
              <tr>
                <th>Internship Title</th>
                <th>Location</th>
                <th>Duration</th>
                <th>Type</th>
                <th>Status</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {loading && (
                <tr>
                  <td colSpan={6} style={{ padding: 16 }}>
                    Loading...
                  </td>
                </tr>
              )}
              {error && !loading && (
                <tr>
                  <td colSpan={6} style={{ padding: 16, color: 'red' }}>
                    {error}
                  </td>
                </tr>
              )}
              {!loading && !error && internships.map((internship, index) => (
                <motion.tr
                  key={internship.id}
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ delay: index * 0.1 }}
                  className="projects-table-row"
                >
                  <td className="projects-table-cell">
                    <div className="project-info">
                      <div className="project-title">{internship.title}</div>
                      <div className="project-description">{internship.description}</div>
                      {internship.skills && (
                        <div className="project-skills">
                          {internship.skills.map((skill, idx) => (
                            <span key={idx} className="project-skill-tag">
                              {skill}
                            </span>
                          ))}
                        </div>
                      )}
                    </div>
                  </td>
                  <td className="projects-table-cell">{internship.location || 'N/A'}</td>
                  <td className="projects-table-cell">{internship.duration || 'N/A'}</td>
                  <td className="projects-table-cell">{internship.isPaid ? 'Paid' : 'Unpaid'}</td>
                  <td className="projects-table-cell">
                    <span className={`project-status ${getStatusColor(internship.status)}`}>
                      {(internship.status || 'Active').charAt(0).toUpperCase() + (internship.status || 'Active').slice(1)}
                    </span>
                  </td>
                  <td className="projects-table-cell">
                    <div className="project-actions">
                      <motion.button
                        whileHover={{ scale: 1.1 }}
                        whileTap={{ scale: 0.9 }}
                        className="project-action-button edit"
                        title="Edit"
                        onClick={() => onEdit(internship)}
                      >
                        <span className="project-action-icon">✏️</span>
                      </motion.button>
                      <motion.button
                        whileHover={{ scale: 1.1 }}
                        whileTap={{ scale: 0.9 }}
                        className="project-action-button delete"
                        title="Delete"
                        onClick={() => onDelete(internship.id)}
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

      {/* Add Internship Modal */}
      {showAddModal && addInternshipModal}
    </div>
  );
};

export default Internships;

