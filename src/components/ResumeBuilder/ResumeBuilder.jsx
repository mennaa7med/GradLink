import React, { useState, useEffect, useRef } from 'react';
import { motion } from 'framer-motion';
import { 
  FiDownload, FiPlus, FiTrash2, FiEdit2, FiSave,
  FiMail, FiPhone, FiMapPin, FiLinkedin, FiGithub, FiGlobe,
  FiBriefcase, FiBook, FiAward, FiCode
} from 'react-icons/fi';
import { useAuth } from '../../contexts/AuthContext';
import './ResumeBuilder.css';

const ResumeBuilder = () => {
  const { user } = useAuth();
  const resumeRef = useRef(null);
  const [selectedTemplate, setSelectedTemplate] = useState('modern');
  
  const [resumeData, setResumeData] = useState({
    personalInfo: {
      fullName: user?.fullName || '',
      email: user?.email || '',
      phone: '',
      location: '',
      linkedIn: '',
      github: '',
      website: '',
      summary: ''
    },
    experience: [],
    education: [],
    skills: [],
    projects: [],
    certifications: []
  });

  const [editingSection, setEditingSection] = useState(null);

  useEffect(() => {
    // Load saved resume data from localStorage
    const savedResume = localStorage.getItem('resumeBuilderData');
    if (savedResume) {
      setResumeData(JSON.parse(savedResume));
    }
  }, []);

  const saveResume = () => {
    localStorage.setItem('resumeBuilderData', JSON.stringify(resumeData));
    alert('Resume saved successfully!');
  };

  const updatePersonalInfo = (field, value) => {
    setResumeData(prev => ({
      ...prev,
      personalInfo: { ...prev.personalInfo, [field]: value }
    }));
  };

  const addItem = (section) => {
    const newItem = section === 'experience' ? {
      id: Date.now(),
      title: '',
      company: '',
      location: '',
      startDate: '',
      endDate: '',
      current: false,
      description: ''
    } : section === 'education' ? {
      id: Date.now(),
      degree: '',
      school: '',
      location: '',
      graduationDate: '',
      gpa: ''
    } : section === 'projects' ? {
      id: Date.now(),
      name: '',
      description: '',
      technologies: '',
      link: ''
    } : section === 'certifications' ? {
      id: Date.now(),
      name: '',
      issuer: '',
      date: '',
      link: ''
    } : section === 'skills' ? {
      id: Date.now(),
      category: '',
      items: ''
    } : {};

    setResumeData(prev => ({
      ...prev,
      [section]: [...prev[section], newItem]
    }));
    setEditingSection({ section, id: newItem.id });
  };

  const updateItem = (section, id, field, value) => {
    setResumeData(prev => ({
      ...prev,
      [section]: prev[section].map(item => 
        item.id === id ? { ...item, [field]: value } : item
      )
    }));
  };

  const removeItem = (section, id) => {
    setResumeData(prev => ({
      ...prev,
      [section]: prev[section].filter(item => item.id !== id)
    }));
  };

  const downloadPDF = () => {
    window.print();
  };

  const templates = [
    { id: 'modern', name: 'Modern', color: '#0c2737' },
    { id: 'classic', name: 'Classic', color: '#1a365d' },
    { id: 'creative', name: 'Creative', color: '#7c3aed' }
  ];

  return (
    <div className="resume-builder">
      {/* Toolbar */}
      <div className="builder-toolbar">
        <div className="template-selector">
          <span>Template:</span>
          {templates.map(t => (
            <button
              key={t.id}
              className={`template-btn ${selectedTemplate === t.id ? 'active' : ''}`}
              onClick={() => setSelectedTemplate(t.id)}
              style={{ '--template-color': t.color }}
            >
              {t.name}
            </button>
          ))}
        </div>
        <div className="toolbar-actions">
          <button className="save-btn" onClick={saveResume}>
            <FiSave /> Save
          </button>
          <button className="download-btn" onClick={downloadPDF}>
            <FiDownload /> Download PDF
          </button>
        </div>
      </div>

      <div className="builder-content">
        {/* Editor Panel */}
        <div className="editor-panel">
          {/* Personal Info */}
          <section className="editor-section">
            <h3><FiEdit2 /> Personal Information</h3>
            <div className="form-grid">
              <input
                type="text"
                placeholder="Full Name"
                value={resumeData.personalInfo.fullName}
                onChange={(e) => updatePersonalInfo('fullName', e.target.value)}
              />
              <input
                type="email"
                placeholder="Email"
                value={resumeData.personalInfo.email}
                onChange={(e) => updatePersonalInfo('email', e.target.value)}
              />
              <input
                type="tel"
                placeholder="Phone"
                value={resumeData.personalInfo.phone}
                onChange={(e) => updatePersonalInfo('phone', e.target.value)}
              />
              <input
                type="text"
                placeholder="Location"
                value={resumeData.personalInfo.location}
                onChange={(e) => updatePersonalInfo('location', e.target.value)}
              />
              <input
                type="url"
                placeholder="LinkedIn URL"
                value={resumeData.personalInfo.linkedIn}
                onChange={(e) => updatePersonalInfo('linkedIn', e.target.value)}
              />
              <input
                type="url"
                placeholder="GitHub URL"
                value={resumeData.personalInfo.github}
                onChange={(e) => updatePersonalInfo('github', e.target.value)}
              />
            </div>
            <textarea
              placeholder="Professional Summary"
              value={resumeData.personalInfo.summary}
              onChange={(e) => updatePersonalInfo('summary', e.target.value)}
              rows="3"
            />
          </section>

          {/* Experience */}
          <section className="editor-section">
            <div className="section-header">
              <h3><FiBriefcase /> Experience</h3>
              <button className="add-btn" onClick={() => addItem('experience')}>
                <FiPlus /> Add
              </button>
            </div>
            {resumeData.experience.map(exp => (
              <div key={exp.id} className="item-card">
                <input
                  placeholder="Job Title"
                  value={exp.title}
                  onChange={(e) => updateItem('experience', exp.id, 'title', e.target.value)}
                />
                <input
                  placeholder="Company"
                  value={exp.company}
                  onChange={(e) => updateItem('experience', exp.id, 'company', e.target.value)}
                />
                <div className="date-row">
                  <input
                    type="month"
                    placeholder="Start Date"
                    value={exp.startDate}
                    onChange={(e) => updateItem('experience', exp.id, 'startDate', e.target.value)}
                  />
                  <input
                    type="month"
                    placeholder="End Date"
                    value={exp.endDate}
                    onChange={(e) => updateItem('experience', exp.id, 'endDate', e.target.value)}
                    disabled={exp.current}
                  />
                  <label className="current-label">
                    <input
                      type="checkbox"
                      checked={exp.current}
                      onChange={(e) => updateItem('experience', exp.id, 'current', e.target.checked)}
                    />
                    Current
                  </label>
                </div>
                <textarea
                  placeholder="Description (use bullet points with -)"
                  value={exp.description}
                  onChange={(e) => updateItem('experience', exp.id, 'description', e.target.value)}
                  rows="3"
                />
                <button className="remove-btn" onClick={() => removeItem('experience', exp.id)}>
                  <FiTrash2 /> Remove
                </button>
              </div>
            ))}
          </section>

          {/* Education */}
          <section className="editor-section">
            <div className="section-header">
              <h3><FiBook /> Education</h3>
              <button className="add-btn" onClick={() => addItem('education')}>
                <FiPlus /> Add
              </button>
            </div>
            {resumeData.education.map(edu => (
              <div key={edu.id} className="item-card">
                <input
                  placeholder="Degree"
                  value={edu.degree}
                  onChange={(e) => updateItem('education', edu.id, 'degree', e.target.value)}
                />
                <input
                  placeholder="School/University"
                  value={edu.school}
                  onChange={(e) => updateItem('education', edu.id, 'school', e.target.value)}
                />
                <input
                  type="month"
                  placeholder="Graduation Date"
                  value={edu.graduationDate}
                  onChange={(e) => updateItem('education', edu.id, 'graduationDate', e.target.value)}
                />
                <input
                  placeholder="GPA (optional)"
                  value={edu.gpa}
                  onChange={(e) => updateItem('education', edu.id, 'gpa', e.target.value)}
                />
                <button className="remove-btn" onClick={() => removeItem('education', edu.id)}>
                  <FiTrash2 /> Remove
                </button>
              </div>
            ))}
          </section>

          {/* Skills */}
          <section className="editor-section">
            <div className="section-header">
              <h3><FiCode /> Skills</h3>
              <button className="add-btn" onClick={() => addItem('skills')}>
                <FiPlus /> Add Category
              </button>
            </div>
            {resumeData.skills.map(skill => (
              <div key={skill.id} className="item-card">
                <input
                  placeholder="Category (e.g., Programming Languages)"
                  value={skill.category}
                  onChange={(e) => updateItem('skills', skill.id, 'category', e.target.value)}
                />
                <input
                  placeholder="Skills (comma separated)"
                  value={skill.items}
                  onChange={(e) => updateItem('skills', skill.id, 'items', e.target.value)}
                />
                <button className="remove-btn" onClick={() => removeItem('skills', skill.id)}>
                  <FiTrash2 /> Remove
                </button>
              </div>
            ))}
          </section>

          {/* Projects */}
          <section className="editor-section">
            <div className="section-header">
              <h3><FiCode /> Projects</h3>
              <button className="add-btn" onClick={() => addItem('projects')}>
                <FiPlus /> Add
              </button>
            </div>
            {resumeData.projects.map(proj => (
              <div key={proj.id} className="item-card">
                <input
                  placeholder="Project Name"
                  value={proj.name}
                  onChange={(e) => updateItem('projects', proj.id, 'name', e.target.value)}
                />
                <input
                  placeholder="Technologies Used"
                  value={proj.technologies}
                  onChange={(e) => updateItem('projects', proj.id, 'technologies', e.target.value)}
                />
                <input
                  placeholder="Project Link (optional)"
                  value={proj.link}
                  onChange={(e) => updateItem('projects', proj.id, 'link', e.target.value)}
                />
                <textarea
                  placeholder="Description"
                  value={proj.description}
                  onChange={(e) => updateItem('projects', proj.id, 'description', e.target.value)}
                  rows="2"
                />
                <button className="remove-btn" onClick={() => removeItem('projects', proj.id)}>
                  <FiTrash2 /> Remove
                </button>
              </div>
            ))}
          </section>
        </div>

        {/* Preview Panel */}
        <div className="preview-panel">
          <div className={`resume-preview template-${selectedTemplate}`} ref={resumeRef}>
            {/* Header */}
            <div className="resume-header">
              <h1>{resumeData.personalInfo.fullName || 'Your Name'}</h1>
              <div className="contact-info">
                {resumeData.personalInfo.email && (
                  <span><FiMail /> {resumeData.personalInfo.email}</span>
                )}
                {resumeData.personalInfo.phone && (
                  <span><FiPhone /> {resumeData.personalInfo.phone}</span>
                )}
                {resumeData.personalInfo.location && (
                  <span><FiMapPin /> {resumeData.personalInfo.location}</span>
                )}
              </div>
              <div className="social-links">
                {resumeData.personalInfo.linkedIn && (
                  <span><FiLinkedin /> LinkedIn</span>
                )}
                {resumeData.personalInfo.github && (
                  <span><FiGithub /> GitHub</span>
                )}
              </div>
            </div>

            {/* Summary */}
            {resumeData.personalInfo.summary && (
              <div className="resume-section">
                <h2>Professional Summary</h2>
                <p>{resumeData.personalInfo.summary}</p>
              </div>
            )}

            {/* Experience */}
            {resumeData.experience.length > 0 && (
              <div className="resume-section">
                <h2>Experience</h2>
                {resumeData.experience.map(exp => (
                  <div key={exp.id} className="resume-item">
                    <div className="item-header">
                      <strong>{exp.title}</strong>
                      <span className="date">
                        {exp.startDate} - {exp.current ? 'Present' : exp.endDate}
                      </span>
                    </div>
                    <div className="item-sub">{exp.company}</div>
                    <p className="item-desc">{exp.description}</p>
                  </div>
                ))}
              </div>
            )}

            {/* Education */}
            {resumeData.education.length > 0 && (
              <div className="resume-section">
                <h2>Education</h2>
                {resumeData.education.map(edu => (
                  <div key={edu.id} className="resume-item">
                    <div className="item-header">
                      <strong>{edu.degree}</strong>
                      <span className="date">{edu.graduationDate}</span>
                    </div>
                    <div className="item-sub">{edu.school}</div>
                    {edu.gpa && <div className="item-gpa">GPA: {edu.gpa}</div>}
                  </div>
                ))}
              </div>
            )}

            {/* Skills */}
            {resumeData.skills.length > 0 && (
              <div className="resume-section">
                <h2>Skills</h2>
                {resumeData.skills.map(skill => (
                  <div key={skill.id} className="skill-row">
                    <strong>{skill.category}:</strong> {skill.items}
                  </div>
                ))}
              </div>
            )}

            {/* Projects */}
            {resumeData.projects.length > 0 && (
              <div className="resume-section">
                <h2>Projects</h2>
                {resumeData.projects.map(proj => (
                  <div key={proj.id} className="resume-item">
                    <div className="item-header">
                      <strong>{proj.name}</strong>
                      {proj.technologies && <span className="tech">{proj.technologies}</span>}
                    </div>
                    <p className="item-desc">{proj.description}</p>
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default ResumeBuilder;















