# Sprint 09 Charter — Customer Profiles & Spawning

**Estado:** `CLOSED / PASS`  
**Fecha de apertura:** 2026-06-25  
**Fecha de cierre:** 2026-06-25  
**Versión objetivo alcanzada:** `0.0.10`  
**Baseline técnica de entrada:** `409b7fe8653aa12ece7d484eb414172e1ed38f70`

## Objetivo

Introducir perfiles de cliente autorables, selección ponderada determinista, cola
de llegada, límite de población, ciclo de vida mínimo, navegación técnica y
paciencia sin introducir todavía búsqueda de producto, reservas, carrito, compra
o checkout.

## Alcance completado

- IDs estables de perfil, instancia, solicitud y punto de navegación.
- Perfiles con preferencias de categoría, peso, paciencia, paradas y velocidad.
- Registro y selección ponderada determinista.
- Plan Entry → Browse* → Exit.
- Estados WaitingToEnter, Entering, Browsing, Leaving y Despawned.
- Cola FIFO y límite de población.
- Reloj de llegadas determinista.
- Autoría mediante ScriptableObjects.
- Cuatro perfiles técnicos y ajustes técnicos de spawn.
- Componentes de runtime para prueba temporal en TestLab/Store.
- Tests EditMode y gates manuales/build.

## Fuera de alcance preservado

- Evaluación de productos y displays.
- Reservas, carrito, abandono de compra y checkout.
- NavMesh definitivo, avoidance avanzado o animaciones finales.
- Arte final de clientes.
- Persistencia completa de clientes.
- Economía, satisfacción o reputación.

## Gates finales

| Gate | Resultado |
|---|---|
| Compilación sin errores | PASS |
| Customers EditMode `96/96` | PASS |
| EditMode completo `429/429` | PASS |
| PlayMode `41/41` | PASS |
| Total `470/470` | PASS |
| Validación manual | PASS |
| Regresión completa | PASS |
| Build Windows x64 Development | PASS |
| Ejecución externa | PASS |
