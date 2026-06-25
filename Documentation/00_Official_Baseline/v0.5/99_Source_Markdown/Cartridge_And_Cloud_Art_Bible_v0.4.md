---
title: "Cartridge & Cloud - Art Bible v0.4"
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
**Versión del documento:** v0.4  
**Estado:** Approved direction / Technical blockout implemented  
**Baseline técnica:** `22d62967aaf9895db7bce75afd2ffa11f7858e0c`  
**Baseline documental:** `b650aa5968ecfc9ebe63fc308b0dc8ea2cafae57`


# 1. Dirección

Estilización cálida y legible, con identidad de tienda independiente, materiales agradables y formas claras. Evitar realismo excesivo y ruido visual.

# 2. Estado actual

- Store técnica 10x15 m.
- Primitivas y materiales técnicos.
- Colores de feedback de placement.
- Marcadores de acceso temporales.
- No existe todavía el pase representativo final.

# 3. Principios

- siluetas reconocibles;
- contraste entre suelo, paredes, equipamiento y productos;
- lectura clara desde cámara orbital;
- densidad controlada;
- señalética ficticia coherente;
- assets modulares alineados a grid 0,5 m.

# 4. Paleta funcional

- madera y crema para calidez;
- verde/teal para identidad y validez;
- rojo para invalidez;
- azul técnico para objetos colocados durante prototipo;
- amarillo para anchors/advertencias temporales.

# 5. Producción futura

Sprint 16 sustituirá placeholders por un set representativo: shell, mostrador, estanterías, cajas, productos, cliente y feedback visual suficiente para evaluación interna.
