# كيفية الحصول على Access Token

## الطريقة 1: من Application Tab (موصى بها)

### الخطوات:
1. افتح Frontend: `http://localhost:5173`
2. سجل دخول كـ Company
3. اضغط **F12** لفتح Developer Tools
4. اضغط على تاب **Application** (في Chrome/Edge)
   - أو **Storage** (في Firefox)
5. في القائمة اليسار، افتح **Local Storage**
6. اضغط على `http://localhost:5173`
7. ابحث عن المفتاح: **accessToken**
8. انسخ القيمة الكاملة

---

## الطريقة 2: من Console

### الخطوات:
1. افتح Frontend: `http://localhost:5173`
2. سجل دخول كـ Company
3. اضغط **F12** → **Console**
4. اكتب:
   ```javascript
   localStorage.getItem('accessToken')
   ```
5. اضغط **Enter**
6. انسخ النص الذي يظهر (بدون علامات التنصيص)

---

## الطريقة 3: من Network Tab

### الخطوات:
1. افتح Frontend: `http://localhost:5173`
2. اضغط **F12** → **Network**
3. سجل دخول كـ Company
4. ابحث عن request اسمه **login** أو **signin**
5. اضغط عليه
6. اضغط على تاب **Response**
7. ابحث عن **accessToken** في الـ JSON
8. انسخ القيمة

---

## الطريقة 4: من Inspect Element

### الخطوات:
1. افتح Frontend وأنت مسجل دخول
2. اضغط **F12**
3. في Console، اكتب:
   ```javascript
   copy(localStorage.getItem('accessToken'))
   ```
4. الـ Token تم نسخه تلقائياً!
5. الصقه في Swagger

---

## استخدام Token في Swagger

### بعد ما تنسخ Token:

1. افتح: `http://localhost:5000/swagger`
2. اضغط **🔒 Authorize** (أعلى اليمين)
3. اكتب:
   ```
   Bearer YOUR_TOKEN_HERE
   ```
   مثال:
   ```
   Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI...
   ```
4. اضغط **Authorize**
5. اضغط **Close**

---

## إذا Token مش موجود

### السبب: مش مسجل دخول صح

### الحل:
1. سجل خروج من Dashboard
2. سجل دخول تاني
3. جرب تجيب Token مرة تانية

---

## الطريقة البديلة: استخدام Frontend مباشرة

**مش محتاج Token من Swagger!**

### بدلاً من Swagger:

1. استخدم Company Dashboard مباشرة
2. Frontend بيبعت Token تلقائياً
3. جرب تضيف Internship من Dashboard
4. لو اشتغل، معناه كل حاجة تمام!

---

## Screenshots للمساعدة

### Application Tab في Chrome:
```
Developer Tools → Application → Local Storage → http://localhost:5173
```

### Console:
```
Developer Tools → Console → Type: localStorage.getItem('accessToken')
```

---

## Troubleshooting

### Problem: "null" يظهر في Console
**الحل:** سجل دخول أولاً

### Problem: Token موجود بس مش شغال في Swagger
**الحل:** تأكد إنك كتبت `Bearer` قبل Token

### Problem: Token expired
**الحل:** سجل دخول تاني وجيب token جديد

---

تاريخ الإنشاء: 2025-11-26













