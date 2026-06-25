---
title: "Cartridge & Cloud - Economy & Balance Specification v0.2"
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
**Versión del documento:** v0.2  
**Estado:** Planned specification - Economy not implemented  
**Baseline técnica:** `22d62967aaf9895db7bce75afd2ffa11f7858e0c`  
**Baseline documental:** `b650aa5968ecfc9ebe63fc308b0dc8ea2cafae57`


# 1. Principios

- precios y costes legibles;
- margen positivo pero no trivial;
- decisiones espaciales y de stock conectadas a ventas;
- progresión gradual;
- reportes suficientes para aprender sin convertir el juego en una hoja de cálculo.

# 2. Modelo mínimo del vertical slice

| Elemento | Regla inicial |
|---|---|
| Coste de compra | definido por producto/proveedor |
| Precio de venta | editable dentro de límites de balance |
| Impuesto | aplicado al cierre según configuración |
| Gasto fijo | alquiler y servicios simplificados |
| Pérdida | abandono, falta de stock o cierre |
| Beneficio | ingresos - coste vendido - gastos - impuestos |

# 3. Fórmulas objetivo

- `margen_unitario = precio_venta - coste_unitario`.
- `ingreso_venta = suma(precio_unitario * cantidad)`.
- `coste_vendido = suma(coste_unitario * cantidad)`.
- `beneficio_dia = ingresos - coste_vendido - gastos_dia - impuestos`.
- `rotacion_stock = unidades_vendidas / stock_medio`.

# 4. Baseline de contenido

El balance definitivo se realizará cuando existan productos, clientes y ciclo diario. Los valores de este documento son reglas, no datos implementados.

# 5. Telemetría interna recomendada

- ventas por producto;
- ventas perdidas por falta de stock;
- abandono por paciencia;
- tiempo medio en cola;
- ocupación de displays;
- margen diario;
- inventario inmovilizado.
