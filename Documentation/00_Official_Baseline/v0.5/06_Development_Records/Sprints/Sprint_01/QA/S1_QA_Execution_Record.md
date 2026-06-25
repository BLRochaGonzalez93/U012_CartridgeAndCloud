# Sprint 01 — QA Execution Record

**Date:** 2026-06-23  
**Result:** PASS

## Automated execution

| Suite | Passed | Failed | Result |
|---|---:|---:|---|
| EditMode | 6 | 0 | PASS |
| PlayMode | 8 | 0 | PASS |
| Total | 14 | 0 | PASS |

Coverage includes project structure regression, approved scene order, Bootstrap initialization, persistent application root, MainMenu and Store transitions, TestLab availability and concurrent-transition rejection.

## Build execution

| Check | Result |
|---|---|
| Windows x64 development build | PASS |
| Player launch | PASS |
| Bootstrap to MainMenu | PASS |
| MainMenu to Store | PASS |
| Store to MainMenu | PASS |
| Quit | PASS |
| Crash or blocking exception | None observed |
| Player.log review | Not required under ADR-0010 |

## Incident history

- Input System installer failure: resolved before final scene generation.
- Initial Player build namespace collision: resolved by qualifying `UnityEngine.Application.Quit()`.
- Final build and external execution: PASS.

No defect is carried into the next sprint.
