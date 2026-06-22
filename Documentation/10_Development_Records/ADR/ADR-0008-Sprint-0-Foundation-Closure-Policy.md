# ADR-0008 — Sprint 0 Foundation Closure Policy

**Estado:** Accepted  
**Fecha:** 2026-06-22

## Contexto

S0.1–S0.9 han establecido el proyecto Unity, repositorio, documentación,
assemblies, escenas, pruebas y primer Player Windows. Antes de comenzar la fase
siguiente es necesario congelar una base técnica coherente y registrar las
deudas aceptadas.

## Decisión

S0.10 cerrará Sprint 0 mediante una auditoría final con estos criterios:

1. Entorno de desarrollo documentado.
2. Application Identifier canónico:
   `com.vrmgames.cartridgeandcloud`.
3. Revisión explícita de todos los paquetes no requeridos.
4. Build Profile de desarrollo con Scene List propia.
5. Eliminación o justificación de archivos huérfanos.
6. 9/9 smoke tests después de los cambios finales.
7. Build Windows final de regresión.
8. Documentación, QA y trazabilidad sincronizadas.
9. Repositorio limpio.
10. Tag de fundación creado solo después del PASS final.

## Política de deuda

Una deuda puede permanecer únicamente cuando:

- tiene propietario;
- tiene una razón explícita;
- no bloquea la siguiente fase;
- tiene un bloque objetivo;
- aparece en QA, riesgos y trazabilidad.

## Hito de cierre

El tag propuesto para el cierre es:

```text
v0.0.1-project-foundation
```

El tag no se crea hasta que S0.10 esté formalmente cerrado.
