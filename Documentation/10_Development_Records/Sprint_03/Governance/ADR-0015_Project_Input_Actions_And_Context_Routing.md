# ADR-0015 — Project Input Actions and Context Routing

**Status:** Proposed  
**Sprint:** 3  
**SubSprint:** CC_S3.4

## Decision

The project owns one deterministic Input Action collection containing:

### UI map
- Point
- Click
- Submit
- Cancel

### Gameplay map
- PointerPosition
- SetDestination
- OrbitDelta
- OrbitHold
- Zoom

`InputActionContextRouter` enables exactly one map according to the active
`IInputContextService` value. `None` disables both maps.

A runtime bootstrap attaches the router to the persistent ApplicationRoot.
Direct TestLab execution creates a local standalone Gameplay router only when
no persistent router exists.

## Integration

`GameplayInputActionDriver` is the only active TestLab input consumer. It routes:
- SetDestination to ClickDestinationInput.
- OrbitDelta + OrbitHold to OrbitCameraRig.
- Zoom to OrbitCameraRig.

The temporary polling components remain only as non-polling compatibility
adapters and are removed from TestLab by the CC_S3.4 installer.

## Rationale

This centralizes context ownership, prevents UI and Gameplay maps from being
active simultaneously and removes direct device polling from the active flow.

## Deferred

- Rebindable controls UI.
- Gamepad gameplay camera bindings.
- Saved binding overrides.
- Accessibility presets.
