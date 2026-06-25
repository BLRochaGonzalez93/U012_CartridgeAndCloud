# Sprint 04 — Grid & Placement Foundation

**State:** Completed  
**Result:** PASS  
**Dependency:** Sprint 3 Closed / PASS  
**Closure date:** 2026-06-25  
**Final application version:** `0.0.5`  
**Final technical commit:** `bcf8cdd309d89570b5b58a0735154b4863e2aebc`

## Sprint objective

Establish a deterministic 0.5-metre logical grid and the minimum placement
pipeline required to preview, rotate, validate, confirm and remove technical
objects without introducing the final Store shell or product systems.

## Completed SubSprints

### CC_S4.1 — Grid Coordinate Foundation

- Pure integer grid coordinates.
- Positive integer grid sizes.
- Quarter-turn rotations.
- Rectangular footprints.
- Logical bounds.
- World-to-cell and cell-to-world projection.
- Negative-coordinate support.
- EditMode coverage.

### CC_S4.2 — Placement Preview & Rotation

- Technical ScriptableObject placeable definition.
- TestLab placement surface.
- Snapped placement ghost.
- Pointer-driven preview.
- Q/E quarter-turn rotation.
- Green/red bounds feedback.
- Gameplay Input Action integration.

### CC_S4.3 — Occupancy & Base Validation

- Explicit construction mode.
- Atomic logical occupancy.
- Bounds and overlap rejection.
- Stable placement identifiers.
- Confirm, cancel and remove flow.
- Click-to-move conflict policy.
- Collider and logical-cell removal fallback.

### CC_S4.4 — Integration, Regression & Closure

- Reproducible empty TestLab baseline.
- Version `0.0.5`.
- Complete `118/118` automated regression.
- Full manual placement and scene-flow regression.
- Windows x64 build and external execution.
- QA, traceability and documentation closure.

## Accepted decisions

- Logical cell size: `0.5 m`.
- Object footprints use positive integer cells.
- Rotation uses 0°, 90°, 180° and 270°.
- Footprint anchor is the minimum occupied X/Z cell.
- Logical occupancy is separated from visual representation.
- Validation completes before occupancy mutation.
- Placement mode owns confirmation input.
- Click-to-move is suppressed only during placement mode.
- Sprint 4 validates bounds and overlap only.
- Door/access/interactability blocking remains Sprint 5 scope.
- Store remains unchanged until Sprint 5.
- Technical validation occurs in TestLab.

## Deferred scope

- Final Store shell.
- Door and access-route validation.
- Interaction-point validation.
- NavMesh updates after construction.
- Furniture economy, purchasing and refunds.
- Product inventory and display assignment.
- Save persistence of placed objects.
- Final placeable catalogue and art.
