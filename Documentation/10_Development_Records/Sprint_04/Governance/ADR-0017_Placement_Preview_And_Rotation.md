# ADR-0017 — Placement Preview and Rotation

**Status:** Proposed  
**Sprint:** 4  
**SubSprint:** CC_S4.2

## Proposed decision

- Technical placeables use ScriptableObject definitions for footprint and
  preview dimensions.
- Presentation owns surface projection, preview state presentation and
  ghost transforms.
- Infrastructure.InputSystem owns pointer and Q/E action polling.
- The preview uses the existing Gameplay Input Action map.
- Q rotates counter-clockwise and E rotates clockwise.
- Rotation occurs in quarter turns.
- The TestLab preview uses a 4x2 technical shelf.
- The logical preview area is 16x16 cells at 0.5 metres per cell.
- Green indicates the footprint is inside logical bounds.
- Red indicates the footprint crosses logical bounds.
- No cell is occupied and no object is confirmed in CC_S4.2.

## Rationale

Keeping preview and rotation separate from occupancy makes the visual/input
pipeline independently testable before mutable construction state is added.

## Deferred

Confirm, cancel, removal, overlap rejection, stable placement IDs, access
validation, NavMesh updates, economy and persistence remain deferred.
