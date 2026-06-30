---
title: "Cartridge & Cloud - Unity Project Setup Guide v0.6"
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

# 1. Requisitos

Unity `6000.3.18f1`, URP `17.3.0`, Windows Build Support, Visual Studio y GitHub Desktop.

# 2. Paquetes directos

AI Navigation 2.0.13, Visual Studio Editor 2.0.26, Input System 1.19.0, Localization 1.5.12, URP 17.3.0, Test Framework 1.6.0 y Unity UI 2.0.0.

# 3. Identidad

Company VRM Games, Product Cartridge & Cloud, identifier `com.vrmgames.cartridgeandcloud`, version `0.0.17`.

# 4. Estructura

`Assets/_Project/{Art,Data,Editor,Prefabs,Scenes,Scripts,Settings,Tests}`. Art y Prefabs se organizan por Architecture, Furniture, Products y Characters.

# 5. Escenas

Bootstrap debe permanecer primera. No sustituir Store en Build Profiles hasta que StoreInitial esté conectada y validada. StoreInitial se crea duplicando la escena funcional, con GUID nuevo.

# 6. Apertura

Esperar importación, cero errores rojos, ejecutar EditMode/PlayMode y comprobar registry/catalogs. Safe Mode se resuelve corrigiendo el primer error de compilación, no borrando assets.

# 7. Importación representativa

La herramienta de importación solo se ejecuta cuando cambian FBX. Los archivos principales pueden terminar en `_LOD0` y contener LOD0/1/2 internos. No borrar `.meta` de prefabs existentes.

# 8. Estado esperado

EditMode 1215 y PlayMode 70 PASS en la working copy actual. La escena visual continúa abierta hasta completar StoreInitial.
