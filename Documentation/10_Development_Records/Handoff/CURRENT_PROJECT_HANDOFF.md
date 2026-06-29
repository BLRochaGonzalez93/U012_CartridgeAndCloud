# Cartridge & Cloud - Current Project Handoff

**Fecha:** 2026-06-29  
**Repositorio:** `BLRochaGonzalez93/U012_CartridgeAndCloud`  
**Rama objetivo:** `main`  
**Versión:** `0.0.15`  
**Commit de cierre Sprint 12:** `2d6b846120dc86536c19c12d07e5b515f7baa8fd`  
**Commit de cierre Sprint 13:** `5a9b80cdce3d6d619d64932838d0b0d7d77bb66d`  
**Commit de cierre Sprint 14:** este commit; registrar SHA después de publicarlo

## Estado

Sprints 0–14 `CLOSED / PASS`.

- EditMode: `961/961 PASS`.
- PlayMode: `41/41 PASS`.
- Total automatizado: `1002/1002 PASS`.
- Regresión manual: PASS.
- Windows x64 Development build: PASS.
- Ejecución externa: PASS.
- Defectos S0/S1: ninguno comunicado.

## Sprint 14 — Save Integration & Recovery

- Compatibilidad con GameSessionSnapshot schema v1.
- Backup persistente y recovery para los tres slots.
- IntegratedGameStateSnapshot schema v2.
- Registros de inventario, proveedores, displays, customers, shopping,
  checkout, day cycle y economy.
- Envelope con schema, slot, generación, timestamp y SHA-256.
- Temporal validado antes de reemplazar.
- Backup de la generación anterior.
- Recuperación y reparación automática del primario.
- Rechazo seguro de corrupción doble.
- Restauración en dos fases.
- Integración con GameSessionSnapshot.
- 105 tests específicos.

## Invariantes vigentes

- Un temporal inválido no sustituye al save válido.
- El backup conserva la generación anterior.
- Un primario inválido se recupera desde backup.
- Dos copias inválidas no aplican estado.
- Restore se ejecuta una vez tras validación y aceptación.
- Los tres slots permanecen aislados.
- Schema v1 y schema v2 conservan sus responsabilidades.
- No se serializan referencias Unity como estado autoritativo.

## Interfaz actual

MainMenu expone `Enter Store` y `Quit`.

La UI de New/Save/Load/Delete y selección de slots no está implementada y se
considera trabajo futuro, no una regresión de Sprint 14.

## Próximo trabajo

Fijar **Sprint 15** desde el roadmap vigente antes de implementar.

El charter del próximo bloque debe aclarar expresamente si incluye:

- UI de slots;
- integración runtime real de capture/restore;
- autosave;
- flujo de Continue/New Game;
- presentación de errores y recovery al jugador.

No asumir estos elementos sin congelar primero el alcance.

## Reglas

1. Preservar `.meta` y GUIDs.
2. Mantener Domain/Application independientes de escenas.
3. No serializar referencias Unity autoritativas.
4. No declarar PASS sin evidencia completa.
5. Distinguir claramente APIs implementadas de UI expuesta.
6. Cerrar cada sprint con QA, trazabilidad, cierre y handoff.
