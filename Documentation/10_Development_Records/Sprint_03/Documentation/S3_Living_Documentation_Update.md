# Sprint 03 — Living Documentation Update

## Input architecture

The persistent ApplicationRoot now owns scene-driven input context state.
Input contexts are None, UI and Gameplay.

The final Input Action collection contains:

- UI: Point, Click, Submit and Cancel.
- Gameplay: PointerPosition, SetDestination, OrbitDelta, OrbitHold and Zoom.

Only the map matching the active context is enabled.

## Player movement

The technical player uses:

- CharacterController locomotion.
- Direct planar destination movement.
- Configurable speed, rotation and stopping tolerance.
- Pointer raycasting against the approved walkable surface.
- No NavMesh or obstacle avoidance.

## Camera

The technical camera uses:

- A CameraTarget owned by the player.
- OrbitCameraRig in Presentation.
- Pitch limits of 25–75 degrees.
- Distance limits of 6–24 units.
- Right-button drag orbit.
- Mouse-wheel zoom with sensitivity 0.5.
- Automatic target follow.

## Scene and assembly boundaries

- TestLab retains Main Camera and Directional Light as its only roots.
- Technical movement objects remain nested below Directional Light.
- Presentation retains the approved production dependencies.
- Unity Input System access remains isolated in Infrastructure.InputSystem.
- Store remains unchanged by the technical TestLab implementation.

## Version and testing

- Application version: `0.0.4`.
- Automated suite: `66/66 PASS`.
- Windows x64 build and external execution: PASS.

## Deferred capabilities

- Final player art and animation.
- NavMesh pathfinding and obstacle avoidance.
- Camera collision and obstruction handling.
- Rebinding UI and persisted binding overrides.
- Expanded gamepad gameplay support.
- Saved player and camera transforms.

## Baseline policy

Official baseline v0.4 remains immutable.
The next official baseline remains deferred to final phase closure unless a
material governance exception is approved.
