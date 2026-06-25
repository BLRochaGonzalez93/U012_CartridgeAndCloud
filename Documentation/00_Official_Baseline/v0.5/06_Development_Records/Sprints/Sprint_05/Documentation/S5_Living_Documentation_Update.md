# Sprint 05 — Living Documentation Update

**Date:** 2026-06-25  
**Result:** PASS  
**Technical baseline:** `22d62967aaf9895db7bce75afd2ffa11f7858e0c`  
**Application version:** `0.0.6`

## Confirmed project state

The following facts are now part of the living project state:

- The initial Store interior is `10 x 15 m`.
- The initial logical Store grid is `20 x 30` cells at `0.5 m`.
- The central entrance is `2 m` / four cells wide.
- At least two adjacent entrance cells must remain open.
- Store access uses deterministic four-direction grid connectivity.
- Three required access anchors are validated.
- Placement candidates are checked against bounds, occupancy and access.
- Store construction uses the existing Gameplay Input Actions.
- TestLab remains the isolated placement regression environment.
- Store starts from an empty, reproducible placement baseline.
- Sprint 5 closes at version `0.0.6`.
- The accepted automated baseline is `168/168 PASS`.

## Deferred systems

These remain outside the accepted Sprint 5 baseline:

- placement persistence;
- economy and purchase/refund flows;
- furniture catalogue;
- products and inventory;
- customers, employees and checkout;
- dynamic NavMesh;
- final Store art;
- Store expansion.

## Baseline policy

Official documentation baseline v0.4 remains immutable. Sprint 5 records are
development evidence and do not replace that baseline.
