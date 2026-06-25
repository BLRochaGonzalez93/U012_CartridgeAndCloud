# Sprint 07 — Charter

**Name:** Supplier Orders & Receiving  
**Target version:** `0.0.8`  
**Dependency baseline:** Sprint 6 Product & Inventory Core, functionally validated  
**Closure model:** evidence-gated  
**Initial status:** IN PROGRESS

## Objective

Deliver the first complete procurement flow over the Sprint 6 product and
inventory core:

1. author technical products and suppliers in Unity;
2. build validated supplier catalogs;
3. create purchase orders using whole shipment boxes;
4. submit orders and generate deterministic deliveries;
5. receive individual boxes into an inventory container without loss,
   duplication or partial mutation.

## In scope

- `ProductDefinitionAsset` and `ProductCatalogAsset`.
- `SupplierDefinitionAsset` and `SupplierCatalogAsset`.
- Technical product and supplier assets.
- Supplier catalog prices, units per box and order limits.
- Purchase-order creation and lifecycle.
- Delivery and shipment-box creation.
- Atomic box receipt into Sprint 6 inventory.
- Duplicate-receipt prevention.
- Typed operational failure results.
- EditMode automated coverage.
- Correction of the inherited project-version test.

## Out of scope

- Player-facing ordering or receiving UI.
- Delivery scheduling through the day cycle.
- Cash deduction, ledger posting, taxes or refunds.
- Save/load integration.
- Final product icons, models, materials or box prefabs.
- Displays and restocking.
- Customer shopping and checkout.
- Scene authority or new scene objects.

## Architectural constraints

- Domain and Application remain free of Unity references.
- ScriptableObjects are authoring inputs, not runtime state authority.
- Product and supplier identity use stable ordinal string IDs.
- Orders request whole boxes.
- Receiving validates every precondition before inventory mutation.
- A failed receipt must leave inventory, delivery and order unchanged.
- No existing asmdef, scene, prefab or ProjectSettings file is changed by
  the package, except the explicitly replaced version assertion source.

## Acceptance gates

Sprint 7 may close only when all of the following are evidenced:

- clean compilation;
- 72/72 focused Sprint 7 EditMode tests;
- 259/259 full EditMode regression;
- 41/41 PlayMode regression;
- 300/300 total automated regression;
- authoring-asset inspection PASS;
- manual project regression PASS;
- Windows x64 Development build PASS;
- external executable run PASS;
- version `0.0.8` confirmed;
- zero open S0/S1 defects;
- QA, traceability and closure records completed with actual results.
