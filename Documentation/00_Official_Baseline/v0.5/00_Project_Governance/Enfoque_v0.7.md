---
title: "Cartridge & Cloud - Enfoque v0.7"
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
**Versión del documento:** v0.7  
**Estado:** Current / Approved  
**Baseline técnica:** `22d62967aaf9895db7bce75afd2ffa11f7858e0c`  
**Baseline documental:** `b650aa5968ecfc9ebe63fc308b0dc8ea2cafae57`


# 1. Propósito

El enfoque de producción prioriza una simulación legible, modular y demostrable. Cada sistema se introduce como una capa cerrable, con pruebas deterministas antes de conectarse a escenas y contenido.

# 2. Fantasía central

El jugador transforma una pequeña tienda de videojuegos en un negocio sólido. La progresión debe sentirse a través del espacio, el surtido, el flujo de clientes, la calidad operativa y las decisiones económicas.

# 3. Pilares

## 3.1 Gestión visible

Las decisiones deben producir cambios observables: mercancía en cajas, productos en expositores, clientes recorriendo pasillos, colas, ventas, gastos y cierres diarios.

## 3.2 Construcción funcional

La colocación no es decorativa. Debe afectar capacidad, circulación, acceso, reposición, visibilidad y rendimiento comercial.

## 3.3 Profundidad gradual

El juego comienza con operaciones simples y añade complejidad por capas. Ningún sistema avanzado debe entrar antes de cerrar el vertical slice.

## 3.4 Claridad y control

El jugador debe comprender por qué una acción es válida o inválida. El feedback visual y sonoro debe ser inmediato y consistente.

# 4. Estado de implementación

## Implemented

- fundación Unity/URP y repositorio;
- Bootstrap, MainMenu, Store y TestLab;
- ApplicationRoot y navegación;
- IDs, sesión y snapshot mínimo;
- contextos UI/Gameplay;
- click-to-move;
- cámara orbital con zoom;
- grid lógico de 0,5 m;
- preview, rotación y ocupación;
- Store inicial 10x15 m;
- reserva de entrada y validación de acceso;
- construcción integrada en Store;
- regresión automatizada 168/168.

## Planned next

- núcleo de producto e inventario;
- pedidos y recepción;
- expositores y reposición;
- clientes y compra;
- caja;
- ciclo diario;
- economía;
- guardado completo;
- UI representativa;
- arte y audio representativos;
- estabilización del vertical slice.

# 5. Filosofía técnica

- Modelos deterministas antes de MonoBehaviours.
- Validación atómica antes de mutación.
- Escenas como composición, no como almacén de lógica.
- Input centralizado y contextual.
- Instaladores Editor idempotentes para cambios complejos de escena.
- TestLab para regresión aislada.
- Store para integración jugable.

# 6. Límites de alcance

El vertical slice no debe incluir todavía empleados complejos, investigación, comercio online, publishing, desarrollo interno, plataforma digital, servicios o metajuego tardío.

# 7. Criterio de calidad

Un sistema está terminado cuando:

- compila limpio;
- sus invariantes tienen cobertura;
- la integración manual funciona;
- la suite completa permanece verde;
- la documentación y trazabilidad están cerradas;
- el build externo pasa cuando el sprint lo exige.
