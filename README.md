# FinTrack API

A RESTful Web API built with ASP.NET Core that fetches stock market data from
[AlphaVantage](https://www.alphavantage.co/) and stores it in a MS SQL Server
database for efficient local caching.

## Overview

FinTrack provides endpoints to retrieve stock market data including company
details and market movers (top gainers, top losers, and most actively traded).

When a request is made:
- If the data already exists in the database, it is returned directly.
- If not, it is fetched from AlphaVantage, saved to the database, and then returned.

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core Web API (.NET 10) |
| Language | C# |
| Database | Microsoft SQL Server |
| Data Access | ADO.NET |
| External API | AlphaVantage Stock Market API |

## Libraries & Design Decisions

### ASP.NET Core Web API
The built-in framework for building RESTful APIs in .NET. Chosen for its
performance, built-in dependency injection, and middleware pipeline support.

### ADO.NET
Used for all database access instead of an ORM (e.g. Entity Framework Core).
ADO.NET provides direct control over SQL queries, giving full visibility into
what is executed against the database. This aligns with the requirement to use
raw SQL queries rather than an abstraction layer that auto-generates them.

### .NET User Secrets
Used to store sensitive configuration (API keys, connection strings) locally
during development without committing them to source control.

## Setup

### Prerequisites
* .NET 10 SDK
* Microsoft SQL Server
* SQL Server Management Studio (SSMS)

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/miyushan/FinTrack.git
cd FinTrack
```

### 2. Database Setup

1. Open SQL Server Management Studio (SSMS).
2. Create a new database(FinTrack_Dev) using:

```sql
CREATE DATABASE FinTrack_Dev;
```

Then execute the following scripts in the `FinTrack_Dev` database:

```sql
CREATE TABLE Movers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Ticker NVARCHAR(10) NOT NULL,
    Price DECIMAL(18,4) NOT NULL,
    ChangeAmount DECIMAL(18,4) NOT NULL,
    ChangePercentage NVARCHAR(20) NOT NULL,
    Volume BIGINT NOT NULL,
    Category NVARCHAR(20) NOT NULL, -- Gainer, Loser, Active
    CachedDate DATE DEFAULT CAST(GETUTCDATE() AS DATE)
);

CREATE TABLE Companies (
    Symbol NVARCHAR(10) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Exchange NVARCHAR(10) NOT NULL,
    Currency NVARCHAR(10) NOT NULL,
    Country NVARCHAR(10) NOT NULL,
    Sector NVARCHAR(100),
    Industry NVARCHAR(100),
    MarketCap BIGINT NOT NULL
);
```

| Table | Purpose |
| --- | --- |
| `Movers` | Stores stock market movers including top gainers, top losers, and most actively traded stocks retrieved from AlphaVantage. |
| `Companies` | Stores company profiles retrieved from AlphaVantage. |

### 3. Configure User Secrets

Set the database connection string:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=YOUR_SERVER_NAME;Database=FinTrack_Dev;Trusted_Connection=True;TrustServerCertificate=True;"
```

Example:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=MIYUSHAN\MSSQLSERVER01;Database=FinTrack_Dev;Trusted_Connection=True;TrustServerCertificate=True;"
```

> Replace `YOUR_SERVER_NAME` with your local SQL Server instance name.

### 4. Configure AlphaVantage API Key

Create a free API key at https://www.alphavantage.co/support/#api-key, then store it:

```bash
dotnet user-secrets set "AlphaVantage:ApiKey" "YOUR_API_KEY"
```

Verify all secrets are stored correctly:

```bash
dotnet user-secrets list
```

### 5. Restore & Build

```bash
dotnet restore
dotnet build
```

### 6. Run the API

```bash
dotnet run --project FinTrack.csproj
```

The terminal will show `Now listening on: http://localhost:5114` when the API
is ready. **Keep this terminal open** while testing.

### 7. Access the API

Open your browser and navigate to:

```
http://localhost:5114/swagger
```

This will open the Swagger UI where you can explore and test all available
endpoints interactively.

You can also test manually using curl or Postman:

```bash
# Get company details by stock symbol
GET http://localhost:5114/api/company/{symbol}

# Example
GET http://localhost:5114/api/company/AAPL

# Get top movers (gainers, losers, most active)
GET http://localhost:5114/api/mover/top-movers
```

## API Rate Limits

The free tier of AlphaVantage allows **25 requests per day** per IP address.

If you encounter the following error:

```json
{
  "statusCode": 503,
  "message": "External service is currently unavailable. Please try again later."
}
```

This means the daily limit has been reached for your current IP. You can:
- **Switch to a different network** (e.g. mobile hotspot) to get a fresh IP and continue testing
- **Use cached data**, if the data was already fetched once, it will be served from the database without hitting the API

## Notes
Unit tests for the Service Layer have been included in the `Tests` folder for 
reference. However, they cannot be executed directly from this repository due 
to project structure constraints(the solution and project share the same 
directory), which prevents the test project from being added to the same 
solution without affecting the Git history.

The test project is maintained in a separate repository.