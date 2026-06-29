# Cartridge & Cloud - Current Project Handoff

**Fecha:** 2026-06-29  
**Repositorio:** `BLRochaGonzalez93/U012_CartridgeAndCloud`  
**Rama objetivo:** `main`  
**Versión:** `0.0.14`  
**Commit de cierre Sprint 11:** `08ec92ba97f4070399f01023c32f49327a951712`  
**Commit de cierre Sprint 12:** `2d6b846120dc86536c19c12d07e5b515f7baa8fd`  
**Commit de cierre Sprint 13:** este commit; registrar SHA después de publicarlo

## Fuente oficial

- `Documentation/00_Official_Baseline/v0.5` permanece histórica e inmutable.
- El estado operativo posterior se conserva en
  `Documentation/10_Development_Records/Sprint_06` a `Sprint_13`.
- No modificar retrospectivamente la baseline v0.5.

## Estado

Sprints 0–13 `CLOSED / PASS`.

- EditMode: `856/856 PASS`.
- PlayMode: `41/41 PASS`.
- Total automatizado: `897/897 PASS`.
- Regresión manual: PASS.
- Windows x64 Development build: PASS.
- Ejecución externa: PASS.
- Defectos S0/S1: ninguno comunicado.

## Implementado

### Fundación y tienda

- Bootstrap, MainMenu, Store, TestLab y ApplicationRoot.
- Save skeleton.
- Movimiento y cámara.
- Grid, placement, ocupación, retirada y acceso.
- Store 10x15 m.

### Sprint 6 — Product & Inventory Core

- Productos, cantidades, contenedores, transferencias e invariantes.

### Sprint 7 — Supplier Orders & Receiving

- Catálogo, proveedores, pedidos, entregas, cajas y recepción.

### Sprint 8 — Displays & Restocking

- Displays, asignación, capacidad, stock y reposición.

### Sprint 9 — Customer Profiles & Spawning

- Perfiles, llegada, población, navegación técnica y paciencia.

### Sprint 10 — Shopping & Reservations

- Intents, búsqueda, disponibilidad, reservas, carrito y abandono.

### Sprint 11 — Queue & Checkout

- Cola FIFO, estación, checkout, transacciones y cancelación segura.

### Sprint 12 — Day Cycle & Store Closure

- Reloj lógico, estados, cierre, drenaje y resumen operativo.

### Sprint 13 — Economy & Daily Results

- `CurrencyCode` y `Money` exacto mediante `long`.
- Catálogo separado de precios de venta.
- Cotización de carrito previa al checkout.
- Integración económica alrededor del checkout autoritativo.
- Ingresos de checkout.
- Costes de proveedor recibidos.
- Ledger append-only e idempotente.
- Prevención de doble contabilización.
- Resultado bruto diario para días `Closed`.
- Validación de ingresos frente a `CompletedCheckouts`.
- Assets y escenario técnico.
- 100 tests específicos.

## Invariantes vigentes

- Todo importe monetario usa unidades menores enteras.
- No se usa punto flotante en el dominio económico.
- El coste de compra permanece en el catálogo de proveedor.
- El precio de venta está separado de ProductDefinition.
- Una cotización fallida no muta checkout ni inventario.
- El ingreso se registra después de checkout completado.
- Cada fuente se contabiliza como máximo una vez.
- El resultado diario solo se genera para días Closed.
- El conteo de ingresos coincide con CompletedCheckouts.
- Resultado bruto = ingresos − costes recibidos.

## Límites actuales

- No existe persistencia integral del estado operativo.
- No existe recuperación robusta ante archivos dañados o parciales.
- No existen migraciones de saves completas.
- No existen impuestos complejos.
- No existe contabilidad avanzada.
- UI, audio y arte finales siguen diferidos.

## Próximo trabajo

Abrir **Sprint 14 — Save Integration & Recovery**.

Debe cubrir:

- snapshot persistible del estado autoritativo;
- serialización y deserialización deterministas;
- guardado atómico mediante archivo temporal y reemplazo;
- validación de versión y schema;
- recuperación desde backup válido;
- detección de corrupción y datos incompletos;
- persistencia de inventario, proveedores, displays, customers, shopping,
  checkout, day cycle y economy;
- reconstrucción de registros e invariantes;
- prevención de duplicados tras cargar;
- save/load técnico y escenario de recovery;
- tests, validación manual, regresión y build externa.

No introducir todavía:

- cloud save;
- múltiples perfiles de usuario finales;
- cifrado o antitamper;
- migraciones históricas extensas;
- autosave con UI final;
- impuestos complejos;
- UI, audio o arte definitivos.

## Reglas

1. Congelar charter y acceptance criteria.
2. Preservar `.meta` y GUIDs.
3. Mantener Domain/Application independientes de escenas.
4. No serializar referencias de Unity como estado autoritativo.
5. Usar escritura atómica y validación antes de reemplazar el save válido.
6. Mantener recuperación determinista.
7. No declarar PASS sin evidencia completa.
8. Cerrar cada sprint con QA, trazabilidad, cierre y handoff.
