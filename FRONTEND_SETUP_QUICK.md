# Frontend-Backend Connection - Quick Setup

## ⚡ Quick Start (5 Minutes)

### Step 1: Create Environment File

Create a new file in the project root: `.env.development`

```bash
# Windows PowerShell
New-Item -Path ".env.development" -ItemType File

# Or just create it manually in your editor
```

**Add this content to `.env.development`:**

```env
# Backend API Base URL
VITE_API_BASE_URL=http://localhost:5000

# SignalR Hub URL (for real-time chat)
VITE_SIGNALR_HUB_URL=http://localhost:5000/hubs/chat
```

### Step 2: Update SignalR Connection

Open `src/api/signalr.js` and change:

**FROM:**
```javascript
const connection = new signalR.HubConnectionBuilder()
  .withUrl(`${baseUrl}/hubs/chat`, {
```

**TO:**
```javascript
const SIGNALR_HUB = import.meta.env.VITE_SIGNALR_HUB_URL || `${baseUrl}/hubs/chat`;

const connection = new signalR.HubConnectionBuilder()
  .withUrl(SIGNALR_HUB, {
```

### Step 3: Verify Backend CORS

Open `backend/GradLink.Api/appsettings.json` and make sure it includes:

```json
"Cors": {
  "AllowedOrigins": [
    "http://localhost:5176",
    "http://localhost:3000"
  ]
}
```

### Step 4: Start Everything

**Terminal 1 - Backend:**
```bash
cd backend/GradLink.Api
dotnet run
```

**Terminal 2 - Frontend:**
```bash
npm run dev
```

### Step 5: Test

1. Open browser: `http://localhost:5176`
2. Try to register or login
3. Check browser console (F12) - should see API calls to `localhost:5000`

## ✅ That's it!

If you see API calls in the Network tab going to `http://localhost:5000`, you're connected! 🎉

For detailed troubleshooting, see `FRONTEND_BACKEND_CONNECTION.md`

---

**Quick Test:** Login with admin account
- Email: `admin@gradlink.com`
- Password: `Admin@123`

