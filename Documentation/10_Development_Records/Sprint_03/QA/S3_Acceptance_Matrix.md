# Sprint 03 — Acceptance Matrix

| ID | SubSprint | Criterion | Evidence | Status |
|---|---|---|---|---|
| AC-S3-01 | S3.1 | Service starts in None | EditMode | PASS |
| AC-S3-02 | S3.1 | Context changes emit one notification | EditMode | PASS |
| AC-S3-03 | S3.1 | Reapplying active context is idempotent | EditMode | PASS |
| AC-S3-04 | S3.1 | Undefined contexts are rejected | EditMode | PASS |
| AC-S3-05 | S3.1 | MainMenu activates UI context | PlayMode | PASS |
| AC-S3-06 | S3.1 | Store activates Gameplay context | PlayMode/manual | PASS |
| AC-S3-07 | S3.2 | Planar movement stops at configured radius | EditMode/PlayMode | PASS |
| AC-S3-08 | S3.2 | Valid floor click assigns and replaces destination | PlayMode/manual | PASS |
| AC-S3-09 | S3.2 | Technical player reaches selected destination | PlayMode/manual | PASS |
| AC-S3-10 | S3.2 | UI context rejects click-to-move | PlayMode | PASS |
| AC-S3-11 | S3.3 | Orbit state normalizes and clamps correctly | EditMode | PASS |
| AC-S3-12 | S3.3 | Camera follows and remains aimed at target | PlayMode/manual | PASS |
| AC-S3-13 | S3.3 | Orbit and zoom remain inside limits | EditMode/PlayMode/manual | PASS |
| AC-S3-14 | S3.4 | UI and Gameplay action maps are mutually exclusive | EditMode/PlayMode | PASS |
| AC-S3-15 | S3.4 | Active gameplay flow uses Input Actions | Inspection/PlayMode | PASS |
| AC-S3-16 | S3.4 | Application version is 0.0.4 | EditMode/build | PASS |
| AC-S3-17 | S3.4 | Complete automated regression passes | 66/66 | PASS |
| AC-S3-18 | S3.4 | Windows build and external run pass | Manual | PASS |

## Final result

**18/18 acceptance criteria passed.**
