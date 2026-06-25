---
title: "Cartridge & Cloud - Game Design Document v0.5"
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


# 1. Resumen

Simulador de gestión de tienda de videojuegos con construcción funcional, logística, atención a clientes, economía y expansión. La versión actual es una fundación jugable, no el vertical slice completo.

# 2. Público y plataforma

- PC / Steam.
- Jugadores de simulación y gestión accesible.
- Sesiones de progresión pausada con objetivos diarios y semanales.
- Control principal con ratón y teclado.

# 3. Bucle de juego objetivo

1. Abrir la tienda.
2. Revisar stock y pedidos.
3. Recibir mercancía.
4. Colocar y reponer productos.
5. Atender el flujo de clientes.
6. Cobrar ventas.
7. Cerrar y revisar resultados.
8. Reinvertir y ampliar.

# 4. Sistemas

## 4.1 Construcción - Implemented foundation

Grid 0,5 m, huellas rectangulares, rotación, preview, ocupación, retirada y preservación de accesos.

## 4.2 Productos e inventario - Planned Sprint 6

Definiciones, cantidades, contenedores, transferencias y reglas de integridad.

## 4.3 Pedidos y recepción - Planned Sprint 7

Catálogo de proveedor, pedido, entrega y cajas de recepción.

## 4.4 Expositores y reposición - Planned Sprint 8

Asignación de producto, capacidad, unidades visibles y reposición manual.

## 4.5 Clientes - Planned Sprints 9-10

Perfiles, aparición, navegación, evaluación, reserva, carrito y abandono.

## 4.6 Caja - Planned Sprint 11

Cola, interacción del jugador, cálculo de cesta y transacción.

## 4.7 Día y economía - Planned Sprints 12-13

Reloj, apertura/cierre, ingresos, gastos, impuestos y reportes.

## 4.8 Guardado - Skeleton implemented, complete slice planned Sprint 14

Existe un esqueleto versionado mínimo. La persistencia integral del vertical slice está pendiente.

# 5. Espacio actual

Store inicial 10x15 m, entrada central 2 m, grid 20x30. La construcción debe mantener dos celdas contiguas de entrada y conectividad hacia tres anchors internos.

# 6. Progresión futura

- tienda física inicial;
- catálogo más amplio;
- mejoras y ampliaciones;
- empleados;
- investigación;
- puestos informáticos;
- comercio online;
- publishing;
- desarrollo interno;
- plataforma digital y mercado tardío.

# 7. UX

El jugador recibe feedback inmediato de validez. Las operaciones complejas se presentan por paneles contextuales, con HUD reducido durante navegación.

# 8. Contenido del vertical slice

- una tienda funcional;
- catálogo representativo limitado;
- recepción y reposición;
- clientes capaces de comprar;
- caja;
- día completo;
- economía resumida;
- guardado/carga;
- UI, arte y audio representativos;
- build estable.

# 9. Criterios de diversión

- decisiones espaciales con efecto observable;
- sensación de actividad creciente;
- tareas cortas con objetivos claros;
- mejora continua del negocio;
- baja fricción de control;
- feedback claro de errores y resultados.
