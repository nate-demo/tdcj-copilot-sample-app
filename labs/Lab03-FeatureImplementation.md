# Lab 03 — Feature Implementation with GitHub Copilot

## Overview

In this lab you will use GitHub Copilot to implement a complete new feature end-to-end: **Inspection Reports**. This feature allows generating a summary report for a facility showing completed inspections and average scores.

---

## Objectives

- Use Copilot to scaffold a new feature from scratch
- Practice the full implementation cycle: model → interface → service → repository → controller → tests
- Validate that Copilot-generated code integrates correctly with existing patterns

---

## Prerequisites

- Labs 01 and 02 complete
- Solution builds and all existing tests pass

---

## Feature: Inspection Report

An inspection report for a facility should include:
- Facility name, city, and type
- Total number of inspections
- Number of completed inspections
- Average inspection score (completed inspections only)
- Most recent inspection date
- List of inspections with status and priority

---

## Required Prompts

### Step 1 — Model

Open `src/FacilityInspectionTracker.Core/Models/` and ask:

`Create a new model class InspectionReport in the FacilityInspectionTracker.Core.Models namespace. It should contain: FacilityId (int), FacilityName (string), City (string), FacilityType (string), TotalInspections (int), CompletedInspections (int), AverageScore (double?), MostRecentInspectionDate (DateTime?), and Inspections (List<Inspection>).`

### Step 2 — Service Interface

Open `src/FacilityInspectionTracker.Core/Interfaces/IInspectionService.cs` and ask:

`Add a method signature Task<InspectionReport?> GetInspectionReportAsync(int facilityId) to the IInspectionService interface.`

### Step 3 — Service Implementation

Open `src/FacilityInspectionTracker.Infrastructure/Services/InspectionService.cs` and ask:

`Implement GetInspectionReportAsync(int facilityId). It should: 1) look up the facility by ID and return null if not found, 2) get all inspections for that facility, 3) calculate TotalInspections, CompletedInspections, AverageScore (average of non-null Score on completed inspections), and MostRecentInspectionDate, 4) return a populated InspectionReport object.`

### Step 4 — Controller Endpoint

Open `src/FacilityInspectionTracker.Api/Controllers/FacilitiesController.cs` and ask:

`Add a GET /api/facilities/{id}/report endpoint that calls IInspectionService.GetInspectionReportAsync. Return 200 with the report or 404 if the facility is not found. Add XML summary comments.`

### Step 5 — Unit Tests

Open `tests/FacilityInspectionTracker.Tests/InspectionServiceTests.cs` and ask:

`Write three xUnit tests for GetInspectionReportAsync: 1) returns null when facility not found, 2) returns a report with correct counts when there are completed and scheduled inspections, 3) returns AverageScore as null when no inspections have a score.`

### Step 6 — Integration Check

`Review FacilitiesController.cs and InspectionService.cs together. Are there any inconsistencies between the interface and implementation? Does the controller correctly handle null returns?`

---

## Expected Outcomes

- A working `GET /api/facilities/{id}/report` endpoint
- `InspectionReport` model in Core
- `GetInspectionReportAsync` implemented in `InspectionService`
- 3 new unit tests that pass
- All existing tests still pass

---

## Success Criteria

- [ ] `dotnet build` succeeds with no errors
- [ ] `dotnet test` passes all tests including your new ones
- [ ] `GET /api/facilities/1/report` returns valid JSON with accurate counts
- [ ] `GET /api/facilities/999/report` returns HTTP 404
- [ ] The new feature follows the existing coding patterns (DI, async/await, interface-based)
