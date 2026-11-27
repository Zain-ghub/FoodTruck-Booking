# FoodTruck-Booking

[![CI](https://github.com/Zain-ghub/FoodTruck-Booking/actions/workflows/ci.yml/badge.svg)](https://github.com/Zain-ghub/FoodTruck-Booking/actions/workflows/ci.yml)

A booking platform for a food truck business. Customers request the truck to come to any location (for example a parking lot), and the owner reviews each request, confirming or declining it from a protected admin area.

Built for a local burger joint and as a portfolio project. Work in progress.

## Features

**Customers**
- Public menu grouped by category (unavailable items are hidden)
- Booking form with client-side and server-side validation (required fields, phone and email format, guest limit, future dates only)

**Owner (admin)**
- JWT login backed by ASP.NET Core Identity
- Bookings dashboard with Confirm and Decline actions
- Admin menu endpoints (list, add, edit, delete) behind authentication

## Tech stack

| Area | Technologies |
| --- | --- |
| Backend | ASP.NET Core Web API (.NET 9), Entity Framework Core (Code First), SQL Server |
| Auth | ASP.NET Core Identity, JWT bearer tokens |
| API design | DTOs, custom validation attribute, global exception handler (`IExceptionHandler`), CORS policy, Swagger |
| Frontend | Angular (standalone components, Reactive Forms, RxJS, HTTP interceptor, route guard), TypeScript |
| Testing and CI | xUnit, FluentAssertions, EF Core in-memory provider, GitHub Actions (builds and tests the .NET solution, builds the Angular app) |

## Architecture

| Project | Purpose |
| --- | --- |
| `Burgers.Core` | Models, service interfaces, shared validation. No dependencies. |
| `Burgers.Data` | `BurgersContext`, migrations, service implementations. |
| `Burgers.Api` | REST API: controllers, DTOs, authentication, error handling. |
| `Burgers` | The original Razor Pages app (first version of the UI, still working). |
| `Burgers.Tests` | Unit tests for the booking and menu services. |
| `foodtruck-client` | Angular single-page app that talks to the API. |

Services are registered through interfaces with dependency injection, so the API and the Razor app share the same business logic.

## API overview

| Method | Route | Access |
| --- | --- | --- |
| GET | `/api/menu` | Public |
| POST | `/api/bookings` | Public |
| POST | `/api/auth/login` | Public |
| GET | `/api/bookings` | Admin |
| POST | `/api/bookings/{id}/confirm` | Admin |
| POST | `/api/bookings/{id}/decline` | Admin |
| GET, POST | `/api/admin/menu` | Admin |
| GET, PUT, DELETE | `/api/admin/menu/{id}` | Admin |

Swagger UI is available at `/swagger` in Development, with an Authorize button for the JWT.

## Run locally

Prerequisites: .NET 9 SDK, Node.js (LTS), and SQL Server LocalDB (installed with Visual Studio).

1. Clone the repository.
2. Set the API secrets (they are never committed). From the repo root:
   ```
   dotnet user-secrets set "Jwt:Key" "<a random string of 32+ characters>" --project Burgers.Api
   dotnet user-secrets set "Admin:Email" "<admin email>" --project Burgers.Api
   dotnet user-secrets set "Admin:Password" "<a strong password>" --project Burgers.Api
   ```
   The admin account is created on first startup from these values.
3. Create the database:
   ```
   dotnet ef database update --project Burgers.Data --startup-project Burgers.Api
   ```
   (or `Update-Database` in the Package Manager Console, with `Burgers.Data` as the default project).
4. Start the API: `dotnet run --project Burgers.Api`. Swagger opens at `/swagger`.
5. Start the client:
   ```
   cd foodtruck-client
   npm install
   npm start
   ```
   Open `http://localhost:4200`. The API address is set in the Angular services (`http://localhost:5077/api`) and the allowed origin in the API's `appsettings` files.

## In progress

- Docker Compose setup (API and SQL Server)
- Live booking notifications with SignalR
- Event-driven processing with Kafka (producer in the API, separate consumer service)
- Admin menu screen in the Angular app
- UTC handling for booking dates
