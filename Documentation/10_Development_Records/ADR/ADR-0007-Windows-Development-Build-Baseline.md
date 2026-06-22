# ADR-0007 — Windows Development Build Baseline

**Estado:** Accepted  
**Fecha:** 2026-06-22

## Contexto

S0.8 ha establecido una suite automática de nueve pruebas. El siguiente paso es
producir el primer ejecutable Windows reproducible del proyecto sin introducir
gameplay ni decisiones prematuras de distribución.

## Decisión

Crear un Build Profile versionado con esta línea base:

| Campo | Valor |
|---|---|
| Nombre | `Windows_Development` |
| Plataforma | Windows |
| Arquitectura | Intel 64-bit |
| Tipo | Development Build |
| Scripting Backend | Mono |
| Compresión | LZ4 |
| Build and Run on | Local Machine |
| Autoconnect Profiler | Desactivado |
| Deep Profiling | Desactivado |
| Script Debugging | Desactivado |
| Wait for Managed Debugger | Desactivado |
| Copy PDB files | Desactivado |
| Create Visual Studio Solution | Desactivado |
| Use Player Log | Activado |

El perfil tendrá su propia Scene List y contendrá:

```text
0 — Bootstrap
1 — MainMenu
2 — Store
3 — TestLab
```

El asset del perfil se almacenará en:

```text
Assets/_Project/Settings/BuildProfiles/Windows_Development
```

Unity determinará la extensión concreta del asset.

## Salida local

```text
Builds/Windows/Development/CAC_v0.0.1_Sprint0_Dev_Windows_x64_2026-06-22_Build001/CartridgeAndCloud.exe
```

La carpeta `Builds` permanece ignorada por Git. El perfil, documentación y
evidencias sí se versionan.

## Justificación

- Windows x64 es el objetivo inicial PC/Steam.
- Mono ofrece iteración rápida para el primer build y es el backend por defecto
  en plataformas de escritorio que admiten Mono e IL2CPP.
- LZ4 es apropiado para builds de desarrollo.
- Se conserva `Bootstrap` como única entrada técnica.
- `TestLab` se incluye porque este es un perfil de desarrollo, no de
  distribución.
- IL2CPP, símbolos de distribución y perfil Release se decidirán en una fase
  posterior mediante un ADR separado.

## Consecuencias

- El primer ejecutable puede mostrar una ventana vacía porque `Bootstrap` no
  contiene cámara ni gameplay.
- El éxito de S0.9 se mide por build correcto, arranque estable, cierre limpio y
  Player.log sin fallos no controlados.
- S0.10 no se autoriza hasta registrar el build y su evidencia.
