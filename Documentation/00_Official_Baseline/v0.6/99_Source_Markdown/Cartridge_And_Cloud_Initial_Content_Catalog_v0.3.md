---
title: "Cartridge & Cloud - Initial Content Catalog v0.3"
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

# 1. Productos funcionales de Sprint 16

| ID | Tipo | Presentación |
|---|---|---|
| `game-neon-drift` | videojuego | caja de juego |
| `case-cloud-runner` | videojuego/case | caja física |
| `console-vertex-one` | consola | consola y packaging |
| `controller-orbit-pad` | mando | controller y packaging |
| `headset-signal-pro` | auriculares | headset y packaging |
| `accessory-memory-core` | accesorio | memory core y packaging |

# 2. Mobiliario funcional

| ID | Rol |
|---|---|
| `checkout-counter` | checkout |
| `wall-shelf` | display de pared |
| `central-shelf` | display central |
| `low-display` | display bajo |
| `featured-display` | display destacado |
| `backroom-storage` | almacenamiento |
| `receiving-crate` | recepción |
| `decoration-plant` | decoración |

# 3. Arquitectura modular

Suelo 1/2 m, bordes, transiciones, muros 1/2/4 m, esquinas, end caps, fachada, cristales, puerta automática, partición de trastienda, señal, zones, raíles de luz, spot, panel lineal y threshold.

# 4. Personajes

Customer, Employee y Supplier tienen presentación representativa de Fase 1, todavía susceptible de mejora artística.

# 5. Normas

- naming ficticio;
- IDs funcionales estables;
- assets visuales intercambiables;
- prefabs con LOD y colliders separados de la lógica;
- no usar marcas reales sin licencia.
