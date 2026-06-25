---
title: "Cartridge & Cloud - Guía Maestra v0.5"
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


> Esta guía es el punto de entrada oficial para continuar el proyecto después del cierre de Sprint 5. Solo se considera implementado aquello que dispone de evidencia técnica, automatizada y manual en los registros de Sprints 0-5.

# 1. Identidad y visión

Cartridge & Cloud es un simulador de gestión de tienda de videojuegos para PC/Steam. El jugador abre, organiza y desarrolla una tienda física, gestiona productos y operaciones diarias y, a largo plazo, amplía el negocio hacia actividades digitales, editoriales y de desarrollo.

La experiencia combina:

- construcción y organización espacial;
- logística de producto e inventario;
- atención de clientes y ventas;
- economía y progresión;
- decisiones operativas con consecuencias visibles;
- evolución gradual de una tienda local a un negocio más amplio.

# 2. Regla de verdad documental

1. La baseline oficial v0.5 es inmutable una vez publicada.
2. Los cambios posteriores se documentan en `Documentation/10_Development_Records`.
3. Una afirmación de implementación exige evidencia en código, pruebas y cierre de sprint.
4. Las especificaciones futuras se marcan como **Planned** o **Deferred**.
5. El repositorio `main` y los commits de cierre son la fuente técnica definitiva.

# 3. Estado actual del producto

## 3.1 Sprints cerrados

| Sprint | Nombre | Estado | Resultado principal |
|---:|---|---|---|
| 0 | Project Foundation | CLOSED / PASS | Proyecto, repositorio, escenas, assemblies, QA y build foundation |
| 1 | Bootstrap & Scene Flow | CLOSED / PASS | ApplicationRoot, Bootstrap, MainMenu, Store y navegación |
| 2 | Core Data & Save Skeleton | CLOSED / PASS | IDs, sesión, slots y snapshot versionado mínimo |
| 3 | Player Movement & Camera | CLOSED / PASS | Input contexts, click-to-move, órbita y zoom |
| 4 | Grid & Placement Foundation | CLOSED / PASS | Grid 0,5 m, preview, rotación, ocupación y retirada |
| 5 | Store Shell & Access Validation | CLOSED / PASS | Store 10x15 m, acceso, construcción integrada y versión 0.0.6 |

## 3.2 Evidencia aceptada

- EditMode: `127/127 PASS`.
- PlayMode: `41/41 PASS`.
- Suite completa: `168/168 PASS`.
- Build Windows x64 Development: PASS.
- Ejecución externa: PASS.
- Sin defectos bloqueantes conocidos.

## 3.3 Escenas

| Índice | Escena | Función |
|---:|---|---|
| 0 | Bootstrap | Crea el ApplicationRoot persistente y dirige el arranque |
| 1 | MainMenu | Entrada principal, acceso a Store y Quit |
| 2 | Store | Escena jugable y de construcción actual |
| 3 | TestLab | Laboratorio técnico y regresión aislada |

# 4. Baseline jugable actual

## 4.1 Flujo

`Bootstrap -> MainMenu -> Store -> MainMenu`

La ejecución directa de Store redirige correctamente mediante Bootstrap cuando es necesario. ApplicationRoot permanece único y persistente.

## 4.2 Control y cámara

- Clic izquierdo sobre suelo: destino del jugador fuera de construcción.
- Arrastre derecho: órbita.
- Rueda: zoom.
- Contextos UI y Gameplay mutuamente exclusivos.
- Durante construcción, el clic de confirmación no mueve al jugador.

## 4.3 Construcción

- `B`: entrar/salir de construcción.
- Clic izquierdo: confirmar una colocación válida.
- `Q/E`: rotar 90 grados.
- `Escape`: cancelar.
- `Delete/Backspace`: retirar.
- Preview verde: válido.
- Preview rojo: fuera de límites, solape o bloqueo de acceso.

## 4.4 Store inicial

- Interior: `10 x 15 m`.
- Grid: `20 x 30` celdas.
- Tamaño de celda: `0,5 m`.
- Entrada central: `2 m` / 4 celdas.
- Anchura mínima libre: 2 celdas adyacentes.
- Altura de paredes: `3 m`.
- Grosor de paredes: `0,2 m`.
- Objeto técnico: estantería `4 x 2`.

## 4.5 Accesibilidad espacial

La colocación valida:

1. límites del grid;
2. ocupación existente;
3. reserva de entrada;
4. anchura mínima de entrada;
5. tres anchors obligatorios;
6. conectividad ortogonal desde entrada a anchors.

# 5. Arquitectura

La arquitectura mantiene separación entre:

- **Domain:** tipos y reglas puras;
- **Application:** casos de uso y cálculos deterministas;
- **Infrastructure:** persistencia, input y adaptadores;
- **Presentation:** MonoBehaviours, escenas y vistas;
- **Editor:** instaladores y validadores idempotentes;
- **Tests:** EditMode y PlayMode por responsabilidad.

Las dependencias deben apuntar hacia dentro. Domain y Application no deben depender de Unity salvo excepciones aprobadas explícitamente.

# 6. Sistemas no implementados todavía

- Product & Inventory Core.
- Supplier Orders & Receiving.
- Displays & Restocking.
- Customer Profiles & Spawning.
- Shopping & Reservations.
- Queue & Checkout.
- Day Cycle & Store Closure.
- Economy & Reports.
- Save/Load Complete Slice.
- UI/UX Integration.
- Representative Art & Audio Pass.
- Vertical Slice Stabilization.

# 7. Roadmap inmediato

El siguiente sprint es **Sprint 6 - Product & Inventory Core**. Debe construir definiciones de producto, contenedores, cantidades, transferencias e invariantes sin introducir todavía pedidos, clientes ni economía final.

# 8. Convenciones operativas

- Integrar por paquetes pequeños y revisables.
- Preservar `.meta` de archivos existentes.
- No declarar PASS antes de compilar y ejecutar la suite completa.
- La validación estática no sustituye Unity Test Runner.
- No crear tags, releases o checksums por sprint salvo gate explícito.
- Cada cierre actualiza QA, aceptación, trazabilidad y documentación viva.

# 9. Handoff

El documento `CURRENT_PROJECT_HANDOFF.md` contiene el resumen compacto que debe proporcionarse al nuevo chat antes de iniciar Sprint 6.
