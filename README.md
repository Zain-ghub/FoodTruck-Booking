# FoodTruck-Booking

A booking website for a food truck business. Customers request the truck to come to any location (for example a parking lot), and the owner reviews and confirms or declines each request.

Work in progress, built for a local burger joint and as a portfolio project.

## Current features

- Booking form with validation (required fields, email and phone format, guest range, no past dates)
- Confirmation page after a successful booking
- Admin bookings table with Confirm and Decline actions
- Public menu page, grouped by category, hiding unavailable items
- SQL Server storage with EF Core migrations
- Unit tests for the booking and menu services

## Tech stack

- ASP.NET Core, Razor Pages
- Entity Framework Core (Code First), SQL Server LocalDB
- Dependency injection with interface-based services
- xUnit, EF Core in-memory provider, FluentAssertions
- Bootstrap

## Project structure

| Project | Contents |
| --- | --- |
| `Burgers.Core` | Models and service interfaces. No dependencies. |
| `Burgers.Data` | `BurgersContext`, migrations, and service implementations. |
| `Burgers` | Razor Pages web app, `Program.cs`, configuration. |
| `Burgers.Tests` | Service unit tests. |

## Planned

- REST Web API with JWT authentication
- Angular front end (Reactive Forms, RxJS, NgRx)
- Live booking updates with SignalR
- Event messaging with Kafka (producer/consumer)
- Docker Compose and GitHub Actions CI
- Owner-managed menu and admin login

## Run locally

1. Clone the repository and open the solution in Visual Studio.
2. Make sure the connection string in `appsettings.json` points to your local SQL Server (the default uses LocalDB).
3. In the Package Manager Console, set the Default project to `Burgers.Data` (the startup project stays `Burgers`) and run `Update-Database`.
4. Start the `Burgers` project and open `/Book` for the booking form, `/Menu` for the menu, and `/Bookings` for the admin list.
