---
title: "Cartridge & Cloud - QA Testing Plan v0.5"
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


# 1. Objetivo

Mantener una baseline reproducible mientras el vertical slice crece por sprints pequeños.

# 2. Pirámide

- EditMode: value objects, cálculos, políticas, validación, inventario futuro.
- PlayMode: componentes, escena, input y composición.
- Manual: UX, cámara, flujo y comportamiento del Player.
- Build: Windows x64 y ejecución externa en gates de fase.

# 3. Baseline actual

- EditMode: 127.
- PlayMode: 41.
- Total: 168.
- Resultado: PASS.

# 4. Regresión obligatoria

## Scene flow

Bootstrap, MainMenu, Store, retorno, direct Store y Quit.

## Input/movement/camera

Contextos exclusivos, click-to-move, órbita y zoom.

## Placement

Preview, rotación, bounds, overlap, confirmación, cancelación y retirada.

## Store access

Entrada, anchors y conectividad.

## Save skeleton

Versionado mínimo y contratos existentes.

# 5. Sprint 6

Debe añadir tests para:

- IDs y definiciones de producto;
- cantidades válidas;
- contenedores;
- capacidad;
- transferencias válidas;
- transferencia insuficiente;
- conservación de unidades;
- atomicidad ante error;
- snapshots o DTOs solo si entran en alcance.

# 6. Severidad

- S0: crash, corrupción o pérdida de datos.
- S1: bloqueo de flujo principal o invariante rota.
- S2: función degradada con workaround.
- S3: visual/cosmético.

Ningún S0/S1 abierto permite cerrar sprint.

# 7. Evidencia

Cada cierre registra compilación, tests, manual, build si aplica, commits, acceptance, trazabilidad y defectos.
