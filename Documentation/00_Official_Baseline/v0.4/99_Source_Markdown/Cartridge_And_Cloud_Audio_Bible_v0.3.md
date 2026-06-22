---
title: "Cartridge & Cloud - Audio Bible"
subtitle: "Música, ambientes, SFX, mezcla e implementación"
author: "VRM Games / Blas Luis Rocha González"
date: "22/06/2026"
lang: es-ES
papersize: a4
fontsize: 10pt
geometry: margin=18mm
toc: true
toc-depth: 3
colorlinks: true
linkcolor: VRMGreen
urlcolor: VRMGreen
citecolor: VRMGreen
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
  \fancyhead[L]{\small Cartridge \& Cloud - Documento interno}
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
**Título técnico provisional:** Cartridge & Cloud  
**Plataforma inicial:** PC / Steam  
**Motor validado:** Unity 6.3 LTS `6000.3.18f1` / C# / URP `17.3.0`  
**Versión del documento:** v0.3  
**Estado:** Revisado en la baseline documental v0.4; contenido funcional vigente  
**Fuente conceptual:** `Enfoque_v0.6.md`  

> **Regla de interpretación:** el proyecto se documenta como una preproducción nueva. Sprint 0 ha validado la fundación técnica (proyecto Unity, assemblies, escenas, tests y builds). Salvo cuando se cite evidencia de implementación, los sistemas de gameplay y contenido descritos en este documento continúan siendo especificación.

\newpage

# 1. Visión sonora

El audio combina ambiente de pequeña tienda, nostalgia tecnológica y feedback empresarial moderno. Debe acompañar sesiones largas sin fatiga y comunicar acciones sin exigir mirar constantemente la UI.

# 2. Pilares

- Claridad funcional.
- Baja fatiga.
- Identidad física y digital.
- Evolución audible por etapas.
- Mezcla contenida para lectura de datos.
- Producción modular y licenciable.

# 3. Buses

`Master`, `Music`, `Ambience`, `SFX`, `UI`, `Characters`, `Notifications`, `Voice` futuro.

# 4. Ambientes

- Tienda cerrada/preparación.
- Tienda abierta con puerta, clientes y equipos.
- Almacén y recepción.
- Zona de ordenadores.
- Logística online.
- Oficina editorial/desarrollo.
- Plataforma e infraestructura.

# 5. Música

Loops lo-fi/electrónicos suaves, capas que evolucionan con etapa y situación, stingers breves para apertura, cierre, hito, expansión, crisis y victoria. Evitar música épica constante y loops muy cortos.

# 6. SFX del vertical slice

| Grupo | Eventos |
|---|---|
| UI | Hover selectivo, click, confirmar, cancelar, error, aviso |
| Mundo | Pasos, puerta, cajas, estantería, colocación |
| Inventario | Recoger, transferir, reponer, vacío/lleno |
| Caja | Escaneo, total, cobro, venta completada |
| Tiempo | Apertura, 21:45, cierre y resumen |
| Economía | Compra, gasto, ingreso, saldo crítico |

# 7. Audio espacial

SFX de puerta, clientes, cajas y estaciones usan 3D con rangos cortos. UI y notificaciones críticas permanecen 2D. La mezcla prioriza acciones cercanas y evita una cacofonía con muchos clientes.

# 8. Variación

Cada acción repetitiva tendrá varias muestras o variaciones de pitch/volumen moderadas. Se aplicarán cooldowns a notificaciones.

# 9. Implementación

AudioLibrary por IDs, AudioService desacoplado, pooling de fuentes, AudioMixer para volumen y snapshots, guardado de opciones y eventos de dominio como disparadores.

# 10. Prioridad de producción

P0: UI, caja, interacción, apertura/cierre.  
P1: ambientes y clientes.  
P2: música y expansión.  
P3: contenido avanzado y voz.

# 11. QA

- No clipping ni picos molestos.
- Sesión de 90 minutos sin fatiga evidente.
- Notificaciones distinguibles.
- Volúmenes persistentes.
- Sin assets sin licencia.
- Audio correcto con pausa y velocidades.
