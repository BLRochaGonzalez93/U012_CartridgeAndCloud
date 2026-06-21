---
title: "Cartridge & Cloud - Steam Publishing Plan"
subtitle: "Preparación de publicación PC/Steam sin iniciar Steamworks"
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

# 1. Estado

Steam es la plataforma comercial inicial prevista, pero no se considera configurada ni contratada. Los requisitos oficiales se revalidarán en el momento de iniciar Steamworks.

# 2. Gates de entrada

- Nombre validado.
- Vertical slice representativo.
- GDD y alcance estables.
- Capturas reales y key art legalmente aprobados.
- Texto ES/EN.
- Build Windows instalable.
- QA, privacidad y soporte definidos.
- Presupuesto y calendario de publicación aprobados.

# 3. Cronología

Preproducción: no abrir página.  
Vertical Slice: preparar borrador y materiales internos.  
MVP/Alpha: decidir Steam Direct y Coming Soon.  
Beta: testers, demo/festival solo si aporta valor.  
RC: revisión de store/build, depots y compliance.  
Release: lanzamiento y hotfix plan.

# 4. Materiales

Cápsulas, logo, descripción corta/larga, screenshots reales, trailer, tags, idiomas, requisitos, iconos, legal, créditos y press kit.

# 5. Tags provisionales

Management, Simulation, Shop Keeper, Building, Economy, Strategy, Indie, Singleplayer. Deben revisarse con la taxonomía vigente de Steam.

# 6. Build y depots

Un depot Windows x64 inicialmente. Ramas `default`, `qa`, `private_test` cuando proceda. La demo sería una app/depot separado y no está confirmada.

# 7. Cloud, achievements y workshop

No forman parte del alcance confirmado. Se evaluarán después de estabilizar saves y producto; no se prometerán en la página.

# 8. Store copy provisional

Debe centrarse en tienda física, expansión multicanal y evolución empresarial. No enumerar cada sistema avanzado si no está listo para lanzamiento.

# 9. QA Steam

Instalación limpia, actualización, rutas, permisos, overlay si se integra, idioma, mando solo si se declara, save, desinstalación y logs.

# 10. Checklist Coming Soon

- [ ] Nombre y app data finales.
- [ ] 5+ capturas reales coherentes.
- [ ] Trailer representativo.
- [ ] Descripciones ES/EN.
- [ ] Tags y categorías revisados.
- [ ] Política de privacidad cuando corresponda.
- [ ] Registro legal cerrado.
- [ ] Roadmap público sin promesas especulativas.

# 11. Lanzamiento

Freeze, RC, revisión, build backup, plan de hotfix, soporte, comunicación, precios regionales y monitorización. Todo se revalida con documentación oficial vigente.
