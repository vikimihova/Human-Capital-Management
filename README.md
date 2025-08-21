# Human-Capital-Management

### ABOUT
A human capital management (HCM) web application with basic features for managing employees records, departments and job titles.

<img width="1919" height="884" alt="Nadnik_preview" src="https://github.com/user-attachments/assets/a8f648b9-3328-4537-8630-dcccee6f9f59" />

### TECHNOLOGIES
|| ASP.NET Core 8.0 || ASP.NET Identity System <br/>
|| Entity Framework Core 8.0 || Microsoft SQL Server 

### DESIGN PATTERNS AND ARCHITECTURE
|| Repository || Dependency Injection || MVC + API

### WEB UI
|| Razor Pages || Bootstrap || AJAX

### DATABASE DIAGRAM
The database can be further expanded by adding the following entities: Permission, Payments, FiscalPeriods (currently not included).
<img width="1118" height="760" alt="Nadnik_database_diagram" src="https://github.com/user-attachments/assets/f0e9fdd3-acae-4625-8922-b88f724db5b8" />

### HOW IT WORKS
Users are assigned one of three roles: Employee, Manager, or HR Admin. Permissions within the app vary based on the assigned role. Only logged-in users can access its features, which include:

1. view their own personal user record (Employees)
2. all of the above + the ability to view and edit certain properties of employees within their own department (Managers)
3. Full access to view, add, edit, or delete all user records, departments, and job titles. (HR Admins)

Searching and filtering functionality is supported.

### INSTRUCTIONS FOR TESTING THE FUNCTIONALITY

1. After cloning the repository, update the connection string in appsettings.json as needed.
2. All required configurations are also located in appsettings.json.
3. Delete the Migrations folder and add an initial migration. Change OnDelete behavior in AspNetUsers for FK Department and FK JobTitle to .NoAction. Create the database.
4. Run the application. The database will be automatically seeded with roles, users, departments, and job titles.
- Role names are stored as application constants in the Common layer.
- User, department, and job title data is stored in a JSON file located in the Data layer under DataProcessor.
- Seeding logic can be found in the Infrastructure layer under Extensions (for roles and users), and in the Data layer within the DbContext (for departments and job titles).
5. To log in, use any of the following credentials:
- Employee: "username": "m.maxwell", "password": "abv123"
- Manager: "username": "ironman", "password": "abv123"
- HR Admin: "username": "admin", "password": "admin123"

### MVC CONTROLLERS AND API CONTROLLERS
The application includes both MVC and API controllers, which share a common context based on ASP.NET Identity. Access to controller actions is secured using authorization attributes. Validation logic is applied within controllers or services where appropriate, to help prevent potential security vulnerabilities.<br/>

API controllers handle operations such as Delete, Remove (soft delete), and Include via AJAX requests. All other actions are managed by MVC controllers. While the two types of controllers currently complement each other in terms of functionality, they are designed to support future flexibility—allowing the application to evolve toward a more MVC-centric or API-driven architecture.<br/>

Hard deletion of users is intentionally restricted to API requests (e.g., via Postman) to prevent accidental data loss through the UI.
