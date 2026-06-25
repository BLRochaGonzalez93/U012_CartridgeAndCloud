# Cartridge & Cloud - Current Project Handoff

**Fecha:** 2026-06-25  
**Uso:** pegar o adjuntar al iniciar un nuevo chat de desarrollo  
**Repositorio:** `BLRochaGonzalez93/U012_CartridgeAndCloud`  
**Rama:** `main`  
**Versión:** `0.0.6`  
**Commit técnico Sprint 5:** `22d62967aaf9895db7bce75afd2ffa11f7858e0c`  
**Commit documental Sprint 5:** `b650aa5968ecfc9ebe63fc308b0dc8ea2cafae57`

## Fuente oficial

Usar `Documentation/00_Official_Baseline/v0.5` como baseline vigente. v0.4 permanece histórica e inmutable.

## Estado

Sprints 0-5 CLOSED / PASS. Suite aceptada: 127 EditMode + 41 PlayMode = 168/168. Build Windows x64 Development y ejecución externa PASS.

## Implementado

- Bootstrap, MainMenu, Store, TestLab y ApplicationRoot.
- Scene flow y contextos UI/Gameplay.
- Save skeleton mínimo.
- Click-to-move, cámara orbital y zoom.
- Grid 0,5 m, huellas, rotación, preview, ocupación y retirada.
- Store 10x15 m, grid 20x30, entrada 2 m.
- Entrada reservada, dos celdas libres mínimas y tres anchors.
- Construcción en Store con validación bounds/overlap/access.

## Controles

- Clic: mover fuera de construcción / confirmar dentro.
- Arrastre derecho: órbita.
- Rueda: zoom.
- B: construcción.
- Q/E: rotar.
- Escape: cancelar.
- Delete/Backspace: retirar.

## Próximo trabajo

Abrir Sprint 6 - Product & Inventory Core. Debe limitarse a definiciones, cantidades, contenedores, transferencias e invariantes. No introducir todavía pedidos, displays, clientes o economía.

## Reglas de trabajo

1. Leer handoff, Guía, Roadmap, TDD, Modelo de Datos y QA Plan.
2. Congelar charter y acceptance criteria antes de código.
3. Preservar metas.
4. Mantener Domain/Application independientes de Unity.
5. Validar estáticamente, pero exigir compilación y Test Runner del usuario.
6. No declarar PASS sin suite completa y manual.
7. Cerrar cada sprint con documentación, QA y trazabilidad.
8. No crear tag/release/checksum salvo gate explícito.

## Baseline Store

- 10x15 m; 20x30 celdas; 0,5 m.
- Entrada: celdas (8,0)-(11,0).
- Mínimo libre: 2 celdas adyacentes.
- Anchors: rear-service (10,27), left-display (3,15), right-display (16,15).
- Objeto técnico: technical-shelf-4x2.
- Store inicia vacío, construcción desactivada y ghost oculto.
