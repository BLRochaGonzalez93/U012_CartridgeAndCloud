# Sprint 06 — Product & Inventory Core Charter

**State:** OPEN / IMPLEMENTATION PREPARED  
**Opening date:** 2026-06-25  
**Baseline application version:** `0.0.6`  
**Target application version:** `0.0.7`  
**Baseline technical commit:** `22d62967aaf9895db7bce75afd2ffa11f7858e0c`

## Goal

Establish a deterministic, Unity-independent product and inventory foundation
that preserves quantities, capacity and atomicity before supplier, display,
customer or economy systems are introduced.

## SubSprints

| SubSprint | Scope | Initial status |
|---|---|---|
| CC_S6.1 | Product definitions and registry | READY |
| CC_S6.2 | Quantities, stacks, containers and capacity | READY |
| CC_S6.3 | Atomic transfers and invariant tests | READY |
| CC_S6.4 | Regression, version, build and closure | PENDING |

## Accepted scope

- Product, category and tag identifiers.
- Immutable product definitions.
- Immutable product-definition registry.
- Non-negative quantity and unit-capacity value objects.
- Inventory container IDs and inert container classification.
- One logical stack per product per container.
- Deterministic read-only stack snapshots.
- Container add/remove operations with typed results.
- Atomic inter-container transfers.
- Product-definition existence validation.
- Capacity, insufficiency and same-container rejection.
- Conservation-of-units assertion.
- EditMode tests without scene dependencies.
- Application version `0.0.7` at closure.

## Excluded scope

- Supplier catalogue, orders, delivery and receiving.
- Shipment boxes.
- Display assignment and restocking.
- Customer shopping, reservation, cart, queue or checkout.
- Unit cost, suggested price, ledger or economy rules.
- Product physical size or volume-based capacity.
- Inventory persistence, snapshots or SaveRootV1 changes.
- UI, scene, prefab, GameObject or MonoBehaviour integration.

## Definition of done

- Clean Unity compilation.
- Every Sprint 6 acceptance criterion PASS.
- New focused EditMode suite `60/60 PASS`.
- Full expected automated regression `228/228 PASS`, or an explicitly
  reconciled actual count with no failures.
- Existing Store, TestLab and scene-flow manual regression PASS.
- Application version `0.0.7` validated.
- Windows x64 Development build and external execution PASS.
- No open S0 or S1 defects.
- Technical commit and documentation closure published.

## Closure rule

This charter opens implementation but does not pre-authorize closure. All
records remain PENDING until evidence is captured from the actual Unity project.
