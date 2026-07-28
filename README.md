# Facility Inspection Tracker

A **GitHub Copilot training repository** built with ASP.NET Core 8. It provides a realistic REST API for managing TDCJ facility inspections and is designed to support six hands-on Copilot labs.

---

## Technology Stack

| Component | Technology |
|-----------|-----------|
| Framework | ASP.NET Core 8 Web API |
| Language | C# 12 |
| API Docs | Swagger / Swashbuckle |
| DI | ASP.NET Core built-in DI |
| Tests | xUnit, Moq, FluentAssertions |
| CI/CD | GitHub Actions |

---

## Features

### Facilities
- `GET /api/facilities` — List all active facilities
- `GET /api/facilities/{id}` — Get a facility by ID

### Inspections
- `GET /api/inspections` — List all inspections
- `GET /api/inspections/{id}` — Get inspection by ID
- `GET /api/inspections/facility/{facilityId}` — List inspections for a facility
- `POST /api/inspections` — Schedule a new inspection
- `DELETE /api/inspections/{id}/cancel` — Cancel an inspection

---

## Project Structure

```
FacilityInspectionTracker/
├── src/
│   ├── FacilityInspectionTracker.Core/          # Models and interfaces
│   ├── FacilityInspectionTracker.Infrastructure/ # Services and repositories
│   └── FacilityInspectionTracker.Api/           # Controllers, Swagger, DI setup
├── tests/
│   └── FacilityInspectionTracker.Tests/         # xUnit tests
├── docs/
│   └── architecture.md                          # Architecture overview
└── labs/
    ├── Lab01-CodeUnderstanding.md
    ├── Lab02-PromptEngineering.md
    ├── Lab03-FeatureImplementation.md
    ├── Lab04-CodeReview.md
    ├── Lab05-Customization.md
    └── Lab06-CloudAgent.md
```

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Run

```bash
dotnet restore
dotnet build
dotnet run --project src/FacilityInspectionTracker.Api
```

Navigate to [http://localhost:5000](http://localhost:5000) to open the Swagger UI.

### Test

```bash
dotnet test
```

---

## Labs

| Lab | Topic |
|-----|-------|
| [Lab 01](labs/Lab01-CodeUnderstanding.md) | Code Understanding |
| [Lab 02](labs/Lab02-PromptEngineering.md) | Prompt Engineering |
| [Lab 03](labs/Lab03-FeatureImplementation.md) | Feature Implementation |
| [Lab 04](labs/Lab04-CodeReview.md) | Code Review & Technical Debt |
| [Lab 05](labs/Lab05-Customization.md) | Copilot Customization |
| [Lab 06](labs/Lab06-CloudAgent.md) | Cloud Agent |

---

## Intentional Technical Debt

This repository contains **intentional technical debt** for use in Lab 04:

1. Hardcoded connection string in `Program.cs`
2. Duplicate business logic in `FacilityService` and `InspectionService`
3. Missing validation for inspection date
4. Weak exception handling (swallowed exceptions)
5. Low unit-test coverage
6. Outdated `Swashbuckle.AspNetCore` package version
7. Accessibility issue in Swagger UI configuration
8. Config values embedded as source-code constants

See [docs/architecture.md](docs/architecture.md) for a full description of each issue.

---

## Architecture

See [docs/architecture.md](docs/architecture.md) for the request flow diagram, project dependency map, and design decisions.
