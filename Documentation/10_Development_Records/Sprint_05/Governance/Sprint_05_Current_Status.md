# Sprint 05 — Current Status

**State:** CC_S5.2 package prepared  
**Result:** In progress  
**Current application version:** `0.0.5`

| SubSprint | State |
|---|---|
| CC_S5.1 — Store Shell Foundation | Published / PASS |
| CC_S5.2 — Access Validation Foundation | Ready for integration |
| CC_S5.3 — Store Placement Integration | Not started |
| CC_S5.4 — Integration, Regression & Closure | Not started |

## Accepted CC_S5.1 baseline

- Store shell: `10 x 15 m`.
- Future logical grid: `20 x 30` cells.
- Cell size: `0.5 m`.
- Central entrance: `4 cells` / `2 m`.
- Technical player, click-to-move, orbit and zoom.
- Normal and direct Store return flow.
- Full automated baseline: `129/129 PASS`.
- Technical commit:
  `7acbdd7e3860238f5a4961f8f17537cd6c505231`.

## CC_S5.2 target

- Add pure access-anchor domain concepts.
- Add immutable Store access layout.
- Add four-direction breadth-first connectivity validation.
- Require at least two adjacent open entrance cells.
- Reserve all four entrance cells against placement candidates.
- Require access to rear, left-display and right-display anchors.
- Validate candidate footprints without mutating occupancy.
- Cover barriers, gaps, rotations and route restoration.
- Add `24` EditMode tests.
- Expected full suite: `153/153`.

## Scope boundary

CC_S5.2 changes no scene, runtime presentation, Input Action, asmdef,
ProjectSettings or application version. Store integration remains CC_S5.3.
