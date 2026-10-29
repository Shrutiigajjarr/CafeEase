# CafeEase

CafeEase is a cafe menu and billing application built with ASP.NET Core MVC, Entity Framework Core, and MySQL.

## Features
- Dashboard with menu count, bill count, today's sales, and recent bills
- Add, view, search, filter, edit, and delete menu items
- Server-side validation for menu details
- Create bills with multiple menu items and quantities
- Automatic bill total calculation and stock deduction
- Bill history, receipt details, and print-friendly receipt
- Responsive Bootstrap layout with a calm green theme

## Requirements
- .NET 8 SDK
- MySQL Server 8 or compatible
- Internet connection on first run to restore NuGet packages and load Bootstrap/validation libraries from CDNs

## 1. Create the database
In MySQL Workbench or the MySQL command line, run:

```sql
CREATE DATABASE cafe_ease CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

## 2. Configure your database
Open `appsettings.json` and update the `CafeEase` connection string with your local MySQL username and password. Do not commit real credentials to GitHub. For a shared or production environment, use environment variables or .NET user-secrets instead.

## 3. Run the project
Open a terminal in the folder containing `CafeEase.csproj`:

```powershell
dotnet restore
dotnet tool install --global dotnet-ef --version 8.0.11
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

If `dotnet-ef` is already installed, update it to the 8.0.11 version instead of installing it again. Open the local URL shown in the terminal.

If your computer has multiple .NET installations, run commands with the full path to the intended `dotnet.exe` and ensure the .NET 8 SDK is installed. This project targets .NET 8; .NET 10 alone does not automatically provide the .NET 8 targeting pack.

## Notes
- The first migration is generated locally from the models using EF Core tools.
- Menu items linked to past bills cannot be deleted, preserving receipt history. Set their available quantity to zero instead.
- Bill creation validates stock on the server and saves the bill and stock changes in a database transaction.
- Authentication and staff roles are not included in this version.
- Today's sales are calculated using UTC dates in the database; timezone-aware business reporting can be added for deployment.
