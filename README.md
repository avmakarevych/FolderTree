# FolderTree Web Application

## Overview

FolderTree is a web application developed using ASP.NET Core, designed to display a hierarchical structure of directories. The application provides a visual representation of directories stored in a database, allowing users to navigate through the hierarchy by clicking on directory links.

## Features

- **Database Integration**: The application uses a database to store the hierarchical system of directories. The database is pre-populated with a predefined directory structure.
- **Web Interface**: The main interface displays the directory structure from the database. Each directory is displayed with its name and a list of its child directories. Each directory name acts as a link, allowing users to navigate deeper into the hierarchy.
- **Import & Export**:
  - **Import from OS/File**: Users have the ability to import the directory structure from the operating system or from a file. This imported structure can then be saved to the database and displayed in the web application.
  - **Export to File**: Existing directory structures in the database can be exported to a file. This file can later be used for importing the structure back into the application or another system.

## Setup & Installation

1. **Prerequisites**:
   - Ensure you have .NET Core SDK installed.
   - A suitable database system (e.g., SQLite, SQL Server) and its connection string.

2. **Installation**:
   - Clone the repository: `git clone https://github.com/avmakarevych/FolderTree.git`
   - Navigate to the project directory: `cd FolderTree`
   - Install the required packages: `dotnet restore`
   - Update the database connection string in `appsettings.json` if necessary.
   - Run the database migrations to set up the database: `dotnet ef database update`
   - Start the application: `dotnet run`

3. **Usage**:
   - Open a web browser and navigate to `http://localhost:5000` (or the port specified).
   - Explore the directory structure by clicking on directory links.
   - Use the Import/Export features as needed.
