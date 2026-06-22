---
title: "Cartridge & Cloud - Production Roadmap / Sprint Plan"
subtitle: "Roadmap actualizado tras la fundación técnica"
author: "VRM Games / Blas Luis Rocha González"
date: "22/06/2026"
lang: es-ES
papersize: a4
fontsize: 10pt
geometry: margin=18mm
toc: true
toc-depth: 3
colorlinks: true
linkcolor: VRMGreen
urlcolor: VRMGreen
citecolor: VRMGreen
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
  \fancyhead[L]{\small Cartridge \& Cloud - Documento interno}
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
**Título técnico provisional:** Cartridge & Cloud  
**Plataforma inicial:** PC / Steam  
**Motor validado:** Unity 6.3 LTS `6000.3.18f1` / C# / URP `17.3.0`  
**Versión del documento:** v0.4  
**Estado:** Baseline documental posterior al cierre de Sprint 0
**Fuente conceptual:** `Enfoque_v0.6.md`  

> **Regla de interpretación v0.4:** las afirmaciones de implementación se limitan a la base técnica validada durante Sprint 0. Los sistemas de gameplay y el vertical slice continúan como especificación hasta que exista evidencia posterior.

\newpage

# 0. Estado de ejecución

## Sprint 0 - Project Foundation

**Estado:** Closed / PASS  
**Tag:** `v0.0.1-project-foundation`

Resultados:

- entorno y versión de Unity congelados;
- proyecto URP creado;
- repositorio y reglas de ignorado validados;
- estructura y seis asmdefs establecidas;
- cuatro escenas base creadas;
- nueve smoke tests automatizados;
- perfil Windows reproducible;
- dos builds externas validadas;
- documentación, QA y trazabilidad sincronizadas.

## Próximo bloque recomendado

Antes de implementar el vertical slice completo, el siguiente sprint debe construir el primer bucle técnico visible sobre la fundación: flujo Bootstrap/MainMenu/Store, control/cámara y una interacción mínima verificable. El alcance concreto se congelará al abrir el sprint y no se considera implementado por esta baseline.


# 1. Principios del roadmap

- Sprints pequeños, cerrables y demostrables.
- Cada sprint entrega valor verificable.
- No se inicia una expansión avanzada antes del vertical slice.
- Estimaciones orientativas; el alcance se ajusta por evidencia.
- Cada sprint incluye QA, documentación y trazabilidad.

# 2. Fases de producto

| Fase | Resultado |
|---|---|
| R0 | Documentación y setup |
| P1 | Mundo/control |
| P2 | Construcción |
| P3 | Productos/logística |
| P4 | Clientes/ventas |
| P5 | Día/economía/save |
| VS | Vertical slice |
| MVP | Empleados, investigación y ordenadores |
| EXP | Online, publishing y desarrollo |
| LATE | Plataforma, infraestructura y mercado |
| REL | Alpha, Beta y lanzamiento |

# 3. Roadmap de sprints desde cero

| Sprint | Nombre | Entregable principal | Dependencias |
|---|---|---|---|
| 0 | Project Foundation | Unity, Git, paquetes, carpetas, asmdefs, CI local | Documentación |
| 1 | Bootstrap & Scene Flow | Bootstrap, MainMenu vacío, StorePrototype y TestLab | S0 |
| 2 | Core Data & Save Skeleton | IDs, configuración, GameSession, slots y snapshot mínimo | S1 |
| 3 | Player Movement & Camera | Click-to-move, orbit, zoom y input contexts | S1 |
| 4 | Grid & Placement Foundation | Grid 0,5 m, ghost, rotación y validación base | S3 |
| 5 | Store Shell & Access Validation | Tienda 10x10, puertas, interacción y bloqueo de accesos | S4 |
| 6 | Product & Inventory Core | Definiciones, contenedores, transferencias e invariantes | S2/S5 |
| 7 | Supplier Orders & Receiving | Catálogo, pedido, entrega, cajas y recepción | S6 |
| 8 | Displays & Restocking | Asignación, capacidad, estados visuales y reposición | S6/S7 |
| 9 | Customer Profiles & Spawning | Arquetipos, llegada, navegación y paciencia | S3/S5 |
| 10 | Shopping & Reservations | Búsqueda, evaluación, reserva, carrito y abandono | S8/S9 |
| 11 | Queue & Checkout | Cola, interacción de jugador y transacción | S10 |
| 12 | Day Cycle & Store Closure | Reloj, velocidades, apertura, 21:45, 22:00 y resolución | S9-S11 |
| 13 | Economy & Reports | Libro mayor, gastos, impuesto, resumen diario/semanal | S7/S11/S12 |
| 14 | Save/Load Complete Slice | Persistencia completa, backup y migración v1 | S2-S13 |
| 15 | UI/UX Integration | HUD, paneles, tutorial y accesibilidad base | S3-S14 |
| 16 | Art & Audio Representative Pass | Assets representativos y feedback sonoro | S15 |
| 17 | Vertical Slice Stabilization | Balance, rendimiento, QA y build interna | S0-S16 |

# 4. Criterios de cierre por sprint

- Entregable demostrable en escena o test.
- Acceptance criteria pasados.
- Sin S0/S1.
- Save actualizado cuando corresponda.
- QA Matrix y trazabilidad actualizadas.
- Tag/build si el sprint cierra un sistema.

# 5. Roadmap posterior al vertical slice

| Bloque | Sistemas |
|---|---|
| MVP-1 | Empleados, tareas, fatiga, salarios |
| MVP-2 | Investigación y ampliaciones |
| MVP-3 | Puestos informáticos |
| EXP-1 | Comercio online y logística |
| EXP-2 | Publishing |
| EXP-3 | Desarrollo interno |
| LATE-1 | Plataforma digital |
| LATE-2 | Infraestructura y servicios |
| LATE-3 | Mercado y competidores |
| REL | Contenido, balance, Alpha, Beta, RC, Steam |

# 6. Hitos

- H0: proyecto vacío compila y genera build.
- H1: caminar y construir local válido.
- H2: inventario y recepción sin pérdida.
- H3: primer cliente compra.
- H4: primer día completo.
- H5: semana guardada/cargada.
- H6: vertical slice aprobado.
- H7: MVP de tienda.
- H8: negocio online.
- H9: ecosistema editorial.
- H10: plataforma y victoria.

# 7. Riesgos de planificación

Scope, dependencia de assets, NavMesh dinámico, volumen de UI, sistemas avanzados y carga documental. Se mitigan con spikes, prototipos, contenido mínimo y gates de fase.

# 8. Backlog por prioridad

P0 vertical slice; P1 MVP tienda; P2 expansión; P3 postgame/pulido. Ningún P2 entra en sprint antes de cerrar H6 salvo spike aislado aprobado.

# 9. Revisión semanal

Comparar plan vs realizado, bugs, riesgos, métricas, deuda, documentación y siguiente sprint. Reestimar sin ocultar desviaciones.
