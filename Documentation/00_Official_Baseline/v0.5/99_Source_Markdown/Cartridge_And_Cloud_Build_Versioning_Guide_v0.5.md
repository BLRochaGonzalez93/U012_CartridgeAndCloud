---
title: "Cartridge & Cloud - Build & Versioning Guide v0.5"
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


# 1. Versión actual

`0.0.6` - cierre de Sprint 5.

# 2. Convención

Durante preproducción técnica se incrementa el patch por sprint cerrado cuando el gate lo exige. El versionado no implica release pública.

# 3. Build profile

- Plataforma: Windows x64.
- Scripting Backend de desarrollo: Mono.
- Development Build.
- Compresión LZ4.
- Salida local: `Builds/Windows/CartridgeAndCloud/`.
- `Builds/` fuera de Git.

# 4. Gates

Un build de cierre debe:

- compilar;
- incluir escenas correctas;
- abrir externamente;
- completar el flujo requerido;
- cerrar sin crash;
- registrar Player.log cuando existe un trigger de fallo;
- mantener la suite verde.

# 5. Tags y releases

- Sprint 0 generó la fundación etiquetada.
- Sprints intermedios no crean tag/release por defecto.
- Build ZIP y checksum se reservan para hitos de fase o solicitud explícita.

# 6. Próximo versionado

Sprint 6 debe congelar su versión objetivo al abrirse. No se asume automáticamente `0.0.7` hasta aprobar el charter del sprint.
