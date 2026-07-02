---
title: "Cartridge & Cloud — Marketing and Communication Plan"
subtitle: "Plan consolidado de posicionamiento, validación de mercado, comunicación pública, comunidad, contenidos, prensa y coordinación de lanzamiento"
author: "VRM Games / Blas Luis Rocha González"
date: "2026-07-01"
lang: "es-ES"
document_number: "28"
document_version: "1.0"
project_version_reference: "0.0.17"
platform: "PC / Steam"
engine: "Unity 6.3 LTS 6000.3.18f1"
render_pipeline: "URP 17.3.0"
status: "PLANNING / INTERNAL ONLY / PUBLIC CAMPAIGN BLOCKED"
---

# 28 — Marketing and Communication Plan

**Proyecto:** Cartridge & Cloud  
**Desarrollador y publisher candidato:** VRM Games / Blas Luis Rocha González  
**Plataforma inicial:** PC / Steam  
**Estado técnico de referencia:** Sprints 0–15 `CLOSED / PASS`; Sprint 16 `IN PROGRESS`; Sprint 17 y H6 `PENDING`  
**Estado comercial actual:** sin Steamworks, AppID, store page, precio, fecha, campaña pública, press kit, demo, Playtest ni build autorizada para distribución  
**Estado legal actual:** título y nombre operativo provisionales; clearance de marcas, dominios, redes y plataforma pendiente  
**Clasificación del documento:** interno; no constituye anuncio, promesa comercial, calendario público ni aprobación de gasto

> **Regla principal:** el marketing de *Cartridge & Cloud* debe aumentar comprensión y descubrimiento sin adelantar la realidad del producto. Ninguna pieza puede convertir visión, concepto, mockup, placeholder, sistema diferido o intención de diseño en una característica presentada como implementada.

> **Gate vigente:** no se presentará públicamente el vertical slice como producto representativo hasta que `StoreInitial`, UI, arte, audio y build hayan superado Sprint 17 y la revisión H6. Un `PASS` de H6 no abre automáticamente una campaña comercial: habilita una decisión posterior y separada de premarketing y Steam.

---

# 0. Propósito, autoridad y forma de uso

## 0.1. Propósito

Este documento consolida la estrategia de marketing y comunicación externa de *Cartridge & Cloud* desde el estado actual de desarrollo hasta el lanzamiento y la transición a soporte postlanzamiento. Su función es convertir una intención comercial general en un sistema operativo verificable:

- qué puede comunicarse en cada estado de madurez;
- a quién se dirige el producto y qué problema de comprensión debe resolver cada mensaje;
- cómo se diferencia el juego sin apoyarse en imitaciones o comparaciones reductoras;
- qué materiales deben producirse, con qué evidencia y bajo qué gates;
- cómo se coordinan Steam, portfolio, web, redes, vídeo, prensa, creadores, comunidad y soporte;
- qué métricas se usarán y qué decisiones permite tomar cada una;
- cómo se protege el tiempo de producción de una persona desarrolladora;
- cómo se evitan riesgos de marca, copyright, privacidad, seguridad, expectativas y sobrepromesa;
- cómo se conserva trazabilidad entre build, claim, captura, copy, campaña y resultado.

El documento no sustituye al Steam Publishing Plan, al Post-Launch Plan, al registro legal, al plan de privacidad, al plan de seguridad ni a las futuras decisiones de precio y fecha. Los coordina desde la perspectiva de posicionamiento, audiencia, adquisición, comunicación y reputación.

## 0.2. Autoridad documental

La jerarquía aplicable es:

1. `00_Enfoque_y_Alcance.md`, para identidad, pilares, límites y estados de alcance.
2. `01_Game_Design_Document.md`, para reglas de producto y visión de largo plazo.
3. `02_Vertical_Slice_Specification.md`, para lo que H6 debe demostrar y lo que no constituye una demo pública.
4. `06_Production_Roadmap_y_Sprint_Plan.md`, para fases, capacidad y gates de madurez.
5. `17_Art_Bible.md`, `18_Audio_Bible.md` y `19_UI_Style_Guide.md`, para representación visual, sonora y de interfaz.
6. `22_Localization_Plan.md`, para ES/EN, terminología y LQA.
7. `23_Legal_Credits_and_Licenses_Register.xlsx`, para autorización de nombres, assets, créditos y distribución.
8. `24_Steam_Publishing_Plan.md`, para onboarding, store page, assets Steam, wishlists, demo, Playtest y lanzamiento.
9. `25_Post_Launch_and_Live_Operations_Plan.md`, para soporte, comunidad, reviews, incidentes y continuidad tras publicar.
10. `26_Privacy_Data_and_Telemetry_Plan.md`, para métricas, newsletter, soporte, comunidad y tratamiento de datos.
11. `27_Security_and_Incident_Response_Plan.md`, para credenciales, claves, embargo, cuentas, accesos y respuesta a incidentes.
12. Este documento, como autoridad especializada sobre marketing y comunicación siempre que no contradiga las fuentes anteriores.

Ante una contradicción no se corrige silenciosamente el dato. Se abre una discrepancia, se identifica la autoridad, se registra la decisión y se actualizan las referencias afectadas.

## 0.3. Estados utilizados

| Estado | Significado |
|---|---|
| `CURRENT` | Hecho demostrado o fotografía documental vigente. |
| `PLANNED` | Trabajo aprobado como dirección, todavía no ejecutado. |
| `PROPOSED` | Recomendación de este plan pendiente de aprobación específica. |
| `INTERNAL ONLY` | Puede usarse en trabajo interno o portfolio técnico controlado, no como campaña comercial. |
| `CONTROLLED TEST` | Distribución o comunicación limitada a una cohorte definida. |
| `PUBLIC READY` | Cumple gates de producto, legal, copy, assets, soporte y operación. |
| `BLOCKED` | No puede ejecutarse hasta cerrar requisitos explícitos. |
| `NOT OPEN` | Visión futura no autorizada para producción ni comunicación comercial. |
| `SUPERSEDED / HISTORICAL` | Fuente preservada por genealogía, no autoridad operativa. |
| `RETIRED` | Material retirado que debe conservarse como evidencia, pero no reutilizarse. |

## 0.4. Principio de evidencia

Toda afirmación pública material debe poder responder:

1. ¿Qué build, versión y commit la demuestran?
2. ¿Qué requisito o feature ID la autoriza?
3. ¿Qué prueba demuestra su comportamiento?
4. ¿Qué asset o captura procede de esa build?
5. ¿Qué idioma y revisión de copy se aprobaron?
6. ¿Qué licencia, permiso o clearance cubre nombres, música, fuente, imagen y logotipo?
7. ¿Qué limitaciones deben comunicarse?
8. ¿Quién aprobó su publicación y cuándo?
9. ¿Qué canal y audiencia la recibieron?
10. ¿Cómo se corrige o retira si deja de ser cierta?

## 0.5. Regla de mantenimiento

Este plan se revisará como mínimo:

- al cerrar Sprint 17;
- en la revisión H6;
- antes de abrir Steamworks;
- antes de publicar Coming Soon;
- antes de anunciar demo, Playtest, festival, precio o fecha;
- antes de enviar claves a prensa o creadores;
- antes de una Release Candidate;
- después del lanzamiento y tras cualquier cambio material de posicionamiento.

Cada revisión debe conservar historial, fecha, owner, motivo, impacto y materiales afectados.

---

# 1. Genealogía histórica completa

## 1.1. Fuentes preservadas

La consolidación no se limita a las últimas versiones. Se han preservado y revisado todas las fuentes históricas localizadas:

| Baseline | Documento | SHA-256 | Estado |
|---|---|---|---|
| v0.3 | `Cartridge_And_Cloud_Marketing_Plan_v0.3.md` | `1900805edf3fd6d0acd18cc95125905eae608adbf46cde0098cb49e82fb25fca` | HISTORICAL |
| v0.4 | `Cartridge_And_Cloud_Marketing_Plan_v0.3.md` | `c38c0faa88c5d3625e8814d5ec02eebb39d321819624b8dd0539d13f364d26e7` | HISTORICAL / REVISED CONTEXT |
| v0.5 | `Cartridge_And_Cloud_Marketing_Plan_v0.4.md` | `dad726385bd8f67c059d6a77413a82accff668d5502367f7dee52b5dfbb2ac59` | HISTORICAL |
| v0.6 | `Cartridge_And_Cloud_Marketing_Plan_v0.5.md` | `96b3223429d38a0c83f4c89f07b63e3c258a72cbc76ada7c5e8e5991b62ab1ea` | HISTORICAL |

También se han utilizado los PDF equivalentes, los Steam Publishing Plans históricos, Enfoque, GDD, Art Bible, Roadmap y registros de desarrollo de sus respectivas baselines.

## 1.2. Aportaciones de v0.3

v0.3 estableció la identidad comercial fundacional:

- simulador de gestión física y empresarial;
- comienzo en una pequeña tienda de videojuegos;
- evolución conceptual hacia comercio online, publishing, desarrollo y plataforma;
- tienda 3D operable y construible;
- cadena física de producto;
- clientes observables;
- crecimiento visible;
- nostalgia sin parodia;
- industria ficticia;
- transparencia de desarrollo;
- uso de comparables para explicar género, nunca como identidad;
- secuencia tienda → online → publisher → estudio → plataforma;
- canales iniciales: portfolio, GitHub, LinkedIn, YouTube/devlogs, redes cortas opcionales y Steam cuando estuviera autorizado;
- materiales previstos: logo, key art, cápsulas, capturas, GIFs, tráiler, press kit, fact sheet, FAQ, roadmap público y guía de creadores;
- métricas: wishlists, conversión, retención de vídeo, uso de demo, feedback, coste de contenido, seguidores y ventas;
- prohibición de mostrar sistemas no implementados como reales.

v0.3 también preservó una dirección visual low poly, handpainted/cartoon, bordes tipo tinta, iluminación cálida, productos coloridos, UI técnica y limpia y una evolución gradual desde “cartucho” hacia “nube”.

## 1.3. Revisión contextual de v0.4

La reedición de v0.3 en la baseline v0.4 mantuvo el contenido funcional y actualizó el contexto técnico: Unity 6.3 LTS, URP 17.3.0 y una fundación técnica validada. La lección conservada es que una base técnica no equivale a producto comercial ni autoriza a presentar gameplay representativo.

## 1.4. Aportaciones de v0.4, baseline v0.5

El plan histórico v0.4 concentró el mensaje en:

- simulador acogedor y profundo de tienda de videojuegos;
- construcción funcional;
- logística visible;
- progresión empresarial;
- materiales futuros centrados en construcción, recepción, clientes y caja;
- capturas comparativas de progresión;
- prohibición de iniciar una campaña pública fuerte antes de aprobar H6;
- uso del contenido técnico solo para portfolio y seguimiento, no como promesa final.

## 1.5. Aportaciones de v0.5, baseline v0.6

El plan histórico v0.5 reforzó:

- tienda de videojuegos con logística visible y construcción funcional;
- progresión desde negocio local;
- futura producción de key art, cápsulas, tráiler, GIFs del bucle diario, capturas de `StoreInitial`, devlogs y press kit;
- gate explícito: no presentar públicamente el vertical slice hasta que `StoreInitial`, UI, arte, audio y build superasen Sprint 17.

## 1.6. Regla de consolidación histórica

Este documento conserva la amplitud estratégica de v0.3, la realidad técnica de v0.4, la disciplina de H6 de v0.4/v0.5 y el gate de Sprint 17 de v0.5. Ninguna versión posterior, por ser más breve, elimina los pilares, canales, métricas, reglas de honestidad o materiales definidos en v0.3.

## 1.7. Decisiones históricas sustituidas o acotadas

- La tienda inicial de referencia ya no es `10 × 10`; la autoridad actual describe una escena autorada aproximada de `10 × 15 m`.
- El antiguo mínimo de doce productos y seis familias de mobiliario no es la referencia vigente; el vertical slice consolidado usa seis productos iniciales y ocho familias de mobiliario.
- La visión tienda → plataforma sigue siendo identidad de largo plazo, pero no puede aparecer como alcance comprometido de la primera versión.
- “Steam Coming Soon” no se activa solo por disponer de un vertical slice: requiere un gate Pre-Steam separado.
- El roadmap público deja de ser un material automático; solo existe si hay compromisos autorizados, recursos y capacidad de actualización.
- Las redes cortas, newsletter y Discord son opciones, no obligaciones. Deben justificar coste, privacidad y moderación.
- Los conceptos generados no son assets comerciales autorizados hasta cerrar registro, licencia, procedencia y decisión de uso.

---

# 2. Resumen ejecutivo y postura actual

## 2.1. Posicionamiento consolidado

*Cartridge & Cloud* es un simulador de gestión 3D centrado en operar físicamente una tienda de videojuegos: organizar el espacio, pedir y recibir mercancía, colocar productos, fijar precios, atender clientes y convertir decisiones espaciales y logísticas en resultados comerciales comprensibles.

La visión de largo plazo amplía el negocio hacia servicios, comercio online, publishing, desarrollo propio y plataforma digital, pero esas capas deben comunicarse como visión controlada o permanecer fuera del marketing hasta que su alcance sea aprobado.

## 2.2. Promesa central

> Cada mejora debe poder verse y operarse: el espacio, el stock, los clientes y la forma de trabajar cambian a medida que el pequeño local se convierte en un negocio más capaz.

La promesa no es “hacer clic en menús para hacer crecer números”. La promesa es comprender y transformar un negocio físico mediante acciones observables.

## 2.3. Postura comercial al 1 de julio de 2026

| Área | Estado |
|---|---|
| Título `Cartridge & Cloud` | Provisional; clearance pendiente |
| Nombre `VRM Games` | Operativo interno; clearance/forma de publicación pendiente |
| Producto | Vertical slice en integración; H6 pendiente |
| Build pública | `BLOCKED`; ninguna build legalmente elegible |
| Steamworks | No iniciado |
| Store page | No existe |
| Precio y fecha | No definidos |
| Capturas comerciales | No existen; conceptos y capturas técnicas no equivalen a gameplay final |
| Key art y cápsulas | No existen como assets finales |
| Tráiler | No existe |
| Audio | Placeholders representativos; licencia/calidad final pendientes |
| UI y fuente | Fuente de producción pendiente; deuda de unificación |
| Localización | ES/EN planificados; tablas operativas no demostradas |
| Press kit | No existe |
| Comunidad pública | No existe hub ni moderación operativa |
| Soporte | No existe canal final ni SLA público |
| Privacidad | No existe política final; newsletter/analytics no abiertos |
| Campaña | `BLOCKED` |
| Comunicación permitida | Interna y portfolio técnico controlado, correctamente etiquetado |

## 2.4. Objetivos del plan

1. Proteger el cierre del vertical slice.
2. Construir una identidad comercial reconocible y honesta.
3. Validar que la fantasía de tienda física resulta comprensible y atractiva.
4. Preparar materiales reutilizables y trazables.
5. Abrir audiencia de forma gradual, no irreversible.
6. Convertir atención en acciones medibles: visita cualificada, wishlist, test, feedback, compra y recomendación.
7. Mantener una comunidad manejable para una producción individual.
8. Evitar compromisos públicos sobre sistemas `NOT OPEN`.
9. Coordinar marketing con Steam, legal, localización, privacidad, seguridad y soporte.
10. Aprender con cada pieza y cerrar campañas que no justifican su coste.

## 2.5. No objetivos

Este plan no autoriza:

- una fecha de lanzamiento;
- un precio;
- Early Access;
- crowdfunding;
- preorders;
- demo pública;
- Steam Playtest;
- participación en festivales;
- campaña con influencers;
- newsletter;
- Discord oficial;
- anuncios pagados;
- promesas de consolas, Linux, macOS o Steam Deck;
- promesas de empleados, comercio online, publishing, desarrollo interno, plataforma o infraestructura;
- uso público de conceptos, música, fuentes, nombres o marcas no autorizados.

---

# 3. Principios rectores de marketing y comunicación

## 3.1. Veracidad por encima de espectacularidad

Una pieza menos llamativa pero cierta protege el proyecto. Una pieza espectacular que muestra sistemas inexistentes produce deuda de expectativa, riesgo legal y pérdida de confianza.

## 3.2. Producto antes que calendario

El calendario de publicaciones se adapta a los gates. Los gates no se rebajan para alimentar redes. El silencio planificado es preferible a producir contenido que interrumpa S16, S17 o H6.

## 3.3. Comunicación proporcional a la capacidad

La producción es principalmente individual. Cada canal abierto crea mantenimiento, moderación, respuesta y riesgo. Se priorizarán pocos canales con continuidad frente a presencia superficial en todos.

## 3.4. Reutilización con procedencia

Una sesión de captura aprobada debe poder producir screenshots, GIFs, fragmentos de tráiler, miniaturas, press kit y posts, conservando build ID, resolución, idioma, fecha y licencia de todos los elementos.

## 3.5. Comparables como traducción, no como identidad

Los comparables ayudan a que una persona entienda género o expectativa. No se utilizarán frases como “el nuevo Game Dev Tycoon”, “X pero mejor” o “el clon de”. La diferenciación debe describir control directo, tienda física, logística visible, clientes observables y progresión espacial.

## 3.6. Visión separada de compromiso

La visión de publishing, estudio y plataforma puede explicarse en documentos de diseño o conversaciones específicas con una etiqueta clara. No se presentará como contenido incluido, roadmap comprometido o razón para comprar la versión inicial.

## 3.7. Accesibilidad y localización desde el material

El marketing debe ser legible, subtitulado cuando corresponda, comprensible sin audio y coherente en ES/EN. La accesibilidad no es solo un claim: cada afirmación necesita evidencia en build.

## 3.8. Privacidad por defecto

Se priorizarán datos agregados de Steam y observación cualitativa antes de instalar SDKs. Newsletter, tracking web, formularios y campañas con píxeles permanecen cerrados hasta tener finalidad, proveedor, consentimiento, política y retención.

## 3.9. Seguridad y embargo

Las claves, builds privadas, listas de creadores, emails, credenciales, documentos bajo embargo y assets no publicados se gestionan con mínimo privilegio, MFA, registro de entrega y revocación cuando proceda.

## 3.10. Aprendizaje accionable

No se mide por acumular seguidores. Cada métrica debe relacionarse con una decisión: cambiar copy, mejorar una captura, ajustar canal, revisar onboarding, retrasar una demo o detener una campaña.

---

# 4. Verdad de producto y perímetro comunicable

## 4.1. Núcleo comunicable cuando exista evidencia representativa

El núcleo de mensaje se limitará a:

- gestión de una tienda de videojuegos;
- control de un personaje en un espacio 3D;
- construcción y organización funcional;
- pedidos y recepción de mercancía;
- inventario y reposición visible;
- asignación de productos a expositores;
- precios y economía comprensible;
- clientes observables;
- cola y checkout;
- ciclo diario;
- cierre, resultados, guardado y carga;
- progresión del pequeño negocio dentro del alcance realmente aprobado.

## 4.2. Contenido que requiere claim matrix

Toda frase sobre las siguientes capacidades debe vincularse a prueba y build:

- libertad de construcción;
- comportamiento de clientes;
- profundidad económica;
- número de productos o muebles;
- duración;
- rejugabilidad;
- accesibilidad;
- idioma;
- soporte de mando;
- rendimiento;
- guardado;
- recuperación;
- compatibilidad con Steam Deck;
- actualizaciones futuras;
- soporte postlanzamiento.

## 4.3. Contenido diferido que no puede venderse como presente

- empleados;
- investigación profunda;
- puestos informáticos;
- comercio online;
- publishing;
- desarrollo interno de videojuegos;
- plataforma digital;
- infraestructura;
- mercado competitivo avanzado;
- multijugador;
- modding;
- logros;
- Steam Cloud;
- mando completo;
- otras plataformas.

## 4.4. Etiquetas obligatorias de material

| Etiqueta | Uso |
|---|---|
| `GAMEPLAY — BUILD` | Captura o vídeo real de build identificada. |
| `EDITOR CAPTURE` | Captura del Editor; no se confunde con experiencia final. |
| `WORK IN PROGRESS` | Implementación real todavía no aprobada como representativa. |
| `TECHNICAL PROTOTYPE` | Prueba técnica destinada a desarrollo/portfolio. |
| `MOCKUP` | Composición no ejecutable. |
| `CONCEPT ART` | Exploración visual, no captura. |
| `VISION / NOT OPEN` | Idea de largo plazo no comprometida. |
| `PLACEHOLDER` | Asset temporal pendiente de sustitución. |

Las etiquetas deben aparecer en nombre de archivo, registro interno y publicación cuando el contexto pueda inducir a error.

## 4.5. Matriz de madurez comunicativa

| Madurez | Audiencia | Permitido | Prohibido |
|---|---|---|---|
| Pre-H6 | Equipo, QA, portfolio técnico selectivo | procesos, decisiones, comparativas técnicas etiquetadas | campaña, “demo”, fecha, precio, wishlist CTA |
| H6 PASS | revisión interna y validación controlada | reel interno, research, tests cerrados | Coming Soon automática, festival automático |
| Pre-Steam aprobado | audiencia inicial | identidad, devlogs representativos, preparación de página | fecha y features futuras no aprobadas |
| Coming Soon | público Steam | gameplay real, wishlist, hechos estables | roadmap especulativo |
| Demo/Playtest | cohortes/público según decisión | contenido exacto de build de prueba | transferencias/save/promesas no verificadas |
| Release | compradores | propuesta final, precio, fecha, soporte | claims no demostrados |
| Postlaunch | jugadores y comunidad | estado, parches, roadmap aprobado | minimizar incidencias o prometer plazos sin QA |

---

# 5. Posicionamiento estratégico

## 5.1. Categoría primaria

Simulador de gestión de tienda en 3D con construcción funcional, logística visible y operación directa.

## 5.2. Categorías secundarias

- shop simulator;
- management/tycoon;
- building/layout;
- logistics light;
- business progression;
- cozy management, siempre que el tono y la presión real de la build lo justifiquen.

“Cozy” no debe emplearse como garantía de ausencia de tensión si existen pérdidas, fallos operativos o presión temporal significativa.

## 5.3. Fantasía central

Empezar con un local modesto, tocar cada parte del negocio y convertir un espacio desordenado en una tienda eficiente, legible y propia.

## 5.4. Propuesta de valor breve

> Diseña y gestiona una tienda de videojuegos en 3D: pide mercancía, organiza expositores, atiende clientes y haz crecer un pequeño local mediante decisiones físicas y comerciales que puedes ver.

## 5.5. Propuesta de valor desarrollada

*Cartridge & Cloud* combina la satisfacción de ordenar y construir un espacio con la lectura de un negocio vivo. Los productos ocupan lugar, las rutas importan, el stock debe llegar y moverse, los clientes muestran intención y la caja convierte el trabajo diario en resultados. La progresión debe sentirse como acumulación y transformación, no como sustitución de la tienda por una pantalla abstracta.

## 5.6. Diferenciadores prioritarios

1. **Trabajo visible:** pedir, recibir, trasladar, colocar, reponer y vender.
2. **Espacio con consecuencias:** el layout afecta circulación, acceso y operación.
3. **Clientes observables:** se puede entender por qué buscan, esperan, compran o abandonan.
4. **Control directo:** el jugador se mueve por el establecimiento.
5. **Crecimiento acumulativo:** las etapas nuevas nacen de capacidades anteriores.
6. **Industria ficticia propia:** productos y proveedores originales, sin depender de licencias reales.
7. **Claridad sistémica:** las decisiones deben tener explicación y feedback.
8. **Tono cálido-tecnológico:** nostalgia del comercio físico y evolución hacia capacidades digitales.

## 5.7. Anti-posicionamiento

No se presentará como:

- simulador de desarrollo de videojuegos;
- clon de un tycoon conocido;
- experiencia de colección basada en marcas reales;
- simulador hiperrealista de retail;
- juego idle;
- sandbox sin objetivos;
- promesa de gestionar toda la industria desde el primer día;
- producto terminado mientras continúe como vertical slice interno.

## 5.8. Escalera de valor

| Nivel | Valor comunicado |
|---|---|
| Funcional | Gestionar productos, espacio, clientes y dinero. |
| Experiencial | Sentir orden, control, mejora y ritmo diario. |
| Identitario | Crear “mi” tienda, con una distribución y especialización propias. |
| Aspiracional | Convertir un pequeño local en una empresa más capaz. |
| Narrativo | Recorrer la transición simbólica del cartucho físico a la nube. |

---

# 6. Audiencias y segmentos

## 6.1. Audiencia primaria: jugadores de gestión y shop simulators

**Buscan:** progreso, optimización, tareas encadenadas, mejoras visibles y una fantasía operativa clara.  
**Necesitan ver:** el bucle completo, no solo decoración.  
**Barrera:** sospecha de repetición superficial o contenido escaso.  
**Prueba adecuada:** vídeo de pedido → recepción → reposición → cliente → venta → resumen.

## 6.2. Audiencia secundaria: jugadores de construcción y organización

**Buscan:** autoría espacial, orden, antes/después y personalización funcional.  
**Necesitan ver:** grid, rotación, acceso, compatibilidad y variedad de layout.  
**Barrera:** que la construcción sea cosmética o demasiado rígida.  
**Prueba adecuada:** comparativa de dos tiendas funcionales y consecuencias de acceso.

## 6.3. Audiencia secundaria: público de tycoon y progresión empresarial

**Buscan:** crecimiento, economía, decisiones y desbloqueos.  
**Necesitan ver:** resultados explicables y perspectiva de expansión sin sobrepromesa.  
**Barrera:** que el juego sea un simulador de tareas sin estrategia.  
**Prueba adecuada:** decisiones de precio, stock, capacidad y resultados diarios.

## 6.4. Audiencia secundaria: aficionados a la cultura del videojuego

**Buscan:** atmósfera de tienda, productos ficticios reconocibles, nostalgia y evolución tecnológica.  
**Necesitan ver:** identidad propia y referencias de época sin copiar marcas.  
**Barrera:** parodia fácil, nombres genéricos o dependencia de IP real.  
**Prueba adecuada:** packaging, proveedores ficticios y ambiente del local.

## 6.5. Audiencia profesional y de proceso

Incluye desarrolladores, reclutadores, otros estudios y personas interesadas en Unity. Es una audiencia válida para portfolio técnico, pero no debe dominar el mensaje comercial del juego. Un post sobre arquitectura, QA o autoría de escenas puede funcionar en GitHub/LinkedIn sin presentarse como campaña de jugadores.

## 6.6. Audiencias no prioritarias iniciales

- público competitivo;
- jugadores que esperan multijugador;
- coleccionistas de marcas reales;
- jugadores que buscan acción rápida;
- audiencia de simulación técnica extrema;
- público atraído solo por desarrollo de juegos.

No se intentará satisfacer todos los segmentos con un único copy.

## 6.7. Personas de trabajo

### Persona A — “Optimizador de tienda”
Valora eficiencia, flujo y economía. Tolera arte estilizado si las reglas son claras. Abandona si los clientes parecen aleatorios o el stock se duplica.

### Persona B — “Constructor organizado”
Valora distribución, legibilidad y transformación del espacio. Abandona si no puede anticipar por qué una colocación es válida o si el mobiliario no cambia la operación.

### Persona C — “Tycoon de largo plazo”
Valora progresión y decisiones de negocio. Debe entender que la visión es amplia, pero no debe comprar basándose en features no confirmadas.

### Persona D — “Nostálgico del retail”
Valora ambiente, packaging y memoria cultural. Debe percibir inspiración sin copias de consolas, logos o carátulas reales.

### Persona E — “Seguidor del desarrollo”
Valora transparencia, decisiones y evolución técnica. Puede ser un primer amplificador, pero no sustituye la validación con compradores reales.

## 6.8. Preguntas de validación de audiencia

- ¿La persona entiende en diez segundos que gestiona una tienda?
- ¿Percibe control directo y logística visible?
- ¿Distingue construcción funcional de decoración?
- ¿Comprende qué hace diferente al juego sin mencionar un comparable?
- ¿Qué parte del bucle desea repetir?
- ¿Qué espera que exista y no está en la build?
- ¿Considera atractiva la estética sin conocer la visión futura?
- ¿Qué palabra usa espontáneamente para describir el juego?
- ¿Qué información falta para añadirlo a wishlist?
- ¿Qué claim genera desconfianza?

---

# 7. Investigación de mercado y comparables

## 7.1. Finalidad

La investigación no busca copiar mecánicas ni estética. Debe informar:

- vocabulario que el público reconoce;
- expectativas de contenido y ritmo;
- estructura de store pages;
- formatos de tráiler;
- tags;
- precios y descuentos cuando llegue el momento;
- reseñas que revelan necesidades no cubiertas;
- señales de saturación o diferenciación.

## 7.2. Categorías de comparables

1. Simuladores de tienda física.
2. Tycoons de crecimiento empresarial.
3. Juegos de construcción funcional.
4. Simuladores logísticos accesibles.
5. Management con estética cozy.
6. Juegos sobre industria del videojuego.

## 7.3. Uso histórico de Game Dev Tycoon

El histórico exige evitar “el nuevo Game Dev Tycoon”. Puede usarse internamente para explicar una expectativa de progresión empresarial, pero la comunicación debe subrayar que *Cartridge & Cloud* comienza en venta física, controla un personaje, representa productos en espacio real, muestra clientes y trata publishing/desarrollo como expansiones diferidas.

## 7.4. Ficha de comparable

Cada comparable debe registrar:

- título y fecha de consulta;
- plataforma;
- estado de lanzamiento;
- precio base y descuento observado;
- tags;
- propuesta de valor;
- primeros treinta segundos del tráiler;
- distribución de screenshots;
- volumen de contenido comunicado;
- temas positivos y negativos de reviews;
- comunidad y frecuencia de updates;
- similitudes;
- diferencias;
- aprendizaje;
- riesgo de imitación;
- decisión que puede informar.

## 7.5. Reglas de investigación

- usar fuentes fechadas;
- no extrapolar ventas sin fuente;
- no convertir seguidores en ventas;
- separar correlación y causalidad;
- revisar comparables de éxito y de fracaso;
- no copiar palabras, key art, composición o tagline;
- actualizar antes de precio, tags y lanzamiento;
- archivar capturas/links conforme a uso interno y copyright.

## 7.6. Entregable

`MKT-RES-001_Market_and_Comparable_Research.xlsx` o sección equivalente en el futuro Excel operativo, con una nota ejecutiva que convierta observaciones en decisiones. Este archivo no se genera con el documento 28 y queda como work package.

---

# 8. Arquitectura de marca

## 8.1. Estado del título

`Cartridge & Cloud` es el nombre provisional oficial. No debe congelarse como marca final, comprar dominios irreversibles, crear campaña de alto alcance ni enviarse a Steam review hasta completar búsqueda de marcas, tiendas, compañías, dominios y redes.

## 8.2. Candidatos históricos preservados

1. Cartridge & Cloud.
2. Shelf to Server.
3. Game District.
4. Pixels & Profits.
5. Save Point Market.

Se conservan como registro histórico. No constituyen una shortlist vigente sin una nueva revisión.

## 8.3. Estado del nombre del estudio

`VRM Games` es nombre operativo interno y candidato de developer/publisher. Debe verificarse forma jurídica, consistencia con Steamworks, dominio, redes y posibles conflictos antes de comunicación comercial.

## 8.4. Significado de la marca

- **Cartridge:** objeto, inventario, estantería, nostalgia, comercio físico, posesión visible.
- **Cloud:** escalado, datos, distribución, servicios y ambición tecnológica futura.
- **&:** continuidad entre etapas; no dos juegos separados.

La marca debe comunicar transición y acumulación, no abandonar el retail en cuanto aparece una capa digital.

## 8.5. Sistema visual

La dirección consolidada utiliza:

- fondos grafito y oscuros;
- verde VRM como acento de marca;
- cyan como acento tecnológico secundario;
- madera, cartón, crema y luz cálida para retail;
- productos y clientes con mayor diversidad cromática;
- low poly estilizado;
- texturas handpainted/cartoon;
- bordes tipo tinta moderados;
- UI técnica, limpia y legible.

## 8.6. Restricción de color

El verde de marca no sustituye estructura, contraste, icono o texto. Debe diferenciarse del verde de éxito/validación. Los proveedores ficticios y clientes no se visten ni diseñan como extensiones de VRM Games.

## 8.7. Logo futuro

El paquete de logo debe incluir:

- versión principal;
- horizontal;
- compacta;
- monocroma clara;
- monocroma oscura;
- icono;
- safe area;
- tamaño mínimo;
- reglas sobre fondos;
- fuente editable y licencia;
- exports vector/raster;
- variante sin subtítulo;
- test en cápsulas pequeñas.

No se crea logo final antes del clearance del título.

## 8.8. Voz de marca

**Profesional, cercana, clara, curiosa y honesta.**  
No adopta una voz corporativa grandilocuente ni un humor basado en memes efímeros. Puede usar humor ligero de situaciones de tienda, pero no ridiculiza al jugador, a clientes, trabajadores o comunidades.

## 8.9. Léxico preferido

Usar: tienda, stock, pedido, recepción, exposición, cliente, distribución, operación, crecer, organizar, comprender, construir, negocio.  
Evitar: imperio, dominar el mercado, revolucionario, infinito, definitivo, realista, IA avanzada, totalmente dinámico, sin límites, “próximamente” sin plan.

---

# 9. Casa de mensajes

## 9.1. Mensaje maestro

> Construye una tienda de videojuegos que funcione: cada producto, mueble, ruta y cliente forma parte de un negocio que puedes observar, comprender y mejorar.

## 9.2. Pilar 1 — Tienda viva

**Mensaje:** el local no es un fondo; es el espacio donde ocurre el negocio.  
**Pruebas:** clientes visibles, entrada, recorridos, cola, checkout, cambios de jornada.  
**Riesgo:** afirmar una simulación social profunda que la build no demuestre.

## 9.3. Pilar 2 — Logística visible

**Mensaje:** el producto debe pedirse, recibirse, moverse, exponerse y venderse.  
**Pruebas:** delivery, almacén, displays, reservas, reposición y consumo de stock.  
**Riesgo:** mostrar acciones de debug o automatizaciones no destinadas al jugador.

## 9.4. Pilar 3 — Construcción funcional

**Mensaje:** la colocación influye en acceso y operación.  
**Pruebas:** grid, huellas, rotación, preview válido/inválido, rutas y puntos obligatorios.  
**Riesgo:** presentarlo como libertad ilimitada.

## 9.5. Pilar 4 — Decisiones comprensibles

**Mensaje:** precios, inventario y espacio producen resultados explicables.  
**Pruebas:** feedback, resumen diario, causas de abandono y movimientos económicos.  
**Riesgo:** usar “economía profunda” antes de validar balance y variedad.

## 9.6. Pilar 5 — Crecimiento visible

**Mensaje:** el pequeño local mejora en capacidad, organización e identidad.  
**Pruebas:** antes/después, nuevo mobiliario, más catálogo o mejor flujo dentro del alcance aprobado.  
**Riesgo:** mostrar las etapas futuras como si estuvieran incluidas.

## 9.7. Pilar 6 — Nostalgia sin copia

**Mensaje:** atmósfera de tienda de videojuegos mediante materiales, packaging y cultura ficticia.  
**Pruebas:** productos y proveedores originales.  
**Riesgo:** trade dress, siluetas, marcas o nombres confundibles con IP real.

## 9.8. Pilar 7 — Desarrollo transparente

**Mensaje:** el proceso se comunica con etiquetas y evidencia.  
**Pruebas:** build IDs, comparativas, decisiones, known issues.  
**Riesgo:** convertir cada deuda en contenido o exponer información sensible.

## 9.9. Pitch por longitud

**Una línea:** Gestiona una tienda de videojuegos en 3D donde el espacio, el stock y los clientes importan.

**Corto:** Pide mercancía, organiza expositores, fija precios y atiende clientes mientras conviertes un pequeño local en una tienda eficiente y propia.

**Desarrollado:** *Cartridge & Cloud* es un simulador de gestión 3D en el que operas físicamente una tienda de videojuegos. Diseñas la distribución, recibes pedidos, mueves inventario, asignas productos, repones displays y observas cómo tus decisiones afectan a clientes y resultados diarios.

## 9.10. Borrador interno de descripción Steam

Este copy procede del Steam Publishing Plan y se conserva como borrador no publicable:

**ES:** Gestiona una pequeña tienda de videojuegos: compra mercancía, recibe pedidos, organiza expositores y atiende a tus clientes mientras conviertes un local modesto en un negocio eficiente.

**EN:** Run a small video game shop: order stock, receive deliveries, arrange displays, and serve customers as you turn a modest storefront into an efficient business.

Antes de publicarlo debe contrastarse con la build, el posicionamiento final, la longitud de Steam y revisión lingüística.

---

# 10. Matriz de claims y lenguaje de certeza

## 10.1. Clases de claim

| Clase | Ejemplo | Requisito |
|---|---|---|
| Hecho de producto | “Coloca expositores” | Feature integrada y probada |
| Resultado | “Mejora el flujo” | Evidencia de efecto o redacción prudente |
| Volumen | “Más de X productos” | Catálogo final y build |
| Compatibilidad | “Steam Deck” | Prueba y declaración aprobada |
| Accesibilidad | “Escala de UI” | Opción funcional, persistente y QA |
| Idioma | “Interfaz en español e inglés” | Cobertura in-game y store |
| Calidad | “Relajante”, “profundo” | Validación cualitativa; no garantía |
| Futuro | “Llegarán empleados” | Roadmap público aprobado |
| Superlativo | “El simulador definitivo” | Prohibido salvo base excepcional y revisión |

## 10.2. Lenguaje autorizado por estado

- `Existe / incluye`: solo `PUBLIC READY`.
- `Está en desarrollo`: implementación abierta y comunicable con etiqueta.
- `Estamos explorando`: experimento sin compromiso.
- `Visión a largo plazo`: solo en contexto explícito, no en lista de features de compra.
- `Planeado`: requiere backlog aprobado, owner y gate.
- `Nos gustaría`: evitar; suele crear expectativa sin decisión.
- `Próximamente`: prohibido sin ventana aprobada.
- `Compatible`: requiere QA.
- `Optimizado`: requiere métricas comparativas.

## 10.3. Registro de claims

Cada claim público recibirá ID `MKT-CLM-###` y campos:

- texto ES;
- texto EN;
- categoría;
- feature/requisito fuente;
- build y commit;
- prueba;
- asset asociado;
- limitación;
- aprobador;
- fecha;
- canales;
- estado;
- fecha de revisión;
- motivo de retirada.

---

# 11. Estrategia de canales

## 11.1. Principio de selección

Un canal se abre solo si tiene:

- audiencia y objetivo definidos;
- capacidad de publicación;
- capacidad de respuesta;
- seguridad de cuenta;
- política de datos;
- archivo/export;
- owner;
- criterio de cierre o abandono.

## 11.2. Steam

**Función:** conversión, wishlists, información oficial, demo/Playtest, anuncios, comunidad y venta.  
**Estado:** `BLOCKED / NOT ONBOARDED`.  
**Entrada:** gate Pre-Steam, identidad, materiales, copy, soporte y presupuesto.  
**Contenido:** store page, announcements, screenshots, tráiler, FAQ, patch notes.  
**Métrica principal:** visitas cualificadas → wishlist/compra, según datos agregados disponibles.

## 11.3. Web o landing page

**Función:** fuente oficial fuera de plataforma, press kit, contacto, privacidad y enlaces.  
**Estado:** no existe.  
**MVP recomendado:** página estática, rápida, accesible, sin tracking por defecto; proyecto, screenshots, Steam CTA cuando exista, press kit, contacto y políticas.  
**Bloqueos:** dominio/nombre, hosting, privacidad, seguridad y mantenimiento.

## 11.4. Portfolio VRM Games

**Función:** demostrar proceso y capacidad profesional.  
**Uso actual posible:** posts técnicos seleccionados con etiquetas WIP/technical prototype.  
**Restricción:** no debe parecer una store page ni afirmar disponibilidad pública.

## 11.5. GitHub

**Función:** proceso técnico y portfolio cuando el repositorio o extractos sean publicables.  
**Riesgos:** secretos, assets sin redistribución, spoilers, superficie de seguridad, confusión entre código y producto final.  
**Regla:** este plan no autoriza hacer público el repositorio. Cualquier publicación requiere revisión legal y de seguridad.

## 11.6. LinkedIn

**Función:** comunicación profesional, hitos de producción, aprendizaje y colaboración.  
**Contenido adecuado:** decisiones de diseño, QA, pipelines, hitos cerrados y reflexiones.  
**Contenido inadecuado:** CTA constante a jugadores, claims de lanzamiento o material sin clearance.

## 11.7. YouTube

**Función:** devlogs, tráileres, breakdowns y contenido con vida útil larga.  
**Entrada recomendada:** material representativo suficiente y capacidad de edición/subtítulos.  
**Formatos:** devlog de 4–8 minutos, clip de 30–60 segundos, tráiler y breakdown técnico.  
**Regla:** priorizar claridad de gameplay y subtítulos; no producir series semanales insostenibles.

## 11.8. Redes de formato corto

**Función:** descubrimiento mediante clips de transformación, logística y clientes.  
**Estado:** opcional.  
**Condición:** un pipeline de reutilización debe producir material sin interrumpir desarrollo.  
**Criterio de abandono:** baja señal cualificada tras una muestra suficiente o coste operativo desproporcionado.

## 11.9. Discord

**Estado:** `NOT OPEN`.  
Abrir Discord implica moderación, privacidad, seguridad, archivo, expectativas de respuesta y fragmentación respecto a Steam. Solo se considerará si existe una comunidad activa que necesite conversación persistente y capacidad real de moderación.

## 11.10. Newsletter

**Estado:** `NOT OPEN / PRIVACY BLOCKED`.  
Requiere proveedor, finalidad, base legal/consentimiento, doble opt-in cuando proceda, política, baja, retención, DPA, seguridad y calendario. Steam wishlists pueden cubrir inicialmente la necesidad de notificación sin crear otra base de datos.

## 11.11. Prensa y creadores

No es un canal de publicación continua, sino una relación de outreach. Se gestiona mediante lista cualificada, press kit, briefing, build estable, claves seguras y seguimiento sin exigir cobertura positiva.

## 11.12. Prioridad inicial recomendada

1. Fuente interna y archivo de materiales.
2. Steam cuando se autorice.
3. Web/press kit mínimo.
4. YouTube o vídeo de alta reutilización.
5. LinkedIn/portfolio para proceso.
6. Un canal corto como experimento.
7. Comunidad adicional solo por necesidad demostrada.

---

# 12. Estrategia editorial y pilares de contenido

## 12.1. Pilar A — Transformación del espacio

- antes/después;
- diseño de layout;
- por qué una entrada o pasillo importa;
- creación de `StoreInitial`;
- mobiliario modular;
- iluminación y señalética.

## 12.2. Pilar B — Cadena de producto

- pedido;
- llegada;
- cajas;
- almacén;
- asignación;
- reposición;
- venta;
- conservación de stock.

## 12.3. Pilar C — Clientes legibles

- búsqueda;
- preferencia;
- reserva;
- cola;
- compra;
- abandono explicado;
- respuesta a precio o disponibilidad.

No se presenta como “IA avanzada” sin una definición y prueba.

## 12.4. Pilar D — Decisiones de negocio

- precio;
- capacidad;
- gasto;
- inventario;
- resumen diario;
- recuperación de errores;
- trade-offs.

## 12.5. Pilar E — Arte y mundo ficticio

- productos;
- proveedores;
- packaging;
- paletas;
- personajes;
- props;
- inspiración retro sin copia.

## 12.6. Pilar F — Ingeniería y QA

- arquitectura;
- persistencia;
- pruebas;
- autoría de escena;
- profiling;
- accesibilidad;
- localización;
- build reproducible.

Este pilar se orienta más a audiencia profesional y debe evitar saturar los canales de jugadores.

## 12.7. Pilar G — Historia del proyecto

- decisiones sustituidas;
- evolución 10×10 → 10×15;
- de blockout a escena autorada;
- por qué se congela alcance;
- cómo se define H6;
- lecciones de sprints.

## 12.8. Fórmula de una pieza útil

1. Gancho visual o pregunta.
2. Una acción o decisión concreta.
3. Evidencia real.
4. Consecuencia para el jugador.
5. Estado del material.
6. CTA proporcional: comentar, responder una pregunta, seguir o wishlist cuando exista.
7. Archivo de fuente y métricas.

## 12.9. Cadencia

No se fija una cadencia pública hasta aprobar el canal. Como regla de capacidad:

- pre-H6: publicación oportunista por hito, no calendario;
- premarketing: una pieza sustantiva cada 2–4 semanas puede ser suficiente;
- Coming Soon: cadencia basada en hitos y assets, no relleno semanal;
- lanzamiento: mayor frecuencia solo con material preproducido;
- postlaunch: prioridad a soporte, parches y claridad.

## 12.10. Límite de capacidad

Por defecto, marketing no consumirá más del 10 % de la capacidad de un sprint antes de H6 ni más del 15 % después del gate de premarketing, salvo decisión explícita que registre beneficio esperado y trabajo de producto desplazado. Son parámetros de gobernanza iniciales y pueden revisarse con evidencia.

---

# 13. Pipeline de contenidos

## 13.1. Flujo general

Idea → objetivo → audiencia → source de producto → guion/shot list → captura → edición → revisión técnica → revisión legal → revisión ES/EN → accesibilidad → aprobación → programación → publicación → archivo → métricas → aprendizaje.

## 13.2. Brief obligatorio

Cada pieza debe registrar:

- ID;
- objetivo;
- canal;
- audiencia;
- mensaje;
- claim IDs;
- CTA;
- formato y duración;
- build/commit;
- assets;
- idioma;
- subtítulos/alt text;
- owner;
- reviewers;
- fecha objetivo;
- estado;
- riesgos;
- métricas;
- resultado y siguiente acción.

## 13.3. Source of truth

Los archivos editables viven en una estructura controlada fuera de carpetas de build, por ejemplo:

```text
Marketing/
  00_Governance/
  01_Brand/
  02_Copy/
  03_Capture_Source/
  04_Screenshots/
  05_Video/
  06_Steam/
  07_Press_Kit/
  08_Outreach/
  09_Campaigns/
  10_Metrics/
  11_Archive/
```

La ruta definitiva se aprobará sin mezclar credenciales, claves, datos personales o masters no redistribuibles en repositorios públicos.

## 13.4. Convención de nombres

```text
CC_MKT_<TYPE>_<SUBJECT>_<LANG>_<BUILD>_<YYYYMMDD>_v###
```

Ejemplos:

```text
CC_MKT_SCREENSHOT_Delivery_ES_BLD-H6_20260715_v001.png
CC_MKT_CLIP_StockToShelf_EN_BLD-PLAYTEST-02_20260820_v003.mp4
CC_MKT_COPY_SteamShort_ESNA_v004.md
```

## 13.5. Versionado y retirada

No se sobreescribe un asset publicado. Se conserva:

- editable;
- export;
- fuente;
- hash;
- build de procedencia;
- licencia;
- aprobación;
- canales;
- fecha;
- estado `ACTIVE`, `SUPERSEDED` o `RETIRED`.

---

# 14. Capturas y GIFs

## 14.1. Requisitos de captura

Toda captura comercial debe proceder de una build o Editor identificado, registrar resolución, escala de UI, idioma, ajustes gráficos, commit, fecha y escena. Para gameplay presentado como final o representativo se exige build, no mockup.

## 14.2. Shot list inicial

1. Exterior/entrada de la tienda.
2. Vista general del local.
3. Recepción de un pedido.
4. Movimiento de stock.
5. Colocación de mobiliario con preview.
6. Reposición de un display.
7. Cliente buscando producto.
8. Cola y checkout.
9. Panel de operaciones o economía.
10. Resumen de cierre.
11. Comparativa antes/después.
12. Estado de error o falta de stock, si aporta comprensión.

La selección final debe mostrar variedad real y no doce ángulos de la misma escena.

## 14.3. Composición

- acción principal legible;
- UI suficiente para demostrar sistema, no saturar;
- texto no cortado;
- sin debug;
- sin datos personales;
- sin assets `NOT OPEN`;
- sin productos o nombres sin clearance;
- contraste y encuadre consistentes;
- diversidad de iluminación solo si pertenece a la build;
- evitar ángulos que oculten limitaciones relevantes.

## 14.4. Retoque

Se permiten correcciones de crop, exposición moderada y export siempre que no alteren estado de juego, número de objetos, calidad del asset, UI o resultado. El fotomontaje se etiqueta. Una captura retocada no sirve como evidencia de QA.

## 14.5. GIF y clip corto

Debe mostrar una transformación completa en 3–12 segundos:

- caja → stock;
- preview inválido → corrección → colocación;
- estante vacío → reposición;
- cliente → cola → venta;
- cierre → resultados.

Se prueba legibilidad sin audio, loop, subtítulos si hay texto y compresión en destino.

## 14.6. Procedencia

Cada archivo recibe una hoja o registro con:

- build ID;
- commit;
- escena;
- cámara;
- save de origen;
- idioma;
- ajustes;
- fuente de audio;
- licencias;
- editor utilizado;
- modificaciones;
- aprobación.

---

# 15. Tráileres y vídeo

## 15.1. Tipos previstos

1. **Internal validation reel:** evidencia H6, no público por defecto.
2. **Announcement/Coming Soon trailer:** presenta fantasía y bucle.
3. **Gameplay overview:** explica sistemas.
4. **Demo/Playtest trailer:** delimita contenido de prueba.
5. **Launch trailer:** representa producto vendible.
6. **Devlog:** proceso y decisiones.
7. **Patch/update video:** solo si el valor justifica producción.

## 15.2. Principio de estructura

Un tráiler debe mostrar primero qué hace el jugador, después cómo progresa y finalmente por qué importa. La visión futura no sustituye gameplay.

## 15.3. Estructura de announcement trailer propuesta

- 0–5 s: tienda y fantasía central;
- 5–20 s: pedido, recepción, construcción y reposición;
- 20–35 s: clientes, cola y venta;
- 35–50 s: precio, resultados y crecimiento visible;
- 50–60 s: identidad, título y CTA a wishlist si la página existe.

La duración es orientación, no requisito.

## 15.4. Audio

No se utilizarán los loops placeholder actuales como master de tráiler. Música, SFX y voz deben tener licencia, master y mezcla aprobados. El tráiler debe funcionar con subtítulos y sin depender de narración.

## 15.5. Texto y claims

- no más claims de los que puede probar el montaje;
- tipografía con licencia;
- safe areas;
- ES/EN como ediciones coherentes, no subtítulos literales automáticos;
- fecha solo si está aprobada;
- plataformas solo si están confirmadas;
- CTA solo si existe destino.

## 15.6. Entregables de vídeo

- master mezzanine;
- versión plataforma;
- versión subtitulada;
- archivo de subtítulos;
- thumbnail;
- transcript;
- cue sheet/licencias;
- lista de shots con build;
- hash;
- proyecto editable archivado.

---

# 16. Key art, cápsulas y materiales de marca

## 16.1. Objetivo del key art

Comunicar de un vistazo tienda, producto físico, gestión y evolución sin mostrar sistemas ausentes. Debe poder adaptarse a formatos Steam y prensa.

## 16.2. Brief visual inicial

Escena interior cálida de una pequeña tienda de videojuegos, con personaje dependiente, estanterías, productos ficticios, cajas de recepción y clientes. El verde/cyan identifica marca e interfaz; proveedores y ropa aportan diversidad. El fondo puede sugerir crecimiento mediante profundidad o elementos arquitectónicos, no mediante servidores o plataformas inexistentes presentados como gameplay.

## 16.3. Restricciones

- no copiar consolas, mandos o packaging reales;
- no usar conceptos sin autorización de uso público;
- no mostrar demasiados sistemas;
- no incluir precio, review score o descuento en cápsulas base;
- no congelar título antes del clearance;
- no utilizar una composición que haga parecer el juego 2D, first-person o puramente decorativo.

## 16.4. Adaptación Steam

Las dimensiones deben revalidarse con las plantillas vigentes al producir. El Steam Publishing Plan registra como referencia Header `920×430`, Small `462×174` y Main `1232×706`, pero la fuente oficial y templates descargados en Steamworks prevalecen.

## 16.5. Biblioteca y ejecutable

El sistema incluye library capsule, hero, logo, icono de aplicación y otros assets exigidos. Cada variante debe probar legibilidad a tamaño real y fondo real.

## 16.6. Paquete de marca

`CC_Brand_Package_v###` contendrá guía, logos, paleta, tipografías, patrones, examples, do/don’t, licencia, fuente editable y exports.

---

# 17. Store page y conversión

## 17.1. Relación con el documento 24

El Steam Publishing Plan define campos, revisión, assets, AppID y release. Este capítulo define cómo esos elementos forman una narrativa de conversión.

## 17.2. Secuencia de la página

1. Cápsula y título: reconocimiento.
2. Tráiler o primera captura: fantasía.
3. Descripción corta: categoría y acción.
4. Screenshots: prueba de variedad.
5. “Acerca de”: bucle y diferenciadores.
6. Features: hechos concretos.
7. Idiomas, requisitos y accesibilidad: confianza.
8. Links y soporte: legitimidad.
9. CTA de wishlist: solo cuando la página sea pública.

## 17.3. Primera pantalla

La persona debe entender sin scroll excesivo:

- que es un juego de gestión de tienda;
- que se juega en 3D;
- que hay stock, organización y clientes;
- que no es un juego de desarrollo de videojuegos;
- que el arte mostrado corresponde al producto.

## 17.4. Screenshots mínimas

El Steam Publishing Plan exige screenshots reales y propone un mínimo operativo de cinco variadas. Este plan recomienda preparar 8–12 aprobadas para elegir, sin publicar redundancias.

## 17.5. A/B y aprendizaje

Si Steam o el canal permiten comparar cambios de forma válida, se registrarán periodos equivalentes, fuente de tráfico y contexto. No se atribuye causalidad a una variación pequeña sin muestra suficiente.

## 17.6. CTA

Antes de Steam, el CTA puede ser “seguir el desarrollo” en un canal sostenible. Tras Coming Soon, el CTA principal es wishlist. En demo/Playtest, el CTA se adapta a jugar y enviar feedback. No se mezclan cinco acciones.

---

# 18. Localización de marketing

## 18.1. Idiomas iniciales

La dirección actual exige ES/EN para juego y store. El copy comercial se crea con source controlado y revisión competente, no como traducción automática sin contexto.

## 18.2. Principio de equivalencia

ES y EN deben transmitir la misma promesa, pero pueden adaptar orden, ritmo y vocabulario. No se fuerza simetría palabra por palabra.

## 18.3. Glosario

El glosario de localización es autoridad para:

- nombres de sistemas;
- productos;
- muebles;
- proveedores;
- estados;
- términos de Steam;
- copy de screenshots;
- subtítulos;
- FAQ;
- press kit.

## 18.4. Assets con texto

Las cápsulas base deben minimizar texto aparte del logo. Imágenes de tutorial, miniaturas o key art con copy necesitan fuentes editables, variantes por locale, alt text y QA.

## 18.5. QA de marketing localizado

- longitud;
- naturalidad;
- consistencia con build;
- truncamiento;
- signos y mayúsculas;
- claims equivalentes;
- fecha/precio;
- enlaces;
- subtítulos;
- captions;
- metadata;
- store preview.

## 18.6. Nuevos idiomas

No se añaden por una traducción voluntaria aislada. Exigen cobertura de juego, store, soporte, patch notes, QA y continuidad.

---

# 19. Accesibilidad e inclusión en comunicación

## 19.1. Material audiovisual

- subtítulos revisados;
- transcript;
- información crítica no solo por color;
- ritmo de edición comprensible;
- evitar flashes innecesarios;
- texto en pantalla con contraste y duración;
- captions descriptivos cuando corresponda;
- alt text en imágenes;
- versiones sin música no requeridas, pero mezcla clara.

## 19.2. Claims de accesibilidad

No usar “accesible” como etiqueta genérica. Se enumeran opciones reales, por ejemplo escalado de UI o controles, solo tras QA y con limitaciones.

## 19.3. Representación

Los personajes y clientes deben mostrar diversidad visual sin convertirla en token promocional. Se evitan estereotipos de edad, género, capacidad, clase o rol laboral.

## 19.4. Lenguaje

- claro y directo;
- frases cortas en material de acción;
- evitar jerga técnica en copy de jugadores;
- no ridiculizar errores;
- separar advertencia de culpa;
- explicar requisitos y limitaciones.

## 19.5. Eventos y creators

Los briefings deben incluir accesibilidad del contenido, pronunciación de nombres, subtítulos disponibles y canales para solicitar materiales alternativos.

---

# 20. Comunidad y moderación

## 20.1. Objetivo

Construir un espacio donde jugadores puedan entender el estado del proyecto, reportar problemas y compartir ideas sin crear una obligación de disponibilidad permanente.

## 20.2. Canales iniciales

Steam Community Hub será el centro preferido cuando exista. Foros adicionales solo se abren por necesidad demostrada.

## 20.3. Categorías mínimas

- anuncios;
- soporte y bugs;
- sugerencias;
- discusión general;
- guías;
- localización, si existe volumen suficiente;
- known issues fijado.

## 20.4. Código de conducta

Debe prohibir acoso, odio, doxxing, spam, suplantación, estafas, distribución de claves, contenido ilegal y publicación de datos personales. La moderación se basa en conducta, no en opinión.

## 20.5. Crítica

No se elimina crítica legítima ni se responde defensivamente. Una crítica se clasifica como:

- bug;
- expectativa creada por marketing;
- preferencia;
- problema de valor;
- problema de rendimiento;
- soporte;
- conducta.

## 20.6. Escalado

- contenido dañino → herramientas de plataforma;
- amenaza/filtración → seguridad;
- bug de save → incidente de producto;
- confusión de claim → corrección de copy;
- review bombing/anomalía → documentación y contacto con plataforma cuando proceda.

## 20.7. Roadmap público

No existe por defecto. Si se abre, contiene solo trabajo autorizado, sin fechas especulativas, con estado y fecha de revisión. El roadmap interno permanece más detallado y no se copia automáticamente.

## 20.8. Capacidad

Antes de abrir comunidad se define:

- ventanas de revisión;
- tiempo objetivo interno;
- ausencia de soporte 24/7;
- suplencia;
- plantillas;
- cierre temporal;
- archivo;
- permisos.

---

# 21. Prensa, curators y creadores

## 21.1. Objetivo

Conseguir cobertura relevante y comprensión correcta del producto, no maximizar envíos indiscriminados.

## 21.2. Segmentación

- prensa de PC/indie;
- medios de simulación/management;
- medios profesionales de desarrollo;
- creadores de shop simulators;
- creadores de tycoon/management;
- creadores de construcción/cozy;
- curators de nicho;
- podcasts o newsletters especializadas.

## 21.3. Criterios de selección

- afinidad real;
- audiencia y formato;
- historial de cobertura;
- calidad de disclosure;
- idioma;
- región;
- conducta;
- contacto verificable;
- riesgo de reventa o suplantación;
- capacidad de representar una build WIP.

## 21.4. Outreach

El mensaje debe ser personalizado, breve y factual:

- por qué encaja;
- qué es el juego;
- qué material existe;
- estado de la build;
- embargo;
- limitaciones;
- contacto;
- opt-out.

No se exige cobertura ni opinión positiva.

## 21.5. Claves

- lote y propósito;
- destinatario;
- fecha;
- tipo;
- build/branch;
- embargo;
- estado;
- revocación;
- resultado;
- no almacenar públicamente.

## 21.6. Embargos

Un embargo solo se usa si existe una razón coordinada y capacidad de gestionar filtraciones. El documento especifica zona horaria, fecha exacta, qué material está embargado y qué ya es público.

## 21.7. Creator kit

- factsheet;
- logo;
- screenshots;
- trailer;
- B-roll;
- guía de pronunciación;
- key features;
- known issues;
- controles;
- contacto;
- disclosure;
- política de monetización de vídeo si procede;
- save o escenario de captura;
- prohibiciones sobre datos/embargos.

## 21.8. Seguimiento

Se registra cobertura, enlaces, errores factuales, feedback, audiencia aproximada cuando sea pública y acciones. No se presiona para cambiar opiniones.

---

# 22. Press kit y documentación pública

## 22.1. Contenido mínimo

- título;
- desarrollador/publisher;
- ubicación;
- plataforma;
- estado;
- género;
- descripción corta/larga;
- features aprobadas;
- fecha solo si existe;
- precio solo si existe;
- idiomas;
- contactos;
- web/Steam;
- logos;
- key art;
- screenshots;
- trailer;
- créditos principales;
- FAQ;
- policy de uso de assets;
- versión y fecha del kit.

## 22.2. Factsheet

Debe caber en una página o bloque fácilmente consultable y evitar narrativas futuras. Cada cifra debe estar aprobada.

## 22.3. Biografía

La biografía de VRM Games y Blas Luis Rocha González debe equilibrar información profesional y privacidad. No publica domicilio, identificadores fiscales ni datos personales innecesarios.

## 22.4. Versionado

El press kit se versiona por build/campaña. Una URL puede apuntar a la versión vigente, pero el archivo histórico se conserva.

## 22.5. Descarga

Los assets deben ser fáciles de descargar sin crear una cuenta o activar tracking innecesario. Se incluyen formatos adecuados y licencias de uso editorial.

---

# 23. Fases de campaña

## 23.1. Fase 0 — Desarrollo privado

**Estado actual.**  
Objetivo: cerrar producto y preparar sistema de marketing.  
Permitido: investigación, arquitectura de marca, briefs, registro de assets, portfolio técnico selectivo.  
No permitido: campaña comercial, wishlist, fecha, precio, claves, demo.

## 23.2. Fase 1 — H6 y validación interna

Entrada: Sprint 17 completo y revisión H6.  
Objetivo: producir evidencia representativa y decidir si la propuesta merece apertura.  
Entregables: validation reel, screenshots internas, message test, informe de audiencia controlada.  
Salida: `NO-GO`, `ITERATE` o `OPEN PREMARKETING`.

## 23.3. Fase 2 — Premarketing controlado

Objetivo: validar posicionamiento y pipeline sin fecha pública.  
Acciones posibles: devlog ocasional, landing mínima, clips WIP representativos, lista inicial de prensa/creadores.  
Bloqueos: clearance, legal, fuente, audio, localización y soporte según material.

## 23.4. Fase 3 — Pre-Steam

Entrada: decisión Pre-Steam del documento 24.  
Objetivo: onboarding, store content pack, key art, capsules, trailer, screenshots, copy ES/EN, soporte y privacidad.  
No se anuncia lanzamiento.

## 23.5. Fase 4 — Coming Soon

Entrada: store review aprobada y materiales estables.  
Objetivo: construir wishlists y aprender qué mensaje convierte.  
Acciones: announcement trailer, devlogs, outreach selectivo, eventos solo si encajan.

## 23.6. Fase 5 — Demo o Playtest

No es obligatoria. Se decide según pregunta:

- ¿necesitamos feedback controlado? → Playtest;
- ¿necesitamos una muestra pública mantenible? → demo;
- ¿solo necesitamos QA? → branch privada;
- ¿no hay capacidad de soporte? → no abrir.

## 23.7. Fase 6 — Alpha/Beta

Objetivo: validar contenido, balance, rendimiento, compatibilidad, accesibilidad y localización. La comunicación se centra en progreso real y expectativas, no en multiplicar anuncios.

## 23.8. Fase 7 — Release Candidate

Objetivo: congelar producto y materiales.  
Acciones: review final, keys bajo control, press embargo, launch assets, runbook y contingencia.  
No se regraba el producto de forma sustancial sin invalidar materiales.

## 23.9. Fase 8 — Lanzamiento

Acciones coordinadas: release, compra/instalación smoke, anuncios, soporte, comunidad, métricas, reviews, incidentes y rollback.

## 23.10. Fase 9 — Postlanzamiento

Prioridad: estabilidad y soporte. El contenido promocional no desplaza hotfixes ni comunicación de incidentes. Las campañas se basan en mejoras reales y eventos aprobados.

---

# 24. Gates de marketing y comunicación

## 24.1. Gate M0 — Portfolio técnico

Requiere:

- material etiquetado;
- no mostrar secretos;
- no usar assets sin licencia;
- copy sin promesa comercial;
- aprobación del owner;
- enlace a contexto.

## 24.2. Gate M1 — H6 Communication Review

Requiere:

- H6 PASS o PASS WITH ACCEPTED DEBT;
- build y evidencia;
- StoreInitial representativa;
- known issues;
- reel interno;
- claim matrix inicial;
- revisión de arte, audio, UI, localización, legal y privacidad.

Salida: no-go, iteración o premarketing.

## 24.3. Gate M2 — Premarketing

Requiere:

- posicionamiento aprobado;
- audiencia y canales;
- título usable al nivel de exposición previsto;
- pipeline de captura;
- mínimo de assets representativos;
- capacidad editorial;
- métricas;
- respuesta a comentarios;
- plan de retirada.

## 24.4. Gate M3 — Pre-Steam

Requiere lo definido en el documento 24:

- producto sostenible;
- identidad/legal encaminados;
- presupuesto;
- owner;
- soporte;
- copy y materiales internos;
- decisión full release/EA/demo/playtest;
- calendario sin fecha pública.

## 24.5. Gate M4 — Coming Soon

- Steamworks activo;
- title/studio clearance;
- store page revisada;
- tráiler/capturas reales;
- cápsulas;
- ES/EN;
- claims;
- requisitos prudentes;
- privacidad;
- soporte;
- community setup;
- calendario y capacidad.

## 24.6. Gate M5 — Demo/Playtest

- objetivo de test;
- build separada;
- contenido y duración;
- onboarding;
- feedback;
- privacidad;
- soporte;
- save policy;
- known issues;
- cierre;
- métricas;
- comunicación.

## 24.7. Gate M6 — Festival

- elegibilidad verificada;
- demo adecuada;
- store page;
- calendario;
- trailer;
- livestream/broadcast solo si sostenible;
- soporte durante el evento;
- plan de seguimiento;
- no conflicto con producción.

## 24.8. Gate M7 — Launch Campaign

- RC aprobada;
- precio/regiones/descuento;
- fecha;
- store/build review;
- press kit;
- creator outreach;
- keys;
- embargo;
- FAQ;
- known issues;
- soporte;
- incident communications;
- rollback;
- backups;
- owners y horarios.

## 24.9. Gate M8 — Postlaunch Campaign

- estabilidad;
- backlog priorizado;
- reviews y soporte analizados;
- update real;
- claims actualizados;
- assets de nueva build;
- capacidad de respuesta;
- privacidad de métricas.

---

# 25. Calendario y planificación

## 25.1. Regla

Las fechas de campaña se calculan hacia atrás desde gates aprobados, no desde una fecha deseada sin build.

## 25.2. Dependencias mínimas

```text
H6
→ decisión de premarketing
→ clearance de nombre
→ paquete de marca
→ Steam onboarding
→ store content pack
→ review de página
→ Coming Soon
→ aprendizaje/wishlists
→ demo/playtest si procede
→ Alpha/Beta
→ RC
→ review de build
→ lanzamiento
```

## 25.3. Buffers

Todo calendario incluirá margen para:

- revisión legal;
- localización;
- regrabación;
- feedback de Steam;
- bugs;
- enfermedad/indisponibilidad;
- assets rechazados;
- cambios de precio/fecha;
- restauración de cuentas.

## 25.4. Editorial calendar schema

- fecha;
- campaña;
- canal;
- pieza;
- audiencia;
- mensaje;
- claim IDs;
- asset IDs;
- build;
- idioma;
- estado;
- owner;
- aprobación;
- CTA;
- URL;
- métricas T+1, T+7, T+30;
- aprendizaje.

## 25.5. Regla de silencio

Se puede pausar comunicación cuando:

- hay un incidente;
- la build dejó de representar el copy;
- una licencia está en duda;
- el título cambia;
- el calendario amenaza el gate;
- no existe información útil;
- la persona responsable no puede moderar.

---

# 26. Métricas y evaluación

## 26.1. Jerarquía

1. **Producto:** comprensión, interés, feedback, finalización de demo.
2. **Conversión:** visita → wishlist → compra.
3. **Retención:** vídeo, página, comunidad y regreso.
4. **Calidad:** sentimiento temático, bugs, confusión de claims.
5. **Eficiencia:** horas/coste por pieza y por resultado.
6. **Vanity:** impresiones o seguidores sin acción; solo contexto.

## 26.2. Métricas pre-Steam

- comprensión de género;
- recuerdo de mensaje;
- preferencia entre capturas;
- intención de seguimiento;
- preguntas repetidas;
- confusión con comparables;
- coste de producción;
- feedback por segmento.

## 26.3. Métricas Coming Soon

- visitas a store;
- wishlists;
- tasa de conversión cuando la plataforma la facilite;
- fuente de tráfico;
- trailer starts/completion;
- CTR;
- follows;
- idiomas/regiones agregados;
- cobertura;
- temas de comentarios.

## 26.4. Demo/Playtest

- accesos;
- instalaciones/inicios;
- finalización;
- tiempo de sesión si existe fuente autorizada;
- abandono por etapa;
- feedback;
- bugs;
- hardware;
- wishlist antes/después a nivel agregado cuando sea posible;
- carga de soporte.

No se añade telemetría propia sin plan de privacidad.

## 26.5. Lanzamiento

- wishlists convertidas;
- unidades/ingresos netos según reportes;
- reembolsos;
- reviews y temas;
- soporte por 100 jugadores si el denominador existe;
- crashes;
- rendimiento;
- regiones/idiomas agregados;
- conversión de campañas;
- coste.

## 26.6. Diccionario de métricas

Cada métrica documenta:

- definición;
- numerador;
- denominador;
- ventana;
- zona horaria;
- fuente;
- frecuencia;
- owner;
- acceso;
- retención;
- sesgo;
- decisión;
- privacidad.

## 26.7. Experimentos

Un experimento de marketing necesita hipótesis, variante, audiencia, periodo, criterio de éxito y límite. No se cambian simultáneamente cápsula, copy, tráiler y fuente de tráfico si se pretende aprender qué causó el resultado.

## 26.8. Revisión

- T+1: errores de publicación;
- T+7: señal inicial;
- T+30: aprendizaje;
- por hito: decisión de continuar, modificar o retirar.

---

# 27. Presupuesto, recursos y proveedores

## 27.1. Categorías de coste

- branding y logo;
- key art y cápsulas;
- edición de tráiler;
- música/SFX;
- traducción y revisión;
- QA/LQA;
- web/hosting;
- herramientas;
- Steam Direct;
- prensa/creadores;
- anuncios pagados;
- eventos;
- soporte;
- contingencia.

## 27.2. Escenarios

### Escenario Lean
Producción interna, outreach orgánico, web estática, un tráiler, assets esenciales. Riesgo: calidad/tiempo del owner.

### Escenario Selectivo
Outsourcing de key art, tráiler, localización o QA en puntos de mayor especialización. Recomendado si el presupuesto permite proteger desarrollo.

### Escenario Amplificado
PR, ads, eventos y mayor volumen. No se aprueba sin evidencia de conversión, build estable y capacidad de soporte.

## 27.3. Make or buy

Se externaliza cuando:

- la calidad requerida supera capacidad interna;
- el coste de oportunidad es alto;
- existe entregable y aceptación claros;
- se puede verificar licencia y seguridad;
- el proveedor puede iterar;
- el calendario tiene buffer.

## 27.4. Brief a proveedor

Incluye alcance, estilo, fuentes, restricciones IP, entregables editables, licencias, IA/generación, confidencialidad, revisiones, fechas, formatos, pago, créditos y transferencia de derechos aplicable.

## 27.5. Publicidad pagada

`NOT OPEN`. Solo se prueba con:

- store page que convierte orgánicamente;
- tracking permitido;
- audiencia;
- creative;
- presupuesto límite;
- criterio de parada;
- análisis de incremento, no solo clicks.

---

# 28. Legal, privacidad y seguridad

## 28.1. Bloqueos legales actuales

- clearance de `Cartridge & Cloud`;
- clearance de `VRM Games`;
- procedencia/licencia por clip de audio;
- fuente de producción;
- LICENSE/NOTICE de paquetes;
- conceptos generados;
- créditos;
- localización;
- modelos y fuentes;
- aprobación legal de una build.

## 28.2. Productos y proveedores ficticios

Los nombres y diseños deben revisarse antes de exposición pública. No se copian logos, tipografías, siluetas de consola, carátulas ni trade dress. Los IDs técnicos no se muestran como nombres finales.

## 28.3. Conceptos generados

Cada imagen o texto generado registra herramienta, fecha, prompt/contexto, input, términos aplicables, modificaciones, similitud, decisión de uso y revisión humana. Un concepto interno no se convierte en key art por conveniencia.

## 28.4. Música y audio

No publicar material con audio placeholder o procedencia incompleta. Se archivan master, licencia, autor, factura/acuerdo, restricciones y cue sheet.

## 28.5. Capturas de usuarios

Los saves, logs, screenshots o vídeos enviados por testers pueden contener datos. Se solicitan de forma opcional, se minimizan y se almacenan fuera del repositorio público.

## 28.6. Newsletter y formularios

Permanecen bloqueados hasta completar data flow, provider review, política, consentimiento, retención, derechos y seguridad.

## 28.7. Cuentas

- MFA;
- password manager;
- cuentas de rol cuando sea viable;
- recuperación;
- mínimo privilegio;
- registro de admins;
- no compartir credenciales;
- backup codes seguros;
- offboarding.

## 28.8. Claves y builds

Se distribuyen mediante canales autorizados, con ledger y expiración/revocación cuando proceda. No se adjuntan a hojas públicas ni se envían en masa sin control.

## 28.9. Incidente de comunicación

Filtración, cuenta comprometida, key leak o publicación anticipada activa el documento 27. Se conserva evidencia, se rota acceso, se evalúa alcance y se comunica solo lo necesario.

---

# 29. Riesgos de marketing y tratamiento

| ID | Riesgo | Impacto | Tratamiento |
|---|---|---|---|
| MKT-RSK-001 | Campaña antes de H6 | Alto | gate y límite de capacidad |
| MKT-RSK-002 | Título no cleared | Crítico | no campaña final/Steam |
| MKT-RSK-003 | Concepto confundido con gameplay | Alto | etiquetas y procedencia |
| MKT-RSK-004 | Visión futura presentada como alcance | Alto | claim matrix y revisión |
| MKT-RSK-005 | Audio/fuente sin licencia | Crítico | bloqueo legal |
| MKT-RSK-006 | Canales excesivos | Alto | priorización y cierre |
| MKT-RSK-007 | Cadencia que desplaza desarrollo | Alto | cap 10/15 % |
| MKT-RSK-008 | Mensaje genérico de tycoon | Medio | diferenciadores y tests |
| MKT-RSK-009 | Comparación reductora | Medio | anti-posicionamiento |
| MKT-RSK-010 | Tráiler no representativo | Alto | build/shot provenance |
| MKT-RSK-011 | Demo sin soporte | Alto | gate M5 |
| MKT-RSK-012 | Fecha prematura | Crítico | no publicar sin RC buffers |
| MKT-RSK-013 | Comunidad sin moderación | Alto | owner y runbook |
| MKT-RSK-014 | Tracking sin privacidad | Crítico | privacy gate |
| MKT-RSK-015 | Claves filtradas | Alto | ledger y mínimo privilegio |
| MKT-RSK-016 | Reviews negativas por expectativa | Alto | copy honesto y FAQ |
| MKT-RSK-017 | Baja wishlist conversion | Medio | tests de cápsula/copy |
| MKT-RSK-018 | Falta de contenido visual | Alto | pipeline y batch capture |
| MKT-RSK-019 | Dependencia de algoritmo | Medio | Steam/web/source mix |
| MKT-RSK-020 | Burnout del desarrollador | Crítico | capacidad, batch y silencio |
| MKT-RSK-021 | Cambio de título tardío | Alto | clearance temprano |
| MKT-RSK-022 | Material ES/EN inconsistente | Medio | glosario y LQA |
| MKT-RSK-023 | Proveedor incumple | Medio/Alto | contratos, editable y buffer |
| MKT-RSK-024 | Crisis/bug en lanzamiento | Crítico | incident comms y rollback |
| MKT-RSK-025 | Métricas vanidosas | Medio | diccionario y decisiones |

Los riesgos globales se transferirán al documento 33 cuando exista.

---

# 30. Playbooks operativos

## 30.1. Publicar un devlog

1. Definir aprendizaje o hito.
2. Confirmar que no desplaza un gate.
3. Seleccionar build/material.
4. Redactar sin prometer.
5. Añadir estado WIP.
6. Revisar seguridad/legal.
7. Subtitular y localizar.
8. Publicar.
9. Responder en ventana definida.
10. Archivar métricas y aprendizaje.

## 30.2. Anunciar un retraso

1. Confirmar decisión.
2. Explicar qué cambia y qué no.
3. Usar fecha exacta solo si existe nueva aprobación.
4. Evitar culpar a una persona o proveedor.
5. Indicar próxima actualización por condición realista.
6. Actualizar Steam, web, FAQ y calendarios.
7. Registrar copy y aprobaciones.

## 30.3. Corregir un claim erróneo

1. Identificar alcance.
2. Retirar/editar material.
3. Publicar aclaración proporcional.
4. Actualizar claim matrix.
5. Revisar assets derivados.
6. Analizar causa.
7. Añadir control preventivo.

## 30.4. Responder a una review negativa

- no responder en caliente;
- identificar si existe bug o dato factual;
- responder solo si ayuda;
- agradecer información;
- indicar versión/solución;
- no pedir cambio de puntuación;
- no discutir preferencia;
- trasladar soporte si se necesitan datos.

## 30.5. Comunicar un incidente

1. Reconocer problema.
2. Describir impacto conocido.
3. Ofrecer workaround seguro.
4. Indicar investigación.
5. Fijar siguiente update por ventana o condición.
6. Publicar resolución con BuildID/versión.
7. Postmortem interno y, si procede, público.

## 30.6. Enviar una build a creador

1. Validar gate.
2. Verificar identidad/contacto.
3. Preparar branch/build.
4. Known issues y briefing.
5. Key ledger.
6. Embargo/disclosure.
7. Canal de soporte.
8. Revocar/cerrar cuando proceda.
9. Registrar cobertura y feedback.

## 30.7. Retirar un canal

1. Exportar datos.
2. Avisar si existe comunidad activa.
3. Redirigir a fuente oficial.
4. Revocar accesos.
5. Archivar assets.
6. Actualizar enlaces.
7. Cerrar tratamiento de datos conforme a política.

---

# 31. Work packages y entregables

| ID | Entregable | Gate | Estado |
|---|---|---|---|
| MKT-WP-001 | Market & Comparable Research | M1/M2 | PENDING |
| MKT-WP-002 | Naming and Clearance Dossier | M2/M3 | BLOCKED |
| MKT-WP-003 | Brand Strategy & Logo Package | M3/M4 | PENDING |
| MKT-WP-004 | Message House & Claim Matrix | M1 | PENDING |
| MKT-WP-005 | Audience Validation Report | M1/M2 | PENDING |
| MKT-WP-006 | Capture Pipeline & Provenance Log | M1 | PENDING |
| MKT-WP-007 | H6 Internal Validation Reel | M1 | PENDING |
| MKT-WP-008 | Screenshot Master Set | M3/M4 | PENDING |
| MKT-WP-009 | Announcement Trailer | M4 | BLOCKED |
| MKT-WP-010 | Steam Store Content Pack ES/EN | M3/M4 | BLOCKED |
| MKT-WP-011 | Steam Capsules & Library Assets | M4 | BLOCKED |
| MKT-WP-012 | Website/Landing MVP | M2/M4 | NOT OPEN |
| MKT-WP-013 | Press Kit ES/EN | M4/M7 | BLOCKED |
| MKT-WP-014 | Creator/Press CRM | M4/M7 | PENDING |
| MKT-WP-015 | Creator Kit & B-roll | M7 | BLOCKED |
| MKT-WP-016 | Editorial Calendar | M2 | PENDING |
| MKT-WP-017 | Community & Moderation Runbook | M4 | PENDING |
| MKT-WP-018 | Support/FAQ Public Pack | M4/M7 | BLOCKED |
| MKT-WP-019 | Demo/Playtest Campaign Pack | M5 | NOT OPEN |
| MKT-WP-020 | Festival Readiness Pack | M6 | NOT OPEN |
| MKT-WP-021 | Launch Campaign Runbook | M7 | NOT OPEN |
| MKT-WP-022 | Metrics Dictionary & Dashboard | M2/M4 | PENDING |
| MKT-WP-023 | Budget and Vendor Plan | M3 | PENDING |
| MKT-WP-024 | Marketing Legal Approval Checklist | M3/M4 | PENDING |
| MKT-WP-025 | Postlaunch Communication Library | M7/M8 | PENDING |

## 31.1. Definition of Ready de una pieza

- objetivo;
- audiencia;
- canal;
- build;
- claims;
- assets;
- licencia;
- idiomas;
- owner;
- reviewers;
- CTA;
- fecha;
- métrica;
- riesgo;
- rollback.

## 31.2. Definition of Done de una pieza

- export aprobado;
- copy aprobado;
- legal/seguridad/privacidad revisados;
- accesibilidad;
- URL y fecha;
- source archivado;
- hash;
- canales actualizados;
- métricas planificadas;
- revisión postpublicación;
- estado en trazabilidad.

---

# 32. Trazabilidad y control de cambios

## 32.1. IDs

- `MKT-OBJ`: objetivo.
- `MKT-AUD`: audiencia.
- `MKT-MSG`: mensaje.
- `MKT-CLM`: claim.
- `MKT-AST`: asset.
- `MKT-CNT`: contenido.
- `MKT-CMP`: campaña.
- `MKT-CHN`: canal.
- `MKT-MET`: métrica.
- `MKT-RSK`: riesgo.
- `MKT-WP`: work package.
- `MKT-DEC`: decisión.

## 32.2. Relaciones mínimas

```text
Feature/Requirement
→ Claim
→ Copy
→ Asset
→ Campaign
→ Channel
→ Metric
→ Decision
```

## 32.3. Cambio material

Se considera material:

- título/logo;
- posicionamiento;
- plataforma;
- precio;
- fecha;
- feature list;
- idioma;
- accesibilidad;
- demo/EA;
- roadmap;
- key art;
- tráiler;
- política de datos;
- canal de soporte.

Requiere evaluación de todos los materiales derivados.

## 32.4. Auditoría

La auditoría final 00–34 verificará que este documento no contradiga el Steam Publishing Plan, Post-Launch, Privacy, Security, Accessibility, Performance, legal y manifest.

---

# 33. Plantillas de copy

## 33.1. Post WIP

```text
[WORK IN PROGRESS — build <ID>]

Esta semana hemos trabajado en <sistema>. El cambio permite <acción del jugador> y busca <consecuencia>. La captura muestra <hecho real>. <Limitación o siguiente validación>.

Estado: <Sprint/Hito>. Este material no representa todavía una demo pública ni una fecha de lanzamiento.
```

## 33.2. Anuncio de hito interno

```text
Hemos cerrado <hito> con una build identificada y pruebas de <áreas>. El resultado valida <hipótesis>, pero no abre automáticamente una demo o campaña pública. El siguiente paso es <decisión/gate>.
```

## 33.3. Known issue

```text
Problema conocido — versión <x> / BuildID <y>

Impacto: <qué ocurre>.
Afecta a: <condiciones>.
Workaround seguro: <si existe>.
Estado: <investigando/validando/corregido>.
Próxima actualización: <ventana o condición>.
```

## 33.4. Patch note

```text
Versión <x> — <fecha>

Destacado
- <cambio y beneficio>

Correcciones
- <problema resuelto, sin atribuir culpa al usuario>

Compatibilidad y saves
- <impacto>

Known issues
- <limitaciones>

Soporte
- <canal>
```

## 33.5. Outreach inicial

```text
Hola, <nombre>:

Te contacto porque sueles cubrir <motivo específico>. Cartridge & Cloud es un simulador de gestión 3D sobre operar una tienda de videojuegos: pedidos, stock, distribución y clientes visibles.

Disponemos de <material/build> en estado <estado>, con <embargo si aplica>. No esperamos una opinión positiva ni existe obligación de cobertura. Puedo facilitar factsheet, capturas, B-roll y acceso bajo las condiciones indicadas.

<Contacto y opt-out>.
```

---

# 34. Anexos operativos

## Anexo A. Checklist de revisión de contenido

- [ ] Build/commit registrado.
- [ ] Material correctamente etiquetado.
- [ ] Claim IDs aprobados.
- [ ] Sin features `NOT OPEN`.
- [ ] Sin debug, secretos o datos personales.
- [ ] IP y licencias verificadas.
- [ ] Título/nombres autorizados al nivel de exposición.
- [ ] Copy ES/EN revisado.
- [ ] Subtítulos/alt text.
- [ ] Contraste y safe areas.
- [ ] CTA válido.
- [ ] Enlaces probados.
- [ ] Owner y moderación disponibles.
- [ ] Source y export archivados.
- [ ] Métricas y fecha de revisión.

## Anexo B. Checklist de screenshot

- [ ] Gameplay real.
- [ ] Resolución y relación correctas.
- [ ] Sin UI de desarrollo.
- [ ] Acción legible.
- [ ] Variedad respecto al set.
- [ ] Producto/proveedor cleared.
- [ ] Fuente final/licenciada si aparece texto.
- [ ] No muestra sistemas futuros.
- [ ] Build ID.
- [ ] Procedencia.
- [ ] Retoque documentado.
- [ ] Alt text.
- [ ] Export sin artefactos.

## Anexo C. Checklist de tráiler

- [ ] Objetivo y audiencia.
- [ ] Hook de gameplay.
- [ ] Bucle principal.
- [ ] Claims demostrados.
- [ ] Audio licenciado.
- [ ] Subtítulos/transcript.
- [ ] Idiomas.
- [ ] Safe areas.
- [ ] Plataformas correctas.
- [ ] Fecha/precio aprobados o ausentes.
- [ ] CTA válido.
- [ ] No mockups engañosos.
- [ ] Build/shot list.
- [ ] Masters y proyectos archivados.

## Anexo D. Campos del CRM de prensa y creadores

- ID;
- nombre/canal;
- contacto;
- idioma/región;
- categoría;
- audiencia;
- afinidad;
- fuente;
- fecha de verificación;
- estado;
- último contacto;
- material;
- key;
- embargo;
- disclosure;
- respuesta;
- cobertura;
- feedback;
- opt-out;
- notas privadas mínimas;
- retención.

## Anexo E. Diccionario inicial de KPIs

| KPI | Definición | Decisión |
|---|---|---|
| Message comprehension | % que describe correctamente el juego | revisar posicionamiento |
| Qualified view | visualización con interacción relevante | evaluar canal |
| Store visit | visita a Steam | fuente/conversión |
| Wishlist | adición agregada | interés previo |
| Wishlist conversion | compras/wishlists según reporte y ventana | lanzamiento/precio |
| Trailer completion | finalizaciones/inicios | edición/mensaje |
| Demo completion | finales/inicios | onboarding/contenido |
| Support rate | tickets/jugadores | calidad/carga |
| Refund rate | reembolsos/ventas | expectativa/calidad |
| Review themes | categorías de reviews | producto/comunicación |
| Content cost | horas + gasto por pieza | eficiencia |

## Anexo F. Taxonomía de campañas

- `CMP-H6`: validación interna.
- `CMP-PRE`: premarketing.
- `CMP-CS`: Coming Soon.
- `CMP-DEMO`: demo.
- `CMP-PT`: Playtest.
- `CMP-FEST`: festival.
- `CMP-ALPHA`: alpha.
- `CMP-BETA`: beta.
- `CMP-LAUNCH`: lanzamiento.
- `CMP-UPDATE`: actualización.
- `CMP-SALE`: descuento/evento.
- `CMP-INC`: incidencia.

## Anexo G. Preguntas frecuentes internas

**¿Podemos publicar conceptos bonitos antes de H6?**  
Solo como concept art claramente etiquetado, con derechos verificados y sin convertirlos en promesa. No sustituyen gameplay.

**¿H6 significa que abrimos Steam?**  
No. H6 valida el slice interno. Pre-Steam es un gate separado.

**¿Podemos hablar de publishing o plataforma?**  
Solo como visión histórica/contextual y con etiqueta `VISION / NOT OPEN`; no en features de compra.

**¿Necesitamos Discord?**  
No. Se abre solo si la comunidad y la capacidad lo justifican.

**¿Necesitamos demo?**  
No. Se elige entre branch privada, Playtest, demo o ninguna según la pregunta y capacidad.

**¿Podemos usar los audios actuales en un tráiler?**  
No como material comercial mientras sean placeholders y no esté cerrada su licencia/calidad.

**¿Podemos anunciar fecha aproximada?**  
No mientras no exista calendario aprobado, RC y buffers.

## Anexo H. Fuentes documentales consolidadas

Este plan se ha construido a partir de:

- cuatro fuentes históricas de Marketing Plan, baselines v0.3–v0.6;
- Steam Publishing Plans históricos;
- `00_Enfoque_y_Alcance.md`;
- `01_Game_Design_Document.md`;
- `02_Vertical_Slice_Specification.md`;
- `06_Production_Roadmap_y_Sprint_Plan.md`;
- `07_QA_Testing_Plan.md`;
- `11_Build_y_Versioning_Guide.md`;
- `12_Excel_Maestro_de_Produccion.xlsx`;
- `13_Trazabilidad_y_Control_de_Cambios.xlsx`;
- `14_Project_Binder_Indice_Maestro.md`;
- `15_Guia_Maestra.md`;
- `16_Auditoria_Global_de_Coherencia.md`;
- `17_Art_Bible.md`;
- `18_Audio_Bible.md`;
- `19_UI_Style_Guide.md`;
- `20_Economy_and_Balance_Specification.md`;
- `21_Initial_Content_Catalog.xlsx`;
- `22_Localization_Plan.md`;
- `23_Legal_Credits_and_Licenses_Register.xlsx`;
- `24_Steam_Publishing_Plan.md`;
- `25_Post_Launch_and_Live_Operations_Plan.md`;
- `26_Privacy_Data_and_Telemetry_Plan.md`;
- `27_Security_and_Incident_Response_Plan.md`;
- proyecto Unity/GitHub previo a la regeneración documental.

## Anexo I. Historial de cambios

| Versión | Fecha | Cambio | Owner |
|---|---|---|---|
| 1.0 | 2026-07-01 | Consolidación completa de Marketing Plans v0.3, v0.4 y v0.5; integración con documentos 00–27; definición de gates, canales, assets, métricas, riesgos y work packages | VRM Games / Blas Luis Rocha González |

---

# 35. Criterio de cierre del documento 28

El documento 28 se considera generado cuando:

1. preserva todas las fuentes históricas localizadas;
2. no trata la brevedad de v0.4/v0.5 como eliminación de v0.3;
3. refleja el estado real del proyecto;
4. separa H6, premarketing, Steam, demo y lanzamiento;
5. define posicionamiento, audiencias, mensajes y diferenciadores;
6. define canales, contenido, assets, prensa, creadores y comunidad;
7. integra localización, accesibilidad, legal, privacidad y seguridad;
8. define métricas, presupuesto, riesgos, playbooks y work packages;
9. evita prometer sistemas `NOT OPEN`;
10. puede enlazarse desde Binder, Guía, Producción, Trazabilidad y auditoría final.

**Estado de este documento:** `COMPLETE AS CONSOLIDATED PLAN / EXECUTION PENDING`.

**Siguiente documento de la secuencia:** `29_Accessibility_and_Inclusive_Design_Plan.md`.
