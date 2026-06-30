---
title: "Cartridge & Cloud - Art Bible v0.5"
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

# 1. Dirección

Estilización cálida, limpia y legible desde cámara orbital. Identidad de VRM Games basada en grafito, negro, verde y cyan controlado, con madera cálida y superficies neutras.

# 2. Tienda

La tienda inicial objetivo ocupa 10x15 m. Arquitectura modular alineada a grid, altura visual aproximada de 3 m, fachada legible, puerta automática y zonas de checkout, receiving y backroom.

# 3. Assets

Arquitectura, ocho muebles, seis productos y packaging, characters y expansiones conceptuales. LODs y colliders deben estar presentes, pero no condicionar el placement manual de la escena.

# 4. Reglas

- siluetas reconocibles;
- densidad controlada;
- materiales URP;
- emissive limitado a señalética y acentos;
- cristal transparente legible;
- pivotes y escala coherentes;
- marcas ficticias;
- clientes/proveedores no obligados a la paleta VRM;
- empleados sí mantienen identidad de marca.

# 5. Estado

Los prefabs representativos se cargan, pero la composición procedural no está aprobada. StoreInitial será el target visual oficial de S16.
