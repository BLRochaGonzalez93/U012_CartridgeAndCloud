---
title: "Cartridge & Cloud - Production Roadmap / Sprint Plan v0.6"
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

# 1. Estado

| Sprint | Nombre | Estado | Entregable |
|---:|---|---|---|
| 0 | Project Foundation | CLOSED / PASS | proyecto y build |
| 1 | Bootstrap & Scene Flow | CLOSED / PASS | navegación |
| 2 | Core Data & Save Skeleton | CLOSED / PASS | sesión y slots |
| 3 | Player Movement & Camera | CLOSED / PASS | input/cámara |
| 4 | Grid & Placement Foundation | CLOSED / PASS | placement |
| 5 | Store Shell & Access | CLOSED / PASS | shell técnico |
| 6 | Product & Inventory Core | CLOSED / PASS | inventario |
| 7 | Supplier Orders & Receiving | CLOSED / PASS | pedidos/recepción |
| 8 | Displays & Restocking | CLOSED / PASS | displays |
| 9 | Customer Profiles & Spawning | CLOSED / PASS | clientes |
| 10 | Shopping & Reservations | CLOSED / PASS | compra |
| 11 | Queue & Checkout | CLOSED / PASS | checkout |
| 12 | Day Cycle & Closure | CLOSED / PASS | día |
| 13 | Economy & Reports | CLOSED / PASS | economía |
| 14 | Save/Load Complete Slice | CLOSED / PASS | persistencia |
| 15 | UI/UX Integration | CLOSED / PASS | UI completa |
| 16 | Art & Audio Representative Pass | IN PROGRESS | escena representativa |
| 17 | Vertical Slice Stabilization | PENDING | balance, rendimiento, QA, build |

# 2. Sprint 16 restante

- autoría de StoreInitial;
- prefab del entorno;
- puerta y mobiliario inicial;
- context y conexión runtime;
- desactivación procedural;
- UI click suppression;
- regresión, Golden Path y build.

# 3. Sprint 17

Balance, profiling, allocations, save/recovery, UX/accessibility, bug fixing, full regression, build Windows, Player.log, cierre documental y aprobación del vertical slice.

# 4. Hitos

H0-H5 completados funcionalmente. H6 Vertical Slice Approved permanece pendiente de S16/S17.
