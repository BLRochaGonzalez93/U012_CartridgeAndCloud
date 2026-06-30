---
title: "Cartridge & Cloud - Vertical Slice Specification v0.4"
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

# 1. Objetivo

Demostrar en una build interna estable un día completo de gestión, desde carga de slot hasta cierre, resultados, guardado y recarga.

# 2. Alcance funcional

El bucle funcional está implementado y automatizado: escena, placement, producto, inventario, pedidos, recepción, displays, clientes, compra, cola, checkout, día, economía, persistencia y UI.

# 3. Alcance representativo pendiente

- escena inicial autorada;
- arquitectura correctamente colocada;
- puerta funcional y visualmente correcta;
- mobiliario inicial en posiciones aprobadas;
- productos y displays visibles;
- iluminación y audio representativos;
- ausencia de placeholders visibles;
- input UI sin propagación al mundo.

# 4. Escenario Golden Path

1. Arrancar desde Bootstrap.
2. Crear o cargar slot.
3. Entrar en StoreInitial.
4. Comprar producto y mobiliario.
5. Recibir pedido.
6. Transferir a warehouse.
7. Colocar/usar displays.
8. Reponer producto.
9. Abrir tienda.
10. Cliente compra y pasa por checkout.
11. Cerrar día y revisar resultado.
12. Guardar, salir y recargar.
13. Verificar persistencia.

# 5. Gates

- cero pérdida/duplicación de inventario;
- acceso y navegación válidos;
- UI no dispara acciones de mundo;
- transacciones atómicas;
- escena visualmente aprobada;
- suite completa verde;
- Golden Path manual PASS;
- Windows x64 Development Build PASS;
- ejecutable externo PASS;
- `Player.log` sin errores bloqueantes.

# 6. Fuera de alcance

Empleados completos, investigación, online, publishing, desarrollo interno, plataforma y servicios tardíos.
