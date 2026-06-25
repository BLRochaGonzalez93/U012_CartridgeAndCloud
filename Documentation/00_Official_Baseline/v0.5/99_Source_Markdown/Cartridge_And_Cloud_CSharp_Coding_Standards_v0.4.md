---
title: "Cartridge & Cloud - C# Coding Standards v0.4"
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
**Versión del documento:** v0.4  
**Estado:** Current / Approved  
**Baseline técnica:** `22d62967aaf9895db7bce75afd2ffa11f7858e0c`  
**Baseline documental:** `b650aa5968ecfc9ebe63fc308b0dc8ea2cafae57`


# 1. Naming

- Namespace raíz: `VRMGames.CartridgeAndCloud`.
- PascalCase para tipos y miembros públicos.
- `_camelCase` para campos privados serializados.
- Un tipo público principal por archivo.
- Nombres de tests: `Method_Scenario_ExpectedResult`.

# 2. Formato

- Cuatro espacios, sin tabs.
- Llaves en línea separada.
- Líneas legibles; dividir expresiones complejas.
- Evitar `var` cuando reduce claridad del dominio.
- Evitar abreviaturas opacas.

# 3. Null y errores

- Validar argumentos públicos.
- `ArgumentNullException`, `ArgumentOutOfRangeException` y `InvalidOperationException` según contrato.
- No ocultar fallos de invariantes.
- Unity logs solo en Presentation/Infrastructure/Editor.

# 4. Arquitectura

- Domain sin Unity.
- Application sin MonoBehaviour.
- Presentation no contiene reglas autoritativas.
- Infrastructure implementa contratos.
- Editor no debe forzar referencias indebidas a Domain.

# 5. Mutación

- Validar antes de mutar.
- Operaciones compuestas atómicas.
- Exponer snapshots/copias cuando una colección interna no debe mutarse.
- Preferir value objects inmutables.

# 6. Unity

- Campos `[SerializeField] private`.
- `Awake` resuelve referencias y establece baseline segura.
- Métodos `Configure` para tests/instaladores.
- Instaladores idempotentes.
- Preservar `.meta` al reemplazar archivos existentes.

# 7. Tests

- Un Arrange/Act/Assert claro.
- Cubrir casos válidos, límites, errores e invariantes.
- No depender de orden de tests.
- Destruir GameObjects creados.
- Mantener la suite completa verde antes de publicar.
