import React, { useState, useEffect } from 'react';
import { FiPlus, FiSearch, FiMail, FiFilter, FiStar, FiCalendar, FiTrash2, FiEdit } from 'react-icons/fi';
import './Teams.css';

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
  const [editingTask, setEditingTask] = useState({ memberId: null, taskIndex: null, text: '' });
  const [showScheduleModal, setShowScheduleModal] = useState(false);
  const [showMessageModal, setShowMessageModal] = useState(false);
  const [messageText, setMessageText] = useState('');
  const [meetingDetails, setMeetingDetails] = useState({
    date: '',
    time: '',
    duration: '30',
    agenda: ''
  });
  
  // Sample team members data with enhanced student-relevant fields
  const [teamMembers, setTeamMembers] = useState([
    {
      id: 1,
      name: 'Alexandra Deff',
      role: 'Frontend Developer',
      avatar: 'https://randomuser.me/api/portraits/women/44.jpg',
      tasksCompleted: 5,
      totalTasks: 8,
      tasks: ['Build Dashboard UI', 'Implement Responsive Design', 'Create Team Page'],
      skills: ['React', 'CSS', 'JavaScript'],
      availability: 'Part-time',
      university: 'MIT',
      graduationYear: '2023',
      projectContributions: 15,
      rating: 4.8,
      email: 'alexandra@example.com'
    },
    {
      id: 2,
      name: 'John Smith',
      role: 'Backend Developer',
      avatar: 'https://randomuser.me/api/portraits/men/32.jpg',
      tasksCompleted: 3,
      totalTasks: 6,
      tasks: ['Set up API endpoints', 'Database optimization', 'Authentication system'],
      skills: ['Node.js', 'Python', 'Database'],
      availability: 'Full-time',
      university: 'Stanford',
      graduationYear: '2022',
      projectContributions: 8,
      rating: 4.2,
      email: 'john@example.com'
    }
  ]);

  const roles = ['All', 'Frontend Developer', 'Backend Developer', 'UI/UX Designer', 'Project Manager', 'AI Engineer'];
  const skills = ['All', 'React', 'JavaScript', 'Python', 'Node.js', 'UI/UX', 'Database', 'Machine Learning'];
  const availabilityOptions = ['All', 'Full-time', 'Part-time', 'Weekends only'];
  
  const filteredMembers = teamMembers.filter(member => {
    return (
      member.name.toLowerCase().includes(searchQuery.toLowerCase()) &&
      (roleFilter === 'All' || member.role === roleFilter) &&
      (skillFilter === 'All' || member.skills.includes(skillFilter)) &&
      (availabilityFilter === 'All' || member.availability === availabilityFilter)
    );
  });

  const handleAddMember = (newMember) => {
    const photoUrl = newMember.photo || 'https://randomuser.me/api/portraits/lego/1.jpg';
    setTeamMembers([...teamMembers, { 
      ...newMember, 
      id: teamMembers.length + 1,
      avatar: photoUrl,
      skills: newMember.skills ? newMember.skills.split(',').map(skill => skill.trim()) : [],
      rating: 0,
      projectContributions: 0,
      tasksCompleted: 0,
      totalTasks: 0,
      tasks: []
    }]);
    setShowAddMemberModal(false);
  };

  const handleDeleteMember = (id) => {
    if (window.confirm('Are you sure you want to remove this team member?')) {
      setTeamMembers(teamMembers.filter(member => member.id !== id));
      if (showMemberDetails && selectedMember?.id === id) {
        setShowMemberDetails(false);
      }
    }
  };

  const viewMemberDetails = (member) => {
    setSelectedMember(member);
    setShowMemberDetails(true);
  };

  const handleAddTask = () => {
    if (!newTask.trim()) return;
    
    const updatedMembers = teamMembers.map(member => {
      if (member.id === selectedMember.id) {
        const updatedMember = {
          ...member,
          tasks: [...member.tasks, newTask],
          totalTasks: member.totalTasks + 1
        };
        setSelectedMember(updatedMember);
        return updatedMember;
      }
      return member;
    });
    
    setTeamMembers(updatedMembers);
    setNewTask('');
  };

  const handleDeleteTask = (taskIndex) => {
    if (!window.confirm('Are you sure you want to delete this task?')) return;
    
    const updatedMembers = teamMembers.map(member => {
      if (member.id === selectedMember.id) {
        const updatedTasks = member.tasks.filter((_, index) => index !== taskIndex);
        const updatedMember = {
          ...member,
          tasks: updatedTasks,
          totalTasks: member.totalTasks - 1,
          tasksCompleted: member.tasksCompleted > 0 ? member.tasksCompleted - 1 : 0
        };
        setSelectedMember(updatedMember);
        return updatedMember;
      }
      return member;
    });
    
    setTeamMembers(updatedMembers);
  };

  const startEditingTask = (taskIndex, taskText) => {
    setEditingTask({
      memberId: selectedMember.id,
      taskIndex,
      text: taskText
    });
  };

  const handleEditTask = () => {
    if (!editingTask.text.trim()) return;
    
    const updatedMembers = teamMembers.map(member => {
      if (member.id === editingTask.memberId) {
        const updatedTasks = [...member.tasks];
        updatedTasks[editingTask.taskIndex] = editingTask.text;
        
        const updatedMember = {
          ...member,
          tasks: updatedTasks
        };
        
        if (selectedMember.id === member.id) {
          setSelectedMember(updatedMember);
        }
        
        return updatedMember;
      }
      return member;
    });
    
    setTeamMembers(updatedMembers);
    setEditingTask({ memberId: null, taskIndex: null, text: '' });
  };

  const handleScheduleMeeting = () => {
    // In a real app, this would send the meeting invitation
    alert(`Meeting scheduled with ${selectedMember.name} on ${meetingDetails.date} at ${meetingDetails.time} for ${meetingDetails.duration} minutes.\n\nAgenda: ${meetingDetails.agenda}`);
    setShowScheduleModal(false);
    
    // Reset meeting details
    setMeetingDetails({
      date: '',
      time: '',
      duration: '30',
      agenda: ''
    });
  };

  const handleSendMessage = () => {
    // In a real app, this would send the message to the member
    alert(`Message sent to ${selectedMember.name} (${selectedMember.email}):\n\n${messageText}`);
    setShowMessageModal(false);
    setMessageText('');
  };

  const handleMessageButtonClick = (member) => {
    setSelectedMember(member);
    setShowMessageModal(true);
  };

  return (
    <div className="teams-page">
      <div className="teams-header">
        <div className="title-section">
          <h1 className="teams-title">Team Members</h1>
          <p className="teams-subtitle">Manage your team and track individual progress</p>
        </div>
        <div className="header-actions">
          <button className="filter-toggle-btn" onClick={() => setShowFilters(!showFilters)}>
            <FiFilter /> Advanced Filters
          </button>
          <button className="add-member-btn" onClick={() => setShowAddMemberModal(true)}>
            <FiPlus /> Add Member
          </button>
        </div>
      </div>

      <div className="filter-section">
        <div className="search-container">
          <FiSearch className="ssearch-icon" />
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
        {filteredMembers.length === 0 ? (
          <p className="no-members">No team members match your search criteria.</p>
        ) : (
          filteredMembers.map(member => (
            <div className="member-card" key={member.id}>
              <div className="member-header">
                <img src={member.avatar} alt={member.name} className="member-avatar" />
                <div className="member-info">
                  <h3 className="member-name">{member.name}</h3>
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
              
              {member.skills && (
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
                  {member.tasks.slice(0, 3).map((task, index) => (
                    <li key={index}>{task}</li>
                  ))}
                  {member.tasks.length > 3 && (
                    <li className="more-tasks">+{member.tasks.length - 3} more tasks</li>
                  )}
                </ul>
              </div>
              
              <div className="member-actions">
                <button className="view-details-btn" onClick={() => viewMemberDetails(member)}>View Details</button>
                <button className="message-btn" onClick={() => handleMessageButtonClick(member)}><FiMail /></button>
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
                graduationYear: formData.get('graduationYear'),
                tasksCompleted: 0,
                totalTasks: 0,
                tasks: []
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
              <img src={selectedMember.avatar} alt={selectedMember.name} className="large-avatar" />
              <div className="profile-info">
                <h3>{selectedMember.name}</h3>
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
                  selectedMember.tasks.map((task, index) => (
                    <div key={index} className="task-item">
                      {editingTask.memberId === selectedMember.id && editingTask.taskIndex === index ? (
                        <div className="edit-task-form">
                          <input 
                            type="text" 
                            value={editingTask.text} 
                            onChange={(e) => setEditingTask({...editingTask, text: e.target.value})}
                            className="edit-task-input"
                          />
                          <div className="edit-actions">
                            <button onClick={handleEditTask} className="save-edit-btn">Save</button>
                            <button onClick={() => setEditingTask({ memberId: null, taskIndex: null, text: '' })} className="cancel-edit-btn">Cancel</button>
                          </div>
                        </div>
                      ) : (
                        <>
                          <span className="task-name">{task}</span>
                          <div className="task-actions">
                            <button className="edit-task-btn" onClick={() => startEditingTask(index, task)}><FiEdit /></button>
                            <button className="delete-task-btn" onClick={() => handleDeleteTask(index)}><FiTrash2 /></button>
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
    </div>
  );
};

export default Teams;
