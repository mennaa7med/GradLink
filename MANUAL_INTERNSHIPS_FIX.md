# إصلاح مشكلة Internships - دليل كامل

## المشكلة الحالية:
```
Server error. The Internships table might not exist in database.
```

## الحل الكامل (خطوة بخطوة)

---

## الطريقة 1: استخدام Batch File (موصى بها)

### افتح PowerShell كـ Administrator:
1. اضغط Windows + X
2. اختر "Windows PowerShell (Admin)"
3. اذهب لمجلد المشروع:
```powershell
cd "D:\كل المهم\viteProject\Newfolder"
```

4. شغّل الملف:
```powershell
.\COMPLETE_INTERNSHIPS_FIX.bat
```

---

## الطريقة 2: Manual Steps (خطوة بخطوة)

### الخطوة 1: تثبيت/تحديث EF Tools

```powershell
# تحديث EF Tools
dotnet tool update --global dotnet-ef

# التحقق من التثبيت
dotnet ef --version
```

يجب أن يظهر:
```
Entity Framework Core .NET Command-line Tools
8.0.x
```

---

### الخطوة 2: اذهب لمجلد API

```powershell
cd "D:\كل المهم\viteProject\Newfolder\backend\GradLink.Api"
```

---

### الخطوة 3: تنظيف وبناء المشروع

```powershell
# ارجع لمجلد Backend
cd ..

# تنظيف
dotnet clean

# بناء
dotnet build

# ارجع لـ API
cd GradLink.Api
```

---

### الخطوة 4: شوف Migrations الموجودة

```powershell
dotnet ef migrations list
```

**النتيجة المتوقعة:**
```
20231101_InitialCreate
20231115_AddJobPostings
... (migrations أخرى)
```

**إذا شفت migration اسمه فيه "Internship":**
- معناه الـ migration موجود بالفعل
- روح للخطوة 5 مباشرة

**إذا مفيش migration اسمه فيه "Internship":**
- كمل الخطوات العادية

---

### الخطوة 5: عمل Migration جديد

```powershell
dotnet ef migrations add AddInternshipsTable --verbose
```

**النتيجة المتوقعة:**
```
Build started...
Build succeeded.
An operation was scaffolded that may result in the loss of data...
Done. To undo this action, use 'ef migrations remove'
```

**إذا ظهر خطأ:**
```
A migration named 'AddInternshipsTable' already exists
```

**الحل:**
```powershell
# استخدم اسم مختلف
dotnet ef migrations add AddInternshipsTableV2 --verbose
```

---

### الخطوة 6: تطبيق Migration على Database

```powershell
dotnet ef database update --verbose
```

**النتيجة المتوقعة:**
```
Build started...
Build succeeded.
Applying migration '20231126_AddInternshipsTable'.
Done.
```

**إذا ظهر خطأ:**

#### خطأ 1: "A network-related error occurred"
```
المشكلة: SQL Server مش شغال أو Connection String غلط
```

**الحل:**
1. افتح SQL Server Configuration Manager
2. تأكد إن SQL Server (MSSQLSERVER) شغال
3. أو تأكد من Connection String في `appsettings.json`

#### خطأ 2: "Cannot open database"
```
المشكلة: Database مش موجود
```

**الحل:**
```powershell
# عمل Database من جديد
dotnet ef database drop --force
dotnet ef database update
```

#### خطأ 3: "Login failed for user"
```
المشكلة: مشكلة في Authentication
```

**الحل:**
افتح `appsettings.json` وتأكد من الـ connection string:
```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GradLinkDb;Trusted_Connection=true;TrustServerCertificate=true"
```

---

### الخطوة 7: التحقق من نجاح العملية

#### طريقة 1: من SQL Server Management Studio

1. افتح SSMS
2. اتصل بالـ Server: `(localdb)\mssqllocaldb`
3. افتح Database: `GradLinkDb`
4. افتح `Tables`
5. لازم تلاقي: `dbo.Internships`

#### طريقة 2: من PowerShell

```powershell
# عرض جميع Migrations المطبقة
dotnet ef migrations list
```

لازم تشوف ✅ علامة صح قدام `AddInternshipsTable`

---

### الخطوة 8: تشغيل Backend

```powershell
dotnet run
```

انتظر حتى يظهر:
```
Now listening on: http://localhost:5000
```

---

### الخطوة 9: اختبار من Swagger

1. افتح المتصفح
2. اذهب لـ: `http://localhost:5000/swagger`
3. ابحث عن **Internships**
4. جرب **GET /api/internships/my**
5. اضغط **Try it out** → **Execute**
6. لازم يرجع `200 OK` مع `[]`

---

### الخطوة 10: اختبار من Frontend

1. شغّل Frontend في terminal جديد:
```powershell
npm run dev
```

2. سجل دخول كـ Company

3. روح **Company Dashboard** → **Internships**

4. لازم الصفحة **تفتح بدون errors** ✅

5. جرب إضافة Internship جديد

---

## الأخطاء الشائعة وحلولها

### 1. "Build failed"
```powershell
cd backend
dotnet clean
dotnet restore
dotnet build
```

### 2. "Unable to create an object of type 'AppDbContext'"
**المشكلة:** الـ connection string مش موجود أو غلط

**الحل:**
تحقق من `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GradLinkDb;Trusted_Connection=true;TrustServerCertificate=true"
  }
}
```

### 3. "The specified table doesn't exist"
**المشكلة:** Migration معملش

**الحل:**
```powershell
# احذف كل Migrations وابدأ من جديد
cd backend/GradLink.Infrastructure/Migrations
# احذف الملفات يدوياً ماعدا InitialCreate

# ارجع لـ API
cd ../../GradLink.Api

# عمل migration جديد
dotnet ef migrations add AddAllTables
dotnet ef database update
```

---

## التحقق النهائي

✅ **Checklist:**
- [ ] dotnet-ef مثبت (`dotnet ef --version`)
- [ ] Project يعمل build (`dotnet build`)
- [ ] Migration موجود في Migrations folder
- [ ] Migration مطبق (`dotnet ef migrations list` يظهر ✅)
- [ ] Backend شغال (`http://localhost:5000/swagger`)
- [ ] Internships endpoints موجودة في Swagger
- [ ] GET /api/internships/my يرجع 200 OK
- [ ] Frontend يفتح Internships page بدون errors
- [ ] يمكن إضافة Internship جديد

---

## Structure الـ Internships Table المتوقع

```sql
CREATE TABLE [dbo].[Internships] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Title] NVARCHAR(200) NOT NULL,
    [Description] NVARCHAR(MAX) NOT NULL,
    [Requirements] NVARCHAR(MAX) NULL,
    [Skills] NVARCHAR(MAX) NULL,
    [Location] NVARCHAR(MAX) NULL,
    [Duration] NVARCHAR(MAX) NULL,
    [IsPaid] BIT NOT NULL DEFAULT 0,
    [Stipend] DECIMAL(18,2) NULL,
    [CompanyName] NVARCHAR(MAX) NULL,
    [Status] NVARCHAR(50) NOT NULL DEFAULT 'Active',
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [UpdatedAt] DATETIME2 NULL,
    [ExpiresAt] DATETIME2 NULL,
    [StartDate] DATETIME2 NULL,
    [EndDate] DATETIME2 NULL,
    [PostedById] NVARCHAR(450) NOT NULL,
    CONSTRAINT [FK_Internships_AspNetUsers_PostedById] 
        FOREIGN KEY ([PostedById]) REFERENCES [AspNetUsers]([Id]) ON DELETE CASCADE
)
```

---

## إذا فشل كل شيء - الحل الأخير

### Reset كامل للـ Database:

```powershell
cd backend/GradLink.Api

# حذف Database
dotnet ef database drop --force

# حذف كل Migrations
Remove-Item ../GradLink.Infrastructure/Migrations/* -Exclude "_*"

# عمل Initial Migration جديد
dotnet ef migrations add InitialCreate

# إنشاء Database من جديد
dotnet ef database update
```

⚠️ **تحذير:** هذا سيحذف جميع البيانات!

---

تاريخ الإنشاء: 2025-11-26













