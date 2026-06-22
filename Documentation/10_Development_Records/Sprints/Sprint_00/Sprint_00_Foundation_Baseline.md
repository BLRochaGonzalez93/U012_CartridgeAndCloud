# Sprint 00 — Foundation Baseline

**Estado técnico:** Closed  
**Resultado:** PASS  
**Versión:** `0.0.1`  
**Commit técnico de cierre:** `478794017015054571d6ca22332a201589abbe5c`  
**Tag objetivo:** `v0.0.1-project-foundation`

## Base técnica validada

- Unity `6000.3.18f1`.
- URP `17.3.0`.
- Windows x64.
- Application Identifier `com.vrmgames.cartridgeandcloud`.
- Perfil `Windows_Development`.
- Mono + LZ4.
- Cuatro assemblies de producción.
- Dos assemblies de pruebas.
- Cuatro escenas.
- Nueve smoke tests.
- Cinco paquetes no esenciales retirados.

## QA

```text
Pre-build: 9/9 PASS
Build002: PASS
External execution: PASS
Player.log: PASS
Post-build: 9/9 PASS
```

## Build002

```text
Duration: 74 seconds
Folder size: 157 MB (165,653,511 bytes)
ZIP size: 65.6 MB (68,875,051 bytes)
SHA-256: 897d85a00e5afd3d3d019ebf646f2128fa9a27e3bcfa8c50ec3e4ee56c3a2ad6
```

## Publicación

El commit técnico de cierre está publicado en `main`.

Antes de crear el tag se publicará un parche documental para:

- reparar dos archivos con codificación UTF-8 dañada;
- consolidar el SHA real del commit técnico;
- cerrar los metadatos operativos.

## Siguiente fase

Autorizada después de publicar el parche y crear el tag.
