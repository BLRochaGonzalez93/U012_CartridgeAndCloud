# Cartridge & Cloud - Current Project Handoff

**Fecha:** 2026-06-25  
**Uso:** pegar o adjuntar al iniciar un nuevo chat de desarrollo  
**Repositorio:** `BLRochaGonzalez93/U012_CartridgeAndCloud`  
**Rama objetivo:** `main`  
**Versión:** `0.0.10`  
**Commit de cierre Sprint 7:** `b997377eae720e960c02ee5dce53da8b37f66b19`  
**Commit de cierre Sprint 8:** `409b7fe8653aa12ece7d484eb414172e1ed38f70`  
**Commit de cierre Sprint 9:** este commit; registrar SHA después de publicarlo

## Fuente oficial y registros operativos

- La baseline publicada `Documentation/00_Official_Baseline/v0.5` permanece
  histórica e inmutable.
- El estado operativo posterior está documentado en
  `Documentation/10_Development_Records/Sprint_06` a `Sprint_09`.
- No modificar retrospectivamente la baseline v0.5; publicar una baseline nueva
  únicamente mediante su gate documental específico.

## Estado

Sprints 0–9 `CLOSED / PASS`.

- EditMode: `429/429 PASS`.
- PlayMode: `41/41 PASS`.
- Baseline automatizada: `470/470 PASS`.
- Regresión manual: PASS.
- Windows x64 Development build: PASS.
- Ejecución externa: PASS.
- Defectos S0/S1 abiertos: ninguno comunicado.

## Implementado

### Fundación previa

- Bootstrap, MainMenu, Store, TestLab y ApplicationRoot.
- Scene flow y contextos UI/Gameplay.
- Save skeleton mínimo.
- Click-to-move, cámara orbital y zoom.
- Grid, placement, ocupación, retirada y validación de acceso.
- Store 10x15 m con baseline vacía reproducible.

### Sprint 6 — Product & Inventory Core

- Definiciones e identificadores de producto.
- Registro determinista.
- Cantidades, capacidades, stacks y contenedores.
- Mutaciones y transferencias atómicas.
- Conservación de unidades.

### Sprint 7 — Supplier Orders & Receiving

- Autoría de productos, catálogos y proveedores.
- Pedidos, líneas, estados y totales.
- Entregas y cajas deterministas.
- Recepción atómica hacia inventario.
- Prevención de recepción duplicada.

### Sprint 8 — Displays & Restocking

- Definiciones, catálogos e instancias de display.
- Un producto asignado por display.
- Capacidad lógica y límite visible.
- Reposición desde Storage o Transit.
- Devolución de stock y limpieza de asignación.
- RestockTask y runtime authoring técnico.

### Sprint 9 — Customer Profiles & Spawning

- CustomerProfile, IDs, registros y catálogo autorable.
- Cuatro perfiles técnicos y spawn settings.
- Selección ponderada reproducible.
- Cola FIFO, reloj de llegada y límite de población.
- CustomerInstance y ciclo de vida inicial.
- Planes técnicos Entry → Browse* → Exit.
- Paciencia y salida automática.
- CustomerSpawnAreaAuthoring, CustomerTechnicalSpawner y cápsulas de fallback.
- Sin dependencia de modelos o animaciones finales.

## Incidencias conocidas resueltas

- Sprint 8: incompatibilidad de `Assert.Multiple` con la versión de NUnit del
  proyecto, corregida sin impacto en producción.
- No se comunicaron incidencias bloqueantes durante Sprint 9.

## Límites actuales

- Los clientes todavía no buscan ni evalúan productos concretos.
- No existen reservas de unidades ni carrito.
- No existe abandono de compra con liberación de reservas.
- No existe cola de checkout ni transacción de venta.
- La navegación de clientes es técnica, no NavMesh final.
- Los modelos, animaciones y prefabs finales siguen diferidos.
- El estado completo todavía no está integrado en SaveRootV1.
- No existe economía completa ni ledger de ventas.

## Unity Services

Existe un aviso conocido de membresía del proyecto de Unity Cloud durante
algunas builds. Las builds se han continuado sin Unity Services y han pasado.
Ningún sistema vigente depende de Unity Gaming Services.

## Próximo trabajo

Abrir **Sprint 10 — Shopping & Reservations**.

Debe cubrir, como mínimo:

- `ShoppingIntent` y criterios deterministas de búsqueda;
- evaluación de productos disponibles en displays;
- selección de producto y display objetivo;
- reserva atómica de unidades para evitar sobreventa;
- carrito mínimo del cliente;
- liberación de reservas por abandono, fallo o salida;
- conservación de unidades entre display, reserva y carrito;
- integración con CustomerInstance sin introducir todavía cola o checkout;
- pruebas EditMode prioritarias, validación manual y regresión completa.

No introducir todavía:

- cola de caja;
- interacción de checkout;
- transacción económica final;
- impuestos, ledger o reportes;
- ciclo de día completo;
- persistencia integral;
- UI o arte definitivos.

## Reglas de trabajo

1. Leer handoff, Guía Maestra, Roadmap, TDD, Modelo de Datos y QA Plan.
2. Congelar charter y acceptance criteria antes de código.
3. Preservar `.meta` y GUIDs existentes.
4. Mantener Domain/Application independientes de escenas y MonoBehaviour.
5. Usar ScriptableObjects como autoría, no como sustituto del dominio.
6. Mantener inventario y reservas como estado lógico autoritativo.
7. Exigir compilación, Test Runner, regresión manual y build externa.
8. No declarar PASS sin evidencia completa.
9. Cerrar cada sprint con QA, trazabilidad, closure report y handoff.
