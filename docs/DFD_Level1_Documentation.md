# GradLink System - Data Flow Diagram Level 1

## Overview

This document describes the Level 1 Data Flow Diagram (DFD) for the GradLink System. The DFD decomposes the system into six main processes, showing how data flows between external entities, processes, and data stores.

---

## External Entities

| ID | Entity | Description |
|----|--------|-------------|
| **E1** | Student / Graduate | Users who create accounts, submit projects, track tasks, apply for jobs, and upload resumes for AI analysis |
| **E2** | Supervisor / Admin | Faculty members or administrators who supervise projects, manage users, and monitor system activities |
| **E3** | Company / Employer | Organizations that post job opportunities, internships, and review candidate applications |
| **E4** | HR Manager | Human resources personnel who use AI resume screening to evaluate candidates |

---

## Processes

### 1.0 User Authentication & Account Management
**Purpose:** Handles user registration, login, profile management, and access control.

| Input Data Flows | Source | Output Data Flows | Destination |
|------------------|--------|-------------------|-------------|
| Login credentials | E1, E2, E3 | Auth token/status | E1, E2, E3 |
| Registration data | E1, E2, E3 | User data | D1 |
| Profile updates | E1, E2, E3 | Updated profile | D1 |

---

### 2.0 Project Management
**Purpose:** Manages graduation projects, including creation, team assignment, and supervision.

| Input Data Flows | Source | Output Data Flows | Destination |
|------------------|--------|-------------------|-------------|
| Project details | E1 | Project data | D2 |
| Supervision data | E2 | Team member refs | D1 |
| Project info | D2 | Task assignment | P3 |

---

### 3.0 Task & Progress Tracking
**Purpose:** Tracks project tasks, milestones, deadlines, and overall progress.

| Input Data Flows | Source | Output Data Flows | Destination |
|------------------|--------|-------------------|-------------|
| Task assignment | P2 | Task data | D3 |
| Project context | D2 | Notify event | P4 |
| Task updates | E1, E2 | Progress reports | E1, E2 |

---

### 4.0 Messaging & Notifications
**Purpose:** Handles real-time messaging and system notifications across all users.

| Input Data Flows | Source | Output Data Flows | Destination |
|------------------|--------|-------------------|-------------|
| Notify event | P3 | Notifications | E1 |
| User contacts | D1 | Admin alerts | E2 |
| Company messages | E3 | Message delivery | E1, E2, E3 |

---

### 5.0 Job-Based Projects Management
**Purpose:** Manages job postings, internships, and connects students with employers.

| Input Data Flows | Source | Output Data Flows | Destination |
|------------------|--------|-------------------|-------------|
| Job/internship details | E3 | Job data | D4 |
| Job listings | D4 | Job opportunities | E1 |
| Applications | E1 | Job requirements | P6 |

---

### 6.0 AI Resume Screening
**Purpose:** Uses artificial intelligence to analyze resumes and match candidates with job requirements.

| Input Data Flows | Source | Output Data Flows | Destination |
|------------------|--------|-------------------|-------------|
| Resume upload | E1 | Analysis results | D5 |
| Job requirements | P5 | Resume score & feedback | E1 |
| Screening requests | E4 | Screening results | E4 |
| Historical data | D5 | Candidate ranking | E3, E4 |

---

## Data Stores

| ID | Data Store | Description | Related Processes |
|----|------------|-------------|-------------------|
| **D1** | Users Database | Stores user accounts, profiles, roles, and authentication data | P1, P2, P4 |
| **D2** | Projects Database | Contains graduation project information, teams, and milestones | P2, P3 |
| **D3** | Tasks Database | Stores task details, assignments, deadlines, and completion status | P3 |
| **D4** | Jobs Database | Contains job postings, internships, and application records | P5 |
| **D5** | Resume Analysis Database | Stores uploaded resumes, AI analysis results, and screening history | P6 |

---

## Data Flow Summary

### Authentication Flows (Pink)
```
E1/E2/E3 ──[credentials]──> P1 ──[auth token]──> E1/E2/E3
                            │
                            └──[user data]──> D1
```

### Project Management Flows (Yellow)
```
E1 ──[project details]──> P2 ──[project data]──> D2
E2 ──[supervision]──────> P2 ──[team refs]────> D1
```

### Task Tracking Flows (Green)
```
P2 ──[task assignment]──> P3 ──[task data]──> D3
D2 ──[project context]──> P3
```

### Notification Flows (Blue)
```
P3 ──[notify event]──> P4 ──[notifications]──> E1/E2
D1 ──[user contacts]──> P4
E3 ──[messages]───────> P4
```

### Job Management Flows (Orange)
```
E3 ──[job details]──> P5 ──[job data]──> D4
D4 ──[job listings]──> P5 ──[opportunities]──> E1
```

### AI Resume Flows (Purple)
```
E1 ──[resume upload]──> P6 ──[analysis]──> D5
P5 ──[requirements]──> P6 ──[score/feedback]──> E1
E4 ──[screening req]──> P6 ──[results]──> E4
```

---

## DFD Notation Reference

| Symbol | Representation | Description |
|--------|----------------|-------------|
| Rectangle | External Entity | Users or systems outside the boundary |
| Circle/Ellipse | Process | Data transformation or activity |
| Open Rectangle | Data Store | Persistent data repository |
| Arrow | Data Flow | Direction of data movement |

---

## Process-Entity Matrix

| Process | E1 (Student) | E2 (Supervisor) | E3 (Company) | E4 (HR) |
|---------|--------------|-----------------|--------------|---------|
| P1 - Authentication | ✓ | ✓ | ✓ | ✓ |
| P2 - Project Mgmt | ✓ | ✓ | - | - |
| P3 - Task Tracking | ✓ | ✓ | - | - |
| P4 - Messaging | ✓ | ✓ | ✓ | - |
| P5 - Job Projects | ✓ | - | ✓ | - |
| P6 - AI Resume | ✓ | - | - | ✓ |

---

## Process-Data Store Matrix

| Process | D1 (Users) | D2 (Projects) | D3 (Tasks) | D4 (Jobs) | D5 (Resume) |
|---------|------------|---------------|------------|-----------|-------------|
| P1 - Authentication | R/W | - | - | - | - |
| P2 - Project Mgmt | R | R/W | - | - | - |
| P3 - Task Tracking | - | R | R/W | - | - |
| P4 - Messaging | R | - | - | - | - |
| P5 - Job Projects | - | - | - | R/W | - |
| P6 - AI Resume | - | - | - | R | R/W |

*R = Read, W = Write, R/W = Read and Write*

---

## File Location

The interactive HTML version of this DFD can be found at:
```
docs/DFD_Level1_GradLink.html
```

Open this file in a web browser to view the full diagram with:
- Color-coded processes
- Animated data flows
- Export to PNG functionality

---

## Version History

| Version | Date | Description |
|---------|------|-------------|
| 1.0 | 2026-02-02 | Initial DFD Level 1 creation |

---

*GradLink System - Graduation Project Documentation*











