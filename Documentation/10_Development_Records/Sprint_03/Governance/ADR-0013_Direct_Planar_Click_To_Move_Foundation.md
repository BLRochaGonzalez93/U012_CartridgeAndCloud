# ADR-0013 — Direct Planar Click-to-Move Foundation

**Status:** Accepted  
**Sprint:** 3  
**SubSprint:** CC_S3.2  
**Accepted:** 2026-06-25  
**Technical commits:** `f5a03fb986c241b3062f989524cf0b5737c0b32a`, `fe3f83ce9d52ded6944f4b57b3b0b6724ceeb7a6`

## Decision

CC_S3.2 uses:

- A CharacterController-backed technical player.
- A pure planar movement calculator in Application.
- A Presentation agent responsible for Unity movement and rotation.
- A Presentation destination selector responsible for raycasting and context checks.
- An isolated Infrastructure Input System layer.
- A TestLab configuration nested below an approved existing root.

## Architectural boundaries

- Presentation retains only Domain and Application production references.
- Input System access remains isolated in the dedicated adapter assembly.
- TestLab keeps the approved roots: Main Camera and Directional Light.
- No NavMesh pathfinding or obstacle avoidance is introduced.
- Store remains unchanged.

## Validation

- Planar movement calculation tests passed.
- Click-to-move PlayMode tests passed.
- Manual destination assignment, replacement, rotation and stopping passed.
- UI context rejected gameplay movement input.
- Final action-driven movement regression passed.
- Full automated suite completed `66/66 PASS`.

## Deferred

NavMesh pathfinding, obstacle avoidance, final player art, animation and
player-transform save persistence remain deferred.
