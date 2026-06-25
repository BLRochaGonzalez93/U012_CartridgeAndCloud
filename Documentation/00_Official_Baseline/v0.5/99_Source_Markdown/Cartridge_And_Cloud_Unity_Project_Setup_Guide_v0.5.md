---
title: "Cartridge & Cloud - Unity Project Setup Guide v0.5"
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


# 1. Requisitos

- Unity `6000.3.18f1`.
- Windows Build Support.
- Visual Studio Community.
- Git/GitHub Desktop.

# 2. Paquetes directos aceptados

- AI Navigation `2.0.13`.
- Visual Studio Editor `2.0.26`.
- Input System `1.19.0`.
- Localization `1.5.12`.
- Universal RP `17.3.0`.
- Test Framework `1.6.0`.
- Unity UI `2.0.0`.

# 3. Identidad

- Company: VRM Games.
- Product: Cartridge & Cloud.
- Application Identifier: `com.vrmgames.cartridgeandcloud`.
- Versión actual: `0.0.6`.

# 4. Escenas habilitadas

0. Bootstrap.
1. MainMenu.
2. Store.
3. TestLab.

Bootstrap debe ser la primera escena del Player.

# 5. Estructura de Assets

- `_Project/Scripts` por assemblies/layers.
- `_Project/Scenes`.
- `_Project/Data`.
- `_Project/Art`.
- `_Project/Editor`.
- `_Project/Tests`.

# 6. Validación de apertura

1. Abrir proyecto.
2. Esperar reimportación.
3. Confirmar cero errores rojos.
4. Ejecutar EditMode.
5. Ejecutar PlayMode.
6. Validar Store y TestLab cuando corresponda.

# 7. Estado esperado

- EditMode 127.
- PlayMode 41.
- Total 168.
- Store inicia sin objetos colocados.
- Construcción inicia desactivada.
