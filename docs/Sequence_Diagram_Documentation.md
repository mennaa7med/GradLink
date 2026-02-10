# GradLink System - Sequence Diagram Documentation

## 📋 Overview

This document describes the UML Sequence Diagram for the **Login Process** in the GradLink System.

---

## 🎭 Participants

| Participant | Stereotype | Technology | Description |
|-------------|------------|------------|-------------|
| **Student** | Actor | - | User attempting to log in |
| **Frontend** | «boundary» | React.js | Client-side web application |
| **Backend API** | «control» | ASP.NET Core | Server-side REST API |
| **Database** | «entity» | SQL Server | Data persistence layer |

---

## 📊 Login Sequence Flow

### Successful Login (Steps 1-7)

```
Student          Frontend           Backend API          Database
   │                 │                   │                   │
   │ 1. Enter email  │                   │                   │
   │    & password   │                   │                   │
   │────────────────>│                   │                   │
   │                 │                   │                   │
   │                 │ 2. POST /api/auth/login              │
   │                 │   {email, password}                  │
   │                 │──────────────────>│                   │
   │                 │                   │                   │
   │                 │                   │ 3. SELECT user    │
   │                 │                   │    WHERE email=?  │
   │                 │                   │──────────────────>│
   │                 │                   │                   │
   │                 │                   │ 4. User data      │
   │                 │                   │<- - - - - - - - - │
   │                 │                   │                   │
   │                 │                   │ [Verify Hash]     │
   │                 │                   │ [Generate JWT]    │
   │                 │                   │                   │
   │                 │                   │ 5. UPDATE         │
   │                 │                   │    lastLoginAt    │
   │                 │                   │──────────────────>│
   │                 │                   │                   │
   │                 │ 6. 200 OK         │                   │
   │                 │    {token, user}  │                   │
   │                 │<- - - - - - - - - │                   │
   │                 │                   │                   │
   │                 │ [Store Token]     │                   │
   │                 │                   │                   │
   │ 7. Redirect to  │                   │                   │
   │    Dashboard    │                   │                   │
   │<- - - - - - - - │                   │                   │
   │                 │                   │                   │
```

### Failed Login (Steps 8-9)

```
Student          Frontend           Backend API          Database
   │                 │                   │                   │
   │                 │ 8. 401            │                   │
   │                 │    Unauthorized   │                   │
   │                 │<- - - - - - - - - │                   │
   │                 │                   │                   │
   │ 9. Show error   │                   │                   │
   │    message      │                   │                   │
   │<- - - - - - - - │                   │                   │
   │                 │                   │                   │
```

---

## 📝 Detailed Step Descriptions

### Step 1: Enter Credentials
| Field | Description |
|-------|-------------|
| **From** | Student |
| **To** | Frontend |
| **Action** | User enters email and password in login form |
| **Data** | email, password (plain text in form) |

### Step 2: Login Request
| Field | Description |
|-------|-------------|
| **From** | Frontend |
| **To** | Backend API |
| **Method** | POST |
| **Endpoint** | `/api/auth/login` |
| **Request Body** | `{ "email": "...", "password": "..." }` |
| **Content-Type** | application/json |

### Step 3: Query User
| Field | Description |
|-------|-------------|
| **From** | Backend API |
| **To** | Database |
| **Action** | Search for user by email |
| **Query** | `SELECT * FROM Users WHERE Email = @email` |

### Step 4: Return User Data
| Field | Description |
|-------|-------------|
| **From** | Database |
| **To** | Backend API |
| **Response** | User record (or null if not found) |
| **Data** | Id, Email, PasswordHash, FullName, Role, etc. |

### Step 5: Update Last Login
| Field | Description |
|-------|-------------|
| **From** | Backend API |
| **To** | Database |
| **Action** | Update user's last login timestamp |
| **Query** | `UPDATE Users SET LastLoginAt = @now WHERE Id = @id` |

### Step 6: Success Response
| Field | Description |
|-------|-------------|
| **From** | Backend API |
| **To** | Frontend |
| **Status** | 200 OK |
| **Response Body** | `{ "token": "JWT...", "user": {...}, "expiresIn": 3600 }` |

### Step 7: Redirect to Dashboard
| Field | Description |
|-------|-------------|
| **From** | Frontend |
| **To** | Student |
| **Action** | Store token in localStorage, redirect to dashboard |

### Step 8: Unauthorized Response
| Field | Description |
|-------|-------------|
| **From** | Backend API |
| **To** | Frontend |
| **Status** | 401 Unauthorized |
| **Response** | `{ "error": "Invalid email or password" }` |

### Step 9: Show Error
| Field | Description |
|-------|-------------|
| **From** | Frontend |
| **To** | Student |
| **Action** | Display error message on login form |

---

## 🔐 Security Considerations

| Aspect | Implementation |
|--------|----------------|
| **Password Storage** | Bcrypt/PBKDF2 hashing |
| **Token Type** | JWT (JSON Web Token) |
| **Token Expiration** | 1 hour (configurable) |
| **HTTPS** | All communications encrypted |
| **Rate Limiting** | Max 5 attempts per minute |
| **Account Lockout** | After 5 failed attempts |

---

## 📐 UML Notation Reference

### Message Types

| Symbol | Name | Description |
|--------|------|-------------|
| `────────>` | Synchronous | Sender waits for response |
| `- - - - ->` | Return | Response to synchronous call |
| `─ ─ ─ ─ >` | Asynchronous | Sender doesn't wait |

### Fragment Types

| Type | Usage |
|------|-------|
| **alt** | Alternative paths (if-else) |
| **opt** | Optional (if without else) |
| **loop** | Iteration |
| **par** | Parallel execution |

---

## 📁 File Locations

| File | Path |
|------|------|
| Sequence Diagram HTML | `docs/Sequence_Diagram_Login.html` |
| Sequence Documentation | `docs/Sequence_Diagram_Documentation.md` |

---

## 🔄 Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0 | 2026-02-02 | Initial diagram creation |

---

*GradLink System - Graduation Project Documentation*











