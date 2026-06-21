---
title: "Cartridge & Cloud - Project Binder / Índice Maestro"
subtitle: "Índice, versiones vigentes, dependencias y orden de consulta"
author: "VRM Games / Blas Luis Rocha González"
date: "21/06/2026"
lang: es-ES
papersize: a4
fontsize: 10pt
geometry: margin=18mm
toc: true
toc-depth: 3
colorlinks: true
linkcolor: VRMGreen
urlcolor: VRMGreen
citecolor: VRMGreen
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
  \fancyhead[L]{\small Cartridge \& Cloud - Documento interno}
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
**Título técnico provisional:** Cartridge & Cloud  
**Plataforma inicial:** PC / Steam  
**Motor previsto:** Unity 6 LTS / C#  
**Versión del documento:** v0.3  
**Estado:** Preproducción reiniciada desde cero  
**Fuente conceptual:** `Enfoque_v0.6.md`  

> **Regla de interpretación:** el proyecto se documenta como una preproducción nueva. No se presupone código, escenas, managers, datos, builds ni sprints implementados. Todo elemento técnico descrito es una especificación o recomendación hasta que exista evidencia de implementación y QA.

\newpage

# 1. Estado maestro

| Campo | Valor |
|---|---|
| Concepto | Cerrado |
| Código | No iniciado |
| Fase | Preproducción reiniciada |
| Próximo paso | Sprint 0 - Project Foundation |
| Fuente conceptual | Enfoque v0.6 |
| Plataforma | PC/Steam |
| Nombre | Provisional |

# 2. Orden de lectura

1. Package Manifest y Documentation Validation Report.
2. Guide v0.3.
3. Enfoque v0.6.
4. GDD v0.4.
5. Vertical Slice Specification v0.1.
6. TDD y Modelo de Datos v0.3.
7. UX/UI/Art/Audio v0.3.
8. Roadmap y QA v0.3.
9. Excel Maestro, QA Matrix y Trazabilidad.
10. Setup, Coding y Build Guides.
11. Legal, Localization, Marketing y Steam.

# 3. Registro de documentos

| ID | Documento | Versión | Área | Estado |
|---|---|---:|---|---|
| DOC-001 | Guía Maestra | 0.3 | Gobierno | Vigente |
| DOC-002 | Enfoque | 0.6 | Concepto | Vigente |
| DOC-003 | GDD | 0.4 | Diseño | Vigente |
| DOC-004 | Vertical Slice Spec | 0.1 | Diseño/Producción | Vigente |
| DOC-005 | Economy & Balance Spec | 0.1 | Diseño | Vigente |
| DOC-006 | Initial Content Catalog | 0.1 | Contenido | Vigente |
| DOC-007 | TDD | 0.3 | Técnica | Preliminar aprobado |
| DOC-008 | Modelo de Datos | 0.3 | Datos | Preliminar aprobado |
| DOC-009 | UX Flow | 0.3 | UX | Vigente |
| DOC-010 | UI Style Guide | 0.3 | UI | Vigente |
| DOC-011 | Art Bible | 0.3 | Arte | Vigente |
| DOC-012 | Audio Bible | 0.3 | Audio | Vigente |
| DOC-013 | QA Testing Plan | 0.3 | QA | Vigente |
| DOC-014 | QA Matrix | 0.3 | QA | Vigente |
| DOC-015 | Production Roadmap | 0.3 | Producción | Vigente |
| DOC-016 | Excel Maestro | 0.3 | Producción | Vigente |
| DOC-017 | Trazabilidad | 0.3 | Proceso | Vigente |
| DOC-018 | Unity Setup Guide | 0.3 | Técnica | Vigente |
| DOC-019 | C# Coding Standards | 0.3 | Técnica | Vigente |
| DOC-020 | Build & Versioning | 0.3 | Release | Vigente |
| DOC-021 | Legal Register | 0.3 | Legal | Vigente |
| DOC-022 | Localization Plan | 0.3 | Localización | Vigente |
| DOC-023 | Marketing Plan | 0.3 | Marketing | Vigente |
| DOC-024 | Steam Publishing Plan | 0.3 | Steam | Vigente |
| DOC-025 | Migration Plan | 0.1 | Gobierno | Vigente |
| DOC-026 | Package Manifest | 0.1 | Gobierno | Vigente |
| DOC-027 | Documentation Validation Report | 0.1 | Gobierno/QA | Vigente |

# 4. Dependencias

GDD gobierna diseño; TDD/Modelo implementan; UX/UI/Art/Audio presentan; QA verifica; Roadmap programa; Exceles controlan; Legal/Localization/Marketing/Steam preparan publicación.

# 5. Política de versiones

Las versiones del concepto anterior no se editan. Toda decisión nueva incrementa el documento afectado y se registra en trazabilidad.

# 6. Rutina de trabajo

Antes de sprint: consultar GDD, spec, TDD y QA. Durante: actualizar backlog y riesgos. Al cerrar: test run, changelog, trazabilidad, versión y Binder si cambian documentos vigentes.

# 7. Próximo gate

Aprobar Sprint 0 y congelar versión exacta de Unity, repositorio, estructura y primer build vacío.
