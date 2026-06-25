# ADR-0019 — Sprint 04 Final Integration and Build Gate

**Status:** Accepted  
**Sprint:** 4  
**SubSprint:** CC_S4.4  
**Accepted date:** 2026-06-25

## Decision

Sprint 4 closes through an evidence-gated final integration rather than
additional gameplay scope.

The final integration:

- Resets TestLab to an empty and reproducible placement baseline.
- Keeps Main Camera and Directional Light as the only TestLab roots.
- Verifies the 16 x 16 logical surface at 0.5 metres per cell.
- Verifies the technical 4 x 2 placeable definition.
- Verifies placement and gameplay drivers share one runtime controller.
- Verifies Bootstrap, MainMenu and Store are enabled build scenes.
- Sets `PlayerSettings.bundleVersion` to `0.0.5`.
- Requires `118/118` automated regression.
- Requires Windows x64 build and external execution evidence.

## Rationale

CC_S4.1 through CC_S4.3 already implemented the complete placement
foundation target. CC_S4.4 protects the integrated baseline and produces
build-oriented evidence without adding gameplay risk.

## Validation

The validator, automated suite, manual regression, Windows build and
external Player execution all passed.

## Deferred

Store construction integration, access validation, NavMesh updates,
economy, persistence and final placeable content remain outside Sprint 4.
