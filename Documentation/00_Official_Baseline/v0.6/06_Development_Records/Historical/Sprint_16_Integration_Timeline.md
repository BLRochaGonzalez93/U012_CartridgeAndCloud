# Sprint 16 - Representative Integration Timeline

## Phase 1 validated baseline

Commit `091090c43855b0b26b09abe9335d18b978ac7eab` established the complete functional vertical-slice baseline before replacing blockout assets.

Accepted evidence:

- EditMode 1215 PASS;
- PlayMode 70 PASS;
- total 1285 PASS;
- Phase 1 PlayMode 17/17 PASS;
- technical/manual Golden Path PASS;
- Windows x64 Development Build and external execution PASS;
- application version 0.0.17.

## Representative asset integration

The working copy subsequently introduced:

- FBX import and prefab generation tool;
- Architecture, Furniture and Product model/prefab libraries;
- material remapping and LOD/collider handling;
- RepresentativePrefabCatalog;
- runtime catalog reference through RuntimeAssetRegistry;
- furniture/product representative factories;
- representative Store visual builder;
- hotfixes for Unity 6 ModelImporter APIs, `_LOD0` source classification, LOD transfer and runtime catalog loading.

Validation achieved:

- Validate Representative Integration PASS;
- Phase1AssetAuthoringTests PASS;
- full EditMode PASS;
- full PlayMode PASS;
- representative prefabs visibly load at runtime.

## Open visual defects

- walls not aligned to the placement perimeter;
- automatic door panels not reliably vertical and opening directions incorrect;
- initial warehouse shelf appears at an unintended location;
- procedural placement inference is too fragile for final scene composition;
- Operations UI click can propagate to click-to-move.

## Accepted decision

Stop iterative transform hotfixes. Author the initial Store explicitly in `StoreInitial.unity`, with a `StoreInitialEnvironment.prefab`, serialized scene references and a controlled migration away from procedural shell generation.
