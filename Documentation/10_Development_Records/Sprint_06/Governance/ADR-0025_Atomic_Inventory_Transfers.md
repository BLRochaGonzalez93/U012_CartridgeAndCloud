# ADR-0025 — Atomic Inventory Transfers

**Status:** Accepted for Sprint 6  
**Date:** 2026-06-25

## Context

A transfer touches two mutable containers. Sequential public mutations could
remove from the source before discovering that the destination cannot accept
the units.

## Decision

`InventoryTransferService` validates all operational preconditions first, then
uses internal validated container mutations. Expected failures return an
`InventoryTransferResult` and leave both containers unchanged.

Successful transfers calculate the combined unit total before and after and
throw if conservation is ever violated.

## Consequences

- No partial mutation on known failure paths.
- Operational callers receive stable typed failure reasons.
- Domain code remains independent from Unity and scenes.
- Threading and distributed transactions are outside current scope.
