# ADR-0013 — Direct Planar Click-to-Move Foundation

**Status:** Proposed  
**Sprint:** 3  
**SubSprint:** CC_S3.2

## Decision

CC_S3.2 uses:
- A CharacterController-backed technical player.
- A pure planar movement calculator in Application.
- A Presentation agent that owns Unity movement and rotation.
- A Presentation destination selector that owns raycasting and context checks.
- An isolated Infrastructure.InputSystem adapter for `Mouse.current`.
- A standalone TestLab configuration nested below an approved existing root.

## Architectural boundaries

- Presentation retains only Domain + Application references.
- `Unity.InputSystem` is referenced only by the dedicated adapter assembly.
- TestLab keeps exactly the approved roots: `Main Camera` and `Directional Light`.
- Technical movement objects live below `Directional Light`.
- No NavMesh pathfinding or obstacle avoidance is introduced.

## Deferred

- Final InputAction maps and bindings.
- Final player art or animation.
- Store scene integration.
- Save persistence for player transform.
- Camera orbit and zoom.

## Rationale

This isolates destination selection and locomotion feel before camera work and preserves the project's approved assembly and scene baselines.

CC_S3.4 will replace the temporary mouse driver with project InputActions while retaining the movement agent and planar movement rules.
