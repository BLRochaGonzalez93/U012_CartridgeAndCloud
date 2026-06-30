# ADR-0035 - StoreInitial Manual Scene Authoring

**Status:** Accepted / In implementation  
**Date:** 2026-07-01

## Context

Automatic visual replacement used placeholder bounds, scaled technical roots and imported FBX axes. Walls, door leaves and static warehouse furniture could not be placed reliably despite tests passing.

## Decision

Create `StoreInitial.unity` and `StoreInitialEnvironment.prefab`. Author fixed architecture and initial furniture manually. Runtime resolves references through `StoreInitialSceneContext` and creates only dynamic state.

## Consequences

- Visual placement becomes explicit and reviewable.
- Technical colliders/grid remain authoritative.
- Procedural shell remains temporary fallback until migration passes.
- Scene and prefab require manual QA.
- Build scene list changes only after runtime connection.
