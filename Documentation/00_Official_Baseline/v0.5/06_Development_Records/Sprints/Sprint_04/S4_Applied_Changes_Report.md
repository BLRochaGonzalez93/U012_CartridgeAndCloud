# Sprint 04 — Applied Changes Report

**Date:** 2026-06-25  
**State:** Implemented and validated  
**Result:** PASS  
**Technical commit:** `bcf8cdd309d89570b5b58a0735154b4863e2aebc`

## CC_S4.1 — Grid Coordinate Foundation

- Added deterministic integer grid coordinates.
- Added positive integer grid sizes.
- Added four quarter-turn rotations.
- Added rectangular footprints and logical bounds.
- Added deterministic world/grid projection.
- Preserved Domain/Application independence from UnityEngine.
- Added 18 EditMode tests.

## CC_S4.2 — Placement Preview & Rotation

- Added technical placeable definitions.
- Added a 16 x 16 TestLab placement surface.
- Added a snapped ghost preview.
- Added Q/E quarter-turn controls.
- Added valid and invalid materials.
- Added bounds feedback.
- Added 12 tests.
- Resolved test type ambiguity and Editor Input System dependency.

## CC_S4.3 — Occupancy & Base Validation

- Added stable runtime placement IDs and records.
- Added atomic logical occupancy.
- Added bounds, overlap and duplicate-ID rejection.
- Added explicit B-controlled placement mode.
- Added confirmation, cancellation and removal.
- Added Delete and Backspace removal bindings.
- Added collider and logical-cell removal selection.
- Added click-to-move conflict handling.
- Added 22 tests.
- Resolved runtime type ambiguity and removal-flow defect.

## CC_S4.4 — Integration & Closure

- Added final integration and validator commands.
- Reset TestLab to an empty baseline.
- Verified build-scene inclusion.
- Advanced application version to `0.0.5`.
- Completed `118/118` automated regression.
- Completed final placement and scene-flow regression.
- Completed Windows x64 Development build.
- Completed external Player execution and Quit.
- Published final technical commit on `main`.

## Final validation

- EditMode: `87/87 PASS`.
- PlayMode: `31/31 PASS`.
- Full suite: `118/118 PASS`.
- Placement regression: PASS.
- Scene and context regression: PASS.
- Windows x64 build: PASS.
- External execution and Quit: PASS.

## Deferred scope

Store construction integration, access-route validation, interaction points,
NavMesh updates, economy, refunds, persistence and final placeable content
remain deferred.
