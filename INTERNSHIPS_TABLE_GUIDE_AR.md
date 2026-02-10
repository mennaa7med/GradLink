# إضافة جدول Internships إلى Database

## 🎯 الهدف
إضافة جدول Internships في الـ Database عشان Company Dashboard يشتغل.

---

## ⚡ الطريقة الأسرع (موصى بها)

### شغّل الملف:
```
ADD_INTERNSHIPS_TABLE.bat
```

**هيعمل كل حاجة تلقائياً!** ✅

---

## 📋 الطريقة اليدوية

### الخطوة 1: افتح PowerShell
```powershell
cd "D:\كل المهم\viteProject\Newfolder"
```

### الخطوة 2: تحديث EF Tools
```powershell
dotnet tool update --global dotnet-ef
```

### الخطوة 3: اذهب لمجلد API
```powershell
cd backend\GradLink.Api
```

### الخطوة 4: بناء المشروع
```powershell
cd ..
dotnet build
cd GradLink.Api
```

### الخطوة 5: إنشاء Migration للـ Internships
```powershell
dotnet ef migrations add AddInternshipsTable --project ../GradLink.Infrastructure --startup-project .
```

**لو قال "migration already exists"، استخدم اسم مختلف:**
```powershell
dotnet ef migrations add CreateInternshipsTable --project ../GradLink.Infrastructure --startup-project .
```

### الخطوة 6: تطبيق Migration على Database
```powershell
dotnet ef database update --project ../GradLink.Infrastructure --startup-project .
```

### الخطوة 7: تشغيل Backend
```powershell
dotnet run
```

---

## ✅ التحقق من نجاح الإضافة

### 1. شوف Migrations
```powershell
dotnet ef migrations list --project ../GradLink.Infrastructure --startup-project .
```

**يجب أن ترى:**
```
✅ AddInternshipsTable (or similar name)
```

### 2. افتح Swagger
```
http://localhost:5000/swagger
```

**ابحث عن Internships** - يجب أن تجد:
- `GET /api/internships`
- `GET /api/internships/my`
- `POST /api/internships`
- `PUT /api/internships/{id}`
- `DELETE /api/internships/{id}`

### 3. اختبر API من Swagger
1. اضغط على **GET /api/internships/my**
2. اضغط **Try it out**
3. اضغط **Execute**
4. يجب أن يرجع **200 OK** مع `[]`

---

## 🎯 اختبار من Frontend

### 1. شغّل Frontend
```powershell
npm run dev
```

### 2. سجل دخول كـ Company

### 3. افتح Company Dashboard

### 4. اضغط على "Internships" من Sidebar

### 5. يجب أن تفتح الصفحة بدون errors ✅

### 6. جرب إضافة Internship جديد:
- اضغط **"+ Add Internship"**
- املأ البيانات:
  - **Title:** Frontend Internship
  - **Description:** Learn React and TypeScript
  - **Location:** Cairo, Egypt
  - **Duration:** 3 months
  - **Is Paid:** ✓
  - **Stipend:** 3000
- اضغط **Create Internship**
- يجب أن يُحفظ بنجاح! ✅

### 7. تحقق من Career Page:
- اذهب إلى **Career** من Navbar
- اضغط على شركتك
- اضغط على **Internships** tab
- يجب أن تشاهد الـ Internship الذي أضفته! ✅

---

## 🗂️ هيكل جدول Internships

بعد Migration، الجدول هيكون فيه الأعمدة دي:

| Column | Type | Description |
|--------|------|-------------|
| Id | INT | معرف فريد (Primary Key) |
| Title | NVARCHAR(200) | عنوان التدريب (مطلوب) |
| Description | NVARCHAR(MAX) | وصف التدريب (مطلوب) |
| Requirements | NVARCHAR(MAX) | المتطلبات (JSON) |
| Skills | NVARCHAR(MAX) | المهارات المطلوبة (JSON) |
| Location | NVARCHAR(MAX) | الموقع |
| Duration | NVARCHAR(MAX) | المدة (3 months, 6 months, etc.) |
| IsPaid | BIT | هل مدفوع؟ (True/False) |
| Stipend | DECIMAL(18,2) | المكافأة الشهرية |
| CompanyName | NVARCHAR(MAX) | اسم الشركة |
| Status | NVARCHAR(50) | الحالة (Active/Closed) |
| CreatedAt | DATETIME2 | تاريخ الإنشاء |
| UpdatedAt | DATETIME2 | تاريخ آخر تحديث |
| ExpiresAt | DATETIME2 | تاريخ الانتهاء |
| StartDate | DATETIME2 | تاريخ البداية |
| EndDate | DATETIME2 | تاريخ النهاية |
| PostedById | NVARCHAR(450) | معرف الشركة (Foreign Key) |

---

## 🚨 حل المشاكل

### مشكلة 1: "Migration already exists"
**الحل:**
```powershell
# استخدم اسم مختلف
dotnet ef migrations add CreateInternshipsTableV2 --project ../GradLink.Infrastructure --startup-project .
```

### مشكلة 2: "Build failed"
**الحل:**
```powershell
cd backend
dotnet clean
dotnet restore
dotnet build
```

### مشكلة 3: "The Internships table already exists"
**الحل:**
```powershell
# الجدول موجود بالفعل، فقط شغّل Backend
cd backend\GradLink.Api
dotnet run
```

### مشكلة 4: "Unable to connect to database"
**الحل:**
- تحقق من اتصال الإنترنت (الـ database على remote server)
- تحقق من Connection String في `appsettings.json`

### مشكلة 5: "Failed to load internships" في Frontend
**الحل:**
```powershell
# تأكد إن Migration تم بنجاح
dotnet ef migrations list --project ../GradLink.Infrastructure --startup-project .

# لو مفيش ✅ قدام AddInternshipsTable، نفذ:
dotnet ef database update --project ../GradLink.Infrastructure --startup-project .
```

---

## 📋 الأوامر الكاملة (نسخ ولصق)

```powershell
# 1. تحديث EF Tools
dotnet tool update --global dotnet-ef

# 2. اذهب للمشروع
cd "D:\كل المهم\viteProject\Newfolder\backend\GradLink.Api"

# 3. بناء المشروع
cd ..
dotnet build
cd GradLink.Api

# 4. إنشاء Migration للـ Internships
dotnet ef migrations add AddInternshipsTable --project ../GradLink.Infrastructure --startup-project .

# 5. تطبيق على Database
dotnet ef database update --project ../GradLink.Infrastructure --startup-project .

# 6. تشغيل Backend
dotnet run
```

---

## ✅ Checklist النهائي

تأكد من:

- [ ] ✅ EF Tools مثبت ومحدث
- [ ] ✅ المشروع يعمل Build بنجاح
- [ ] ✅ Migration تم إنشاؤه
- [ ] ✅ Migration تم تطبيقه على Database
- [ ] ✅ Backend شغال على port 5000
- [ ] ✅ Swagger يعرض Internships endpoints
- [ ] ✅ GET /api/internships/my يرجع 200
- [ ] ✅ Frontend - Internships page يفتح بدون errors
- [ ] ✅ يمكن إضافة Internship جديد
- [ ] ✅ Internship يظهر في Career Page

---

## 🎉 بعد النجاح

الآن يمكنك:

1. ✅ إضافة تدريبات من Company Dashboard
2. ✅ عرض التدريبات في Career Page
3. ✅ تعديل وحذف التدريبات
4. ✅ البحث والفلترة في التدريبات

**كل شيء جاهز للعمل!** 🚀

---

تاريخ الإنشاء: 2025-11-26













