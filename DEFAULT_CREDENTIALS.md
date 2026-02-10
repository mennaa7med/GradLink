# 🔑 Default Login Credentials

## ✅ Admin Account (Pre-created)

The system automatically creates an admin account on first run:

```
Email:    admin@gradlink.com
Password: Admin@123
```

**Use this account to:**
- Test the login functionality
- Access admin features
- Manage the system

---

## 🆕 Create New Account

If you want to create a new user account:

### Option 1: Sign Up via UI
1. Go to: http://localhost:5173
2. Click **"Sign Up"**
3. Fill the form:
   - **Name:** Your Name
   - **Email:** your-email@example.com
   - **Phone:** (optional)
   - **Password:** At least 6 characters
   - ✅ Agree to terms
4. Click **"Sign Up"**
5. You'll be automatically logged in

### Option 2: Sign Up via API (Swagger)
1. Go to: http://localhost:5000/swagger
2. Find: `POST /api/auth/register`
3. Click "Try it out"
4. Fill the JSON:
```json
{
  "email": "newuser@example.com",
  "password": "YourPassword123",
  "fullName": "Your Full Name",
  "phoneNumber": ""
}
```
5. Click "Execute"

---

## 🧪 Test Accounts

You can create multiple test accounts:

### Student Account
```
Email:    student@test.com
Password: Student123
Name:     Test Student
```

### Company Account
```
Email:    company@test.com
Password: Company123
Name:     Test Company
```

### Mentor Account
```
Email:    mentor@test.com
Password: Mentor123
Name:     Test Mentor
```

**To create these:**
1. Go to Sign Up page
2. Enter the credentials above
3. Create the account

---

## ⚠️ Common Login Issues

### Issue 1: "Invalid credentials"
**Causes:**
- ❌ Wrong email or password
- ❌ Account doesn't exist (need to Sign Up first)
- ❌ Typo in email/password

**Solutions:**
1. Use default admin account: `admin@gradlink.com` / `Admin@123`
2. Or create new account via Sign Up
3. Check for typos (especially in email)

---

### Issue 2: Email already exists (during Sign Up)
**Cause:** That email is already registered

**Solution:** Use a different email or Sign In with existing account

---

### Issue 3: Password too short
**Cause:** Password must be at least 6 characters

**Solution:** Use longer password (e.g., "Test123456")

---

### Issue 4: Network Error
**Cause:** Backend not running

**Solution:**
1. Check backend is running: http://localhost:5000/swagger
2. If not, run: `START_BACKEND_ONLY.bat`

---

## 🔐 Password Requirements

- ✅ Minimum 6 characters
- ✅ Can contain letters, numbers, symbols
- ✅ No specific complexity requirements (for development)

**Examples of valid passwords:**
- `Test123`
- `Password123`
- `Admin@123`
- `MyPass2024`

---

## 📊 User Roles

The system supports 5 roles:

1. **Admin** - Full system access
2. **Student** - Student features
3. **Company** - Company/employer features
4. **Mentor** - Mentorship features
5. **Sponsor** - Sponsorship features

**Role is selected during Sign Up** (Student/Company toggle)

---

## 🎯 Quick Test Flow

### Test Login (Existing Admin):
```
1. Go to: http://localhost:5173
2. Click "Sign In" 
3. Email: admin@gradlink.com
4. Password: Admin@123
5. Select: Student or Company
6. Click "Sign In"
✅ Should redirect to Dashboard
```

### Test Sign Up (New User):
```
1. Go to: http://localhost:5173
2. Click "Sign Up"
3. Name: Test User
4. Email: test@example.com
5. Password: Test123456
6. Check: Agree to terms
7. Click "Sign Up"
✅ Should create account and auto-login
```

---

## 🆘 Still Can't Login?

### Debug Steps:

1. **Check Backend Console**
   - Look for login attempt logs
   - Check for errors

2. **Check Browser Console (F12)**
   - Look for API errors
   - Check network tab for response

3. **Test API Directly**
   - Go to: http://localhost:5000/swagger
   - Try: `POST /api/auth/login`
   - Use admin credentials

4. **Check Database**
   - Backend console should show: "Created admin user: admin@gradlink.com"
   - If not, restart backend

---

## 🔄 Reset Everything

If nothing works, try this:

### Reset Backend:
```bash
cd backend\GradLink.Api
# Delete database file (if using SQLite)
del gradlink.db
# Restart backend
dotnet run
```

This will:
- Create fresh database
- Re-seed admin account
- Start clean

---

## 📝 Summary

**Default Login:**
- 📧 admin@gradlink.com
- 🔑 Admin@123

**New Account:**
- Click "Sign Up" (not "Sign In")
- Fill form with your details
- Minimum 6 characters password

**Test It:**
1. Backend running ✅
2. Frontend running ✅
3. Try admin login ✅
4. Or create new account ✅

**You're ready! 🎉**

