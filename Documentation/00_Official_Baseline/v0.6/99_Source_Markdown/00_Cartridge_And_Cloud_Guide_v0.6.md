---
title: "Cartridge & Cloud - Guía Maestra v0.6"
subtitle: "Baseline documental v0.6 - Sprint 16 en curso"
author: "VRM Games / Blas Luis Rocha González"
date: "01/07/2026"
lang: es-ES
papersize: a4
fontsize: 10pt
geometry: margin=18mm
toc: true
toc-depth: 3
colorlinks: true
linkcolor: VRMGreen
urlcolor: VRMGreen
header-includes:
- |
  ```{=latex}
  \usepackage{xcolor}
  \definecolor{VRMGreen}{HTML}{008F46}
  \definecolor{VRMDark}{HTML}{101716}
  \definecolor{VRMGray}{HTML}{5B6660}
  \usepackage{sectsty}
  \sectionfont{\color{VRMGreen}}
  \subsectionfont{\color{VRMDark}}
  \subsubsectionfont{\color{VRMGray}}
  \usepackage{fancyhdr}
  \pagestyle{fancy}
  \fancyhf{}
  \fancyhead[L]{\small Cartridge \& Cloud - Baseline v0.6}
  \fancyhead[R]{\small VRM Games}
  \fancyfoot[C]{\thepage}
  \setlength{\headheight}{14pt}
  \usepackage{enumitem}
  \setlist{nosep}
  \usepackage{longtable}
  \usepackage{booktabs}
  \usepackage{array}
  \renewcommand{\arraystretch}{1.15}
  ```
---

**Proyecto:** Cartridge & Cloud  
**Plataforma:** PC / Steam  
**Motor:** Unity 6.3 LTS `6000.3.18f1` / URP `17.3.0`  
**Versión de aplicación:** `0.0.17`  
**Baseline documental:** `v0.6`  
**Último commit técnico validado:** `091090c43855b0b26b09abe9335d18b978ac7eab`  
**Último commit observado en main:** `d54316c771aab2143993e99b9fd58f2f88016568`  
**Estado global:** Sprints 0-15 CLOSED / PASS; Sprint 16 IN PROGRESS; Sprint 17 PENDING

> Este documento es el punto de entrada oficial y autosuficiente para continuar el desarrollo. No es necesario consultar chats anteriores ni baselines previas. Toda afirmación se clasifica como **Closed**, **Validated working copy**, **In progress**, **Planned** o **Deferred**.

# 1. Identidad y visión

Cartridge & Cloud es un simulador de gestión de una tienda de videojuegos para PC/Steam. El jugador organiza el espacio, compra y recibe mercancía, repone productos, atiende clientes, cobra ventas, cierra el día, revisa resultados y reinvierte para hacer crecer el negocio.

La fantasía central combina:

- construcción funcional y legible;
- logística física visible;
- atención a clientes y operaciones diarias;
- economía comprensible;
- progresión gradual desde una tienda pequeña;
- expansión futura hacia empleados, investigación, online, publishing y desarrollo.

# 2. Regla de verdad documental

1. `Documentation/00_Official_Baseline/v0.6` es la baseline vigente y autosuficiente.
2. Las baselines v0.3-v0.5 son históricas e inmutables.
3. El último commit técnico validado de Fase 1 es `091090c43855b0b26b09abe9335d18b978ac7eab`.
4. El commit `d54316c771aab2143993e99b9fd58f2f88016568` contiene una preintegración de prefabs; la working copy local incluye cambios posteriores no asumidos como publicados.
5. Tests verdes no equivalen a aceptación visual ni cierre de sprint.
6. No se declara un gate PASS sin evidencia de compilación, Test Runner, recorrido manual y build cuando corresponda.
7. La working copy debe verificarse en GitHub Desktop antes de continuar o publicar.

# 3. Estado del roadmap

| Sprint | Nombre | Estado |
|---:|---|---|
| 0-15 | Foundation hasta UI/UX Integration | CLOSED / PASS |
| 16 | Art & Audio Representative Pass | IN PROGRESS |
| 17 | Vertical Slice Stabilization | PENDING |

Sprint 16 tiene su Fase 1 funcional validada, pero no está cerrado porque la presentación representativa de la tienda no supera todavía la revisión manual.

# 4. Evidencia técnica aceptada

## 4.1 Fase 1 de Sprint 16

- EditMode: `1215/1215 PASS`.
- PlayMode: `70/70 PASS`.
- Total: `1285/1285 PASS`.
- PlayMode específico de Fase 1: `17/17 PASS` tras las reparaciones de LOD.
- Golden Path técnico y manual previo a la integración representativa: PASS.
- Windows x64 Development Build `0.0.17`: PASS.
- Ejecutable externo: PASS.
- Commit de baseline: `091090c43855b0b26b09abe9335d18b978ac7eab`.

## 4.2 Working copy representativa

- FBX importados y prefabs representativos generados.
- Catálogo representativo enlazado de forma determinista al runtime registry.
- `Validate Representative Integration`: PASS.
- EditMode completo después de integración: PASS.
- PlayMode completo después de integración: PASS.
- Aceptación visual: FAIL / pendiente.
- Build posterior a la integración representativa: no validado todavía.

# 5. Escenas y flujo

Escenas históricas habilitadas:

0. Bootstrap.
1. MainMenu.
2. Store.
3. TestLab.

Flujo actual: `Bootstrap -> MainMenu -> Store -> MainMenu`.

Decisión de Sprint 16: crear `Assets/_Project/Scenes/StoreInitial.unity` como copia controlada de la escena funcional y autorizar manualmente el entorno. La escena todavía no debe darse por creada en esta baseline.

# 6. Store y placement

- Interior: `10 x 15 m`.
- Grid: `20 x 30`.
- Celda: `0,5 m`.
- Entrada central: `2 m` / 4 celdas.
- Paredes objetivo: `3 m` de altura y `0,2 m` de grosor.
- Entrada reservada y mínimo dos celdas contiguas libres.
- Anchors históricos: rear-service `(10,27)`, left-display `(3,15)`, right-display `(16,15)`.
- La validación de acceso usa conectividad ortogonal determinista.

# 7. Sistemas funcionales implementados

- Bootstrap y navegación.
- Tres slots, New Game, Continue, Replace y Delete.
- Save schema, backup y recuperación.
- Input UI/Gameplay, click-to-move, cámara orbital y zoom.
- Grid, placement, rotación, ocupación, retirada y persistencia.
- Productos, inventarios, proveedores, pedidos, entregas y recepción.
- Displays, asignación y restocking.
- Clientes, perfiles, spawning, browsing, shopping, reservas y carrito.
- Cola y checkout.
- Día, apertura, cierre y autosave.
- Money exacto, venta, costes, economía y resultados diarios.
- Operations UI, HUD, ayuda, tutorial y accesibilidad base.
- Audio, VFX y feedback de Fase 1.

# 8. Integración de assets representativos

Rutas funcionales:

- `Assets/_Project/Art/Models/Architecture`.
- `Assets/_Project/Art/Models/Furniture`.
- `Assets/_Project/Art/Models/Products`.
- `Assets/_Project/Prefabs/Architecture`.
- `Assets/_Project/Prefabs/Furniture`.
- `Assets/_Project/Prefabs/Products`.
- `Assets/_Project/Data/Catalogs/RepresentativePrefabCatalog.asset`.
- `Assets/_Project/Settings/Runtime/RuntimeAssetRegistry.asset`.

Fuente Blender local:

`Tools/Blender/CC_Blender_Modular_Kit_v0.1/Production/Exports`

La importación reconoce FBX completos terminados en `_LOD0` con LODs internos. No debe repetirse salvo cambio real de FBX.

# 9. Defectos abiertos y decisión de escena

Defectos visuales actuales:

- muros representativos mal colocados;
- hojas de puerta mal orientadas y con sentido de apertura incorrecto;
- mobiliario estático de warehouse desplazado;
- creación procedural y visual representativa compiten por transforms;
- clic sobre `Vertical Slice Operations` puede propagarse al suelo y mover al jugador.

Decisión: detener la cadena de inferencias automáticas y construir manualmente `StoreInitial.unity` y `StoreInitialEnvironment.prefab`, manteniendo código para registrar y operar una escena ya autorada.

# 10. Próximo trabajo obligatorio

1. Crear `StoreInitial.unity` copiando la escena Store funcional.
2. Crear `StoreInitialEnvironment.prefab`.
3. Montar suelo y arquitectura manualmente.
4. Configurar puerta mediante referencias explícitas.
5. Colocar mobiliario inicial.
6. Añadir anchors y roots técnicos.
7. Crear `StoreInitialSceneContext`.
8. Conectar runtime existente.
9. Desactivar generación procedural del shell.
10. Bloquear interacción de mundo cuando el puntero está sobre UI.
11. Ejecutar regresión, Golden Path y build.

# 11. Reglas operativas

- Preservar `.meta` al reemplazar scripts o assets existentes.
- No borrar fallbacks antes de que la escena manual pase QA.
- No modificar tests para ocultar un fallo real.
- No reimportar FBX para resolver transforms de escena.
- No hacer commit/push sin revisar el diff.
- Separar arquitectura fija, mobiliario inicial y mobiliario dinámico.
- Managers globales, HUD, cámara, EventSystem y persistencia no deben entrar en el prefab de entorno.

# 12. Handoff

Leer en este orden:

1. `CURRENT_PROJECT_HANDOFF.md`.
2. `NEXT_CHAT_START_HERE.md`.
3. Guía Maestra.
4. TDD.
5. Modelo de Datos.
6. Vertical Slice Specification.
7. Roadmap.
8. QA Plan y QA Matrix.
9. `Sprint_16_Current_Status.md`.
10. `StoreInitial_Authoring_Plan.md`.
