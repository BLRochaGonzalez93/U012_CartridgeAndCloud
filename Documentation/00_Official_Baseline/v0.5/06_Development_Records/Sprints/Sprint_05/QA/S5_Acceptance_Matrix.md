# Sprint 05 — Acceptance Matrix

| ID | SubSprint | Criterion | Evidence | Status |
|---|---|---|---|---|
| AC-S5-01 | S5.1 | Store shell measures 10 x 15 metres | Manual/validator | PASS |
| AC-S5-02 | S5.1 | Central entrance measures 2 metres | Manual/validator | PASS |
| AC-S5-03 | S5.1 | Technical player movement works in Store | Manual/PlayMode | PASS |
| AC-S5-04 | S5.1 | Orbit and zoom work in Store | Manual/PlayMode | PASS |
| AC-S5-05 | S5.1 | Normal and direct Store return flow works | Smoke/manual | PASS |
| AC-S5-06 | S5.2 | Logical Store layout is 20 x 30 cells | EditMode | PASS |
| AC-S5-07 | S5.2 | Entrance reserve contains four cells | EditMode | PASS |
| AC-S5-08 | S5.2 | Two adjacent entrance cells must remain open | EditMode | PASS |
| AC-S5-09 | S5.2 | Required anchors are validated | EditMode | PASS |
| AC-S5-10 | S5.2 | Four-direction connectivity is deterministic | EditMode | PASS |
| AC-S5-11 | S5.2 | Route-blocking candidates reject without mutation | EditMode | PASS |
| AC-S5-12 | S5.2 | Rotation changes access outcome deterministically | EditMode | PASS |
| AC-S5-13 | S5.3 | Store construction uses existing Input Actions | Input/manual | PASS |
| AC-S5-14 | S5.3 | Store preview snaps to 0.5-metre cells | PlayMode/manual | PASS |
| AC-S5-15 | S5.3 | Bounds and overlap remain atomic | EditMode/PlayMode | PASS |
| AC-S5-16 | S5.3 | Entrance-reserve placement is rejected | EditMode/PlayMode | PASS |
| AC-S5-17 | S5.3 | Required-route blockage is rejected | EditMode/PlayMode | PASS |
| AC-S5-18 | S5.3 | Valid placement confirms and creates a view | PlayMode/manual | PASS |
| AC-S5-19 | S5.3 | Escape cancellation works | PlayMode/manual | PASS |
| AC-S5-20 | S5.3 | Delete/Backspace removal frees occupancy | PlayMode/manual | PASS |
| AC-S5-21 | S5.3 | Movement is suppressed only during construction | Regression/manual | PASS |
| AC-S5-22 | S5.3 | Store maintains six approved roots | Validator/smoke | PASS |
| AC-S5-23 | S5.4 | Application version is 0.0.6 | EditMode/build | PASS |
| AC-S5-24 | S5.4 | Complete automated regression passes | 168/168 | PASS |
| AC-S5-25 | S5.4 | Final Store and TestLab regression passes | Manual | PASS |
| AC-S5-26 | S5.4 | Bootstrap/MainMenu/Store regression passes | Manual | PASS |
| AC-S5-27 | S5.4 | Windows x64 build and external execution pass | Manual | PASS |
| AC-S5-28 | S5.4 | Final technical commit is published to main | GitHub | PASS |

## Final result

**28/28 acceptance criteria passed.**
