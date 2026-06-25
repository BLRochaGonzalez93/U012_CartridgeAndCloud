# Cartridge & Cloud - Current Project Handoff

**Fecha:** 2026-06-25  
**Uso:** pegar o adjuntar al iniciar un nuevo chat de desarrollo  
**Repositorio:** `BLRochaGonzalez93/U012_CartridgeAndCloud`  
**Rama objetivo:** `main`  
**Versión:** `0.0.8`  
**Commit de cierre Sprints 6-7:** este commit; registrar SHA después de publicarlo

## Fuente oficial y registros operativos

- La baseline publicada `Documentation/00_Official_Baseline/v0.5` permanece
  histórica e inmutable.
- El estado operativo posterior está documentado en
  `Documentation/10_Development_Records/Sprint_06` y `Sprint_07`.
- No modificar retrospectivamente la baseline v0.5; publicar una baseline nueva
  únicamente mediante su gate documental específico.

## Estado

Sprints 0-7 CLOSED / PASS.

- EditMode: `259/259 PASS`.
- PlayMode: `41/41 PASS`.
- Baseline automatizada: `300/300 PASS`.
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
- Términos de catálogo con coste entero en céntimos y unidades por caja.
- Pedidos de compra, estados y totales.
- Entregas y cajas deterministas.
- Recepción atómica de cajas hacia el inventario.
- Prevención de recepción duplicada.

## Límites actuales

- No existe todavía UI jugable para pedidos o recepción.
- No existe todavía interacción física de entrega.
- Los iconos, modelos y prefabs finales de producto están diferidos.
- Pedidos y recepción aún no publican movimientos al ledger económico.
- El estado completo de Sprint 6-7 aún no está integrado en SaveRootV1.

## Unity Services

La build de Sprint 7 mostró un aviso de falta de membresía en el proyecto de
Unity Cloud. Se continuó sin Unity Services y la build externa pasó. Ningún
sistema vigente depende de Unity Gaming Services. Revisar la vinculación solo
antes de introducir una dependencia cloud real.

## Próximo trabajo

Abrir **Sprint 8 — Displays & Restocking**.

Debe cubrir, como mínimo:

- definiciones y capacidades de displays;
- asignación de producto a display;
- stock expuesto separado del stock de almacén;
- reposición desde inventario disponible;
- transferencias atómicas sin pérdida ni duplicación;
- representación técnica suficiente para validar el flujo;
- pruebas EditMode prioritarias y regresión completa.

No introducir todavía clientes, checkout, economía completa ni persistencia
integral fuera del alcance aprobado para Sprint 8.

## Reglas de trabajo

1. Leer handoff, Guía Maestra, Roadmap, TDD, Modelo de Datos y QA Plan.
2. Congelar charter y acceptance criteria antes de código.
3. Preservar `.meta` y GUIDs existentes.
4. Mantener Domain/Application independientes de escenas y MonoBehaviour.
5. Usar ScriptableObjects como autoría, no como sustituto del dominio.
6. Exigir compilación, Test Runner, regresión manual y build externa.
7. No declarar PASS sin evidencia completa.
8. Cerrar cada sprint con QA, trazabilidad, closure report y handoff.
