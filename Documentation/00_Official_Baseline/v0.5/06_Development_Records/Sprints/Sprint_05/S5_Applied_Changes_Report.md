# Sprint 05 — Applied Changes Report

**Date:** `2026-06-25`  
**Application version:** `0.0.6`  
**Final technical commit:** `22d62967aaf9895db7bce75afd2ffa11f7858e0c`

## CC_S5.1 — Store Shell Foundation

- Added a playable `10 x 15 m` Store shell.
- Added central `2 m` entrance, apron and threshold.
- Added technical player and spawn.
- Reused click-to-move, orbit and zoom.
- Preserved normal and direct Store return flow.
- Published commit `7acbdd7e3860238f5a4961f8f17537cd6c505231`.

## CC_S5.2 — Access Validation Foundation

- Added access-anchor identifiers and records.
- Added immutable `20 x 30` Store access layout.
- Added four-direction breadth-first reachability.
- Added entrance-width and entrance-reservation policy.
- Added three required access anchors.
- Added candidate validation without occupancy mutation.
- Published commit `3b4d33698dd4f71c8499960887a39cfb43a414e9`.

## CC_S5.3 — Store Placement Integration

- Integrated Sprint 4 placement into Store.
- Added `4 x 2` technical placeable.
- Added Store placement surface, ghost, placed-object root and access markers.
- Added optional access validation to placement runtime.
- Added detailed access-failure reporting.
- Preserved TestLab behaviour.
- Added serialized Store access activation after scene reload.
- Published commit `0c71fd1c622d86947483a12455ff83fb9585665b`.

## CC_S5.4 — Integration, Regression & Closure

- Reset Store to a reproducible empty baseline.
- Validated Store shell, placement and access wiring.
- Validated enabled build scenes and Bootstrap ordering.
- Updated application version to `0.0.6`.
- Updated version assertions and Sprint 5 validators.
- Completed final automated, manual, build and external-execution gates.
- Published commit `22d62967aaf9895db7bce75afd2ffa11f7858e0c`.

## Final test growth

| Baseline | Total |
|---|---:|
| Sprint 4 closure | 118 |
| Sprint 5 closure | 168 |
| Tests added | 50 |

## Final result

All accepted Sprint 5 changes are integrated, tested and published.
