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

### Database Setup
1. Open SQL Server Management Studio (SSMS).
2. Create a new database(FinTrack_Dev) using:

```sql
CREATE DATABASE FinTrack_Dev;
```

### Configure User Secrets

1. Navigate to the project directory and initialize User Secrets:

```bash
dotnet user-secrets init
```

2. Configure the connection string using .NET User Secrets:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=YOUR_SERVER_NAME;Database=FinTrack_Dev;Trusted_Connection=True;TrustServerCertificate=True;"
```

Example:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=MIYUSHAN\MSSQLSERVER01;Database=FinTrack_Dev;Trusted_Connection=True;TrustServerCertificate=True;"
```

> Replace `YOUR_SERVER_NAME` with your local SQL Server instance name.

### Create Database Tables

Execute the following SQL scripts in the `FinTrack_Dev` database:

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

#### Table Overview

| Table       | Purpose                                                                                                                    |
| ----------- | -------------------------------------------------------------------------------------------------------------------------- |
| `Movers`    | Stores stock market movers including top gainers, top losers, and most actively traded stocks retrieved from AlphaVantage. |
| `Companies` | Stores company profiles retrieved from AlphaVantage.                                                                       |


### Configure AlphaVantage API

1. Create a free API key from AlphaVantage:

   * Visit https://www.alphavantage.co/support/#api-key
   * Generate your personal API key.

2. Store the API key using .NET User Secrets:

```bash
dotnet user-secrets set "AlphaVantage:ApiKey" "YOUR_API_KEY"
```

Example:
```bash
dotnet user-secrets set "AlphaVantage:ApiKey" "8P63H5GNUXPWN1SO"
```

3. Verify the secret has been stored successfully:

```bash
dotnet user-secrets list
```

## Build & Run

### 1. Clone the repository

```bash
git clone https://github.com/miyushan/FinTrack.git
cd FinTrack
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Build the project

```bash
dotnet build
```

### 4. Run the API

```bash
dotnet run --project FinTrack
```

The API will start at `http://localhost:5114`.

### 5. Access the API

Once running, open your browser and navigate to:

```
http://localhost:5114/swagger
```

This will open the Swagger UI where you can explore and test all available
endpoints interactively.

If you want to manually test the APIs, you can access the following endpoints
using a tool like curl or Postman:

```bash
# Get company details by stock symbol
GET http://localhost:5114/api/companies/{symbol}

# Example
GET http://localhost:5114/api/companies/AAPL

# Get top movers (gainers, losers, most active)
GET http://localhost:5114/api/movers/top-movers
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