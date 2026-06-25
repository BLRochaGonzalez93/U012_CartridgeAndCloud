# Sprint 04 — Acceptance Matrix

| ID | SubSprint | Criterion | Evidence | Status |
|---|---|---|---|---|
| AC-S4-01 | S4.1 | Grid coordinate equality and hashing are deterministic | EditMode | PASS |
| AC-S4-02 | S4.1 | Cell size is positive and defaults to 0.5 metres | EditMode | PASS |
| AC-S4-03 | S4.1 | World-to-cell projection supports negative coordinates | EditMode | PASS |
| AC-S4-04 | S4.1 | 90/270 degree rotation swaps rectangular footprint axes | EditMode | PASS |
| AC-S4-05 | S4.1 | Logical bounds use half-open regions | EditMode | PASS |
| AC-S4-06 | S4.2 | Preview follows the pointer on the placement surface | PlayMode/manual | PASS |
| AC-S4-07 | S4.2 | Preview snaps to 0.5-metre grid cells | PlayMode/manual | PASS |
| AC-S4-08 | S4.2 | Q/E rotate in quarter turns | Input/PlayMode/manual | PASS |
| AC-S4-09 | S4.2 | Preview displays valid and invalid feedback | PlayMode/manual | PASS |
| AC-S4-10 | S4.2 | Click-to-move, orbit and zoom remain green | Regression | PASS |
| AC-S4-11 | S4.3 | Placement mode has explicit activation and cancellation | Input/manual | PASS |
| AC-S4-12 | S4.3 | Valid confirmation occupies every footprint cell atomically | EditMode/PlayMode | PASS |
| AC-S4-13 | S4.3 | Out-of-bounds placement is rejected | EditMode/PlayMode | PASS |
| AC-S4-14 | S4.3 | Overlap is rejected without partial mutation | EditMode/PlayMode | PASS |
| AC-S4-15 | S4.3 | Runtime placement IDs are stable and unique | EditMode | PASS |
| AC-S4-16 | S4.3 | Delete/Backspace removal frees every occupied cell | PlayMode/manual | PASS |
| AC-S4-17 | S4.3 | Movement input is suppressed only during placement mode | EditMode/manual | PASS |
| AC-S4-18 | S4.4 | Application version is 0.0.5 | EditMode/build | PASS |
| AC-S4-19 | S4.4 | Complete automated regression passes | 118/118 | PASS |
| AC-S4-20 | S4.4 | Final TestLab placement regression passes | Manual | PASS |
| AC-S4-21 | S4.4 | Bootstrap/MainMenu/Store regression passes | Manual | PASS |
| AC-S4-22 | S4.4 | Windows x64 build and external execution pass | Manual | PASS |
| AC-S4-23 | S4.4 | Final technical commit is published to main | GitHub | PASS |

## Final result

**23/23 acceptance criteria passed.**
