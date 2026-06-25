# ADR-0026 — Sprint 6 Version and Persistence Boundary

**Status:** Accepted for Sprint 6  
**Date:** 2026-06-25

## Decision

- Freeze target application version at `0.0.7`.
- Do not change SaveRootV1, schema versions or persistence DTOs.
- Do not add scene or presentation integration.
- Expose inventory collections as immutable snapshots only.

## Rationale

Sprint 6 must establish domain invariants without prematurely defining the
complete vertical-slice save schema or coupling inventory state to scene
objects. Version change is applied explicitly during closure preparation.
