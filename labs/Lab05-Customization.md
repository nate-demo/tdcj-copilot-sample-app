# Lab 05 — Copilot Customization

## Overview

In this lab you will explore GitHub Copilot's customization features to make Copilot more effective for your team's specific coding standards and patterns. You will create custom instructions, practice using Copilot in agent mode, and configure workspace-level settings.

---

## Objectives

- Create a `.github/copilot-instructions.md` file to encode team conventions
- Practice using Copilot workspace context to improve suggestions
- Use Copilot's `/fix`, `/doc`, and `/tests` slash commands
- Explore Copilot agent mode for multi-file refactoring

---

## Prerequisites

- Labs 01–04 complete
- GitHub Copilot Chat with agent mode enabled (VS Code 1.90+)

---

## Required Prompts & Tasks

### Section A — Custom Instructions

**Task 1:** Create `.github/copilot-instructions.md` with the following content, then verify that Copilot follows these rules in subsequent suggestions:

```
# Copilot Instructions for FacilityInspectionTracker

## Code Style
- Use C# 12 primary constructors where applicable
- Prefer `Task<T>` return types on all service and repository methods
- Use `ArgumentException` for input validation, not generic `Exception`
- Log using structured logging: `_logger.LogWarning("Message {Property}", value)`

## Testing
- All tests must use xUnit and FluentAssertions
- Mock dependencies using Moq
- Name tests using the pattern: MethodName_Condition_ExpectedResult

## Architecture
- Controllers must remain thin — no business logic
- Services must not reference controller models
- Repository interfaces must be defined in Core, not Infrastructure

## Error Handling
- Never swallow exceptions silently
- Always log before re-throwing
- Use problem details (RFC 7807) for API error responses
```

Then ask: `Following the custom instructions, add a new InspectorService class that provides a method to look up an inspector's inspection history. Generate the interface, implementation, and two tests.`

### Section B — Slash Commands

**Task 2:** Open `InspectionService.cs` and use:
- `/doc` — Generate documentation for all public methods
- `/fix` — Fix any code issues Copilot identifies
- `/tests` — Generate tests for uncovered methods

**Task 3:** Open `FacilitiesController.cs` and use:
- `/explain` — Ask Copilot to explain the controller's behavior
- `/tests` — Generate integration-style tests using `WebApplicationFactory`

### Section C — Agent Mode (Multi-File Refactoring)

**Task 4:** In agent mode, run this prompt:

`Refactor the exception handling across FacilityService.cs and InspectionService.cs. Replace all "catch (Exception) { return empty }" patterns with proper logging and typed exceptions. Update all affected unit tests to verify the exceptions are thrown. Make all changes in one operation.`

### Section D — Workspace Context

**Task 5:** Ask Copilot to use the full workspace:

`Looking at the entire FacilityInspectionTracker solution, what are the top 3 architectural improvements you would recommend? Explain the rationale for each.`

`Based on the current test files, what test scenarios are completely missing across the solution?`

---

## Expected Outcomes

- `.github/copilot-instructions.md` created and influencing suggestions
- At least one new feature scaffolded following custom instructions
- Slash commands used to generate docs, fixes, and tests
- Agent mode successfully completed a multi-file refactoring
- You understand how workspace context improves Copilot suggestions

---

## Success Criteria

- [ ] `.github/copilot-instructions.md` exists and contains team conventions
- [ ] `dotnet build` succeeds after all agent-mode changes
- [ ] `dotnet test` passes all tests
- [ ] At least one slash command was used successfully
- [ ] You completed the multi-file refactoring in agent mode
- [ ] You can explain how custom instructions change Copilot's behavior
