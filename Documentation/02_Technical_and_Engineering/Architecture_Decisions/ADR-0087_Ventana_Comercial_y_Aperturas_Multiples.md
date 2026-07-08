# ADR-0087 — Ventana comercial y aperturas múltiples dentro del mismo día

- **Estado:** ACCEPTED / IMPLEMENTED CANDIDATE
- **Fecha:** 2026-07-08
- **Decisión de origen:** S17-MOD-008
- **Propietarios:** Design, Engineering, Economy, UX y QA

## Contexto

La baseline W1-W8 modelaba una única apertura por jornada y asociaba el cierre comercial con la liquidación diaria. El nuevo reloj continuo exige separar el estado comercial del final de día.

## Decisión

1. `00:00–08:00`: la tienda permanece cerrada y no puede abrir.
2. `08:00–22:00`: pueden ejecutarse múltiples ciclos `Open → Closing → Closed → Open`.
3. Una reapertura exige checkout funcional, capacidad válida y cierre anterior completado.
4. A las `22:00`, una tienda abierta entra automáticamente en `Closing`; no se admiten nuevos clientes.
5. `22:00–24:00`: no puede iniciarse una apertura nueva.
6. Un cierre comercial intermedio no reinicia estadísticas, no liquida el día y no ejecuta autosave diario.
7. A las `24:00`, tras drenar clientes y checkout, se ejecutan una sola liquidación, cierre diario y autosave.
8. El siguiente día solo puede comenzar desde `Closed` a `24:00`.

## Consecuencias

- Management acumula todas las aperturas del mismo día.
- El impuesto semanal continúa siendo idempotente y se aplica únicamente en el cierre diario correspondiente.
- El checkout cambia entre `Available` y `Closed` con cada ciclo sin reconstruir la sesión.
- Los pedidos y repartidores continúan según el reloj global incluso con la tienda cerrada.

## Validación requerida

- Casos límite `07:59`, `08:00`, `21:59`, `22:00`, `23:59` y `24:00`.
- Tres ciclos comerciales en un mismo día sin reinicio de métricas.
- Save/load en `Open`, `Closing` y `Closed`.
- Cierre automático a las 22:00 y liquidación única a medianoche.
- Ausencia de autosaves, impuestos o resúmenes duplicados por cierres intermedios.
