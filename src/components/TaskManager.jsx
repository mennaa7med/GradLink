import React, { useState, useEffect } from 'react';
import { DndProvider, useDrag, useDrop } from 'react-dnd';
import { HTML5Backend } from 'react-dnd-html5-backend';
import { motion, AnimatePresence } from 'framer-motion';
import Modal from 'react-modal';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import * as tasksApi from '../api/tasks';
import './TaskManager.css';

Modal.setAppElement('#root');

const ItemTypes = {
  TASK: 'task',
};

const TaskItem = ({ task, index, status, moveTask, onDelete, onEdit, onAddSubtask, onToggleSubtask, onDeleteSubtask }) => {
  const [isEditing, setIsEditing] = useState(false);
  const [editTask, setEditTask] = useState({ ...task, tags: task.tags.join(', ') });
  const [isSubtasksOpen, setIsSubtasksOpen] = useState(false);
  const [isNotesOpen, setIsNotesOpen] = useState(false);
  const [newSubtask, setNewSubtask] = useState('');

  const [{ isDragging }, drag] = useDrag({
    type: ItemTypes.TASK,
    item: { id: task.id, index, status },
    collect: (monitor) => ({
      isDragging: monitor.isDragging(),
    }),
  });

  const [, drop] = useDrop({
    accept: ItemTypes.TASK,
    hover: (item) => {
      if (item.id === task.id && item.status === status) return;
      moveTask(item.status, status, item.index, index, item.id);
      item.status = status;
      item.index = index;
    },
  });

  const handleAddSubtask = (e) => {
    e.preventDefault();
    if (newSubtask.trim()) {
      onAddSubtask(task.id, newSubtask);
      setNewSubtask('');
    }
  };

  const handleEdit = () => {
    if (!isEditing) {
      setIsEditing(true);
    }
  };

  const saveEdit = () => {
    if (editTask.name && editTask.person && editTask.dueDate) {
      onEdit(task.id, {
        ...editTask,
        tags: editTask.tags.split(',').map((tag) => tag.trim()).filter((tag) => tag),
      });
      setIsEditing(false);
    }
  };

  const isOverdue = task.dueDate && new Date(task.dueDate) < new Date() && task.status !== 'Completed';
  const totalSubtasks = task.subtasks.length;
  const completedSubtasks = task.subtasks.filter((subtask) => subtask.completed).length;
  const progress = totalSubtasks > 0 ? Math.round((completedSubtasks / totalSubtasks) * 100) : 0;

  return (
    <motion.li
      ref={(node) => drag(drop(node))}
      className={`task-item ${isDragging ? 'opacity-50' : ''} ${isOverdue ? 'border-red-500' : ''} status-${task.status.toLowerCase().replace(' ', '-')}`}
      initial={{ opacity: 0, y: 10 }}
      animate={{ opacity: 1, y: 0 }}
      exit={{ opacity: 0, y: -10 }}
      onDoubleClick={handleEdit}
    >
      {isEditing ? (
        <div className="task-edit">
          <input
            type="text"
            placeholder="Task Name"
            className="modal-input"
            value={editTask.name}
            onChange={(e) => setEditTask({ ...editTask, name: e.target.value })}
            required
          />
          <textarea
            placeholder="Description"
            className="modal-textarea"
            value={editTask.description}
            onChange={(e) => setEditTask({ ...editTask, description: e.target.value })}
          />
          <textarea
            placeholder="Notes"
            className="modal-textarea"
            value={editTask.notes}
            onChange={(e) => setEditTask({ ...editTask, notes: e.target.value })}
          />
          <input
            type="text"
            placeholder="Tags (comma-separated)"
            className="modal-input"
            value={editTask.tags}
            onChange={(e) => setEditTask({ ...editTask, tags: e.target.value })}
          />
          <input
            type="date"
            className="modal-input"
            value={editTask.dueDate ? editTask.dueDate.split('T')[0] : ''}
            onChange={(e) => setEditTask({ ...editTask, dueDate: e.target.value })}
            required
          />
          <select
            className="modal-select"
            value={editTask.priority}
            onChange={(e) => setEditTask({ ...editTask, priority: e.target.value })}
          >
            <option value="Low">Low</option>
            <option value="Medium">Medium</option>
            <option value="High">High</option>
          </select>
          <input
            type="text"
            placeholder="Responsible Person"
            className="modal-input"
            value={editTask.person}
            onChange={(e) => setEditTask({ ...editTask, person: e.target.value })}
            required
          />
          <select
            className="modal-select"
            value={editTask.status}
            onChange={(e) => setEditTask({ ...editTask, status: e.target.value })}
          >
            <option value="Pending">Not Started</option>
            <option value="In Progress">In Progress</option>
            <option value="Completed">Completed</option>
          </select>
          <select
            className="modal-select"
            value={editTask.category}
            onChange={(e) => setEditTask({ ...editTask, category: e.target.value })}
          >
            <option value="Frontend">Frontend</option>
            <option value="Backend">Backend</option>
            <option value="Presentation">Presentation</option>
            <option value="Other">Other</option>
          </select>
          <div className="modal-buttons">
            <button type="button" className="cancel-button" onClick={() => setIsEditing(false)}>
              Cancel
            </button>
            <button type="button" className="submit-button" onClick={saveEdit}>
              Save
            </button>
          </div>
        </div>
      ) : (
        <div className="task-content">
          <h3 className="task-name">{task.name}</h3>
          <p className="task-detail">Person: {task.person}</p>
          <p className="task-detail">Due: {task.dueDate ? new Date(task.dueDate).toLocaleDateString() : '-'}</p>
          <p className="task-detail">Category: {task.category}</p>
          <p className="task-detail">Priority: {task.priority}</p>
          {task.tags.length > 0 && (
            <div className="task-tags">
              {task.tags.map((tag, index) => (
                <span key={index} className="tag">{tag}</span>
              ))}
            </div>
          )}
          <div className="progress-container" data-progress={progress}>
            <div className="progress-bar" style={{ width: `${progress}%` }}></div>
          </div>
          <p className="task-detail">Progress: {progress}%</p>
          <button
            className="subtask-toggle"
            onClick={() => setIsSubtasksOpen(!isSubtasksOpen)}
          >
            {isSubtasksOpen ? 'Hide Subtasks' : `Show Subtasks (${task.subtasks.length})`}
          </button>
          {isSubtasksOpen && (
            <div className="subtask-container">
              <form onSubmit={handleAddSubtask} className="subtask-form">
                <input
                  type="text"
                  placeholder="Add subtask..."
                  className="subtask-input"
                  value={newSubtask}
                  onChange={(e) => setNewSubtask(e.target.value)}
                />
                <button type="submit" className="subtask-add-button">Add</button>
              </form>
              <ul className="subtask-list">
                <AnimatePresence>
                  {task.subtasks.map((subtask) => (
                    <motion.li
                      key={subtask.id}
                      className={`subtask-item ${subtask.completed ? 'completed' : ''}`}
                      initial={{ opacity: 0, x: -10 }}
                      animate={{ opacity: 1, x: 0 }}
                      exit={{ opacity: 0, x: 10 }}
                    >
                      <input
                        type="checkbox"
                        checked={subtask.completed}
                        onChange={() => onToggleSubtask(task.id, subtask.id)}
                      />
                      <span>{subtask.name}</span>
                      <button
                        className="subtask-delete-button"
                        onClick={() => onDeleteSubtask(task.id, subtask.id)}
                      >
                        ×
                      </button>
                    </motion.li>
                  ))}
                </AnimatePresence>
              </ul>
            </div>
          )}
          <button
            className="notes-toggle"
            onClick={() => setIsNotesOpen(!isNotesOpen)}
          >
            {isNotesOpen ? 'Hide Notes' : 'Show Notes'}
          </button>
          {isNotesOpen && (
            <div className="notes-container">
              <p className="task-detail">{task.notes || 'No notes available.'}</p>
            </div>
          )}
          <button
            className="delete-button"
            onClick={() => onDelete(task.id)}
          >
            Delete
          </button>
        </div>
      )}
    </motion.li>
  );
};

const TaskManager = () => {
  const { user, loading: authLoading } = useAuth();
  const navigate = useNavigate();

  const [tasks, setTasks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [newTask, setNewTask] = useState({
    name: '',
    description: '',
    dueDate: '',
    priority: 'Medium',
    person: '',
    status: 'Pending',
    category: 'Other',
    notes: '',
    tags: '',
  });

  const [isAddModalOpen, setIsAddModalOpen] = useState(false);
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(null);
  const [filterStatus, setFilterStatus] = useState('All');
  const [filterPerson, setFilterPerson] = useState('');
  const [searchQuery, setSearchQuery] = useState('');

  // Check authentication and load tasks
  useEffect(() => {
    if (!authLoading && !user) {
      navigate('/login');
      return;
    }
    if (user) {
      loadTasks();
    }
  }, [user, authLoading, navigate]);

  const loadTasks = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await tasksApi.listTasks();
      setTasks(data);
    } catch (err) {
      console.error('Failed to load tasks:', err);
      setError('Failed to load tasks. Please make sure you are logged in.');
    } finally {
      setLoading(false);
    }
  };

  const uniquePeople = [...new Set(tasks.map((task) => task.person).filter(Boolean))];

  const filteredTasks = tasks.filter(
    (task) =>
      (filterStatus === 'All' || task.status === filterStatus) &&
      (filterPerson === '' || task.person === filterPerson) &&
      (searchQuery === '' ||
        task.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
        (task.description && task.description.toLowerCase().includes(searchQuery.toLowerCase())) ||
        (task.notes && task.notes.toLowerCase().includes(searchQuery.toLowerCase())))
  );

  const handleAddTask = async (e) => {
    e.preventDefault();
    if (newTask.name && newTask.person && newTask.dueDate) {
      try {
        await tasksApi.createTask({
          name: newTask.name,
          description: newTask.description,
          notes: newTask.notes,
          tags: newTask.tags,
          dueDate: newTask.dueDate,
          priority: newTask.priority,
          person: newTask.person,
          status: newTask.status,
          category: newTask.category,
        });
        await loadTasks(); // Refresh tasks from server
        setNewTask({
          name: '',
          description: '',
          dueDate: '',
          priority: 'Medium',
          person: '',
          status: 'Pending',
          category: 'Other',
          notes: '',
          tags: '',
        });
        setIsAddModalOpen(false);
        // Notify sidebar to update task count
        window.dispatchEvent(new Event('taskUpdated'));
      } catch (err) {
        console.error('Failed to add task:', err);
        alert(err.response?.data?.error || 'Failed to add task. Please try again.');
      }
    }
  };

  const deleteTask = async (taskId) => {
    try {
      await tasksApi.deleteTask(taskId);
      setTasks(tasks.filter((task) => task.id !== taskId));
      setIsDeleteModalOpen(null);
      // Notify sidebar to update task count
      window.dispatchEvent(new Event('taskUpdated'));
    } catch (err) {
      console.error('Failed to delete task:', err);
      alert('Failed to delete task. Please try again.');
    }
  };

  const editTask = async (taskId, updatedTask) => {
    try {
      await tasksApi.updateTask(taskId, {
        name: updatedTask.name,
        description: updatedTask.description,
        notes: updatedTask.notes,
        tags: updatedTask.tags.join(','),
        dueDate: updatedTask.dueDate,
        priority: updatedTask.priority,
        person: updatedTask.person,
        status: updatedTask.status,
        category: updatedTask.category,
      });
      await loadTasks(); // Refresh from server
      // Notify sidebar to update task count
      window.dispatchEvent(new Event('taskUpdated'));
    } catch (err) {
      console.error('Failed to update task:', err);
      alert('Failed to update task. Please try again.');
    }
  };

  const addSubtask = async (taskId, subtaskName) => {
    try {
      await tasksApi.createSubtask(taskId, subtaskName);
      await loadTasks(); // Refresh from server
    } catch (err) {
      console.error('Failed to add subtask:', err);
      alert('Failed to add subtask. Please try again.');
    }
  };

  const toggleSubtask = async (taskId, subtaskId) => {
    const task = tasks.find(t => t.id === taskId);
    const subtask = task?.subtasks.find(s => s.id === subtaskId);
    if (subtask) {
      try {
        await tasksApi.updateSubtask(taskId, subtaskId, { completed: !subtask.completed });
        await loadTasks(); // Refresh from server
      } catch (err) {
        console.error('Failed to toggle subtask:', err);
        alert('Failed to update subtask. Please try again.');
      }
    }
  };

  const deleteSubtask = async (taskId, subtaskId) => {
    try {
      await tasksApi.deleteSubtask(taskId, subtaskId);
      await loadTasks(); // Refresh from server
    } catch (err) {
      console.error('Failed to delete subtask:', err);
      alert('Failed to delete subtask. Please try again.');
    }
  };

  const moveTask = async (fromStatus, toStatus, fromIndex, toIndex, taskId) => {
    // Optimistically update UI
    setTasks((prev) => {
      const task = prev.find((t) => t.id === taskId);
      if (!task) return prev;
      const newTasks = prev.map(t => 
        t.id === taskId ? { ...t, status: toStatus } : t
      );
      return newTasks;
    });

    // Update on server
    try {
      await tasksApi.updateTask(taskId, { status: toStatus });
      // Notify sidebar to update task count
      window.dispatchEvent(new Event('taskUpdated'));
    } catch (err) {
      console.error('Failed to move task:', err);
      // Revert on error
      await loadTasks();
    }
  };

  const exportTasks = () => {
    const dataStr = JSON.stringify(tasks, null, 2);
    const blob = new Blob([dataStr], { type: 'application/json' });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = 'tasks.json';
    link.click();
    URL.revokeObjectURL(url);
  };

  if (loading || authLoading) {
    return (
      <div className="task-manager">
        <div className="loading">Loading tasks...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="task-manager">
        <div className="error-message">{error}</div>
        <button onClick={() => navigate('/login')} className="retry-button">Go to Login</button>
      </div>
    );
  }

  return (
    <DndProvider backend={HTML5Backend}>
      <motion.div
        className="task-manager"
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        transition={{ duration: 0.5 }}
      >
        <div className="header-container">
          <h1 className="task-title">Task Management</h1>
        </div>
        <div className="filter-container">
          <input
            type="text"
            placeholder="Search tasks..."
            className="search-input"
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
          />
          <select
            className="filter-select"
            value={filterStatus}
            onChange={(e) => setFilterStatus(e.target.value)}
          >
            <option value="All">All Statuses</option>
            <option value="Pending">Not Started</option>
            <option value="In Progress">In Progress</option>
            <option value="Completed">Completed</option>
          </select>
          <select
            className="filter-select"
            value={filterPerson}
            onChange={(e) => setFilterPerson(e.target.value)}
          >
            <option value="">All People</option>
            {uniquePeople.map((person) => (
              <option key={person} value={person}>
                {person}
              </option>
            ))}
          </select>
          <button className="add-task-button" onClick={() => setIsAddModalOpen(true)}>
            Add Task
          </button>
          <button className="export-button" onClick={exportTasks}>
            Export Tasks
          </button>
        </div>
        <div className="kanban-board">
          {['Pending', 'In Progress', 'Completed'].map((status) => (
            <motion.div
              key={status}
              className="kanban-column"
              initial={{ opacity: 0, x: -20 }}
              animate={{ opacity: 1, x: 0 }}
              transition={{ delay: 0.2 }}
            >
              <h2 className="column-title">{status === 'Pending' ? 'Not Started' : status}</h2>
              <ul className="task-list">
                <AnimatePresence>
                  {filteredTasks
                    .filter((task) => task.status === status)
                    .map((task, index) => (
                      <TaskItem
                        key={task.id}
                        task={task}
                        index={index}
                        status={status}
                        moveTask={moveTask}
                        onDelete={() => setIsDeleteModalOpen(task.id)}
                        onEdit={editTask}
                        onAddSubtask={addSubtask}
                        onToggleSubtask={toggleSubtask}
                        onDeleteSubtask={deleteSubtask}
                      />
                    ))}
                </AnimatePresence>
              </ul>
            </motion.div>
          ))}
        </div>
        <Modal
          isOpen={isAddModalOpen}
          onRequestClose={() => setIsAddModalOpen(false)}
          className="modal"
          overlayClassName="modal-overlay"
        >
          <motion.div
            initial={{ scale: 0.8, opacity: 0 }}
            animate={{ scale: 1, opacity: 1 }}
            exit={{ scale: 0.8, opacity: 0 }}
          >
            <h2 className="modal-title">Add New Task</h2>
            <form onSubmit={handleAddTask}>
              <input
                type="text"
                placeholder="Task Name"
                className="modal-input"
                value={newTask.name}
                onChange={(e) => setNewTask({ ...newTask, name: e.target.value })}
                required
              />
              <textarea
                placeholder="Description"
                className="modal-textarea"
                value={newTask.description}
                onChange={(e) => setNewTask({ ...newTask, description: e.target.value })}
              />
              <textarea
                placeholder="Notes"
                className="modal-textarea"
                value={newTask.notes}
                onChange={(e) => setNewTask({ ...newTask, notes: e.target.value })}
              />
              <input
                type="text"
                placeholder="Tags (comma-separated)"
                className="modal-input"
                value={newTask.tags}
                onChange={(e) => setNewTask({ ...newTask, tags: e.target.value })}
              />
              <input
                type="date"
                className="modal-input"
                value={newTask.dueDate}
                onChange={(e) => setNewTask({ ...newTask, dueDate: e.target.value })}
                required
              />
              <select
                className="modal-select"
                value={newTask.priority}
                onChange={(e) => setNewTask({ ...newTask, priority: e.target.value })}
              >
                <option value="Low">Low</option>
                <option value="Medium">Medium</option>
                <option value="High">High</option>
              </select>
              <input
                type="text"
                placeholder="Responsible Person"
                className="modal-input"
                value={newTask.person}
                onChange={(e) => setNewTask({ ...newTask, person: e.target.value })}
                required
              />
              <select
                className="modal-select"
                value={newTask.status}
                onChange={(e) => setNewTask({ ...newTask, status: e.target.value })}
              >
                <option value="Pending">Not Started</option>
                <option value="In Progress">In Progress</option>
                <option value="Completed">Completed</option>
              </select>
              <select
                className="modal-select"
                value={newTask.category}
                onChange={(e) => setNewTask({ ...newTask, category: e.target.value })}
              >
                <option value="Frontend">Frontend</option>
                <option value="Backend">Backend</option>
                <option value="Presentation">Presentation</option>
                <option value="Other">Other</option>
              </select>
              <div className="modal-buttons">
                <button type="button" className="cancel-button" onClick={() => setIsAddModalOpen(false)}>
                  Cancel
                </button>
                <button type="submit" className="submit-button">
                  Add
                </button>
              </div>
            </form>
          </motion.div>
        </Modal>
        <Modal
          isOpen={!!isDeleteModalOpen}
          onRequestClose={() => setIsDeleteModalOpen(null)}
          className="modal"
          overlayClassName="modal-overlay"
        >
          <motion.div
            initial={{ scale: 0.8, opacity: 0 }}
            animate={{ scale: 1, opacity: 1 }}
            exit={{ scale: 0.8, opacity: 0 }}
          >
            <h2 className="modal-title">Confirm Deletion</h2>
            <p>Are you sure you want to delete this task?</p>
            <div className="modal-buttons">
              <button
                className="delete-button"
                onClick={() => deleteTask(isDeleteModalOpen)}
              >
                Delete
              </button>
              <button
                className="cancel-button"
                onClick={() => setIsDeleteModalOpen(null)}
              >
                Cancel
              </button>
            </div>
          </motion.div>
        </Modal>
      </motion.div>
    </DndProvider>
  );
};

export default TaskManager;
