# Mangala

A full-stack restaurant ordering system. The ASP.NET Core backend handles authentication, menu, orders and payments, and the Vue frontend lets customers browse the menu, build a cart, pay and track their orders. Staff and admins can manage menu items, categories, users and roles.

## Features

- **Authentication pages**: register, log in, and Google sign-in, with the JWT kept on the client
- **Role-based access**: users have roles (for example Customer) that control what they can do
- **Menu browsing**: view the daily menu by category using reusable menu item cards
- **My Orders**: view past orders with their order and payment status
- **Cart**: add and remove items, adjust quantities, and see the running total
- **Checkout**: place an order and be redirected to the Bachs payment page
- **Payments**: checkout flow using the Bachs payment gateway (sandbox)
- **Emails**: welcome emails sent through Brevo
- **Rate limiting**: protects the API from too many requests
- **Swagger**: interactive API documentation
- **Responsive layout**: shared navigation bar and footer across all pages

## Tech stack

- C# and ASP.NET Core Web API
- Entity Framework Core (code-first migrations)
- PostgreSQL
- JWT authentication and Google OAuth
- Brevo (email) and Bachs (payments)
- Vue 3 with Vite (frontend)
- Vue Router for page navigation
- A state store for the cart and logged-in user (Pinia)
- An `api` layer that talks to the Mangala API

## Project structure            

```
Mangala/
  Mangala.API/     Controllers, startup (Program.cs) and service configuration
  Mangala.Core/    Models, DTOs, interfaces, services and settings (business logic)
  Mangala.Data/    AppDbContext, repositories and EF Core migrations
  Mangala.API.slnx Solution file
  Mangala.Frontend/   Vue app: components, views, router, stores and API calls
```

The solution follows a layered design: the API layer talks to services in `Core`, and services use repositories in `Data` to reach the database.

## Getting started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (the version targeted in `Mangala.API.csproj`)
- [PostgreSQL](https://www.postgresql.org/download/)
- Visual Studio 2022 or later, or any editor that supports .NET

### 1. Clone the repository

```
git clone https://github.com/ObialoDaniel/Mangala.git
cd Mangala
```

### 2. Add your secrets

Real keys are **not** stored in the repo. `appsettings.json` contains blank placeholders, and you provide the real values with .NET User Secrets, which are stored on your own computer.

From the `Mangala.API` folder, run:

```
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=mangala_db;Username=your-user;Password=your-password;"
dotnet user-secrets set "Jwt:Key" "a-long-random-string-of-at-least-32-characters"
dotnet user-secrets set "Authentication:Google:ClientId" "your-google-client-id"
dotnet user-secrets set "Authentication:Google:ClientSecret" "your-google-client-secret"
dotnet user-secrets set "Brevo:ApiKey" "your-brevo-api-key"
dotnet user-secrets set "Bachs:ApiKey" "your-bachs-sandbox-key"
```

In Visual Studio you can also right-click the `Mangala.API` project and choose **Manage User Secrets**.

### 3. Create the database

From the solution folder:

```
dotnet ef database update --project Mangala.Data --startup-project Mangala.API
```

If the `dotnet ef` command isn't found, install it first with `dotnet tool install --global dotnet-ef`.

### 4. Run the API

```
cd Mangala.API
dotnet run
```

Then open the Swagger page (the URL is printed in the terminal, usually `https://localhost:<port>/swagger`) to try the endpoints.

### 5. Run the frontend

```bash
cd Mangala.Frontend
npm install
npm run dev
```

The app runs at http://localhost:5173 and expects the API to be running too.

## API overview

| Area | Controller | What it does |
| --- | --- | --- |
| Auth | `AuthController` | Register, log in, Google sign-in |
| Users | `UserController` | Manage users |
| Roles | `RoleController` | Manage roles |
| Categories | `CategoryController` | Manage menu categories |
| Menu | `MenuController` | Manage menu items |
| Orders | `OrderController` | Create and view orders |
| Payments | `PaymentController` | Start checkout for an order |

## Security notes

- Never commit real passwords, API keys or connection strings. Use User Secrets locally.
- GitHub push protection is enabled on this repo and will block pushes that contain known secret formats.

## Roadmap

- [ ] Add unit and integration tests
- [ ] Add a frontend client
- [ ] Deploy to a hosting provider

## Author

Daniel Obialo - [GitHub](https://github.com/ObialoDaniel)
