# Sprint 04 — Grid & Placement Foundation

**State:** Started  
**Dependency:** Sprint 3 Closed / PASS  
**Final target version:** `0.0.5`

## Sprint objective

Establish a deterministic 0.5-metre logical grid and the minimum placement
pipeline required to preview, rotate, validate and confirm technical objects
without introducing the final Store shell or product systems.

## SubSprint plan

### CC_S4.1 — Grid Coordinate Foundation

- Pure grid coordinates.
- Positive integer sizes.
- Quarter-turn rotations.
- Rectangular footprints.
- Logical bounds.
- World-to-cell and cell-to-world projection.
- EditMode coverage.
- No scene or Input Action changes.

### CC_S4.2 — Placement Preview & Rotation

- Technical placeable definition.
- TestLab placement surface.
- Placement ghost.
- Pointer-driven snapped preview.
- Q/E rotation in 90-degree steps.
- Valid/invalid visual state.
- Input-context integration.

### CC_S4.3 — Occupancy & Base Validation

- Logical occupancy registry.
- Bounds validation.
- Overlap rejection.
- Confirm/cancel/remove technical placement.
- Stable placement identifiers.
- EditMode and PlayMode coverage.

### CC_S4.4 — Integration, Regression & Closure

- Complete placement flow regression.
- Movement/camera/input-context regression.
- Version `0.0.5`.
- Windows x64 build and external execution.
- QA, traceability and documentation closure.

## Frozen decisions

- Logical cell size: `0.5 m`.
- Object footprints use positive integer cells.
- Rotation uses four quarter turns: 0°, 90°, 180° and 270°.
- The footprint anchor is the minimum X/Z occupied cell.
- Logical occupancy is separated from visual representation.
- Sprint 4 validates bounds and overlap only.
- Door/access/interactability blocking belongs to Sprint 5.
- Store remains unchanged until Sprint 5.
- Sprint 4 technical validation occurs in TestLab.

## Out of scope

- Final Store shell.
- Doors and access-path validation.
- NavMesh rebake.
- Furniture economy, purchasing or refunds.
- Product inventory or display assignment.
- Save persistence of placed objects.
- Final furniture art.
