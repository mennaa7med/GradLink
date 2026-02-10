# GradLink System - Use Case Diagram Documentation

## 📋 Overview

This document provides a comprehensive description of the UML Use Case Diagram for the **GradLink System** - a web-based graduation project management and career platform.

### System Description
GradLink is designed to facilitate graduation project management by connecting students with mentors, companies, and sponsors. The platform provides tools for project tracking, task management, communication, and career opportunities.

---

## 👥 Actors

### Primary Actors

| Actor | Description | Responsibilities |
|-------|-------------|------------------|
| **Student** | University students working on graduation projects | Create projects, manage tasks, upload files, communicate with mentors, apply for jobs, upload resumes |
| **Mentor** | Faculty members or industry experts | View student projects, provide feedback, communicate with students |
| **Company** | Employers and recruiters | Post job opportunities, view applications, screen resumes |
| **Admin** | System administrators | Manage users, manage content, monitor system activities |

---

## 📊 Use Case Summary

| # | Use Case | Primary Actor(s) | Category |
|---|----------|------------------|----------|
| UC1 | Register Account | Student | Authentication |
| UC2 | Login | All Actors | Authentication |
| UC3 | Manage Profile | All Actors | Authentication |
| UC4 | Create Graduation Project | Student | Project Management |
| UC5 | View Dashboard | Student | Project Management |
| UC6 | Manage Tasks | Student | Task Management |
| UC7 | Upload Project Files | Student | Project Management |
| UC8 | Create Subtask | Student | Task Management |
| UC9 | Track Progress | Student | Task Management |
| UC10 | Communicate with Mentor | Student, Mentor | Communication |
| UC11 | Send Message | Student, Mentor | Communication |
| UC12 | View Notifications | All Actors | Communication |
| UC13 | Apply for Job-Based Projects | Student | Career |
| UC14 | Upload Resume | Student | AI Screening |
| UC15 | View Job Listings | Student | Career |
| UC16 | AI Resume Analysis | System | AI Screening |
| UC17 | View Student Projects | Mentor | Project Management |
| UC18 | Provide Feedback | Mentor | Communication |
| UC19 | Post Job-Based Projects | Company | Career |
| UC20 | Post Internships | Company | Career |
| UC21 | View Student Applications | Company | Career |
| UC22 | Screen Resumes | Company | AI Screening |
| UC23 | AI Matching | System | AI Screening |
| UC24 | Manage Users | Admin | Administration |
| UC25 | Manage Content | Admin | Administration |
| UC26 | Monitor System Activities | Admin | Administration |
| UC27 | Generate Reports | Admin | Administration |

---

## 📝 Detailed Use Case Descriptions

### Authentication Use Cases

---

#### UC1: Register Account

| Field | Description |
|-------|-------------|
| **Use Case ID** | UC1 |
| **Name** | Register Account |
| **Actor(s)** | Student |
| **Description** | A new user registers an account on the GradLink platform |
| **Preconditions** | User has valid email address |
| **Postconditions** | User account is created and ready for use |
| **Main Flow** | 1. User navigates to registration page<br>2. User enters personal information (name, email, password)<br>3. User selects role (Student/Graduate)<br>4. User submits registration form<br>5. System validates information<br>6. System creates account<br>7. System sends verification email |
| **Alternative Flow** | 5a. Email already exists → Display error message |

---

#### UC2: Login

| Field | Description |
|-------|-------------|
| **Use Case ID** | UC2 |
| **Name** | Login |
| **Actor(s)** | Student, Mentor, Company, Admin |
| **Description** | User authenticates to access the system |
| **Preconditions** | User has registered account |
| **Postconditions** | User is logged in and redirected to dashboard |
| **Main Flow** | 1. User enters email and password<br>2. System validates credentials<br>3. System generates authentication token<br>4. System redirects to appropriate dashboard |
| **Alternative Flow** | 2a. Invalid credentials → Display error<br>2b. Account locked → Display lockout message |

---

#### UC3: Manage Profile

| Field | Description |
|-------|-------------|
| **Use Case ID** | UC3 |
| **Name** | Manage Profile |
| **Actor(s)** | All Actors |
| **Description** | User updates their profile information |
| **Preconditions** | User is logged in |
| **Postconditions** | Profile information is updated |
| **Main Flow** | 1. User navigates to profile settings<br>2. User updates desired fields<br>3. User saves changes<br>4. System validates and saves data |

---

### Project Management Use Cases

---

#### UC4: Create Graduation Project

| Field | Description |
|-------|-------------|
| **Use Case ID** | UC4 |
| **Name** | Create Graduation Project |
| **Actor(s)** | Student |
| **Description** | Student creates a new graduation project |
| **Preconditions** | Student is logged in |
| **Postconditions** | New project is created in the system |
| **Main Flow** | 1. Student clicks "Create Project"<br>2. Student enters project details (title, description, category)<br>3. Student optionally adds team members<br>4. Student submits project<br>5. System creates project record |

---

#### UC5: View Dashboard

| Field | Description |
|-------|-------------|
| **Use Case ID** | UC5 |
| **Name** | View Dashboard |
| **Actor(s)** | Student |
| **Description** | Student views their personalized dashboard |
| **Preconditions** | Student is logged in |
| **Postconditions** | Dashboard data is displayed |
| **Main Flow** | 1. System retrieves user's projects<br>2. System retrieves pending tasks<br>3. System retrieves recent notifications<br>4. System displays dashboard overview |
| **Includes** | UC9: Track Progress |

---

#### UC6: Manage Tasks

| Field | Description |
|-------|-------------|
| **Use Case ID** | UC6 |
| **Name** | Manage Tasks |
| **Actor(s)** | Student |
| **Description** | Student creates, updates, and manages project tasks |
| **Preconditions** | Student is logged in, has active project |
| **Postconditions** | Task is created/updated |
| **Main Flow** | 1. Student selects project<br>2. Student creates/selects task<br>3. Student sets task details (name, priority, due date)<br>4. Student saves task<br>5. System updates project progress |
| **Includes** | UC8: Create Subtask |

---

#### UC7: Upload Project Files

| Field | Description |
|-------|-------------|
| **Use Case ID** | UC7 |
| **Name** | Upload Project Files |
| **Actor(s)** | Student |
| **Description** | Student uploads files related to their project |
| **Preconditions** | Student is logged in, has active project |
| **Postconditions** | File is stored and linked to project |
| **Main Flow** | 1. Student navigates to project files<br>2. Student selects file to upload<br>3. System validates file type and size<br>4. System uploads and stores file<br>5. System links file to project |

---

### Communication Use Cases

---

#### UC10: Communicate with Mentor

| Field | Description |
|-------|-------------|
| **Use Case ID** | UC10 |
| **Name** | Communicate with Mentor |
| **Actor(s)** | Student, Mentor |
| **Description** | Students and mentors exchange messages |
| **Preconditions** | Both parties are registered users |
| **Postconditions** | Message is delivered to recipient |
| **Main Flow** | 1. User opens conversation<br>2. User composes message<br>3. User sends message<br>4. System delivers message to recipient<br>5. System sends notification |
| **Includes** | UC11: Send Message |

---

#### UC12: View Notifications

| Field | Description |
|-------|-------------|
| **Use Case ID** | UC12 |
| **Name** | View Notifications |
| **Actor(s)** | All Actors |
| **Description** | User views system notifications |
| **Preconditions** | User is logged in |
| **Postconditions** | Notifications are displayed |
| **Main Flow** | 1. User clicks notification icon<br>2. System retrieves user notifications<br>3. System displays notification list<br>4. User can mark notifications as read |
| **Extends** | UC2: Login |

---

### Career Use Cases

---

#### UC13: Apply for Job-Based Projects

| Field | Description |
|-------|-------------|
| **Use Case ID** | UC13 |
| **Name** | Apply for Job-Based Projects |
| **Actor(s)** | Student |
| **Description** | Student applies for job opportunities posted by companies |
| **Preconditions** | Student is logged in, job posting is active |
| **Postconditions** | Application is submitted to company |
| **Main Flow** | 1. Student browses job listings<br>2. Student selects job posting<br>3. Student attaches resume<br>4. Student writes cover letter (optional)<br>5. Student submits application<br>6. System notifies company |
| **Extends** | UC15: View Job Listings |

---

#### UC14: Upload Resume

| Field | Description |
|-------|-------------|
| **Use Case ID** | UC14 |
| **Name** | Upload Resume |
| **Actor(s)** | Student |
| **Description** | Student uploads their resume for analysis |
| **Preconditions** | Student is logged in |
| **Postconditions** | Resume is stored and analyzed |
| **Main Flow** | 1. Student navigates to resume section<br>2. Student uploads PDF/DOC file<br>3. System extracts text content<br>4. System stores resume<br>5. System triggers AI analysis |
| **Extends** | UC16: AI Resume Analysis |

---

#### UC19: Post Job-Based Projects

| Field | Description |
|-------|-------------|
| **Use Case ID** | UC19 |
| **Name** | Post Job-Based Projects |
| **Actor(s)** | Company |
| **Description** | Company posts a new job opportunity |
| **Preconditions** | Company is logged in |
| **Postconditions** | Job posting is published |
| **Main Flow** | 1. Company clicks "Post Job"<br>2. Company enters job details<br>3. Company sets requirements and skills<br>4. Company publishes posting<br>5. System makes job visible to students |

---

#### UC22: Screen Resumes

| Field | Description |
|-------|-------------|
| **Use Case ID** | UC22 |
| **Name** | Screen Resumes |
| **Actor(s)** | Company |
| **Description** | Company reviews and screens applicant resumes |
| **Preconditions** | Company has received applications |
| **Postconditions** | Resumes are reviewed and ranked |
| **Main Flow** | 1. Company views applications<br>2. System displays resume list with AI scores<br>3. Company reviews individual resumes<br>4. Company updates application status |
| **Includes** | UC23: AI Matching |

---

### Administration Use Cases

---

#### UC24: Manage Users

| Field | Description |
|-------|-------------|
| **Use Case ID** | UC24 |
| **Name** | Manage Users |
| **Actor(s)** | Admin |
| **Description** | Admin manages user accounts |
| **Preconditions** | Admin is logged in |
| **Postconditions** | User account is updated |
| **Main Flow** | 1. Admin navigates to user management<br>2. Admin searches/filters users<br>3. Admin selects user<br>4. Admin performs action (edit/suspend/delete)<br>5. System updates user record |

---

#### UC26: Monitor System Activities

| Field | Description |
|-------|-------------|
| **Use Case ID** | UC26 |
| **Name** | Monitor System Activities |
| **Actor(s)** | Admin |
| **Description** | Admin monitors system usage and activities |
| **Preconditions** | Admin is logged in |
| **Postconditions** | Activity data is displayed |
| **Main Flow** | 1. Admin navigates to monitoring dashboard<br>2. System retrieves activity logs<br>3. System displays usage statistics<br>4. Admin can filter by date/type/user |

---

## 🔗 Relationship Types

### Include Relationships (<<include>>)

Include relationships represent required functionality that is always executed.

| Base Use Case | Included Use Case | Description |
|---------------|-------------------|-------------|
| Manage Tasks | Create Subtask | Creating a subtask is part of task management |
| View Dashboard | Track Progress | Viewing progress is included in dashboard |
| Communicate with Mentor | Send Message | Sending messages is required for communication |
| Screen Resumes | AI Matching | AI matching is integral to screening |

### Extend Relationships (<<extend>>)

Extend relationships represent optional or conditional behavior.

| Base Use Case | Extending Use Case | Condition |
|---------------|-------------------|-----------|
| Upload Resume | AI Resume Analysis | When resume is uploaded |
| Login | View Notifications | After successful login |
| Apply for Job-Based Projects | View Job Listings | Before applying |

---

## 📊 Actor-Use Case Matrix

| Use Case | Student | Mentor | Company | Admin |
|----------|:-------:|:------:|:-------:|:-----:|
| Register Account | ✓ | - | - | - |
| Login | ✓ | ✓ | ✓ | ✓ |
| Manage Profile | ✓ | ✓ | ✓ | ✓ |
| Create Graduation Project | ✓ | - | - | - |
| View Dashboard | ✓ | - | - | - |
| Manage Tasks | ✓ | - | - | - |
| Upload Project Files | ✓ | - | - | - |
| Communicate with Mentor | ✓ | ✓ | - | - |
| Apply for Job-Based Projects | ✓ | - | - | - |
| Upload Resume | ✓ | - | - | - |
| View Student Projects | - | ✓ | - | - |
| Provide Feedback | - | ✓ | - | - |
| Post Job-Based Projects | - | - | ✓ | - |
| Post Internships | - | - | ✓ | - |
| View Student Applications | - | - | ✓ | - |
| Screen Resumes | - | - | ✓ | - |
| Manage Users | - | - | - | ✓ |
| Manage Content | - | - | - | ✓ |
| Monitor System Activities | - | - | - | ✓ |
| Generate Reports | - | - | - | ✓ |

---

## 📐 UML Notation Reference

### Symbols Used

| Symbol | Name | Description |
|--------|------|-------------|
| 🧍 Stick Figure | Actor | External entity that interacts with the system |
| ⭕ Ellipse | Use Case | System functionality or service |
| ▭ Rectangle | System Boundary | Defines the scope of the system |
| ─── Solid Line | Association | Actor participates in use case |
| - - - > Dashed Arrow | Include/Extend | Relationship between use cases |

### Relationship Notation

```
Actor ─────────── Use Case       (Association)

Use Case A - - - «include» - - - > Use Case B
(A always includes B)

Use Case A < - - - «extend» - - - Use Case B
(B optionally extends A)
```

---

## 📁 File Locations

| File | Path |
|------|------|
| Use Case Diagram HTML | `docs/UseCase_Diagram_GradLink.html` |
| Use Case Documentation | `docs/UseCase_Documentation.md` |

---

## 📊 Statistics

| Metric | Count |
|--------|-------|
| Total Actors | 4 |
| Total Use Cases | 27 |
| Include Relationships | 4 |
| Extend Relationships | 3 |
| Authentication Use Cases | 3 |
| Project Management Use Cases | 5 |
| Communication Use Cases | 4 |
| Career Use Cases | 7 |
| Administration Use Cases | 4 |

---

## 🔄 Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0 | 2026-02-02 | Initial Use Case Diagram creation |

---

*GradLink System - Graduation Project Documentation*











