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

- .NET 8 SDK
- Node.js
- npm

## Running the API and UI

```bash
cd YardView.TaskManager.Api
dotnet restore
dotnet ef database update
dotnet run --launch-profile=https
```