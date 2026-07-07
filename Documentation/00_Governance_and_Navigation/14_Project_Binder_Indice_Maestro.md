---
title: "Cartridge & Cloud — Project Binder / Índice Maestro"
author: "VRM Games / Blas Luis Rocha González"
date: "2026-07-07"
lang: es-ES
version: "1.1-RC1"
status: "Current Candidate / Updated during documentation consolidation"
baseline: "00–34 / RC1 consolidation"
---

# Cartridge & Cloud — Project Binder / Índice Maestro

**Proyecto:** Cartridge & Cloud  
**Estudio / autor:** VRM Games / Blas Luis Rocha González  
**Plataforma inicial:** PC / Steam  
**Motor observado:** Unity `6000.3.18f1` / URP `17.3.0`  
**Versión de aplicación de referencia:** `0.0.21`  
**Versión del Binder:** `1.1-RC1`  
**Fotografía documental:** 2026-07-06  
**Estado técnico:** Sprints 0–15 `CLOSED / PASS`; Sprint 16 `COMPLETED / PASS`; Sprint 17 `PENDING / READY TO OPEN`; H6 `BLOCKED / NOT RUN`.  
**Estado documental:** 00–34 presentes; 12 y 13 regenerados; 14 actualizado por este archivo; consolidación final todavía abierta.

> **Regla maestra:** este Binder gobierna la navegación, la localización de autoridad y la ruta inicial de decisión. No sustituye los contratos de producto, las especificaciones técnicas, los registros operativos, las evidencias ni los signoffs. Una afirmación solo puede considerarse cerrada cuando coinciden requisito, estado operativo, implementación observada, prueba, evidencia y decisión formal cuando corresponda.

> **Estado RC1:** el conjunto 00–34 está completo como inventario, pero no está congelado como baseline final. Este Binder es el cuarto paso de una secuencia de 22 controles de consolidación. Su aprobación no cierra H6 ni Release Readiness.

# 0. Control del documento

| Campo | Valor |
| --- | --- |
| ID | 14 |
| Archivo | `14_Project_Binder_Indice_Maestro.md` |
| Versión | 1.1-RC1 |
| Estado | Current Candidate / Updated |
| Acción de consolidación | CNS-004 — actualizar Project Binder |
| Predecesores | 12 regenerado; 13 regenerado |
| Siguiente acción | Regenerar `15_Guia_Maestra.md` |
| Owner | Documentation / Production |
| Hash propio | Se registra externamente después de guardar el archivo para evitar autorreferencia |
| Baseline fuente | RC1 preconsolidación + documentos regenerados 12 y 13 + documentos 28–34 |

## 0.1. Cambios principales de esta revisión

- Extiende el alcance desde 00–13 hasta el conjunto completo 00–34.
- Incorpora como fuentes operativas los Excel 31, 32 y 33 y el manual 34.
- Reconoce los nuevos planes 28–30 como documentos actuales, no futuros.
- Actualiza las rutas de lectura por rol, tarea, sistema y gate.
- Integra el estado real de la consolidación documental, H6, release y continuidad.
- Actualiza los inventarios de los Excel 12 y 13 a sus versiones regeneradas.
- Distingue baseline candidata, baseline final, proyecto Unity, evidencia y archivo histórico.
- Conserva la genealogía v0.3–v0.6 y el índice ADR sin convertir historia en autoridad vigente.

## 0.2. Criterio de aceptación de la revisión

La revisión se considera editorialmente completa cuando un lector sin contexto previo puede identificar el documento competente, seguir una ruta de lectura, conocer el estado real de la baseline, localizar el control operativo correspondiente y entender qué evidencia falta. La aceptación final del Binder queda subordinada a la auditoría 16 regenerada y a la validación del paquete.

# 1. Propósito, alcance, audiencia y límites

## 1.1. Propósito

El Binder es la puerta de entrada oficial al sistema documental de Cartridge & Cloud. Reduce el coste de orientación sin ocultar la complejidad real: identifica autoridad, dependencias, estados, rutas de lectura, mecanismos de cambio, genealogía y condiciones de cierre. Su objetivo no es resumir cada documento, sino impedir que una decisión se tome desde la fuente equivocada o desde una copia histórica descontextualizada.

## 1.2. Alcance

Cubre los 35 documentos numerados 00–34, los artefactos externos que se generarán al congelar la baseline, el proyecto Unity previo a la regeneración, los ADR, commits, builds, evidencias, sprints, handoffs y 745 archivos históricos inventariados. Incluye navegación para producción, diseño, ingeniería, arte, audio, UI/UX, accesibilidad, localización, QA, legal, seguridad, marketing, Steam, continuidad y documentación.

## 1.3. Audiencia

- Propietario y director del proyecto.
- Colaboradores de cualquier disciplina.
- QA, producción, release y documentación.
- Personas responsables de cuentas, legal, seguridad o continuidad.
- Nuevos chats o asistentes sin memoria previa.
- Auditores internos y futuros responsables de mantenimiento.

## 1.4. Lo que el Binder no hace

- No redefine requisitos ni arquitectura.
- No declara implementado lo que solo está especificado.
- No convierte una fórmula de Excel en evidencia técnica.
- No autoriza comunicación, Steam, demo, RC o lanzamiento.
- No sustituye el asesoramiento legal ni de seguridad.
- No permite borrar historia para simplificar la baseline.

# 2. Fotografía ejecutiva y lectura correcta del estado

| Dimensión | Estado actual | Fuente principal |
| --- | --- | --- |
| Producto | Título provisional; PC/Steam; vertical slice representativo | 00, 01, 02 |
| Tecnología | Unity 6000.3.18f1; URP 17.3.0; aplicación 0.0.21 | 03, 10, 11, 34 |
| Producción | Sprint 16 COMPLETED/PASS; Sprint 17 pendiente y listo para apertura formal | 06, 12 |
| H6 | 266 controles; estado inicial NOT RUN/BLOCKED | 02, 31 |
| Release | 209 controles; fase comercial no autorizada | 24, 28, 31 |
| Documentación | 00–34 presentes; cierre de baseline en curso | 12–16, 32, 34 |
| Trazabilidad | 708 requisitos y 708 cadenas end-to-end | 13 |
| Riesgos | 180 riesgos; 174 fuera de tolerancia de forma provisional | 33 |
| Historia | 745 archivos; 643 hashes únicos; 98 grupos duplicados | 32 |
| Consolidación | 4 de 22 acciones al adoptar esta revisión | 12, 13, 34 |

## 2.1. Interpretación obligatoria

La generación de documentos 00–34 significa que existe un marco de trabajo completo; no significa que los work packages, controles, builds, licencias, traducciones, accesibilidad, rendimiento, backups o gates estén ejecutados. StoreInitial y el cierre de Sprint 16 ya están aprobados; el estado técnico queda condicionado por la apertura y estabilización de Sprint 17 y por la ejecución reproducible de H6.

## 2.2. Métricas operativas vigentes

| Registro | Población / estado |
| --- | --- |
| Backlog maestro 12 | 612 elementos; 186 Done; 18 In Progress; 10 Blocked; 337 Planned; 50 Policy Active |
| Work packages 12 | 224; un único paquete de consolidación cerrado como trabajo documental |
| Consolidación documental | 22 acciones; 12, 13 y 14 completados tras aprobar este Binder, además del freeze inicial |
| Controles 31 | 266 H6 + 209 Release |
| Evidencias 31 | 40 paquetes inicializados como MISSING |
| Riesgos 33 | 180; 13 críticos inherentes y residuales provisionales; 174 fuera de tolerancia |
| Trazabilidad 13 | 131 cambios; 163 impactos; 475 controles; 665 pruebas/escenarios; 745 fuentes históricas |

## 2.3. Diferencia entre especificación, implementación, validación y claim

| Nivel | Pregunta | Prueba necesaria |
| --- | --- | --- |
| Especificación | ¿Debe existir o comportarse así? | Documento normativo vigente |
| Planificación | ¿Está priorizado y secuenciado? | 12 y 06 |
| Implementación | ¿Existe en el proyecto/build? | Código, escena, asset o configuración observada |
| Validación | ¿Funciona bajo el escenario requerido? | Resultado de prueba y evidencia |
| Aceptación | ¿Cumple el gate? | Checklist, cero blockers y signoff |
| Claim público | ¿Puede comunicarse? | Aceptación, legal, marketing y revisión de claims |

# 3. Baseline documental vigente 00–34

La tabla siguiente es el índice canónico de nombres. Los enlaces son relativos y asumen que todos los documentos actuales se encuentran en la misma carpeta de baseline. El estado describe la acción documental, no la implementación del producto.

| ID | Documento | Dominio | Clase de autoridad | Estado RC1 |
| --- | --- | --- | --- | --- |
| 00 | [00_Enfoque_y_Alcance.md](00_Enfoque_y_Alcance.md) | Constitución del producto | Constitucional / normativo | READY — sin acción dirigida, salvo correcciones propagadas |
| 01 | [01_Game_Design_Document.md](01_Game_Design_Document.md) | Diseño de juego | Contrato de producto | READY — sin acción dirigida, salvo correcciones propagadas |
| 02 | [02_Vertical_Slice_Specification.md](02_Vertical_Slice_Specification.md) | Vertical Slice y aceptación | Contrato de aceptación / gate | READY — sin acción dirigida, salvo correcciones propagadas |
| 03 | [03_Technical_Design_Document.md](03_Technical_Design_Document.md) | Arquitectura técnica | Autoridad técnica | READY — sin acción dirigida, salvo correcciones propagadas |
| 04 | [04_Modelo_de_Datos.md](04_Modelo_de_Datos.md) | Datos y persistencia | Contrato de datos | READY — sin acción dirigida, salvo correcciones propagadas |
| 05 | [05_UX_Flow.md](05_UX_Flow.md) | UX y flujos | Autoridad UX | READY — sin acción dirigida, salvo correcciones propagadas |
| 06 | [06_Production_Roadmap_y_Sprint_Plan.md](06_Production_Roadmap_y_Sprint_Plan.md) | Producción y roadmap | Autoridad de planificación | READY — sin acción dirigida, salvo correcciones propagadas |
| 07 | [07_QA_Testing_Plan.md](07_QA_Testing_Plan.md) | Estrategia QA | Política de calidad | READY — sin acción dirigida, salvo correcciones propagadas |
| 08 | [08_QA_Testing_Matrix.xlsx](08_QA_Testing_Matrix.xlsx) | QA operativa | Operativo / evidencia | READY — sin acción dirigida, salvo correcciones propagadas |
| 09 | [09_CSharp_Coding_Standards.md](09_CSharp_Coding_Standards.md) | Estándares C# | Normativo técnico | READY — sin acción dirigida, salvo correcciones propagadas |
| 10 | [10_Unity_Project_Setup_Guide.md](10_Unity_Project_Setup_Guide.md) | Entorno Unity | Normativo operativo | READY — sin acción dirigida, salvo correcciones propagadas |
| 11 | [11_Build_y_Versioning_Guide.md](11_Build_y_Versioning_Guide.md) | Build y versionado | Normativo operativo | READY — sin acción dirigida, salvo correcciones propagadas |
| 12 | [12_Excel_Maestro_de_Produccion.xlsx](12_Excel_Maestro_de_Produccion.xlsx) | Producción operativa | Fuente operativa central | REGENERATED RC1 — hash externo capturado |
| 13 | [13_Trazabilidad_y_Control_de_Cambios.xlsx](13_Trazabilidad_y_Control_de_Cambios.xlsx) | Trazabilidad y cambios | Gobierno / auditoría operativa | REGENERATED RC1 — hash externo capturado |
| 14 | [14_Project_Binder_Indice_Maestro.md](14_Project_Binder_Indice_Maestro.md) | Índice maestro | Navegación / gobierno | UPDATED RC1 — este archivo; pendiente auditoría final |
| 15 | [15_Guia_Maestra.md](15_Guia_Maestra.md) | Guía Maestra | Manual de uso integral | PENDING REGENERATION — siguiente documento |
| 16 | [16_Auditoria_Global_de_Coherencia.md](16_Auditoria_Global_de_Coherencia.md) | Auditoría global | Auditoría / verificación | PENDING REGENERATION — después de actualizaciones 14–34 |
| 17 | [17_Art_Bible.md](17_Art_Bible.md) | Arte y dirección visual | Autoridad de disciplina | READY — sin acción dirigida, salvo correcciones propagadas |
| 18 | [18_Audio_Bible.md](18_Audio_Bible.md) | Audio | Autoridad de disciplina | READY — sin acción dirigida, salvo correcciones propagadas |
| 19 | [19_UI_Style_Guide.md](19_UI_Style_Guide.md) | UI visual y componentes | Autoridad de disciplina | READY — sin acción dirigida, salvo correcciones propagadas |
| 20 | [20_Economy_and_Balance_Specification.md](20_Economy_and_Balance_Specification.md) | Economía y balance | Especificación de sistema | READY — sin acción dirigida, salvo correcciones propagadas |
| 21 | [21_Initial_Content_Catalog.xlsx](21_Initial_Content_Catalog.xlsx) | Contenido inicial | Catálogo operativo | TARGETED UPDATE PENDING |
| 22 | [22_Localization_Plan.md](22_Localization_Plan.md) | Localización | Plan de disciplina | READY — sin acción dirigida, salvo correcciones propagadas |
| 23 | [23_Legal_Credits_and_Licenses_Register.xlsx](23_Legal_Credits_and_Licenses_Register.xlsx) | Legal, créditos y licencias | Registro operativo de cumplimiento | TARGETED UPDATE PENDING |
| 24 | [24_Steam_Publishing_Plan.md](24_Steam_Publishing_Plan.md) | Steam publishing | Plan de publicación | READY — sin acción dirigida, salvo correcciones propagadas |
| 25 | [25_Post_Launch_and_Live_Operations_Plan.md](25_Post_Launch_and_Live_Operations_Plan.md) | Postlanzamiento y Live Ops | Plan operativo | READY — sin acción dirigida, salvo correcciones propagadas |
| 26 | [26_Privacy_Data_and_Telemetry_Plan.md](26_Privacy_Data_and_Telemetry_Plan.md) | Privacidad, datos y telemetría | Plan de cumplimiento | READY — sin acción dirigida, salvo correcciones propagadas |
| 27 | [27_Security_and_Incident_Response_Plan.md](27_Security_and_Incident_Response_Plan.md) | Seguridad e incidentes | Plan de seguridad | READY — sin acción dirigida, salvo correcciones propagadas |
| 28 | [28_Marketing_and_Communication_Plan.md](28_Marketing_and_Communication_Plan.md) | Marketing y comunicación | Plan comercial y de comunicación | READY — sin acción dirigida, salvo correcciones propagadas |
| 29 | [29_Accessibility_and_Inclusive_Design_Plan.md](29_Accessibility_and_Inclusive_Design_Plan.md) | Accesibilidad y diseño inclusivo | Plan transversal | READY — sin acción dirigida, salvo correcciones propagadas |
| 30 | [30_Performance_and_Optimization_Plan.md](30_Performance_and_Optimization_Plan.md) | Rendimiento y optimización | Plan técnico transversal | READY — sin acción dirigida, salvo correcciones propagadas |
| 31 | [31_H6_and_Release_Readiness_Checklist.xlsx](31_H6_and_Release_Readiness_Checklist.xlsx) | H6 y Release Readiness | Checklist operativo / signoff | TARGETED UPDATE PENDING |
| 32 | [32_Documentation_Baseline_Manifest.xlsx](32_Documentation_Baseline_Manifest.xlsx) | Manifiesto de baseline | Integridad documental | CANDIDATE — regenerar antes y después de auditoría |
| 33 | [33_Project_Risk_Register_and_Business_Continuity_Plan.xlsx](33_Project_Risk_Register_and_Business_Continuity_Plan.xlsx) | Riesgos y continuidad | Registro operativo / BCP | TARGETED UPDATE PENDING |
| 34 | [34_Final_Handoff_and_Project_Operations_Manual.md](34_Final_Handoff_and_Project_Operations_Manual.md) | Handoff y operaciones | Manual operativo final | TARGETED UPDATE PENDING |

## 3.1. Estructura candidata de carpeta

```text
Documentacion/
├── 00_Enfoque_y_Alcance.md
├── 01_Game_Design_Document.md
├── 02_Vertical_Slice_Specification.md
├── 03_Technical_Design_Document.md
├── 04_Modelo_de_Datos.md
├── 05_UX_Flow.md
├── 06_Production_Roadmap_y_Sprint_Plan.md
├── 07_QA_Testing_Plan.md
├── 08_QA_Testing_Matrix.xlsx
├── 09_CSharp_Coding_Standards.md
├── 10_Unity_Project_Setup_Guide.md
├── 11_Build_y_Versioning_Guide.md
├── 12_Excel_Maestro_de_Produccion.xlsx
├── 13_Trazabilidad_y_Control_de_Cambios.xlsx
├── 14_Project_Binder_Indice_Maestro.md
├── 15_Guia_Maestra.md
├── 16_Auditoria_Global_de_Coherencia.md
├── 17_Art_Bible.md
├── 18_Audio_Bible.md
├── 19_UI_Style_Guide.md
├── 20_Economy_and_Balance_Specification.md
├── 21_Initial_Content_Catalog.xlsx
├── 22_Localization_Plan.md
├── 23_Legal_Credits_and_Licenses_Register.xlsx
├── 24_Steam_Publishing_Plan.md
├── 25_Post_Launch_and_Live_Operations_Plan.md
├── 26_Privacy_Data_and_Telemetry_Plan.md
├── 27_Security_and_Incident_Response_Plan.md
├── 28_Marketing_and_Communication_Plan.md
├── 29_Accessibility_and_Inclusive_Design_Plan.md
├── 30_Performance_and_Optimization_Plan.md
├── 31_H6_and_Release_Readiness_Checklist.xlsx
├── 32_Documentation_Baseline_Manifest.xlsx
├── 33_Project_Risk_Register_and_Business_Continuity_Plan.xlsx
├── 34_Final_Handoff_and_Project_Operations_Manual.md
├── PACKAGE_MANIFEST.md                 # se genera al cierre
├── CHECKSUMS_SHA256.txt                # se genera al cierre
├── DOCUMENTATION_VALIDATION_REPORT.md  # se genera al cierre
├── README_FIRST.md                     # recomendado
├── VERSION.txt                         # recomendado
└── Historical_Baselines/               # historia separada y de solo lectura
```

## 3.2. Baseline candidata frente a baseline final

La RC1 permite trabajar y auditar, pero todavía admite cambios controlados. La baseline final solo existe después de actualizar los documentos señalados, regenerar 16, resolver findings, regenerar 16 y 32, producir manifests/checksums, verificar una extracción limpia y asociar el paquete a un commit y tag. Una carpeta con 35 archivos no es por sí sola una baseline final.

## 3.3. Snapshot preconsolidación

La copia `Cartridge_And_Cloud_Documentation_RC1_PreConsolidation_Snapshot.zip` conserva el estado anterior a reemplazar 12–16. Su SHA-256 externo es `c36b66c03d73b6144fb252e2a38d6bbf1c205c46d17ffcce99ebc4ff5f061924`. No debe sobrescribirse ni tratarse como paquete final.

# 4. Jerarquía de autoridad y resolución de contradicciones

| Nivel | Capa | Documentos | Gobierna |
| --- | --- | --- | --- |
| T0 | Constitución | 00 | Identidad, pilares, alcance y límites |
| T1 | Producto y aceptación | 01, 02, 05 | Experiencia, comportamiento, Golden Path y criterios |
| T2 | Arquitectura y datos | 03, 04, 09, 10, 11 | Solución técnica, datos, entorno y builds |
| T3 | Producción y calidad | 06, 07, 08, 12, 13, 31 | Secuencia, estado, prueba, cambio y signoff |
| T4 | Especialidades | 17–22, 28–30 | Arte, audio, UI, economía, contenido, localización, marketing, accesibilidad y performance |
| T5 | Cumplimiento y publicación | 23–27 | Legal, Steam, live ops, privacidad y seguridad |
| T6 | Integridad y continuidad | 32, 33, 34 | Baseline, riesgos, continuidad, handoff y operaciones |
| T7 | Navegación, guía y auditoría | 14, 15, 16 | Cómo encontrar, usar y auditar el sistema documental |

## 4.1. Reglas acumulativas

- **Competencia:** primero se elige el documento que gobierna la pregunta, no el más reciente en términos absolutos.
- **Constitución:** ninguna capa subordinada puede contradecir 00 sin decisión explícita.
- **Actualidad:** entre registros de la misma competencia prevalece el vigente y más reciente.
- **Especificidad:** una norma especializada puede concretar una general, pero no invalidarla silenciosamente.
- **Evidencia:** el proyecto, build y resultados pueden demostrar discrepancia; no reescriben el requisito por sí solos.
- **Cambio formal:** toda contradicción material se resuelve mediante 13, con impacto, decisión, owner y propagación.
- **Gate:** cuando existe un gate, ningún resumen o porcentaje sustituye sus condiciones y signoff.

## 4.2. Procedimiento en ocho pasos

1. Describir la pregunta sin asumir la respuesta.
2. Identificar la competencia y los documentos candidatos mediante este Binder.
3. Consultar la versión vigente y su autoridad.
4. Contrastar 12 y 13 para conocer estado, cambios e impactos.
5. Contrastar implementación y evidencia cuando la pregunta sea factual.
6. Clasificar la diferencia como error, deuda, cambio, excepción o información histórica.
7. Registrar la decisión y propagarla a los documentos dependientes.
8. Reejecutar pruebas/auditoría y actualizar hashes cuando corresponda.

## 4.3. Ejemplos de contradicción

| Caso | Fuentes | Resolución |
| --- | --- | --- |
| Store vs StoreInitial | 02, 03, 06, 12, ADR-0035-S16, proyecto | Store es baseline funcional histórica; StoreInitial es escena representativa en integración. No se elimina fallback hasta validar. |
| 60 FPS como hecho | 30, 31, benchmarks | Es objetivo histórico y presupuesto; no claim hasta profiling reproducible. |
| Accesibilidad disponible | 29, código, 31, Steam | Distinguir preferencias observadas de remapeo/subtítulos/claims aún no probados. |
| Documento generado = trabajo terminado | 12, 13, plan especializado | Generación cierra el entregable documental, no sus work packages técnicos. |
| Riesgo residual bajo | 33, evidencia | No reducir puntuación solo porque un control esté definido; requiere validación. |

# 5. Tipos, estados y ciclo de vida documental

| Estado / clase | Significado | Uso permitido |
| --- | --- | --- |
| CURRENT AUTHORITATIVE | Fuente vigente de una competencia | Decisiones normativas dentro de su ámbito |
| CURRENT CANDIDATE | Versión activa aún sometida a consolidación/auditoría | Trabajo controlado; no freeze final |
| OPERATIONAL LIVE | Registro que cambia con la ejecución | Estado, resultados, riesgos, evidencias |
| READY | Sin acción dirigida actual | Puede cambiar por findings propagados |
| REGENERATE | Debe reconstruirse contra el conjunto completo | No usar como prueba de cierre final |
| UPDATE | Revisión dirigida de referencias, hashes o contenido | Vigente con limitación conocida |
| FINALIZE LATER | Debe cerrarse después de que los demás dejen de cambiar | Manifiestos y autorreferencias |
| HISTORICAL | Autoridad de su momento, conservada | Genealogía, auditoría y decisiones históricas |
| SUPERSEDED | Reemplazado como autoridad | No borrar; no usar para contradecir vigente |
| EVIDENCE | Prueba de ejecución o estado | Sustenta una afirmación concreta |
| DERIVED | Resumen, exportación o copia | No prevalece sobre su fuente |

## 5.1. Regla de no borrado silencioso

Retirar autoridad no equivale a borrar. Toda sustitución debe conservar elemento anterior, reemplazo, periodo, motivo, cambio y disposición. Los duplicados históricos pueden compartir hash y seguir siendo relevantes porque su ruta demuestra contexto de publicación o trabajo.

## 5.2. Hash y autorreferencia

Los archivos 12, 13, 14, 31–33 y cualquier manifest no deben intentar incorporar dentro de sí mismos su hash final. El hash se calcula después de exportar y se registra en 13, 32 o `CHECKSUMS_SHA256.txt`. Modificar el archivo para insertar su hash lo invalidaría.

# 6. Rutas de lectura

## 6.1. Orientación en 15 minutos

- Leer 00 para identidad y límites.
- Leer la tabla ejecutiva de este Binder.
- Abrir 12/01_Dashboard para estado y siguiente acción.
- Abrir 13/01_Dashboard para cambios e impactos.
- Leer 34 para el procedimiento de arranque/retoma.
- No modificar nada hasta identificar el documento competente.

## 6.2. Orientación en dos horas

- 00, 01 y 02 completos o sus secciones maestras.
- 03, 04 y 05 para arquitectura, datos y UX.
- 06, 07 y 11 para producción, QA y builds.
- 12 y 13 para estado vivo.
- 17–30 según disciplina.
- 31, 33 y 34 para gates, riesgos y operación.

## 6.3. Rutas por rol

| Rol | Lectura principal | Operación diaria | Antes de aprobar |
| --- | --- | --- | --- |
| Project Owner / Production | 00, 02, 06, 14, 15, 34 | 12, 13, 33 | 16, 31, 32 |
| Game Design | 00, 01, 02, 05, 20 | 12, 13, 21 | QA/31 y evidencia de playtest |
| Engineering | 02–04, 09–11, 30 | 12, 13, ADR, 08 | 31, build y logs |
| QA | 02, 07, 08, 31 | 08, 13, evidencias | 31 y 16 |
| Art / Technical Art | 00–02, 10, 17, 19, 30 | 12, 13, 21 | 31, 23 y QA visual |
| Audio | 01–02, 05, 18, 23, 29–30 | 12, 13, 21 | 31 y licencia/evidencia |
| UX/UI/Accessibility | 01, 05, 19, 22, 29 | 12, 13, 31 | LQA, accessibility QA y claims |
| Localization | 05, 19, 22, 24, 28–29 | 12, 13, catálogos | LQA y store review |
| Legal / Privacy / Security | 23, 26, 27, 33 | 13, registros y evidencias | 31, 32 y signoff |
| Marketing / Steam | 00–02, 17, 22–24, 28 | 12, 13, 31 | Legal, claims y gate |
| Documentation | 13–16, 32, 34 | 12, 13, manifests | Auditoría y checksums |

## 6.4. Rutas por tarea

| Tarea | Secuencia mínima |
| --- | --- |
| Implementar feature C# | 00/01 → 02 → 03/04/05 → 09 → ADR si procede → 12/13 → 07/08 → build/evidencia |
| Corregir defecto | 08/31 → 13 → documento competente → cambio/código → prueba afectada → build → cierre |
| Cambiar save/schema | 01/03/04 → ADR → migración/rollback → 07/08 → 11 → 13 → 31/33 |
| Integrar escena/asset | 01/02 → 10/17/19/21/23/30 → 12/13 → QA → build |
| Crear contenido de marketing | 00/01 → 17/19 → 23/24/28 → claim review → gate 31 |
| Añadir telemetría/proveedor | 03/04 → 23/26/27/33 → ADR/DPIA si procede → QA/security → 31 |
| Preparar H6 | 02 → 12/13 → 31 → 08/11 → evidencias → signoff |
| Congelar baseline | 13–16 → 31/32/34 → audit → checksums → ZIP → restore test → commit/tag |

# 7. Registro maestro de documentos 00–34

Cada ficha resume competencia y uso. El contenido normativo sigue residiendo en el archivo enlazado.

## 7.1. Documento 00 — [00_Enfoque_y_Alcance.md](00_Enfoque_y_Alcance.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Constitución del producto |
| Clase | Constitucional / normativo |
| Owner | Product Owner / Creative Direction |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Fija identidad, visión, pilares, plataforma inicial, límites de alcance y principios que ninguna decisión subordinada puede contradecir sin change control. |
| Autoridad | Prevalece para decidir qué producto se está construyendo, qué no forma parte de la fase actual y qué principios deben conservarse. |
| Cuándo consultarlo | Antes de aceptar scope, evaluar una idea, redactar claims, cambiar plataforma o resolver una contradicción de producto. |
| Cuándo actualizarlo | Solo ante una decisión de producto aprobada que altere visión, pilares, audiencia, plataforma o límites constitucionales. |
| Entradas | Baselines históricas de Enfoque, decisiones de dirección y evidencia de alcance. |
| Salidas | Restricciones para 01–34 y criterio de rechazo de scope creep. |
| Límite de uso | No describe implementación, sprint ni estado real de una feature. |

## 7.2. Documento 01 — [01_Game_Design_Document.md](01_Game_Design_Document.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Diseño de juego |
| Clase | Contrato de producto |
| Owner | Game Design |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Define experiencia, bucles, sistemas, reglas, progresión, contenido y comportamiento esperado del juego. |
| Autoridad | Gobierna qué debe hacer el producto dentro de los límites de 00 y antes de traducirlo a arquitectura o tareas. |
| Cuándo consultarlo | Al diseñar, implementar o revisar mecánicas, economía, clientes, tienda, inventario, proveedores y jornada. |
| Cuándo actualizarlo | Cuando cambia una regla de juego aceptada, una interacción, una progresión o un requisito de experiencia. |
| Entradas | 00, investigación de diseño, feedback y decisiones aprobadas. |
| Salidas | Requisitos para 02–08, 17–22, 24–30 y backlog. |
| Límite de uso | No demuestra que una mecánica exista ni que esté validada. |

## 7.3. Documento 02 — [02_Vertical_Slice_Specification.md](02_Vertical_Slice_Specification.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Vertical Slice y aceptación |
| Clase | Contrato de aceptación / gate |
| Owner | Production / QA |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Convierte el alcance del vertical slice en Golden Path, criterios verificables y condiciones de H6. |
| Autoridad | Prevalece para decidir si el slice está dentro de alcance y si una evidencia satisface la aceptación contractual. |
| Cuándo consultarlo | Antes de planificar Sprint 16/17, ejecutar H6, discutir blockers o aceptar una excepción del slice. |
| Cuándo actualizarlo | Solo mediante cambio de alcance formal con impacto en 06–08, 12–13 y 31. |
| Entradas | 00, 01, 03–07 y baselines históricas VSS. |
| Salidas | Criterios para 08, 12, 13, 31 y la auditoría 16. |
| Límite de uso | No sustituye los resultados de ejecución ni el signoff humano. |

## 7.4. Documento 03 — [03_Technical_Design_Document.md](03_Technical_Design_Document.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Arquitectura técnica |
| Clase | Autoridad técnica |
| Owner | Engineering |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Define capas, runtime, ownership, dependencias, servicios, escenas, persistencia y contratos de integración. |
| Autoridad | Gobierna cómo se implementan los requisitos salvo decisión ADR posterior y trazada. |
| Cuándo consultarlo | Antes de añadir código, servicio, assembly, scene composition root, dependencia o integración transversal. |
| Cuándo actualizarlo | Ante cambios arquitectónicos, de dependencias, ciclo de vida o ownership; normalmente exige ADR. |
| Entradas | 00–02, ADR y estado real del proyecto. |
| Salidas | Restricciones para 04, 09–11, 26–30 y tareas técnicas. |
| Límite de uso | No convierte una arquitectura planificada en código existente. |

## 7.5. Documento 04 — [04_Modelo_de_Datos.md](04_Modelo_de_Datos.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Datos y persistencia |
| Clase | Contrato de datos |
| Owner | Engineering / Data |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Define entidades, identificadores, invariantes, schemas, serialización, migración, integridad y recuperación. |
| Autoridad | Prevalece para formatos de datos y compatibilidad, coordinado con ADR de persistencia y evidencia de builds. |
| Cuándo consultarlo | Antes de cambiar save, IDs, dinero, inventario, snapshots, migraciones, backups o telemetría. |
| Cuándo actualizarlo | Todo cambio de schema o invariantes debe propagarse a 03, 07–08, 11–13, 26–27, 29–30, 31 y 33. |
| Entradas | 01, 03, ADR y pruebas de persistencia. |
| Salidas | Schemas y criterios de migración/recuperación. |
| Límite de uso | No acredita compatibilidad hasta ejecutar pruebas y restauraciones. |

## 7.6. Documento 05 — [05_UX_Flow.md](05_UX_Flow.md)

| Campo | Descripción |
| --- | --- |
| Dominio | UX y flujos |
| Clase | Autoridad UX |
| Owner | UX / Engineering |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Describe navegación, estados, jerarquía de acciones, mensajes, cancelación, errores y flujos de usuario. |
| Autoridad | Gobierna el comportamiento de interacción junto con 01, 19 y 29. |
| Cuándo consultarlo | Antes de crear pantallas, flujos de slots, tienda, tutorial, settings, errores o navegación por dispositivo. |
| Cuándo actualizarlo | Cuando cambia un flujo, mensaje, jerarquía de foco, ruta de cancelación o estado visible. |
| Entradas | 01–04, accesibilidad, UI y feedback. |
| Salidas | Requisitos para 07–08, 19, 22, 29 y QA. |
| Límite de uso | No define por sí solo el estilo visual ni la implementación final. |

## 7.7. Documento 06 — [06_Production_Roadmap_y_Sprint_Plan.md](06_Production_Roadmap_y_Sprint_Plan.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Producción y roadmap |
| Clase | Autoridad de planificación |
| Owner | Production |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Ordena sprints, hitos, dependencias, gates, capacidad y secuencia de entrega. |
| Autoridad | Prevalece para la intención de planificación; el estado vivo se confirma en 12 y el cambio se registra en 13. |
| Cuándo consultarlo | Para decidir qué se hace ahora, qué depende de qué y cuándo puede abrirse un gate. |
| Cuándo actualizarlo | Ante replanificación, cambio de hito, capacidad, dependencia o definición de entrada/salida. |
| Entradas | 00–05, 07, riesgos y capacidad. |
| Salidas | Sprints, hitos y restricciones para 12, 31 y 33. |
| Límite de uso | Una fecha o sprint planificado no demuestra ejecución. |

## 7.8. Documento 07 — [07_QA_Testing_Plan.md](07_QA_Testing_Plan.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Estrategia QA |
| Clase | Política de calidad |
| Owner | QA |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Define niveles de prueba, severidades, triage, regresión, entornos, evidencias y reglas de calidad. |
| Autoridad | Gobierna el proceso QA; los casos y resultados operativos viven en 08, 13 y 31. |
| Cuándo consultarlo | Antes de diseñar pruebas, clasificar defectos, cerrar una corrección o preparar un gate. |
| Cuándo actualizarlo | Cuando cambia estrategia, severidad, criterio de evidencia, entorno o política de regresión. |
| Entradas | 02–06, builds, riesgos y estándares especializados. |
| Salidas | Políticas para 08, 12–13, 31 y auditoría. |
| Límite de uso | No contiene por sí solo el resultado de una ejecución. |

## 7.9. Documento 08 — [08_QA_Testing_Matrix.xlsx](08_QA_Testing_Matrix.xlsx)

| Campo | Descripción |
| --- | --- |
| Dominio | QA operativa |
| Clase | Operativo / evidencia |
| Owner | QA |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Mantiene requisitos QA, casos, ejecuciones, defectos, cobertura, builds y gates de calidad. |
| Autoridad | Es la fuente operativa para resultados QA del producto, sin sustituir el checklist de H6/release de 31. |
| Cuándo consultarlo | Para ejecutar regresión, revisar defectos, comprobar cobertura o verificar una build. |
| Cuándo actualizarlo | Después de cada ejecución, defecto, corrección, build o cambio de requisito. |
| Entradas | 02, 05, 07, 11 y artefactos de ejecución. |
| Salidas | Resultados para 12–13, 16 y 31. |
| Límite de uso | No debe usarse para declarar H6 sin signoff y evidencia completa. |

## 7.10. Documento 09 — [09_CSharp_Coding_Standards.md](09_CSharp_Coding_Standards.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Estándares C# |
| Clase | Normativo técnico |
| Owner | Engineering |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Define estilo, arquitectura de código, nullability, errores, rendimiento, tests y revisión. |
| Autoridad | Prevalece para código C# salvo una excepción o ADR aprobado. |
| Cuándo consultarlo | Antes de escribir o revisar C#, diseñar APIs, tests o refactors. |
| Cuándo actualizarlo | Cuando cambia la versión de lenguaje, tooling, reglas de arquitectura o estándares aceptados. |
| Entradas | 03, Unity actual, experiencia del proyecto y ADR. |
| Salidas | Definition of Done técnica y criterios de review. |
| Límite de uso | No sustituye el TDD ni justifica una arquitectura por sí solo. |

## 7.11. Documento 10 — [10_Unity_Project_Setup_Guide.md](10_Unity_Project_Setup_Guide.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Entorno Unity |
| Clase | Normativo operativo |
| Owner | Engineering / Technical Art |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Documenta versión de Unity, paquetes, estructura, importación, escenas, settings y setup reproducible. |
| Autoridad | Gobierna cómo reconstruir el entorno y mantener coherencia de proyecto. |
| Cuándo consultarlo | Al clonar, recuperar, actualizar Unity/paquetes, importar assets o configurar escenas. |
| Cuándo actualizarlo | Ante cambios de editor, paquetes, estructura, perfiles, settings o pipeline. |
| Entradas | 03, 09, estado observado del proyecto y package manifests. |
| Salidas | Entorno reproducible para 11, QA y colaboradores. |
| Límite de uso | No garantiza que una máquina concreta reproduzca el entorno sin verificación. |

## 7.12. Documento 11 — [11_Build_y_Versioning_Guide.md](11_Build_y_Versioning_Guide.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Build y versionado |
| Clase | Normativo operativo |
| Owner | Build / Engineering |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Define numeración, perfiles, procedencia, manifests, hashes, canales, retención y release. |
| Autoridad | Prevalece para producir y custodiar builds; 31 decide readiness y 32 gobierna la baseline documental. |
| Cuándo consultarlo | Antes de crear una build, tag, candidata H6/RC o artefacto distribuible. |
| Cuándo actualizarlo | Cuando cambia el pipeline, versionado, perfil, canal, integridad o retención. |
| Entradas | 03, 06–10, seguridad y Steam. |
| Salidas | Builds reproducibles y evidencias para 08, 13 y 31. |
| Límite de uso | Una build existente no implica aprobación del gate. |

## 7.13. Documento 12 — [12_Excel_Maestro_de_Produccion.xlsx](12_Excel_Maestro_de_Produccion.xlsx)

| Campo | Descripción |
| --- | --- |
| Dominio | Producción operativa |
| Clase | Fuente operativa central |
| Owner | Production / Documentation |
| Estado RC1 | REGENERATED RC1 — hash externo capturado |
| Función | Consolida backlog, sprints, work packages, consolidación documental, dependencias, riesgos, gates, builds, documentos y métricas. |
| Autoridad | Es la fotografía operativa de trabajo y prioridad; no sustituye los contratos normativos ni la trazabilidad de 13. |
| Cuándo consultarlo | Diariamente para priorizar, revisar WIP, blockers, capacidad, siguiente acción y progreso de cierre. |
| Cuándo actualizarlo | Ante cualquier cambio de estado, prioridad, owner, dependencia, sprint, riesgo, gate o documento. |
| Entradas | 00–11, 17–34, registros históricos y ejecución real. |
| Salidas | Estado para Binder, Guía, auditoría y decisiones de producción. |
| Límite de uso | Las fórmulas resumen entradas; no convierten datos no verificados en verdad técnica. |

## 7.14. Documento 13 — [13_Trazabilidad_y_Control_de_Cambios.xlsx](13_Trazabilidad_y_Control_de_Cambios.xlsx)

| Campo | Descripción |
| --- | --- |
| Dominio | Trazabilidad y cambios |
| Clase | Gobierno / auditoría operativa |
| Owner | Documentation / Production |
| Estado RC1 | REGENERATED RC1 — hash externo capturado |
| Función | Relaciona requisito, cambio, impacto, ADR, tarea, prueba, build, evidencia, riesgo, baseline y fuente histórica. |
| Autoridad | Prevalece para saber qué cambió, por qué, qué debe propagarse y qué evidencia respalda una afirmación. |
| Cuándo consultarlo | Antes de cambiar documentos o código, cerrar un cambio, investigar una decisión o preparar auditoría. |
| Cuándo actualizarlo | Append-only para cambios; actualizar impactos y cierres sin borrar la historia. |
| Entradas | 00–12, 31–34, commits, builds, ADR y evidencias. |
| Salidas | Cadena auditable para 14–16, 31–34 y package manifest. |
| Límite de uso | Trazabilidad completa significa vínculos presentes, no ejecución ni conformidad automática. |

## 7.15. Documento 14 — [14_Project_Binder_Indice_Maestro.md](14_Project_Binder_Indice_Maestro.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Índice maestro |
| Clase | Navegación / gobierno |
| Owner | Documentation / Production |
| Estado RC1 | UPDATED RC1 — este archivo; pendiente auditoría final |
| Función | Indica dónde está la autoridad, cómo se navega 00–34 y qué rutas seguir por rol, tarea, sistema y gate. |
| Autoridad | Gobierna navegación y resolución inicial de fuentes, pero nunca reemplaza el contenido del documento competente. |
| Cuándo consultarlo | Como primera lectura, al incorporar colaboradores, iniciar un chat, resolver contradicciones o preparar un handoff. |
| Cuándo actualizarlo | Cuando se añade, renombra, retira o cambia de autoridad un documento; también al cambiar gates o estado de baseline. |
| Entradas | 00–13, 15–34, 32, genealogía histórica y estado operativo. |
| Salidas | Mapa maestro para todo el equipo y la Guía 15. |
| Límite de uso | No es una especificación funcional, técnica ni una evidencia de ejecución. |

## 7.16. Documento 15 — [15_Guia_Maestra.md](15_Guia_Maestra.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Guía Maestra |
| Clase | Manual de uso integral |
| Owner | Documentation / Production |
| Estado RC1 | PENDING REGENERATION — siguiente documento |
| Función | Explica cómo dirigir el trabajo de principio a fin usando la jerarquía, los Excel y los gates. |
| Autoridad | Gobierna el procedimiento de uso coordinado, subordinado a contratos y fuentes operativas. |
| Cuándo consultarlo | Para onboarding, planificación, ejecución, revisión, cierre, handoff y retoma. |
| Cuándo actualizarlo | Después de actualizar 12–14 y cuando cambie el flujo operativo o de gobernanza. |
| Entradas | 00–14 y 17–34. |
| Salidas | Procedimientos reproducibles para personas y asistentes. |
| Límite de uso | Su versión actual requiere regeneración contra 00–34. |

## 7.17. Documento 16 — [16_Auditoria_Global_de_Coherencia.md](16_Auditoria_Global_de_Coherencia.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Auditoría global |
| Clase | Auditoría / verificación |
| Owner | Documentation / QA |
| Estado RC1 | PENDING REGENERATION — después de actualizaciones 14–34 |
| Función | Comprueba coherencia estructural, de producto, técnica, operativa, comercial, legal e integral de 00–34. |
| Autoridad | Es la fuente de hallazgos de coherencia de la baseline auditada, no una sustitución de sus documentos. |
| Cuándo consultarlo | Antes de congelar una baseline, después de cambios transversales y para priorizar correcciones. |
| Cuándo actualizarlo | Debe regenerarse con el conjunto completo, repetirse tras resolver hallazgos y cerrarse antes del ZIP final. |
| Entradas | 00–35, código/proyecto, hashes y evidencias. |
| Salidas | Findings, severidades y decisión de validación. |
| Límite de uso | La versión preconsolidación está desactualizada y no puede aprobar el paquete final. |

## 7.18. Documento 17 — [17_Art_Bible.md](17_Art_Bible.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Arte y dirección visual |
| Clase | Autoridad de disciplina |
| Owner | Art Direction / Technical Art |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Define estilo, formas, materiales, iluminación, personajes, entornos, props, producción y aceptación visual. |
| Autoridad | Prevalece para decisiones visuales dentro de 00, 01, 02, 05 y requisitos técnicos. |
| Cuándo consultarlo | Al crear, comprar, integrar o revisar arte, StoreInitial, personajes, props y marketing visual. |
| Cuándo actualizarlo | Ante cambios de estilo, pipeline, target visual, presupuesto, naming o criterios de aceptación. |
| Entradas | 00–05, 10, referencias visuales e historia de arte. |
| Salidas | Requisitos para 19, 21, 28–31 y QA visual. |
| Límite de uso | No autoriza licencias ni afirma que un asset esté integrado. |

## 7.19. Documento 18 — [18_Audio_Bible.md](18_Audio_Bible.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Audio |
| Clase | Autoridad de disciplina |
| Owner | Audio / Design |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Define identidad sonora, música, SFX, mezcla, implementación, licencias, accesibilidad y QA. |
| Autoridad | Prevalece para diseño de audio, subordinado a UX, accesibilidad, legal y rendimiento. |
| Cuándo consultarlo | Al crear, adquirir, integrar o probar audio y cuando se diseñen captions o feedback redundante. |
| Cuándo actualizarlo | Ante cambios de lenguaje sonoro, pipeline, mezcla, formatos, licencias o budgets. |
| Entradas | 00–07, 17, 22–23, 29–30. |
| Salidas | Work packages, criterios QA y necesidades legales. |
| Límite de uso | No demuestra que audio final o licencias estén cerrados. |

## 7.20. Documento 19 — [19_UI_Style_Guide.md](19_UI_Style_Guide.md)

| Campo | Descripción |
| --- | --- |
| Dominio | UI visual y componentes |
| Clase | Autoridad de disciplina |
| Owner | UI / UX |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Define sistema visual de interfaz, componentes, estados, layouts, tipografía, iconografía y responsive behavior. |
| Autoridad | Gobierna presentación UI junto con 05 y 29; la lógica técnica sigue 03 y 09. |
| Cuándo consultarlo | Al diseñar o implementar pantallas, HUD, settings, focus, prompts y componentes. |
| Cuándo actualizarlo | Cuando cambian tokens, componentes, layouts, tipografía, estados o soporte de escalado. |
| Entradas | 01, 05, 17, 22, 29. |
| Salidas | Especificaciones para implementación y QA visual/accesible. |
| Límite de uso | No sustituye el flujo UX ni acredita accesibilidad. |

## 7.21. Documento 20 — [20_Economy_and_Balance_Specification.md](20_Economy_and_Balance_Specification.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Economía y balance |
| Clase | Especificación de sistema |
| Owner | Game Design / Economy |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Define modelos económicos, precios, costes, demanda, márgenes, progresión y metodología de balance. |
| Autoridad | Prevalece para balance dentro del diseño de 01 y los invariantes monetarios de 04. |
| Cuándo consultarlo | Al ajustar catálogo, proveedores, ventas, dinero, progresión o simulaciones. |
| Cuándo actualizarlo | Ante cambios de fórmulas, objetivos, datos, catálogo o resultados de playtest. |
| Entradas | 01, 04, evidencia de juego y 21. |
| Salidas | Parámetros y escenarios para contenido, QA y telemetría. |
| Límite de uso | Los valores son candidatos hasta validación de balance. |

## 7.22. Documento 21 — [21_Initial_Content_Catalog.xlsx](21_Initial_Content_Catalog.xlsx)

| Campo | Descripción |
| --- | --- |
| Dominio | Contenido inicial |
| Clase | Catálogo operativo |
| Owner | Content / Design |
| Estado RC1 | TARGETED UPDATE PENDING |
| Función | Inventaría productos, proveedores, assets, IDs, estados de autoría, fuentes, licencias y cobertura inicial. |
| Autoridad | Es la fuente operativa de contenido, coordinada con 04, 17–20 y 23. |
| Cuándo consultarlo | Al crear contenido, asignar IDs, comprobar cobertura o preparar StoreInitial y campañas. |
| Cuándo actualizarlo | Ante altas, bajas, cambios de estado, rutas, licencias, hashes o requisitos especializados. |
| Entradas | 01, 04, 17–20, 22–23. |
| Salidas | Contenido trazable para Unity, QA, legal y marketing. |
| Límite de uso | Requiere actualización dirigida tras 28–34 y hashes finales. |

## 7.23. Documento 22 — [22_Localization_Plan.md](22_Localization_Plan.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Localización |
| Clase | Plan de disciplina |
| Owner | Localization / UX |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Define idiomas, arquitectura de strings, glosario, pseudolocalización, LQA, expansión y ownership. |
| Autoridad | Prevalece para localización, subordinado a UX, UI, accesibilidad, legal y Steam. |
| Cuándo consultarlo | Al crear texto, UI, store copy, subtítulos, glosario o builds ES/EN. |
| Cuándo actualizarlo | Ante cambios de idiomas, términos, pipeline, fuentes, layouts o requisitos de publicación. |
| Entradas | 01, 05, 19, 24, 28–29. |
| Salidas | Strings y criterios LQA para QA y Steam. |
| Límite de uso | No declara ES/EN completos hasta que existan assets y LQA. |

## 7.24. Documento 23 — [23_Legal_Credits_and_Licenses_Register.xlsx](23_Legal_Credits_and_Licenses_Register.xlsx)

| Campo | Descripción |
| --- | --- |
| Dominio | Legal, créditos y licencias |
| Clase | Registro operativo de cumplimiento |
| Owner | Legal / Production |
| Estado RC1 | TARGETED UPDATE PENDING |
| Función | Registra titularidad, licencias, obligaciones, créditos, notices, naming, marcas y evidencia legal. |
| Autoridad | Es la fuente operativa de autorización de uso y distribución de assets y servicios. |
| Cuándo consultarlo | Antes de incorporar un asset/servicio, publicar material, distribuir una build o cerrar Steam. |
| Cuándo actualizarlo | Ante cualquier alta, licencia, cambio de proveedor, atribución, clearance o documento legal. |
| Entradas | 17–18, 21, 24, 26–28 y evidencias de adquisición. |
| Salidas | Créditos, notices, approvals y blockers legales. |
| Límite de uso | Requiere actualización dirigida y no sustituye asesoramiento jurídico. |

## 7.25. Documento 24 — [24_Steam_Publishing_Plan.md](24_Steam_Publishing_Plan.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Steam publishing |
| Clase | Plan de publicación |
| Owner | Steam / Production |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Define account readiness, AppID, store page, depots, builds, revisión, releases, soporte y gates de Steam. |
| Autoridad | Prevalece para el proceso Steam, sujeto a H6, legal, privacidad, marketing y release readiness. |
| Cuándo consultarlo | Antes de abrir Steamworks, publicar Coming Soon, subir depots, demo, Playtest o RC. |
| Cuándo actualizarlo | Cuando cambian reglas de Steam, estrategia de publicación, builds, fechas, owners o store assets. |
| Entradas | 02, 06–08, 11, 22–23, 25–31. |
| Salidas | Gates y entregables para store y lanzamiento. |
| Límite de uso | No autoriza publicar sin decisión formal y evidencias. |

## 7.26. Documento 25 — [25_Post_Launch_and_Live_Operations_Plan.md](25_Post_Launch_and_Live_Operations_Plan.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Postlanzamiento y Live Ops |
| Clase | Plan operativo |
| Owner | Production / Support |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Define soporte, triage, hotfixes, updates, rollback, comunicación, métricas y mantenimiento después de release. |
| Autoridad | Gobierna operaciones postlanzamiento una vez abiertas, coordinado con seguridad, privacidad y Steam. |
| Cuándo consultarlo | Al preparar soporte, políticas de parche, incidentes, roadmap postlaunch o cierre de servicio. |
| Cuándo actualizarlo | Ante cambios de capacidad, SLAs, canales, herramientas, telemetría o estrategia de contenidos. |
| Entradas | 07–13, 23–27, 30–34. |
| Salidas | Runbooks y gates operativos para launch y soporte. |
| Límite de uso | No implica compromiso de live service ni calendario futuro. |

## 7.27. Documento 26 — [26_Privacy_Data_and_Telemetry_Plan.md](26_Privacy_Data_and_Telemetry_Plan.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Privacidad, datos y telemetría |
| Clase | Plan de cumplimiento |
| Owner | Privacy / Engineering |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Define minimización, base de datos, telemetría, consentimiento, retención, derechos y evaluación de proveedores. |
| Autoridad | Prevalece para tratamiento de datos personales y telemetría, sujeto a legal aplicable. |
| Cuándo consultarlo | Antes de añadir analytics, crash reporting, cuentas, formularios, comunidad o soporte con datos. |
| Cuándo actualizarlo | Ante nueva categoría de datos, proveedor, finalidad, retención, región o requisito legal. |
| Entradas | 03–04, 23–25, 27, 29, 33. |
| Salidas | Data inventory, decisiones de telemetría y requisitos de privacidad. |
| Límite de uso | No constituye asesoramiento legal ni declara telemetría implementada. |

## 7.28. Documento 27 — [27_Security_and_Incident_Response_Plan.md](27_Security_and_Incident_Response_Plan.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Seguridad e incidentes |
| Clase | Plan de seguridad |
| Owner | Security / Engineering |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Define secretos, dependencias, vulnerabilidades, accesos, detección, respuesta, recuperación y comunicación de incidentes. |
| Autoridad | Prevalece para controles de seguridad y respuesta, coordinado con 26, 33 y 34. |
| Cuándo consultarlo | Al gestionar credenciales, dependencias, repositorios, builds, proveedores o un incidente. |
| Cuándo actualizarlo | Ante cambios de amenazas, herramientas, cuentas, dependencias, incidentes o controles. |
| Entradas | 03–04, 10–11, 23–26, 33. |
| Salidas | Controles, playbooks y evidencias para release y continuidad. |
| Límite de uso | Controles definidos no equivalen a controles verificados. |

## 7.29. Documento 28 — [28_Marketing_and_Communication_Plan.md](28_Marketing_and_Communication_Plan.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Marketing y comunicación |
| Clase | Plan comercial y de comunicación |
| Owner | Marketing / Product |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Consolida posicionamiento, audiencias, mensajes, canales, contenidos, prensa, creadores, comunidad y fases de campaña. |
| Autoridad | Gobierna comunicación autorizada, subordinada a producto, legal, Steam y gates de evidencia. |
| Cuándo consultarlo | Antes de comunicar públicamente, crear store media, contactar prensa/creadores o planificar campañas. |
| Cuándo actualizarlo | Ante cambios de naming, posicionamiento, build representativa, claims, canales, presupuesto o calendario. |
| Entradas | 00–02, 17–19, 22–27, 29–31. |
| Salidas | Claims, content pipeline, campañas y criterios de autorización. |
| Límite de uso | No abre campaña ni autoriza claims mientras gates sigan pendientes. |

## 7.30. Documento 29 — [29_Accessibility_and_Inclusive_Design_Plan.md](29_Accessibility_and_Inclusive_Design_Plan.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Accesibilidad y diseño inclusivo |
| Clase | Plan transversal |
| Owner | UX / Engineering / QA |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Centraliza barreras visuales, auditivas, motoras y cognitivas, settings, input, subtítulos, lenguaje, QA y claims. |
| Autoridad | Prevalece para requisitos de accesibilidad junto con UX/UI, sin afirmar conformidad no probada. |
| Cuándo consultarlo | Al diseñar UI, input, audio, VFX, localización, tutoriales, Steam features o playtests. |
| Cuándo actualizarlo | Ante nueva capacidad, barrera, opción, claim, prueba o feedback inclusivo. |
| Entradas | 01, 05, 07–10, 18–19, 22, 24, 26, 30–31. |
| Salidas | Work packages, acceptance criteria y matriz de claims. |
| Límite de uso | Distingue requisito, implementación, validación y declaración pública. |

## 7.31. Documento 30 — [30_Performance_and_Optimization_Plan.md](30_Performance_and_Optimization_Plan.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Rendimiento y optimización |
| Clase | Plan técnico transversal |
| Owner | Engineering / QA / Technical Art |
| Estado RC1 | READY — sin acción dirigida, salvo correcciones propagadas |
| Función | Define presupuestos, hardware, escenarios, profiling, CPU/GPU, memoria, GC, carga, assets, audio y gates. |
| Autoridad | Gobierna objetivos y metodología de rendimiento, subordinado a evidencia medida en Player. |
| Cuándo consultarlo | Antes de optimizar, configurar URP, integrar contenido, fijar hardware o aprobar RC. |
| Cuándo actualizarlo | Ante cambios de target, hardware, configuración, escenas, presupuestos o resultados de benchmark. |
| Entradas | 02–03, 07–11, 17–19, 22, 29, 31. |
| Salidas | Benchmarks, budgets, deuda y gates de performance. |
| Límite de uso | Los objetivos 60 FPS/1080p y tiempos son pendientes de validación hasta benchmark. |

## 7.32. Documento 31 — [31_H6_and_Release_Readiness_Checklist.xlsx](31_H6_and_Release_Readiness_Checklist.xlsx)

| Campo | Descripción |
| --- | --- |
| Dominio | H6 y Release Readiness |
| Clase | Checklist operativo / signoff |
| Owner | QA / Production |
| Estado RC1 | TARGETED UPDATE PENDING |
| Función | Convierte H6, Steam, RC y launch en controles ejecutables, evidencias, builds, defectos, excepciones y firmas. |
| Autoridad | Es la fuente operativa para el estado calculado y formal de los gates, junto con el contrato 02. |
| Cuándo consultarlo | Al preparar, ejecutar o firmar Sprint 16/17, H6, premarketing, Steam, demo, RC o launch. |
| Cuándo actualizarlo | Después de cada ejecución, evidencia, build, defecto, excepción, auditoría o cambio de documento. |
| Entradas | 02, 07–08, 11–13, 17–30, 32–34. |
| Salidas | Readiness, blockers y decision records. |
| Límite de uso | Todo comienza NOT RUN; no se deben pintar PASS sin evidencia. |

## 7.33. Documento 32 — [32_Documentation_Baseline_Manifest.xlsx](32_Documentation_Baseline_Manifest.xlsx)

| Campo | Descripción |
| --- | --- |
| Dominio | Manifiesto de baseline |
| Clase | Integridad documental |
| Owner | Documentation / QA |
| Estado RC1 | CANDIDATE — regenerar antes y después de auditoría |
| Función | Inventaría 00–34, artefactos del paquete, hashes, tamaños, autoridad, relaciones, linaje, duplicados y controles de cierre. |
| Autoridad | Prevalece para composición e integridad del paquete documental congelado. |
| Cuándo consultarlo | Al comparar baselines, verificar archivos, generar checksums, preservar historia o congelar el ZIP. |
| Cuándo actualizarlo | Casi al final: versión candidata antes de auditoría y definitiva después de todas las correcciones. |
| Entradas | 00–34, inventario histórico, cambios y hashes. |
| Salidas | Manifest y datos para PACKAGE_MANIFEST/CHECKSUMS. |
| Límite de uso | Su hash propio debe registrarse externamente y la versión actual es candidata. |

## 7.34. Documento 33 — [33_Project_Risk_Register_and_Business_Continuity_Plan.xlsx](33_Project_Risk_Register_and_Business_Continuity_Plan.xlsx)

| Campo | Descripción |
| --- | --- |
| Dominio | Riesgos y continuidad |
| Clase | Registro operativo / BCP |
| Owner | Risk Owners / Production |
| Estado RC1 | TARGETED UPDATE PENDING |
| Función | Mantiene riesgos, tratamientos, BIA, RTO/RPO, backups, dependencias, playbooks, runbooks, ejercicios e incidentes. |
| Autoridad | Es la fuente operativa para exposición y continuidad, no una afirmación de que los controles funcionen. |
| Cuándo consultarlo | En revisión de riesgos, planificación, incidentes, restore tests, pérdida de herramientas o preparación de release. |
| Cuándo actualizarlo | Ante cambios de riesgo, control, owner, evidencia, proveedor, incidente, ejercicio o auditoría. |
| Entradas | 00–32 y genealogía histórica de riesgos. |
| Salidas | Tratamientos, recovery capabilities y blockers para H6/release. |
| Límite de uso | El estado inicial es conservador; residual y RTO/RPO requieren prueba. |

## 7.35. Documento 34 — [34_Final_Handoff_and_Project_Operations_Manual.md](34_Final_Handoff_and_Project_Operations_Manual.md)

| Campo | Descripción |
| --- | --- |
| Dominio | Handoff y operaciones |
| Clase | Manual operativo final |
| Owner | Production / Documentation |
| Estado RC1 | TARGETED UPDATE PENDING |
| Función | Permite arrancar, retomar, transferir, operar, recuperar y cerrar el proyecto sin depender del historial del chat. |
| Autoridad | Gobierna procedimientos de operación y handoff, subordinado a las autoridades de cada disciplina. |
| Cuándo consultarlo | Al iniciar jornada, incorporar o retirar colaboradores, retomar tras pausa, transferir el proyecto o recuperar una pérdida. |
| Cuándo actualizarlo | Después de cerrar gobernanza, baseline, commits/tags y cuando cambie el proceso operativo. |
| Entradas | 00–33, handoffs históricos, proyecto Unity y registros de sprint. |
| Salidas | Onboarding, runbooks, checklists y paquete de transferencia. |
| Límite de uso | Requiere actualización dirigida con hashes, commit/tag y baseline definitivos. |

# 8. Mapas de relación y cadenas de evidencia

## 8.1. Cadena producto → implementación → aceptación

```text
00 Enfoque
  ↓
01 GDD + 05 UX + especialidad 17–30
  ↓
02 VSS / criterio contractual
  ↓
03–04 arquitectura y datos + ADR
  ↓
12 tarea / owner / dependencia
  ↓
Código / escena / asset / configuración
  ↓
07–08 prueba + 11 build
  ↓
13 trazabilidad + evidencia
  ↓
31 gate + signoff
```

## 8.2. Cadena de publicación

`H6 PASS → decisión de abrir Steam → 23 legal → 24 publishing → 28 marketing → 22 localización → 29 accesibilidad → 30 rendimiento → 31 Coming Soon/Demo/RC/Launch → 25 postlaunch`. Saltarse una flecha debe registrarse como excepción formal; no puede resolverse con un resumen verbal.

## 8.3. Cadena de incidentes y continuidad

`27 detección/respuesta → 33 riesgo/BIA/playbook/runbook → 26 privacidad cuando haya datos → 23 obligaciones → 25 soporte/comunicación → 34 recuperación y handoff → 13 registro del cambio/incidente`.

## 8.4. Cadena de cierre documental

`Freeze RC1 → 12 → 13 → 14 → 15 → 21/23/31/33/34 → 32 candidata → 16 auditoría → correcciones → 16 final → 32 final → manifests/checksums/report → ZIP → restore test → commit/tag`.

## 8.5. Relaciones registradas

El Excel 13 conserva 424 relaciones documentales normalizadas. Este Binder ofrece rutas de alto nivel; no replica todas las filas para evitar que una copia derivada compita con la matriz operativa. Cuando una modificación sea transversal, se consulta `19_Relaciones_Doc` y se abre impacto en 13.

# 9. Mapa de sistemas y autoridades

| Sistema / área | Autoridad de producto | Autoridad técnica / datos | Operación / QA | Especialidad |
| --- | --- | --- | --- | --- |
| Bootstrap y escenas | 01, 02, 05 | 03, 10 | 08, 11–13, 31 | 17, 19, 30 |
| Movimiento, cámara e input | 01, 05 | 03, 09 | 08, 12–13, 31 | 19, 29, 30 |
| Grid y placement | 01, 02 | 03, 04 | 08, 12–13 | 17, 21, 30 |
| Inventario y proveedores | 01, 20 | 03, 04 | 08, 12–13 | 21, 23 |
| Clientes y checkout | 01, 02, 05, 20 | 03, 04 | 08, 12–13, 31 | 17–19, 29–30 |
| Jornada y economía | 01, 20 | 03, 04 | 08, 12–13 | 21, 26 |
| Persistencia y recovery | 02 | 03, 04, 11 | 08, 13, 31, 33 | 26, 27, 34 |
| UI, settings y tutorial | 01, 05 | 03, 04, 09 | 08, 12–13, 31 | 19, 22, 29 |
| StoreInitial | 00–02, 05 | 03, 10 | 06–08, 12–13, 31 | 17–19, 21, 29–30 |
| Steam y lanzamiento | 00, 02 | 11 | 12–13, 31 | 22–28, 33–34 |

# 10. Sprints, hitos y gates

## 10.1. Estado de sprints

| Periodo | Estado | Interpretación |
| --- | --- | --- |
| Sprints 0–15 | CLOSED / PASS | Se hereda la baseline funcional y sus ADR; no se reabre sin cambio formal. |
| Sprint 16 | COMPLETED / PASS | Integración representativa de arte/audio/StoreInitial y build `0.0.21` aprobadas. |
| Sprint 17 | PENDING / READY TO OPEN | La condición de entrada de S16 está satisfecha; falta kickoff formal. |
| Post-H6 | NOT OPEN | Steam, campañas y release requieren decisiones separadas. |

## 10.2. Gates principales

| Gate | Documento contractual | Control operativo | Estado |
| --- | --- | --- | --- |
| Sprint 16 close | 06 / ADR-0067 | 12, 31 | COMPLETED / PASS |
| Sprint 17 open/close | 06 | 12, 31 | PENDING / READY TO OPEN |
| H6 | 02 | 31 | BLOCKED / NOT RUN |
| Premarketing | 28 | 31 | PENDING |
| Steam phase | 24 | 31 | PENDING |
| Coming Soon | 24, 28 | 31 | PENDING |
| Demo / Playtest | 24, 28 | 31 | PENDING |
| Release Candidate | 11, 24, 30 | 31 | BLOCKED |
| Launch | 24–28, 33–34 | 31 | BLOCKED |
| Final documentation baseline | 13–16, 32, 34 | 31/32 | IN PROGRESS |

## 10.3. Registro de cierre de Sprint 16

El `2026-07-06`, VRM Games registró aprobación visual y funcional de StoreInitial, build externa `0.0.21`, Golden Path, persistencia, suites automatizadas, regresión manual y Player.log en PASS. El cierre desbloquea la apertura formal de Sprint 17, pero no modifica el estado de H6.

## 10.4. Condiciones mínimas de H6

- Sprint 16 y 17 formalmente cerrados.
- Build candidata inmutable y reproducible.
- Golden Path completo y persistente.
- Cero S0/S1 o excepciones formales permitidas.
- Controles blocking de 31 cerrados.
- Paquetes de evidencia verificados.
- Rendimiento, accesibilidad, legal y seguridad evaluados en el alcance aplicable.
- Signoff del owner y revisores requeridos.

# 11. Índice de QA, evidencias, builds y defectos

## 11.1. Distribución de responsabilidades

| Fuente | Responsabilidad |
| --- | --- |
| 07 | Política y estrategia QA |
| 08 | Casos, ejecuciones, defectos y cobertura del producto |
| 11 | Build/versionado/integridad |
| 13 | Cadena de cambio y evidencia |
| 31 | Checklist de gates, evidencias, excepciones y signoff |
| 16 | Coherencia global del paquete |
| 33 | Pruebas de restore y continuidad |

## 11.2. Estados que no deben confundirse

| Estado | Significado |
| --- | --- |
| NOT RUN | No existe ejecución registrada. |
| PASS | Resultado verificable para el escenario y build indicados. |
| FAIL | Criterio incumplido. |
| BLOCKED | No puede ejecutarse por dependencia. |
| EXCEPTION APPROVED | Desviación aceptada con owner, alcance y expiración. |
| NOT APPLICABLE | Exclusión justificada, nunca celda vacía. |
| MISSING evidence | El control puede estar implementado, pero falta el paquete probatorio. |

## 11.3. Política de build

Una evidencia de gate debe identificar build ID, versión, commit/tag, Unity, perfil, escenas/contenido, fecha, autor, clean checkout, tests, revisión de Player.log, manifest, hash y ruta. La palabra “build” sin procedencia no es evidencia suficiente.

# 12. Índice de producción y planificación

## 12.1. Excel Maestro 12

El documento 12 es la fuente operativa para backlog, sprints, work packages, consolidación, dependencias, riesgos, gates, builds, ADR, métricas y trazabilidad. Su dashboard muestra el estado actual, pero el detalle de cada disciplina sigue residiendo en el documento especializado.

## 12.2. Uso correcto de métricas

- Un porcentaje de completion no autoriza un gate.
- Done describe el estado declarado del item; QA y evidencia se revisan por separado.
- Policy Active no es trabajo implementado.
- Planned no es compromiso de fecha.
- Blocked requiere dependencia, owner y siguiente acción.
- Los work packages extraídos de planes no se cierran por haber generado el plan.

## 12.3. Capacidad y estudio unipersonal

Cuando una persona acumule roles, deben mantenerse separados los sombreros de solicitante, implementador, revisor y aprobador en el registro, aunque el nombre sea el mismo. La separación de decisión evita que una autoaprobación implícita borre el criterio de gate.

# 13. Índice de trazabilidad y control de cambios

## 13.1. Cadena mínima

Todo cambio material debe poder recorrerse como `requisito → change ID → impacto → ADR/decisión → tarea/implementación → prueba → build → evidencia → baseline`. Los huecos pueden ser legítimos, pero deben estar justificados y auditados.

## 13.2. Población actual del libro 13

| Registro | Cantidad |
| --- | --- |
| Requisitos únicos | 708 |
| Trazabilidades end-to-end | 708 |
| Cambios | 131 |
| Impactos documentales | 163 |
| Relaciones documentales | 424 |
| Controles H6/release | 475 |
| Riesgos / acciones | 180 / 180 |
| Pruebas y escenarios | 665 |
| Fuentes históricas | 745 |

## 13.3. Flujo de cambio

1. Abrir change ID y describir motivo.
2. Identificar requisitos/documentos/sistemas afectados.
3. Evaluar alcance, datos, QA, build, legal, seguridad, rendimiento y comunicación.
4. Crear ADR cuando la decisión sea duradera o arquitectónica.
5. Planificar en 12 y ejecutar.
6. Registrar commit/artefacto/build.
7. Ejecutar pruebas y adjuntar evidencia.
8. Cerrar impactos y actualizar versiones/hashes.
9. Auditar propagación.

# 14. Riesgo, privacidad, seguridad y continuidad

## 14.1. Modelo coordinado

26 define qué datos pueden tratarse; 27 define cómo proteger sistemas y responder a incidentes; 33 convierte riesgos y continuidad en registros ejecutables; 34 explica cómo operar y recuperar; 13 conserva la decisión y la evidencia. Legal 23 interviene cuando existen obligaciones, licencias, marcas, contratos o notificaciones.

## 14.2. Estado conservador

Los 180 riesgos iniciales de 33 comienzan abiertos y con controles no verificados. Las puntuaciones residuales no deben reducirse hasta que owner y evidencia demuestren madurez. Del mismo modo, RTO/RPO son objetivos de BIA hasta superar un ejercicio de recuperación.

## 14.3. Continuidad mínima antes de freeze final

- Copia de repositorio y documentación en ubicaciones independientes.
- Backups de cuentas/credenciales mediante mecanismo seguro y recuperación probada.
- Restore test de documentación y proyecto en entorno limpio.
- Propietario alternativo o procedimiento de recuperación para dependencias críticas.
- Hashes y manifest verificados después de extraer el ZIP.
- Registro de ejercicio, desviaciones y acciones correctivas.

# 15. Publicación, marketing, comunidad y postlanzamiento

## 15.1. Separación de fases

Portfolio técnico interno, premarketing autorizado, página Coming Soon, demo/Playtest, RC y lanzamiento son gates distintos. H6 no abre automáticamente Steam y una build representativa no autoriza claims. Cada fase requiere alcance, owner, capacidad, legal, materiales, soporte, rollback y evidencia.

## 15.2. Claims

Todo claim sobre plataformas, idiomas, accesibilidad, rendimiento, contenido, fecha, precio, soporte o características debe enlazarse con una fuente, una validación y un owner. Marketing 28 mantiene la casa de mensajes; 31 verifica readiness; 23 y 24 validan legal/Steam.

## 15.3. Naming provisional

Cartridge & Cloud y VRM Games se tratan como nombres de trabajo sujetos a clearance y registro. Ningún material público debe implicar protección o disponibilidad definitiva sin la comprobación correspondiente en 23.

# 16. Consolidación y cierre de la baseline

## 16.1. Secuencia exacta

| ID | Acción | Estado |
| --- | --- | --- |
| CNS-001 | Congelar copia candidata 00–34 | DONE |
| CNS-002 | Regenerar 12 | DONE |
| CNS-003 | Regenerar 13 | DONE |
| CNS-004 | Actualizar 14 | DONE al adoptar este archivo |
| CNS-005 | Regenerar 15 | NEXT |
| CNS-006 | Actualizar 21 | PENDING |
| CNS-007 | Actualizar 23 | PENDING |
| CNS-008 | Actualizar 31 | PENDING |
| CNS-009 | Actualizar 33 | PENDING |
| CNS-010 | Actualizar 34 | PENDING |
| CNS-011 | Generar 32 candidata | PENDING |
| CNS-012 | Regenerar 16 auditoría candidata | PENDING |
| CNS-013 | Resolver findings | PENDING |
| CNS-014 | Regenerar 16 final | PENDING |
| CNS-015 | Regenerar 32 final | PENDING |
| CNS-016 | Generar PACKAGE_MANIFEST | PENDING |
| CNS-017 | Generar CHECKSUMS_SHA256 | PENDING |
| CNS-018 | Generar validation report | PENDING |
| CNS-019 | Crear ZIP inmutable | PENDING |
| CNS-020 | Verificar restore/extracción | PENDING |
| CNS-021 | Asociar commit y tag | PENDING |
| CNS-022 | Activar H6 y validar continuidad | PENDING |

## 16.2. Reglas de cierre

- No recalcular hashes definitivos antes de que el archivo deje de cambiar.
- No cerrar 32 antes de la auditoría y correcciones.
- No cerrar 16 con findings BLOCKER/HIGH sin resolución o excepción formal.
- No crear el ZIP desde una carpeta de trabajo contaminada.
- No asociar tag hasta verificar extracción y hashes.
- No iniciar H6 desde una versión de 31 con referencias obsoletas.

# 17. Genealogía, ADR y preservación histórica

## 17.1. Baselines históricas

| Versión | Momento | Aporte preservado |
| --- | --- | --- |
| v0.3 | Reinicio de preproducción | Inventario inicial, Enfoque, planes amplios y regla de no asumir implementación. |
| v0.4 | Foundation / Sprint 0 | Separación entre especificación, evidencia y cierre de foundation. |
| v0.5 | Sprints 0–5 | Jerarquía, handoff, baselines y primeros sistemas funcionales. |
| v0.6 | Sprints 0–15 + S16 parcial | Estado técnico actual, ADR, StoreInitial y registros operativos. |
| 1.0 / RC1 | Documentos 00–34 | Reconstrucción completa y consolidación en curso. |

## 17.2. Inventario histórico

El manifiesto 32 registra 745 archivos suministrados, 643 hashes únicos y 98 grupos de duplicados. La duplicación por hash no autoriza borrado automático: una copia puede demostrar publicación en una baseline, otra autoría Markdown y otra uso operativo en un expediente de sprint.

## 17.3. Principios preservados

- No reescribir silenciosamente baselines.
- Distinguir objetivo, implementación, validación y aceptación.
- Mantener ADR y evidencia junto a la decisión.
- Separar registros vivos de paquetes inmutables.
- Exigir evidencia más fuerte en gates de fase.
- Preservar el contexto de StoreInitial y la transición desde Store.

## 17.4. Índice de ADR preservado

La fuente auditada contiene **74 decisiones ADR únicas** en el registro consolidado, incluyendo el alias `ADR-0035-S16` para evitar la colisión histórica con la decisión de Sprint 9.

| ADR | Sprint | Decisión | Estado |
| --- | --- | --- | --- |
| ADR-0001 | 0 | ADR-0001 — Unity and Package Baseline | Accepted |
| ADR-0002 | 0 | ADR-0002 — Version Control and Repository Workflow | Accepted |
| ADR-0003 | 0 | ADR-0003 — Documentation Baseline and Operational Layer | Accepted |
| ADR-0004 | 0 | ADR-0004 — Assembly Dependency Boundaries | Accepted |
| ADR-0005 | 0 | ADR-0005 — Scene Baseline and Global Build Order | Accepted |
| ADR-0006 | 0 | ADR-0006 — Smoke Test Baseline and Execution Policy | Accepted |
| ADR-0007 | 0 | ADR-0007 — Windows Development Build Baseline | Accepted |
| ADR-0008 | 0 | ADR-0008 — Sprint 0 Foundation Closure Policy | Accepted |
| ADR-0009 | 1 | ADR-0009 — Bootstrap-Owned Scene Flow | Accepted |
| ADR-0010 | 1 | ADR-0010 — Phase-Level Build Evidence and Release Policy | Accepted |
| ADR-0011 | 2 | ADR-0011 — Minimal Versioned Save Skeleton | Accepted |
| ADR-0012 | 3 | ADR-0012 — Scene-Driven Input Contexts | Accepted |
| ADR-0013 | 3 | ADR-0013 — Direct Planar Click-to-Move Foundation | Accepted |
| ADR-0014 | 3 | ADR-0014 — Orbit, Zoom and Follow Camera | Accepted |
| ADR-0015 | 3 | ADR-0015 — Project Input Actions and Context Routing | Accepted |
| ADR-0016 | 4 | ADR-0016 — Grid Coordinate and Footprint Foundation | Accepted |
| ADR-0017 | 4 | ADR-0017 — Placement Preview and Rotation | Accepted |
| ADR-0018 | 4 | ADR-0018 — Atomic Occupancy and Placement Mode | Accepted |
| ADR-0019 | 4 | ADR-0019 — Sprint 04 Final Integration and Build Gate | Accepted |
| ADR-0020 | 5 | ADR-0020 — Initial Store Shell Dimensions and Entrance | Accepted |
| ADR-0021 | 5 | ADR-0021 — Logical Store Access Validation | Accepted |
| ADR-0022 | 5 | ADR-0022 — Store Placement and Access Integration | Accepted |
| ADR-0023 | 5 | ADR-0023 — Sprint 05 Final Integration and Build Gate | Accepted |
| ADR-0024 | 6 | ADR-0024 — Stack and Unit-Capacity Model | Accepted |
| ADR-0025 | 6 | ADR-0025 — Atomic Inventory Transfers | Accepted |
| ADR-0026 | 6 | ADR-0026 — Sprint 6 Version and Persistence Boundary | Accepted |
| ADR-0027 | 7 | ADR-0027 — Product and Supplier Authoring with ScriptableObjects | Accepted |
| ADR-0028 | 7 | ADR-0028 — Purchase Orders Use Whole Shipment Boxes | Accepted |
| ADR-0029 | 7 | ADR-0029 — Shipment-Box Receiving Is Atomic | Accepted |
| ADR-0030 | 8 | ADR-0030 — Single-product display assignment | Accepted |
| ADR-0031 | 8 | ADR-0031 — Display capacity and visible units | Accepted |
| ADR-0032 | 8 | ADR-0032 — Atomic manual restocking | Accepted |
| ADR-0033 | 8 | ADR-0033 — Display authoring and placement boundary | Accepted |
| ADR-0034 | 9 | ADR-0034 — Selección determinista y cola de spawn | Accepted |
| ADR-0035 | 9 | ADR-0035 — Ciclo de vida y paciencia | Accepted |
| ADR-0036 | 9 | ADR-0036 — Límite de navegación técnica | Accepted |
| ADR-0037 | 9 | ADR-0037 — Límite de población y representación técnica | Accepted |
| ADR-0038 | 10 | ADR-0038 — Reservation-backed cart | Accepted |
| ADR-0039 | 10 | ADR-0039 — Deterministic shopping search | Accepted |
| ADR-0040 | 10 | ADR-0040 — Reservation provenance | Accepted |
| ADR-0041 | 10 | ADR-0041 — Customer shopping session boundary | Accepted |
| ADR-0042 | 11 | ADR-0042 — Strict FIFO checkout queue | Accepted |
| ADR-0043 | 11 | ADR-0043 — Single-entry checkout station | Accepted |
| ADR-0044 | 11 | ADR-0044 — Preflight then commit checkout | Accepted |
| ADR-0045 | 11 | ADR-0045 — Checkout idempotency | Accepted |
| ADR-0046 | 12 | ADR-0046 — Logical Store Day aggregate | Accepted |
| ADR-0047 | 12 | ADR-0047 — Closing admission gates | Accepted |
| ADR-0048 | 12 | ADR-0048 — Deterministic drain and resolution | Accepted |
| ADR-0049 | 12 | ADR-0049 — Closure readiness snapshot | Accepted |
| ADR-0050 | 12 | ADR-0050 — Technical day summary | Accepted |
| ADR-0051 | 13 | ADR-0051 — Integer minor-unit money | Accepted |
| ADR-0052 | 13 | ADR-0052 — Separate sale-price catalog | Accepted |
| ADR-0053 | 13 | ADR-0053 — Quote before physical checkout | Accepted |
| ADR-0054 | 13 | ADR-0054 — Idempotent economy ledger | Accepted |
| ADR-0055 | 13 | ADR-0055 — Closed-day economic result | Accepted |
| ADR-0056 | 14 | ADR-0056 — Compatible integrated save v2 | Accepted |
| ADR-0057 | 14 | ADR-0057 — Validated atomic write | Accepted |
| ADR-0058 | 14 | ADR-0058 — Checksum and generation envelope | Accepted |
| ADR-0059 | 14 | ADR-0059 — Backup-first recovery | Accepted |
| ADR-0060 | 14 | ADR-0060 — Two-phase restore | Accepted |
| ADR-0061 | 15 | ADR-0061 — Runtime UI Composition Root | Accepted |
| ADR-0062 | 15 | ADR-0062 — Slot UI uses integrated repository | Accepted |
| ADR-0063 | 15 | ADR-0063 — Closed-day autosave idempotency | Accepted |
| ADR-0064 | 15 | ADR-0064 — Tutorial progress sidecar | Accepted |
| ADR-0065 | 15 | ADR-0065 — Global accessibility preferences | Accepted |
| ADR-0066 | 15 | ADR-0066 — Exclusive UI input | Accepted |
| ADR-0035-S16 | 16 | ADR-0035 - StoreInitial Manual Scene Authoring | Accepted |
| ADR-0067 | 16 | ADR-0067 — Sprint 16 two-phase delivery | Accepted |
| ADR-0068 | 16 | ADR-0068 — Pure Phase 1 sidecar | Accepted |
| ADR-0069 | 16 | ADR-0069 — Autosave-coordinated checkpoint | Accepted |
| ADR-0070 | 16 | ADR-0070 — Placement compatibility bridge | Accepted |
| ADR-0071 | 16 | ADR-0071 — Decoupled presentation fallbacks | Accepted |
| ADR-0072 | 16 | ADR-0072 — Shared materials and prefab IDs | Accepted |
| ADR-0073 | 16 | ADR-0073 — Completed checkout snapshot records | Accepted |

### 17.4.1. Política de identificadores ADR

A partir de esta baseline no se debe reutilizar un número ADR. El registro de Trazabilidad es la fuente para reservar el siguiente identificador. El alias `ADR-0035-S16` se mantiene como compatibilidad documental; una futura normalización física del archivo requerirá cambio trazado para no romper referencias.

### 17.4.2. Cuándo crear ADR

- cambio de arquitectura o dependencias entre capas;
- cambio de schema, persistencia o estrategia de recuperación;
- cambio de Unity, paquete, plataforma, profile o pipeline;
- cambio significativo de escena, composition root o autoría runtime/editor;
- cambio de alcance o política que afecte múltiples documentos o sprints;
- elección con alternativas relevantes y consecuencias duraderas.

# 18. Convenciones de nombres, versiones, rutas y enlaces

## 18.1. Documentos actuales

El nombre canónico es el nombre numerado exacto de la tabla de la sección 3. No deben crearse variantes como “final”, “final2”, “new” o copias sin versión. Las iteraciones viven en control de versiones y en el registro de cambios, no en nombres ambiguos.

## 18.2. Identificadores

| Tipo | Patrón | Fuente de reserva |
| --- | --- | --- |
| Requisito | VS-AREA-NNN / dominio especializado | 02 / 13 |
| Cambio | CHG-AREA-NNN | 13 |
| ADR | ADR-NNNN | 13 |
| Work package | DOM-WP-NNN | 12 |
| Riesgo | R-CAT-NNN | 33 |
| Acción | TA-CAT-NNN | 33 |
| Evidencia | EVD-H6/REL-NNN | 31 |
| Build | BLD-FASE-NNN | 11 / 31 |
| Baseline | BASE-x.y.z / tag | 13 / 32 |

## 18.3. Enlaces

Los Markdown de la baseline usan enlaces relativos. Rutas absolutas de `/mnt/data`, máquinas locales o carpetas temporales no deben publicarse como navegación canónica. Las rutas históricas se conservan literalmente solo en inventarios y manifests.

# 19. Política de actualización e impacto

| Cambio | Documentos mínimos a revisar |
| --- | --- |
| Visión o scope | 00, 01, 02, 06, 12–16, 28, 31–34 |
| Mecánica o UX | 01, 02, 05, 07–08, 12–13, especialidades y 31 |
| Arquitectura | 03, 09–11, 12–13, ADR, QA, 30, 31, 34 |
| Schema/save | 03–04, 07–08, 11–13, 26–27, 31, 33–34 |
| Asset/contenido | 17–23, 12–13, 28–31 |
| Idioma/texto | 05, 19, 22–24, 28–29, QA y 31 |
| Proveedor/servicio | 10–11, 23, 26–27, 33–34 |
| Build/release | 06–08, 11–13, 23–31, 33–34 |
| Documento/autoridad | 12–16, 31–32, 34 y artefactos del paquete |

## 19.1. Cuándo actualizar este Binder

- Se añade, renombra, retira o sustituye un documento.
- Cambia la jerarquía o autoridad.
- Cambia una ruta de lectura, gate o proceso de baseline.
- Se cierra Sprint 16/17, H6, Steam RC o launch.
- Se publica una nueva baseline.
- La auditoría detecta navegación o relaciones incorrectas.

# 20. Checklists operativos

## 20.1. Antes de iniciar una tarea

- Identificar requisito y fuente competente.
- Comprobar estado y prioridad en 12.
- Consultar cambios/impactos en 13.
- Verificar dependencias, riesgos y gate.
- Confirmar criterios y evidencia esperada.
- Crear ADR si la decisión es duradera.

## 20.2. Antes de cerrar una tarea

- Implementación revisada.
- Prueba afectada ejecutada.
- Build/evidencia identificadas.
- Cambios e impactos actualizados.
- Documentos propagados.
- Riesgos y deuda actualizados.
- No quedan claims o estados inflados.

## 20.3. Antes de cerrar un sprint o gate

- Scope y entrada válidos.
- Cero blockers no resueltos.
- Regresión y Golden Path ejecutados.
- Build candidata con integridad.
- Evidencias verificadas.
- Deuda/excepciones aprobadas.
- Signoff registrado.
- Handoff y siguiente fase actualizados.

## 20.4. Antes de congelar una baseline

- 00–34 presentes y nombres exactos.
- 12–16 y documentos dirigidos actualizados.
- 16 final sin blockers/high abiertos.
- 32 final y hashes externos.
- Manifests y validation report.
- ZIP extraído y verificado en limpio.
- Commit/tag registrados.
- Copia de recuperación probada.

# 21. Reglas para nuevos chats, colaboradores y handoffs

## 21.1. Paquete mínimo de contexto

- Este Binder 14.
- Guía 15 y Manual 34.
- Enfoque 00 y VSS 02.
- Excel 12 y 13.
- Documento especializado de la tarea.
- Checklist 31 si afecta gate.
- Proyecto/commit/build exactos si afecta implementación.

## 21.2. Preguntas obligatorias antes de actuar

- ¿Qué documento gobierna la decisión?
- ¿Es un requisito, un plan, un estado o una evidencia?
- ¿Qué versión/commit/build se está usando?
- ¿Qué cambio e impacto deben registrarse?
- ¿Qué gate o riesgo puede bloquear?
- ¿Qué prueba y evidencia demostrarán el cierre?

## 21.3. Conductas prohibidas

- Asumir que “último” significa vigente sin comprobar autoridad.
- Modificar scope para facilitar implementación.
- Marcar PASS sin evidencia.
- Borrar históricos o duplicados por parecer redundantes.
- Publicar claims desde una especificación no validada.
- Crear una nueva fuente de verdad paralela.

## 21.4. Plantilla breve de handoff

```text
Objetivo y alcance:
Fuente normativa:
Estado en 12 / cambio en 13:
Implementación o artefactos modificados:
Pruebas y evidencia:
Build / commit / hash:
Riesgos, deuda y excepciones:
Siguiente acción exacta:
Documentos que deben actualizarse:
```

# 22. Glosario

| Término | Significado |
| --- | --- |
| ADR | Architecture Decision Record; decisión duradera con contexto y consecuencias. |
| Baseline | Conjunto identificado y controlado de artefactos. |
| Candidate | Versión sometida a validación antes de freeze. |
| Evidence | Artefacto verificable asociado a requisito, build y resultado. |
| Gate | Decisión de entrada/salida con condiciones y signoff. |
| Golden Path | Recorrido contractual principal del slice. |
| H6 | Aprobación del vertical slice representativo. |
| MTPD | Máximo periodo tolerable de interrupción. |
| RPO | Pérdida de datos máxima tolerada medida en tiempo. |
| RTO | Tiempo objetivo de recuperación. |
| RC | Release Candidate exacta, no una build aproximada. |
| Store | Escena funcional histórica. |
| StoreInitial | Escena representativa autorada/integrada en Sprint 16. |
| Working copy | Estado local, potencialmente distinto del commit publicado. |
| Claim | Afirmación pública que requiere fuente, validación y autorización. |

# 23. Inventario técnico de la baseline candidata

| ID | Archivo | Tamaño | Líneas | Palabras | Encabezados | SHA-256 actual |
| --- | --- | --- | --- | --- | --- | --- |
| 00 | 00_Enfoque_y_Alcance.md | 124.42 KiB | 4495 | 15880 | 284 | `63dd6f0f1b9c2c80be2aec55718d18d9d031ce4acb14f677769d6e69ceaad21f` |
| 01 | 01_Game_Design_Document.md | 129.71 KiB | 4490 | 16583 | 296 | `a9cd9e3203abc5269c25c59dc7f626b0771df4ec2d65e781109ff146bceb2177` |
| 02 | 02_Vertical_Slice_Specification.md | 63.02 KiB | 1505 | 8451 | 90 | `75893f130116e88a08b8da5590dd81876a234581081eafaa8d08374c1a60513f` |
| 03 | 03_Technical_Design_Document.md | 99.60 KiB | 3305 | 12463 | 285 | `66264703c5ff4959d03093690e9845a98ec0e5025e685b9a639ac8cbb616cd12` |
| 04 | 04_Modelo_de_Datos.md | 100.54 KiB | 2691 | 10630 | 201 | `d851a72b55742c2c704cc7c002929bc5894e7142511c6af843440bb193fb19d4` |
| 05 | 05_UX_Flow.md | 55.47 KiB | 2458 | 7071 | 195 | `e08da5ff67fb073a8b950babe325ae58ddb2e121513dc2f553acd4f2c14b867e` |
| 06 | 06_Production_Roadmap_y_Sprint_Plan.md | 60.12 KiB | 2408 | 7590 | 205 | `d00b156c0ad1c66069278119c55f76c738a577a3e2835f770208d1a5f2faadff` |
| 07 | 07_QA_Testing_Plan.md | 77.57 KiB | 3324 | 9604 | 278 | `bc9c05a3737e345546ef16e5390e034295ef35ad66c31647852aec81b4e7affe` |
| 08 | 08_QA_Testing_Matrix.xlsx | 180.70 KiB | — | — | — | `233e57f01d2468fe13c136c8fcd3ee75b39bd3de6530349626ea98d4ff254214` |
| 09 | 09_CSharp_Coding_Standards.md | 93.09 KiB | 3664 | 11320 | 308 | `3b734d6cd49f31c09c10bb4941625e143f6c634e3fb50c157f0720e73b1c2a68` |
| 10 | 10_Unity_Project_Setup_Guide.md | 76.57 KiB | 2748 | 9656 | 265 | `31430bb0544edd9577323aa0009d6ca3df8b9fbcf6c8a150fc33df69b9f963a4` |
| 11 | 11_Build_y_Versioning_Guide.md | 83.32 KiB | 3156 | 10803 | 281 | `38a16a0f97505c0b405509dd3c7e3d1b50e77b89fbe3f4cb1931d3e04f602215` |
| 12 | 12_Excel_Maestro_de_Produccion.xlsx | 473.57 KiB | — | — | — | `3d70e97326868389dac977934e6000285c22b3c33c0e62be31313880726b4d7d` |
| 13 | 13_Trazabilidad_y_Control_de_Cambios.xlsx | 564.02 KiB | — | — | — | `97cf106d7707de9e6dcadfa2ec9374676b0ea9fe0945ac87eb89db788df8da43` |
| 14 | 14_Project_Binder_Indice_Maestro.md | — | — | — | — | `EXTERNAL_AFTER_EXPORT` |
| 15 | 15_Guia_Maestra.md | 117.11 KiB | 3227 | 16185 | 211 | `879bc1925cccf77e10df6c104302f3afc72f71e8c58dd85ae4f06e84e6f8b624` |
| 16 | 16_Auditoria_Global_de_Coherencia.md | 85.39 KiB | 1152 | 12211 | 101 | `4a62376b8aee623be40957ae0511d2dcab2919947f3abb2cef3502b9bb60863e` |
| 17 | 17_Art_Bible.md | 130.77 KiB | 2033 | 17620 | 112 | `095c6cc42223a972c7faa68435595fc7c22f839fabc62b2038371a0ef6c14239` |
| 18 | 18_Audio_Bible.md | 141.14 KiB | 1098 | 19651 | 162 | `c2eabe93a7c26f70e1d4d77fb0ddcaa97e2cdd1e70247e637b9db5bd2f07ce92` |
| 19 | 19_UI_Style_Guide.md | 146.29 KiB | 1722 | 19758 | 222 | `b34ca4db1eba7a536466b6a9a105d8d92396ade2108aa7dd99fa7d5d4eaebc1a` |
| 20 | 20_Economy_and_Balance_Specification.md | 150.92 KiB | 1185 | 19995 | 203 | `908c70772ed35eecdb05c3036c6901523a4caf2c3e708ca8f049c2a160ef2aac` |
| 21 | 21_Initial_Content_Catalog.xlsx | 170.30 KiB | — | — | — | `9047299f60cfbd368fc4a2c8bee8d0ff4132dc5642363a9a67d1523559eb9ed3` |
| 22 | 22_Localization_Plan.md | 153.64 KiB | 1743 | 20139 | 132 | `184a53b2813bc1327a6913e6e88ea4c81142da1ee4ddeff88f75467c784f1ff0` |
| 23 | 23_Legal_Credits_and_Licenses_Register.xlsx | 454.23 KiB | — | — | — | `f543b3b5de8900f405910fac5d72697045f8c4969d98fce4c1cf4f12e532b2f1` |
| 24 | 24_Steam_Publishing_Plan.md | 167.03 KiB | 3199 | 20653 | 174 | `f087e24d3a18f8c4a6651728d5f70d1df804042587e206fdeac2e7e637a87646` |
| 25 | 25_Post_Launch_and_Live_Operations_Plan.md | 319.55 KiB | 3967 | 42786 | 179 | `158e1e43514c5d5d6c365f6cd1a73929f54c4a48c5cc460b593530ae5e49bac9` |
| 26 | 26_Privacy_Data_and_Telemetry_Plan.md | 313.65 KiB | 4253 | 38888 | 186 | `284abfe537793c2ec8b0e5cbac484cd9395567c4434aef7f5ad5ee7005e95f36` |
| 27 | 27_Security_and_Incident_Response_Plan.md | 412.29 KiB | 6577 | 51695 | 1081 | `f925bfbf3df2000323c12a4486480d06c8ab8181684e5922168b8bdf89d05f92` |
| 28 | 28_Marketing_and_Communication_Plan.md | 85.46 KiB | 2514 | 11456 | 277 | `28dd332855126558b3da6019f2e546b1e8ab7db81dc530a89f3e3fbfe757cded` |
| 29 | 29_Accessibility_and_Inclusive_Design_Plan.md | 102.54 KiB | 2853 | 13064 | 264 | `678ceabb509062cd93c7690315397b1cc6204c0da6a5d57324ba11e2fda64333` |
| 30 | 30_Performance_and_Optimization_Plan.md | 106.92 KiB | 3106 | 13627 | 295 | `18f53eaa61734ad735c02c8b7c70ed37f5f0e80f7c01828b05d092c49b0de260` |
| 31 | 31_H6_and_Release_Readiness_Checklist.xlsx | 189.74 KiB | — | — | — | `3ec6265ca8009a6f502008a103161492687efbc026a61c55b1205b976c255aee` |
| 32 | 32_Documentation_Baseline_Manifest.xlsx | 192.63 KiB | — | — | — | `05ef1aa53febb8015b1eb40be8f43a4f6cee5be3ce1d31c3eda03346b86ebc64` |
| 33 | 33_Project_Risk_Register_and_Business_Continuity_Plan.xlsx | 288.19 KiB | — | — | — | `aff3d5173017c6e90eae4b743b00af9a5106ad8e8b61f4789ff6857d10a61d88` |
| 34 | 34_Final_Handoff_and_Project_Operations_Manual.md | 100.29 KiB | 2405 | 11845 | 250 | `3b3fb3519caa4f899da10bf8bec73d72b5e0c3e3c6839c83157d1351a9ef9001` |

## 23.1. Hojas de 08_QA_Testing_Matrix.xlsx

```text
00_Instrucciones
01_Resumen
02_Requisitos
03_Casos_Prueba
04_Golden_Path
05_Persistencia
06_Sistemas
07_UX_Accesibilidad
08_Escena_Contenido
09_Rendimiento
10_Builds
11_Ejecuciones
12_Defectos
13_Cobertura
14_Gates
15_Catalogos
16_Trazabilidad
```

## 23.2. Hojas de 12_Excel_Maestro_de_Produccion.xlsx regenerado

```text
01_Dashboard
00_Instrucciones
02_Hitos
03_Sprints
04_Backlog_Maestro
05_Sprint_16
06_Sprint_17_H6
07_Work_Packages
08_Consolidacion_Doc
09_Dependencias
10_Riesgos
11_Acciones_Riesgo
12_Defectos_Deuda
13_Contenido
14_Documentacion
15_QA_Gates
16_Builds_Releases
17_Decisiones_ADR
18_Recursos_Capacidad
19_Metricas
20_Roadmap_Futuro
21_Trazabilidad
22_Relaciones_Doc
23_Historico
24_Catalogos
25_Control_Cambios
```

## 23.3. Hojas de 13_Trazabilidad_y_Control_de_Cambios.xlsx regenerado

```text
01_Dashboard
00_Instrucciones
02_Requisitos
03_Matriz_Trazabilidad
04_Cambios
05_Impacto_Documental
06_Documentos_00_34
07_Relaciones_Doc
08_Linaje_Documental
09_Sustituciones
10_Consolidacion
11_Work_Packages
12_Controles_H6_Release
13_Riesgos
14_Acciones_Riesgo
15_Decisiones_ADR
16_Commits
17_Builds
18_Pruebas_Evidencia
19_Defectos_Deuda
20_Baselines
21_Control_Alcance
22_Migraciones_Datos
23_Assets_Contenido
24_Paquetes_Evidencia
25_Fuentes_Historicas
26_Auditoria_Controles
27_Catalogos
28_Control_Cambios
```

## 23.4. Hojas de 31_H6_and_Release_Readiness_Checklist.xlsx

```text
01_Dashboard
00_Instrucciones
02_H6_Checklist
03_Release_Checklist
04_Gates_Signoff
05_Evidencias
06_Defectos_Excepciones
07_Builds_Integridad
08_Ejecucion_Pruebas
09_Trazabilidad_Historica
10_Riesgos_Dependencias
11_Catalogos
```

## 23.5. Hojas de 32_Documentation_Baseline_Manifest.xlsx

```text
01_Dashboard
00_Instrucciones
02_Baseline_Actual
03_Artefactos_Paquete
04_Relaciones_Actuales
05_Linaje_Documental
06_Inventario_Historico
07_Duplicados_Integridad
08_Paquetes_Fuente
09_Autoridad_Estados
10_Cierre_Baseline
11_Control_Cambios
12_Catalogos
```

## 23.6. Hojas de 33_Project_Risk_Register_and_Business_Continuity_Plan.xlsx

```text
01_Dashboard
00_Instrucciones
02_Risk_Register
03_Treatment_Actions
04_BIA
05_Continuity_Strategies
06_Backup_Restore
07_Critical_Dependencies
08_Crisis_Playbooks
09_Recovery_Runbooks
10_Exercises_Tests
11_Incident_Log
12_Risk_Acceptance
13_Crisis_Roles
14_Current_Traceability
15_Historical_Traceability
16_Risk_Taxonomy
17_Change_Log
18_Catalogs
19_Scoring_Methodology
```

# 24. Fuentes de consolidación y procedencia

Esta revisión se basa en el conjunto RC1 preconsolidación, los documentos regenerados 12 y 13, los planes 28–30, los libros 31–33, el manual 34 y las fuentes históricas inventariadas en 32. El proyecto Unity previo se utiliza como fotografía técnica, no como sustituto de la documentación aprobada.

| Fuente | SHA-256 / estado |
| --- | --- |
| RC1 PreConsolidation Snapshot ZIP | `c36b66c03d73b6144fb252e2a38d6bbf1c205c46d17ffcce99ebc4ff5f061924` |
| 12 regenerado | `3d70e97326868389dac977934e6000285c22b3c33c0e62be31313880726b4d7d` |
| 13 regenerado | `97cf106d7707de9e6dcadfa2ec9374676b0ea9fe0945ac87eb89db788df8da43` |
| 32 manifiesto candidato | `05ef1aa53febb8015b1eb40be8f43a4f6cee5be3ce1d31c3eda03346b86ebc64` |
| 33 riesgo/continuidad | `aff3d5173017c6e90eae4b743b00af9a5106ad8e8b61f4789ff6857d10a61d88` |
| 34 handoff/operaciones | `3b3fb3519caa4f899da10bf8bec73d72b5e0c3e3c6839c83157d1351a9ef9001` |
| 14 actualizado | EXTERNAL_AFTER_EXPORT |

## 24.1. Evolución del Binder

| Versión | Estado | Aporte |
| --- | --- | --- |
| v0.3 | Histórica | Índice de preproducción y prudencia sobre implementación. |
| v0.4 | Histórica | Foundation y evidencia de Sprint 0. |
| v0.5 | Histórica | Sprints 0–5, handoff y jerarquía. |
| v0.6 | Histórica | Sprints 0–15 y Sprint 16/StoreInitial. |
| 1.0 | Sustituida como candidata | Navegación 00–14 antes de documentos especializados completos. |
| 1.1-RC1 | Current Candidate | Navegación 00–34, consolidación, riesgos, release y operaciones. |

# 25. Historial, próxima revisión y aceptación

## 25.1. Historial del propio Binder

| Versión | Fecha | Estado | Cambio |
| --- | --- | --- | --- |
| v0.3 | 2026-06-21 | Historical | Índice inicial. |
| v0.4 | 2026-06-22 | Historical | Foundation. |
| v0.5 | 2026-06-25 | Historical | Sprints 0–5 y handoff. |
| v0.6 | 2026-07-01 | Historical | Sprints 0–15 y S16. |
| 1.0 | 2026-07-01 | Superseded Candidate | Consolidación 00–14. |
| 1.1-RC1 | 2026-07-01 | Current Candidate | Conjunto completo 00–34 y cierre RC1. |

## 25.2. Próxima revisión obligatoria

Debe revisarse después de regenerar 15; si la auditoría 16 detecta un finding que afecte jerarquía o navegación; al cerrar la baseline final; al cerrar Sprint 16/17 o H6; y ante cualquier alta, baja o cambio de autoridad documental.

## 25.3. Criterios de aceptación

- Incluye todos los documentos 00–34 con nombres exactos y enlaces relativos.
- Distingue autoridad, estado documental, estado técnico y evidencia.
- Ofrece rutas por rol, tarea, sistema, gate y proceso de baseline.
- Registra el estado de consolidación sin fingir que es final.
- Conserva la genealogía y ADR históricos.
- Explica el uso de 12, 13, 31, 32, 33 y 34.
- No declara H6, Steam, RC, launch, accesibilidad, rendimiento o continuidad como aprobados.
- Permite que un lector nuevo determine qué consultar y qué registrar antes de actuar.
- Supera validación Markdown y la auditoría global final.

## 25.4. Siguiente documento

El siguiente entregable de la secuencia es la regeneración de `15_Guia_Maestra.md` contra 00–34 y los Excel 12/13 actualizados. Después se ejecutarán las actualizaciones dirigidas de 21, 23, 31, 33 y 34.

---

<!-- W0_S17_PHASE1_START -->

# 26. Actualización normativa W0 - Sprint17_Phase1

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


## Decisiones formalizadas

| ADR | DEC | Decisión cerrada | Consecuencia inmediata |
|---|---|---|---|
| ADR-0074 | DEC-01 | Día H6 configurable de 300 s; velocidades visibles x0,5, x1, x2 y x4; `PauseService` independiente | Caracterizar tiempo, HUD, pausa y transiciones antes de W1 |
| ADR-0075 | DEC-02 | `SimulationClock` / `StoreDay` es la única autoridad de tiempo de negocio | Ningún estado o sistema mantiene un reloj de negocio paralelo |
| ADR-0076 | DEC-03 | Guardado manual solo desde pausa en `BeforeOpen`, `Closed` o `Results`, sin mutaciones pendientes | Reanudación exacta a mitad de día queda Post-H6 |
| ADR-0077 | DEC-04 | Checkout funcional obligatorio para abrir; no retirar el último durante `Open` | Una invalidación sobrevenida bloquea nuevos spawns y exige recuperación o cierre controlado |
| ADR-0078 | DEC-05 | Máximo H6 configurable de 8 clientes activos; activo = no `Despawned` | Pruebas de capacidad, abandono, cierre y ausencia de deadlock |
| ADR-0079 | DEC-06 | La solicitud reserva fondos; la recepción descuenta caja, registra `SupplierReceivingCost` y añade stock una vez | Commit de recepción atómico e idempotente |
| ADR-0080 | DEC-07 | `Process All` es todo-o-nada sobre órdenes independientes, reserva el total y crea un `DeliveryRun` | Sin cobros, recepciones ni stock parciales |
| ADR-0081 | DEC-08 | Displays monoproducto hasta H6; cantidades, retirada, retorno y limpieza de vacío obligatorios | Multiproducto queda Post-H6 |
| ADR-0082 | DEC-09 | Management conserva detalle del día actual y los dos completados anteriores; días más antiguos mantienen `DailySummary` individual y acumulados lifetime | Campaña mínima de siete días con navegación y persistencia |
| ADR-0083 | DEC-10 | Al cerrar el día 7 se aplica una vez el 10 % de `max(0, resultado bruto técnico semanal)` | Costes fijos quedan Post-H6; el impuesto debe ser idempotente |
| ADR-0084 | DEC-11 | Wall occlusion desactivada mediante feature flag prioritaria; toggle oculto; valores antiguos migrados o ignorados | Regresión obligatoria de cámara y settings |
| ADR-0085 | DEC-12 | Contrato de pivote base-centro mediante wrappers o `GroundAnchor`, con migración/compensación única | No desplazar escenas o saves previos; congelar contrato tras W7 |

El siguiente identificador ADR disponible queda reservado como `ADR-0086`.

## Autoridad y navegación

- Las decisiones ADR-0074 a ADR-0085 se registran en `13_Trazabilidad_y_Control_de_Cambios.xlsx` y se reflejan en los contratos 00-07, 10-11, 14-17, 19-20, 29-34 afectados.
- El Binder sigue siendo índice y ruta de autoridad; no sustituye los contratos ni la evidencia.
- La baseline de entrada de remediación queda congelada en el commit y versión indicados.
- `16_Auditoria_Global_de_Coherencia.md` debe distinguir cierre de decisiones, implementación y ejecución de QA.

## Regla de cierre y no propagación

- El cierre de W0 no abre W1-W7 automáticamente; cada ola requiere su propia evidencia y control de cambios.
- No se declara Sprint17_Phase1 completada mientras W8 no haya ejecutado la regresión integral.
- No se propaga `PASS` de Sprint 16 a H6 ni a la Vertical Slice.
- No se introducen sistemas Post-H6 durante la remediación.
- Código, escenas, prefabs, builds y tests ejecutables no han sido modificados por esta actualización documental.

<!-- W0_S17_PHASE1_END -->
