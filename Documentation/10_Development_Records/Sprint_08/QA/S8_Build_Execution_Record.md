# Sprint 08 — Build and External Execution Record

**Execution status:** PASS  
**Executor:** VRM Games / project developer  
**Date:** 2026-06-25  
**Commit under test:** local Sprint 8 working tree; SHA recorded after publication

## Build configuration

| Field | Value |
|---|---|
| Platform | Windows x64 |
| Configuration | Development Build |
| Unity | `6000.3.18f1` |
| Application version | `0.0.9` |
| Output path | Local developer build; path not retained in this record |

## Build result

| Gate | Result | Evidence / notes |
|---|---|---|
| Build completes | PASS | Build executable created successfully |
| Executable launches externally | PASS | External Player opened correctly |
| Bootstrap → MainMenu → Store | PASS | Full scene flow validated |
| Movement and camera | PASS | Existing controls preserved |
| Placement and return flow | PASS | Existing placement and navigation preserved |
| Version `0.0.9` | PASS | Sprint target version validated |
| Crash or blocking error | PASS | None reported |
| Unity Services membership message | NON-BLOCKING | Previous known project-link warning has no dependency impact on current systems |

## External smoke scope

- Application launch.
- Bootstrap initialization.
- MainMenu load.
- Store entry.
- Movement and camera.
- Construction mode, placement, cancellation and removal.
- Return to MainMenu.
- Normal application exit.

## Final build decision

**PASS.** Windows x64 Development build and external Player execution satisfy
the Sprint 8 closure gate.
