# ADR-0020 — Initial Store Shell Dimensions and Entrance

**Status:** Proposed  
**Sprint:** 5  
**SubSprint:** CC_S5.1

## Proposed decision

The initial technical Store shell uses:

- Interior width: `10 m`.
- Interior depth: `15 m`.
- Future grid size: `20 x 30` cells.
- Cell size: `0.5 m`.
- Wall height: `3 m`.
- Wall thickness: `0.2 m`.
- Central entrance width: `2 m` / `4 cells`.
- Two front-wall segments of `4 m` each.
- Player spawn at the entrance interior.
- Technical primitives and materials only.

The existing Store roots remain intact. `S5_StoreShell` is added as the
only new scene root. The old full-screen background, title and scope notice
are disabled, while ReturnButton remains active.

## Rationale

The 10 x 15 metre footprint preserves the accepted visual direction while
giving enough longitudinal space to test entrances, aisles and future
placement without prematurely implementing final content.

A 2 metre entrance supports four future grid cells and provides a stable
basis for CC_S5.2 access validation.

## Deferred

Logical access anchors, construction integration, NavMesh updates, economy,
persistence and final art are deferred.
