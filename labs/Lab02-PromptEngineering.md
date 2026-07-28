# Lab 02 — Prompt Engineering with GitHub Copilot

## Overview

In this lab you will practice crafting effective prompts for GitHub Copilot to generate high-quality code, tests, and documentation. You will compare vague prompts against precise prompts and observe how prompt quality directly affects output quality.

---

## Objectives

- Understand the elements of an effective Copilot prompt
- Practice iterative prompt refinement
- Use context anchoring (opening a file before prompting) to improve results
- Generate code, tests, and docs using well-formed prompts

---

## Prerequisites

- Lab 01 complete
- Familiarity with the codebase from Lab 01

---

## Required Prompts

### Section A — Prompt Quality Comparison

For each pair below, run the **weak** prompt first, then the **strong** prompt. Compare the outputs.

**Exercise 1 — Adding a Feature**

Weak: `Add a new endpoint`

Strong: `Add a GET /api/facilities/{id}/inspections endpoint to FacilitiesController that returns all inspections for a given facility ID. Use the existing IInspectionService.GetInspectionsByFacilityAsync method. Return 404 if the facility is not found.`

**Exercise 2 — Writing Tests**

Weak: `Write a test`

Strong: `Write an xUnit test in InspectionServiceTests.cs that verifies CancelInspectionAsync returns false when called with an inspection that has Status == Completed. Use Moq to mock IInspectionRepository and IFacilityRepository.`

**Exercise 3 — Explaining Code**

Weak: `What does this do?`

Strong (with `FacilityService.cs` open): `Explain why GetAllFacilitiesAsync catches all exceptions and returns an empty collection. What are the risks of this pattern and what would a better approach look like?`

### Section B — Context-Aware Prompts

4. Open `Program.cs` and ask:
   `This file registers repositories as Singleton and services as Scoped. Explain the difference and why this combination is used here. Then suggest whether the registration should be changed.`

5. Open `InspectionService.cs` and ask:
   `This service method ScheduleInspectionAsync does not validate that ScheduledDate is in the future. Write the validation logic and show where to insert it. Also write an xUnit test that verifies the validation works.`

### Section C — Documentation Generation

6. `Generate XML doc comments for all public methods in FacilitiesController.cs following the existing comment style.`

7. `Write a CONTRIBUTING.md that explains how to add a new feature to this API, including where to add the model, interface, service, repository, controller, and test.`

### Section D — Iterative Refinement

8. Start with this prompt and refine it through three iterations:

   - Iteration 1: `Add logging`
   - Iteration 2: `Add ILogger<T> to FacilityService and log when GetAllFacilitiesAsync is called`
   - Iteration 3: `Inject ILogger<FacilityService> into FacilityService. Add a structured log entry at the start of GetAllFacilitiesAsync that logs the method name and timestamp at Information level. Add a Warning log when the catch block is hit that includes the exception message.`

---

## Expected Outcomes

- You can articulate what makes a prompt strong vs. weak
- You understand how file context influences Copilot's output
- You can use Copilot to generate tests, validation logic, and documentation
- You practiced iterative refinement to improve an initial prompt

---

## Success Criteria

- [ ] You compared at least 2 weak/strong prompt pairs and noted differences in output
- [ ] You generated at least one test using a precise prompt
- [ ] You used the context-anchoring technique (opening a file before prompting) at least once
- [ ] You completed the iterative refinement exercise through all 3 iterations
