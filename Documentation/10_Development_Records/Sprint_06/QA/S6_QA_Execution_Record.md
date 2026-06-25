# Sprint 06 — QA Execution Record

**Execution date:** 2026-06-25  
**Unity version:** `6000.3.18f1`  
**Application version:** `0.0.7`  
**Evidence source:** developer execution in Unity Editor and external Windows Player

## Compilation

| Check | Result | Evidence |
|---|---|---|
| Script compilation | PASS | Unity Editor compilation completed without blocking errors |
| Product/Inventory focused compilation | PASS | Focused EditMode suite executed |
| New functional warnings | NONE REPORTED | Developer validation |

## Automated tests

| Suite | Expected | Actual | Result |
|---|---:|---:|---|
| Sprint 6 focused EditMode | 60 | 60 | PASS |
| Full EditMode at first Sprint 6 run | 187 | 186 pass / 1 known assertion mismatch | ACCEPTED WITH KNOWN TEST-MAINTENANCE ISSUE |
| Full PlayMode | 41 | 41 | PASS |
| Current full regression after Sprint 7 correction | 300 | 300 | PASS |

### Known assertion mismatch

`ProjectVersion_IsSprintFiveTarget` expected `0.0.6` while Sprint 6 correctly
used `0.0.7`. The mismatch was classified as a stale inherited test, not a
runtime or domain defect. The test was replaced in Sprint 7 and now validates
the current target version.

## Manual regression

| Area | Result | Notes |
|---|---|---|
| Bootstrap and MainMenu | PASS | Existing scene flow preserved |
| Store normal/direct flow | PASS | Existing flows preserved |
| Movement and camera | PASS | No regression reported |
| Placement and removal | PASS | No regression reported |
| Access preservation | PASS | No regression reported |
| TestLab | PASS | No regression reported |

## Version and build

| Check | Result | Evidence |
|---|---|---|
| Application version `0.0.7` | PASS | Player Settings and build validation |
| Windows x64 Development build | PASS | Developer-generated build |
| External execution | PASS | Executable launched and smoke-tested |

## Defects

| Severity | Open defects |
|---|---:|
| S0 | 0 |
| S1 | 0 |

## Final QA decision

**PASS.** Sprint 6 is accepted. The obsolete version assertion was tracked and
resolved as part of Sprint 7 compatibility work.
