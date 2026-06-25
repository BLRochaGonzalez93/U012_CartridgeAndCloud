# ADR-0018 — Atomic Occupancy and Placement Mode

**Status:** Proposed  
**Sprint:** 4  
**SubSprint:** CC_S4.3

## Proposed decision

- Logical occupancy is owned by `PlacementOccupancyRegistry`.
- Validation completes before any cell is mutated.
- A placement is rejected if it is outside bounds, overlaps an occupied
  cell or reuses an existing placement instance ID.
- Removal frees every occupied cell before the runtime view is destroyed.
- Confirmed objects use stable runtime IDs:
  `technical-shelf-0001`, `technical-shelf-0002`, and so on.
- `B` toggles placement mode.
- Left click confirms only while placement mode is active.
- `Escape` cancels placement mode.
- `Delete` removes a pointed confirmed object.
- Q/E retain 90-degree rotation.
- Click-to-move is suppressed only while placement mode is active.
- Orbit and zoom remain active during placement mode.

## Rationale

Explicit mode ownership prevents one click from both moving the player and
placing an object. Atomic registry mutation prevents partial occupancy and
makes overlap/removal behaviour deterministic and independently testable.

## Deferred

Door/access validation, interaction-point validation, NavMesh updates,
construction economy, refunds, persistence and final furniture content
remain deferred.
