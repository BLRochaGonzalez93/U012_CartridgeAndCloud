# ADR-0002 — Version Control and Repository Workflow

**Estado:** Accepted  
**Fecha:** 2026-06-21

## Decisión

Usar:

- GitHub.
- GitHub Desktop.
- Repositorio `BLRochaGonzalez93/U012_CartridgeAndCloud`.
- Rama `main`.
- Commits pequeños y descriptivos.

Se versionan:

- Assets.
- Packages.
- ProjectSettings.
- Documentation.
- Evidence.
- `.gitignore`.
- `.gitattributes`.
- `.vsconfig`.

Se ignoran caches, builds y archivos de solución generados.

Git LFS queda aplazado hasta incorporar binarios grandes.

## Consecuencias

- PowerShell no forma parte del flujo diario.
- Unity debe abrirse y cerrarse antes de cerrar cada bloque.
- GitHub Desktop debe quedar limpio después de validar.
