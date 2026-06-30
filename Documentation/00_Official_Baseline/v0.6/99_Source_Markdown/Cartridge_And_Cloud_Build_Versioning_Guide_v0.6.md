---
title: "Cartridge & Cloud - Build & Versioning Guide v0.6"
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

# 1. Versión

Versión de aplicación observada: `0.0.17`.

# 2. Baselines

- Último commit técnico validado: `091090c43855b0b26b09abe9335d18b978ac7eab`.
- Último commit observado en main: `d54316c771aab2143993e99b9fd58f2f88016568`.
- Cambios posteriores de integración pueden estar solo en working copy; registrar su SHA antes de abrir el siguiente sprint.

# 3. Perfil de build

- Windows x64.
- Development Build durante S16-S17.
- Mono para desarrollo salvo decisión posterior.
- LZ4.
- `Builds/Windows/CartridgeAndCloud/` fuera de Git.

# 4. Gate

Compilación, escenas correctas, suite verde, Golden Path, ejecución externa, cierre sin crash y revisión de `Player.log`.

# 5. Estado actual

El build `0.0.17` anterior a la integración representativa pasó. La working copy visual actual debe generar un build nuevo antes de cerrar S16.

# 6. Publicación

No crear tag, release, ZIP de build o checksum de release salvo gate explícito. El cierre de S17 decidirá la build interna de vertical slice.
