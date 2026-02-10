"""
Test script to verify PyPDF2 PDF extraction functionality
"""
import PyPDF2
import os

def extract_text_from_pdf(file_path):
    """Extract text from PDF using PyPDF2"""
    text = ""
    with open(file_path, "rb") as f:
        reader = PyPDF2.PdfReader(f)
        for page in reader.pages:
            text += page.extract_text()
    return text

def test_extraction():
    """Test PDF extraction"""
    print("=" * 50)
    print("Testing PyPDF2 PDF Extraction")
    print("=" * 50)
    
    # Test if PyPDF2 is installed
    try:
        import PyPDF2
        print("✅ PyPDF2 is installed")
        print(f"   Version: {PyPDF2.__version__ if hasattr(PyPDF2, '__version__') else 'Unknown'}")
    except ImportError:
        print("❌ PyPDF2 is NOT installed")
        print("   Run: pip install PyPDF2")
        return
    
    # Check uploads folder
    uploads_dir = "uploads"
    if not os.path.exists(uploads_dir):
        os.makedirs(uploads_dir)
        print(f"✅ Created '{uploads_dir}' directory")
    else:
        print(f"✅ '{uploads_dir}' directory exists")
    
    # Look for test PDF
    pdf_files = [f for f in os.listdir(uploads_dir) if f.endswith('.pdf')]
    
    if pdf_files:
        print(f"\n📄 Found {len(pdf_files)} PDF file(s):")
        for pdf_file in pdf_files:
            pdf_path = os.path.join(uploads_dir, pdf_file)
            print(f"\nTesting: {pdf_file}")
            try:
                text = extract_text_from_pdf(pdf_path)
                print(f"✅ Successfully extracted {len(text)} characters")
                print(f"   First 100 chars: {text[:100]}...")
            except Exception as e:
                print(f"❌ Error: {str(e)}")
    else:
        print(f"\n⚠️  No PDF files found in '{uploads_dir}' directory")
        print("   Place a test PDF file there to test extraction")
    
    print("\n" + "=" * 50)
    print("Test completed!")
    print("=" * 50)

if __name__ == "__main__":
    test_extraction()

