# 🚀 ApexTradeHub

> An enterprise-grade, real-time financial order matching and telemetry streaming backend built with **C# .NET Core**, **CQRS (MediatR)**, **Entity Framework Core**, and **ASP.NET Core SignalR WebSockets**.

---

## 🏛️ Architecture & Design Patterns

ApexTradeHub is architected around strict separation of concerns and production scalability:

* **CQRS (Command Query Responsibility Segregation via MediatR):** Completely decouples state-mutating operations (**Commands**) from data-fetching operations (**Queries**). Controllers remain thin dispatchers, housing zero business logic.
* **Real-Time WebSockets (ASP.NET Core SignalR):** Establishes a persistent bi-directional connection. Executed trades are instantly pushed out to connected clients grouped by ticker symbols (e.g., `BTC-USD`) with zero polling overhead.
* **Domain Persistence & Fluent API:** Uses Entity Framework Core configured with precise SQL database constraints, decimal precision for financial calculations (`18,4`), and high-performance indexing.

---

## 🛠️ Tech Stack

* **Language:** C# / .NET Core
* **Database:** Microsoft SQL Server (`ApexTradeDb`)
* **ORM:** Entity Framework Core (Code-First Migrations)
* **Messaging / Pipeline:** MediatR (In-process mediator pattern)
* **Real-Time Streaming:** ASP.NET Core SignalR
* **API Documentation:** OpenAPI / Swagger

---

## 📂 Project Directory Structure

```text
ApexTradeHub/
│
├── Controllers/            # Thin API entry points (Dispatchers)
│   └── OrdersController.cs 
├── Data/                   # Database Context & Factory
│   ├── TradeDbContext.cs
│   └── TradeDbContextFactory.cs
├── Features/               # CQRS Vertical Slices
│   └── Orders/
│       ├── Commands/       # Write operations (e.g., CreateOrder)
│       └── Queries/        # Read operations (e.g., GetOrders)
├── Hubs/                   # SignalR Real-Time WebSocket Hubs
│   └── TradeHub.cs
├── Models/                 # Domain Entities
│   └── TradeOrder.cs
├── appsettings.json        # Configuration & Connection Strings
└── Program.cs              # Dependency Injection & Middleware Pipeline
```

---

## 🚀 Getting Started Locally

### Prerequisites
* [.NET SDK](https://dotnet.microsoft.com/) (.NET 8 or higher)
* [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server) (Running locally, e.g., `localhost\MSSQLSERVER03`)

### 1. Clone & Configure
Clone this repository and verify your connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=localhost\\MSSQLSERVER03;Initial Catalog=ApexTradeDb;Integrated Security=True;TrustServerCertificate=True;"
}
```

### 2. Run Database Migrations
Apply Code-First migrations to spin up your dedicated `ApexTradeDb` catalog in SQL Server:
```bash
dotnet ef database update
```

### 3. Run the Application
Start the enterprise server:
```bash
dotnet run
```

---

## 📡 API Endpoints & Usage

### 1. Place a Trade Order (Command)
* **Endpoint:** `POST /api/orders`
* **Payload Example:**
  ```json
  {
    "symbol": "BTC-USD",
    "price": 67500.5000,
    "quantity": 2.5000,
    "isBuy": true
  }
  ```
* **Behavior:** Validates, saves to SQL Server, and instantly broadcasts the execution payload via SignalR to all clients subscribed to the `BTC-USD` group.

### 2. Fetch Orders (Query)
* **Endpoint:** `GET /api/orders?symbol=btc-usd`
* **Behavior:** Uses optimized `AsNoTracking()` projection queries via MediatR to return recent filtered order history.

---

## 🔌 SignalR WebSocket Hub
* **Endpoint URL:** `https://localhost:5001/hubs/trade`
* **Client Hub Methods:**
  * `JoinSymbolGroup(string symbol)` - Subscribe to real-time ticker broadcasts.
  * `LeaveSymbolGroup(string symbol)` - Unsubscribe from ticker broadcasts.
* **Server Broadcast Events:**
  * `ReceiveTradeExecution` - Pushes real-time trade packets to group subscribers.