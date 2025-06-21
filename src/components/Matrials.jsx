import React, { useState, useEffect, useRef } from 'react';
import './Matrials.css';
import { 
  FiDownload, FiExternalLink, FiBook, FiDatabase, FiVideo, 
  FiSearch, FiFilter, FiPlus, FiStar, FiBookmark, FiAward, 
  FiTrendingUp, FiHeart, FiShare2, FiClock, FiCalendar, 
  FiTag, FiX, FiChevronDown, FiChevronUp, FiGrid, FiList
} from 'react-icons/fi';

const Matrials = () => {
  const [activeSection, setActiveSection] = useState('templates');
  const [searchTerm, setSearchTerm] = useState('');
  const [showFilters, setShowFilters] = useState(false);
  const [selectedField, setSelectedField] = useState('all');
  const [showAddModal, setShowAddModal] = useState(false);
  const [viewMode, setViewMode] = useState('grid'); // grid or list
  const [sortBy, setSortBy] = useState('popular'); // popular, newest, name
  const [favorites, setFavorites] = useState([]);
  const [showToast, setShowToast] = useState(false);
  const [toastMessage, setToastMessage] = useState('');
  const [recentlyViewed, setRecentlyViewed] = useState([]);
  const [showRecentlyViewed, setShowRecentlyViewed] = useState(false);
  const searchInputRef = useRef(null);

  // Cargar favoritos desde localStorage al iniciar
  useEffect(() => {
    const savedFavorites = localStorage.getItem('materialsFavorites');
    if (savedFavorites) {
      setFavorites(JSON.parse(savedFavorites));
    }
    
    const savedRecent = localStorage.getItem('materialsRecentlyViewed');
    if (savedRecent) {
      setRecentlyViewed(JSON.parse(savedRecent));
    }
    
    // Atajo de teclado para búsqueda
    const handleKeyDown = (e) => {
      if ((e.ctrlKey || e.metaKey) && e.key === 'k') {
        e.preventDefault();
        searchInputRef.current?.focus();
      }
    };
    
    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, []);

  // Guardar favoritos en localStorage cuando cambian
  useEffect(() => {
    localStorage.setItem('materialsFavorites', JSON.stringify(favorites));
  }, [favorites]);
  
  // Guardar vistos recientemente
  useEffect(() => {
    localStorage.setItem('materialsRecentlyViewed', JSON.stringify(recentlyViewed));
  }, [recentlyViewed]);

  // Función para mostrar toast
  const showToastMessage = (message) => {
    setToastMessage(message);
    setShowToast(true);
    setTimeout(() => setShowToast(false), 3000);
  };

  // Función para añadir/quitar de favoritos
  const toggleFavorite = (item) => {
    const itemId = `${activeSection}-${item.title}`;
    if (favorites.some(fav => fav.id === itemId)) {
      setFavorites(favorites.filter(fav => fav.id !== itemId));
      showToastMessage('Removed from favorites');
    } else {
      setFavorites([...favorites, { 
        id: itemId, 
        section: activeSection, 
        item: item 
      }]);
      showToastMessage('Added to favorites');
    }
  };

  // Función para registrar vista de un recurso
  const viewResource = (item) => {
    const itemId = `${activeSection}-${item.title}`;
    // Eliminar si ya existe y añadir al principio
    const updatedRecent = [
      { id: itemId, section: activeSection, item: item, timestamp: new Date().toISOString() },
      ...recentlyViewed.filter(rec => rec.id !== itemId)
    ].slice(0, 10); // Mantener solo los 10 más recientes
    
    setRecentlyViewed(updatedRecent);
  };

  // Función para compartir un recurso
  const shareResource = async (item) => {
    if (navigator.share) {
      try {
        await navigator.share({
          title: item.title,
          text: item.description,
          url: item.link || window.location.href,
        });
        showToastMessage('Resource shared successfully');
      } catch (error) {
        console.error('Error sharing:', error);
      }
    } else {
      // Fallback para navegadores que no soportan Web Share API
      navigator.clipboard.writeText(item.link || window.location.href);
      showToastMessage('Link copied to clipboard');
    }
  };

  // Ordenar datos según el criterio seleccionado
  const sortData = (data) => {
    switch (sortBy) {
      case 'popular':
        return [...data].sort((a, b) => b.downloads - a.downloads);
      case 'newest':
        return [...data].sort((a, b) => {
          // Si no hay fecha, asumimos que es más antiguo
          const dateA = a.dateAdded ? new Date(a.dateAdded) : new Date(0);
          const dateB = b.dateAdded ? new Date(b.dateAdded) : new Date(0);
          return dateB - dateA;
        });
      case 'name':
        return [...data].sort((a, b) => a.title.localeCompare(b.title));
      default:
        return data;
    }
  };

  const fields = ['all', 'computer science', 'engineering', 'business', 'medicine', 'arts'];

  const templates = [
    {
      title: 'Project Presentation Template',
      description: 'Professional PowerPoint template for graduation project presentations',
      fileType: 'PPTX',
      downloadLink: '/files/project_presentation_template.pptx',
      field: 'computer science',
      downloads: 1245
    },
    {
      title: 'Project Documentation Template',
      description: 'Comprehensive template for documenting your graduation project',
      fileType: 'DOCX',
      downloadLink: '/files/project_documentation_template.docx',
      field: 'engineering',
      downloads: 987
    },
    {
      title: 'Project Proposal Template',
      description: 'Template for creating a professional project proposal',
      fileType: 'DOCX',
      downloadLink: '/files/project_proposal_template.docx',
      field: 'business',
      downloads: 1532
    },
    {
      title: 'Gantt Chart Template',
      description: 'Excel template for project timeline and task management',
      fileType: 'XLSX',
      downloadLink: '/files/gantt_chart_template.xlsx',
      field: 'engineering',
      downloads: 756
    }
  ];

  const tools = [
    {
      title: 'Figma',
      description: 'Design interfaces, prototypes, and collaborate with team members',
      link: 'https://www.figma.com',
      field: 'computer science',
      downloads: 2345
    },
    {
      title: 'Canva',
      description: 'Create professional presentations and graphics for your project',
      link: 'https://www.canva.com',
      field: 'arts',
      downloads: 1876
    },
    {
      title: 'GitHub',
      description: 'Host your code, collaborate with team members, and manage versions',
      link: 'https://github.com',
      field: 'computer science',
      downloads: 3421
    },
    {
      title: 'Google Scholar',
      description: 'Find academic papers and references for your research',
      link: 'https://scholar.google.com',
      field: 'medicine',
      downloads: 1234
    },
    {
      title: 'Trello',
      description: 'Manage your project tasks and track progress',
      link: 'https://trello.com',
      field: 'business',
      downloads: 987
    }
  ];

  const books = [
    {
      title: 'Clean Code',
      author: 'Robert C. Martin',
      description: 'A handbook of agile software craftsmanship',
      link: 'https://www.amazon.com/Clean-Code-Handbook-Software-Craftsmanship/dp/0132350882',
      field: 'computer science',
      downloads: 4532
    },
    {
      title: 'Introduction to Algorithms',
      author: 'Thomas H. Cormen, et al.',
      description: 'Comprehensive introduction to algorithms and data structures',
      link: 'https://www.amazon.com/Introduction-Algorithms-3rd-MIT-Press/dp/0262033844',
      field: 'computer science',
      downloads: 3245
    },
    {
      title: 'Artificial Intelligence: A Modern Approach',
      author: 'Stuart Russell and Peter Norvig',
      description: 'The standard text in AI, used in universities worldwide',
      link: 'https://www.amazon.com/Artificial-Intelligence-Modern-Approach-3rd/dp/0136042597',
      field: 'computer science',
      downloads: 2876
    },
    {
      title: 'Database System Concepts',
      author: 'Abraham Silberschatz, et al.',
      description: 'Fundamental concepts of database management systems',
      link: 'https://www.amazon.com/Database-Concepts-Abraham-Silberschatz-Professor/dp/0073523321',
      field: 'computer science',
      downloads: 1987
    }
  ];

  const datasets = [
    {
      title: 'Kaggle',
      description: 'Platform for data science competitions with thousands of datasets',
      link: 'https://www.kaggle.com/datasets',
      field: 'computer science',
      downloads: 3421
    },
    {
      title: 'UCI Machine Learning Repository',
      description: 'Collection of databases, domain theories, and data generators',
      link: 'https://archive.ics.uci.edu/ml/index.php',
      field: 'computer science',
      downloads: 2134
    },
    {
      title: 'Google Dataset Search',
      description: 'Search engine for datasets published across the web',
      link: 'https://datasetsearch.research.google.com/',
      field: 'computer science',
      downloads: 1876
    },
    {
      title: 'Data.gov',
      description: "U.S. government's open data portal with thousands of datasets",
      link: 'https://www.data.gov/',
      field: 'business',
      downloads: 1543
    }
  ];

  const videos = [
    {
      title: 'How to Create a React App',
      description: 'Step-by-step tutorial on building a React application from scratch',
      link: 'https://www.youtube.com/watch?v=SqcY0GlETPk',
      field: 'computer science',
      downloads: 2345
    },
    {
      title: 'HTML course',
      description: 'Step-by-step tutorial on HTML fundamentals',
      link: 'https://www.youtube.com/watch?v=6QAELgirvjs&list=PLDoPjvoNmBAw_t_XWUFbBX-c9MafPk9ji',
      field: 'computer science',
      downloads: 3421
    },
    {
      title: 'CSS course',
      description: 'Step-by-step tutorial on CSS styling',
      link: 'https://www.youtube.com/watch?v=X1ulCwyhCVM&list=PLDoPjvoNmBAzjsz06gkzlSrlev53MGIKe',
      field: 'computer science',
      downloads: 2987
    },
    {
      title: 'JavaScript course',
      description: 'Step-by-step tutorial on JavaScript programming',
      link: 'https://www.youtube.com/watch?v=GM6dQBmc-Xg&list=PLDoPjvoNmBAx3kiplQR_oeDqLDBUDYwVv',
      field: 'computer science',
      downloads: 3876
    },
    {
      title: 'MongoDB and Express Explanation',
      description: 'Learn how to connect MongoDB with Express.js for your backend',
      link: 'https://www.youtube.com/watch?v=vjf774RKrLc',
      field: 'computer science',
      downloads: 1765
    },
    {
      title: 'How to Create a Professional Graduation Project Research',
      description: 'Guide to conducting and documenting research for your graduation project',
      link: 'https://www.youtube.com/watch?v=PJjCCmAKcrY',
      field: 'engineering',
      downloads: 1432
    },
    {
      title: 'Git & GitHub Crash Course',
      description: 'Learn the basics of version control with Git and GitHub',
      link: 'https://www.youtube.com/watch?v=RGOj5yH7evk',
      field: 'computer science',
      downloads: 2876
    }
  ];

  // Get current data based on active section
  const getCurrentData = () => {
    switch (activeSection) {
      case 'templates': return templates;
      case 'tools': return tools;
      case 'books': return books;
      case 'datasets': return datasets;
      case 'videos': return videos;
      default: return [];
    }
  };

  // Filter data based on search term and selected field
  const filterData = (data) => {
    return data.filter(item => {
      const matchesSearch = item.title.toLowerCase().includes(searchTerm.toLowerCase()) || 
                           item.description.toLowerCase().includes(searchTerm.toLowerCase());
      const matchesField = selectedField === 'all' || item.field === selectedField;
      return matchesSearch && matchesField;
    });
  };

  // Get top rated items
  const getTopRated = () => {
    const allData = [...templates, ...tools, ...books, ...datasets, ...videos];
    return allData.sort((a, b) => b.downloads - a.downloads).slice(0, 4);
  };

  const renderTopRated = () => {
    const topRated = getTopRated();
    
    return (
      <div className="top-rated-section">
        <h2 className="section-title"><FiStar className="section-icon" /> Top Rated Resources</h2>
        <div className="materials-grid">
          {topRated.map((item, index) => (
            <div className="material-card top-rated" key={index}>
              <div className="material-content">
                <div className="downloads-badge">{item.downloads} downloads</div>
                <h3 className="material-title">{item.title}</h3>
                {'author' in item && <p className="material-author">by {item.author}</p>}
                <p className="material-description">{item.description}</p>
                {'fileType' in item && <span className="file-type">{item.fileType}</span>}
                <span className="field-tag">{item.field}</span>
              </div>
              {'downloadLink' in item ? (
                <a href={item.downloadLink} className="download-btn" download>
                  <FiDownload /> Download
                </a>
              ) : (
                <a href={item.link} className="external-link-btn" target="_blank" rel="noopener noreferrer">
                  <FiExternalLink /> View Resource
                </a>
              )}
            </div>
          ))}
        </div>
      </div>
    );
  };

  // Renderizar la sección de vistos recientemente
  const renderRecentlyViewed = () => {
    if (recentlyViewed.length === 0) {
      return null;
    }
    
    return (
      <div className="recently-viewed-section">
        <div className="section-header">
          <h2 className="section-title">
            <FiClock className="section-icon" /> Recently Viewed
          </h2>
          <button 
            className="toggle-btn"
            onClick={() => setShowRecentlyViewed(!showRecentlyViewed)}
          >
            {showRecentlyViewed ? <FiChevronUp /> : <FiChevronDown />}
          </button>
        </div>
        
        {showRecentlyViewed && (
          <div className="recently-viewed-grid">
            {recentlyViewed.map((item, index) => (
              <div className="recent-item" key={index}>
                <div className="recent-content">
                  <h4>{item.item.title}</h4>
                  <p className="recent-meta">
                    <span className="recent-section">{item.section}</span>
                    <span className="recent-time">
                      {new Date(item.timestamp).toLocaleDateString()}
                    </span>
                  </p>
                </div>
                <div className="recent-actions">
                  {'downloadLink' in item.item ? (
                    <a href={item.item.downloadLink} className="action-btn" download>
                      <FiDownload />
                    </a>
                  ) : (
                    <a href={item.item.link} className="action-btn" target="_blank" rel="noopener noreferrer">
                      <FiExternalLink />
                    </a>
                  )}
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    );
  };

  // Renderizar la sección de favoritos
  const renderFavorites = () => {
    if (favorites.length === 0 || activeSection !== 'favorites') {
      return null;
    }
    
    return (
      <div className="materials-grid">
        {favorites.map((fav, index) => {
          const item = fav.item;
          return (
            <div className="material-card favorite" key={index}>
              <div className="material-content">
                <div className="card-badge">{fav.section}</div>
                <h3 className="material-title">{item.title}</h3>
                {'author' in item && <p className="material-author">by {item.author}</p>}
                <p className="material-description">{item.description}</p>
                <div className="card-footer">
                  {'fileType' in item && <span className="file-type">{item.fileType}</span>}
                  <span className="field-tag">{item.field}</span>
                  <span className="downloads-count"><FiDownload /> {item.downloads}</span>
                </div>
              </div>
              <div className="card-actions">
                <button 
                  className="action-btn remove-favorite"
                  onClick={() => toggleFavorite(item)}
                  title="Remove from favorites"
                >
                  <FiHeart className="filled" />
                </button>
                <button 
                  className="action-btn"
                  onClick={() => shareResource(item)}
                  title="Share"
                >
                  <FiShare2 />
                </button>
                {'downloadLink' in item ? (
                  <a href={item.downloadLink} className="download-btn" download onClick={() => viewResource(item)}>
                    <FiDownload /> Download
                  </a>
                ) : (
                  <a href={item.link} className="external-link-btn" target="_blank" rel="noopener noreferrer" onClick={() => viewResource(item)}>
                    <FiExternalLink /> View Resource
                  </a>
                )}
              </div>
            </div>
          );
        })}
      </div>
    );
  };

  const renderContent = () => {
    if (activeSection === 'favorites') {
      return renderFavorites();
    }
    
    const currentData = getCurrentData();
    const filteredData = filterData(currentData);
    const sortedData = sortData(filteredData);
    
    if (sortedData.length === 0) {
      return <div className="no-results">No resources match your search criteria.</div>;
    }
    
    return (
      <div className={`materials-${viewMode}`}>
        {sortedData.map((item, index) => (
          <div className={`material-card ${viewMode === 'list' ? 'list-view' : ''}`} key={index}>
            <div className="material-content">
              <h3 className="material-title">{item.title}</h3>
              {'author' in item && <p className="material-author">by {item.author}</p>}
              <p className="material-description">{item.description}</p>
              <div className="card-footer">
                {'fileType' in item && <span className="file-type">{item.fileType}</span>}
                <span className="field-tag">{item.field}</span>
                <span className="downloads-count"><FiDownload /> {item.downloads}</span>
                {'dateAdded' in item && (
                  <span className="date-added">
                    <FiCalendar /> {new Date(item.dateAdded).toLocaleDateString()}
                  </span>
                )}
              </div>
            </div>
            <div className="card-actions">
              <button 
                className="action-btn"
                onClick={() => toggleFavorite(item)}
                title={favorites.some(fav => fav.id === `${activeSection}-${item.title}`) ? "Remove from favorites" : "Add to favorites"}
              >
                <FiHeart className={favorites.some(fav => fav.id === `${activeSection}-${item.title}`) ? "filled" : ""} />
              </button>
              <button 
                className="action-btn"
                onClick={() => shareResource(item)}
                title="Share"
              >
                <FiShare2 />
              </button>
              {'downloadLink' in item ? (
                <a href={item.downloadLink} className="download-btn" download onClick={() => viewResource(item)}>
                  <FiDownload /> Download
                </a>
              ) : (
                <a href={item.link} className="external-link-btn" target="_blank" rel="noopener noreferrer" onClick={() => viewResource(item)}>
                  <FiExternalLink /> View Resource
                </a>
              )}
            </div>
          </div>
        ))}
      </div>
    );
  };

  // Componente para el modal de añadir recurso
  const AddResourceModal = () => {
    const [newResource, setNewResource] = useState({
      title: '',
      description: '',
      field: 'computer science',
      link: '',
      type: 'tool' // tool, book, template, dataset, video
    });
    
    const handleChange = (e) => {
      const { name, value } = e.target;
      setNewResource({
        ...newResource,
        [name]: value
      });
    };

    const handleSubmit = (e) => {
      e.preventDefault();
      // Aquí normalmente enviarías los datos al backend
      console.log('New resource submitted:', newResource);
      setShowAddModal(false);
      showToastMessage('Resource submitted successfully! It will be reviewed before being added.');
    };
    
    return (
      <div className="modal-overlay" onClick={() => setShowAddModal(false)}>
        <div className="modal-content" onClick={e => e.stopPropagation()}>
          <h2>Add New Resource</h2>
          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <label htmlFor="type">Resource Type</label>
              <select 
                id="type" 
                name="type" 
                value={newResource.type}
                onChange={handleChange}
                required
              >
                <option value="tool">Tool</option>
                <option value="book">Book</option>
                <option value="template">Template</option>
                <option value="dataset">Dataset</option>
                <option value="video">Video</option>
              </select>
            </div>
            
            <div className="form-group">
              <label htmlFor="title">Title</label>
              <input 
                type="text" 
                id="title" 
                name="title" 
                value={newResource.title}
                onChange={handleChange}
                required
                placeholder="Enter resource title"
              />
            </div>
            
            <div className="form-group">
              <label htmlFor="description">Description</label>
              <textarea 
                id="description" 
                name="description" 
                value={newResource.description}
                onChange={handleChange}
                required
                placeholder="Describe this resource"
              />
            </div>
            
            <div className="form-group">
              <label htmlFor="field">Field</label>
              <select 
                id="field" 
                name="field" 
                value={newResource.field}
                onChange={handleChange}
                required
              >
                {fields.filter(f => f !== 'all').map(field => (
                  <option key={field} value={field}>
                    {field.charAt(0).toUpperCase() + field.slice(1)}
                  </option>
                ))}
              </select>
            </div>
            
            <div className="form-group">
              <label htmlFor="link">Resource Link</label>
              <input 
                type="url" 
                id="link" 
                name="link" 
                value={newResource.link}
                onChange={handleChange}
                required
                placeholder="https://example.com"
              />
            </div>
            
            {newResource.type === 'template' && (
              <div className="form-group">
                <label htmlFor="fileType">File Type</label>
                <input 
                  type="text" 
                  id="fileType" 
                  name="fileType" 
                  placeholder="e.g., DOCX, PDF, PPTX"
                  onChange={handleChange}
                />
              </div>
            )}
            
            {newResource.type === 'book' && (
              <div className="form-group">
                <label htmlFor="author">Author</label>
                <input 
                  type="text" 
                  id="author" 
                  name="author" 
                  placeholder="Author name"
                  onChange={handleChange}
                />
              </div>
            )}
            
            <div className="modal-buttons">
              <button 
                type="button" 
                className="cancel-btn"
                onClick={() => setShowAddModal(false)}
              >
                Cancel
              </button>
              <button type="submit" className="submit-btn">Submit Resource</button>
            </div>
          </form>
        </div>
      </div>
    );
  };

  // Toast notification
  const Toast = () => {
    return (
      <div className={`toast-notification ${showToast ? 'show' : ''}`}>
        {toastMessage}
      </div>
    );
  };

  return (
    <div className="materials-container">
      <h1 className="materials-title">Materials & Resources</h1>
      
      {/* Navegación como elemento principal */}
      <div className="materials-nav">
        <button 
          className={`nav-item ${activeSection === 'templates' ? 'active' : ''}`}
          onClick={() => setActiveSection('templates')}
        >
          <FiDownload className="nav-icon" /> Templates
        </button>
        <button 
          className={`nav-item ${activeSection === 'tools' ? 'active' : ''}`}
          onClick={() => setActiveSection('tools')}
        >
          <FiExternalLink className="nav-icon" /> Tools
        </button>
        <button 
          className={`nav-item ${activeSection === 'books' ? 'active' : ''}`}
          onClick={() => setActiveSection('books')}
        >
          <FiBook className="nav-icon" /> Books
        </button>
        <button 
          className={`nav-item ${activeSection === 'datasets' ? 'active' : ''}`}
          onClick={() => setActiveSection('datasets')}
        >
          <FiDatabase className="nav-icon" /> Datasets
        </button>
        <button 
          className={`nav-item ${activeSection === 'videos' ? 'active' : ''}`}
          onClick={() => setActiveSection('videos')}
        >
          <FiVideo className="nav-icon" /> Videos
        </button>
        <button 
          className={`nav-item ${activeSection === 'favorites' ? 'active' : ''}`}
          onClick={() => setActiveSection('favorites')}
        >
          <FiHeart className="nav-icon" /> Favorites
        </button>
      </div>
      
      <div className="search-filter-container">
        <div className="search-box">
          <FiSearch className="search-iconn" />
          <input 
            type="text" 
            placeholder="Search resources... (Ctrl+K)"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            ref={searchInputRef}
          />
          {searchTerm && (
            <button 
              className="clear-search" 
              onClick={() => setSearchTerm('')}
              title="Clear search"
            >
              <FiX />
            </button>
          )}
        </div>
        
        <div className="view-controls">
          <button 
            className={`view-btn ${viewMode === 'grid' ? 'active' : ''}`}
            onClick={() => setViewMode('grid')}
            title="Grid view"
          >
            <FiGrid />
          </button>
          <button 
            className={`view-btn ${viewMode === 'list' ? 'active' : ''}`}
            onClick={() => setViewMode('list')}
            title="List view"
          >
            <FiList />
          </button>
        </div>
        
        <div className="sort-control">
          <select 
            value={sortBy}
            onChange={(e) => setSortBy(e.target.value)}
          >
            <option value="popular">Most Popular</option>
            <option value="newest">Newest</option>
            <option value="name">Name</option>
          </select>
        </div>
        
        <button 
          className="filter-btn"
          onClick={() => setShowFilters(!showFilters)}
        >
          <FiFilter /> Filter
        </button>
        
        <button 
          className="add-resource-btn"
          onClick={() => setShowAddModal(true)}
        >
          <FiPlus /> Add Resource
        </button>
      </div>
      
      {showFilters && (
        <div className="filters-container">
          <div className="filter-group">
            <label>Field:</label>
            <div className="field-options">
              {fields.map(field => (
                <button 
                  key={field}
                  className={`field-option ${selectedField === field ? 'active' : ''}`}
                  onClick={() => setSelectedField(field)}
                >
                  {field.charAt(0).toUpperCase() + field.slice(1)}
                </button>
              ))}
            </div>
          </div>
        </div>
      )}
      
      {/* Contenido principal según la sección activa */}
      {renderContent()}
      
      {/* Top Rated Section - Ahora debajo de la cuadrícula principal */}
      {!searchTerm && selectedField === 'all' && renderTopRated()}
      
      {showAddModal && <AddResourceModal />}
      {showToast && <Toast />}
      {renderRecentlyViewed()}
    </div>
  );
};

export default Matrials;
