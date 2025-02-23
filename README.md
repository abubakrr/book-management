# Book Management System

## Overview

The Book Management System is a project designed to help users manage their book collections efficiently. It provides features to add, update, delete, and search for books within the collection.

## Features

- Add new books with details such as title, author, genre, and publication date.
- Update existing book information.
- Delete books from the collection.
- Search for books by title, author, or genre.
- Keep track of view count.
- Provide rank based search that includes view count and publication year

## Docker setup

This step is required in order to be able to connect with the SQL Server through docker

<!--
This command runs a Docker container for Microsoft SQL Server. It uses the `mcr.microsoft.com/mssql/server` image and sets the platform to `linux/amd64`. The environment variables `ACCEPT_EULA` and `SA_PASSWORD` are set to accept the end-user license agreement and define the system administrator password, respectively. The container maps port 1433 on the host to port 1433 on the container and names the container `exadel-sql-server-container`. The `-d` flag runs the container in detached mode.
-->

```bash
docker run --platform linux/amd64 -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=HappyParrot_0609" -p 1433:1433 --name exadel-sql-server-container -d mcr.microsoft.com/mssql/server
```

## Installation

To install and run the project locally, follow these steps:

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/book-management.git
   ```
2. Navigate to the project directory:
   ```bash
   cd book-management
   ```
3. Install the required .NET SDK:
   ```bash
   dotnet restore
   ```
4. Build the project:
   ```bash
   dotnet build
   ```
5. Run the application:
   ```bash
   dotnet run
   ```
