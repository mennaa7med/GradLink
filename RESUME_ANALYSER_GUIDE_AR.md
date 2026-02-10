# 📋 دليل تشغيل Resume Analyser - Flask API

## 📦 المتطلبات:
- ✅ Python 3.7+ (مثبت بالفعل: Python 3.14.0)
- ✅ Google Gemini API Key

---

## 🚀 خطوات التشغيل:

### **الطريقة الأولى - استخدام ملف .bat (الأسهل):**

1. **افتح المجلد الرئيسي للمشروع**
2. **اضغط دبل كليك على:**
   ```
   START_RESUME_ANALYSER.bat
   ```
3. **هيشتغل تلقائياً على:**
   ```
   http://localhost:5005
   ```

---

### **الطريقة الثانية - يدوياً من الـ Terminal:**

#### **1️⃣ افتح Terminal/CMD في مجلد المشروع:**
```bash
cd "D:\كل المهم\viteProject\Newfolder\Resume_Analyser_Using_Python-Main"
```

#### **2️⃣ نصب الـ Dependencies:**
```bash
python -m pip install -r requirements.txt
```

#### **3️⃣ شغل الـ Flask API:**
```bash
python main.py
```

---

## 🔑 إعداد Gemini API Key:

### **⚠️ مهم جداً:**
البرنامج محتاج **Gemini API Key** علشان يشتغل.

### **خطوات الحصول على API Key:**

1. **اذهب إلى:**
   ```
   https://makersuite.google.com/app/apikey
   ```
   أو
   ```
   https://ai.google.dev/
   ```

2. **سجل دخول بحساب Google**

3. **اضغط "Create API Key"**

4. **انسخ الـ API Key**

5. **افتح ملف `.env` في مجلد:**
   ```
   Resume_Analyser_Using_Python-Main\.env
   ```

6. **ضع الـ API Key:**
   ```
   GEMINI_API_KEY=AIzaSyXXXXXXXXXXXXXXXXXXXXXXXXX
   ```

### **📝 إذا لم يكن ملف `.env` موجود:**
انسخ `.env.example` واعمله rename لـ `.env`:
```bash
copy .env.example .env
```
ثم افتحه وضع الـ API Key الخاص بك.

---

## ✅ التحقق من التشغيل:

### **1. افتح المتصفح واذهب إلى:**
```
http://localhost:5005
```

### **2. المفروض تشوف رسالة:**
```json
{
  "status": "success",
  "message": "Resume Analyzer API is running",
  "endpoints": {
    "analyze": "/api/analyze (POST)",
    "health": "/ (GET)"
  }
}
```

---

## 📡 API Endpoints:

### **1. Health Check (GET):**
```
GET http://localhost:5005/
```

### **2. Analyze Resume (POST):**
```
POST http://localhost:5005/api/analyze

Form Data:
- resume: PDF file
- job_description: Text
```

**مثال باستخدام cURL:**
```bash
curl -X POST http://localhost:5005/api/analyze \
  -F "resume=@resume.pdf" \
  -F "job_description=Looking for Python developer with Flask experience"
```

---

## 📦 Dependencies المثبتة:

- **Flask** 3.0.0 - Web framework
- **flask-cors** 4.0.0 - CORS support
- **PyMuPDF** 1.23.8 - PDF text extraction
- **google-generativeai** 0.3.2 - Gemini AI integration

---

## 🛠️ استكشاف الأخطاء:

### **خطأ: "GEMINI_API_KEY not found"**
**الحل:**
- تأكد من وجود ملف `.env`
- تأكد من أن الـ API Key موجود فيه
- أعد تشغيل البرنامج

### **خطأ: "pip is not recognized"**
**الحل:**
استخدم `python -m pip` بدلاً من `pip` مباشرة:
```bash
python -m pip install -r requirements.txt
```

### **خطأ: "Module not found"**
**الحل:**
نصب الـ dependency المفقود:
```bash
python -m pip install flask flask-cors PyMuPDF google-generativeai
```

### **الـ Port 5005 مستخدم بالفعل:**
**الحل:**
غير الـ port في `main.py` السطر الأخير:
```python
app.run(debug=True, host='0.0.0.0', port=5006)  # غير 5005 لـ 5006
```

---

## 🔗 الربط مع Frontend:

الـ API جاهز للربط مع الـ Frontend على:
```
http://localhost:5173
```

الـ CORS مفعل بالفعل لجميع الـ origins.

---

## ⏹️ إيقاف السيرفر:

اضغط:
```
Ctrl + C
```
في نافذة الـ Terminal

---

## 📝 ملاحظات:

- الـ API يشتغل على **port 5005**
- الـ .NET Backend يشتغل على **port 5000**
- الـ Frontend يشتغل على **port 5173**
- الـ uploads folder يتم إنشاؤه تلقائياً
- الملفات المرفوعة يتم حذفها تلقائياً بعد التحليل

---

## 🎯 للتشغيل الكامل (All Services):

استخدم:
```
START_ALL.bat
```

سيقوم بتشغيل:
1. ✅ Backend (.NET) - Port 5000
2. ✅ Frontend (Vite) - Port 5173
3. ✅ Resume Analyser (Flask) - Port 5005

---

**✅ تم! الآن Resume Analyser جاهز للعمل** 🚀

