# U012 — Implementación integral de S17-MOD-002 a S17-MOD-010

- **Fecha:** 2026-07-08
- **Estado:** IMPLEMENTED CANDIDATE / PENDING UNITY VALIDATION
- **Baseline:** proyecto posterior a W8 `COMPLETED / PASS`
- **S17-MOD-001:** absorbida por ADR-0086 / S17-MOD-007

## Decisiones implementadas

| ID | Resultado candidato |
|---|---|
| S17-MOD-002 | Visitas admitidas sin producto visible; browsing de fallback y salida sin compra |
| S17-MOD-003 | `Process All` crea una única visita visible por `DeliveryRun` |
| S17-MOD-004 | Estados `Reserved`, `InTransit` y `Received`; stock, caja y ledger solo cambian en recepción física |
| S17-MOD-005 | Caja de reparto reducida y último LOD de todos los prefabs fijado a umbral 0 para no desaparecer por culling LOD en ningún nivel de zoom |
| S17-MOD-006 | HUD y Management muestran hora `HH:mm` derivada del reloj central |
| S17-MOD-007 | Reloj global continuo salvo pausa; cubre S17-MOD-001 |
| S17-MOD-008 | Aperturas múltiples 08:00–22:00 y cierre diario único a 24:00 |
| S17-MOD-009 | Grounding basado en anchor mundial y plano inferior del root técnico |
| S17-MOD-010 | `StaffPoint`/`StaffLookTarget`; orientación horizontal del root de gameplay |

## Compatibilidad

- Los valores numéricos históricos de `Ordered/Reserved` y `Received` se conservan.
- `InTransit` y el estado de run correspondiente se añaden sin reenumerar estados persistidos anteriores.
- Los campos temporales antiguos conservan nombre y representación; se añaden alias semánticos.
- Las órdenes y runs en tránsito se restauran y vuelven a presentar tras cargar.

## Cobertura añadida

- Caracterización específica `S17_MOD_002` a `S17_MOD_010`.
- Actualización de `CHAR-ODR-003`, `004`, `007` y `008` para despacho y recepción física separados.
- Adaptación de la regresión previa mediante helpers de test que completan explícitamente el run cuando el caso no estudia tránsito.
- Validación de 48 prefabs con último LOD a umbral 0 y sin modificación de los límites de zoom.
- Validación de anchors anidados y orientación del staff.
- Actualización de `13_Trazabilidad_y_Control_de_Cambios.xlsx` con ADR-0086/0087, pruebas, deuda y baseline candidata.

## Gate pendiente

1. Compilación sin Safe Mode.
2. EditMode y PlayMode completos.
3. Golden Path con tienda vacía, `Process All`, save/load en tránsito y tres aperturas.
4. Día completo `00:00–24:00` en todas las velocidades.
5. Inspección visual de caja, LOD, mobiliario y dependienta.
6. Build Windows x64 y `Player.log` sin errores.
