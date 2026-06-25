# ADR-0022 — Store Placement and Access Integration

**Status:** Accepted  
**Sprint:** 5  
**Decision date:** 2026-06-25

## Decision

Store reuses the Sprint 4 placement components and Input Actions.

Candidates are validated in this order:

1. placement-surface bounds;
2. occupancy overlap;
3. entrance and required-anchor access.

Access validation is optional on `PlacementRuntimeController`. Store enables the
initial Store access layout; TestLab does not, preserving its Sprint 4
behaviour.

Store exposes the detailed access failure while mapping invalid access to the
generic `AccessBlocked` placement failure for red preview feedback.

## Scene structure

`S5_StorePlacement` is a child of `S5_StoreShell`. No extra Store root is added.

It contains the input driver, preview controller, runtime controller, ghost
visual, placed-object root and technical access markers.

## Consequences

- Construction coexists with movement, orbit and zoom.
- Movement input is suppressed only while construction mode is active.
- Store maintains the approved six-root scene structure.
