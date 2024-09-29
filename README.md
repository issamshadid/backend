# Contributing to Template

Thank you for considering contributing to this project! We appreciate your help in improving and expanding the project.

To ensure a smooth contribution process, please read the following guidelines before contributing.

---

## Table of Contents
- [How to Contribute](#how-to-contribute)
- [Code of Conduct](#code-of-conduct)
- [Types of Contributions](#types-of-contributions)
- [Getting Started](#getting-started)
- [How to Submit Changes](#how-to-submit-changes)
- [Style Guidelines](#style-guidelines)
- [Code Reviews](#code-reviews)
- [Reporting Issues](#reporting-issues)
---

## How to Contribute

### Reporting Bugs or Requesting Features
If you find a bug or want to request a feature, feel free to open an issue on GitHub. Before creating a new issue, please check to ensure it hasn't been reported already.

1. Navigate to the **Issues** section of the repository.
2. Click **New Issue**.
3. Provide as much detail as possible, including steps to reproduce (for bugs) or a clear description of the feature you want (for feature requests).

### Submitting Code Changes
If you are looking to contribute code, please follow these steps:

1. **Fork the repository**: Click the "Fork" button at the top-right of the repository page.
2. **Create a new branch**: 
   ```bash
   git checkout -b your-branch-name
   ```
3. **Make your changes**: Add features or fix bugs.
4. **Test your changes**: Ensure that all tests pass before submitting a pull request.
5. **Submit a pull request**: Push your changes to your fork and open a pull request to the `main` branch of this repository.

---

## Code of Conduct

We are committed to fostering a welcoming, respectful, and collaborative environment for everyone. Please follow our [Code of Conduct](#) (https://github.com/issamshadid/backend/blob/main/CODE_OF_CONDUCT.md).

---

## Types of Contributions

### Bug Fixes
If you notice a bug or issue with the project, feel free to submit a bug report or even fix the bug yourself. Always check the open issues to see if someone else is already working on it.

### New Features
We're always open to new ideas! If you have a feature in mind, please open an issue to discuss it before creating a pull request. This ensures that your effort aligns with the project's direction and isn't duplicated.

### Documentation
Contributions to documentation are highly valued. Whether it’s improving existing documentation, adding missing pieces, or creating new tutorials, your contributions will help others understand and use the project better.

---

Here’s the updated section for your **CONTRIBUTING.md** file, reflecting the technologies used in your project:

---

## Getting Started

### Prerequisites
Ensure you have the following installed before starting:

- **.NET Core SDK 8**: You can download the latest version of the .NET Core SDK from [here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).
- **Entity Framework Core**: This is included in the dependencies, but you may want to be familiar with [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) for working with the database.
- **SQL Server**: Ensure that you have access to a SQL Server instance. If you're setting it up locally, you can download SQL Server from [here](https://www.microsoft.com/en-us/sql-server/sql-server-downloads).

### Setting Up the Project Locally

1. **Clone the repository**:
   ```bash
   git clone https://github.com/yourusername/yourproject.git
   cd yourproject
   ```

2. **Install dependencies**:
   Restore the necessary .NET and Entity Framework dependencies:
   ```bash
   dotnet restore
   ```

3. **Set up the database**:
   Ensure that your connection string in the `appsettings.json` file points to your SQL Server instance. Run migrations to set up the database schema:
   ```bash
   dotnet ef database update
   ```

4. **Run the project**:
   Start the application:
   ```bash
   dotnet run
   ```

---

This will guide your contributors through the necessary setup steps, including ensuring they have the correct versions of .NET, Entity Framework, and SQL Server. Let me know if you need further adjustments!

---

## How to Submit Changes

### Commit Message Guidelines
Please write meaningful commit messages that describe the reason for the change. Use the following format for your commits:

- **Type**: `fix`, `feat`, `docs`, `style`, `refactor`, `test`, `chore`.
- **Scope**: A short description of the component you’re working on.
- **Message**: Describe what was changed and why.

Example:
```
feat(auth): add login functionality with OAuth support
```

To maintain high code quality and ensure that contributions to your .NET project align with the project's goals, it's important to have clear **Pull Request (PR) Guidelines**. Here's a customized **Pull Request Guidelines** section for your `.NET Core 8` project using Entity Framework and SQL Server:

---

## Pull Request Guidelines

When submitting a pull request to the project, please follow these guidelines to ensure a smooth review and integration process.

### 1. **Create a Separate Branch**
Ensure that your work is done in a separate branch. This keeps the `main` or `master` branch clean and makes it easier to review changes.

- Branch naming convention:
  ```bash
  git checkout -b feature/your-feature-name
  git checkout -b fix/issue-description
  ```

### 2. **Check for Code Style Consistency**
Follow the code style and conventions used in the project. Here are some common guidelines:
- Use **4 spaces** for indentation.
- Use **camelCase** for local variables and parameters.
- Use **PascalCase** for method names, classes, and public properties.
- Keep methods short and focused; each method should perform a single task.

### 3. **Run Static Code Analysis (Optional)**
If your project uses static code analysis tools like **StyleCop**, **Roslyn Analyzers**, or **SonarQube**, make sure to run the analysis before submitting the PR to ensure that there are no style violations or critical issues.

Example:
```bash
dotnet build
```

### 4. **Database Changes (Migrations)**
If your changes include database modifications (via **Entity Framework Migrations**):
- Make sure you have added a new migration using the following command:
  ```bash
  dotnet ef migrations add MigrationName
  ```
- Run the migration against the local database to ensure it works:
  ```bash
  dotnet ef database update
  ```
- Include the generated migration files in the pull request.

### 5. **Commit Message Format**
Ensure that your commit messages are clear and descriptive. Use the following format for your commit messages:

- **Type**: `feat`, `fix`, `docs`, `style`, `refactor`, `test`, `chore`.
- **Scope**: A short description of the component you’re working on (e.g., `auth`, `db`, `api`).
- **Message**: Describe what was changed and why.

Example:
```
feat(api): add user authentication with JWT
fix(db): resolve issue with foreign key constraints in User table
```

### 6. **Resolve Merge Conflicts**
If there are any merge conflicts with the `main` or `master` branch, resolve them locally before submitting the pull request.

1. Pull the latest changes from `main`:
   ```bash
   git checkout main
   git pull origin main
   ```
2. Switch back to your branch and rebase:
   ```bash
   git checkout your-branch-name
   git rebase main
   ```

### 7. **Pull Request Description**
In the pull request description, make sure to include:
- **What was changed**: A brief explanation of the changes introduced in the PR.
- **Why the change was necessary**: Explain the context and the problem your change addresses.
- **Related issues**: Reference any related issues by using the syntax `Closes #<issue-number>` if the PR resolves an open issue.
- **Screenshots (if applicable)**: If your PR affects the UI or specific functionality, include screenshots to make it easier to review the changes.

### 8. **Limit the Scope of the PR**
Each pull request should focus on a single issue or feature. Avoid submitting "mega-PRs" with multiple unrelated changes, as these are difficult to review.

### 9. **Code Review Process**
After submitting the PR:
- Your PR will be reviewed by one or more project maintainers.
- Respond to feedback and be open to making adjustments.
- Be prepared to explain design decisions, especially if introducing significant architectural changes.
- Ensure that tests pass and the build is successful after each review iteration.

### 10. **Follow-Up on the Review**
If requested, update your pull request based on feedback and push changes toyour branch. You can update your pull request by simply pushing new commits to the same branch.

1. **Make the requested changes** on your local branch.
2. **Push the updates** to your remote branch:
   ```bash
   git push origin your-branch-name
   ```
3. Once all feedback has been addressed and the changes are approved, the PR will be merged into the `main` branch.

---

By following these **Pull Request Guidelines**, you help ensure that the project maintains high-quality code and that contributions are easy to review and integrate. Thank you for your contribution!

---

Here’s a detailed **Style Guidelines** section for your `.NET Core` project, including **Code Style** conventions. These guidelines will help contributors maintain consistent code formatting and structure throughout the project.

---

## Style Guidelines

### Code Style

To ensure a clean and readable codebase, please follow the code style guidelines below when contributing to the project.

#### 1. **Indentation**
- Use **4 spaces** for indentation (no tabs).
- Ensure that code is consistently indented throughout files.

#### 2. **Brace Style**
- Use **K&R** (Kernighan & Ritchie) style braces:
  - Opening brace `{` on the same line as the declaration.
  - Closing brace `}` on a new line.
  
  Example:
  ```csharp
  public class Example
  {
      public void DoSomething()
      {
          if (condition)
          {
              // Do work here
          }
          else
          {
              // Handle alternative
          }
      }
  }
  ```

#### 3. **Variable Naming**
- **camelCase** for local variables, method parameters, and private fields.
  - Example: `int userId = 10;`
- **PascalCase** for method names, public properties, and class names.
  - Example: `public string UserName { get; set; }`
  
#### 4. **Method and Function Naming**
- Method names should use **PascalCase**.
- Method names should be descriptive and use action verbs (e.g., `GetData`, `SaveUser`, `ProcessOrder`).
  
  Example:
  ```csharp
  public void SaveUserData(User user)
  {
      // Implementation here
  }
  ```

#### 5. **Class and Interface Naming**
- Class names should be **PascalCase** and nouns that clearly describe the entity.
- Interface names should be prefixed with an `I` and follow **PascalCase**.
  
  Example:
  ```csharp
  public class UserRepository { }

  public interface IUserService { }
  ```

#### 6. **Comments**
- Use comments to explain **why** the code exists, especially for complex logic.
- Use **XML comments** (`///`) for public methods and classes to provide context and documentation.
  
  Example:
  ```csharp
  /// <summary>
  /// Retrieves user data from the database.
  /// </summary>
  /// <param name="userId">The ID of the user.</param>
  /// <returns>User data.</returns>
  public User GetUserById(int userId)
  {
      // Explanation of complex logic if necessary
      return _userRepository.Get(userId);
  }
  ```

#### 7. **Spacing and Readability**
- Add a single space between operators and expressions:
  ```csharp
  int result = a + b;
  ```
- Add blank lines to separate logical sections of your code (e.g., between method definitions, within a method before returning values).
- No trailing whitespace at the end of lines.

#### 8. **Line Length**
- Keep lines to a maximum of **120 characters**. If a line exceeds this length, consider refactoring it for better readability.

#### 9. **Constants**
- Use **PascalCase** for constant names and declare them as `readonly` or `const` where appropriate.
  
  Example:
  ```csharp
  public const int MaxItems = 100;
  ```

#### 10. **LINQ Queries**
- For **LINQ** queries, prefer **method syntax** over **query syntax** unless query syntax improves readability.
  
  Example (preferred method syntax):
  ```csharp
  var users = usersList.Where(u => u.IsActive).OrderBy(u => u.Name).ToList();
  ```

#### 11. **Dependency Injection**
- Use constructor injection for dependencies whenever possible.
  
  Example:
  ```csharp
  public class UserService
  {
      private readonly IUserRepository _userRepository;

      public UserService(IUserRepository userRepository)
      {
          _userRepository = userRepository;
      }
  }
  ```

#### 12. **Async/Await Best Practices**
- Use **async/await** for all asynchronous calls.
- Methods returning asynchronous tasks should be named with the `Async` suffix.
  
  Example:
  ```csharp
  public async Task<User> GetUserAsync(int userId)
  {
      return await _userRepository.GetUserByIdAsync(userId);
  }
  ```

#### 13. **Properties**
- Use **auto-properties** where possible:
  
  Example:
  ```csharp
  public string Name { get; set; }
  ```

#### 14. **Null Checking**
- Use `??` (null coalescing operator) for simple null checks.
  
  Example:
  ```csharp
  string name = user?.Name ?? "Default Name";
  ```

- For more complex null checks or when you expect a thrown exception, use:
  
  ```csharp
  if (user == null)
  {
      throw new ArgumentNullException(nameof(user));
  }
  ```

---

### Automated Code Style Checks
If applicable, run automated style checks before submitting your pull request:
- **Linting Tool**: Ensure that code passes any static analysis or linting tools configured for the project (e.g., StyleCop or .NET Analyzers).
- Example:
  ```bash
  dotnet format
  ```

---

By adhering to these style guidelines, we can maintain a consistent and clean codebase, making it easier for everyone to contribute, read, and maintain the code.

---

## Code Reviews

All pull requests will be reviewed by a project maintainer before merging. Be prepared to make adjustments based on feedback, and try to respond to review comments in a timely manner.

---

## Reporting Issues

When submitting an issue, please include as much detail as possible:
- A clear title and description.
- Steps to reproduce the issue.
- Screenshots or logs if applicable.

### Security Vulnerabilities
If you discover a security vulnerability, please **do not** open a public issue. Instead, report it privately by contacting the project maintainers at [shadeed.1990@gmail.com].

---
