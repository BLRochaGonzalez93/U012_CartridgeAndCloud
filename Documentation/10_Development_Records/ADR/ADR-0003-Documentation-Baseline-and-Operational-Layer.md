# ADR-0003 — Documentation Baseline and Operational Layer

**Estado:** Accepted  
**Fecha:** 2026-06-21

## Contexto

La documentación v0.3 es la fuente inicial y declara correctamente que el
proyecto no estaba implementado. Modificarla silenciosamente eliminaría la
trazabilidad histórica.

## Decisión

Mantener:

```text
Documentation/00_Official_Baseline/v0.3
```

como línea base inmutable.

Registrar el estado ejecutado en:

```text
Documentation/10_Development_Records
```

Los Excel operativos se almacenan en:

```text
Documentation/10_Development_Records/Production_Tracking
```

## Consecuencias

- La línea base conserva su función probatoria.
- El estado real se actualiza sin reescribir el pasado.
- Las nuevas versiones oficiales se producirán en un cierre documental
  explícito.
