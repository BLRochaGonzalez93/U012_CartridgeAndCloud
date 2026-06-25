# ADR-0015 — Project Input Actions and Context Routing

**Status:** Accepted  
**Sprint:** 3  
**SubSprint:** CC_S3.4  
**Accepted:** 2026-06-25  
**Technical commit:** `fe3f83ce9d52ded6944f4b57b3b0b6724ceeb7a6`

## Decision

The project owns one deterministic Input Action collection.

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
`IInputContextService` value. None disables both maps.

A runtime bootstrap attaches the router to the persistent ApplicationRoot.
Direct TestLab execution creates a local standalone Gameplay router only when
no persistent router exists.

`GameplayInputActionDriver` routes:

- SetDestination to ClickDestinationInput.
- OrbitDelta and OrbitHold to OrbitCameraRig.
- Zoom to OrbitCameraRig.

Temporary polling components remain only as non-polling compatibility adapters
and are removed from the active TestLab configuration.

## Rationale

Central action ownership prevents UI and Gameplay maps from being active
simultaneously and removes direct device polling from the validated flow.

## Validation

- ProjectInputActionsTests completed `7/7 PASS`.
- InputActionRuntimeIntegrationTests completed `3/3 PASS`.
- MainMenu activated UI only.
- Store and TestLab activated Gameplay only.
- Persistent router survived scene transitions.
- Full suite completed `66/66 PASS`.
- Windows x64 build and external execution passed.

## Deferred

Rebindable controls UI, saved overrides, expanded gamepad gameplay bindings and
accessibility presets remain deferred.
