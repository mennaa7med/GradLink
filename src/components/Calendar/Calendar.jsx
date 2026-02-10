import React, { useState, useEffect } from 'react';
import { FiChevronLeft, FiChevronRight, FiPlus, FiCalendar, FiClock, FiMapPin, FiExternalLink } from 'react-icons/fi';
import { motion, AnimatePresence } from 'framer-motion';
import api from '../../api/client';
import './Calendar.css';

const Calendar = () => {
  const [currentDate, setCurrentDate] = useState(new Date());
  const [events, setEvents] = useState([]);
  const [selectedDate, setSelectedDate] = useState(null);
  const [showEventModal, setShowEventModal] = useState(false);
  const [loading, setLoading] = useState(false);
  const [view, setView] = useState('month'); // month, week, day
  const [newEvent, setNewEvent] = useState({
    title: '',
    description: '',
    date: '',
    time: '',
    duration: 60,
    type: 'meeting',
    location: ''
  });

  useEffect(() => {
    loadEvents();
  }, [currentDate]);

  const loadEvents = async () => {
    setLoading(true);
    try {
      const startDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1);
      const endDate = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0);
      
      const { data } = await api.get('/api/calendar/events', {
        params: {
          startDate: startDate.toISOString(),
          endDate: endDate.toISOString()
        }
      });
      setEvents(data);
    } catch (error) {
      console.error('Failed to load events:', error);
      // Load from tasks and sessions as fallback
      loadFromLocalSources();
    } finally {
      setLoading(false);
    }
  };

  const loadFromLocalSources = async () => {
    try {
      // Load tasks with due dates
      const { data: tasks } = await api.get('/api/tasks');
      const taskEvents = tasks
        .filter(t => t.dueDate)
        .map(t => ({
          id: `task-${t.id}`,
          title: t.name || t.title,
          date: t.dueDate,
          type: 'task',
          color: '#3b82f6'
        }));

      // Load mentoring sessions
      const { data: sessions } = await api.get('/api/mentoring-sessions').catch(() => ({ data: [] }));
      const sessionEvents = sessions.map(s => ({
        id: `session-${s.id}`,
        title: s.topic || 'Mentoring Session',
        date: s.scheduledAt,
        type: 'session',
        color: '#10b981'
      }));

      setEvents([...taskEvents, ...sessionEvents]);
    } catch (error) {
      console.error('Failed to load events from local sources:', error);
    }
  };

  const getDaysInMonth = () => {
    const year = currentDate.getFullYear();
    const month = currentDate.getMonth();
    const firstDay = new Date(year, month, 1);
    const lastDay = new Date(year, month + 1, 0);
    const daysInMonth = lastDay.getDate();
    const startingDay = firstDay.getDay();
    
    const days = [];
    
    // Previous month's days
    const prevMonth = new Date(year, month, 0);
    const prevMonthDays = prevMonth.getDate();
    for (let i = startingDay - 1; i >= 0; i--) {
      days.push({
        date: prevMonthDays - i,
        isCurrentMonth: false,
        fullDate: new Date(year, month - 1, prevMonthDays - i)
      });
    }
    
    // Current month's days
    for (let i = 1; i <= daysInMonth; i++) {
      days.push({
        date: i,
        isCurrentMonth: true,
        fullDate: new Date(year, month, i)
      });
    }
    
    // Next month's days
    const remainingDays = 42 - days.length;
    for (let i = 1; i <= remainingDays; i++) {
      days.push({
        date: i,
        isCurrentMonth: false,
        fullDate: new Date(year, month + 1, i)
      });
    }
    
    return days;
  };

  const getEventsForDate = (date) => {
    return events.filter(event => {
      const eventDate = new Date(event.date);
      return eventDate.toDateString() === date.toDateString();
    });
  };

  const goToPrevMonth = () => {
    setCurrentDate(new Date(currentDate.getFullYear(), currentDate.getMonth() - 1, 1));
  };

  const goToNextMonth = () => {
    setCurrentDate(new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 1));
  };

  const goToToday = () => {
    setCurrentDate(new Date());
  };

  const handleDateClick = (day) => {
    setSelectedDate(day.fullDate);
    setNewEvent(prev => ({
      ...prev,
      date: day.fullDate.toISOString().split('T')[0]
    }));
  };

  const handleCreateEvent = async (e) => {
    e.preventDefault();
    try {
      await api.post('/api/calendar/events', {
        ...newEvent,
        date: `${newEvent.date}T${newEvent.time}:00`
      });
      await loadEvents();
      setShowEventModal(false);
      setNewEvent({
        title: '',
        description: '',
        date: '',
        time: '',
        duration: 60,
        type: 'meeting',
        location: ''
      });
    } catch (error) {
      console.error('Failed to create event:', error);
    }
  };

  const addToGoogleCalendar = (event) => {
    const startDate = new Date(event.date);
    const endDate = new Date(startDate.getTime() + (event.duration || 60) * 60000);
    
    const formatDate = (d) => d.toISOString().replace(/-|:|\.\d{3}/g, '');
    
    const url = `https://calendar.google.com/calendar/render?action=TEMPLATE&text=${encodeURIComponent(event.title)}&dates=${formatDate(startDate)}/${formatDate(endDate)}&details=${encodeURIComponent(event.description || '')}&location=${encodeURIComponent(event.location || '')}`;
    
    window.open(url, '_blank');
  };

  const isToday = (date) => {
    const today = new Date();
    return date.toDateString() === today.toDateString();
  };

  const monthNames = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
  const dayNames = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];

  return (
    <div className="calendar-container">
      <div className="calendar-header">
        <div className="calendar-nav">
          <button className="nav-btn" onClick={goToPrevMonth}>
            <FiChevronLeft />
          </button>
          <h2>{monthNames[currentDate.getMonth()]} {currentDate.getFullYear()}</h2>
          <button className="nav-btn" onClick={goToNextMonth}>
            <FiChevronRight />
          </button>
        </div>
        <div className="calendar-actions">
          <button className="today-btn" onClick={goToToday}>
            Today
          </button>
          <button className="add-event-btn" onClick={() => setShowEventModal(true)}>
            <FiPlus /> Add Event
          </button>
        </div>
      </div>

      <div className="calendar-grid">
        <div className="day-names">
          {dayNames.map(day => (
            <div key={day} className="day-name">{day}</div>
          ))}
        </div>
        
        <div className="days-grid">
          {getDaysInMonth().map((day, index) => {
            const dayEvents = getEventsForDate(day.fullDate);
            return (
              <motion.div
                key={index}
                className={`day-cell ${!day.isCurrentMonth ? 'other-month' : ''} ${isToday(day.fullDate) ? 'today' : ''} ${selectedDate?.toDateString() === day.fullDate.toDateString() ? 'selected' : ''}`}
                onClick={() => handleDateClick(day)}
                whileHover={{ scale: 1.02 }}
                whileTap={{ scale: 0.98 }}
              >
                <span className="day-number">{day.date}</span>
                <div className="day-events">
                  {dayEvents.slice(0, 3).map(event => (
                    <div 
                      key={event.id} 
                      className={`event-dot ${event.type}`}
                      title={event.title}
                      style={{ backgroundColor: event.color }}
                    />
                  ))}
                  {dayEvents.length > 3 && (
                    <span className="more-events">+{dayEvents.length - 3}</span>
                  )}
                </div>
              </motion.div>
            );
          })}
        </div>
      </div>

      {/* Selected Date Events */}
      {selectedDate && (
        <motion.div 
          className="selected-date-events"
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
        >
          <h3>Events for {selectedDate.toLocaleDateString()}</h3>
          {getEventsForDate(selectedDate).length === 0 ? (
            <p className="no-events">No events scheduled</p>
          ) : (
            <div className="events-list">
              {getEventsForDate(selectedDate).map(event => (
                <div key={event.id} className={`event-card ${event.type}`}>
                  <div className="event-info">
                    <h4>{event.title}</h4>
                    {event.time && (
                      <span className="event-time">
                        <FiClock /> {event.time}
                      </span>
                    )}
                    {event.location && (
                      <span className="event-location">
                        <FiMapPin /> {event.location}
                      </span>
                    )}
                  </div>
                  <button 
                    className="add-to-google"
                    onClick={() => addToGoogleCalendar(event)}
                    title="Add to Google Calendar"
                  >
                    <FiExternalLink />
                  </button>
                </div>
              ))}
            </div>
          )}
        </motion.div>
      )}

      {/* Add Event Modal */}
      <AnimatePresence>
        {showEventModal && (
          <motion.div 
            className="modal-overlay"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            onClick={() => setShowEventModal(false)}
          >
            <motion.div 
              className="modal-content"
              initial={{ scale: 0.9, opacity: 0 }}
              animate={{ scale: 1, opacity: 1 }}
              exit={{ scale: 0.9, opacity: 0 }}
              onClick={e => e.stopPropagation()}
            >
              <h2>Add New Event</h2>
              <form onSubmit={handleCreateEvent}>
                <div className="form-group">
                  <label>Title</label>
                  <input
                    type="text"
                    value={newEvent.title}
                    onChange={e => setNewEvent({...newEvent, title: e.target.value})}
                    required
                  />
                </div>
                <div className="form-row">
                  <div className="form-group">
                    <label>Date</label>
                    <input
                      type="date"
                      value={newEvent.date}
                      onChange={e => setNewEvent({...newEvent, date: e.target.value})}
                      required
                    />
                  </div>
                  <div className="form-group">
                    <label>Time</label>
                    <input
                      type="time"
                      value={newEvent.time}
                      onChange={e => setNewEvent({...newEvent, time: e.target.value})}
                      required
                    />
                  </div>
                </div>
                <div className="form-group">
                  <label>Type</label>
                  <select
                    value={newEvent.type}
                    onChange={e => setNewEvent({...newEvent, type: e.target.value})}
                  >
                    <option value="meeting">Meeting</option>
                    <option value="task">Task</option>
                    <option value="session">Mentoring Session</option>
                    <option value="deadline">Deadline</option>
                    <option value="other">Other</option>
                  </select>
                </div>
                <div className="form-group">
                  <label>Location (optional)</label>
                  <input
                    type="text"
                    value={newEvent.location}
                    onChange={e => setNewEvent({...newEvent, location: e.target.value})}
                    placeholder="e.g., Zoom, Google Meet, Room 101"
                  />
                </div>
                <div className="form-group">
                  <label>Description</label>
                  <textarea
                    value={newEvent.description}
                    onChange={e => setNewEvent({...newEvent, description: e.target.value})}
                    rows={3}
                  />
                </div>
                <div className="modal-actions">
                  <button type="button" onClick={() => setShowEventModal(false)}>Cancel</button>
                  <button type="submit" className="primary">Create Event</button>
                </div>
              </form>
            </motion.div>
          </motion.div>
        )}
      </AnimatePresence>
    </div>
  );
};

export default Calendar;















