---
title: "Cartridge & Cloud - Project Binder / Índice Maestro v0.5"
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


# 1. Jerarquía documental

1. Guía Maestra v0.5.
2. Enfoque v0.7.
3. GDD v0.5.
4. Vertical Slice Specification v0.3.
5. TDD v0.5.
6. Modelo de Datos v0.4.
7. UX Flow v0.4.
8. Production Roadmap / Sprint Plan v0.5.
9. QA Testing Plan y Matrix v0.5.
10. Build & Versioning Guide v0.5.
11. Registros de Sprints 0-5.
12. Handoff actual.

# 2. Estructura de la baseline

| Carpeta | Contenido |
|---|---|
| `00_Project_Governance` | guía, enfoque, binder, migración, validación y manifiesto |
| `01_Design` | GDD, vertical slice, economía, catálogo y UX |
| `02_Technical` | TDD, datos, código, setup y build/versionado |
| `03_Visual_Audio` | art bible, audio bible y UI style guide |
| `04_QA_Production` | roadmap y libros maestros de producción, QA y trazabilidad |
| `05_Business_Release` | legal, localización, marketing y Steam |
| `06_Development_Records` | cierres, ADRs, evidencias y handoff |
| `99_Source_Markdown` | fuentes editables de los documentos PDF |

# 3. Política de lectura para un nuevo chat

Leer en este orden:

1. `CURRENT_PROJECT_HANDOFF.md`.
2. Guía Maestra.
3. Roadmap.
4. TDD.
5. Modelo de Datos.
6. QA Testing Plan.
7. Sprint 5 Closure Report.
8. Documentación específica del sprint que vaya a abrirse.

# 4. Estado de baseline

- v0.4: baseline tras Sprint 0, inmutable.
- v0.5: baseline tras Sprint 5, actual y autosuficiente.
- Los registros futuros se mantienen fuera de la baseline hasta la siguiente consolidación oficial.
