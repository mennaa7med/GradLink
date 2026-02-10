# Frontend-Backend Connection Guide

Complete step-by-step guide to connect your React frontend with the .NET 8 backend.

## Overview

Your application has:
- **Frontend (React + Vite)**: Running on `http://localhost:5176`
- **Backend (.NET 8 API)**: Running on `http://localhost:5000` (HTTP) and `https://localhost:5001` (HTTPS)

## 📋 Prerequisites

Before connecting, ensure:

✅ **Backend is configured and running**
- SQL Server is running
- Backend builds successfully: `cd backend && dotnet build`
- Backend can start: `cd backend/GradLink.Api && dotnet run`

✅ **Frontend dependencies are installed**
- Node modules installed: `npm install`

## 🔧 Step-by-Step Connection Setup

### Step 1: Configure Frontend Environment Variables

**1.1: Environment files have been created**

Three new files have been added to your project root:

- `.env.development` - Development configuration ✅
- `.env.production` - Production configuration ✅
- `.env.example` - Template for reference ✅

**1.2: Verify `.env.development` content**

File location: `Newfolder/.env.development`

```env
# Backend API Base URL
VITE_API_BASE_URL=http://localhost:5000

# SignalR Hub URL (for real-time chat)
VITE_SIGNALR_HUB_URL=http://localhost:5000/hubs/chat
```

**Note:** Using HTTP (5000) is easier for development. If you want HTTPS (5001), uncomment those lines.

### Step 2: Verify Backend CORS Configuration

**2.1: Check backend allows your frontend origin**

Open: `backend/GradLink.Api/appsettings.json`

Verify this section includes your frontend URL:

```json
{
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:5176",
      "http://localhost:3000"
    ]
  }
}
```

Your frontend port (`5176`) is already configured! ✅

**2.2: If you need to add more origins**

Edit `backend/GradLink.Api/appsettings.json` and add to the `AllowedOrigins` array:

```json
"Cors": {
  "AllowedOrigins": [
    "http://localhost:5176",
    "http://localhost:3000",
    "http://localhost:5173",  // Add any other ports
    "https://localhost:5176"  // HTTPS version if needed
  ]
}
```

### Step 3: Update Frontend API Client (Already Done ✅)

The API client (`src/api/client.js`) is already configured to use environment variables:

```javascript
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5080';
```

It will now use: `http://localhost:5000` from `.env.development`

### Step 4: Update SignalR Configuration (Already Done ✅)

The SignalR connection (`src/api/signalr.js`) has been updated to use environment variables:

```javascript
const SIGNALR_HUB_URL = import.meta.env.VITE_SIGNALR_HUB_URL || 'http://localhost:5000/hubs/chat';
```

## 🚀 Testing the Connection

### Test 1: Start Backend

**Terminal 1:**
```bash
cd backend/GradLink.Api
dotnet run
```

**Expected output:**
```
info: Now listening on: https://localhost:5001
info: Now listening on: http://localhost:5000
info: Application started.
```

**Verify Swagger UI works:**
Open browser: `http://localhost:5000/swagger`

You should see the API documentation! ✅

### Test 2: Start Frontend

**Terminal 2:**
```bash
npm run dev
```

**Expected output:**
```
  VITE v5.x.x  ready in xxx ms

  ➜  Local:   http://localhost:5176/
  ➜  Network: use --host to expose
```

### Test 3: Verify Connection

**3.1: Open Frontend**
Navigate to: `http://localhost:5176/`

**3.2: Test Authentication**

1. **Register a new user:**
   - Go to Signup page
   - Fill in the form:
     ```
     Email: test@example.com
     Password: Test@123
     Full Name: Test User
     Phone: +1234567890
     ```
   - Click Register

2. **Check browser console:**
   - Press `F12` → Console tab
   - You should see API requests to `http://localhost:5000/api/auth/register`
   - Response should be `200 OK` with tokens

3. **Login:**
   - Use the same credentials
   - If successful, you'll be redirected to dashboard

**3.3: Test API Calls**

Open browser DevTools (F12) → Network tab

Try different features and verify API calls:

| Feature | Endpoint Called | Expected Status |
|---------|----------------|-----------------|
| Register | `POST /api/auth/register` | 200 OK |
| Login | `POST /api/auth/login` | 200 OK |
| Get Profile | `GET /api/users/me` | 200 OK |
| Upload Resume | `POST /api/resumes/upload` | 201 Created |
| Get Projects | `GET /api/projects` | 200 OK |

### Test 4: Verify SignalR Connection (Chat)

1. Navigate to Chat page
2. Open browser console (F12)
3. Look for: `SignalR connection established` or similar message
4. Send a message
5. Verify WebSocket connection in Network tab (WS filter)

## 🐛 Troubleshooting

### Issue 1: CORS Error

**Error in browser console:**
```
Access to XMLHttpRequest at 'http://localhost:5000/api/...' from origin 'http://localhost:5176' has been blocked by CORS policy
```

**Solution:**

1. Check `backend/GradLink.Api/appsettings.json` includes your frontend URL:
   ```json
   "Cors": {
     "AllowedOrigins": ["http://localhost:5176"]
   }
   ```

2. Restart the backend:
   ```bash
   cd backend/GradLink.Api
   dotnet run
   ```

### Issue 2: Connection Refused / Network Error

**Error:**
```
Error: connect ECONNREFUSED 127.0.0.1:5000
```

**Solutions:**

1. **Verify backend is running:**
   ```bash
   cd backend/GradLink.Api
   dotnet run
   ```

2. **Check the port:**
   - Backend should show: "Now listening on: http://localhost:5000"
   - If different port, update `.env.development`

3. **Check firewall:**
   - Temporarily disable firewall to test
   - Or allow dotnet.exe through firewall

### Issue 3: 404 Not Found

**Error:**
```
GET http://localhost:5000/api/auth/login 404 (Not Found)
```

**Solution:**

Verify backend is running and routes are registered:
- Open `http://localhost:5000/swagger`
- You should see all API endpoints listed

### Issue 4: 401 Unauthorized

**Error:**
```
GET http://localhost:5000/api/users/me 401 (Unauthorized)
```

**This is actually normal!** It means:
- ✅ Connection works
- ❌ You're not logged in OR token expired

**Solution:**
1. Login first
2. Check localStorage for `accessToken`
3. Verify token is being sent in request headers

### Issue 5: SignalR Connection Failed

**Error:**
```
Error: Failed to start the connection
```

**Solutions:**

1. **Verify Hub URL in `.env.development`:**
   ```env
   VITE_SIGNALR_HUB_URL=http://localhost:5000/hubs/chat
   ```

2. **Check you're logged in:**
   SignalR requires authentication token

3. **Restart frontend:**
   ```bash
   # Stop frontend (Ctrl+C)
   npm run dev
   ```

### Issue 6: Environment Variables Not Loading

**Symptoms:**
- Frontend still trying to connect to `http://localhost:5080`
- Changes in `.env.development` not taking effect

**Solutions:**

1. **Restart Vite dev server:**
   ```bash
   # Stop frontend (Ctrl+C in terminal)
   npm run dev
   ```

2. **Clear browser cache:**
   - Press `Ctrl + Shift + R` (Hard reload)
   - Or clear cache in DevTools

3. **Verify file name:**
   - Must be exactly `.env.development` (not `.env.dev` or `env.development`)
   - Check for spaces or typos

### Issue 7: Mixed Content (HTTPS/HTTP)

**Error:**
```
Mixed Content: The page at 'https://...' was loaded over HTTPS, but requested an insecure resource
```

**Solution:**

Both frontend and backend must use same protocol:

**Option A: Both HTTP (Easier for development)**
- Frontend: `http://localhost:5176`
- Backend: `http://localhost:5000`
- `.env.development`: `VITE_API_BASE_URL=http://localhost:5000`

**Option B: Both HTTPS**
- Frontend: `https://localhost:5176`
- Backend: `https://localhost:5001`
- `.env.development`: `VITE_API_BASE_URL=https://localhost:5001`

## 🔍 Verification Checklist

Use this checklist to verify everything is connected:

### Backend Verification

- [ ] SQL Server is running
- [ ] Backend builds without errors: `dotnet build`
- [ ] Backend starts successfully: `dotnet run`
- [ ] Swagger UI accessible at `http://localhost:5000/swagger`
- [ ] Can login via Swagger with admin credentials:
  - Email: `admin@gradlink.com`
  - Password: `Admin@123`

### Frontend Verification

- [ ] `.env.development` file exists and has correct values
- [ ] Frontend starts: `npm run dev`
- [ ] Frontend accessible at `http://localhost:5176`
- [ ] No CORS errors in browser console (F12)
- [ ] Can register a new user
- [ ] Can login successfully
- [ ] Dashboard loads after login
- [ ] Network tab shows API calls to `http://localhost:5000`

### Integration Verification

- [ ] Login works and redirects to dashboard
- [ ] User profile displays correctly
- [ ] Can upload a resume
- [ ] Can create a project
- [ ] Real-time chat connects (if testing chat)
- [ ] Logout works

## 📝 Development Workflow

### Daily Development Routine

**1. Start Backend (Terminal 1):**
```bash
cd backend/GradLink.Api
dotnet watch run  # Auto-reloads on code changes
```

**2. Start Frontend (Terminal 2):**
```bash
npm run dev  # Auto-reloads on code changes
```

**3. Open Browser:**
- Frontend: `http://localhost:5176`
- Backend Swagger: `http://localhost:5000/swagger`
- DevTools (F12) → Network tab to monitor API calls

### Making Changes

**Backend Changes:**
- Edit C# files
- `dotnet watch run` will auto-reload
- Check logs in terminal for errors

**Frontend Changes:**
- Edit React/JS files
- Vite will hot-reload automatically
- Check browser console for errors

**Database Changes:**
- Modify entities in `backend/GradLink.Domain/Entities/`
- Create migration:
  ```bash
  cd backend/GradLink.Api
  dotnet ef migrations add YourMigrationName --project ../GradLink.Infrastructure
  dotnet ef database update
  ```

## 🔐 Security Notes

### Development
- ✅ HTTP is fine for local development
- ✅ CORS allows localhost
- ✅ Default passwords are okay locally

### Production
- ⚠️ Use HTTPS only
- ⚠️ Restrict CORS to your domain only
- ⚠️ Change all default passwords
- ⚠️ Use environment variables for secrets
- ⚠️ Enable rate limiting
- ⚠️ Use proper SSL certificates

## 📊 API Endpoints Reference

### Authentication
```
POST   /api/auth/register     - Register new user
POST   /api/auth/login        - Login
POST   /api/auth/refresh      - Refresh token
POST   /api/auth/logout       - Logout
```

### Users
```
GET    /api/users/me          - Get current user
PUT    /api/users/me          - Update profile
POST   /api/users/me/avatar   - Upload avatar
```

### Projects
```
GET    /api/projects          - Get user projects
POST   /api/projects          - Create project
PUT    /api/projects/{id}     - Update project
DELETE /api/projects/{id}     - Delete project
```

### Resumes
```
GET    /api/resumes           - Get user resumes
POST   /api/resumes/upload    - Upload resume
DELETE /api/resumes/{id}      - Delete resume
```

### Jobs
```
GET    /api/jobs              - Get all jobs
GET    /api/jobs/my           - Get my job postings
POST   /api/jobs              - Create job posting
```

### SignalR Hub
```
WS     /hubs/chat             - Real-time chat connection
```

See Swagger UI for complete API documentation: `http://localhost:5000/swagger`

## 🎯 Quick Test Script

Run this in your browser console to test the connection:

```javascript
// Test API connection
fetch('http://localhost:5000/api/auth/login', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    email: 'admin@gradlink.com',
    password: 'Admin@123'
  })
})
  .then(res => res.json())
  .then(data => console.log('✅ Connected!', data))
  .catch(err => console.error('❌ Connection failed:', err));
```

If you see `✅ Connected!` with token data, your connection works! 🎉

## 📞 Need Help?

If you encounter issues:

1. **Check both terminals** - Look for error messages in backend and frontend
2. **Check browser console** (F12) - Look for network/console errors
3. **Check Swagger UI** - Verify backend is working: `http://localhost:5000/swagger`
4. **Restart everything:**
   ```bash
   # Stop both terminals (Ctrl+C)
   # Start backend
   cd backend/GradLink.Api && dotnet run
   # Start frontend (new terminal)
   npm run dev
   ```

## ✅ Summary

Your frontend and backend are now connected! 🎉

**What we configured:**
1. ✅ Created `.env.development` with backend URL
2. ✅ Updated SignalR to use environment variables
3. ✅ Verified CORS configuration in backend
4. ✅ Backend runs on: `http://localhost:5000`
5. ✅ Frontend runs on: `http://localhost:5176`
6. ✅ They can now communicate via HTTP API calls

**Next steps:**
1. Start both backend and frontend
2. Test registration and login
3. Start building features! 🚀

---

**Connection Status:** ✅ Ready to use!

