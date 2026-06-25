# Sprint 04 — QA Execution Record

**Date:** 2026-06-25  
**Result:** PASS  
**Application version:** `0.0.5`  
**Technical commit:** `bcf8cdd309d89570b5b58a0735154b4863e2aebc`

## Automated execution

| Suite | Result |
|---|---|
| EditMode | `87/87 PASS` |
| PlayMode | `31/31 PASS` |
| Full suite | `118/118 PASS` |

## Progressive regression totals

| Milestone | EditMode | PlayMode | Total |
|---|---:|---:|---:|
| Sprint 3 baseline | 43 | 23 | 66 |
| CC_S4.1 | 61 | 23 | 84 |
| CC_S4.2 | 70 | 26 | 96 |
| CC_S4.3 | 87 | 31 | 118 |
| CC_S4.4 final | 87 | 31 | 118 |

## Coverage added by Sprint 4

- Grid coordinate, size, rotation and footprint invariants.
- World/grid projection and logical bounds.
- Placement preview calculation.
- Technical placeable definition.
- Placement Input Actions.
- Snapped preview runtime behaviour.
- Stable placement identifiers.
- Atomic occupancy, overlap and removal.
- Placement-mode movement conflict policy.
- Runtime confirmation, cancellation and cell release.

## Manual execution

| Check | Result |
|---|---|
| TestLab empty closure baseline | PASS |
| B placement-mode activation | PASS |
| 0.5-metre snapping | PASS |
| Q/E quarter-turn rotation | PASS |
| Bounds feedback | PASS |
| Confirmation | PASS |
| Overlap rejection | PASS |
| Escape cancellation | PASS |
| Delete removal | PASS |
| Backspace removal | PASS |
| Cell release after removal | PASS |
| Click-to-move conflict handling | PASS |
| Orbit and zoom coexistence | PASS |
| Bootstrap/MainMenu/Store flow | PASS |
| Exclusive UI/Gameplay contexts | PASS |
| Windows x64 build | PASS |
| External Player launch | PASS |
| External navigation flow | PASS |
| External Quit | PASS |

## Integration incidents

| Incident | Resolution | State |
|---|---|---|
| Ambiguous `Object` in CC_S4.2 test | Qualified `UnityEngine.Object` | Closed |
| Editor assembly lacked `Unity.InputSystem` | Added direct asmdef reference | Closed |
| Ambiguous `Object` in CC_S4.3 runtime | Qualified `UnityEngine.Object` | Closed |
| Removal only processed in active placement mode | Reordered input flow and added logical-cell fallback | Closed |

All incidents occurred before final acceptance. None remains in the validated result.

## Player.log disposition

`Player.log` review was not triggered because the external Player launched,
completed the required flow and exited without crash, unhandled exception,
blocking failure or Editor/Player discrepancy.

## Final decision

**QA PASS. No open Sprint 4 defect is carried forward.**
