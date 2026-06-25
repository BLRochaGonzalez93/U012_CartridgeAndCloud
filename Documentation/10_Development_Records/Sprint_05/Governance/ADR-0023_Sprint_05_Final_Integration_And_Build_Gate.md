# ADR-0023 — Sprint 05 Final Integration and Build Gate

**Status:** Accepted  
**Sprint:** 5  
**Decision date:** 2026-06-25

## Decision

Sprint 5 closes through an evidence-gated integration.

The accepted closure baseline:

- resets Store to zero confirmed placements;
- starts with construction inactive and ghost hidden;
- preserves the six approved Store roots;
- verifies Store shell, placement surface, input wiring and access validation;
- enables Bootstrap, MainMenu and Store as build scenes;
- keeps Bootstrap as the first enabled build scene;
- sets application version `0.0.6`;
- requires the complete `168/168` automated regression;
- requires Windows x64 Development build and external execution.

## Final evidence

All technical, automated, manual, build and external-execution gates passed.

## Release disposition

No sprint tag, GitHub release, build archive or checksum is required. These
remain reserved for the final build of the current phase.
