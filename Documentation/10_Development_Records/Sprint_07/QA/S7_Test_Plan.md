# Sprint 07 — Test Plan

## Automated scope

### Focused EditMode suites

- `VRMGames.CartridgeAndCloud.Tests.EditMode.Suppliers`
- `VRMGames.CartridgeAndCloud.Tests.EditMode.Orders`
- `VRMGames.CartridgeAndCloud.Tests.EditMode.Receiving`
- `VRMGames.CartridgeAndCloud.Tests.EditMode.Authoring`

Expected focused result: **72/72 PASS**.

### Full regression target

| Suite | Accepted Sprint 6 target | Sprint 7 additions | Sprint 7 target |
|---|---:|---:|---:|
| EditMode | 187 | 72 | 259 |
| PlayMode | 41 | 0 | 41 |
| Total | 228 | 72 | 300 |

Actual counts must be taken from Unity Test Runner.

## High-priority invariant coverage

- stable, ordinal identifiers;
- no duplicate catalog or order products;
- positive packaging and cost values;
- deterministic totals;
- legal order-state transitions only;
- deterministic box generation;
- exact quantity receipt;
- no mutation on operational failure;
- no duplicate receipt;
- capacity enforcement;
- product-registry enforcement;
- final receipt closes delivery and order.

## Manual asset validation

- inspect all six product assets;
- inspect product catalog;
- inspect both supplier assets;
- inspect both supplier catalogs;
- confirm no missing scripts;
- confirm object references resolve;
- confirm optional icon and prefab fields may remain empty.

## Manual regression

- Bootstrap;
- MainMenu;
- normal and direct Store flow;
- movement;
- camera;
- placement;
- access validation;
- TestLab;
- return to MainMenu.

## Build validation

- Windows x64 Development build;
- external launch;
- version `0.0.8`;
- baseline scene flow and controls;
- no startup exceptions or missing-script warnings.
