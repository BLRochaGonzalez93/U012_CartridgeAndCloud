---
title: "Cartridge & Cloud - Art Bible"
subtitle: "Dirección artística 3D, producción modular y evolución visual"
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

# 1. Visión artística

Estilo 3D estilizado, limpio y modular, con lectura inmediata desde cámara elevada. La tienda inicial transmite cercanía, nostalgia y limitación de recursos; las etapas avanzadas incorporan tecnología y escala sin perder la identidad humana del negocio.

# 2. Pilares visuales

- Legibilidad antes que detalle.
- Modularidad y reutilización.
- Siluetas claras.
- Evolución visible del negocio.
- Negro/verde como identidad, no como monocromía.
- Productos y marcas ficticias.
- Producción viable para un desarrollador principal.

# 3. Mundo y escala

## 1. Base de la dirección actual

**Cartridge & Cloud** se planifica como un proyecto nuevo en fase de preproducción. El concepto sustituye el enfoque anterior de simulador de estudio de videojuegos, pero no presupone que exista código, arquitectura, escenas o sistemas reutilizables.

La dirección aprobada se apoya en:

- Interacción directa dentro de un espacio físico 3D.
- Cámara fijada sobre el jugador en una perspectiva intermedia entre top-down e isométrica cenital.
- Estilo visual low poly con texturas handpainted/cartoon y bordes sombreados tipo tinta.
- Terreno dividido mediante una cuadrícula lógica a nivel de suelo.
- Movimiento libre y fluido sobre el terreno.
- Colocación modular de mobiliario y equipamiento.
- Clientes representados como NPCs con comportamiento comprensible.
- Productos físicos, estanterías, vitrinas, almacén y mobiliario funcional.
- Progresión empresarial gradual y acumulativa.
- Desarrollo y publicación de videojuegos como líneas de negocio avanzadas.
- Evolución desde el comercio físico hacia servicios digitales sin sustituir los sistemas anteriores.

### Dimensiones generales del mundo

- El mapa completo tendrá una extensión inicial de `100 × 100` celdas.
- Cada celda de `1 × 1` equivale a `0,5 × 0,5 metros`.
- El mundo lógico completo equivale a `50 × 50 metros`.
- La totalidad del mapa no será visible ni accesible desde el comienzo.
- Cada zona tendrá dimensiones y ubicación predeterminadas desde el inicio.
- Las zonas bloqueadas permanecerán ocultas o no construidas hasta cumplir sus requisitos.
- La primera tienda ocupará `10 × 10` celdas, equivalentes a `5 × 5 metros`, y estará centrada en el mapa.
- La anchura mínima de paso será de una celda, equivalente a `0,5 metros`.
- Las huellas de futuros objetos se definirán conforme se incorporen al diseño.

### Huellas iniciales de objetos

- **Estantería:** `4 × 2` celdas.
- **Vitrina:** `2 × 2` celdas.
- **Setup de ordenador:** mesa de `3 × 2` celdas y silla de `1 × 1` en una posición fija.
- **Mostrador:** `3 × 2` celdas y una celda funcional de `1 × 1` para quien lo atienda.
- **Otros objetos:** tendrán huellas y reglas propias declaradas en sus datos.

### Reglas universales de colocación

- Los objetos se alinearán automáticamente con la cuadrícula.
- La rotación se realizará en incrementos de `90°`.
- Las teclas iniciales serán `Q` y `E`.
- La asignación de teclas podrá modificarse desde las opciones de control cuando se implemente el sistema correspondiente.
- No se permitirá que dos objetos físicos ocupen la misma celda.
- La única excepción funcional será el cliente sentado, que podrá solaparse con el punto de asiento de la silla reservada.
- La validación impedirá bloquear permanentemente entradas, salidas y puntos de interacción obligatorios.
- Al colocar o retirar objetos se actualizará la navegación de la zona afectada.

### Puertas y conexiones

Se utilizarán tres soluciones:

1. **Puerta automática deslizante:** opción principal para accesos visibles de clientes, jugador y empleados.
2. **Puerta lateral automática:** alternativa para conexiones interiores con poco espacio.
3. **Paso invisible:** reservado para transiciones técnicas o zonas donde una puerta física no aporte valor visual.

Las puertas visibles se abrirán al detectar una entidad autorizada dentro de su zona de proximidad y se cerrarán cuando el paso quede libre.

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> El mapa, la escala, las huellas iniciales y las reglas universales de colocación están definidos. Las huellas de objetos futuros se declararán al diseñar cada contenido, sin considerarse una decisión conceptual pendiente.

## 3. Dirección general del proyecto

### Concepto principal

```text
Tienda de videojuegos
→ ampliaciones y servicios
→ comercio online
→ distribución
→ publishing
→ desarrollo interno
→ plataforma digital
→ servicios online y servidores
```

La progresión es **aditiva**. Cada nueva etapa amplía el negocio sin eliminar ni reemplazar las anteriores.

El jugador no comienza como desarrollador de videojuegos. El desarrollo aparece progresivamente como una nueva actividad empresarial.

### Modelo físico de expansión

- Existirá una única tienda principal sometida a ampliaciones modulares.
- Las líneas de negocio que necesiten espacios independientes aparecerán como edificios colindantes.
- Los edificios estarán conectados mediante puertas y formarán un único complejo empresarial.
- Cada edificio o zona tendrá una ubicación definida dentro del mapa global desde el inicio.
- La zona no será visible, accesible ni funcional hasta ser desbloqueada y construida.
- Las ampliaciones conservarán la actividad de la tienda inicial.

### Distribución provisional del complejo

Las siguientes huellas funcionan como base inicial editable:

| Zona | Huella provisional | Posición relativa | Etapa |
|---|---:|---|---:|
| Tienda inicial | `10 × 10` | Centro | 1 |
| Almacén ampliado | `8 × 10` | Norte | 2 |
| Zona de puestos informáticos | `12 × 10` | Este | 2 |
| Área logística y pedidos online | `10 × 12` | Oeste | 3 |
| Oficina de publishing | `12 × 12` | Nordeste | 4 |
| Estudio de desarrollo interno | `14 × 12` | Noroeste | 5 |
| Infraestructura de servidores | `12 × 12` | Sur, no transitable | 6 |

La disposición podrá ajustarse durante el prototipado, pero seguirá estas reglas:

- Las zonas forman un complejo continuo.
- Todas se conectan mediante accesos visibles o pasos técnicos.
- Ninguna expansión elimina las anteriores.
- Las ubicaciones se reservan desde el comienzo para impedir construcciones incompatibles.
- Los edificios avanzados pueden mostrarse exteriormente sin disponer de interior jugable.

### Requisitos provisionales de expansión

| Categoría | Nivel mínimo | Reputación | Coste orientativo |
|---|---:|---:|---:|
| Ampliación básica | 2 | 50 | 5.000–15.000 € |
| Servicio especializado | 3 | 150 | 15.000–35.000 € |
| Logística y comercio online | 4 | 300 | 40.000–80.000 € |
| Publishing | 5 | 600 | 100.000–200.000 € |
| Desarrollo interno | 6 | 1.000 | 200.000–400.000 € |
| Plataforma e infraestructura | 8 | 2.000 | 500.000 € o más |

Los valores se configurarán mediante datos y se balancearán con la economía real.

### Conocimiento del mercado

La tienda física es el origen del conocimiento comercial:

- Productos y géneros más demandados.
- Plataformas utilizadas.
- Sensibilidad al precio.
- Reservas.
- Tendencias.
- Públicos habituales.
- Servicios más demandados.
- Productos con poca rotación.

Ese conocimiento podrá utilizarse para decidir:

- Qué juegos publicar.
- Qué estudios financiar.
- Qué juegos desarrollar internamente.
- Qué productos promocionar.
- Qué líneas de negocio ampliar.
- Qué servicios incorporar a la plataforma digital.

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> El modelo de complejo empresarial, la distribución inicial y los requisitos orientativos están definidos. Las cifras se balancearán durante el desarrollo y no requieren una nueva decisión conceptual.

## 29. Identidad visual

La identidad aprobada es:

- Mundo 3D low poly.
- Texturas handpainted/cartoon.
- Shaders con bordes tipo tinta.
- Mobiliario modular.
- Iluminación cálida.
- Productos coloridos.
- UI técnica y limpia.
- Evolución visual del complejo.

### Valores visuales provisionales

#### Paleta de interfaz

| Uso | Color provisional |
|---|---|
| Fondo principal | `#0D1110` |
| Superficie | `#19201D` |
| Superficie elevada | `#242D29` |
| Verde principal | `#35D07F` |
| Verde secundario | `#1F8F5F` |
| Texto principal | `#E8EFEA` |
| Texto secundario | `#AAB7B0` |
| Advertencia | `#E8B44A` |
| Error | `#D95D5D` |

#### Mundo

- Temperatura de iluminación interior cálida.
- Sombras suaves.
- Bordes oscuros moderados.
- Personajes estilizados y legibles desde cámara elevada.
- Siluetas diferenciadas.
- Materiales simples con variación pintada.
- Nivel de detalle medio-bajo para mantener claridad.

Estos valores son provisionales.

### Identidad narrativa y atmosférica

- Tono profesional y cercano.
- Humor ligero basado en situaciones y personalidades.
- Componente nostálgico más visible durante la etapa de tienda física.
- Evolución gradual hacia una imagen empresarial y tecnológica.
- Coherencia entre la estética de cartuchos, cajas y estanterías y la posterior evolución hacia nube, plataforma e infraestructura.

> **Estado del apartado: PARCIALMENTE DEFINIDO.**  
> La dirección visual, cromática y narrativa está aprobada. Las referencias, personajes, materiales, iluminación definitiva y guía de arte se crearán durante la preproducción visual.

# 4. Tienda inicial

Local de 5x5 m lógicos, iluminación cálida, mobiliario Tier E, paredes neutras, cartelería sencilla y pequeñas referencias nostálgicas. Debe verse funcional aunque modesto.

# 5. Arquitectura modular

Paredes, suelos, puertas, esquinas y expansiones usan módulos compatibles con grid de 0,5 m. Los elementos interactivos tienen pivotes y puntos de navegación normalizados.

# 6. Mobiliario y props

- Mostradores, estanterías, vitrinas, racks y recepción.
- Cajas, paquetes, etiquetas y material de almacén.
- Productos ficticios con variantes de color y packaging.
- Señalización, precios, pósteres y merchandising.
- Props decorativos reutilizables por tier.

# 7. Personajes

Proporciones estilizadas, animación económica y expresiva, diversidad visual y ropa por rol. La lectura de intención se apoya en postura, iconos y burbujas, no en animaciones faciales costosas.

# 8. Evolución por etapas

| Etapa | Lenguaje visual |
|---|---|
| Tienda pequeña | Improvisada, cálida, nostálgica |
| Tienda ampliada | Más orden, señalización y especialización |
| Logística online | Industrial ligera y funcional |
| Publishing | Oficina editorial profesional |
| Desarrollo interno | Estudio creativo y técnico |
| Plataforma | Centro digital, paneles y marca consolidada |
| Infraestructura | Sala técnica sobria, modular y legible |

# 9. VFX y feedback

Contornos de selección, grid, validación de colocación, partículas discretas de compra/éxito, alertas de stock, estados de interacción y transiciones de apertura/cierre.

# 10. Producción y LOD

Usar atlas, materiales compartidos, LOD en props grandes, iluminación baked/mixta donde convenga y variantes mediante materiales. Los productos pequeños pueden representarse por grupos y estados de llenado.

# 11. Pipeline

Concepto -> blockout -> prueba de escala -> low/mid poly -> UV/material -> prefab -> colisión/nav points -> QA visual -> registro legal.

# 12. Nomenclatura

`ENV_`, `FUR_`, `PRD_`, `CHR_`, `VFX_`, `UI_`, seguido de categoría, tier y variante.

# 13. Lista de assets del vertical slice

- Kit modular de tienda.
- 5 muebles funcionales.
- 12 productos con packaging.
- Cajas y recepción.
- Personaje jugador.
- 4 variantes de cliente.
- Señalización y precios.
- Iconografía principal.
- Iluminación y materiales de entorno.

# 14. Marketing art

Todo material debe etiquetarse como captura real, mockup, arte conceptual o key art. Las cápsulas pueden ser aspiracionales, pero no mostrar sistemas inexistentes como gameplay.
