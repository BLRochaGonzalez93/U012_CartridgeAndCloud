# ADR-0028 — Purchase Orders Use Whole Shipment Boxes

**Status:** Accepted for Sprint 7

## Context

Supplier catalogs need deterministic order, delivery and receipt quantities.
Ordering arbitrary loose units would require packaging and partial-box rules
that are not part of the current Vertical Slice scope.

## Decision

A supplier catalog entry defines:

- product ID;
- unit purchase cost in integer cents;
- units per box;
- minimum boxes per order line;
- maximum boxes per order line.

A purchase-order request specifies a positive whole-number box count. The
resulting ordered quantity is `box count × units per box`.

## Consequences

- Every ordered box maps to exactly one `ShipmentBox`.
- Delivery generation is deterministic.
- Cost and quantity calculations avoid floating-point arithmetic.
- Fractional boxes and mixed-content boxes remain deferred.
