# HotelListing.Api

A RESTful Web API built with **ASP.NET Core 10** for managing hotels, countries, bookings, and users. Features JWT-based authentication with multiple auth schemes (JWT Bearer, Basic, and API Key).

## Features

- **Countries & Hotels** — CRUD operations with pagination, filtering, and sorting
- **Bookings** — Manage hotel room bookings for authenticated users
- **Authentication** — JWT Bearer token, Basic auth, and API Key authentication
- **ASP.NET Core Identity** — User registration, login, and role management
- **Entity Framework Core** — Code-first migrations with SQL Server
- **AutoMapper** — DTO/entity mapping with custom profiles
- **OpenAPI** — Swagger/OpenAPI documentation for API exploration

## Tech Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server
- JWT Bearer Authentication
- ASP.NET Core Identity
- AutoMapper 13

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/sql-server) (local or containerized)

## Getting Started

1. **Clone the repository**

   ```bash
   git clone <repository-url>
   cd HotelListing.Api/HotelListing.Api
   ```

2. **Configure the database**

   Update the connection string in `appsettings.json`:

   ```json
   "ConnectionStrings": {
     "HotelListingDbConnectionString": "Server=localhost,1433;Database=HotelListingDb;User Id=sa;Password=YourStrong@Password;MultipleActiveResultSets=True;TrustServerCertificate=True;Encrypt=false"
   }
   ```

   Alternatively, use [User Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets) for local development:

   ```bash
   dotnet user-secrets set "ConnectionStrings:HotelListingDbConnectionString" "<your-connection-string>"
   ```

3. **Apply database migrations**

   ```bash
   dotnet ef database update
   ```

4. **Run the API**

   ```bash
   dotnet run
   ```

   The API will be available at `https://localhost:5001` (or the port configured in `Properties/launchSettings.json`).

5. **Explore the API**

   When running in development mode, the OpenAPI documentation is available at `/openapi`.

## API Endpoints

| Method   | Endpoint                        | Description              |
|----------|---------------------------------|--------------------------|
| `GET`    | `/api/countries`                | List all countries       |
| `GET`    | `/api/countries/{id}`           | Get a country by ID      |
| `POST`   | `/api/countries`                | Create a country         |
| `PUT`    | `/api/countries/{id}`           | Update a country         |
| `DELETE` | `/api/countries/{id}`           | Delete a country         |
| `GET`    | `/api/hotels`                   | List all hotels          |
| `GET`    | `/api/hotels/{id}`              | Get a hotel by ID        |
| `POST`   | `/api/hotels`                   | Create a hotel           |
| `PUT`    | `/api/hotels/{id}`              | Update a hotel           |
| `DELETE` | `/api/hotels/{id}`              | Delete a hotel           |
| `GET`    | `/api/hotelbookings`            | List user bookings       |
| `POST`   | `/api/hotelbookings`            | Create a booking         |
| `POST`   | `/api/defaultauth/register`     | Register a new user      |
| `POST`   | `/api/defaultauth/login`        | Login and get JWT token  |

## Authentication

The API supports three authentication schemes:

- **JWT Bearer** — Standard token-based auth for clients
- **Basic** — Username/password via `Authorization: Basic` header
- **API Key** — Key-based auth via `X-Api-Key` header

Configure JWT settings in `appsettings.json`:

```json
"JwtSettings": {
  "Issuer": "HotelListingAPI",
  "Audience": "HotelListingAPIClient",
  "DurationInMinutes": 60,
  "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!"
}
```

## Project Structure

```
HotelListing.Api/
├── AuthorizationFilters/   # Custom authorization filters
├── Constants/              # Auth scheme constants
├── Contracts/              # Service interfaces
├── Controllers/            # API controllers
├── Data/                   # DbContext and entity models
├── DTOs/                   # Data Transfer Objects
├── Handlers/               # Custom auth handlers (Basic, API Key)
├── MappingProfiles/        # AutoMapper profiles
├── Migrations/             # EF Core migrations
├── Results/                # Operation result pattern
├── Services/               # Business logic services
├── Program.cs              # Application entry point
├── appsettings.json        # Application configuration
└── HotelListing.Api.csproj # Project file
```
