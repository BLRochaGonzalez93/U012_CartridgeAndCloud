---
title: "Cartridge & Cloud - UX Flow v0.5"
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

# 1. Flujo principal

`Bootstrap -> MainMenu -> Slot flow -> Store -> Day flow -> MainMenu`

TestLab permanece fuera del flujo de usuario.

# 2. Slots

- New Game en slot vacío.
- Continue en slot válido.
- Replace con confirmación.
- Delete con confirmación.
- Recuperación desde backup cuando el primario es inválido.

# 3. Store

## Navegación

Clic en suelo mueve al jugador. Arrastre derecho orbita. Rueda ajusta zoom.

## Operations UI

El panel abre compras, inventario, proveedores, displays, clientes, checkout, día, economía, ayuda y accesibilidad.

## Regla de input pendiente

Antes de raycast al mundo debe comprobarse:

```csharp
if (EventSystem.current != null &&
    EventSystem.current.IsPointerOverGameObject())
{
    return;
}
```

El clic consumido por UI no puede mover al jugador ni confirmar placement.

# 4. Construcción

Entrar en modo, seleccionar definición, mover preview, rotar, validar, confirmar, cancelar o retirar. Feedback visual y textual debe explicar el rechazo.

# 5. Operación diaria

BeforeOpen -> Open -> Closing -> Closed -> Results -> Save -> Next Day.

# 6. Accesibilidad

Escala de UI, contraste, estados no dependientes solo del color, texto localizado, velocidades ajustables y confirmación de acciones destructivas.
