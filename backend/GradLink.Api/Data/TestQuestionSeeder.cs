using GradLink.Domain.Entities;
using GradLink.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GradLink.Api.Data
{
    public static class TestQuestionSeeder
    {
        public static async Task SeedTestQuestionsAsync(AppDbContext context)
        {
            // Only seed if we have less than 40 questions (meaning old seed)
            var existingCount = await context.TestQuestions.CountAsync();
            if (existingCount >= 40)
            {
                return; // Already have enough questions
            }
            
            // Delete old questions and reseed with new coding questions
            if (existingCount > 0)
            {
                var oldQuestions = await context.TestQuestions.ToListAsync();
                context.TestQuestions.RemoveRange(oldQuestions);
                await context.SaveChangesAsync();
            }

            var questions = new List<TestQuestion>
            {
                // ==================== General Mentorship Questions ====================
                new TestQuestion
                {
                    Category = "General",
                    QuestionText = "What is the most important quality of a good mentor?",
                    OptionA = "Being an expert in all topics",
                    OptionB = "Active listening and empathy",
                    OptionC = "Having many years of experience",
                    OptionD = "Being strict and demanding",
                    CorrectAnswer = "B",
                    Difficulty = "Easy",
                    Explanation = "Active listening and empathy help mentors understand mentees' needs."
                },
                new TestQuestion
                {
                    Category = "General",
                    QuestionText = "How should a mentor handle disagreement with a mentee?",
                    OptionA = "Insist the mentor is always right",
                    OptionB = "End the mentoring relationship",
                    OptionC = "Listen to their perspective and discuss alternatives",
                    OptionD = "Ignore the disagreement",
                    CorrectAnswer = "C",
                    Difficulty = "Medium"
                },
                new TestQuestion
                {
                    Category = "General",
                    QuestionText = "What is the primary goal of mentorship?",
                    OptionA = "To provide all the answers",
                    OptionB = "To help mentees develop skills and find their own solutions",
                    OptionC = "To complete tasks for the mentee",
                    OptionD = "To build a network",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },

                // ==================== Software Engineering - Coding ====================
                new TestQuestion
                {
                    Category = "Software Engineering",
                    QuestionText = "What is the output of this code?\n\nint x = 5;\nint y = x++ + ++x;\nConsole.WriteLine(y);",
                    OptionA = "10",
                    OptionB = "11",
                    OptionC = "12",
                    OptionD = "13",
                    CorrectAnswer = "C",
                    Difficulty = "Hard",
                    Explanation = "x++ returns 5 then increments to 6, ++x increments to 7 then returns 7. So 5+7=12"
                },
                new TestQuestion
                {
                    Category = "Software Engineering",
                    QuestionText = "Which code correctly implements the Singleton pattern in C#?",
                    OptionA = "public class S { public static S Instance = new S(); }",
                    OptionB = "public class S { private static S _i; public static S I => _i ??= new S(); private S(){} }",
                    OptionC = "public class S { public S GetInstance() => new S(); }",
                    OptionD = "public class S { static S() { } }",
                    CorrectAnswer = "B",
                    Difficulty = "Medium",
                    Explanation = "Singleton requires private constructor and lazy initialization."
                },
                new TestQuestion
                {
                    Category = "Software Engineering",
                    QuestionText = "What is the time complexity of this code?\n\nfor(int i=0; i<n; i++)\n  for(int j=i; j<n; j++)\n    sum++;",
                    OptionA = "O(n)",
                    OptionB = "O(n log n)",
                    OptionC = "O(n²)",
                    OptionD = "O(2ⁿ)",
                    CorrectAnswer = "C",
                    Difficulty = "Medium",
                    Explanation = "Nested loops with n iterations each = O(n²)"
                },
                new TestQuestion
                {
                    Category = "Software Engineering",
                    QuestionText = "What does SOLID's 'L' (Liskov Substitution) principle mean?",
                    OptionA = "Classes should be open for extension",
                    OptionB = "Subclasses must be substitutable for their base classes",
                    OptionC = "Interfaces should be small and specific",
                    OptionD = "Depend on abstractions not concretions",
                    CorrectAnswer = "B",
                    Difficulty = "Medium"
                },
                new TestQuestion
                {
                    Category = "Software Engineering",
                    QuestionText = "What is the output?\n\nstring[] arr = {\"a\", \"b\", \"c\"};\nvar result = arr.Where(x => x != \"b\").First();\nConsole.WriteLine(result);",
                    OptionA = "a",
                    OptionB = "b",
                    OptionC = "c",
                    OptionD = "Error",
                    CorrectAnswer = "A",
                    Difficulty = "Easy"
                },
                new TestQuestion
                {
                    Category = "Software Engineering",
                    QuestionText = "What design pattern does this represent?\n\ninterface ICommand { void Execute(); }\nclass Invoker { ICommand cmd; void Run() => cmd.Execute(); }",
                    OptionA = "Observer Pattern",
                    OptionB = "Command Pattern",
                    OptionC = "Strategy Pattern",
                    OptionD = "Factory Pattern",
                    CorrectAnswer = "B",
                    Difficulty = "Medium"
                },

                // ==================== Web Development - Coding ====================
                new TestQuestion
                {
                    Category = "Web Development",
                    QuestionText = "What does this JavaScript code output?\n\nconsole.log(typeof null);",
                    OptionA = "null",
                    OptionB = "undefined",
                    OptionC = "object",
                    OptionD = "Error",
                    CorrectAnswer = "C",
                    Difficulty = "Medium",
                    Explanation = "This is a known JavaScript quirk - typeof null returns 'object'"
                },
                new TestQuestion
                {
                    Category = "Web Development",
                    QuestionText = "What is the output?\n\nconst arr = [1, 2, 3];\narr[10] = 11;\nconsole.log(arr.length);",
                    OptionA = "3",
                    OptionB = "4",
                    OptionC = "10",
                    OptionD = "11",
                    CorrectAnswer = "D",
                    Difficulty = "Medium"
                },
                new TestQuestion
                {
                    Category = "Web Development",
                    QuestionText = "In React, what is the correct way to update state based on previous state?",
                    OptionA = "setState(count + 1)",
                    OptionB = "setState(prev => prev + 1)",
                    OptionC = "state.count = count + 1",
                    OptionD = "this.count++",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },
                new TestQuestion
                {
                    Category = "Web Development",
                    QuestionText = "What HTTP status code should be returned for a successful POST that creates a resource?",
                    OptionA = "200 OK",
                    OptionB = "201 Created",
                    OptionC = "204 No Content",
                    OptionD = "202 Accepted",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },
                new TestQuestion
                {
                    Category = "Web Development",
                    QuestionText = "What is the output?\n\nPromise.resolve(1)\n  .then(x => x + 1)\n  .then(x => { throw new Error(); })\n  .catch(() => 3)\n  .then(x => console.log(x));",
                    OptionA = "1",
                    OptionB = "2",
                    OptionC = "3",
                    OptionD = "Error",
                    CorrectAnswer = "C",
                    Difficulty = "Hard"
                },
                new TestQuestion
                {
                    Category = "Web Development",
                    QuestionText = "Which CSS property creates a flex container?",
                    OptionA = "display: block",
                    OptionB = "display: flex",
                    OptionC = "position: flex",
                    OptionD = "flex: 1",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },

                // ==================== Data Science - Coding ====================
                new TestQuestion
                {
                    Category = "Data Science",
                    QuestionText = "What does this pandas code return?\n\ndf = pd.DataFrame({'A': [1, 2, 3], 'B': [4, 5, 6]})\ndf['A'].mean()",
                    OptionA = "1",
                    OptionB = "2",
                    OptionC = "3",
                    OptionD = "6",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },
                new TestQuestion
                {
                    Category = "Data Science",
                    QuestionText = "What is the shape of the result?\n\nimport numpy as np\na = np.array([[1,2],[3,4]])\nb = np.dot(a, a)",
                    OptionA = "(2,)",
                    OptionB = "(2, 2)",
                    OptionC = "(4,)",
                    OptionD = "(4, 4)",
                    CorrectAnswer = "B",
                    Difficulty = "Medium"
                },
                new TestQuestion
                {
                    Category = "Data Science",
                    QuestionText = "Which sklearn function splits data for training and testing?",
                    OptionA = "model_selection.split()",
                    OptionB = "train_test_split()",
                    OptionC = "data_split()",
                    OptionD = "cross_validate()",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },
                new TestQuestion
                {
                    Category = "Data Science",
                    QuestionText = "What does this code do?\n\nfrom sklearn.preprocessing import StandardScaler\nscaler = StandardScaler()\nX_scaled = scaler.fit_transform(X)",
                    OptionA = "Normalizes data to 0-1 range",
                    OptionB = "Standardizes data to mean=0, std=1",
                    OptionC = "Removes outliers",
                    OptionD = "Encodes categorical variables",
                    CorrectAnswer = "B",
                    Difficulty = "Medium"
                },
                new TestQuestion
                {
                    Category = "Data Science",
                    QuestionText = "What metric is best for imbalanced classification?",
                    OptionA = "Accuracy",
                    OptionB = "F1-Score",
                    OptionC = "Mean Squared Error",
                    OptionD = "R-squared",
                    CorrectAnswer = "B",
                    Difficulty = "Medium"
                },

                // ==================== Machine Learning - Coding ====================
                new TestQuestion
                {
                    Category = "Machine Learning",
                    QuestionText = "What activation function is commonly used in the output layer for binary classification?",
                    OptionA = "ReLU",
                    OptionB = "Sigmoid",
                    OptionC = "Tanh",
                    OptionD = "Softmax",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },
                new TestQuestion
                {
                    Category = "Machine Learning",
                    QuestionText = "What does this Keras code create?\n\nmodel.add(Dense(64, activation='relu'))\nmodel.add(Dropout(0.5))",
                    OptionA = "A layer that removes 50% of neurons during training",
                    OptionB = "A layer that removes 50% of data",
                    OptionC = "A regularization penalty",
                    OptionD = "A batch normalization layer",
                    CorrectAnswer = "A",
                    Difficulty = "Medium"
                },
                new TestQuestion
                {
                    Category = "Machine Learning",
                    QuestionText = "What is the purpose of the learning rate in gradient descent?",
                    OptionA = "Controls model complexity",
                    OptionB = "Controls how big steps we take towards minimum",
                    OptionC = "Controls number of epochs",
                    OptionD = "Controls batch size",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },
                new TestQuestion
                {
                    Category = "Machine Learning",
                    QuestionText = "What does model.fit() do in scikit-learn?",
                    OptionA = "Makes predictions",
                    OptionB = "Trains the model on data",
                    OptionC = "Evaluates the model",
                    OptionD = "Preprocesses data",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },

                // ==================== Mobile Development - Coding ====================
                new TestQuestion
                {
                    Category = "Mobile Development",
                    QuestionText = "In Flutter, what widget is used for scrollable lists?",
                    OptionA = "Column",
                    OptionB = "ListView",
                    OptionC = "Stack",
                    OptionD = "Container",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },
                new TestQuestion
                {
                    Category = "Mobile Development",
                    QuestionText = "What is the correct way to navigate in React Native?\n\nnavigation.___('Screen2')",
                    OptionA = "go",
                    OptionB = "push",
                    OptionC = "navigate",
                    OptionD = "move",
                    CorrectAnswer = "C",
                    Difficulty = "Easy"
                },
                new TestQuestion
                {
                    Category = "Mobile Development",
                    QuestionText = "In Swift, what keyword makes a property observable?",
                    OptionA = "@State",
                    OptionB = "@Published",
                    OptionC = "@Observable",
                    OptionD = "@Binding",
                    CorrectAnswer = "B",
                    Difficulty = "Medium"
                },
                new TestQuestion
                {
                    Category = "Mobile Development",
                    QuestionText = "What is the Android equivalent of iOS's UIViewController?",
                    OptionA = "View",
                    OptionB = "Activity",
                    OptionC = "Fragment",
                    OptionD = "Service",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },

                // ==================== UI/UX Design ====================
                new TestQuestion
                {
                    Category = "UI/UX Design",
                    QuestionText = "What is the ideal touch target size for mobile buttons (in pixels)?",
                    OptionA = "24x24",
                    OptionB = "32x32",
                    OptionC = "44x44",
                    OptionD = "64x64",
                    CorrectAnswer = "C",
                    Difficulty = "Medium",
                    Explanation = "Apple's HIG recommends 44x44 points minimum"
                },
                new TestQuestion
                {
                    Category = "UI/UX Design",
                    QuestionText = "What is 'F-pattern' in UX?",
                    OptionA = "A layout grid system",
                    OptionB = "How users scan web pages",
                    OptionC = "A color scheme",
                    OptionD = "A navigation pattern",
                    CorrectAnswer = "B",
                    Difficulty = "Medium"
                },
                new TestQuestion
                {
                    Category = "UI/UX Design",
                    QuestionText = "What is the purpose of a design system?",
                    OptionA = "To replace designers",
                    OptionB = "To ensure consistency across products",
                    OptionC = "To speed up loading times",
                    OptionD = "To reduce server costs",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },
                new TestQuestion
                {
                    Category = "UI/UX Design",
                    QuestionText = "What Figma feature allows reusable components?",
                    OptionA = "Frames",
                    OptionB = "Components",
                    OptionC = "Groups",
                    OptionD = "Layers",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },

                // ==================== DevOps ====================
                new TestQuestion
                {
                    Category = "DevOps",
                    QuestionText = "What does this Docker command do?\n\ndocker run -d -p 8080:80 nginx",
                    OptionA = "Builds an image",
                    OptionB = "Runs container in background, maps port 8080 to 80",
                    OptionC = "Stops a container",
                    OptionD = "Lists containers",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },
                new TestQuestion
                {
                    Category = "DevOps",
                    QuestionText = "What is the purpose of a Kubernetes Pod?",
                    OptionA = "Storage volume",
                    OptionB = "Smallest deployable unit containing containers",
                    OptionC = "Network configuration",
                    OptionD = "Load balancer",
                    CorrectAnswer = "B",
                    Difficulty = "Medium"
                },
                new TestQuestion
                {
                    Category = "DevOps",
                    QuestionText = "What does CI/CD stand for?",
                    OptionA = "Code Integration / Code Deployment",
                    OptionB = "Continuous Integration / Continuous Delivery",
                    OptionC = "Container Integration / Container Delivery",
                    OptionD = "Cloud Integration / Cloud Deployment",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },
                new TestQuestion
                {
                    Category = "DevOps",
                    QuestionText = "What command shows running Docker containers?",
                    OptionA = "docker list",
                    OptionB = "docker ps",
                    OptionC = "docker show",
                    OptionD = "docker containers",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },

                // ==================== Project Management ====================
                new TestQuestion
                {
                    Category = "Project Management",
                    QuestionText = "In Scrum, who is responsible for the product backlog?",
                    OptionA = "Scrum Master",
                    OptionB = "Product Owner",
                    OptionC = "Development Team",
                    OptionD = "Stakeholders",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },
                new TestQuestion
                {
                    Category = "Project Management",
                    QuestionText = "What is a 'burndown chart' used for?",
                    OptionA = "Tracking budget",
                    OptionB = "Tracking remaining work over time",
                    OptionC = "Tracking team velocity",
                    OptionD = "Tracking defects",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },
                new TestQuestion
                {
                    Category = "Project Management",
                    QuestionText = "What is 'velocity' in Agile?",
                    OptionA = "Speed of deployment",
                    OptionB = "Amount of work completed per sprint",
                    OptionC = "Number of bugs fixed",
                    OptionD = "Team size",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },

                // ==================== Cybersecurity ====================
                new TestQuestion
                {
                    Category = "Cybersecurity",
                    QuestionText = "What type of attack is this?\n\nSELECT * FROM users WHERE id = '1' OR '1'='1'",
                    OptionA = "XSS",
                    OptionB = "SQL Injection",
                    OptionC = "CSRF",
                    OptionD = "DDoS",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },
                new TestQuestion
                {
                    Category = "Cybersecurity",
                    QuestionText = "What does HTTPS provide that HTTP doesn't?",
                    OptionA = "Faster loading",
                    OptionB = "Encryption and authentication",
                    OptionC = "Better SEO",
                    OptionD = "Larger file transfers",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },
                new TestQuestion
                {
                    Category = "Cybersecurity",
                    QuestionText = "What is the purpose of salting passwords?",
                    OptionA = "Makes passwords longer",
                    OptionB = "Prevents rainbow table attacks",
                    OptionC = "Encrypts the database",
                    OptionD = "Speeds up verification",
                    CorrectAnswer = "B",
                    Difficulty = "Medium"
                },
                new TestQuestion
                {
                    Category = "Cybersecurity",
                    QuestionText = "What is a JWT used for?",
                    OptionA = "Database encryption",
                    OptionB = "Stateless authentication tokens",
                    OptionC = "File compression",
                    OptionD = "Network monitoring",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },

                // ==================== Cloud Computing ====================
                new TestQuestion
                {
                    Category = "Cloud Computing",
                    QuestionText = "What AWS service is used for serverless functions?",
                    OptionA = "EC2",
                    OptionB = "Lambda",
                    OptionC = "S3",
                    OptionD = "RDS",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                },
                new TestQuestion
                {
                    Category = "Cloud Computing",
                    QuestionText = "What is the difference between IaaS and PaaS?",
                    OptionA = "IaaS provides infrastructure, PaaS provides platform",
                    OptionB = "They are the same",
                    OptionC = "IaaS is more expensive",
                    OptionD = "PaaS is only for databases",
                    CorrectAnswer = "A",
                    Difficulty = "Medium"
                },
                new TestQuestion
                {
                    Category = "Cloud Computing",
                    QuestionText = "What is Azure Blob Storage used for?",
                    OptionA = "Running virtual machines",
                    OptionB = "Storing unstructured data (files, images)",
                    OptionC = "Running SQL databases",
                    OptionD = "Load balancing",
                    CorrectAnswer = "B",
                    Difficulty = "Easy"
                }
            };

            await context.TestQuestions.AddRangeAsync(questions);
            await context.SaveChangesAsync();
        }
    }
}
