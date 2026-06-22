# Sprint 00 — Foundation Baseline

**Estado técnico:** Validated  
**Resultado:** PASS  
**Versión:** `0.0.1`  
**Commit final:** PENDIENTE PUBLICACIÓN  
**Tag objetivo:** `v0.0.1-project-foundation`

## Bloques

| Bloque | Estado | Resultado |
|---|---|---|
| S0.1 | Closed | PASS con deuda documental resuelta en S0.10 |
| S0.2 | Closed | PASS |
| S0.3 | Closed | PASS con observación resuelta en S0.10 |
| S0.4 | Closed | PASS |
| S0.5 | Closed | PASS |
| S0.6 | Closed | PASS |
| S0.6D | Closed | PASS |
| S0.7 | Closed | PASS |
| S0.8 | Closed | PASS |
| S0.9 | Closed | PASS con observaciones transferidas |
| S0.10 | Validated | PASS — publicación pendiente |

## Base técnica

- Unity `6000.3.18f1`.
- URP `17.3.0`.
- Windows x64.
- Application Identifier `com.vrmgames.cartridgeandcloud`.
- Perfil `Windows_Development`.
- Mono + LZ4 Development Build.
- Root namespace `VRMGames.CartridgeAndCloud`.
- Cuatro assemblies de producción.
- Dos assemblies de pruebas.
- Cuatro escenas.
- Nueve smoke tests.
- Cinco paquetes no esenciales retirados.

## Escenas

```text
0 — Bootstrap
1 — MainMenu
2 — Store
3 — TestLab
```

## QA

```text
EditMode: 5/5 PASS
PlayMode: 4/4 PASS
Pre-build: 9/9 PASS
Post-build: 9/9 PASS
```

## Build final

```text
Build ID: S0.10-WIN-FINAL-002
Duration: 74 seconds
Folder size: 157 MB (165,653,511 bytes)
ZIP size: 65.6 MB (68,875,051 bytes)
SHA-256: 897d85a00e5afd3d3d019ebf646f2128fa9a27e3bcfa8c50ec3e4ee56c3a2ad6
External execution: PASS
Player.log: PASS
```

## Estado de publicación

El contenido técnico de Sprint 0 está validado. Quedan exclusivamente:

- commit final;
- push;
- verificación en `main`;
- tag posterior.

## Siguiente fase

La siguiente fase queda autorizada después de publicar el commit final y crear
el tag de fundación.
