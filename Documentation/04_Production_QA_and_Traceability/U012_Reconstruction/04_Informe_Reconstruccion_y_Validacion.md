# U012 — Reconstrucción estructural y validación

**Proyecto:** Cartridge & Cloud  
**Baseline de entrada:** `U012_CartridgeAndCloud.zip`  
**SHA-256 de la baseline:** `34f2cf14f9a9eb323c38a6cf366474b4a9a3c09d619ea057c702050fd9138a68`  
**Unity:** `6000.3.18f1`  
**AI Navigation:** `2.0.13`  
**Fecha de cierre:** 2026-07-06

## 1. Método aplicado

La baseline original se mantuvo inmutable. La reconstrucción se realizó sobre una copia, con revisión en cuatro niveles:

1. auditoría inicial e inventario antes de modificar;
2. validación dirigida después de cada bloque funcional;
3. auditoría integral previa al empaquetado;
4. reauditoría con el mismo validador sobre el proyecto extraído del ZIP candidato y, externamente, sobre el ZIP final exacto.

No se conservaron fixes incrementales paralelos. Cada responsabilidad quedó asociada a una única autoridad serializada o runtime.

## 2. Resultado de la reconstrucción

### Prefabs y autoría

- Los prefabs de arquitectura, producto, expansión y mobiliario declaran su responsabilidad física mediante `PrefabPhysicalContractAuthoring`.
- Los ocho muebles poseen jerarquías `Collision` y `Anchors`, colisión serializada y obstáculo de navegación autorado.
- Los siete actores actuales poseen `CapsuleCollider`, `NavMeshAgent`, `ActorPrefabAuthoring`, `CharacterPresence` y locomoción serializados.
- La puerta automática es autosuficiente: paneles, colliders, sensor, Rigidbody y anchors viven en el prefab.

### Navegación

- `StoreInitialEnvironment.prefab` es la única autoridad de navegación de la tienda inicial.
- Su jerarquía `Navigation` contiene interior, exterior y transición de entrada.
- Existe un único `NavMeshSurface`, limitado a hijos y geometría de colliders físicos.
- El controlador no busca colliders globales, no crea superficies y no rebakea por cambios económicos.
- Los planos técnicos duplicados de entrada se retiraron de la escena; `PlacementSurfaceGrid` se conserva únicamente para colocación.

### Runtime y entrega

- `AuthoredStoreRuntimeBinder` enlaza autoría existente; no construye geometría ni añade identidad al jugador.
- `StorePrefabFactory` y `CharacterPrefabFactory` validan y crean únicamente prefabs catalogados.
- La presentación de entregas es idempotente por `orderId` y usa `SupplierDeliveryView.prefab` y `ReceivingCrate.prefab`.
- No quedan primitivas, placeholders de proveedor, builders experimentales ni loaders por rutas en producción.

### Limpieza y pruebas

- Los tres prefabs genéricos legacy de personajes fueron retirados.
- Las herramientas one-shot se archivaron fuera de `Assets`.
- Los scenario runners se aislaron en `Tests/TestSupport`.
- `TestLab` permanece disponible, pero está deshabilitado en Build Settings.
- Se añadieron un validador Editor read-only, tests EditMode de contratos y el validador estático reproducible `Tools/Validation/u012_static_validate.py`.

## 3. Validación previa al empaquetado

Resultado: **42 PASS · 0 FAIL · 1 INFO**.

La única entrada informativa corresponde a 17 GUID de scripts externos/paquetes que no se resuelven mediante metas dentro de `Assets`; no se clasifican como referencias rotas del proyecto. Todas las referencias locales `fileID`, pares archivo/meta y GUID propios pasan.

Inventario final validado:

- 361 scripts C# propios bajo `Assets/_Project`;
- 70 prefabs propios;
- 4 escenas propias;
- 12 definiciones de ensamblado válidas.

## 4. Limitación de validación

Este entorno no dispone del ejecutable de Unity ni de un compilador C# compatible con el proyecto. Por tanto, esta entrega **no afirma** haber ejecutado:

- importación/reimportación en Unity;
- compilación real de assemblies;
- pruebas EditMode o PlayMode;
- bake visual del NavMesh;
- build de Windows/Steam.

La validación realizada cubre estructura de proyecto, YAML serializado, GUID/meta/fileID, contratos de prefab, dependencias asmdef, eliminación de caminos legacy, coherencia léxica de C# y reproducibilidad del paquete. La primera apertura en Unity debe ejecutar los tests incluidos y un smoke test de StoreInitial antes de publicar una build.

## 5. Evidencias incluidas

- auditoría de baseline;
- registro inicial de depuración y consolidación;
- matriz de prefabs inicial;
- registro punto por punto de la reconstrucción;
- resultados JSON y Markdown de validación prepaquete;
- manifiesto SHA-256 del contenido entregado;
- script exacto utilizado para la revisión estática.

## 6. Reauditoría del paquete

**Estado del ZIP candidato extraído:** **PASS — 42 PASS · 0 FAIL · 1 INFO**. El proyecto se extrajo en una carpeta limpia y se ejecutó desde esa copia el mismo `u012_static_validate.py` incluido en el paquete. El inventario y los resultados coincidieron con la validación prepaquete.  
**Estado del ZIP final exacto:** se documenta en el informe externo generado después de cerrar el archivo, para no modificar el propio ZIP tras auditarlo.
