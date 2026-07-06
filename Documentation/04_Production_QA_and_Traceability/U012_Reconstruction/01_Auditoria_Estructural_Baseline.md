# U012 — Auditoría estructural completa previa a modificación

**Proyecto:** Cartridge & Cloud  
**Baseline auditada:** `U012_CartridgeAndCloud.zip`  
**SHA-256:** `34f2cf14f9a9eb323c38a6cf366474b4a9a3c09d619ea057c702050fd9138a68`  
**Unity:** `6000.3.18f1`  
**AI Navigation:** `2.0.13`  
**Estado:** auditoría estática terminada; no se ha modificado ningún archivo del proyecto.

---

## 1. Dictamen ejecutivo

La baseline no necesita otro parche puntual. Necesita una normalización controlada de autoría.

La arquitectura de capas de código es razonablemente sana: no se han detectado dependencias impropias graves desde Domain/Application hacia Unity. El problema principal se concentra en la frontera entre **prefabs, escena, navegación y composición runtime**:

1. Los prefabs propios son mayoritariamente envoltorios visuales incompletos.
2. Colliders, obstáculos, puntos de interacción y correcciones de pivote se fabrican en runtime.
3. El NavMesh usa geometría física global y una superficie técnica suelta en la escena que mantiene las dimensiones interiores antiguas.
4. Existen herramientas históricas de migración/reparación capaces de volver a generar o parchear contenido ya consolidado.
5. Hay catálogos y rutas de personajes duplicados o divergentes.
6. El flujo de proveedor contiene una instancia placeholder inactiva y carece de una identidad de entrega idempotente; el análisis estático no demuestra por sí solo el origen exacto de los dos proveedores visibles.

**Decisión recomendada:** reconstruir la autoría técnica dentro de los prefabs, declarar una sola fuente de navegación y retirar los fallbacks solo cuando todos los prefabs hayan superado validación. Las fases deben ejecutarse en una rama de trabajo, pero producir **un único paquete final coherente**, no entregas parciales con compatibilidad acumulada.

---

## 2. Alcance e inventario

| Elemento | Cantidad / estado |
|---|---:|
| Scripts C# propios | 360 |
| Prefabs propios auditados | 72 |
| Escenas propias | 4 |
| Prefabs de arquitectura | 24 |
| Prefabs de mobiliario | 8 |
| Prefabs de productos | 10 |
| Prefabs de expansiones | 19 |
| Prefabs de personajes en `Resources` | 7 |
| Prefabs genéricos de personajes | 3 |
| Prefab compuesto de tienda | 1 |
| Prefabs con collider directo | 1 de 72 |
| Escenas incluidas en Build Settings | Bootstrap, MainMenu, StoreInitial y TestLab |

Los paquetes de Synty y demás contenido de terceros se han considerado dependencias externas. No se propone reestructurar sus fuentes; cualquier adaptación debe vivir en prefabs envoltorio propios.

### Metodología

- Inventario de scripts, prefabs, escenas, catálogos y configuración de paquetes.
- Resolución de GUID y referencias serializadas.
- Inspección YAML de los 72 prefabs propios y las 4 escenas.
- Lectura de los controladores de navegación, composición, personajes, visuales, blockout y herramientas de Editor.
- Comparación entre la autoría documentada y el comportamiento implementado.

### Limitación

Esta es una auditoría estática. No se abrió Unity, no se horneó NavMesh en Editor y no se ejecutaron pruebas PlayMode/EditMode. Los hallazgos de estructura son concluyentes; el origen exacto de fenómenos exclusivamente runtime, como la visualización de dos proveedores, debe confirmarse con instrumentación durante la fase de implementación.

---

## 3. Hallazgos críticos

### 3.1. La fuente actual del NavMesh es implícita y conserva el volumen interior antiguo

`DynamicStoreNavMeshController` crea o configura un `NavMeshSurface` en runtime con estas características:

- `CollectObjects.Volume`.
- Geometría basada en `PhysicsColliders`.
- Máscara de capas global.
- Volumen calculado a partir de colliders encontrados en la escena.
- Reconstrucción asociada tanto a cambios de mobiliario como a `StoreOperationsFacade.StateChanged`.

En `StoreInitial.unity` existe un objeto técnico `WalkableFloor` con un `BoxCollider` aproximadamente centrado en `(0, -0.1, 0)` y escalado a `(10, 0.2, 15)`. Ese volumen representa la planta interior histórica. El exterior visual no dispone de una geometría física equivalente que entre en el cálculo. Por eso el interior se incluye aunque sus módulos visuales no tengan collider y el exterior queda fuera.

También existe `EntranceApron`, pero no constituye una superficie exterior completa. El prefab `EntranceThreshold` aparece escalado en escena para cubrir parte del acceso, lo que evidencia que el acceso se ha resuelto por ajuste visual, no mediante una definición técnica única.

#### Consecuencia

Aumentar solo el `WalkableFloor` sería otro arreglo local: podría resolver temporalmente el área exterior, pero mantendría la navegación dependiente de colliders dispersos, capas globales y rebakes indiscriminados.

#### Decisión obligatoria

Crear una única autoridad de navegación dentro de `StoreInitialEnvironment.prefab`:

```text
StoreInitialEnvironment
├── Visual
├── Collision
├── Navigation
│   ├── WalkableBase_Interior
│   ├── WalkableBase_Exterior
│   ├── EntranceTransition
│   └── NavMeshSurface
└── Anchors
```

- El interior, umbral y exterior deben formar una continuidad explícita.
- El `NavMeshSurface` debe recolectar hijos o capas técnicas dedicadas, no todos los colliders de la escena.
- Las paredes estáticas deben quedar incluidas por contrato.
- El mobiliario dinámico debe usar `NavMeshObstacle` con carving o una actualización agrupada al confirmar colocación.
- No se debe rebakear por cambios económicos, recepción de pedidos u otros eventos sin impacto geométrico.

---

### 3.2. Los prefabs no son actualmente la autoridad física y funcional

Los 72 prefabs tienen raíces normalizadas en posición cero y escala uno, lo cual es una buena envolvente. Sin embargo, 71 no contienen collider directo. Esto no significa que todos necesiten uno, pero sí confirma que la física se está resolviendo fuera del prefab.

`StoreVisualPrefabFactory` realiza en runtime tareas que deberían quedar autoradas:

- Instancia el visual representativo.
- Compensa escalas.
- Calcula bounds de renderers.
- Alinea el visual a una base técnica.
- Añade offsets de suelo.
- Crea colliders.
- Crea `NavMeshObstacle`.
- Genera puntos de espera de clientes.
- Genera puntos de checkout.
- Genera spots de exposición de productos.
- Añade marcadores auxiliares.

Esta fábrica es el núcleo de la deuda actual: permite que un prefab incompleto parezca funcional, pero oculta errores de pivote y reparte el contrato entre el asset y el código.

#### Contrato de prefab recomendado

Todos los prefabs interactivos propios deben usar una estructura equivalente:

```text
PrefabRoot                # posición 0, rotación 0, escala 1; pivote en base-centro
├── VisualRoot            # arte y LOD; sin lógica física implícita
├── Collision             # colliders simples y editables
├── Navigation            # obstacle/modifier cuando proceda
└── Anchors
    ├── InteractionSpot
    ├── CustomerStand
    ├── EmployeeStand
    ├── RestockSpot
    ├── CarrySocket
    └── DisplaySpots
```

No todos los nodos se aplican a todos los tipos. La regla es que cualquier punto funcional usado por runtime debe existir serializado en el prefab o en un descriptor explícito del propio prefab.

---

### 3.3. Estado por familia de prefabs

| Familia | Diagnóstico | Acción requerida |
|---|---|---|
| Arquitectura, 24 | Sin colliders directos. La física está concentrada en el compuesto de tienda o se genera por herramienta. | Autorizar colisión en módulos bloqueantes o centralizarla deliberadamente en `StoreInitialEnvironment/Collision`; clasificar módulos decorativos como no bloqueantes. |
| Mobiliario, 8 | `StoreFixturePrefabAuthoring` presente, pero sin collider, obstacle ni spots explícitos. | Incorporar collider, navegación y anchors por función. El checkout requiere cola y puestos; estanterías requieren display/restock spots. |
| Productos, 10 | Seis productos principales tienen authoring/marker; packaging es solo visual. | Definir si son display-only o interactivos. No añadir colliders indiscriminadamente; usar trigger/collider simple solo si el gameplay lo exige. |
| Personajes `Resources`, 7 | Tienen presencia/animación; los colliders y agentes se añaden con valores hardcoded en runtime. Proveedores sí contienen `CarrySocket`. | Autorizar `CapsuleCollider`, perfil de agente y anchors en wrappers propios; mantener Synty intacto. |
| Personajes genéricos, 3 | Sin referencias GUID entrantes y fuera de `Resources`; pertenecen al sistema antiguo de authoring. | Eliminar tras migrar cualquier dato útil al catálogo único. |
| Expansiones, 19 | Envueltos y catalogados, pero fuera del núcleo StoreInitial. | No eliminar automáticamente. Mover a contenido futuro o a un paquete separado y sacarlos del catálogo runtime del vertical slice si no se usan. |
| StoreInitialEnvironment, 1 | Único prefab con colliders; además contiene anchors principales. | Convertirlo en autoridad de entorno, colisión estática, navegación y accesos. |

### Observaciones específicas

- `AutomaticDoor.prefab` contiene `AutomaticDoorParts`, pero no el contrato físico completo ni el trigger dentro del propio prefab. Debe ser autosuficiente: controlador, trigger, hojas, colliders y estados/anchors.
- El sistema de corrección por bounds demuestra que los pivotes reales de los visuales no son fiables. Cada envoltorio propio debe fijar base-centro y +Z como frente; después debe eliminarse la corrección runtime.
- `ReceivingCrate.prefab` existe, pero la secuencia de proveedor crea una caja primitiva. La entrega debe usar el prefab real con sockets de transporte y depósito.
- Los módulos de luces, cartelería, zonas y otros elementos decorativos no deben recibir colliders por defecto.

La matriz completa por prefab se entrega como anexo CSV.

---

### 3.4. Herramientas históricas con capacidad de reintroducir deuda

Se han identificado varias herramientas de Editor de migración, reparación o integración acumuladas:

- `SyntyCharacterIntegrationInstaller.cs`
- `ProjectAssetOrganizationMigration.cs`
- `ProjectAssetOrganizationPlayModeRepair.cs`
- `ProjectAssetOrganizationPrefabFixtureRepair.cs`
- `RepresentativeAssetIntegrationTool.cs`
- `RepresentativeRuntimeCatalogLinkRepair.cs`
- `StoreInitialSceneAuthoringTool.cs`

`StoreInitialSceneAuthoringTool.cs` conserva rutas antiguas sin el segmento `Architecture/Modular`, reconstruye la planta interior histórica y puede sobrescribir una autoría directa posterior. No debe ejecutarse sobre la nueva baseline.

`SyntyCharacterIntegrationInstaller.cs` está marcado para inicialización automática de Editor. Una herramienta capaz de reescribir prefabs o controladores no debe ejecutarse implícitamente al cargar el proyecto.

#### Decisión

Después de la reconstrucción:

- Conservar únicamente un **importador explícito e idempotente** para wrappers de terceros, si sigue siendo necesario.
- Crear un **validador de prefabs** que nunca modifique assets por sí solo.
- Archivar las herramientas one-shot fuera de `Assets` o eliminarlas del paquete de producción.
- Prohibir herramientas que parcheen texto fuente como mecanismo de mantenimiento habitual.

---

### 3.5. Fallbacks y blockout siguen dentro del runtime de producción

`StoreBlockoutBuilder` combina dos responsabilidades incompatibles:

1. Vincular una escena autorada mediante `BindAuthoredScene`.
2. Reconstruir proceduralmente shell, zonas, iluminación y placeholders mediante `Build`.

`StoreVisualBuilder.cs` sirve al camino procedural. `StoreBlockoutVisualFactory.cs` intenta cargar prefabs, pero si algo falla crea cubos, cápsulas y otros primitivos. El fallback es útil durante prototipado, pero impide detectar un prefab roto y mantiene dos representaciones posibles del mismo contenido.

#### Decisión

- Extraer `BindAuthoredScene` a un servicio con nombre de producción, por ejemplo `AuthoredStoreRuntimeBinder`.
- Eliminar el camino procedural de `StoreBlockoutBuilder`.
- Eliminar `StoreVisualBuilder.cs` cuando deje de tener consumidores.
- Retirar `StoreBlockoutVisualFactory.cs` una vez que el catálogo y todos los prefabs requeridos validen correctamente.
- En producción, la ausencia de un prefab obligatorio debe producir un error de validación claro, no un cubo silencioso.

---

### 3.6. Catálogos y carga de personajes divergentes

El proyecto mantiene al menos dos modelos conceptuales:

- Catálogos serializados con rutas legacy como `Characters/Employee`, `Characters/Customer` y `Characters/Supplier`.
- `CharacterPrefabLoader` con rutas `Resources` hardcoded hacia los wrappers `PF_CC_*` actuales.

Los tres prefabs genéricos no están bajo `Resources` y no presentan referencias GUID entrantes. Por tanto, las rutas legacy no describen el contenido realmente cargado.

#### Decisión

Consolidar en un único `ActorPrefabCatalog` con referencias directas a prefab —o Addressables cuando se adopten— y un único `CharacterPrefabFactory`.

El catálogo debe definir por rol:

- Prefab.
- Perfil de locomoción/agente.
- Cápsula física.
- Variante visual.
- Sockets requeridos.

Después de migrar:

- Eliminar las rutas mágicas de `Resources`.
- Eliminar los tres prefabs genéricos legacy.
- Evitar que tests y runtime utilicen catálogos distintos.

---

### 3.7. Doble proveedor: conclusión de la auditoría estática

`StoreCharacterLoopController.Configure()` crea un empleado y también llama a `SpawnSupplierPlaceholder()`. Ese proveedor placeholder se instancia y se desactiva inmediatamente. No se ha localizado una reactivación posterior.

La entrega real se inicia al recibir feedback `OrderReceived` y utiliza una guarda global `_supplierDeliveryRunning`. `ReceiveOrder` emite un evento de pedido recibido y un evento de gasto; solo el primero debería iniciar la secuencia.

Por tanto:

- El placeholder es código muerto y debe retirarse.
- El análisis estático no demuestra que ese objeto inactivo sea el segundo proveedor visible.
- Las causas restantes más probables son una duplicación de `StoreCharacterLoopController`/composición, una doble suscripción efectiva o dos secuencias para una misma entrega.

#### Corrección estructural propuesta

- Una sola instancia validada de `StoreCharacterLoopController`.
- Una identidad `deliveryId`/`orderId` obligatoria.
- Diccionario de presentaciones activas para garantizar idempotencia por entrega.
- Un único prefab `SupplierDeliveryView` que agrupe proveedor, crate y sockets.
- Instrumentación de creación/destrucción con IDs durante las pruebas.
- Test PlayMode: una recepción confirmada produce exactamente una entrega visual y un solo proveedor activo.

No debe introducirse otro booleano o búsqueda global como parche.

---

### 3.8. Escenas y configuración de build

`EditorBuildSettings.asset` incluye `TestLab.unity` como cuarta escena habilitada. `TestLab` debe conservarse como laboratorio, pero no formar parte del build de producción.

`StoreInitialSceneContext` sí constituye una base válida: contiene referencias explícitas a superficie de colocación, shell, spawns, zonas, puerta, cámara, entorno, mobiliario y roots dinámicos. Debe conservarse y endurecerse su validación; no debe reemplazarse por búsquedas globales.

---

## 4. Fuente única de verdad propuesta

| Dominio | Autoridad única |
|---|---|
| Entorno StoreInitial | `StoreInitialEnvironment.prefab` |
| Pivote y dimensiones de un objeto | Root y descriptor del prefab |
| Colisión estática | `Collision` del prefab o del compuesto de entorno, de forma explícita |
| Navegación estática | `Navigation` + un solo `NavMeshSurface` del entorno |
| Bloqueo dinámico | `NavMeshObstacle`/modifier del prefab colocado |
| Puntos funcionales | `Anchors` serializados dentro del prefab |
| Personajes | Un catálogo de actores y una fábrica |
| Productos/mobiliario | Catálogo con referencias directas y prefabs completos |
| Enlace escena-runtime | `StoreInitialSceneContext` + binder de escena autorada |
| Validación | Herramientas read-only y tests; nunca fallbacks silenciosos |

---

## 5. Registro de eliminación, archivo y consolidación

### 5.1. Eliminar después de completar su migración

| Elemento | Motivo / condición |
|---|---|
| `Assets/_Project/Prefabs/Characters/Customers/Customer.prefab` | Prefab genérico legacy sin referencias útiles; eliminar tras catálogo único. |
| `Assets/_Project/Prefabs/Characters/Employees/Employee.prefab` | Igual. |
| `Assets/_Project/Prefabs/Characters/Suppliers/Supplier.prefab` | Igual. |
| `StoreVisualBuilder.cs` | Solo sostiene el camino procedural antiguo. |
| Parte procedural de `StoreBlockoutBuilder.cs` | La tienda debe estar autorada, no reconstruida en runtime. |
| `StoreBlockoutVisualFactory.cs` | Eliminar cuando todos los prefabs y catálogos sean obligatorios y válidos. |
| `SpawnSupplierPlaceholder()` y su estado asociado | Instancia inactiva sin responsabilidad de producción. |
| Generación de crate con `GameObject.CreatePrimitive` | Sustituir por `ReceivingCrate.prefab`. |
| Rutas legacy embebidas `Characters/Employee`, `Characters/Customer`, `Characters/Supplier` | No corresponden al contenido actual. |
| Siete `*ScenarioRunner.cs` bajo Runtime/Development | Mover a TestSupport o integrar en tests; no deben compilar en el runtime de producción. |
| `TestLab` de Build Settings | Quitar del build, sin borrar la escena. |

### 5.2. Archivar fuera de `Assets` o retirar del paquete final

| Herramienta | Tratamiento |
|---|---|
| `ProjectAssetOrganizationMigration.cs` | One-shot histórica; conservar solo en historial/archivo. |
| `ProjectAssetOrganizationPlayModeRepair.cs` | Parche de código/test; retirar. |
| `ProjectAssetOrganizationPrefabFixtureRepair.cs` | Reparación puntual; retirar tras normalización. |
| `RepresentativeRuntimeCatalogLinkRepair.cs` | Sustituir por validador no mutante. |
| `RepresentativeAssetIntegrationTool.cs` | Descomponer en importador explícito + validador; archivar el monolito. |
| `StoreInitialSceneAuthoringTool.cs` | No ejecutar; sustituir por validación del prefab compuesto. |
| `SyntyCharacterIntegrationInstaller.cs` | Eliminar autoejecución; conservar solo si se transforma en importador manual idempotente. |

### 5.3. Consolidar

| Orígenes actuales | Destino recomendado |
|---|---|
| `CharacterPrefabLoader` + rutas hardcoded + catálogos legacy | `ActorPrefabCatalog` + `CharacterPrefabFactory` |
| `StoreBlockoutBuilder.BindAuthoredScene` | `AuthoredStoreRuntimeBinder` |
| `StoreVisualPrefabFactory` + authoring incompleto | Prefabs completos + `StorePrefabValidator` |
| `WalkableFloor` suelto + colliders globales + surface runtime | `StoreInitialEnvironment/Navigation` + un solo surface |
| Placeholder, secuencia de proveedor y crate primitiva | `SupplierDeliveryPresenter` idempotente + prefab de entrega |
| Herramientas de reparación/integración | Importadores manuales idempotentes + validadores read-only |
| Scenario runners de producción | Ensamblado de tests/TestSupport |

### 5.4. Conservar y reforzar

- Separación Domain/Application/Infrastructure/Presentation/Runtime.
- `StoreInitialSceneContext` como contrato explícito.
- `StoreRuntimeCompositionRoot` como punto de composición, reduciendo búsquedas y componentes creados dinámicamente.
- `StoreInitialEnvironment.prefab` como compuesto autoritativo.
- Wrappers propios sobre Synty; no modificar assets de terceros.
- Tests actuales que expresen comportamiento válido, adaptándolos al nuevo contrato.

### 5.5. No eliminar automáticamente

Los 19 prefabs de expansiones y varios prefabs de packaging tienen pocas referencias directas porque están vinculados principalmente a catálogos. No deben eliminarse solo por su conteo de referencias. Deben clasificarse como:

- Núcleo del vertical slice.
- Contenido futuro conservado fuera del catálogo runtime.
- Asset descartado con aprobación explícita de diseño.

---

## 6. Orden de trabajo propuesto para un único paquete coherente

Estas son puertas internas de validación, no releases ni fixes acumulativos.

### Puerta A — Congelar y proteger la baseline

- Conservar el ZIP y su hash.
- Crear una rama de reconstrucción.
- Deshabilitar temporalmente las herramientas automáticas/mutantes.
- Capturar compilación y tests iniciales en Unity antes de editar.

### Puerta B — Normalizar todos los prefabs

1. Definir convención de pivote, frente, escala y jerarquía.
2. Corregir primero arquitectura y `StoreInitialEnvironment`.
3. Corregir mobiliario y sus spots.
4. Corregir personajes y perfiles de agente.
5. Corregir productos según su interacción real.
6. Ejecutar un validador read-only sobre los 72 prefabs.

No se toca el runtime para compensar prefabs fallidos durante esta puerta.

### Puerta C — Unificar navegación

- Mover la superficie caminable interior/exterior al compuesto de entorno.
- Establecer layers/children explícitos.
- Eliminar `WalkableFloor` y otros proxies sueltos una vez migrados.
- Cambiar rebakes globales por carving o actualizaciones geométricas agrupadas.
- Validar rutas completas entre todos los anchors requeridos.

### Puerta D — Simplificar runtime

- Sustituir carga y catálogos de personajes.
- Sustituir generación visual por instanciación de prefabs completos.
- Retirar blockout/fallbacks.
- Hacer idempotente la entrega del proveedor.
- Mantener un único composition root y un único controller por responsabilidad.

### Puerta E — Limpieza y validación integral

- Mover escenarios a tests.
- Retirar herramientas históricas.
- Quitar TestLab del build.
- Ejecutar EditMode, PlayMode, smoke test manual y build Development/Release.
- Generar un solo ZIP final con manifiesto y checksums.

---

## 7. Criterios de aceptación

### Prefabs

- 100 % de raíces propias con posición/rotación neutras y escala uno.
- Todos los objetos colocables apoyan en `y=0` sin corrección por bounds en runtime.
- Todo punto funcional consumido por código existe dentro del prefab y está validado.
- Ningún collider bloqueante se crea silenciosamente en runtime.
- Los elementos decorativos están marcados explícitamente como no físicos.

### Navegación

- Existe exactamente un `NavMeshSurface` autoritativo para StoreInitial.
- El área navegable cubre interior, puerta, umbral y exterior previsto.
- Todos los anchors obligatorios proyectan en NavMesh.
- Hay ruta completa desde spawns exteriores a receiving, checkout, backroom y puntos de cliente.
- Colocar/mover mobiliario actualiza solo lo necesario.
- Eventos económicos no disparan reconstrucción del NavMesh.

### Runtime

- Una recepción crea exactamente una secuencia visual de proveedor.
- No quedan placeholders procedurales ni primitivas de fallback en una build de producción.
- No hay rutas de Resources mágicas duplicadas.
- No se usa `FindObjectsByType` para resolver dependencias estructurales que ya conoce el contexto.

### Build y QA

- TestLab fuera del build de producción.
- Cero missing scripts propios.
- Todos los tests automatizados pasan.
- Smoke test manual completo: entrar, comprar, colocar, recibir, vender, guardar/cargar y salir.
- Build reproducible con manifiesto y SHA-256.

---

## 8. Riesgos y controles

| Riesgo | Severidad | Control |
|---|---:|---|
| Ejecutar el authoring tool antiguo y sobrescribir prefabs | Crítica | Deshabilitar/archivar antes de modificar. |
| Retirar fallbacks antes de completar prefabs | Alta | Hacerlo solo después del validador al 100 %. |
| Duplicar navegación al crear un nuevo surface sin retirar el actual | Alta | Test de unicidad de `NavMeshSurface`; migración atómica. |
| Alterar pivotes y romper saves/placements existentes | Alta | Migración de datos o invalidez explícita de saves de desarrollo. |
| Añadir colliders a elementos decorativos y cerrar pasillos | Media | Matriz por tipo y prueba de rutas mínimas. |
| Eliminar expansiones por tener pocas referencias | Media | Clasificación de contenido, no heurística automática. |
| Mantener dos catálogos de actores durante transición | Alta | Corte único en la misma rama antes del paquete final. |
| No reproducir el doble proveedor en tests | Media | Instrumentación por `deliveryId` y test de conteo de instancias. |

---

## 9. Conclusión

La causa del NavMesh exterior y la fragilidad de los objetos tienen el mismo origen: **el prefab no es aún la fuente de verdad completa**. El runtime está compensando autoría ausente mediante colliders, offsets, anchors, obstáculos y modelos de fallback creados al vuelo.

La intervención correcta no es ampliar otro collider ni añadir otra condición. Es trasladar el contrato técnico a los prefabs, centralizar la navegación en `StoreInitialEnvironment`, consolidar catálogos/fábricas y retirar de una sola vez los caminos legacy una vez superadas las puertas de validación.

La baseline debe considerarse **apta para reconstrucción estructural, pero no para nuevos fixes incrementales**.
