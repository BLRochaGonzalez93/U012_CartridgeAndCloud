# Sprint 01 — Test Plan

## Expected automatic suite

### EditMode — 6

1. Production assembly graph.
2. Test assembly references and platforms.
3. Approved scene assets exist.
4. Global scene list is exact.
5. Build indexes are 0–3.
6. Transition gate rejects concurrent entry.

### PlayMode — 8

1. Bootstrap opens MainMenu with one ApplicationRoot.
2. MainMenu loads its technical objects.
3. Store loads its technical objects.
4. TestLab retains its Sprint 0 baseline.
5. MainMenu controller loads Store.
6. Store controller returns to MainMenu.
7. A complete round trip preserves one ApplicationRoot.
8. A concurrent transition request is rejected.

## Manual regression

- Ten consecutive scene-flow loops.
- Rapid multi-click test.
- Unity restart and second full test run.
- Windows external run and clean exit.
- Player.log review.
