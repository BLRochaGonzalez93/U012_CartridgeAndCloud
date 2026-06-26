# Cartridge & Cloud - Current Project Handoff

**Fecha:** 2026-06-26  
**Uso:** pegar o adjuntar al iniciar un nuevo chat de desarrollo  
**Repositorio:** `BLRochaGonzalez93/U012_CartridgeAndCloud`  
**Rama objetivo:** `main`  
**Versión:** `0.0.11`  
**Commit de cierre Sprint 8:** `409b7fe8653aa12ece7d484eb414172e1ed38f70`  
**Commit de cierre Sprint 9:** `b8ddf15356c73cd7d0c88a32805f0c6ee4058422`  
**Commit de cierre Sprint 10:** este commit; registrar SHA después de publicarlo

## Fuente oficial y registros operativos

- `Documentation/00_Official_Baseline/v0.5` permanece histórica e inmutable.
- El estado posterior está en `Documentation/10_Development_Records/Sprint_06`
  a `Sprint_10`.
- No modificar retrospectivamente la baseline v0.5.

## Estado

Sprints 0–10 `CLOSED / PASS`.

- EditMode: `508/508 PASS`.
- PlayMode: `41/41 PASS`.
- Total automatizado: `549/549 PASS`.
- Regresión manual: PASS.
- Windows x64 Development build: PASS.
- Ejecución externa: PASS.
- Defectos S0/S1: ninguno comunicado.

## Implementado

### Fundación y tienda

- Bootstrap, MainMenu, Store, TestLab y ApplicationRoot.
- Save skeleton.
- Movimiento y cámara.
- Grid, placement, ocupación y acceso.
- Store 10x15 m.

### Sprint 6 — Product & Inventory Core

- Productos, cantidades, contenedores y transferencias.
- Invariantes y conservación de unidades.

### Sprint 7 — Supplier Orders & Receiving

- Catálogo, proveedores, pedidos, entregas, cajas y recepción.

### Sprint 8 — Displays & Restocking

- Displays, asignación, capacidad, stock y reposición.

### Sprint 9 — Customer Profiles & Spawning

- Perfiles, llegada, población, navegación técnica y paciencia.

### Sprint 10 — Shopping & Reservations

- ShoppingIntent y política autorable.
- Disponibilidad on-hand/reserved/available.
- Búsqueda determinista por preferencias.
- Reservas atómicas con procedencia.
- Prevención de sobreventa.
- Carrito respaldado por reservas.
- CustomerShoppingSession.
- Liberación por abandono.
- Consistencia y conservación.
- ShoppingTechnicalScenarioRunner.
- 79 tests específicos.

## Invariantes vigentes

- Inventario físico autoritativo en DisplayInstance.Inventory.
- `on-hand = available + reserved`.
- El carrito no duplica inventario.
- Cada línea del carrito requiere una reserva activa coincidente.
- El abandono libera reservas.
- El consumo definitivo se realizará en checkout.

## Límites actuales

- No existe cola de checkout.
- No existe interacción de caja.
- Las reservas no se consumen definitivamente.
- No existe transacción económica final.
- No existen impuestos, ledger ni reportes.
- No existe ciclo de día completo.
- El estado integral no está en SaveRootV1.
- UI, modelos y animaciones finales siguen diferidos.

## Próximo trabajo

Abrir **Sprint 11 — Queue & Checkout**.

Debe cubrir:

- identificadores y estados de cola;
- entrada y salida determinista de clientes con carrito;
- orden FIFO y posición de cola;
- estación de checkout técnica;
- interacción mínima de procesamiento;
- validación de carrito y reservas activas;
- consumo atómico de reservas y stock del display;
- prevención de doble checkout;
- resultado de transacción sin introducir todavía economía completa;
- liberación segura ante cancelación o fallo;
- integración con CustomerShoppingSession;
- tests, validación manual, regresión y build externa.

No introducir todavía:

- ledger económico completo;
- impuestos o reportes;
- ciclo de día;
- persistencia integral;
- UI, audio o arte definitivos.

## Reglas de trabajo

1. Congelar charter y acceptance criteria antes de código.
2. Preservar `.meta` y GUIDs.
3. Mantener Domain/Application sin dependencias de escenas.
4. Usar ScriptableObjects para autoría, no como dominio.
5. Mantener inventario, reservas y transacción atómicos.
6. No declarar PASS sin evidencia completa.
7. Cerrar cada sprint con QA, trazabilidad, closure report y handoff.
