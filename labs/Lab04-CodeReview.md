# Lab 04 — Code Review with GitHub Copilot

## Overview

In this lab you will use GitHub Copilot to identify and remediate intentional technical debt embedded in the **Facility Inspection Tracker** codebase. You will practice using Copilot as a code reviewer and refactoring assistant.

---

## Objectives

- Use Copilot to identify code smells and quality issues
- Remediate hardcoded values, duplicate logic, and weak exception handling
- Improve test coverage using Copilot suggestions
- Understand the difference between a code smell and a security vulnerability

---

## Prerequisites

- Labs 01–03 complete
- Familiarity with all source files in the solution

---

## Technical Debt Catalog

The following issues are intentionally embedded in the codebase. Your task is to find and fix each one with Copilot's help.

| # | Issue | Location |
|---|-------|----------|
| 1 | Hardcoded connection string | `Program.cs` |
| 2 | Duplicate business logic | `FacilityService.cs` and `InspectionService.cs` |
| 3 | Missing validation for inspection date | `InspectionService.ScheduleInspectionAsync` |
| 4 | Weak exception handling | `FacilityService.cs`, `InspectionService.cs` |
| 5 | Low unit-test coverage | `FacilityServiceTests.cs`, `InspectionServiceTests.cs` |
| 6 | Outdated package reference | `FacilityInspectionTracker.Api.csproj` |
| 7 | Accessibility issue in Swagger | `Program.cs` SwaggerUI config |
| 8 | Config values embedded in source | `FacilityService.cs`, `InspectionService.cs` |

---

## Required Prompts

### Issue 1 — Hardcoded Connection String

Open `Program.cs` and ask:

`This file contains a hardcoded connection string. Rewrite it to read the connection string from IConfiguration using the key "ConnectionStrings:DefaultConnection". Show the change in Program.cs and the required entry in appsettings.json.`

### Issue 2 — Duplicate Business Logic

Open `FacilityService.cs` and `InspectionService.cs` side by side and ask:

`Both FacilityService and InspectionService contain similar active-filtering and count-limiting logic. Extract the count-limiting logic into a private static helper method or a shared extension method and eliminate the duplication. Show the refactored code.`

### Issue 3 — Missing Date Validation

Open `InspectionService.cs` and ask:

`ScheduleInspectionAsync does not validate that ScheduledDate is not in the past. Add validation that throws an ArgumentException with a descriptive message if ScheduledDate is before today's date.`

### Issue 4 — Weak Exception Handling

Open `FacilityService.cs` and ask:

`The catch block in GetAllFacilitiesAsync swallows exceptions silently. Refactor it to: 1) log the exception using ILogger, 2) re-throw as a typed exception rather than returning an empty list, 3) update the controller to handle this exception and return a 500 response.`

### Issue 5 — Low Test Coverage

`List all the untested code paths in FacilityService and InspectionService based on the comments marked "Technical Debt" in the test files. Write xUnit tests for each missing path.`

### Issue 6 — Outdated Package

`The project uses Swashbuckle.AspNetCore 6.5.0. What is the latest stable version? Show the csproj change needed to upgrade it and describe any breaking changes to watch for.`

### Issue 7 — Swagger Accessibility

`The Swagger UI configuration in Program.cs is missing the lang attribute on the HTML root element, which is an accessibility requirement (WCAG 2.1 SC 3.1.1). How can this be fixed using Swashbuckle's configuration options?`

### Issue 8 — Embedded Config Values

Open `FacilityService.cs` and ask:

`The constant MaxActiveFacilities = 50 is hardcoded. Move it to appsettings.json under a "FacilitySettings:MaxActiveFacilities" key. Inject IConfiguration and read the value at runtime with a fallback default of 50.`

---

## Expected Outcomes

- All 8 technical debt items identified and remediated
- Connection string moved to configuration
- Duplicate logic consolidated
- Date validation added with test coverage
- Exception handling improved with logging
- Test coverage increased
- Package version updated
- Swagger accessibility issue resolved
- Config values externalized

---

## Success Criteria

- [ ] `dotnet build` succeeds with no errors
- [ ] `dotnet test` passes all tests
- [ ] No hardcoded connection strings remain in source files
- [ ] `ScheduleInspectionAsync` rejects past dates with a clear error message
- [ ] At least 5 new unit tests were added
- [ ] `MaxActiveFacilities` is read from `appsettings.json`
