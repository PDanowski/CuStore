# CuStore .NET 10 Migration

This repository contains the migrated stack for .NET 10 with ASP.NET Core and gRPC.

## New architecture

- `CuStore.Core`: domain entities and business abstractions
- `CuStore.Infrastructure`: EF Core data access, repositories, checkout service
- `CuStore.Crm.Contracts`: shared gRPC `.proto` contract
- `CuStore.Crm.GrpcService`: gRPC CRM backend replacing WCF service
- `CuStore.WebApi`: ASP.NET Core Web API replacing MVC5/WCF client integration
- `CuStore.UnitTests`: xUnit tests for core cart/checkout flow

## Legacy to new mapping

- `CuStore.CRMService` (WCF) -> `CuStore.Crm.GrpcService` (gRPC)
- `CuStore.Domain` (EF6 + ASP.NET Identity coupling) -> `CuStore.Core` + `CuStore.Infrastructure` (EF Core)
- `CuStore.WebUI` (MVC5 + OWIN) -> `CuStore.WebApi` (ASP.NET Core API)

## Implemented API surface

- `GET /api/products`
- `GET /api/products/{id}`
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

## Runtime notes

- `CuStore.WebApi` uses SQLite `custore.store.db`
- `CuStore.Crm.GrpcService` uses SQLite `custore.crm.db`
- gRPC endpoint URL is configured in `CuStore.WebApi/appsettings.json` under `Grpc:CrmUrl`

## Remaining parity work

- Port MVC views/Razor pages UI from legacy `CuStore.WebUI` if UI parity is required
- Port account/login flows to ASP.NET Core Identity + auth tokens/cookies
- Add richer validation and integration tests for gRPC failure paths and auth scenarios
