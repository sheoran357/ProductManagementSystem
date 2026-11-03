# Product Management System

A small product management application built with ASP.NET Core 10.

The solution has two separate applications:

- **ProductManagement.API** – REST API, business rules and database access.
- **ProductManagement.MVC** – Razor UI that calls the API using HttpClient. It does not connect to the database.

## Technology

- C# / ASP.NET Core 10
- Entity Framework Core 10
- SQLite
- ASP.NET Core MVC and Razor Views
- HttpClient and dependency injection
- Swagger / OpenAPI

## Requirements

Install the .NET 10 SDK. You can check it with:

```powershell
dotnet --version
```

## 1. Create the database

Open a terminal in the folder containing `ProductManagement.sln`.

Install the EF Core command-line tool if it is not already installed:

```powershell
dotnet tool install --global dotnet-ef --version 10.0.0
```

If the tool is already installed, update it instead:

```powershell
dotnet tool update --global dotnet-ef --version 10.0.0
```

Create and apply the first migration:

```powershell
dotnet ef migrations add InitialCreate --project ProductManagement.API --startup-project ProductManagement.API
dotnet ef database update --project ProductManagement.API --startup-project ProductManagement.API
```

The migration creates the Categories and Products tables, their relationship, and the four sample categories configured in the DbContext.

## 2. Run the API

Open a terminal in the solution folder:

```powershell
dotnet run --project ProductManagement.API --launch-profile http
```

The API listens at `http://localhost:5000`.

Open Swagger at:

`http://localhost:5000/swagger`

Keep this terminal running.

## 3. Run the MVC application

Open a second terminal in the solution folder:

```powershell
dotnet run --project ProductManagement.MVC --launch-profile http
```

Open the website at:

`http://localhost:5001`

The MVC application calls the API at `http://localhost:5000`. Start the API before using the MVC website.

## API endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/api/categories` | Get all categories |
| GET | `/api/products` | Get all products with category names |
| GET | `/api/products?categoryId=1` | Filter products by category |
| GET | `/api/products/{id}` | Get one product |
| POST | `/api/products` | Create a product |
| PUT | `/api/products/{id}` | Update a product |
| DELETE | `/api/products/{id}` | Delete a product |

## Project structure

```text
ProductManagementSystem/
├── ProductManagement.API/
│   ├── Controllers/
│   ├── Data/
│   ├── DTOs/
│   ├── Middleware/
│   ├── Models/
│   └── Services/
├── ProductManagement.MVC/
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   └── Views/
└── ProductManagement.sln
```

## Architecture

The API follows a simple Controller → Service → Data flow:

1. Controllers handle HTTP requests and return status codes.
2. Services contain application rules and coordinate database work.
3. Entity Framework Core uses the DbContext to read and save data.

The MVC application uses a typed HttpClient service to communicate with the API. It never uses the API's DbContext or SQLite database directly.

## Notes

- Category deletion is configured with `DeleteBehavior.Restrict`, so a category with products cannot be deleted.
- Product creation and updates check that the selected category exists.
- Product creation time is assigned by the API in UTC.
- Request DTOs are validated with data annotations.
- Unexpected API errors are handled by global exception middleware.
