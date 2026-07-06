# U012 — Resultado de validación estructural estática

**Estado:** PASS
**Raíz validada:** `/mnt/data/U012_rebuild`
**Comprobaciones:** 42 PASS · 0 FAIL · 1 INFO

| Categoría | Comprobación | Estado | Detalle |
|---|---|---:|---|
| baseline | Unity project roots | **PASS** | Assets, Packages, ProjectSettings |
| baseline | Unity version | **PASS** | m_EditorVersion: 6000.3.18f1 m_EditorVersionWithRevision: 6000.3.18f1 (5ebeb53e4c07) |
| baseline | AI Navigation package | **PASS** | 2.0.13 |
| inventory | Project script inventory | **PASS** | 361 scripts |
| inventory | Project prefab inventory | **PASS** | 70 prefabs |
| inventory | Project scene inventory | **PASS** | 4 scenes |
| integrity | Unity file/meta pairs | **PASS** | complete |
| integrity | Unique Unity GUIDs inside Assets | **PASS** | unique |
| integrity | No references to archived migration scripts | **PASS** | none |
| integrity | External/package script references | **INFO** | 17 unresolved through Assets metadata; package references are expected |
| integrity | Serialized local fileID references | **PASS** | resolved |
| prefabs | Identity root transforms | **PASS** | all project prefabs |
| prefabs | Architecture physical contracts | **PASS** | count=24, roles={0: 7, 1: 5, 2: 11, 3: 1} |
| prefabs | Products physical contracts | **PASS** | count=10, roles={4: 10} |
| prefabs | Expansions physical contracts | **PASS** | count=19, roles={5: 19} |
| prefabs | Furniture physical contracts | **PASS** | count=8, roles={3: 8} |
| prefabs | Architecture role classification | **PASS** | {0: 7, 1: 5, 2: 11, 3: 1} |
| furniture | Collision, obstacle and functional anchors | **PASS** | 8 complete prefabs |
| navigation | Environment navigation authority | **PASS** | required hierarchy=True; three collider-backed walkable sources=True; one NavMeshSurface=True; authored navigation component=True; CollectObjects.Children=True; PhysicsColliders geometry=True; no legacy TechnicalColliders=True |
| navigation | Scene navigation cleanup | **PASS** | PlacementSurfaceGrid retained; duplicate entry/walkable objects absent |
| navigation | Single runtime navigation source | **PASS** | authored Navigation subtree only; no global scans or economy subscriptions |
| door | Self-contained automatic door prefab | **PASS** | colliders=3, names complete=True |
| door | No global sensor fallback | **PASS** | trigger-owned sensor is mandatory |
| actors | Seven complete actor wrapper prefabs | **PASS** | count=7, roles={0: 2, 1: 2, 2: 2, 3: 1} |
| actors | Technical player presence is authored | **PASS** | scene-owned CharacterPresence; no runtime registration required |
| actors | Player visual physical components are disabled | **PASS** | visual collider and NavMeshAgent cannot duplicate TechnicalPlayer physics |
| actors | Legacy generic character prefabs removed | **PASS** | removed |
| catalogs | Direct actor and delivery references | **PASS** | actors=['player-clerk-male-01', 'employee-female-01', 'employee-male-01', 'supplier-female-01', 'supplier-male-01', 'customer-female-01', 'customer-male-01'], deliveryPrefab=True |
| catalogs | Future expansions excluded from StoreInitial runtime | **PASS** | assets retained under Prefabs/Expansions; runtime list empty |
| delivery | Idempotent delivery presentation | **PASS** | registry=True; presenter=True; correlation=True; authoredView=True |
| cleanup | Experimental builders/loaders removed | **PASS** | removed |
| cleanup | No primitive fallback in production | **PASS** | none |
| cleanup | Authored binder and strict prefab factory | **PASS** | binder=True; factory=True |
| cleanup | Legacy blockout settings removed | **PASS** | StoreRuntimeSettings uses authored-store terminology |
| cleanup | One-shot editor migrations archived | **PASS** | archived=['ProjectAssetOrganizationMigration.cs', 'ProjectAssetOrganizationPlayModeRepair.cs', 'ProjectAssetOrganizationPrefabFixtureRepair.cs', 'RepresentativeAssetIntegrationTool.cs', 'RepresentativeRuntimeCatalogLinkRepair.cs', 'StoreInitialSceneAuthoringTool.cs'] |
| cleanup | Synty installer is explicit and contract-safe | **PASS** | manual execution only; regenerated wrappers keep collider, agent and authoring profile |
| tests | Scenario runners isolated to TestSupport | **PASS** | runtime=0, testSupport=7 |
| tests | Read-only structural validation assets | **PASS** | editor validator and EditMode contract suite present |
| build | Production scene list | **PASS** | production enabled=True; TestLab enabled=0 |
| compile-static | Assembly definition JSON | **PASS** | 12 assemblies |
| compile-static | Required assembly references | **PASS** | present |
| compile-static | C# lexical balance | **PASS** | balanced |
| compile-static | Unique declared type names | **PASS** | unique |

Esta validación comprueba estructura serializada, referencias, contratos de prefab y coherencia de código. No sustituye la importación, compilación, pruebas EditMode/PlayMode, bake de NavMesh ni build dentro de Unity.
