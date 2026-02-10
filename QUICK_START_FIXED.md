# 🚀 GradLink - Quick Start Guide (Updated)

## ✅ Network Error FIXED!

All CORS and network connectivity issues have been resolved.

---

## 📦 Prerequisites

Make sure you have installed:
- [x] .NET 8 SDK
- [x] Node.js & npm
- [x] Python 3.x
- [x] SQL Server (or using the cloud database)

---

## ⚡ Quick Start (3 Commands)

### Option 1: Automated Start (Windows)
```bash
# Just double-click:
START_ALL.bat
# OR right-click and "Run with PowerShell":
START_ALL.ps1
```

### Option 2: Manual Start (3 Terminals)

**Terminal 1 - Backend (.NET):**
```bash
cd backend\GradLink.Api
dotnet run
```

**Terminal 2 - Flask (Resume Analyzer):**
```bash
cd Resume_Analyser_Using_Python-Main
..\venv\Scripts\activate
python main.py
```

**Terminal 3 - Frontend (React):**
```bash
npm run dev
```

---

## 🌐 URLs

| Service | URL | Description |
|---------|-----|-------------|
| Frontend | http://localhost:5173 | React App |
| Backend API | http://localhost:5000 | .NET Core API |
| Swagger Docs | http://localhost:5000/swagger | API Documentation |
| Flask API | http://localhost:5005 | Resume Analyzer |

---

## 🧪 Test It

1. Open: http://localhost:5173
2. Click "Sign Up"
3. Fill the form
4. Submit
5. ✅ Should work without network errors!

---

## 🔧 What Was Fixed

### Frontend:
- ✅ axios baseURL changed to `http://localhost:5000`
- ✅ Matches backend port

### Backend:
- ✅ CORS allows any origin
- ✅ HTTPS redirect disabled in development
- ✅ All ports properly configured

---

## 📚 More Info

- **Full Fix Details**: See `NETWORK_ERROR_FIXED.md`
- **Test Guide**: See `TEST_SIGNUP.md`
- **Original Setup**: See `QUICKSTART.md`

---

## 🆘 Quick Troubleshooting

**Network Error?**
```bash
# Check if backend is running:
curl http://localhost:5000/swagger
```

**Port in use?**
```bash
# Find and kill process:
netstat -ano | findstr :5000
taskkill /PID <PID> /F
```

**Still not working?**
1. Restart all services
2. Clear browser cache (Ctrl+Shift+Delete)
3. Check backend console for errors
4. Check browser console (F12) for errors

---

## 🎉 You're Ready!

Everything should work now. Happy coding! 🚀

