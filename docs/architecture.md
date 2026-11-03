# Architecture notes

This project is intentionally split into two applications.

## Web API

The API owns the database and business rules.

```text
HTTP request
    |
    v
Controller
    |
    v
Service
    |
    v
AppDbContext (Entity Framework Core)
    |
    v
SQLite
```

- **Controller:** Reads route/body/query values and returns HTTP responses.
- **Service:** Checks business rules and performs the requested operation.
- **AppDbContext:** Represents the database session and exposes Categories and Products.
- **DTO:** Defines the data accepted or returned by the API. Database entities are not sent directly to clients.

## Category and Product

A category can have many products. A product belongs to one category through CategoryId.

The relationship uses `DeleteBehavior.Restrict`. This means a category cannot be removed while products still reference it.

## MVC application

The MVC application is only a client of the API.

```text
Browser
   |
   v
MVC Controller
   |
   v
ProductApiService (typed HttpClient)
   |
   | HTTP + JSON
   v
Web API
```

The MVC project has no Entity Framework package, DbContext or database connection string. It asks the API for product and category data.

## Why async methods are used

Database and HTTP calls can take time. Async methods allow the server to handle other requests while waiting for those operations instead of blocking the request thread.

## Local development

Run the API on port 5000 first, then run the MVC application on port 5001. The MVC API base address is kept in its appsettings.json file.
