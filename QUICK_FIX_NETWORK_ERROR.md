# 🔧 حل مشكلة Network Error - دليل سريع

## ❌ **المشكلة:**
عند محاولة التسجيل في صفحة Signup، تظهر رسالة: **"Network Error"**

---

## 💡 **السبب:**
الـ **Backend API مش شغال** على `http://localhost:5000`

Frontend بيحاول يتصل بالـ Backend لكن مش لاقيه، فبيرجع Network Error.

---

## ✅ **الحل:**

### **الطريقة 1: استخدام الملف الجاهز** (الأسهل)

1. **افتح الملف:** `START_BACKEND.bat` (في المجلد الرئيسي)
2. **اضغط عليه مرتين** (double-click)
3. **انتظر** حتى ترى رسالة: `Now listening on: http://localhost:5000`
4. **جرب التسجيل مرة تانية**

---

### **الطريقة 2: من Terminal مباشرة**

**افتح Command Prompt (CMD) وشغّل:**

```cmd
cd /d "D:\كل المهم\viteProject\Newfolder\backend\GradLink.Api"
dotnet run
```

**انتظر حتى ترى:**
```
info: Now listening on: http://localhost:5000
info: Now listening on: https://localhost:5001
info: Application started.
```

---

### **الطريقة 3: من PowerShell**

```powershell
# روح للمجلد
Set-Location -LiteralPath "D:\كل المهم\viteProject\Newfolder"

# شغّل Backend
cd backend\GradLink.Api
dotnet run --urls http://localhost:5000
```

---

### **الطريقة 4: من VS Code**

1. افتح VS Code
2. افتح Terminal (Ctrl + `)
3. اكتب:
```bash
cd backend/GradLink.Api
dotnet run
```

---

## 🧪 **كيف تتحقق إن Backend شغال:**

### **1. من المتصفح:**
افتح: http://localhost:5000/swagger

**إذا Backend شغال:** هتشوف صفحة Swagger UI  
**إذا مش شغال:** هتشوف "This site can't be reached"

### **2. من PowerShell:**
```powershell
Test-NetConnection -ComputerName localhost -Port 5000
```

**إذا شغال:** `TcpTestSucceeded: True`  
**إذا مش شغال:** `TcpTestSucceeded: False`

### **3. من CMD:**
```cmd
curl http://localhost:5000/swagger
```

**إذا شغال:** هتشوف HTML  
**إذا مش شغال:** هتشوف "Failed to connect"

---

## 🎯 **خطوات الحل الكاملة:**

### **خطوة 1: شغّل Backend**
```cmd
cd /d "D:\كل المهم\viteProject\Newfolder\backend\GradLink.Api"
dotnet run
```

**انتظر حتى ترى:**
```
✅ Now listening on: http://localhost:5000
```

### **خطوة 2: تحقق من Backend**
افتح في المتصفح: http://localhost:5000/swagger

**يجب أن ترى:** صفحة Swagger بكل الـ endpoints

### **خطوة 3: شغّل Frontend** (في terminal جديد)
```bash
npm run dev
```

**انتظر حتى ترى:**
```
✅ Local: http://localhost:5176/
```

### **خطوة 4: جرب التسجيل**
1. افتح: http://localhost:5176/signup
2. املأ البيانات
3. اضغط "Sign Up"

**المتوقع:** تسجيل ناجح + redirect للـ Dashboard ✅

---

## 🚨 **مشاكل إضافية محتملة:**

### **Problem 1: Port 5000 مشغول**
```
Error: Address already in use
```

**الحل:**
```powershell
# اوقف البرنامج القديم
Get-Process -Name dotnet | Stop-Process -Force

# أو استخدم port تاني
dotnet run --urls http://localhost:5001
```

**ثم غيّر في `.env.development`:**
```env
VITE_API_BASE_URL=http://localhost:5001
```

---

### **Problem 2: Database Connection Error**
```
Error: Cannot connect to SQL Server
```

**الحل:**
1. تحقق من إعدادات قاعدة البيانات في `appsettings.json`
2. تأكد إن Connection String صحيح
3. شغّل migrations:
```bash
cd backend/GradLink.Api
dotnet ef database update
```

---

### **Problem 3: CORS Error**
```
Access to XMLHttpRequest has been blocked by CORS policy
```

**الحل:**
في `backend/GradLink.Api/appsettings.json`:
```json
"Cors": {
  "AllowedOrigins": [
    "http://localhost:5176",
    "http://localhost:3000"
  ]
}
```

---

### **Problem 4: .env.development مش موجود**
```
API URL: undefined
```

**الحل:**
1. أنشئ ملف `.env.development` في المجلد الرئيسي
2. أضف:
```env
VITE_API_BASE_URL=http://localhost:5000
VITE_SIGNALR_HUB_URL=http://localhost:5000/hubs/chat
```
3. أعد تشغيل Frontend (Ctrl+C ثم `npm run dev`)

---

## 📋 **Checklist - تأكد من الآتي:**

قبل ما تجرب التسجيل، تحقق من:

- [ ] ✅ Backend شغال (`dotnet run` في terminal)
- [ ] ✅ ترى: "Now listening on: http://localhost:5000"
- [ ] ✅ Swagger يفتح: http://localhost:5000/swagger
- [ ] ✅ Frontend شغال (`npm run dev` في terminal جديد)
- [ ] ✅ ترى: "Local: http://localhost:5176/"
- [ ] ✅ ملف `.env.development` موجود وصحيح
- [ ] ✅ مافيش CORS errors في Console (F12)

إذا كل النقاط دي ✅، يبقى هيشتغل معاك!

---

## 🎬 **سيناريو كامل (من الصفر):**

### **Terminal 1 (Backend):**
```cmd
cd /d "D:\كل المهم\viteProject\Newfolder\backend\GradLink.Api"
dotnet run

# انتظر...
# ✅ Now listening on: http://localhost:5000
```

### **Terminal 2 (Frontend):**
```bash
cd "D:\كل المهم\viteProject\Newfolder"
npm run dev

# انتظر...
# ✅ Local: http://localhost:5176/
```

### **Browser:**
```
1. افتح: http://localhost:5176/signup
2. املأ النموذج
3. اضغط Sign Up
4. ✅ Success! → Dashboard
```

---

## 🆘 **لو لسه مش شغال:**

### **جرّب دا:**

1. **أوقف كل حاجة:**
```bash
# أوقف Frontend (Ctrl+C في terminal)
# أوقف Backend (Ctrl+C في terminal التاني)
```

2. **امسح كل processes:**
```powershell
Get-Process -Name dotnet | Stop-Process -Force
Get-Process -Name node | Stop-Process -Force
```

3. **ابدأ من جديد:**
```bash
# Terminal 1
cd backend/GradLink.Api
dotnet clean
dotnet build
dotnet run

# Terminal 2
npm run dev
```

4. **Hard Refresh في المتصفح:**
```
Ctrl + Shift + R
```

---

## 🎉 **علامات النجاح:**

### **Backend Terminal:**
```
✅ Now listening on: http://localhost:5000
✅ Application started. Press Ctrl+C to shut down.
```

### **Frontend Terminal:**
```
✅ VITE v7.x.x ready in xxx ms
✅ Local: http://localhost:5176/
```

### **Browser Console (F12):**
```
✅ No red errors
✅ API calls to http://localhost:5000
✅ Response 200 OK
```

### **Signup Page:**
```
✅ Form submits
✅ Green success message appears
✅ Redirects to /dashboard
✅ User is logged in
```

---

## 🔗 **ملفات مهمة:**

### **Backend:**
- `backend/GradLink.Api/Program.cs` - تشغيل البرنامج
- `backend/GradLink.Api/appsettings.json` - الإعدادات
- `backend/GradLink.Api/Controllers/AuthController.cs` - Login/Register

### **Frontend:**
- `.env.development` - عنوان الـ API
- `src/api/client.js` - Axios config
- `src/contexts/AuthContext.jsx` - Auth logic
- `src/components/Signup/Signup.jsx` - Signup form

---

## 📞 **Debug Info:**

إذا لسه في مشكلة، ابعت لي:

1. **Backend Terminal Output** (آخر 20 سطر)
2. **Browser Console** (F12 → Console → أي errors حمرا)
3. **Network Tab** (F12 → Network → الـ failed request)
4. **Screenshot** من الـ error

---

**تم الإنشاء:** 2025-11-19  
**الحالة:** جاهز للاستخدام ✅

