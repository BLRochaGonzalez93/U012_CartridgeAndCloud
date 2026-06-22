# ADR-0006 — Smoke Test Baseline and Execution Policy

**Estado:** Accepted  
**Fecha:** 2026-06-22

## Contexto

Sprint 0 necesita una comprobación automatizada mínima antes del primer build.
Todavía no existe gameplay ni lógica funcional que justifique pruebas de
comportamiento de dominio.

## Decisión

Crear una línea base de nueve pruebas:

### EditMode — cinco pruebas

- Dos pruebas para los seis Assembly Definitions y su grafo de dependencias.
- Una prueba para la existencia de las cuatro escenas.
- Una prueba para la lista global exacta de escenas.
- Una prueba para sus índices 0–3.

### PlayMode — cuatro pruebas

- Cargar `Bootstrap` y comprobar que no tiene GameObjects raíz.
- Cargar `MainMenu`, `Store` y `TestLab`.
- Comprobar en las tres escenas que solo existen `Main Camera` y
  `Directional Light`.
- Comprobar los componentes básicos de cámara, audio y luz direccional.

## Política de ejecución

- Ejecutar EditMode antes de PlayMode.
- Exigir 5/5 y 4/4 pruebas aprobadas.
- No cambiar escenas o assemblies para ocultar un fallo.
- Corregir la causa y volver a ejecutar la suite completa.
- No establecer todavía un objetivo de cobertura.
- No añadir CI ni ejecución por línea de comandos durante Sprint 0.
- No introducir gameplay para crear pruebas artificiales.

## Consecuencias

- Los límites arquitectónicos quedan protegidos por regresión.
- Los nombres, rutas y orden de escenas quedan automatizados.
- El primer build Windows no se autoriza hasta obtener 9/9 PASS.
