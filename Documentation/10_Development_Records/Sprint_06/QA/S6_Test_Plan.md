# Sprint 06 — QA Test Plan

## Automated focus

1. Product identifiers and immutable definitions.
2. Product registry uniqueness and lookup.
3. Quantity and capacity bounds.
4. Stack validity.
5. Initial container validation.
6. Add/remove success, limits and no-mutation failures.
7. Deterministic stack snapshots.
8. Valid transfers and stack merging.
9. Zero, undefined, missing, insufficient, full and same-container failures.
10. Conservation across one and multiple transfers.

## Test level

- Domain: EditMode only.
- Application version: manual validation in Project Settings > Player.
- No new PlayMode tests because no component, scene or presentation behavior is
  introduced.

## Regression gates

- Full EditMode.
- Full PlayMode.
- Bootstrap/MainMenu/Store smoke.
- Store movement, camera, placement and access smoke.
- Windows x64 Development build and external execution.

## Severity policy

- S0: crash, corruption or unit loss/duplication.
- S1: transfer atomicity, quantity, capacity or product-identity invariant broken.
- S2: degraded API or non-blocking workflow issue.
- S3: documentation or cosmetic issue.

No open S0 or S1 defect permits closure.
