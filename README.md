# Smart Resume Screening System

A complete frontend React application for smart resume screening with PDF text extraction and intelligent matching algorithms.

## Features

- **Upload Resumes**: Support for PDF, DOC, and DOCX files with text extraction
- **Job Description Analysis**: Enter job requirements and required skills
- **Intelligent Matching**: Client-side algorithm for resume-job matching with scoring
- **Interactive Results**: Visual progress bars, skill analysis, and application submission
- **Responsive Design**: Clean, modern UI that works on all devices

## Installation

### Prerequisites
- Node.js (v14 or higher)
- npm or yarn

### Setup

1. **Install Dependencies**
   ```bash
   npm install pdfjs-dist
   ```

2. **Start Development Server**
   ```bash
   npm run dev
   ```

3. **Build for Production**
   ```bash
   npm run build
   ```

## Usage Flow

### 1. Upload Resumes (`/upload-resume`)
- Choose multiple PDF, DOC, or DOCX files
- View selected files with names and sizes
- Extract text content from PDFs automatically
- Navigate to job description page

### 2. Job Description (`/job-description`)
- Enter detailed job description
- Add required skills (comma-separated)
- Start the screening process

### 3. Resume Screening (`/resume-screener`)
- View all uploaded resumes with matching scores
- Filter resumes by minimum score threshold
- Submit applications for high-scoring resumes (≥85%)
- Get improvement suggestions for low-scoring resumes

## Technical Implementation

### PDF Text Extraction
- Uses `pdfjs-dist` library for client-side PDF processing
- Extracts text page by page for comprehensive content analysis
- Handles errors gracefully with user-friendly messages

### Matching Algorithm
- **Skill-based matching**: Counts required skills found in resume
- **Keyword overlap**: Falls back to job description keyword matching
- **Score calculation**: (matched skills / total required skills) × 100
- **85% threshold**: Applications can only be submitted for scores ≥85%

### Data Storage
- Uses localStorage for persistence between pages:
  - `uploadedResumes`: Array of extracted resume data
  - `jobDescription`: Job description text
  - `jobRequiredSkills`: Array of required skills
  - `submittedApplications`: Track submitted applications

## File Structure

```
src/
├── components/
│   ├── UploadResume.jsx          # File upload and PDF extraction
│   ├── UploadResume.css
│   ├── JobDescription.jsx        # Job description and skills input
│   ├── JobDescription.css
│   ├── ResumeScreener.jsx        # Matching results and scoring
│   ├── ResumeScreener.css
│   └── ...
├── App.jsx                       # Routing and main app
└── ...
```

## API Reference

### Components

#### UploadResume
- `handleFileSelect()`: Process selected files
- `extractTextFromPDF()`: Extract text from PDF files
- `handleNext()`: Save extracted data and navigate

#### JobDescription
- `handleStartScreening()`: Save job data and start screening
- `handleBack()`: Navigate back to upload page

#### ResumeScreener
- `calculateMatchScore()`: Compute resume-job match percentage
- `getMissingSkills()`: Identify missing required skills
- `submitApplication()`: Submit application for high-scoring resume

## Error Handling

- PDF extraction failures are handled gracefully
- User-friendly error messages for all operations
- Confirmation dialogs for destructive actions
- Validation for required fields

## Browser Support

- Chrome/Edge (recommended for PDF processing)
- Firefox
- Safari
- Mobile browsers

## Development Notes

- All matching logic runs client-side (no external APIs)
- PDF processing requires modern browser with File API support
- localStorage is used for data persistence
- Responsive design with mobile-first approach

## Future Enhancements

- DOCX text extraction using `mammoth` library
- Advanced NLP for better matching
- Export results to CSV/PDF
- Batch processing for large file sets
- Real-time collaboration features