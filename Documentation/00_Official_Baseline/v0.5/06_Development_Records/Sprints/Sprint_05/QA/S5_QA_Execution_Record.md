# Sprint 05 — QA Execution Record

**Date:** `2026-06-25`  
**Result:** PASS  
**Application version:** `0.0.6`  
**Technical commit:** `22d62967aaf9895db7bce75afd2ffa11f7858e0c`

## Final automated execution

| Suite | Result |
|---|---|
| EditMode | `127/127 PASS` |
| PlayMode | `41/41 PASS` |
| Full suite | `168/168 PASS` |

## Progressive regression totals

| Milestone | EditMode | PlayMode | Total |
|---|---:|---:|---:|
| Sprint 4 baseline | 87 | 31 | 118 |
| CC_S5.1 | 95 | 34 | 129 |
| CC_S5.2 | 119 | 34 | 153 |
| CC_S5.3 | 127 | 41 | 168 |
| CC_S5.4 final | 127 | 41 | 168 |

## Coverage added by Sprint 5

- Store shell specification and descriptor.
- Store player, camera and scene wiring.
- Access-anchor IDs and layout invariants.
- Four-direction reachability.
- Entrance-width and reservation policy.
- Required-anchor reachability.
- Candidate footprint access validation.
- Occupancy snapshots.
- Optional Store access integration in placement runtime.
- Store placement runtime persistence after scene reload.
- Placement and movement conflict policy in Store.

## Final manual execution

| Check | Result |
|---|---|
| Store empty closure baseline | PASS |
| Store dimensions and entrance | PASS |
| Click-to-move and collision | PASS |
| Orbit and zoom | PASS |
| B construction activation | PASS |
| 0.5-metre snapping | PASS |
| Q/E rotation | PASS |
| Bounds and overlap feedback | PASS |
| Entrance reservation feedback | PASS |
| Required-route feedback | PASS |
| Confirmation and cancellation | PASS |
| Delete and Backspace removal | PASS |
| Occupancy and route restoration | PASS |
| Movement conflict handling | PASS |
| TestLab regression | PASS |
| Bootstrap/MainMenu/Store flow | PASS |
| Direct Store execution and return | PASS |
| Exclusive UI/Gameplay contexts | PASS |
| Windows x64 build | PASS |
| External Player execution | PASS |
| External Quit | PASS |

## Integration incidents

| Incident | Resolution | State |
|---|---|---|
| Invalid helper reference in initial S5.1 package | Corrected helper usage | Closed |
| Store smoke root baseline expected five roots | Updated accepted six-root baseline | Closed |
| Direct Store execution lacked injected navigator | Added Bootstrap fallback | Closed |
| Editor installer risked direct Domain dependency | Removed direct dependency before delivery | Prevented |
| Store access layout risked loss after scene reload | Added serialized activation and reconstruction | Prevented |

No incident remains in the accepted implementation.

## Final decision

**QA PASS. No open Sprint 5 defect is carried forward.**
