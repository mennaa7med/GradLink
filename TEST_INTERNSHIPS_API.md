# اختبار وإصلاح مشكلة Internships API

## الخطوات لحل المشكلة "Failed to load internships"

### الخطوة 1: تحقق من Backend

#### 1.1 تأكد إن Backend شغال
افتح المتصفح:
```
http://localhost:5000/swagger
```

إذا ما فتحش → Backend مش شغال → شغّله:
```bash
cd backend\GradLink.Api
dotnet run
```

#### 1.2 ابحث عن Internships في Swagger
في صفحة Swagger، ابحث عن:
```
Internships
  GET /api/internships
  GET /api/internships/my
  POST /api/internships
```

**إذا مش موجودين** → روح الخطوة 2

**إذا موجودين** → روح الخطوة 3

---

### الخطوة 2: عمل Database Migration (إذا الـ endpoints مش موجودة)

#### الطريقة السريعة:
```bash
cd backend\GradLink.Api
dotnet ef migrations add AddInternshipsTable
dotnet ef database update
```

#### بعد كده:
1. أعد تشغيل Backend
2. افتح Swagger تاني وتأكد إن الـ endpoints ظهرت

---

### الخطوة 3: اختبر الـ API من Swagger

#### 3.1 سجل دخول في Swagger
1. اضغط على **Authorize** (🔒) في أعلى اليمين
2. أدخل الـ token من Frontend:
   - افتح Console في المتصفح (F12)
   - اكتب: `localStorage.getItem('accessToken')`
   - انسخ الـ token
   - ارجع Swagger والصقه في: `Bearer YOUR_TOKEN_HERE`
   - اضغط Authorize

#### 3.2 جرب GET /api/internships/my
1. اضغط على **GET /api/internships/my**
2. اضغط **Try it out**
3. اضغط **Execute**

**النتائج المحتملة:**

**✅ نجح (200 OK):**
```json
[]
```
أو
```json
[
  {
    "id": 1,
    "title": "Backend Internship",
    ...
  }
]
```
→ **المشكلة في Frontend** → روح الخطوة 4

**❌ فشل (404 Not Found):**
```
Error: Not Found
```
→ **الـ endpoint مش موجود** → ارجع الخطوة 2

**❌ فشل (500 Internal Server Error):**
```
SqlException: Invalid object name 'Internships'
```
→ **الـ table مش موجود** → ارجع الخطوة 2

**❌ فشل (401 Unauthorized):**
```
Error: Unauthorized
```
→ **مشكلة في الـ authentication** → سجل دخول تاني في Frontend

---

### الخطوة 4: تحقق من Frontend

#### 4.1 افتح Console في المتصفح
اضغط **F12** → **Console**

#### 4.2 روح Company Dashboard → Internships
شوف الـ errors في Console:

**مثال 1:**
```
Failed to load internships: Network Error
```
→ Backend مش شغال أو الـ URL غلط

**مثال 2:**
```
Failed to load internships: Request failed with status code 404
```
→ الـ endpoint مش موجود → ارجع الخطوة 2

**مثال 3:**
```
Failed to load internships: Request failed with status code 500
Error response: {data: "SqlException: Invalid object name 'Internships'"}
```
→ الـ table مش موجود → ارجع الخطوة 2

**مثال 4:**
```
Internships loaded: []
```
→ **شغال تمام!** بس مفيش internships → جرب تضيف واحد

---

### الخطوة 5: جرب تضيف Internship

في Company Dashboard → Internships:
1. اضغط **"+ Add Internship"**
2. املأ البيانات:
   - Title: `Frontend Internship`
   - Description: `Learn React and Next.js`
   - Location: `Cairo, Egypt`
   - Duration: `3 months`
3. اضغط **Create Internship**

**إذا نجح:**
✅ تمام! المشكلة متحلت

**إذا فشل:**
شوف الـ error في Console واتبع الخطوات المناسبة

---

### الخطوة 6: تحقق من Career Page

1. روح `/career`
2. لازم تشوف الشركات اللي عندها internships
3. اضغط على شركة → اضغط على Internships tab
4. لازم تشوف الـ internships اللي أضفتها

---

## الأخطاء الشائعة وحلولها

### خطأ 1: "The Internships table might not exist in database"
**الحل:**
```bash
cd backend\GradLink.Api
dotnet ef migrations add AddInternshipsTable
dotnet ef database update
```

### خطأ 2: "Internships endpoint not found"
**الحل:**
تأكد إن الملف موجود:
```
backend/GradLink.Api/Controllers/InternshipsController.cs
```
إذا موجود، أعد build الـ backend:
```bash
cd backend
dotnet clean
dotnet build
dotnet run --project GradLink.Api
```

### خطأ 3: "Not authorized"
**الحل:**
سجل خروج وسجل دخول تاني في Frontend

### خطأ 4: Backend مش شغال
**الحل:**
```bash
cd backend\GradLink.Api
dotnet run
```

---

## Checklist سريع

قبل ما تقول "مش شغال"، تأكد من:

- [ ] ✅ Backend شغال (`http://localhost:5000/swagger`)
- [ ] ✅ Internships endpoints موجودة في Swagger
- [ ] ✅ Database migration تم بنجاح
- [ ] ✅ Internships table موجود في database
- [ ] ✅ مسجل دخول كـ Company في Frontend
- [ ] ✅ Token موجود في localStorage
- [ ] ✅ Console ما فيهوش errors
- [ ] ✅ Network tab يظهر request للـ `/api/internships/my`

---

## أوامر مفيدة

### تحقق من الـ Migrations
```bash
cd backend\GradLink.Api
dotnet ef migrations list
```
لازم تشوف migration اسمه فيه "Internship"

### تحقق من الـ Database
في SQL Server Management Studio:
```sql
USE GradLinkDb
GO
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Internships'
```

### أعد بناء Backend
```bash
cd backend
dotnet clean
dotnet build
cd GradLink.Api
dotnet run
```

### أعد تشغيل Frontend
```bash
npm run dev
```

---

تاريخ الإنشاء: 2025-11-26














