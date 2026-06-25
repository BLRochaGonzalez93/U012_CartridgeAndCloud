# ADR-0029 — Shipment-Box Receiving Is Atomic

**Status:** Accepted for Sprint 7

## Context

The receiving flow crosses order state, delivery state and Sprint 6 inventory.
A failure after partial mutation could duplicate or lose units.

## Decision

Before mutation, `ReceivingService` validates:

1. the order is in Delivered state;
2. delivery and order IDs match;
3. supplier IDs match;
4. the requested box exists;
5. the box is not already received;
6. its product exists in the product registry;
7. the destination has sufficient capacity.

Only after all validations pass are units added to inventory and the box marked
received. If the delivery mutation unexpectedly rejects the box, the inventory
addition is defensively rolled back.

The final received box transitions the purchase order to Received.

## Consequences

- Operational failures use typed results.
- Failed receipt attempts preserve all involved state.
- Duplicate receipt cannot increase stock.
- Inventory conservation can be tested directly.
