---
title: "Cartridge & Cloud - Marketing Plan"
subtitle: "Posicionamiento, comunicación y validación de mercado"
author: "VRM Games / Blas Luis Rocha González"
date: "21/06/2026"
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
**Motor previsto:** Unity 6 LTS / C#  
**Versión del documento:** v0.3  
**Estado:** Preproducción reiniciada desde cero  
**Fuente conceptual:** `Enfoque_v0.6.md`  

> **Regla de interpretación:** el proyecto se documenta como una preproducción nueva. No se presupone código, escenas, managers, datos, builds ni sprints implementados. Todo elemento técnico descrito es una especificación o recomendación hasta que exista evidencia de implementación y QA.

\newpage

# 1. Posicionamiento

Simulador de gestión física y empresarial que comienza como una pequeña tienda de videojuegos y evoluciona hacia publishing, desarrollo y plataforma digital.

# 2. Diferenciación

- Tienda 3D operable y construible.
- Cadena física completa de producto.
- Clientes observables.
- Progresión desde comercio local a ecosistema digital.
- Desarrollo de juegos como expansión, no como único bucle.
- Relación entre tienda, datos, comunidad, publishing y plataforma.

# 3. Público

Jugadores de management, shop simulators, tycoon, logística ligera y progresión prolongada; público secundario interesado en la industria del videojuego.

# 4. Pitch

**Corto:** Gestiona una pequeña tienda de videojuegos, conviértela en un negocio multicanal y construye tu propio ecosistema de publishing, desarrollo y distribución digital.

**Promesa:** cada etapa nace físicamente de la anterior: el stock, los clientes y la comunidad de la tienda se convierten en datos, catálogo y oportunidades empresariales.

# 5. Pilares de comunicación

Tienda viva, crecimiento visible, decisiones comprensibles, nostalgia sin parodia, industria ficticia y transparencia de desarrollo.

# 6. Comparables y mensaje

Usar comparables para explicar género, nunca como identidad. Evitar “el nuevo Game Dev Tycoon”. Comunicar el viaje tienda -> online -> publisher -> estudio -> plataforma.

# 7. Fases

| Fase | Comunicación |
|---|---|
| Preproducción | Diario de diseño selectivo; sin promesas comerciales |
| Prototipo | Clips técnicos identificados como WIP |
| Vertical Slice | Primer material representativo y tests cerrados |
| MVP/Alpha | Página de proyecto, devlogs, comunidad inicial |
| Steam Coming Soon | Solo con capturas reales, trailer y alcance estable |
| Beta | Wishlist, testers, festivales si procede |
| Lanzamiento | Campaña, reviews, soporte y roadmap honesto |

# 8. Canales

Portfolio VRM Games, GitHub para proceso técnico, LinkedIn profesional, YouTube/devlogs cuando exista material, redes cortas opcionales y Steam en fase autorizada.

# 9. Materiales

Logo provisional/final, key art, cápsulas, capturas reales, GIFs, trailer, press kit, fact sheet, FAQ, roadmap público y guía de creadores.

# 10. Métricas

Wishlists, conversión de página, retención de vídeo, demos iniciadas/completadas, feedback, coste de contenido, seguidores y ventas. No perseguir métricas vanidosas sin impacto.

# 11. Calendario y carga

Marketing no debe impedir cerrar el vertical slice. Agrupar creación de contenido por hitos y reutilizar capturas/materiales.

# 12. Reglas de honestidad

No mostrar plataforma, empleados, Steam Cloud, logros, demo o sistemas avanzados como implementados hasta existir. Etiquetar mockup, concepto y captura real.

# 13. Propuesta de valor completa

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

## 30. Propuesta de valor consolidada

> **Cartridge & Cloud** es un simulador de gestión 3D en el que el jugador trabaja físicamente en una pequeña tienda de videojuegos, organiza el espacio, recibe y repone mercancía, fija precios, atiende clientes y convierte el comercio local en un ecosistema que integra servicios para jugadores, comercio online, publishing, desarrollo propio, plataforma digital e infraestructura tecnológica.

El nombre **Cartridge & Cloud** se utilizará como nombre provisional oficial en documentos y materiales internos. Los cinco finalistas conservados para una validación posterior son:

1. Cartridge & Cloud.
2. Shelf to Server.
3. Game District.
4. Pixels & Profits.
5. Save Point Market.

Antes de aprobar el nombre definitivo deberán comprobarse coincidencias relevantes en Steam, marcas, redes y dominios.

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> El nombre de trabajo, la propuesta de valor, el tono y el público están definidos. Solo queda la validación comercial y legal del nombre antes de considerarlo definitivo.

## 31. Diferenciación respecto a Game Dev Tycoon

La nueva dirección se diferencia porque:

- El jugador controla directamente un personaje.
- Existe un establecimiento físico jugable.
- Los clientes son agentes visibles.
- Los productos ocupan espacio real.
- El inventario debe recibirse, almacenarse y colocarse.
- El negocio comienza en la venta, no en el desarrollo.
- El publishing aparece como expansión.
- Crear videojuegos no es la única fuente de progreso.
- La evolución amplía el modelo de negocio sin sustituir etapas anteriores.
- La tienda física y la plataforma digital forman parte de la misma trayectoria.
- La distribución espacial importa.
- La observación de los clientes aporta información del mercado.
- Los sistemas avanzados tienen presencia física o visual dentro del mundo.

> **Estado del apartado: COMPLETO.**  
> La diferenciación conceptual principal está suficientemente definida.

---

# 14. Riesgos

Nombre no validado, exceso de alcance, comparación con clones, materiales sin identidad, campaña demasiado temprana y consumo de tiempo del desarrollador.
