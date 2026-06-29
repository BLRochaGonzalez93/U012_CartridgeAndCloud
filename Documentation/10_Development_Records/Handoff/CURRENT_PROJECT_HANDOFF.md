# Cartridge & Cloud - Current Project Handoff

**Fecha:** 2026-06-29  
**Repositorio:** `BLRochaGonzalez93/U012_CartridgeAndCloud`  
**Rama objetivo:** `main`  
**Versión:** `0.0.13`  
**Commit de cierre Sprint 10:** `f91c8fda6c1ff1ff9e811717b05fc8fa427707e6`  
**Commit de cierre Sprint 11:** `08ec92ba97f4070399f01023c32f49327a951712`  
**Commit de cierre Sprint 12:** este commit; registrar SHA después de publicarlo

## Estado

Sprints 0–12 `CLOSED / PASS`.

- EditMode: `756/756 PASS`.
- PlayMode: `41/41 PASS`.
- Total automatizado: `797/797 PASS`.
- Regresión manual: PASS.
- Windows x64 Development build: PASS.
- Ejecución externa: PASS.
- Defectos S0/S1: ninguno comunicado.

## Implementado

- Fundación, navegación, placement y Store 10x15 m.
- Product & Inventory Core.
- Supplier Orders & Receiving.
- Displays & Restocking.
- Customer Profiles & Spawning.
- Shopping & Reservations.
- Queue & Checkout.
- Day Cycle & Store Closure.

### Sprint 12

- `StoreDayId`, política y agregado del día.
- Estados BeforeOpen, Open, Closing y Closed.
- Reloj lógico y cierre automático o manual.
- Sellado de cola.
- Bloqueo de spawning.
- Drenaje de clientes a Exit.
- Resolución de reservas, carritos y sesiones pendientes.
- Preservación de checkout Processing.
- Cierre determinista por snapshot.
- Resumen técnico no económico.
- StoreDayRuntimeController.
- StoreDayTechnicalScenarioRunner.
- 143 tests específicos.

## Invariantes vigentes

- Solo Open admite nuevos clientes.
- Closing sella la cola y bloquea el spawner.
- El trabajo admitido puede drenarse.
- Un checkout Processing puede terminar durante Closing.
- Reservas y carritos pendientes se resuelven exactamente una vez.
- Closed exige cero trabajo operativo pendiente.
- El resumen del día no contiene economía.

## Límites actuales

- No existen precios de venta definitivos.
- No existen ingresos, impuestos o ledger.
- No existen resultados económicos diarios.
- No existe persistencia integral.
- UI, audio y arte finales siguen diferidos.

## Próximo trabajo

Abrir **Sprint 13 — Economy & Daily Results**.

Debe cubrir:

- representación monetaria exacta y determinista;
- precio de compra y precio de venta;
- valoración de líneas y transacciones;
- ingresos de checkout;
- costes de proveedor recibidos;
- resultado bruto técnico del día;
- resumen diario económico;
- prevención de doble contabilización;
- integración con checkout y StoreDayActivitySummary;
- invariantes de conservación monetaria;
- tests, validación manual, regresión y build externa.

No introducir todavía impuestos complejos, contabilidad avanzada, préstamos,
intereses, nóminas, persistencia integral ni presentación final.

## Reglas

1. Congelar charter y acceptance criteria.
2. Preservar `.meta` y GUIDs.
3. Mantener Domain/Application independientes de escenas.
4. Usar una representación monetaria exacta.
5. Evitar doble contabilización.
6. No declarar PASS sin evidencia completa.
7. Cerrar cada sprint con QA, trazabilidad, cierre y handoff.
