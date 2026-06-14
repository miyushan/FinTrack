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
