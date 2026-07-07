---
title: "Cartridge & Cloud — Accessibility and Inclusive Design Plan"
subtitle: "Plan maestro consolidado de accesibilidad visual, auditiva, motora y cognitiva, diseño inclusivo, validación, Steam y operación de lanzamiento"
author: "VRM Games / Blas Luis Rocha González"
date: "2026-07-07"
lang: "es-ES"
document_number: "29"
document_version: "1.0"
project_version_reference: "0.0.21"
platform: "PC / Steam"
engine: "Unity 6.3 LTS 6000.3.18f1"
render_pipeline: "URP 17.3.0"
status: "CONSOLIDATED PLAN / PARTIALLY IMPLEMENTED / RELEASE VALIDATION PENDING"
---

# 29 — Accessibility and Inclusive Design Plan

**Proyecto:** Cartridge & Cloud  
**Desarrollador:** VRM Games / Blas Luis Rocha González  
**Plataforma inicial:** PC / Steam  
**Estado técnico de referencia:** Sprints 0–15 `CLOSED / PASS`; Sprint 16 `COMPLETED / PASS`; Sprint 17 `PENDING / READY TO OPEN`; H6 `BLOCKED / NOT RUN`  
**Estado de accesibilidad actual:** existe una base funcional de preferencias globales, escala de UI/texto, reducción de movimiento, tutorial, confirmaciones destructivas y navegación por ratón/teclado; la cobertura integral y la validación de lanzamiento permanecen abiertas  
**Clasificación del documento:** interno y normativo para diseño, implementación, QA, localización, arte, audio, publicación y soporte  
**Naturaleza:** plan de producto y proceso; no constituye certificación legal, declaración pública de conformidad ni autorización para marcar características en Steam

> **Regla principal:** ninguna capacidad de accesibilidad se considera disponible por existir una intención, un campo de datos, un paquete de Unity, una opción visible o una prueba aislada. Debe funcionar de extremo a extremo en la build candidata, persistir correctamente, ser comprensible, no bloquear el Golden Path y estar validada con evidencia reproducible.

> **Principio inclusivo:** la accesibilidad no se tratará como un menú añadido al final. Debe influir en las reglas, el ritmo, el input, la cámara, la presentación visual, el audio, la localización, el tutorial, los errores, el guardado, la publicación y el soporte desde el diseño de cada sistema.

> **Gate vigente:** Sprint 15 demuestra una fundación técnica, no una declaración comercial completa. Las etiquetas de accesibilidad de Steam solo podrán activarse después de verificar cada característica en la Release Candidate según la definición oficial vigente y conservar la evidencia asociada.

---

# 0. Propósito, autoridad y forma de uso

## 0.1. Propósito

Este documento consolida la política de accesibilidad y diseño inclusivo de *Cartridge & Cloud* desde el estado actual del vertical slice hasta la Release Candidate, el lanzamiento y el soporte postlanzamiento. Sus objetivos son:

- preservar todas las decisiones históricas localizadas, incluidas las versiones extensas anteriores a las baselines resumidas;
- transformar principios dispersos en requisitos verificables de producto;
- distinguir capacidad implementada, capacidad parcialmente conectada, capacidad planificada y aspiración futura;
- evitar que dificultad, accesibilidad y preferencias se mezclen de forma que penalicen o estigmaticen al jugador;
- establecer contratos para UI, texto, color, cámara, audio, input, ritmo, guardado, tutorial y feedback;
- definir cómo se prueban las rutas con ratón, teclado, mando y configuraciones alternativas;
- integrar accesibilidad con localización ES/EN, arte, audio, QA, rendimiento, Steam, marketing, privacidad y soporte;
- convertir los requisitos en work packages, criterios de aceptación, gates y evidencia de lanzamiento;
- prevenir declaraciones públicas imprecisas sobre características no demostradas;
- permitir que colaboradores futuros retomen el trabajo sin depender del historial del chat.

El plan cubre tanto la experiencia dentro del juego como los materiales y procesos que la rodean: store page, tráileres, capturas, documentación, formularios, soporte y comunicación de incidencias.

## 0.2. Autoridad documental

La jerarquía aplicable es:

1. `00_Enfoque_y_Alcance.md`, para visión, pilares, estados de alcance y separación entre dificultad y accesibilidad.
2. `01_Game_Design_Document.md`, para reglas, ritmo, progresión, dificultad, pausa y guardado.
3. `02_Vertical_Slice_Specification.md`, para el Golden Path y la evidencia exigida en H6.
4. `03_Technical_Design_Document.md` y `04_Modelo_de_Datos.md`, para arquitectura, persistencia e invariantes.
5. `05_UX_Flow.md`, para navegación, input, mensajes, opciones y objetivos de accesibilidad.
6. `06_Production_Roadmap_y_Sprint_Plan.md`, para fases, capacidad, Sprint 17 y gates.
7. `07_QA_Testing_Plan.md` y `08_QA_Testing_Matrix.xlsx`, para estrategia de prueba y registro de resultados.
8. `09_CSharp_Coding_Standards.md`, `10_Unity_Project_Setup_Guide.md` y `11_Build_y_Versioning_Guide.md`, para implementación y evidencia reproducible.
9. `17_Art_Bible.md`, `18_Audio_Bible.md` y `19_UI_Style_Guide.md`, para lectura visual, señalización, sonido y componentes de interfaz.
10. `20_Economy_and_Balance_Specification.md`, para dificultad, presión económica y ritmo.
11. `22_Localization_Plan.md`, para ES/EN, fuentes, expansión, terminología, subtítulos y LQA.
12. `24_Steam_Publishing_Plan.md`, para declaraciones comerciales, input, Steam Deck y publicación.
13. `25_Post_Launch_and_Live_Operations_Plan.md`, para soporte y corrección tras lanzamiento.
14. `26_Privacy_Data_and_Telemetry_Plan.md`, para playtests, métricas y tratamiento de datos.
15. `27_Security_and_Incident_Response_Plan.md`, para builds externas, cuentas y respuesta a incidentes.
16. `28_Marketing_and_Communication_Plan.md`, para representación accesible y claims públicos.
17. Este documento, como autoridad especializada en accesibilidad y diseño inclusivo siempre que no contradiga una fuente superior.

Ante una contradicción:

- no se elige silenciosamente la opción más cómoda;
- se identifica el requisito afectado;
- se registra la discrepancia en trazabilidad;
- se decide qué documento cambia;
- se actualizan tests, copy, Steam y documentación si la decisión altera una capacidad declarada.

## 0.3. Estados utilizados

| Estado | Significado |
|---|---|
| `CURRENT / VERIFIED` | Implementado y demostrado en una build o suite vigente con evidencia suficiente. |
| `CURRENT / LIMITED` | Existe, pero su alcance es parcial, técnico o no cubre el Golden Path completo. |
| `PLANNED` | Requisito aprobado, todavía no implementado o no validado. |
| `PROPOSED` | Recomendación de este plan pendiente de decisión formal. |
| `REQUIRED FOR H6` | Necesario para aprobar el vertical slice interno. |
| `REQUIRED FOR RELEASE` | Necesario antes de declarar la primera versión publicable. |
| `OPTIONAL ENHANCEMENT` | Mejora valiosa que no bloquea la primera versión si la cobertura base está cerrada. |
| `NOT APPLICABLE` | No corresponde al producto o a la versión actual; debe justificarse. |
| `NOT OPEN` | Visión futura no autorizada para producción o comunicación. |
| `HISTORICAL` | Fuente preservada, no autoridad operativa por sí sola. |
| `SUPERSEDED` | Decisión reemplazada por otra posterior y trazada. |
| `BLOCKED` | No puede declararse ni publicarse hasta cerrar dependencias. |

## 0.4. Modelo de requisito

Cada requisito de accesibilidad deberá incluir como mínimo:

- ID estable;
- barrera que reduce;
- población o contexto beneficiado;
- estado de alcance;
- comportamiento observable;
- plataformas y dispositivos cubiertos;
- valores por defecto y rangos;
- persistencia y migración;
- interacción con pausa, guardado, localización y rendimiento;
- casos de prueba positivos y negativos;
- evidencia de build;
- owner;
- estado de claim público;
- dependencia de contenido, arte, audio o copy.

## 0.5. Principio de evidencia

Para afirmar que una capacidad existe deben poder responderse estas preguntas:

1. ¿Dónde se activa y puede alcanzarse desde el primer arranque?
2. ¿Qué parte exacta del juego modifica?
3. ¿Funciona en MainMenu, StoreInitial, modales, tutorial, resultados y errores?
4. ¿Persiste tras reiniciar y se recupera ante archivo inválido?
5. ¿Funciona en ES y EN sin truncamiento crítico?
6. ¿Funciona a `1280×720`, `1920×1080` y `2560×1440`, y en los modos de ventana autorizados?
7. ¿Sigue siendo operable con el dispositivo o modalidad declarada?
8. ¿Existe prueba manual o automatizada que lo demuestre?
9. ¿Ha sido revisada por una persona distinta del implementador o mediante playtest relevante cuando sea posible?
10. ¿La store page describe exactamente esa cobertura, sin extrapolar?

## 0.6. No certificación automática

Este documento usa WCAG 2.2, Xbox Accessibility Guidelines y la documentación de Steamworks como referencias de diseño y prueba. Eso no convierte al juego en un producto certificado ni implica cumplimiento jurídico automático. Cualquier declaración legal o contractual necesitará revisión específica de jurisdicción, plataforma, versión y material publicado.

## 0.7. Regla de mantenimiento

Se revisará como mínimo:

- al cerrar Sprint 16;
- durante Sprint 17;
- en H6;
- al estabilizar la UI final;
- al integrar audio final o voz;
- al implementar remapeo o soporte de mando;
- antes de publicar Coming Soon;
- antes de una demo, Playtest o festival;
- antes de la Release Candidate;
- tras cambios materiales de cámara, input, tutorial, dificultad, guardado o localización;
- después del lanzamiento y ante defectos de accesibilidad relevantes.

---

# 1. Genealogía histórica completa

## 1.1. Origen distribuido

No se localizó un antiguo `Accessibility Plan` autónomo. La accesibilidad apareció de forma transversal en:

- Enfoque y GDD;
- UX Flow;
- UI Style Guide;
- Art Bible;
- Audio Bible;
- Localization Plan;
- QA Testing Plan;
- Steam Publishing Plan;
- ADR de input, UI y preferencias globales;
- sprints de implementación y validación.

Por tanto, la ausencia de un título histórico específico no significa ausencia de intención. Este documento conserva la genealogía completa y evita que las versiones breves de v0.5 y v0.6 borren el detalle presente en v0.3.

## 1.2. Baseline v0.3: aportaciones fundacionales

La baseline v0.3 contiene la especificación conceptual más extensa. Sus aportaciones conservadas incluyen:

- UI técnica, limpia y legible sobre fondos oscuros;
- paleta con texto principal/secundario, advertencia y error diferenciados;
- personajes y objetos legibles desde cámara elevada;
- siluetas diferenciadas y densidad visual controlada;
- asignaciones de control modificables cuando existiera el sistema correspondiente;
- navegación y feedback que no dependieran solo del color;
- claridad de estados, confirmaciones, errores y acciones destructivas;
- localización ES/EN con expansión, fallback y fuentes adecuadas;
- revisión de tamaños, contraste, iconografía y lectura a varias resoluciones;
- separación de dificultad y opciones de accesibilidad;
- posibilidad de pausar o reducir presión temporal;
- mensajes comprensibles y ayuda contextual;
- necesidad de subtítulos o equivalentes si se incorporaba voz;
- pruebas de UI, localización, input, guardado y build.

v0.3 también estableció una tensión que este plan resuelve: el juego usa color como identidad visual y feedback, pero el estado funcional nunca debe depender exclusivamente de ese color.

## 1.3. Baseline v0.4: actualización de contexto

La reedición v0.4 mantuvo la mayor parte de la intención de v0.3 y actualizó el entorno técnico. Las decisiones de accesibilidad siguieron siendo objetivo de diseño, no capacidad demostrada. La lección conservada es que actualizar motor, URP, paquetes o documentación no equivale a cerrar una experiencia accesible.

## 1.4. Baseline v0.5: reducción documental y fundación jugable

Las versiones v0.4 de UX/UI/arte/audio/localización y v0.5 de QA son más breves porque el paquete pasó a priorizar implementación y registros de sprint. No sustituyen el detalle anterior. Aportan:

- contexto de una tienda `10 × 15 m`;
- control por clic, cámara orbital y zoom;
- Input System y contextos de escena;
- reglas de construcción y navegación ya implementadas;
- obligación de no prometer soporte no validado;
- QA basada en gates de sprint y builds externas.

## 1.5. Baseline v0.6: estado real antes de la regeneración

La baseline v0.6 añade la verdad de implementación:

- Sprints 0–15 cerrados;
- Sprint 15 entrega UI/UX integrada, tutorial y accesibilidad;
- versión de aplicación `0.0.16` al cerrar Sprint 15 y `0.0.17` como referencia posterior;
- navegación por ratón y teclado;
- jerarquía de Escape y exclusividad UI/gameplay;
- preferencias globales persistentes;
- Sprint 16 orientado a representación visual y audio de Phase 1;
- Sprint 17 reservado para balance, rendimiento, QA, correcciones, revisión de accesibilidad y cierre del vertical slice.

## 1.6. ADR y registros de desarrollo preservados

Los siguientes registros son especialmente relevantes:

- `ADR-0012_Scene_Driven_Input_Contexts.md`: contextos de input dependientes de escena.
- `ADR-0015_Project_Input_Actions_And_Context_Routing.md`: acciones de input y routing del proyecto.
- `ADR-0065-GlobalAccessibilityPreferences.md`: escala de UI, escala de texto, reducción de movimiento, tutorial y confirmaciones como preferencias globales persistentes.
- `Sprint_15_Charter.md`: entrega de accesibilidad y navegación integral.
- `S15_Validation_Checklist.md`: `Accessibility PASS`, navegación teclado PASS e input exclusivity PASS.
- `Sprints_00_15_Consolidated_Closure_Ledger.md`: Sprint 15 cerrado con tutorial y accesibilidad.
- `Sprint_17_Opening_Brief.md`: revisión de accesibilidad dentro del cierre del vertical slice.

## 1.7. Regla de consolidación histórica

Cuando una versión posterior es más corta se interpreta como actualización de estado, no como eliminación de requisitos, salvo que exista una decisión explícita de sustitución. En consecuencia:

- el detalle de v0.3 se conserva como intención de diseño;
- v0.5–v0.6 determinan qué se implementó realmente;
- la documentación 00–28 fija el estado vigente;
- el código y las pruebas determinan la realidad ejecutable;
- este documento transforma la suma en un plan verificable.

## 1.8. Decisiones históricas sustituidas o acotadas

| Decisión histórica | Tratamiento vigente |
|---|---|
| Teclas concretas mostradas como fijas | Sustituidas por prompts que consulten bindings; mientras no exista remapeo, se etiquetan como defaults, no como permanentes. |
| Color verde = válido y rojo = inválido | Se conserva como capa de refuerzo, nunca como canal único. Debe combinarse con icono, forma, texto, patrón o estado. |
| Soporte de mando inferido por Input System | No se declara hasta completar el Golden Path con mando y aprobar navegación/foco/prompts. |
| “Accessibility PASS” de Sprint 15 como cierre total | Se acota a las capacidades incluidas en Sprint 15; no cubre toda la matriz de lanzamiento. |
| Reducir movimiento como simple booleano | Se conserva la preferencia, pero debe conectarse a cada efecto relevante y probarse. |
| Subtítulos previstos | Permanecen `NOT APPLICABLE` mientras no haya voz o audio semántico no visualizado; pasan a `REQUIRED` si se introduce. |
| Pantallas generadas en runtime como UI final | Se consideran evidencia funcional y deuda de presentación; deben migrarse o validarse antes de lanzamiento. |

---

# 2. Resumen ejecutivo y postura actual

## 2.1. Objetivo de experiencia

*Cartridge & Cloud* debe permitir que el jugador comprenda, opere y haga progresar una tienda sin depender exclusivamente de:

- distinguir colores concretos;
- escuchar avisos;
- leer texto pequeño;
- recordar combinaciones no visibles;
- realizar movimientos precisos o rápidos;
- soportar movimiento de cámara intenso;
- mantener atención continua sin pausa;
- interpretar iconos sin etiqueta;
- reaccionar dentro de ventanas temporales breves;
- conocer previamente convenciones de simuladores de gestión.

## 2.2. Fotografía de implementación

| Área | Estado | Evidencia / límite |
|---|---|---|
| Escala de UI | `CURRENT / LIMITED` | Preferencia global `80–150 %`, pasos de 10, aplicada mediante `CanvasScaler`; falta cobertura final de todas las pantallas y resoluciones. |
| Escala de texto | `CURRENT / LIMITED` | Preferencia `80–150 %`; aplicada al texto creado por las fábricas actuales; falta validación con UI final, localización y fuentes finales. |
| Reducción de movimiento | `CURRENT / LIMITED` | Preferencia persistente visible; las referencias de código muestran principalmente el dato y UI, no una cobertura demostrada de todos los movimientos/efectos. |
| Tutorial configurable | `CURRENT / VERIFIED` para Sprint 15 | Puede activarse/desactivarse y reiniciarse; progreso por slot. Debe revalidarse en la experiencia final. |
| Confirmaciones destructivas | `CURRENT / VERIFIED` para Sprint 15 | Preferencia global; usada en borrado/skip y operaciones relevantes observadas. Debe cubrir todas las acciones destructivas futuras. |
| Duración de mensajes | `CURRENT / HIDDEN FOUNDATION` | El modelo admite `1–30 s`, default 6; no se ha demostrado un control completo expuesto al jugador ni uso uniforme. |
| Navegación por ratón | `CURRENT / VERIFIED` para Sprint 15 | MainMenu y HUD funcionales; requiere regresión con UI final. |
| Navegación por teclado | `CURRENT / VERIFIED` para Sprint 15 | Checklist PASS; requiere cobertura completa de foco, modales y nueva UI. |
| Mando | `PLANNED / BLOCKED FOR CLAIM` | Input System y bindings genéricos existen, pero no hay evidencia suficiente de Golden Path completo, prompts, foco y remapeo con mando. |
| Remapeo | `PLANNED` | Requisito UX vigente; no se considera implementado. |
| Mouse-only | `PROPOSED / REQUIRES TEST` | El movimiento por clic favorece esta modalidad; toda acción, cámara y UI deben demostrarse sin teclado antes de declararla. |
| Keyboard-only | `PROPOSED / REQUIRES IMPLEMENTATION` | Steam exige que también pueda controlarse la cámara; el diseño actual sigue muy orientado a puntero. |
| Volúmenes por canal | `CURRENT / LIMITED` | `Phase1AudioRouter` persiste volumen por canal; falta validar UI final, taxonomía y cobertura de todos los sonidos. |
| Señales visuales para audio | `PLANNED` | Requisito de UX/QA; falta matriz completa y pruebas. |
| Subtítulos/captions | `NOT APPLICABLE NOW / REQUIRED IF VOICE` | No se ha demostrado contenido hablado final; el sistema debe diseñarse antes de introducir voz. |
| Contraste configurable | `PLANNED` | Existe paleta y reglas de legibilidad, no controles de contraste demostrados. |
| Alternativas al color | `PLANNED / PARTIAL BY DESIGN` | Requisito documentado; necesita auditoría sistemática de mundo y UI. |
| Narración/lector de pantalla | `OPTIONAL ENHANCEMENT / NOT IMPLEMENTED` | El módulo Unity presente no demuestra integración; no puede declararse. |
| Dificultad ajustable | `PLANNED` | Presets económicos y Sandbox conceptual; la implementación y la explicación de diferencias deben validarse. |
| Save anytime | `BLOCKED FOR CLAIM` | Hay tres slots, autosave y guardado integrado, pero debe verificarse si el jugador puede guardar manualmente en cualquier estado permitido sin pérdida. |
| Playable at your own pace | `PROPOSED` | El juego evita QTE y ofrece pausa/velocidad conceptual, pero la jornada, paciencia y timers deben auditarse. |
| Accesibilidad de Steam | `PLANNED` | El wizard se completará en Release Candidate con evidencia por característica. |

## 2.3. Postura de producto

La primera versión no intentará marcar todas las características posibles. Priorizará una base sólida y honesta:

1. texto e interfaz escalables;
2. lectura sin dependencia exclusiva del color;
3. cámara cómoda y configurable;
4. control adaptable con remapeo cuando esté listo;
5. ritmo que permita pausar, guardar y procesar información;
6. audio ajustable y avisos críticos multimodales;
7. tutorial y ayuda recuperables;
8. errores claros, reversibles y no punitivos;
9. ES/EN sin pérdida funcional;
10. documentación pública exacta.

## 2.4. Objetivos de lanzamiento recomendados

### Mínimo obligatorio de producto

- UI y texto escalables sin bloquear controles críticos.
- Navegación completa por ratón y teclado.
- Remapeo de acciones esenciales o decisión explícita de alcance antes de publicar soporte alternativo.
- Sensibilidad/velocidad de cámara y reducción real de efectos de movimiento.
- Estados críticos con color + icono/forma/texto.
- Volúmenes separados al menos para master, música, efectos y UI/avisos, si existen esas categorías en contenido final.
- Pausa segura y lectura sin temporizadores punitivos en menús.
- Confirmaciones para acciones destructivas.
- tutorial omitible y reiniciable.
- mensajes persistentes o historial para avisos relevantes.
- guardado y recuperación comprensibles.
- ES/EN, fuente final y pseudolocalización.
- auditoría de fotosensibilidad.
- matriz Steam revisada contra la build candidata.

### Objetivos deseables

- mouse-only completo;
- mando completo con prompts dinámicos;
- opciones granulares de contraste;
- filtros o selección de colores por elemento cuando el sistema lo requiera;
- subtítulos configurables si aparece voz;
- presets de accesibilidad;
- narración de menús en una fase posterior si la arquitectura lo permite.

## 2.5. No objetivos de la primera versión

Salvo decisión posterior y evidencia específica, no se promete:

- jugabilidad completa sin visión;
- narración integral de UI y gameplay;
- chat TTS/STT, porque no existe chat multijugador previsto;
- touch-only;
- lengua de signos integrada;
- audio descripción cinematográfica;
- soporte de tecnologías asistivas no probado;
- cumplimiento de todos los niveles AAA de WCAG dentro del runtime del juego;
- compatibilidad universal con cualquier dispositivo adaptativo.

---

# 3. Principios rectores

## 3.1. Diseñar para variabilidad, no para un usuario medio

Las capacidades visuales, auditivas, motoras, cognitivas y lingüísticas varían entre personas y también dentro de una misma persona según fatiga, distancia a la pantalla, lesión temporal, ruido ambiental, dispositivo o contexto. Las opciones deben ser combinables y no asumir una única necesidad.

## 3.2. Acceso equivalente, no experiencia idéntica

Una alternativa puede usar otro canal siempre que preserve:

- la información necesaria;
- la capacidad de decidir;
- el resultado de la acción;
- el feedback de éxito o fallo;
- la dignidad del jugador;
- el ritmo razonable de la experiencia.

## 3.3. No depender de un único canal

Toda información crítica debe usar al menos dos señales compatibles cuando sea razonable:

- color + icono;
- sonido + texto/animación;
- texto + forma/posición;
- vibración + feedback visual;
- temporizador + representación numérica y aviso.

## 3.4. Separar accesibilidad de dificultad

La dificultad modifica presión económica, demanda, costes, sensibilidad al precio, pérdida de reputación u otros parámetros de desafío. La accesibilidad modifica cómo se percibe, controla y procesa la experiencia. Activar texto grande, reducir movimiento, confirmar acciones o remapear controles no debe penalizar puntuaciones, logros, economía ni contenido.

## 3.5. Evitar presets cerrados como única solución

Los presets pueden facilitar el primer arranque, pero deben desglosarse en opciones individuales. “Modo daltónico”, “modo accesible” o “modo fácil” no cubren necesidades diversas por sí solos.

## 3.6. Defaults seguros

Los valores por defecto deben ser cómodos para una audiencia amplia:

- texto legible a 1080p;
- movimiento de cámara moderado;
- motion blur desactivado salvo justificación;
- flashes limitados;
- confirmaciones destructivas activadas;
- tutorial activado pero omitible;
- avisos críticos con texto visible;
- audio sin picos inesperados.

## 3.7. Configuración temprana

Las opciones esenciales deben estar disponibles:

- desde MainMenu;
- antes de iniciar una partida;
- durante la partida mediante pausa;
- sin perder progreso;
- sin exigir completar un tutorial;
- con previsualización cuando sea posible.

## 3.8. Reversibilidad

Toda configuración debe poder:

- modificarse de nuevo;
- restaurarse por categoría o globalmente;
- mostrar sus valores actuales;
- recuperarse de un archivo inválido con defaults seguros;
- sobrevivir a una actualización mediante migración.

## 3.9. Lenguaje no estigmatizante

Las opciones se nombran por función, no por diagnóstico. Se prefiere:

- “Tamaño de texto”;
- “Reducir movimiento”;
- “Mantener para confirmar / Pulsar para confirmar”;
- “Duración de mensajes”;
- “Contraste de interacción”.

Se evita presentar la accesibilidad como ventaja injusta, trampa o modo inferior.

## 3.10. Probar con personas y contextos reales

Los simuladores automáticos de daltonismo, analizadores de contraste y tests de input ayudan, pero no sustituyen playtests con personas que usan las opciones en condiciones reales.

---

# 4. Perfiles de barrera y situaciones de uso

## 4.1. Barreras visuales

Incluyen:

- baja visión;
- visión borrosa o fatiga ocular;
- deficiencias de percepción del color;
- sensibilidad a bajo o alto contraste;
- dificultad para localizar el cursor o foco;
- distancia elevada a la pantalla;
- pantallas pequeñas o escalado del sistema;
- fondos visualmente densos;
- texto sobre superficies en movimiento.

## 4.2. Barreras auditivas

Incluyen:

- sordera o hipoacusia;
- uso sin audio;
- entornos ruidosos;
- dificultad para separar voz, música y efectos;
- sensibilidad a picos, sonidos repetitivos o frecuencias concretas;
- procesamiento auditivo lento.

## 4.3. Barreras motoras

Incluyen:

- uso de una sola mano;
- movilidad o fuerza limitada;
- temblores;
- dificultad para mantener pulsaciones;
- precisión reducida del puntero;
- necesidad de dispositivos adaptativos;
- fatiga ante acciones repetitivas;
- imposibilidad de usar simultáneamente ratón y teclado.

## 4.4. Barreras cognitivas y de aprendizaje

Incluyen:

- memoria de trabajo limitada;
- dificultad para procesar instrucciones extensas;
- atención fluctuante;
- dislexia;
- necesidad de más tiempo para leer o decidir;
- dificultad para interpretar iconos ambiguos;
- sobrecarga por múltiples avisos;
- ansiedad ante pérdida irreversible o presión temporal.

## 4.5. Barreras situacionales

El plan también beneficia a:

- jugadores con lesión temporal;
- personas cansadas;
- usuarios de portátil o Steam Deck;
- quienes juegan con un niño dormido y audio desactivado;
- jugadores con monitores ultrawide o TV lejana;
- usuarios que cambian de dispositivo;
- personas que retoman una partida tras semanas;
- jugadores no nativos del idioma seleccionado.

## 4.6. Personas operativas de prueba

### A — Baja visión y pantalla lejana

Necesita texto grande, contraste claro, cursor visible, foco fuerte y ausencia de información diminuta en mundo.

### B — Percepción de color variable

Necesita formas, iconos, patrones y etiquetas para distinguir válido/inválido, stock, rareza, warnings y categorías.

### C — Una sola mano / mouse-only

Necesita movimiento, cámara, selección, cancelación, pausa y paneles accesibles sin teclado.

### D — Teclado y dispositivo adaptativo

Necesita foco predecible, acciones remapeables, ausencia de trampas de foco y control de cámara sin puntero.

### E — Sensibilidad al movimiento

Necesita reducir rotación automática, shake, zoom brusco, motion blur, transiciones y efectos de énfasis.

### F — Procesamiento lento

Necesita pausa, mensajes persistentes, tutorial recuperable, explicaciones claras y ausencia de decisiones críticas con cuenta atrás breve.

### G — Sin audio

Necesita ver apertura/cierre, error, venta, llegada de pedido, advertencia y estado del cliente sin depender del sonido.

### H — Retorno tras una pausa larga

Necesita resumen de estado, objetivos actuales, ayuda permanente, historial y terminología consistente.

---

# 5. Niveles de prioridad y cobertura

## 5.1. Tier A — Bloqueantes de lanzamiento

Una barrera que impide iniciar, navegar, guardar, cargar, operar el Golden Path o salir de forma segura es Tier A. Ejemplos:

- texto ilegible sin alternativa;
- foco perdido que bloquea un modal;
- acción crítica solo por color;
- cámara incontrolable;
- no poder pausar o salir;
- conflicto de binding sin resolución;
- prompt incorrecto que impide avanzar;
- opción de escala que oculta botones;
- error de guardado no visible.

## 5.2. Tier B — Requisitos fuertes

No siempre bloquean a toda la audiencia, pero afectan acceso significativo:

- controles de volumen incompletos;
- falta de sensibilidad de cámara;
- ausencia de historial de mensajes;
- contraste insuficiente en estados secundarios;
- tutorial no reiniciable;
- ausencia de alternativa visual para un aviso importante;
- texto truncado en una pantalla frecuente.

## 5.3. Tier C — Mejoras de inclusión

Incluyen:

- presets opcionales;
- personalización avanzada de colores;
- narración parcial;
- iconos alternativos;
- reducción adicional de animaciones decorativas;
- perfiles de configuración compartibles.

## 5.4. Regla de deuda

Una deuda Tier A no puede aceptarse en Release Candidate. Una deuda Tier B requiere:

- impacto documentado;
- población afectada;
- workaround real;
- fecha y owner;
- decisión Go/No-Go;
- copy público que no declare la capacidad afectada.

---

# 6. Arquitectura de opciones de accesibilidad

## 6.1. Ubicación

El menú se divide por categorías comprensibles:

- Pantalla e interfaz;
- Cámara y movimiento;
- Controles;
- Audio;
- Subtítulos y avisos, cuando aplique;
- Juego y ritmo;
- Tutorial y ayuda.

No se debe crear una lista monolítica sin búsqueda visual. Las opciones más importantes estarán disponibles desde el primer arranque y el menú de pausa.

## 6.2. Presentación de cada opción

Cada control mostrará:

- nombre funcional;
- valor actual;
- explicación breve;
- efecto esperado;
- necesidad de reinicio, si existe;
- previsualización cuando sea segura;
- botón de restaurar;
- dependencia o incompatibilidad, si la hay.

## 6.3. Aplicación inmediata frente a confirmada

Se aplican inmediatamente:

- tamaño de texto;
- escala de UI;
- contraste de foco;
- volumen;
- sensibilidad de cámara;
- reducción de movimiento;
- duración de mensajes.

Cambios que puedan dejar la pantalla inutilizable, como resolución o modo de pantalla, usarán confirmación con reversión temporal.

## 6.4. Persistencia

Las preferencias globales no pertenecen a un save slot. Deben persistir en un archivo de configuración independiente, tal como establece ADR-0065. El sistema debe:

- escribir de forma atómica;
- validar rangos;
- conservar backup o fallback seguro cuando proceda;
- migrar versiones;
- ignorar campos desconocidos de forma controlada;
- registrar errores sin exponer datos sensibles;
- volver a defaults legibles ante corrupción.

## 6.5. Perfiles

Para la primera versión se recomienda un único perfil global local. Perfiles múltiples o sincronización con Steam Cloud se consideran posteriores y requieren resolver conflictos, privacidad y portabilidad.

## 6.6. Presets propuestos

Los presets son puntos de partida y no bloquean edición granular:

- **Predeterminado:** valores generales.
- **Texto y contraste:** aumenta texto/UI y refuerza foco/contornos.
- **Movimiento reducido:** desactiva shake, suaviza transiciones y limita automatismos.
- **Ritmo relajado:** amplía mensajes, reduce presión temporal configurable y refuerza confirmaciones.
- **Una mano / puntero:** prioriza mouse-only y evita chords.

Estos presets permanecen `PROPOSED` hasta implementar todas las opciones subyacentes.

## 6.7. Restablecimiento

Debe existir:

- restaurar una opción;
- restaurar una categoría;
- restaurar todas las opciones;
- confirmación para el restablecimiento global;
- mensaje de resultado;
- foco devuelto al control adecuado.

---

# 7. Accesibilidad visual: interfaz y texto

## 7.1. Escala de UI

La implementación actual admite `80–150 %`. Para considerarla cerrada:

- todos los paneles deben responder al mismo sistema;
- no pueden quedar botones fuera de pantalla;
- el scroll debe aparecer cuando el contenido crece;
- los modales deben mantener título, cuerpo y acciones visibles;
- el foco no puede desplazarse a elementos ocultos;
- tooltips y tutorial deben escalar;
- los hit targets no deben reducirse al bajar escala;
- la escala se probará en todas las resoluciones objetivo.

El mínimo `80 %` no se usará para justificar texto demasiado pequeño por defecto. El máximo `150 %` se considerará baseline técnica actual, no límite inmutable; las pruebas pueden exigir ampliarlo o introducir modos de layout alternativo.

## 7.2. Escala de texto

El texto se escalará independientemente de la UI. Requisitos:

- no rasterizar texto crítico en imágenes;
- evitar tamaños fijos fuera del sistema de tokens;
- mantener wrapping y altura dinámica;
- permitir scroll en textos extensos;
- no truncar cantidades, precios, nombres de slot, errores o acciones;
- no reducir automáticamente hasta ilegibilidad para “hacer que quepa”;
- mantener una jerarquía relativa coherente;
- probar ES/EN y pseudolocale.

Como referencia externa, Steam recomienda que la opción permita alcanzar al menos un tamaño visual equivalente a 38 píxeles de alto a 1080p o 76 a 4K para texto relevante. La implementación final deberá medir altura renderizada, no limitarse al valor nominal de la fuente.

## 7.3. Tipografía

La fuente final debe:

- ser legible en tamaños pequeños y medianos;
- diferenciar caracteres similares;
- cubrir ES/EN y símbolos económicos necesarios;
- disponer de licencia de distribución;
- incluir fallback definido;
- funcionar con TextMeshPro o la tecnología final elegida;
- evitar pesos excesivamente finos;
- mantener números tabulares o alineación adecuada donde se comparen cantidades.

No se usará mayúscula sostenida en párrafos, mensajes de error o tutoriales extensos.

## 7.4. Longitud de línea y bloques

- Instrucciones: frases breves y una acción principal.
- Párrafos: anchura moderada y separación suficiente.
- Tablas densas: encabezados persistentes, orden y filtros cuando proceda.
- Números: alineación y unidades consistentes.
- Tooltips: no deben desaparecer antes de poder leerse; preferible cierre explícito o duración configurable para información esencial.

## 7.5. Cursor

El cursor debe:

- mantener contraste sobre superficies claras y oscuras;
- indicar estado interactivo sin depender solo del color;
- disponer de tamaño o variante ampliada si las pruebas lo requieren;
- no desaparecer durante UI;
- distinguir selección, colocación, invalidez y espera mediante forma + texto/icono;
- respetar escala del sistema sin volverse diminuto.

## 7.6. Foco de teclado/mando

Todo elemento interactivo requiere estado de foco visible mediante combinación de:

- contorno;
- cambio de forma o grosor;
- contraste;
- animación limitada;
- etiqueta o estado.

El foco debe ser visible sobre hover, selected, disabled, pressed y error. No se usará solo un cambio verde sutil.

## 7.7. Objetivos de contraste

Para UI, web, documentación y materiales se toma WCAG 2.2 AA como referencia práctica cuando sea aplicable:

- contraste de texto normal objetivo mínimo `4.5:1`;
- texto grande objetivo mínimo `3:1`;
- componentes, límites y estados importantes objetivo mínimo `3:1` respecto a adyacentes;
- foco claramente identificable.

El runtime 3D no siempre permite medir como una página web; se usarán capturas representativas, luminancia, revisión en escena y pruebas humanas. El cumplimiento numérico no sustituye legibilidad real.

## 7.8. Alto y bajo contraste

Se recomienda incluir:

- intensidad de contornos interactivos;
- fondo/opacidad de mensajes y tooltips;
- realce de objetos interactivos;
- posibilidad de reducir contrastes extremos si causan fatiga;
- modo de alto contraste en UI cuando la arquitectura esté lista.

No se aplicará un filtro global que degrade arte, texto o percepción de profundidad como única solución.

---

# 8. Color, iconografía y señalización

## 8.1. Regla de no dependencia exclusiva

Los siguientes estados nunca dependerán solo de rojo/verde:

- placement válido/inválido;
- ruta accesible/bloqueada;
- stock disponible/agotado;
- acción permitida/denegada;
- warning/error/success;
- cliente satisfecho/frustrado;
- pedido pendiente/recibido/cancelado;
- caja abierta/cerrada;
- selección/foco;
- deuda/beneficio cuando sea crítico;
- rareza o categoría si afecta a una decisión.

## 8.2. Canales alternativos

| Estado | Color | Canal adicional obligatorio |
|---|---|---|
| Placement válido | Verde | check, borde continuo, texto “Válido” y preview estable. |
| Placement inválido | Rojo | cruz, patrón rayado, texto de motivo y borde discontinuo. |
| Warning | Ámbar | icono triangular y título. |
| Error | Rojo | icono de error, texto persistente y acción de recuperación. |
| Éxito | Verde | check, texto y sonido opcional. |
| Agotado | Gris/rojo | etiqueta `0`, icono de caja vacía y estado textual. |
| Seleccionado | Verde/cyan | contorno de grosor distinto y marcador. |

## 8.3. Paleta de marca

El verde de VRM Games puede seguir siendo acento de marca, foco y validez, pero:

- no ocupará grandes superficies de forma fatigante;
- no será el único indicador de selección;
- no se confundirá con producto, proveedor o decoración;
- se probará contra grafito, madera, cartón, blanco y escenas iluminadas;
- tendrá variantes de luminancia y patrón.

## 8.4. Deficiencias de percepción del color

Se probarán como mínimo escenarios de protanopia, deuteranopia y tritanopia mediante simulación y playtest. La simulación no autoriza por sí sola un claim. Se comprobará:

- placement;
- indicadores de stock;
- gráficos económicos;
- cola y clientes;
- botones primario/secundario/destructivo;
- mapa o heatmaps futuros;
- carátulas y packaging cuando transmitan categoría.

## 8.5. Personalización de colores

Cuando una mecánica dependa de conjuntos de colores, se preferirá personalización por elemento o paletas seleccionables a filtros de pantalla completa. Cualquier selector deberá ofrecer ejemplos en contexto y restauración.

## 8.6. Iconos

Los iconos críticos incluyen texto o tooltip. Deben:

- tener siluetas distintas;
- evitar detalles subpíxel;
- mantener lectura a escala alta y baja;
- no reutilizar el mismo símbolo con significados opuestos;
- disponer de estado disabled comprensible;
- tener nombre accesible en documentación o narración futura.

---

# 9. Legibilidad del mundo 3D

## 9.1. Objetivo

La tienda debe ser comprensible desde cámara elevada sin exigir distinguir detalles minúsculos. Arte, iluminación y composición sirven a la función.

## 9.2. Objetos interactivos

Cada objeto interactivo debe comunicar:

- qué es por silueta;
- si puede usarse;
- desde dónde se usa;
- su estado;
- la acción disponible;
- por qué falla una interacción.

No se hará depender la interacción de texto impreso en packaging o pantallas del mundo.

## 9.3. Iluminación

- Ningún punto crítico queda en sombra ilegible.
- Emisión no sustituye iluminación funcional.
- Las zonas mantienen separación suficiente.
- Los reflejos no ocultan texto o contornos.
- La exposición no quema UI ni superficies claras.
- Se prueba el recorrido completo, no solo capturas heroicas.

## 9.4. Silueta y densidad

Muebles, clientes, empleados, proveedores y productos deben distinguirse por:

- volumen;
- postura;
- altura;
- accesorios;
- paleta secundaria;
- movimiento;
- marcador contextual cuando sea necesario.

La diversidad visual no puede convertir la escena en ruido. Se controlará densidad en pasillos, cola y checkout.

## 9.5. Señalización espacial

La entrada, checkout, receiving, almacén y zonas funcionales necesitan señales redundantes:

- arquitectura;
- mobiliario;
- iluminación;
- iconografía;
- texto localizado;
- ruta o marcador opcional.

## 9.6. Oclusión

La cámara y paredes no deben ocultar al jugador, el destino o un feedback crítico. Las preferencias de wall occlusion se coordinarán con reducción de movimiento y contraste.

---

# 10. Fotosensibilidad, flashes y VFX

## 10.1. Política

Se evitarán destellos rápidos, patrones de alto contraste y flashes a pantalla completa. Todo VFX debe tener una función clara y una variante reducida cuando sea necesario.

## 10.2. Auditoría

Se revisarán:

- transiciones de escena;
- confirmaciones de compra/venta;
- error de placement;
- highlight de objetos;
- puertas automáticas;
- UI pulsante;
- alarmas de cierre;
- partículas de celebración;
- trailers y GIFs de marketing.

## 10.3. Requisitos

- No usar parpadeo como único indicador.
- Evitar alternancia intensa rojo/blanco.
- Limitar área, frecuencia, luminancia y duración.
- Permitir reducir animaciones decorativas.
- No bloquear progreso por desactivar VFX.
- Mantener feedback mediante texto, icono o audio.

## 10.4. Material audiovisual externo

Tráileres, GIFs y clips seguirán los mismos límites. Cuando exista contenido potencialmente sensible se revisará antes de publicación y se añadirá advertencia adecuada si no puede eliminarse.

---

# 11. Cámara y comodidad de movimiento

## 11.1. Riesgos del modelo actual

La cámara orbital, zoom y seguimiento pueden causar incomodidad por:

- rotación rápida;
- aceleración brusca;
- zoom excesivo;
- recentrado automático;
- cambios de altura;
- oclusión que activa/desactiva paredes;
- seguimiento elástico;
- transiciones de foco.

## 11.2. Opciones requeridas

- velocidad de rotación;
- velocidad de zoom;
- inversión de ejes cuando aplique;
- sensibilidad de puntero/cámara;
- reducción o eliminación de shake;
- reducción de suavizado elástico;
- desactivación de motion blur;
- opción de recentrado manual o menor automatismo;
- intensidad de transiciones.

## 11.3. Conexión de `ReduceMotion`

La preferencia existente debe actuar como política común. Al activarla:

- elimina shake no esencial;
- reduce overshoot y easing pronunciado;
- acorta o simplifica transiciones de panel;
- evita zoom automático agresivo;
- limita animaciones pulsantes;
- mantiene feedback funcional;
- no modifica gameplay ni velocidad económica salvo opción separada.

Cada sistema de movimiento deberá registrar si respeta la política. Un booleano sin consumidores no constituye una capacidad.

## 11.4. Prueba

Se medirán:

- grados por segundo;
- tiempo de aceleración/deceleración;
- rango de zoom;
- frecuencia de automatismos;
- cambios de FOV si existen;
- estabilidad a 30/60 FPS;
- funcionamiento con baja tasa de frames.

## 11.5. Default

La cámara por defecto será moderada. No se obligará al jugador a soportar una introducción cinemática antes de acceder a opciones.

---

# 12. Accesibilidad auditiva

## 12.1. Principio

Ninguna acción crítica dependerá exclusivamente del audio. El sonido refuerza, orienta y confirma, pero la UI o el mundo deben ofrecer alternativa visual.

## 12.2. Eventos que requieren alternativa visual

- apertura y cierre de tienda;
- llegada de pedido;
- cliente listo para checkout;
- venta completada;
- error de transacción;
- stock agotado;
- aviso de guardado/autoguardado;
- fallo de guardado;
- advertencia de cierre;
- operación inválida;
- incidente o notificación futura.

## 12.3. Controles de volumen

La versión final debe exponer categorías coherentes con el contenido real. Objetivo mínimo:

- Master;
- Música;
- Efectos;
- UI y avisos;
- Ambiente;
- Voz, si existe.

Los canales deben poder silenciarse individualmente. Los sliders mostrarán valor numérico, soportarán teclado/mando y tendrán reset.

## 12.4. Estado técnico actual

`Phase1AudioRouter` crea fuentes por canal y persiste volúmenes mediante `PlayerPrefs` con default `0.8`. Esto es una fundación limitada. Antes de release debe resolverse:

- UI de control;
- taxonomía estable;
- mezcla final;
- relación entre volumen de entrada y canal;
- migración desde PlayerPrefs si se centraliza configuración;
- pruebas de mute y persistencia;
- límites de loudness y picos;
- alternativa visual de cada evento crítico.

## 12.5. Mezcla

- Evitar que música o ambiente oculten avisos.
- No aumentar volumen de forma inesperada al abrir paneles.
- Mantener sonidos de UI cortos y no fatigantes.
- Ofrecer ducking moderado cuando exista voz.
- Probar altavoces, auriculares y audio mono/estéreo según alcance.
- No usar panoramización como única fuente de información.

## 12.6. Mono y localización espacial

El juego no declarará Stereo Sound o Surround Sound en Steam solo por usar AudioSource. Debe comprobarse que la información direccional sea significativa y que no bloquee a quien use mono. Los eventos críticos tendrán alternativa no espacial.

## 12.7. Sensibilidad sonora

Se evitarán:

- sonidos agudos repetitivos;
- ticks continuos de temporizadores;
- alarmas largas sin control;
- picos de volumen;
- repetición frecuente de error.

Se considerará control separado de avisos o intensidad en una fase posterior si los playtests lo exigen.

---

# 13. Subtítulos, captions y equivalentes textuales

## 13.1. Activación del requisito

Mientras no exista voz o contenido narrativo hablado final, los subtítulos permanecen `NOT APPLICABLE`. En el momento en que se apruebe voz, vídeos hablados o audio semántico no visualizado, el sistema pasa a `REQUIRED FOR RELEASE`.

## 13.2. Cobertura

Los subtítulos deben cubrir:

- todo diálogo hablado;
- voz en tutorial;
- vídeos;
- anuncios o megafonía con información necesaria;
- audio esencial no verbal mediante captions cuando corresponda.

## 13.3. Opciones

- activado/desactivado;
- tamaño independiente;
- fondo y opacidad;
- identificación de hablante;
- color de hablante solo como refuerzo;
- duración suficiente;
- alineación segura;
- captions de efectos esenciales;
- vista previa.

## 13.4. Reglas editoriales

- Texto sincronizado sin fragmentación excesiva.
- No ocultar HUD o acciones.
- No usar solo mayúsculas.
- Identificar hablante de forma localizada.
- Mantener variables y nombres estables.
- No censurar información presente en audio.
- Probar ES/EN y expansión.

## 13.5. Marketing y vídeos

Todo vídeo promocional con voz o texto esencial tendrá:

- subtítulos revisados;
- transcript cuando el canal lo permita;
- texto dentro de safe areas;
- contraste;
- versión localizada o metadata adecuada.

---

# 14. Accesibilidad motora e input

## 14.1. Principios

- Evitar acciones simultáneas innecesarias.
- Evitar button mashing.
- Evitar pulsaciones mantenidas cuando una pulsación simple sea suficiente.
- Permitir alternar hold/toggle donde exista una acción continua.
- No exigir precisión extrema del puntero.
- Proporcionar tolerancia y snap razonables.
- Evitar secuencias rápidas para decisiones administrativas.
- Permitir cancelar de forma consistente.

## 14.2. Modelo de dispositivos

La primera versión considera:

- ratón + teclado como baseline actual;
- ratón solo como objetivo valioso por el movimiento click-to-move;
- teclado solo como objetivo que requiere diseño adicional;
- mando como objetivo de publicación sujeto a Golden Path completo;
- dispositivos adaptativos como beneficiarios del remapeo y acciones digitales, sin claim universal.

## 14.3. Acciones esenciales

El action map final debe cubrir:

- mover/destino;
- rotar cámara;
- zoom;
- seleccionar/interactuar;
- confirmar;
- cancelar/Escape;
- abrir pausa;
- abrir/cerrar Operations;
- navegar tabs/listas;
- rotar placement;
- retirar/mover objeto;
- cambiar velocidad temporal;
- guardar, cuando proceda;
- ayuda contextual.

## 14.4. Contextos

Los contextos históricos se conservan:

- MainMenu;
- Gameplay;
- UI exclusive;
- Placement;
- Paused;
- Modal.

Cada acción debe tener propietario claro. No puede propagarse un clic de UI al mundo ni ejecutarse placement mientras un modal consume input.

## 14.5. Remapeo

El remapeo completo es `PLANNED` y debe cumplir:

- todas las acciones esenciales remapeables;
- bindings por dispositivo;
- detección de conflicto;
- opción de reemplazar, intercambiar o cancelar;
- restaurar defaults;
- guardar cambios;
- prompts dinámicos;
- soporte de teclas modificadoras sin obligar chords;
- protección de una ruta mínima para confirmar/cancelar;
- no impedir abrir opciones por un mapping inválido;
- migración cuando se renombren acciones.

## 14.6. Pulsación, mantener y alternar

Para cada acción se decide explícitamente:

- `Press`;
- `Hold`;
- `Toggle`;
- repetición;
- delay;
- velocidad de repetición.

Cuando mantener pueda causar barrera se ofrece toggle. El Input Actions genérico observado contiene al menos una interacción `Hold`; no se asumirá que esa plantilla representa la experiencia final.

## 14.7. Precisión de puntero

- Hit targets con margen suficiente.
- Placement con snap y feedback de celda.
- Tolerancia ante pequeños movimientos.
- Drag no obligatorio si puede sustituirse por click-select-click.
- Alternativa a sliders mediante botones y entrada discreta.
- Doble clic no esencial.

## 14.8. Mouse-only

Para declarar `Mouse Only Option` en Steam debe poder completarse el juego sin teclado. Deben existir controles en pantalla para:

- cámara y zoom;
- pausa;
- cancelar;
- rotación de objetos;
- velocidad temporal;
- navegación de pestañas;
- confirmación y cierre de modales.

La mera existencia de click-to-move no es suficiente.

## 14.9. Keyboard-only

Para declarar `Keyboard Only Option`:

- la cámara debe controlarse por teclado;
- todo elemento de UI debe tener foco;
- listas y grids deben ser navegables;
- placement debe operarse sin puntero;
- el foco debe volver a origen tras cerrar modal;
- no puede existir hover-only;
- los prompts deben reflejar bindings.

## 14.10. Mando

Para soporte de mando:

- navegación espacial predecible;
- foco inicial en cada pantalla;
- prompts por glyph set;
- desconexión/reconexión segura;
- cambio de dispositivo en caliente;
- deadzones y sensibilidad;
- scroll y sliders operables;
- texto legible a distancia;
- teclado virtual o alternativa para cualquier entrada de texto;
- Golden Path completo en build.

## 14.11. Steam Input

La integración con Steam Input se decide en el documento 24. No se usará para ocultar deficiencias del action map interno. La configuración oficial debe mapear acciones semánticas y probarse en Steam Client/Deck cuando se abra esa fase.

---

# 15. Navegación de UI y foco

## 15.1. Orden

El orden de foco seguirá lectura y jerarquía visual. Evitar saltos entre columnas o elementos fuera de pantalla.

## 15.2. Apertura de pantalla

Al abrir:

- el título y contexto son visibles;
- se selecciona la acción primaria o primer control lógico;
- no se activa accidentalmente una acción;
- el cursor/foco se conserva según dispositivo.

## 15.3. Cierre

Al cerrar:

- se devuelve foco al control que abrió;
- se restaura el contexto de input;
- no se ejecuta la acción subyacente;
- Escape respeta la jerarquía modal → panel → pausa → gameplay.

## 15.4. Estados disabled

Un control disabled debe explicar por qué cuando la información sea útil. No desaparece sin contexto si el jugador necesita comprender una dependencia.

## 15.5. Scroll

- foco visible durante scroll;
- auto-scroll para mantener el elemento seleccionado;
- scrollbar con tamaño suficiente;
- rueda, teclas, mando y drag opcional;
- evitar scroll anidado confuso.

## 15.6. Modales

- foco atrapado dentro del modal;
- título claro;
- cuerpo breve;
- acción primaria y secundaria diferenciadas por texto, no solo color;
- botón de cancelación;
- Escape coherente;
- no cerrar por clic accidental fuera cuando la acción sea destructiva.

## 15.7. Errores de foco

Se consideran Tier A:

- foco invisible;
- foco fuera de pantalla;
- loop que impide llegar a una acción;
- pérdida de foco tras cambiar escala/idioma;
- modal sin salida;
- hover requerido;
- activación doble por el mismo input.

---

# 16. Accesibilidad cognitiva y comprensión

## 16.1. Lenguaje claro

Los mensajes seguirán:

1. qué ocurrió;
2. por qué, si se conoce;
3. qué cambia;
4. qué puede hacer el jugador.

Ejemplo preferido:

> No se puede colocar la estantería. Bloquea la ruta entre la entrada y el mostrador. Muévela una celda o gírala.

Se evita:

> Invalid placement error 17.

## 16.2. Consistencia terminológica

Una entidad mantiene el mismo nombre en:

- mundo;
- HUD;
- Operations;
- tutorial;
- errores;
- ayuda;
- localización;
- Steam y soporte cuando corresponda.

Los sinónimos se controlan mediante glosario.

## 16.3. Densidad de información

- Mostrar lo necesario para la decisión actual.
- Permitir detalle bajo demanda.
- Agrupar por función.
- Usar espacios y encabezados.
- Evitar múltiples banners simultáneos.
- Priorizar errores sobre toasts informativos.
- Conservar historial para no obligar a memorizar.

## 16.4. Tutorial

El tutorial debe ser:

- contextual;
- breve;
- omitible;
- reiniciable;
- persistente por slot;
- compatible con escala e idioma;
- independiente de una tecla fija;
- no bloqueante por timing;
- capaz de explicar el motivo de una regla.

## 16.5. Ayuda permanente

La ayuda no desaparece al omitir tutorial. Debe incluir:

- controles actuales;
- bucle diario;
- construcción;
- inventario;
- pedidos;
- displays;
- clientes y checkout;
- guardado;
- opciones de accesibilidad;
- glosario básico.

## 16.6. Reanudación

Al volver a una partida se recomienda mostrar:

- día y estado;
- caja;
- objetivo o tarea pendiente;
- pedidos en tránsito;
- incidencias;
- último autoguardado;
- acceso a ayuda.

## 16.7. Acciones destructivas

Las confirmaciones actuales se conservan. Cada confirmación debe:

- nombrar la acción;
- describir pérdida;
- diferenciar confirmar/cancelar;
- evitar defaults peligrosos;
- permitir deshacer cuando sea viable;
- no usar “Sí/No” sin contexto.

## 16.8. Errores y recuperación

Los errores importantes permanecen hasta acción o confirmación. Un toast temporal no es suficiente para:

- fallo de guardado;
- corrupción recuperada;
- pérdida de conexión futura;
- compra rechazada con impacto económico;
- operación irreversible;
- incompatibilidad de versión.

---

# 17. Ritmo, tiempo y dificultad

## 17.1. Jugar al propio ritmo

El diseño debe minimizar reacciones precisas. La simulación puede tener tiempo y presión económica sin exigir QTE. Para acercarse a `Playable at Your Own Pace`:

- menús pausan o protegen decisiones cuando sea seguro;
- texto no desaparece por temporizador breve;
- acciones administrativas no caducan mientras se leen;
- la velocidad temporal puede ajustarse;
- el jugador puede pausar para comprender estado;
- los clientes no castigan injustamente tiempo invertido en opciones/UI;
- no hay button mashing.

## 17.2. Velocidad temporal

La visión vigente contempla `×0,5`, `×1`, velocidades superiores y pausa. La implementación final debe:

- mostrar valor actual;
- distinguir tiempo de simulación y tiempo real;
- no acelerar animaciones hasta ilegibilidad;
- preservar transacciones e invariantes;
- respetar lectura de resultados y tutorial;
- ofrecer atajos remapeables y controles en pantalla;
- guardar el valor si se decide que sea preferencia.

## 17.3. Pausa

La pausa:

- detiene simulación cuando el estado lo permite;
- mantiene UI;
- no deja transacciones a medio aplicar;
- no mueve personaje;
- no confirma placement;
- permite acceder a opciones;
- mantiene avisos críticos;
- no borra contexto.

## 17.4. Dificultad

Los presets Relajada, Estándar y Exigente deben explicar diferencias concretas. La dificultad no cambia opciones de accesibilidad. Se recomienda:

- descripción previa a crear partida;
- posibilidad de revisar parámetros;
- Sandbox personalizable;
- advertencia si una decisión no puede modificarse después;
- evitar etiquetas moralizantes como “casual” o “para principiantes”.

## 17.5. Ajustes granulares futuros

Posibles parámetros:

- paciencia de clientes;
- velocidad de jornada;
- frecuencia de incidencias;
- presión económica;
- ayudas de placement;
- pausa en paneles;
- recordatorios.

No se implementan sin evaluar impacto en balance y guardado.

---

# 18. Guardado, interrupción y recuperación

## 18.1. Necesidad de interrupción segura

El jugador puede necesitar detenerse por fatiga, dolor, cuidado de otra persona o contexto. El sistema debe reducir pérdida de progreso.

## 18.2. Estado actual

Existen:

- tres slots;
- guardado integrado versionado;
- autosave diario idempotente;
- backup y recuperación;
- feedback de slot recuperado;
- persistencia de tutorial por slot;
- preferencias globales separadas.

## 18.3. Save Anytime

No se declarará la etiqueta Steam hasta demostrar:

- guardado manual accesible en cualquier estado seguro;
- excepciones claramente explicadas;
- autosave separado de manual;
- ausencia de pérdida significativa;
- carga que restaura estado suficiente;
- feedback visible;
- manejo de fallo;
- prueba durante el Golden Path.

## 18.4. Salida

Al salir con cambios:

- ofrecer guardar y salir, salir sin guardar o cancelar cuando corresponda;
- indicar tiempo/estado del último guardado;
- no esconder el error de guardado detrás de la salida;
- permitir volver al juego.

## 18.5. Recuperación

La recuperación desde backup debe usar lenguaje claro y no culpabilizar. Se explicará qué versión se cargó y si puede haberse perdido progreso.

---

# 19. Localización, idioma y accesibilidad

## 19.1. Idiomas iniciales

- `es-ES` como idioma fuente/primario de diseño documental.
- `en-US` como idioma inicial adicional.

La implementación real de Unity Localization permanece pendiente según el documento 22; las strings hardcodeadas observadas son deuda.

## 19.2. Cambio de idioma

Debe poder hacerse desde MainMenu y opciones. Objetivo:

- aplicar en runtime cuando la arquitectura lo permita;
- conservar selección;
- fallback explícito;
- actualizar prompts, tutorial, mensajes y accesibilidad;
- no perder foco;
- no requerir reinicio salvo limitación documentada.

## 19.3. Expansión

Se probará pseudolocalización con:

- expansión de longitud;
- acentos;
- delimitadores;
- caracteres especiales;
- strings faltantes;
- truncamiento;
- wrapping;
- escalas de texto/UI combinadas.

La prueba crítica combina idioma más expansivo + texto 150 % + UI 150 % + resolución mínima.

## 19.4. Lectura y tono

- Evitar abreviaturas ambiguas.
- Explicar unidades y moneda.
- Mantener números y separadores locales.
- No incrustar texto en imágenes.
- Proteger variables y tags.
- Usar traducción humana o revisión adecuada para contenido público y crítico.

## 19.5. Dislexia y procesamiento

No se asumirá que una “fuente para dislexia” resuelve la barrera. Se prioriza:

- fuente legible;
- espaciado razonable;
- alineación consistente;
- lenguaje claro;
- bloques breves;
- control de tamaño;
- no justificar párrafos extensos;
- no usar cursivas en instrucciones largas.

## 19.6. Lectores de pantalla

No existe evidencia suficiente de narración o exposición de árbol UI. La presencia de `com.unity.modules.accessibility` no constituye soporte. Si se abre este objetivo:

- se realizará spike técnico;
- se evaluará UI Automation/narración propia;
- se definirá lectura de nombre, rol, valor, estado y contexto;
- se permitirá activación temprana;
- se probará con tecnología asistiva real;
- no se declarará “Playable without Vision” si el mundo sigue requiriendo visión.

---

# 20. Diseño inclusivo de personajes, marcas y contenido

## 20.1. Diversidad sin estereotipo

Los personajes pueden variar en:

- edad aparente adulta;
- altura y complexión estilizadas;
- peinado;
- ropa;
- accesorios;
- tono de piel;
- movilidad visible cuando se diseñe de forma respetuosa;
- expresión y postura.

No se asociarán rasgos físicos a:

- paciencia;
- honestidad;
- gasto;
- inteligencia;
- dificultad;
- comportamiento problemático.

## 20.2. Roles legibles

Empleado, cliente, proveedor y trabajador logístico deben distinguirse por contexto, silueta y señales funcionales, no por clichés culturales.

## 20.3. Discapacidad visible

La inclusión de dispositivos de movilidad, prótesis, audífonos u otros elementos requiere:

- investigación;
- consulta cuando sea posible;
- animación y navegación coherentes;
- evitar que sean decoración o chiste;
- no convertirlos en penalización de IA;
- revisar colisiones, accesos y escala del entorno.

## 20.4. Entorno inclusivo

Aunque la tienda sea un espacio ficticio, su diseño debe evitar comunicar barreras innecesarias:

- pasillos legibles;
- accesos claros;
- mostrador interpretable;
- ausencia de clutter extremo;
- señalización comprensible.

La cuadrícula y footprints son abstracciones de gameplay; no se presentarán como simulación normativa de accesibilidad arquitectónica real.

## 20.5. Marcas ficticias

Productos y proveedores no usarán nombres, símbolos o lenguaje que ridiculicen grupos. El humor será situacional y no dependerá de discapacidad, origen, género o identidad.

---

# 21. Steam, store page y declaraciones públicas

## 21.1. Sistema oficial

Steamworks dispone de un Accessibility Feature Wizard en la pestaña Basic Info. Las características seleccionadas aparecen en la store page y pueden usarse como filtros de búsqueda. La selección se realiza contra la build candidata, no contra el roadmap.

## 21.2. Categorías oficiales relevantes

Steam documenta actualmente:

- Adjustable Difficulty;
- Save Anytime;
- Custom Volume Controls;
- Narrated Game Menus;
- Stereo Sound;
- Surround Sound;
- Adjustable Text Size;
- Subtitle Options;
- Color Alternatives;
- Contrast Controls;
- Camera Comfort;
- Playable without Vision;
- Keyboard Only Option;
- Mouse Only Option;
- Touch Only Option;
- Playable without Quick Time Events;
- Playable at Your Own Pace;
- Chat Text-to-speech;
- Chat Speech-to-text.

La lista puede cambiar; se verificará de nuevo al abrir Steamworks y antes de release.

## 21.3. Matriz provisional de claims Steam

| Feature Steam | Estado provisional | Condición para marcar |
|---|---|---|
| Adjustable Difficulty | `PLANNED` | Presets implementados, diferencias explicadas y cambio/selección probado. |
| Save Anytime | `BLOCKED` | Guardado manual + autosave en estados seguros, restore y prueba completa. |
| Custom Volume Controls | `PARTIAL` | UI final y canales independientes demostrados. |
| Narrated Game Menus | `NO` | Solo tras narración integral de menús/notificaciones. |
| Stereo Sound | `UNASSESSED` | Mezcla espacial significativa y prueba; no necesaria para release. |
| Surround Sound | `NO / UNASSESSED` | Solo si se implementa y prueba. |
| Adjustable Text Size | `PARTIAL` | Escala en todas las pantallas, textos y resoluciones; máximo útil validado. |
| Subtitle Options | `N/A NOW` | Si hay voz, opciones completas y cobertura total. |
| Color Alternatives | `PLANNED` | No dependencia de color o personalización comprobada en toda la experiencia. |
| Contrast Controls | `PLANNED` | Control real de contraste/realce en UI/gameplay. |
| Camera Comfort | `PARTIAL` | Velocidad, efectos y reduce motion conectados y probados. |
| Playable without Vision | `NO` | No marcar salvo rediseño/narración integral del mundo. |
| Keyboard Only Option | `BLOCKED` | Golden Path completo con cámara y UI solo teclado. |
| Mouse Only Option | `CANDIDATE` | Controles en pantalla + Golden Path completo solo ratón. |
| Touch Only Option | `NO` | No objetivo inicial. |
| Playable without QTE | `LIKELY CANDIDATE` | Auditoría que confirme ausencia total de QTE/button mashing. |
| Playable at Your Own Pace | `CANDIDATE` | Auditoría de timers, pausa, paciencia y decisiones. |
| Chat TTS/STT | `N/A` | No existe chat. |

## 21.4. Alt text y assets

Steam permite añadir texto alternativo a imágenes de la descripción. El plan 24/28 deberá garantizar:

- alt text de cada imagen informativa;
- imágenes localizadas cuando contengan texto;
- captions/transcripts de vídeo cuando el canal lo permita;
- copy de accesibilidad exacto y no promocionalmente exagerado.

## 21.5. Claim matrix

Cada claim público incluirá:

- feature;
- definición oficial consultada;
- build;
- dispositivos;
- idiomas;
- limitaciones;
- tests;
- fecha de revisión;
- aprobador.

## 21.6. Política de corrección

Si una actualización rompe una característica declarada:

- se clasifica como incidente de release;
- se evalúa hotfix o rollback;
- se corrige la store page si no puede restaurarse de inmediato;
- se comunica con precisión;
- se conserva evidencia.

---

# 22. Arquitectura técnica en Unity

## 22.1. Estado de paquetes

El proyecto previo contiene, entre otros:

- Unity Input System `1.19.0`;
- Unity Localization `1.5.12`;
- uGUI `2.0.0`;
- módulo Unity Accessibility `1.0.0`;
- URP `17.3.0`.

La instalación de un paquete solo crea capacidad potencial. Cada feature necesita integración, UI, datos, pruebas y mantenimiento.

## 22.2. Modelo actual de preferencias

`UiAccessibilitySettings` define:

- `UiScalePercent`;
- `TextScalePercent`;
- `ReduceMotion`;
- `MessageDurationSeconds`;
- `TutorialEnabled`;
- `ConfirmDestructiveActions`.

Rangos actuales:

- UI/texto: `80–150`;
- mensajes: `1–30 s`;
- defaults: `100 %`, `100 %`, movimiento completo, `6 s`, tutorial activado y confirmaciones activadas.

## 22.3. Repositorio y servicio

`JsonAccessibilitySettingsRepository` carga y guarda JSON mediante escritura atómica. `AccessibilitySettingsService` mantiene estado, persiste y emite `Changed`. Esta separación es válida y debe conservarse o migrarse explícitamente.

## 22.4. Deuda detectada

- `MessageDurationSeconds` existe en dominio pero no se ha demostrado un control visible completo.
- `ReduceMotion` no muestra consumidores suficientes para afirmar cobertura global.
- strings de UI observadas están en inglés hardcodeado;
- la UI actual se genera en runtime y necesita migración/validación visual;
- volumen usa PlayerPrefs separado, no el repositorio global;
- no se observa runtime rebinding completo;
- no se observa narración;
- no se observa sistema final de subtítulos;
- no se observa personalización de contraste/color.

## 22.5. Arquitectura objetivo

Se recomienda:

```text
AccessibilityProfile
├── VisualSettings
│   ├── UiScale
│   ├── TextScale
│   ├── ContrastMode
│   ├── InteractionHighlight
│   ├── ColorOverrides
│   └── CursorScale
├── MotionSettings
│   ├── ReduceMotion
│   ├── CameraRotationSpeed
│   ├── CameraZoomSpeed
│   ├── CameraShakeIntensity
│   └── TransitionIntensity
├── InputSettings
│   ├── BindingOverrides
│   ├── HoldTogglePolicy
│   ├── PointerSensitivity
│   ├── GamepadSensitivity
│   └── DeviceGlyphPreference
├── AudioSettings
│   ├── Master
│   ├── Music
│   ├── Effects
│   ├── UiAlerts
│   ├── Ambience
│   └── Voice
├── CaptionSettings
│   ├── Enabled
│   ├── TextScale
│   ├── BackgroundOpacity
│   └── SpeakerLabels
└── CognitiveSettings
    ├── MessageDuration
    ├── TutorialEnabled
    ├── ConfirmDestructiveActions
    ├── PauseOnPanels
    └── ReminderLevel
```

No es obligatorio usar una única clase monolítica. La arquitectura puede separar dominios siempre que exista una fachada, versionado y aplicación coherente.

## 22.6. Servicios de política

Los sistemas deben consultar servicios, no leer flags directamente desde archivos:

- `IMotionPolicy`;
- `ITextPresentationPolicy`;
- `IInputPromptService`;
- `IAudioSettingsService`;
- `IColorSignalPolicy`;
- `IAccessibilitySettingsRepository`.

Esto permite pruebas, migración y sustitución.

## 22.7. Eventos

Al cambiar una opción:

- se notifica solo lo necesario;
- se evita reconstruir toda la escena si no hace falta;
- se preserva foco;
- no se duplica suscripción;
- se comprueba memoria y GC;
- se aplica a elementos ya visibles y futuros.

## 22.8. Tokens de UI

Tamaños, colores, spacing, foco y targets se definen mediante tokens versionados, no literales dispersos. Los componentes finales deben consumirlos.

## 22.9. Prompts de input

Los prompts se generan desde acciones/bindings. No se incrustan teclas en sprites o strings. Deben:

- actualizarse al remapear;
- cambiar al último dispositivo cuando sea estable;
- permitir fijar glyph set;
- disponer de fallback textual;
- localizar conectores y frases.

## 22.10. Compatibilidad de save

Las preferencias globales no deben contaminar snapshots de partida. Si una opción afecta simulación de forma material, debe analizarse si es accesibilidad, dificultad o regla de save.

---

# 23. Datos, versionado y migración

## 23.1. Schema

El archivo de preferencias tendrá:

- versión;
- fecha opcional de última escritura;
- categorías;
- valores validados;
- binding overrides serializados;
- defaults por versión.

## 23.2. Migración

Cuando cambie un rango o se añada una opción:

- conservar valor equivalente;
- clamp solo si es necesario;
- registrar migración;
- no resetear todas las preferencias por un campo nuevo;
- probar archivos de versiones anteriores;
- mantener fallback seguro.

## 23.3. Corrupción

Ante JSON inválido:

- no bloquear arranque;
- conservar copia diagnóstica si es seguro;
- aplicar defaults legibles;
- informar sin alarmismo si la pérdida afecta al jugador;
- no sobrescribir evidencia antes de registrarla.

## 23.4. Portabilidad

Si Steam Cloud incluye preferencias en el futuro:

- definir conflictos entre dispositivos;
- no sincronizar resolución ciegamente;
- separar bindings por dispositivo;
- proteger cambios locales;
- documentar privacidad y soporte.

---

# 24. QA de accesibilidad

## 24.1. Estrategia

La accesibilidad se prueba en cuatro capas:

1. **Unit/EditMode:** rangos, migración, servicios, tokens y reglas.
2. **PlayMode:** aplicación en UI, foco, input contexts y escenas.
3. **Manual dirigida:** Golden Path con configuraciones extremas.
4. **Playtest:** comprensión y uso por personas relevantes.

## 24.2. Matriz mínima de configuraciones

| Perfil | UI | Texto | Movimiento | Input | Audio | Idioma | Resolución |
|---|---:|---:|---|---|---|---|---|
| Default | 100 | 100 | normal | ratón+teclado | normal | ES | 1920×1080 |
| Texto máximo | 150 | 150 | normal | ratón | normal | EN | 1280×720 |
| Movimiento reducido | 120 | 120 | reducido | teclado | sin música | ES | 1920×1080 |
| Sin audio | 100 | 125 | normal | ratón | mute total | EN | 2560×1440 |
| Contraste/color | 125 | 125 | reducido | mando candidato | normal | pseudolocale | 1920×1080 |
| Ventana mínima | 150 | 150 | reducido | ratón+teclado | bajo | ES | mínimo autorizado |

## 24.3. Golden Path accesible

Se repite:

- arrancar;
- abrir opciones;
- modificar configuración;
- crear/cargar slot;
- entrar en StoreInitial;
- mover cámara/personaje;
- construir;
- pedir/recibir;
- asignar/reponer;
- abrir tienda;
- atender clientes;
- checkout;
- cerrar día;
- revisar resultados;
- guardar/cargar;
- salir.

Cada perfil debe registrar bloqueos, fricción, errores, tiempo y workaround.

## 24.4. Tests automatizados recomendados

- validación de rangos;
- defaults;
- igualdad/hash;
- persistencia round-trip;
- JSON corrupto;
- migración;
- evento Changed;
- no evento en valor idéntico;
- escala aplicada;
- foco inicial;
- Escape hierarchy;
- input exclusivity;
- prompts no vacíos;
- bindings esenciales presentes;
- ausencia de conflictos críticos;
- strings localizadas presentes;
- layout overflow detectable cuando sea posible;
- controles interactivos con nombre/label.

## 24.5. Tests manuales visuales

- blur de captura y zoom out para jerarquía;
- escala de grises;
- simulaciones de color;
- monitor con brillo bajo/alto;
- distancia de TV;
- cursor sobre materiales variados;
- UI sobre StoreInitial en zonas claras/oscuras;
- movimiento y VFX con ReduceMotion.

## 24.6. Tests auditivos

- mute total;
- música 0, efectos 100;
- efectos 0, UI 100;
- mono;
- altavoces de portátil;
- auriculares;
- pérdida de foco de aplicación;
- persistencia de volúmenes;
- aviso visual de cada evento crítico.

## 24.7. Tests de input

- ratón solo;
- teclado solo;
- ratón+teclado;
- mando si está en alcance;
- desconexión/reconexión;
- cambio de dispositivo;
- binding duplicado;
- binding inválido;
- reset;
- navegación a escala máxima;
- pulsación repetida/hold;
- clic sobre UI no propagado.

## 24.8. Tests cognitivos

Preguntas:

- ¿Qué debes hacer ahora?
- ¿Por qué falló la acción?
- ¿Cómo la corriges?
- ¿Dónde se cambia una opción?
- ¿Qué se perderá al confirmar?
- ¿Qué ocurrió durante el último día?
- ¿Cómo reanudas el tutorial?

No se ayuda al participante hasta registrar el primer intento.

## 24.9. Severidad específica

| Severidad | Ejemplo |
|---|---|
| `A0 / Critical` | Riesgo de daño por flashes o pérdida irreversible no comunicada. |
| `A1 / Blocker` | Una modalidad declarada no puede completar el Golden Path. |
| `A2 / Major` | Texto máximo oculta una acción crítica o el foco desaparece. |
| `A3 / Moderate` | Tooltip secundario truncado o feedback redundante insuficiente. |
| `A4 / Minor` | Inconsistencia estética sin pérdida funcional. |

## 24.10. Evidencia

Cada ejecución conserva:

- build/version/commit;
- hardware;
- resolución;
- input;
- configuración;
- idioma;
- pasos;
- resultado;
- captura/vídeo/log cuando proceda;
- defectos;
- tester;
- fecha.

---

# 25. Playtests inclusivos

## 25.1. Objetivo

Los playtests deben descubrir barreras, no “aprobar a personas”. Se evalúa el producto.

## 25.2. Reclutamiento

Cuando la capacidad lo permita, incluir participantes con experiencia real en:

- baja visión;
- deficiencias de color;
- sordera/hipoacusia;
- movilidad limitada;
- uso de una mano;
- dispositivos adaptativos;
- dislexia o procesamiento cognitivo diverso;
- sensibilidad al movimiento.

## 25.3. Compensación y consentimiento

- compensación proporcional;
- consentimiento informado;
- datos mínimos;
- opción de retirarse;
- no pedir diagnóstico innecesario;
- accesibilidad del propio formulario/sesión;
- confidencialidad de builds.

## 25.4. Guion

- configurar el juego sin instrucciones externas;
- completar tareas representativas;
- verbalizar cuando sea cómodo;
- registrar estrategias y barreras;
- probar opciones individuales;
- comparar antes/después;
- recoger feedback abierto.

## 25.5. Métricas

- tasa de finalización;
- tiempo por tarea;
- errores;
- asistencia requerida;
- opciones descubiertas;
- opciones usadas;
- motivo de abandono;
- comodidad;
- comprensión;
- confianza para recuperarse de un error.

## 25.6. Interpretación

Una sola barrera grave puede justificar corrección aunque la mayoría no la experimente. No se promedian problemas de acceso hasta desaparecer.

---

# 26. Telemetría, privacidad y accesibilidad

## 26.1. Principio

No se recopilan diagnósticos ni se infiere discapacidad. Las opciones pueden analizarse solo de forma agregada y mínima si existe una pregunta de producto legítima y consentimiento/base aplicable.

## 26.2. Métricas posibles

- porcentaje agregado de escalas usadas;
- activación de ReduceMotion;
- abandono durante tutorial;
- errores de input;
- cambio de idioma;
- bloqueos de navegación;
- uso de pausa;
- tamaño de ventana/resolución en forma no identificable.

## 26.3. Prohibiciones

- asociar una opción a un diagnóstico;
- usar datos de accesibilidad para publicidad dirigida;
- registrar texto libre sensible sin necesidad;
- identificar públicamente a participantes;
- enviar configuración a terceros sin inventario y revisión.

## 26.4. Alternativa sin telemetría

La falta de telemetría no impide validar mediante QA, playtests, soporte y encuestas voluntarias.

---

# 27. Producción y ownership

## 27.1. Responsabilidades

| Área | Responsabilidad |
|---|---|
| Game Design | Ritmo, dificultad, reglas, alternativas y no penalización. |
| UX/UI | Navegación, foco, texto, opciones, errores y layouts. |
| Engineering | Servicios, input, persistencia, aplicación y tests. |
| Art | Color, contraste, silueta, VFX, fotosensibilidad y mundo. |
| Audio | Mezcla, canales, captions y alternativas visuales. |
| Localization | ES/EN, fuentes, expansión, subtítulos y LQA. |
| QA | Matriz, ejecución, severidad y evidencia. |
| Production | Prioridad, deuda, gates y capacidad. |
| Marketing/Steam | Claims exactos, alt text y materiales accesibles. |
| Support | Canales accesibles, triage y comunicación. |

En un proyecto individual, una persona puede ejercer varios roles, pero las responsabilidades no desaparecen.

## 27.2. Definition of Ready

Una feature está lista para implementación cuando:

- barrera y usuario están definidos;
- alcance y default aprobados;
- interacción con sistemas identificada;
- UI/copy diseñados;
- datos/versionado definidos;
- tests previstos;
- dependencia de localización y arte registrada;
- claim Steam asociado si existe.

## 27.3. Definition of Done

- implementación completa;
- pruebas automatizadas apropiadas;
- prueba manual en matriz;
- ES/EN;
- persistencia;
- foco/input;
- configuración extrema;
- rendimiento;
- documentación;
- evidencia;
- defectos bloqueantes cerrados;
- claim actualizado.

## 27.4. Regla de capacidad

La accesibilidad se integra en cada feature. No se reserva exclusivamente para un “sprint de accesibilidad” al final, aunque Sprint 17 tenga una campaña específica de revisión.

---

# 28. Gates de accesibilidad

## 28.1. Gate A0 — Fundación documental

**Salida:** este documento, backlog trazable, estados claros y ausencia de claims prematuros.

## 28.2. Gate A1 — Sprint 16 visual/audio

Requiere:

- StoreInitial legible;
- señales de interacción visibles;
- UI funcional sobre arte representativo;
- audio no esencial para comprender;
- VFX sin riesgo evidente;
- ReduceMotion conectado a efectos introducidos;
- capturas de contraste y oclusión.

## 28.3. Gate A2 — Sprint 17 accessibility review

Campañas:

- UI/texto 80–150 %;
- resolución;
- foco;
- ratón/teclado;
- cámara;
- color;
- audio mute;
- ES/EN;
- tutorial;
- errores;
- guardado;
- Golden Path.

## 28.4. Gate A3 — H6

H6 requiere una experiencia interna representativa sin defectos Tier A. No autoriza aún claims Steam si faltan UI final, localización, mando u otras dependencias de publicación.

## 28.5. Gate A4 — Pre-Steam

- revisar definiciones oficiales;
- completar matriz de features;
- no marcar ninguna sin evidencia;
- preparar copy y alt text;
- verificar input declarado;
- revisar requisitos Steam Deck si aplica.

## 28.6. Gate A5 — Demo/Playtest

- opciones disponibles desde primer arranque;
- feedback y soporte accesibles;
- formulario accesible;
- known issues claros;
- build sin bloqueo de configuración;
- privacidad revisada.

## 28.7. Gate A6 — Release Candidate

- matriz completa;
- todas las combinaciones críticas;
- playtest inclusivo razonable;
- cero A0/A1;
- deuda A2 explícita;
- store page exacta;
- manual/support actualizados;
- rollback/hotfix preparado.

## 28.8. Gate A7 — Postlanzamiento

- monitorizar feedback;
- triage de barreras;
- no romper configuración en patches;
- actualizar claims;
- publicar cambios de accesibilidad en patch notes.

---

# 29. Work packages

| ID | Work package | Prioridad | Dependencias | Salida |
|---|---|---|---|---|
| ACC-WP-001 | Auditoría de implementación Sprint 15 | A | código/build | mapa real de capacidades y deuda |
| ACC-WP-002 | Centralización de settings y versionado | A | TDD/modelo | schema migrable |
| ACC-WP-003 | UI final de accesibilidad | A | UI Style Guide | menú en MainMenu y pausa |
| ACC-WP-004 | Cobertura UI/texto 150 % | A | UI final/localización | layouts validados |
| ACC-WP-005 | Foco y navegación integral | A | input/UI | Golden Path teclado |
| ACC-WP-006 | Sistema de prompts dinámicos | A | Input System | prompts por binding/dispositivo |
| ACC-WP-007 | Runtime rebinding | A/B | action map | remapeo, conflictos y persistencia |
| ACC-WP-008 | Mouse-only audit | B | controles en pantalla | decisión de claim |
| ACC-WP-009 | Gamepad complete path | B | UI/input/Steam | decisión de soporte |
| ACC-WP-010 | Camera comfort | A | cámara/VFX | sliders y ReduceMotion conectado |
| ACC-WP-011 | Color/contrast audit | A | arte/UI | matriz de señales redundantes |
| ACC-WP-012 | Cursor/foco visible | A | UI/arte | variantes y test |
| ACC-WP-013 | Audio settings integration | A | Audio Bible | mixer UI y persistencia |
| ACC-WP-014 | Visual alternatives to audio | A | audio/UI | event matrix |
| ACC-WP-015 | Photosensitivity review | A | VFX/marketing | informe y correcciones |
| ACC-WP-016 | Message duration/history | B | UX | control visible e historial |
| ACC-WP-017 | Tutorial accessibility pass | A | tutorial/localización | escalado, prompts y replay |
| ACC-WP-018 | Save/pause interruption review | A | persistencia/day cycle | decisión Save Anytime |
| ACC-WP-019 | Difficulty explanation | B | balance/UI | presets claros |
| ACC-WP-020 | Localization + pseudolocale matrix | A | doc 22 | ES/EN sin bloqueo |
| ACC-WP-021 | Subtitle framework readiness | C/triggered | audio/loc | arquitectura antes de voz |
| ACC-WP-022 | Inclusive character review | B | Art Bible | checklist y aprobación |
| ACC-WP-023 | Accessibility QA suite | A | QA plan | casos automáticos/manuales |
| ACC-WP-024 | Inclusive playtest | B | build estable/privacy | informe de barreras |
| ACC-WP-025 | Steam feature wizard evidence | A release | doc 24/RC | matriz aprobada |
| ACC-WP-026 | Accessible marketing assets | A release | doc 28 | alt text/subtitles/contrast |
| ACC-WP-027 | Support knowledge base | B release | postlaunch | respuestas y intake |
| ACC-WP-028 | Postlaunch regression pack | A release | CI/build | suite permanente |

---

# 30. Criterios de aceptación maestros

## 30.1. Visual

| ID | Criterio |
|---|---|
| ACC-VIS-001 | Ningún estado crítico depende solo del color. |
| ACC-VIS-002 | UI y texto se escalan hasta el máximo aprobado sin ocultar acciones críticas. |
| ACC-VIS-003 | El foco es visible en todos los controles. |
| ACC-VIS-004 | Cursor legible en zonas claras y oscuras. |
| ACC-VIS-005 | Placement válido/inválido usa señal redundante y motivo. |
| ACC-VIS-006 | No existen flashes o patrones inseguros en rutas normales. |
| ACC-VIS-007 | ReduceMotion modifica todos los efectos registrados. |
| ACC-VIS-008 | La cámara dispone de velocidad configurable y defaults moderados. |
| ACC-VIS-009 | ES/EN no presentan truncamientos funcionales. |
| ACC-VIS-010 | Las imágenes públicas informativas tienen alt text donde la plataforma lo permita. |

## 30.2. Auditiva

| ID | Criterio |
|---|---|
| ACC-AUD-001 | Cada evento crítico audible dispone de alternativa visual. |
| ACC-AUD-002 | Los canales finales pueden ajustarse y silenciarse de forma independiente. |
| ACC-AUD-003 | El volumen persiste y se restaura de forma segura. |
| ACC-AUD-004 | El juego es operable con audio desactivado. |
| ACC-AUD-005 | Si existe voz, toda voz esencial tiene subtítulos configurables. |
| ACC-AUD-006 | No hay picos o repeticiones auditivas bloqueantes. |

## 30.3. Motora

| ID | Criterio |
|---|---|
| ACC-MOT-001 | No hay button mashing ni QTE obligatorios. |
| ACC-MOT-002 | Las acciones esenciales están disponibles en el action map final. |
| ACC-MOT-003 | La UI es navegable por teclado. |
| ACC-MOT-004 | No existe trampa de foco. |
| ACC-MOT-005 | Remapeo, si se declara, cubre acciones esenciales y conflictos. |
| ACC-MOT-006 | Las acciones destructivas tienen confirmación configurable. |
| ACC-MOT-007 | Drag no es la única forma de ejecutar una acción esencial. |
| ACC-MOT-008 | La modalidad pública declarada completa el Golden Path. |

## 30.4. Cognitiva

| ID | Criterio |
|---|---|
| ACC-COG-001 | Los errores explican problema y recuperación. |
| ACC-COG-002 | El tutorial es omitible, reiniciable y no cronometrado. |
| ACC-COG-003 | La ayuda permanece disponible. |
| ACC-COG-004 | Los avisos críticos no desaparecen antes de ser comprendidos. |
| ACC-COG-005 | La pausa permite leer y configurar sin penalización indebida. |
| ACC-COG-006 | Accesibilidad y dificultad son independientes. |
| ACC-COG-007 | La terminología es consistente entre sistemas e idiomas. |
| ACC-COG-008 | Las acciones irreversibles describen la pérdida. |

## 30.5. Persistencia y release

| ID | Criterio |
|---|---|
| ACC-REL-001 | Preferencias sobreviven al reinicio. |
| ACC-REL-002 | Configuración corrupta vuelve a defaults seguros sin bloquear. |
| ACC-REL-003 | Migraciones conservan valores equivalentes. |
| ACC-REL-004 | Build limpia y update mantienen opciones. |
| ACC-REL-005 | La matriz Steam coincide con la build. |
| ACC-REL-006 | Ningún claim se basa solo en código no expuesto o intención. |
| ACC-REL-007 | Patch notes identifican cambios que afecten accesibilidad. |
| ACC-REL-008 | Soporte puede reproducir incidencias con configuración y dispositivo. |

---

# 31. Riesgos y tratamiento

| Riesgo | Impacto | Tratamiento |
|---|---|---|
| Tratar Sprint 15 PASS como cierre total | Claims falsos y barreras no detectadas | limitar PASS al alcance exacto y revalidar RC |
| Escala rompe layouts | Bloqueo visual | layouts flexibles, scroll, matriz extrema |
| ReduceMotion no conectado | Opción engañosa | registry de consumidores y tests |
| Remapeo llega tarde | arquitectura rígida | action map semántico y prompts dinámicos temprano |
| UI runtime provisional se conserva | deuda visual y foco inconsistente | migración planificada y regression |
| Color de marca domina estados | exclusión por color | redundancia, patrones y auditoría |
| Audio final introduce información única | juego no operable sin sonido | event matrix antes de integrar clips |
| Voz se añade sin captions | deuda de release | gate que activa framework y localización |
| Mando se anuncia por bindings genéricos | expectativa rota | Golden Path real y store claim gate |
| Steam cambia definiciones | metadata obsoleta | reverificación Pre-Steam y RC |
| Datos de playtest sensibles | privacidad | minimización, consentimiento y retención |
| Accesibilidad se recorta por plazo | barreras de lanzamiento | Tier A no negociable y scope reduction en otros sistemas |
| Opciones afectan rendimiento | frame pacing peor | profiling por combinación y aplicación eficiente |
| Localización + escala extrema | controles ocultos | pseudolocale combinado y resolución mínima |
| Defaults inseguros | primer arranque excluyente | defaults moderados y acceso temprano |

---

# 32. Playbooks operativos

## 32.1. Añadir una nueva opción

1. Definir barrera y resultado.
2. Asignar ID.
3. Diseñar default/rango.
4. Definir aplicación y consumidores.
5. Diseñar UI/copy ES/EN.
6. Añadir persistencia y migración.
7. Añadir tests.
8. Probar combinaciones.
9. Actualizar ayuda.
10. Revisar claim Steam.

## 32.2. Introducir un nuevo sonido crítico

1. Registrar evento.
2. Clasificar canal.
3. Diseñar alternativa visual.
4. Añadir volumen/mute.
5. Probar sin audio.
6. Revisar repetición/loudness.
7. Localizar texto asociado.

## 32.3. Introducir voz

1. Activar requisito de subtítulos.
2. Diseñar framework.
3. Crear script fuente.
4. Localizar.
5. Implementar opciones.
6. Sincronizar.
7. Probar cobertura 100 %.
8. Actualizar Steam/marketing.

## 32.4. Añadir una acción de input

1. Nombrar semánticamente.
2. Asignar contextos.
3. Proponer bindings por dispositivo.
4. Garantizar remapeo.
5. Crear prompt dinámico.
6. Revisar conflictos.
7. Probar UI/gameplay/pausa.
8. Migrar overrides.

## 32.5. Corregir un defecto reportado por accesibilidad

1. Agradecer y registrar sin exigir diagnóstico.
2. Solicitar configuración mínima reproducible.
3. Clasificar barrera y severidad.
4. Reproducir con perfil.
5. Corregir sin romper otras modalidades.
6. Añadir regression.
7. Comunicar cambio.
8. Actualizar claims si era público.

## 32.6. Completar Steam Accessibility Wizard

1. Obtener definiciones oficiales vigentes.
2. Congelar build candidata.
3. Ejecutar caso por feature.
4. Registrar evidencia.
5. Marcar solo las aprobadas.
6. Revisar copy ES/EN.
7. Capturar configuración final.
8. Repetir tras cambios materiales.

---

# 33. Soporte postlanzamiento

## 33.1. Intake accesible

El formulario o canal debe permitir indicar:

- versión/build;
- sistema operativo;
- resolución;
- dispositivo;
- opción afectada;
- pasos;
- resultado esperado/real;
- captura/log opcional.

No debe exigir completar todos los campos ni usar CAPTCHA inaccesible sin alternativa.

## 33.2. Categorías

- visual/texto;
- color/contraste;
- cámara/movimiento;
- audio/subtítulos;
- input/remapeo;
- foco/navegación;
- tutorial/comprensión;
- guardado/interrupción;
- localización;
- Steam metadata.

## 33.3. SLA interno por severidad

- A0: respuesta inmediata dentro del proceso de incidente.
- A1: priorización de hotfix/rollback.
- A2: siguiente parche apropiado o workaround validado.
- A3/A4: backlog con comunicación transparente.

No se promete públicamente un plazo que la capacidad no pueda sostener.

## 33.4. Patch notes

Se crea una sección “Accesibilidad” cuando haya cambios. Se describe comportamiento, no solo “mejoras varias”.

---

# 34. Anexos operativos

## Anexo A. Checklist de pantalla

- [ ] Título y contexto claros.
- [ ] Foco inicial visible.
- [ ] Orden de foco lógico.
- [ ] Ratón y teclado.
- [ ] Mando si está declarado.
- [ ] Escape/cancelar.
- [ ] UI 150 %.
- [ ] Texto 150 %.
- [ ] ES/EN.
- [ ] Pseudolocale.
- [ ] Resolución mínima.
- [ ] Scroll correcto.
- [ ] Sin hover-only.
- [ ] Estado disabled explicado.
- [ ] Error/empty/loading/success.
- [ ] Confirmación destructiva.
- [ ] Color redundante.
- [ ] Contraste y foco.
- [ ] ReduceMotion.
- [ ] Sin clic propagado al mundo.
- [ ] Captura y evidencia.

## Anexo B. Checklist de sistema de gameplay

- [ ] Información percibible por más de un canal.
- [ ] Acción remapeable o justificación.
- [ ] Sin QTE/button mashing.
- [ ] Pausa segura.
- [ ] Feedback de éxito/fallo.
- [ ] Motivo de operación rechazada.
- [ ] Recuperación.
- [ ] Guardado/restauración.
- [ ] Velocidad temporal considerada.
- [ ] Cámara/oclusiones.
- [ ] Color/patrón/icono.
- [ ] Audio mute.
- [ ] Tutorial/ayuda.
- [ ] Localización.
- [ ] Rendimiento con opciones.

## Anexo C. Checklist de input

- [ ] Acción semántica.
- [ ] Contexto.
- [ ] Default KBM.
- [ ] Default gamepad si aplica.
- [ ] Prompt dinámico.
- [ ] Remapeo.
- [ ] Conflicto.
- [ ] Reset.
- [ ] Persistencia.
- [ ] Cambio de dispositivo.
- [ ] Desconexión.
- [ ] Hold/toggle.
- [ ] No chord obligatorio.
- [ ] Test Golden Path.

## Anexo D. Checklist de audio

- [ ] Canal correcto.
- [ ] Volumen/mute.
- [ ] Alternativa visual.
- [ ] No pico.
- [ ] No repetición fatigante.
- [ ] Funciona sin música.
- [ ] Funciona sin efectos.
- [ ] Funciona sin audio.
- [ ] Caption si es esencial.
- [ ] Licencia y procedencia.

## Anexo E. Checklist de material público

- [ ] Claims demostrados.
- [ ] Alt text.
- [ ] Subtítulos/transcript.
- [ ] Contraste.
- [ ] Texto grande y safe areas.
- [ ] Sin flashes inseguros.
- [ ] ES/EN.
- [ ] No estereotipos.
- [ ] Enlaces accesibles.
- [ ] Build y fecha.

## Anexo F. Plantilla de requisito

```text
ID:
Nombre:
Barrera:
Beneficiarios/contextos:
Estado:
Prioridad:
Comportamiento:
Default/rango:
UI/copy:
Persistencia:
Dispositivos:
Idiomas:
Dependencias:
Casos positivos:
Casos negativos:
Evidencia:
Claim Steam:
Owner:
```

## Anexo G. Plantilla de defecto

```text
ID:
Build/commit:
Perfil de accesibilidad:
Resolución/idioma/input:
Opciones activas:
Pasos:
Resultado real:
Resultado esperado:
Barrera:
Severidad:
Workaround:
Captura/log:
Regresión añadida:
```

## Anexo H. Glosario

| Término | Definición |
|---|---|
| Accesibilidad | Reducción de barreras para percibir, operar, comprender y disfrutar el producto. |
| Diseño inclusivo | Proceso que considera diversidad de capacidades y contextos desde el inicio. |
| Modalidad | Forma completa de interacción, como mouse-only o keyboard-only. |
| Canal redundante | Segunda vía que comunica la misma información esencial. |
| Foco | Elemento de UI que recibirá la siguiente acción de navegación/confirmación. |
| QTE | Evento que exige input preciso dentro de un tiempo breve. |
| Caption | Texto que representa diálogo y/o sonido esencial. |
| Claim | Afirmación pública sobre una característica. |
| Golden Path | Recorrido representativo obligatorio del vertical slice. |
| LQA | QA lingüístico y funcional de localización. |
| ReduceMotion | Política que reduce movimiento no esencial y efectos incómodos. |
| Assistive technology | Herramienta externa que ayuda a interactuar con software. |

## Anexo I. Referencias externas de trabajo

Consultadas el `2026-07-01`:

- Steamworks Documentation — Accessibility Features: `https://partner.steamgames.com/doc/accessibility_features`
- Steamworks Documentation — Store Page Extra Asset Management / Accessibility Text: `https://partner.steamgames.com/doc/store/page/assets`
- W3C — Web Content Accessibility Guidelines 2.2: `https://www.w3.org/TR/WCAG22/`
- Microsoft — Accessibility overview / Xbox Accessibility Guidelines: `https://learn.microsoft.com/en-us/gaming/gdk/docs/gdk-dev/game-principles/accessibility/accessibility-overview`

Estas fuentes se usan como guía. Sus versiones deberán verificarse de nuevo antes de publicación.

## Anexo J. Historial de cambios

| Versión | Fecha | Cambio | Owner |
|---|---|---|---|
| 1.0 | 2026-07-01 | Consolidación completa de fuentes históricas v0.3–v0.6, documentos 00–28, código y registros de Sprints 3, 15–17; definición de requisitos, Steam, QA, gates y work packages | VRM Games / Blas Luis Rocha González |

---

# 35. Inventario de fuentes históricas y vigentes

## 35.1. Fuentes históricas especializadas

La siguiente tabla incluye todas las versiones localizadas de los documentos históricos con incidencia directa en accesibilidad. Los hashes permiten distinguir reediciones contextuales aunque compartan número de versión.

| Ruta histórica | Bytes | SHA-256 |
|---|---:|---|
| `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Art_Bible_v0.3.md` | 13594 | `de2f2c02aef523f9e01bee94c071c44903425c32f63eb0c6b8786ee792397bb3` |
| `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Audio_Bible_v0.3.md` | 3927 | `8530be74a5a22c33b4000ae322d90249eeb7a4567ec123a402c7b5c76a7023c9` |
| `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Enfoque_v0.6_PDF_Source.md` | 124327 | `50f9151e64949a4255514d419c4f75524ddcecc4b1337c293db7d269f3aec052` |
| `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_GDD_v0.4_PCSteam.md` | 114534 | `8939cf748467b18cee2e351b5eac309fe108b7fe02cd6da59a96230e9dd125fe` |
| `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Localization_Plan_v0.3.md` | 3806 | `f48dcd0d33e717b3e65734a9f87677d3b72edb9e67b54ff630720ecc46686349` |
| `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_QA_Testing_Plan_v0.3.md` | 9293 | `517a90d50868dd91010be4eaaca335326b63e8d370613c82a5b5664753ab3891` |
| `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Steam_Publishing_Plan_v0.3.md` | 3923 | `1a004be75991bdcbbaf457a57da123e3297fce15a38c591e3f589e11e277dbd5` |
| `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_UI_Style_Guide_v0.3.md` | 47855 | `d446fa1988af7343cacee8c3817afd3e3fd10f933fe1524d7e0b65f718842065` |
| `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_UX_Flow_v0.3.md` | 87463 | `abb29bbf9b9e9dfbef5161feb03ec07c7bc405cfe4b217e8843534279eb3b50c` |
| `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Art_Bible_v0.3.md` | 13699 | `9d12adda06afbf7db50291337b88095e23005722431fdb4959ffb33afc1ad0c7` |
| `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Audio_Bible_v0.3.md` | 4032 | `9de246aa116d50e54ce46f23ea6e683d337ce3d58f06023af66c068fa6803f4a` |
| `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Enfoque_v0.6_PDF_Source.md` | 124432 | `8e6ba5102d28b0aa36d1306b531407587510850d2a7ca50691389d53ce938548` |
| `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_GDD_v0.4_PCSteam.md` | 114639 | `88be82c3d3cfa4cb5965379e621b0aba522efa1217041cdc23a129ec4817485a` |
| `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Localization_Plan_v0.3.md` | 3911 | `2158be2c46ec697e617419a9192bbaa8a0dd4435b5c650fca9295921b958cb3a` |
| `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_QA_Testing_Plan_v0.4.md` | 10116 | `623c83bd390d3fb1e7cb09e554f6a29f63db662c7ca3e296c65938c65969a724` |
| `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Steam_Publishing_Plan_v0.3.md` | 4028 | `56979c2830951e4a82a7d01793cebe9a930df4a773245de2b7489d0e73cb0777` |
| `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_UI_Style_Guide_v0.3.md` | 47960 | `8e0d241b40ecc46860cc6c51036f436d85ecfeca0cecfc721897b52396b7fca2` |
| `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_UX_Flow_v0.3.md` | 87568 | `2f7e2355cb0e994394e536f8de514ba499aca16a2a81b8e6be1867a025842a9d` |
| `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Art_Bible_v0.4.md` | 2426 | `85241caca78cbac42e40a735b73a116ee47e5937605952cebb092272bb3a1979` |
| `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Audio_Bible_v0.4.md` | 1955 | `b63eff2e7197d01df8375a9d815ddf046d123a222fdaf70079a8356d58c9e092` |
| `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Enfoque_v0.7.md` | 4112 | `dff2fd6d5ae6a688ae147c6eebc2e81b5667cf2433198204867ed51559cc11ea` |
| `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_GDD_v0.5_PCSteam.md` | 4208 | `17284e1e8d67332d5a8e76e56d194b7e176225dfb49e2729caf6664a96c6aa09` |
| `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Localization_Plan_v0.4.md` | 1892 | `01a3ddb286f975644985bc0c034eab75ef2ba4a283cd200f2e5dfd673732839f` |
| `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_QA_Testing_Plan_v0.5.md` | 2853 | `c19ab67f26aae4b44fd5c6d431fb163ddbfccefcbcc0ca68518826040b534f1c` |
| `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Steam_Publishing_Plan_v0.4.md` | 2025 | `19664d4092ea89898c5bc37483a0b1aba6f473abbfa02c2794df8f2dd482058d` |
| `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_UI_Style_Guide_v0.4.md` | 2262 | `8b3346d21414eb370a5e98eb7871f5083cf7bc8d4214ab5cff7ae87ff0b46a22` |
| `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_UX_Flow_v0.4.md` | 3023 | `43143ab9e7504624ffdb4a2e9612332ee795f91ae05ea5d6448e5ce2f018bba8` |
| `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Art_Bible_v0.5.md` | 2501 | `27d4fe8de7bd166329880e6975b1bf90883cd92ca7133b667f1036b09816ca42` |
| `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Audio_Bible_v0.5.md` | 2018 | `3edc3d0db88a36ac7726381883bbf4ffb5f5260a6cb5d972ecf659fbd5116aed` |
| `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Enfoque_v0.8.md` | 3665 | `7723b3a72d7b63ace8f1401c9eb2ed250a671e6a49e867f134583c4572ce524c` |
| `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_GDD_v0.6_PCSteam.md` | 4005 | `333c7777584bcdee29c1caa8f90f4e43e3013e88cc339466a1a51f5bdd42c0d7` |
| `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Localization_Plan_v0.5.md` | 1891 | `e8c63ec8a67f5d87f5dcec078c94ac08620b27a064a5a6866a20fa78de1becf9` |
| `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_QA_Testing_Plan_v0.6.md` | 2637 | `7659a777a9aa78f083d8a6d577898bec89430cae108b079ee7137c769da7f069` |
| `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Steam_Publishing_Plan_v0.5.md` | 1866 | `f7840ee791e07430dd750500421dc6e262d21d5119e634634919247ff13cf8d0` |
| `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_UI_Style_Guide_v0.5.md` | 2181 | `857f070207a752796ff5c21968020d3230a8c357d84a140593c74c47c59ee645` |
| `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_UX_Flow_v0.5.md` | 2707 | `5c755541bbc69510af12839f8435e090972edc7b7bb484139683614f69fcb85c` |

## 35.2. Documentación consolidada vigente

Este plan ha revisado el conjunto actual:

- `00_Enfoque_y_Alcance.md`;
- `01_Game_Design_Document.md`;
- `02_Vertical_Slice_Specification.md`;
- `03_Technical_Design_Document.md`;
- `04_Modelo_de_Datos.md`;
- `05_UX_Flow.md`;
- `06_Production_Roadmap_y_Sprint_Plan.md`;
- `07_QA_Testing_Plan.md`;
- `08_QA_Testing_Matrix.xlsx`;
- `09_CSharp_Coding_Standards.md`;
- `10_Unity_Project_Setup_Guide.md`;
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
- `28_Marketing_and_Communication_Plan.md`.

## 35.3. Código y configuración inspeccionados

Entre los artefactos relevantes:

- `Packages/manifest.json`;
- `Assets/_Project/InputSystem_Actions.inputactions`;
- `UiAccessibilitySettings.cs`;
- `AccessibilitySettingsService.cs`;
- `UIUXRepositoryContracts.cs`;
- `JsonAccessibilitySettingsRepository.cs`;
- `MainMenuSlotScreen.cs`;
- `StoreHudScreen.cs`;
- `Sprint15RuntimeCompositionRoot.cs`;
- `Sprint15InputMapGate.cs`;
- `Phase1AudioRouter.cs`;
- tests EditMode/PlayMode de UIUX;
- ADR y registros de Sprints 3, 15, 16 y 17.

## 35.4. Limitación de la fotografía

El ZIP de proyecto representa el estado previo a la regeneración documental. Este plan no afirma que sea idéntico a cualquier rama futura. Al iniciar implementación deberá verificarse commit, branch, paquetes y estado de la build.

---

# 36. Criterio de cierre del documento 29

El documento 29 se considera generado cuando:

1. preserva la especificación histórica extensa y las revisiones posteriores;
2. separa intención, implementación parcial y claim público;
3. documenta la base real de Sprint 15 sin sobredimensionarla;
4. cubre accesibilidad visual, auditiva, motora y cognitiva;
5. integra input, cámara, ritmo, guardado, tutorial, localización, arte y audio;
6. define diseño inclusivo y representación;
7. mapea las características oficiales de Steam de forma provisional y verificable;
8. define arquitectura, persistencia, migración y deuda técnica;
9. establece QA, playtests, privacidad, severidades y evidencia;
10. fija gates, work packages, riesgos y playbooks;
11. puede incorporarse al Binder, Guía, Producción, Trazabilidad y auditoría final;
12. no presenta como implementada una capacidad que solo exista en documentación o paquete.

**Estado de este documento:** `COMPLETE AS CONSOLIDATED PLAN / IMPLEMENTATION AND RELEASE VALIDATION PENDING`.

**Siguiente documento de la secuencia:** `30_Performance_and_Optimization_Plan.md`.

---

<!-- W0_S17_PHASE1_START -->

# Actualización de accesibilidad W0

## Estado operativo vigente tras W0

| Elemento | Estado vigente | Alcance de la afirmación |
|---|---|---|
| Baseline de entrada | `main@efbc1a885a1d6819f764ec8cff8a465ad1986661` / aplicación `0.0.21` | Referencia congelada para iniciar la remediación |
| W0 documental | `COMPLETED / DOCUMENTAL PASS` | Decisiones formalizadas y contratos sincronizados; no implica implementación |
| Decisiones funcionales abiertas | `0` | DEC-01 a DEC-12 cerradas mediante ADR-0074 a ADR-0085 |
| Sprint17_Phase1 | `BLOCKED / REMEDIATION REQUIRED` | W1-W7 no iniciadas y W8 no ejecutada |
| H6 | `BLOCKED / NOT RUN` | Sin ejecución ni signoff H6 |
| Vertical Slice | `NOT ACCEPTED` | No debe declararse completa ni aprobada |

Las referencias anteriores a Sprint 17 como `PENDING / READY TO OPEN` se conservan como fotografía histórica de cierre de Sprint 16. Para cualquier trabajo posterior prevalece el estado de esta actualización W0.

## Controles añadidos

- Velocidad y pausa se comunican mediante texto, estado y feedback, no solo color o animación.
- Los controles x0,5/x1/x2/x4 mantienen targets legibles y navegación por teclado.
- Un guardado bloqueado informa la causa y la siguiente acción.
- La falta de checkout funcional se explica antes de abrir y durante una invalidación.
- El histórico de Management es operable con scroll, foco, teclado y resoluciones 720p/1080p en ES/EN.
- Ocultar wall occlusion elimina una opción no soportada sin dejar foco invisible, etiqueta huérfana ni preferencia efectiva.
- Las correcciones de pivote no cambian áreas de interacción, colliders o navegación de forma que creen barreras nuevas.

Estas condiciones se verifican en las suites `CHAR-TIM`, `CHAR-CUS`, `CHAR-MGT`, `CHAR-ART` y `CHAR-REG`; siguen `NOT RUN`.

## Regla de cierre y no propagación

- El cierre de W0 no abre W1-W7 automáticamente; cada ola requiere su propia evidencia y control de cambios.
- No se declara Sprint17_Phase1 completada mientras W8 no haya ejecutado la regresión integral.
- No se propaga `PASS` de Sprint 16 a H6 ni a la Vertical Slice.
- No se introducen sistemas Post-H6 durante la remediación.
- Código, escenas, prefabs, builds y tests ejecutables no han sido modificados por esta actualización documental.

<!-- W0_S17_PHASE1_END -->
