# Lab 06 — GitHub Copilot Cloud Agent

## Overview

In this lab you will use the **GitHub Copilot Coding Agent** (cloud agent) to implement a significant new feature autonomously. You will assign a GitHub issue to Copilot, review the resulting pull request, and practice giving feedback that improves the agent's output.

---

## Objectives

- Create a well-formed GitHub issue that the cloud agent can act on
- Assign the issue to GitHub Copilot and monitor progress
- Review and provide feedback on an agent-generated pull request
- Understand what tasks are best suited for cloud agent vs. inline Copilot

---

## Prerequisites

- Labs 01–05 complete
- Repository hosted on GitHub with Copilot coding agent enabled
- Write access to create issues and review PRs

---

## Part 1 — Write an Effective Issue

### What Makes a Good Issue for the Cloud Agent

The cloud agent works best when the issue:
- Has a clear, single-responsibility scope
- Describes acceptance criteria explicitly
- References existing files and patterns to follow
- Specifies what NOT to change

### Task 1 — Create the Issue

Create a GitHub issue with the following title and body:

**Title:** `Add PATCH /api/inspections/{id} endpoint to update inspection notes and priority`

**Body:**
```
## Summary
Add a new PATCH endpoint that allows updating the Notes and Priority fields of an existing inspection.

## Acceptance Criteria
- [ ] New `PatchInspectionRequest` model with `Notes` (string?) and `Priority` (string?) fields in `FacilityInspectionTracker.Api/Models/`
- [ ] New `PatchInspectionAsync(int id, string? notes, InspectionPriority? priority)` method on `IInspectionService` interface
- [ ] Implementation in `InspectionService.cs` that updates only the provided fields
- [ ] `PATCH /api/inspections/{id}` endpoint in `InspectionsController.cs` returning 200 on success, 404 if not found
- [ ] At least 3 xUnit tests in `InspectionServiceTests.cs` covering: success, not-found, and partial-update scenarios

## Do Not Change
- Do not modify `FacilitiesController.cs`
- Do not change existing endpoint signatures
- Do not add new NuGet packages

## Patterns to Follow
- Follow the same DI and async patterns as `ScheduleInspectionAsync`
- Use the same response shapes as the existing endpoints (200 with object, 404 with `{ message }`)
- Tests should use Moq and FluentAssertions following `InspectionServiceTests.cs`
```

### Task 2 — Assign to Copilot

In the GitHub issue UI, assign the issue to **@copilot** (or use the "Ask Copilot" button if available). Monitor the session via the issue comments or the linked PR.

---

## Part 2 — Review the Pull Request

Once the cloud agent opens a PR, review it using the following checklist:

### Review Checklist

**Architecture**
- [ ] `PatchInspectionRequest` model is in `Api/Models/`, not in Core or Infrastructure
- [ ] Interface change is in `Core/Interfaces/IInspectionService.cs`
- [ ] Implementation is in `Infrastructure/Services/InspectionService.cs`
- [ ] Controller stays thin — no business logic added to `InspectionsController.cs`

**Correctness**
- [ ] The endpoint returns 200 with the updated inspection on success
- [ ] The endpoint returns 404 when the inspection ID does not exist
- [ ] Only `Notes` and `Priority` are updated — `Status`, `ScheduledDate`, etc. are not changed
- [ ] Null fields in the request are treated as "no change" (partial update semantics)

**Tests**
- [ ] At least 3 tests present in `InspectionServiceTests.cs`
- [ ] Tests follow naming convention `MethodName_Condition_ExpectedResult`
- [ ] All tests use Moq and FluentAssertions
- [ ] `dotnet test` passes

**Quality**
- [ ] No new hardcoded values introduced
- [ ] No new packages added
- [ ] XML doc comment on the new controller action

### Task 3 — Leave Feedback

If the PR does not meet the checklist, leave specific review comments. Examples of good feedback comments:

- `The PatchInspectionRequest model was added to Infrastructure — please move it to Api/Models/ to follow the pattern in ScheduleInspectionRequest.cs.`
- `The test for the not-found case is missing. Add a test called PatchInspectionAsync_ReturnsNull_WhenInspectionNotFound.`
- `The PATCH handler currently overwrites Notes even when the request field is null. Null should mean "no change".`

### Task 4 — Re-request Changes

If feedback requires changes, select "Request changes" on the GitHub PR review and @mention `@copilot` in a comment summarizing the required fixes. Observe the agent's follow-up commits.

---

## Part 3 — Reflect

After the PR is merged (or ready to merge), answer these questions:

1. What did the cloud agent implement correctly on the first attempt?
2. What required your feedback to correct?
3. What would have been faster to do manually vs. delegating to the agent?
4. How did the quality of the original issue affect the quality of the agent's output?

---

## Expected Outcomes

- A well-formed GitHub issue following the template above
- A PR opened by the cloud agent implementing the PATCH endpoint
- A code review with specific, actionable feedback
- A PR that meets all acceptance criteria after any necessary feedback rounds

---

## Success Criteria

- [ ] Issue created with all required sections
- [ ] Issue assigned to @copilot
- [ ] PR opened by Copilot and reviewed
- [ ] All acceptance criteria on the issue are checked off in the PR
- [ ] `dotnet build` and `dotnet test` pass on the PR branch
- [ ] You provided at least one specific code review comment (even if the PR was perfect)
- [ ] Reflection questions answered (in your notes or as a PR comment)
