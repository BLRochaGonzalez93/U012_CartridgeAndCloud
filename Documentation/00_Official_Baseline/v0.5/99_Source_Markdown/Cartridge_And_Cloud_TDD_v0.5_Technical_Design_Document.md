---
title: "Cartridge & Cloud - Technical Design Document v0.5"
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


# 1. Arquitectura

La solución aplica una arquitectura por capas y assemblies con dependencias dirigidas hacia Domain/Application.

## Domain

- identificadores y value objects;
- grid, tamaños, rotaciones y huellas;
- registros de objetos colocados;
- IDs de acceso;
- reglas sin Unity.

## Application

- sesión y snapshot;
- cálculos de movimiento/cámara;
- proyección de grid;
- ocupación;
- validación de acceso;
- políticas de input y futuros casos de uso.

## Infrastructure

- persistencia de archivos futura;
- Input System y adapters;
- composición del ApplicationRoot;
- servicios de navegación.

## Presentation

- controladores de escena;
- player movement;
- cámara;
- superficies, previews y vistas de placement;
- Store shell descriptor.

## Editor

- instaladores idempotentes;
- validadores de escena y baseline;
- comandos por sub-sprint.

# 2. Sistemas implementados

## 2.1 Scene flow

ApplicationRoot persistente, navegación asíncrona con rechazo de transición concurrente y rutas Bootstrap/MainMenu/Store.

## 2.2 Save skeleton

IDs y snapshots versionados mínimos. No persiste todavía el vertical slice completo.

## 2.3 Input

Maps UI y Gameplay, router de contexto, frame adapters y políticas para evitar conflicto entre movimiento y construcción.

## 2.4 Movement/camera

Movimiento planar mediante CharacterController y cámara orbital/zoom con target técnico.

## 2.5 Grid/placement

- coordenadas enteras X/Z;
- celda 0,5 m;
- rotaciones 0/90/180/270;
- huella determinista;
- validación bounds/overlap;
- ocupación y retirada atómicas;
- preview visual.

## 2.6 Store/access

- shell 10x15 m;
- surface 20x30;
- entrada reservada;
- anchors;
- BFS ortogonal;
- integración opcional de acceso en runtime de placement.

# 3. Invariantes

- IDs no vacíos y estables.
- No se confirma una huella fuera de bounds.
- No se solapan ocupantes.
- No se muta ocupación al validar candidato.
- Store conserva mínimo dos celdas adyacentes de entrada.
- Todos los anchors permanecen alcanzables.
- ApplicationRoot es único.
- UI y Gameplay no se habilitan simultáneamente.

# 4. Estrategia de pruebas

- Domain/Application: EditMode.
- MonoBehaviours y integración: PlayMode.
- Scene smoke y build: gates de cierre.
- TestLab: regresión aislada.
- Store: integración jugable.

# 5. Próxima arquitectura - Sprint 6

Introducir:

- ProductDefinitionId y ProductDefinition;
- InventoryContainerId;
- Quantity/value objects;
- InventoryStack o slot model;
- transferencias atómicas;
- invariantes de capacidad y producto;
- tests sin dependencia de escenas.

No introducir todavía proveedores, pedidos o clientes.

# 6. Riesgos

- crecimiento de assemblies y referencias;
- serialización prematura;
- acoplamiento de inventario a GameObjects;
- mutaciones parciales;
- escenas como estado autoritativo;
- deuda de UI y contenido.
