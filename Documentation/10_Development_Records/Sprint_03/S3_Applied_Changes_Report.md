# Sprint 03 — Applied Changes Report

**Date:** 2026-06-25  
**State:** Implemented and validated  
**Result:** PASS  
**Technical commit:** `fe3f83ce9d52ded6944f4b57b3b0b6724ceeb7a6`

## CC_S3.1 — Input Context Foundation

- Added InputContextId values None, UI and Gameplay.
- Added pure InputContextService.
- Added IInputContextConsumer.
- Extended ApplicationRoot with scene-driven context ownership.
- Added input-context EditMode and PlayMode coverage.

## CC_S3.2 — Click-to-Move

- Added pure planar movement calculation.
- Added CharacterController-backed ClickToMoveAgent.
- Added screen-to-world destination selection.
- Added isolated Input System adapter boundary.
- Added TestLab movement laboratory.
- Preserved the approved production assembly graph and scene roots.

## CC_S3.3 — Orbit & Zoom Camera

- Added pure orbit constraints and state calculation.
- Added OrbitCameraRig.
- Added player-owned CameraTarget.
- Added right-drag orbit, wheel zoom and follow behaviour.
- Established pitch limits 25–75 degrees and distance limits 6–24.
- Set accepted zoom sensitivity to `0.5`.

## CC_S3.4 — Input Integration & Closure

- Added deterministic UI and Gameplay action maps.
- Added persistent InputActionContextRouter.
- Added GameplayInputActionDriver.
- Removed temporary polling drivers from active TestLab configuration.
- Added Input System EditMode and PlayMode coverage.
- Advanced application version to `0.0.4`.
- Completed full regression, Windows build and external execution.

## Resolved incidents

Four package-integration incidents were resolved before final validation:

1. Presentation.Camera namespace collision with UnityEngine.Camera.
2. Project Application namespace collision with UnityEngine.Application.
3. Missing Unity.InputSystem reference in the new EditMode test assembly.
4. Missing UNITY_INCLUDE_TESTS constraint in the new PlayMode test assembly.

## Validation

- EditMode: `43/43 PASS`.
- PlayMode: `23/23 PASS`.
- Full suite: `66/66 PASS`.
- Context and scene-flow regression: PASS.
- Movement and camera regression: PASS.
- Windows x64 build: PASS.
- External execution and Quit: PASS.

## Deferred scope

Final art, animation, NavMesh pathfinding, camera collision, input rebinding UI,
gamepad gameplay expansion and player/camera state persistence remain deferred.
