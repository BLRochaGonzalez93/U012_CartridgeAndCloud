# ADR-0014 — Orbit, Zoom and Follow Camera

**Status:** Accepted  
**Sprint:** 3  
**SubSprint:** CC_S3.3  
**Accepted:** 2026-06-25  
**Technical commits:** `a4e56e7b16f16c385a350cf3078f2e70f280a45f`, `fe3f83ce9d52ded6944f4b57b3b0b6724ceeb7a6`

## Decision

Sprint 3 uses:

- Pure orbit-state constraints and calculations in Application.
- `OrbitCameraRig` in Presentation.
- A CameraTarget below the technical player.
- Right-button drag orbit.
- Mouse-wheel zoom.
- Pitch limits of 25–75 degrees.
- Distance limits of 6–24 units.
- Automatic follow through the player-owned target transform.
- Zoom sensitivity `0.5`.

## Architectural boundaries

- Presentation does not reference Unity.InputSystem.
- Input is routed through Infrastructure.InputSystem.
- No Cinemachine dependency is introduced.
- No new TestLab root object is introduced.
- Store remains unchanged.

## Validation

- Orbit state normalization and constraint tests passed.
- Camera rig pose, follow, distance and aiming tests passed.
- Manual orbit, pitch, zoom and follow regression passed.
- Final Input Action routing preserved all camera behaviour.
- Full automated suite completed `66/66 PASS`.

## Deferred

Camera collision, obstruction handling, smoothing polish, edge scrolling,
keyboard camera movement and saved camera state remain deferred.
