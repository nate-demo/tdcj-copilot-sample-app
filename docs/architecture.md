# Architecture Overview — Facility Inspection Tracker

## Table of Contents

1. [Project Overview](#project-overview)
2. [Request Flow](#request-flow)
3. [Project Structure](#project-structure)
4. [Dependencies](#dependencies)
5. [Design Decisions](#design-decisions)
6. [Known Technical Debt](#known-technical-debt)

---

## Project Overview

The **Facility Inspection Tracker** is an ASP.NET Core 8 Web API that manages TDCJ facility inspections. It provides endpoints for querying facilities, scheduling inspections, and tracking inspection status.

---

## Request Flow

```
Client (HTTP Request)
        │
        ▼
┌─────────────────────────────┐
│  ASP.NET Core Middleware    │  ← Routing, Auth, Swagger
└────────────┬────────────────┘
             │
        ▼
┌─────────────────────────────┐
│      API Controllers        │  ← FacilitiesController
│  (FacilityInspectionTracker │     InspectionsController
│         .Api)               │
└────────────┬────────────────┘
             │  Calls IFacilityService / IInspectionService
        ▼
┌─────────────────────────────┐
│      Service Layer          │  ← FacilityService
│  (FacilityInspectionTracker │     InspectionService
│     .Infrastructure)        │  ← Business logic lives here
└────────────┬────────────────┘
             │  Calls IFacilityRepository / IInspectionRepository
        ▼
┌─────────────────────────────┐
│    Repository Layer         │  ← FacilityRepository
│  (FacilityInspectionTracker │     InspectionRepository
│     .Infrastructure)        │  ← Data access abstraction
└────────────┬────────────────┘
             │  (In-memory; would use EF Core / SQL in production)
        ▼
┌─────────────────────────────┐
│      In-Memory Data Store   │  ← SeedData.cs (sample data)
└─────────────────────────────┘
```

---

## Project Structure

```
FacilityInspectionTracker/
├── src/
│   ├── FacilityInspectionTracker.Core/
│   │   ├── Models/
│   │   │   ├── Facility.cs
│   │   │   ├── Inspection.cs
│   │   │   └── InspectionPriority.cs
│   │   └── Interfaces/
│   │       ├── IFacilityRepository.cs
│   │       ├── IInspectionRepository.cs
│   │       ├── IFacilityService.cs
│   │       └── IInspectionService.cs
│   │
│   ├── FacilityInspectionTracker.Infrastructure/
│   │   ├── Data/
│   │   │   └── SeedData.cs
│   │   ├── Repositories/
│   │   │   ├── FacilityRepository.cs
│   │   │   └── InspectionRepository.cs
│   │   └── Services/
│   │       ├── FacilityService.cs
│   │       └── InspectionService.cs
│   │
│   └── FacilityInspectionTracker.Api/
│       ├── Controllers/
│       │   ├── FacilitiesController.cs
│       │   └── InspectionsController.cs
│       ├── Models/
│       │   └── ScheduleInspectionRequest.cs
│       └── Program.cs
│
├── tests/
│   └── FacilityInspectionTracker.Tests/
│       ├── FacilityServiceTests.cs
│       └── InspectionServiceTests.cs
│
├── docs/
│   └── architecture.md
│
└── labs/
    ├── Lab01-CodeUnderstanding.md
    ├── Lab02-PromptEngineering.md
    ├── Lab03-FeatureImplementation.md
    ├── Lab04-CodeReview.md
    ├── Lab05-Customization.md
    └── Lab06-CloudAgent.md
```

---

## Dependencies

### FacilityInspectionTracker.Core
- No external dependencies
- Contains domain models and service/repository interfaces
- All other projects reference Core

### FacilityInspectionTracker.Infrastructure
- References: `FacilityInspectionTracker.Core`
- Implements repositories (in-memory) and business services

### FacilityInspectionTracker.Api
- References: `FacilityInspectionTracker.Core`, `FacilityInspectionTracker.Infrastructure`
- NuGet: `Swashbuckle.AspNetCore 6.5.0` (Swagger)
- Entry point; wires DI container and middleware

### FacilityInspectionTracker.Tests
- References: `FacilityInspectionTracker.Core`, `FacilityInspectionTracker.Infrastructure`
- NuGet: `xUnit`, `Moq`, `FluentAssertions`

---

## Design Decisions

### Repository Pattern
The repository pattern isolates data access from business logic. Interfaces (`IFacilityRepository`, `IInspectionRepository`) are defined in Core so they can be swapped to a real EF Core/SQL Server implementation without changing service or controller code.

### Service Layer
Business logic lives in the Infrastructure service classes. Controllers are kept thin — they parse requests, call services, and return HTTP results.

### Dependency Injection
All dependencies are registered in `Program.cs` using ASP.NET Core's built-in DI container:
- Repositories: `Singleton` (preserve in-memory state across requests)
- Services: `Scoped` (new instance per HTTP request)

### In-Memory Data Store
The current implementation uses in-memory `List<T>` collections initialized with realistic sample data. A production deployment would replace repositories with EF Core targeting SQL Server.

### Swagger / OpenAPI
Swagger is enabled in the Development environment only. XML doc comments on controller actions populate Swagger descriptions automatically.

---

## Known Technical Debt

The following issues exist intentionally for demonstration and training purposes:

| # | Issue | Location |
|---|-------|----------|
| 1 | **Hardcoded connection string** | `Program.cs` — connection string stored as a literal string |
| 2 | **Duplicate business logic** | `FacilityService.cs` and `InspectionService.cs` both implement active-facility filtering and count-limiting logic |
| 3 | **Missing validation for inspection date** | `InspectionService.ScheduleInspectionAsync` — no check that `ScheduledDate` is in the future |
| 4 | **Weak exception handling** | `FacilityService` and `InspectionService` — `catch (Exception)` swallows errors silently |
| 5 | **Low unit-test coverage** | Only `FacilityService` and `InspectionService` are covered; controllers, repositories, and edge cases are untested |
| 6 | **Outdated package reference** | `Swashbuckle.AspNetCore 6.5.0` is used; a newer version is available |
| 7 | **Accessibility issue in Swagger** | `Program.cs` SwaggerUI configuration is missing `lang` attribute on the HTML root element |
| 8 | **Config value embedded in source code** | `MaxActiveFacilities` (50) and `MaxInspectionsPerFacility` (20) are hardcoded constants instead of being read from `appsettings.json` |
