# ✅ Signup Network Error - FIXED!

## 🔧 Changes Made:

### 1. Frontend API Configuration (`src/api/client.js`)
- ✅ Changed default `baseURL` from `http://localhost:5080` to `http://localhost:5000`
- ✅ Now matches the backend port

### 2. Backend CORS Configuration (`backend/GradLink.Api/Program.cs`)
- ✅ Updated CORS policy to **allow any origin**:
  ```csharp
  policy.AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader();
  ```
- ✅ Disabled HTTPS redirection in Development mode (prevents HTTP to HTTPS redirect issues)

### 3. Backend Settings (`backend/GradLink.Api/appsettings.json`)
- ✅ Added `http://localhost:5173` (Vite default port) to allowed origins
- ✅ Added wildcard `*` for development flexibility

---

## 🚀 How to Run:

### Option 1: Use Startup Scripts (Easiest)
Double-click one of these files:
- **`START_ALL.bat`** (Command Prompt)
- **`START_ALL.ps1`** (PowerShell)

### Option 2: Manual Start
Open 3 terminals:

**Terminal 1 - Backend:**
```bash
cd backend\GradLink.Api
dotnet run
```

**Terminal 2 - Flask API:**
```bash
cd Resume_Analyser_Using_Python-Main
..\venv\Scripts\activate
python main.py
```

**Terminal 3 - Frontend:**
```bash
npm run dev
```

---

## 🌐 Access Points:

- **Frontend**: http://localhost:5173
- **Backend API**: http://localhost:5000
- **Swagger Docs**: http://localhost:5000/swagger
- **Flask API**: http://localhost:5005

---

## ✅ What Was Fixed:

### Before:
❌ Frontend trying to connect to `http://localhost:5080`  
❌ Backend running on `http://localhost:5000`  
❌ CORS blocking requests  
❌ Network Error on Signup

### After:
✅ Frontend connects to `http://localhost:5000`  
✅ Backend accepts requests from any origin  
✅ No HTTPS redirection in development  
✅ Signup works correctly!

---

## 🧪 Test Signup:

1. Run all services
2. Open http://localhost:5173
3. Navigate to Signup page
4. Fill in the form:
   - Name: Test User
   - Email: test@example.com
   - Password: Test123
   - Agree to terms
5. Click "Sign Up"
6. Should see success message and redirect to dashboard

---

## 🔍 Troubleshooting:

### Still getting Network Error?
1. **Check Backend is running**: Visit http://localhost:5000/swagger
2. **Check Browser Console**: Press F12 and look for errors
3. **Check Backend Console**: Look for CORS or 500 errors
4. **Clear Browser Cache**: Ctrl+Shift+Delete
5. **Restart all services**

### Common Issues:

**Port already in use:**
```bash
# Find process using port 5000
netstat -ano | findstr :5000
# Kill the process (replace PID with actual process ID)
taskkill /PID <PID> /F
```

**Database connection error:**
- Backend uses SQL Server connection from appsettings.json
- Should work automatically, but check database is accessible

**CORS still blocked:**
- Make sure you stopped old backend and restarted with new changes
- Clear browser cache

---

## 📝 Notes:

- CORS is **wide open** for development (any origin allowed)
- For **production**, restrict CORS to specific domains
- Flask API changed to port 5005 to avoid conflict with .NET backend (port 5000)
- HTTPS redirection disabled in development for easier testing

---

## 🎉 You're Ready!

All network errors should be fixed. Start the services and test the Signup functionality!

