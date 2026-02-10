import React, { useState, useEffect } from 'react';
import { FiPlus, FiSearch, FiMail, FiFilter, FiStar, FiCalendar, FiTrash2, FiEdit } from 'react-icons/fi';
import './Teams.css';
import * as teamMembersApi from '../api/teamMembers';
import * as projectsApi from '../api/projects';

const Teams = () => {
  const [searchQuery, setSearchQuery] = useState('');
  const [roleFilter, setRoleFilter] = useState('All');
  const [showAddMemberModal, setShowAddMemberModal] = useState(false);
  const [showFilters, setShowFilters] = useState(false);
  const [skillFilter, setSkillFilter] = useState('All');
  const [availabilityFilter, setAvailabilityFilter] = useState('All');
  const [selectedMember, setSelectedMember] = useState(null);
  const [showMemberDetails, setShowMemberDetails] = useState(false);
  const [newTask, setNewTask] = useState('');
  const [editingTask, setEditingTask] = useState({ taskId: null, text: '' });
  const [showScheduleModal, setShowScheduleModal] = useState(false);
  const [showMessageModal, setShowMessageModal] = useState(false);
  const [messageText, setMessageText] = useState('');
  const [meetingDetails, setMeetingDetails] = useState({
    date: '',
    time: '',
    duration: '30',
    agenda: ''
  });
  
  // State for data from API
  const [teamMembers, setTeamMembers] = useState([]);
  const [projects, setProjects] = useState([]);
  const [selectedProjectId, setSelectedProjectId] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const roles = ['All', 'Frontend Developer', 'Backend Developer', 'UI/UX Designer', 'Project Manager', 'AI Engineer'];
  const skills = ['All', 'React', 'JavaScript', 'Python', 'Node.js', 'UI/UX', 'Database', 'Machine Learning'];
  const availabilityOptions = ['All', 'Full-time', 'Part-time', 'Weekends only'];

  // Load from localStorage on mount
  useEffect(() => {
    const savedMembers = localStorage.getItem('dashboardTeamMembers');
    if (savedMembers) {
      try {
        const parsed = JSON.parse(savedMembers);
        setTeamMembers(parsed);
      } catch (e) {
        console.error('Failed to parse saved members:', e);
      }
    }
    setLoading(false);
  }, []);

  // Load projects on mount
  useEffect(() => {
    loadProjects();
  }, []);

  // Load team members when project is selected
  useEffect(() => {
    if (selectedProjectId) {
      loadTeamMembers(selectedProjectId);
    }
  }, [selectedProjectId]);

  const loadProjects = async () => {
    try {
      setLoading(true);
      const data = await projectsApi.listProjects();
      setProjects(data);
      if (data.length > 0) {
        setSelectedProjectId(data[0].id);
      }
    } catch (err) {
      console.error('Failed to load projects:', err);
      setError('Failed to load projects. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const loadTeamMembers = async (projectId) => {
    try {
      setLoading(true);
      const data = await teamMembersApi.listTeamMembers(projectId);
      // Transform data to match component expectations
      const transformedData = data.map(member => ({
        ...member,
        name: member.fullName,
        avatar: member.profileImageUrl || `https://ui-avatars.com/api/?name=${encodeURIComponent(member.fullName)}&background=random`,
        skills: member.skills ? member.skills.split(',').map(s => s.trim()) : [],
        tasks: member.tasks ? member.tasks.map(t => ({ id: t.id, title: t.title, status: t.status, priority: t.priority })) : []
      }));
      setTeamMembers(transformedData);
      setError(null);
    } catch (err) {
      console.error('Failed to load team members:', err);
      setError('Failed to load team members.');
    } finally {
      setLoading(false);
    }
  };
  
  const filteredMembers = teamMembers.filter(member => {
    const memberName = member.name || member.fullName || '';
    const memberSkills = Array.isArray(member.skills) ? member.skills : [];
    return (
      memberName.toLowerCase().includes(searchQuery.toLowerCase()) &&
      (roleFilter === 'All' || member.role === roleFilter) &&
      (skillFilter === 'All' || memberSkills.includes(skillFilter)) &&
      (availabilityFilter === 'All' || member.availability === availabilityFilter)
    );
  });

  const handleAddMember = async (newMember) => {
    // Create member object
    const memberData = {
      id: Date.now(),
      fullName: newMember.name,
      name: newMember.name,
      email: newMember.email,
      role: newMember.role,
      skills: newMember.skills,
      availability: newMember.availability,
      university: newMember.university,
      graduationYear: newMember.graduationYear ? parseInt(newMember.graduationYear) : null,
      profileImageUrl: newMember.photo,
      avatar: newMember.photo || `https://ui-avatars.com/api/?name=${encodeURIComponent(newMember.name)}&background=random`,
      createdAt: new Date().toISOString()
    };
    
    // Save to localStorage for dashboard display
    const savedMembers = localStorage.getItem('dashboardTeamMembers');
    const currentMembers = savedMembers ? JSON.parse(savedMembers) : [];
    const updatedMembers = [memberData, ...currentMembers];
    localStorage.setItem('dashboardTeamMembers', JSON.stringify(updatedMembers));
    
    // Update local state
    setTeamMembers(prev => [{ ...memberData, skills: memberData.skills ? memberData.skills.split(',').map(s => s.trim()) : [] }, ...prev]);
    setShowAddMemberModal(false);
    
    // Dispatch event to update dashboard
    window.dispatchEvent(new Event('teamMemberUpdated'));
    
    // Try to save to API if project is selected
    if (selectedProjectId) {
      try {
        await teamMembersApi.createTeamMember({
          ...memberData,
          projectId: selectedProjectId
        });
      } catch (err) {
        console.error('Failed to save to API:', err);
      }
    }
  };

  const handleDeleteMember = async (id) => {
    if (!window.confirm('Are you sure you want to remove this team member?')) return;
    
    // Remove from localStorage
    const savedMembers = localStorage.getItem('dashboardTeamMembers');
    if (savedMembers) {
      const currentMembers = JSON.parse(savedMembers);
      const updatedMembers = currentMembers.filter(m => m.id !== id);
      localStorage.setItem('dashboardTeamMembers', JSON.stringify(updatedMembers));
    }
    
    // Update local state
    setTeamMembers(prev => prev.filter(m => m.id !== id));
    
    if (showMemberDetails && selectedMember?.id === id) {
      setShowMemberDetails(false);
    }
    
    // Dispatch event to update dashboard
    window.dispatchEvent(new Event('teamMemberUpdated'));
    
    // Try to delete from API
    try {
      await teamMembersApi.deleteTeamMember(id);
    } catch (err) {
      console.error('Failed to delete from API:', err);
    }
  };

  const viewMemberDetails = async (member) => {
    // First try to use local data
    const transformedMember = {
      ...member,
      name: member.name || member.fullName,
      avatar: member.avatar || member.profileImageUrl || `https://ui-avatars.com/api/?name=${encodeURIComponent(member.name || member.fullName || 'User')}&background=random`,
      skills: Array.isArray(member.skills) ? member.skills : (member.skills ? member.skills.split(',').map(s => s.trim()) : []),
      tasks: member.tasks || []
    };
    setSelectedMember(transformedMember);
    setShowMemberDetails(true);
    
    // Try to fetch fresh data from API (optional)
    try {
      const freshData = await teamMembersApi.getTeamMember(member.id);
      if (freshData) {
        const apiMember = {
          ...freshData,
          name: freshData.fullName,
          avatar: freshData.profileImageUrl || `https://ui-avatars.com/api/?name=${encodeURIComponent(freshData.fullName)}&background=random`,
          skills: freshData.skills ? freshData.skills.split(',').map(s => s.trim()) : [],
          tasks: freshData.tasks || []
        };
        setSelectedMember(apiMember);
      }
    } catch (err) {
      // Silently fail - we already have local data
      console.log('API fetch failed, using local data');
    }
  };

  const handleAddTask = async () => {
    if (!newTask.trim() || !selectedMember) return;
    
    try {
      await teamMembersApi.createMemberTask({
        title: newTask,
        teamMemberId: selectedMember.id,
        priority: 'Medium'
      });
      
      // Refresh member details
      await viewMemberDetails(selectedMember);
      await loadTeamMembers(selectedProjectId);
      setNewTask('');
    } catch (err) {
      console.error('Failed to add task:', err);
      alert(err.response?.data?.error || 'Failed to add task. Please try again.');
    }
  };

  const handleDeleteTask = async (taskId) => {
    if (!window.confirm('Are you sure you want to delete this task?')) return;
    
    try {
      await teamMembersApi.deleteMemberTask(taskId);
      await viewMemberDetails(selectedMember);
      await loadTeamMembers(selectedProjectId);
    } catch (err) {
      console.error('Failed to delete task:', err);
      alert('Failed to delete task. Please try again.');
    }
  };

  const startEditingTask = (task) => {
    setEditingTask({
      taskId: task.id,
      text: task.title
    });
  };

  const handleEditTask = async () => {
    if (!editingTask.text.trim() || !editingTask.taskId) return;
    
    try {
      await teamMembersApi.updateMemberTask(editingTask.taskId, {
        title: editingTask.text
      });
      
      await viewMemberDetails(selectedMember);
      await loadTeamMembers(selectedProjectId);
      setEditingTask({ taskId: null, text: '' });
    } catch (err) {
      console.error('Failed to update task:', err);
      alert('Failed to update task. Please try again.');
    }
  };

  const handleToggleTaskStatus = async (task) => {
    try {
      const newStatus = task.status === 'Completed' ? 'Pending' : 'Completed';
      await teamMembersApi.updateMemberTask(task.id, { status: newStatus });
      await viewMemberDetails(selectedMember);
      await loadTeamMembers(selectedProjectId);
    } catch (err) {
      console.error('Failed to update task status:', err);
    }
  };

  const handleScheduleMeeting = () => {
    alert(`Meeting scheduled with ${selectedMember.name || selectedMember.fullName} on ${meetingDetails.date} at ${meetingDetails.time} for ${meetingDetails.duration} minutes.\n\nAgenda: ${meetingDetails.agenda}`);
    setShowScheduleModal(false);
    setMeetingDetails({
      date: '',
      time: '',
      duration: '30',
      agenda: ''
    });
  };

  const handleSendMessage = () => {
    alert(`Message sent to ${selectedMember.name || selectedMember.fullName} (${selectedMember.email}):\n\n${messageText}`);
    setShowMessageModal(false);
    setMessageText('');
  };

  if (loading && projects.length === 0) {
    return <div className="teams-page"><div className="loading">Loading...</div></div>;
  }

  return (
    <div className="teams-page">
      <div className="teams-header">
        <div className="title-section">
          <h1 className="teams-title">Team Members</h1>
          <p className="teams-subtitle">Manage your team and track individual progress</p>
        </div>
        <div className="header-actions">
          {/* Project Selector */}
          <select 
            className="project-selector"
            value={selectedProjectId || ''} 
            onChange={(e) => setSelectedProjectId(parseInt(e.target.value))}
          >
            <option value="" disabled>Select Project</option>
            {projects.map(project => (
              <option key={project.id} value={project.id}>{project.title}</option>
            ))}
          </select>
          <button className="filter-toggle-btn" onClick={() => setShowFilters(!showFilters)}>
            <FiFilter /> Advanced Filters
          </button>
          <button 
            className="add-member-btn" 
            onClick={() => setShowAddMemberModal(true)}
          >
            <FiPlus /> Add Member
          </button>
        </div>
      </div>

      {error && <div className="error-message">{error}</div>}

      <div className="filter-section">
            <div className="search-containeer">
              <FiSearch className="search-iconnn" />
              <input
                type="text"
                placeholder="Search members..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                className="search-input"
              />
            </div>
            
            {showFilters && (
              <div className="advanced-filters">
                <div className="filter-group">
                  <label>Role:</label>
                  <select value={roleFilter} onChange={(e) => setRoleFilter(e.target.value)}>
                    {roles.map(role => (
                      <option key={role} value={role}>{role}</option>
                    ))}
                  </select>
                </div>
                
                <div className="filter-group">
                  <label>Skill:</label>
                  <select value={skillFilter} onChange={(e) => setSkillFilter(e.target.value)}>
                    {skills.map(skill => (
                      <option key={skill} value={skill}>{skill}</option>
                    ))}
                  </select>
                </div>
                
                <div className="filter-group">
                  <label>Availability:</label>
                  <select value={availabilityFilter} onChange={(e) => setAvailabilityFilter(e.target.value)}>
                    {availabilityOptions.map(option => (
                      <option key={option} value={option}>{option}</option>
                    ))}
                  </select>
                </div>
              </div>
            )}
            
            {!showFilters && (
              <div className="role-filter">
                <label>Filter by role:</label>
                <select value={roleFilter} onChange={(e) => setRoleFilter(e.target.value)}>
                  {roles.map(role => (
                    <option key={role} value={role}>{role}</option>
                  ))}
                </select>
              </div>
            )}
          </div>

          <div className="team-members-grid">
            {loading ? (
              <p className="loading">Loading team members...</p>
            ) : filteredMembers.length === 0 ? (
              <p className="no-members">No team members found. Add your first team member!</p>
            ) : (
              filteredMembers.map(member => (
                <div className="member-card" key={member.id}>
                  <div className="member-header">
                    <img src={member.avatar || member.profileImageUrl || `https://ui-avatars.com/api/?name=${encodeURIComponent(member.name || member.fullName || 'User')}&background=random`} alt={member.name || member.fullName} className="member-avatar" />
                    <div className="member-info">
                      <h3 className="member-name">{member.name || member.fullName}</h3>
                      <p className="member-role">{member.role}</p>
                      {member.university && (
                        <p className="member-university">{member.university} ({member.graduationYear})</p>
                      )}
                    </div>
                    {member.rating > 0 && (
                      <div className="member-rating">
                        <FiStar /> {member.rating.toFixed(1)}
                      </div>
                    )}
                  </div>
                  
                  <div className="member-progress">
                    <div className="progress-text">
                      <span>Task Completion</span>
                      <span>{member.tasksCompleted}/{member.totalTasks} Tasks</span>
                    </div>
                    <div className="progress-bar-container">
                      <div 
                        className="progress-bar" 
                        style={{ width: `${member.totalTasks ? (member.tasksCompleted / member.totalTasks) * 100 : 0}%` }}
                      ></div>
                    </div>
                    <div className="progress-percentage">
                      {member.totalTasks ? Math.round((member.tasksCompleted / member.totalTasks) * 100) : 0}%
                    </div>
                  </div>
                  
                  {member.skills && member.skills.length > 0 && (
                    <div className="member-skills">
                      <h4>Skills</h4>
                      <div className="skills-tags">
                        {member.skills.slice(0, 3).map((skill, index) => (
                          <span key={index} className="skill-tag">{skill}</span>
                        ))}
                        {member.skills.length > 3 && (
                          <span className="more-skills">+{member.skills.length - 3}</span>
                        )}
                      </div>
                    </div>
                  )}
                  
                  <div className="member-tasks">
                    <h4>Assigned Tasks</h4>
                    <ul className="tasks-list">
                      {member.tasks && member.tasks.slice(0, 3).map((task, index) => (
                        <li key={index}>{typeof task === 'string' ? task : task.title}</li>
                      ))}
                      {member.tasks && member.tasks.length > 3 && (
                        <li className="more-tasks">+{member.tasks.length - 3} more tasks</li>
                      )}
                      {(!member.tasks || member.tasks.length === 0) && (
                        <li className="no-tasks">No tasks assigned</li>
                      )}
                    </ul>
                  </div>
                  
                  <div className="member-actions">
                    <button className="view-details-btn" onClick={() => viewMemberDetails(member)}>View Details</button>
                    <button className="delete-btn" onClick={() => handleDeleteMember(member.id)}><FiTrash2 /></button>
                  </div>
                </div>
              ))
            )}
          </div>

      {showAddMemberModal && (
        <div className="modal-overlay">
          <div className="add-member-modal">
            <h2>Add New Team Member</h2>
            <form onSubmit={(e) => {
              e.preventDefault();
              const formData = new FormData(e.target);
              handleAddMember({
                name: formData.get('name'),
                role: formData.get('role'),
                email: formData.get('email'),
                photo: formData.get('photo'),
                skills: formData.get('skills'),
                availability: formData.get('availability'),
                university: formData.get('university'),
                graduationYear: formData.get('graduationYear')
              });
            }}>
              <div className="form-group">
                <label htmlFor="name">Full Name</label>
                <input type="text" id="name" name="name" required />
              </div>
              
              <div className="form-group">
                <label htmlFor="email">Email</label>
                <input type="email" id="email" name="email" required />
              </div>
              
              <div className="form-group">
                <label htmlFor="role">Role</label>
                <select id="role" name="role" required>
                  {roles.filter(role => role !== 'All').map(role => (
                    <option key={role} value={role}>{role}</option>
                  ))}
                </select>
              </div>
              
              <div className="form-group">
                <label htmlFor="skills">Skills (comma separated)</label>
                <input type="text" id="skills" name="skills" placeholder="React, JavaScript, CSS" />
              </div>
              
              <div className="form-group">
                <label htmlFor="availability">Availability</label>
                <select id="availability" name="availability">
                  {availabilityOptions.filter(option => option !== 'All').map(option => (
                    <option key={option} value={option}>{option}</option>
                  ))}
                </select>
              </div>
              
              <div className="form-group">
                <label htmlFor="university">University/Institution</label>
                <input type="text" id="university" name="university" />
              </div>
              
              <div className="form-group">
                <label htmlFor="graduationYear">Graduation Year</label>
                <input type="text" id="graduationYear" name="graduationYear" placeholder="2023" />
              </div>
              
              <div className="form-group">
                <label htmlFor="photo">Photo URL (optional)</label>
                <input type="text" id="photo" name="photo" />
              </div>
              
              <div className="modal-actions">
                <button type="button" onClick={() => setShowAddMemberModal(false)}>Cancel</button>
                <button type="submit" className="submit-btn">Add Member</button>
              </div>
            </form>
          </div>
        </div>
      )}

      {showMemberDetails && selectedMember && (
        <div className="modal-overlay">
          <div className="member-details-modal">
            <div className="modal-header">
              <h2>Member Details</h2>
              <button className="close-btn" onClick={() => setShowMemberDetails(false)}>×</button>
            </div>
            
            <div className="member-profile">
              <img src={selectedMember.avatar || selectedMember.profileImageUrl || `https://ui-avatars.com/api/?name=${encodeURIComponent(selectedMember.name || selectedMember.fullName || 'User')}&background=random`} alt={selectedMember.name || selectedMember.fullName} className="large-avatar" />
              <div className="profile-info">
                <h3>{selectedMember.name || selectedMember.fullName}</h3>
                <p className="member-role">{selectedMember.role}</p>
                {selectedMember.university && (
                  <p className="university-info">
                    <span className="label">University:</span> {selectedMember.university}
                  </p>
                )}
                {selectedMember.graduationYear && (
                  <p className="graduation-info">
                    <span className="label">Graduation Year:</span> {selectedMember.graduationYear}
                  </p>
                )}
                <p className="availability-info">
                  <span className="label">Availability:</span> {selectedMember.availability || 'Not specified'}
                </p>
                <p className="contributions-info">
                  <span className="label">Project Contributions:</span> {selectedMember.projectContributions || 0}
                </p>
              </div>
            </div>
            
            <div className="skills-section">
              <h4>Skills</h4>
              <div className="all-skills">
                {selectedMember.skills && selectedMember.skills.map((skill, index) => (
                  <span key={index} className="skill-tag">{skill}</span>
                ))}
                {(!selectedMember.skills || selectedMember.skills.length === 0) && (
                  <p>No skills specified</p>
                )}
              </div>
            </div>
            
            <div className="tasks-section">
              <h4>Assigned Tasks</h4>
              <div className="task-list-detailed">
                {selectedMember.tasks && selectedMember.tasks.length > 0 ? (
                  selectedMember.tasks.map((task) => (
                    <div key={task.id} className={`task-item ${task.status === 'Completed' ? 'completed' : ''}`}>
                      {editingTask.taskId === task.id ? (
                        <div className="edit-task-form">
                          <input 
                            type="text" 
                            value={editingTask.text} 
                            onChange={(e) => setEditingTask({...editingTask, text: e.target.value})}
                            className="edit-task-input"
                          />
                          <div className="edit-actions">
                            <button onClick={handleEditTask} className="save-edit-btn">Save</button>
                            <button onClick={() => setEditingTask({ taskId: null, text: '' })} className="cancel-edit-btn">Cancel</button>
                          </div>
                        </div>
                      ) : (
                        <>
                          <input 
                            type="checkbox" 
                            checked={task.status === 'Completed'}
                            onChange={() => handleToggleTaskStatus(task)}
                            className="task-checkbox"
                          />
                          <span className="task-name">{task.title}</span>
                          <span className={`task-priority ${task.priority?.toLowerCase()}`}>{task.priority}</span>
                          <div className="task-actions">
                            <button className="edit-task-btn" onClick={() => startEditingTask(task)}><FiEdit /></button>
                            <button className="delete-task-btn" onClick={() => handleDeleteTask(task.id)}><FiTrash2 /></button>
                          </div>
                        </>
                      )}
                    </div>
                  ))
                ) : (
                  <p>No tasks assigned</p>
                )}
              </div>
              
              <div className="add-task-form">
                <input 
                  type="text" 
                  value={newTask} 
                  onChange={(e) => setNewTask(e.target.value)}
                  placeholder="Add a new task..."
                  className="new-task-input"
                  onKeyPress={(e) => e.key === 'Enter' && handleAddTask()}
                />
                <button className="add-task-btn" onClick={handleAddTask}>Add Task</button>
              </div>
            </div>
            
            <div className="modal-actions">
              <button className="schedule-btn" onClick={() => setShowScheduleModal(true)}><FiCalendar /> Schedule Meeting</button>
              <button className="message-btn" onClick={() => setShowMessageModal(true)}><FiMail /> Send Message</button>
            </div>
          </div>
        </div>
      )}

      {/* Schedule Meeting Modal */}
      {showScheduleModal && selectedMember && (
        <div className="modal-overlay">
          <div className="schedule-modal">
            <h2>Schedule Meeting with {selectedMember.name || selectedMember.fullName}</h2>
            <form onSubmit={(e) => { e.preventDefault(); handleScheduleMeeting(); }}>
              <div className="form-group">
                <label>Date</label>
                <input 
                  type="date" 
                  value={meetingDetails.date}
                  onChange={(e) => setMeetingDetails({...meetingDetails, date: e.target.value})}
                  required 
                />
              </div>
              <div className="form-group">
                <label>Time</label>
                <input 
                  type="time" 
                  value={meetingDetails.time}
                  onChange={(e) => setMeetingDetails({...meetingDetails, time: e.target.value})}
                  required 
                />
              </div>
              <div className="form-group">
                <label>Duration (minutes)</label>
                <select 
                  value={meetingDetails.duration}
                  onChange={(e) => setMeetingDetails({...meetingDetails, duration: e.target.value})}
                >
                  <option value="15">15 minutes</option>
                  <option value="30">30 minutes</option>
                  <option value="45">45 minutes</option>
                  <option value="60">1 hour</option>
                </select>
              </div>
              <div className="form-group">
                <label>Agenda</label>
                <textarea 
                  value={meetingDetails.agenda}
                  onChange={(e) => setMeetingDetails({...meetingDetails, agenda: e.target.value})}
                  placeholder="Meeting agenda..."
                />
              </div>
              <div className="modal-actions">
                <button type="button" onClick={() => setShowScheduleModal(false)}>Cancel</button>
                <button type="submit" className="submit-btn">Schedule</button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Message Modal */}
      {showMessageModal && selectedMember && (
        <div className="modal-overlay">
          <div className="message-modal">
            <h2>Send Message to {selectedMember.name || selectedMember.fullName}</h2>
            <form onSubmit={(e) => { e.preventDefault(); handleSendMessage(); }}>
              <div className="form-group">
                <label>To: {selectedMember.email}</label>
              </div>
              <div className="form-group">
                <textarea 
                  value={messageText}
                  onChange={(e) => setMessageText(e.target.value)}
                  placeholder="Type your message..."
                  rows="5"
                  required
                />
              </div>
              <div className="modal-actions">
                <button type="button" onClick={() => setShowMessageModal(false)}>Cancel</button>
                <button type="submit" className="submit-btn">Send</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default Teams;
