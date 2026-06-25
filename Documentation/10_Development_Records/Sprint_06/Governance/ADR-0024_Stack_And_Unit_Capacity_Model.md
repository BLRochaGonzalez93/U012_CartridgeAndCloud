# ADR-0024 — Stack and Unit-Capacity Model

**Status:** Accepted for Sprint 6  
**Date:** 2026-06-25

## Context

The baseline leaves stack versus slot representation and capacity semantics open.
Sprint 6 needs a deterministic model that does not assume shelf facings,
physical volume or later economy data.

## Decision

Each container holds at most one logical stack per product. Capacity is the
maximum total number of units in the container. Additions merge by product ID;
removing the final unit removes the stack.

## Consequences

- Quantity and capacity rules are simple and testable.
- Transfers can conserve integer units exactly.
- Display slots, product dimensions, weight and volume remain deferred.
- A future capacity policy may replace unit capacity behind a new approved ADR;
  it must preserve migration and invariant coverage.
