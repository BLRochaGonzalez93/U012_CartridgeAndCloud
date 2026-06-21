---
title: "Cartridge & Cloud - Matriz de Impacto y Plan de Migración Documental"
subtitle: "Sustitución del concepto Indie Studio Simulator y reinicio de producción"
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
**Versión del documento:** v0.1  
**Estado:** Preproducción reiniciada desde cero  
**Fuente conceptual:** `Enfoque_v0.6.md`  

> **Regla de interpretación:** el proyecto se documenta como una preproducción nueva. No se presupone código, escenas, managers, datos, builds ni sprints implementados. Todo elemento técnico descrito es una especificación o recomendación hasta que exista evidencia de implementación y QA.

\newpage

# 1. Objetivo

Registrar cómo se transforman los documentos antiguos y asegurar que ninguna versión vigente siga afirmando que existe código o sprints implementados.

# 2. Regla de migración

Las versiones anteriores se conservan como histórico `Pre_Pivot_Indie_Studio_Simulator`. Los documentos nuevos son la única referencia operativa.

# 3. Matriz

| Documento anterior | Acción | Documento nuevo |
|---|---|---|
| Guía Maestra v0.2 | Reescritura | Guide v0.3 |
| GDD v0.3 | Reescritura profunda | GDD v0.4 |
| TDD v0.2 | Reescritura desde cero | TDD v0.3 |
| Modelo de Datos v0.2 | Reescritura profunda | Modelo v0.3 |
| UX Flow v0.2 | Reescritura | UX v0.3 |
| UI Style Guide v0.2 | Adaptación importante | UI v0.3 |
| Art Bible v0.2 | Reescritura visual 3D | Art v0.3 |
| Audio Bible v0.2 | Reescritura | Audio v0.3 |
| QA Plan/Matrix | Reinicio | QA v0.3 |
| Roadmap | Sustitución completa | Roadmap v0.3 Sprint 0 |
| Unity Setup | Sustitución completa | Setup v0.3 |
| Coding Standards | Adaptación | Coding v0.3 |
| Build Guide | Reinicio de versiones | Build v0.3 |
| Legal/Localization | Actualización | v0.3 |
| Marketing/Steam | Reposicionamiento | v0.3 |
| Binder | Reconstrucción al final | Binder v0.3 |
| Exceles | Reinicio de contenido | v0.3 |
| README | Reescritura ES/EN | nuevos README |

# 4. Declaraciones eliminadas

- Sprints completados.
- Managers implementados.
- UI o escenas existentes.
- Datos estáticos ya creados.
- Builds o tags históricos como base activa.
- Próximo sprint 6.
- Estado de prototipo técnico avanzado.

# 5. Declaraciones vigentes

- PC/Steam.
- Unity/C#.
- Desarrollo principalmente en solitario.
- Identidad VRM Games.
- Título provisional Cartridge & Cloud.
- Enfoque v0.6 como fuente conceptual.
- Vertical slice de tienda como primer gran objetivo.

# 6. Resultado

El paquete v0.3 puede utilizarse para iniciar un repositorio/proyecto limpio sin depender del código del concepto anterior.
