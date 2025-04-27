
# LibraryAPI Project

This project is a simple Library Management API built with .NET Core and connected to a MySQL database. It includes RESTful API endpoints, a basic HTML/CSS/JS frontend, and automated testing with Unit Tests, Postman, Cypress, and JMeter.

---

## Project Setup & Run

### 1. Clone the Repository

### 2. MySQL Database Setup
- Ensure MySQL Server is running on your local machine.
- Open MySQL Workbench.
- Create a new schema (you can name it `librarydb`).
- Open the provided raw `LibraryDB.sql` file (located in the "Submissions" folder) and execute it to create the database and necessary tables.

### 3. Configure `appsettings.json`
- In the `LibraryAPI` project folder, locate the `appsettings.json` file.
- Update the connection string to match your local MySQL setup:

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=librarydb;user=root;password=YOUR_PASSWORD"
}
```
Replace `YOUR_PASSWORD` with your MySQL root password.

---

### 4. Run the Project
- Open the project in Rider or Visual Studio.
- Restore NuGet packages (necessary ones written in the notes section below).
- Run the project. The API will be available at:
```
https://localhost:44318/api
```
- Open `index.html` under `wwwroot` folder to view the frontend.
  "https://localhost:44318/index.html"
---

## Testing Guide

### Unit Tests
- Location: `LibraryAPI.Tests` project.
- Run directly from Rider using the test explorer.
- Test Results: Screenshots/logs included in the `SubmissionFiles/UnitTesting` folder.

---

### Postman Tests
- Collection file: `SubmissionFiles/Postman/LibraryAPI.postman_collection.json`.
- Test Results: Exported results are in the `SubmissionFiles/Postman` folder.

---

### UI Tests (Cypress)
- Cypress config and test scripts are under:  
  `LibraryAPI/wwwroot/cypress/`
- UI Test Results: Located in `SubmissionFiles/UITesting`.

---

### Performance Tests (JMeter)
- Results for 10, 50, and 100 users as CSV and the report file are saved in `SubmissionFiles/PerformanceTest`.

---

## ℹ️ Notes
- Ensure MySQL is running before starting the API.
- If port `44318` is busy, adjust the launch settings.
- Before running the project, make sure the following NuGet packages are installed:

 Microsoft.EntityFrameworkCore
 Microsoft.EntityFrameworkCore.Tools
 Pomelo.EntityFrameworkCore.MySql

You can install them via NuGet Package Manager or using the CLI:

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Pomelo.EntityFrameworkCore.MySql
---

Please feel free to reach me out in any case :)