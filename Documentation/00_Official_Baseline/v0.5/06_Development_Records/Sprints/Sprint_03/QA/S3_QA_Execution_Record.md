# Sprint 03 — QA Execution Record

**Date:** 2026-06-25  
**Result:** PASS  
**Application version:** `0.0.4`  
**Technical commit:** `fe3f83ce9d52ded6944f4b57b3b0b6724ceeb7a6`

## Automated execution

| Suite | Result |
|---|---|
| EditMode | `43/43 PASS` |
| PlayMode | `23/23 PASS` |
| Full suite | `66/66 PASS` |

## Progressive regression totals

| Milestone | Total |
|---|---|
| Sprint 2 baseline | 29/29 |
| CC_S3.1 | 35/35 |
| CC_S3.2 | 46/46 |
| CC_S3.3 | 56/56 |
| CC_S3.4 final | 66/66 |

## Coverage added by Sprint 3

- Input context invariants and scene-driven switching.
- Direct planar movement calculation.
- Click destination raycasting and context rejection.
- CharacterController destination movement.
- Orbit state normalization and clamping.
- Camera pose, follow and zoom behaviour.
- Input Action map definitions.
- Exclusive UI/Gameplay action routing.
- Persistent router composition.
- Final action-driven movement-camera integration.

## Manual execution

| Check | Result |
|---|---|
| Bootstrap reaches MainMenu | PASS |
| MainMenu activates UI context | PASS |
| MainMenu opens Store | PASS |
| Store activates Gameplay context | PASS |
| Store returns to MainMenu | PASS |
| One ApplicationRoot persists | PASS |
| Click-to-move assigns and replaces destination | PASS |
| Player stopping and facing | PASS |
| Orbit and pitch limits | PASS |
| Zoom limits and sensitivity 0.5 | PASS |
| Camera follow | PASS |
| TestLab approved roots preserved | PASS |
| Windows x64 build | PASS |
| External Player launch | PASS |
| External navigation flow | PASS |
| External Quit | PASS |

## Integration incidents

| Incident | Resolution | State |
|---|---|---|
| `Camera` namespace/type collision after adding Presentation.Camera | Explicit `UnityEngine.Camera` alias in ClickDestinationInput | Closed |
| `Application.isPlaying` resolved to project Application namespace | Fully qualified `UnityEngine.Application.isPlaying` | Closed |
| EditMode Input System test assembly lacked Unity.InputSystem reference | Added direct asmdef reference | Closed |
| PlayMode test assembly entered Player build | Added `UNITY_INCLUDE_TESTS` define constraint | Closed |

All incidents occurred before final validation. None remains in the accepted result.

## Player.log disposition

Player.log review was not triggered because the external Player launched,
completed the required flow and exited without crash, unhandled exception,
blocking failure or Editor/Player discrepancy.

## Final decision

**QA PASS. No open Sprint 3 defect is carried forward.**
