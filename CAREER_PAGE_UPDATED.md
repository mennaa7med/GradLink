# Career Page - التصميم المحدّث

## التصميم الجديد

تم إعادة تصميم صفحة Career بشكل كامل لتكون أبسط وأكثر تنظيماً.

## المميزات الجديدة

### 1. تصميم Accordion Style
- قائمة عمودية بجميع الشركات
- عند الضغط على شركة، تظهر الأقسام (Jobs, Internships, Projects)
- عند الضغط على قسم، تظهر جميع الفرص في grid منظم

### 2. التفاعل المتدرج (Nested Accordion)

**المستوى الأول - الشركات:**
- عرض اسم الشركة مع أيقونة 🏢
- عرض إحصائيات سريعة (عدد الوظائف، التدريبات، المشاريع)
- أيقونة سهم لتوضيح حالة التوسيع ▶/▼

**المستوى الثاني - الأقسام:**
- 💼 Jobs - جميع الوظائف
- 🎓 Internships - جميع التدريبات  
- 📁 Projects - جميع المشاريع

**المستوى الثالث - التفاصيل:**
- عرض الفرص في grid responsive
- كل بطاقة تحتوي على:
  - العنوان والوصف
  - التفاصيل (الموقع، النوع، المدة، الراتب)
  - المهارات المطلوبة

### 3. الألوان والتصميم

**Color Palette:**
- Primary Gradient: `#667eea` → `#764ba2`
- Background: نفس الـ gradient
- Cards: أبيض نظيف
- Hover Effects: لون أزرق فاتح

**Animations:**
- Smooth expand/collapse للـ accordions
- Hover effects على البطاقات
- Scale animations عند الدخول

### 4. Responsive Design

**Desktop (> 768px):**
- Grid بـ 2-3 بطاقات في الصف
- Padding كامل
- أحجام خط كبيرة

**Tablet (480px - 768px):**
- Grid بـ 1-2 بطاقة في الصف
- Padding متوسط
- أحجام خط متوسطة

**Mobile (< 480px):**
- Grid ببطاقة واحدة في الصف
- Padding صغير
- إحصائيات الشركة vertical
- أحجام خط صغيرة

## هيكل الصفحة

```
Career Page
│
├── Header
│   ├── Title: "Career Opportunities"
│   └── Subtitle
│
└── Companies List (Accordion)
    │
    ├── Company 1 [Expandable]
    │   ├── Company Info (Name + Stats)
    │   └── Sections [When Expanded]
    │       ├── Jobs Section [Expandable]
    │       │   └── Jobs Grid (Cards)
    │       ├── Internships Section [Expandable]
    │       │   └── Internships Grid (Cards)
    │       └── Projects Section [Expandable]
    │           └── Projects Grid (Cards)
    │
    ├── Company 2 [Expandable]
    │   └── ...
    │
    └── Company N [Expandable]
        └── ...
```

## User Flow (تدفق المستخدم)

1. **المستخدم يفتح صفحة Career**
   - يرى قائمة بجميع الشركات مغلقة

2. **يضغط على شركة**
   - تتوسع الشركة وتظهر 3 أقسام (Jobs, Internships, Projects)
   - الأقسام الأخرى تنطوي تلقائياً

3. **يضغط على قسم (مثلاً Jobs)**
   - يتوسع قسم Jobs ويظهر grid بجميع الوظائف
   - يمكنه قراءة تفاصيل كل وظيفة

4. **يضغط على قسم آخر (مثلاً Internships)**
   - ينطوي قسم Jobs تلقائياً
   - يتوسع قسم Internships

5. **يضغط على شركة أخرى**
   - تنطوي الشركة الأولى تلقائياً
   - تتوسع الشركة الجديدة

## الكود الرئيسي

### State Management
```javascript
const [companies, setCompanies] = useState([]);
const [expandedCompany, setExpandedCompany] = useState(null);
const [expandedSection, setExpandedSection] = useState(null);
```

### Toggle Functions
```javascript
const toggleCompany = (companyName) => {
  if (expandedCompany === companyName) {
    setExpandedCompany(null);
    setExpandedSection(null);
  } else {
    setExpandedCompany(companyName);
    setExpandedSection(null);
  }
};

const toggleSection = (section) => {
  if (expandedSection === section) {
    setExpandedSection(null);
  } else {
    setExpandedSection(section);
  }
};
```

## CSS Classes الرئيسية

### Layout
- `.career-page` - الصفحة الرئيسية
- `.career-container` - الحاوية الرئيسية
- `.career-companies-container` - حاوية الشركات

### Company Accordion
- `.company-accordion` - accordion الشركة
- `.company-header` - رأس الشركة (قابل للضغط)
- `.company-sections` - أقسام الشركة (قابلة للتوسيع)

### Section Accordion
- `.section-wrapper` - wrapper للقسم
- `.section-header` - رأس القسم (قابل للضغط)
- `.section-content` - محتوى القسم (قابل للتوسيع)

### Cards
- `.career-items-grid` - grid الفرص
- `.career-item-card` - بطاقة الفرصة
- `.career-item-title` - عنوان الفرصة
- `.career-badge` - badges للتفاصيل
- `.career-skill-badge` - badges للمهارات

## الحالات الخاصة

### Loading State
- عرض loader دوار مع رسالة "Loading opportunities..."

### Empty State
- عرض أيقونة 📭 مع رسالة "No Companies Found"

### No Items in Section
- عرض رسالة "No {type} available"

## المميزات التقنية

### Animations (Framer Motion)
- `initial`, `animate`, `exit` للـ accordions
- `AnimatePresence` للـ smooth transitions
- `whileHover`, `whileTap` للـ buttons

### Performance
- Lazy rendering للـ sections
- Only expanded content is rendered
- Smooth animations بدون lag

### Accessibility
- Semantic HTML (buttons للـ clickable elements)
- Clear visual feedback
- Keyboard navigation support

## الاختبار

### سيناريوهات الاختبار:

1. ✅ فتح وإغلاق شركة
2. ✅ فتح وإغلاق قسم
3. ✅ فتح شركتين (تنطوي الأولى تلقائياً)
4. ✅ فتح قسمين (ينطوي الأول تلقائياً)
5. ✅ عرض البطاقات بشكل صحيح
6. ✅ Responsive على جميع الشاشات
7. ✅ Animations سلسة
8. ✅ Loading state
9. ✅ Empty state

## الملفات المحدثة

1. ✅ `src/pages/Career.jsx` - الكود الرئيسي
2. ✅ `src/pages/Career.css` - التنسيقات

## الخلاصة

التصميم الجديد:
- ✅ أبسط وأوضح
- ✅ منظم بشكل accordion
- ✅ سهل الاستخدام
- ✅ responsive تماماً
- ✅ animations جميلة
- ✅ يستخدم نفس color palette الموقع

---

تم التحديث: 2025-11-26














