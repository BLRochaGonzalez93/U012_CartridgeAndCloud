# ADR-0070 — Placement compatibility bridge

**Estado:** Accepted for Phase 1

Phase 1 reuses the validated placement grid and occupancy registry. A runtime
bridge supplies catalog definitions, records confirmed placement, restores
placed fixtures and reconciles removal.

The bridge is replaceable after the vertical slice and does not alter Domain.
