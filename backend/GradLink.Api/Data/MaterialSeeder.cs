using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Data;

public class MaterialSeeder
{
    private readonly AppDbContext _context;

    public MaterialSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedMaterialsAsync()
    {
        // Check if materials already exist
        if (await _context.Materials.AnyAsync(m => m.Status == "approved"))
        {
            return;
        }

        var materials = new List<Material>
        {
            // ============ TEMPLATES ============
            new Material
            {
                Type = "template",
                Title = "Project Presentation Template",
                Description = "Professional PowerPoint template for graduation project presentations with modern design, infographics, and timeline slides",
                FileType = "PPTX",
                Link = "/files/project_presentation_template.pptx",
                Field = "computer science",
                Tags = "presentation,graduation,project,powerpoint",
                Status = "approved",
                IsFeatured = true,
                DifficultyLevel = "beginner",
                EstimatedTimeMinutes = 30,
                DownloadCount = 1245,
                AverageRating = 4.5,
                RatingCount = 89,
                CreatedAt = DateTime.UtcNow.AddDays(-60)
            },
            new Material
            {
                Type = "template",
                Title = "Project Documentation Template",
                Description = "Comprehensive template for documenting your graduation project with all required sections including methodology, results, and analysis",
                FileType = "DOCX",
                Link = "/files/project_documentation_template.docx",
                Field = "engineering",
                Tags = "documentation,thesis,research,word",
                Status = "approved",
                DifficultyLevel = "intermediate",
                EstimatedTimeMinutes = 120,
                DownloadCount = 987,
                AverageRating = 4.3,
                RatingCount = 67,
                CreatedAt = DateTime.UtcNow.AddDays(-55)
            },
            new Material
            {
                Type = "template",
                Title = "Project Proposal Template",
                Description = "Professional template for creating a compelling project proposal with budget, timeline, and methodology sections",
                FileType = "DOCX",
                Link = "/files/project_proposal_template.docx",
                Field = "business",
                Tags = "proposal,planning,project management",
                Status = "approved",
                IsFeatured = true,
                DifficultyLevel = "beginner",
                EstimatedTimeMinutes = 60,
                DownloadCount = 1532,
                AverageRating = 4.7,
                RatingCount = 112,
                CreatedAt = DateTime.UtcNow.AddDays(-50)
            },
            new Material
            {
                Type = "template",
                Title = "Gantt Chart Template",
                Description = "Excel template for project timeline and task management with automatic progress tracking",
                FileType = "XLSX",
                Link = "/files/gantt_chart_template.xlsx",
                Field = "engineering",
                Tags = "gantt,timeline,planning,excel",
                Status = "approved",
                DifficultyLevel = "intermediate",
                EstimatedTimeMinutes = 45,
                DownloadCount = 756,
                AverageRating = 4.2,
                RatingCount = 45,
                CreatedAt = DateTime.UtcNow.AddDays(-45)
            },
            new Material
            {
                Type = "template",
                Title = "Research Paper Template (IEEE Format)",
                Description = "IEEE format template for writing research papers with proper citations and formatting",
                FileType = "DOCX",
                Link = "/files/ieee_paper_template.docx",
                Field = "computer science",
                Tags = "ieee,research,paper,academic",
                Status = "approved",
                DifficultyLevel = "advanced",
                EstimatedTimeMinutes = 180,
                DownloadCount = 890,
                AverageRating = 4.6,
                RatingCount = 78,
                CreatedAt = DateTime.UtcNow.AddDays(-40)
            },

            // ============ TOOLS ============
            new Material
            {
                Type = "tool",
                Title = "Figma",
                Description = "Design interfaces, prototypes, and collaborate with team members in real-time",
                Link = "https://www.figma.com",
                Field = "computer science",
                Tags = "design,ui,ux,prototype,collaboration",
                Status = "approved",
                IsFeatured = true,
                DifficultyLevel = "beginner",
                DownloadCount = 2345,
                AverageRating = 4.8,
                RatingCount = 156,
                CreatedAt = DateTime.UtcNow.AddDays(-90)
            },
            new Material
            {
                Type = "tool",
                Title = "Canva",
                Description = "Create professional presentations, graphics, and marketing materials for your project",
                Link = "https://www.canva.com",
                Field = "arts",
                Tags = "design,graphics,presentation,marketing",
                Status = "approved",
                DifficultyLevel = "beginner",
                DownloadCount = 1876,
                AverageRating = 4.6,
                RatingCount = 134,
                CreatedAt = DateTime.UtcNow.AddDays(-85)
            },
            new Material
            {
                Type = "tool",
                Title = "GitHub",
                Description = "Host your code, collaborate with team members, and manage versions with the world's leading developer platform",
                Link = "https://github.com",
                Field = "computer science",
                Tags = "git,version control,collaboration,code",
                Status = "approved",
                IsFeatured = true,
                DifficultyLevel = "intermediate",
                DownloadCount = 3421,
                AverageRating = 4.9,
                RatingCount = 234,
                CreatedAt = DateTime.UtcNow.AddDays(-120)
            },
            new Material
            {
                Type = "tool",
                Title = "Google Scholar",
                Description = "Find academic papers, citations, and references for your research",
                Link = "https://scholar.google.com",
                Field = "medicine",
                Tags = "research,academic,papers,citations",
                Status = "approved",
                DifficultyLevel = "beginner",
                DownloadCount = 1234,
                AverageRating = 4.4,
                RatingCount = 89,
                CreatedAt = DateTime.UtcNow.AddDays(-100)
            },
            new Material
            {
                Type = "tool",
                Title = "Trello",
                Description = "Manage your project tasks and track progress with visual boards and cards",
                Link = "https://trello.com",
                Field = "business",
                Tags = "project management,tasks,kanban,collaboration",
                Status = "approved",
                DifficultyLevel = "beginner",
                DownloadCount = 987,
                AverageRating = 4.3,
                RatingCount = 67,
                CreatedAt = DateTime.UtcNow.AddDays(-80)
            },
            new Material
            {
                Type = "tool",
                Title = "Notion",
                Description = "All-in-one workspace for notes, docs, databases, and project management",
                Link = "https://www.notion.so",
                Field = "computer science",
                Tags = "notes,documentation,project management,wiki",
                Status = "approved",
                IsFeatured = true,
                DifficultyLevel = "intermediate",
                DownloadCount = 1567,
                AverageRating = 4.7,
                RatingCount = 123,
                CreatedAt = DateTime.UtcNow.AddDays(-70)
            },

            // ============ BOOKS ============
            new Material
            {
                Type = "book",
                Title = "Clean Code",
                Author = "Robert C. Martin",
                Description = "A handbook of agile software craftsmanship that teaches you how to write clean, maintainable code",
                Link = "https://www.amazon.com/Clean-Code-Handbook-Software-Craftsmanship/dp/0132350882",
                Field = "computer science",
                Tags = "programming,best practices,software development",
                Status = "approved",
                IsFeatured = true,
                DifficultyLevel = "intermediate",
                EstimatedTimeMinutes = 1200,
                DownloadCount = 4532,
                AverageRating = 4.8,
                RatingCount = 298,
                CreatedAt = DateTime.UtcNow.AddDays(-180)
            },
            new Material
            {
                Type = "book",
                Title = "Introduction to Algorithms",
                Author = "Thomas H. Cormen, et al.",
                Description = "Comprehensive introduction to algorithms and data structures used in universities worldwide",
                Link = "https://www.amazon.com/Introduction-Algorithms-3rd-MIT-Press/dp/0262033844",
                Field = "computer science",
                Tags = "algorithms,data structures,computer science",
                Status = "approved",
                DifficultyLevel = "advanced",
                EstimatedTimeMinutes = 2400,
                DownloadCount = 3245,
                AverageRating = 4.5,
                RatingCount = 187,
                CreatedAt = DateTime.UtcNow.AddDays(-200)
            },
            new Material
            {
                Type = "book",
                Title = "Artificial Intelligence: A Modern Approach",
                Author = "Stuart Russell and Peter Norvig",
                Description = "The standard text in AI, covering machine learning, neural networks, and modern AI techniques",
                Link = "https://www.amazon.com/Artificial-Intelligence-Modern-Approach-3rd/dp/0136042597",
                Field = "computer science",
                Tags = "ai,machine learning,artificial intelligence",
                Status = "approved",
                IsFeatured = true,
                DifficultyLevel = "advanced",
                EstimatedTimeMinutes = 2000,
                DownloadCount = 2876,
                AverageRating = 4.6,
                RatingCount = 165,
                CreatedAt = DateTime.UtcNow.AddDays(-150)
            },
            new Material
            {
                Type = "book",
                Title = "Database System Concepts",
                Author = "Abraham Silberschatz, et al.",
                Description = "Fundamental concepts of database management systems including SQL, NoSQL, and database design",
                Link = "https://www.amazon.com/Database-Concepts-Abraham-Silberschatz-Professor/dp/0073523321",
                Field = "computer science",
                Tags = "database,sql,data management",
                Status = "approved",
                DifficultyLevel = "intermediate",
                EstimatedTimeMinutes = 1800,
                DownloadCount = 1987,
                AverageRating = 4.4,
                RatingCount = 134,
                CreatedAt = DateTime.UtcNow.AddDays(-140)
            },
            new Material
            {
                Type = "book",
                Title = "The Lean Startup",
                Author = "Eric Ries",
                Description = "How today's entrepreneurs use continuous innovation to create successful businesses",
                Link = "https://www.amazon.com/Lean-Startup-Entrepreneurs-Continuous-Innovation/dp/0307887898",
                Field = "business",
                Tags = "startup,entrepreneurship,business",
                Status = "approved",
                DifficultyLevel = "beginner",
                EstimatedTimeMinutes = 600,
                DownloadCount = 2156,
                AverageRating = 4.5,
                RatingCount = 189,
                CreatedAt = DateTime.UtcNow.AddDays(-130)
            },

            // ============ DATASETS ============
            new Material
            {
                Type = "dataset",
                Title = "Kaggle",
                Description = "Platform for data science competitions with thousands of datasets across all domains",
                Link = "https://www.kaggle.com/datasets",
                Field = "computer science",
                Tags = "data science,machine learning,datasets,competitions",
                Status = "approved",
                IsFeatured = true,
                DifficultyLevel = "intermediate",
                DownloadCount = 3421,
                AverageRating = 4.7,
                RatingCount = 234,
                CreatedAt = DateTime.UtcNow.AddDays(-100)
            },
            new Material
            {
                Type = "dataset",
                Title = "UCI Machine Learning Repository",
                Description = "Collection of databases, domain theories, and data generators for machine learning research",
                Link = "https://archive.ics.uci.edu/ml/index.php",
                Field = "computer science",
                Tags = "machine learning,research,datasets",
                Status = "approved",
                DifficultyLevel = "advanced",
                DownloadCount = 2134,
                AverageRating = 4.5,
                RatingCount = 156,
                CreatedAt = DateTime.UtcNow.AddDays(-120)
            },
            new Material
            {
                Type = "dataset",
                Title = "Google Dataset Search",
                Description = "Search engine for datasets published across the web",
                Link = "https://datasetsearch.research.google.com/",
                Field = "computer science",
                Tags = "search,datasets,research",
                Status = "approved",
                DifficultyLevel = "beginner",
                DownloadCount = 1876,
                AverageRating = 4.3,
                RatingCount = 98,
                CreatedAt = DateTime.UtcNow.AddDays(-90)
            },
            new Material
            {
                Type = "dataset",
                Title = "Data.gov",
                Description = "U.S. government's open data portal with thousands of datasets on various topics",
                Link = "https://www.data.gov/",
                Field = "business",
                Tags = "government,open data,public datasets",
                Status = "approved",
                DifficultyLevel = "beginner",
                DownloadCount = 1543,
                AverageRating = 4.1,
                RatingCount = 76,
                CreatedAt = DateTime.UtcNow.AddDays(-85)
            },

            // ============ VIDEOS ============
            new Material
            {
                Type = "video",
                Title = "Complete React Course 2024",
                Description = "Step-by-step tutorial on building React applications from scratch, including hooks, context, and Redux",
                Link = "https://www.youtube.com/watch?v=SqcY0GlETPk",
                Field = "computer science",
                Tags = "react,javascript,frontend,web development",
                Status = "approved",
                IsFeatured = true,
                DifficultyLevel = "intermediate",
                DurationMinutes = 480,
                EstimatedTimeMinutes = 600,
                DownloadCount = 2345,
                AverageRating = 4.8,
                RatingCount = 189,
                CreatedAt = DateTime.UtcNow.AddDays(-30)
            },
            new Material
            {
                Type = "video",
                Title = "HTML Complete Course",
                Description = "Comprehensive HTML tutorial covering all elements, forms, and semantic HTML5",
                Link = "https://www.youtube.com/watch?v=6QAELgirvjs&list=PLDoPjvoNmBAw_t_XWUFbBX-c9MafPk9ji",
                Field = "computer science",
                Tags = "html,web development,frontend,beginner",
                Status = "approved",
                DifficultyLevel = "beginner",
                DurationMinutes = 300,
                EstimatedTimeMinutes = 360,
                DownloadCount = 3421,
                AverageRating = 4.6,
                RatingCount = 234,
                CreatedAt = DateTime.UtcNow.AddDays(-60)
            },
            new Material
            {
                Type = "video",
                Title = "CSS Complete Course",
                Description = "Master CSS styling from basics to advanced animations and responsive design",
                Link = "https://www.youtube.com/watch?v=X1ulCwyhCVM&list=PLDoPjvoNmBAzjsz06gkzlSrlev53MGIKe",
                Field = "computer science",
                Tags = "css,web development,frontend,styling",
                Status = "approved",
                DifficultyLevel = "beginner",
                DurationMinutes = 360,
                EstimatedTimeMinutes = 420,
                DownloadCount = 2987,
                AverageRating = 4.5,
                RatingCount = 198,
                CreatedAt = DateTime.UtcNow.AddDays(-55)
            },
            new Material
            {
                Type = "video",
                Title = "JavaScript Complete Course",
                Description = "Full JavaScript tutorial covering ES6+, async/await, and modern JavaScript patterns",
                Link = "https://www.youtube.com/watch?v=GM6dQBmc-Xg&list=PLDoPjvoNmBAx3kiplQR_oeDqLDBUDYwVv",
                Field = "computer science",
                Tags = "javascript,programming,web development",
                Status = "approved",
                IsFeatured = true,
                DifficultyLevel = "intermediate",
                DurationMinutes = 720,
                EstimatedTimeMinutes = 900,
                DownloadCount = 3876,
                AverageRating = 4.7,
                RatingCount = 267,
                CreatedAt = DateTime.UtcNow.AddDays(-50)
            },
            new Material
            {
                Type = "video",
                Title = "MongoDB and Express Tutorial",
                Description = "Learn how to build RESTful APIs with MongoDB and Express.js",
                Link = "https://www.youtube.com/watch?v=vjf774RKrLc",
                Field = "computer science",
                Tags = "mongodb,express,node.js,backend",
                Status = "approved",
                DifficultyLevel = "intermediate",
                DurationMinutes = 180,
                EstimatedTimeMinutes = 240,
                DownloadCount = 1765,
                AverageRating = 4.4,
                RatingCount = 134,
                CreatedAt = DateTime.UtcNow.AddDays(-45)
            },
            new Material
            {
                Type = "video",
                Title = "Professional Research Methods",
                Description = "Guide to conducting and documenting research for your graduation project",
                Link = "https://www.youtube.com/watch?v=PJjCCmAKcrY",
                Field = "engineering",
                Tags = "research,methodology,thesis,academic",
                Status = "approved",
                DifficultyLevel = "advanced",
                DurationMinutes = 120,
                EstimatedTimeMinutes = 180,
                DownloadCount = 1432,
                AverageRating = 4.3,
                RatingCount = 98,
                CreatedAt = DateTime.UtcNow.AddDays(-40)
            },
            new Material
            {
                Type = "video",
                Title = "Git & GitHub Crash Course",
                Description = "Learn the basics of version control with Git and GitHub in under 2 hours",
                Link = "https://www.youtube.com/watch?v=RGOj5yH7evk",
                Field = "computer science",
                Tags = "git,github,version control,collaboration",
                Status = "approved",
                IsFeatured = true,
                DifficultyLevel = "beginner",
                DurationMinutes = 90,
                EstimatedTimeMinutes = 120,
                DownloadCount = 2876,
                AverageRating = 4.8,
                RatingCount = 212,
                CreatedAt = DateTime.UtcNow.AddDays(-35)
            },
            new Material
            {
                Type = "video",
                Title = "Python for Data Science",
                Description = "Complete Python tutorial for data science, including NumPy, Pandas, and Matplotlib",
                Link = "https://www.youtube.com/watch?v=LHBE6Q9XlzI",
                Field = "computer science",
                Tags = "python,data science,numpy,pandas",
                Status = "approved",
                IsFeatured = true,
                DifficultyLevel = "intermediate",
                DurationMinutes = 540,
                EstimatedTimeMinutes = 720,
                DownloadCount = 2543,
                AverageRating = 4.6,
                RatingCount = 178,
                CreatedAt = DateTime.UtcNow.AddDays(-25)
            }
        };

        _context.Materials.AddRange(materials);
        await _context.SaveChangesAsync();

        // Create some learning paths
        var adminUser = await _context.Users.FirstOrDefaultAsync(u => u.Role == "Admin");
        if (adminUser != null)
        {
            var webDevPath = new MaterialCollection
            {
                Title = "Web Development Fundamentals",
                Description = "Learn the core technologies of web development: HTML, CSS, and JavaScript",
                Type = "learning_path",
                Field = "computer science",
                DifficultyLevel = "beginner",
                IsSystemPath = true,
                IsPublic = true,
                OwnerId = adminUser.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.MaterialCollections.Add(webDevPath);
            await _context.SaveChangesAsync();

            // Add materials to learning path
            var htmlMaterial = await _context.Materials.FirstOrDefaultAsync(m => m.Title.Contains("HTML Complete"));
            var cssMaterial = await _context.Materials.FirstOrDefaultAsync(m => m.Title.Contains("CSS Complete"));
            var jsMaterial = await _context.Materials.FirstOrDefaultAsync(m => m.Title.Contains("JavaScript Complete"));

            var order = 0;
            if (htmlMaterial != null)
            {
                _context.CollectionMaterials.Add(new CollectionMaterial
                {
                    CollectionId = webDevPath.Id,
                    MaterialId = htmlMaterial.Id,
                    Order = order++,
                    IsPrerequisite = true
                });
            }
            if (cssMaterial != null)
            {
                _context.CollectionMaterials.Add(new CollectionMaterial
                {
                    CollectionId = webDevPath.Id,
                    MaterialId = cssMaterial.Id,
                    Order = order++,
                    IsPrerequisite = true
                });
            }
            if (jsMaterial != null)
            {
                _context.CollectionMaterials.Add(new CollectionMaterial
                {
                    CollectionId = webDevPath.Id,
                    MaterialId = jsMaterial.Id,
                    Order = order++,
                    IsPrerequisite = false
                });
            }

            var reactMaterial = await _context.Materials.FirstOrDefaultAsync(m => m.Title.Contains("React"));
            if (reactMaterial != null)
            {
                _context.CollectionMaterials.Add(new CollectionMaterial
                {
                    CollectionId = webDevPath.Id,
                    MaterialId = reactMaterial.Id,
                    Order = order++,
                    IsPrerequisite = false
                });
            }

            // Create another learning path for project management
            var projectMgmtPath = new MaterialCollection
            {
                Title = "Project Management Essentials",
                Description = "Essential tools and templates for managing your graduation project effectively",
                Type = "learning_path",
                Field = "business",
                DifficultyLevel = "beginner",
                IsSystemPath = true,
                IsPublic = true,
                OwnerId = adminUser.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.MaterialCollections.Add(projectMgmtPath);
            await _context.SaveChangesAsync();

            var proposalTemplate = await _context.Materials.FirstOrDefaultAsync(m => m.Title.Contains("Proposal Template"));
            var ganttTemplate = await _context.Materials.FirstOrDefaultAsync(m => m.Title.Contains("Gantt Chart"));
            var trelloTool = await _context.Materials.FirstOrDefaultAsync(m => m.Title.Contains("Trello"));
            var notionTool = await _context.Materials.FirstOrDefaultAsync(m => m.Title.Contains("Notion"));

            order = 0;
            if (proposalTemplate != null)
            {
                _context.CollectionMaterials.Add(new CollectionMaterial
                {
                    CollectionId = projectMgmtPath.Id,
                    MaterialId = proposalTemplate.Id,
                    Order = order++,
                    IsPrerequisite = true
                });
            }
            if (ganttTemplate != null)
            {
                _context.CollectionMaterials.Add(new CollectionMaterial
                {
                    CollectionId = projectMgmtPath.Id,
                    MaterialId = ganttTemplate.Id,
                    Order = order++,
                    IsPrerequisite = false
                });
            }
            if (trelloTool != null)
            {
                _context.CollectionMaterials.Add(new CollectionMaterial
                {
                    CollectionId = projectMgmtPath.Id,
                    MaterialId = trelloTool.Id,
                    Order = order++,
                    IsPrerequisite = false
                });
            }
            if (notionTool != null)
            {
                _context.CollectionMaterials.Add(new CollectionMaterial
                {
                    CollectionId = projectMgmtPath.Id,
                    MaterialId = notionTool.Id,
                    Order = order++,
                    IsPrerequisite = false
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}















