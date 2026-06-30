---
title: "Cartridge & Cloud - Document Migration Plan v0.3"
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

# 1. Origen y destino

Origen: baseline v0.5, registros operativos de Sprints 6-16, commits técnicos y validaciones comunicadas durante la integración representativa.

Destino: baseline autosuficiente v0.6.

# 2. Regla de migración

- Se conserva la visión y los límites de diseño.
- Se sustituyen secciones de estado obsoletas.
- Se consolidan Sprints 6-15 como cerrados.
- Sprint 16 se registra como abierto pese a tests verdes.
- Se documentan cambios locales posteriores al último commit sin atribuirles un SHA inexistente.
- Se mantiene la baseline v0.5 intacta.

# 3. Elementos migrados

- 25 documentos normativos principales.
- Tres libros Excel.
- Handoff completo.
- Historia resumida de Sprints 0-15.
- Registros detallados de Sprint 16.
- ADR index actualizado.
- Known Issues y plan de escena manual.
- Manifiesto y checksums.

# 4. Elementos no arrastrados literalmente

Los registros granulares históricos de Sprints 0-5 permanecen archivados en v0.5. v0.6 incluye su resumen suficiente para continuidad, evitando duplicación innecesaria.

# 5. Criterio de finalización

La migración se considera válida cuando todos los archivos listados existen, los PDFs se renderizan, los XLSX se abren, los checksums coinciden y el handoff no depende de conversaciones externas.
