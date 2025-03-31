# Template Project for ASP.NET Blazor Webassembly

This repository contains a multi-project template for a Blazor application. It is structured into several projects, each serving a specific purpose. It also has implemented user authentification via JWT and Identity.

## Project Structure

```
.gitignore
Directory.Packages.props
Makefile
Sample.sln
Sample/
    .env
    appsettings.Development.json
    appsettings.json
    Program.cs
    Sample.csproj
    Components/
    Controllers/
    Properties/
    wwwroot/
Sample.Client/
    _Imports.razor
    Program.cs
    Routes.razor
    Sample.Client.csproj
Sample.Core/
    Sample.Core.csproj
    Entities/
Sample.Infrastructure/
    Sample.Infrastructure.csproj
    Repositories/
    Data/
    Seed/
    Migrations/
```

### Projects

- **Sample**: The main application project containing the entry point (`Program.cs`) and configuration files.
- **Sample.Client**: A Blazor-based client application.
- **Sample.Core**: Contains core entities and business logic.
- **Sample.Infrastructure**: Handles infrastructure concerns like data access and external integrations.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- A compatible IDE like [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/).

## Getting Started

1. Clone the repository:
   ```sh
   git clone https://github.com/MaxHotiuk/SampleBlazorWithAuth.git
   cd SampleBlazorWithAuth
   ```

2. Restore dependencies:
   ```sh
   make restore
   ```

3. Build the solution:
   ```sh
   make build
   ```
4. Create a .env file in /Sample directory
   ```
   CONNECTION_STRING="your_connection_string"
   JWT_SECRET="your_jwt_secret"
   ```
   
4. Migrate:
   ```sh
   make migrate
   ```

5. Update database:
   ```sh
   make update
   ```

6. Run the application:
   ```sh
   make run
   ```

## Configuration

- **App Settings**: Modify `appsettings.json` or `appsettings.Development.json` for application-level configuration.
