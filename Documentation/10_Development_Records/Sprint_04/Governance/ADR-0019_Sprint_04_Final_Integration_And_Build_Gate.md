# ADR-0019 — Sprint 04 Final Integration and Build Gate

**Status:** Proposed  
**Sprint:** 4  
**SubSprint:** CC_S4.4

## Decision

Sprint 4 closes through an evidence-gated final integration rather than
additional gameplay scope.

The final integration:

- Resets TestLab to an empty and reproducible placement baseline.
- Keeps the approved Main Camera and Directional Light root baseline.
- Verifies the 16 x 16 logical surface at 0.5 metres per cell.
- Verifies the technical 4 x 2 placeable definition.
- Verifies placement and gameplay input drivers share one runtime controller.
- Verifies Bootstrap, MainMenu and Store are enabled build scenes.
- Sets `PlayerSettings.bundleVersion` to `0.0.5`.
- Requires the existing `118/118` automated suite to remain green.
- Requires Windows x64 build and external execution evidence.

## Rationale

CC_S4.1 through CC_S4.3 already implement and test the complete placement
foundation target. CC_S4.4 therefore protects the integrated baseline and
produces release-oriented evidence without introducing additional risk.

## Deferred

- Store construction integration.
- Door and mandatory access-route validation.
- Interaction-point validation.
- NavMesh updates after placement.
- Economy and refunds.
- Persistence of placed objects.
- Final furniture content.

These items remain outside Sprint 4.
