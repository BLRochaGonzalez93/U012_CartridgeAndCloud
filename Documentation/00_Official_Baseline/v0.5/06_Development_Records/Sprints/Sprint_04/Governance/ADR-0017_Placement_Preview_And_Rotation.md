# ADR-0017 — Placement Preview and Rotation

**Status:** Accepted  
**Sprint:** 4  
**SubSprint:** CC_S4.2  
**Accepted date:** 2026-06-25

## Decision

- Technical placeables use ScriptableObject definitions.
- Presentation owns surface projection, preview state and ghost transforms.
- Infrastructure.InputSystem owns pointer and placement-action polling.
- Placement uses the existing Gameplay Input Action map.
- Q rotates counter-clockwise and E rotates clockwise.
- Rotation occurs in quarter turns.
- TestLab uses a 4 x 2 technical shelf.
- The logical preview area is 16 x 16 cells at 0.5 metres per cell.
- Green indicates a valid preview.
- Red indicates bounds or occupancy rejection.

## Rationale

Separating preview from occupancy allowed visual/input behaviour to be
tested independently before mutable construction state was introduced.

## Validation

Snapping, rotation, bounds feedback and input-context coexistence passed
automated and manual validation and remained green through final regression.

## Deferred

Store construction integration, NavMesh updates, economy and persistence
remain deferred.
