# ADR-0016 — Grid Coordinate and Footprint Foundation

**Status:** Accepted  
**Sprint:** 4  
**SubSprint:** CC_S4.1  
**Accepted date:** 2026-06-25

## Decision

The construction grid uses:

- `GridCoordinate` for integer X/Z cells.
- `GridSize` for positive integer width/depth.
- `GridRotation` for quarter turns.
- `GridFootprint` for oriented rectangular occupied cells.
- `GridBounds` for logical area containment.
- `GridProjectionCalculator` for deterministic world/grid conversion.

The footprint anchor represents the minimum occupied X/Z cell after
orientation. Cell regions are half-open in world space. With the default
0.5-metre cell size, cell `(0, 0)` covers `[0, 0.5)` on both axes and its
centre is `(0.25, 0.25)`.

## Rationale

This model avoids Unity dependencies in Domain and Application, supports
negative coordinates, makes rotations deterministic and separates
occupancy from scene representation.

## Validation

The decision is accepted by the CC_S4.1 automated suite and remained
green through the final `118/118` Sprint 4 regression.

## Deferred

Access validation, NavMesh updates, economy and persistence remain deferred.
