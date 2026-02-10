# GradLink System - Activity Diagram Documentation

## 📋 Overview

This document describes the UML Activity Diagram for the **Student Graduation Project Workflow** in the GradLink System.

---

## 🔄 Workflow Summary

The activity diagram illustrates the complete process a student follows from logging into the system to submitting their final graduation project.

```
┌─────────────────────────────────────────────────────────────┐
│                    PROJECT WORKFLOW                          │
├─────────────────────────────────────────────────────────────┤
│  ● Start                                                     │
│  ↓                                                           │
│  [Login to System]                                           │
│  ↓                                                           │
│  [Create Graduation Project]                                 │
│  ↓                                                           │
│  [Add Project Details & Team]                                │
│  ↓                                                           │
│  ◇ Mentor Assigned? ──[No]──→ [Request Mentor]              │
│  │                              ↓                            │
│  │                           [Wait for Approval]             │
│  │                              ↓                            │
│  [Yes]←─────────────────────────┘                           │
│  ↓                                                           │
│  [Create & Assign Tasks]                                     │
│  ↓                                                           │
│  [Work on Tasks] ←──────────────┐                           │
│  ↓                              │                            │
│  ◇ All Tasks Complete? ──[No]──┘                            │
│  │                                                           │
│  [Yes]                                                       │
│  ↓                                                           │
│  [Submit Final Project]                                      │
│  ↓                                                           │
│  ◉ End                                                       │
└─────────────────────────────────────────────────────────────┘
```

---

## 📊 Activity Descriptions

### 1. Login to System
| Aspect | Description |
|--------|-------------|
| **Actor** | Student |
| **Input** | Email, Password |
| **Output** | Authenticated session |
| **System Action** | Validate credentials, generate JWT token |

### 2. Create Graduation Project
| Aspect | Description |
|--------|-------------|
| **Actor** | Student |
| **Input** | Project title |
| **Output** | New project record |
| **System Action** | Create project in database |

### 3. Add Project Details & Team
| Aspect | Description |
|--------|-------------|
| **Actor** | Student |
| **Input** | Description, Category, Team members, Technologies |
| **Output** | Complete project information |
| **System Action** | Update project, create team member records |

### 4. Request Mentor (Conditional)
| Aspect | Description |
|--------|-------------|
| **Condition** | Mentor not assigned |
| **Actor** | Student |
| **Input** | Mentor request with goals |
| **Output** | Pending mentorship request |
| **System Action** | Create mentorship relation with "Pending" status |

### 5. Wait for Approval
| Aspect | Description |
|--------|-------------|
| **Actor** | System (Student waits) |
| **Input** | Mentor's decision |
| **Output** | Approved/Rejected mentorship |
| **System Action** | Notify student of decision |

### 6. Create & Assign Tasks
| Aspect | Description |
|--------|-------------|
| **Actor** | Student |
| **Input** | Task details (name, description, priority, due date) |
| **Output** | Task list for project |
| **System Action** | Create tasks, assign to team members |

### 7. Work on Tasks
| Aspect | Description |
|--------|-------------|
| **Actor** | Student, Team Members |
| **Input** | Work progress, file uploads |
| **Output** | Updated task status |
| **System Action** | Update task completion, calculate progress |

### 8. Submit Final Project
| Aspect | Description |
|--------|-------------|
| **Condition** | All tasks completed |
| **Actor** | Student |
| **Input** | Final submission confirmation |
| **Output** | Completed project |
| **System Action** | Update project status to "Completed" |

---

## ◇ Decision Points

### Decision 1: Mentor Assigned?
| Branch | Condition | Next Activity |
|--------|-----------|---------------|
| **Yes** | Project has assigned mentor | Create & Assign Tasks |
| **No** | No mentor assigned | Request Mentor |

### Decision 2: All Tasks Complete?
| Branch | Condition | Next Activity |
|--------|-----------|---------------|
| **Yes** | All tasks have "Completed" status | Submit Final Project |
| **No** | One or more tasks pending | Work on Tasks (loop) |

---

## 📐 UML Notation Reference

### Node Types

| Symbol | Name | Description |
|--------|------|-------------|
| ● (filled circle) | Initial Node | Starting point of the workflow |
| ◉ (circle with ring) | Final Node | Ending point of the workflow |
| ▢ (rounded rectangle) | Activity | Action or task to be performed |
| ◇ (diamond) | Decision | Branch point based on condition |
| ◆ (filled diamond) | Merge | Joins multiple flows into one |

### Flow Types

| Symbol | Name | Description |
|--------|------|-------------|
| → (arrow) | Control Flow | Direction of activity sequence |
| [condition] | Guard | Condition for taking a path |

---

## 🔀 Alternative Paths

### Happy Path (Mentor Already Assigned)
```
Login → Create Project → Add Details → [Mentor: Yes] → Create Tasks → Work → [Complete: Yes] → Submit → End
```

### Mentor Request Path
```
Login → Create Project → Add Details → [Mentor: No] → Request Mentor → Wait → Create Tasks → ...
```

### Task Loop Path
```
... → Work on Tasks → [Complete: No] → Work on Tasks → [Complete: Yes] → Submit → End
```

---

## ⏱️ Time Estimates

| Activity | Typical Duration |
|----------|------------------|
| Create Project | 5-10 minutes |
| Add Details | 15-30 minutes |
| Request Mentor | 5 minutes |
| Wait for Approval | 1-7 days |
| Create Tasks | 30-60 minutes |
| Work on Tasks | Weeks/Months |
| Submit Project | 10-15 minutes |

---

## 📁 File Locations

| File | Path |
|------|------|
| Activity Diagram HTML | `docs/Activity_Diagram_Project.html` |
| Activity Documentation | `docs/Activity_Diagram_Documentation.md` |

---

## 🔄 Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0 | 2026-02-02 | Initial diagram creation |

---

*GradLink System - Graduation Project Documentation*











