# Product Management System

A product management application built with ASP.NET Core 10. It has a REST API for managing products and categories, along with an MVC application that uses the API.

The solution contains two projects:

- **ProductManagement.API** – Handles API requests, application logic and database operations.
- **ProductManagement.MVC** – Provides the Razor-based interface and communicates with the API through HttpClient.

## Technologies used

- C# and ASP.NET Core 10
- Entity Framework Core 10
- SQLite
- ASP.NET Core MVC and Razor Views
- HttpClient and dependency injection
- Swagger / OpenAPI

## Requirements

Install the .NET 10 SDK. To check that it is installed, run:

```powershell
dotnet --version
```

You will also need the EF Core command-line tool. If it is not installed, run:

```powershell
dotnet tool install --global dotnet-ef --version 10.0.0
```

## Set up the database

Open a terminal in the folder that contains `ProductManagement.sln`.

The initial EF Core migration is already included in the repository. Apply it to create the SQLite database and add the sample categories:

```powershell
dotnet ef database update --project ProductManagement.API --startup-project ProductManagement.API
```

This creates the Categories and Products tables, sets up their relationship, and inserts the four sample categories.

## Run the application

Start the API first:

```powershell
dotnet run --project ProductManagement.API --launch-profile http
```

The API runs at `http://localhost:5000`. You can open Swagger at:

`http://localhost:5000/swagger`

Leave the API running, then open a second terminal and start the MVC application:

```powershell
dotnet run --project ProductManagement.MVC --launch-profile http
```

The MVC website runs at `http://localhost:5001` and sends its requests to the API at `http://localhost:5000`.

## API endpoints

| Method | Endpoint | Purpose |
|---|---|---|
| GET | `/api/categories` | Get all categories |
| GET | `/api/products` | Get all products with category names |
| GET | `/api/products?categoryId=1` | Filter products by category |
| GET | `/api/products/{id}` | Get a product by ID |
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

## How it is organised

The API is split into three main parts:

1. **Controllers** receive requests and return the appropriate HTTP responses.
2. **Services** handle the application logic.
3. **Data** contains the Entity Framework Core DbContext and database configuration.

The MVC project does not access the database directly. It uses a typed HttpClient service to send requests to the API.

## A few implementation details

- A category cannot be deleted while it still has products linked to it.
- Product creation and updates check that the selected category exists.
- Product creation time is stored in UTC.
- Request data is validated using data annotations.
- Unexpected API errors are handled by middleware.
