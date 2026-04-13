# YardView.TaskManager

A simple full-stack task management application built with ASP.NET Core, SQLite, Angular, and Tailwind CSS.

## Tech Stack

- ASP.NET Core Minimal API
- Entity Framework Core + SQLite
- Angular
- Tailwind CSS
- Serilog
- xUnit

## Features

- List all tasks
- Filter tasks by status
- Create tasks
- Update task status inline
- Delete tasks with confirmation
- Loading, error, and empty states
- Optional due date with overdue highlighting

## Prerequisites

- .NET 10 SDK
- Node.js
- npm


## Running with Visual Studio

1. Open the solution in Visual Studio.
2. Set `YardView.TaskManager.Server` as the startup project.
3. Run the project with the `https` launch profile.
4. The application will open in the browser at the configured HTTPS URL.

If the database has not been created yet, run the following once from a terminal:

```bash
cd YardView.TaskManager.Server
dotnet ef database update
```

## Running the Server and UI with CLI

```bash
cd YardView.TaskManager.Server
dotnet restore
dotnet ef database update
dotnet run --launch-profile=https

```
Once both server and client start, open the SPA directly at: [https://localhost:64860/](https://localhost:64860/)

## Notes

- On first startup, the application may take longer to load while dependencies are restored and the Angular client is built.
- Subsequent runs will be significantly faster.