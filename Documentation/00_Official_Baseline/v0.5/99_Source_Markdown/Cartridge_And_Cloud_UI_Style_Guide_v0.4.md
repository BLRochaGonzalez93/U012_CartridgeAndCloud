---
title: "Cartridge & Cloud - UI Style Guide v0.4"
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
**Versión del documento:** v0.4  
**Estado:** Approved direction / Minimal UI implemented  
**Baseline técnica:** `22d62967aaf9895db7bce75afd2ffa11f7858e0c`  
**Baseline documental:** `b650aa5968ecfc9ebe63fc308b0dc8ea2cafae57`


# 1. Estado actual

MainMenu y ReturnButton son funcionales pero técnicos. La UI completa del vertical slice se integra en Sprint 15.

# 2. Principios

- legibilidad inmediata;
- jerarquía clara;
- paneles compactos;
- consistencia con paleta de marca;
- estados de foco/hover/disabled visibles;
- soporte de localización;
- no depender solo del color.

# 3. Componentes

- HUD de hora/dinero/estado;
- panel de construcción;
- catálogo de pedido;
- inventario y transferencias;
- panel de display;
- checkout;
- informe diario/semanal;
- opciones y accesibilidad;
- tutorial contextual.

# 4. Tokens

- verde principal: `#008F46`.
- fondo oscuro: `#101716`.
- gris texto secundario: `#5B6660`.
- estados success/warning/error con icono y texto.

# 5. Escalado

Diseñar para 16:9, con anchors correctos, safe margins, escalado de texto y pruebas en varias resoluciones de PC.
