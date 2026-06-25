# Sprint 08 — Charter

**Name:** Displays & Restocking  
**Target version:** `0.0.9`  
**Dependency baseline:** Sprint 7 closure commit `b997377eae720e960c02ee5dce53da8b37f66b19`  
**Accepted automated baseline:** `300/300 PASS`  
**Closure model:** evidence-gated  
**Initial status:** IN PROGRESS

## Objective

Deliver the display-stock layer required by the Vertical Slice:

1. author display definitions in Unity;
2. create runtime display instances with isolated inventory containers;
3. assign one validated product to each display;
4. restock manually from Storage or Transit inventory;
5. return exposed stock safely before changing assignment;
6. expose deterministic visible-unit counts for later presentation and customer systems.

## In scope

- Display definition and instance identifiers.
- Capacity measured in total units.
- Separate visible-unit limits.
- Optional category restrictions.
- One assigned product per display.
- Assignment clearing only when empty.
- Display inventory backed by Sprint 6.
- Exact restocking and restock-to-capacity.
- Return-to-storage and return-all-and-clear.
- RestockTask domain model.
- ScriptableObject definition/catalog authoring.
- Runtime authoring component for placed prefabs.
- Three technical definitions and one technical catalog.
- EditMode coverage and Sprint 8 version assertion.

## Out of scope

- Customers, reservations or checkout.
- Final player-facing UI.
- Employee AI or timed animation.
- Persistent GameObject per product unit.
- Final models, materials, icons or product meshes.
- Economy and ledger.
- Complete save/load.
- Automatic scene or prefab replacement.

## Architectural constraints

- Domain and Application remain independent of Unity.
- ScriptableObjects are authoring inputs, not runtime state authority.
- Display stock uses `InventoryContainerType.Display`.
- A display may hold only its assigned product.
- Failed operations are non-mutating.
- Successful transfers conserve units.
- Visible count is derived and capped.
- Existing scenes, prefabs, asmdefs and ProjectSettings are not replaced.

## Acceptance gates

- Clean compilation.
- Focused Sprint 8 EditMode tests PASS.
- Full EditMode and PlayMode regression PASS.
- Technical asset inspection PASS.
- Manual runtime-authoring and Store regression PASS.
- Windows x64 Development build and external run PASS.
- Version `0.0.9`.
- Zero S0/S1 defects.
- QA, traceability, closure and handoff completed with actual evidence.
