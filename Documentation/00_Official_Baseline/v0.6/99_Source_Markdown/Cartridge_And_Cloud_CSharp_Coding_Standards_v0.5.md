---
title: "Cartridge & Cloud - C# Coding Standards v0.5"
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

# 1. Principios

- Código legible, determinista y testeable.
- Una responsabilidad por tipo.
- Dependencias explícitas.
- No ocultar estado global mediante búsquedas de escena cuando puede serializarse una referencia.
- Validar argumentos e invariantes en límites del dominio.

# 2. Naming

- PascalCase: tipos, métodos y propiedades.
- camelCase: parámetros y variables locales.
- `_camelCase`: campos privados serializados.
- IDs de contenido: kebab-case estable.
- Namespaces: `VRMGames.CartridgeAndCloud.<Layer>.<Feature>`.

# 3. Unity

- MonoBehaviours finos.
- No usar nombres de GameObject como identidad autoritativa.
- Evitar `FindFirstObjectByType` como mecanismo principal.
- Referencias de escena por `StoreInitialSceneContext`.
- Editor scripts idempotentes.
- Preservar `.meta`.

# 4. Errores

- Excepciones para configuración inválida y estados imposibles.
- Result objects para validación de gameplay esperable.
- No capturar excepciones sin registrar contexto.
- No convertir warnings de Unity en ruido recurrente.

# 5. Tests

- Arrange/Act/Assert.
- Un comportamiento por test.
- EditMode para dominio/aplicación.
- PlayMode para integración Unity.
- No modificar expectativas para acomodar un bug visual o funcional.
