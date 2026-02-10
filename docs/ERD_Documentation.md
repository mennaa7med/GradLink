# GradLink System - Entity Relationship Diagram Documentation

## 📋 Overview

This document provides a comprehensive description of the Entity Relationship Diagram (ERD) for the **GradLink System** - a graduation project management platform that connects students, mentors, companies, and sponsors.

### System Scope
- **Frontend**: React.js with Vite
- **Backend**: ASP.NET Core Web API
- **AI Service**: Flask Python API (Resume Screening)
- **Database**: SQL Server / PostgreSQL

### User Roles
| Role | Description |
|------|-------------|
| **Student** | University students working on graduation projects |
| **Graduate** | Alumni who completed their graduation projects |
| **Mentor** | Faculty/industry experts providing guidance |
| **Company** | Employers posting jobs and internships |
| **Sponsor** | Organizations funding student projects |
| **Admin** | System administrators |

---

## 📊 Entity Summary

| # | Entity | Module | Description |
|---|--------|--------|-------------|
| 1 | User | Core | Central user account entity |
| 2 | StudentProfile | Core | Extended profile for students |
| 3 | CompanyProfile | Core | Extended profile for companies |
| 4 | MentorProfile | Core | Extended profile for mentors |
| 5 | Project | Project Mgmt | Graduation projects |
| 6 | TeamMember | Project Mgmt | Project team members |
| 7 | TaskItem | Task Mgmt | Tasks within projects |
| 8 | Subtask | Task Mgmt | Sub-tasks within tasks |
| 9 | JobPosting | Career | Job opportunities |
| 10 | JobApplication | Career | Job applications |
| 11 | Internship | Career | Internship opportunities |
| 12 | Conversation | Messaging | Chat conversations |
| 13 | ChatMessage | Messaging | Messages within conversations |
| 14 | Notification | Messaging | System notifications |
| 15 | MentorshipRelation | Mentorship | Mentor-student relationships |
| 16 | MentoringSession | Mentorship | Scheduled mentoring sessions |
| 17 | MentorReview | Mentorship | Reviews for mentors |
| 18 | Material | Resources | Learning materials |
| 19 | Favorite | Resources | User favorites |
| 20 | Resume | AI Screening | Uploaded resumes |
| 21 | Match | AI Screening | Resume-job matching results |
| 22 | Sponsor | Sponsorship | Sponsor organizations |
| 23 | SponsoredProject | Sponsorship | Sponsored projects |

---

## 🗂️ Detailed Entity Descriptions

### 1. User (ApplicationUser)

The central entity representing all system users. Inherits from ASP.NET Identity.

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | string | PK | Unique identifier (GUID) |
| Email | string | Unique, Required | User email address |
| PasswordHash | string | Required | Hashed password |
| FullName | string | - | User's full name |
| Role | string | Default: "Student" | User role (Student/Graduate/Company/Mentor/Admin) |
| ProfilePicture | string | - | Profile image URL |
| University | string | - | University name (for students) |
| Major | string | - | Field of study |
| GraduationYear | int | - | Expected/actual graduation year |
| Skills | string | - | User skills (comma-separated) |
| CompanyName | string | - | Company name (for employers) |
| AccountStatus | string | Default: "Active" | Account status |
| CreatedAt | datetime | Required | Registration timestamp |
| LastLoginAt | datetime | - | Last login timestamp |

**Relationships:**
- 1 User → M Projects (owns)
- 1 User → M TaskItems (creates)
- 1 User → M Resumes (uploads)
- 1 User → M JobPostings (posts)
- 1 User → M Favorites (has)
- 1 User → M Notifications (receives)
- 1 User → M MentorshipRelations (as Mentor)
- 1 User → M MentorshipRelations (as Mentee)

---

### 2. StudentProfile

Extended profile information specific to students.

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | int | PK | Unique identifier |
| UserId | string | FK, Unique | Reference to User |
| ProjectTitle | string | - | Graduation project title |
| ProjectDescription | string | - | Project description |
| ProjectCategory | string | - | Category (AI/Web/Mobile/IoT) |
| SupervisorName | string | - | Academic supervisor |
| IsLookingForMentor | bool | Default: false | Seeking mentor |
| FundingNeeded | decimal | - | Required funding amount |

**Relationships:**
- 1 StudentProfile ↔ 1 User (belongs to)

---

### 3. Project

Graduation projects created by students.

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | int | PK | Unique identifier |
| OwnerId | string | FK, Required | Project owner (User) |
| Title | string | Required | Project title |
| Description | string | - | Project description |
| Category | string | - | Project category |
| Status | string | Default: "Active" | Project status |
| CreatedAt | datetime | Required | Creation timestamp |
| UpdatedAt | datetime | - | Last update timestamp |

**Relationships:**
- M Projects ↔ 1 User (owned by)
- 1 Project → M TeamMembers (has)
- 1 Project → M TaskItems (contains)
- 1 Project → M SponsoredProjects (can be sponsored)

---

### 4. TeamMember

Members assigned to graduation projects.

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | int | PK | Unique identifier |
| ProjectId | int | FK, Required | Associated project |
| UserId | string | FK | Linked user account |
| FullName | string | Required | Member name |
| Email | string | Required | Member email |
| Role | string | Required | Team role (Frontend/Backend/Design) |
| Skills | string | - | Member skills |
| Rating | decimal | - | Performance rating |

**Relationships:**
- M TeamMembers ↔ 1 Project (belongs to)
- M TeamMembers ↔ 1 User (optionally linked)

---

### 5. TaskItem

Tasks within graduation projects.

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | int | PK | Unique identifier |
| UserId | string | FK, Required | Task owner |
| ProjectId | int | FK | Associated project |
| Name | string | Required | Task name |
| Description | string | - | Task description |
| Priority | string | Default: "Medium" | Priority level (Low/Medium/High) |
| Status | string | Default: "Pending" | Task status |
| DueDate | datetime | - | Due date |
| CreatedAt | datetime | Required | Creation timestamp |
| CompletedAt | datetime | - | Completion timestamp |

**Relationships:**
- M TaskItems ↔ 1 User (owned by)
- M TaskItems ↔ 1 Project (belongs to)
- 1 TaskItem → M Subtasks (has)

---

### 6. Subtask

Sub-tasks within main tasks.

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | int | PK | Unique identifier |
| TaskItemId | int | FK, Required | Parent task |
| Name | string | Required | Subtask name |
| Completed | bool | Default: false | Completion status |
| CreatedAt | datetime | Required | Creation timestamp |

**Relationships:**
- M Subtasks ↔ 1 TaskItem (belongs to)

---

### 7. JobPosting

Job opportunities posted by companies.

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | int | PK | Unique identifier |
| PostedById | string | FK, Required | Company/Employer |
| Title | string | Required | Job title |
| Description | string | Required | Job description |
| Requirements | string | - | Job requirements (JSON) |
| Skills | string | - | Required skills (JSON) |
| Location | string | - | Job location |
| EmploymentType | string | - | Full-time/Part-time/Contract |
| SalaryMin | decimal | - | Minimum salary |
| SalaryMax | decimal | - | Maximum salary |
| Status | string | Default: "Active" | Posting status |
| ExpiresAt | datetime | - | Expiration date |

**Relationships:**
- M JobPostings ↔ 1 User (posted by)
- 1 JobPosting → M JobApplications (receives)
- 1 JobPosting → M Matches (matched with resumes)

---

### 8. JobApplication

Applications submitted for job postings.

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | int | PK | Unique identifier |
| JobPostingId | int | FK, Required | Target job |
| ApplicantId | string | FK, Required | Applicant (User) |
| ResumeId | int | FK | Attached resume |
| CoverLetter | string | - | Cover letter text |
| Status | string | Default: "Pending" | Application status |
| AppliedAt | datetime | Required | Application timestamp |

**Relationships:**
- M JobApplications ↔ 1 JobPosting (for)
- M JobApplications ↔ 1 User (submitted by)
- M JobApplications ↔ 1 Resume (uses)

---

### 9. Internship

Internship opportunities posted by companies.

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | int | PK | Unique identifier |
| PostedById | string | FK, Required | Company/Employer |
| Title | string | Required | Internship title |
| Description | string | - | Description |
| CompanyName | string | - | Company name |
| Location | string | - | Work location |
| Duration | string | - | Internship duration |
| Stipend | decimal | - | Monthly stipend |
| Status | string | Default: "Active" | Posting status |

**Relationships:**
- M Internships ↔ 1 User (posted by)
- 1 Internship → M InternshipApplications (receives)

---

### 10. Conversation

Chat conversations between users.

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | int | PK | Unique identifier |
| User1Id | string | FK, Required | First participant |
| User2Id | string | FK, Required | Second participant |
| CreatedAt | datetime | Required | Creation timestamp |
| LastMessageAt | datetime | - | Last message timestamp |

**Relationships:**
- M Conversations ↔ 2 Users (participants)
- 1 Conversation → M ChatMessages (contains)

---

### 11. ChatMessage

Messages within conversations.

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | int | PK | Unique identifier |
| ConversationId | int | FK, Required | Parent conversation |
| SenderId | string | FK, Required | Message sender |
| Content | string | Required | Message content |
| SentAt | datetime | Required | Sent timestamp |
| IsRead | bool | Default: false | Read status |

**Relationships:**
- M ChatMessages ↔ 1 Conversation (belongs to)
- M ChatMessages ↔ 1 User (sent by)

---

### 12. Notification

System notifications for users.

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | int | PK | Unique identifier |
| UserId | string | FK, Required | Recipient user |
| Type | string | Required | Notification type |
| Title | string | Required | Notification title |
| Message | string | - | Notification message |
| Link | string | - | Action link |
| IsRead | bool | Default: false | Read status |
| CreatedAt | datetime | Required | Creation timestamp |

**Relationships:**
- M Notifications ↔ 1 User (sent to)

---

### 13. MentorshipRelation

Mentor-student guidance relationships.

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | int | PK | Unique identifier |
| MentorId | string | FK, Required | Mentor (User) |
| MenteeId | string | FK, Required | Student (User) |
| Status | string | Default: "Pending" | Relationship status |
| RequestMessage | string | - | Initial request message |
| MenteeGoals | string | - | Student's goals |
| TotalHours | decimal | Default: 0 | Total mentoring hours |
| RequestedAt | datetime | Required | Request timestamp |
| AcceptedAt | datetime | - | Acceptance timestamp |

**Relationships:**
- M MentorshipRelations ↔ 1 User (as Mentor)
- M MentorshipRelations ↔ 1 User (as Mentee)
- 1 MentorshipRelation → M MentoringSessions (has)

---

### 14. MentoringSession

Scheduled mentoring sessions.

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | int | PK | Unique identifier |
| MentorId | string | FK, Required | Mentor |
| MenteeId | string | FK, Required | Student |
| MentorshipRelationId | int | FK | Parent relationship |
| Topic | string | Required | Session topic |
| Description | string | - | Session description |
| ScheduledDate | datetime | Required | Scheduled date/time |
| DurationMinutes | int | - | Session duration |
| Status | string | Default: "Scheduled" | Session status |
| MeetingLink | string | - | Online meeting URL |

**Relationships:**
- M MentoringSessions ↔ 1 MentorshipRelation (part of)
- M MentoringSessions ↔ 1 User (as Mentor)
- M MentoringSessions ↔ 1 User (as Mentee)

---

### 15. MentorReview

Reviews and ratings for mentors.

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | int | PK | Unique identifier |
| MentorId | string | FK, Required | Reviewed mentor |
| ReviewerId | string | FK, Required | Review author |
| Rating | int | Required | Rating (1-5) |
| Comment | string | - | Review text |
| CreatedAt | datetime | Required | Review timestamp |
| IsApproved | bool | Default: false | Moderation status |

**Relationships:**
- M MentorReviews ↔ 1 User (for Mentor)
- M MentorReviews ↔ 1 User (by Reviewer)

---

### 16. Material

Learning resources and materials.

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | int | PK | Unique identifier |
| SubmittedById | string | FK | Uploader |
| Title | string | Required | Material title |
| Description | string | - | Description |
| Type | string | Required | Type (template/tool/book/video) |
| Field | string | - | Field/Category |
| Link | string | - | Access link |
| FilePath | string | - | File storage path |
| Downloads | int | Default: 0 | Download count |
| AverageRating | decimal | - | Average user rating |
| Status | string | Default: "pending" | Review status |

**Relationships:**
- M Materials ↔ 1 User (submitted by)
- 1 Material → M Favorites (favorited by)
- 1 Material → M MaterialRatings (rated by)

---

### 17. Favorite

User favorite materials (junction table).

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | int | PK | Unique identifier |
| UserId | string | FK, Required | User |
| MaterialId | int | FK, Required | Favorited material |
| CreatedAt | datetime | Required | Favorite timestamp |

**Relationships:**
- M Favorites ↔ 1 User (belongs to)
- M Favorites ↔ 1 Material (references)

---

### 18. Resume

Uploaded resumes for AI analysis.

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | int | PK | Unique identifier |
| UserId | string | FK, Required | Owner |
| FileName | string | Required | Original filename |
| FilePath | string | Required | Storage path |
| FileSize | long | - | File size in bytes |
| ExtractedText | string | - | Extracted text content |
| Skills | string | - | Detected skills (JSON) |
| Education | string | - | Detected education |
| Experience | string | - | Detected experience |
| QualityScore | double | - | AI quality score |
| AnalysisStatus | string | Default: "Pending" | Analysis status |
| UploadedAt | datetime | Required | Upload timestamp |
| AnalyzedAt | datetime | - | Analysis timestamp |

**Relationships:**
- M Resumes ↔ 1 User (owned by)
- 1 Resume → M Matches (generates)
- 1 Resume → M JobApplications (used in)

---

### 19. Match

AI-generated resume-job matching results.

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | int | PK | Unique identifier |
| ResumeId | int | FK, Required | Matched resume |
| JobPostingId | int | FK, Required | Matched job |
| Score | double | Required | Match score (0-100) |
| MatchedSkills | string | - | Matched skills (JSON) |
| MissingSkills | string | - | Missing skills (JSON) |
| MatchedAt | datetime | Required | Match timestamp |

**Relationships:**
- M Matches ↔ 1 Resume (for)
- M Matches ↔ 1 JobPosting (with)

---

### 20. Sponsor

Sponsor organizations.

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | int | PK | Unique identifier |
| Name | string | Required | Sponsor name |
| Field | string | - | Industry/Field |
| SupportedProject | string | - | Type of projects supported |
| Logo | string | - | Logo image URL |
| Link | string | - | Website URL |
| CreatedAt | datetime | Required | Registration timestamp |

**Relationships:**
- 1 Sponsor → M SponsorApplications (receives)

---

### 21. SponsoredProject

Projects receiving sponsorship.

| Attribute | Type | Constraint | Description |
|-----------|------|------------|-------------|
| Id | int | PK | Unique identifier |
| SponsorProfileId | int | FK, Required | Funding sponsor |
| ProjectId | int | FK | Sponsored project |
| StudentUserId | string | FK | Project student |
| ProjectTitle | string | Required | Project title |
| FundingAmount | decimal | - | Funding amount |
| FundingDelivered | decimal | Default: 0 | Amount delivered |
| Status | string | Default: "Pending" | Sponsorship status |
| CurrentMilestone | string | - | Current project milestone |

**Relationships:**
- M SponsoredProjects ↔ 1 SponsorProfile (funded by)
- M SponsoredProjects ↔ 1 Project (sponsors)
- M SponsoredProjects ↔ 1 User (student)

---

## 🔗 Relationship Summary

### One-to-Many (1:M) Relationships

| Parent Entity | Child Entity | Description |
|---------------|--------------|-------------|
| User | Project | User creates many projects |
| User | TaskItem | User creates many tasks |
| User | Resume | User uploads many resumes |
| User | JobPosting | Company posts many jobs |
| User | Notification | User receives many notifications |
| User | Favorite | User has many favorites |
| Project | TeamMember | Project has many team members |
| Project | TaskItem | Project contains many tasks |
| TaskItem | Subtask | Task has many subtasks |
| JobPosting | JobApplication | Job receives many applications |
| JobPosting | Match | Job matched with many resumes |
| Conversation | ChatMessage | Conversation has many messages |
| Resume | Match | Resume generates many matches |
| Material | Favorite | Material has many favorites |
| MentorshipRelation | MentoringSession | Relation has many sessions |

### One-to-One (1:1) Relationships

| Entity A | Entity B | Description |
|----------|----------|-------------|
| User | StudentProfile | User has one student profile |
| User | CompanyProfile | User has one company profile |
| User | MentorProfile | User has one mentor profile |
| User | SponsorProfile | User has one sponsor profile |

### Many-to-Many (M:M) Relationships

| Entity A | Entity B | Junction | Description |
|----------|----------|----------|-------------|
| User | Material | Favorite | Users favorite materials |
| User | Material | RecentlyViewed | Users view materials |
| User | Conversation | - | Users participate in conversations |

---

## 📐 Cardinality Notation

This ERD uses **Crow's Foot Notation**:

| Symbol | Meaning |
|--------|---------|
| `──────` | Line connecting entities |
| `──○` | Zero or one (optional) |
| `────│` | Exactly one (mandatory) |
| `──<` | Many (zero or more) |
| `──│<` | One or many (at least one) |

---

## 🎯 Design Decisions

### 1. Single User Table with Role-Based Profiles
Instead of separate tables for each user type, we use a single `User` table with extended profile tables (`StudentProfile`, `CompanyProfile`, `MentorProfile`) for role-specific data. This allows:
- Unified authentication
- Easier user management
- Flexible role switching

### 2. Soft Deletes
Critical entities use status fields instead of hard deletes:
- `AccountStatus` for users
- `Status` for projects, jobs, applications
- Preserves data integrity and audit trails

### 3. JSON Fields for Flexible Data
Complex, variable-length data stored as JSON:
- Skills arrays
- Requirements lists
- Preferences settings
- Allows schema flexibility without additional tables

### 4. Indexed Fields
Key fields indexed for query performance:
- Foreign keys
- Status fields
- Date fields (CreatedAt, DueDate)
- Search fields (Title, Email)

---

## 📁 File Locations

| File | Path |
|------|------|
| ERD HTML Diagram | `docs/ERD_GradLink_Professional.html` |
| ERD Documentation | `docs/ERD_Documentation.md` |
| Entity Classes | `backend/GradLink.Domain/Entities/` |
| DbContext | `backend/GradLink.Infrastructure/Persistence/AppDbContext.cs` |

---

## 📊 Database Statistics

| Metric | Value |
|--------|-------|
| Total Entities | 23+ |
| Total Relationships | 30+ |
| Primary Tables | 19 |
| Junction Tables | 4 |
| Profile Tables | 4 |

---

## 🔄 Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0 | 2026-02-02 | Initial ERD creation |

---

*GradLink System - Graduation Project Documentation*











