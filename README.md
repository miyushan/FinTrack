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