# ADR-0032 — Atomic manual restocking

**Status:** Accepted for Sprint 8

Restocking validates source type, assignment, product, quantity, stock and capacity before using the Sprint 6 transfer service. Sources are limited to Storage and Transit. Returns use the same transfer foundation in reverse.

Failures leave source and display unchanged; successes conserve units.
