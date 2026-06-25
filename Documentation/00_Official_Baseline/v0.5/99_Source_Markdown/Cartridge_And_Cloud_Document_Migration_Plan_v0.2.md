---
title: "Cartridge & Cloud - Plan de Migración Documental v0.2"
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
**Versión del documento:** v0.2  
**Estado:** Current / Approved  
**Baseline técnica:** `22d62967aaf9895db7bce75afd2ffa11f7858e0c`  
**Baseline documental:** `b650aa5968ecfc9ebe63fc308b0dc8ea2cafae57`


# 1. Objetivo

Migrar de la baseline v0.4, centrada en la fundación técnica, a una baseline v0.5 que incorpore el estado validado de Sprints 1-5 sin alterar la evidencia histórica.

# 2. Principios

- v0.4 permanece intacta.
- v0.5 se añade como nueva carpeta oficial.
- Los cierres originales se copian a `06_Development_Records`.
- Las afirmaciones futuras siguen marcadas como especificación.
- Los libros operativos se regeneran desde evidencia consolidada.

# 3. Transformaciones

| Área | Acción |
|---|---|
| Gobernanza | actualizar guía, enfoque, binder, manifiesto e integridad |
| Diseño | separar visión futura de sistemas implementados |
| Técnica | documentar arquitectura, input, grid, placement, Store y acceso |
| QA | fijar baseline 127/41/168 y roadmap 6-17 |
| Producción | registrar estado CLOSED/PASS de Sprints 0-5 |
| Handoff | añadir continuidad operativa para nuevo chat |

# 4. Validación

- manifiesto completo;
- checksums SHA-256;
- PDFs renderizables;
- hojas XLSX legibles;
- cero archivos de código o assets Unity;
- enlaces y rutas coherentes;
- ausencia de estados Pending en Sprints 0-5.
