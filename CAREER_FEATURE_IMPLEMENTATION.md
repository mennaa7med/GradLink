# Career Feature Implementation - تنفيذ ميزة Career

## نظرة عامة / Overview

تم تنفيذ ميزة Career الجديدة بالكامل في الموقع. هذه الميزة تسمح للشركات بنشر الوظائف والتدريبات والمشاريع، وللمستخدمين بعرضها من خلال صفحة Career.

## التغييرات في Backend

### 1. إنشاء Internship Entity
**الملف:** `backend/GradLink.Domain/Entities/Internship.cs`
- Entity جديد للتدريبات مع جميع الخصائص المطلوبة (العنوان، الوصف، المدة، الراتب، إلخ)

### 2. إنشاء DTOs للـ Internships
**الملفات:**
- `backend/GradLink.Application/DTOs/Internships/CreateInternshipRequest.cs`
- `backend/GradLink.Application/DTOs/Internships/InternshipDto.cs`

### 3. إنشاء InternshipsController
**الملف:** `backend/GradLink.Api/Controllers/InternshipsController.cs`
- CRUD endpoints كاملة للتدريبات
- GET `/api/internships` - جميع التدريبات النشطة
- GET `/api/internships/my` - تدريبات الشركة
- POST `/api/internships` - إنشاء تدريب جديد
- PUT `/api/internships/{id}` - تحديث تدريب
- DELETE `/api/internships/{id}` - حذف تدريب

### 4. تحديث Database Context
**الملف:** `backend/GradLink.Infrastructure/Persistence/AppDbContext.cs`
- إضافة `DbSet<Internship>`
- إضافة configuration للـ Internship entity

### 5. تحديث ApplicationUser
**الملف:** `backend/GradLink.Domain/Entities/ApplicationUser.cs`
- إضافة navigation property للـ Internships

## التغييرات في Frontend

### 1. API Functions
**الملفات الجديدة:**
- `src/api/jobs.js` - API functions للوظائف
- `src/api/internships.js` - API functions للتدريبات

**الوظائف المتاحة:**
- `listJobs()` / `listInternships()` - جلب جميع الوظائف/التدريبات
- `listMyJobs()` / `listMyInternships()` - جلب وظائف/تدريبات الشركة
- `createJob()` / `createInternship()` - إنشاء جديد
- `updateJob()` / `updateInternship()` - تحديث
- `deleteJob()` / `deleteInternship()` - حذف

### 2. Company Dashboard - Sidebar
**الملف:** `src/components/CompanyDashboard/Sidebar.jsx`
- إضافة قائمة "Jobs" 💼
- إضافة قائمة "Internships" 🎓

### 3. Company Dashboard - Content Router
**الملف:** `src/components/CompanyDashboard/Content.jsx`
- إضافة routing للصفحات الجديدة

### 4. صفحات Company Dashboard الجديدة
**الملفات:**
- `src/components/CompanyDashboard/Pages/Jobs.jsx`
- `src/components/CompanyDashboard/Pages/Internships.jsx`

**المميزات:**
- عرض جميع الوظائف/التدريبات في جدول
- زر "Add" لإضافة وظيفة/تدريب جديد
- Modal منسق لإدخال البيانات
- Edit و Delete للوظائف/التدريبات الموجودة
- عرض المهارات والمتطلبات
- حالة (Status) لكل وظيفة/تدريب

### 5. صفحة Career الجديدة
**الملفات:**
- `src/pages/Career.jsx`
- `src/pages/Career.css`

**المميزات:**
- عرض جميع الشركات في sidebar
- عند اختيار شركة، يظهر:
  - Tab للوظائف (Jobs)
  - Tab للتدريبات (Internships)
  - Tab للمشاريع (Projects)
- عرض تفاصيل كل فرصة مع:
  - العنوان والوصف
  - الموقع ونوع العمل
  - المهارات المطلوبة
  - الراتب/المكافأة
- تصميم جميل ومنسق مع ألوان الموقع
- Responsive design يعمل على جميع الأحجام

### 6. تحديث Navbar
**الملف:** `src/components/Navbar.jsx`
- إضافة رابط "Career" في القائمة الرئيسية

### 7. تحديث App Router
**الملف:** `src/App.jsx`
- إضافة route `/career` للصفحة الجديدة

## كيفية الاستخدام

### للشركات (Company Dashboard):

1. **تسجيل الدخول** إلى dashboard الشركة
2. **الوظائف (Jobs):**
   - اضغط على "Jobs" من الـ sidebar
   - اضغط على زر "+ Add Job" لإضافة وظيفة جديدة
   - املأ البيانات المطلوبة (العنوان، الوصف، الموقع، الراتب، المهارات، إلخ)
   - اضغط "Create Job"
   - يمكنك Edit أو Delete أي وظيفة موجودة

3. **التدريبات (Internships):**
   - اضغط على "Internships" من الـ sidebar
   - اضغط على زر "+ Add Internship"
   - املأ البيانات (العنوان، الوصف، المدة، هل مدفوعة، المهارات، إلخ)
   - اضغط "Create Internship"
   - يمكنك Edit أو Delete أي تدريب

### للمستخدمين (Career Page):

1. **افتح الصفحة الرئيسية** واضغط على "Career" من الـ navbar
2. **اختر شركة** من القائمة على اليسار
3. **اختر tab** (Jobs, Internships, أو Projects)
4. **استعرض الفرص** المتاحة مع جميع التفاصيل

## التصميم والألوان

- تم استخدام نفس الـ color palette للموقع:
  - Primary: `#667eea` (Purple-Blue)
  - Secondary: `#764ba2` (Purple)
  - Gradient: من `#667eea` إلى `#764ba2`
- التصميم responsive ويعمل على:
  - Desktop (1024px+)
  - Tablet (768px - 1024px)
  - Mobile (< 768px)

## الملاحظات المهمة

### Database Migration
⚠️ **مهم:** بعد تشغيل Backend، يجب عمل migration للـ database لإضافة جدول Internships:

```bash
cd backend/GradLink.Api
dotnet ef migrations add AddInternships
dotnet ef database update
```

أو إذا كنت تستخدم Package Manager Console في Visual Studio:

```powershell
Add-Migration AddInternships
Update-Database
```

### الحقول المطلوبة (Required)

**للوظائف (Jobs):**
- Title (required)
- Description (required)

**للتدريبات (Internships):**
- Title (required)
- Description (required)

جميع الحقول الأخرى اختيارية.

## الاختبار

### اختبار Backend:
1. شغل Backend server
2. افتح Swagger: `http://localhost:5000/swagger`
3. اختبر endpoints:
   - `/api/jobs`
   - `/api/internships`

### اختبار Frontend:
1. شغل Frontend: `npm run dev`
2. سجل دخول كشركة
3. اذهب إلى Company Dashboard
4. جرب إضافة وظيفة/تدريب
5. افتح صفحة Career للتأكد من ظهور البيانات

## الملفات المضافة/المعدلة

### Backend (7 ملفات):
1. ✅ `backend/GradLink.Domain/Entities/Internship.cs` (جديد)
2. ✅ `backend/GradLink.Application/DTOs/Internships/CreateInternshipRequest.cs` (جديد)
3. ✅ `backend/GradLink.Application/DTOs/Internships/InternshipDto.cs` (جديد)
4. ✅ `backend/GradLink.Api/Controllers/InternshipsController.cs` (جديد)
5. ✅ `backend/GradLink.Infrastructure/Persistence/AppDbContext.cs` (معدل)
6. ✅ `backend/GradLink.Domain/Entities/ApplicationUser.cs` (معدل)

### Frontend (11 ملف):
1. ✅ `src/api/jobs.js` (جديد)
2. ✅ `src/api/internships.js` (جديد)
3. ✅ `src/components/CompanyDashboard/Sidebar.jsx` (معدل)
4. ✅ `src/components/CompanyDashboard/Content.jsx` (معدل)
5. ✅ `src/components/CompanyDashboard/Pages/Jobs.jsx` (جديد)
6. ✅ `src/components/CompanyDashboard/Pages/Internships.jsx` (جديد)
7. ✅ `src/pages/Career.jsx` (جديد)
8. ✅ `src/pages/Career.css` (جديد)
9. ✅ `src/components/Navbar.jsx` (معدل)
10. ✅ `src/App.jsx` (معدل)

## الخلاصة

تم تنفيذ جميع المطلوبات بنجاح:
- ✅ إضافة Jobs و Internships في Company Dashboard Sidebar
- ✅ صفحات Jobs و Internships مع زر Add في كل صفحة
- ✅ صفحة Career في الـ navbar
- ✅ عرض جميع الشركات مع Jobs, Internships, Projects
- ✅ تصميم جميل ومنسق مع ألوان الموقع
- ✅ Responsive design

---

تم التنفيذ بواسطة: AI Assistant
التاريخ: 2025-11-26














