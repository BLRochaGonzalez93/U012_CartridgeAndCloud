# ADR-0022 — Store Placement and Access Integration

**Status:** Proposed  
**Sprint:** 5  
**SubSprint:** CC_S5.3

## Proposed decision

Store reuses the Sprint 4 placement components and Input Actions.

The placement runtime validates candidates in this order:

1. placement surface bounds;
2. logical occupancy overlap;
3. Store entrance and required-anchor connectivity.

Access validation is optional on `PlacementRuntimeController`. TestLab does
not configure it and therefore preserves Sprint 4 behaviour.

Store configures `StoreAccessLayout.InitialTier()` and exposes the detailed
failure through `CurrentAccessFailureReason`. The generic placement failure
is `AccessBlocked`, allowing the existing ghost view to use its invalid
material.

## Scene structure

`S5_StorePlacement` is created below `S5_StoreShell`; no additional root is
introduced.

It contains:

- placement input driver;
- preview controller;
- runtime controller;
- ghost visual;
- placed-object root;
- entrance and required-anchor technical markers.

## Deferred

- Save persistence.
- Catalogue and purchase flows.
- Final models and materials.
- Dynamic navigation mesh updates.
