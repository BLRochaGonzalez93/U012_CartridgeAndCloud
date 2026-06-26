# Cartridge & Cloud - Current Project Handoff

**Fecha:** 2026-06-26  
**Repositorio:** `BLRochaGonzalez93/U012_CartridgeAndCloud`  
**Rama objetivo:** `main`  
**Versión:** `0.0.12`  
**Commit de cierre Sprint 9:** `b8ddf15356c73cd7d0c88a32805f0c6ee4058422`  
**Commit de cierre Sprint 10:** `f91c8fda6c1ff1ff9e811717b05fc8fa427707e6`  
**Commit de cierre Sprint 11:** este commit; registrar SHA después de publicarlo

## Fuente oficial

- `Documentation/00_Official_Baseline/v0.5` permanece histórica e inmutable.
- El estado operativo posterior se conserva en
  `Documentation/10_Development_Records/Sprint_06` a `Sprint_11`.
- No modificar retrospectivamente la baseline v0.5.

## Estado

Sprints 0–11 `CLOSED / PASS`.

- EditMode: `613/613 PASS`.
- PlayMode: `41/41 PASS`.
- Total automatizado: `654/654 PASS`.
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

- Cola FIFO y posiciones.
- Entradas Waiting, Called, Processing, Completed y Cancelled.
- Estación Closed, Available y Busy.
- Validación completa de carrito, reservas, display y stock.
- Consumo de stock físico y reservas.
- Carrito vacío y sesión CheckedOut.
- Prevención de doble checkout.
- Cancelación segura y rollback.
- Registro técnico de transacción.
- CheckoutTechnicalScenarioRunner.
- 105 tests específicos.

## Invariantes vigentes

- El inventario físico es autoritativo.
- Las reservas activas respaldan el carrito.
- Solo la primera entrada avanza en la cola.
- Una estación procesa una sola entrada.
- Un carrito completado no puede procesarse otra vez.
- Checkout consume stock y reservas exactamente una vez.
- Los fallos de validación no mutan el estado.

## Límites actuales

- No existe ciclo de día.
- No existe apertura o cierre operativo de tienda.
- No existen precios, ingresos, impuestos o ledger.
- No existen reportes de fin de día.
- No existe persistencia integral.
- UI, audio y arte finales siguen diferidos.

## Próximo trabajo

Abrir **Sprint 12 — Day Cycle & Store Closure**.

Debe cubrir:

- reloj lógico del día;
- estados BeforeOpen, Open, Closing y Closed;
- apertura y cierre controlados;
- bloqueo de nuevos spawns durante Closing;
- drenaje de clientes activos;
- cierre seguro de cola y checkout;
- resolución de reservas y carritos pendientes;
- condiciones deterministas de fin de día;
- resumen técnico de actividad sin economía completa;
- integración con Store, customers, shopping y checkout;
- tests, validación manual, regresión y build externa.

No introducir todavía:

- precios e ingresos definitivos;
- impuestos;
- ledger y reportes económicos;
- persistencia integral;
- UI, audio o arte definitivos.

## Reglas

1. Congelar charter y acceptance criteria.
2. Preservar `.meta` y GUIDs.
3. Mantener Domain/Application independientes de escenas.
4. Mantener operaciones atómicas.
5. No declarar PASS sin evidencia completa.
6. Cerrar cada sprint con QA, trazabilidad, cierre y handoff.
