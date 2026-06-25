# ADR-0021 — Logical Store Access Validation

**Status:** Proposed  
**Sprint:** 5  
**SubSprint:** CC_S5.2

## Proposed decision

Store access is validated on the existing logical grid rather than through
physics or NavMesh.

The initial layout uses:

- Bounds: `20 x 30` cells from `(0, 0)`.
- Entrance reserve: `(8,0)` through `(11,0)`.
- Minimum functional entrance width: `2` adjacent cells.
- Required anchor `rear-service` at `(10,27)`.
- Required anchor `left-display` at `(3,15)`.
- Required anchor `right-display` at `(16,15)`.

Connectivity uses four-direction movement only:

- positive X;
- negative X;
- positive Z;
- negative Z.

Diagonal movement does not satisfy access.

## Candidate-placement policy

`ValidateWithCandidate` evaluates the union of current blocked cells and a
proposed footprint without mutating either input.

A candidate is rejected when:

- any candidate cell enters the four-cell reserved entrance;
- a blocked cell lies outside logical bounds;
- fewer than two adjacent entrance cells remain open;
- a required anchor cell is blocked;
- any required anchor cannot be reached from an open entrance cell.

## Rationale

Pure logical validation is deterministic, testable without scenes and
compatible with the Sprint 4 occupancy model. It avoids coupling access
rules to colliders, camera state, NavMesh baking or runtime views.

## Deferred

- Store placement integration.
- Preview colour integration for access failures.
- Dynamic NavMesh updates.
- Final door and interaction points.
- Economy and persistence.
