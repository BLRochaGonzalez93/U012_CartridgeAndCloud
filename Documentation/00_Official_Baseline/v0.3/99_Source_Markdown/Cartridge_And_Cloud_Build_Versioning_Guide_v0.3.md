---
title: "Cartridge & Cloud - Build & Versioning Guide"
subtitle: "Versiones, ramas, builds, tags y changelog desde cero"
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

# 1. Esquema de versión

SemVer adaptado:

- `v0.0.x`: preproducción, setup y spikes.
- `v0.1.0`: primer bucle técnico integrado.
- `v0.2.0`: prototipo jugable.
- `v0.3.0`: vertical slice.
- `v0.4+`: MVP y expansiones.
- `v1.0.0`: release comercial.

Los números concretos pueden revisarse en Sprint 0, pero no se reutilizan tags del concepto anterior.

# 2. Ramas

`main` estable; ramas `feature/`, `fix/`, `docs/`, `release/`. Desarrollo en solitario puede integrar rápidamente, pero cada cambio debe compilar y pasar smoke tests.

# 3. Tags propuestos

| Tag | Hito |
|---|---|
| v0.0.1-project-foundation | Sprint 0 |
| v0.0.2-bootstrap-scene-flow | Sprint 1 |
| v0.1.0-world-construction-prototype | Sprints 3-5 |
| v0.1.1-inventory-logistics | Sprints 6-8 |
| v0.2.0-customer-sales-loop | Sprints 9-13 |
| v0.2.1-save-ui-integration | Sprints 14-15 |
| v0.3.0-vertical-slice | Sprint 17 |

# 4. Tipos de build

Editor Test, Dev Build, QA Build, Private Tester, Vertical Slice Candidate, Alpha, Beta, RC, Release.

# 5. Nombre

```text
CAC_v0.3.0_VerticalSlice_Windows_x64_2026-XX-XX_Build001.zip
```

# 6. Build metadata

Versión, commit SHA, fecha UTC, tipo, Unity version, save version, content version y entorno.

# 7. Checklist previo

Compilación, tests, validación de contenido, escenas, guardado, core loop, logs, localización, licencias, número de versión y espacio en disco.

# 8. Checklist posterior

Ejecutar fuera del editor, instalación limpia, smoke test, save/load, logs, hash SHA-256, archivado de reporte y actualización de changelog.

# 9. Changelog

Categorías Added, Changed, Fixed, Removed, Known Issues. No incluir afirmaciones no verificadas.

# 10. Compatibilidad de saves

Cada build declara `saveVersion`. Los breaking changes requieren migración o aviso explícito. Las builds de prototipo pueden invalidar saves solo si se documenta antes de distribuirlas.

# 11. Steam

Los depots y ramas Steam no se crean como parte del setup inicial. Se incorporan cuando el Steam Publishing Plan autorice la fase.
