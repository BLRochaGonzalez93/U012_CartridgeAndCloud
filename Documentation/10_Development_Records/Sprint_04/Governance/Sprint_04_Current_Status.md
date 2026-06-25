# Sprint 04 — Current Status

**State:** CC_S4.4 ready for integration and evidence  
**Result:** In progress  
**Application version before integration:** `0.0.4`  
**Target version:** `0.0.5`

| SubSprint | State |
|---|---|
| CC_S4.1 — Grid Coordinate Foundation | Published / PASS |
| CC_S4.2 — Placement Preview & Rotation | Published / PASS |
| CC_S4.3 — Occupancy & Base Validation | Published / PASS |
| CC_S4.4 — Integration, Regression & Closure | Ready for validation |

## Verified technical commits

- CC_S4.1:
  `1e19509edd9c729fb1d2af7b6ef580eabbb670d8`
- CC_S4.2:
  `bda38e0e84654aaabe8414b772d6c88e25e70e41`
- CC_S4.3:
  `8c3cad65c7968e5317cde64177803f7f705f484f`

## Current validated baseline

- Grid cell size: `0.5 m`.
- Technical placement surface: `16 x 16` cells.
- Technical placeable: `technical-shelf-4x2`.
- Quarter-turn rotation: Q/E.
- Explicit placement mode: B.
- Confirm: left click.
- Cancel: Escape.
- Remove: Delete or Backspace.
- Logical bounds and overlap validation: PASS.
- Atomic occupancy and removal: PASS.
- Full automated suite before CC_S4.4: `118/118 PASS`.

## Remaining closure gates

- Integrate final version `0.0.5`.
- Run CC_S4.4 validator.
- Run full automated regression `118/118`.
- Run final TestLab manual regression.
- Run MainMenu/Store scene-flow regression.
- Build Windows x64 development player.
- Execute the player externally.
- Record final evidence.
- Publish the final technical commit.
- Publish documentation closure.
