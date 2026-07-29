# Contributing to Facility Inspection Tracker

Thank you for your interest in contributing! This document explains how to add new features to the API so that contributions stay consistent with the existing codebase.

---

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Project Structure](#project-structure)
3. [How to Add a New Feature](#how-to-add-a-new-feature)
4. [Coding Conventions](#coding-conventions)
5. [Testing Guidelines](#testing-guidelines)
6. [Running Locally](#running-locally)
7. [Submitting Changes](#submitting-changes)

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Git
- An IDE with C# support (Visual Studio, VS Code + C# Dev Kit, or Rider)

---

## Project Structure

```
FacilityInspectionTracker/
├── src/
│   ├── FacilityInspectionTracker.Core/          # Domain models and interfaces (no external deps)
│   ├── FacilityInspectionTracker.Infrastructure/ # Service and repository implementations
│   └── FacilityInspectionTracker.Api/           # Controllers, request models, DI setup
├── tests/
│   └── FacilityInspectionTracker.Tests/         # xUnit tests
├── docs/
│   └── architecture.md                          # Architecture overview and request flow
└── labs/
    └── *.md                                     # Hands-on GitHub Copilot lab exercises
```

The three-layer architecture follows this dependency order:

```
FacilityInspectionTracker.Api
    └── FacilityInspectionTracker.Infrastructure
            └── FacilityInspectionTracker.Core
```

`Core` has **no** external project references; all interfaces and models live there so any layer can depend on them without creating circular references.

---

## How to Add a New Feature

Follow these steps in order. The example below walks through adding a hypothetical **InspectorService** that tracks inspectors.

### 1. Add the Model (Core)

Create a new class in `src/FacilityInspectionTracker.Core/Models/`:

```csharp
// src/FacilityInspectionTracker.Core/Models/Inspector.cs
namespace FacilityInspectionTracker.Core.Models;

/// <summary>Represents an inspector who can be assigned to facility inspections.</summary>
public class Inspector
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string BadgeNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
```

### 2. Add the Repository Interface (Core)

Create a new interface in `src/FacilityInspectionTracker.Core/Interfaces/`:

```csharp
// src/FacilityInspectionTracker.Core/Interfaces/IInspectorRepository.cs
namespace FacilityInspectionTracker.Core.Interfaces;

public interface IInspectorRepository
{
    Task<IEnumerable<Inspector>> GetAllAsync();
    Task<Inspector?> GetByIdAsync(int id);
}
```

### 3. Add the Service Interface (Core)

```csharp
// src/FacilityInspectionTracker.Core/Interfaces/IInspectorService.cs
namespace FacilityInspectionTracker.Core.Interfaces;

public interface IInspectorService
{
    Task<IEnumerable<Inspector>> GetAllInspectorsAsync();
    Task<Inspector?> GetInspectorByIdAsync(int id);
}
```

### 4. Implement the Repository (Infrastructure)

Create the implementation in `src/FacilityInspectionTracker.Infrastructure/Repositories/`:

```csharp
// src/FacilityInspectionTracker.Infrastructure/Repositories/InspectorRepository.cs
namespace FacilityInspectionTracker.Infrastructure.Repositories;

public class InspectorRepository : IInspectorRepository
{
    private readonly List<Inspector> _inspectors = new();

    public Task<IEnumerable<Inspector>> GetAllAsync() =>
        Task.FromResult<IEnumerable<Inspector>>(_inspectors.ToList());

    public Task<Inspector?> GetByIdAsync(int id) =>
        Task.FromResult(_inspectors.FirstOrDefault(i => i.Id == id));
}
```

### 5. Implement the Service (Infrastructure)

Create the implementation in `src/FacilityInspectionTracker.Infrastructure/Services/`:

```csharp
// src/FacilityInspectionTracker.Infrastructure/Services/InspectorService.cs
namespace FacilityInspectionTracker.Infrastructure.Services;

public class InspectorService : IInspectorService
{
    private readonly IInspectorRepository _repo;

    public InspectorService(IInspectorRepository repo) => _repo = repo;

    public async Task<IEnumerable<Inspector>> GetAllInspectorsAsync() =>
        await _repo.GetAllAsync();

    public async Task<Inspector?> GetInspectorByIdAsync(int id) =>
        await _repo.GetByIdAsync(id);
}
```

### 6. Register in DI (`Program.cs`)

Add the registrations in `src/FacilityInspectionTracker.Api/Program.cs` alongside the existing ones:

```csharp
// Repositories — Singleton to preserve in-memory state
builder.Services.AddSingleton<IInspectorRepository, InspectorRepository>();

// Services — Scoped (new instance per HTTP request)
builder.Services.AddScoped<IInspectorService, InspectorService>();
```

### 7. Add the Controller (Api)

Create a new controller in `src/FacilityInspectionTracker.Api/Controllers/`:

```csharp
// src/FacilityInspectionTracker.Api/Controllers/InspectorsController.cs
[ApiController]
[Route("api/[controller]")]
public class InspectorsController : ControllerBase
{
    private readonly IInspectorService _inspectorService;

    public InspectorsController(IInspectorService inspectorService) =>
        _inspectorService = inspectorService;

    /// <summary>Retrieves all active inspectors.</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var inspectors = await _inspectorService.GetAllInspectorsAsync();
        return Ok(inspectors);
    }

    /// <summary>Retrieves a specific inspector by their identifier.</summary>
    /// <param name="id">The inspector ID.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var inspector = await _inspectorService.GetInspectorByIdAsync(id);
        if (inspector is null)
            return NotFound(new { message = $"Inspector {id} not found." });

        return Ok(inspector);
    }
}
```

### 8. Write Unit Tests

Add tests in `tests/FacilityInspectionTracker.Tests/` following the naming pattern `MethodName_Condition_ExpectedResult`:

```csharp
// tests/FacilityInspectionTracker.Tests/InspectorServiceTests.cs
public class InspectorServiceTests
{
    [Fact]
    public async Task GetInspectorByIdAsync_ReturnsNull_WhenInspectorNotFound()
    {
        var repo = new Mock<IInspectorRepository>();
        repo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Inspector?)null);

        var service = new InspectorService(repo.Object);
        var result = await service.GetInspectorByIdAsync(99);

        result.Should().BeNull();
    }
}
```

---

## Coding Conventions

| Concern | Convention |
|---------|-----------|
| **Language** | C# 12, nullable reference types enabled |
| **Async** | All service and repository methods return `Task<T>` |
| **Validation** | Throw `ArgumentException` (not generic `Exception`) for invalid input |
| **Exception handling** | Never silently swallow exceptions; log before re-throwing |
| **XML docs** | All `public` classes, interfaces, and methods must have `/// <summary>` comments |
| **DI lifetime** | Repositories → `Singleton`; Services → `Scoped` |
| **Controllers** | Stay thin — no business logic; delegate everything to the service layer |
| **Architecture** | Services must not reference `Api` models; Core must have no external project references |

---

## Testing Guidelines

- Use **xUnit** as the test framework
- Use **Moq** to mock interfaces
- Use **FluentAssertions** for readable assertions
- Name tests using `MethodName_Condition_ExpectedResult`
- Cover at minimum: happy path, not-found, and invalid-input scenarios

Run all tests with:

```bash
dotnet test
```

---

## Running Locally

```bash
# Restore packages
dotnet restore

# Build the solution
dotnet build

# Run the API (Swagger UI opens at http://localhost:5000)
dotnet run --project src/FacilityInspectionTracker.Api
```

---

## Submitting Changes

1. Fork the repository and create a feature branch: `git checkout -b feature/my-feature`
2. Make your changes following the conventions above
3. Ensure `dotnet build` succeeds with zero errors or warnings
4. Ensure `dotnet test` passes all tests
5. Open a pull request with a clear description of what was changed and why
