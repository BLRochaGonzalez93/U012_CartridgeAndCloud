---
title: "Cartridge & Cloud - Project Binder / Índice Maestro v0.6"
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

# 1. Objetivo

Este índice define la jerarquía de verdad de la baseline v0.6 y el orden de consulta recomendado.

# 2. Jerarquía documental

1. Guía Maestra y Current Project Handoff.
2. Enfoque, GDD y Vertical Slice Specification.
3. TDD, Modelo de Datos y Unity Setup Guide.
4. Roadmap, QA Plan, QA Matrix y Excel Maestro.
5. Art/Audio/UI bibles.
6. Registros de Sprint 16 y Known Issues.
7. Business/Release, mientras no contradiga los documentos anteriores.

# 3. Índice

| Área | Documento principal | Función |
|---|---|---|
| Governance | Guía Maestra v0.6 | verdad operativa y estado |
| Governance | Enfoque v0.8 | pilares y límites |
| Governance | Current Project Handoff | arranque de un nuevo chat |
| Design | GDD v0.6 | experiencia y sistemas |
| Design | Vertical Slice Spec v0.4 | alcance y gates |
| Design | UX Flow v0.5 | flujos y input |
| Technical | TDD v0.6 | arquitectura y runtime |
| Technical | Modelo de Datos v0.5 | entidades e invariantes |
| Technical | Unity Setup v0.6 | proyecto, escenas y assets |
| QA | Roadmap v0.6 | sprints y entregables |
| QA | QA Plan v0.6 | estrategia y gates |
| QA | QA Matrix v0.6 | casos ejecutables |
| QA | Excel Maestro v0.6 | planificación consolidada |
| Visual | Art Bible v0.5 | dirección visual y assets |
| Visual | Audio Bible v0.5 | eventos y mezcla |
| Visual | UI Style Guide v0.5 | HUD y paneles |
| Development | Sprint 16 Current Status | estado no ambiguo |
| Development | StoreInitial Authoring Plan | transición a escena manual |
| Historical | Sprints 00-15 Closure Ledger | resumen autosuficiente de cierres |
| Historical | Sprint 16 Integration Timeline | integración, hotfixes y deuda visual |

# 4. Estados permitidos

- `CLOSED / PASS`: gate completo y evidencia archivada.
- `VALIDATED WORKING COPY`: tests y comportamiento comprobados, pero no necesariamente publicados.
- `IN PROGRESS`: alcance abierto.
- `BLOCKED`: impedimento activo.
- `PLANNED`: aprobado pero no implementado.
- `DEFERRED`: fuera de la fase actual.

# 5. Política de contradicciones

El estado más reciente y específico prevalece. Los registros de Sprint 16 prevalecen sobre descripciones históricas de v0.5. Nunca se interpretará un placeholder histórico como contenido aprobado si el registro actual indica deuda visual.
