# 🔧 Fix: localhost:5000 returns 404 Error

## ❌ Problem:
```
This localhost page can't be found
No webpage was found for the web address: http://localhost:5000/
HTTP ERROR 404
```

**This means the Backend API is NOT running!**

---

## ✅ Solution - Step by Step:

### Step 1: Check if Backend can build
**Run this file:**
```
CHECK_BACKEND.bat
```

**Double-click it and wait.**

It will:
1. ✅ Check .NET SDK is installed
2. ✅ Check project files exist
3. ✅ Restore NuGet packages
4. ✅ Build the project

**If it shows errors, read them carefully!**

Common errors:
- **Package restore failed**: Check internet connection
- **Build failed**: Check error messages for missing files
- **.NET SDK not found**: Install .NET 8 SDK from https://dotnet.microsoft.com/download

---

### Step 2: Start Backend ONLY
**Run this file:**
```
START_BACKEND_ONLY.bat
```

**This will:**
- Start ONLY the backend
- Show all errors clearly
- Keep the window open so you can read errors

**Wait until you see:**
```
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```

**Then test:** Open http://localhost:5000/swagger in your browser

✅ **Should see Swagger API documentation page**

---

### Step 3: Start Frontend
**In another window, run:**
```
START_FRONTEND_ONLY.bat
```

**Wait until you see:**
```
Local:   http://localhost:5173/
```

**Then open:** http://localhost:5173

---

## 🔍 Common Issues & Solutions:

### Issue 1: "dotnet: command not found"
**Problem:** .NET SDK not installed

**Solution:**
1. Download .NET 8 SDK from: https://dotnet.microsoft.com/download
2. Install it
3. Restart your terminal
4. Run `CHECK_BACKEND.bat` again

---

### Issue 2: "Port 5000 is already in use"
**Problem:** Another program is using port 5000

**Solution (Windows):**
```bash
# Open CMD as Administrator
netstat -ano | findstr :5000
# Note the PID (last column)
taskkill /PID <number> /F
```

**Or use different port:**
Edit `START_BACKEND_ONLY.bat`:
```batch
dotnet run --urls http://localhost:5001
```

Then update `src/api/client.js`:
```javascript
const API_BASE_URL = 'http://localhost:5001';
```

---

### Issue 3: "Cannot connect to database"
**Problem:** Database connection failed

**Solution:**
The project uses SQL Server. Check `backend/GradLink.Api/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=..."
}
```

**For development, use SQLite instead:**

Edit `backend/GradLink.Api/appsettings.json`:
```json
"DatabaseProvider": "Sqlite",
"ConnectionStrings": {
  "DefaultConnection": "Data Source=gradlink.db"
}
```

---

### Issue 4: Build errors about missing packages
**Problem:** NuGet packages not restored

**Solution:**
```bash
cd backend\GradLink.Api
dotnet restore
dotnet build
```

---

### Issue 5: "Failed to bind to address"
**Problem:** Permission issue or port blocked

**Solution:**
1. **Run as Administrator:**
   - Right-click `START_BACKEND_ONLY.bat`
   - Select "Run as Administrator"

2. **Or disable firewall temporarily** (for testing)

---

## 🧪 Test Backend is Working:

### Test 1: Swagger UI
Open: http://localhost:5000/swagger

✅ Should see API documentation

---

### Test 2: Health Check
Open: http://localhost:5000/api/auth/login

✅ Should see an error (because no credentials) but NOT 404

---

### Test 3: From terminal
```bash
curl http://localhost:5000/swagger
```

✅ Should return HTML

---

## 📋 Correct Startup Order:

### Option A: All at once
```
START_ALL.bat
```

**Wait for:**
- Backend window shows: "Now listening on :5000"
- Frontend window shows: "Local: http://localhost:5173"

### Option B: One by one (Better for debugging)
1. **First:** `CHECK_BACKEND.bat` (check everything is OK)
2. **Second:** `START_BACKEND_ONLY.bat` (wait for "Now listening")
3. **Third:** `START_FRONTEND_ONLY.bat` (start frontend)
4. **Fourth:** Open browser → http://localhost:5173

---

## 🔥 Quick Fix - Try This:

1. **Close all terminal windows**

2. **Open CMD as Administrator:**
   - Press Windows key
   - Type "cmd"
   - Right-click "Command Prompt"
   - Select "Run as Administrator"

3. **Navigate to project:**
```bash
cd /d "D:\كل المهم\viteProject\Newfolder"
```

4. **Check backend:**
```bash
CHECK_BACKEND.bat
```

5. **Start backend:**
```bash
START_BACKEND_ONLY.bat
```

6. **In another CMD window (also as Admin):**
```bash
cd /d "D:\كل المهم\viteProject\Newfolder"
START_FRONTEND_ONLY.bat
```

---

## 📊 Visual Checklist:

```
┌─────────────────────────────────────────┐
│ ✅ .NET SDK installed (dotnet --version)│
├─────────────────────────────────────────┤
│ ✅ Backend builds (CHECK_BACKEND.bat)   │
├─────────────────────────────────────────┤
│ ✅ Backend starts (START_BACKEND_ONLY)  │
├─────────────────────────────────────────┤
│ ✅ Can access http://localhost:5000     │
├─────────────────────────────────────────┤
│ ✅ Swagger works at :5000/swagger       │
├─────────────────────────────────────────┤
│ ✅ Frontend starts (npm run dev)        │
├─────────────────────────────────────────┤
│ ✅ Can access http://localhost:5173     │
├─────────────────────────────────────────┤
│ 🎉 Everything working!                  │
└─────────────────────────────────────────┘
```

---

## 🆘 Still Not Working?

### Check Backend Console Window for these messages:

**Good messages (✅):**
```
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```

**Bad messages (❌):**
```
Failed to bind to address
Unable to start Kestrel
Database connection failed
Port already in use
```

**If you see errors:**
1. Read the full error message
2. Copy it
3. Search for solution
4. Or try the solutions above

---

## 📞 Debug Commands:

```bash
# Check .NET version
dotnet --version

# Check if port 5000 is in use
netstat -ano | findstr :5000

# Check project can build
cd backend\GradLink.Api
dotnet build

# Try running with verbose output
dotnet run --urls http://localhost:5000 --verbosity detailed
```

---

## ✅ Expected Result:

### Backend Console:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

### Browser (http://localhost:5000/swagger):
- Should see "GradLink API v1" page
- List of API endpoints
- Green/blue UI from Swagger

### Frontend (http://localhost:5173):
- Should see GradLink homepage
- Can navigate to Sign Up
- No network errors

---

## 🎯 Summary:

1. **Run:** `CHECK_BACKEND.bat` - Fix any errors
2. **Run:** `START_BACKEND_ONLY.bat` - Wait for "Now listening"
3. **Test:** http://localhost:5000/swagger - Should work
4. **Run:** `START_FRONTEND_ONLY.bat` - Start frontend
5. **Test:** http://localhost:5173 - Should work
6. **Try Signup** - Should work without network errors

---

## 🎉 Once Working:

You can go back to using `START_ALL.bat` which starts everything automatically!

**Happy Coding! 💻**

