# Sprint 06 — Acceptance Matrix

| ID | SubSprint | Criterion | Evidence | Status |
|---|---|---|---|---|
| AC-S6-01 | S6.1 | Product IDs reject empty values and use ordinal equality | EditMode 60/60 | PASS |
| AC-S6-02 | S6.1 | Product definitions are immutable and require valid identity data | EditMode 60/60 | PASS |
| AC-S6-03 | S6.1 | Duplicate product tags are rejected | EditMode 60/60 | PASS |
| AC-S6-04 | S6.1 | Registry rejects null and duplicate definitions | EditMode 60/60 | PASS |
| AC-S6-05 | S6.1 | Registry lookup and deterministic ordering work | EditMode 60/60 | PASS |
| AC-S6-06 | S6.2 | Quantity never accepts negative values | EditMode 60/60 | PASS |
| AC-S6-07 | S6.2 | Capacity never accepts negative values | EditMode 60/60 | PASS |
| AC-S6-08 | S6.2 | Container IDs and types must be valid | EditMode 60/60 | PASS |
| AC-S6-09 | S6.2 | Zero-quantity stacks are rejected | EditMode 60/60 | PASS |
| AC-S6-10 | S6.2 | Initial stacks cannot duplicate products or exceed capacity | EditMode 60/60 | PASS |
| AC-S6-11 | S6.2 | Add merges one product stack and updates capacity | EditMode 60/60 | PASS |
| AC-S6-12 | S6.2 | Remove preserves non-negative quantities and removes empty stacks | EditMode 60/60 | PASS |
| AC-S6-13 | S6.2 | Failed add/remove operations do not mutate state | EditMode 60/60 | PASS |
| AC-S6-14 | S6.2 | Stack snapshots are deterministic and detached | EditMode 60/60 | PASS |
| AC-S6-15 | S6.3 | Valid transfer moves requested units | EditMode 60/60 | PASS |
| AC-S6-16 | S6.3 | Valid transfer merges destination stack | EditMode 60/60 | PASS |
| AC-S6-17 | S6.3 | Full source transfer removes empty source stack | EditMode 60/60 | PASS |
| AC-S6-18 | S6.3 | Zero transfer is rejected atomically | EditMode 60/60 | PASS |
| AC-S6-19 | S6.3 | Undefined product is rejected atomically | EditMode 60/60 | PASS |
| AC-S6-20 | S6.3 | Missing or insufficient source quantity is rejected atomically | EditMode 60/60 | PASS |
| AC-S6-21 | S6.3 | Destination capacity overflow is rejected atomically | EditMode 60/60 | PASS |
| AC-S6-22 | S6.3 | Same logical container transfer is rejected atomically | EditMode 60/60 | PASS |
| AC-S6-23 | S6.3 | Total units are conserved by successful transfers | EditMode 60/60 | PASS |
| AC-S6-24 | S6.4 | Focused Sprint 6 suite passes | 60/60 PASS | PASS |
| AC-S6-25 | S6.4 | Complete automated regression passes after inherited assertion correction | Current 300/300 PASS | PASS |
| AC-S6-26 | S6.4 | Existing Store, TestLab and scene-flow regression passes | Manual validation | PASS |
| AC-S6-27 | S6.4 | Application version is 0.0.7 | Editor/build | PASS |
| AC-S6-28 | S6.4 | Windows x64 Development build and external execution pass | Build/manual | PASS |
| AC-S6-29 | S6.4 | No open S0/S1 defect remains | QA record | PASS |
| AC-S6-30 | S6.4 | Implementation and closure documentation are included in repository history | Git commit containing closure records | PASS |

## Final result

**PASS: 30/30 criteria accepted.**
