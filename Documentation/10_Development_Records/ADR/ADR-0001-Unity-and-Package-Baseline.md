# ADR-0001 — Unity and Package Baseline

**Estado:** Accepted  
**Fecha:** 2026-06-21

## Contexto

El proyecto comienza desde cero y necesita una línea base estable para un
desarrollo en solitario.

## Decisión

- Unity 6.3 LTS `6000.3.18f1`.
- Universal Render Pipeline.
- Windows x64.
- Paquetes Released resueltos por Package Manager.
- `manifest.json` y `packages-lock.json` versionados.

Paquetes obligatorios:

- AI Navigation `2.0.13`.
- Input System `1.19.0`.
- Localization `1.5.12`.
- URP `17.3.0`.
- Test Framework `1.6.0`.
- Unity UI `2.0.0`.
- Visual Studio Editor `2.0.26`.

## Consecuencias

- No se actualiza automáticamente a otra rama Unity.
- Cada upgrade requiere QA y trazabilidad.
- Paquetes Preview no están permitidos.
- Los paquetes de plantilla no requeridos deben revisarse, no eliminarse a mano.
