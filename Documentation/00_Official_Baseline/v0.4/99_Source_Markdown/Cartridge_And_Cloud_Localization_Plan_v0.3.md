---
title: "Cartridge & Cloud - Localization Plan"
subtitle: "Arquitectura ES/EN, terminología y QA lingüístico"
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

# 1. Alcance

Español `es-ES` como idioma de desarrollo y QA; inglés `en-US` como idioma comercial prioritario. Otros idiomas dependen de viabilidad.

# 2. Principios

- Todo texto visible termina en clave.
- IDs persistentes nunca se traducen.
- No concatenar frases complejas.
- Reservar expansión de texto.
- Género/plural y cifras se formatean por locale.
- Los textos comerciales solo describen sistemas reales.

# 3. Dominios de claves

`ui`, `world`, `product`, `customer`, `employee`, `economy`, `order`, `online`, `publishing`, `development`, `platform`, `infrastructure`, `market`, `tutorial`, `notification`, `error`, `steam`, `credits`.

# 4. Ejemplos

```text
ui.store.open_button
ui.build.invalid_access
product.family.consoles.name
customer.leave.out_of_stock
order.status.in_transit
publishing.contract.revenue_share
platform.refund.approved
```

# 5. Terminología base

| Español | Inglés recomendado |
|---|---|
| Tienda | Store |
| Almacén | Warehouse |
| Expositor | Display fixture |
| Reposición | Restocking |
| Pedido a proveedor | Purchase order |
| Preparación de pedidos | Order fulfilment |
| Publishing | Publishing |
| Desarrollo interno | Internal development |
| Hito | Milestone |
| Greenlight | Greenlight |
| Compilación/build | Build |
| Plataforma digital | Digital platform |
| Disponibilidad | Uptime/availability según contexto |

# 6. UI y layout

Diseñar botones para +30 % de longitud, tablas con wrap, tooltips y fuentes con caracteres latinos completos. Probar 1280x720.

# 7. Datos localizables

Definiciones estáticas guardan `nameKey`, `descriptionKey` y claves de variantes. Save solo almacena IDs.

# 8. Pipeline

Extracción -> tabla fuente -> traducción -> revisión -> integración -> pseudo-localización -> QA visual -> aprobación.

# 9. Pseudo-localización

Expandir texto, añadir diacríticos y detectar hardcodeo antes del vertical slice.

# 10. QA

Truncamiento, acentos, fallback, variables, plural, formato monetario/fecha, orden de palabras y coherencia terminológica.

# 11. Steam

Descripción, cápsulas con texto, tags, noticias y requisitos se traducen por separado y se validan contra la build real.
