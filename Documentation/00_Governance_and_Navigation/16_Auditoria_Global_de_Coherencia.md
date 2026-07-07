---
title: "Cartridge & Cloud — Auditoría Global de Coherencia"
author: "VRM Games / Blas Luis Rocha González"
date: "2026-07-07"
lang: "es-ES"
document_version: "1.1-CANDIDATE"
status: "Auditoría candidata — baseline no congelada"
project: "Cartridge & Cloud"
platform: "PC / Steam"
engine_observed: "Unity 6000.3.18f1"
application_version_observed: "0.0.17"
audited_document_range: "00–34"
manifest_candidate_sha256: "bc1f2fb749cdcabd2dcdc4f60dc0bdc22be498954ede49a870052977e6e1b658"
self_sha256: "EXTERNAL AFTER EXPORT"
---

# Cartridge & Cloud — Auditoría Global de Coherencia

**Paquete auditado:** conjunto candidato numerado `00–34`, proyecto Unity suministrado, documentación histórica e inputs de regeneración  
**Fotografía:** `2026-07-02`  
**Manifiesto candidato:** `32_Documentation_Baseline_Manifest.xlsx` — SHA-256 externo `bc1f2fb749cdcabd2dcdc4f60dc0bdc22be498954ede49a870052977e6e1b658`  
**Commit técnico declarado en la documentación:** `091090c43855b0b26b09abe9335d18b978ac7eab` — **no verificable en el ZIP suministrado**  
**Resultado de coherencia de producto/técnica:** `PASS WITH HIGH FINDINGS`  
**Resultado de candidata documental:** `FAIL / BLOCKERS OPEN`  
**Resultado de baseline final:** `NOT READY`  

> El paquete ya contiene los 35 documentos numerados y el manifiesto 32 reproduce correctamente los bytes actuales de los otros 34 archivos. Sin embargo, la candidata no puede congelarse porque la autoridad está dividida: 12, 13, 14, 15, 31, 33 y 34 conservan estados, métricas o hashes anteriores. Además, la matriz de trazabilidad continúa calificando como `Complete` relaciones que carecen de artefacto técnico, ejecución o evidencia suficiente. El proyecto suministrado tampoco incluye `Packages/manifest.json`, `Packages/packages-lock.json` ni metadatos Git, por lo que el vínculo exacto entre documentación y proyecto no puede demostrarse desde el material auditado.

# 0. Resumen ejecutivo

La candidata auditada contiene **35 documentos**: 27 Markdown y 8 Excel, con 6,344,208 bytes totales. Los Markdown suman 76,833 líneas y 498,260 palabras. Los Excel contienen 171 hojas y 686 fórmulas inspeccionadas. No se encontraron enlaces relativos rotos, tablas Markdown mal formadas, bloques de código sin cerrar ni errores reales de fórmula.

La consolidación previa resolvió una parte importante del trabajo RC1: ya existen 33 y 34; 21, 23 y 31 fueron regenerados; 32 identifica correctamente los archivos actuales; 33 amplió riesgo, continuidad, backups y restore; 34 formalizó operación, handoff y autorreferencia de hashes. El problema restante no está en la ausencia física de documentos, sino en la propagación incompleta de esos cambios hacia los registros que siguen actuando como autoridad operativa.

Los tres bloqueantes son:

1. **Autoridad dividida e inventarios obsoletos.** El manifiesto 32 es correcto, pero otros documentos de gobierno siguen apuntando a archivos y estados anteriores.
2. **Trazabilidad semánticamente inflada.** `Complete` se usa como cobertura de registro aunque falten implementación, ejecución o evidencia.
3. **Reproducibilidad técnica no demostrable.** Faltan Packages y Git en el proyecto suministrado, aunque la documentación declara paquetes, hashes y commit concretos.

| Dimensión | Veredicto | Motivo principal |
| --- | --- | --- |
| Identidad, alcance y producto | PASS WITH FINDINGS | Los hechos centrales son coherentes. |
| Arquitectura, datos y UX | PASS WITH FINDINGS | Las deudas conocidas están declaradas; no se reejecutó Unity. |
| Unity y configuración observable | HIGH FINDINGS | StoreInitial no está conectado; Packages/Git no están en el snapshot. |
| QA y H6 | NOT READY | Controles honestamente no ejecutados; StoreInitial y build candidata pendientes. |
| Trazabilidad y cambios | FAIL | `Complete` no equivale a end-to-end y los registros de gobierno están desfasados. |
| Riesgo y continuidad | NOT VERIFIED | 192 riesgos, 186 fuera de tolerancia, 0 acciones/restore tests validados. |
| Integridad documental candidata | FAIL | Autoridad dividida y hashes/estados obsoletos fuera de 32. |
| Baseline final | NOT READY | BLOCKER/HIGH abiertos; artefactos externos, Git y restore ausentes. |

# 1. Objetivo y alcance

Esta auditoría evalúa el conjunto actual `00–34` como una candidata de baseline documental. Comprueba que los documentos puedan utilizarse como un sistema de gobierno coherente, que los estados observados no se confundan con objetivos, que los Excel no produzcan false PASS semánticos y que los hashes, rutas, relaciones y sustituciones permitan identificar una única candidata.

El alcance incluye:

- los 35 documentos numerados actuales;
- la estructura, valores visibles y fórmulas de los ocho Excel;
- el proyecto Unity suministrado en `U012_CartridgeAndCloud.zip`;
- `ProjectSettings`, escenas, scripts, asmdefs, Build Profiles y settings runtime observables;
- el inventario histórico de 745 archivos preservado en 32/33;
- la secuencia de consolidación, cierre, empaquetado, restauración y handoff.

No se ha ejecutado Unity, Test Runner, una build ni una prueba de restauración. Las cifras `1215 EditMode + 70 PlayMode` se tratan como evidencia histórica declarada, no como una nueva certificación. Tampoco se ha verificado el commit declarado porque el ZIP no contiene `.git` ni un bundle/clone verificable.

# 2. Fuentes y jerarquía aplicada

1. `00_Enfoque_y_Alcance.md` gobierna visión, pilares y límites.
2. `01–13` gobiernan contratos de producto, técnica, datos, UX, producción, QA, build y trazabilidad.
3. `14–16` gobiernan navegación, método y auditoría.
4. `17–30` gobiernan dominios especializados.
5. `31–34` gobiernan evidencia de gates, manifiesto, riesgo/continuidad y operación/handoff.
6. El proyecto Unity observado responde qué existe en la fotografía suministrada.
7. La historia preservada explica evolución; no sustituye la autoridad actual.

Para esta candidata, `32_Documentation_Baseline_Manifest.xlsx` se usa como inventario de referencia porque es el único registro cuya tabla de bytes y SHA-256 coincide con todos los archivos externos actuales. Esta prioridad no convierte 32 en baseline final ni permite ignorar las contradicciones internas señaladas.

# 3. Método

Se aplicaron: inventario y SHA-256; comparación de hashes registrados; análisis de front matter; cierre de bloques de código; validación básica de tablas y enlaces; inspección de 171 hojas Excel; scan de errores de fórmula; comparación de dashboards con filas base; revisión de ProjectVersion, ProjectSettings, EditorBuildSettings, Build Profiles, StoreRuntimeSettings, escenas, scripts y asmdefs; comparación de poblaciones de requisitos, riesgos, controles, evidencias y pruebas; y revisión de estados de cierre, package artifacts, Git y restore.

Las conclusiones distinguen:

- **contradicción documental**, cuando dos autoridades describen de forma incompatible el mismo estado;
- **deuda o trabajo abierto**, cuando el documento representa correctamente una capacidad aún no ejecutada;
- **evidencia insuficiente**, cuando existe una afirmación sin artefacto verificable;
- **diferencia histórica justificada**, cuando versiones anteriores se conservan sin competir con la autoridad actual.

# 4. Modelo de severidad

| Severidad | Definición | Efecto sobre el cierre |
| --- | --- | --- |
| BLOCKER | Invalida autoridad, integridad o reproducibilidad esencial. | No se permite freeze ni baseline final. |
| HIGH | Puede causar aprobación incorrecta, pérdida de trazabilidad o fallo de un gate crítico. | Debe resolverse o recibir excepción formal antes del cierre afectado. |
| MEDIUM | Inconsistencia relevante de mantenimiento, claridad o evidencia. | Debe corregirse o planificarse explícitamente. |
| LOW | Mejora de normalización o portabilidad. | No bloquea por sí sola. |
| INFORMATIONAL | Resultado positivo o estado abierto correctamente representado. | Mantener evidencia; no requiere corrección salvo cambio. |

La auditoría contiene **29 hallazgos**: BLOCKER=3, HIGH=10, MEDIUM=9, LOW=2, INFORMATIONAL=5.

# 5. Inventario auditado

| ID | Archivo | Bytes | Métrica estructural | SHA-256 |
| --- | --- | ---: | --- | --- |
| 00 | `00_Enfoque_y_Alcance.md` | 127401 | 4495 líneas / 15880 palabras | `63dd6f0f1b9c2c80be2aec55718d18d9d031ce4acb14f677769d6e69ceaad21f` |
| 01 | `01_Game_Design_Document.md` | 132822 | 4490 líneas / 16583 palabras | `a9cd9e3203abc5269c25c59dc7f626b0771df4ec2d65e781109ff146bceb2177` |
| 02 | `02_Vertical_Slice_Specification.md` | 64531 | 1505 líneas / 8451 palabras | `75893f130116e88a08b8da5590dd81876a234581081eafaa8d08374c1a60513f` |
| 03 | `03_Technical_Design_Document.md` | 101994 | 3305 líneas / 12463 palabras | `66264703c5ff4959d03093690e9845a98ec0e5025e685b9a639ac8cbb616cd12` |
| 04 | `04_Modelo_de_Datos.md` | 102948 | 2691 líneas / 10630 palabras | `d851a72b55742c2c704cc7c002929bc5894e7142511c6af843440bb193fb19d4` |
| 05 | `05_UX_Flow.md` | 56805 | 2458 líneas / 7071 palabras | `e08da5ff67fb073a8b950babe325ae58ddb2e121513dc2f553acd4f2c14b867e` |
| 06 | `06_Production_Roadmap_y_Sprint_Plan.md` | 61564 | 2408 líneas / 7590 palabras | `d00b156c0ad1c66069278119c55f76c738a577a3e2835f770208d1a5f2faadff` |
| 07 | `07_QA_Testing_Plan.md` | 79435 | 3324 líneas / 9604 palabras | `bc9c05a3737e345546ef16e5390e034295ef35ad66c31647852aec81b4e7affe` |
| 08 | `08_QA_Testing_Matrix.xlsx` | 185032 | 17 hojas / 92 fórmulas | `233e57f01d2468fe13c136c8fcd3ee75b39bd3de6530349626ea98d4ff254214` |
| 09 | `09_CSharp_Coding_Standards.md` | 95321 | 3664 líneas / 11320 palabras | `3b734d6cd49f31c09c10bb4941625e143f6c634e3fb50c157f0720e73b1c2a68` |
| 10 | `10_Unity_Project_Setup_Guide.md` | 78408 | 2748 líneas / 9656 palabras | `31430bb0544edd9577323aa0009d6ca3df8b9fbcf6c8a150fc33df69b9f963a4` |
| 11 | `11_Build_y_Versioning_Guide.md` | 85324 | 3156 líneas / 10803 palabras | `38a16a0f97505c0b405509dd3c7e3d1b50e77b89fbe3f4cb1931d3e04f602215` |
| 12 | `12_Excel_Maestro_de_Produccion.xlsx` | 484932 | 26 hojas / 68 fórmulas | `3d70e97326868389dac977934e6000285c22b3c33c0e62be31313880726b4d7d` |
| 13 | `13_Trazabilidad_y_Control_de_Cambios.xlsx` | 577554 | 29 hojas / 76 fórmulas | `97cf106d7707de9e6dcadfa2ec9374676b0ea9fe0945ac87eb89db788df8da43` |
| 14 | `14_Project_Binder_Indice_Maestro.md` | 95577 | 1665 líneas / 11431 palabras | `d0e8c2b0bf78a414d00c290d8ab8e4b2742e80de7bf509764318c6b7a9e4f144` |
| 15 | `15_Guia_Maestra.md` | 149595 | 3272 líneas / 20132 palabras | `70aa63feb69785d3b2e8b044c73b9c5f075829fd198e4de5504196d2262cae71` |
| 16 | `16_Auditoria_Global_de_Coherencia.md` | SELF — candidato generado por esta auditoría | Métricas y SHA-256 externos tras exportación | EXTERNAL AFTER EXPORT |
| 17 | `17_Art_Bible.md` | 133913 | 2033 líneas / 17620 palabras | `095c6cc42223a972c7faa68435595fc7c22f839fabc62b2038371a0ef6c14239` |
| 18 | `18_Audio_Bible.md` | 144525 | 1098 líneas / 19651 palabras | `c2eabe93a7c26f70e1d4d77fb0ddcaa97e2cdd1e70247e637b9db5bd2f07ce92` |
| 19 | `19_UI_Style_Guide.md` | 149800 | 1722 líneas / 19758 palabras | `b34ca4db1eba7a536466b6a9a105d8d92396ade2108aa7dd99fa7d5d4eaebc1a` |
| 20 | `20_Economy_and_Balance_Specification.md` | 154547 | 1185 líneas / 19995 palabras | `908c70772ed35eecdb05c3036c6901523a4caf2c3e708ca8f049c2a160ef2aac` |
| 21 | `21_Initial_Content_Catalog.xlsx` | 186599 | 26 hojas / 100 fórmulas | `2f98e2b7070e5a1f70d4feee03a7d154a2dcdeff8078b040c442e2393fc676c8` |
| 22 | `22_Localization_Plan.md` | 157323 | 1743 líneas / 20139 palabras | `184a53b2813bc1327a6913e6e88ea4c81142da1ee4ddeff88f75467c784f1ff0` |
| 23 | `23_Legal_Credits_and_Licenses_Register.xlsx` | 489706 | 26 hojas / 62 fórmulas | `df975425a8b8c1cfd9375179893a68422c5691c95f63be5f0862b3d6ce783e63` |
| 24 | `24_Steam_Publishing_Plan.md` | 171037 | 3199 líneas / 20653 palabras | `f087e24d3a18f8c4a6651728d5f70d1df804042587e206fdeac2e7e637a87646` |
| 25 | `25_Post_Launch_and_Live_Operations_Plan.md` | 327218 | 3967 líneas / 42786 palabras | `158e1e43514c5d5d6c365f6cd1a73929f54c4a48c5cc460b593530ae5e49bac9` |
| 26 | `26_Privacy_Data_and_Telemetry_Plan.md` | 321182 | 4253 líneas / 38888 palabras | `284abfe537793c2ec8b0e5cbac484cd9395567c4434aef7f5ad5ee7005e95f36` |
| 27 | `27_Security_and_Incident_Response_Plan.md` | 422182 | 6577 líneas / 51695 palabras | `f925bfbf3df2000323c12a4486480d06c8ab8181684e5922168b8bdf89d05f92` |
| 28 | `28_Marketing_and_Communication_Plan.md` | 87510 | 2514 líneas / 11456 palabras | `28dd332855126558b3da6019f2e546b1e8ab7db81dc530a89f3e3fbfe757cded` |
| 29 | `29_Accessibility_and_Inclusive_Design_Plan.md` | 105005 | 2853 líneas / 13064 palabras | `678ceabb509062cd93c7690315397b1cc6204c0da6a5d57324ba11e2fda64333` |
| 30 | `30_Performance_and_Optimization_Plan.md` | 109482 | 3106 líneas / 13627 palabras | `18f53eaa61734ad735c02c8b7c70ed37f5f0e80f7c01828b05d092c49b0de260` |
| 31 | `31_H6_and_Release_Readiness_Checklist.xlsx` | 219011 | 14 hojas / 84 fórmulas | `c61a903e5a6d5944b60c4ffde403f6184bc96d23d68996798c43b7ecc9515832` |
| 32 | `32_Documentation_Baseline_Manifest.xlsx` | 203050 | 13 hojas / 120 fórmulas | `bc1f2fb749cdcabd2dcdc4f60dc0bdc22be498954ede49a870052977e6e1b658` |
| 33 | `33_Project_Risk_Register_and_Business_Continuity_Plan.xlsx` | 314777 | 20 hojas / 84 fórmulas | `2bde968afc77c8a2ad3c8b28c06fcb0542d1aedb6985b1c33139d2382e006521` |
| 34 | `34_Final_Handoff_and_Project_Operations_Manual.md` | 114714 | 2565 líneas / 13425 palabras | `9e1c620512928547658c1ea8ca889d23429fd5fcfbfdf647717dc9624cc40dcf` |

El SHA-256 del propio documento 16 se calcula después de exportarlo y debe registrarse externamente. Introducirlo dentro del Markdown cambiaría sus bytes y produciría una autorreferencia inestable.

# 6. Resultados estructurales

## 6.1. Markdown

- 27 archivos Markdown.
- 76,833 líneas y 498,260 palabras.
- 0 enlaces relativos rotos.
- 0 tablas básicas mal formadas.
- 0 code fences sin cerrar.
- Excepciones de front matter: 10 y 27.

## 6.2. Excel

| Archivo | Hojas | Fórmulas | Scan de errores |
| --- | ---: | ---: | --- |
| `08_QA_Testing_Matrix.xlsx` | 17 | 92 | PASS |
| `12_Excel_Maestro_de_Produccion.xlsx` | 26 | 68 | PASS |
| `13_Trazabilidad_y_Control_de_Cambios.xlsx` | 29 | 76 | PASS |
| `21_Initial_Content_Catalog.xlsx` | 26 | 100 | PASS |
| `23_Legal_Credits_and_Licenses_Register.xlsx` | 26 | 62 | PASS |
| `31_H6_and_Release_Readiness_Checklist.xlsx` | 14 | 84 | PASS |
| `32_Documentation_Baseline_Manifest.xlsx` | 13 | 120 | PASS |
| `33_Project_Risk_Register_and_Business_Continuity_Plan.xlsx` | 20 | 84 | PASS |

La búsqueda textual de 31 encontró las cadenas `#REF!`, `#VALUE!` y similares dentro de una instrucción de prueba; no son errores calculados. No se detectaron celdas con errores visibles.

# 7. Hechos coherentes de producto y estado

| Hecho | Valor observado/coherente |
| --- | --- |
| Proyecto / estudio | Cartridge & Cloud / VRM Games |
| Plataforma | PC / Steam; objetivo Windows x64 |
| Unity | 6000.3.18f1 |
| Aplicación | 0.0.17 |
| Company / Product | VRM Games / Cartridge & Cloud |
| Identifier Standalone | com.vrmgames.cartridgeandcloud |
| Escenas observadas | Bootstrap, MainMenu, Store, StoreInitial, TestLab |
| Scripts C# de proyecto | 457 |
| Assemblies propios | 12 |
| Producción | Sprints 0–15 CLOSED/PASS; Sprint 16 IN PROGRESS; Sprint 17 PENDING |
| H6 / release | PENDING / NOT RUN |
| Manifiesto documental | 35/35 documentos presentes; 29 READY; 6 con acción |

# 8. Evidencia técnica observada

| Elemento | Estado observado | Interpretación |
| --- | --- | --- |
| ProjectVersion | 6000.3.18f1 (5ebeb53e4c07) | Coincide con documentación. |
| Bundle Version | 0.0.17 | Coincide. |
| Resolución / ventana | 1024×768; resizableWindow=0 | Deuda/decisión conocida. |
| Input System | activeInputHandler=1 | Activo. |
| EditorBuildSettings | Bootstrap; MainMenu; Store; TestLab | No es candidata H6. |
| Windows_Development | Store y TestLab incluidos | Perfil de desarrollo, no final. |
| StoreRuntimeSettings | `_storeSceneName: Store`; `_buildBlockoutOnLoad: 1` | Migración S16 pendiente. |
| StoreInitial.unity | Presente | Escena existe, pero no demuestra integración representativa. |
| StoreInitialSceneContext | Ausente | Control objetivo no implementado. |
| StoreInitialEnvironment.prefab | Ausente | Autoría representativa pendiente. |
| Localización | Solo `.gitkeep` | ES/EN no demostrado. |
| Packages | Directorio ausente | No se verifican manifest/lock ni URP desde el snapshot. |
| Git | `.git`/bundle ausentes | Commit/tag no verificables. |

# 9. Registro maestro de hallazgos

| ID | Severidad | Área | Hallazgo | Documentos / fuente | Gate | Estado |
| --- | --- | --- | --- | --- | --- | --- |
| AUD-CAND-001 | BLOCKER | Gobierno e integridad | La autoridad documental está dividida entre el manifiesto 32 actual y registros de gobierno obsoletos | 12, 13, 14, 15, 31, 32, 33, 34 | Baseline final / freeze | OPEN |
| AUD-CAND-002 | BLOCKER | Trazabilidad | La matriz end-to-end sigue calificando como Complete relaciones sin implementación ni evidencia suficiente | 13; impacto en 14, 15, 31 y 32 | Baseline final / H6 | OPEN |
| AUD-CAND-003 | BLOCKER | Reproducibilidad técnica | El proyecto suministrado no contiene Packages ni metadatos Git para verificar la configuración y el commit declarados | 03, 10, 11, 13, 16, 22, 27, 30, 32, 34; proyecto Unity | Baseline final / restauración / Git binding | OPEN |
| AUD-CAND-004 | HIGH | Riesgos y producción | 12 y 13 conservan la población anterior de 180 riesgos mientras 33 gobierna 192 | 12, 13, 33, 34 | Baseline final / continuidad | OPEN |
| AUD-CAND-005 | HIGH | Readiness y handoff | El manual 34 utiliza recuentos anteriores del checklist 31 | 31, 34 | Baseline final / handoff | OPEN |
| AUD-CAND-006 | HIGH | Readiness documental | 31 conserva una secuencia anterior a la regeneración de 33, 34 y 32 candidata | 31, 32, 33, 34 | G-DOC-RC / G-BASE | OPEN |
| AUD-CAND-007 | HIGH | Riesgos y continuidad | 33 conserva 32 y 34 como pendientes y referencia sus hashes RC1 | 32, 33, 34 | G-DOC-RC / continuidad | OPEN |
| AUD-CAND-008 | HIGH | Handoff e integridad | 34 conserva el estado y hash anteriores del manifiesto 32 | 32, 34 | Handoff / package freeze | OPEN |
| AUD-CAND-009 | HIGH | Manifiesto de baseline | 32 contiene texto interno que todavía describe 33–34 como planificados | 32 | Baseline final | OPEN |
| AUD-CAND-010 | HIGH | Auditoría automática | Los controles automáticos de 13 no detectan la insuficiencia semántica de la trazabilidad | 13 | Baseline final / confianza del dashboard | OPEN |
| AUD-CAND-011 | HIGH | Builds y evidencia | Las builds históricas PASS no tienen procedencia suficiente para actuar como evidencia reproducible | 11, 12, 13, 31, 34 | H6 / baseline técnica | OPEN |
| AUD-CAND-012 | HIGH | Unity / H6 | StoreInitial existe, pero todavía no está conectado como entorno representativo de build | 02, 03, 05–08, 10–13, 17, 19, 21, 29–31, 34 | Sprint 16 / H6 | OPEN |
| AUD-CAND-013 | HIGH | Paquete final | Los artefactos externos, el ZIP inmutable y las pruebas de restauración no existen todavía | 31–34; artefactos externos | G-BASE / G-DOC-RESTORE | OPEN |
| AUD-CAND-014 | MEDIUM | Binder | 14 conserva estados e inventario anteriores a las regeneraciones posteriores | 14; referencias a 15, 21, 23, 31–34 | Navegación y gobierno | OPEN |
| AUD-CAND-015 | MEDIUM | Guía Maestra | 15 conserva estados, tamaños y hashes anteriores para documentos actualizados | 15; referencias a 21, 23, 31–34 | Retoma y operación documental | OPEN |
| AUD-CAND-016 | MEDIUM | Taxonomía de requisitos | Los universos de QA y trazabilidad no tienen una taxonomía común explícita | 08, 13, 14, 15, 31 | Métricas y cobertura | OPEN |
| AUD-CAND-017 | MEDIUM | Build Profiles | Los perfiles citados para QA, H6 y Steam no existen en el proyecto suministrado | 10, 11, 13, 31, 34; Assets/_Project/Settings/BuildProfiles | Sprint 16 / H6 / Steam | OPEN |
| AUD-CAND-018 | MEDIUM | Localización | La implementación ES/EN no está demostrada en la fotografía del proyecto | 05, 07, 08, 10, 19, 21, 22, 24, 29, 31 | Sprint 17 / H6 / release | OPEN |
| AUD-CAND-019 | MEDIUM | Formato documental | La convención de front matter no es uniforme | 10, 27 | Validación estructural | OPEN |
| AUD-CAND-020 | MEDIUM | Metadatos | Versiones y fechas de regeneración no siguen una regla única | 12–15, 21, 23, 31–34 | Trazabilidad de versión | OPEN |
| AUD-CAND-021 | MEDIUM | Mantenibilidad | Varios documentos especializados contienen grandes bloques de boilerplate repetido | 19, 22, 25, 26, 27 | Calidad documental | OPEN |
| AUD-CAND-022 | MEDIUM | Placeholders y cierre | Los estados TBD/PENDING/PLACEHOLDER no tienen una allowlist final única | 00–34 | Baseline final | OPEN |
| AUD-CAND-023 | LOW | Terminología | Los estados mezclan español, inglés y variantes equivalentes | 00–34 | Mantenibilidad | OPEN |
| AUD-CAND-024 | LOW | Portabilidad | Algunos registros de procedencia contienen rutas absolutas del entorno de regeneración | 32 y registros de fuente | Paquete final / restauración | OPEN |
| AUD-CAND-025 | INFORMATIONAL | Integridad Markdown | La estructura básica de los 27 Markdown es válida | Todos los Markdown | Validación estructural | VERIFIED |
| AUD-CAND-026 | INFORMATIONAL | Integridad Excel | Los ocho Excel se abren y no muestran errores de fórmula reales | 08, 12, 13, 21, 23, 31, 32, 33 | Validación estructural | VERIFIED |
| AUD-CAND-027 | INFORMATIONAL | Manifiesto candidato | 32 identifica correctamente los bytes actuales de todos los documentos externos | 32 y 00–34 | Candidata de auditoría | VERIFIED |
| AUD-CAND-028 | INFORMATIONAL | Coherencia de producto y técnica | Los hechos centrales de identidad y estado siguen siendo coherentes | 00–34 y ProjectSettings | Uso operativo | VERIFIED |
| AUD-CAND-029 | INFORMATIONAL | Honestidad operativa | 31 y 33 evitan correctamente afirmar H6, release o continuidad sin evidencia | 31, 33, 34 | H6 / release / continuidad | VERIFIED |

# 10. Análisis detallado de hallazgos

## 10.1. AUD-CAND-001 — La autoridad documental está dividida entre el manifiesto 32 actual y registros de gobierno obsoletos

| Campo | Valor |
| --- | --- |
| Severidad | BLOCKER |
| Área | Gobierno e integridad |
| Documentos / fuente | 12, 13, 14, 15, 31, 32, 33, 34 |
| Gate afectado | Baseline final / freeze |
| Estado | OPEN |

**Evidencia.** El documento 32 candidato coincide con los bytes actuales de todos los archivos externos, pero 12 conserva 10 tamaños/hashes obsoletos, 13 conserva 9, el Binder 14 contiene 7 hashes antiguos, la Guía 15 contiene 6, 33 contiene 2 y 34 contiene el hash anterior de 32. Los estados también discrepan: 14/15 presentan actualizaciones ya completadas como pendientes; 31 continúa indicando que 33/34 y 32 candidata están pendientes; 33 señala 34 como siguiente documento.

**Evaluación.** No existe una única respuesta coherente a “qué archivos y estados componen la candidata actual”. Un lector que use 12, 13, 14, 15, 31, 33 o 34 puede seleccionar hashes o secuencias anteriores.

**Resolución requerida.** Usar 32 como fotografía candidata de referencia; actualizar 12, 13, 14, 15, 31, 33 y 34 con los hallazgos, estados y hashes vigentes. Después regenerar 16 final y 32 final.

## 10.2. AUD-CAND-002 — La matriz end-to-end sigue calificando como Complete relaciones sin implementación ni evidencia suficiente

| Campo | Valor |
| --- | --- |
| Severidad | BLOCKER |
| Área | Trazabilidad |
| Documentos / fuente | 13; impacto en 14, 15, 31 y 32 |
| Gate afectado | Baseline final / H6 |
| Estado | OPEN |

**Evidencia.** En 13/03_Matriz_End_to_End existen 708 filas. 296 figuran como Complete; de esas, 294 no tienen Código/Escena/Asset, 222 carecen de evidencia, 262 tienen Not Run/Pending, 256 reutilizan el Requirement ID como Implementación/Tarea y 284 se vinculan a cambios CHG-DOC genéricos.

**Evaluación.** La hoja demuestra cobertura de registro y definición, pero no trazabilidad end-to-end. El estado Complete puede inducir a aprobar gates sin artefacto, ejecución o prueba atribuible.

**Resolución requerida.** Separar cobertura de definición, vínculo de implementación, cobertura de prueba, ejecución y evidencia. Reservar Complete para filas con artefacto verificable, prueba ejecutada, resultado y evidencia.

## 10.3. AUD-CAND-003 — El proyecto suministrado no contiene Packages ni metadatos Git para verificar la configuración y el commit declarados

| Campo | Valor |
| --- | --- |
| Severidad | BLOCKER |
| Área | Reproducibilidad técnica |
| Documentos / fuente | 03, 10, 11, 13, 16, 22, 27, 30, 32, 34; proyecto Unity |
| Gate afectado | Baseline final / restauración / Git binding |
| Estado | OPEN |

**Evidencia.** U012_CartridgeAndCloud.zip no contiene Packages/manifest.json, Packages/packages-lock.json ni directorio .git. La documentación declara hashes concretos de manifest/lock, URP 17.3.0, 41 dependencias directas, 57 entradas de lock y el commit 091090c…, pero esta fotografía no permite comprobarlos independientemente.

**Evaluación.** ProjectVersion y ProjectSettings sí son verificables, pero la dependencia exacta de paquetes y el vínculo documental con un commit no son reproducibles desde el material suministrado.

**Resolución requerida.** Incluir Packages/manifest.json, Packages/packages-lock.json y evidencia Git verificable —bundle, clone, commit/tag o export controlado— en la fuente asociada a la baseline. Registrar hashes y prueba de checkout/importación.

## 10.4. AUD-CAND-004 — 12 y 13 conservan la población anterior de 180 riesgos mientras 33 gobierna 192

| Campo | Valor |
| --- | --- |
| Severidad | HIGH |
| Área | Riesgos y producción |
| Documentos / fuente | 12, 13, 33, 34 |
| Gate afectado | Baseline final / continuidad |
| Estado | OPEN |

**Evidencia.** 12 muestra 180 riesgos y 174 fuera de tolerancia; 13 replica 180 acciones y 174 riesgos fuera de tolerancia; 33 contiene 192 riesgos, 192 acciones y 186 fuera de tolerancia.

**Evaluación.** Los dashboards y registros operativos no representan el registro de riesgos vigente.

**Resolución requerida.** Sincronizar 12 y 13 con 33, incluidos los 12 riesgos añadidos, acciones, BIA, backups, dependencias y estados de evidencia.

## 10.5. AUD-CAND-005 — El manual 34 utiliza recuentos anteriores del checklist 31

| Campo | Valor |
| --- | --- |
| Severidad | HIGH |
| Área | Readiness y handoff |
| Documentos / fuente | 31, 34 |
| Gate afectado | Baseline final / handoff |
| Estado | OPEN |

**Evidencia.** 31 contiene 280 controles H6, 223 de release, 48 paquetes de evidencia y 79 pruebas; 34 declara 266, 209, 40 y 71 respectivamente.

**Evaluación.** El manual operativo no describe el universo real que debe ejecutar un operador alternativo.

**Resolución requerida.** Actualizar los recuentos y cualquier procedimiento dependiente; conservar los estados NOT RUN/MISSING/OPEN.

## 10.6. AUD-CAND-006 — 31 conserva una secuencia anterior a la regeneración de 33, 34 y 32 candidata

| Campo | Valor |
| --- | --- |
| Severidad | HIGH |
| Área | Readiness documental |
| Documentos / fuente | 31, 32, 33, 34 |
| Gate afectado | G-DOC-RC / G-BASE |
| Estado | OPEN |

**Evidencia.** El dashboard de 31 aún indica “33/34 directed updates” y “32 candidate pending”; sus filas de consolidación describen 33 y 34 como los siguientes trabajos.

**Evaluación.** Los controles operativos son válidos, pero el estado de la consolidación documental no es actual.

**Resolución requerida.** Marcar 33, 34 y 32 candidata como completados; señalar 16 candidata como siguiente paso; incorporar los hallazgos de esta auditoría.

## 10.7. AUD-CAND-007 — 33 conserva 32 y 34 como pendientes y referencia sus hashes RC1

| Campo | Valor |
| --- | --- |
| Severidad | HIGH |
| Área | Riesgos y continuidad |
| Documentos / fuente | 32, 33, 34 |
| Gate afectado | G-DOC-RC / continuidad |
| Estado | OPEN |

**Evidencia.** 33 registra 32 con 197250 bytes y hash 05ef… y 34 con 102697 bytes y hash 3b3f…; los archivos actuales son 203050 bytes/hash bc1f… y 114714 bytes/hash 9e1c…. El dashboard sigue señalando 34 como siguiente documento.

**Evaluación.** El registro de riesgos no conoce la candidata real que debe proteger y restaurar.

**Resolución requerida.** Actualizar 14_Current_Traceability, dashboard, instrucciones, change log y cualquier riesgo/dependencia afectado por los findings.

## 10.8. AUD-CAND-008 — 34 conserva el estado y hash anteriores del manifiesto 32

| Campo | Valor |
| --- | --- |
| Severidad | HIGH |
| Área | Handoff e integridad |
| Documentos / fuente | 32, 34 |
| Gate afectado | Handoff / package freeze |
| Estado | OPEN |

**Evidencia.** La tabla de inventario de 34 marca 32 como “RC1 / regeneración candidata pendiente” con hash 05ef…; la candidata actual tiene hash externo bc1f2f….

**Evaluación.** El manual no puede guiar una restauración correcta mientras apunte al manifiesto anterior.

**Resolución requerida.** Actualizar el estado/hash candidato de 32 ahora y volver a actualizar 34 con el 32 final, paquete, commit/tag y resultado de restauración después del freeze.

## 10.9. AUD-CAND-009 — 32 contiene texto interno que todavía describe 33–34 como planificados

| Campo | Valor |
| --- | --- |
| Severidad | HIGH |
| Área | Manifiesto de baseline |
| Documentos / fuente | 32 |
| Gate afectado | Baseline final |
| Estado | OPEN |

**Evidencia.** 02_Baseline_Actual!A2 indica “Rows 33–34 are planned”; 11_Control_Cambios conserva la entrada BL-0002 “Still planned”, aunque ambos archivos están presentes y registrados en el resto del libro.

**Evaluación.** El inventario de filas es correcto, pero la descripción y el historial visible se contradicen con él.

**Resolución requerida.** Corregir el texto y cerrar/sustituir BL-0002 en la versión final de 32.

## 10.10. AUD-CAND-010 — Los controles automáticos de 13 no detectan la insuficiencia semántica de la trazabilidad

| Campo | Valor |
| --- | --- |
| Severidad | HIGH |
| Área | Auditoría automática |
| Documentos / fuente | 13 |
| Gate afectado | Baseline final / confianza del dashboard |
| Estado | OPEN |

**Evidencia.** AUD-RC1-003 da PASS porque ninguna fila carece simultáneamente de Test ID y Evidencia; sin embargo, 294 filas Complete carecen de artefacto, 222 de evidencia y 262 no están ejecutadas.

**Evaluación.** El control verifica presencia nominal, no trazabilidad end-to-end.

**Resolución requerida.** Añadir controles separados para artefacto técnico, ejecución, resultado, build/commit y evidencia; hacer fallar cualquier Complete que no cumpla todos los criterios.

## 10.11. AUD-CAND-011 — Las builds históricas PASS no tienen procedencia suficiente para actuar como evidencia reproducible

| Campo | Valor |
| --- | --- |
| Severidad | HIGH |
| Área | Builds y evidencia |
| Documentos / fuente | 11, 12, 13, 31, 34 |
| Gate afectado | H6 / baseline técnica |
| Estado | OPEN |

**Evidencia.** BLD-S00–BLD-S15 figuran PASS sin commit, checksum ni artefacto; BLD-016-PRE tiene commit declarado pero no checksum. Las candidatas posteriores siguen NOT RUN/MISSING.

**Evaluación.** Pueden conservarse como historia aceptada, pero no deben utilizarse como evidencia de reproducibilidad ni integridad de una candidata actual.

**Resolución requerida.** Clasificarlas explícitamente como evidencia histórica no reproducible; exigir commit, checksum, perfil, escenas, tests, Player.log y ubicación para toda candidata futura.

## 10.12. AUD-CAND-012 — StoreInitial existe, pero todavía no está conectado como entorno representativo de build

| Campo | Valor |
| --- | --- |
| Severidad | HIGH |
| Área | Unity / H6 |
| Documentos / fuente | 02, 03, 05–08, 10–13, 17, 19, 21, 29–31, 34 |
| Gate afectado | Sprint 16 / H6 |
| Estado | OPEN |

**Evidencia.** EditorBuildSettings y Windows_Development usan Bootstrap, MainMenu, Store y TestLab; StoreRuntimeSettings apunta a Store y activa buildBlockoutOnLoad. No existen StoreInitialSceneContext ni StoreInitialEnvironment.prefab.

**Evaluación.** La documentación representa correctamente el estado abierto, pero Sprint 16 y H6 siguen bloqueados.

**Resolución requerida.** Autorizar StoreInitial, crear/validar el contexto, desactivar la shell procedural, crear perfil candidato sin TestLab y ejecutar regresión/build externa.

## 10.13. AUD-CAND-013 — Los artefactos externos, el ZIP inmutable y las pruebas de restauración no existen todavía

| Campo | Valor |
| --- | --- |
| Severidad | HIGH |
| Área | Paquete final |
| Documentos / fuente | 31–34; artefactos externos |
| Gate afectado | G-BASE / G-DOC-RESTORE |
| Estado | OPEN |

**Evidencia.** 32 registra como MISSING README_FIRST, VERSION, PACKAGE_MANIFEST, CHECKSUMS, validation report, changelog, restore guide, Git record, ZIP y restore log. 33 mantiene EX-17/EX-18 NOT RUN.

**Evaluación.** Es el estado esperado de una candidata, pero bloquea necesariamente la baseline final.

**Resolución requerida.** Generar solo después de resolver findings y congelar 16/32 finales; verificar extracción, hashes, apertura de Excel/Markdown y handoff independiente.

## 10.14. AUD-CAND-014 — 14 conserva estados e inventario anteriores a las regeneraciones posteriores

| Campo | Valor |
| --- | --- |
| Severidad | MEDIUM |
| Área | Binder |
| Documentos / fuente | 14; referencias a 15, 21, 23, 31–34 |
| Gate afectado | Navegación y gobierno |
| Estado | OPEN |

**Evidencia.** 14 marca 15 como pendiente, 21/23/31/33/34 como updates pendientes y contiene 7 hashes obsoletos.

**Evaluación.** El documento sigue siendo útil como índice, pero sus tablas de estado no deben usarse para decidir la candidata actual.

**Resolución requerida.** Actualizar estados, secuencia, inventario, hashes candidatos y checklist CNS.

## 10.15. AUD-CAND-015 — 15 conserva estados, tamaños y hashes anteriores para documentos actualizados

| Campo | Valor |
| --- | --- |
| Severidad | MEDIUM |
| Área | Guía Maestra |
| Documentos / fuente | 15; referencias a 21, 23, 31–34 |
| Gate afectado | Retoma y operación documental |
| Estado | OPEN |

**Evidencia.** La tabla final de 15 contiene 6 hashes obsoletos y presenta 21/23/31/33/34 como pendientes.

**Evaluación.** La guía describe bien el proceso, pero su fotografía de archivos no es vigente.

**Resolución requerida.** Actualizar la fotografía candidata y limitar la futura pasada final a findings, paquete, Git y restore.

## 10.16. AUD-CAND-016 — Los universos de QA y trazabilidad no tienen una taxonomía común explícita

| Campo | Valor |
| --- | --- |
| Severidad | MEDIUM |
| Área | Taxonomía de requisitos |
| Documentos / fuente | 08, 13, 14, 15, 31 |
| Gate afectado | Métricas y cobertura |
| Estado | OPEN |

**Evidencia.** 08 declara 173 requisitos activos; 13 contiene 708 filas procedentes de requisitos de producto, checklist H6/release y controles documentales.

**Evaluación.** Las cifras pueden ser válidas para universos distintos, pero los dashboards no explican suficientemente la relación entre requisito de producto, control, gate y evidencia.

**Resolución requerida.** Añadir tipo de registro, universo, fuente y regla de agregación; evitar comparar 173 y 708 como si fueran la misma población.

## 10.17. AUD-CAND-017 — Los perfiles citados para QA, H6 y Steam no existen en el proyecto suministrado

| Campo | Valor |
| --- | --- |
| Severidad | MEDIUM |
| Área | Build Profiles |
| Documentos / fuente | 10, 11, 13, 31, 34; Assets/_Project/Settings/BuildProfiles |
| Gate afectado | Sprint 16 / H6 / Steam |
| Estado | OPEN |

**Evidencia.** Solo existe Windows_Development.asset. Los registros citan Windows_QA, Windows_H6_Candidate y Windows_Steam_RC como perfiles previstos.

**Evaluación.** Los nombres son contratos futuros, no configuración implementada.

**Resolución requerida.** Mantenerlos como PLANNED hasta crear los assets; no presentar un nombre de perfil como evidencia de build.

## 10.18. AUD-CAND-018 — La implementación ES/EN no está demostrada en la fotografía del proyecto

| Campo | Valor |
| --- | --- |
| Severidad | MEDIUM |
| Área | Localización |
| Documentos / fuente | 05, 07, 08, 10, 19, 21, 22, 24, 29, 31 |
| Gate afectado | Sprint 17 / H6 / release |
| Estado | OPEN |

**Evidencia.** Assets/_Project/Content/Localization solo contiene .gitkeep; no se suministran tablas, colecciones ni keys runtime. El paquete Localization solo puede inferirse de manifiestos históricos no incluidos en el snapshot.

**Evaluación.** El plan es coherente y el estado pendiente está documentado, pero no existe evidencia de implementación.

**Resolución requerida.** Restaurar manifest/lock, crear tablas ES/EN, ejecutar pseudo-localización, clipping, fallback y pruebas en build.

## 10.19. AUD-CAND-019 — La convención de front matter no es uniforme

| Campo | Valor |
| --- | --- |
| Severidad | MEDIUM |
| Área | Formato documental |
| Documentos / fuente | 10, 27 |
| Gate afectado | Validación estructural |
| Estado | OPEN |

**Evidencia.** 10 contiene una línea vacía antes de “---”, por lo que el front matter no comienza en el primer byte; 27 carece completamente de front matter YAML.

**Evaluación.** No rompe la lectura humana, pero dificulta parsers, generación de catálogos y validación automatizada.

**Resolución requerida.** Normalizar ambos archivos al esquema usado por el resto de Markdown.

## 10.20. AUD-CAND-020 — Versiones y fechas de regeneración no siguen una regla única

| Campo | Valor |
| --- | --- |
| Severidad | MEDIUM |
| Área | Metadatos |
| Documentos / fuente | 12–15, 21, 23, 31–34 |
| Gate afectado | Trazabilidad de versión |
| Estado | OPEN |

**Evidencia.** Archivos generados durante la consolidación usan combinaciones 1.0, 1.1-RC1, 1.1-CANDIDATE y fechas 2026-07-01/2026-07-02 sin una tabla única de significado.

**Evaluación.** La diferencia puede ser legítima, pero no existe una política explícita que distinga versión de contenido, revisión candidata y fecha de exportación.

**Resolución requerida.** Definir campos version, revision/status y exported_at; registrar el cambio en 13 y 32.

## 10.21. AUD-CAND-021 — Varios documentos especializados contienen grandes bloques de boilerplate repetido

| Campo | Valor |
| --- | --- |
| Severidad | MEDIUM |
| Área | Mantenibilidad |
| Documentos / fuente | 19, 22, 25, 26, 27 |
| Gate afectado | Calidad documental |
| Estado | OPEN |

**Evidencia.** El análisis detectó párrafos largos repetidos hasta 32 veces en 19, 16 en 22, 27 en 25, 20 en 26 y 147 en 27.

**Evaluación.** La repetición no crea una contradicción directa, pero dificulta revisión, cambios y detección de diferencias reales.

**Resolución requerida.** Consolidar reglas transversales en secciones únicas y referenciarlas; conservar por capítulo solo la variación específica.

## 10.22. AUD-CAND-022 — Los estados TBD/PENDING/PLACEHOLDER no tienen una allowlist final única

| Campo | Valor |
| --- | --- |
| Severidad | MEDIUM |
| Área | Placeholders y cierre |
| Documentos / fuente | 00–34 |
| Gate afectado | Baseline final |
| Estado | OPEN |

**Evidencia.** Los documentos contienen numerosos estados pendientes, en su mayoría intencionados para S16, H6, release y paquete. No existe todavía un reporte final que distinga placeholder permitido de omisión bloqueante.

**Evaluación.** En candidata son válidos; en final deben estar clasificados y enlazados a riesgo, deuda, control o excepción.

**Resolución requerida.** Generar una allowlist en DOCUMENTATION_VALIDATION_REPORT y hacer fallar placeholders no justificados.

## 10.23. AUD-CAND-023 — Los estados mezclan español, inglés y variantes equivalentes

| Campo | Valor |
| --- | --- |
| Severidad | LOW |
| Área | Terminología |
| Documentos / fuente | 00–34 |
| Gate afectado | Mantenibilidad |
| Estado | OPEN |

**Evidencia.** Coexisten OPEN/Planned/PENDING/NOT RUN/No iniciada/Current Candidate/RC1 y formas distintas de “regenerado”.

**Evaluación.** El significado suele ser comprensible, pero dificulta fórmulas, filtros y lectura transversal.

**Resolución requerida.** Mantener catálogos canónicos por campo y traducciones solo en presentación.

## 10.24. AUD-CAND-024 — Algunos registros de procedencia contienen rutas absolutas del entorno de regeneración

| Campo | Valor |
| --- | --- |
| Severidad | LOW |
| Área | Portabilidad |
| Documentos / fuente | 32 y registros de fuente |
| Gate afectado | Paquete final / restauración |
| Estado | OPEN |

**Evidencia.** 08_Paquetes_Fuente conserva rutas /mnt/data/... como evidencia de la sesión de generación.

**Evaluación.** Son útiles durante la auditoría, pero no son rutas restaurables en otro equipo.

**Resolución requerida.** Conservar la ruta original como provenance y añadir una ruta relativa canónica dentro del paquete final.

## 10.25. AUD-CAND-025 — La estructura básica de los 27 Markdown es válida

| Campo | Valor |
| --- | --- |
| Severidad | INFORMATIONAL |
| Área | Integridad Markdown |
| Documentos / fuente | Todos los Markdown |
| Gate afectado | Validación estructural |
| Estado | VERIFIED |

**Evidencia.** No se detectaron enlaces relativos rotos, tablas mal formadas ni bloques de código sin cerrar.

**Evaluación.** PASS estructural, con las excepciones de front matter descritas en AUD-CAND-019.

**Resolución requerida.** No se requiere corrección adicional fuera de los findings registrados.

## 10.26. AUD-CAND-026 — Los ocho Excel se abren y no muestran errores de fórmula reales

| Campo | Valor |
| --- | --- |
| Severidad | INFORMATIONAL |
| Área | Integridad Excel |
| Documentos / fuente | 08, 12, 13, 21, 23, 31, 32, 33 |
| Gate afectado | Validación estructural |
| Estado | VERIFIED |

**Evidencia.** 171 hojas y 686 fórmulas inspeccionadas. La única coincidencia textual con #REF/#VALUE en 31 pertenece a una instrucción de prueba, no a un error de celda.

**Evaluación.** PASS de integridad de fórmula; los hallazgos Excel son semánticos y de estado, no errores de cálculo visibles.

**Resolución requerida.** Repetir el scan después de cada corrección.

## 10.27. AUD-CAND-027 — 32 identifica correctamente los bytes actuales de todos los documentos externos

| Campo | Valor |
| --- | --- |
| Severidad | INFORMATIONAL |
| Área | Manifiesto candidato |
| Documentos / fuente | 32 y 00–34 |
| Gate afectado | Candidata de auditoría |
| Estado | VERIFIED |

**Evidencia.** La comparación de tamaños y SHA-256 no encontró discrepancias entre 32 y los otros 34 archivos; el hash propio de 32 se mantiene externamente como corresponde.

**Evaluación.** 32 es la mejor fotografía candidata actual, aunque contiene los textos obsoletos de AUD-CAND-009 y no sustituye su futura versión final.

**Resolución requerida.** Usarlo como entrada de esta auditoría y regenerarlo al final.

## 10.28. AUD-CAND-028 — Los hechos centrales de identidad y estado siguen siendo coherentes

| Campo | Valor |
| --- | --- |
| Severidad | INFORMATIONAL |
| Área | Coherencia de producto y técnica |
| Documentos / fuente | 00–34 y ProjectSettings |
| Gate afectado | Uso operativo |
| Estado | VERIFIED |

**Evidencia.** Cartridge & Cloud, VRM Games, PC/Steam, Unity 6000.3.18f1, aplicación 0.0.17, cinco escenas, 12 asmdefs, 457 scripts de proyecto, Sprints 0–15 cerrados, Sprint 16 abierto y H6 pendiente son consistentes.

**Evaluación.** El núcleo documental sigue siendo utilizable durante la corrección de gobierno e integridad.

**Resolución requerida.** No convertir esta coherencia en PASS de H6 o baseline final.

## 10.29. AUD-CAND-029 — 31 y 33 evitan correctamente afirmar H6, release o continuidad sin evidencia

| Campo | Valor |
| --- | --- |
| Severidad | INFORMATIONAL |
| Área | Honestidad operativa |
| Documentos / fuente | 31, 33, 34 |
| Gate afectado | H6 / release / continuidad |
| Estado | VERIFIED |

**Evidencia.** Los controles permanecen NOT RUN, MISSING, OPEN, PLANNED o UNVERIFIED; 33 registra 0 acciones terminadas, 0 backups verificados y 0 ejercicios PASS.

**Evaluación.** La ausencia de ejecución es un bloqueo operativo real, pero no un false PASS documental.

**Resolución requerida.** Mantener estos estados hasta ejecutar y enlazar evidencia real.

# 11. Estado de los hallazgos de la auditoría RC1 anterior

| Grupo RC1 | Estado candidato | Resultado |
| --- | --- | --- |
| Ausencia de 33 y 34 | RESOLVED | Ambos existen y fueron regenerados. |
| Actualización de 21, 23 y 31 | RESOLVED AS CANDIDATE | Los archivos existen y sus fórmulas son válidas; quedan propagaciones finales. |
| Manifiesto 32 candidato | RESOLVED AS CANDIDATE | Hashes externos correctos; versión final pendiente. |
| Inventarios 12/13/14/15 | OPEN | Continúan desfasados respecto a archivos posteriores. |
| Cobertura end-to-end sobreestimada | OPEN | El problema persiste con 296 filas Complete. |
| False PASS de builds pendientes | PARTIALLY RESOLVED | Candidatas nuevas figuran NOT RUN/MISSING, pero las builds históricas PASS no son reproducibles. |
| StoreInitial / build profile | OPEN | Escena presente, integración y perfil candidato pendientes. |
| Localización ES/EN | OPEN | No se aportan tablas o colecciones runtime. |
| Front matter 10 | OPEN | Sigue existiendo una línea inicial vacía. |
| Política de self-hash | RESOLVED | 31–34 distinguen correctamente hashes externos. |
| Paquete final y restauración | OPEN / EXPECTED | No deben existir antes del freeze final. |

# 12. Correcciones obligatorias antes de la auditoría final

## 12.1. Prioridad BLOCKER

1. Corregir la semántica y auditoría de `13/03_Matriz_End_to_End`.
2. Actualizar los registros de estado/hashes en 12, 13, 14, 15, 31, 33 y 34 usando 32 como fotografía candidata.
3. Incorporar Packages y evidencia Git verificable a la fuente asociada a la baseline, o aprobar formalmente una excepción que limite la reproducibilidad.

## 12.2. Prioridad HIGH

1. Sincronizar 12/13 con los 192 riesgos y 186 exposiciones fuera de tolerancia de 33.
2. Actualizar 34 con 280 controles H6, 223 de release, 48 evidencias y 79 pruebas.
3. Marcar 33, 34 y 32 candidata como completados en 31; registrar esta auditoría como siguiente etapa.
4. Actualizar 33 y 34 con el hash candidato actual de 32 y con los findings aplicables.
5. Corregir 32/A2 y BL-0002.
6. Reclasificar las builds históricas como historia no reproducible y exigir integridad completa para nuevas candidatas.
7. Mantener StoreInitial, H6 y los package artifacts como bloqueos operativos explícitos.

## 12.3. Prioridad MEDIUM/LOW

1. Normalizar front matter, versiones, fechas, estados y taxonomías.
2. Reducir boilerplate repetido.
3. Crear allowlist de placeholders y rutas relativas canónicas.
4. Distinguir perfiles previstos de assets realmente existentes.

# 13. Documentos que deben cambiar como consecuencia de esta auditoría

| Documento | Cambio mínimo requerido |
| --- | --- |
| 12 | Inventario 00–34, consolidación, riesgos 192/186, siguiente acción y hashes. |
| 13 | Versiones/hashes, log y consolidación; matriz end-to-end; auditoría semántica; riesgos y builds. |
| 14 | Estados/hashes actuales, checklist CNS y secuencia post-auditoría. |
| 15 | Inventario/hashes, estados y ruta final de corrección. |
| 31 | Estado de 33/34/32, findings nuevos, gates y trazabilidad actual. |
| 33 | Hashes/estado de 32/34, siguiente acción, nuevos riesgos o agravamiento. |
| 34 | Hash 32, recuentos de 31, findings, Git/package/restore cuando existan. |
| 10 / 27 | Normalización de front matter. |
| 16 | Regenerar como auditoría final después de las correcciones. |
| 32 | Regenerar como manifiesto final después de 16 final y del último cambio. |

21 y 23 no necesitan una regeneración completa por esta auditoría salvo que las correcciones de otros documentos alteren referencias, hashes, assets, licencias o claims. Sus hashes finales deberán actualizarse en los registros superiores si cambian.

# 14. Criterios para la reauditoría final

- [ ] Cero BLOCKER abiertos.
- [ ] Cero HIGH abiertos o cada excepción formal contiene owner, alcance, compensaciones y expiración.
- [ ] 13 no contiene filas Complete sin artefacto, ejecución y evidencia.
- [ ] 12, 13, 14, 15, 31, 33 y 34 describen la misma candidata.
- [ ] 12/13/33 muestran la misma población de riesgos.
- [ ] 31 y 34 muestran los mismos recuentos operativos.
- [ ] Packages y Git son verificables, o existe excepción formal explícita.
- [ ] 32 final coincide con todos los archivos aprobados y conserva self-hash externo.
- [ ] No existen errores de fórmula, enlaces rotos, tablas inválidas o code fences abiertos.
- [ ] PACKAGE_MANIFEST, CHECKSUMS y validation report se generan después del freeze.
- [ ] El ZIP se extrae y verifica en una ubicación limpia.
- [ ] RB-16 y RB-17 producen evidencia PASS o excepción aprobada.

# 15. Veredicto y decisión recomendada

| Decisión | Resultado |
| --- | --- |
| Usar los documentos para continuar la corrección | APPROVED |
| Considerar 32 como fotografía candidata de trabajo | APPROVED WITH LIMITATIONS |
| Congelar baseline final | REJECTED |
| Generar ZIP final | REJECTED UNTIL CORRECTIONS |
| Declarar H6 o Release Readiness | REJECTED / NOT RUN |
| Ejecutar correcciones y reauditoría final | REQUIRED |

**Resultado global:** `FAIL / CANDIDATE NOT READY FOR FREEZE`.

El núcleo de producto y técnica es utilizable, y la candidata ya tiene una estructura documental completa. El rechazo se limita al cierre de baseline y a las afirmaciones de trazabilidad, reproducibilidad, H6, release y continuidad. La siguiente acción no es generar artefactos finales: es resolver los findings, actualizar los documentos dependientes y regenerar esta auditoría como versión final.

# 16. Orden exacto recomendado tras esta auditoría

1. Registrar `AUD-CAND-001–029` en 13.
2. Corregir 13/matriz y controles de auditoría.
3. Actualizar 12, 13, 14 y 15.
4. Actualizar 31 con findings y estado real.
5. Actualizar 33 con riesgos/hashes/estado y 34 con recuentos/hash 32.
6. Corregir 32 únicamente si se necesita una nueva candidata intermedia; en caso contrario reservar la siguiente regeneración para la versión final.
7. Normalizar 10 y 27; incorporar Packages/Git o aprobar excepción.
8. Reejecutar validaciones estructurales y de fórmula.
9. Regenerar `16_Auditoria_Global_de_Coherencia.md` como auditoría final.
10. Regenerar `32_Documentation_Baseline_Manifest.xlsx` como manifiesto final.
11. Generar artefactos externos, ZIP, checksums y validation report.
12. Ejecutar restauración limpia y handoff independiente.
13. Registrar commit/tag y firmar el freeze.

# 17. Apéndice — hashes actuales de documentos regenerados

| ID | Archivo | Bytes | SHA-256 candidato |
| --- | --- | ---: | --- |
| 12 | `12_Excel_Maestro_de_Produccion.xlsx` | 484932 | `3d70e97326868389dac977934e6000285c22b3c33c0e62be31313880726b4d7d` |
| 13 | `13_Trazabilidad_y_Control_de_Cambios.xlsx` | 577554 | `97cf106d7707de9e6dcadfa2ec9374676b0ea9fe0945ac87eb89db788df8da43` |
| 14 | `14_Project_Binder_Indice_Maestro.md` | 95577 | `d0e8c2b0bf78a414d00c290d8ab8e4b2742e80de7bf509764318c6b7a9e4f144` |
| 15 | `15_Guia_Maestra.md` | 149595 | `70aa63feb69785d3b2e8b044c73b9c5f075829fd198e4de5504196d2262cae71` |
| 21 | `21_Initial_Content_Catalog.xlsx` | 186599 | `2f98e2b7070e5a1f70d4feee03a7d154a2dcdeff8078b040c442e2393fc676c8` |
| 23 | `23_Legal_Credits_and_Licenses_Register.xlsx` | 489706 | `df975425a8b8c1cfd9375179893a68422c5691c95f63be5f0862b3d6ce783e63` |
| 31 | `31_H6_and_Release_Readiness_Checklist.xlsx` | 219011 | `c61a903e5a6d5944b60c4ffde403f6184bc96d23d68996798c43b7ecc9515832` |
| 32 | `32_Documentation_Baseline_Manifest.xlsx` | 203050 | `bc1f2fb749cdcabd2dcdc4f60dc0bdc22be498954ede49a870052977e6e1b658` |
| 33 | `33_Project_Risk_Register_and_Business_Continuity_Plan.xlsx` | 314777 | `2bde968afc77c8a2ad3c8b28c06fcb0542d1aedb6985b1c33139d2382e006521` |
| 34 | `34_Final_Handoff_and_Project_Operations_Manual.md` | 114714 | `9e1c620512928547658c1ea8ca889d23429fd5fcfbfdf647717dc9624cc40dcf` |

# 18. Apéndice — limitaciones de la auditoría

- No se ejecutó Unity, Test Runner ni build.
- No se abrió Steamworks ni servicios externos.
- No se verificó el commit declarado por ausencia de Git.
- No se verificó el package graph por ausencia de Packages.
- No se realizó OCR ni análisis visual de PDFs históricos; se utilizó el inventario/hashes ya preservado.
- No se certificó cumplimiento legal, privacidad, seguridad o accesibilidad; se auditó coherencia documental y estado de evidencia.

---

**Estado de este documento:** `CANDIDATE AUDIT COMPLETE / BLOCKERS OPEN / FINAL REAUDIT REQUIRED`.

**Siguiente acción:** registrar y resolver findings; actualizar documentos dependientes; regenerar esta auditoría como final antes de cerrar 32 y el paquete inmutable.

---

<!-- W0_S17_PHASE1_START -->

# 18. Auditoría dirigida W0 - 2026-07-07

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

## Resultado de coherencia documental

| Control | Resultado W0 | Observación |
|---|---|---|
| Decisiones funcionales DEC-01 a DEC-12 | `PASS DOCUMENTAL` | Formalizadas como ADR-0074 a ADR-0085 |
| Contratos afectados | `SYNCHRONIZED` | Actualización propagada sin eliminar contenido previo |
| Pruebas de caracterización | `DEFINED / NOT RUN` | Catálogo añadido a QA; no existe evidencia de ejecución |
| Implementación W1-W7 | `NOT STARTED` | Fuera del alcance de W0 |
| Regresión W8 | `NOT RUN` | Bloqueante para levantar la remediación |
| H6 / Vertical Slice | `BLOCKED` | Sin propagación de PASS |

Los hallazgos de ambigüedad funcional asociados a DEC-01 a DEC-12 quedan cerrados a nivel de decisión. Los hallazgos de implementación, evidencia, rendimiento, accesibilidad, build, persistencia o signoff permanecen abiertos hasta W1-W8.

## Pruebas de caracterización previas a las olas

| Grupo | IDs | Ola protegida | Cobertura mínima | Estado |
|---|---|---|---|---|
| Tiempo y pausa | `CHAR-TIM-001` a `CHAR-TIM-008` | W1 | reloj, velocidades, pausa anidada, HUD, transiciones y persistencia | `DEFINED / NOT RUN` |
| Clientes y checkout | `CHAR-CUS-001` a `CHAR-CUS-009` | W2 | ocho activos, checkout obligatorio, FIFO, cierre, abandono y recuperación | `DEFINED / NOT RUN` |
| Pedidos y economía | `CHAR-ODR-001` a `CHAR-ODR-010` | W3 | reserva de fondos, Process All, receipt, stock, ledger, semana e impuesto | `DEFINED / NOT RUN` |
| Displays | `CHAR-DSP-001` a `CHAR-DSP-006` | W4 | asignación única, cantidades, retorno, clear-empty y save/load | `DEFINED / NOT RUN` |
| Guardado | `CHAR-SAV-001` a `CHAR-SAV-008` | W5 | estados seguros, mutaciones pendientes, backup, recovery e idempotencia | `DEFINED / NOT RUN` |
| Management | `CHAR-MGT-001` a `CHAR-MGT-008` | W6 | día actual + 2, `DailySummary`, lifetime, scroll, filtros, ES/EN y resolución | `DEFINED / NOT RUN` |
| Autoría y settings | `CHAR-ART-001` a `CHAR-ART-007` | W7 | occlusion false, migración de preferencias, pivotes, GroundAnchor y compatibilidad | `DEFINED / NOT RUN` |
| Regresión integral | `CHAR-REG-001` a `CHAR-REG-010` | W8 | siete días, build externa, logs, save/reload, reconciliación y no duplicados | `DEFINED / NOT RUN` |

La definición documental de un caso no equivale a ejecución ni a PASS. Ninguna ola puede modificar el comportamiento protegido sin capturar primero el resultado de caracterización correspondiente.


## Regla de cierre y no propagación

- El cierre de W0 no abre W1-W7 automáticamente; cada ola requiere su propia evidencia y control de cambios.
- No se declara Sprint17_Phase1 completada mientras W8 no haya ejecutado la regresión integral.
- No se propaga `PASS` de Sprint 16 a H6 ni a la Vertical Slice.
- No se introducen sistemas Post-H6 durante la remediación.
- Código, escenas, prefabs, builds y tests ejecutables no han sido modificados por esta actualización documental.

<!-- W0_S17_PHASE1_END -->
