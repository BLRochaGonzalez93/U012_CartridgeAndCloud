# Sprint 04 — Closure Report

**Sprint:** Grid & Placement Foundation  
**Status:** Pending evidence-gated closure  
**Target version:** `0.0.5`

## Delivered scope

- Deterministic integer grid coordinates.
- 0.5-metre world/grid projection.
- Rectangular logical footprints.
- Quarter-turn rotation.
- Technical snapped placement preview.
- Bounds feedback.
- Explicit construction mode.
- Atomic logical occupancy.
- Overlap and duplicate-ID rejection.
- Runtime confirmation and removal.
- Input conflict handling with movement and camera.

## Deferred scope

- Store construction integration.
- Access-route and interaction-point validation.
- NavMesh updates.
- Economy.
- Persistence.
- Final placeable catalogue.

## Technical commits

- CC_S4.1:
  `1e19509edd9c729fb1d2af7b6ef580eabbb670d8`
- CC_S4.2:
  `bda38e0e84654aaabe8414b772d6c88e25e70e41`
- CC_S4.3:
  `8c3cad65c7968e5317cde64177803f7f705f484f`
- CC_S4.4:
  Pending.

## Automated evidence

- CC_S4.1 full suite: `84/84 PASS`.
- CC_S4.2 full suite: `96/96 PASS`.
- CC_S4.3 full suite: `118/118 PASS`.
- CC_S4.4 final suite: Pending.

## Manual evidence

Pending final integrated TestLab and scene-flow regression.

## Build evidence

Pending Windows x64 development build and external execution.

## Incidents

- CC_S4.2 compilation ambiguity for `UnityEngine.Object`: Resolved.
- CC_S4.2 missing Editor reference to `Unity.InputSystem`: Resolved.
- CC_S4.3 compilation ambiguity for `UnityEngine.Object`: Resolved.
- CC_S4.3 removal control flow and selection fallback: Resolved.

## Final decision

Pending.
