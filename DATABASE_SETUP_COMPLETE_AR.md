# دليل إعداد Database الكامل - خطوة بخطوة

## 🎯 الهدف
توصيل المشروع بـ SQL Server Database عشان يشتغل بشكل صحيح.

---

## الخطوة 1️⃣: تثبيت SQL Server

### الخيار 1: استخدام LocalDB (الأسهل - موصى به)

**LocalDB** بيجي مع Visual Studio تلقائياً!

#### التحقق من وجود LocalDB:
افتح PowerShell:
```powershell
sqllocaldb info
```

**إذا ظهر:**
```
MSSQLLocalDB
```
✅ **تمام! LocalDB موجود**

**إذا ظهر خطأ:**
❌ **محتاج تثبيت LocalDB**

---

### الخيار 2: تثبيت SQL Server Express (إذا LocalDB مش موجود)

#### خطوات التثبيت:

1. **حمّل SQL Server Express:**
   - اذهب إلى: https://www.microsoft.com/sql-server/sql-server-downloads
   - اختر **Express**
   - حمّل الملف

2. **ثبّت SQL Server:**
   - شغّل الملف
   - اختر **Basic Installation**
   - اضغط **Accept**
   - انتظر التثبيت...

3. **ثبّت SQL Server Management Studio (SSMS) - اختياري:**
   - حمّله من: https://aka.ms/ssmsfullsetup
   - هيساعدك تشوف الـ Database بصرياً

---

## الخطوة 2️⃣: تحديث Connection String

### افتح الملف:
```
backend/GradLink.Api/appsettings.json
```

### استخدم واحدة من Connection Strings دي:

#### الخيار 1: LocalDB (الأسهل)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GradLinkDb;Trusted_Connection=true;TrustServerCertificate=true"
  }
}
```

#### الخيار 2: SQL Server Express
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=GradLinkDb;Trusted_Connection=true;TrustServerCertificate=true"
  }
}
```

#### الخيار 3: SQL Server with Username/Password
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GradLinkDb;User Id=sa;Password=YourPassword123;TrustServerCertificate=true"
  }
}
```

✅ **احفظ الملف بعد التعديل**

---

## الخطوة 3️⃣: تثبيت EF Core Tools

افتح PowerShell:

```powershell
# تثبيت أدوات Entity Framework
dotnet tool install --global dotnet-ef

# أو تحديثها إذا كانت مثبتة
dotnet tool update --global dotnet-ef

# التحقق من التثبيت
dotnet ef --version
```

**يجب أن يظهر:**
```
Entity Framework Core .NET Command-line Tools
8.0.x
```

---

## الخطوة 4️⃣: إنشاء Database

### اذهب لمجلد المشروع:
```powershell
cd "D:\كل المهم\viteProject\Newfolder\backend\GradLink.Api"
```

### تنظيف وبناء المشروع:
```powershell
cd ..
dotnet clean
dotnet build
cd GradLink.Api
```

### شوف Migrations الموجودة:
```powershell
dotnet ef migrations list
```

**السيناريو A: لا يوجد migrations**
```powershell
# إنشاء migration أولي
dotnet ef migrations add InitialCreate

# إنشاء Database
dotnet ef database update
```

**السيناريو B: يوجد migrations بالفعل**
```powershell
# تطبيق Migrations على Database
dotnet ef database update
```

**السيناريو C: إضافة Internships (المطلوب حالياً)**
```powershell
# إضافة migration للـ Internships
dotnet ef migrations add AddInternshipsTable

# تطبيق على Database
dotnet ef database update
```

---

## الخطوة 5️⃣: التحقق من نجاح الإنشاء

### الطريقة 1: من PowerShell
```powershell
# عرض Migrations المطبقة
dotnet ef migrations list
```

**يجب أن ترى ✅ أمام كل migration**

### الطريقة 2: من SQL Server Management Studio (إذا مثبت)

1. افتح SSMS
2. اتصل بـ Server:
   - **LocalDB:** `(localdb)\mssqllocaldb`
   - **SQL Express:** `localhost\SQLEXPRESS`
3. ابحث عن Database: `GradLinkDb`
4. افتح **Tables**
5. يجب أن تجد:
   - `dbo.AspNetUsers`
   - `dbo.JobPostings`
   - `dbo.Internships` ✅
   - `dbo.Projects`
   - وجداول أخرى...

### الطريقة 3: من Azure Data Studio (بديل خفيف لـ SSMS)

1. حمّل من: https://aka.ms/azuredatastudio
2. اتصل بنفس الطريقة
3. استعرض الجداول

---

## الخطوة 6️⃣: تشغيل Backend

```powershell
cd backend\GradLink.Api
dotnet run
```

**انتظر حتى يظهر:**
```
Now listening on: http://localhost:5000
```

✅ **Backend شغال!**

---

## الخطوة 7️⃣: اختبار الاتصال

### افتح Swagger:
```
http://localhost:5000/swagger
```

### جرب أي endpoint:
1. اضغط على **POST /api/auth/register**
2. اضغط **Try it out**
3. املأ البيانات
4. اضغط **Execute**

**إذا رجع 200 أو 201:**
✅ **Database متصل وشغال!**

---

## 🔧 حل المشاكل الشائعة

### مشكلة 1: "A network-related error occurred"

**السبب:** SQL Server مش شغال

**الحل:**

#### لو بتستخدم LocalDB:
```powershell
# تشغيل LocalDB
sqllocaldb start MSSQLLocalDB

# التحقق
sqllocaldb info MSSQLLocalDB
```

#### لو بتستخدم SQL Server Express:
1. اضغط **Windows + R**
2. اكتب: `services.msc`
3. ابحث عن **SQL Server (SQLEXPRESS)**
4. اضغط كليك يمين → **Start**

---

### مشكلة 2: "Cannot open database"

**السبب:** Database مش موجود

**الحل:**
```powershell
cd backend\GradLink.Api
dotnet ef database update
```

---

### مشكلة 3: "Login failed for user"

**السبب:** Connection String غلط

**الحل:**
1. افتح `appsettings.json`
2. غيّر الـ Connection String لواحد من الأمثلة فوق
3. احفظ الملف
4. أعد تشغيل Backend

---

### مشكلة 4: "Table 'Internships' doesn't exist"

**السبب:** Migration للـ Internships معملش

**الحل:**
```powershell
cd backend\GradLink.Api
dotnet ef migrations add AddInternshipsTable
dotnet ef database update
```

---

## 🎯 Checklist النهائي

تأكد من:

- [ ] ✅ SQL Server مثبت (LocalDB أو Express)
- [ ] ✅ Connection String صحيح في `appsettings.json`
- [ ] ✅ dotnet-ef مثبت (`dotnet ef --version`)
- [ ] ✅ Migrations موجودة (`dotnet ef migrations list`)
- [ ] ✅ Database تم إنشاؤه (`dotnet ef database update`)
- [ ] ✅ Backend شغال (`http://localhost:5000/swagger`)
- [ ] ✅ Tables موجودة (يمكن التحقق من SSMS)
- [ ] ✅ Internships table موجود
- [ ] ✅ يمكن عمل Register/Login
- [ ] ✅ Company Dashboard شغال
- [ ] ✅ Internships section شغال

---

## 📋 الأوامر الكاملة (نسخ ولصق)

```powershell
# 1. تثبيت EF Tools
dotnet tool install --global dotnet-ef

# 2. اذهب للمشروع
cd "D:\كل المهم\viteProject\Newfolder\backend\GradLink.Api"

# 3. نظف وابني
cd ..
dotnet clean
dotnet build
cd GradLink.Api

# 4. إنشاء/تحديث Database
dotnet ef database update

# 5. إضافة Internships (إذا لزم الأمر)
dotnet ef migrations add AddInternshipsTable
dotnet ef database update

# 6. تشغيل Backend
dotnet run
```

---

## 🎨 Structure الـ Database النهائي

بعد ما تخلص كل الخطوات، الـ Database هيكون فيه:

### Tables الرئيسية:
- **AspNetUsers** - المستخدمين (Companies, Students)
- **AspNetRoles** - الأدوار
- **JobPostings** - الوظائف
- **Internships** - التدريبات ✅
- **Projects** - المشاريع
- **Resumes** - السير الذاتية
- **Matches** - المطابقات
- **Conversations** - المحادثات
- **ChatMessages** - الرسائل

---

## 📞 إذا احتجت مساعدة

### شوف الـ Logs:
عند تشغيل Backend، شوف الرسائل في Console.
إذا ظهر خطأ، انسخه وأرسله.

### اختبر Connection String:
```powershell
# من PowerShell
sqlcmd -S (localdb)\mssqllocaldb -Q "SELECT @@VERSION"
```

إذا نجح، معناه الاتصال سليم.

---

تاريخ الإنشاء: 2025-11-26
آخر تحديث: 2025-11-26

---

## 🚀 ما بعد الإعداد

بعد ما توصل الـ Database:

1. ✅ شغّل Backend
2. ✅ شغّل Frontend: `npm run dev`
3. ✅ سجل دخول كـ Company
4. ✅ جرب إضافة Jobs/Internships/Projects
5. ✅ روح Career Page وشوف البيانات

**كل حاجة هتشتغل تمام!** 🎉













