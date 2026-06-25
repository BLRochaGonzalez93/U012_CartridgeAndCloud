# ADR-0018 — Atomic Occupancy and Placement Mode

**Status:** Accepted  
**Sprint:** 4  
**SubSprint:** CC_S4.3  
**Accepted date:** 2026-06-25

## Decision

- Logical occupancy is owned by `PlacementOccupancyRegistry`.
- Validation completes before any cell is mutated.
- Out-of-bounds, overlapping and duplicate-ID placements are rejected.
- Removal frees every occupied cell before destroying the runtime view.
- Confirmed objects use stable sequential runtime identifiers.
- B toggles placement mode.
- Left click confirms only while placement mode is active.
- Escape cancels placement mode.
- Delete and Backspace remove a pointed confirmed object.
- Q/E retain 90-degree rotation.
- Click-to-move is suppressed only while placement mode is active.
- Orbit and zoom remain active during placement mode.
- Removal first checks colliders and falls back to the occupied grid cell.

## Rationale

Explicit mode ownership prevents one click from both moving the player and
placing an object. Atomic registry mutation prevents partial occupancy and
makes overlap and removal deterministic.

## Validation

Confirmation, overlap rejection, duplicate-ID rejection, removal and cell
release passed `22/22` new tests, final manual validation and the complete
`118/118` regression.

## Deferred

Door/access validation, interaction-point validation, NavMesh updates,
economy, refunds, persistence and final furniture content remain deferred.
