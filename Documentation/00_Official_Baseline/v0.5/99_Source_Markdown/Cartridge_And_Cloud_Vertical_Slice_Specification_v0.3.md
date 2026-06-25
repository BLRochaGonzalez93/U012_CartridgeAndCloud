---
title: "Cartridge & Cloud - Vertical Slice Specification v0.3"
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
**Versión del documento:** v0.3  
**Estado:** Approved specification / Partially implemented  
**Baseline técnica:** `22d62967aaf9895db7bce75afd2ffa11f7858e0c`  
**Baseline documental:** `b650aa5968ecfc9ebe63fc308b0dc8ea2cafae57`


# 1. Objetivo

Demostrar el bucle completo de una tienda de videojuegos en una sesión interna representativa, desde apertura hasta cierre y guardado.

# 2. Alcance mínimo

## Implementado ya

- arranque y navegación;
- Store jugable;
- control/cámara;
- construcción y acceso;
- save skeleton;
- testing y build foundation.

## Pendiente

- productos e inventario;
- pedidos y recepción;
- displays y reposición;
- clientes y compra;
- cola y checkout;
- ciclo de día;
- economía/reportes;
- save/load completo;
- UI/UX integrada;
- arte/audio representativo;
- estabilización.

# 3. Escenario de demostración

1. Cargar una partida o crear sesión.
2. Entrar en Store.
3. Colocar equipamiento sin bloquear accesos.
4. Pedir mercancía.
5. Recibir cajas.
6. Transferir productos a almacén y expositores.
7. Abrir al público.
8. Ver clientes buscar y comprar.
9. Cobrar en caja.
10. Cerrar el día.
11. Revisar resultados.
12. Guardar, salir y recargar.

# 4. Gates

- cero pérdida o duplicación de inventario;
- transacciones atómicas;
- acceso espacial válido;
- flujo de cliente sin bloqueo;
- cierre diario determinista;
- guardado migrable;
- suite automatizada verde;
- build Windows x64 ejecutable externamente.

# 5. Fuera del vertical slice

Empleados completos, investigación, puestos informáticos, comercio online, publishing, desarrollo interno, plataforma digital y sistemas tardíos.
