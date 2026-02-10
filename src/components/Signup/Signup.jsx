import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faGoogle, faApple } from '@fortawesome/free-brands-svg-icons';
import graduatesImg from '../../assets/images/graduates.png';
import logo from '../../assets/images/site.png';

import './Signup.css';
import { useAuth } from '../../contexts/AuthContext';

function Signup() {
  // Basic Info (All users)
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [phone, setPhone] = useState('');
  const [userType, setUserType] = useState('Student');
  
  // Student-specific fields
  const [university, setUniversity] = useState('');
  const [faculty, setFaculty] = useState('');
  const [major, setMajor] = useState('');
  const [academicYear, setAcademicYear] = useState('');
  const [graduationYear, setGraduationYear] = useState('');
  
  // Company-specific fields
  const [companyName, setCompanyName] = useState('');
  const [industry, setIndustry] = useState('');
  const [website, setWebsite] = useState('');
  const [companySize, setCompanySize] = useState('');
  const [companyDescription, setCompanyDescription] = useState('');
  
  // Sponsor-specific fields
  const [initialBudget, setInitialBudget] = useState('');
  
  // Mentor-specific fields
  const [specialization, setSpecialization] = useState('');
  const [experienceYears, setExperienceYears] = useState('');
  const [jobTitle, setJobTitle] = useState('');
  
  // Form state
  const [agreeTerms, setAgreeTerms] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [step, setStep] = useState(1);
  
  const navigate = useNavigate();
  const { register } = useAuth();

  // Options
  const industries = [
    'Technology', 'Finance', 'Healthcare', 'Education', 
    'Manufacturing', 'Retail', 'Real Estate', 'Consulting',
    'Marketing', 'Media', 'Telecommunications', 'Energy', 'Other'
  ];

  const companySizes = [
    { value: '1-10', label: '1-10 employees' },
    { value: '11-50', label: '11-50 employees' },
    { value: '51-200', label: '51-200 employees' },
    { value: '201-500', label: '201-500 employees' },
    { value: '500+', label: '500+ employees' }
  ];

  const academicYears = ['1st Year', '2nd Year', '3rd Year', '4th Year', '5th Year', 'Graduate'];
  const currentYear = new Date().getFullYear();
  const graduationYears = Array.from({ length: 6 }, (_, i) => currentYear + i);

  const specializations = [
    'Web Development', 'Mobile Development', 'AI/Machine Learning',
    'Data Science', 'Cybersecurity', 'Cloud Computing', 'DevOps',
    'UI/UX Design', 'Project Management', 'Business Development', 'Other'
  ];

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setSuccess('');
    
    if (!(name && email && password && agreeTerms)) {
      setError('Please fill all required fields and agree to the terms.');
      return;
    }

    if (password.length < 6) {
      setError('Password must be at least 6 characters long.');
      return;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
      setError('Please enter a valid email address.');
      return;
    }

    if ((userType === 'Company' || userType === 'Sponsor') && !companyName.trim()) {
      setError('Please enter your company name.');
      return;
    }

    if (userType === 'Student' && !university.trim()) {
      setError('Please enter your university.');
      return;
    }

    setIsLoading(true);
    try {
      const userData = {
        email,
        password,
        fullName: name,
        phone,
        role: userType,
        university: userType === 'Student' ? university : null,
        faculty: userType === 'Student' ? faculty : null,
        major: userType === 'Student' ? major : null,
        academicYear: userType === 'Student' ? academicYear : null,
        graduationYear: userType === 'Student' ? parseInt(graduationYear) || null : null,
        companyName: (userType === 'Company' || userType === 'Sponsor') ? companyName : null,
        industry: (userType === 'Company' || userType === 'Sponsor') ? industry : null,
        companyWebsite: (userType === 'Company' || userType === 'Sponsor') ? website : null,
        companySize: userType === 'Company' ? companySize : null,
        companyDescription: (userType === 'Company' || userType === 'Sponsor') ? companyDescription : null,
        specialization: userType === 'Mentor' ? specialization : null,
        experienceYears: userType === 'Mentor' ? parseInt(experienceYears) || null : null,
        jobTitle: userType === 'Mentor' ? jobTitle : null,
      };

      if (userType === 'Sponsor') {
        const mockUser = {
          id: 'demo-sponsor-' + Date.now(),
          email: email,
          fullName: name,
          roles: ['Sponsor'],
          companyName: companyName
        };
        
        localStorage.setItem('token', 'demo-token-' + Date.now());
        localStorage.setItem('user', JSON.stringify(mockUser));
        localStorage.setItem('userRole', 'Sponsor');
        localStorage.setItem('companyName', companyName);
        
        setSuccess('Sponsor account created! Redirecting to dashboard...');
        setTimeout(() => {
          navigate('/sponsor-dashboard');
          window.location.reload();
        }, 1500);
      } else {
        const response = await register(email, password, name, phone, userType, companyName, userData);
        
        if (response?.requiresVerification) {
          setSuccess('Account created! 📧 Please verify your email with the code we sent.');
          setTimeout(() => {
            navigate('/verify-otp', { state: { email: email } });
          }, 1500);
        } else {
          setSuccess('Account created successfully! Redirecting...');
          setTimeout(() => {
            if (userType === 'Company') {
              navigate('/company-dashboard-new');
            } else if (userType === 'Mentor') {
              navigate('/mentor-dashboard');
            } else {
              navigate('/dashboard');
            }
          }, 1500);
        }
      }
    } catch (err) {
      console.error('Registration error:', err);
      let errorMessage = 'Registration failed. Please try again.';
      
      if (err.response?.data?.errors) {
        if (Array.isArray(err.response.data.errors)) {
          errorMessage = err.response.data.errors.join(', ');
        } else if (typeof err.response.data.errors === 'object') {
          errorMessage = Object.values(err.response.data.errors).flat().join(', ');
        }
      } else if (err.response?.data?.error) {
        errorMessage = err.response.data.error;
      } else if (err.message) {
        errorMessage = err.message;
      }
      
      setError(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  const handleGoogleSignup = () => {
    console.log('Google Signup Attempt');
  };

  const handleAppleSignup = () => {
    console.log('Apple Signup Attempt');
  };

  const nextStep = () => {
    if (step === 1) {
      if (!name || !email || !password) {
        setError('Please fill all required fields.');
        return;
      }
      if (password.length < 6) {
        setError('Password must be at least 6 characters.');
        return;
      }
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(email)) {
        setError('Please enter a valid email address.');
        return;
      }
      setError('');
    }
    setStep(step + 1);
  };

  const prevStep = () => {
    setStep(step - 1);
    setError('');
  };

  const renderUserTypeFields = () => {
    switch (userType) {
      case 'Student':
        return (
          <>
            <div className="form-section-title">
              <span className="section-icon">🎓</span>
              Academic Information
            </div>
            <div className="input-group">
              <input
                type="text"
                value={university}
                onChange={(e) => setUniversity(e.target.value)}
                placeholder="University Name *"
                required
              />
            </div>
            <div className="input-row">
              <div className="input-group half">
                <input
                  type="text"
                  value={faculty}
                  onChange={(e) => setFaculty(e.target.value)}
                  placeholder="Faculty/College"
                />
              </div>
              <div className="input-group half">
                <input
                  type="text"
                  value={major}
                  onChange={(e) => setMajor(e.target.value)}
                  placeholder="Major/Department"
                />
              </div>
            </div>
            <div className="input-row">
              <div className="input-group half">
                <select
                  value={academicYear}
                  onChange={(e) => setAcademicYear(e.target.value)}
                  className="user-type-select"
                >
                  <option value="">Academic Year</option>
                  {academicYears.map(year => (
                    <option key={year} value={year}>{year}</option>
                  ))}
                </select>
              </div>
              <div className="input-group half">
                <select
                  value={graduationYear}
                  onChange={(e) => setGraduationYear(e.target.value)}
                  className="user-type-select"
                >
                  <option value="">Expected Graduation</option>
                  {graduationYears.map(year => (
                    <option key={year} value={year}>{year}</option>
                  ))}
                </select>
              </div>
            </div>
          </>
        );

      case 'Company':
        return (
          <>
            <div className="form-section-title">
              <span className="section-icon">🏢</span>
              Company Information
            </div>
            <div className="input-group">
              <input
                type="text"
                value={companyName}
                onChange={(e) => setCompanyName(e.target.value)}
                placeholder="Company Name *"
                required
              />
            </div>
            <div className="input-row">
              <div className="input-group half">
                <select
                  value={industry}
                  onChange={(e) => setIndustry(e.target.value)}
                  className="user-type-select"
                >
                  <option value="">Select Industry</option>
                  {industries.map(ind => (
                    <option key={ind} value={ind}>{ind}</option>
                  ))}
                </select>
              </div>
              <div className="input-group half">
                <select
                  value={companySize}
                  onChange={(e) => setCompanySize(e.target.value)}
                  className="user-type-select"
                >
                  <option value="">Company Size</option>
                  {companySizes.map(size => (
                    <option key={size.value} value={size.value}>{size.label}</option>
                  ))}
                </select>
              </div>
            </div>
            <div className="input-group">
              <input
                type="url"
                value={website}
                onChange={(e) => setWebsite(e.target.value)}
                placeholder="Company Website (optional)"
              />
            </div>
          </>
        );

      case 'Sponsor':
        return (
          <>
            <div className="form-section-title sponsor-section">
              <span className="section-icon">💰</span>
              Sponsor Information
            </div>
            <div className="sponsor-info-box">
              <span className="info-icon">💡</span>
              <p>As a sponsor, you can fund and support student graduation projects.</p>
            </div>
            <div className="input-group">
              <input
                type="text"
                value={companyName}
                onChange={(e) => setCompanyName(e.target.value)}
                placeholder="Company/Organization Name *"
                required
              />
            </div>
            <div className="input-row">
              <div className="input-group half">
                <select
                  value={industry}
                  onChange={(e) => setIndustry(e.target.value)}
                  className="user-type-select"
                >
                  <option value="">Select Industry</option>
                  {industries.map(ind => (
                    <option key={ind} value={ind}>{ind}</option>
                  ))}
                </select>
              </div>
              <div className="input-group half">
                <input
                  type="number"
                  value={initialBudget}
                  onChange={(e) => setInitialBudget(e.target.value)}
                  placeholder="Initial Budget ($)"
                  min="0"
                  step="100"
                />
              </div>
            </div>
          </>
        );

      case 'Mentor':
        return (
          <>
            <div className="form-section-title mentor-section">
              <span className="section-icon">👨‍🏫</span>
              Mentor Information
            </div>
            <div className="mentor-info-box">
              <span className="info-icon">🌟</span>
              <p>As a mentor, you can guide students through their graduation projects.</p>
            </div>
            <div className="input-group">
              <input
                type="text"
                value={jobTitle}
                onChange={(e) => setJobTitle(e.target.value)}
                placeholder="Current Job Title *"
                required
              />
            </div>
            <div className="input-row">
              <div className="input-group half">
                <select
                  value={specialization}
                  onChange={(e) => setSpecialization(e.target.value)}
                  className="user-type-select"
                  required
                >
                  <option value="">Area of Expertise *</option>
                  {specializations.map(spec => (
                    <option key={spec} value={spec}>{spec}</option>
                  ))}
                </select>
              </div>
              <div className="input-group half">
                <input
                  type="number"
                  value={experienceYears}
                  onChange={(e) => setExperienceYears(e.target.value)}
                  placeholder="Years of Experience"
                  min="0"
                  max="50"
                />
              </div>
            </div>
          </>
        );

      default:
        return null;
    }
  };

  return (
    <div className="signup-page">
      {/* Left Side - Branding */}
      <div className="signup-branding">
        <div className="branding-content">
          <Link to="/" className="brand-logo">
            <img src={logo} alt="GradLink" />
            <span>GradLink</span>
          </Link>
          
          <div className="branding-text">
            <h1>Start Your Journey</h1>
            <p>Connect with mentors, companies, and opportunities. Build your future with GradLink.</p>
          </div>

          <div className="branding-image">
            <img src={graduatesImg} alt="Graduates" />
          </div>

          <div className="branding-features">
            <div className="feature-item">
              <span className="feature-icon">🎯</span>
              <span>Find Projects</span>
            </div>
            <div className="feature-item">
              <span className="feature-icon">👥</span>
              <span>Connect with Mentors</span>
            </div>
            <div className="feature-item">
              <span className="feature-icon">💼</span>
              <span>Get Hired</span>
            </div>
          </div>
        </div>
      </div>

      {/* Right Side - Form */}
      <div className="signup-form-side">
        <div className="signup-form-container">
          <div className="signup-header">
            <h2>Create Account</h2>
            <p>{step === 1 ? 'Enter your details to get started' : 'Complete your profile'}</p>
          </div>

          {/* Progress indicator */}
          <div className="step-indicator">
            <div className={`step ${step >= 1 ? 'active' : ''}`}>1</div>
            <div className="step-line"></div>
            <div className={`step ${step >= 2 ? 'active' : ''}`}>2</div>
          </div>

          {error && <div className="error-message">{error}</div>}
          {success && <div className="success-message">{success}</div>}

          <form onSubmit={handleSubmit}>
            {step === 1 && (
              <>
                <div className="user-type-options">
                  {['Student', 'Company', 'Sponsor', 'Mentor'].map((type) => (
                    <button
                      key={type}
                      type="button"
                      className={`user-type-btn ${userType === type ? 'active' : ''}`}
                      onClick={() => setUserType(type)}
                    >
                      <span className="type-icon">
                        {type === 'Student' && '🎓'}
                        {type === 'Company' && '🏢'}
                        {type === 'Sponsor' && '💰'}
                        {type === 'Mentor' && '👨‍🏫'}
                      </span>
                      <span className="type-label">{type}</span>
                    </button>
                  ))}
                </div>

                <div className="input-group">
                  <label>Full Name</label>
                  <input
                    type="text"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    placeholder={userType === 'Company' || userType === 'Sponsor' ? 'Contact Person Name' : 'Enter your full name'}
                    required
                  />
                </div>

                <div className="input-group">
                  <label>Email Address</label>
                  <input
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    placeholder="Enter your email"
                    required
                  />
                </div>

                <div className="input-group">
                  <label>Phone Number <span className="optional">(optional)</span></label>
                  <input
                    type="tel"
                    value={phone}
                    onChange={(e) => setPhone(e.target.value)}
                    placeholder="Enter your phone number"
                  />
                </div>

                <div className="input-group">
                  <label>Password</label>
                  <input
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    placeholder="Min 6 characters"
                    required
                    minLength="6"
                  />
                </div>

                <button type="button" onClick={nextStep} className="signup-btn">
                  Continue
                  <span className="btn-arrow">→</span>
                </button>

                <div className="divider">
                  <span>or continue with</span>
                </div>

                <div className="social-buttons">
                  <button type="button" className="social-btn google" onClick={handleGoogleSignup}>
                    <FontAwesomeIcon icon={faGoogle} />
                    Google
                  </button>
                  <button type="button" className="social-btn apple" onClick={handleAppleSignup}>
                    <FontAwesomeIcon icon={faApple} />
                    Apple
                  </button>
                </div>
              </>
            )}

            {step === 2 && (
              <>
                {renderUserTypeFields()}

                <div className="terms-section">
                  <input
                    type="checkbox"
                    id="terms"
                    checked={agreeTerms}
                    onChange={(e) => setAgreeTerms(e.target.checked)}
                    required
                  />
                  <label htmlFor="terms">
                    I agree to the <Link to="/terms">Terms of Service</Link> and <Link to="/privacy">Privacy Policy</Link>
                  </label>
                </div>

                <div className="button-group">
                  <button type="button" onClick={prevStep} className="back-btn">
                    ← Back
                  </button>
                  <button type="submit" className="signup-btn" disabled={isLoading}>
                    {isLoading ? (
                      <>
                        <span className="spinner"></span>
                        Creating...
                      </>
                    ) : (
                      'Create Account'
                    )}
                  </button>
                </div>
              </>
            )}
          </form>

          <div className="signin-link">
            Already have an account? <Link to="/signin">Sign In</Link>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Signup;
