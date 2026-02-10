# 🚀 GradLink - Quick Start Guide

## Option 1: Docker Compose (Recommended - Easiest)

### Prerequisites
- Docker Desktop installed and running
- Ports 8080 and 8081 available

### Steps

1. **Open terminal in project root** (`D:\كل المهم\viteProject\Newfolder`)

2. **Set environment variables** (PowerShell):
   ```powershell
   $env:JWT_SECRET="YourSuperSecretKeyAtLeast32CharactersLong!"
   $env:ADMIN_EMAIL="admin@example.com"
   $env:ADMIN_PASSWORD="P@ssw0rd!"
   $env:VITE_API_BASE_URL="http://localhost:8080"
   # Optional - for Gemini chat:
   $env:GEMINI_API_KEY="your_google_api_key_here"
   ```

3. **Build and run everything:**
   ```bash
   docker compose up --build
   ```

4. **Access the application:**
   - 🌐 **Web App**: http://localhost:8081
   - 📚 **API Swagger**: http://localhost:8080/swagger
   - 🔐 **Login**: Use `admin@example.com` / `P@ssw0rd!` (or your custom credentials)

5. **Stop everything:**
   ```bash
   docker compose down
   ```

---

## Option 2: Local Development (Windows)

### Prerequisites
- .NET 8 SDK installed
- Node.js 18+ installed
- SQLite (comes with .NET) or SQL Server

### Steps

1. **Install frontend dependencies:**
   ```bash
   npm install
   ```
   
   If you get an error about `@microsoft/signalr`, install it:
   ```bash
   npm install @microsoft/signalr
   ```

2. **Set environment variables** (PowerShell):
   ```powershell
   $env:JWT_SECRET="YourSuperSecretKeyAtLeast32CharactersLong!"
   $env:Admin__Email="admin@example.com"
   $env:Admin__Password="P@ssw0rd!"
   $env:VITE_API_BASE_URL="http://localhost:5080"
   $env:Gemini__ApiKey="your_google_api_key_here"  # Optional
   ```

3. **Run the startup script:**
   ```powershell
   .\run-dev.ps1
   ```

   Or manually:
   - **Terminal 1 - Backend:**
     ```bash
     cd backend/Api
     dotnet run
     ```
   - **Terminal 2 - Frontend:**
     ```bash
     npm run dev
     ```

4. **Access the application:**
   - 🌐 **Web App**: http://localhost:5173
   - 📚 **API Swagger**: http://localhost:5080/swagger (or check terminal output for actual port)

---

## What Happens Automatically

✅ **Database migrations** are applied on API startup  
✅ **Admin user** is seeded automatically (if env vars are set)  
✅ **CORS** is configured for frontend  
✅ **JWT authentication** is ready  
✅ **All features** are connected end-to-end

---

## Features Available

- ✅ **Authentication**: Register, Login, JWT tokens
- ✅ **Dashboard**: Stats, Projects overview
- ✅ **Projects**: Full CRUD operations
- ✅ **Tasks**: Nested tasks under projects
- ✅ **Resume Analysis**: Upload and analyze resumes
- ✅ **Chat**: SignalR real-time chat
- ✅ **Gemini AI**: Server-side proxy (if API key provided)
- ✅ **Admin Stats**: Dashboard statistics

---

## Troubleshooting

### Port already in use
- Change ports in `docker-compose.yml` or stop conflicting services

### CORS errors
- Ensure `VITE_API_BASE_URL` matches your API URL
- Check `Cors__AllowedOrigins` in backend config

### Database errors
- SQLite database is created automatically in `backend/Api/Data/`
- For SQL Server, set `Database__UseSqlite=false` and provide connection string

### Frontend can't connect to API
- Check `VITE_API_BASE_URL` environment variable
- Ensure API is running and accessible
- Check browser console for errors

---

## Default Admin Credentials

- **Email**: `admin@example.com`
- **Password**: `P@ssw0rd!`

(Or use your custom values from environment variables)

---

## Next Steps

1. Register a new user account
2. Create a project
3. Add tasks to your project
4. Upload a resume and analyze it
5. Try the chat feature
6. Explore the dashboard stats

**Enjoy building with GradLink! 🎉**

