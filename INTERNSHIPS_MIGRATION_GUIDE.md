# إصلاح مشكلة Internships في Company Dashboard

## المشكلة
سكشن Internships مش شغال في Company Dashboard لأن الـ table مش موجود في الـ database.

## الحل السريع

### الطريقة 1: استخدام الـ Batch File (الأسهل)

1. **شغّل الملف:**
   ```
   CREATE_INTERNSHIPS_MIGRATION.bat
   ```

2. انتظر حتى يظهر "Migration Complete!"

3. شغّل Backend من جديد

---

### الطريقة 2: Manual Commands

#### خطوة 1: افتح Terminal في مجلد Backend
```bash
cd backend/GradLink.Api
```

#### خطوة 2: عمل Migration
```bash
dotnet ef migrations add AddInternshipsTable
```

#### خطوة 3: تحديث Database
```bash
dotnet ef database update
```

---

### الطريقة 3: من Visual Studio

1. افتح **Package Manager Console** من:
   ```
   Tools → NuGet Package Manager → Package Manager Console
   ```

2. تأكد إن Default project: **GradLink.Api**

3. نفذ الأوامر:
   ```powershell
   Add-Migration AddInternshipsTable
   Update-Database
   ```

---

## التحقق من نجاح العملية

### 1. تحقق من وجود الـ Migration Files
يجب أن تجد ملف جديد في:
```
backend/GradLink.Infrastructure/Migrations/
```
اسمه مثل: `20231126_AddInternshipsTable.cs`

### 2. تحقق من الـ Database
افتح SQL Server Management Studio أو Azure Data Studio:

```sql
SELECT * FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME = 'Internships'
```

يجب أن يظهر الـ table.

### 3. اختبر الـ API
افتح Swagger:
```
http://localhost:5000/swagger
```

ابحث عن **Internships** endpoints:
- GET /api/internships
- POST /api/internships
- PUT /api/internships/{id}
- DELETE /api/internships/{id}

---

## الأخطاء الشائعة وحلولها

### خطأ: "Build failed"
**الحل:**
```bash
cd backend
dotnet clean
dotnet build
```

### خطأ: "Unable to create migration"
**الحل:**
1. تأكد من تثبيت EF Tools:
```bash
dotnet tool install --global dotnet-ef
```

2. حدّث الأدوات:
```bash
dotnet tool update --global dotnet-ef
```

### خطأ: "A connection was successfully established with the server, but..."
**الحل:**
تحقق من connection string في `appsettings.json`

---

## بعد إصلاح المشكلة

### 1. شغّل Backend
```bash
cd backend/GradLink.Api
dotnet run
```

### 2. شغّل Frontend
```bash
npm run dev
```

### 3. اختبر الـ Internships Section
1. سجل دخول كـ Company
2. اذهب إلى Company Dashboard
3. اضغط على "Internships" من الـ sidebar
4. جرب إضافة Internship جديد

---

## Structure الـ Internships Table

```sql
CREATE TABLE Internships (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Requirements NVARCHAR(MAX),
    Skills NVARCHAR(MAX),
    Location NVARCHAR(MAX),
    Duration NVARCHAR(MAX),
    IsPaid BIT DEFAULT 0,
    Stipend DECIMAL(18,2),
    CompanyName NVARCHAR(MAX),
    Status NVARCHAR(50) DEFAULT 'Active',
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2,
    ExpiresAt DATETIME2,
    StartDate DATETIME2,
    EndDate DATETIME2,
    PostedById NVARCHAR(450) NOT NULL,
    
    FOREIGN KEY (PostedById) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
)
```

---

## الخلاصة

بعد عمل Migration:
✅ Internships table موجود في الـ database
✅ APIs شغالة في Backend
✅ Company Dashboard يقدر يضيف internships
✅ Career Page تعرض internships تلقائياً

---

تاريخ الإنشاء: 2025-11-26














