# Sprint 08 — Test Plan

## Automated

Validate identifiers, capacity/visibility invariants, category policy, registries, assignment, safe clearing, restock, return, unit conservation, RestockTask transitions, ScriptableObject mapping and runtime-authoring behavior.

## Manual

1. Inspect the three technical definitions and catalog.
2. Confirm capacities, visible limits, categories and placement IDs.
3. Confirm prefab references are intentionally empty.
4. In TestLab, create a temporary GameObject with `DisplayRuntimeAuthoring`, assign a unique ID and technical definition, then remove it without saving baseline changes.
5. Regression-test Bootstrap, MainMenu, Store, movement, camera, placement, access and TestLab.

## Build

Create and run a Windows x64 Development build and confirm version `0.0.9`.
