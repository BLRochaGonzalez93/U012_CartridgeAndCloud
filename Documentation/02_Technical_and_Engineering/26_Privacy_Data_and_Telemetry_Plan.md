---
title: "Cartridge & Cloud — Privacy, Data and Telemetry Plan"
subtitle: "Plan consolidado de privacidad, datos locales, soporte, plataforma y futura telemetría"
author: "VRM Games / Blas Luis Rocha González"
date: "2026-07-01"
lang: es-ES
document_version: "1.0"
project_version: "0.0.17"
status: "PLANNING / NO OWN TELEMETRY / ONLINE SYSTEMS NOT OPEN"
---

# Cartridge & Cloud — Privacy, Data and Telemetry Plan

**Proyecto:** Cartridge & Cloud  
**Desarrollador:** VRM Games / Blas Luis Rocha González  
**Plataforma prevista:** PC / Steam  
**Motor:** Unity 6.3 LTS `6000.3.18f1` / URP `17.3.0`  
**Versión observada:** `0.0.17`  
**Fecha de verificación normativa y de fuentes:** 1 de julio de 2026  
**Estado:** Sprints 0–15 `CLOSED / PASS`; Sprint 16 `IN PROGRESS`; Sprint 17 y H6 `PENDING`  
**Estado de datos:** `LOCAL-FIRST`; telemetría propia, backend, Cloud, analytics y crash reporting externo `NOT OPEN`

> **Aviso:** este documento es un plan técnico, operativo y de gobernanza. No sustituye asesoramiento jurídico profesional ni una evaluación concreta de la autoridad o del responsable del tratamiento. Antes de publicar una política, activar un SDK, transferir datos o tratar datos de menores debe realizarse revisión jurídica y de seguridad adecuada al caso.

## Resumen ejecutivo

Cartridge & Cloud puede mantener una primera versión **sin telemetría propia**. El juego observado es offline, no integra Steamworks, no dispone de backend, cuenta propia, servidor, newsletter, website analytics, ticketing o crash reporting externo. El save y las preferencias permanecen localmente en el equipo del jugador. Esta reducción de superficie es una decisión favorable que debe preservarse hasta que una necesidad real justifique abrir un nuevo flujo.

La ausencia de SDK no equivale a “cero datos” en sentido absoluto. Steam y las herramientas de desarrollo mantienen sus propios tratamientos; UnityConnect contiene configuración de servicios y Engine Diagnostics; `Player.log`, saves, screenshots o dumps pueden llegar al estudio si el jugador los remite voluntariamente; una futura web o email de soporte también trataría datos. Este plan registra esas fronteras y evita atribuir al estudio datos que Valve procesa por su cuenta.

La guía específica de AEPD y Autoridad Belga de junio de 2026 advierte que la telemetría y las inferencias en videojuegos pueden individualizar a jugadores y ser invisibles para ellos. Por ello, cualquier futura instrumentación debe partir de finalidad, minimización, transparencia, roles, base jurídica, retención, seguridad y capacidad de intervenir. La telemetría no es un requisito de H6 ni una condición para lanzar.

| Superficie | Estado observado | Política |
|---|---|---|
| Save | local, schema 2 (`CurrentSchemaVersion = 2`), primary/backup/temp/recovery | no se transmite salvo acción voluntaria o Cloud futura |
| PlayerPrefs | volúmenes y ocultación de paredes | local, no sensible |
| Player.log | local | soporte voluntario, redacción y retención limitada |
| UGS Analytics | no integrado; setting desactivado | `NOT OPEN` |
| Unity Cloud Diagnostics | desactivado | `NOT OPEN` |
| Engine Diagnostics | setting observado habilitado | revisar editor/herramienta y términos; no inferir player telemetry |
| Steamworks / SteamID | no integrado | `NOT OPEN` |
| Steam Cloud | no integrado | `NOT OPEN` |
| Crash SDK externo | no integrado | `NOT OPEN` |
| Soporte/ticketing | no existe | diseñar antes de publicación |
| Website/cookies/newsletter | no documentados | comenzar sin tracking |

## Convenciones de estado

| Estado | Significado |
|---|---|
| `CURRENT` | Evidencia observada en código, settings o documentos vigentes. |
| `LOCAL` | Dato permanece en el dispositivo y no lo recibe VRM Games por defecto. |
| `VOLUNTARY SUPPORT` | El usuario decide enviar un archivo o información para resolver una incidencia. |
| `PLANNED` | Diseño autorizado, todavía no implementado. |
| `NOT OPEN` | No se permite implementar ni comunicar la feature sin nuevo gate. |
| `BLOCKED` | Existe una condición explícita que impide avanzar. |
| `HISTORICAL` | Se conserva por genealogía, no gobierna el runtime actual. |

# 0. Propósito, alcance y autoridad

Este capítulo pertenece al bloque **Gobernanza y estado**. Su objetivo es convertir **propósito, alcance y autoridad** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe un plan histórico dedicado de privacidad; las baselines exigían política solo cuando hubiera recogida, y trataban telemetría, Cloud y plataforma como sistemas futuros.

**Regla de Cartridge & Cloud.** Toda decisión debe distinguir evidencia actual, intención histórica y sistema futuro. La ausencia de SDK no autoriza a afirmar que no existe ningún tratamiento externo de plataforma o herramienta.

**Procedimiento operativo.**
1. Inventariar fuentes y responsables.
2. Registrar estado CURRENT/PLANNED/NOT OPEN.
3. Separar datos locales, datos enviados y datos de terceros.
4. Asignar owner y gate.
5. Registrar específicamente el impacto de `Propósito, alcance y autoridad` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Documento versionado, hashes, decision log y mapa de autoridad.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004`.

# 1. Genealogía histórica

Este capítulo pertenece al bloque **Gobernanza y estado**. Su objetivo es convertir **genealogía histórica** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Las baselines v0.3/v0.4 exigían “política de privacidad si se recopilan datos”; v0.5/v0.6 añadieron privacidad al gate previo a Steam. Los GDD hablaban de telemetría interna y analytics como visión, mientras Setup y Sprints excluyeron Analytics y Cloud Save. Este es el primer plan dedicado.

**Regla de Cartridge & Cloud.** La genealogía se preserva sin reinterpretar la palabra “telemetría interna” como integración real. Toda referencia futura permanece VISION/NOT OPEN.

**Procedimiento operativo.**
1. Inventariar fuentes y responsables.
2. Registrar estado CURRENT/PLANNED/NOT OPEN.
3. Separar datos locales, datos enviados y datos de terceros.
4. Asignar owner y gate.
5. Registrar específicamente el impacto de `Genealogía histórica` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Tabla de versiones, citas de fuente, hash y clasificación CURRENT/HISTORICAL/VISION.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004`.

Los adjuntos voluntarios se consideran potencialmente identificables, se almacenan fuera del repositorio y se vinculan a un ticket mediante un ID interno, no mediante el nombre público del jugador.

# 2. Estado actual del proyecto

Este capítulo pertenece al bloque **Gobernanza y estado**. Su objetivo es convertir **estado actual del proyecto** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Aplicación `0.0.17`, Windows x64, Unity `6000.3.18f1`. No se encontraron Steamworks, UGS Analytics runtime, SDK externo de analytics/crash, HTTP de producto, Cloud o device fingerprinting. Unity Analytics, Cloud Diagnostics, Ads, Purchasing y Performance Reporting aparecen desactivados en `UnityConnectSettings.asset`; Engine Diagnostics está configurado, lo que requiere revisión separada de editor/herramienta.

**Regla de Cartridge & Cloud.** Se puede publicar una primera versión sin telemetría propia. Antes de afirmar “cero datos”, se verifican build, plataforma, herramientas, soporte y website.

**Procedimiento operativo.**
1. Inventariar fuentes y responsables.
2. Registrar estado CURRENT/PLANNED/NOT OPEN.
3. Separar datos locales, datos enviados y datos de terceros.
4. Asignar owner y gate.
5. Registrar específicamente el impacto de `Estado actual del proyecto` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Escaneo de código/paquetes/settings, build externa y captura de tráfico controlada.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004`.

# 3. Principios de privacidad

Este capítulo pertenece al bloque **Principios y accountability**. Su objetivo es convertir **principios de privacidad** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** La arquitectura actual es predominantemente offline y local, lo que reduce la superficie, pero soporte, Steamworks y futuros SDK pueden crear tratamientos.

**Regla de Cartridge & Cloud.** Licititud, lealtad, transparencia, finalidad, minimización, exactitud, conservación limitada, seguridad y responsabilidad proactiva se aplican desde diseño.

**Procedimiento operativo.**
1. Definir finalidad concreta.
2. Evaluar necesidad y proporcionalidad.
3. Eliminar campos “por si acaso”.
4. Documentar medidas y revisar cambios.
5. Registrar específicamente el impacto de `Principios de privacidad` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Ficha de tratamiento, revisión de riesgo, ADR y criterio de aceptación.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007`.

La clasificación aplica contexto y riesgo de inferencia. No se recopila edad exacta, salud, biometría, chat o categorías especiales para “mejorar segmentación”.

# 4. Privacidad por diseño

Este capítulo pertenece al bloque **Principios y accountability**. Su objetivo es convertir **privacidad por diseño** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El proyecto tiene una ventaja estructural: gameplay offline, sin cuenta propia ni red. Esa arquitectura debe considerarse un requisito, no una ausencia accidental.

**Regla de Cartridge & Cloud.** Cualquier feature que introduzca red, identidad o SDK debe demostrar por qué no puede resolverse localmente o mediante reporting agregado de Steam.

**Procedimiento operativo.**
1. Definir finalidad concreta.
2. Evaluar necesidad y proporcionalidad.
3. Eliminar campos “por si acaso”.
4. Documentar medidas y revisar cambios.
5. Registrar específicamente el impacto de `Privacidad por diseño` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** ADR, data-flow antes/después, threat model, minimization review y decisión.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007`.

# 5. Minimización

Este capítulo pertenece al bloque **Principios y accountability**. Su objetivo es convertir **minimización** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save actual necesita estado del juego, no identidad real. Los PlayerPrefs observados guardan volumen y ocultación de paredes. Ninguna métrica futura debe añadir nombre, email, SteamID o dispositivo si basta con un agregado.

**Regla de Cartridge & Cloud.** Se prohíben campos libres y atributos “nice to have”. Los schemas se podan periódicamente y se retiran campos sin consumidor.

**Procedimiento operativo.**
1. Definir finalidad concreta.
2. Evaluar necesidad y proporcionalidad.
3. Eliminar campos “por si acaso”.
4. Documentar medidas y revisar cambios.
5. Registrar específicamente el impacto de `Minimización` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Data dictionary con justificación por campo y reporte de campos eliminados.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007`.

# 6. Limitación de finalidad

Este capítulo pertenece al bloque **Principios y accountability**. Su objetivo es convertir **limitación de finalidad** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** La arquitectura actual es predominantemente offline y local, lo que reduce la superficie, pero soporte, Steamworks y futuros SDK pueden crear tratamientos.

**Regla de Cartridge & Cloud.** Licititud, lealtad, transparencia, finalidad, minimización, exactitud, conservación limitada, seguridad y responsabilidad proactiva se aplican desde diseño.

**Procedimiento operativo.**
1. Definir finalidad concreta.
2. Evaluar necesidad y proporcionalidad.
3. Eliminar campos “por si acaso”.
4. Documentar medidas y revisar cambios.
5. Registrar específicamente el impacto de `Limitación de finalidad` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Ficha de tratamiento, revisión de riesgo, ADR y criterio de aceptación.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007`.

# 7. Exactitud

Este capítulo pertenece al bloque **Principios y accountability**. Su objetivo es convertir **exactitud** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** La arquitectura actual es predominantemente offline y local, lo que reduce la superficie, pero soporte, Steamworks y futuros SDK pueden crear tratamientos.

**Regla de Cartridge & Cloud.** Licititud, lealtad, transparencia, finalidad, minimización, exactitud, conservación limitada, seguridad y responsabilidad proactiva se aplican desde diseño.

**Procedimiento operativo.**
1. Definir finalidad concreta.
2. Evaluar necesidad y proporcionalidad.
3. Eliminar campos “por si acaso”.
4. Documentar medidas y revisar cambios.
5. Registrar específicamente el impacto de `Exactitud` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Ficha de tratamiento, revisión de riesgo, ADR y criterio de aceptación.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007`.

# 8. Retención

Este capítulo pertenece al bloque **Principios y accountability**. Su objetivo es convertir **retención** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** La arquitectura actual es predominantemente offline y local, lo que reduce la superficie, pero soporte, Steamworks y futuros SDK pueden crear tratamientos.

**Regla de Cartridge & Cloud.** Licititud, lealtad, transparencia, finalidad, minimización, exactitud, conservación limitada, seguridad y responsabilidad proactiva se aplican desde diseño.

**Procedimiento operativo.**
1. Definir finalidad concreta.
2. Evaluar necesidad y proporcionalidad.
3. Eliminar campos “por si acaso”.
4. Documentar medidas y revisar cambios.
5. Registrar específicamente el impacto de `Retención` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Ficha de tratamiento, revisión de riesgo, ADR y criterio de aceptación.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007`.

El borrado cubre índices, objetos primarios y copias operativas; los backups tienen una ventana documentada y no se restauran para reactivar datos que debían eliminarse.

# 9. Seguridad

Este capítulo pertenece al bloque **Principios y accountability**. Su objetivo es convertir **seguridad** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** La arquitectura actual es predominantemente offline y local, lo que reduce la superficie, pero soporte, Steamworks y futuros SDK pueden crear tratamientos.

**Regla de Cartridge & Cloud.** Licititud, lealtad, transparencia, finalidad, minimización, exactitud, conservación limitada, seguridad y responsabilidad proactiva se aplican desde diseño.

**Procedimiento operativo.**
1. Definir finalidad concreta.
2. Evaluar necesidad y proporcionalidad.
3. Eliminar campos “por si acaso”.
4. Documentar medidas y revisar cambios.
5. Registrar específicamente el impacto de `Seguridad` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Ficha de tratamiento, revisión de riesgo, ADR y criterio de aceptación.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007`.

Los controles incluyen least privilege, MFA, segregación de entornos, inventario de secretos, rotación, logs de acceso, restauración y simulacros, con evidencia fechada.

# 10. Transparencia

Este capítulo pertenece al bloque **Principios y accountability**. Su objetivo es convertir **transparencia** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** La arquitectura actual es predominantemente offline y local, lo que reduce la superficie, pero soporte, Steamworks y futuros SDK pueden crear tratamientos.

**Regla de Cartridge & Cloud.** Licititud, lealtad, transparencia, finalidad, minimización, exactitud, conservación limitada, seguridad y responsabilidad proactiva se aplican desde diseño.

**Procedimiento operativo.**
1. Definir finalidad concreta.
2. Evaluar necesidad y proporcionalidad.
3. Eliminar campos “por si acaso”.
4. Documentar medidas y revisar cambios.
5. Registrar específicamente el impacto de `Transparencia` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Ficha de tratamiento, revisión de riesgo, ADR y criterio de aceptación.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007`.

La información se presenta por capas: resumen contextual en el punto de recogida, detalle completo accesible y versión archivada. El texto no usa finalidades vagas como “mejorar la experiencia” sin explicar datos y uso.

# 11. Responsabilidad

Este capítulo pertenece al bloque **Principios y accountability**. Su objetivo es convertir **responsabilidad** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** La arquitectura actual es predominantemente offline y local, lo que reduce la superficie, pero soporte, Steamworks y futuros SDK pueden crear tratamientos.

**Regla de Cartridge & Cloud.** Licititud, lealtad, transparencia, finalidad, minimización, exactitud, conservación limitada, seguridad y responsabilidad proactiva se aplican desde diseño.

**Procedimiento operativo.**
1. Definir finalidad concreta.
2. Evaluar necesidad y proporcionalidad.
3. Eliminar campos “por si acaso”.
4. Documentar medidas y revisar cambios.
5. Registrar específicamente el impacto de `Responsabilidad` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Ficha de tratamiento, revisión de riesgo, ADR y criterio de aceptación.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007`.

# 12. Datos personales

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **datos personales** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save contiene estado de juego, IDs sintéticos y timestamps; no contiene nombres humanos, email, SteamID ni datos de dispositivo observados. Logs y archivos de soporte podrían contener datos incidentales.

**Regla de Cartridge & Cloud.** La clasificación depende de identificabilidad y contexto, no del nombre del campo. Un ID persistente, una IP o un patrón conductual pueden ser datos personales.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Datos personales` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diccionario de datos, ejemplo de payload y matriz de acceso.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

| Tipo | Ejemplo | Estado actual |
|---|---|---|
| Identificador directo | email de soporte | no existe / futuro |
| Identificador de plataforma | SteamID | no recogido |
| Identificador indirecto | IP, device fingerprint | no recogido por juego |
| Datos conductuales | secuencia de acciones por jugador | no recogida remotamente |
| Datos aportados | log/save/screenshot | solo soporte voluntario futuro |

# 13. Datos no personales

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **datos no personales** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save contiene estado de juego, IDs sintéticos y timestamps; no contiene nombres humanos, email, SteamID ni datos de dispositivo observados. Logs y archivos de soporte podrían contener datos incidentales.

**Regla de Cartridge & Cloud.** La clasificación depende de identificabilidad y contexto, no del nombre del campo. Un ID persistente, una IP o un patrón conductual pueden ser datos personales.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Datos no personales` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diccionario de datos, ejemplo de payload y matriz de acceso.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

Un dato agregado puede ser no personal solo si no permite identificar razonablemente a una persona. Conteos globales con umbrales y sin cohortes pequeñas pueden acercarse a esa condición; un ID hashado, un evento individual o una secuencia única no se clasifican automáticamente como no personales.

# 14. Pseudonimización

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **pseudonimización** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Un hash, SteamID transformado o identificador estable sigue siendo dato personal cuando puede vincularse o combinarse. La seudonimización reduce riesgo, pero no saca el tratamiento del RGPD.

**Regla de Cartridge & Cloud.** La tabla de reidentificación, salt o secreto se separa, limita y rota. No se usa el mismo ID entre títulos/campañas sin necesidad.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Pseudonimización` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diseño criptográfico revisado, key custody y prueba de rotación/borrado.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

# 15. Anonimización

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **anonimización** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Solo se denomina anónimo a un conjunto cuando la identificación ya no es razonablemente posible considerando datos auxiliares y singularidad conductual. Borrar el nombre no basta.

**Regla de Cartridge & Cloud.** Los agregados aplican umbrales, generalización y revisión de unicidad. Si persiste linkability, se trata como dato personal.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Anonimización` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Anonymisation assessment, test de singularidad y decisión firmada.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

# 16. Identificadores

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **identificadores** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save contiene estado de juego, IDs sintéticos y timestamps; no contiene nombres humanos, email, SteamID ni datos de dispositivo observados. Logs y archivos de soporte podrían contener datos incidentales.

**Regla de Cartridge & Cloud.** La clasificación depende de identificabilidad y contexto, no del nombre del campo. Un ID persistente, una IP o un patrón conductual pueden ser datos personales.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Identificadores` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diccionario de datos, ejemplo de payload y matriz de acceso.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

# 17. Datos técnicos

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **datos técnicos** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save contiene estado de juego, IDs sintéticos y timestamps; no contiene nombres humanos, email, SteamID ni datos de dispositivo observados. Logs y archivos de soporte podrían contener datos incidentales.

**Regla de Cartridge & Cloud.** La clasificación depende de identificabilidad y contexto, no del nombre del campo. Un ID persistente, una IP o un patrón conductual pueden ser datos personales.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Datos técnicos` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diccionario de datos, ejemplo de payload y matriz de acceso.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

| Campo técnico | Necesidad potencial | Riesgo | Política |
|---|---|---|---|
| versión/build | reproducir defectos | bajo | permitido en soporte |
| OS | compatibilidad | medio | versión general, no serial |
| GPU/CPU | rendimiento | medio | modelo, no device ID |
| resolución | UI/performance | bajo | bucket |
| branch/manifest | diagnóstico | bajo | interno/soporte |
| IP | transporte/seguridad | alto | no almacenar por defecto |

# 18. Datos de dispositivo

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **datos de dispositivo** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save contiene estado de juego, IDs sintéticos y timestamps; no contiene nombres humanos, email, SteamID ni datos de dispositivo observados. Logs y archivos de soporte podrían contener datos incidentales.

**Regla de Cartridge & Cloud.** La clasificación depende de identificabilidad y contexto, no del nombre del campo. Un ID persistente, una IP o un patrón conductual pueden ser datos personales.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Datos de dispositivo` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diccionario de datos, ejemplo de payload y matriz de acceso.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

# 19. Sistema operativo

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **sistema operativo** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save contiene estado de juego, IDs sintéticos y timestamps; no contiene nombres humanos, email, SteamID ni datos de dispositivo observados. Logs y archivos de soporte podrían contener datos incidentales.

**Regla de Cartridge & Cloud.** La clasificación depende de identificabilidad y contexto, no del nombre del campo. Un ID persistente, una IP o un patrón conductual pueden ser datos personales.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Sistema operativo` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diccionario de datos, ejemplo de payload y matriz de acceso.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

# 20. Hardware

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **hardware** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save contiene estado de juego, IDs sintéticos y timestamps; no contiene nombres humanos, email, SteamID ni datos de dispositivo observados. Logs y archivos de soporte podrían contener datos incidentales.

**Regla de Cartridge & Cloud.** La clasificación depende de identificabilidad y contexto, no del nombre del campo. Un ID persistente, una IP o un patrón conductual pueden ser datos personales.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Hardware` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diccionario de datos, ejemplo de payload y matriz de acceso.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

# 21. Resolución

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **resolución** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save contiene estado de juego, IDs sintéticos y timestamps; no contiene nombres humanos, email, SteamID ni datos de dispositivo observados. Logs y archivos de soporte podrían contener datos incidentales.

**Regla de Cartridge & Cloud.** La clasificación depende de identificabilidad y contexto, no del nombre del campo. Un ID persistente, una IP o un patrón conductual pueden ser datos personales.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Resolución` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diccionario de datos, ejemplo de payload y matriz de acceso.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

# 22. Idioma

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **idioma** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save contiene estado de juego, IDs sintéticos y timestamps; no contiene nombres humanos, email, SteamID ni datos de dispositivo observados. Logs y archivos de soporte podrían contener datos incidentales.

**Regla de Cartridge & Cloud.** La clasificación depende de identificabilidad y contexto, no del nombre del campo. Un ID persistente, una IP o un patrón conductual pueden ser datos personales.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Idioma` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diccionario de datos, ejemplo de payload y matriz de acceso.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

# 23. Versión

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **versión** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save contiene estado de juego, IDs sintéticos y timestamps; no contiene nombres humanos, email, SteamID ni datos de dispositivo observados. Logs y archivos de soporte podrían contener datos incidentales.

**Regla de Cartridge & Cloud.** La clasificación depende de identificabilidad y contexto, no del nombre del campo. Un ID persistente, una IP o un patrón conductual pueden ser datos personales.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Versión` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diccionario de datos, ejemplo de payload y matriz de acceso.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

# 24. Branch

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **branch** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save contiene estado de juego, IDs sintéticos y timestamps; no contiene nombres humanos, email, SteamID ni datos de dispositivo observados. Logs y archivos de soporte podrían contener datos incidentales.

**Regla de Cartridge & Cloud.** La clasificación depende de identificabilidad y contexto, no del nombre del campo. Un ID persistente, una IP o un patrón conductual pueden ser datos personales.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Branch` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diccionario de datos, ejemplo de payload y matriz de acceso.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

# 25. Manifest ID

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **manifest id** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save contiene estado de juego, IDs sintéticos y timestamps; no contiene nombres humanos, email, SteamID ni datos de dispositivo observados. Logs y archivos de soporte podrían contener datos incidentales.

**Regla de Cartridge & Cloud.** La clasificación depende de identificabilidad y contexto, no del nombre del campo. Un ID persistente, una IP o un patrón conductual pueden ser datos personales.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Manifest ID` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diccionario de datos, ejemplo de payload y matriz de acceso.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

Los informes de Steam se tratan como una fuente separada: el estudio registra qué campos recibe, si son agregados y quién tiene permiso; no infiere acceso a datos individuales por el mero hecho de usar Steamworks.

# 26. App ID

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **app id** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save contiene estado de juego, IDs sintéticos y timestamps; no contiene nombres humanos, email, SteamID ni datos de dispositivo observados. Logs y archivos de soporte podrían contener datos incidentales.

**Regla de Cartridge & Cloud.** La clasificación depende de identificabilidad y contexto, no del nombre del campo. Un ID persistente, una IP o un patrón conductual pueden ser datos personales.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `App ID` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diccionario de datos, ejemplo de payload y matriz de acceso.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

Los informes de Steam se tratan como una fuente separada: el estudio registra qué campos recibe, si son agregados y quién tiene permiso; no infiere acceso a datos individuales por el mero hecho de usar Steamworks.

# 27. Steam ID

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **steam id** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No se recoge actualmente. Una futura autenticación Steam puede entregar SteamID y permitir vincular propiedad, cuenta o backend. SteamID es un identificador persistente y no debe tratarse como anónimo.

**Regla de Cartridge & Cloud.** No se obtiene ni almacena SteamID salvo feature aprobada. Si se almacena, se implementan acceso/supresión y seguimiento de cuentas eliminadas cuando sea aplicable.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Steam ID` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Integración Steam, RAT, mapping, delete workflow y prueba con cuenta eliminada.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

Los informes de Steam se tratan como una fuente separada: el estudio registra qué campos recibe, si son agregados y quién tiene permiso; no infiere acceso a datos individuales por el mero hecho de usar Steamworks.

# 28. Nombre visible

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **nombre visible** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save contiene estado de juego, IDs sintéticos y timestamps; no contiene nombres humanos, email, SteamID ni datos de dispositivo observados. Logs y archivos de soporte podrían contener datos incidentales.

**Regla de Cartridge & Cloud.** La clasificación depende de identificabilidad y contexto, no del nombre del campo. Un ID persistente, una IP o un patrón conductual pueden ser datos personales.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Nombre visible` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diccionario de datos, ejemplo de payload y matriz de acceso.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

La clasificación aplica contexto y riesgo de inferencia. No se recopila edad exacta, salud, biometría, chat o categorías especiales para “mejorar segmentación”.

# 29. Email

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **email** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save contiene estado de juego, IDs sintéticos y timestamps; no contiene nombres humanos, email, SteamID ni datos de dispositivo observados. Logs y archivos de soporte podrían contener datos incidentales.

**Regla de Cartridge & Cloud.** La clasificación depende de identificabilidad y contexto, no del nombre del campo. Un ID persistente, una IP o un patrón conductual pueden ser datos personales.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Email` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diccionario de datos, ejemplo de payload y matriz de acceso.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

La clasificación aplica contexto y riesgo de inferencia. No se recopila edad exacta, salud, biometría, chat o categorías especiales para “mejorar segmentación”.

# 30. Dirección IP

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **dirección ip** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save contiene estado de juego, IDs sintéticos y timestamps; no contiene nombres humanos, email, SteamID ni datos de dispositivo observados. Logs y archivos de soporte podrían contener datos incidentales.

**Regla de Cartridge & Cloud.** La clasificación depende de identificabilidad y contexto, no del nombre del campo. Un ID persistente, una IP o un patrón conductual pueden ser datos personales.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Dirección IP` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diccionario de datos, ejemplo de payload y matriz de acceso.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

La clasificación aplica contexto y riesgo de inferencia. No se recopila edad exacta, salud, biometría, chat o categorías especiales para “mejorar segmentación”.

# 31. Logs

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **logs** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** `Player.log` y logs de build existen localmente. Pueden incluir rutas, nombres de carpetas, stack traces o datos incidentales según entorno. No hay envío automático observado.

**Regla de Cartridge & Cloud.** El soporte solicita el mínimo intervalo, explica cómo revisar/redactar y elimina el archivo tras resolución. Los logs de producto no incluyen texto libre del jugador.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Logs` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Log sanitizado, hash, ticket, acceso y deletion record.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

Los adjuntos voluntarios se consideran potencialmente identificables, se almacenan fuera del repositorio y se vinculan a un ticket mediante un ID interno, no mediante el nombre público del jugador.

# 32. Saves

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **saves** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Los saves viven bajo `Application.persistentDataPath/IntegratedSaveGames` con primary, `.bak`, `.tmp` y `.recovery`, schema 2 (`CurrentSchemaVersion = 2`), IDs sintéticos, timestamps UTC y estado económico/jugable. Permanecen en el equipo salvo envío voluntario o futura Cloud.

**Regla de Cartridge & Cloud.** No se piden saves completos por defecto. Para soporte se recibe una copia, se trabaja en entorno aislado y se elimina según schedule.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Saves` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Save data dictionary, consent/support notice, hash y deletion record.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

Los adjuntos voluntarios se consideran potencialmente identificables, se almacenan fuera del repositorio y se vinculan a un ticket mediante un ID interno, no mediante el nombre público del jugador.

# 33. Crash dumps

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **crash dumps** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe crash SDK externo. Un dump puede contener memoria, rutas, nombres y fragmentos inesperados; es más sensible que un contador de crash.

**Regla de Cartridge & Cloud.** No se habilita captura/subida automática sin proveedor, DPA, política, límites y evaluación. Para soporte manual, se informa antes de solicitarlo.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Crash dumps` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Crash data map, retention, access, vendor contract y redaction procedure.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

Los adjuntos voluntarios se consideran potencialmente identificables, se almacenan fuera del repositorio y se vinculan a un ticket mediante un ID interno, no mediante el nombre público del jugador.

# 34. Capturas

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **capturas** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save contiene estado de juego, IDs sintéticos y timestamps; no contiene nombres humanos, email, SteamID ni datos de dispositivo observados. Logs y archivos de soporte podrían contener datos incidentales.

**Regla de Cartridge & Cloud.** La clasificación depende de identificabilidad y contexto, no del nombre del campo. Un ID persistente, una IP o un patrón conductual pueden ser datos personales.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Capturas` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diccionario de datos, ejemplo de payload y matriz de acceso.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

Los adjuntos voluntarios se consideran potencialmente identificables, se almacenan fuera del repositorio y se vinculan a un ticket mediante un ID interno, no mediante el nombre público del jugador.

# 35. Tickets

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **tickets** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save contiene estado de juego, IDs sintéticos y timestamps; no contiene nombres humanos, email, SteamID ni datos de dispositivo observados. Logs y archivos de soporte podrían contener datos incidentales.

**Regla de Cartridge & Cloud.** La clasificación depende de identificabilidad y contexto, no del nombre del campo. Un ID persistente, una IP o un patrón conductual pueden ser datos personales.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Tickets` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diccionario de datos, ejemplo de payload y matriz de acceso.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

Los adjuntos voluntarios se consideran potencialmente identificables, se almacenan fuera del repositorio y se vinculan a un ticket mediante un ID interno, no mediante el nombre público del jugador.

# 36. Community Hub

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **community hub** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save contiene estado de juego, IDs sintéticos y timestamps; no contiene nombres humanos, email, SteamID ni datos de dispositivo observados. Logs y archivos de soporte podrían contener datos incidentales.

**Regla de Cartridge & Cloud.** La clasificación depende de identificabilidad y contexto, no del nombre del campo. Un ID persistente, una IP o un patrón conductual pueden ser datos personales.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Community Hub` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diccionario de datos, ejemplo de payload y matriz de acceso.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

# 37. Reviews

Este capítulo pertenece al bloque **Clasificación de datos y superficies**. Su objetivo es convertir **reviews** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El save contiene estado de juego, IDs sintéticos y timestamps; no contiene nombres humanos, email, SteamID ni datos de dispositivo observados. Logs y archivos de soporte podrían contener datos incidentales.

**Regla de Cartridge & Cloud.** La clasificación depende de identificabilidad y contexto, no del nombre del campo. Un ID persistente, una IP o un patrón conductual pueden ser datos personales.

**Procedimiento operativo.**
1. Clasificar dato y origen.
2. Identificar quién puede vincularlo.
3. Definir exposición y retención.
4. Marcar si se comparte o permanece local.
5. Registrar específicamente el impacto de `Reviews` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Diccionario de datos, ejemplo de payload y matriz de acceso.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-006; PRIV-OFF-009`.

# 38. Telemetría

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **telemetría** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe integración activa. La AEPD describe la telemetría como monitorización continua que puede individualizar y perfilar; por ello no se trata como simple logging técnico.

**Regla de Cartridge & Cloud.** La primera release deberá funcionar sin telemetría propia. Si se abre, será modular, granular, revisable y desligada del gameplay esencial salvo necesidad demostrada.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Telemetría` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Feature flag off, event catalog, legal assessment, policy, UI y network test.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

La ficha de métrica documenta pregunta, decisión, población, denominador, ventana, timezone, sesgo, coste de privacidad y condición de retirada; sin esa ficha el evento no puede abrirse.

# 39. Analytics

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **analytics** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El módulo built-in `com.unity.modules.unityanalytics` aparece en manifest, pero Unity Analytics está desactivado y no se encontró `Unity.Services.Analytics`. La presencia de un módulo no equivale a recogida activa.

**Regla de Cartridge & Cloud.** No activar Analytics desde Dashboard, package o código sin actualizar arquitectura, RAT, contratos y política.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Analytics` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Settings export, package lock, dashboard screenshot y traffic capture.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

La ficha de métrica documenta pregunta, decisión, población, denominador, ventana, timezone, sesgo, coste de privacidad y condición de retirada; sin esa ficha el evento no puede abrirse.

# 40. Métricas de gameplay

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **métricas de gameplay** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No se encontró telemetría runtime, analytics de UGS, SDK externo, Steamworks ni HTTP de producto. Steamworks futuro puede ofrecer datos agregados de tráfico, ventas y reembolsos.

**Regla de Cartridge & Cloud.** No se recopila un evento porque sea fácil. Cada métrica debe responder a una pregunta y conducir a una decisión autorizada.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Métricas de gameplay` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Metric card, query reproducible, data owner y decisión asociada.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

La ficha de métrica documenta pregunta, decisión, población, denominador, ventana, timezone, sesgo, coste de privacidad y condición de retirada; sin esa ficha el evento no puede abrirse.

# 41. Economía

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **economía** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Las métricas económicas pueden revelar estrategias y patrones si se vinculan a una persona. Para tuning suele bastar distribución agregada de caja, margen, rotación y resultados por día.

**Regla de Cartridge & Cloud.** No enviar ledger completo ni secuencias de compra por usuario. Preferir histogramas/buckets y análisis local en QA.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Economía` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Metric card, bucket design, aggregation threshold y query.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

# 42. Progreso

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **progreso** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No se encontró telemetría runtime, analytics de UGS, SDK externo, Steamworks ni HTTP de producto. Steamworks futuro puede ofrecer datos agregados de tráfico, ventas y reembolsos.

**Regla de Cartridge & Cloud.** No se recopila un evento porque sea fácil. Cada métrica debe responder a una pregunta y conducir a una decisión autorizada.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Progreso` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Metric card, query reproducible, data owner y decisión asociada.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

# 43. Sesiones

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **sesiones** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No se encontró telemetría runtime, analytics de UGS, SDK externo, Steamworks ni HTTP de producto. Steamworks futuro puede ofrecer datos agregados de tráfico, ventas y reembolsos.

**Regla de Cartridge & Cloud.** No se recopila un evento porque sea fácil. Cada métrica debe responder a una pregunta y conducir a una decisión autorizada.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Sesiones` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Metric card, query reproducible, data owner y decisión asociada.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

# 44. Retención de jugadores

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **retención de jugadores** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No se encontró telemetría runtime, analytics de UGS, SDK externo, Steamworks ni HTTP de producto. Steamworks futuro puede ofrecer datos agregados de tráfico, ventas y reembolsos.

**Regla de Cartridge & Cloud.** No se recopila un evento porque sea fácil. Cada métrica debe responder a una pregunta y conducir a una decisión autorizada.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Retención de jugadores` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Metric card, query reproducible, data owner y decisión asociada.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

El borrado cubre índices, objetos primarios y copias operativas; los backups tienen una ventana documentada y no se restauran para reactivar datos que debían eliminarse.

# 45. Abandono

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **abandono** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No se encontró telemetría runtime, analytics de UGS, SDK externo, Steamworks ni HTTP de producto. Steamworks futuro puede ofrecer datos agregados de tráfico, ventas y reembolsos.

**Regla de Cartridge & Cloud.** No se recopila un evento porque sea fácil. Cada métrica debe responder a una pregunta y conducir a una decisión autorizada.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Abandono` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Metric card, query reproducible, data owner y decisión asociada.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

# 46. Rendimiento

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **rendimiento** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No se encontró telemetría runtime, analytics de UGS, SDK externo, Steamworks ni HTTP de producto. Steamworks futuro puede ofrecer datos agregados de tráfico, ventas y reembolsos.

**Regla de Cartridge & Cloud.** No se recopila un evento porque sea fácil. Cada métrica debe responder a una pregunta y conducir a una decisión autorizada.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Rendimiento` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Metric card, query reproducible, data owner y decisión asociada.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

# 47. Errores

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **errores** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No se encontró telemetría runtime, analytics de UGS, SDK externo, Steamworks ni HTTP de producto. Steamworks futuro puede ofrecer datos agregados de tráfico, ventas y reembolsos.

**Regla de Cartridge & Cloud.** No se recopila un evento porque sea fácil. Cada métrica debe responder a una pregunta y conducir a una decisión autorizada.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Errores` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Metric card, query reproducible, data owner y decisión asociada.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

# 48. Softlocks

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **softlocks** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No se encontró telemetría runtime, analytics de UGS, SDK externo, Steamworks ni HTTP de producto. Steamworks futuro puede ofrecer datos agregados de tráfico, ventas y reembolsos.

**Regla de Cartridge & Cloud.** No se recopila un evento porque sea fácil. Cada métrica debe responder a una pregunta y conducir a una decisión autorizada.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Softlocks` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Metric card, query reproducible, data owner y decisión asociada.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

# 49. Stockouts

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **stockouts** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No se encontró telemetría runtime, analytics de UGS, SDK externo, Steamworks ni HTTP de producto. Steamworks futuro puede ofrecer datos agregados de tráfico, ventas y reembolsos.

**Regla de Cartridge & Cloud.** No se recopila un evento porque sea fácil. Cada métrica debe responder a una pregunta y conducir a una decisión autorizada.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Stockouts` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Metric card, query reproducible, data owner y decisión asociada.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

# 50. Conversiones

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **conversiones** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No se encontró telemetría runtime, analytics de UGS, SDK externo, Steamworks ni HTTP de producto. Steamworks futuro puede ofrecer datos agregados de tráfico, ventas y reembolsos.

**Regla de Cartridge & Cloud.** No se recopila un evento porque sea fácil. Cada métrica debe responder a una pregunta y conducir a una decisión autorizada.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Conversiones` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Metric card, query reproducible, data owner y decisión asociada.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

Los informes de Steam se tratan como una fuente separada: el estudio registra qué campos recibe, si son agregados y quién tiene permiso; no infiere acceso a datos individuales por el mero hecho de usar Steamworks.

# 51. Wishlists

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **wishlists** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Steamworks ofrece cifras de plataforma; no requieren que el juego instrumente al jugador. Se consumen como datos comerciales agregados bajo permisos de partner.

**Regla de Cartridge & Cloud.** No intentar reconstruir identidades ni combinar pequeñas cohortes. Acceso restringido a quienes toman decisiones de marketing.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Wishlists` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Steam report snapshot, permission review y analysis note.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

Los informes de Steam se tratan como una fuente separada: el estudio registra qué campos recibe, si son agregados y quién tiene permiso; no infiere acceso a datos individuales por el mero hecho de usar Steamworks.

# 52. Ventas

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **ventas** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No se encontró telemetría runtime, analytics de UGS, SDK externo, Steamworks ni HTTP de producto. Steamworks futuro puede ofrecer datos agregados de tráfico, ventas y reembolsos.

**Regla de Cartridge & Cloud.** No se recopila un evento porque sea fácil. Cada métrica debe responder a una pregunta y conducir a una decisión autorizada.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Ventas` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Metric card, query reproducible, data owner y decisión asociada.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

Los informes de Steam se tratan como una fuente separada: el estudio registra qué campos recibe, si son agregados y quién tiene permiso; no infiere acceso a datos individuales por el mero hecho de usar Steamworks.

# 53. Reembolsos

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **reembolsos** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No se encontró telemetría runtime, analytics de UGS, SDK externo, Steamworks ni HTTP de producto. Steamworks futuro puede ofrecer datos agregados de tráfico, ventas y reembolsos.

**Regla de Cartridge & Cloud.** No se recopila un evento porque sea fácil. Cada métrica debe responder a una pregunta y conducir a una decisión autorizada.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Reembolsos` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Metric card, query reproducible, data owner y decisión asociada.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

Los informes de Steam se tratan como una fuente separada: el estudio registra qué campos recibe, si son agregados y quién tiene permiso; no infiere acceso a datos individuales por el mero hecho de usar Steamworks.

# 54. Datos de Steam

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **datos de steam** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Valve es responsable de sus tratamientos de plataforma y ofrece Privacy Dashboard. El estudio solo debe documentar los datos que recibe o decide tratar, como informes agregados o SteamID si integra APIs.

**Regla de Cartridge & Cloud.** No copiar datos de Steam a sistemas propios sin finalidad y contrato. Separar “Valve procesa” de “VRM Games recibe”.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Datos de Steam` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Steam data-flow matrix, API inventory and permissions.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

Los informes de Steam se tratan como una fuente separada: el estudio registra qué campos recibe, si son agregados y quién tiene permiso; no infiere acceso a datos individuales por el mero hecho de usar Steamworks.

# 55. Datos propios

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **datos propios** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No se encontró telemetría runtime, analytics de UGS, SDK externo, Steamworks ni HTTP de producto. Steamworks futuro puede ofrecer datos agregados de tráfico, ventas y reembolsos.

**Regla de Cartridge & Cloud.** No se recopila un evento porque sea fácil. Cada métrica debe responder a una pregunta y conducir a una decisión autorizada.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Datos propios` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Metric card, query reproducible, data owner y decisión asociada.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

# 56. Datos derivados

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **datos derivados** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No se encontró telemetría runtime, analytics de UGS, SDK externo, Steamworks ni HTTP de producto. Steamworks futuro puede ofrecer datos agregados de tráfico, ventas y reembolsos.

**Regla de Cartridge & Cloud.** No se recopila un evento porque sea fácil. Cada métrica debe responder a una pregunta y conducir a una decisión autorizada.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Datos derivados` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Metric card, query reproducible, data owner y decisión asociada.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

# 57. Datos sensibles

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **datos sensibles** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No se encontró telemetría runtime, analytics de UGS, SDK externo, Steamworks ni HTTP de producto. Steamworks futuro puede ofrecer datos agregados de tráfico, ventas y reembolsos.

**Regla de Cartridge & Cloud.** No se recopila un evento porque sea fácil. Cada métrica debe responder a una pregunta y conducir a una decisión autorizada.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Datos sensibles` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Metric card, query reproducible, data owner y decisión asociada.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

La clasificación aplica contexto y riesgo de inferencia. No se recopila edad exacta, salud, biometría, chat o categorías especiales para “mejorar segmentación”.

# 58. Datos de menores

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **datos de menores** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No se encontró telemetría runtime, analytics de UGS, SDK externo, Steamworks ni HTTP de producto. Steamworks futuro puede ofrecer datos agregados de tráfico, ventas y reembolsos.

**Regla de Cartridge & Cloud.** No se recopila un evento porque sea fácil. Cada métrica debe responder a una pregunta y conducir a una decisión autorizada.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Datos de menores` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Metric card, query reproducible, data owner y decisión asociada.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

La clasificación aplica contexto y riesgo de inferencia. No se recopila edad exacta, salud, biometría, chat o categorías especiales para “mejorar segmentación”.

# 59. Datos prohibidos

Este capítulo pertenece al bloque **Telemetría, métricas y datos comerciales**. Su objetivo es convertir **datos prohibidos** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No se encontró telemetría runtime, analytics de UGS, SDK externo, Steamworks ni HTTP de producto. Steamworks futuro puede ofrecer datos agregados de tráfico, ventas y reembolsos.

**Regla de Cartridge & Cloud.** No se recopila un evento porque sea fácil. Cada métrica debe responder a una pregunta y conducir a una decisión autorizada.

**Procedimiento operativo.**
1. Formular pregunta y decisión.
2. Elegir métrica mínima.
3. Definir denominador y ventana.
4. Evaluar privacidad y sesgos.
5. Aprobar o rechazar instrumentación.
6. Registrar específicamente el impacto de `Datos prohibidos` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Metric card, query reproducible, data owner y decisión asociada.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-010; PRIV-OFF-011; PRIV-OFF-012`.

La clasificación aplica contexto y riesgo de inferencia. No se recopila edad exacta, salud, biometría, chat o categorías especiales para “mejorar segmentación”.

# 60. Fuentes de datos

Este capítulo pertenece al bloque **Fuentes, plataformas y roles**. Su objetivo es convertir **fuentes de datos** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Unity, Steam y futuros proveedores pueden actuar con roles distintos por actividad. El proyecto no dispone hoy de backend, cuenta propia o procesador de telemetría.

**Regla de Cartridge & Cloud.** El rol de responsable, corresponsable o encargado se determina por quién decide fines y medios reales, no por una etiqueta contractual genérica.

**Procedimiento operativo.**
1. Mapear flujo y decisiones.
2. Revisar términos y contrato.
3. Definir reutilización del proveedor.
4. Registrar transferencias y subprocesadores.
5. Registrar específicamente el impacto de `Fuentes de datos` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Matriz de roles, DPA/contrato, lista de subprocesadores y data-flow.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-009; PRIV-OFF-013; PRIV-OFF-014`.

# 61. Unity

Este capítulo pertenece al bloque **Fuentes, plataformas y roles**. Su objetivo es convertir **unity** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El proyecto contiene módulos built-in y `UnityConnectSettings`. Analytics, Cloud Diagnostics, Ads, Purchasing y Performance Reporting están desactivados; Engine Diagnostics aparece habilitado en configuración de herramienta. Esto exige verificación de editor y build por separado.

**Regla de Cartridge & Cloud.** No se asume que un ajuste de editor se incluye en player ni que está exento. Se revisan términos, settings y tráfico de una build limpia.

**Procedimiento operativo.**
1. Mapear flujo y decisiones.
2. Revisar términos y contrato.
3. Definir reutilización del proveedor.
4. Registrar transferencias y subprocesadores.
5. Registrar específicamente el impacto de `Unity` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Unity settings, services dashboard, package manifest y controlled network test.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-009; PRIV-OFF-013; PRIV-OFF-014`.

# 62. Steamworks

Este capítulo pertenece al bloque **Fuentes, plataformas y roles**. Su objetivo es convertir **steamworks** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No está integrado ni existe AppID. Cuando se incorpore, cada API se inventaría por datos, permisos, finalidad y retención.

**Regla de Cartridge & Cloud.** Usar primero reporting agregado de Steam. Autenticación, Web API o backend requieren gate adicional y secretos fuera del cliente.

**Procedimiento operativo.**
1. Mapear flujo y decisiones.
2. Revisar términos y contrato.
3. Definir reutilización del proveedor.
4. Registrar transferencias y subprocesadores.
5. Registrar específicamente el impacto de `Steamworks` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** App/API inventory, permissions, Steamworks users and data-flow.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-009; PRIV-OFF-013; PRIV-OFF-014`.

Los informes de Steam se tratan como una fuente separada: el estudio registra qué campos recibe, si son agregados y quién tiene permiso; no infiere acceso a datos individuales por el mero hecho de usar Steamworks.

# 63. Steam Cloud

Este capítulo pertenece al bloque **Fuentes, plataformas y roles**. Su objetivo es convertir **steam cloud** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No está implementado. Auto-Cloud podría sincronizar `IntegratedSaveGames` sin código, mientras API ofrece control granular. Ambos crean un flujo remoto de archivos por usuario.

**Regla de Cartridge & Cloud.** No activar hasta probar paths, conflictos, backup/recovery, múltiples equipos, opt-out, schema y privacy notice.

**Procedimiento operativo.**
1. Mapear flujo y decisiones.
2. Revisar términos y contrato.
3. Definir reutilización del proveedor.
4. Registrar transferencias y subprocesadores.
5. Registrar específicamente el impacto de `Steam Cloud` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Cloud config, two-device campaign, cloud_log, retention/role assessment.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-009; PRIV-OFF-013; PRIV-OFF-014`.

Los informes de Steam se tratan como una fuente separada: el estudio registra qué campos recibe, si son agregados y quién tiene permiso; no infiere acceso a datos individuales por el mero hecho de usar Steamworks.

# 64. Crash reporting

Este capítulo pertenece al bloque **Fuentes, plataformas y roles**. Su objetivo es convertir **crash reporting** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Unity, Steam y futuros proveedores pueden actuar con roles distintos por actividad. El proyecto no dispone hoy de backend, cuenta propia o procesador de telemetría.

**Regla de Cartridge & Cloud.** El rol de responsable, corresponsable o encargado se determina por quién decide fines y medios reales, no por una etiqueta contractual genérica.

**Procedimiento operativo.**
1. Mapear flujo y decisiones.
2. Revisar términos y contrato.
3. Definir reutilización del proveedor.
4. Registrar transferencias y subprocesadores.
5. Registrar específicamente el impacto de `Crash reporting` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Matriz de roles, DPA/contrato, lista de subprocesadores y data-flow.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-009; PRIV-OFF-013; PRIV-OFF-014`.

Los adjuntos voluntarios se consideran potencialmente identificables, se almacenan fuera del repositorio y se vinculan a un ticket mediante un ID interno, no mediante el nombre público del jugador.

# 65. Sistema de soporte

Este capítulo pertenece al bloque **Fuentes, plataformas y roles**. Su objetivo es convertir **sistema de soporte** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay ticketing ni email público. El soporte futuro puede tratar email, Steam alias, logs, saves, screenshots y hardware aportados voluntariamente.

**Regla de Cartridge & Cloud.** El formulario debe separar campos obligatorios de adjuntos opcionales y mostrar finalidad, retención, contacto y derechos.

**Procedimiento operativo.**
1. Mapear flujo y decisiones.
2. Revisar términos y contrato.
3. Definir reutilización del proveedor.
4. Registrar transferencias y subprocesadores.
5. Registrar específicamente el impacto de `Sistema de soporte` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Support privacy notice, ticket schema, access groups and deletion test.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-009; PRIV-OFF-013; PRIV-OFF-014`.

Los adjuntos voluntarios se consideran potencialmente identificables, se almacenan fuera del repositorio y se vinculan a un ticket mediante un ID interno, no mediante el nombre público del jugador.

# 66. Email de soporte

Este capítulo pertenece al bloque **Fuentes, plataformas y roles**. Su objetivo es convertir **email de soporte** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe dirección pública documentada. Un buzón introduce proveedor, metadatos, spam filtering, backups y accesos.

**Regla de Cartridge & Cloud.** Crear cuenta de dominio con MFA, aliases, acceso mínimo, retención y export/delete. No usar dirección personal del desarrollador.

**Procedimiento operativo.**
1. Mapear flujo y decisiones.
2. Revisar términos y contrato.
3. Definir reutilización del proveedor.
4. Registrar transferencias y subprocesadores.
5. Registrar específicamente el impacto de `Email de soporte` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Provider record, DPA/terms, MFA evidence, retention and test request.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-009; PRIV-OFF-013; PRIV-OFF-014`.

Los adjuntos voluntarios se consideran potencialmente identificables, se almacenan fuera del repositorio y se vinculan a un ticket mediante un ID interno, no mediante el nombre público del jugador. La clasificación aplica contexto y riesgo de inferencia. No se recopila edad exacta, salud, biometría, chat o categorías especiales para “mejorar segmentación”.

# 67. Formularios

Este capítulo pertenece al bloque **Fuentes, plataformas y roles**. Su objetivo es convertir **formularios** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Unity, Steam y futuros proveedores pueden actuar con roles distintos por actividad. El proyecto no dispone hoy de backend, cuenta propia o procesador de telemetría.

**Regla de Cartridge & Cloud.** El rol de responsable, corresponsable o encargado se determina por quién decide fines y medios reales, no por una etiqueta contractual genérica.

**Procedimiento operativo.**
1. Mapear flujo y decisiones.
2. Revisar términos y contrato.
3. Definir reutilización del proveedor.
4. Registrar transferencias y subprocesadores.
5. Registrar específicamente el impacto de `Formularios` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Matriz de roles, DPA/contrato, lista de subprocesadores y data-flow.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-009; PRIV-OFF-013; PRIV-OFF-014`.

# 68. Proveedores externos

Este capítulo pertenece al bloque **Fuentes, plataformas y roles**. Su objetivo es convertir **proveedores externos** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay proveedor de analytics/crash/ticketing seleccionado. OpenAI se ha usado en documentación/concepto, pero no como backend del juego.

**Regla de Cartridge & Cloud.** Cada proveedor se evalúa por datos, rol, localización, subprocesadores, reutilización, seguridad, export/delete y salida.

**Procedimiento operativo.**
1. Mapear flujo y decisiones.
2. Revisar términos y contrato.
3. Definir reutilización del proveedor.
4. Registrar transferencias y subprocesadores.
5. Registrar específicamente el impacto de `Proveedores externos` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Vendor questionnaire, DPA, TOMs, transfer assessment and exit plan.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-009; PRIV-OFF-013; PRIV-OFF-014`.

El proveedor no se aprueba solo por disponer de una política pública: se revisan contrato, DPA, subprocesadores, localización, reutilización, incidentes, exportación, borrado y plan de salida.

# 69. Procesadores

Este capítulo pertenece al bloque **Fuentes, plataformas y roles**. Su objetivo es convertir **procesadores** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Unity, Steam y futuros proveedores pueden actuar con roles distintos por actividad. El proyecto no dispone hoy de backend, cuenta propia o procesador de telemetría.

**Regla de Cartridge & Cloud.** El rol de responsable, corresponsable o encargado se determina por quién decide fines y medios reales, no por una etiqueta contractual genérica.

**Procedimiento operativo.**
1. Mapear flujo y decisiones.
2. Revisar términos y contrato.
3. Definir reutilización del proveedor.
4. Registrar transferencias y subprocesadores.
5. Registrar específicamente el impacto de `Procesadores` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Matriz de roles, DPA/contrato, lista de subprocesadores y data-flow.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-009; PRIV-OFF-013; PRIV-OFF-014`.

El proveedor no se aprueba solo por disponer de una política pública: se revisan contrato, DPA, subprocesadores, localización, reutilización, incidentes, exportación, borrado y plan de salida.

# 70. Subprocesadores

Este capítulo pertenece al bloque **Fuentes, plataformas y roles**. Su objetivo es convertir **subprocesadores** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Unity, Steam y futuros proveedores pueden actuar con roles distintos por actividad. El proyecto no dispone hoy de backend, cuenta propia o procesador de telemetría.

**Regla de Cartridge & Cloud.** El rol de responsable, corresponsable o encargado se determina por quién decide fines y medios reales, no por una etiqueta contractual genérica.

**Procedimiento operativo.**
1. Mapear flujo y decisiones.
2. Revisar términos y contrato.
3. Definir reutilización del proveedor.
4. Registrar transferencias y subprocesadores.
5. Registrar específicamente el impacto de `Subprocesadores` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Matriz de roles, DPA/contrato, lista de subprocesadores y data-flow.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-009; PRIV-OFF-013; PRIV-OFF-014`.

El proveedor no se aprueba solo por disponer de una política pública: se revisan contrato, DPA, subprocesadores, localización, reutilización, incidentes, exportación, borrado y plan de salida.

# 71. Transferencias internacionales

Este capítulo pertenece al bloque **Fuentes, plataformas y roles**. Su objetivo es convertir **transferencias internacionales** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Unity, Steam y futuros proveedores pueden actuar con roles distintos por actividad. El proyecto no dispone hoy de backend, cuenta propia o procesador de telemetría.

**Regla de Cartridge & Cloud.** El rol de responsable, corresponsable o encargado se determina por quién decide fines y medios reales, no por una etiqueta contractual genérica.

**Procedimiento operativo.**
1. Mapear flujo y decisiones.
2. Revisar términos y contrato.
3. Definir reutilización del proveedor.
4. Registrar transferencias y subprocesadores.
5. Registrar específicamente el impacto de `Transferencias internacionales` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Matriz de roles, DPA/contrato, lista de subprocesadores y data-flow.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-009; PRIV-OFF-013; PRIV-OFF-014`.

El proveedor no se aprueba solo por disponer de una política pública: se revisan contrato, DPA, subprocesadores, localización, reutilización, incidentes, exportación, borrado y plan de salida.

# 72. Jurisdicción

Este capítulo pertenece al bloque **Fuentes, plataformas y roles**. Su objetivo es convertir **jurisdicción** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Unity, Steam y futuros proveedores pueden actuar con roles distintos por actividad. El proyecto no dispone hoy de backend, cuenta propia o procesador de telemetría.

**Regla de Cartridge & Cloud.** El rol de responsable, corresponsable o encargado se determina por quién decide fines y medios reales, no por una etiqueta contractual genérica.

**Procedimiento operativo.**
1. Mapear flujo y decisiones.
2. Revisar términos y contrato.
3. Definir reutilización del proveedor.
4. Registrar transferencias y subprocesadores.
5. Registrar específicamente el impacto de `Jurisdicción` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Matriz de roles, DPA/contrato, lista de subprocesadores y data-flow.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-009; PRIV-OFF-013; PRIV-OFF-014`.

El proveedor no se aprueba solo por disponer de una política pública: se revisan contrato, DPA, subprocesadores, localización, reutilización, incidentes, exportación, borrado y plan de salida.

# 73. Responsable del tratamiento

Este capítulo pertenece al bloque **Fuentes, plataformas y roles**. Su objetivo es convertir **responsable del tratamiento** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Unity, Steam y futuros proveedores pueden actuar con roles distintos por actividad. El proyecto no dispone hoy de backend, cuenta propia o procesador de telemetría.

**Regla de Cartridge & Cloud.** El rol de responsable, corresponsable o encargado se determina por quién decide fines y medios reales, no por una etiqueta contractual genérica.

**Procedimiento operativo.**
1. Mapear flujo y decisiones.
2. Revisar términos y contrato.
3. Definir reutilización del proveedor.
4. Registrar transferencias y subprocesadores.
5. Registrar específicamente el impacto de `Responsable del tratamiento` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Matriz de roles, DPA/contrato, lista de subprocesadores y data-flow.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-009; PRIV-OFF-013; PRIV-OFF-014`.

# 74. Encargado del tratamiento

Este capítulo pertenece al bloque **Fuentes, plataformas y roles**. Su objetivo es convertir **encargado del tratamiento** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Unity, Steam y futuros proveedores pueden actuar con roles distintos por actividad. El proyecto no dispone hoy de backend, cuenta propia o procesador de telemetría.

**Regla de Cartridge & Cloud.** El rol de responsable, corresponsable o encargado se determina por quién decide fines y medios reales, no por una etiqueta contractual genérica.

**Procedimiento operativo.**
1. Mapear flujo y decisiones.
2. Revisar términos y contrato.
3. Definir reutilización del proveedor.
4. Registrar transferencias y subprocesadores.
5. Registrar específicamente el impacto de `Encargado del tratamiento` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Matriz de roles, DPA/contrato, lista de subprocesadores y data-flow.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-009; PRIV-OFF-013; PRIV-OFF-014`.

# 75. Base jurídica

Este capítulo pertenece al bloque **Bases jurídicas y control del jugador**. Su objetivo es convertir **base jurídica** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe hoy tratamiento propio remoto que requiera una pantalla de consentimiento. Activar telemetría, newsletter o cuenta cambiaría ese estado.

**Regla de Cartridge & Cloud.** La base jurídica se define por finalidad antes de recoger. El consentimiento, cuando se use, debe ser libre, específico, informado, inequívoco, granular y revocable.

**Procedimiento operativo.**
1. Describir finalidad y necesidad.
2. Seleccionar y justificar base.
3. Diseñar alternativa cuando proceda.
4. Registrar elección y revocación.
5. Registrar específicamente el impacto de `Base jurídica` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Legal-basis assessment, copy, UI, consentimiento versionado y prueba de retirada.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-002; PRIV-OFF-004`.

# 76. Contrato

Este capítulo pertenece al bloque **Bases jurídicas y control del jugador**. Su objetivo es convertir **contrato** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe hoy tratamiento propio remoto que requiera una pantalla de consentimiento. Activar telemetría, newsletter o cuenta cambiaría ese estado.

**Regla de Cartridge & Cloud.** La base jurídica se define por finalidad antes de recoger. El consentimiento, cuando se use, debe ser libre, específico, informado, inequívoco, granular y revocable.

**Procedimiento operativo.**
1. Describir finalidad y necesidad.
2. Seleccionar y justificar base.
3. Diseñar alternativa cuando proceda.
4. Registrar elección y revocación.
5. Registrar específicamente el impacto de `Contrato` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Legal-basis assessment, copy, UI, consentimiento versionado y prueba de retirada.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-002; PRIV-OFF-004`.

El proveedor no se aprueba solo por disponer de una política pública: se revisan contrato, DPA, subprocesadores, localización, reutilización, incidentes, exportación, borrado y plan de salida.

# 77. Interés legítimo

Este capítulo pertenece al bloque **Bases jurídicas y control del jugador**. Su objetivo es convertir **interés legítimo** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe hoy tratamiento propio remoto que requiera una pantalla de consentimiento. Activar telemetría, newsletter o cuenta cambiaría ese estado.

**Regla de Cartridge & Cloud.** La base jurídica se define por finalidad antes de recoger. El consentimiento, cuando se use, debe ser libre, específico, informado, inequívoco, granular y revocable.

**Procedimiento operativo.**
1. Describir finalidad y necesidad.
2. Seleccionar y justificar base.
3. Diseñar alternativa cuando proceda.
4. Registrar elección y revocación.
5. Registrar específicamente el impacto de `Interés legítimo` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Legal-basis assessment, copy, UI, consentimiento versionado y prueba de retirada.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-002; PRIV-OFF-004`.

# 78. Consentimiento

Este capítulo pertenece al bloque **Bases jurídicas y control del jugador**. Su objetivo es convertir **consentimiento** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay UI de consentimiento. No debe añadirse un banner genérico si no existe tratamiento opcional. Si se usa consentimiento, la experiencia no empleará dark patterns ni aceptación agrupada.

**Regla de Cartridge & Cloud.** Consentimiento separado por finalidad, rechazo equivalente, registro versionado y retirada desde opciones.

**Procedimiento operativo.**
1. Describir finalidad y necesidad.
2. Seleccionar y justificar base.
3. Diseñar alternativa cuando proceda.
4. Registrar elección y revocación.
5. Registrar específicamente el impacto de `Consentimiento` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Consent receipt, UI screenshots, revoke test and behavior verification.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-002; PRIV-OFF-004`.

La QA verifica simetría entre aceptar y rechazar, ausencia de transmisión previa, persistencia de la elección y efecto real de la revocación, no solo el cambio visual del toggle.

# 79. Obligación legal

Este capítulo pertenece al bloque **Bases jurídicas y control del jugador**. Su objetivo es convertir **obligación legal** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe hoy tratamiento propio remoto que requiera una pantalla de consentimiento. Activar telemetría, newsletter o cuenta cambiaría ese estado.

**Regla de Cartridge & Cloud.** La base jurídica se define por finalidad antes de recoger. El consentimiento, cuando se use, debe ser libre, específico, informado, inequívoco, granular y revocable.

**Procedimiento operativo.**
1. Describir finalidad y necesidad.
2. Seleccionar y justificar base.
3. Diseñar alternativa cuando proceda.
4. Registrar elección y revocación.
5. Registrar específicamente el impacto de `Obligación legal` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Legal-basis assessment, copy, UI, consentimiento versionado y prueba de retirada.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-002; PRIV-OFF-004`.

# 80. Ejecución del servicio

Este capítulo pertenece al bloque **Bases jurídicas y control del jugador**. Su objetivo es convertir **ejecución del servicio** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe hoy tratamiento propio remoto que requiera una pantalla de consentimiento. Activar telemetría, newsletter o cuenta cambiaría ese estado.

**Regla de Cartridge & Cloud.** La base jurídica se define por finalidad antes de recoger. El consentimiento, cuando se use, debe ser libre, específico, informado, inequívoco, granular y revocable.

**Procedimiento operativo.**
1. Describir finalidad y necesidad.
2. Seleccionar y justificar base.
3. Diseñar alternativa cuando proceda.
4. Registrar elección y revocación.
5. Registrar específicamente el impacto de `Ejecución del servicio` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Legal-basis assessment, copy, UI, consentimiento versionado y prueba de retirada.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-002; PRIV-OFF-004`.

# 81. Consentimiento opcional

Este capítulo pertenece al bloque **Bases jurídicas y control del jugador**. Su objetivo es convertir **consentimiento opcional** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe hoy tratamiento propio remoto que requiera una pantalla de consentimiento. Activar telemetría, newsletter o cuenta cambiaría ese estado.

**Regla de Cartridge & Cloud.** La base jurídica se define por finalidad antes de recoger. El consentimiento, cuando se use, debe ser libre, específico, informado, inequívoco, granular y revocable.

**Procedimiento operativo.**
1. Describir finalidad y necesidad.
2. Seleccionar y justificar base.
3. Diseñar alternativa cuando proceda.
4. Registrar elección y revocación.
5. Registrar específicamente el impacto de `Consentimiento opcional` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Legal-basis assessment, copy, UI, consentimiento versionado y prueba de retirada.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-002; PRIV-OFF-004`.

La QA verifica simetría entre aceptar y rechazar, ausencia de transmisión previa, persistencia de la elección y efecto real de la revocación, no solo el cambio visual del toggle.

# 82. Revocación

Este capítulo pertenece al bloque **Bases jurídicas y control del jugador**. Su objetivo es convertir **revocación** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe hoy tratamiento propio remoto que requiera una pantalla de consentimiento. Activar telemetría, newsletter o cuenta cambiaría ese estado.

**Regla de Cartridge & Cloud.** La base jurídica se define por finalidad antes de recoger. El consentimiento, cuando se use, debe ser libre, específico, informado, inequívoco, granular y revocable.

**Procedimiento operativo.**
1. Describir finalidad y necesidad.
2. Seleccionar y justificar base.
3. Diseñar alternativa cuando proceda.
4. Registrar elección y revocación.
5. Registrar específicamente el impacto de `Revocación` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Legal-basis assessment, copy, UI, consentimiento versionado y prueba de retirada.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-002; PRIV-OFF-004`.

La QA verifica simetría entre aceptar y rechazar, ausencia de transmisión previa, persistencia de la elección y efecto real de la revocación, no solo el cambio visual del toggle.

# 83. Telemetría opt-in

Este capítulo pertenece al bloque **Bases jurídicas y control del jugador**. Su objetivo es convertir **telemetría opt-in** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** La política objetivo es opt-in para telemetría opcional de gameplay si llega a implementarse, salvo que revisión legal documentada concluya otra base y diseño proporcional.

**Regla de Cartridge & Cloud.** El juego funciona plenamente sin aceptar. La opción se presenta después de información clara y puede modificarse.

**Procedimiento operativo.**
1. Describir finalidad y necesidad.
2. Seleccionar y justificar base.
3. Diseñar alternativa cuando proceda.
4. Registrar elección y revocación.
5. Registrar específicamente el impacto de `Telemetría opt-in` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** A/B-free consent UI, default off, network capture before/after and receipt.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-002; PRIV-OFF-004`.

La ficha de métrica documenta pregunta, decisión, población, denominador, ventana, timezone, sesgo, coste de privacidad y condición de retirada; sin esa ficha el evento no puede abrirse. La QA verifica simetría entre aceptar y rechazar, ausencia de transmisión previa, persistencia de la elección y efecto real de la revocación, no solo el cambio visual del toggle.

# 84. Telemetría opt-out

Este capítulo pertenece al bloque **Bases jurídicas y control del jugador**. Su objetivo es convertir **telemetría opt-out** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe hoy tratamiento propio remoto que requiera una pantalla de consentimiento. Activar telemetría, newsletter o cuenta cambiaría ese estado.

**Regla de Cartridge & Cloud.** La base jurídica se define por finalidad antes de recoger. El consentimiento, cuando se use, debe ser libre, específico, informado, inequívoco, granular y revocable.

**Procedimiento operativo.**
1. Describir finalidad y necesidad.
2. Seleccionar y justificar base.
3. Diseñar alternativa cuando proceda.
4. Registrar elección y revocación.
5. Registrar específicamente el impacto de `Telemetría opt-out` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Legal-basis assessment, copy, UI, consentimiento versionado y prueba de retirada.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-002; PRIV-OFF-004`.

La ficha de métrica documenta pregunta, decisión, población, denominador, ventana, timezone, sesgo, coste de privacidad y condición de retirada; sin esa ficha el evento no puede abrirse. La QA verifica simetría entre aceptar y rechazar, ausencia de transmisión previa, persistencia de la elección y efecto real de la revocación, no solo el cambio visual del toggle.

# 85. Configuración por defecto

Este capítulo pertenece al bloque **Bases jurídicas y control del jugador**. Su objetivo es convertir **configuración por defecto** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El default actual efectivo es ausencia de telemetría propia. Debe conservarse hasta un gate formal.

**Regla de Cartridge & Cloud.** Privacy by default: funciones opcionales desactivadas, granularidad mínima y no transmisión antes de decisión.

**Procedimiento operativo.**
1. Describir finalidad y necesidad.
2. Seleccionar y justificar base.
3. Diseñar alternativa cuando proceda.
4. Registrar elección y revocación.
5. Registrar específicamente el impacto de `Configuración por defecto` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Fresh-install test, config snapshot and packet capture.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-002; PRIV-OFF-004`.

La QA verifica simetría entre aceptar y rechazar, ausencia de transmisión previa, persistencia de la elección y efecto real de la revocación, no solo el cambio visual del toggle.

# 86. Datos imprescindibles

Este capítulo pertenece al bloque **Bases jurídicas y control del jugador**. Su objetivo es convertir **datos imprescindibles** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe hoy tratamiento propio remoto que requiera una pantalla de consentimiento. Activar telemetría, newsletter o cuenta cambiaría ese estado.

**Regla de Cartridge & Cloud.** La base jurídica se define por finalidad antes de recoger. El consentimiento, cuando se use, debe ser libre, específico, informado, inequívoco, granular y revocable.

**Procedimiento operativo.**
1. Describir finalidad y necesidad.
2. Seleccionar y justificar base.
3. Diseñar alternativa cuando proceda.
4. Registrar elección y revocación.
5. Registrar específicamente el impacto de `Datos imprescindibles` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Legal-basis assessment, copy, UI, consentimiento versionado y prueba de retirada.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-002; PRIV-OFF-004`.

# 87. Datos opcionales

Este capítulo pertenece al bloque **Bases jurídicas y control del jugador**. Su objetivo es convertir **datos opcionales** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe hoy tratamiento propio remoto que requiera una pantalla de consentimiento. Activar telemetría, newsletter o cuenta cambiaría ese estado.

**Regla de Cartridge & Cloud.** La base jurídica se define por finalidad antes de recoger. El consentimiento, cuando se use, debe ser libre, específico, informado, inequívoco, granular y revocable.

**Procedimiento operativo.**
1. Describir finalidad y necesidad.
2. Seleccionar y justificar base.
3. Diseñar alternativa cuando proceda.
4. Registrar elección y revocación.
5. Registrar específicamente el impacto de `Datos opcionales` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Legal-basis assessment, copy, UI, consentimiento versionado y prueba de retirada.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-002; PRIV-OFF-004`.

# 88. Eventos de telemetría

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **eventos de telemetría** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** El catálogo futuro se limita a eventos definidos, por ejemplo resultado de load/save, cierre de día o bucket de rendimiento; no contiene narrativa, texto libre ni IDs de mundo innecesarios.

**Regla de Cartridge & Cloud.** Todo evento tiene owner, pregunta, propiedades permitidas, retention, schema, sampling y fecha de retirada.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Eventos de telemetría` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Event catalog and generated validator tests.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

La ficha de métrica documenta pregunta, decisión, población, denominador, ventana, timezone, sesgo, coste de privacidad y condición de retirada; sin esa ficha el evento no puede abrirse.

| Event ID propuesto | Finalidad | Propiedades permitidas | Estado |
|---|---|---|---|
| `app_session_started` | estabilidad de versión | app_version, build, locale | NOT OPEN |
| `save_operation_result` | detectar fallos de save | operation, result, schema | NOT OPEN |
| `day_closed` | validar loop | day_bucket, cash_bucket | NOT OPEN |
| `checkout_completed` | balance agregado | basket_size_bucket, revenue_bucket | NOT OPEN |
| `stockout_observed` | contenido/balance | category, duration_bucket | NOT OPEN |
| `performance_sample` | rendimiento | fps_bucket, scene, quality_tier | NOT OPEN |
| `ui_error_shown` | calidad UI | error_code, screen | NOT OPEN |

# 89. Convenciones de eventos

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **convenciones de eventos** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay catálogo de eventos activo. El plan propone una convención futura y deshabilitada por defecto, sin texto libre ni IDs de plataforma.

**Regla de Cartridge & Cloud.** El esquema debe impedir datos excesivos por construcción, usar allowlists, límites, validación, separación de entornos y minimización de linkability.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Convenciones de eventos` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Event catalog, JSON schema, tests negativos y muestra de payload sanitizado.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

La ficha de métrica documenta pregunta, decisión, población, denominador, ventana, timezone, sesgo, coste de privacidad y condición de retirada; sin esa ficha el evento no puede abrirse.

```text
event_id: snake_case, estable y semántico
schema_version: entero positivo
occurred_at: UTC, precisión mínima necesaria
properties: allowlist tipada
user_id: prohibido por defecto
free_text: prohibido
unknown_properties: reject
```

# 90. Propiedades

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **propiedades** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay catálogo de eventos activo. El plan propone una convención futura y deshabilitada por defecto, sin texto libre ni IDs de plataforma.

**Regla de Cartridge & Cloud.** El esquema debe impedir datos excesivos por construcción, usar allowlists, límites, validación, separación de entornos y minimización de linkability.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Propiedades` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Event catalog, JSON schema, tests negativos y muestra de payload sanitizado.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

# 91. IDs de evento

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **ids de evento** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay catálogo de eventos activo. El plan propone una convención futura y deshabilitada por defecto, sin texto libre ni IDs de plataforma.

**Regla de Cartridge & Cloud.** El esquema debe impedir datos excesivos por construcción, usar allowlists, límites, validación, separación de entornos y minimización de linkability.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `IDs de evento` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Event catalog, JSON schema, tests negativos y muestra de payload sanitizado.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

La ficha de métrica documenta pregunta, decisión, población, denominador, ventana, timezone, sesgo, coste de privacidad y condición de retirada; sin esa ficha el evento no puede abrirse.

# 92. Versionado del esquema

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **versionado del esquema** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay catálogo de eventos activo. El plan propone una convención futura y deshabilitada por defecto, sin texto libre ni IDs de plataforma.

**Regla de Cartridge & Cloud.** El esquema debe impedir datos excesivos por construcción, usar allowlists, límites, validación, separación de entornos y minimización de linkability.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Versionado del esquema` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Event catalog, JSON schema, tests negativos y muestra de payload sanitizado.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

# 93. Eventos obsoletos

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **eventos obsoletos** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay catálogo de eventos activo. El plan propone una convención futura y deshabilitada por defecto, sin texto libre ni IDs de plataforma.

**Regla de Cartridge & Cloud.** El esquema debe impedir datos excesivos por construcción, usar allowlists, límites, validación, separación de entornos y minimización de linkability.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Eventos obsoletos` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Event catalog, JSON schema, tests negativos y muestra de payload sanitizado.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

La ficha de métrica documenta pregunta, decisión, población, denominador, ventana, timezone, sesgo, coste de privacidad y condición de retirada; sin esa ficha el evento no puede abrirse.

# 94. Validación

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **validación** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay catálogo de eventos activo. El plan propone una convención futura y deshabilitada por defecto, sin texto libre ni IDs de plataforma.

**Regla de Cartridge & Cloud.** El esquema debe impedir datos excesivos por construcción, usar allowlists, límites, validación, separación de entornos y minimización de linkability.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Validación` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Event catalog, JSON schema, tests negativos y muestra de payload sanitizado.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

# 95. Sampling

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **sampling** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay catálogo de eventos activo. El plan propone una convención futura y deshabilitada por defecto, sin texto libre ni IDs de plataforma.

**Regla de Cartridge & Cloud.** El esquema debe impedir datos excesivos por construcción, usar allowlists, límites, validación, separación de entornos y minimización de linkability.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Sampling` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Event catalog, JSON schema, tests negativos y muestra de payload sanitizado.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

La ficha de métrica documenta pregunta, decisión, población, denominador, ventana, timezone, sesgo, coste de privacidad y condición de retirada; sin esa ficha el evento no puede abrirse.

# 96. Frecuencia

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **frecuencia** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay catálogo de eventos activo. El plan propone una convención futura y deshabilitada por defecto, sin texto libre ni IDs de plataforma.

**Regla de Cartridge & Cloud.** El esquema debe impedir datos excesivos por construcción, usar allowlists, límites, validación, separación de entornos y minimización de linkability.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Frecuencia` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Event catalog, JSON schema, tests negativos y muestra de payload sanitizado.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

La ficha de métrica documenta pregunta, decisión, población, denominador, ventana, timezone, sesgo, coste de privacidad y condición de retirada; sin esa ficha el evento no puede abrirse.

# 97. Batching

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **batching** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay catálogo de eventos activo. El plan propone una convención futura y deshabilitada por defecto, sin texto libre ni IDs de plataforma.

**Regla de Cartridge & Cloud.** El esquema debe impedir datos excesivos por construcción, usar allowlists, límites, validación, separación de entornos y minimización de linkability.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Batching` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Event catalog, JSON schema, tests negativos y muestra de payload sanitizado.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

La ficha de métrica documenta pregunta, decisión, población, denominador, ventana, timezone, sesgo, coste de privacidad y condición de retirada; sin esa ficha el evento no puede abrirse.

# 98. Envío offline

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **envío offline** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay catálogo de eventos activo. El plan propone una convención futura y deshabilitada por defecto, sin texto libre ni IDs de plataforma.

**Regla de Cartridge & Cloud.** El esquema debe impedir datos excesivos por construcción, usar allowlists, límites, validación, separación de entornos y minimización de linkability.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Envío offline` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Event catalog, JSON schema, tests negativos y muestra de payload sanitizado.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

# 99. Reintentos

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **reintentos** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay catálogo de eventos activo. El plan propone una convención futura y deshabilitada por defecto, sin texto libre ni IDs de plataforma.

**Regla de Cartridge & Cloud.** El esquema debe impedir datos excesivos por construcción, usar allowlists, límites, validación, separación de entornos y minimización de linkability.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Reintentos` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Event catalog, JSON schema, tests negativos y muestra de payload sanitizado.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

# 100. Límites

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **límites** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay catálogo de eventos activo. El plan propone una convención futura y deshabilitada por defecto, sin texto libre ni IDs de plataforma.

**Regla de Cartridge & Cloud.** El esquema debe impedir datos excesivos por construcción, usar allowlists, límites, validación, separación de entornos y minimización de linkability.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Límites` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Event catalog, JSON schema, tests negativos y muestra de payload sanitizado.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

# 101. Evitar texto libre

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **evitar texto libre** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay catálogo de eventos activo. El plan propone una convención futura y deshabilitada por defecto, sin texto libre ni IDs de plataforma.

**Regla de Cartridge & Cloud.** El esquema debe impedir datos excesivos por construcción, usar allowlists, límites, validación, separación de entornos y minimización de linkability.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Evitar texto libre` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Event catalog, JSON schema, tests negativos y muestra de payload sanitizado.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

# 102. Evitar saves completos

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **evitar saves completos** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Un save contiene una trayectoria detallada y puede convertirse en perfil conductual si se sube. No debe utilizarse como payload de analytics.

**Regla de Cartridge & Cloud.** Para diagnóstico se extrae localmente un resumen mínimo o se solicita voluntariamente en un ticket concreto.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Evitar saves completos` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Extraction tool, consent text, sample minimized payload.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

Los adjuntos voluntarios se consideran potencialmente identificables, se almacenan fuera del repositorio y se vinculan a un ticket mediante un ID interno, no mediante el nombre público del jugador.

# 103. Evitar rutas de usuario

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **evitar rutas de usuario** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Rutas como `C:\Users\Nombre` pueden identificar al usuario y revelar estructura local. Stack traces y excepciones pueden incluirlas.

**Regla de Cartridge & Cloud.** Redactar segmentos de home/user antes de enviar o persistir. Usar tokens como `<USER_HOME>`.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Evitar rutas de usuario` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Unit tests with Windows/Linux path fixtures and sanitized output.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

# 104. Evitar nombres de PC

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **evitar nombres de pc** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay catálogo de eventos activo. El plan propone una convención futura y deshabilitada por defecto, sin texto libre ni IDs de plataforma.

**Regla de Cartridge & Cloud.** El esquema debe impedir datos excesivos por construcción, usar allowlists, límites, validación, separación de entornos y minimización de linkability.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Evitar nombres de PC` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Event catalog, JSON schema, tests negativos y muestra de payload sanitizado.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

# 105. Evitar tokens

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **evitar tokens** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Nunca registrar Steam Web API keys, OAuth/session tickets, email tokens, cookies o secretos de proveedor.

**Regla de Cartridge & Cloud.** Secret scanning, allowlist de campos, logs estructurados y redaction antes de storage.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Evitar tokens` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Secret-scan report and negative tests.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

# 106. Redacción de logs

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **redacción de logs** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** La redacción ocurre antes de transmisión y, cuando sea posible, antes de escritura. No depende solo de un agente de soporte humano.

**Regla de Cartridge & Cloud.** Patrones para rutas, emails, IP, SteamID, tokens y query strings; fallar cerrado para campos desconocidos.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Redacción de logs` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Redaction library tests and corpus.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

Los adjuntos voluntarios se consideran potencialmente identificables, se almacenan fuera del repositorio y se vinculan a un ticket mediante un ID interno, no mediante el nombre público del jugador.

```text
C:\Users\Alice\AppData\...  -> <USER_HOME>\AppData\...
alice@example.com              -> <EMAIL>
7656119xxxxxxxxxx              -> <STEAM_ID>
Authorization: Bearer ...     -> Authorization: <REDACTED>
203.0.113.42                  -> <IP>
```

# 107. Sanitización

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **sanitización** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay catálogo de eventos activo. El plan propone una convención futura y deshabilitada por defecto, sin texto libre ni IDs de plataforma.

**Regla de Cartridge & Cloud.** El esquema debe impedir datos excesivos por construcción, usar allowlists, límites, validación, separación de entornos y minimización de linkability.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Sanitización` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Event catalog, JSON schema, tests negativos y muestra de payload sanitizado.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

# 108. Hashing

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **hashing** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Hashing no anonimiza automáticamente. IDs de dominio pequeño, SteamID o email pueden ser atacados o correlacionados.

**Regla de Cartridge & Cloud.** Usar hashing solo con finalidad técnica concreta, salt/pepper protegido y evaluación de linkability.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Hashing` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Threat model, algorithm/version and key management.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

# 109. Salt

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **salt** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay catálogo de eventos activo. El plan propone una convención futura y deshabilitada por defecto, sin texto libre ni IDs de plataforma.

**Regla de Cartridge & Cloud.** El esquema debe impedir datos excesivos por construcción, usar allowlists, límites, validación, separación de entornos y minimización de linkability.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Salt` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Event catalog, JSON schema, tests negativos y muestra de payload sanitizado.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

# 110. Identificadores rotatorios

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **identificadores rotatorios** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay catálogo de eventos activo. El plan propone una convención futura y deshabilitada por defecto, sin texto libre ni IDs de plataforma.

**Regla de Cartridge & Cloud.** El esquema debe impedir datos excesivos por construcción, usar allowlists, límites, validación, separación de entornos y minimización de linkability.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Identificadores rotatorios` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Event catalog, JSON schema, tests negativos y muestra de payload sanitizado.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

# 111. Agregación

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **agregación** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay catálogo de eventos activo. El plan propone una convención futura y deshabilitada por defecto, sin texto libre ni IDs de plataforma.

**Regla de Cartridge & Cloud.** El esquema debe impedir datos excesivos por construcción, usar allowlists, límites, validación, separación de entornos y minimización de linkability.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Agregación` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Event catalog, JSON schema, tests negativos y muestra de payload sanitizado.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

La ficha de métrica documenta pregunta, decisión, población, denominador, ventana, timezone, sesgo, coste de privacidad y condición de retirada; sin esa ficha el evento no puede abrirse.

# 112. K-anonymity o umbrales

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **k-anonymity o umbrales** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Steam aplica umbrales en UTM reporting. Para informes propios futuros, grupos pequeños deben suprimirse o agruparse.

**Regla de Cartridge & Cloud.** K-anonymity es una medida auxiliar, no garantía completa; revisar atributos cuasi-identificadores y ataques por composición.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `K-anonymity o umbrales` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Threshold policy, query tests and suppressed-cell audit.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

# 113. Retención por categoría

Este capítulo pertenece al bloque **Diseño de eventos y privacy engineering**. Su objetivo es convertir **retención por categoría** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe retención remota. Este plan propone máximos iniciales para soporte y futuros datos, sujetos a validación legal y operacional.

**Regla de Cartridge & Cloud.** Cada categoría tiene purpose, trigger de inicio, plazo, borrado, backup lag y excepción documentada.

**Procedimiento operativo.**
1. Definir schema y allowlist.
2. Validar cliente y servidor/endpoint.
3. Aplicar límites y redacción.
4. Versionar y retirar eventos.
5. Probar ausencia de datos prohibidos.
6. Registrar específicamente el impacto de `Retención por categoría` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Retention table and automated deletion evidence.

**Referencias oficiales relacionadas:** `PRIV-OFF-004; PRIV-OFF-006`.

El borrado cubre índices, objetos primarios y copias operativas; los backups tienen una ventana documentada y no se restauran para reactivar datos que debían eliminarse.

| Categoría futura | Plazo inicial propuesto | Trigger | Observación |
|---|---:|---|---|
| ticket y email | 24 meses tras cierre | cierre del caso | revisar anual y obligaciones |
| log/save adjunto | 90 días tras cierre | resolución | ampliar solo con motivo documentado |
| crash raw/dump | 90 días | ingestión | acceso muy restringido |
| eventos raw opcionales | 90 días | evento | si se abre; preferir menos |
| agregados seudónimos | 13 meses | periodo de reporte | evaluar linkability |
| agregados anonimizados | según necesidad histórica | anonimización verificada | revisión periódica |
| consent receipts | mientras se trate + defensa aplicable | revocación/fin | revisión legal |
| security access logs | 12 meses | registro | ajustar a riesgo |

# 114. Eliminación

Este capítulo pertenece al bloque **Retención, eliminación y derechos**. Su objetivo es convertir **eliminación** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe repositorio remoto propio. Los archivos locales son controlados por el jugador; los recibidos para soporte requerirán retención y borrado.

**Regla de Cartridge & Cloud.** Conservar solo mientras sea necesario. La seudonimización no justifica retención indefinida. Los derechos deben ser ejercitables y verificables.

**Procedimiento operativo.**
1. Asignar plazo y trigger de borrado.
2. Separar backup y producción.
3. Implementar búsqueda/export/delete.
4. Registrar solicitud y respuesta.
5. Registrar específicamente el impacto de `Eliminación` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Retention schedule, deletion log, export sample y rights request record.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-006; PRIV-OFF-009; PRIV-OFF-015`.

El borrado cubre índices, objetos primarios y copias operativas; los backups tienen una ventana documentada y no se restauran para reactivar datos que debían eliminarse.

# 115. Backups

Este capítulo pertenece al bloque **Retención, eliminación y derechos**. Su objetivo es convertir **backups** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe repositorio remoto propio. Los archivos locales son controlados por el jugador; los recibidos para soporte requerirán retención y borrado.

**Regla de Cartridge & Cloud.** Conservar solo mientras sea necesario. La seudonimización no justifica retención indefinida. Los derechos deben ser ejercitables y verificables.

**Procedimiento operativo.**
1. Asignar plazo y trigger de borrado.
2. Separar backup y producción.
3. Implementar búsqueda/export/delete.
4. Registrar solicitud y respuesta.
5. Registrar específicamente el impacto de `Backups` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Retention schedule, deletion log, export sample y rights request record.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-006; PRIV-OFF-009; PRIV-OFF-015`.

El borrado cubre índices, objetos primarios y copias operativas; los backups tienen una ventana documentada y no se restauran para reactivar datos que debían eliminarse.

# 116. Restauración

Este capítulo pertenece al bloque **Retención, eliminación y derechos**. Su objetivo es convertir **restauración** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe repositorio remoto propio. Los archivos locales son controlados por el jugador; los recibidos para soporte requerirán retención y borrado.

**Regla de Cartridge & Cloud.** Conservar solo mientras sea necesario. La seudonimización no justifica retención indefinida. Los derechos deben ser ejercitables y verificables.

**Procedimiento operativo.**
1. Asignar plazo y trigger de borrado.
2. Separar backup y producción.
3. Implementar búsqueda/export/delete.
4. Registrar solicitud y respuesta.
5. Registrar específicamente el impacto de `Restauración` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Retention schedule, deletion log, export sample y rights request record.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-006; PRIV-OFF-009; PRIV-OFF-015`.

# 117. Legal hold

Este capítulo pertenece al bloque **Retención, eliminación y derechos**. Su objetivo es convertir **legal hold** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe repositorio remoto propio. Los archivos locales son controlados por el jugador; los recibidos para soporte requerirán retención y borrado.

**Regla de Cartridge & Cloud.** Conservar solo mientras sea necesario. La seudonimización no justifica retención indefinida. Los derechos deben ser ejercitables y verificables.

**Procedimiento operativo.**
1. Asignar plazo y trigger de borrado.
2. Separar backup y producción.
3. Implementar búsqueda/export/delete.
4. Registrar solicitud y respuesta.
5. Registrar específicamente el impacto de `Legal hold` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Retention schedule, deletion log, export sample y rights request record.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-006; PRIV-OFF-009; PRIV-OFF-015`.

El borrado cubre índices, objetos primarios y copias operativas; los backups tienen una ventana documentada y no se restauran para reactivar datos que debían eliminarse.

# 118. Derechos del usuario

Este capítulo pertenece al bloque **Retención, eliminación y derechos**. Su objetivo es convertir **derechos del usuario** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Valve gestiona derechos sobre datos Steam mediante su Privacy Dashboard. VRM Games deberá gestionar solo los datos propios que llegue a controlar.

**Regla de Cartridge & Cloud.** Proporcionar canal único, verificar identidad proporcionalmente y no redirigir a Valve cuando la solicitud afecta datos propios.

**Procedimiento operativo.**
1. Asignar plazo y trigger de borrado.
2. Separar backup y producción.
3. Implementar búsqueda/export/delete.
4. Registrar solicitud y respuesta.
5. Registrar específicamente el impacto de `Derechos del usuario` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Rights workflow, identity checklist, response template and log.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-006; PRIV-OFF-009; PRIV-OFF-015`.

La verificación de identidad es proporcional: no se pide documento oficial cuando basta con responder desde el canal o cuenta ya vinculada. Se evita generar más datos para atender un derecho.

# 119. Acceso

Este capítulo pertenece al bloque **Retención, eliminación y derechos**. Su objetivo es convertir **acceso** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe repositorio remoto propio. Los archivos locales son controlados por el jugador; los recibidos para soporte requerirán retención y borrado.

**Regla de Cartridge & Cloud.** Conservar solo mientras sea necesario. La seudonimización no justifica retención indefinida. Los derechos deben ser ejercitables y verificables.

**Procedimiento operativo.**
1. Asignar plazo y trigger de borrado.
2. Separar backup y producción.
3. Implementar búsqueda/export/delete.
4. Registrar solicitud y respuesta.
5. Registrar específicamente el impacto de `Acceso` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Retention schedule, deletion log, export sample y rights request record.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-006; PRIV-OFF-009; PRIV-OFF-015`.

Los controles incluyen least privilege, MFA, segregación de entornos, inventario de secretos, rotación, logs de acceso, restauración y simulacros, con evidencia fechada.

# 120. Rectificación

Este capítulo pertenece al bloque **Retención, eliminación y derechos**. Su objetivo es convertir **rectificación** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe repositorio remoto propio. Los archivos locales son controlados por el jugador; los recibidos para soporte requerirán retención y borrado.

**Regla de Cartridge & Cloud.** Conservar solo mientras sea necesario. La seudonimización no justifica retención indefinida. Los derechos deben ser ejercitables y verificables.

**Procedimiento operativo.**
1. Asignar plazo y trigger de borrado.
2. Separar backup y producción.
3. Implementar búsqueda/export/delete.
4. Registrar solicitud y respuesta.
5. Registrar específicamente el impacto de `Rectificación` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Retention schedule, deletion log, export sample y rights request record.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-006; PRIV-OFF-009; PRIV-OFF-015`.

La verificación de identidad es proporcional: no se pide documento oficial cuando basta con responder desde el canal o cuenta ya vinculada. Se evita generar más datos para atender un derecho.

# 121. Supresión

Este capítulo pertenece al bloque **Retención, eliminación y derechos**. Su objetivo es convertir **supresión** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe repositorio remoto propio. Los archivos locales son controlados por el jugador; los recibidos para soporte requerirán retención y borrado.

**Regla de Cartridge & Cloud.** Conservar solo mientras sea necesario. La seudonimización no justifica retención indefinida. Los derechos deben ser ejercitables y verificables.

**Procedimiento operativo.**
1. Asignar plazo y trigger de borrado.
2. Separar backup y producción.
3. Implementar búsqueda/export/delete.
4. Registrar solicitud y respuesta.
5. Registrar específicamente el impacto de `Supresión` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Retention schedule, deletion log, export sample y rights request record.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-006; PRIV-OFF-009; PRIV-OFF-015`.

El borrado cubre índices, objetos primarios y copias operativas; los backups tienen una ventana documentada y no se restauran para reactivar datos que debían eliminarse.

# 122. Restricción

Este capítulo pertenece al bloque **Retención, eliminación y derechos**. Su objetivo es convertir **restricción** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe repositorio remoto propio. Los archivos locales son controlados por el jugador; los recibidos para soporte requerirán retención y borrado.

**Regla de Cartridge & Cloud.** Conservar solo mientras sea necesario. La seudonimización no justifica retención indefinida. Los derechos deben ser ejercitables y verificables.

**Procedimiento operativo.**
1. Asignar plazo y trigger de borrado.
2. Separar backup y producción.
3. Implementar búsqueda/export/delete.
4. Registrar solicitud y respuesta.
5. Registrar específicamente el impacto de `Restricción` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Retention schedule, deletion log, export sample y rights request record.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-006; PRIV-OFF-009; PRIV-OFF-015`.

# 123. Portabilidad

Este capítulo pertenece al bloque **Retención, eliminación y derechos**. Su objetivo es convertir **portabilidad** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe repositorio remoto propio. Los archivos locales son controlados por el jugador; los recibidos para soporte requerirán retención y borrado.

**Regla de Cartridge & Cloud.** Conservar solo mientras sea necesario. La seudonimización no justifica retención indefinida. Los derechos deben ser ejercitables y verificables.

**Procedimiento operativo.**
1. Asignar plazo y trigger de borrado.
2. Separar backup y producción.
3. Implementar búsqueda/export/delete.
4. Registrar solicitud y respuesta.
5. Registrar específicamente el impacto de `Portabilidad` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Retention schedule, deletion log, export sample y rights request record.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-006; PRIV-OFF-009; PRIV-OFF-015`.

La verificación de identidad es proporcional: no se pide documento oficial cuando basta con responder desde el canal o cuenta ya vinculada. Se evita generar más datos para atender un derecho.

# 124. Oposición

Este capítulo pertenece al bloque **Retención, eliminación y derechos**. Su objetivo es convertir **oposición** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe repositorio remoto propio. Los archivos locales son controlados por el jugador; los recibidos para soporte requerirán retención y borrado.

**Regla de Cartridge & Cloud.** Conservar solo mientras sea necesario. La seudonimización no justifica retención indefinida. Los derechos deben ser ejercitables y verificables.

**Procedimiento operativo.**
1. Asignar plazo y trigger de borrado.
2. Separar backup y producción.
3. Implementar búsqueda/export/delete.
4. Registrar solicitud y respuesta.
5. Registrar específicamente el impacto de `Oposición` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Retention schedule, deletion log, export sample y rights request record.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-006; PRIV-OFF-009; PRIV-OFF-015`.

La verificación de identidad es proporcional: no se pide documento oficial cuando basta con responder desde el canal o cuenta ya vinculada. Se evita generar más datos para atender un derecho.

# 125. Retirada de consentimiento

Este capítulo pertenece al bloque **Retención, eliminación y derechos**. Su objetivo es convertir **retirada de consentimiento** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe repositorio remoto propio. Los archivos locales son controlados por el jugador; los recibidos para soporte requerirán retención y borrado.

**Regla de Cartridge & Cloud.** Conservar solo mientras sea necesario. La seudonimización no justifica retención indefinida. Los derechos deben ser ejercitables y verificables.

**Procedimiento operativo.**
1. Asignar plazo y trigger de borrado.
2. Separar backup y producción.
3. Implementar búsqueda/export/delete.
4. Registrar solicitud y respuesta.
5. Registrar específicamente el impacto de `Retirada de consentimiento` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Retention schedule, deletion log, export sample y rights request record.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-006; PRIV-OFF-009; PRIV-OFF-015`.

La QA verifica simetría entre aceptar y rechazar, ausencia de transmisión previa, persistencia de la elección y efecto real de la revocación, no solo el cambio visual del toggle.

# 126. Verificación de identidad

Este capítulo pertenece al bloque **Retención, eliminación y derechos**. Su objetivo es convertir **verificación de identidad** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe repositorio remoto propio. Los archivos locales son controlados por el jugador; los recibidos para soporte requerirán retención y borrado.

**Regla de Cartridge & Cloud.** Conservar solo mientras sea necesario. La seudonimización no justifica retención indefinida. Los derechos deben ser ejercitables y verificables.

**Procedimiento operativo.**
1. Asignar plazo y trigger de borrado.
2. Separar backup y producción.
3. Implementar búsqueda/export/delete.
4. Registrar solicitud y respuesta.
5. Registrar específicamente el impacto de `Verificación de identidad` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Retention schedule, deletion log, export sample y rights request record.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-006; PRIV-OFF-009; PRIV-OFF-015`.

La verificación de identidad es proporcional: no se pide documento oficial cuando basta con responder desde el canal o cuenta ya vinculada. Se evita generar más datos para atender un derecho.

# 127. Plazos

Este capítulo pertenece al bloque **Retención, eliminación y derechos**. Su objetivo es convertir **plazos** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe repositorio remoto propio. Los archivos locales son controlados por el jugador; los recibidos para soporte requerirán retención y borrado.

**Regla de Cartridge & Cloud.** Conservar solo mientras sea necesario. La seudonimización no justifica retención indefinida. Los derechos deben ser ejercitables y verificables.

**Procedimiento operativo.**
1. Asignar plazo y trigger de borrado.
2. Separar backup y producción.
3. Implementar búsqueda/export/delete.
4. Registrar solicitud y respuesta.
5. Registrar específicamente el impacto de `Plazos` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Retention schedule, deletion log, export sample y rights request record.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-006; PRIV-OFF-009; PRIV-OFF-015`.

# 128. Registro de solicitudes

Este capítulo pertenece al bloque **Retención, eliminación y derechos**. Su objetivo es convertir **registro de solicitudes** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe repositorio remoto propio. Los archivos locales son controlados por el jugador; los recibidos para soporte requerirán retención y borrado.

**Regla de Cartridge & Cloud.** Conservar solo mientras sea necesario. La seudonimización no justifica retención indefinida. Los derechos deben ser ejercitables y verificables.

**Procedimiento operativo.**
1. Asignar plazo y trigger de borrado.
2. Separar backup y producción.
3. Implementar búsqueda/export/delete.
4. Registrar solicitud y respuesta.
5. Registrar específicamente el impacto de `Registro de solicitudes` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Retention schedule, deletion log, export sample y rights request record.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-006; PRIV-OFF-009; PRIV-OFF-015`.

| Campo | Obligatorio |
|---|---|
| Request ID | sí |
| fecha/hora y zona | sí |
| derecho solicitado | sí |
| sistema/fuente | sí |
| verificación usada | sí |
| búsqueda realizada | sí |
| decisión/excepción | sí |
| respuesta y fecha | sí |
| borrado/export evidence | cuando aplique |

# 129. Política de privacidad

Este capítulo pertenece al bloque **Transparencia, políticas, web y comunicaciones**. Su objetivo es convertir **política de privacidad** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe política final. Una release offline sin recolección propia puede usar una política breve y exacta que explique datos locales, Steam/Valve y soporte voluntario.

**Regla de Cartridge & Cloud.** La política cambia antes o simultáneamente con el comportamiento; nunca después de activar un SDK.

**Procedimiento operativo.**
1. Inventariar tratamientos reales.
2. Redactar por capas y lenguaje claro.
3. Publicar ES/EN accesible.
4. Versionar y avisar cambios materiales.
5. Registrar específicamente el impacto de `Política de privacidad` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Versioned ES/EN policy, legal review, public URL and in-game link.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-003; PRIV-OFF-004; PRIV-OFF-008; PRIV-OFF-009`.

La información se presenta por capas: resumen contextual en el punto de recogida, detalle completo accesible y versión archivada. El texto no usa finalidades vagas como “mejorar la experiencia” sin explicar datos y uso.

# 130. Contenido mínimo

Este capítulo pertenece al bloque **Transparencia, políticas, web y comunicaciones**. Su objetivo es convertir **contenido mínimo** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay política final, web analítica, newsletter ni email público. Steam mantiene su propia política para datos de plataforma.

**Regla de Cartridge & Cloud.** La política debe describir el comportamiento real. No se publica texto genérico que anuncie tratamientos inexistentes ni oculte proveedores activos.

**Procedimiento operativo.**
1. Inventariar tratamientos reales.
2. Redactar por capas y lenguaje claro.
3. Publicar ES/EN accesible.
4. Versionar y avisar cambios materiales.
5. Registrar específicamente el impacto de `Contenido mínimo` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Política aprobada, snapshot publicado, copy in-game y registro de versiones.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-003; PRIV-OFF-004; PRIV-OFF-008; PRIV-OFF-009`.

La política final deberá identificar responsable y contacto; categorías y fuentes; finalidades y bases; destinatarios/proveedores; transferencias; conservación; derechos; reclamación; carácter obligatorio/opcional; decisiones automatizadas; menores; seguridad a nivel comprensible; cambios; y relación con Valve/Steam sin atribuirse sus tratamientos.

# 131. Versión y fecha

Este capítulo pertenece al bloque **Transparencia, políticas, web y comunicaciones**. Su objetivo es convertir **versión y fecha** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay política final, web analítica, newsletter ni email público. Steam mantiene su propia política para datos de plataforma.

**Regla de Cartridge & Cloud.** La política debe describir el comportamiento real. No se publica texto genérico que anuncie tratamientos inexistentes ni oculte proveedores activos.

**Procedimiento operativo.**
1. Inventariar tratamientos reales.
2. Redactar por capas y lenguaje claro.
3. Publicar ES/EN accesible.
4. Versionar y avisar cambios materiales.
5. Registrar específicamente el impacto de `Versión y fecha` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Política aprobada, snapshot publicado, copy in-game y registro de versiones.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-003; PRIV-OFF-004; PRIV-OFF-008; PRIV-OFF-009`.

# 132. Idiomas de la política

Este capítulo pertenece al bloque **Transparencia, políticas, web y comunicaciones**. Su objetivo es convertir **idiomas de la política** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay política final, web analítica, newsletter ni email público. Steam mantiene su propia política para datos de plataforma.

**Regla de Cartridge & Cloud.** La política debe describir el comportamiento real. No se publica texto genérico que anuncie tratamientos inexistentes ni oculte proveedores activos.

**Procedimiento operativo.**
1. Inventariar tratamientos reales.
2. Redactar por capas y lenguaje claro.
3. Publicar ES/EN accesible.
4. Versionar y avisar cambios materiales.
5. Registrar específicamente el impacto de `Idiomas de la política` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Política aprobada, snapshot publicado, copy in-game y registro de versiones.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-003; PRIV-OFF-004; PRIV-OFF-008; PRIV-OFF-009`.

La información se presenta por capas: resumen contextual en el punto de recogida, detalle completo accesible y versión archivada. El texto no usa finalidades vagas como “mejorar la experiencia” sin explicar datos y uso.

# 133. Accesibilidad de la política

Este capítulo pertenece al bloque **Transparencia, políticas, web y comunicaciones**. Su objetivo es convertir **accesibilidad de la política** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay política final, web analítica, newsletter ni email público. Steam mantiene su propia política para datos de plataforma.

**Regla de Cartridge & Cloud.** La política debe describir el comportamiento real. No se publica texto genérico que anuncie tratamientos inexistentes ni oculte proveedores activos.

**Procedimiento operativo.**
1. Inventariar tratamientos reales.
2. Redactar por capas y lenguaje claro.
3. Publicar ES/EN accesible.
4. Versionar y avisar cambios materiales.
5. Registrar específicamente el impacto de `Accesibilidad de la política` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Política aprobada, snapshot publicado, copy in-game y registro de versiones.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-003; PRIV-OFF-004; PRIV-OFF-008; PRIV-OFF-009`.

La información se presenta por capas: resumen contextual en el punto de recogida, detalle completo accesible y versión archivada. El texto no usa finalidades vagas como “mejorar la experiencia” sin explicar datos y uso.

# 134. Store page

Este capítulo pertenece al bloque **Transparencia, políticas, web y comunicaciones**. Su objetivo es convertir **store page** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay política final, web analítica, newsletter ni email público. Steam mantiene su propia política para datos de plataforma.

**Regla de Cartridge & Cloud.** La política debe describir el comportamiento real. No se publica texto genérico que anuncie tratamientos inexistentes ni oculte proveedores activos.

**Procedimiento operativo.**
1. Inventariar tratamientos reales.
2. Redactar por capas y lenguaje claro.
3. Publicar ES/EN accesible.
4. Versionar y avisar cambios materiales.
5. Registrar específicamente el impacto de `Store page` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Política aprobada, snapshot publicado, copy in-game y registro de versiones.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-003; PRIV-OFF-004; PRIV-OFF-008; PRIV-OFF-009`.

La información se presenta por capas: resumen contextual en el punto de recogida, detalle completo accesible y versión archivada. El texto no usa finalidades vagas como “mejorar la experiencia” sin explicar datos y uso.

# 135. Dentro del juego

Este capítulo pertenece al bloque **Transparencia, políticas, web y comunicaciones**. Su objetivo es convertir **dentro del juego** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay política final, web analítica, newsletter ni email público. Steam mantiene su propia política para datos de plataforma.

**Regla de Cartridge & Cloud.** La política debe describir el comportamiento real. No se publica texto genérico que anuncie tratamientos inexistentes ni oculte proveedores activos.

**Procedimiento operativo.**
1. Inventariar tratamientos reales.
2. Redactar por capas y lenguaje claro.
3. Publicar ES/EN accesible.
4. Versionar y avisar cambios materiales.
5. Registrar específicamente el impacto de `Dentro del juego` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Política aprobada, snapshot publicado, copy in-game y registro de versiones.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-003; PRIV-OFF-004; PRIV-OFF-008; PRIV-OFF-009`.

La información se presenta por capas: resumen contextual en el punto de recogida, detalle completo accesible y versión archivada. El texto no usa finalidades vagas como “mejorar la experiencia” sin explicar datos y uso.

# 136. Website

Este capítulo pertenece al bloque **Transparencia, políticas, web y comunicaciones**. Su objetivo es convertir **website** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay política final, web analítica, newsletter ni email público. Steam mantiene su propia política para datos de plataforma.

**Regla de Cartridge & Cloud.** La política debe describir el comportamiento real. No se publica texto genérico que anuncie tratamientos inexistentes ni oculte proveedores activos.

**Procedimiento operativo.**
1. Inventariar tratamientos reales.
2. Redactar por capas y lenguaje claro.
3. Publicar ES/EN accesible.
4. Versionar y avisar cambios materiales.
5. Registrar específicamente el impacto de `Website` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Política aprobada, snapshot publicado, copy in-game y registro de versiones.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-003; PRIV-OFF-004; PRIV-OFF-008; PRIV-OFF-009`.

La información se presenta por capas: resumen contextual en el punto de recogida, detalle completo accesible y versión archivada. El texto no usa finalidades vagas como “mejorar la experiencia” sin explicar datos y uso.

# 137. Soporte

Este capítulo pertenece al bloque **Transparencia, políticas, web y comunicaciones**. Su objetivo es convertir **soporte** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay política final, web analítica, newsletter ni email público. Steam mantiene su propia política para datos de plataforma.

**Regla de Cartridge & Cloud.** La política debe describir el comportamiento real. No se publica texto genérico que anuncie tratamientos inexistentes ni oculte proveedores activos.

**Procedimiento operativo.**
1. Inventariar tratamientos reales.
2. Redactar por capas y lenguaje claro.
3. Publicar ES/EN accesible.
4. Versionar y avisar cambios materiales.
5. Registrar específicamente el impacto de `Soporte` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Política aprobada, snapshot publicado, copy in-game y registro de versiones.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-003; PRIV-OFF-004; PRIV-OFF-008; PRIV-OFF-009`.

Los adjuntos voluntarios se consideran potencialmente identificables, se almacenan fuera del repositorio y se vinculan a un ticket mediante un ID interno, no mediante el nombre público del jugador.

# 138. Actualizaciones de política

Este capítulo pertenece al bloque **Transparencia, políticas, web y comunicaciones**. Su objetivo es convertir **actualizaciones de política** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay política final, web analítica, newsletter ni email público. Steam mantiene su propia política para datos de plataforma.

**Regla de Cartridge & Cloud.** La política debe describir el comportamiento real. No se publica texto genérico que anuncie tratamientos inexistentes ni oculte proveedores activos.

**Procedimiento operativo.**
1. Inventariar tratamientos reales.
2. Redactar por capas y lenguaje claro.
3. Publicar ES/EN accesible.
4. Versionar y avisar cambios materiales.
5. Registrar específicamente el impacto de `Actualizaciones de política` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Política aprobada, snapshot publicado, copy in-game y registro de versiones.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-003; PRIV-OFF-004; PRIV-OFF-008; PRIV-OFF-009`.

La información se presenta por capas: resumen contextual en el punto de recogida, detalle completo accesible y versión archivada. El texto no usa finalidades vagas como “mejorar la experiencia” sin explicar datos y uso.

# 139. Aviso de cambios

Este capítulo pertenece al bloque **Transparencia, políticas, web y comunicaciones**. Su objetivo es convertir **aviso de cambios** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay política final, web analítica, newsletter ni email público. Steam mantiene su propia política para datos de plataforma.

**Regla de Cartridge & Cloud.** La política debe describir el comportamiento real. No se publica texto genérico que anuncie tratamientos inexistentes ni oculte proveedores activos.

**Procedimiento operativo.**
1. Inventariar tratamientos reales.
2. Redactar por capas y lenguaje claro.
3. Publicar ES/EN accesible.
4. Versionar y avisar cambios materiales.
5. Registrar específicamente el impacto de `Aviso de cambios` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Política aprobada, snapshot publicado, copy in-game y registro de versiones.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-003; PRIV-OFF-004; PRIV-OFF-008; PRIV-OFF-009`.

La información se presenta por capas: resumen contextual en el punto de recogida, detalle completo accesible y versión archivada. El texto no usa finalidades vagas como “mejorar la experiencia” sin explicar datos y uso.

# 140. Cookies

Este capítulo pertenece al bloque **Transparencia, políticas, web y comunicaciones**. Su objetivo es convertir **cookies** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay web documentada. Una futura web estática puede evitar cookies no esenciales; embeds, analytics, newsletter o pixels cambian el análisis.

**Regla de Cartridge & Cloud.** Preferir cero tracking. Si existen tecnologías no necesarias, ofrecer información y elección válida según LSSI/AEPD.

**Procedimiento operativo.**
1. Inventariar tratamientos reales.
2. Redactar por capas y lenguaje claro.
3. Publicar ES/EN accesible.
4. Versionar y avisar cambios materiales.
5. Registrar específicamente el impacto de `Cookies` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Cookie inventory, scanner, consent test and preference log.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-003; PRIV-OFF-004; PRIV-OFF-008; PRIV-OFF-009`.

# 141. Website analytics

Este capítulo pertenece al bloque **Transparencia, políticas, web y comunicaciones**. Su objetivo es convertir **website analytics** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No está documentado. Steam dejó de soportar Google Analytics en store pages y ofrece reporting agregado propio.

**Regla de Cartridge & Cloud.** Para la web propia, comenzar sin analytics o con medición privacy-preserving evaluada; no copiar GA por defecto.

**Procedimiento operativo.**
1. Inventariar tratamientos reales.
2. Redactar por capas y lenguaje claro.
3. Publicar ES/EN accesible.
4. Versionar y avisar cambios materiales.
5. Registrar específicamente el impacto de `Website analytics` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Website traffic capture, cookie scan, provider assessment and policy.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-003; PRIV-OFF-004; PRIV-OFF-008; PRIV-OFF-009`.

La ficha de métrica documenta pregunta, decisión, población, denominador, ventana, timezone, sesgo, coste de privacidad y condición de retirada; sin esa ficha el evento no puede abrirse. La información se presenta por capas: resumen contextual en el punto de recogida, detalle completo accesible y versión archivada. El texto no usa finalidades vagas como “mejorar la experiencia” sin explicar datos y uso.

# 142. Newsletter

Este capítulo pertenece al bloque **Transparencia, políticas, web y comunicaciones**. Su objetivo es convertir **newsletter** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay política final, web analítica, newsletter ni email público. Steam mantiene su propia política para datos de plataforma.

**Regla de Cartridge & Cloud.** La política debe describir el comportamiento real. No se publica texto genérico que anuncie tratamientos inexistentes ni oculte proveedores activos.

**Procedimiento operativo.**
1. Inventariar tratamientos reales.
2. Redactar por capas y lenguaje claro.
3. Publicar ES/EN accesible.
4. Versionar y avisar cambios materiales.
5. Registrar específicamente el impacto de `Newsletter` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Política aprobada, snapshot publicado, copy in-game y registro de versiones.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-003; PRIV-OFF-004; PRIV-OFF-008; PRIV-OFF-009`.

# 143. Marketing

Este capítulo pertenece al bloque **Transparencia, políticas, web y comunicaciones**. Su objetivo es convertir **marketing** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay política final, web analítica, newsletter ni email público. Steam mantiene su propia política para datos de plataforma.

**Regla de Cartridge & Cloud.** La política debe describir el comportamiento real. No se publica texto genérico que anuncie tratamientos inexistentes ni oculte proveedores activos.

**Procedimiento operativo.**
1. Inventariar tratamientos reales.
2. Redactar por capas y lenguaje claro.
3. Publicar ES/EN accesible.
4. Versionar y avisar cambios materiales.
5. Registrar específicamente el impacto de `Marketing` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Política aprobada, snapshot publicado, copy in-game y registro de versiones.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-003; PRIV-OFF-004; PRIV-OFF-008; PRIV-OFF-009`.

# 144. Community management

Este capítulo pertenece al bloque **Transparencia, políticas, web y comunicaciones**. Su objetivo es convertir **community management** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay política final, web analítica, newsletter ni email público. Steam mantiene su propia política para datos de plataforma.

**Regla de Cartridge & Cloud.** La política debe describir el comportamiento real. No se publica texto genérico que anuncie tratamientos inexistentes ni oculte proveedores activos.

**Procedimiento operativo.**
1. Inventariar tratamientos reales.
2. Redactar por capas y lenguaje claro.
3. Publicar ES/EN accesible.
4. Versionar y avisar cambios materiales.
5. Registrar específicamente el impacto de `Community management` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Política aprobada, snapshot publicado, copy in-game y registro de versiones.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-003; PRIV-OFF-004; PRIV-OFF-008; PRIV-OFF-009`.

# 145. Moderación

Este capítulo pertenece al bloque **Transparencia, políticas, web y comunicaciones**. Su objetivo es convertir **moderación** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay política final, web analítica, newsletter ni email público. Steam mantiene su propia política para datos de plataforma.

**Regla de Cartridge & Cloud.** La política debe describir el comportamiento real. No se publica texto genérico que anuncie tratamientos inexistentes ni oculte proveedores activos.

**Procedimiento operativo.**
1. Inventariar tratamientos reales.
2. Redactar por capas y lenguaje claro.
3. Publicar ES/EN accesible.
4. Versionar y avisar cambios materiales.
5. Registrar específicamente el impacto de `Moderación` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Política aprobada, snapshot publicado, copy in-game y registro de versiones.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-003; PRIV-OFF-004; PRIV-OFF-008; PRIV-OFF-009`.

# 146. Evidencia de consentimiento

Este capítulo pertenece al bloque **Transparencia, políticas, web y comunicaciones**. Su objetivo es convertir **evidencia de consentimiento** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay política final, web analítica, newsletter ni email público. Steam mantiene su propia política para datos de plataforma.

**Regla de Cartridge & Cloud.** La política debe describir el comportamiento real. No se publica texto genérico que anuncie tratamientos inexistentes ni oculte proveedores activos.

**Procedimiento operativo.**
1. Inventariar tratamientos reales.
2. Redactar por capas y lenguaje claro.
3. Publicar ES/EN accesible.
4. Versionar y avisar cambios materiales.
5. Registrar específicamente el impacto de `Evidencia de consentimiento` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Política aprobada, snapshot publicado, copy in-game y registro de versiones.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-003; PRIV-OFF-004; PRIV-OFF-008; PRIV-OFF-009`.

La QA verifica simetría entre aceptar y rechazar, ausencia de transmisión previa, persistencia de la elección y efecto real de la revocación, no solo el cambio visual del toggle.

# 147. Seguridad técnica y organizativa

Este capítulo pertenece al bloque **Seguridad, riesgo, auditoría y calidad**. Su objetivo es convertir **seguridad técnica y organizativa** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay secretos o endpoints de producto observados; sí existen source packages, settings y futura evidencia de soporte que deben protegerse.

**Regla de Cartridge & Cloud.** Las medidas se ajustan al riesgo y cubren confidencialidad, integridad, disponibilidad, restauración y evaluación regular.

**Procedimiento operativo.**
1. Aplicar acceso mínimo y MFA.
2. Separar secretos y código.
3. Probar redacción, borrado y recuperación.
4. Mantener RAT y riesgo.
5. Simular incidente/brecha.
6. Registrar específicamente el impacto de `Seguridad técnica y organizativa` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Security checklist, audit log, incident drill, RAT/EIPD y test report.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007; PRIV-OFF-016`.

Los controles incluyen least privilege, MFA, segregación de entornos, inventario de secretos, rotación, logs de acceso, restauración y simulacros, con evidencia fechada.

# 148. Control de acceso

Este capítulo pertenece al bloque **Seguridad, riesgo, auditoría y calidad**. Su objetivo es convertir **control de acceso** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay secretos o endpoints de producto observados; sí existen source packages, settings y futura evidencia de soporte que deben protegerse.

**Regla de Cartridge & Cloud.** Las medidas se ajustan al riesgo y cubren confidencialidad, integridad, disponibilidad, restauración y evaluación regular.

**Procedimiento operativo.**
1. Aplicar acceso mínimo y MFA.
2. Separar secretos y código.
3. Probar redacción, borrado y recuperación.
4. Mantener RAT y riesgo.
5. Simular incidente/brecha.
6. Registrar específicamente el impacto de `Control de acceso` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Security checklist, audit log, incident drill, RAT/EIPD y test report.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007; PRIV-OFF-016`.

Los controles incluyen least privilege, MFA, segregación de entornos, inventario de secretos, rotación, logs de acceso, restauración y simulacros, con evidencia fechada.

# 149. Cifrado

Este capítulo pertenece al bloque **Seguridad, riesgo, auditoría y calidad**. Su objetivo es convertir **cifrado** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay secretos o endpoints de producto observados; sí existen source packages, settings y futura evidencia de soporte que deben protegerse.

**Regla de Cartridge & Cloud.** Las medidas se ajustan al riesgo y cubren confidencialidad, integridad, disponibilidad, restauración y evaluación regular.

**Procedimiento operativo.**
1. Aplicar acceso mínimo y MFA.
2. Separar secretos y código.
3. Probar redacción, borrado y recuperación.
4. Mantener RAT y riesgo.
5. Simular incidente/brecha.
6. Registrar específicamente el impacto de `Cifrado` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Security checklist, audit log, incident drill, RAT/EIPD y test report.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007; PRIV-OFF-016`.

Los controles incluyen least privilege, MFA, segregación de entornos, inventario de secretos, rotación, logs de acceso, restauración y simulacros, con evidencia fechada.

# 150. Secretos

Este capítulo pertenece al bloque **Seguridad, riesgo, auditoría y calidad**. Su objetivo es convertir **secretos** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay secretos o endpoints de producto observados; sí existen source packages, settings y futura evidencia de soporte que deben protegerse.

**Regla de Cartridge & Cloud.** Las medidas se ajustan al riesgo y cubren confidencialidad, integridad, disponibilidad, restauración y evaluación regular.

**Procedimiento operativo.**
1. Aplicar acceso mínimo y MFA.
2. Separar secretos y código.
3. Probar redacción, borrado y recuperación.
4. Mantener RAT y riesgo.
5. Simular incidente/brecha.
6. Registrar específicamente el impacto de `Secretos` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Security checklist, audit log, incident drill, RAT/EIPD y test report.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007; PRIV-OFF-016`.

Los controles incluyen least privilege, MFA, segregación de entornos, inventario de secretos, rotación, logs de acceso, restauración y simulacros, con evidencia fechada.

# 151. API keys

Este capítulo pertenece al bloque **Seguridad, riesgo, auditoría y calidad**. Su objetivo es convertir **api keys** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay secretos o endpoints de producto observados; sí existen source packages, settings y futura evidencia de soporte que deben protegerse.

**Regla de Cartridge & Cloud.** Las medidas se ajustan al riesgo y cubren confidencialidad, integridad, disponibilidad, restauración y evaluación regular.

**Procedimiento operativo.**
1. Aplicar acceso mínimo y MFA.
2. Separar secretos y código.
3. Probar redacción, borrado y recuperación.
4. Mantener RAT y riesgo.
5. Simular incidente/brecha.
6. Registrar específicamente el impacto de `API keys` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Security checklist, audit log, incident drill, RAT/EIPD y test report.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007; PRIV-OFF-016`.

Los controles incluyen least privilege, MFA, segregación de entornos, inventario de secretos, rotación, logs de acceso, restauración y simulacros, con evidencia fechada.

# 152. Variables de entorno

Este capítulo pertenece al bloque **Seguridad, riesgo, auditoría y calidad**. Su objetivo es convertir **variables de entorno** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay secretos o endpoints de producto observados; sí existen source packages, settings y futura evidencia de soporte que deben protegerse.

**Regla de Cartridge & Cloud.** Las medidas se ajustan al riesgo y cubren confidencialidad, integridad, disponibilidad, restauración y evaluación regular.

**Procedimiento operativo.**
1. Aplicar acceso mínimo y MFA.
2. Separar secretos y código.
3. Probar redacción, borrado y recuperación.
4. Mantener RAT y riesgo.
5. Simular incidente/brecha.
6. Registrar específicamente el impacto de `Variables de entorno` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Security checklist, audit log, incident drill, RAT/EIPD y test report.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007; PRIV-OFF-016`.

# 153. Logs de infraestructura

Este capítulo pertenece al bloque **Seguridad, riesgo, auditoría y calidad**. Su objetivo es convertir **logs de infraestructura** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay secretos o endpoints de producto observados; sí existen source packages, settings y futura evidencia de soporte que deben protegerse.

**Regla de Cartridge & Cloud.** Las medidas se ajustan al riesgo y cubren confidencialidad, integridad, disponibilidad, restauración y evaluación regular.

**Procedimiento operativo.**
1. Aplicar acceso mínimo y MFA.
2. Separar secretos y código.
3. Probar redacción, borrado y recuperación.
4. Mantener RAT y riesgo.
5. Simular incidente/brecha.
6. Registrar específicamente el impacto de `Logs de infraestructura` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Security checklist, audit log, incident drill, RAT/EIPD y test report.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007; PRIV-OFF-016`.

Los adjuntos voluntarios se consideran potencialmente identificables, se almacenan fuera del repositorio y se vinculan a un ticket mediante un ID interno, no mediante el nombre público del jugador.

# 154. Incident response

Este capítulo pertenece al bloque **Seguridad, riesgo, auditoría y calidad**. Su objetivo es convertir **incident response** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay secretos o endpoints de producto observados; sí existen source packages, settings y futura evidencia de soporte que deben protegerse.

**Regla de Cartridge & Cloud.** Las medidas se ajustan al riesgo y cubren confidencialidad, integridad, disponibilidad, restauración y evaluación regular.

**Procedimiento operativo.**
1. Aplicar acceso mínimo y MFA.
2. Separar secretos y código.
3. Probar redacción, borrado y recuperación.
4. Mantener RAT y riesgo.
5. Simular incidente/brecha.
6. Registrar específicamente el impacto de `Incident response` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Security checklist, audit log, incident drill, RAT/EIPD y test report.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007; PRIV-OFF-016`.

Los controles incluyen least privilege, MFA, segregación de entornos, inventario de secretos, rotación, logs de acceso, restauración y simulacros, con evidencia fechada.

# 155. Data breach

Este capítulo pertenece al bloque **Seguridad, riesgo, auditoría y calidad**. Su objetivo es convertir **data breach** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe repositorio remoto de jugadores, pero un buzón, ticketing, crash vendor o Cloud puede sufrir brecha. El RGPD exige documentar todas y notificar cuando proceda, con referencia de 72 horas para la autoridad cuando haya riesgo.

**Regla de Cartridge & Cloud.** Activar canal interno, preservar evidencia, contener, evaluar riesgo, decidir notificación y comunicar en lenguaje claro cuando corresponda.

**Procedimiento operativo.**
1. Aplicar acceso mínimo y MFA.
2. Separar secretos y código.
3. Probar redacción, borrado y recuperación.
4. Mantener RAT y riesgo.
5. Simular incidente/brecha.
6. Registrar específicamente el impacto de `Data breach` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Breach register, timeline, risk assessment, authority/user drafts and postmortem.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007; PRIV-OFF-016`.

Los controles incluyen least privilege, MFA, segregación de entornos, inventario de secretos, rotación, logs de acceso, restauración y simulacros, con evidencia fechada.

# 156. Evaluación de impacto

Este capítulo pertenece al bloque **Seguridad, riesgo, auditoría y calidad**. Su objetivo es convertir **evaluación de impacto** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No se ha realizado EIPD porque no existe tratamiento alto riesgo activo. Profiling, menores, telemetría extensa, biometría o combinación a gran escala serían triggers claros de revisión.

**Regla de Cartridge & Cloud.** La EIPD se realiza antes de abrir el tratamiento y puede concluir que no debe implementarse.

**Procedimiento operativo.**
1. Aplicar acceso mínimo y MFA.
2. Separar secretos y código.
3. Probar redacción, borrado y recuperación.
4. Mantener RAT y riesgo.
5. Simular incidente/brecha.
6. Registrar específicamente el impacto de `Evaluación de impacto` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Completed EIPD or documented screening and residual-risk decision.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007; PRIV-OFF-016`.

# 157. Registro de actividades

Este capítulo pertenece al bloque **Seguridad, riesgo, auditoría y calidad**. Su objetivo es convertir **registro de actividades** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No existe RAT formal. Debe crearse para soporte, web, colaboradores, Steamworks data y cualquier proveedor que procese datos personales.

**Regla de Cartridge & Cloud.** Una actividad por finalidad coherente; incluir categorías, destinatarios, transferencias, plazos y medidas.

**Procedimiento operativo.**
1. Aplicar acceso mínimo y MFA.
2. Separar secretos y código.
3. Probar redacción, borrado y recuperación.
4. Mantener RAT y riesgo.
5. Simular incidente/brecha.
6. Registrar específicamente el impacto de `Registro de actividades` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** RAT export and owner review.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007; PRIV-OFF-016`.

# 158. Inventario de datos

Este capítulo pertenece al bloque **Seguridad, riesgo, auditoría y calidad**. Su objetivo es convertir **inventario de datos** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay secretos o endpoints de producto observados; sí existen source packages, settings y futura evidencia de soporte que deben protegerse.

**Regla de Cartridge & Cloud.** Las medidas se ajustan al riesgo y cubren confidencialidad, integridad, disponibilidad, restauración y evaluación regular.

**Procedimiento operativo.**
1. Aplicar acceso mínimo y MFA.
2. Separar secretos y código.
3. Probar redacción, borrado y recuperación.
4. Mantener RAT y riesgo.
5. Simular incidente/brecha.
6. Registrar específicamente el impacto de `Inventario de datos` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Security checklist, audit log, incident drill, RAT/EIPD y test report.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007; PRIV-OFF-016`.

Los informes de Steam se tratan como una fuente separada: el estudio registra qué campos recibe, si son agregados y quién tiene permiso; no infiere acceso a datos individuales por el mero hecho de usar Steamworks.

# 159. Data-flow diagrams

Este capítulo pertenece al bloque **Seguridad, riesgo, auditoría y calidad**. Su objetivo es convertir **data-flow diagrams** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay secretos o endpoints de producto observados; sí existen source packages, settings y futura evidencia de soporte que deben protegerse.

**Regla de Cartridge & Cloud.** Las medidas se ajustan al riesgo y cubren confidencialidad, integridad, disponibilidad, restauración y evaluación regular.

**Procedimiento operativo.**
1. Aplicar acceso mínimo y MFA.
2. Separar secretos y código.
3. Probar redacción, borrado y recuperación.
4. Mantener RAT y riesgo.
5. Simular incidente/brecha.
6. Registrar específicamente el impacto de `Data-flow diagrams` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Security checklist, audit log, incident drill, RAT/EIPD y test report.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007; PRIV-OFF-016`.

```text
CURRENT
Player device -> local save / PlayerPrefs / Player.log
Player device -X-> VRM Games backend (no existe)
Player device -X-> analytics/crash endpoint (no integrado)

FUTURE SUPPORT
Player -> support email/ticket -> restricted case storage -> deletion

FUTURE OPTIONAL TELEMETRY
Game -> redaction/validation -> endpoint -> raw TTL -> aggregate -> deletion
```

# 160. Matriz de proveedores

Este capítulo pertenece al bloque **Seguridad, riesgo, auditoría y calidad**. Su objetivo es convertir **matriz de proveedores** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay secretos o endpoints de producto observados; sí existen source packages, settings y futura evidencia de soporte que deben protegerse.

**Regla de Cartridge & Cloud.** Las medidas se ajustan al riesgo y cubren confidencialidad, integridad, disponibilidad, restauración y evaluación regular.

**Procedimiento operativo.**
1. Aplicar acceso mínimo y MFA.
2. Separar secretos y código.
3. Probar redacción, borrado y recuperación.
4. Mantener RAT y riesgo.
5. Simular incidente/brecha.
6. Registrar específicamente el impacto de `Matriz de proveedores` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Security checklist, audit log, incident drill, RAT/EIPD y test report.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007; PRIV-OFF-016`.

El proveedor no se aprueba solo por disponer de una política pública: se revisan contrato, DPA, subprocesadores, localización, reutilización, incidentes, exportación, borrado y plan de salida.

# 161. Contratos

Este capítulo pertenece al bloque **Seguridad, riesgo, auditoría y calidad**. Su objetivo es convertir **contratos** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay secretos o endpoints de producto observados; sí existen source packages, settings y futura evidencia de soporte que deben protegerse.

**Regla de Cartridge & Cloud.** Las medidas se ajustan al riesgo y cubren confidencialidad, integridad, disponibilidad, restauración y evaluación regular.

**Procedimiento operativo.**
1. Aplicar acceso mínimo y MFA.
2. Separar secretos y código.
3. Probar redacción, borrado y recuperación.
4. Mantener RAT y riesgo.
5. Simular incidente/brecha.
6. Registrar específicamente el impacto de `Contratos` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Security checklist, audit log, incident drill, RAT/EIPD y test report.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007; PRIV-OFF-016`.

El proveedor no se aprueba solo por disponer de una política pública: se revisan contrato, DPA, subprocesadores, localización, reutilización, incidentes, exportación, borrado y plan de salida.

# 162. Auditoría

Este capítulo pertenece al bloque **Seguridad, riesgo, auditoría y calidad**. Su objetivo es convertir **auditoría** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay secretos o endpoints de producto observados; sí existen source packages, settings y futura evidencia de soporte que deben protegerse.

**Regla de Cartridge & Cloud.** Las medidas se ajustan al riesgo y cubren confidencialidad, integridad, disponibilidad, restauración y evaluación regular.

**Procedimiento operativo.**
1. Aplicar acceso mínimo y MFA.
2. Separar secretos y código.
3. Probar redacción, borrado y recuperación.
4. Mantener RAT y riesgo.
5. Simular incidente/brecha.
6. Registrar específicamente el impacto de `Auditoría` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Security checklist, audit log, incident drill, RAT/EIPD y test report.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007; PRIV-OFF-016`.

# 163. QA

Este capítulo pertenece al bloque **Seguridad, riesgo, auditoría y calidad**. Su objetivo es convertir **qa** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay secretos o endpoints de producto observados; sí existen source packages, settings y futura evidencia de soporte que deben protegerse.

**Regla de Cartridge & Cloud.** Las medidas se ajustan al riesgo y cubren confidencialidad, integridad, disponibilidad, restauración y evaluación regular.

**Procedimiento operativo.**
1. Aplicar acceso mínimo y MFA.
2. Separar secretos y código.
3. Probar redacción, borrado y recuperación.
4. Mantener RAT y riesgo.
5. Simular incidente/brecha.
6. Registrar específicamente el impacto de `QA` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Security checklist, audit log, incident drill, RAT/EIPD y test report.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007; PRIV-OFF-016`.

# 164. Tests

Este capítulo pertenece al bloque **Seguridad, riesgo, auditoría y calidad**. Su objetivo es convertir **tests** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** La baseline `1215 EditMode + 70 PlayMode` no incluye una campaña de privacidad. Se añadirán tests de no transmisión, redacción, consent, deletion, schema y provider-off behavior.

**Regla de Cartridge & Cloud.** Privacidad se valida como comportamiento observable y no solo documentación.

**Procedimiento operativo.**
1. Aplicar acceso mínimo y MFA.
2. Separar secretos y código.
3. Probar redacción, borrado y recuperación.
4. Mantener RAT y riesgo.
5. Simular incidente/brecha.
6. Registrar específicamente el impacto de `Tests` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Automated results, packet capture, clean-machine test and evidence hash.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007; PRIV-OFF-016`.

# 165. Work packages

Este capítulo pertenece al bloque **Seguridad, riesgo, auditoría y calidad**. Su objetivo es convertir **work packages** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** No hay secretos o endpoints de producto observados; sí existen source packages, settings y futura evidencia de soporte que deben protegerse.

**Regla de Cartridge & Cloud.** Las medidas se ajustan al riesgo y cubren confidencialidad, integridad, disponibilidad, restauración y evaluación regular.

**Procedimiento operativo.**
1. Aplicar acceso mínimo y MFA.
2. Separar secretos y código.
3. Probar redacción, borrado y recuperación.
4. Mantener RAT y riesgo.
5. Simular incidente/brecha.
6. Registrar específicamente el impacto de `Work packages` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Security checklist, audit log, incident drill, RAT/EIPD y test report.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007; PRIV-OFF-016`.

| ID | Paquete | Gate |
|---|---|---|
| PRIV-WP-01 | Inventario/RAT inicial | Pre-Steam |
| PRIV-WP-02 | Política ES/EN local-first | H6 / store |
| PRIV-WP-03 | Soporte, notice y retención | Pre-release |
| PRIV-WP-04 | Redacción de logs y adjuntos | Pre-release |
| PRIV-WP-05 | Vendor/DPA template | Before provider |
| PRIV-WP-06 | Rights request workflow | Pre-release |
| PRIV-WP-07 | Breach runbook and drill | Pre-release |
| PRIV-WP-08 | Website/cookie audit | Before website |
| PRIV-WP-09 | Steam data/permissions map | Onboarding |
| PRIV-WP-10 | Cloud privacy gate | Optional feature |
| PRIV-WP-11 | Crash provider gate | Optional feature |
| PRIV-WP-12 | Telemetry experiment gate | Post-launch only if justified |

# 166. Gates

Este capítulo pertenece al bloque **Ejecución, gates y mantenimiento**. Su objetivo es convertir **gates** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Todo sistema online permanece `NOT OPEN`. H6 no exige telemetría; la release puede y debe avanzar sin ella si no aporta valor proporcional.

**Regla de Cartridge & Cloud.** Gate exige finalidad, RAT, legal basis, DPA, policy, security, QA, rollback/kill switch y aprobación manual.

**Procedimiento operativo.**
1. Crear paquete con owner y DoR.
2. Cerrar diseño/contrato/privacidad.
3. Implementar detrás de feature flag.
4. Ejecutar QA y auditoría.
5. Tomar decisión Go/No-Go.
6. Registrar específicamente el impacto de `Gates` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Signed Go/No-Go with evidence pack.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007`.

| Gate | Condiciones mínimas |
|---|---|
| Privacy baseline | RAT, roles, local data map, owner |
| Support | notice, provider, MFA, retention, delete |
| Website | cookie scan, policy, consent if needed |
| Steam APIs | data/API inventory, permissions, secrets, rights |
| Steam Cloud | paths, role, policy, conflict and delete tests |
| Crash SDK | DPA, data map, redaction, retention, opt/control |
| Telemetry | purpose, necessity, schema, base, policy, QA, kill switch |
| Public release | behavior matches policy; no unauthorized endpoints |

# 167. Checklists

Este capítulo pertenece al bloque **Ejecución, gates y mantenimiento**. Su objetivo es convertir **checklists** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** La implementación permanece bloqueada hasta existir finalidad, arquitectura, proveedor, política, seguridad, QA y revisión legal.

**Regla de Cartridge & Cloud.** Ningún SDK, endpoint, cookie, formulario o feature online se incorpora fuera de un work package y gate explícito.

**Procedimiento operativo.**
1. Crear paquete con owner y DoR.
2. Cerrar diseño/contrato/privacidad.
3. Implementar detrás de feature flag.
4. Ejecutar QA y auditoría.
5. Tomar decisión Go/No-Go.
6. Registrar específicamente el impacto de `Checklists` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Work package, checklist, risk acceptance, evidence pack y changelog.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007`.

- [ ] No endpoint/SDK nuevo sin ADR y privacy review.
- [ ] Package/settings scan actualizado.
- [ ] Captura de tráfico de build limpia.
- [ ] Política ES/EN coincide con comportamiento.
- [ ] Retención y borrado probados.
- [ ] Proveedores y subprocesadores registrados.
- [ ] Secrets fuera del cliente/repositorio.
- [ ] Derechos y breach drill ejecutados.
- [ ] Build/commit/checksum archivados.

# 168. Plantillas

Este capítulo pertenece al bloque **Ejecución, gates y mantenimiento**. Su objetivo es convertir **plantillas** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** La implementación permanece bloqueada hasta existir finalidad, arquitectura, proveedor, política, seguridad, QA y revisión legal.

**Regla de Cartridge & Cloud.** Ningún SDK, endpoint, cookie, formulario o feature online se incorpora fuera de un work package y gate explícito.

**Procedimiento operativo.**
1. Crear paquete con owner y DoR.
2. Cerrar diseño/contrato/privacidad.
3. Implementar detrás de feature flag.
4. Ejecutar QA y auditoría.
5. Tomar decisión Go/No-Go.
6. Registrar específicamente el impacto de `Plantillas` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Work package, checklist, risk acceptance, evidence pack y changelog.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007`.

```text
DATA ACTIVITY ID:
Purpose / decision:
Controller / processor roles:
Data categories and sources:
Recipients / transfers:
Legal basis:
Retention and deletion:
Security controls:
Rights workflow:
DPIA screening:
Build / feature flag:
Approval / date:
```

# 169. Riesgos

Este capítulo pertenece al bloque **Ejecución, gates y mantenimiento**. Su objetivo es convertir **riesgos** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** Riesgos principales: activar SDK accidentalmente, confundir módulo con servicio, logs con PII, save uploads, ID estable, proveedor que reutiliza datos, transferencias, política inexacta, Cloud prematuro y brecha sin runbook.

**Regla de Cartridge & Cloud.** Cada riesgo tiene probabilidad, impacto, trigger, owner, mitigación, contingencia y evidencia de cierre.

**Procedimiento operativo.**
1. Crear paquete con owner y DoR.
2. Cerrar diseño/contrato/privacidad.
3. Implementar detrás de feature flag.
4. Ejecutar QA y auditoría.
5. Tomar decisión Go/No-Go.
6. Registrar específicamente el impacto de `Riesgos` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Risk register reviewed per release.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007`.

# 170. Glosario

Este capítulo pertenece al bloque **Ejecución, gates y mantenimiento**. Su objetivo es convertir **glosario** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** La implementación permanece bloqueada hasta existir finalidad, arquitectura, proveedor, política, seguridad, QA y revisión legal.

**Regla de Cartridge & Cloud.** Ningún SDK, endpoint, cookie, formulario o feature online se incorpora fuera de un work package y gate explícito.

**Procedimiento operativo.**
1. Crear paquete con owner y DoR.
2. Cerrar diseño/contrato/privacidad.
3. Implementar detrás de feature flag.
4. Ejecutar QA y auditoría.
5. Tomar decisión Go/No-Go.
6. Registrar específicamente el impacto de `Glosario` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Work package, checklist, risk acceptance, evidence pack y changelog.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007`.

| Término | Definición operativa |
|---|---|
| dato personal | información sobre persona identificada o identificable |
| seudónimo | dato aún personal con vínculo reducido/separado |
| anónimo | no identificable razonablemente tras evaluación |
| telemetría | monitorización estructurada de uso/rendimiento enviada o procesada |
| evento | unidad versionada del esquema de telemetría |
| RAT | registro de actividades de tratamiento |
| EIPD/DPIA | evaluación previa para tratamientos de alto riesgo |
| responsable | decide fines y medios esenciales |
| encargado | trata por cuenta e instrucciones del responsable |
| subprocesador | encargado contratado por otro encargado |

# 171. Historial de cambios

Este capítulo pertenece al bloque **Ejecución, gates y mantenimiento**. Su objetivo es convertir **historial de cambios** en una decisión verificable de producto, no en una declaración genérica de cumplimiento. Se aplica a desarrollo, QA, soporte, Steam, web y cualquier proveedor futuro.

**Estado y contexto.** La implementación permanece bloqueada hasta existir finalidad, arquitectura, proveedor, política, seguridad, QA y revisión legal.

**Regla de Cartridge & Cloud.** Ningún SDK, endpoint, cookie, formulario o feature online se incorpora fuera de un work package y gate explícito.

**Procedimiento operativo.**
1. Crear paquete con owner y DoR.
2. Cerrar diseño/contrato/privacidad.
3. Implementar detrás de feature flag.
4. Ejecutar QA y auditoría.
5. Tomar decisión Go/No-Go.
6. Registrar específicamente el impacto de `Historial de cambios` en el inventario de datos, el riesgo, la política y la build afectada.

**Evidencia mínima.** Work package, checklist, risk acceptance, evidence pack y changelog.

**Referencias oficiales relacionadas:** `PRIV-OFF-001; PRIV-OFF-004; PRIV-OFF-007`.

# Anexo A. Inventario de datos actual

| Superficie | Datos | Ubicación | Recibe VRM Games por defecto | Estado |
|---|---|---|---|---|
| Integrated save | schema, session ID sintético, slot, timestamps, día, cash, inventarios, pedidos, clientes sintéticos, checkout, ledger | `persistentDataPath/IntegratedSaveGames` | no | CURRENT / LOCAL |
| Legacy save skeleton | slot, session ID sintético, timestamps, día, cash | `persistentDataPath/SaveGames` | no | TECHNICAL / LOCAL |
| PlayerPrefs audio | volumen por canal | registro/preferences local Unity | no | CURRENT / LOCAL |
| PlayerPrefs walls | `CC_S16_P1_HideOccludingWalls` | preferences local Unity | no | CURRENT / LOCAL |
| Player.log | eventos, excepciones, stack traces | local Unity | no | CURRENT / LOCAL |
| Build evidence | logs/checksums internos | documentación interna | sí, generado por desarrollo | CURRENT / INTERNAL |
| Support attachments | email, log, save, screenshot, hardware | no existe sistema | no | FUTURE / VOLUNTARY |
| Steam reports | tráfico/UTM/ventas/reembolsos agregados | Steamworks | no AppID | FUTURE PLATFORM |
| SteamID | identificador de plataforma | no integrado | no | NOT OPEN |
| Telemetry events | comportamiento/rendimiento | no endpoint | no | NOT OPEN |
| Crash dumps | memoria/estado | no SDK | no | NOT OPEN |

# Anexo B. Unity Services y paquetes observados

| Elemento | Valor observado | Interpretación prudente |
|---|---|---|
| UnityConnect global | `1` | configuración de conexión del proyecto; revisar por separado del player |
| Insights enabled | `0` | desactivado |
| Engine diagnostics | `1` | habilitado en setting de herramienta; verificar alcance/editor |
| Cloud Diagnostics reporting | `0` | desactivado |
| Unity Analytics | `0` | desactivado |
| Unity Ads | `0` | desactivado |
| Unity Purchasing | `0` | desactivado |
| Performance Reporting | `0` | desactivado |
| Built-in unityanalytics module | `sí` | presencia de módulo no demuestra activación |
| Direct packages | `41` | revisar licencias/settings por release |
| Locked packages | `57` | incluye transitivos |

# Anexo C. Resultado del escaneo de integraciones

| Búsqueda | Hits | Interpretación |
|---|---:|---|
| Steamworks integration | 0 | No encontrado |
| UGS Analytics runtime integration | 0 | No encontrado |
| External analytics SDK | 1 | Revisar lista; no asumir tratamiento sin analizar contexto |
| External crash SDK | 0 | No encontrado |
| Network / HTTP product code | 0 | No encontrado |
| Steam Cloud integration | 0 | No encontrado |
| Device fingerprinting | 0 | No encontrado |
| Email/account fields | 1 | Revisar lista; no asumir tratamiento sin analizar contexto |

Archivos con `PlayerPrefs`: **3**. Archivos con llamadas `Debug.Log*`: **26**. Estas cifras describen código, no transmisiones de red.

# Anexo D. RAT inicial propuesto

| Activity ID | Finalidad | Datos | Fuente | Base a revisar | Destinatarios | Retención | Estado |
|---|---|---|---|---|---|---|---|
| RAT-001 | guardar progreso | estado de juego local | jugador/dispositivo | ejecución local del producto; fuera de recepción del estudio | ninguno | hasta borrado por usuario | CURRENT / LOCAL |
| RAT-002 | preferencias | volumen/occlusion | jugador | ejecución local | ninguno | hasta reset/uninstall | CURRENT / LOCAL |
| RAT-003 | soporte | email, log, save, screenshot, hardware | usuario | contrato/interés/consentimiento según campo, revisar | proveedor email/ticketing | según schedule | PLANNED |
| RAT-004 | Steam store reporting | agregados de tráfico/comercio | Valve | relación partner / términos, revisar | personal autorizado | según Steam/archivo interno | FUTURE |
| RAT-005 | crash diagnostics | crash report/dump | juego/dispositivo | TBD | proveedor crash | TBD | NOT OPEN |
| RAT-006 | gameplay telemetry | eventos estructurados | juego | TBD, opt-in target | analytics processor | TBD | NOT OPEN |
| RAT-007 | Steam Cloud | saves/config por Steam user | dispositivo/Steam | prestación feature, revisar roles | Valve | mientras feature/cuenta | NOT OPEN |
| RAT-008 | newsletter | email/preferencias | usuario | consentimiento | email provider | hasta revocación + evidence | NOT OPEN |

# Anexo E. Matriz de proveedores

| Proveedor/servicio | Uso actual | Datos potenciales | Rol por confirmar | Gate |
|---|---|---|---|---|
| Valve / Steam | futuro storefront | cuenta Steam, commerce, reports | Valve controller; VRM role según dato recibido | Steam onboarding |
| Unity | motor/editor/packages | cuenta de desarrollo, diagnostics de herramienta | según servicio/términos | release/tool audit |
| Email provider | no elegido | email, headers, attachments | processor/controller aspects | support gate |
| Ticketing | no elegido | tickets, logs, saves | processor | support gate |
| Crash provider | no elegido | crash, device, dump | processor/controller reuse risk | crash gate |
| Analytics provider | no elegido | events, identifiers | processor/controller reuse risk | telemetry gate |
| Website host/CDN | no elegido | IP/access logs | processor/controller | website gate |
| Newsletter provider | no elegido | email, consent, engagement | processor | marketing gate |

# Anexo F. Fuentes oficiales verificadas

Las siguientes fuentes se verificaron el **1 de julio de 2026**. Deben revalidarse antes de publicar o activar un tratamiento. La guía AEPD de videojuegos es sectorial y reciente, pero no sustituye el análisis específico del proyecto.

| ID | Fuente | URL | Uso |
|---|---|---|---|
| `PRIV-OFF-001` | RGPD — Reglamento (UE) 2016/679 | https://www.boe.es/buscar/doc.php?id=DOUE-L-2016-80807 | Principios, licitud, transparencia, derechos, diseño, seguridad, brechas, EIPD y responsabilidad. |
| `PRIV-OFF-002` | LOPDGDD — Ley Orgánica 3/2018 | https://www.boe.es/buscar/act.php?id=BOE-A-2018-16673 | Marco español y consentimiento de menores desde los catorce años, con las excepciones legales aplicables. |
| `PRIV-OFF-003` | LSSI-CE — Ley 34/2002 | https://www.boe.es/buscar/act.php?id=BOE-A-2002-13758 | Servicios online, comunicaciones y almacenamiento/acceso en equipos terminales, incluido el marco de cookies. |
| `PRIV-OFF-004` | AEPD / Autoridad Belga — Recomendaciones para videojuegos (2026) | https://www.aepd.es/guias/recomendaciones-industria-videojuego.pdf | Guía sectorial sobre cuentas, telemetría, inferencias, roles, minimización, consentimiento, retención, menores y lifecycle. |
| `PRIV-OFF-005` | AEPD — Nota de publicación de la guía de videojuegos | https://www.aepd.es/prensa-y-comunicacion/notas-de-prensa/aepd-y-autoridad-belga-impulsan-buenas-practicas-sector-videojuego | Contexto, alcance y fecha de la guía sectorial. |
| `PRIV-OFF-006` | EDPB — Anonymisation / Pseudonymisation | https://www.edpb.europa.eu/topics/ai-and-technology/anonymisationpseudonymisation_en | Diferencia entre anonimización y seudonimización; la segunda sigue siendo dato personal. |
| `PRIV-OFF-007` | AEPD — Gestiona RGPD | https://www.aepd.es/guias-y-herramientas/herramientas/gestiona2 | Herramienta de apoyo para RAT, riesgo y EIPD; no reemplaza el análisis responsable. |
| `PRIV-OFF-008` | AEPD — Guía de cookies | https://www.aepd.es/es/documento/guia-cookies.pdf | Criterios para información, consentimiento, rechazo y gestión de preferencias en web. |
| `PRIV-OFF-009` | Valve — Steam Privacy Policy | https://store.steampowered.com/privacy_agreement/ | Tratamientos de Valve, Privacy Dashboard y derechos de usuarios Steam. |
| `PRIV-OFF-010` | Steamworks — Store and Platform Traffic Reporting | https://partner.steamgames.com/doc/marketing/traffic_reporting | Informes agregados de tráfico de la plataforma. |
| `PRIV-OFF-011` | Steamworks — Google Analytics support ended | https://partner.steamgames.com/doc/marketing/google_analytics | Steam dejó de soportar Google Analytics y prioriza reporting agregado con privacidad. |
| `PRIV-OFF-012` | Steamworks — UTM Analytics | https://partner.steamgames.com/doc/marketing/utm_analytics | Métricas agregadas, umbrales y ausencia de SteamID/datos personales en reportes UTM. |
| `PRIV-OFF-013` | Steamworks — Steam Cloud | https://partner.steamgames.com/doc/features/cloud | API y Auto-Cloud, archivos por usuario y sincronización. |
| `PRIV-OFF-014` | Steamworks — Authentication and Ownership | https://partner.steamgames.com/doc/features/auth | Obtención y uso de SteamID cuando se integra autenticación/ownership. |
| `PRIV-OFF-015` | Steamworks Web API — ISteamUser | https://partner.steamgames.com/doc/webapi/ISteamUser | GetDeletedSteamIDs y obligaciones operativas si se almacenan datos ligados a SteamID. |
| `PRIV-OFF-016` | Steamworks — Managing Users | https://partner.steamgames.com/doc/gettingstarted/managing_users | Permisos y control de acceso a datos/reportes en Steamworks. |

# Anexo G. Evidencia técnica hashada

| Archivo | Bytes | SHA-256 |
|---|---:|---|
| `Assets/_Project/Scripts/Domain/Persistence/IntegratedGameStateSnapshot.cs` | 20654 | `af49722b2ffdac7370ffa90bf104430e830b9ed2c69ac4952b2929478a547f62` |
| `Assets/_Project/Scripts/Infrastructure/Persistence/IntegratedSaveJsonCodec.cs` | 31736 | `d38c0c5b67e14d3f4fd81813d5f22d31759e45ac70f73ed5eff8a22593143656` |
| `Assets/_Project/Scripts/Infrastructure/Persistence/JsonIntegratedSaveRepository.cs` | 13424 | `8a5408551acedf29a900d04ba627f82345f7f99e2d0c6ad439f0cff0b7a88106` |
| `Assets/_Project/Scripts/Infrastructure/GameSession/JsonSaveGameRepository.cs` | 11042 | `4841b006e79fece72618b5d9569b875307bbd8143c0003f86d74935e5e6d1e39` |
| `Assets/_Project/Scripts/Infrastructure/UIUX/AtomicJsonFile.cs` | 2065 | `ead9646924fe5361a035cfacb10be8ab92beff9d510e8d393b175286ba49020a` |
| `Assets/_Project/Scripts/Infrastructure/UIUX/Sprint15RuntimeCompositionRoot.cs` | 15188 | `9ad97ab7a06f4c859d916645198b9ed3ac2689c9c431ecd5f9db7c7e9932aaf9` |
| `Assets/_Project/Scripts/Runtime/VerticalSlicePhase1/Sprint16Phase1RuntimeRoot.cs` | 15407 | `f5222d4ac5ff971d94606cbf6d0f5ef70d3a24edfeaa4f9aaac424cb7aafeeb8` |
| `Assets/_Project/Scripts/Runtime/VerticalSlicePhase1/Phase1AudioRouter.cs` | 3924 | `cb09500abc2614d75a6f47fea02698ee542eac3987c2468a57341da476c54398` |
| `Assets/_Project/Scripts/Infrastructure/VerticalSlicePhase1/Phase1WallOcclusionController.cs` | 3204 | `a5dcbd4ed2f3d782e8817b21537d06096d9c62bd71a9ffe4947d6343cad1a784` |
| `Assets/_Project/Settings/BuildProfiles/Windows_Development.asset` | 1946 | `63e658288030c33844a84c37fc636fa908e9d8fbe44d2180aefcf03a0e5c93fa` |

# Anexo H. Escenas de build observadas

| Enabled | Escena | GUID |
|---|---|---|
| sí | `Assets/_Project/Scenes/Bootstrap.unity` | `ada1e52ac6e09b9468e2109ccc7cbfc0` |
| sí | `Assets/_Project/Scenes/MainMenu.unity` | `b00786c6952a55d418cf16503030c766` |
| sí | `Assets/_Project/Scenes/Store.unity` | `45adcce906944f74bad3a0fb7d62f8e5` |
| sí | `Assets/_Project/Scenes/TestLab.unity` | `002b2ad62af3c614b91e32722c9452be` |

# Anexo I. Documentos consolidados

| Documento | Bytes | SHA-256 |
|---|---:|---|
| `00_Enfoque_y_Alcance.md` | 127401 | `63dd6f0f1b9c2c80be2aec55718d18d9d031ce4acb14f677769d6e69ceaad21f` |
| `01_Game_Design_Document.md` | 132822 | `a9cd9e3203abc5269c25c59dc7f626b0771df4ec2d65e781109ff146bceb2177` |
| `02_Vertical_Slice_Specification.md` | 64531 | `75893f130116e88a08b8da5590dd81876a234581081eafaa8d08374c1a60513f` |
| `02_Vertical_Slice_Specification.preview.md` | 64531 | `75893f130116e88a08b8da5590dd81876a234581081eafaa8d08374c1a60513f` |
| `03_Technical_Design_Document.draft.md` | 101994 | `66264703c5ff4959d03093690e9845a98ec0e5025e685b9a639ac8cbb616cd12` |
| `03_Technical_Design_Document.md` | 101994 | `66264703c5ff4959d03093690e9845a98ec0e5025e685b9a639ac8cbb616cd12` |
| `04_Modelo_de_Datos.draft.md` | 102948 | `d851a72b55742c2c704cc7c002929bc5894e7142511c6af843440bb193fb19d4` |
| `04_Modelo_de_Datos.md` | 102948 | `d851a72b55742c2c704cc7c002929bc5894e7142511c6af843440bb193fb19d4` |
| `05_UX_Flow.md` | 56805 | `e08da5ff67fb073a8b950babe325ae58ddb2e121513dc2f553acd4f2c14b867e` |
| `06_Production_Roadmap_y_Sprint_Plan.md` | 61564 | `d00b156c0ad1c66069278119c55f76c738a577a3e2835f770208d1a5f2faadff` |
| `07_QA_Testing_Plan.md` | 79435 | `bc9c05a3737e345546ef16e5390e034295ef35ad66c31647852aec81b4e7affe` |
| `08_QA_Testing_Matrix.xlsx` | 185032 | `233e57f01d2468fe13c136c8fcd3ee75b39bd3de6530349626ea98d4ff254214` |
| `09_CSharp_Coding_Standards.md` | 95321 | `3b734d6cd49f31c09c10bb4941625e143f6c634e3fb50c157f0720e73b1c2a68` |
| `10_Unity_Project_Setup_Guide.md` | 78408 | `31430bb0544edd9577323aa0009d6ca3df8b9fbcf6c8a150fc33df69b9f963a4` |
| `11_Build_y_Versioning_Guide.md` | 85324 | `38a16a0f97505c0b405509dd3c7e3d1b50e77b89fbe3f4cb1931d3e04f602215` |
| `12_Excel_Maestro_de_Produccion.xlsx` | 158100 | `405376cb64f49b34f1f842f3840c12654e9f08e2ec4534b5c149fa811e87dd83` |
| `13_Trazabilidad_y_Control_de_Cambios.xlsx` | 189424 | `f674e21d1a3a1f0970a3d339f26f2a2b51a19b5c257c24b9d3fa4118a5cf50eb` |
| `14_Project_Binder_Indice_Maestro.md` | 92828 | `0c24dcd47e0d44793e75e764e3fc2c146e5b1fea04d887d43feb0df558711559` |
| `15_Guia_Maestra.md` | 119920 | `879bc1925cccf77e10df6c104302f3afc72f71e8c58dd85ae4f06e84e6f8b624` |
| `16_Auditoria_Global_de_Coherencia.md` | 87439 | `4a62376b8aee623be40957ae0511d2dcab2919947f3abb2cef3502b9bb60863e` |
| `17_Art_Bible.md` | 133913 | `095c6cc42223a972c7faa68435595fc7c22f839fabc62b2038371a0ef6c14239` |
| `18_Audio_Bible.md` | 144525 | `c2eabe93a7c26f70e1d4d77fb0ddcaa97e2cdd1e70247e637b9db5bd2f07ce92` |
| `19_UI_Style_Guide.md` | 149800 | `b34ca4db1eba7a536466b6a9a105d8d92396ade2108aa7dd99fa7d5d4eaebc1a` |
| `20_Economy_and_Balance_Specification.md` | 154547 | `908c70772ed35eecdb05c3036c6901523a4caf2c3e708ca8f049c2a160ef2aac` |
| `21_Initial_Content_Catalog.xlsx` | 174383 | `9047299f60cfbd368fc4a2c8bee8d0ff4132dc5642363a9a67d1523559eb9ed3` |
| `22_Localization_Plan.md` | 157323 | `184a53b2813bc1327a6913e6e88ea4c81142da1ee4ddeff88f75467c784f1ff0` |
| `23_Legal_Credits_and_Licenses_Register.xlsx` | 465135 | `f543b3b5de8900f405910fac5d72697045f8c4969d98fce4c1cf4f12e532b2f1` |
| `24_Steam_Publishing_Plan.md` | 171037 | `f087e24d3a18f8c4a6651728d5f70d1df804042587e206fdeac2e7e637a87646` |
| `25_Post_Launch_and_Live_Operations_Plan.md` | 327218 | `158e1e43514c5d5d6c365f6cd1a73929f54c4a48c5cc460b593530ae5e49bac9` |

# Anexo J. Inventario histórico y operativo preservado

Se inventarían **354** fuentes relacionadas con privacidad, legal, Steam, soporte, persistencia, QA, builds, analytics, Cloud y gobernanza. La ausencia de un plan histórico dedicado se conserva como hallazgo: la política previa era condicional y fragmentaria.

| Baseline/área | Tipo | Fuente | Bytes | SHA-256 |
|---|---|---|---:|---|
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/00_Project_Governance/00_Cartridge_And_Cloud_Guide_v0.3.pdf` | 103150 | `99c1672344d83538646b37426ea23c6812c2f83c3386296618de7ee7a8524131` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/00_Project_Governance/Cartridge_And_Cloud_Document_Migration_Plan_v0.1.pdf` | 22634 | `9fb9be3ff80060dad211034db3e372b0d816978b92b3dd3c220ec694a6e713fc` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/00_Project_Governance/Cartridge_And_Cloud_Documentation_Validation_Report_v0.1.pdf` | 29737 | `2650c8310ea64b2328ee460843e04eed8b2dfa5f954e0acf0c1914e895a3628f` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/00_Project_Governance/Cartridge_And_Cloud_Enfoque_v0.6.pdf` | 338168 | `38f2028567bb433fb5162f8c008b4e21d532f819cf5c0c3204c2ca06a2eb7eef` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/00_Project_Governance/Cartridge_And_Cloud_Package_Manifest_v0.1.pdf` | 40833 | `4aed1b174d2405952bd79800726f58314e2bf96842da2b233cf005db84689f83` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/00_Project_Governance/Cartridge_And_Cloud_Project_Binder_Indice_Maestro_v0.3.pdf` | 26270 | `c71af37b9e9be41ba8f31ae35ac244d4fb3163ab19a7e43656ced308f746ee58` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/00_Project_Governance/Enfoque_v0.6.md` | 122708 | `6765207677c2adad0d124d20b9c27c82563eebc5eb3a0acb817dddbd148b3fbb` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/01_Design/Cartridge_And_Cloud_Economy_Balance_Specification_v0.1.pdf` | 102712 | `9539e4a8e097a84c413a8b65a8bc77d2d2a859eeab9c9697e84473ce67025ab7` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/01_Design/Cartridge_And_Cloud_GDD_v0.4_PCSteam.pdf` | 292627 | `9d07c1cbfde60aa02403129f5b0b2b36852cfb514123e836d8c8e81ee7fa837d` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/01_Design/Cartridge_And_Cloud_Initial_Content_Catalog_v0.1.pdf` | 82740 | `f2f3531bc334dba5740d3278f9d4fc2a767dd0520029888fda5d2957503b5b52` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/01_Design/Cartridge_And_Cloud_UX_Flow_v0.3.pdf` | 227782 | `f46ac97e14ce7b1ec45258c84a7f3fc8209f43ee807219114eee1d517aa976db` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/01_Design/Cartridge_And_Cloud_Vertical_Slice_Specification_v0.1.pdf` | 37172 | `86b0a49dac832eda01fa751b5a9b720610868ba389093d2a8784222d01326ac3` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/02_Technical/Cartridge_And_Cloud_Build_Versioning_Guide_v0.3.pdf` | 25562 | `81e38d9861f5caee0d4448c0aff214e0ffc922e82e2491f8b37c7f37719bd97a` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/02_Technical/Cartridge_And_Cloud_CSharp_Coding_Standards_v0.3.pdf` | 27744 | `7728a56bdb4c65c4a74cd02791293449dd2277d5a544f038744476c3c0b73c43` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/02_Technical/Cartridge_And_Cloud_Modelo_de_Datos_v0.3.pdf` | 201297 | `7909e47fb733c5129b9d5ead2743b9d82e75bb4a592f931cfaa61a419a671380` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/02_Technical/Cartridge_And_Cloud_TDD_v0.3_Technical_Design_Document.pdf` | 59929 | `de5a87992375dbef16f3f4ea2013262c31f08b184d074e8b952b8a124a6b8afb` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/02_Technical/Cartridge_And_Cloud_Unity_Project_Setup_Guide_v0.3.pdf` | 28287 | `c0725431f52d44b931bc58773ebd56fae72d00dca0ff456c41167a1a7705e537` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/03_Visual_Audio/Cartridge_And_Cloud_Art_Bible_v0.3.pdf` | 48581 | `12e33a6d179e22457d8f03c7c5cb48d597194b21989beae47ceb0c53ea079b47` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/03_Visual_Audio/Cartridge_And_Cloud_Audio_Bible_v0.3.pdf` | 24871 | `0932a2c018ffa74ea933999c973d56e378dde4cb73ca315bd9a95198c9742760` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/03_Visual_Audio/Cartridge_And_Cloud_UI_Style_Guide_v0.3.pdf` | 128476 | `1af3dcb8429dc562aa2d696f74ef1c27cb4100f5050e5041d3cdb8e8ba609b1d` |
| 0.3 | XLSX | `Documentation/00_Official_Baseline/v0.3/04_QA_Production/Cartridge_And_Cloud_Excel_Maestro_Produccion_v0.3.xlsx` | 28877 | `c33a49d4c392b679949daf895029e9eb769241084d49ce51acec79126b47379c` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/04_QA_Production/Cartridge_And_Cloud_Production_Roadmap_Sprint_Plan_v0.3.pdf` | 28949 | `2b48b54738598dbaa9b461d7cad6f4a6a2f70bb8589df55e72424ec7c9e40e6e` |
| 0.3 | XLSX | `Documentation/00_Official_Baseline/v0.3/04_QA_Production/Cartridge_And_Cloud_QA_Testing_Matrix_v0.3.xlsx` | 21821 | `f7d48e93b2ca3cc8e86b1dfc2164126d923af38ba0c76cb4dde566e6bc4da05d` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/04_QA_Production/Cartridge_And_Cloud_QA_Testing_Plan_v0.3.pdf` | 39300 | `2d71d4dd720598429744bc6cf959162a8915301ad69171bc6816168e47f75709` |
| 0.3 | XLSX | `Documentation/00_Official_Baseline/v0.3/04_QA_Production/Cartridge_And_Cloud_Trazabilidad_Control_Cambios_v0.3.xlsx` | 12755 | `cc7407a324fabe7e7b88fbc4b4209a2d8bc74f481b8c881715923a3ec104e2bd` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/05_Business_Release/Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.3.pdf` | 22534 | `e358622a237b57cdcd65fb380460d70eafa0ef1085b43bf5e8fb7dff3ee734f0` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/05_Business_Release/Cartridge_And_Cloud_Localization_Plan_v0.3.pdf` | 27103 | `461add7f8bf216439351ce4afe63b573de50e8bae3f9e270992917f616d82ad8` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/05_Business_Release/Cartridge_And_Cloud_Marketing_Plan_v0.3.pdf` | 33440 | `60f88b8fa2619a7c11c116b756e3a10e860b95ca90b04a0a6aaa5863d117cd1c` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/05_Business_Release/Cartridge_And_Cloud_Steam_Publishing_Plan_v0.3.pdf` | 23140 | `10ff2852e8621c210bb578f63181382665ccbedeba3d62bfdce7eb4cb4e53472` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/00_Cartridge_And_Cloud_Guide_v0.3.md` | 39503 | `84c400f2ea145d0c56ee277c892d46b9261f405a59a830edca258f2b0905d858` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Art_Bible_v0.3.md` | 13594 | `de2f2c02aef523f9e01bee94c071c44903425c32f63eb0c6b8786ee792397bb3` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Audio_Bible_v0.3.md` | 3927 | `8530be74a5a22c33b4000ae322d90249eeb7a4567ec123a402c7b5c76a7023c9` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Build_Versioning_Guide_v0.3.md` | 3678 | `fcc0988f428b45303c4cc2f5a681cbed9970475667bb6a37fa97a36529dc3705` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_CSharp_Coding_Standards_v0.3.md` | 4466 | `2a948ae7f681636e28761e91da7828ecbe44c15e5e0b02efa2cb8e0a6873278f` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Document_Migration_Plan_v0.1.md` | 3639 | `1ffeaf8d2c86d41ed3fba6e0d718639362a520cd285f3351113861ab4a1d7b6a` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Documentation_Validation_Report_v0.1.md` | 7445 | `d0b43ff719168cf7c2e4375cd828d9d86f0884cb1c8dcdde12a3250babe7934e` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Economy_Balance_Specification_v0.1.md` | 30444 | `a79f65cb5d960ff1e8fa6c83b8369b295547600f2f058163078f07a3b892c9e4` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Enfoque_v0.6_PDF_Source.md` | 124327 | `50f9151e64949a4255514d419c4f75524ddcecc4b1337c293db7d269f3aec052` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_GDD_v0.4_PCSteam.md` | 114534 | `8939cf748467b18cee2e351b5eac309fe108b7fe02cd6da59a96230e9dd125fe` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Initial_Content_Catalog_v0.1.md` | 23013 | `5b416cba7975a0ee42762eabb9ea236c30a19091fb3d3031285ac67ab0a83dcb` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.3.md` | 3972 | `ffc5bc6af65b27ede021acfdef2dbc5f20f680560e0be1a872586ab146a2f101` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Localization_Plan_v0.3.md` | 3806 | `f48dcd0d33e717b3e65734a9f87677d3b72edb9e67b54ff630720ecc46686349` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Marketing_Plan_v0.3.md` | 8241 | `1900805edf3fd6d0acd18cc95125905eae608adbf46cde0098cb49e82fb25fca` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Modelo_de_Datos_v0.3.md` | 71576 | `6d3fd4346cf55ecdf33d5aae3e1c1f267936e594d8902d3c51f3edb76003a46b` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Package_Manifest_v0.1.md` | 6551 | `58b7f326720277d2b490784a8091cff34f3182ac4fff57796e70d7817d9a4bc1` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Production_Roadmap_Sprint_Plan_v0.3.md` | 5738 | `93b345037c1356c7e176a7377bd66a168aa356366aa3c4e16062b09fcb46ee60` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Project_Binder_Indice_Maestro_v0.3.md` | 4609 | `77ee49ee91e1f527ddd567115d08b5b3c529712cada20c25e4e82510108e0c2d` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_QA_Testing_Plan_v0.3.md` | 9293 | `517a90d50868dd91010be4eaaca335326b63e8d370613c82a5b5664753ab3891` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Steam_Publishing_Plan_v0.3.md` | 3923 | `1a004be75991bdcbbaf457a57da123e3297fce15a38c591e3f589e11e277dbd5` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_TDD_v0.3_Technical_Design_Document.md` | 17342 | `86160690276d137118b119d6376eee8d1834097dbfe92fb6da7d18356d737eb2` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_UI_Style_Guide_v0.3.md` | 47855 | `d446fa1988af7343cacee8c3817afd3e3fd10f933fe1524d7e0b65f718842065` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_UX_Flow_v0.3.md` | 87463 | `abb29bbf9b9e9dfbef5161feb03ec07c7bc405cfe4b217e8843534279eb3b50c` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Unity_Project_Setup_Guide_v0.3.md` | 4564 | `0a6f42c71d9d22cee8aa12796842fd5391c3570f675f7361f1932418b74e1e55` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Vertical_Slice_Specification_v0.1.md` | 8682 | `69792ba4790022b23d631d437e043d777844f775b94d6552bc33bc682b876bb0` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/PACKAGE_MANIFEST.md` | 6098 | `7fb1bdbd0b11a372ccf4ad87efea6f88326188a21a8f4760680b0e49cd928783` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/00_Project_Governance/00_Cartridge_And_Cloud_Guide_v0.4.pdf` | 105014 | `07587b4bc74c59fb87a887454c679d0f670ab92180d7b7e437922d941a2a796a` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/00_Project_Governance/Cartridge_And_Cloud_Document_Migration_Plan_v0.1.pdf` | 38754 | `5c3f2fa8bfaae1476447296e17b470fa9b142b1229145ab348fef332766e3bf6` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/00_Project_Governance/Cartridge_And_Cloud_Documentation_Validation_Report_v0.2.pdf` | 32668 | `680c5a900685d90ebae0311b301c87a9f2a4d97ffca9e66ebeec8d2a36cf6bde` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/00_Project_Governance/Cartridge_And_Cloud_Enfoque_v0.6.pdf` | 248732 | `782c7edc39f04bb54e2bd168e4ad3ddca750ad7ff8af2c77b05f84ed8b4cbc9d` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/00_Project_Governance/Cartridge_And_Cloud_Package_Manifest_v0.2.pdf` | 36002 | `2fb3d34140d531bb28015e95ebed32e69d5e38f40fd2e591f99af99304035b5e` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/00_Project_Governance/Cartridge_And_Cloud_Project_Binder_Indice_Maestro_v0.4.pdf` | 32834 | `bc1362d541745d2e5935c1c0d3140dfc49e5a957830760ea242e655164c9661e` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/00_Project_Governance/Enfoque_v0.6.md` | 122708 | `6765207677c2adad0d124d20b9c27c82563eebc5eb3a0acb817dddbd148b3fbb` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/01_Design/Cartridge_And_Cloud_Economy_Balance_Specification_v0.1.pdf` | 90214 | `75b1684eaf50b546febc2608230524385fb5f7685be8d885c0cff309389af643` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/01_Design/Cartridge_And_Cloud_GDD_v0.4_PCSteam.pdf` | 233208 | `daf3d2376783c00533ce932e3da58dddd77b65fab1efa2aa7a141241dc125069` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/01_Design/Cartridge_And_Cloud_Initial_Content_Catalog_v0.1.pdf` | 81138 | `03be2e2f3c6f50ee0db68ff42a952b0b164cb480b6f4478a7ab02baf74c1fd29` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/01_Design/Cartridge_And_Cloud_UX_Flow_v0.3.pdf` | 191110 | `a93ec6bf330e929ad03bc9b046f78937a8b13cb4b7facfb6fb5b98b7f4b5e3f6` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/01_Design/Cartridge_And_Cloud_Vertical_Slice_Specification_v0.2.pdf` | 53623 | `d2505a3ba89c696248ddad9d0e3c4c7516eb0cf93b124111f2a4e3c6291bc46e` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/02_Technical/Cartridge_And_Cloud_Build_Versioning_Guide_v0.4.pdf` | 46424 | `b26821c7646a0648c9bca240688bc7e3f82edd0c5ff853ac115e88ee3a742c4c` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/02_Technical/Cartridge_And_Cloud_CSharp_Coding_Standards_v0.3.pdf` | 44429 | `996a4d5fe70a5a60c8e0194dfd167f527db03d9965cfdae83f6b8352447fec64` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/02_Technical/Cartridge_And_Cloud_Modelo_de_Datos_v0.3.pdf` | 167248 | `7b23bfd9335d0cca818896512b434579a5c94b20eac7185a6928e02e19175ab9` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/02_Technical/Cartridge_And_Cloud_TDD_v0.4_Technical_Design_Document.pdf` | 75576 | `b0f63b8bfcb72cb8b5a2671c6bdd3b51f8d79fffde681ae77be7539245f320ca` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/02_Technical/Cartridge_And_Cloud_Unity_Project_Setup_Guide_v0.4.pdf` | 50446 | `c12af29fd274e9ae5e297364a7c48f65b78de03445aa666fcd8556339a7fc8b3` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/03_Visual_Audio/Cartridge_And_Cloud_Art_Bible_v0.3.pdf` | 61240 | `e11742ff33fd1b846ba63c46c029c949fafd19681cf39f473bfcb9d300d2bbc4` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/03_Visual_Audio/Cartridge_And_Cloud_Audio_Bible_v0.3.pdf` | 42593 | `eb7f12085ef69502fe5e1b53dc930bf97e9616fcdbc1fd2725e0022ec08e1214` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/03_Visual_Audio/Cartridge_And_Cloud_UI_Style_Guide_v0.3.pdf` | 129348 | `738d1287f5d50a845095abe3fe39c9984f52c325f5a2bb2783b92e1648442417` |
| 0.4 | XLSX | `Documentation/00_Official_Baseline/v0.4/04_QA_Production/Cartridge_And_Cloud_Excel_Maestro_Produccion_v0.4.xlsx` | 23649 | `2debe763b682a8988c47fa2a6abf179123d625588365dc7fe2eb9e3c484f7a37` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/04_QA_Production/Cartridge_And_Cloud_Production_Roadmap_Sprint_Plan_v0.4.pdf` | 45968 | `a633ddefdface673e33411b00fd52a84d6984fecf5a49586352d2efb95ef4951` |
| 0.4 | XLSX | `Documentation/00_Official_Baseline/v0.4/04_QA_Production/Cartridge_And_Cloud_QA_Testing_Matrix_v0.4.xlsx` | 26049 | `80d79fa25df8c4dfaf0e91ff3f0e519ffcd283d802cf9e418e06fa88fa01d4d4` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/04_QA_Production/Cartridge_And_Cloud_QA_Testing_Plan_v0.4.pdf` | 59968 | `047a7bdd0a0b1ed6f9dec50aacc429bf1f18a2ba874fca0a28a270cc61bbfa45` |
| 0.4 | XLSX | `Documentation/00_Official_Baseline/v0.4/04_QA_Production/Cartridge_And_Cloud_Trazabilidad_Control_Cambios_v0.4.xlsx` | 26372 | `b85a395a547c35e420ac1932cd80b1ac0b591d379abb119fd13626dc915e8684` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/05_Business_Release/Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.3.pdf` | 42670 | `e4105813b235d24b75edf539eae505342f58f1f26a4b087e870dcbda9d63f558` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/05_Business_Release/Cartridge_And_Cloud_Localization_Plan_v0.3.pdf` | 43688 | `fc6cb711af7107fecd82d8963f6461a6d2f00941e08d93ea535c16c8ce917c17` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/05_Business_Release/Cartridge_And_Cloud_Marketing_Plan_v0.3.pdf` | 49493 | `e3f2d73b70e08702998abaf7eda9ccb1bf6b10c56f3b41188d02001a15e81a2c` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/05_Business_Release/Cartridge_And_Cloud_Steam_Publishing_Plan_v0.3.pdf` | 43916 | `b052a99f72f3b531b75b5950e5459fc6bd2a62c516be7dc1ad5e53d3a4684ec3` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/00_Cartridge_And_Cloud_Guide_v0.4.md` | 40622 | `d2648692ec3db14ec9f36979f4e7efd35260635913de372de6f95e707f473bbf` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Art_Bible_v0.3.md` | 13699 | `9d12adda06afbf7db50291337b88095e23005722431fdb4959ffb33afc1ad0c7` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Audio_Bible_v0.3.md` | 4032 | `9de246aa116d50e54ce46f23ea6e683d337ce3d58f06023af66c068fa6803f4a` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Build_Versioning_Guide_v0.4.md` | 4723 | `6ac0c7072b5ac4b1844d82daffe1ec5f241dba369145e5c62152e290b8309acd` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_CSharp_Coding_Standards_v0.3.md` | 4571 | `d49fa93c9948fe7c66d049193e866b80a8aa58dea240d666941ceca171e38565` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Document_Migration_Plan_v0.1.md` | 3744 | `686e0e814121e526e3f2505fa867e4be9b2dbfe91e80d9cbce12fa59db4de37f` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Documentation_Validation_Report_v0.2.md` | 2115 | `cc60a6477abb733f9abfc2fab159b0a22d30dc134abe5e20ef6bdd4546b9c732` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Economy_Balance_Specification_v0.1.md` | 30549 | `f83578295bab346c04186abd7d5ca64c006565617f38e71b8c229605b717fabc` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Enfoque_v0.6_PDF_Source.md` | 124432 | `8e6ba5102d28b0aa36d1306b531407587510850d2a7ca50691389d53ce938548` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_GDD_v0.4_PCSteam.md` | 114639 | `88be82c3d3cfa4cb5965379e621b0aba522efa1217041cdc23a129ec4817485a` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Initial_Content_Catalog_v0.1.md` | 23118 | `2bb04a31e3a2661e17e00f23ac0becc61b2c198a36fce87cf9f7aa1c76f07103` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.3.md` | 4077 | `1a0e921f139b73179fe734eec452dcc3aa8c43860309390fe41ea1fb588fe81f` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Localization_Plan_v0.3.md` | 3911 | `2158be2c46ec697e617419a9192bbaa8a0dd4435b5c650fca9295921b958cb3a` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Marketing_Plan_v0.3.md` | 8346 | `c38c0faa88c5d3625e8814d5ec02eebb39d321819624b8dd0539d13f364d26e7` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Modelo_de_Datos_v0.3.md` | 71681 | `3c722995fa9423f0de31cf9a45cf527574c5f77eb13f2788db0b84585baf8a0b` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Package_Manifest_v0.2.md` | 2456 | `ccf1ff8da3ecfb69e1ee93a902e1d13d706d5e7ada9ac4a185515c8f93cd2b7a` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Production_Roadmap_Sprint_Plan_v0.4.md` | 6571 | `e9976daa630e8bc1460831eea0c3d280d24f38d3907ef3dd97baec6e35661404` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Project_Binder_Indice_Maestro_v0.4.md` | 2334 | `02d4f9ad4e668399497cec77be435926531690b2e1636a0c288b15156b61c4fc` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_QA_Testing_Plan_v0.4.md` | 10116 | `623c83bd390d3fb1e7cb09e554f6a29f63db662c7ca3e296c65938c65969a724` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Steam_Publishing_Plan_v0.3.md` | 4028 | `56979c2830951e4a82a7d01793cebe9a930df4a773245de2b7489d0e73cb0777` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_TDD_v0.4_Technical_Design_Document.md` | 19073 | `bd2237fa0323821c4d4717d89940f005bbce2c2232ac91a4dd72f54301fb879d` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_UI_Style_Guide_v0.3.md` | 47960 | `8e0d241b40ecc46860cc6c51036f436d85ecfeca0cecfc721897b52396b7fca2` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_UX_Flow_v0.3.md` | 87568 | `2f7e2355cb0e994394e536f8de514ba499aca16a2a81b8e6be1867a025842a9d` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Unity_Project_Setup_Guide_v0.4.md` | 5564 | `0feb637a512b120a1515127467a5a9af0e97758c5a2f93627b6a4f3165b63cf8` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Vertical_Slice_Specification_v0.2.md` | 9317 | `884464e9599f0f1b66fdeff7a3c6422bf9d37b883817f480e0429ebdcd126b9d` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/PACKAGE_MANIFEST.md` | 2456 | `ccf1ff8da3ecfb69e1ee93a902e1d13d706d5e7ada9ac4a185515c8f93cd2b7a` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/00_Project_Governance/00_Cartridge_And_Cloud_Guide_v0.5.pdf` | 31429 | `b7236ef8d3e5212160686a57f0ddd3d67691a66b992491031c4eafddde3aebd0` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/00_Project_Governance/Cartridge_And_Cloud_Document_Migration_Plan_v0.2.pdf` | 18680 | `0036d616cf1983bfcd5e92aed7439766395266146a1eb035aa4220c518a821b1` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/00_Project_Governance/Cartridge_And_Cloud_Documentation_Validation_Report_v0.3.pdf` | 15684 | `0a4d869f4f3db6974159dfe60eee9c3831af32d33aed8b31b91b26f67c5550ea` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/00_Project_Governance/Cartridge_And_Cloud_Enfoque_v0.7.pdf` | 21083 | `e892bfaee0c3891ee54a2eec76de183dc00669ea47c776705ba72dda65586b5f` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/00_Project_Governance/Cartridge_And_Cloud_Package_Manifest_v0.3.pdf` | 16782 | `d74ae4fb1a2590813315e14d668ce2eca7a22081a7acdafbb5a55e07fb7a4f87` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/00_Project_Governance/Cartridge_And_Cloud_Project_Binder_Indice_Maestro_v0.5.pdf` | 21024 | `1f894f92311731b8ab15532eac377534739fe7d9e1e409fcecf64931787c6943` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/00_Project_Governance/Enfoque_v0.7.md` | 4112 | `dff2fd6d5ae6a688ae147c6eebc2e81b5667cf2433198204867ed51559cc11ea` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/01_Design/Cartridge_And_Cloud_Economy_Balance_Specification_v0.2.pdf` | 19491 | `57a71ee88a5a25dd9afe1a94d798a0d6522e1007c6dfb1d91aa98089a7cbcfc3` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/01_Design/Cartridge_And_Cloud_GDD_v0.5_PCSteam.pdf` | 23195 | `7061f5ee8528d2fdffc2c219ff8a54f997705fe38cf3a713856caf519a997264` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/01_Design/Cartridge_And_Cloud_Initial_Content_Catalog_v0.2.pdf` | 18125 | `9f604761d6e6047ddb34de9743087470bebcf4f8cdaeab327c1c7c3c98be1213` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/01_Design/Cartridge_And_Cloud_UX_Flow_v0.4.pdf` | 22040 | `75fdf25a47e7b022718047e09f2b1b5ca443c089e82e3d2a10a04e952441a690` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/01_Design/Cartridge_And_Cloud_Vertical_Slice_Specification_v0.3.pdf` | 18610 | `b256eb82b1c49fe2598797ec5ad74d4e4a22523ebbba62ef77ca27a777d231fc` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/02_Technical/Cartridge_And_Cloud_Build_Versioning_Guide_v0.5.pdf` | 18588 | `34fc32adf9c6383d0bf5b79ed446cd4dd00c9518cbd67c6e9d7cd5623b236347` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/02_Technical/Cartridge_And_Cloud_CSharp_Coding_Standards_v0.4.pdf` | 20856 | `dbb8bd5fd565a2c4078c26f56d1d1474daaa44ba98920fcf3bfc72f4a4081632` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/02_Technical/Cartridge_And_Cloud_Modelo_de_Datos_v0.4.pdf` | 23279 | `022bbed24274749a627b0547d51d9e6570dcf711194c747394a3979241c6314a` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/02_Technical/Cartridge_And_Cloud_TDD_v0.5_Technical_Design_Document.pdf` | 23003 | `87ca947c5d4fc56096449d54e59f4b3abea4daa028ce41a00cb3162d72284d24` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/02_Technical/Cartridge_And_Cloud_Unity_Project_Setup_Guide_v0.5.pdf` | 20165 | `5892ba151a6fce83012f252b41260b2bac73809cb27559ab4f3fcf03173e89b0` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/03_Visual_Audio/Cartridge_And_Cloud_Art_Bible_v0.4.pdf` | 17001 | `0a1b58c57c3e45c4a698f3c5a57227876a8e541e80b4e8d275b3f8585aa0c732` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/03_Visual_Audio/Cartridge_And_Cloud_Audio_Bible_v0.4.pdf` | 16228 | `a8991cac49c1999b4f34c42910f3fb7a4cb07c77b44fe2f54469bd2505147b0f` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/03_Visual_Audio/Cartridge_And_Cloud_UI_Style_Guide_v0.4.pdf` | 17718 | `e9fa1ac1d737bb8945eb76a22a7a593b1e7e453c8930be72037f18f1c2f8b4b1` |
| 0.5 | XLSX | `Documentation/00_Official_Baseline/v0.5/04_QA_Production/Cartridge_And_Cloud_Excel_Maestro_Produccion_v0.5.xlsx` | 11471 | `d6a252b5afeaa9a51a7ccd160d3e4f2c4f5dead46cd5a3677df095a669181d4c` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/04_QA_Production/Cartridge_And_Cloud_Production_Roadmap_Sprint_Plan_v0.5.pdf` | 21719 | `d75c34e0085a41c5a875fd17cf8fe3f7ea3f6b6ba6e496246a2cf93e06427b93` |
| 0.5 | XLSX | `Documentation/00_Official_Baseline/v0.5/04_QA_Production/Cartridge_And_Cloud_QA_Testing_Matrix_v0.5.xlsx` | 8327 | `dbff9b69bb8edae19c2158fb4293aaf574a584a407cca672cf6f89dabae6690e` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/04_QA_Production/Cartridge_And_Cloud_QA_Testing_Plan_v0.5.pdf` | 20203 | `0763d0d411746199eaa762acc995f591393eb39a1c8d88511c4dec488e56e74e` |
| 0.5 | XLSX | `Documentation/00_Official_Baseline/v0.5/04_QA_Production/Cartridge_And_Cloud_Trazabilidad_Control_Cambios_v0.5.xlsx` | 8674 | `da5c5afe865e1ae53df4c3453485263fe0dec512d824e2cefdd1d973566c6734` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/05_Business_Release/Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.4.pdf` | 15779 | `d8fa9f63a454facc7e8d4e3fa8e50d1a95f0bb7a8f8bef06211e9d12accb5343` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/05_Business_Release/Cartridge_And_Cloud_Localization_Plan_v0.4.pdf` | 15507 | `0194a14af0cb3121540d5ac73f3264c2d29037256953c15a74379690e1727e4c` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/05_Business_Release/Cartridge_And_Cloud_Marketing_Plan_v0.4.pdf` | 16171 | `edf4ba3e10869a15504bc315314bc9a500475ce249e9617835e9dfbf5d1e180b` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/05_Business_Release/Cartridge_And_Cloud_Steam_Publishing_Plan_v0.4.pdf` | 16309 | `f0382da1359aa1339758e067dc58c2d921c46ca37aef870c74ecee0ef320a3b2` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Handoff/CURRENT_PROJECT_HANDOFF.md` | 2275 | `9c251d0f3c14584f5da35a50ab3a5b62564e6f1f656d903fa2590eda74e2e396` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_01/QA/S1_Acceptance_Matrix.md` | 1455 | `a5a4e0a192fe33559c582efd9ee4d53170e26af9e07db63aae1e22b1f5c66f49` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_01/QA/S1_QA_Execution_Record.md` | 1065 | `f9ae3d8cb7bd112af8d518e93ccfe03fbce2309d555f3130b0325427ef68daad` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_02/Governance/ADR-0011_Minimal_Versioned_Save_Skeleton.md` | 2505 | `4048b071ec41b0d5caf4beb0d5a4ff7547e409f566a403d0e9331a604de076dd` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_02/QA/S2_Acceptance_Matrix.md` | 993 | `1c184fd5c44de6051e83614676a1571d4d49e7cd8b4d68c8ef85167b64e2692f` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_02/QA/S2_QA_Execution_Record.md` | 1827 | `70ad9d5ab6b32071b69fcbb35e7f76889b95b4d84ec6c6e6174ab2d793ccea4f` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_03/QA/S3.4_Build_Execution_Record.md` | 1303 | `bdbcbe899c627a7abf1f302f7db42ce29de1f0809c8373a454e3bc070984ab7d` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_03/QA/S3_Acceptance_Matrix.md` | 1643 | `dcd3a753ec03bf462ed61fee96f551716970f05a54702fcec5ebdc271bda15b7` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_03/QA/S3_QA_Execution_Record.md` | 2573 | `c36caf9cccc1d762ab9f201d7eaedf2e07ae88f1787d7ed0df6bf13305850116` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_04/QA/S4.4_Build_Execution_Record.md` | 1164 | `6f662c5fe7ccef7a3080584798ccf51c720d4ade1dc50b1be2f8c4c107da529d` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_04/QA/S4_Acceptance_Matrix.md` | 2209 | `30a86d51980351f806ef30aa6187ce90857bde474c163d47a6a3396b60522f89` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_04/QA/S4_QA_Execution_Record.md` | 2537 | `0c5ec51188fd94404436c7093e01ed0a0d0d75fc466da104e777fd7494aa0603` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_05/QA/S5.4_Build_Execution_Record.md` | 1347 | `83700192228af99fb4487bdf4cdd1332d13a194be7cd5fb197278013952f8fe1` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_05/QA/S5_Acceptance_Matrix.md` | 2498 | `3c265e9509b133370788c1ea024e4de481f773b526a524f2ec880d60abe053b6` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_05/QA/S5_QA_Execution_Record.md` | 2607 | `28e52cfc1189efc63cafabf899b0b94924bf2fb3edb92a43973797fcb77616d4` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/00_Cartridge_And_Cloud_Guide_v0.5.md` | 6916 | `1774286d76e6fef44bd4f38f054ed334ae6bf969fe9b477fc81d7392f9033e54` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Art_Bible_v0.4.md` | 2426 | `85241caca78cbac42e40a735b73a116ee47e5937605952cebb092272bb3a1979` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Audio_Bible_v0.4.md` | 1955 | `b63eff2e7197d01df8375a9d815ddf046d123a222fdaf70079a8356d58c9e092` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Build_Versioning_Guide_v0.5.md` | 2384 | `3df0fc5321a57c528f6386bcd922c2e930ba194736af94f064262c371bb63be6` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_CSharp_Coding_Standards_v0.4.md` | 2940 | `e83ba0009eda604345b2939a8cddf7f8d1a4b8b3ab2445bfcf79996889844e6d` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Document_Migration_Plan_v0.2.md` | 2508 | `125794b93ecfa0ba466066a84f7b543b8fbcf2407d577d2583211db9e0c09ae1` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Documentation_Validation_Report_v0.3.md` | 1944 | `f0580c1ab55321eefc1ff3f1f57ec2337ca1c313c1c98f24b19d2967f73cf010` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Economy_Balance_Specification_v0.2.md` | 2773 | `06c11a0e1ea6a93a2f3fd18794007ce2160fbbee1658d43456d4b16fecd8c484` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Enfoque_v0.7.md` | 4112 | `dff2fd6d5ae6a688ae147c6eebc2e81b5667cf2433198204867ed51559cc11ea` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_GDD_v0.5_PCSteam.md` | 4208 | `17284e1e8d67332d5a8e76e56d194b7e176225dfb49e2729caf6664a96c6aa09` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Initial_Content_Catalog_v0.2.md` | 2513 | `02439c64569896a24e292ec42717612412828e784bf7c1c701fbc477d57b0d51` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.4.md` | 2001 | `18f78b69a56edee6f8783d32859d6e231d061422d24abd6feefdbf06fe68567b` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Localization_Plan_v0.4.md` | 1892 | `01a3ddb286f975644985bc0c034eab75ef2ba4a283cd200f2e5dfd673732839f` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Marketing_Plan_v0.4.md` | 2049 | `dad726385bd8f67c059d6a77413a82accff668d5502367f7dee52b5dfbb2ac59` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Modelo_de_Datos_v0.4.md` | 3193 | `e657c304a8f9572abfd1e8240c23860b4feb42b9b314f6384d31993f2f5a01a3` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Package_Manifest_v0.3.md` | 1783 | `7ae705ca39b1e39345748c208af4274cbbe9eaf887a249e5666bceefadf9d0d2` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Production_Roadmap_Sprint_Plan_v0.5.md` | 3654 | `d0623a03b2ff75fa95ecd6bbaa5fcba0fa8faada31fc2a6b4a569caa50f0e285` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Project_Binder_Indice_Maestro_v0.5.md` | 2844 | `fa8494254188fa107542500dc097635c85cdc6b92152b41538cb61af7f7f4802` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_QA_Testing_Plan_v0.5.md` | 2853 | `c19ab67f26aae4b44fd5c6d431fb163ddbfccefcbcc0ca68518826040b534f1c` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Steam_Publishing_Plan_v0.4.md` | 2025 | `19664d4092ea89898c5bc37483a0b1aba6f473abbfa02c2794df8f2dd482058d` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_TDD_v0.5_Technical_Design_Document.md` | 4246 | `2067d16cac0aae694c69895371dca234c4daaa5258f13922844214283bd09e25` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_UI_Style_Guide_v0.4.md` | 2262 | `8b3346d21414eb370a5e98eb7871f5083cf7bc8d4214ab5cff7ae87ff0b46a22` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_UX_Flow_v0.4.md` | 3023 | `43143ab9e7504624ffdb4a2e9612332ee795f91ae05ea5d6448e5ce2f018bba8` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Unity_Project_Setup_Guide_v0.5.md` | 2491 | `cb4a9ea65fc1c8b95df59019c3bf6887cf220606980d221904494dfb057cf6ca` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Vertical_Slice_Specification_v0.3.md` | 2813 | `17213417be3b7b2b95f8e3841d09a6679eee689bb62a35335e3f318135c15e46` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/CURRENT_PROJECT_HANDOFF.md` | 2275 | `9c251d0f3c14584f5da35a50ab3a5b62564e6f1f656d903fa2590eda74e2e396` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/PACKAGE_MANIFEST.md` | 814 | `8bb540cf6f6eb6e6f079cf55786ab3b53f4ec042bb1dd01147edb03222f306c5` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/00_Project_Governance/00_Cartridge_And_Cloud_Guide_v0.6.pdf` | 53846 | `62da82c424e2544d77c40b8bcca4226fcd1173eaee00bed0834b1f031304b27f` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/00_Project_Governance/Cartridge_And_Cloud_Document_Migration_Plan_v0.3.pdf` | 36820 | `ec3fd62d6af2a6b049db86df7dfe1269aaec60f150b4010ea77f4c78513a6197` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/00_Project_Governance/Cartridge_And_Cloud_Documentation_Validation_Report_v0.4.pdf` | 31493 | `5a65390d74b595633ab09e1e3f050b09a219c76bd0cbcb22b3f1b92224536ced` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/00_Project_Governance/Cartridge_And_Cloud_Enfoque_v0.8.pdf` | 38515 | `ee80fd31e71e506c52780363b771af767b9d36ddcd4b645b3b1eb11e9d26945b` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/00_Project_Governance/Cartridge_And_Cloud_Package_Manifest_v0.4.pdf` | 31216 | `e2b87252fcc868ccf8c9bd6b7e9737260653ed2ae07d2eb86394cb503d64f48c` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/00_Project_Governance/Cartridge_And_Cloud_Project_Binder_Indice_Maestro_v0.6.pdf` | 40686 | `e2ae658017c7deb8055e886df6dd66707fbff8d1db4752d5b98c982e54631469` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/00_Project_Governance/Enfoque_v0.8.md` | 3665 | `7723b3a72d7b63ace8f1401c9eb2ed250a671e6a49e867f134583c4572ce524c` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/01_Design/Cartridge_And_Cloud_Economy_Balance_Specification_v0.3.pdf` | 37357 | `814b045831b60bd22488cbb9b255a87e9c714abb9f0c9716227004af6334b38b` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/01_Design/Cartridge_And_Cloud_GDD_v0.6_PCSteam.pdf` | 43453 | `f7002d60df83e5ed854264b4a802324de145e6389d3ec1de3b9f02ca4f619acd` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/01_Design/Cartridge_And_Cloud_Initial_Content_Catalog_v0.3.pdf` | 37702 | `9566db558196999b36eaff6c58b48adaf7cbbf005544cca3692ce2bbdfa216e8` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/01_Design/Cartridge_And_Cloud_UX_Flow_v0.5.pdf` | 42891 | `59f5922437a9c5cd0719b49c14beb50d5a7a74d5ee5d863ba2763153a7c378db` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/01_Design/Cartridge_And_Cloud_Vertical_Slice_Specification_v0.4.pdf` | 38854 | `326720d1a0d1302142034421279586f319992a9ecd3772c1922b6f5be085a692` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/02_Technical/Cartridge_And_Cloud_Build_Versioning_Guide_v0.6.pdf` | 37901 | `758372f65f3e817c8cc192d557cac69f2ecb2b22d0705e8feeac9e5034cf0a02` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/02_Technical/Cartridge_And_Cloud_CSharp_Coding_Standards_v0.5.pdf` | 38900 | `bcbd99d67deb5d0d1f6a3e90a8fb6058cbd4798b1b011ff6ad4e0980642383ad` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/02_Technical/Cartridge_And_Cloud_Modelo_de_Datos_v0.5.pdf` | 38808 | `296fbc062ddb5cb16dad4f5c98bccb1301c437eaeac7b58a23d51009d8f32fe3` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/02_Technical/Cartridge_And_Cloud_TDD_v0.6_Technical_Design_Document.pdf` | 44591 | `47cb69c540b382355f767b5cef1cbecc0f4ed29cf5711be34cefe196007b50d9` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/02_Technical/Cartridge_And_Cloud_Unity_Project_Setup_Guide_v0.6.pdf` | 40504 | `415fcd4c16c68e395c49179492c10db67374b3fb1c28257ec41987b2735d7ba8` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/03_Visual_Audio/Cartridge_And_Cloud_Art_Bible_v0.5.pdf` | 35406 | `ab41d9f663bddc58edd4d1591281b74097f0b3a4cd772706d923cf275222f414` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/03_Visual_Audio/Cartridge_And_Cloud_Audio_Bible_v0.5.pdf` | 31080 | `1266f4f2b51202caa9eccfec07800cd089001a454ce72e90a205aa093b21198f` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/03_Visual_Audio/Cartridge_And_Cloud_UI_Style_Guide_v0.5.pdf` | 31466 | `a1898f385154ecfbb12b0106ed150e6ac0307d2987650fe8d2397c0e428c5da7` |
| 0.6 | XLSX | `Documentation/00_Official_Baseline/v0.6/04_QA_Production/Cartridge_And_Cloud_Excel_Maestro_Produccion_v0.6.xlsx` | 12166 | `90e3ccee5e392f8471b66090d99c97dcff596256cd7a65f3b1919376f346eb6a` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/04_QA_Production/Cartridge_And_Cloud_Production_Roadmap_Sprint_Plan_v0.6.pdf` | 37116 | `5e1a0fea9e45f7af252162baba7544778134216c0f9c839aaabdd48687b3c370` |
| 0.6 | XLSX | `Documentation/00_Official_Baseline/v0.6/04_QA_Production/Cartridge_And_Cloud_QA_Testing_Matrix_v0.6.xlsx` | 8110 | `96a6eb8272a2c9a42f718f71406a1b8d770e2d7cf0d96d2c3d0a0c418445d681` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/04_QA_Production/Cartridge_And_Cloud_QA_Testing_Plan_v0.6.pdf` | 38292 | `ff451a919545374e6bd22a44efd7f8e65d3e9001741a2ddcac58b4f339052c04` |
| 0.6 | XLSX | `Documentation/00_Official_Baseline/v0.6/04_QA_Production/Cartridge_And_Cloud_Trazabilidad_Control_Cambios_v0.6.xlsx` | 6699 | `fb8eb9ca993738f590fb5978de9f4eea8224730540cb3af9d2324cd96b646e32` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/05_Business_Release/Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.5.pdf` | 28754 | `eee199263d089442dc5ea6228b10b562d7930163c707a7e90402e734bf94db51` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/05_Business_Release/Cartridge_And_Cloud_Localization_Plan_v0.5.pdf` | 28785 | `0a7e40cfb0a39f46a303f93fd89ea65073e507955c15d89a67639ae1f445aaf8` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/05_Business_Release/Cartridge_And_Cloud_Marketing_Plan_v0.5.pdf` | 28482 | `ee3aba1109fbbde0c6773080e32064694f3eaffd20d557fa941cab855120a9e6` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/05_Business_Release/Cartridge_And_Cloud_Steam_Publishing_Plan_v0.5.pdf` | 28734 | `a21ea9abe67f1d825731d25b8608c8f7b67e5e1b437356e0e1ee3d97870d7728` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Handoff/CURRENT_PROJECT_HANDOFF.md` | 1241 | `7a297d84a36ecdd9e4296a631fc5acbff882218d125e63312850e8f28b46cd87` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Handoff/NEXT_CHAT_START_HERE.md` | 896 | `e0cca56b839dc2071363b310c06543178dbf978622990fc7f4be301e077fc754` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Historical/Sprint_16_Integration_Timeline.md` | 1783 | `89fde821c460328fbb8700022883006d2eb8a1ab892c266972b7a1cc207cbcc0` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Sprints/Sprint_16/Documentation/StoreInitial_Authoring_Plan.md` | 741 | `f67d195bcda2bd1f861dcf5c53d0a299260c1f04c2f299250856a76616131eba` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Sprints/Sprint_16/Governance/Sprint_16_Current_Status.md` | 665 | `bc336c8bf852d942d1b0289cebfdde9db889088990fcc6151df5db559e94d1a9` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Sprints/Sprint_16/Governance/Sprint_16_Representative_Asset_Integration_Record.md` | 554 | `91133cf2823d732d58fcd23601d6816e610e00375cc38e39737929d88fc4ec54` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Sprints/Sprint_16/QA/S16_Acceptance_Matrix.md` | 572 | `4ad529aceeaf7e2ea87def0e0be2f8f3998d6218f6ac11a06b40f770e66c44fd` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Sprints/Sprint_16/QA/S16_QA_Execution_Record.md` | 328 | `40c6de3168db5dcf5c71afc166c0a3a2bf34f2dc3e71344c3ccea6176d573766` |
| 0.6 | CSV | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Sprints/Sprint_16/Traceability/S16_Traceability_Entries.csv` | 460 | `3263ee5adec14e81588139e9c291ae973115ac75bef79233e6a514ac3643fd67` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Sprints/Sprint_17/Governance/Sprint_17_Opening_Brief.md` | 239 | `fb2c1a557e3b6cd765497315fa554eacc048c2fd21a2be9c7f5f28895a189ac1` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/00_Cartridge_And_Cloud_Guide_v0.6.md` | 8360 | `ea57886a818c892391b8bc669d5248c91d83500526d31d38550ad3da92552428` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Art_Bible_v0.5.md` | 2501 | `27d4fe8de7bd166329880e6975b1bf90883cd92ca7133b667f1036b09816ca42` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Audio_Bible_v0.5.md` | 2018 | `3edc3d0db88a36ac7726381883bbf4ffb5f5260a6cb5d972ecf659fbd5116aed` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Build_Versioning_Guide_v0.6.md` | 2462 | `8cd0f62e233cfbad2bed3a07122696e2ea81b4fd5280987cffb9a8a2a1e98d43` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_CSharp_Coding_Standards_v0.5.md` | 2690 | `99f4cf1616d3c5e3d825ce713c6996928a09fa5c28c43802c4104336a28bfe57` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Document_Migration_Plan_v0.3.md` | 2740 | `e72d9cd5b5f41609df36f157d26025eb93cb707e433341a939a4b5201cb725c8` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Documentation_Validation_Report_v0.4.md` | 2021 | `a08d4ff750799f8b8d4cf0bd9412f4e6c5c7258055d089a896d0abaf8bf52d69` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Economy_Balance_Specification_v0.3.md` | 2564 | `0f3a938d2dcfb83e03b20cc8b7902342d8124f42965cb9585c81a8854ce16abc` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Enfoque_v0.8.md` | 3665 | `7723b3a72d7b63ace8f1401c9eb2ed250a671e6a49e867f134583c4572ce524c` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_GDD_v0.6_PCSteam.md` | 4005 | `333c7777584bcdee29c1caa8f90f4e43e3013e88cc339466a1a51f5bdd42c0d7` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Initial_Content_Catalog_v0.3.md` | 2784 | `68f8841d7f5e83b1078bbf8c110e799165b74f5d75a823839d0acfd3086b333e` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.5.md` | 1951 | `610fc2ca1dd8ea1859fb779039e5de841610646d1e5601d316b2fe34410c3c81` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Localization_Plan_v0.5.md` | 1891 | `e8c63ec8a67f5d87f5dcec078c94ac08620b27a064a5a6866a20fa78de1becf9` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Marketing_Plan_v0.5.md` | 1864 | `96b3223429d38a0c83f4c89f07b63e3c258a72cbc76ada7c5e8e5991b62ab1ea` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Modelo_de_Datos_v0.5.md` | 3168 | `409201b990309744b05a65a3306034e605e7254d10a5d4232b707cdbc8779b4a` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Package_Manifest_v0.4.md` | 1910 | `89ae5a7d733d8d70f36b64cfe3653aed99149827155fcddec0cadf650fd38acb` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Production_Roadmap_Sprint_Plan_v0.6.md` | 3191 | `f7b50c87cbc0faa2478f677f868e2e29dd1e74abfe74c1cbb30aed74ab7ac70a` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Project_Binder_Indice_Maestro_v0.6.md` | 3763 | `04094957ed2328e54337bc8fc2d0822abb440a4602ee9d4055d358d0c165669f` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_QA_Testing_Plan_v0.6.md` | 2637 | `7659a777a9aa78f083d8a6d577898bec89430cae108b079ee7137c769da7f069` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Steam_Publishing_Plan_v0.5.md` | 1866 | `f7840ee791e07430dd750500421dc6e262d21d5119e634634919247ff13cf8d0` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_TDD_v0.6_Technical_Design_Document.md` | 4268 | `daa5578e670e61ffe40db754ef512d02885d1bb7600c74487194ad8ec27cb2ce` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_UI_Style_Guide_v0.5.md` | 2181 | `857f070207a752796ff5c21968020d3230a8c357d84a140593c74c47c59ee645` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_UX_Flow_v0.5.md` | 2707 | `5c755541bbc69510af12839f8435e090972edc7b7bb484139683614f69fcb85c` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Unity_Project_Setup_Guide_v0.6.md` | 2805 | `e0ba621f7cbd548da5ffa6484a1c2a0c91b820f845e5d410e318f01c34547497` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Vertical_Slice_Specification_v0.4.md` | 3027 | `efff0d3056515ad3e71a9c751f7ec86ce932d3098fb5870b246c04559695a3f5` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/CURRENT_PROJECT_HANDOFF.md` | 1241 | `7a297d84a36ecdd9e4296a631fc5acbff882218d125e63312850e8f28b46cd87` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/PACKAGE_MANIFEST.md` | 876 | `c25b2f638ce7814f8f2d9a153481efe6891a01b7e873f1a3251775ee51b922dd` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/00_Visual_Targets/ChatGPT Image 29 jun 2026, 21_37_54.png` | 1977934 | `cfdd490b742396ac2fff6ba0b1e6cea8d71fb2005305ddd9c82487ec7ea88b84` |
| working/current records | FILE | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/01_Architecture/.gitkeep` | 0 | `e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/02_Furniture/ChatGPT Image 29 jun 2026, 21_38_14 (10).png` | 1767735 | `42175ffa89a40d6bfb57217b38a5dfee04e337bc44b838ca8db01812e56454e5` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/02_Furniture/ChatGPT Image 29 jun 2026, 21_38_14 (7).png` | 1670316 | `6d5032c38dc50979430763f7ee18a84deee243325d3400affc2012551864ee1a` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/02_Furniture/ChatGPT Image 29 jun 2026, 21_38_14 (8).png` | 1996425 | `78f6c5d8ecc65077ee7191715e71f31b15a7ce22b59becf54e69dcca19bded48` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/02_Furniture/ChatGPT Image 29 jun 2026, 21_38_14 (9).png` | 1575644 | `16f940c84ce6c14a277a5ed236d1306df17d80419e118507f0f2032f0b4ad558` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/03_Products/ChatGPT Image 29 jun 2026, 21_38_14 (1).png` | 1590353 | `410d0bf38ab8848df5a496e250786d793d1d45bf8bbeab48c8f8ca83fa91d8fd` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/03_Products/ChatGPT Image 29 jun 2026, 21_38_14 (2).png` | 1786999 | `6107ce0119514adab534d271d224c0a3b9b2006eebe551cc43297299a9edc404` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/03_Products/ChatGPT Image 29 jun 2026, 21_38_14 (3).png` | 1954907 | `27639fb594bb9c24fc20403cbcf15a683b91af6b12250d3062fb2d0ec054528c` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/03_Products/ChatGPT Image 29 jun 2026, 21_38_14 (4).png` | 1939499 | `7ade04828ed3b7623c7e7dc271adde637cdcbf746c6a78566a3faa3cb5073e63` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/03_Products/ChatGPT Image 29 jun 2026, 21_38_14 (5).png` | 1901472 | `48be40e282d818092991e3f24d9e6b21e324025374a15ae762faf4fca8762779` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/03_Products/ChatGPT Image 29 jun 2026, 21_38_14 (6).png` | 1774013 | `56891223f619f891a523c01037823ca04fa386cc2b0963858c6115e9984df4bd` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/04_Characters/ChatGPT Image 29 jun 2026, 21_38_50 (1).png` | 1728315 | `fd439e31144c41f1eedd8d417911c1f78fbdbe02623b4859535db50082e8a491` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/04_Characters/ChatGPT Image 29 jun 2026, 21_38_50 (2).png` | 1685372 | `e5bca43f10a16a56d793260cf1bad77fa308db981207d5e6d37325159e6d8a11` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/04_Characters/ChatGPT Image 29 jun 2026, 21_38_50 (3).png` | 1651401 | `093b91e470d56a2609622789d9955d99f700014a6e9733fb4dc32da839d6d7e2` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/04_Characters/ChatGPT Image 29 jun 2026, 21_38_50 (4).png` | 1521051 | `546539231dbaf0ac0f0d3bd92bd4349e7ea7c13bee7dd1aa0768363529198c07` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/04_Characters/ChatGPT Image 29 jun 2026, 21_38_50 (5).png` | 1854890 | `1d02083cf9c0736568b4fbefea98d57e1104d84e1def585b1c8bf87fec8462d1` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/04_Characters/ChatGPT Image 29 jun 2026, 21_38_50 (6).png` | 1675117 | `6f0416232df549d46156a2f6ff92d039b785cf7aa44f82f17c840d05bc1927a6` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/04_Characters/ChatGPT Image 29 jun 2026, 21_38_50 (7).png` | 1684465 | `ebb549522b8d6ac4ca57d47c8bf1d8cd0e96608824dfb5f0f2d7bd51b6c2dd98` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/04_Characters/ChatGPT Image 29 jun 2026, 21_38_50 (8).png` | 1902077 | `3f1d37c6c690d4173bc0067a6460e29bc5bce2e8911c965d6a7b8aae05d296cf` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/05_Expansion_Concepts/ChatGPT Image 29 jun 2026, 21_39_17 (1).png` | 2230307 | `ce23b3b1e2bf6186ab627de7247b794ea9ed6f8b80bd8d4cb02df94619801f09` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/05_Expansion_Concepts/ChatGPT Image 29 jun 2026, 21_39_17 (2).png` | 2385231 | `79e9ccc8ba8126f7a63b5f38fdc4bafcbab5479b39e6882de1add15f66529e7f` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/05_Expansion_Concepts/ChatGPT Image 29 jun 2026, 21_39_17 (3).png` | 2195489 | `78cb05222f8155d5bd9e709e8e214f4517023444ed79538c3693bb0b09ea232f` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/05_Expansion_Concepts/ChatGPT Image 29 jun 2026, 21_39_17 (4).png` | 2065962 | `1074b34d1051c2e32fa23325f042158d04e7497b54d61c44f4cc1c3b4505b1f5` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/05_Expansion_Concepts/ChatGPT Image 29 jun 2026, 21_39_17 (5).png` | 2198732 | `686585e4a4b1054b23cdea08297e28ddd0df211d966dc0ab8a6116ce9c17ca25` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/05_Expansion_Concepts/ChatGPT Image 29 jun 2026, 21_39_17 (6).png` | 2232046 | `7c349ed7b9258e832c0e7c3f3529fb4fe8ea322f9d7c52ef004d98a741b21519` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/05_Expansion_Concepts/ChatGPT Image 29 jun 2026, 21_39_17 (7).png` | 2038776 | `cf9a87944e4aaff1730c4162d300a6204d248f4e6a62f8c0fe46ee7152819ff5` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/05_Expansion_Concepts/ChatGPT Image 29 jun 2026, 21_39_17 (8).png` | 1962162 | `cb6d28e87a016aa78cb5b6d1d84dbe42368fadc7b50f3ef8345214bf1e6a59dd` |
| working/current records | MD | `Documentation/10_Development_Records/Handoff/CURRENT_PROJECT_HANDOFF.md` | 1168 | `ea48666ee43dbf978bef364ee645cd3ff32f31e4e6dfbd96b9edfab316546028` |
| working/current records | XLSX | `Documentation/10_Development_Records/Production_Tracking/Cartridge_And_Cloud_Production_Tracking_Operational_v0.3.9.xlsx` | 23710 | `7d9a25ae4fd823d8bc8e2f23ba37604cd30a92eb8a464f8bd07eedcffa88012e` |
| working/current records | XLSX | `Documentation/10_Development_Records/Production_Tracking/Cartridge_And_Cloud_Production_Tracking_Operational_v0.5.0.xlsx` | 11471 | `d6a252b5afeaa9a51a7ccd160d3e4f2c4f5dead46cd5a3677df095a669181d4c` |
| working/current records | XLSX | `Documentation/10_Development_Records/Production_Tracking/Cartridge_And_Cloud_QA_Operational_v0.3.9.xlsx` | 26038 | `80084cd5532d9f79a59f03210f1cf9ce3b828cd13b58cc80678b5887a5b06a07` |
| working/current records | XLSX | `Documentation/10_Development_Records/Production_Tracking/Cartridge_And_Cloud_QA_Operational_v0.5.0.xlsx` | 8327 | `dbff9b69bb8edae19c2158fb4293aaf574a584a407cca672cf6f89dabae6690e` |
| working/current records | XLSX | `Documentation/10_Development_Records/Production_Tracking/Cartridge_And_Cloud_Traceability_Operational_v0.3.9.xlsx` | 26270 | `ad72213e64e596c8d3540e8d20d84c75d0a1e7399efc5e0f1d09b4ca5af55ca4` |
| working/current records | XLSX | `Documentation/10_Development_Records/Production_Tracking/Cartridge_And_Cloud_Traceability_Operational_v0.5.0.xlsx` | 8674 | `da5c5afe865e1ae53df4c3453485263fe0dec512d824e2cefdd1d973566c6734` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_01/QA/S1_Acceptance_Matrix.md` | 1455 | `a5a4e0a192fe33559c582efd9ee4d53170e26af9e07db63aae1e22b1f5c66f49` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_01/QA/S1_QA_Execution_Record.md` | 1065 | `f9ae3d8cb7bd112af8d518e93ccfe03fbce2309d555f3130b0325427ef68daad` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_02/Governance/ADR-0011_Minimal_Versioned_Save_Skeleton.md` | 2505 | `4048b071ec41b0d5caf4beb0d5a4ff7547e409f566a403d0e9331a604de076dd` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_02/QA/S2_Acceptance_Matrix.md` | 993 | `1c184fd5c44de6051e83614676a1571d4d49e7cd8b4d68c8ef85167b64e2692f` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_02/QA/S2_QA_Execution_Record.md` | 1827 | `70ad9d5ab6b32071b69fcbb35e7f76889b95b4d84ec6c6e6174ab2d793ccea4f` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_03/QA/S3.4_Build_Execution_Record.md` | 1303 | `bdbcbe899c627a7abf1f302f7db42ce29de1f0809c8373a454e3bc070984ab7d` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_03/QA/S3_Acceptance_Matrix.md` | 1643 | `dcd3a753ec03bf462ed61fee96f551716970f05a54702fcec5ebdc271bda15b7` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_03/QA/S3_QA_Execution_Record.md` | 2573 | `c36caf9cccc1d762ab9f201d7eaedf2e07ae88f1787d7ed0df6bf13305850116` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_04/QA/S4.4_Build_Execution_Record.md` | 1164 | `6f662c5fe7ccef7a3080584798ccf51c720d4ade1dc50b1be2f8c4c107da529d` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_04/QA/S4_Acceptance_Matrix.md` | 2209 | `30a86d51980351f806ef30aa6187ce90857bde474c163d47a6a3396b60522f89` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_04/QA/S4_QA_Execution_Record.md` | 2537 | `0c5ec51188fd94404436c7093e01ed0a0d0d75fc466da104e777fd7494aa0603` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_05/QA/S5.4_Build_Execution_Record.md` | 1347 | `83700192228af99fb4487bdf4cdd1332d13a194be7cd5fb197278013952f8fe1` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_05/QA/S5_Acceptance_Matrix.md` | 2498 | `3c265e9509b133370788c1ea024e4de481f773b526a524f2ec880d60abe053b6` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_05/QA/S5_QA_Execution_Record.md` | 2607 | `28e52cfc1189efc63cafabf899b0b94924bf2fb3edb92a43973797fcb77616d4` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_06/Governance/ADR-0026_Sprint_06_Version_And_Persistence_Boundary.md` | 587 | `88e809b7e5a461f57c5d9f943adaf6eeb0445f62ba3cd08440f5d2e45c48ed1f` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_06/QA/S6_Acceptance_Matrix.md` | 3014 | `b55fe039c6404ff22749ff07f20dc741ee277ece3a892d9d1a6a526eb61dab8e` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_06/QA/S6_QA_Execution_Record.md` | 2097 | `a7a14df3c2dc18a98c1354176d48502bafbd3309a651ea8bff3846db4c348e7c` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_07/QA/S7_Acceptance_Matrix.md` | 3351 | `e5517ab2f2b047aacb634c1ee7e42ea6f4b1b0f3bb6477cc7a0fb595439546b2` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_07/QA/S7_QA_Execution_Record.md` | 2132 | `ed5696f59956c5341e66f5d4bd7c9ff10b1c2ae56c47f0e70f7acdf412d62626` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_08/Governance/S8_Handoff_Update_Template.md` | 1297 | `752c345e77607a3d096548520f2ce082a816d2eeb0d01ea0d29ddbe2bd32013a` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_08/QA/S8_Acceptance_Matrix.md` | 1833 | `514b68f2839e76b1757df9d2d457dca9701cbe8f3de4169c6e38fe7e7c1e576c` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_08/QA/S8_Build_Execution_Record.md` | 1531 | `975e35974c3ac193dedc354ad39f652dccd31356a4d69ec241dddcfdd84f29c1` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_08/QA/S8_Compilation_Incident_001_NUnit_Assert_Multiple.md` | 1971 | `c3f9557dbe19d9326df55919c56712a4c7b57a77e3d1044708cbe974705cb69c` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_08/QA/S8_QA_Execution_Record.md` | 1991 | `6b648c6839cb4cd0f249f4318624210896b63a393cd888a3b1ac2d9f70edc61b` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_09/Governance/S9_Handoff_Update_Template.md` | 501 | `e3d501b14918d549aab4d42cc2d289a8f1d83730600d1083109ea64ea409baae` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_09/QA/S9_Acceptance_Matrix.md` | 727 | `476e8342efea9323b73b46337890b795fe2b3bc500486fd08a9e9f5dbf6706ef` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_09/QA/S9_Build_Execution_Record.md` | 773 | `743edb2e5421984273fb169aab3fbb16c48d6b0ac2e177c5e83ba01b51099304` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_09/QA/S9_QA_Execution_Record.md` | 661 | `ee2c2033cb369301abe3eb39bf1f0d3a71c241431b1889651e14a895437d114a` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_10/Governance/S10_Handoff_Update_Template.md` | 429 | `e7893c392feb7b35d3e270034b36f9da758d73673adb7e3f50cecea5763003a0` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_10/QA/S10_Acceptance_Matrix.md` | 665 | `8a3558314d7d868e3e2ce0dc33fe62e7b1b33eddf1e70b2d6eab43c9b3600d8c` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_10/QA/S10_Build_Execution_Record.md` | 535 | `adea9b7fcdab6e319f0ded3171260cc0200e2ae3ab424916a8068a7175872369` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_10/QA/S10_QA_Execution_Record.md` | 544 | `c45fb3dd29d4ec4dc67ad42f496932c94cbe384096c560c08d3d1c9615c6a5fe` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_11/Governance/S11_Handoff_Update_Template.md` | 422 | `3da63a8600ab4da7f1afe4032690763029d3551297fad0d51afbfa841687dad5` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_11/QA/S11_Acceptance_Matrix.md` | 650 | `a6f876fb02398be1ee466d2f9dde9613ad9c0fa2fc0088778b791ab9a3d00e8d` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_11/QA/S11_Build_Execution_Record.md` | 494 | `6e5b3ee3b6b9a1a6d2f6c90fc07df567cfdd81126f6e2c06aa4c3307db206f24` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_11/QA/S11_QA_Execution_Record.md` | 457 | `e09346a52a005447f98244392575dce8bfe0e1c24395d87ef4efa9949582222d` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_12/Governance/S12_Handoff_Update_Template.md` | 361 | `683e99d27f7af84f319fad7c98230f6dec7f9b9eeddf8b4066f750b22c230bd0` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_12/QA/S12_Acceptance_Matrix.md` | 661 | `9b16fac6f71a8d05203d140a77a1b0c202a5f445556cc663208b04e5c45392b9` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_12/QA/S12_Build_Execution_Record.md` | 431 | `e3531d9bc28665f33ed38c176188246308d38bd480be42c1758f848e433f69b4` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_12/QA/S12_QA_Execution_Record.md` | 402 | `e53fa611bf41782ecb96b8d41010b2607f3e38223dad09898a92bdfec0f6ba9f` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_13/Governance/S13_Handoff_Update_Template.md` | 431 | `7d91ce81d3567cd4f392f6d9784c7415d628eec2b65e7468ea5e1b694af743f8` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_13/QA/S13_Acceptance_Matrix.md` | 693 | `e7f134005949b363e84fadb28d740ff268adb3c42de47070fbcc968862319f2b` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_13/QA/S13_Build_Execution_Record.md` | 493 | `887232c7346268536f197fc6069ad7f518735c2f51c302f0d27e6c2625cd005b` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_13/QA/S13_QA_Execution_Record.md` | 456 | `a0ecdfbd34ae6a3e96f2ff4b19602fcfdad3628da0f4647a96631aecce9bd9e0` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_14/Architecture/ADR-0056-CompatibleIntegratedSaveV2.md` | 198 | `5cc539efb691df4398f23002613d08079fa8869bde84816f89a91be781ffcf6a` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_14/Architecture/ADR-0059-BackupFirstRecovery.md` | 209 | `22af6a1df416b4c7fb8389f08ec428c61289bbb5984d958e0576aa2478ad03d4` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_14/Governance/S14_Handoff_Update_Template.md` | 437 | `70ac9acc5473a29f44723f4e1499f027f5adbc772ae45635ebdb035f2374bd4f` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_14/QA/S14_Acceptance_Matrix.md` | 761 | `6abc7f9f6b1f199f797ede8e4610be0819c89c52ba763b09489f76e5b6d2b3e9` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_14/QA/S14_Build_Execution_Record.md` | 490 | `758b82a0301e5f3de699b76365cf74c577377bcb6d88333c4b65a26e6065af33` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_14/QA/S14_QA_Execution_Record.md` | 407 | `61083207815690667077272760527c8529f70916831a1bd7d75cc58125663a7f` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_15/Architecture/ADR-0063-ClosedDayAutosaveIdempotency.md` | 200 | `8575a38534155149713d4c711e65216ce43402c06dce60d181fbad87d806ad59` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_15/QA/S15_Acceptance_Matrix.md` | 744 | `ef5c3d5a5d788d7a52965e858620ca76d260711f96e5132bb7938c765b6762da` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_15/QA/S15_Build_Execution_Record.md` | 359 | `cb74c7605f9918b31b4f49ebbe0f63004b03886d5468bd258a3eb6f83ea9b61a` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_15/QA/S15_QA_Execution_Record.md` | 373 | `317dd72193364df1dd104ef8be879fa22dbd4f8374e3e0bd454fef8f0881277d` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Governance/Sprint_16_Two_Phase_Charter.md` | 640 | `b191b8b5d47654141df7860d6a79430552cc68e1bcdc786d1d4fef55583dc605` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Architecture/ADR-0067-Sprint16TwoPhaseDelivery.md` | 214 | `59f759e966b057f091473d52cf205d1a089a0917d2f8fff80342ca8ce2ca0a75` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Architecture/ADR-0068-PurePhase1Sidecar.md` | 232 | `5a4b767d6daf552a48dc2b4d256b38c698da130b41314243902c4b5fbab68459` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Architecture/ADR-0069-AutosaveCoordinatedCheckpoint.md` | 250 | `c4aaad9ce98b604d52894350668f7e59cc8c9d893ec3e96658a467bff0dbd44a` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Architecture/ADR-0070-PlacementCompatibilityBridge.md` | 353 | `66a5b5172267c6984edd5474ba03dc2fb0da28de72c310ced74feba060f6df17` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Architecture/ADR-0071-DecoupledPresentationFallbacks.md` | 223 | `f2e025be2f68b6de2388bd63a6b25549d4e6d111012bc685b034fa120ff9616c` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Architecture/ADR-0072-SharedMaterialAndPrefabIds.md` | 223 | `f32e2b4b274522d03df3c1c08084d86114a19dbe15fa2b3008c217943e2ff11a` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Architecture/ADR-0073-CompletedCheckoutSnapshotRecords.md` | 377 | `7d401e745e557c63167dae48cd0f4b05b1d08f5c58a2e652b4321925fc35caa7` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Documentation/S16_P1_Implementation_Record.md` | 819 | `3538c025e899624c8dfb10c740bcbc06387f5be41a78a3396a8f5937a5cfc147` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Documentation/S16_P1_PreImplementation_Record.md` | 496 | `0d6915434ed6282040553f42f849399d6a8e3380eb4e38a6082ea3d13fba0e9c` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Governance/S16_P1_Charter.md` | 874 | `4b5190df961a1180ce9fed6ced5fe194e88998f5f65df176bea9104c7bc825dc` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Governance/S16_P1_Current_Status.md` | 569 | `6494de60296d252da3368dab5a5850938c4a5c219d414f80bde328379f1c7f3b` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/QA/S16_P1_Acceptance_Matrix.md` | 1130 | `d6d2ca42287be0fced9c3d9da09df5db9729442157035a9a711e0ddbb9d91337` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/QA/S16_P1_QA_Execution_Record.md` | 443 | `a044ba4e7ebb4af622c74397e2a8f0ff6e59ad7e9c1bf89ddb62f5b06bcacec7` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/QA/S16_P1_Test_Plan.md` | 836 | `ff35438f414f45a152e04532664e348d0fed9bae380c9dc70bf520af5c7c04c8` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/QA/S16_P1_Validation_Checklist.md` | 646 | `ac85fc7fe9c1e905ca5ea2eb245aa016acf8c611b2a3fe1a10fb3d0740e0198b` |
| working/current records | CSV | `Documentation/10_Development_Records/Sprint_16/Phase_1/Traceability/S16_P1_Asset_Inventory.csv` | 6002 | `c91f4676f6c8b07c075fda4bf3563530380a5e35770d9f68b5497b77cf99641b` |
| working/current records | CSV | `Documentation/10_Development_Records/Sprint_16/Phase_1/Traceability/S16_P1_Traceability.csv` | 1635 | `d1e5ada71ed4d81b6a067aa05ea35626d5156967a1f8b212da829be32a4d318d` |

# Anexo K. Definition of Ready / Done

| Sistema | Ready | Done | Estado actual |
|---|---|---|---|
| Privacy baseline | data inventory, roles, owner | RAT, policy and tests | PARTIAL |
| Support | provider, notice, retention, access | request/delete drill | NOT READY |
| Website | host, content, cookie design | scan and policy match | NOT OPEN |
| Steam data | AppID/APIs/permissions | data map and role review | NOT OPEN |
| Steam Cloud | paths, conflict model, legal review | two-device/delete campaign | NOT OPEN |
| Crash reporting | provider and minimized schema | redaction/retention/rights tests | NOT OPEN |
| Telemetry | question, necessity, legal basis | event catalog, UI, kill switch, QA | NOT OPEN |
| Breach response | contacts and templates | timed drill completed | PLANNED |
