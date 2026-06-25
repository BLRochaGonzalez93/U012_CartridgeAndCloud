# Cartridge & Cloud - Current Project Handoff

**Fecha:** 2026-06-25  
**Uso:** pegar o adjuntar al iniciar un nuevo chat de desarrollo  
**Repositorio:** `BLRochaGonzalez93/U012_CartridgeAndCloud`  
**Rama objetivo:** `main`  
**Versión:** `0.0.9`  
**Commit de cierre Sprint 7:** `b997377eae720e960c02ee5dce53da8b37f66b19`  
**Commit de cierre Sprint 8:** este commit; registrar SHA después de publicarlo

## Fuente oficial y registros operativos

- La baseline publicada `Documentation/00_Official_Baseline/v0.5` permanece
  histórica e inmutable.
- El estado operativo posterior está documentado en
  `Documentation/10_Development_Records/Sprint_06`, `Sprint_07` y `Sprint_08`.
- No modificar retrospectivamente la baseline v0.5; publicar una baseline nueva
  únicamente mediante su gate documental específico.

## Estado

Sprints 0–8 CLOSED / PASS.

- EditMode: `333/333 PASS`.
- PlayMode: `41/41 PASS`.
- Baseline automatizada: `374/374 PASS`.
- Regresión manual: PASS.
- Windows x64 Development build: PASS.
- Ejecución externa: PASS.
- Defectos S0/S1 abiertos: ninguno comunicado.

## Implementado

### Fundación previa

- Bootstrap, MainMenu, Store, TestLab y ApplicationRoot.
- Scene flow y contextos UI/Gameplay.
- Save skeleton mínimo.
- Click-to-move, cámara orbital y zoom.
- Grid, placement, ocupación, retirada y validación de acceso.
- Store 10x15 m con baseline vacía reproducible.

### Sprint 6 — Product & Inventory Core

- Definiciones e identificadores de producto puros en C#.
- Registro determinista de productos.
- Cantidades, capacidades, stacks y contenedores.
- Mutaciones y transferencias atómicas.
- Conservación de unidades y fallos sin mutación parcial.

### Sprint 7 — Supplier Orders & Receiving

- ProductDefinitionAsset y ProductCatalogAsset.
- SupplierDefinitionAsset y SupplierCatalogAsset.
- Seis productos técnicos y dos proveedores técnicos.
- Pedidos de compra, estados, totales, entregas y cajas.
- Recepción atómica de cajas hacia inventario.
- Prevención de recepción duplicada.

### Sprint 8 — Displays & Restocking

- DisplayDefinitionAsset y DisplayCatalogAsset.
- Tres definiciones técnicas de display.
- Instancias runtime con inventario Display aislado.
- Un producto asignado por display y restricciones opcionales de categoría.
- Capacidad total y límite visible separados.
- Reposición exacta y hasta capacidad desde Storage o Transit.
- Devolución parcial, devolución completa y limpieza segura de asignación.
- RestockTask domain lifecycle.
- DisplayRuntimeAuthoring como puente de autoría, sin autoridad persistente.
- Sin GameObject persistente por unidad de producto.

## Incidencia resuelta de Sprint 8

La primera importación de Sprint 8 entró en Safe Mode porque la versión de NUnit
del proyecto no exponía `Assert.Multiple`. Se sustituyeron veinte wrappers por
aserciones secuenciales compatibles en siete archivos de tests. La recompilación
y la regresión completa pasaron. No hubo impacto en código de producción.

## Límites actuales

- No existe todavía comportamiento de clientes ni spawning de perfiles.
- No existe UI jugable definitiva para pedidos, recepción o displays.
- No existe interacción física completa de entregas o reposición.
- Los iconos, modelos y prefabs finales están diferidos.
- Pedidos, displays y ventas aún no publican una economía completa.
- El estado completo de Sprints 6–8 aún no está integrado en SaveRootV1.
- Los displays técnicos tienen autoría y lógica, pero no una presentación final.

## Unity Services

Existe un aviso conocido de membresía del proyecto de Unity Cloud durante
algunas builds. Las builds se han continuado sin Unity Services y han pasado.
Ningún sistema vigente depende de Unity Gaming Services. Revisar la vinculación
solo antes de introducir una dependencia cloud real.

## Próximo trabajo

Abrir **Sprint 9 — Customer Profiles & Spawning**.

Debe cubrir, como mínimo:

- identificadores y definiciones de perfiles de cliente;
- arquetipos técnicos autorables;
- reglas de spawning y límites de población;
- estado y ciclo de vida inicial del cliente;
- puntos de entrada y salida de Store;
- integración técnica con navegación y escena sin introducir todavía compra
  completa, reservas, checkout o economía;
- pruebas EditMode prioritarias, smoke PlayMode y regresión completa.

No introducir todavía shopping completo, reservations, queue, checkout,
economía completa ni persistencia integral fuera del alcance aprobado para
Sprint 9.

## Reglas de trabajo

1. Leer handoff, Guía Maestra, Roadmap, TDD, Modelo de Datos y QA Plan.
2. Congelar charter y acceptance criteria antes de código.
3. Preservar `.meta` y GUIDs existentes.
4. Mantener Domain/Application independientes de escenas y MonoBehaviour.
5. Usar ScriptableObjects como autoría, no como sustituto del dominio.
6. Evitar un GameObject persistente por unidad de inventario.
7. Exigir compilación, Test Runner, regresión manual y build externa.
8. No declarar PASS sin evidencia completa.
9. Cerrar cada sprint con QA, trazabilidad, closure report y handoff.
