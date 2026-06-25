# ADR-0021 — Logical Store Access Validation

**Status:** Accepted  
**Sprint:** 5  
**Decision date:** 2026-06-25

## Decision

Store access is validated on the logical grid rather than through physics or
NavMesh.

The accepted initial layout uses:

- Bounds: `20 x 30` cells from `(0, 0)`.
- Entrance reserve: `(8,0)` through `(11,0)`.
- Minimum open entrance width: two adjacent cells.
- Required anchor `rear-service` at `(10,27)`.
- Required anchor `left-display` at `(3,15)`.
- Required anchor `right-display` at `(16,15)`.

Connectivity uses positive/negative X and positive/negative Z only. Diagonal
movement does not satisfy access.

## Candidate policy

A candidate is rejected when:

- any proposed cell enters the reserved entrance;
- a blocked cell lies outside logical bounds;
- fewer than two adjacent entrance cells remain open;
- a required anchor cell is blocked;
- a required anchor cannot be reached from an open entrance cell.

Candidate evaluation does not mutate committed occupancy.

## Consequences

The access foundation is deterministic, engine-independent and reusable by the
Store placement runtime.
