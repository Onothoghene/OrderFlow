# Order Processing System (Clean Architecture - .NET)

## Overview

This project is a **resilient order processing backend system** built with **ASP.NET Core (.NET 9), Clean Architecture, and Entity Framework Core**.

It demonstrates how to design and implement a **production-ready e-commerce order workflow** with focus on:

* Data consistency
* Concurrency control
* Inventory management
* Event-driven architecture (in-process)
* Clean and maintainable code structure
  
## Architecture

The system follows **Clean Architecture principles**:

WebApi (Presentation Layer)
│
Application (Business Logic)
│   - CQRS (MediatR)
│   - DTOs
│   - Feature(Commands / Queries Handlers)
│   - Events
│
Domain (Core Entities)
│   - Business models
│   - Enums
│   - Domain rules
│
Infrastructure (Persistence & External Services)
    - EF Core DbContext
    - Identity Context
    - Repositories
    - Seeding

## Tech Stack

* ASP.NET Core Web API (.NET 8/9)
* Entity Framework Core
* MediatR (CQRS Pattern)
* AutoMapper
* SQL Server / LocalDB
* Swagger / OpenAPI
* Built-in `ILogger` (logging)
* TransactionScope (atomic operations)


## Features

### 🛒 Order Processing

* Place orders with multiple products
* Quantity validation
* Stock validation before order creation
* Payment handling
* Cart synchronization

---

### Inventory Management

* Prevents overselling
* Stock validation during:

  * Cart addition
  * Order placement
* RowVersion used for **optimistic concurrency control**
* Stock deduction during order creation
* Stock restoration on order cancellation

---

### Order Lifecycle

* Create Order
* Update Order
* Cancel Order
* Delete Order (soft/hard based on implementation)

---

### Event-Driven Flow (In-Process Pub/Sub)

After successful order placement:

* OrderPlacedEvent is published using **MediatR**
* Handlers simulate:
  * Inventory confirmation
  * Notification logging
  * Payment flow simulation

### Logging
* Built-in ASP.NET Core ILogger
* Logs:
  * Order creation
  * Stock validation
  * Event publishing
  * Inventory updates
* Logs available in:
  * Console
  * Visual Studio Output window
    
### Database Seeding
Automatic seeding includes:
* Menu Items
* Restaurants
* User Profiles
* Couriers & Comments
* Files

Seeding runs on application startup via Program.cs.

## Concurrency & Consistency Strategy

The system ensures data integrity using:

* `TransactionScope` for atomic operations
* Optimistic concurrency (`RowVersion`)
* Stock validation before updates
* Exception handling for race conditions (`DbUpdateConcurrencyException`)

## Event-Driven Design

This project uses **in-process eventing via MediatR**:

### Flow:

1. Order is created
2. `OrderPlacedEvent` is published
3. Multiple handlers respond:
   * Inventory confirmation
   * Notification logging

## How to Run the Project

### 1. Clone repository

**bash**
git clone https://github.com/your-repo-url.git


### 2. Configure database

Update connection string in:
appsettings.json

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=OrderFlowDB;Trusted_Connection=True;"
}
```

### 3. Apply migrations

```bash
Update-Database -Context ApplicationDbContext
```

---

### 4. Run application

### 5. Access Swagger

```
http://localhost:50771/swagger/index.html
```

## Key Engineering Decisions

### Clean Architecture

Separation of concerns for scalability and maintainability.

### TransactionScope

Ensures order consistency across:
* Order creation
* Payment
* Stock updates

### In-process Eventing

Chosen over a message broker for simplicity and interview scope.

### Optimistic Concurrency

Used for stock safety under concurrent requests.

## Trade-offs

| Decision          | Trade-off                        |
| ----------------- | -------------------------------- |
| In-process events | No distributed scalability       |
| TransactionScope  | Slight performance overhead      |
| Built-in logging  | No persistent log storage        |
| EF Core tracking  | Easier development, less control |


## Future Improvements

* Background job processing (Hangfire / Hosted Services)
* Message broker (RabbitMQ / Kafka)
* Distributed caching (Redis)
* Centralized logging (Serilog + Seq / ELK)
* Payment gateway integration
* Docker containerization

## 📌 Summary

This project demonstrates:

* Real-world order processing logic
* Inventory safety under concurrency
* Clean Architecture design
* Event-driven patterns
* Production-level thinking
