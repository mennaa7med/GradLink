# 🔗 Signup Page - Backend Integration Guide

## ✅ الربط مكتمل!

صفحة التسجيل (Signup) الآن متصلة بالكامل مع الـ Backend اللي عملناه.

---

## 📋 كيف يعمل النظام؟

### **1. المستخدم يملأ النموذج** 📝
```
- Full Name (required)
- Email (required, validated)
- Phone Number (optional)
- Password (required, min 6 characters)
- Agree to Terms (required)
```

### **2. Frontend يرسل الطلب** 🚀
عند الضغط على "Sign Up"، يتم:

```javascript
// في AuthContext.jsx
const register = async (email, password, fullName, phoneNumber) => {
  await api.post('/api/auth/register', { 
    email, 
    password, 
    fullName,
    phoneNumber 
  });
  // auto-login بعد التسجيل
  await login(email, password);
};
```

**الـ Request يروح على:**
```
POST http://localhost:5000/api/auth/register
```

**بـ Body:**
```json
{
  "email": "user@example.com",
  "password": "SecurePass123",
  "fullName": "John Doe",
  "phoneNumber": "+1234567890"
}
```

### **3. Backend يعالج الطلب** ⚙️

في `AuthController.cs`:
```csharp
[HttpPost("register")]
public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
{
    // 1. إنشاء مستخدم جديد
    var user = new ApplicationUser
    {
        UserName = request.Email,
        Email = request.Email,
        FullName = request.FullName,
        PhoneNumber = request.PhoneNumber,
        CreatedAt = DateTime.UtcNow
    };

    // 2. حفظ المستخدم في قاعدة البيانات
    var result = await _userManager.CreateAsync(user, request.Password);

    // 3. إضافة دور "Student" للمستخدم
    await _userManager.AddToRoleAsync(user, "Student");

    // 4. إرجاع Access Token و Refresh Token
    return await GenerateAuthResponse(user);
}
```

### **4. Backend يرجع الاستجابة** 📨

**Success Response (200 OK):**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
  "user": {
    "id": "user-uuid",
    "email": "user@example.com",
    "fullName": "John Doe",
    "roles": ["Student"]
  }
}
```

**Error Response (400 Bad Request):**
```json
{
  "errors": [
    "Password must have at least one uppercase letter.",
    "Password must have at least one number."
  ]
}
```

### **5. Frontend يتعامل مع الاستجابة** ✨

**إذا نجح التسجيل:**
```javascript
// يتم حفظ الـ tokens
localStorage.setItem('accessToken', data.accessToken);
localStorage.setItem('refreshToken', data.refreshToken);

// عرض رسالة نجاح
setSuccess('Account created successfully! Redirecting to dashboard...');

// التوجه للـ Dashboard بعد 1.5 ثانية
setTimeout(() => navigate('/dashboard'), 1500);
```

**إذا فشل التسجيل:**
```javascript
// عرض رسالة الخطأ
setError('Registration failed. Email already exists.');
```

---

## 🔒 التحققات (Validations)

### **Frontend Validations:**
✅ جميع الحقول المطلوبة ممتلئة  
✅ البريد الإلكتروني بصيغة صحيحة  
✅ كلمة المرور 6 أحرف على الأقل  
✅ الموافقة على الشروط والأحكام

### **Backend Validations:**
✅ البريد الإلكتروني بصيغة صحيحة (`[EmailAddress]`)  
✅ كلمة المرور 6 أحرف على الأقل (`[MinLength(6)]`)  
✅ جميع الحقول المطلوبة موجودة (`[Required]`)  
✅ البريد الإلكتروني غير مستخدم مسبقاً  
✅ كلمة المرور تتبع سياسة ASP.NET Identity

---

## 🎯 مميزات الربط

### **1. Error Handling محسّن** ❌
```javascript
// معالجة أخطاء ASP.NET بكفاءة
if (err.response?.data?.errors) {
  if (Array.isArray(err.response.data.errors)) {
    errorMessage = err.response.data.errors.join(', ');
  } else if (typeof err.response.data.errors === 'object') {
    errorMessage = Object.values(err.response.data.errors).flat().join(', ');
  }
}
```

### **2. Auto-Login بعد التسجيل** 🔓
```javascript
// تسجيل الدخول تلقائياً بعد إنشاء الحساب
await register(email, password, name, phone);
// الآن المستخدم مسجل دخول تلقائياً!
```

### **3. Loading State** ⏳
```javascript
// عرض "Signing Up..." أثناء المعالجة
<button type="submit" disabled={isLoading}>
  {isLoading ? 'Signing Up...' : 'Sign Up'}
</button>
```

### **4. Success Message** ✅
```javascript
// رسالة نجاح مع countdown
setSuccess('Account created successfully! Redirecting to dashboard...');
setTimeout(() => navigate('/dashboard'), 1500);
```

---

## 🔐 JWT Token Management

### **كيف يتم حفظ الـ Tokens:**
```javascript
// في AuthContext بعد التسجيل/تسجيل الدخول
localStorage.setItem('accessToken', data.accessToken);
localStorage.setItem('refreshToken', data.refreshToken);
```

### **كيف يتم استخدامها:**
```javascript
// في api/client.js - Axios Interceptor
api.interceptors.request.use((config) => {
  const access = localStorage.getItem('accessToken');
  if (access) {
    config.headers.Authorization = `Bearer ${access}`;
  }
  return config;
});
```

### **Automatic Token Refresh:**
```javascript
// عند انتهاء صلاحية الـ Access Token (401)
if (error.response?.status === 401) {
  // يتم تجديد الـ Token تلقائياً
  const { data } = await axios.post('/api/auth/refresh', { refreshToken });
  // يتم إعادة محاولة الطلب الأصلي
  return api(originalRequest);
}
```

---

## 📂 الملفات المعنية

### **Frontend Files:**
```
src/
├── components/
│   └── Signup/
│       ├── Signup.jsx          # مكون التسجيل
│       └── Signup.css          # تصميم الصفحة
├── contexts/
│   └── AuthContext.jsx         # إدارة المصادقة
└── api/
    └── client.js               # Axios client + interceptors
```

### **Backend Files:**
```
backend/
├── GradLink.Api/
│   └── Controllers/
│       └── AuthController.cs              # معالج طلبات المصادقة
├── GradLink.Application/
│   └── DTOs/
│       └── Auth/
│           ├── RegisterRequest.cs         # نموذج طلب التسجيل
│           └── AuthResponse.cs            # نموذج استجابة المصادقة
├── GradLink.Domain/
│   └── Entities/
│       └── ApplicationUser.cs             # كيان المستخدم
└── GradLink.Infrastructure/
    ├── Identity/
    │   └── JwtTokenService.cs             # خدمة JWT
    └── Persistence/
        └── AppDbContext.cs                # قاعدة البيانات
```

---

## 🧪 كيف تختبر؟

### **1. تشغيل Backend:**
```bash
cd backend/GradLink.Api
dotnet run
```
**يجب أن ترى:**
```
info: Now listening on: http://localhost:5000
```

### **2. تشغيل Frontend:**
```bash
npm run dev
```
**يجب أن ترى:**
```
Local: http://localhost:5176/
```

### **3. اختبار التسجيل:**

**الطريقة 1: عبر الواجهة**
1. افتح: http://localhost:5176/signup
2. املأ النموذج:
   - Name: Test User
   - Email: test@example.com
   - Phone: +1234567890
   - Password: Test123
   - ✓ Agree to terms
3. اضغط "Sign Up"
4. **المتوقع:**
   - رسالة نجاح خضراء ✅
   - التوجه تلقائياً للـ Dashboard
   - حفظ الـ tokens في localStorage

**الطريقة 2: عبر Swagger**
1. افتح: http://localhost:5000/swagger
2. اذهب لـ: `POST /api/auth/register`
3. اضغط "Try it out"
4. أدخل:
```json
{
  "email": "test2@example.com",
  "password": "Test123",
  "fullName": "Test User 2",
  "phoneNumber": "+9876543210"
}
```
5. اضغط "Execute"
6. **المتوقع:** Response 200 مع tokens

**الطريقة 3: عبر Console**
```javascript
// افتح Console في المتصفح (F12)
fetch('http://localhost:5000/api/auth/register', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    email: 'test3@example.com',
    password: 'Test123',
    fullName: 'Test User 3',
    phoneNumber: ''
  })
})
  .then(res => res.json())
  .then(data => console.log('✅ Success:', data))
  .catch(err => console.error('❌ Error:', err));
```

---

## 🚨 حالات الخطأ الشائعة

### **1. Email Already Exists**
```json
{
  "errors": ["Email 'test@example.com' is already taken."]
}
```
**الحل:** استخدم بريد إلكتروني مختلف

### **2. Weak Password**
```json
{
  "errors": [
    "Passwords must have at least one uppercase ('A'-'Z').",
    "Passwords must have at least one digit ('0'-'9')."
  ]
}
```
**الحل:** استخدم كلمة مرور أقوى (مثل: `Test123@`)

### **3. Invalid Email Format**
```json
{
  "errors": ["The Email field is not a valid e-mail address."]
}
```
**الحل:** تحقق من صيغة البريد الإلكتروني

### **4. Backend Not Running**
```
Error: Network Error
```
**الحل:** تأكد من تشغيل Backend على http://localhost:5000

### **5. Database Connection Error**
```
Error: Cannot connect to SQL Server
```
**الحل:** تحقق من إعدادات قاعدة البيانات في `appsettings.json`

---

## 🎯 الحالة الحالية

| المكون | الحالة | ملاحظات |
|--------|--------|---------|
| Frontend Signup Form | ✅ جاهز | تصميم Glass Morphism متطابق مع Signin |
| Backend API | ✅ جاهز | ASP.NET Core 8 + Identity |
| Database | ✅ جاهز | SQL Server (Remote) |
| JWT Authentication | ✅ جاهز | Access + Refresh Tokens |
| Auto-Login | ✅ جاهز | بعد التسجيل مباشرة |
| Error Handling | ✅ جاهز | معالجة شاملة للأخطاء |
| Success Messages | ✅ جاهز | مع Animation |
| Loading States | ✅ جاهز | زر معطل أثناء المعالجة |
| Validations | ✅ جاهز | Frontend + Backend |
| Token Refresh | ✅ جاهز | تلقائي عند انتهاء الصلاحية |

---

## 📊 Flow Chart

```
┌─────────────────┐
│  User fills     │
│  Signup Form    │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Frontend       │
│  Validations    │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  POST /register │
│  to Backend     │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Backend        │
│  Validations    │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Create User    │
│  in Database    │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Generate       │
│  JWT Tokens     │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Return         │
│  AuthResponse   │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Auto-Login     │
│  (save tokens)  │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Navigate to    │
│  Dashboard      │
└─────────────────┘
```

---

## 🎉 كل شيء جاهز!

النظام الآن يعمل بشكل كامل:
- ✅ **Frontend متصل بـ Backend**
- ✅ **JWT Authentication شغال**
- ✅ **Error Handling محسّن**
- ✅ **Auto-Login بعد التسجيل**
- ✅ **Token Refresh تلقائي**
- ✅ **UI/UX ممتاز**

---

**Created:** 2025-11-19  
**Status:** ✅ Production Ready

