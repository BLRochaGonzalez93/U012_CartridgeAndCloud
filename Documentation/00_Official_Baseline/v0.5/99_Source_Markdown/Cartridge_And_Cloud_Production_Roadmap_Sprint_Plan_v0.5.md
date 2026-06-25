---
title: "Cartridge & Cloud - Production Roadmap / Sprint Plan v0.5"
subtitle: "Baseline documental v0.5 - estado tras Sprint 5"
author: "VRM Games / Blas Luis Rocha González"
date: "25/06/2026"
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
  \fancyhead[L]{\small Cartridge \& Cloud - Baseline v0.5}
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
**Versión de aplicación:** `0.0.6`  
**Versión del documento:** v0.5  
**Estado:** Current / Approved  
**Baseline técnica:** `22d62967aaf9895db7bce75afd2ffa11f7858e0c`  
**Baseline documental:** `b650aa5968ecfc9ebe63fc308b0dc8ea2cafae57`


# 1. Estado de ejecución

| Sprint | Nombre | Estado | Evidencia |
|---:|---|---|---|
| 0 | Project Foundation | CLOSED / PASS | fundación, 9 tests y build |
| 1 | Bootstrap & Scene Flow | CLOSED / PASS | 14/14 y build |
| 2 | Core Data & Save Skeleton | CLOSED / PASS | 27/27 |
| 3 | Player Movement & Camera | CLOSED / PASS | 67/67 y build |
| 4 | Grid & Placement Foundation | CLOSED / PASS | 118/118 y build |
| 5 | Store Shell & Access Validation | CLOSED / PASS | 168/168 y build |

# 2. Sprints pendientes del vertical slice

| Sprint | Nombre | Entregable | Dependencias |
|---:|---|---|---|
| 6 | Product & Inventory Core | definiciones, contenedores, transferencias e invariantes | S2/S5 |
| 7 | Supplier Orders & Receiving | catálogo, pedido, entrega, cajas y recepción | S6 |
| 8 | Displays & Restocking | asignación, capacidad y reposición | S6/S7 |
| 9 | Customer Profiles & Spawning | arquetipos, llegada, navegación y paciencia | S3/S5 |
| 10 | Shopping & Reservations | búsqueda, evaluación, reserva, carrito y abandono | S8/S9 |
| 11 | Queue & Checkout | cola, interacción y transacción | S10 |
| 12 | Day Cycle & Store Closure | reloj, velocidades, apertura y cierre | S9-S11 |
| 13 | Economy & Reports | ledger, gastos, impuestos y reportes | S7/S11/S12 |
| 14 | Save/Load Complete Slice | persistencia, backup y migración v1 | S2-S13 |
| 15 | UI/UX Integration | HUD, paneles, tutorial y accesibilidad | S3-S14 |
| 16 | Art & Audio Representative Pass | assets y feedback representativos | S15 |
| 17 | Vertical Slice Stabilization | balance, rendimiento, QA y build interna | S0-S16 |

# 3. Hitos

- H0: proyecto genera build - completado.
- H1: caminar y construir local válido - completado.
- H2: inventario y recepción sin pérdida - siguiente objetivo.
- H3: primer cliente compra.
- H4: primer día completo.
- H5: semana guardada/cargada.
- H6: vertical slice aprobado.

# 4. Regla de apertura de Sprint 6

Congelar charter, acceptance criteria, test plan, versión objetivo y límites. No incluir pedidos ni clientes hasta cerrar el núcleo de inventario.

# 5. Post vertical slice

MVP empleados/investigación/ordenadores; expansión online/publishing/desarrollo; sistemas tardíos de plataforma/servicios/mercado; Alpha/Beta/RC/Steam.
