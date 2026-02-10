# 🧪 Test Signup - Quick Guide

## Steps to Test:

### 1. Start All Services
Run `START_ALL.bat` or `START_ALL.ps1`

Wait for all 3 windows to open:
- ✅ Backend API (Port 5000)
- ✅ Flask API (Port 5005)
- ✅ Frontend (Port 5173)

---

### 2. Verify Backend is Running
Open browser and go to: **http://localhost:5000/swagger**

You should see the Swagger API documentation page.

---

### 3. Open Frontend
Go to: **http://localhost:5173**

---

### 4. Navigate to Signup
Click on "Sign Up" or go to: **http://localhost:5173/signup**

---

### 5. Fill the Form

**Test Data:**
- **Name:** Ahmed Mohamed
- **Email:** ahmed@test.com
- **Phone:** 01234567890 (optional)
- **Password:** Test123456
- **Checkbox:** ✅ Agree to terms

---

### 6. Submit
Click **"Sign Up"** button

---

### 7. Expected Results

#### ✅ Success:
- Green success message: "Account created successfully!"
- Automatically redirects to dashboard after 1.5 seconds
- You're logged in

#### ❌ If Error Occurs:

**Network Error:**
- Check backend is running: http://localhost:5000/swagger
- Check browser console (F12) for error details
- Restart all services

**"Email already exists":**
- This is normal! The email is already registered
- Try a different email: ahmed2@test.com, ahmed3@test.com, etc.

**Validation Error:**
- Password must be at least 6 characters
- Email must be valid format
- All required fields must be filled

---

### 8. Check Backend Response

**In Backend Console Window, you should see:**
```
info: GradLink.Api.Controllers.AuthController[0]
      User registered: ahmed@test.com
info: GradLink.Api.Controllers.AuthController[0]
      User logged in: ahmed@test.com
```

---

## 📊 Test Different Scenarios:

### Test 1: Valid Signup ✅
- Name: John Doe
- Email: john@example.com
- Password: Pass123456
- **Expected:** Success

### Test 2: Duplicate Email ⚠️
- Use same email twice
- **Expected:** Error "Email already exists"

### Test 3: Weak Password ❌
- Password: 123
- **Expected:** Error "Password must be at least 6 characters"

### Test 4: Invalid Email ❌
- Email: notanemail
- **Expected:** Error "Invalid email format"

### Test 5: Missing Fields ❌
- Leave name empty
- **Expected:** Error "Please fill all required fields"

---

## 🔧 Debugging Tools:

### Browser Console (F12):
Check for:
- Network requests to http://localhost:5000
- Response status (200 = success, 400 = validation error, 500 = server error)
- CORS errors (should not appear now)

### Backend Console:
Watch for:
- Incoming requests
- Registration success/failure logs
- Database errors

---

## 🎯 Quick Test Command:

You can also test the API directly using curl:

```bash
curl -X POST http://localhost:5000/api/auth/register ^
  -H "Content-Type: application/json" ^
  -d "{\"email\":\"test@example.com\",\"password\":\"Test123\",\"fullName\":\"Test User\",\"phoneNumber\":\"\"}"
```

**Expected Response:**
```json
{
  "message": "Registration successful"
}
```

---

## ✅ Success Indicators:

- ✅ No CORS errors in browser console
- ✅ Request reaches backend (status 200 or 400)
- ✅ Backend logs show registration attempt
- ✅ Success message appears in UI
- ✅ Automatic redirect to dashboard
- ✅ User is logged in (can see dashboard)

---

## 🚨 Common Issues & Solutions:

| Issue | Solution |
|-------|----------|
| Network Error | Restart backend, check port 5000 |
| CORS Error | Clear browser cache, restart backend |
| 500 Server Error | Check backend console for details |
| Cannot connect to database | Check SQL Server connection |
| Port already in use | Kill process or change port |

---

## 🎉 All Working?

If signup works:
1. ✅ Backend is running correctly
2. ✅ Frontend can reach backend
3. ✅ CORS is configured properly
4. ✅ Database connection is working
5. ✅ Authentication flow is working

You're ready to develop! 🚀

