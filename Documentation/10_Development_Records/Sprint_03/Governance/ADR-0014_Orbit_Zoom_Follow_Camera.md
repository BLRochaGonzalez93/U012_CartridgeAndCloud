# ADR-0014 — Orbit, Zoom and Follow Camera

**Status:** Proposed  
**Sprint:** 3  
**SubSprint:** CC_S3.3

## Decision

CC_S3.3 introduces:

- Pure orbit-state constraints and calculations in Application.
- An `OrbitCameraRig` in Presentation.
- A `CameraTarget` below the technical player.
- Right-mouse drag orbit.
- Mouse-wheel zoom.
- Pitch limits of 25–75 degrees.
- Distance limits of 6–24 units.
- Automatic follow through the player-owned target transform.

## Architectural boundaries

- Presentation does not reference `Unity.InputSystem`.
- Mouse reading remains isolated in the existing
  `Infrastructure.InputSystem` adapter assembly.
- No Cinemachine dependency is introduced.
- No new TestLab root object is introduced.
- Store remains unchanged.
- Final InputAction routing remains deferred to CC_S3.4.

## Rationale

A small deterministic rig is sufficient to validate movement-camera composition
before committing to final bindings, presentation polish or camera collision.

## Deferred

- Camera collision and obstruction handling.
- Edge scrolling.
- Keyboard camera movement.
- InputAction bindings.
- Saved camera state.
- Final smoothing and accessibility tuning.
