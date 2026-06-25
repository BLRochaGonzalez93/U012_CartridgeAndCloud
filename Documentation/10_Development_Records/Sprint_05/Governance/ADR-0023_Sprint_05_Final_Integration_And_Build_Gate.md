# ADR-0023 — Sprint 05 Final Integration and Build Gate

**Status:** Proposed  
**Sprint:** 5  
**SubSprint:** CC_S5.4

## Decision

Sprint 5 closes through an evidence-gated integration rather than by adding
new gameplay scope.

The final integration:

- Resets Store to an empty and reproducible placement baseline.
- Keeps construction mode inactive and the ghost hidden.
- Preserves the six approved Store scene roots.
- Verifies the 10 x 15 metre Store shell.
- Verifies the 20 x 30 placement surface at 0.5 metres per cell.
- Verifies the technical 4 x 2 placeable.
- Verifies placement, movement and camera input wiring.
- Verifies reserved entrance and required-anchor validation is enabled.
- Verifies Bootstrap, MainMenu and Store are enabled build scenes.
- Requires Bootstrap to be the first enabled build scene.
- Sets `PlayerSettings.bundleVersion` to `0.0.6`.
- Requires the existing `168/168` automated suite to remain green.
- Requires Windows x64 Development build and external execution evidence.

## Rationale

CC_S5.1 through CC_S5.3 already deliver and validate the Store shell,
access foundation and placement integration. CC_S5.4 therefore protects
the integrated baseline and produces release-oriented evidence without
introducing another gameplay feature.

## Deferred

- Placement persistence.
- Economy, purchase and refund flows.
- Furniture catalogue.
- Products and inventory.
- Customers and employees.
- Checkout gameplay.
- Dynamic NavMesh updates.
- Final Store art and expansion.
