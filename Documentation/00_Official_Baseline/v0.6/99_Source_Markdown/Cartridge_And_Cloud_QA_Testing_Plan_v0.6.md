---
title: "Cartridge & Cloud - QA Testing Plan v0.6"
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

# 1. Pirámide

EditMode para dominio/aplicación; PlayMode para Unity/integración; manual para UX/visual; build para ejecución real.

# 2. Baseline automatizada

- EditMode: 1215 PASS.
- PlayMode: 70 PASS.
- Total: 1285 PASS.

# 3. Regresión obligatoria

Scene flow, slots, input, movimiento, cámara, placement, acceso, inventario, proveedores, displays, clientes, shopping, checkout, día, economía, persistencia, UI, audio/VFX y assets representativos.

# 4. QA de StoreInitial

- una sola arquitectura visible;
- suelo coincide con placement surface;
- muros bordean 10x15 m;
- puerta vertical y abre hacia lados opuestos;
- colliders sin bloquear entrada;
- mobiliario inicial en posiciones aprobadas;
- no hay duplicados al cargar save;
- muebles dinámicos reaparecen;
- anchors accesibles;
- UI no propaga clicks;
- wall occlusion funciona.

# 5. Severidad

S0 crash/corrupción; S1 bloqueo/invariante; S2 degradación con workaround; S3 visual. S0/S1 impiden cierre. Los defectos visuales de arquitectura se tratan como S1 para S16 porque bloquean el entregable representativo.

# 6. Gates

Compilación, suites, targeted tests, manual, Golden Path, build externa y revisión de logs.
