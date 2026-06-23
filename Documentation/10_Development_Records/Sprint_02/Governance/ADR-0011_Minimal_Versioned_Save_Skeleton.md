# ADR-0011 — Minimal Versioned Save Skeleton

**Status:** Proposed  
**Sprint:** 2

## Decision
Sprint 2 introduces a versioned, JSON-backed save skeleton with:
- Pure Domain session and snapshot types.
- Application-layer service and repository contracts.
- Infrastructure JSON persistence.
- Three explicit slots: 0, 1 and 2.
- Schema version 1.
- UTC timestamps stored as ticks.
- One temporary-file write followed by replacement of the target slot.

Bootstrap creates a new in-memory session in slot 0 but does not autosave it.

## Rationale
This establishes persistence boundaries and deterministic identifiers without coupling
future gameplay systems to Unity serialization or scene objects.

## Deferred
Autosave, backup rotation, migrations, UI, cloud saves and full vertical-slice state
remain deferred to later sprints.
