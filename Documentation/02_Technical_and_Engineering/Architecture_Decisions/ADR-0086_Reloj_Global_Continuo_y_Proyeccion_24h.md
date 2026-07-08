# ADR-0086 — Reloj global continuo y proyección virtual de 24 horas

- **Estado:** ACCEPTED / IMPLEMENTED CANDIDATE
- **Fecha:** 2026-07-08
- **Decisión de origen:** S17-MOD-006 y S17-MOD-007; absorbe S17-MOD-001
- **Propietarios:** Design, Engineering, UX y QA

## Contexto

La baseline W1 vinculaba el avance del reloj de negocio al estado `Open` y mostraba el progreso técnico como `0 / 300`. La velocidad seleccionada podía divergir de la velocidad efectiva en estados no comerciales.

## Decisión

1. `SimulationClock` es la única autoridad temporal del día.
2. El reloj avanza en todos los estados de la tienda salvo cuando `PauseService` mantiene una pausa efectiva.
3. Los 300 segundos configurables de H6 representan un día virtual completo de `00:00` a `24:00`.
4. El HUD y las proyecciones muestran `HH:mm`; el valor técnico se conserva para persistencia y tests.
5. `Time.timeScale` utiliza el multiplicador seleccionado en todos los estados jugables; la pausa tiene prioridad y fuerza `0`.
6. `Open`, `Closing`, `Closed` y `BeforeOpen` regulan la operación comercial, no la autoridad temporal.
7. Los nombres persistidos `OpenDurationSeconds` y `ElapsedOpenSeconds` se conservan por compatibilidad, con alias semánticos `DayDurationSeconds` y `ElapsedDaySeconds`.

## Consecuencias

- S17-MOD-001 queda cubierta por esta decisión.
- La preparación, cierres voluntarios y espera de entregas consumen tiempo.
- El autosave diario y la liquidación no se disparan por un cierre comercial intermedio.
- La migración no requiere reinterpretar los segundos guardados.

## Validación requerida

- `00:00`, `08:00`, `22:00` y `24:00` se proyectan exactamente.
- El reloj avanza en `BeforeOpen`, `Open`, `Closing` y `Closed`.
- La pausa es la única congelación normal.
- Save/load restaura el mismo segundo y la misma hora virtual.
- No existen eventos de cierre o autosave repetidos mientras el reloj avanza con la tienda cerrada.
