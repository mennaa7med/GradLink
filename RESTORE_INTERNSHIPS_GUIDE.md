# إعادة Internships Section

## تم إزالة Internships من Company Dashboard

تم تعطيل سكشن Internships من:
- ✅ Sidebar (القائمة الجانبية)
- ✅ Content Router

---

## لإعادة تفعيل Internships مرة أخرى

### في ملف: `src/components/CompanyDashboard/Sidebar.jsx`

ابحث عن السطر:
```javascript
// { id: 'internships', label: 'Internships', icon: '🎓' }, // Removed
```

احذف `//` في البداية ليصبح:
```javascript
{ id: 'internships', label: 'Internships', icon: '🎓' },
```

---

### في ملف: `src/components/CompanyDashboard/Content.jsx`

#### 1. في الـ imports، ابحث عن:
```javascript
// import Internships from './Pages/Internships'; // Removed
```

احذف `//` ليصبح:
```javascript
import Internships from './Pages/Internships';
```

#### 2. في renderPage function، ابحث عن:
```javascript
// case 'internships':
//   return <Internships />; // Removed
```

احذف `//` من الـ 2 أسطر ليصبح:
```javascript
case 'internships':
  return <Internships />;
```

---

## الملفات المحفوظة (لم يتم حذفها)

جميع الملفات التالية لا تزال موجودة:

### Backend:
- ✅ `backend/GradLink.Domain/Entities/Internship.cs`
- ✅ `backend/GradLink.Application/DTOs/Internships/*`
- ✅ `backend/GradLink.Api/Controllers/InternshipsController.cs`

### Frontend:
- ✅ `src/components/CompanyDashboard/Pages/Internships.jsx`
- ✅ `src/api/internships.js`

### Database:
- ✅ Internships table في Database

---

## إعادة التفعيل السريع

إذا أردت إعادة تفعيل Internships:

1. افتح `src/components/CompanyDashboard/Sidebar.jsx`
2. ابحث عن `// { id: 'internships'`
3. احذف `//` من بداية السطر
4. احفظ الملف

5. افتح `src/components/CompanyDashboard/Content.jsx`
6. ابحث عن `// import Internships`
7. احذف `//` من الـ 2 أماكن (import و case)
8. احفظ الملف

9. أعد تشغيل Frontend (إن لزم الأمر)

✅ **Internships سيعود للظهور في Dashboard**

---

تاريخ الإزالة: 2025-11-26













