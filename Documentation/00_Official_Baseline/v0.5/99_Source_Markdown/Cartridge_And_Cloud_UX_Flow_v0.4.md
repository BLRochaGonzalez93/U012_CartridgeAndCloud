---
title: "Cartridge & Cloud - UX Flow v0.4"
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


# 1. Flujo actual implementado

```text
Bootstrap
  -> MainMenu
      -> Store
          -> MainMenu
      -> Quit
```

TestLab permanece fuera del flujo normal.

# 2. Contextos de input

| Escena/estado | Contexto |
|---|---|
| MainMenu | UI |
| Store navegación | Gameplay |
| Store construcción | Gameplay con política de supresión de movimiento |
| Transición | None o contexto controlado por ApplicationRoot |

# 3. Flujo de construcción

```text
B -> entrar
pointer sobre suelo -> preview
Q/E -> rotar
clic -> confirmar si válido
Escape -> cancelar
Delete/Backspace -> retirar
B -> salir
```

# 4. Feedback

- Verde: acción válida.
- Rojo: límites, solape o acceso inválido.
- Marcadores técnicos: entrada y anchors en fase actual.
- El feedback final se sustituirá por UI y arte representativos en Sprint 15-16.

# 5. Flujos futuros

## Inventario

Seleccionar origen -> seleccionar producto/cantidad -> seleccionar destino -> validar -> transferir -> confirmar feedback.

## Pedidos

Abrir catálogo -> añadir líneas -> revisar coste -> confirmar -> esperar entrega -> recibir cajas.

## Reposición

Seleccionar display -> producto asignado -> tomar stock -> reponer hasta capacidad.

## Cliente y caja

Cliente entra -> busca -> reserva -> carrito -> cola -> checkout -> salida o abandono.

## Día

Preparación -> apertura -> operación -> aviso 21:45 -> cierre 22:00 -> resolución -> informe -> guardado.

# 6. Accesibilidad base

- escalado de UI;
- contraste suficiente;
- estados no dependientes solo del color;
- remapeo futuro;
- velocidades de cámara ajustables;
- texto localizado;
- confirmaciones para acciones destructivas relevantes.
