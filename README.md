# Smart Recruitment Platform (SRP)

A web-based **Smart Recruitment Platform** that connects job seekers and employers through authentication, job management, applications, profiles, CV management, notifications, and intelligent job matching.

## Project Overview

The Smart Recruitment Platform provides separate functionality for:

- **Job Seekers** – create and manage profiles, skills, education, experience and CVs, search for jobs, apply for jobs, view applications, receive notifications and use job matching.
- **Employers** – manage employer/company information, create and manage jobs, view applicants, update application statuses and send contact requests.
- **Administrators** – view platform statistics, manage users and activate/deactivate user accounts.
- **Authentication** – registration, login and JWT-based authorization.
- **Job Matching** – calculate job suitability using skills, experience, education and location.

## Main Features

### 1. Authentication & Authorization

- User registration
- User login
- Password hashing using BCrypt
- JWT authentication
- Role-based authorization
- Supported roles:
  - `JobSeeker`
  - `Employer`
  - `Admin`
- Active/inactive account validation

### 2. Job Seeker Module

- Job seeker dashboard
- Profile management
- Skills management
- Education management
- Experience management
- CV management
- Job browsing
- Job details
- Job applications
- Application status tracking
- Notifications
- Skill-gap checking
- Job matching
- Employer contact request handling

### 3. Employer Module

- Employer dashboard
- Company profile management
- Job creation
- Job editing
- Job closing
- Job listing
- Applicant viewing
- Applicant application-status management
- Contact request management

### 4. Admin Module

- Admin dashboard
- User management
- User details
- Activate/deactivate users
- Admin profile/settings pages
- Platform-level user monitoring

### 5. Smart Job Matching

The matching system calculates a suitability score using configurable weights:

| Matching Factor | Weight |
|---|---:|
| Skills | 50% |
| Experience | 25% |
| Education | 15% |
| Location | 10% |

The weights are configured in `Backend/appsettings.json` under `Member4MatchingWeights`.

## Technology Stack

### Backend

- C#
- ASP.NET Core 8
- Entity Framework Core 8
- SQL Server / LocalDB
- JWT Bearer Authentication
- BCrypt password hashing
- Swagger / OpenAPI
- Repository Pattern
- Service Layer

### Frontend

- HTML5
- CSS3
- JavaScript
- Fetch API
- Role-based pages

### Database

- Microsoft SQL Server
- LocalDB for local development
- Entity Framework Core migrations

## Project Structure

```text
SRP/
│
├── Backend/
│   ├── Controllers/
│   │   ├── Admin/
│   │   ├── Authentication/
│   │   ├── Employer/
│   │   ├── JobMatching/
│   │   └── JobSeeker/
│   │
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   │
│   ├── DTOs/
│   ├── Models/
│   ├── Repositories/
│   ├── Services/
│   ├── Migrations/
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── Program.cs
│
├── Frontend/
│   ├── css/
│   ├── js/
│   └── pages/
│       ├── Admin/
│       ├── Authentication/
│       ├── Employer/
│       ├── JobMatching/
│       └── JobSeeker/
│
├── SmartRecruitmentPlatform.csproj
└── SmartRecruitmentPlatform.slnx
```

## Requirements

Before running the project, install:

1. **.NET 8 SDK**
2. **SQL Server LocalDB** or SQL Server
3. **Visual Studio 2022** or **Visual Studio Code**
4. **Git** (for version control)

Check the .NET installation:

```bash
dotnet --version
```

The project targets:

```text
.NET 8.0
```

## Database Configuration

The default database configuration uses SQL Server LocalDB:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=SmartRecruitmentDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

If you use another SQL Server instance, update the `DefaultConnection` value in:

```text
Backend/appsettings.json
```

## JWT Configuration

JWT settings are stored in:

```text
Backend/appsettings.json
```

Example structure:

```json
"JwtSettings": {
  "Key": "CHANGE_THIS_TO_A_SECURE_SECRET",
  "Issuer": "SmartRecruitmentPlatform",
  "Audience": "SmartRecruitmentPlatformUsers",
  "ExpiryMinutes": 60
}
```

**Important:** For production, do not commit real JWT secrets, database passwords or other sensitive credentials to GitHub. Use environment variables or a secure secrets manager.

## Database Setup

The project already contains Entity Framework Core migrations.

Available migrations include:

```text
InitialCreate
JobMatching
AddIsActiveToUser
AddRemainingProjectTables
```

From the project root, restore the dependencies:

```bash
dotnet restore
```

Apply the database migrations:

```bash
dotnet ef database update
```

If `dotnet ef` is not installed, install it with:

```bash
dotnet tool install --global dotnet-ef
```

Then run:

```bash
dotnet ef database update
```

This creates/updates the `SmartRecruitmentDB` database according to the project's migrations.

## Running the Application

Open a terminal in the project root:

```bash
cd SRP
```

Restore packages:

```bash
dotnet restore
```

Update the database:

```bash
dotnet ef database update
```

Run the application:

```bash
dotnet run
```

The ASP.NET Core application serves the frontend from the `Frontend` folder and exposes the backend API through controllers.

After starting the application, use the URL shown in the terminal.

## Swagger API Documentation

When the application runs in the Development environment, Swagger is enabled.

Open:

```text
/swagger
```

or:

```text
/swagger/index.html
```

Swagger can be used to:

- View available API endpoints
- Test GET/POST/PUT/DELETE requests
- Test JWT-protected endpoints
- Enter a Bearer token for authorized API calls

For protected endpoints, use:

```text
Bearer YOUR_JWT_TOKEN
```

## Important API Endpoints

### Authentication

```text
POST /api/Auth/register
POST /api/Auth/login
GET  /api/Auth/test
GET  /api/Auth/admin-test
```

### Admin

```text
GET /api/admin/dashboard
GET /api/admin/users
GET /api/admin/users/{id}
PUT /api/admin/users/{id}/status
```

### Job Seeker

```text
GET    /api/jobseeker/profile
PUT    /api/jobseeker/profile

GET    /api/jobseeker/skills
POST   /api/jobseeker/skills
PUT    /api/jobseeker/skills/{skillId}
DELETE /api/jobseeker/skills/{skillId}

GET    /api/jobseeker/education
POST   /api/jobseeker/education
PUT    /api/jobseeker/education/{educationId}
DELETE /api/jobseeker/education/{educationId}

GET    /api/jobseeker/experience
POST   /api/jobseeker/experience
PUT    /api/jobseeker/experience/{experienceId}
DELETE /api/jobseeker/experience/{experienceId}

GET    /api/jobseeker/cv
POST   /api/jobseeker/cv
DELETE /api/jobseeker/cv/{cvId}
```

### Job Matching

```text
GET  /api/job-matching/health
GET  /api/job-matching/demo-profile
GET  /api/job-matching/jobs
GET  /api/job-matching/jobs/{jobId}
POST /api/job-matching/jobs/{jobId}/apply
GET  /api/job-matching/applications
```

### Employer

Employer APIs cover:

- Employer information
- Company management
- Job management
- Applicant management
- Application status updates
- Contact requests

Examples:

```text
GET  /api/Employer/{employerId}

GET  /api/Company/{companyId}
GET  /api/Company/employer/{employerId}
POST /api/Company
PUT  /api/Company/{companyId}

GET  /api/Job/{jobId}
GET  /api/Job/company/{companyId}
POST /api/Job
PUT  /api/Job/{jobId}
PUT  /api/Job/{jobId}/close
```

## Default Admin Account

The application currently contains a temporary startup seed for an administrator account.

The configured account is:

```text
Email: admin@gmail.com
```

The password is defined directly in `Backend/Program.cs`.

**For security, change the password and remove the temporary seed code before deploying the application to production.**

## User Flow

### Job Seeker

```text
Register
   ↓
Login
   ↓
Job Seeker Dashboard
   ↓
Complete Profile
   ↓
Add Skills / Education / Experience / CV
   ↓
Browse Jobs
   ↓
View Job Details
   ↓
Check Matching / Skill Gap
   ↓
Apply for Job
   ↓
Track Application
   ↓
Receive Notifications
```

### Employer

```text
Register
   ↓
Login
   ↓
Employer Dashboard
   ↓
Create Company Profile
   ↓
Create Job
   ↓
View Applicants
   ↓
Review Applications
   ↓
Update Application Status
   ↓
Contact Suitable Job Seekers
```

### Administrator

```text
Login
   ↓
Admin Dashboard
   ↓
View Users
   ↓
View User Details
   ↓
Activate / Deactivate Users
   ↓
Monitor Platform
```

## Architecture

The backend follows a layered architecture:

```text
Frontend
   │
   ▼
Controllers
   │
   ▼
Services
   │
   ▼
Repositories
   │
   ▼
Entity Framework Core
   │
   ▼
SQL Server Database
```

### Controllers

Handle HTTP requests and responses.

### Services

Contain business logic and application rules.

### Repositories

Handle database access and data operations.

### Entity Framework Core

Maps C# models to SQL Server database tables.

## Database Entities

The main database entities include:

- User
- Employer
- Company
- Job
- Application
- ContactRequest
- JobSeekerProfile
- JobSeekerSkill
- Education
- Experience
- CvDocument
- Notification

## Security

The application uses:

- JWT Bearer authentication
- Role-based authorization
- BCrypt password hashing
- Active-account validation
- Protected API endpoints

Never store plain-text passwords in the database.

## GitHub Setup

If the project is not yet pushed to GitHub:

```bash
git init
git add .
git commit -m "Initial project submission"
git branch -M main
git remote add origin YOUR_GITHUB_REPOSITORY_URL
git push -u origin main
```

Before pushing, make sure sensitive information is not committed.

Recommended files/folders to keep out of Git:

```text
bin/
obj/
.vs/
*.user
*.suo
```

The project already contains a `.gitignore`; review it before pushing.

## Troubleshooting

### Database connection error

Check:

- SQL Server LocalDB is installed
- LocalDB service is available
- `DefaultConnection` is correct
- Database migrations have been applied

Try:

```bash
dotnet ef database update
```

### Swagger does not appear

Make sure the application is running in the Development environment.

### 401 Unauthorized

Check that:

- You have logged in successfully
- The JWT token has not expired
- The token is sent in the `Authorization` header
- The format is:

```text
Bearer YOUR_TOKEN
```

### 403 Forbidden

The logged-in user may not have the required role.

For example, Admin endpoints require:

```text
Role = Admin
```

Employer endpoints require:

```text
Role = Employer
```

Job Seeker endpoints require:

```text
Role = JobSeeker
```

### Frontend page does not load

Run the application using:

```bash
dotnet run
```

The backend is configured to serve files from:

```text
Frontend/
```

Open the application through the ASP.NET Core server rather than opening HTML files directly from the file system.

## Development Notes

This project is intended as an academic/project implementation of a smart recruitment platform. The current configuration is suitable for local development and demonstration.

For production deployment, additionally configure:

- Production database
- Secure JWT secret
- HTTPS
- Secure environment variables
- Proper CORS policy if frontend and backend are hosted separately
- Production logging
- Error handling
- File upload validation
- Secure administrator account management

## License

This project is developed for academic/project purposes.

## Authors

**Smart Recruitment Platform Team**

Project modules include:

- Authentication
- Admin
- Employer
- Job Seeker
- Job Matching
- Integration / Testing

---

**Smart Recruitment Platform – Connecting Employers with the Right Talent**
