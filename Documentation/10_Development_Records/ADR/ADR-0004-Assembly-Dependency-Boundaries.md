# ADR-0004 — Assembly Dependency Boundaries

**Estado:** Accepted  
**Fecha:** 2026-06-21

## Decisión

Assemblies de producción:

- `VRMGames.CartridgeAndCloud.Domain`
- `VRMGames.CartridgeAndCloud.Application`
- `VRMGames.CartridgeAndCloud.Infrastructure`
- `VRMGames.CartridgeAndCloud.Presentation`

Assemblies de pruebas:

- `VRMGames.CartridgeAndCloud.Tests.EditMode`
- `VRMGames.CartridgeAndCloud.Tests.PlayMode`

Dependencias:

```text
Domain
  ↑
Application
  ↑            ↑
Infrastructure Presentation
```

Domain y Application usan `No Engine References`.

## Consecuencias

- Las reglas de simulación no dependen de Unity.
- La presentación no accede directamente a infraestructura.
- Se elimina la carpeta genérica `Shared`.
- Nuevas referencias requieren revisión.
