# ADR-0033 — Display authoring and placement boundary

**Status:** Accepted for Sprint 8

`DisplayDefinitionAsset` stores capacity, visibility, category policy, placement-definition ID and an optional prefab. `DisplayRuntimeAuthoring` can be attached to a future placed display prefab and builds a pure domain instance.

Sprint 8 does not overwrite the Store scene or current technical placement prefab.
