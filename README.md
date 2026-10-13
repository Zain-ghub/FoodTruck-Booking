# FoodTruck-Booking

A booking website for a food truck business. Customers request the truck to come to any location (for example a parking lot), and the owner reviews and confirms or declines each request.

Work in progress, built as a real project for a local burger joint and as a portfolio piece.

## Current features

- Booking form with validation (required fields, email and phone format, guest range, no past dates)
- Confirmation page after a successful booking
- Admin bookings table with Confirm and Decline actions
- Bookings stored in SQL Server with EF Core migrations

## Tech stack

- ASP.NET Core, Razor Pages
- Entity Framework Core (Code First)
- SQL Server LocalDB
- Bootstrap

## Planned

- Service layer with dependency injection and unit tests
- REST Web API with JWT authentication
- Angular front end (Reactive Forms, RxJS, NgRx)
- Live booking updates with SignalR
- Event messaging with Kafka (producer/consumer)
- Docker Compose and GitHub Actions CI
- Menu page managed by the owner

## Run locally

1. Clone the repository and open the solution in Visual Studio.
2. Make sure the connection string in `appsettings.json` points to your local SQL Server (the default uses LocalDB).
3. In the Package Manager Console, run `Update-Database`.
4. Start the project and open `/Book` for the booking form and `/Bookings` for the admin list.
