# CuStore .NET 10 Migration

This repository contains the migrated CuStore stack on .NET 10 using ASP.NET Core and gRPC.

## What was migrated

- `CuStore.CRMService` (WCF) -> `CuStore.Crm.GrpcService` (gRPC)
- `CuStore.Domain` (EF6 + ASP.NET Identity coupling) -> `CuStore.Core` + `CuStore.Infrastructure` (EF Core)
- `CuStore.WebUI` (ASP.NET MVC5 + OWIN) -> `CuStore.WebUI` (ASP.NET Core MVC)

## Current shape of the solution

Projects in `CuStore.slnx`:

- `CuStore.Core`: domain entities and business abstractions
- `CuStore.Infrastructure`: EF Core data access, repositories, checkout service
- `CuStore.Crm.Contracts`: shared gRPC contracts and generated gRPC C# types
- `CuStore.Crm.GrpcService`: CRM backend over gRPC
- `CuStore.Api`: backend HTTP API used by UI
- `CuStore.WebUI`: ASP.NET Core MVC storefront (presentation layer) calling `CuStore.Api`
- `CuStore.UnitTests`: xUnit tests for cart/checkout flow

### Runtime topology

In development, the system runs as 3 cooperating services:

1. `CuStore.WebUI` (MVC frontend)
2. `CuStore.Api` (application/backend API)
3. `CuStore.Crm.GrpcService` (CRM over gRPC)

Call flow:

1. Browser -> `CuStore.WebUI`
2. `CuStore.WebUI` -> `CuStore.Api` (HTTP JSON)
3. `CuStore.Api` -> `CuStore.Crm.GrpcService` (gRPC) for CRM operations

## API surface (`CuStore.Api`)

- `GET /api/products`
- `GET /api/products/{id}`
- `GET /api/shippingmethods`
- `GET /api/users/{userId}/cart`
- `POST /api/users/{userId}/cart/items`
- `DELETE /api/users/{userId}/cart/items/{productId}`
- `POST /api/users/{userId}/register`
- `POST /api/users/{userId}/checkout`

## Build and test

```powershell
dotnet build .\CuStore.slnx -c Release
dotnet test .\CuStore.slnx -c Release
```

## Run locally (all services)

Start each project in a separate terminal:

```powershell
dotnet run --project .\CuStore.Crm.GrpcService
dotnet run --project .\CuStore.Api
dotnet run --project .\CuStore.WebUI
```

Default development URLs (from launch settings):

- `CuStore.WebUI`: `http://localhost:5215` (or `https://localhost:7194`)
- `CuStore.Api`: `http://localhost:5123` (or `https://localhost:7231`)
- `CuStore.Crm.GrpcService`: `http://localhost:5233` (or `https://localhost:7184`)

## Configuration and data

- `CuStore.WebUI/appsettings.json`
  - `Api:BaseUrl`: URL of `CuStore.Api` (default `http://localhost:5123`)
- `CuStore.Api/appsettings.json`
  - `Grpc:CrmUrl`: URL of `CuStore.Crm.GrpcService`
  - `ConnectionStrings:Store`: SQLite store DB path
- `CuStore.Crm.GrpcService/appsettings.json`
  - `ConnectionStrings:Crm`: SQLite CRM DB path

SQLite files used in development:

- `custore.store.db` (store data)
- `custore.crm.db` (CRM data)

## Remaining parity work

- Port account/login flows to ASP.NET Core Identity + auth tokens/cookies
- Add richer validation and integration tests for gRPC failure paths and auth scenarios
