# Sprint 09 Closure Report

**Sprint:** 09 — Customer Profiles & Spawning  
**Fecha de cierre:** 2026-06-25  
**Versión:** `0.0.10`  
**Baseline técnica de entrada:** `409b7fe8653aa12ece7d484eb414172e1ed38f70`  
**Commit de cierre:** este commit; registrar SHA después de publicarlo  
**Estado:** `CLOSED / PASS`

## 1. Resultado

Sprint 09 queda cerrado después de completar implementación, compilación,
validación automatizada, validación manual, build Windows x64 Development y
ejecución externa.

## 2. Entregables aceptados

- Identificadores estables de perfil, instancia, solicitud y navegación.
- `CustomerProfile` y registro determinista.
- Preferencias de categoría, peso de spawn, paciencia, paradas y velocidad.
- Selección ponderada reproducible.
- Cola FIFO de solicitudes sin IDs duplicados.
- Política de población activa y preservación de solicitudes cuando se alcanza
  el límite.
- Reloj determinista de llegadas.
- Ciclo de vida `WaitingToEnter`, `Entering`, `Browsing`, `Leaving`,
  `Despawned`.
- Planes técnicos `Entry → Browse* → Exit`.
- Consumo de paciencia y salida al agotarse.
- Autoría mediante `CustomerProfileAsset`, catálogo y spawn settings.
- Cuatro perfiles técnicos y configuración técnica de aparición.
- `CustomerSpawnAreaAuthoring`, `CustomerRuntimeAuthoring`,
  `CustomerTechnicalSpawner` y vista técnica con cápsula de fallback.
- 96 tests específicos de Sprint 09.

## 3. Evidencia automatizada

| Suite | Resultado |
|---|---:|
| Customers EditMode | `96/96 PASS` |
| EditMode completo | `429/429 PASS` |
| PlayMode completo | `41/41 PASS` |
| Total | `470/470 PASS` |

## 4. Evidencia manual

- Assets de perfiles, catálogo y spawn settings: PASS.
- Autoría de área de spawn: PASS.
- Spawner técnico y creación de vistas: PASS.
- Navegación Entry/Browse/Exit: PASS.
- Límite máximo de población: PASS.
- Consumo de paciencia y salida: PASS.
- Regresión Bootstrap/MainMenu/Store/TestLab: PASS.
- Movimiento, cámara y placement existentes: PASS.

## 5. Build

- Plataforma: Windows x64.
- Configuración: Development Build.
- Versión: `0.0.10`.
- Generación de build: PASS.
- Ejecución externa: PASS.
- Avisos de Unity Services, si aparecieron, se consideran informativos mientras
  ningún sistema vigente dependa de Unity Gaming Services.

## 6. Calidad y defectos

- Compilación sin errores.
- Sin `Missing Script` comunicado.
- Sin pérdida de funcionalidad previa comunicada.
- Defectos S0/S1 abiertos: ninguno comunicado.
- No se han proporcionado logs exportados, hash del ejecutable o ruta de build;
  no se inventan en este registro.

## 7. Límites preservados

Sprint 09 no introduce:

- evaluación o selección comercial de productos;
- reservas;
- carrito;
- abandono de compra con devolución de reservas;
- cola de checkout;
- transacciones;
- economía completa;
- persistencia integral de clientes;
- navegación final, avoidance avanzado, animaciones o arte final.

## 8. Decisión de cierre

Todos los gates definidos por el charter han sido comunicados como PASS. Sprint
09 queda `CLOSED / PASS` y habilita la apertura de Sprint 10 — Shopping &
Reservations.
