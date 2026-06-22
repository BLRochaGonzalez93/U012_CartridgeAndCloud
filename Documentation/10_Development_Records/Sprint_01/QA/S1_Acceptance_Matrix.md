# Sprint 01 — Acceptance Matrix

| ID | Criterion | Evidence | Status |
|---|---|---|---|
| AC-S1-01 | Player starts through Bootstrap index 0 | Scene list + external run | PASS |
| AC-S1-02 | Bootstrap opens MainMenu automatically | PlayMode/manual | PASS |
| AC-S1-03 | Exactly one ApplicationRoot persists | PlayMode | PASS |
| AC-S1-04 | MainMenu enters Store | PlayMode/manual | PASS |
| AC-S1-05 | Store returns to MainMenu | PlayMode/manual | PASS |
| AC-S1-06 | Concurrent transitions are rejected | EditMode/PlayMode | PASS |
| AC-S1-07 | TestLab remains loadable and outside user flow | PlayMode | PASS |
| AC-S1-08 | Scene indexes remain 0–3 | EditMode | PASS |
| AC-S1-09 | No new asmdef or circular dependency | EditMode/inspection | PASS |
| AC-S1-10 | Movement, camera gameplay, NavMesh and save remain out of scope | Code review | PASS |
| AC-S1-11 | Automatic suite passes in final implementation state | 6 EditMode + 8 PlayMode | PASS |
| AC-S1-12 | Windows Player completes the round trip | External build run | PASS |
| AC-S1-13 | Player diagnostics contain no known blocking failure | Successful external run; log review not triggered under ADR-0010 | NOT REQUIRED |
| AC-S1-14 | No open S0/S1 defect | Closure review | PASS |
| AC-S1-15 | QA, traceability, ADR and records updated | Sprint 01 closure package | PASS |
| AC-S1-16 | No build, cache or local log is versioned | Repository review | PASS |

**Overall result:** PASS.
