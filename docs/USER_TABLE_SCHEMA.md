# GradLink - Complete User Table Schema

## Overview

This document describes the comprehensive User table design for the GradLink platform, supporting all user types: Students, Graduates, Companies, Sponsors, Mentors, and Admins.

---

## 🗂️ Database Structure

### Main Table: `AspNetUsers` (ApplicationUser)

The core user table inherits from ASP.NET Identity's `IdentityUser` and extends it with GradLink-specific fields.

#### Inherited from IdentityUser (ASP.NET Identity)
| Column | Type | Description |
|--------|------|-------------|
| `Id` | string (PK) | Unique user identifier (GUID) |
| `UserName` | string | Username for login |
| `Email` | string | Email address |
| `EmailConfirmed` | bool | Email verification status |
| `PasswordHash` | string | Hashed password (never plain text) |
| `PhoneNumber` | string | Phone number |
| `PhoneNumberConfirmed` | bool | Phone verification status |
| `TwoFactorEnabled` | bool | 2FA enabled |
| `LockoutEnd` | DateTime? | Lockout end time |
| `LockoutEnabled` | bool | Lockout enabled |
| `AccessFailedCount` | int | Failed login attempts |

---

### A) Core Authentication Data

| Column | Type | Description | Used By |
|--------|------|-------------|---------|
| `Role` | string | User role: Student, Graduate, Company, Sponsor, Mentor, Admin | All |
| `AccountStatus` | string | Active, Suspended, Deactivated, Pending | All |
| `IsVerified` | bool | Whether account is verified | All |
| `IsProfilePublic` | bool | Profile visibility setting | All |

---

### B) Personal Profile Data

| Column | Type | Description | Used By |
|--------|------|-------------|---------|
| `FullName` | string | Full name | All |
| `Bio` | string | Biography/description | All |
| `ProfilePicture` | string | Profile picture URL | All |
| `CoverImage` | string | Cover/banner image URL | All |
| `DateOfBirth` | DateTime? | Date of birth | Students, Graduates, Mentors |
| `Gender` | string | Male, Female, Other | Students, Graduates |
| `Nationality` | string | User's nationality | Students, Graduates |

---

### C) Education & Academic Data

| Column | Type | Description | Used By |
|--------|------|-------------|---------|
| `University` | string | University/Institution name | Students, Graduates |
| `Faculty` | string | Faculty/College name | Students, Graduates |
| `Department` | string | Department name | Students, Graduates |
| `Major` | string | Field of study | Students, Graduates |
| `AcademicYear` | string | Current academic year | Students |
| `GraduationYear` | int? | Expected/actual graduation year | Students, Graduates |
| `GPA` | decimal? | Grade Point Average | Students, Graduates |
| `StudentId` | string | Student ID number | Students |

---

### D) Professional Data

| Column | Type | Description | Used By |
|--------|------|-------------|---------|
| `JobTitle` | string | Current job title | Graduates, Mentors |
| `Company` | string | Company/Organization name | Graduates, Mentors |
| `Industry` | string | Industry/Field of work | All professionals |
| `ExperienceYears` | int? | Years of experience | Graduates, Mentors |
| `Skills` | string | Skills (comma-separated) | Students, Graduates, Mentors |
| `Specialization` | string | Area of expertise | Mentors, Graduates |
| `Certifications` | string | Certifications (JSON) | All |
| `Languages` | string | Languages spoken | All |

---

### E) Company/Sponsor Data

| Column | Type | Description | Used By |
|--------|------|-------------|---------|
| `CompanyName` | string | Company name | Companies, Sponsors |
| `CompanyWebsite` | string | Company website URL | Companies, Sponsors |
| `CompanyLogo` | string | Company logo URL | Companies, Sponsors |
| `CompanySize` | string | Small, Medium, Large, Enterprise | Companies, Sponsors |
| `CompanyDescription` | string | Company description | Companies, Sponsors |

---

### F) Contact & Social Links

| Column | Type | Description | Used By |
|--------|------|-------------|---------|
| `Location` | string | City/Location | All |
| `Address` | string | Full address | Companies, Sponsors |
| `Country` | string | Country | All |
| `WhatsApp` | string | WhatsApp number | All |
| `LinkedInUrl` | string | LinkedIn profile URL | All |
| `GitHubUrl` | string | GitHub profile URL | Students, Graduates, Mentors |
| `TwitterUrl` | string | Twitter/X profile URL | All |
| `FacebookUrl` | string | Facebook profile URL | All |
| `WebsiteUrl` | string | Personal website/Portfolio | Students, Graduates, Mentors |

---

### G) Activity & Statistics

| Column | Type | Description | Used By |
|--------|------|-------------|---------|
| `ProjectsCount` | int | Number of projects | Students, Graduates |
| `CompletedProjectsCount` | int | Completed projects count | Students, Graduates |
| `JobsPostedCount` | int | Number of jobs posted | Companies |
| `ApplicationsCount` | int | Job applications count | Students, Graduates |
| `ProfileViewsCount` | int | Profile views | All |
| `ProfileCompletionPercentage` | int | Profile completion % | All |

---

### H) Premium & Subscription

| Column | Type | Description | Used By |
|--------|------|-------------|---------|
| `IsPremium` | bool | Has premium subscription | All |
| `PlanType` | string | Free, Basic, Pro, Enterprise | All |
| `PremiumStartDate` | DateTime? | Subscription start date | All |
| `PremiumEndDate` | DateTime? | Subscription end date | All |

---

### I) System & Tracking

| Column | Type | Description | Used By |
|--------|------|-------------|---------|
| `CreatedAt` | DateTime | Account creation time | All |
| `UpdatedAt` | DateTime? | Last profile update | All |
| `LastLoginAt` | DateTime? | Last login time | All |
| `LastActivityAt` | DateTime? | Last activity time | All |
| `IsOnline` | bool | Currently online | All |
| `LastLoginIp` | string | IP of last login | All (security) |
| `LastLoginDevice` | string | Device of last login | All (security) |
| `Preferences` | string | User preferences (JSON) | All |
| `NotificationSettings` | string | Notification settings (JSON) | All |
| `DeactivationReason` | string | Reason for deactivation | All |

---

## 📋 Related Profile Tables

### 1. StudentProfile
Extended data for students including graduation project details, academic achievements, and career preferences.

| Column | Type | Description |
|--------|------|-------------|
| `Id` | int (PK) | Profile ID |
| `UserId` | string (FK) | Link to User |
| `ProjectTitle` | string | Graduation project title |
| `ProjectDescription` | string | Project description |
| `ProjectCategory` | string | AI, Web, Mobile, IoT, etc. |
| `ProjectStatus` | string | Idea, InProgress, Completed |
| `TeamName` | string | Team name |
| `SupervisorName` | string | Supervisor name |
| `CareerInterests` | string | Career interests |
| `IsLookingForMentor` | bool | Looking for mentor |
| `IsLookingForSponsor` | bool | Looking for funding |
| `FundingNeeded` | decimal | Funding amount needed |

### 2. CompanyProfile
Extended data for companies including hiring preferences and recruitment statistics.

| Column | Type | Description |
|--------|------|-------------|
| `Id` | int (PK) | Profile ID |
| `UserId` | string (FK) | Link to User |
| `RegistrationNumber` | string | Company registration |
| `FoundedYear` | int | Year founded |
| `EmployeeCount` | string | Number of employees |
| `CompanyType` | string | Startup, SME, Enterprise |
| `IsHiring` | bool | Currently hiring |
| `OpenPositionsCount` | int | Open positions |
| `TotalHires` | int | Total hires made |
| `Rating` | decimal | Company rating |
| `IsVerified` | bool | Verified company |

### 3. MentorProfile
Extended data for mentors including mentoring preferences and session data.

| Column | Type | Description |
|--------|------|-------------|
| `Id` | int (PK) | Profile ID |
| `UserId` | string (FK) | Link to User |
| `ExpertiseAreas` | string | Areas of expertise |
| `MentoringStyle` | string | Hands-on, Advisory, etc. |
| `MaxMentees` | int | Maximum mentees |
| `IsAcceptingMentees` | bool | Accepting new mentees |
| `SessionDurationMinutes` | int | Session duration |
| `HourlyRate` | decimal | Rate (if paid) |
| `TotalSessions` | int | Sessions conducted |
| `AverageRating` | decimal | Mentor rating |

### 4. SponsorProfile
Extended data for sponsors (already exists in the system).

---

## 🔐 Security Best Practices

1. **Password Storage**: Only hashed passwords are stored (using ASP.NET Identity)
2. **Sensitive Data**: IP addresses and device info for security tracking
3. **Verification**: Email and phone verification supported
4. **2FA**: Two-factor authentication available
5. **Lockout**: Account lockout after failed attempts

---

## 📊 Indexes for Performance

- `Role` - Quick filtering by user type
- `AccountStatus` - Filter active/inactive users
- `University` - Search by university
- `CompanyName` - Search by company
- `CreatedAt` - Sort by registration date
- `UserId` (in profile tables) - Unique constraint

---

## 🔄 How to Update Database

Run the following command to apply the migration:

```bash
cd backend/GradLink.Api
dotnet ef migrations add UpdateUserTableComplete --project ../GradLink.Infrastructure
dotnet ef database update
```

Or simply run:
```
UPDATE_USER_TABLE.bat
```

---

## 📝 Notes

1. The `Role` field is used for quick identification, but ASP.NET Identity Roles are also available
2. Profile tables (StudentProfile, CompanyProfile, MentorProfile) have a 1:1 relationship with User
3. All foreign keys use `UserId` consistently for clarity
4. JSON fields are used for complex data that doesn't need to be queried directly






