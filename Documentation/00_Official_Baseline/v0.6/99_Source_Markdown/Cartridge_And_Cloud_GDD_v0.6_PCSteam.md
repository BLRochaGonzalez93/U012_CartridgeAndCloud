---
title: "Cartridge & Cloud - Game Design Document v0.6"
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

# 1. Resumen

Simulador de gestión de una tienda de videojuegos. El vertical slice ya implementa el bucle funcional completo; Sprint 16 sustituye la presentación provisional por una escena inicial representativa y controlada.

# 2. Público y plataforma

- PC / Steam.
- Jugadores de simulación y gestión accesible.
- Ratón y teclado como control principal.
- Sesiones diarias con progreso persistente.

# 3. Bucle de juego

1. Crear o cargar slot.
2. Preparar la tienda.
3. Comprar equipamiento o producto.
4. Recibir entregas y cajas.
5. Trasladar stock a warehouse.
6. Asignar productos a displays y reponer.
7. Abrir al público.
8. Gestionar clientes y cola.
9. Completar ventas.
10. Cerrar y revisar resultados.
11. Guardar y continuar al siguiente día.

# 4. Sistemas implementados

## Construcción

Grid 0,5 m, huellas, rotación, validación, acceso y persistencia.

## Inventario y pedidos

Definiciones de producto, contenedores, transferencias atómicas, proveedores, órdenes, entregas y recepción.

## Displays

Asignación única de producto, capacidad, stock visible y tareas de reposición.

## Clientes

Perfiles, spawning determinista, navegación técnica, preferencias, paciencia, reservas, carrito y abandono.

## Checkout

FIFO, estación, preflight, consumo físico, transacción económica y cancelación.

## Día y economía

Estados BeforeOpen/Open/Closing/Closed, tiempo lógico, cierre, Money exacto, costes, ingresos y resultados.

## Guardado

Tres slots, schema versionado, backup, recuperación y restauración de estado integrado.

## UI

Main Menu, HUD, Operations, paneles de gestión, tutorial contextual, ayuda y accesibilidad base.

# 5. Tienda inicial

La tienda fija mide 10x15 m. Su arquitectura final se autorará manualmente en `StoreInitial.unity`. El entorno visual no será generado a partir de placeholders. El gameplay seguirá usando grid, colliders y contextos explícitos.

# 6. Contenido representativo

Ocho familias de mobiliario y seis productos iniciales. Marcas ficticias. Identidad VRM negro/verde para tienda y empleados; clientes y proveedores pueden usar paletas propias.

# 7. Progresión post vertical slice

Empleados, investigación, ordenadores, online, publishing, desarrollo interno, plataforma digital y servicios permanecen fuera del cierre de S17.

# 8. Criterios de diversión

- decisiones espaciales con efecto observable;
- actividad creciente y legible;
- tareas cortas encadenadas;
- mejora económica progresiva;
- baja fricción de control;
- feedback claro y consistente.
