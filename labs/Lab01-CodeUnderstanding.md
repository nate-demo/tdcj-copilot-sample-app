# Lab 01 — Code Understanding with GitHub Copilot

## Overview

In this lab you will use GitHub Copilot Chat to navigate and understand an unfamiliar codebase — the **Facility Inspection Tracker** API. You will learn how to ask Copilot targeted questions about architecture, data flow, and individual components without reading every line of code manually.

---

## Objectives

- Use Copilot Chat to understand project structure and architecture
- Generate plain-language explanations of existing code
- Ask Copilot to trace a request from HTTP endpoint to data layer
- Identify relationships between models, services, and repositories

---

## Prerequisites

- GitHub Copilot extension installed and signed in (VS Code or Visual Studio)
- Repository cloned locally
- Solution builds successfully (`dotnet build`)

---

## Required Prompts

Work through each prompt in Copilot Chat. Open the relevant file before asking file-scoped questions.

### Section A — Project Structure

1. `Explain the overall architecture of this solution. What are the layers and how do they communicate?`

2. `What is the role of FacilityInspectionTracker.Core? Why does it have no project dependencies?`

3. `How does dependency injection wire together the repositories and services in this project?`

### Section B — Models

4. Open `src/FacilityInspectionTracker.Core/Models/Inspection.cs` and ask:
   `Explain each property on the Inspection model and what it represents in a real-world inspection workflow.`

5. `What is the relationship between Facility and Inspection? How is it expressed in code?`

### Section C — Request Tracing

6. `Trace a GET /api/inspections HTTP request from the controller through to the data layer. Show each method call.`

7. `What happens when a client calls POST /api/inspections? Walk me through each layer.`

### Section D — Data

8. Open `src/FacilityInspectionTracker.Infrastructure/Data/SeedData.cs` and ask:
   `What sample data does this project include? Summarize the facilities and inspections.`

---

## Expected Outcomes

- You can describe the three-layer architecture (API → Service → Repository) in your own words
- You understand how interfaces in Core decouple the layers
- You can trace any HTTP request through the codebase with Copilot's help
- You recognize the in-memory data store and understand how it would be replaced with a real database

---

## Success Criteria

- [ ] You asked at least 6 of the prompts above and received meaningful responses
- [ ] You can explain what `IFacilityService` and `IInspectionRepository` interfaces are for
- [ ] You understand what `SeedData.GetFacilities()` returns and why it exists
- [ ] You identified that `FacilityRepository` and `InspectionRepository` are registered as **Singleton** in DI — and can explain why
