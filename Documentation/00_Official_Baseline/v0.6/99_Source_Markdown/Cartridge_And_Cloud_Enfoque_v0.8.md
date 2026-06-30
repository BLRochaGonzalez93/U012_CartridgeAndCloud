---
title: "Cartridge & Cloud - Enfoque v0.8"
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

# 1. Propósito

El proyecto se desarrolla por capas cerrables y verificables. Cada sprint debe aportar una capacidad observable, mantener las invariantes existentes y producir evidencia suficiente para que otro desarrollador pueda continuar sin conocimiento tácito.

# 2. Pilares

## 2.1 Gestión visible

Las decisiones se manifiestan en el espacio: cajas recibidas, stock almacenado, displays repuestos, clientes recorriendo la tienda, colas, ventas y resultados.

## 2.2 Construcción funcional

La colocación afecta circulación, acceso, capacidad y operación. La geometría visual no puede sustituir al modelo lógico de grid y ocupación.

## 2.3 Profundidad gradual

Los sistemas avanzados se mantienen fuera del vertical slice hasta cerrar el bucle base. Empleados, investigación, online, publishing y desarrollo interno permanecen Deferred.

## 2.4 Claridad y control

Cada acción debe explicar su validez, resultado y coste. UI y mundo no deben procesar el mismo clic cuando la UI lo consume.

## 2.5 Autoría explícita del espacio

La arquitectura inicial es contenido fijo autorado, no un resultado que deba deducirse de bounds, nombres de FBX o escalas de placeholders. El runtime registra y opera el entorno; no debe reconstruir visualmente una tienda aprobada.

# 3. Filosofía técnica

- Domain/Application deterministas antes de MonoBehaviours.
- Mutaciones atómicas y validación previa.
- IDs estables independientes del nombre visual.
- Escenas como composición con referencias explícitas.
- ScriptableObjects para catálogos y configuración.
- Prefabs para unidades reutilizables, no para encapsular managers globales.
- TestLab para regresión aislada y Store/StoreInitial para integración real.

# 4. Estado

Sprints 0-15 están cerrados. Sprint 16 continúa abierto por deuda visual. Sprint 17 será el gate de estabilización, balance, rendimiento, QA y build interna.

# 5. Criterio de calidad

Una capacidad se considera cerrada cuando:

- compila sin errores;
- sus invariantes tienen cobertura;
- el recorrido manual reproduce el resultado esperado;
- no hay S0/S1 abiertos;
- el build externo pasa cuando el gate lo requiere;
- documentación, aceptación y trazabilidad están actualizadas.
