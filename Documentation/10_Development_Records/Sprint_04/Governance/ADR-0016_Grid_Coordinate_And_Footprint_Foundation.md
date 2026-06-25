# ADR-0016 — Grid Coordinate and Footprint Foundation

**Status:** Proposed  
**Sprint:** 4  
**SubSprint:** CC_S4.1

## Proposed decision

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
negative coordinates, makes rotations deterministic and separates occupancy
from scene representation.

## Deferred

Ghost rendering, input, placement confirmation, occupancy mutation, access
validation, NavMesh updates, economy and persistence are deferred.
