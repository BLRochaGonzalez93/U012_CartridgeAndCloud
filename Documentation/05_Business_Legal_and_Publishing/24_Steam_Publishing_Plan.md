---
title: "Cartridge & Cloud — Steam Publishing Plan"
subtitle: "Plan consolidado de preparación, publicación, lanzamiento y operación inicial en Steam"
author: "VRM Games / Blas Luis Rocha González"
date: "2026-07-01"
lang: es-ES
document_version: "1.0"
project_version: "0.0.17"
status: "PLANNING / NOT ONBOARDED / PUBLIC DISTRIBUTION BLOCKED"
---

# Cartridge & Cloud — Steam Publishing Plan

**Proyecto:** Cartridge & Cloud  
**Desarrollador previsto:** VRM Games / Blas Luis Rocha González  
**Plataforma inicial:** PC / Steam  
**Motor:** Unity 6.3 LTS `6000.3.18f1` / URP `17.3.0`  
**Versión observada:** `0.0.17`  
**Estado:** Sprints 0–15 `CLOSED / PASS`; Sprint 16 `IN PROGRESS`; Sprint 17 y H6 `PENDING`  
**Fecha de verificación de Steamworks:** 1 de julio de 2026

> **Aviso:** este documento es un plan operativo de publicación y control. No sustituye asesoramiento jurídico, fiscal, contable o contractual. Los requisitos de Steamworks, las plantillas, los términos, las políticas de contenido y los procesos de revisión se revalidarán en la cuenta de partner antes de cada decisión irreversible.

# 0. Control del documento

Define la autoridad, el alcance y la forma de mantener este plan. La publicación se trata como una cadena verificable desde la identidad legal hasta el soporte posterior, no como una única subida de archivos.

**Estado actual.** Este es el documento consolidado número 24. Los planes históricos v0.3, v0.4 y v0.5 siguen preservados como genealogía y evidencia de decisiones anteriores.

**Regla de aprobación.** Toda modificación material debe registrar fecha, motivo, fuente oficial, impacto en coste/calendario, owner, gate y rollback. Una decisión de Steamworks no se considera adoptada por aparecer aquí: necesita ejecución y evidencia.

**Procedimiento operativo.**
1. Actualizar el historial y la matriz de fuentes.
2. Revisar contradicciones con Build, QA, Localization y Legal.
3. Separar claramente `CURRENT`, `PLANNED`, `DEFERRED`, `NOT OPEN` y `BLOCKED`.
4. Revalidar fuentes oficiales antes de onboarding, Coming Soon, review y release.

**Evidencia mínima.** Hash del documento, lista de fuentes, changelog y aprobación del propietario del proyecto.

# 1. Propósito

El propósito es preparar una publicación honesta, reproducible y reversible en Steam, minimizando riesgos técnicos, legales, comerciales y reputacionales.

**Estado actual.** No hay AppID, página pública, SDK Steamworks, depots ni store assets finales en el proyecto observado.

**Regla de aprobación.** El plan solo puede avanzar a la siguiente fase cuando el producto, los materiales y la capacidad de soporte estén al mismo nivel de madurez.

**Procedimiento operativo.**
1. Convertir requisitos en work packages verificables.
2. No prometer features que no estén probadas en build.
3. Conservar una candidata anterior utilizable para rollback.
4. Preparar soporte y comunicación antes de aceptar dinero.

**Evidencia mínima.** Roadmap, checklist de gates y paquete de release completo.

# 2. Autoridad y relación con otros documentos

La autoridad se distribuye: GDD define producto; VSS define H6; Build Guide define artefactos; QA define pruebas; Localization define ES/EN; Legal define autorización; este plan coordina Steam.

**Estado actual.** Los documentos `00–23` están disponibles y se han usado como fuente. La auditoría global anterior quedó pospuesta hasta completar la generación documental.

**Regla de aprobación.** Ante conflicto, prevalece la fuente especializada vigente y se abre una discrepancia; no se corrige silenciosamente una cifra o requisito en este plan.

**Procedimiento operativo.**
1. Comparar con 02, 07, 11, 12, 13, 16, 19, 22 y 23.
2. Registrar discrepancias en trazabilidad.
3. No duplicar datos sensibles de Steamworks en documentos públicos.
4. Repetir auditoría global al final de la jerarquía documental.

**Evidencia mínima.** Matriz de autoridad y enlaces a requisitos/gates.

# 3. Genealogía documental completa

La genealogía incluye el plan v0.3 de baseline v0.3, su reedición en v0.4, el plan v0.4 de baseline v0.5 y el plan v0.5 de baseline v0.6.

**Estado actual.** Se han localizado 4 fuentes Markdown históricas del Steam Publishing Plan, además de sus PDF y documentos de soporte.

**Regla de aprobación.** Las versiones anteriores no se eliminan ni se reinterpretan como implementación. Se preservan con hash, baseline y estado `SUPERSEDED / HISTORICAL`.

**Procedimiento operativo.**
1. Inventariar cada versión y hash.
2. Identificar reglas conservadas, sustituidas y diferidas.
3. Mantener la regla histórica de revalidar Steamworks.
4. Vincular la autoridad actual a este archivo.

**Evidencia mínima.** Tabla de genealogía y fuentes inmutables.

| Baseline | Documento | SHA-256 | Estado |
|---|---|---|---|
| 0.3 | `Cartridge_And_Cloud_Steam_Publishing_Plan_v0.3.md` | `1a004be75991bdcbbaf457a57da123e3297fce15a38c591e3f589e11e277dbd5` | HISTORICAL |
| 0.4 | `Cartridge_And_Cloud_Steam_Publishing_Plan_v0.3.md` | `56979c2830951e4a82a7d01793cebe9a930df4a773245de2b7489d0e73cb0777` | HISTORICAL |
| 0.5 | `Cartridge_And_Cloud_Steam_Publishing_Plan_v0.4.md` | `19664d4092ea89898c5bc37483a0b1aba6f473abbfa02c2794df8f2dd482058d` | HISTORICAL |
| 0.6 | `Cartridge_And_Cloud_Steam_Publishing_Plan_v0.5.md` | `f7840ee791e07430dd750500421dc6e262d21d5119e634634919247ff13cf8d0` | HISTORICAL |

# 4. Lecciones de v0.3

v0.3 estableció los gates esenciales: nombre validado, vertical slice representativo, capturas reales, texto ES/EN, build Windows instalable, QA, privacidad, soporte, presupuesto y calendario.

**Estado actual.** Era una especificación de preproducción sin suponer implementación. Esa cautela sigue siendo válida.

**Regla de aprobación.** Ningún gate histórico se da por cerrado por antigüedad. Debe demostrarse con evidencia actual.

**Procedimiento operativo.**
1. Conservar los gates como requisitos de entrada.
2. Actualizar taxonomía, plantillas y tiempos oficiales.
3. No usar mockups como gameplay.
4. No iniciar una campaña pública sin capacidad de entrega.

**Evidencia mínima.** Comparativa de gates históricos frente a estado actual.

# 5. Lecciones de la reedición v0.4

La reedición confirmó Unity 6.3 LTS y una fundación técnica, pero mantuvo Steam sin configurar. Introdujo la idea de branches `default`, `qa` y `private_test` cuando existiera build publicable.

**Estado actual.** La fundación técnica no equivale a producto comercial. Esta distinción se mantiene.

**Regla de aprobación.** Las branches se crean cuando existe AppID y política de acceso; no se simulan nombres sin infraestructura.

**Procedimiento operativo.**
1. Mantener Windows x64 como plataforma inicial.
2. Crear branches solo tras definir owners y passwords.
3. Documentar qué build vive en cada branch.
4. No exponer `default` antes de autorización.

**Evidencia mínima.** Mapa de branches y permisos aprobado.

# 6. Lecciones de v0.4 baseline v0.5

El plan v0.4 redujo el alcance a una secuencia Alpha interna → Beta limitada → Release Candidate → review → lanzamiento y declaró que la baseline solo validaba Windows Development local.

**Estado actual.** La secuencia sigue siendo adecuada, pero ahora debe incorporar S16/S17/H6, legal y localización.

**Regla de aprobación.** No se salta de development local a default branch. Cada transición necesita artefacto, QA, decisión y rollback.

**Procedimiento operativo.**
1. Definir criterios por fase.
2. Mantener evidencia de build y checksum.
3. Añadir legal, store page y soporte.
4. Diferenciar H6 de una Release Candidate de Steam.

**Evidencia mínima.** Tabla de transición por fase y gate.

# 7. Lecciones de v0.5 baseline v0.6

v0.5 reconoció Sprints 0–15 cerrados, Sprint 16 en curso y Steam diferido. Cloud, achievements y otros servicios no se consideraban parte de S16–S17.

**Estado actual.** Ese alcance sigue vigente: la integración Steam no debe desviar el cierre de StoreInitial, la estabilización o H6.

**Regla de aprobación.** Este plan prepara la futura ejecución sin abrir features ni añadir SDK en Sprint 16 por anticipación.

**Procedimiento operativo.**
1. Cerrar primero StoreInitial y S16.
2. Estabilizar en S17.
3. Aprobar H6 como hito interno.
4. Abrir onboarding Steam mediante decisión posterior.

**Evidencia mínima.** Decisión de apertura del programa de publicación, separada de H6.

# 8. Principios rectores

La publicación se rige por veracidad, trazabilidad, reversibilidad, mínimo privilegio, privacidad, accesibilidad, evidencia y soporte proporcional.

**Estado actual.** El proyecto ya tiene una base funcional, pero la presentación, la escena representativa y varios gates legales siguen abiertos.

**Regla de aprobación.** No se utiliza Steam para ocultar deuda ni para convertir un prototipo en producto por mera visibilidad.

**Procedimiento operativo.**
1. Describir solo lo que la build hace.
2. Usar capturas reales de la candidata.
3. No recopilar datos sin propósito y política.
4. Poder retirar o revertir un build defectuoso.

**Evidencia mínima.** Checklist firmado y materiales coherentes con la build.

# 9. Estado ejecutivo actual

Resume la preparación real al 1 de julio de 2026.

**Estado actual.** Proyecto `Cartridge & Cloud` de `VRM Games`, versión `0.0.17`. No hay integración Steamworks, paquetes Steam ni assets de marketing Steam detectados; las búsquedas encontraron 0 referencias técnicas y 0 recursos con nombres de Steam/marketing.

**Regla de aprobación.** El estado de distribución pública es `BLOCKED`. Las builds actuales solo son aptas para desarrollo o QA interna según su registro.

**Procedimiento operativo.**
1. No crear AppID hasta aclarar identidad y título.
2. No publicar Coming Soon hasta disponer de materiales reales.
3. No subir un build con `TestLab` o Store histórica como candidata.
4. No anunciar fecha.

**Evidencia mínima.** Dashboard legal, build profile, listado de ausencias y decisión no-go.

| Área | Estado observado |
|---|---|
| Steamworks SDK/API | No integrado |
| AppID | No documentado |
| Página de tienda | No preparada |
| Store assets/tráiler | No detectados |
| Build Profile | Bootstrap, MainMenu, Store, TestLab |
| StoreInitial | Objetivo; fuera del perfil actual |
| Localización ES/EN | Planificada; tablas no demostradas |
| Legal público | 0 builds elegibles |
| H6 | PENDING |

# 10. Verificación oficial vigente

Steamworks cambia con el tiempo. Las cifras y reglas operativas de este documento se verificaron contra documentación oficial el 1 de julio de 2026.

**Estado actual.** Las fuentes oficiales se usan como autoridad externa; los planes históricos se mantienen como contexto interno.

**Regla de aprobación.** Antes de una acción irreversible se vuelve a abrir la página oficial y se registra fecha/captura o export, porque plantillas, permisos y políticas pueden cambiar.

**Procedimiento operativo.**
1. Revisar onboarding y fee.
2. Revisar release/review/Coming Soon.
3. Descargar plantillas gráficas actuales.
4. Revisar SteamPipe, packages, pricing y funciones declaradas.

**Evidencia mínima.** Tabla de URL, fecha de revisión, owner y resultado.

| Fuente oficial | URL | Uso |
|---|---|---|
| Onboarding | https://partner.steamgames.com/doc/gettingstarted/onboarding | Fee/onboarding, partner data, 30-day wait and review overview. |
| Steam Direct Fee | https://partner.steamgames.com/doc/gettingstarted/appfee | USD 100 or equivalent per new app; recheck recovery and tax treatment. |
| Release Process | https://partner.steamgames.com/doc/store/releasing | Store checklist, build checklist, review and release permissions. |
| Review Process | https://partner.steamgames.com/doc/store/review_process | Typical 3–5 business days; plan at least 7 business days. |
| Coming Soon | https://partner.steamgames.com/doc/store/coming_soon | Minimum public Coming Soon period of two weeks for new products. |
| Store Page | https://partner.steamgames.com/doc/store/page | Publishing behavior, public assets and store-page rules. |
| Written Description | https://partner.steamgames.com/doc/store/page/description | Short description, About This Game and prohibited presentation patterns. |
| Graphical Assets | https://partner.steamgames.com/doc/store/assets | Current templates and required store/library asset families. |
| Graphical Asset Rules | https://partner.steamgames.com/doc/store/assets/rules | Capsule content restrictions and temporary overrides. |
| Store Graphical Assets | https://partner.steamgames.com/doc/store/assets/standard | Current capsule dimensions and screenshot requirements. |
| Library Assets | https://partner.steamgames.com/doc/store/assets/libraryassets | Library capsule, hero and logo requirements. |
| Steam Tags | https://partner.steamgames.com/doc/store/tags | Tag Wizard and relevance/order of top tags. |
| Localization and Languages | https://partner.steamgames.com/doc/store/localization | Separate store-page and in-game localization declarations. |
| Pricing | https://partner.steamgames.com/doc/store/pricing | Base package prices, currencies and minimum-price constraints. |
| Uploading to Steam | https://partner.steamgames.com/doc/sdk/uploading | SteamPipe ContentBuilder, app build scripts and depot scripts. |
| Builds | https://partner.steamgames.com/doc/store/application/builds | Builds, depots, manifests and live branch assignment. |
| Branches | https://partner.steamgames.com/doc/store/application/branches | Password branches and Steam client testing. |
| Updating Builds | https://partner.steamgames.com/doc/sdk/updating | Upload to test branch before default and package/depot checks. |
| Testing on Steam | https://partner.steamgames.com/doc/store/testing | Release override keys and Steam Playtest options. |
| Steam Playtest | https://partner.steamgames.com/doc/features/playtest | Separate Playtest AppID and controlled access. |
| Demos | https://partner.steamgames.com/doc/store/application/demos | Demo AppID, base-game relation and truthful feature declarations. |
| Steam Cloud | https://partner.steamgames.com/doc/features/cloud | Auto-Cloud or API integration and save synchronization. |
| Steam Deck Compatibility | https://partner.steamgames.com/doc/steamhardware/compat | Deck/Steam Machine review and Proton execution of Windows builds. |
| User Reviews | https://partner.steamgames.com/doc/store/reviews | Review scores, community expectations and support response. |

# 11. Condición para iniciar Steamworks

El onboarding se inicia cuando el proyecto puede sostener el coste, la espera, la configuración legal y una página Coming Soon honesta.

**Estado actual.** No se ha documentado una cuenta de partner operativa ni AppID.

**Regla de aprobación.** La apertura necesita un decision record que confirme titular, nombre provisional, presupuesto, responsable de seguridad y calendario realista.

**Procedimiento operativo.**
1. Cerrar identidad legal mínima.
2. Reservar presupuesto y tiempo de revisión.
3. Definir quién tendrá permisos Admin.
4. Preparar un paquete interno de store page antes de publicarlo.

**Evidencia mínima.** ADR/decisión de onboarding, responsable y presupuesto.

# 12. Titular legal y partner account

Steam exige que la entidad indicada sea quien posea o tenga derecho a publicar el producto y firme el acuerdo de distribución.

**Estado actual.** El registro legal identifica a Blas Luis Rocha González y VRM Games como titular/estudio operativo, pero forma jurídica, contacto y datos de partner siguen pendientes.

**Regla de aprobación.** No se crean cuentas con identidades inconsistentes ni datos provisionales que después compliquen pagos, impuestos o transferencia.

**Procedimiento operativo.**
1. Determinar persona o entidad firmante.
2. Alinear nombre legal, cuenta bancaria y fiscal.
3. Conservar contrato fuera del repositorio.
4. Registrar roles internos sin exponer datos sensibles.

**Evidencia mínima.** Checklist de onboarding legal y confirmación de titular.

# 13. Tarifa Steam Direct

La documentación oficial verificada indica una tarifa de 100 USD o equivalente por cada nueva aplicación que se quiera distribuir.

**Estado actual.** No existe partida presupuestaria aprobada ni crédito de aplicación documentado.

**Regla de aprobación.** El importe exacto, tratamiento fiscal y política de recuperación se verifican en la cuenta de partner el día del pago; no se paga hasta autorizar el AppID.

**Procedimiento operativo.**
1. Confirmar método de pago y owner Admin.
2. Registrar fecha, importe y comprobante.
3. Vincular el pago al AppID correcto.
4. No mezclar coste de juego base, demo u otras apps.

**Evidencia mínima.** Comprobante seguro, AppID y registro financiero.

# 14. Espera mínima tras el pago

La documentación oficial de onboarding establece una espera de 30 días entre el pago de la tarifa y la posibilidad de lanzar el producto.

**Estado actual.** El proyecto no ha iniciado ese reloj.

**Regla de aprobación.** El calendario interno debe incluir esta espera, sin prometer una fecha pública que dependa de un pago aún no realizado.

**Procedimiento operativo.**
1. Registrar `fee_paid_at`.
2. Calcular primera fecha técnicamente posible.
3. Añadir margen para Coming Soon y reviews.
4. No usar la fecha mínima como compromiso comercial.

**Evidencia mínima.** Timeline con fecha de pago, +30 días y buffers.

# 15. AppID y registros relacionados

El AppID será el identificador raíz de la aplicación base y no debe inventarse en documentos antes de recibirlo.

**Estado actual.** No hay AppID, demo AppID ni Playtest AppID.

**Regla de aprobación.** El AppID se almacena en un registro seguro y se referencia en scripts/configuración sin incluir credenciales.

**Procedimiento operativo.**
1. Crear registro de aplicación.
2. Separar AppID base, demo y Playtest.
3. Vincular depots/packages.
4. Actualizar build/version/legal/localization.

**Evidencia mínima.** Registro de AppID, permisos y relaciones.

# 16. Permisos Steamworks

Las acciones de publicar Coming Soon y lanzar requieren permisos específicos dentro de Steamworks.

**Estado actual.** No existe matriz de permisos.

**Regla de aprobación.** Aplicar mínimo privilegio: Admin solo para quienes lo necesiten; build upload, store edit y release se separan cuando sea posible.

**Procedimiento operativo.**
1. Definir roles.
2. Activar MFA y cuentas individuales.
3. Prohibir compartir credenciales.
4. Registrar quién puede aprobar una release.

**Evidencia mínima.** Matriz de permisos y revisión trimestral/por cambio de equipo.

# 17. Seguridad de la cuenta

Una cuenta comprometida puede alterar página, precios, builds o lanzamiento.

**Estado actual.** La seguridad operativa no está documentada fuera del marco general.

**Regla de aprobación.** MFA, contraseñas únicas, dispositivos confiables, recuperación segura y revocación inmediata son requisitos de entrada.

**Procedimiento operativo.**
1. Activar MFA.
2. Usar gestor de contraseñas.
3. Definir recuperación.
4. Revisar usuarios y sesiones antes de release.

**Evidencia mínima.** Checklist de seguridad sin secretos en repositorio.

# 18. Clearance del título Cartridge & Cloud

El título sigue siendo provisional y el registro legal lo marca como pendiente de clearance.

**Estado actual.** No se ha acreditado búsqueda de marcas, compañías, dominios, tiendas o conflictos de producto.

**Regla de aprobación.** El título final debe estar resuelto antes de la revisión pre-release; la documentación actual de Steam advierte que el nombre no puede cambiarse después de ese proceso.

**Procedimiento operativo.**
1. Realizar búsquedas jurídicas y comerciales.
2. Comprobar Steam, web, dominios y redes.
3. Documentar candidatos alternativos.
4. Aprobar el nombre antes de enviar la store page a pre-release review.

**Evidencia mínima.** Informe de clearance y decisión firmada.

# 19. Clearance del nombre VRM Games

El nombre operativo del estudio también puede afectar el partner account, la store page y los créditos.

**Estado actual.** No hay búsqueda formal documentada.

**Regla de aprobación.** El nombre del developer/publisher debe coincidir con la identidad comercial aprobada y no inducir a confusión.

**Procedimiento operativo.**
1. Buscar nombre comercial y marcas.
2. Alinear dominio/correo/Steamworks.
3. Decidir developer y publisher visibles.
4. Registrar sustitución si cambia.

**Evidencia mínima.** Informe y datos consistentes en Steamworks/legal/créditos.

# 20. Datos fiscales

El onboarding incluye información fiscal, que debe completarse con datos verdaderos y actuales.

**Estado actual.** No se conservan datos fiscales en los documentos del proyecto, correctamente.

**Regla de aprobación.** Los datos sensibles se introducen y almacenan solo en sistemas autorizados; este plan registra estado, no números fiscales.

**Procedimiento operativo.**
1. Preparar documentación fuera del repo.
2. Completar entrevista fiscal.
3. Revisar retenciones y país.
4. Guardar confirmación segura.

**Evidencia mínima.** Estado `COMPLETE` y fecha, sin copiar datos sensibles.

# 21. Datos bancarios y pagos

El partner que recibe las ventas debe ser el mismo que corresponde a la entidad contractual.

**Estado actual.** No hay configuración documentada.

**Regla de aprobación.** La cuenta bancaria, moneda y beneficiario se validan antes de fijar precios o lanzar.

**Procedimiento operativo.**
1. Verificar titularidad.
2. Realizar validaciones del portal.
3. Definir conciliación contable.
4. Restringir acceso a pagos.

**Evidencia mínima.** Confirmación de Steamworks y procedimiento contable.

# 22. Roles internos de publicación

Aunque el proyecto sea individual, conviene separar funciones: Product Owner, Build Owner, Store Owner, Legal Owner, QA Owner y Release Approver.

**Estado actual.** Actualmente todas recaen en VRM Games/Blas Luis Rocha González.

**Regla de aprobación.** La misma persona puede ocupar varios roles, pero cada checklist debe firmarse desde su función para evitar omisiones.

**Procedimiento operativo.**
1. Asignar RACI.
2. Definir sustituto de emergencia.
3. Registrar hora de disponibilidad en release.
4. No delegar release sin handoff.

**Evidencia mínima.** RACI y contactos operativos.

# 23. Gestión de secretos

Credenciales, tokens de upload, claves de branch, datos fiscales y contraseñas no pertenecen al repositorio ni a este documento.

**Estado actual.** No se han detectado secretos Steam porque no hay integración.

**Regla de aprobación.** Usar almacén seguro, variables de entorno y rotación. Los scripts se versionan sin secretos.

**Procedimiento operativo.**
1. Definir secret store.
2. Crear plantillas `.example`.
3. Escanear commits.
4. Rotar tras incidente o cambio de personal.

**Evidencia mínima.** Resultado de secret scan y lista de secretos gestionados.

# 24. Presupuesto de publicación

El coste no es solo la tarifa: incluye arte de tienda, tráiler, traducción/LQA, hardware, QA externa, legal, font/audio, soporte y contingencia.

**Estado actual.** No hay presupuesto de Steam aprobado.

**Regla de aprobación.** No se fija fecha pública hasta que el presupuesto cubra los requisitos mínimos y una reserva de hotfix/soporte.

**Procedimiento operativo.**
1. Crear presupuesto base y escenario adverso.
2. Separar costes únicos y recurrentes.
3. Incluir impuestos/comisiones sin estimaciones engañosas.
4. Aprobar contingencia.

**Evidencia mínima.** Presupuesto versionado y autorización.

# 25. Definición del producto publicable

La aplicación base debe representar una tienda física de videojuegos operable, con compra, recepción, stock, displays, clientes, checkout, ciclo diario y persistencia.

**Estado actual.** La vertical slice funcional existe; la presentación representativa y StoreInitial siguen en integración.

**Regla de aprobación.** La store page se redacta desde la build que se pretende vender, no desde la visión completa de ecommerce, publishing o plataforma.

**Procedimiento operativo.**
1. Congelar lista de sistemas incluidos.
2. Relacionar cada claim con evidencia.
3. Excluir visión futura.
4. Definir known issues aceptables.

**Evidencia mínima.** Feature matrix y Golden Path de la candidata.

# 26. Alcance de la primera versión

La primera versión debe ser pequeña, coherente y soportable. Los sistemas futuros permanecen `NOT OPEN` hasta decisión posterior.

**Estado actual.** Expansiones, ecommerce, publishing, infraestructura y otros metajuegos no están comprometidos.

**Regla de aprobación.** No incluir menús vacíos, promesas de roadmap ni assets de expansión en el depot de lanzamiento.

**Procedimiento operativo.**
1. Marcar inclusiones/exclusiones.
2. Escanear assets NOT OPEN.
3. Revisar copy y trailers.
4. Alinear precio con alcance real.

**Evidencia mínima.** Manifest de alcance y exclusión.

# 27. Modelo de lanzamiento: completo frente a Early Access

Early Access no debe usarse para financiar una idea sin experiencia jugable ni para evitar un estándar de calidad.

**Estado actual.** No se ha aprobado Early Access; los planes históricos hablan de vertical slice/MVP y lanzamiento futuro.

**Regla de aprobación.** La decisión se toma tras H6 y validación de contenido, roadmap, capacidad de comunicación y duración real del desarrollo restante.

**Procedimiento operativo.**
1. Comparar release completo, demo, Playtest y Early Access.
2. Definir valor actual y roadmap financiado.
3. Evaluar soporte continuo.
4. Registrar no-go si la promesa no es sostenible.

**Evidencia mínima.** ADR comercial con razones y copy específico si se adopta Early Access.

# 28. Plataforma inicial

La plataforma comprometida es Windows x64 a través de Steam.

**Estado actual.** Las builds existentes son Windows x64 locales; no hay depot Steam.

**Regla de aprobación.** No declarar macOS o Linux nativo sin builds, QA y soporte. Steam Deck puede evaluarse mediante Proton sobre Windows.

**Procedimiento operativo.**
1. Preparar un depot Windows.
2. Definir ejecutable y launch options.
3. Probar rutas/permissions.
4. Mantener plataformas futuras fuera de store data.

**Evidencia mínima.** Build Windows candidate y smoke en Steam client.

# 29. Windows x64

La candidata debe instalar, iniciar y guardar en una cuenta de usuario estándar, sin depender del Editor ni de rutas de desarrollo.

**Estado actual.** El perfil actual incluye Store y TestLab y usa 1024×768 no redimensionable.

**Regla de aprobación.** Antes de Steam se requiere perfil Release separado, StoreInitial, TestLab excluido y decisiones de ventana/resolución verificadas.

**Procedimiento operativo.**
1. Crear Windows_Steam_RC.
2. Definir arquitectura x64.
3. Excluir símbolos/debug no necesarios.
4. Probar instalación limpia y permisos.

**Evidencia mínima.** Manifest de archivos, launch test y Player.log limpio.

# 30. Steam Deck y Proton

Steam puede ejecutar juegos Windows en Deck mediante Proton y ofrece revisión de compatibilidad.

**Estado actual.** No hay evidencia de prueba en Deck/Proton.

**Regla de aprobación.** No declarar compatibilidad ni icono de soporte hasta probar controles, legibilidad, rendimiento, teclado virtual, guardado y suspensión.

**Procedimiento operativo.**
1. Probar mediante Deck o entorno adecuado.
2. Revisar 1280×800 y UI escalable.
3. Comprobar textos, input y rendimiento.
4. Solicitar/revisar compatibilidad cuando la build sea estable.

**Evidencia mínima.** Informe Deck/Proton y resultado de revisión si existe.

# 31. Linux nativo futuro

Un build Linux nativo implica depot, toolchain, QA, soporte y save paths propios.

**Estado actual.** No forma parte del alcance actual.

**Regla de aprobación.** No marcar Linux soportado por el hecho de que Proton pueda ejecutar la versión Windows.

**Procedimiento operativo.**
1. Mantener `NOT OPEN`.
2. Abrir solo mediante ADR.
3. Crear CI/build y QA específicos.
4. Actualizar store page y requisitos.

**Evidencia mínima.** Decisión y build nativo si se abre.

# 32. macOS futuro

macOS requiere firma, notarización, arquitecturas y permisos específicos.

**Estado actual.** No está incluido.

**Regla de aprobación.** No anunciarlo ni reservar una fecha hasta contar con hardware, pipeline y QA.

**Procedimiento operativo.**
1. Mantener excluido.
2. Evaluar mercado/coste.
3. Definir firma/notarización.
4. Crear depot separado si se aprueba.

**Evidencia mínima.** ADR y evidencia de plataforma.

# 33. Requisitos mínimos y recomendados

Los requisitos de sistema deben proceder de perfilado y pruebas, no de intuición.

**Estado actual.** No se dispone de matriz de hardware representativa ni datos suficientes para publicar CPU/GPU/RAM fiables.

**Regla de aprobación.** Mantener campos `TBD` internamente hasta ejecutar una campaña con hardware mínimo, objetivo y recomendado.

**Procedimiento operativo.**
1. Definir escenarios y resolución.
2. Medir FPS, memoria, carga y estabilidad.
3. Probar iGPU/GPU representativas.
4. Publicar valores conservadores y comprensibles.

**Evidencia mínima.** Informe de hardware y valores aprobados ES/EN.

# 34. Resolución, ventana y relación de aspecto

La store page y QA deben declarar una experiencia coherente en resoluciones habituales.

**Estado actual.** ProjectSettings observa 1024×768 y ventana no redimensionable, mientras las guías de UI usan 1920×1080 como referencia y escalado 80–150 %.

**Regla de aprobación.** Resolver la discrepancia antes de la candidata; probar 16:9, 16:10 y resolución mínima admitida.

**Procedimiento operativo.**
1. Decidir default y resize/fullscreen.
2. Probar HUD, modales y Operations.
3. Validar Deck 16:10 si se evalúa.
4. Actualizar requisitos y capturas.

**Evidencia mínima.** Matriz de resolución y configuración final.

# 35. Input y soporte de mando

Solo se declara soporte de mando cuando el juego completo es operable sin teclado/ratón y la store page usa la categoría correcta.

**Estado actual.** El Input System está activo, pero el alcance actual prioriza teclado/ratón y existe deuda de click-through/foco.

**Regla de aprobación.** No anunciar mando ni Full Controller Support hasta campaña completa.

**Procedimiento operativo.**
1. Cerrar exclusividad UI/mundo.
2. Probar teclado/ratón.
3. Evaluar mando en fase posterior.
4. Alinear Steam Input y glyphs si se abre.

**Evidencia mínima.** Matriz de dispositivos y declaración de store.

# 36. Idiomas de lanzamiento

El objetivo es interfaz y textos en es-ES y en-US, con store page localizada en ambos idiomas.

**Estado actual.** El paquete Localization está instalado, pero las tablas no están implementadas y la UI sigue mayoritariamente en inglés directo.

**Regla de aprobación.** No declarar un idioma como interfaz completa hasta que la build, no solo la store page, lo soporte.

**Procedimiento operativo.**
1. Completar Sprint 17 Localization.
2. Ejecutar ES/EN/pseudo.
3. Localizar store copy y assets con texto.
4. Verificar créditos/notices.

**Evidencia mínima.** Tabla de idiomas Steam coherente con la build.

# 37. Accesibilidad comercial

La página debe describir capacidades reales, como escala de UI, reducción de movimiento o canales de audio, sin utilizar claims genéricos no probados.

**Estado actual.** Existen opciones parciales de accesibilidad y deuda de fuente/localización.

**Regla de aprobación.** Cada claim debe tener caso QA y captura o vídeo.

**Procedimiento operativo.**
1. Inventariar opciones reales.
2. Probar persistencia y navegación.
3. Documentar limitaciones.
4. Actualizar store copy/FAQ.

**Evidencia mínima.** Accessibility feature matrix y pruebas.

# 38. Clasificación de contenido

Los cuestionarios de contenido y clasificación deben responder según la build final, incluyendo texto, imágenes, humor, violencia o elementos sensibles.

**Estado actual.** No se han identificado contenidos maduros relevantes, pero no existe evaluación formal.

**Regla de aprobación.** No seleccionar categorías por intuición ni copiar respuestas de otro título.

**Procedimiento operativo.**
1. Inventariar contenido.
2. Revisar conceptos y marketing.
3. Completar cuestionarios oficiales.
4. Conservar respuestas y fecha.

**Evidencia mínima.** Export/captura de cuestionarios y decisión.

# 39. Privacidad y datos

La política debe corresponder a datos realmente recogidos, no ser una plantilla genérica.

**Estado actual.** No se ha observado telemetría Steam ni online; sistemas futuros están diferidos.

**Regla de aprobación.** Mientras no se recojan datos remotos, documentar la ausencia; si se añade crash reporting, analytics, newsletter o soporte, reabrir revisión.

**Procedimiento operativo.**
1. Inventariar flujos.
2. Identificar proveedores y base legal.
3. Minimizar datos.
4. Publicar política antes de activar recogida.

**Evidencia mínima.** Data-flow map y política aprobada cuando aplique.

# 40. EULA

Una EULA es opcional según el modelo, pero si se usa debe ser coherente con jurisdicción, Steam Distribution Agreement, privacidad y producto.

**Estado actual.** No existe EULA aprobada.

**Regla de aprobación.** No generar una EULA decorativa. Decidir con asesoramiento adecuado si aporta valor y cómo se presenta.

**Procedimiento operativo.**
1. Evaluar necesidad.
2. Redactar/revisar.
3. Localizar si procede.
4. Versionar y archivar aceptación.

**Evidencia mínima.** Decisión y texto legal aprobado.

# 41. Créditos y notices

La release necesita créditos completos y avisos de terceros cuando las licencias lo requieran.

**Estado actual.** El registro 23 muestra audio/localización/notices pendientes y un bloque de créditos `BLOCKED`.

**Regla de aprobación.** No lanzar mientras falten créditos obligatorios o `THIRD_PARTY_NOTICES` verificados.

**Procedimiento operativo.**
1. Cerrar autores/proveedores.
2. Extraer LICENSE/NOTICE por paquete.
3. Generar créditos in-game y archivo externo.
4. Probar legibilidad y localización.

**Evidencia mínima.** Credits capture, notices hash y aprobación legal.

# 42. Arquitectura de la store page

La página se compone de Basic Info, descripciones, assets gráficos, trailers/screenshots, idiomas, requisitos, categorías y enlaces oficiales.

**Estado actual.** No existe página ni borrador estructurado.

**Regla de aprobación.** Preparar primero un paquete offline versionado y revisado; después transcribirlo a Steamworks.

**Procedimiento operativo.**
1. Crear matriz de campos.
2. Asignar source ES/EN.
3. Vincular claims a features.
4. Registrar captura/preview antes de publicar.

**Evidencia mínima.** Store page content pack y preview aprobada.

# 43. Descripción corta

Debe explicar rápidamente el género, la fantasía y el bucle principal sin fechas, calls-to-action impropios ni features futuras.

**Estado actual.** No hay copy final.

**Regla de aprobación.** Escribir una frase comprensible para jugadores que no conocen el proyecto y mantener ES/EN equivalentes, no traducciones literales rígidas.

**Procedimiento operativo.**
1. Redactar varias opciones.
2. Validar con positioning.
3. Eliminar promesas no demostradas.
4. Probar longitud en Steam preview.

**Evidencia mínima.** Copy aprobado y revisión lingüística.

**Borrador de trabajo, no publicable todavía:**

- **ES:** Gestiona una pequeña tienda de videojuegos: compra mercancía, recibe pedidos, organiza expositores y atiende a tus clientes mientras conviertes un local modesto en un negocio eficiente.
- **EN:** Run a small video game shop: order stock, receive deliveries, arrange displays, and serve customers as you turn a modest storefront into an efficient business.

# 44. Acerca de este juego

La descripción larga debe explicar la experiencia y la relación entre sistemas, usando imágenes reales y secciones legibles.

**Estado actual.** La visión histórica incluye sistemas no comprometidos; deben excluirse.

**Regla de aprobación.** Centrar el texto en tienda física, inventario, pedidos, displays, clientes, checkout, economía y ciclo diario demostrados.

**Procedimiento operativo.**
1. Crear outline.
2. Asociar cada sección a screenshot/clip.
3. Revisar 15 MB total de imágenes/GIFs según guía actual.
4. Localizar y previsualizar.

**Evidencia mínima.** HTML/BBCode preview ES/EN y claim matrix.

# 45. Key features

Las viñetas deben describir acciones y decisiones, no una lista de módulos internos.

**Estado actual.** Las features candidatas se conocen, pero su presentación final depende de StoreInitial y H6.

**Regla de aprobación.** Cada viñeta debe poder demostrarse en menos de un minuto de gameplay capturado.

**Procedimiento operativo.**
1. Seleccionar 4–6 features.
2. Evitar superlativos sin prueba.
3. No mencionar expansiones.
4. Validar orden y lenguaje.

**Evidencia mínima.** Feature-to-evidence table.

# 46. Tags provisionales

Los planes históricos proponen Management, Simulation, Shop Keeper, Building, Economy, Strategy, Indie y Singleplayer.

**Estado actual.** No se ha ejecutado Tag Wizard ni comparación competitiva.

**Regla de aprobación.** Usar solo tags existentes y relevantes; revisar el orden de los 20 principales porque Steam los usa para similitud/recomendaciones.

**Procedimiento operativo.**
1. Ejecutar Tag Wizard.
2. Comparar páginas realmente similares.
3. Eliminar tags aspiracionales.
4. Revisar orden tras Playtest.

**Evidencia mínima.** Captura de Tag Wizard y lista ordenada aprobada.

# 47. Géneros, categorías y features

Las categorías de tienda deben corresponder a capacidades reales: single-player, input, cloud, achievements, languages y plataformas.

**Estado actual.** Singleplayer es claro; otras features de Steam no están implementadas.

**Regla de aprobación.** No marcar achievements, Cloud, Workshop, mando completo o Remote Play basándose en intención.

**Procedimiento operativo.**
1. Completar feature checklist desde build.
2. Probar cada opción.
3. Revisar cambios antes de release.
4. Mantener historial.

**Evidencia mínima.** Matriz de checkboxes con evidencia.

# 48. Idiomas de store e in-game

Steam trata por separado la localización de la store page y el soporte dentro del juego.

**Estado actual.** ES/EN están planificados, no implementados.

**Regla de aprobación.** Declarar únicamente idiomas completos en la tabla in-game; localizar también la store page para esos públicos.

**Procedimiento operativo.**
1. Completar tablas Unity.
2. Probar selector/persistencia.
3. Traducir copy y assets.
4. Configurar Steam con el nivel real.

**Evidencia mínima.** Golden Path bilingüe y store preview.

# 49. Requisitos de sistema en la página

Los campos mínimo/recomendado deben ser claros, medidos y consistentes con la plataforma marcada.

**Estado actual.** No hay requisitos aprobados.

**Regla de aprobación.** No publicar datos ficticios. Utilizar placeholders internos hasta cerrar perfilado.

**Procedimiento operativo.**
1. Ejecutar matriz hardware.
2. Definir SO, CPU, RAM, GPU, almacenamiento.
3. Añadir notas solo si ayudan.
4. Localizar sin alterar modelos técnicos.

**Evidencia mínima.** Informe y campos aprobados.

# 50. Developer, publisher y copyright

La página debe mostrar developer/publisher y avisos coherentes con titularidad.

**Estado actual.** VRM Games es el nombre operativo, pendiente de clearance; la entidad firmante no está cerrada.

**Regla de aprobación.** No rellenar con datos inconsistentes ni afirmar registros inexistentes.

**Procedimiento operativo.**
1. Cerrar identidad.
2. Definir copyright year/holder.
3. Alinear créditos y web.
4. Revisar símbolos ™/® antes de uso.

**Evidencia mínima.** Datos aprobados por legal.

# 51. Enlaces externos

Steam proporciona campos específicos para web y redes; las descripciones no deben incluir URLs, QR o enlaces implícitos.

**Estado actual.** No existe web/soporte oficial consolidado.

**Regla de aprobación.** Solo enlazar propiedades controladas, seguras y mantenibles; no crear una red social vacía por cumplir checklist.

**Procedimiento operativo.**
1. Decidir web y soporte.
2. Configurar HTTPS y contacto.
3. Revisar privacidad/cookies.
4. Probar todos los enlaces.

**Evidencia mínima.** Lista de enlaces y owner.

# 52. Screenshots

Las screenshots deben mostrar el juego real y, según plantillas actuales, usar al menos 1920×1080 y relación 16:9.

**Estado actual.** No hay screenshots de build candidata; solo conceptos y capturas técnicas.

**Regla de aprobación.** Capturar desde StoreInitial aprobado, sin debug, mockups, UI de desarrollo, información personal ni assets no autorizados.

**Procedimiento operativo.**
1. Definir shot list.
2. Capturar ES y/o EN según estrategia.
3. Verificar resolución y composición.
4. Registrar build/commit de origen.

**Evidencia mínima.** Mínimo operativo de cinco capturas variadas y una hoja de procedencia; Steam exige screenshots reales, no conceptos.

# 53. Tráiler

El primer tráiler visible condiciona el microtráiler generado por Steam y debe comenzar con gameplay representativo.

**Estado actual.** No hay tráiler aprobado.

**Regla de aprobación.** Evitar logos largos, cinemáticas inexistentes o montaje que oculte el juego. Música, fuente y clips deben estar licenciados.

**Procedimiento operativo.**
1. Crear guion por beats.
2. Capturar candidata.
3. Editar ES-neutral o sin texto.
4. Revisar audio/legal/claims.

**Evidencia mínima.** Master, proyecto editable, cue sheet, captions y build source.

# 54. Cápsulas de tienda

Las plantillas vigentes incluyen Header 920×430, Small 462×174 y Main 1232×706; deben descargarse de Steamworks al producir.

**Estado actual.** No hay cápsulas finales.

**Regla de aprobación.** Las cápsulas base contienen arte, nombre y subtítulo oficial; no incluyen descuentos, reseñas, fechas ni otros textos. Las campañas temporales usan overrides conforme a reglas.

**Procedimiento operativo.**
1. Descargar templates actuales.
2. Crear key art adaptable.
3. Comprobar logo en tamaños pequeños.
4. Revisar PG-13, IP y localización.

**Evidencia mínima.** Archivos fuente, exports y revisión contra templates.

# 55. Assets de biblioteca

Steam requiere assets para la biblioteca, incluidos Library Capsule y hero/logo conforme a plantillas actuales.

**Estado actual.** No están preparados.

**Regla de aprobación.** La Library Capsule actual es 920×430; el hero debe ser visual y sin texto, y el logo se entrega separado según especificación vigente.

**Procedimiento operativo.**
1. Descargar templates.
2. Diseñar conjunto coherente.
3. Probar contraste y recortes.
4. Publicar junto a la store page.

**Evidencia mínima.** Exports y preview en Steam client.

# 56. Logo e iconos

El logo debe ser legible en cápsulas, biblioteca, ejecutable y marketing, sin confundirlo con nombres reales.

**Estado actual.** La identidad negra/verde está definida conceptualmente, pero el título no está cleared.

**Regla de aprobación.** No congelar logo final hasta cerrar título; conservar versión monocroma, horizontal, compacta y fuente editable/licenciada.

**Procedimiento operativo.**
1. Cerrar naming.
2. Diseñar variantes.
3. Crear icono Windows/Steam.
4. Validar tamaños y contraste.

**Evidencia mínima.** Brand package y licencia de tipografía.

# 57. Pipeline de materiales de tienda

Todo asset comercial debe conservar source editable, export, plantilla, autor, licencia, build asociada y revisión.

**Estado actual.** El proyecto tiene conceptos, pero no una carpeta de Steam marketing.

**Regla de aprobación.** Crear un árbol fuera de `Assets` runtime o claramente separado para evitar incluir masters en depots.

**Procedimiento operativo.**
1. Definir rutas.
2. Nombrar por locale/tipo/versión.
3. Generar checksums.
4. Archivar aprobados y rechazados.

**Evidencia mínima.** Manifest de marketing assets.

# 58. Gameplay real y veracidad

La página debe representar lo que recibe el comprador.

**Estado actual.** StoreInitial aún no es runtime y las imágenes conceptuales son más maduras que la build actual.

**Regla de aprobación.** No publicar conceptos como screenshots ni usar renders para insinuar sistemas no jugables. El arte de cápsula puede ser ilustrativo, pero screenshots/tráiler deben ser gameplay real.

**Procedimiento operativo.**
1. Etiquetar conceptos.
2. Capturar candidata.
3. Comparar visualmente página/build.
4. Retirar assets obsoletos.

**Evidencia mínima.** Auditoría de veracidad por material.

# 59. Prohibición de mockups engañosos

Steam prohíbe gráficos que imiten su UI y la ética de publicación exige no fabricar interfaces o precios ficticios.

**Estado actual.** La UI actual tiene herramientas de desarrollo y conceptos.

**Regla de aprobación.** Excluir fake wishlist/buy buttons, precios embebidos, reviews inventadas y pantallas no implementadas.

**Procedimiento operativo.**
1. Revisar cada imagen.
2. Eliminar elementos tipo Steam UI.
3. Validar copy.
4. Conservar evidencia de origen.

**Evidencia mínima.** Checklist de compliance visual.

# 60. Copy en español

El español es idioma fuente y debe sonar natural, directo y consistente con el glosario.

**Estado actual.** No existen tablas operativas ni copy comercial final.

**Regla de aprobación.** Usar terminología acordada: tienda, almacén, expositor, pedido, recepción, reposición y caja; evitar traducciones técnicas crudas.

**Procedimiento operativo.**
1. Redactar source copy.
2. Revisar tono y claims.
3. Probar en preview.
4. Congelar junto a build.

**Evidencia mínima.** Copy deck es-ES aprobado.

# 61. Copy en inglés

El inglés estadounidense debe ser comercialmente natural y coherente con la UI.

**Estado actual.** No hay traducción final ni revisor asignado.

**Regla de aprobación.** No aprobar traducción automática sin revisión competente; resolver términos como backroom, display fixture, case y checkout según contexto.

**Procedimiento operativo.**
1. Traducir desde source congelado.
2. Revisión bilingüe.
3. Revisar claims legales.
4. Probar layout.

**Evidencia mínima.** Copy deck en-US y registro de revisión.

# 62. Matriz de claims

Cada afirmación comercial se conecta a una función, escena, prueba, build y owner.

**Estado actual.** No existe matriz.

**Regla de aprobación.** Un claim sin evidencia se elimina o se marca interno; una feature diferida no puede aparecer como “coming soon” salvo estrategia aprobada.

**Procedimiento operativo.**
1. Extraer claims de copy/tráiler.
2. Vincular evidencia.
3. Clasificar exacto/condicional/prohibido.
4. Revisar antes de publicar.

**Evidencia mínima.** Tabla claim → evidence → build.

# 63. Features futuras y roadmap público

La visión incluye multicanal, publishing, desarrollo interno y otros sistemas; no pertenecen a la primera versión confirmada.

**Estado actual.** Están documentados como visión/NOT OPEN.

**Regla de aprobación.** No publicar roadmap especulativo ni utilizarlo para justificar el precio. Un roadmap público exige recursos y lenguaje no contractual cuidadoso.

**Procedimiento operativo.**
1. Mantener fuera de store.
2. Abrir solo tras decisión.
3. No mostrar tabs vacíos.
4. Actualizar si una feature entra realmente.

**Evidencia mínima.** Revisión de alcance y copy.

# 64. Posicionamiento

Cartridge & Cloud se posiciona como simulación/gestión de una pequeña tienda de videojuegos, con construcción ligera, inventario, economía y atención al cliente.

**Estado actual.** El posicionamiento no se ha probado con audiencia ni comparables.

**Regla de aprobación.** Evitar comparaciones directas con marcas/juegos ajenos en copy. Usar comparables solo internamente para precio, tags y expectativas.

**Procedimiento operativo.**
1. Definir audiencia primaria.
2. Analizar competidores actuales.
3. Probar mensajes.
4. Alinear arte y precio.

**Evidencia mínima.** Positioning statement y test de comprensión.

# 65. Precio base

El precio debe reflejar alcance, calidad, duración, comparables y soporte, no los costes hundidos.

**Estado actual.** No hay precio aprobado ni análisis comercial actualizado.

**Regla de aprobación.** No introducir un precio en Steamworks hasta completar alcance y benchmarking. El partner que recibe las ventas debe ser el mismo configurado para pagos.

**Procedimiento operativo.**
1. Crear rango interno.
2. Analizar comparables y valor.
3. Probar sensibilidad.
4. Aprobar precio y rationale.

**Evidencia mínima.** Pricing memo con fecha y fuentes.

# 66. Precios regionales

Steam permite precios por moneda/región y aplica restricciones de precio mínimo.

**Estado actual.** No hay matriz regional.

**Regla de aprobación.** Revisar recomendaciones actuales, poder adquisitivo, impuestos y mínimos; no copiar una tabla antigua.

**Procedimiento operativo.**
1. Configurar base price.
2. Revisar sugerencias regionales.
3. Comprobar mínimos y redondeos.
4. Aprobar excepciones.

**Evidencia mínima.** Export/captura de pricing aprobado.

# 67. Descuentos

Los descuentos afectan percepción, elegibilidad y calendario promocional.

**Estado actual.** No existe estrategia.

**Regla de aprobación.** No prometer descuentos ni crear una política agresiva antes de conocer retención y ventas. Revalidar cooldowns/reglas oficiales al programar.

**Procedimiento operativo.**
1. Definir launch discount o ninguno.
2. Revisar eventos.
3. Simular ingresos.
4. Registrar aprobación.

**Evidencia mínima.** Discount calendar y decisión.

# 68. Fecha de lanzamiento

La fecha pública se fija solo después de contar con build, store assets, reviews, soporte y buffers.

**Estado actual.** No hay fecha ni onboarding.

**Regla de aprobación.** No anunciar una fecha mientras existan S0/S1 o bloqueos legales. Dentro de ventanas cercanas Steam puede limitar cambios; verificar reglas actuales.

**Procedimiento operativo.**
1. Crear calendario interno.
2. Añadir fee wait, Coming Soon y review.
3. Definir no-go date.
4. Publicar solo tras aprobación.

**Evidencia mínima.** Release timeline y go/no-go record.

# 69. Coming Soon

Para productos nuevos, la documentación actual exige una página Coming Soon pública al menos dos semanas antes del lanzamiento.

**Estado actual.** No existe página ni inicio del periodo.

**Regla de aprobación.** Publicar Coming Soon solo cuando el título, assets, copy y promesa sean estables; no usarla como placeholder vacío.

**Procedimiento operativo.**
1. Enviar store page a review.
2. Corregir feedback.
3. Publicar Coming Soon.
4. Registrar timestamp y no bajar la página sin plan.

**Evidencia mínima.** URL pública, fecha/hora y aprobación.

# 70. Wishlists

Coming Soon permite construir audiencia y Steam notifica a wishlisters en lanzamiento.

**Estado actual.** No hay campaña ni métricas.

**Regla de aprobación.** Las wishlists son señal, no garantía. No usar tácticas engañosas ni pagar por listas de baja calidad.

**Procedimiento operativo.**
1. Definir canales.
2. Medir visitas/conversión.
3. Aprender de mensajes.
4. Proteger privacidad.

**Evidencia mínima.** Dashboard de campaña y aprendizajes.

# 71. Calendario de marketing

El calendario debe coordinar store page, devlogs, demo/Playtest, festivales, tráiler, prensa y lanzamiento.

**Estado actual.** No existe calendario comercial.

**Regla de aprobación.** El calendario se deriva de capacidad y producto. No obliga al equipo a publicar contenido si compromete S16/S17/H6.

**Procedimiento operativo.**
1. Definir hitos y assets.
2. Asignar owners.
3. Crear buffers.
4. Mantener plan de silencio/delay.

**Evidencia mínima.** Calendario versionado y dependencia por gate.

# 72. Demo

Una demo es una aplicación separada relacionada con el juego base y debe declarar solo features/languages realmente presentes.

**Estado actual.** No está aprobada.

**Regla de aprobación.** Crear demo solo si mejora descubrimiento y puede mantenerse. Debe tener save/transfer policy, duración, onboarding y branch propios.

**Procedimiento operativo.**
1. Evaluar objetivo.
2. Definir contenido y corte.
3. Solicitar/configurar Demo AppID.
4. QA y store page de demo.

**Evidencia mínima.** Demo decision record y build separada.

# 73. Steam Playtest

Steam Playtest ofrece un AppID separado con acceso controlado y menor carga de store/community.

**Estado actual.** No está configurado.

**Regla de aprobación.** Es preferible a distribuir claves del juego base cuando se necesita feedback previo sin afectar reviews/wishlists del producto principal.

**Procedimiento operativo.**
1. Definir preguntas de test.
2. Configurar acceso.
3. Preparar privacidad/feedback.
4. Cerrar y desactivar al terminar.

**Evidencia mínima.** Playtest plan, cohortes y resultados.

# 74. Claves de Steam

Las claves son un mecanismo de acceso, no una moneda promocional ilimitada.

**Estado actual.** No hay AppID ni key policy.

**Regla de aprobación.** Emitir por motivo, lote, owner y destinatario; evitar reventa, filtraciones y promesas de pre-purchase no autorizadas.

**Procedimiento operativo.**
1. Definir tipos de key.
2. Registrar lotes.
3. Entregar de forma segura.
4. Revocar/analizar abusos cuando proceda.

**Evidencia mínima.** Key ledger seguro y propósito.

# 75. Curators

Steam Curators puede formar parte de una estrategia de descubrimiento, pero el outreach debe ser selectivo y transparente.

**Estado actual.** No hay lista ni material.

**Regla de aprobación.** No enviar builds no aprobadas ni pagar por cobertura encubierta. Preparar pitch y key tracking.

**Procedimiento operativo.**
1. Crear lista relevante.
2. Revisar autenticidad.
3. Enviar paquete coherente.
4. Registrar resultados.

**Evidencia mínima.** Outreach log y material enviado.

# 76. Press kit

El press kit reúne hechos, logos, screenshots, trailer, bios, contactos y enlaces descargables.

**Estado actual.** No existe.

**Regla de aprobación.** No incluir conceptos como gameplay, datos personales innecesarios ni claims sin evidencia. Versionar por fecha/build.

**Procedimiento operativo.**
1. Crear factsheet.
2. Añadir assets aprobados.
3. ES/EN si procede.
4. Probar descarga y licencias.

**Evidencia mínima.** Press kit hash y URL controlada.

# 77. Influencers y creadores

La colaboración requiere selección, transparencia, claves y expectativas claras.

**Estado actual.** No hay programa.

**Regla de aprobación.** Cumplir normas de divulgación; no exigir opinión positiva ni proporcionar builds inestables sin advertencia.

**Procedimiento operativo.**
1. Definir criterios.
2. Preparar briefing.
3. Registrar claves/embargos.
4. Gestionar soporte.

**Evidencia mínima.** Outreach/consent/embargo records.

# 78. Community Hub

La comunidad necesitará anuncios, discusiones, guías y soporte coherentes con la capacidad real.

**Estado actual.** No hay hub activo.

**Regla de aprobación.** No abrir espacios públicos sin moderación mínima y ruta de escalado. Fijar posts de FAQ, bugs conocidos y reporting.

**Procedimiento operativo.**
1. Definir categorías.
2. Crear normas.
3. Asignar moderación.
4. Preparar mensajes de lanzamiento.

**Evidencia mínima.** Community setup checklist.

# 79. Foros y moderación

La moderación protege a usuarios y equipo, pero debe ser proporcional y transparente.

**Estado actual.** No existe política.

**Regla de aprobación.** Diferenciar crítica, bug, abuso, spam y contenido ilegal. No borrar críticas por ser negativas.

**Procedimiento operativo.**
1. Redactar reglas.
2. Definir sanciones y apelación.
3. Guardar evidencia mínima.
4. Escalar amenazas/datos personales.

**Evidencia mínima.** Moderation policy y log.

# 80. Soporte al jugador

El soporte cubre instalación, saves, input, rendimiento, crashes, localización, reembolsos derivados a Steam y bugs.

**Estado actual.** No hay canal final ni SLA.

**Regla de aprobación.** Publicar un contacto mantenible, plantillas y datos solicitados mínimos. No pedir información sensible innecesaria.

**Procedimiento operativo.**
1. Elegir email/helpdesk.
2. Crear categorías.
3. Definir tiempo objetivo.
4. Preparar handoff de bugs.

**Evidencia mínima.** Support runbook y contacto probado.

# 81. FAQ

La FAQ reduce carga y alinea expectativas sobre plataforma, idiomas, saves, contenido y roadmap.

**Estado actual.** No existe.

**Regla de aprobación.** Responder solo hechos confirmados; actualizar con cambios de build y no usarla para prometer fechas o features.

**Procedimiento operativo.**
1. Recoger preguntas.
2. Redactar ES/EN.
3. Vincular soporte.
4. Revisar cada parche mayor.

**Evidencia mínima.** FAQ versionada.

# 82. Bug reporting

Los reportes deben incluir build, sistema, pasos, logs y save cuando sea seguro.

**Estado actual.** QA interno tiene defectos y Player.log, pero no flujo público.

**Regla de aprobación.** Crear formulario sencillo y advertir sobre datos personales; ofrecer método seguro para archivos.

**Procedimiento operativo.**
1. Definir campos.
2. Automatizar build ID visible.
3. Preparar upload seguro.
4. Triar por severidad.

**Evidencia mínima.** Formulario y proceso de triage.

# 83. Comunicación y tono

La comunicación pública debe ser honesta, concreta y coherente entre Steam, redes, soporte y patch notes.

**Estado actual.** No hay guía específica de comunidad.

**Regla de aprobación.** No minimizar defectos críticos ni prometer plazos sin evidencia. Publicar retrasos con claridad y próximos pasos.

**Procedimiento operativo.**
1. Definir voz.
2. Crear plantillas.
3. Revisar legal/localización.
4. Mantener single source of truth.

**Evidencia mínima.** Communication guide y approvals.

# 84. Arquitectura App–Package–Depot–Build

Steam separa aplicación, packages, depots y builds. El juego base tendrá un AppID y al menos un depot Windows.

**Estado actual.** No existe configuración.

**Regla de aprobación.** Diseñar el mínimo necesario: un depot de contenido Windows, packages correctos y branches controladas. No crear depots por carpetas internas sin necesidad.

**Procedimiento operativo.**
1. Definir diagrama.
2. Asignar depot IDs tras AppID.
3. Configurar packages.
4. Documentar ownership.

**Evidencia mínima.** Mapa App/Package/Depot/Branch.

# 85. Depot Windows inicial

El depot debe contener solo los archivos que necesita la instalación de producción.

**Estado actual.** El build local puede incluir Development/TestLab y archivos no destinados a release.

**Regla de aprobación.** Crear staging limpio y filtrar logs, fuentes editables, documentación interna, tests y assets NOT OPEN.

**Procedimiento operativo.**
1. Definir content root.
2. Generar manifest.
3. Comparar allowlist/denylist.
4. Instalar desde Steam y ejecutar.

**Evidencia mínima.** Depot file manifest y diff aprobado.

# 86. Branches

Las branches permiten separar default, QA, internal y hotfix candidate.

**Estado actual.** No existen.

**Regla de aprobación.** Propuesta inicial: `default`, `qa`, `internal`, `release_candidate` y una branch temporal de rollback/hotfix si es necesaria. Usar passwords y permisos.

**Procedimiento operativo.**
1. Crear naming policy.
2. Definir promoción.
3. Rotar passwords.
4. Registrar build live por branch.

**Evidencia mínima.** Branch matrix y Steam build IDs.

# 87. Default branch

La branch default es la experiencia que recibe el público.

**Estado actual.** No debe existir públicamente antes de release.

**Regla de aprobación.** Solo promover un build aprobado manualmente, después de probar exactamente el manifest que quedará live.

**Procedimiento operativo.**
1. Seleccionar build.
2. Verificar packages/depots.
3. Ejecutar smoke final.
4. Promover y registrar.

**Evidencia mínima.** BuildID, manifest IDs y aprobación.

# 88. QA e internal branches

Las branches privadas permiten probar el proceso real de Steam sin exponerlo.

**Estado actual.** No están configuradas.

**Regla de aprobación.** Usar `internal` para integración temprana y `qa` para candidatas; no dejar passwords en tickets públicos.

**Procedimiento operativo.**
1. Configurar access.
2. Seleccionar cohortes.
3. Definir expiración.
4. Documentar diferencias respecto a default.

**Evidencia mínima.** Branch access log y resultados.

# 89. SteamPipe ContentBuilder

SteamPipe utiliza scripts que describen la aplicación y cada depot y se ejecuta mediante herramientas del SDK.

**Estado actual.** No existe SDK ni scripts.

**Regla de aprobación.** Versionar scripts y plantillas sin credenciales. El contenido a subir se genera desde un staging reproducible, no directamente desde una carpeta manual.

**Procedimiento operativo.**
1. Instalar SDK controladamente.
2. Crear `app_build_*.vdf`.
3. Crear `depot_build_*.vdf`.
4. Probar upload a branch privada.

**Evidencia mínima.** Scripts, logs de upload y BuildID.

# 90. Content root y staging

El staging es la frontera entre Unity build y SteamPipe.

**Estado actual.** La Build Guide mantiene `Builds/` fuera de Git y versiona records/checksums.

**Regla de aprobación.** Crear un staging limpio, efímero y reconstruible con allowlist; nunca subir el proyecto Unity completo.

**Procedimiento operativo.**
1. Limpiar destino.
2. Copiar output aprobado.
3. Validar hashes.
4. Generar manifest de archivos.

**Evidencia mínima.** Staging manifest y hash.

# 91. Exclusión de TestLab

`TestLab` es una escena de desarrollo y no debe distribuirse.

**Estado actual.** Está actualmente incluido en EditorBuildSettings/Development profile.

**Regla de aprobación.** El perfil Steam RC excluye TestLab y cualquier acceso, launch option o asset exclusivamente de test.

**Procedimiento operativo.**
1. Crear profile separado.
2. Validar escena list.
3. Buscar referencias.
4. Probar build instalada.

**Evidencia mínima.** Build report sin TestLab.

# 92. Exclusión de assets NOT OPEN

Los 19 modelos/prefabs de expansión y otros contenidos de visión no deben formar parte del depot si no los necesita la release.

**Estado actual.** Existen en el source tree.

**Regla de aprobación.** Excluir del build mediante referencias/addressables/build configuration, no solo ocultarlos en UI.

**Procedimiento operativo.**
1. Inventariar dependencies.
2. Romper referencias accidentales.
3. Inspeccionar build report.
4. Verificar tamaño/depot.

**Evidencia mínima.** Asset inclusion report.

# 93. StoreInitial como escena runtime

StoreInitial es el objetivo representativo y debe reemplazar Store de forma controlada antes de una candidata Steam.

**Estado actual.** El perfil actual carga Store; blockout procedural sigue activo y StoreInitial no está en build list.

**Regla de aprobación.** No publicar hasta que StoreInitial opere el Golden Path, tenga contexto explícito y el fallback esté desactivado o justificado.

**Procedimiento operativo.**
1. Cerrar Sprint 16 authoring.
2. Registrar SceneContext.
3. Migrar Build Profile/settings.
4. Ejecutar build post-integración.

**Evidencia mínima.** Capturas, PlayMode, Golden Path, Player.log y build record.

# 94. Save paths

Los saves deben vivir en una ruta estable por usuario y soportar actualización, backup y eventual Cloud.

**Estado actual.** La persistencia local está estable y usa schemas 1/2, pero Steam Cloud no está configurado.

**Regla de aprobación.** Documentar ruta exacta Windows, archivos incluidos/excluidos, tamaño, conflictos y migración.

**Procedimiento operativo.**
1. Inventariar archivos.
2. Probar perfiles de usuario.
3. Documentar backup/restore.
4. Diseñar Cloud después.

**Evidencia mínima.** Save manifest y pruebas clean machine.

# 95. Steam Cloud

Steam Cloud puede configurarse mediante Auto-Cloud o API; no debe activarse hasta conocer rutas y conflictos.

**Estado actual.** No está integrado.

**Regla de aprobación.** Para este proyecto, evaluar Auto-Cloud primero por simplicidad. Probar multi-PC, offline, conflictos, backup y no sincronizar logs/config sensible.

**Procedimiento operativo.**
1. Definir archivos.
2. Configurar roots/patterns.
3. Probar publicación de cambios y client refresh.
4. Documentar recuperación.

**Evidencia mínima.** Cloud test matrix y configuración exportada.

# 96. Achievements

Los logros pueden mejorar objetivos, pero añaden diseño, localización, iconos, API y QA.

**Estado actual.** No forman parte del alcance confirmado.

**Regla de aprobación.** No anunciarlos ni crear placeholders. Abrir después de producto estable mediante ADR y lista pequeña significativa.

**Procedimiento operativo.**
1. Mantener NOT OPEN.
2. Evaluar después de H6.
3. Definir IDs estables.
4. Probar unlock/reset/offline.

**Evidencia mínima.** ADR y implementation evidence si se abre.

# 97. Overlay y Steam API

El overlay y APIs requieren SDK/plugin compatible, inicialización segura y comportamiento fuera de Steam.

**Estado actual.** No hay integración ni paquete.

**Regla de aprobación.** No añadir una dependencia justo antes de H6. Cuando se abra, aislar en infrastructure y ofrecer fallback no-op.

**Procedimiento operativo.**
1. Seleccionar integración.
2. Auditar licencia/version.
3. Encapsular API.
4. Probar overlay, offline y no-Steam.

**Evidencia mínima.** ADR, package record y PlayMode/build tests.

# 98. Steam Input y mando

Steam Input se configura si el juego soporta mando y necesita acciones/glifos consistentes.

**Estado actual.** No está aprobado soporte de mando.

**Regla de aprobación.** Mantener fuera hasta cerrar navegación completa. Si se abre, definir action set, defaults, community configs y Deck.

**Procedimiento operativo.**
1. Auditar input.
2. Diseñar action sets.
3. Probar remapping.
4. Actualizar store declarations.

**Evidencia mínima.** Controller compliance matrix.

# 99. Family Sharing y features de plataforma

Features como Family Sharing, Remote Play, Workshop, leaderboards o cards deben revisarse individualmente.

**Estado actual.** Ninguna está implementada o confirmada.

**Regla de aprobación.** No marcarlas por defecto ni mencionarlas en marketing. Mantener una tabla `NOT OPEN` con condición de apertura.

**Procedimiento operativo.**
1. Inventariar features.
2. Evaluar valor/coste.
3. Abrir por ADR.
4. Probar y documentar.

**Evidencia mínima.** Feature decision register.

# 100. Nomenclatura de builds Steam

La versión interna, Unity bundle version, Git commit, Build ID propio y Steam BuildID deben poder correlacionarse.

**Estado actual.** Existe versión 0.0.17 y registros BLD-S00–S15/Build001/002, pero no Steam BuildID.

**Regla de aprobación.** No usar Steam BuildID como versión del producto ni perder la correlación.

**Procedimiento operativo.**
1. Definir display version.
2. Generar internal build ID.
3. Registrar commit/checksum.
4. Añadir Steam BuildID tras upload.

**Evidencia mínima.** Release manifest con todos los identificadores.

# 101. Versionado

El versionado debe expresar madurez sin confundir marketing con schemas de save.

**Estado actual.** La Build Guide prepara H6 candidate y release futuros.

**Regla de aprobación.** Mantener SemVer o convención aprobada, con build metadata separada; no incrementar versión para ocultar un rebuild idéntico.

**Procedimiento operativo.**
1. Definir release version.
2. Congelar antes de RC.
3. Registrar cambios.
4. Validar display/store consistency.

**Evidencia mínima.** Versioning record y release notes.

# 102. Manifest IDs y checksums

Steam genera manifests por depot; el proyecto además conserva SHA-256 del paquete local.

**Estado actual.** Los builds históricos tienen checksums, pero no manifests Steam.

**Regla de aprobación.** Registrar AppID, DepotID, ManifestID, Steam BuildID, branch, commit y SHA-256 en una única ficha.

**Procedimiento operativo.**
1. Capturar output SteamPipe.
2. Exportar manifest IDs.
3. Hash del staging/zip.
4. Firmar decisión.

**Evidencia mínima.** Steam build record completo.

# 103. Reproducibilidad

Una build publicable debe poder regenerarse desde commit, Unity version, packages, settings y scripts.

**Estado actual.** La baseline técnica está bien documentada, pero no hay BuildCommand completo ni Steam pipeline.

**Regla de aprobación.** No depender de pasos manuales ocultos. Documentar excepciones y preparar automatización gradual.

**Procedimiento operativo.**
1. Clean checkout.
2. Restaurar packages.
3. Ejecutar tests/build.
4. Comparar manifest.

**Evidencia mínima.** Reproduction report.

# 104. Clean checkout

El clean checkout detecta referencias locales, assets ignorados y configuración no versionada.

**Estado actual.** No hay prueba Steam-specific.

**Regla de aprobación.** Antes del RC, construir desde un clone limpio en una ruta nueva y usuario sin cache de proyecto.

**Procedimiento operativo.**
1. Clonar commit.
2. Restaurar Unity.
3. Ejecutar tests.
4. Build, stage y upload a QA.

**Evidencia mínima.** Log completo y hashes.

# 105. Upload a Steam

La subida se realiza a una branch privada y produce un BuildID.

**Estado actual.** No hay SDK ni cuenta.

**Regla de aprobación.** Nunca subir directamente a default como primer test. Revisar rutas absolutas, exclusiones y logs.

**Procedimiento operativo.**
1. Ejecutar SteamCMD/ContentBuilder.
2. Guardar output.
3. Comprobar depot size.
4. Asignar a `internal`/`qa`.

**Evidencia mínima.** Upload log y BuildID.

# 106. Promoción entre branches

Promover un build cambia quién lo recibe, sin alterar sus bytes.

**Estado actual.** No existe workflow.

**Regla de aprobación.** QA debe aprobar el mismo BuildID que se promueve; no reconstruir después de la aprobación salvo nueva ronda.

**Procedimiento operativo.**
1. Seleccionar BuildID.
2. Verificar branch actual.
3. Aprobar.
4. Promover y smoke.

**Evidencia mínima.** Promotion record y tester confirmation.

# 107. Rollback técnico

Rollback significa volver una branch al BuildID anterior conocido, no compilar deprisa una versión supuestamente equivalente.

**Estado actual.** La Build Guide exige conservar una candidata utilizable.

**Regla de aprobación.** Antes de release, probar el procedimiento y confirmar compatibilidad de saves hacia atrás o definir límites.

**Procedimiento operativo.**
1. Retener BuildID anterior.
2. Documentar trigger.
3. Probar cambio de branch.
4. Verificar save after rollback.

**Evidencia mínima.** Rollback drill y build previous-good.

# 108. Hotfix

Un hotfix reduce riesgo inmediato y debe tener scope mínimo, pruebas dirigidas y rollback.

**Estado actual.** No hay pipeline Steam.

**Regla de aprobación.** No mezclar refactor, contenido y corrección crítica. Versionar patch notes y conservar el RC anterior.

**Procedimiento operativo.**
1. Abrir incidente.
2. Crear branch/commit mínimo.
3. Test regresión.
4. Subir QA, aprobar y promover.

**Evidencia mínima.** Hotfix record, BuildID y communication.

# 109. Pruebas internas en Steam client

La build debe probarse instalada por Steam, no solo ejecutando el EXE local.

**Estado actual.** No es posible aún.

**Regla de aprobación.** La branch internal valida install, launch options, working directory, permissions, updates, overlay si aplica y saves.

**Procedimiento operativo.**
1. Instalar desde cero.
2. Ejecutar todos los entry points.
3. Actualizar a otra build.
4. Recoger logs.

**Evidencia mínima.** Steam client test report.

# 110. QA externa

Testers externos encuentran problemas de hardware, comprensión y entorno que el desarrollador no ve.

**Estado actual.** H6 todavía no tiene campaña externa post-integración.

**Regla de aprobación.** Usar Steam Playtest o branch cerrada solo con build legalmente autorizada para test, acuerdo/privacidad y preguntas concretas.

**Procedimiento operativo.**
1. Definir cohortes.
2. Preparar build/known issues.
3. Recoger feedback.
4. Triar y cerrar.

**Evidencia mínima.** Test report y consentimiento/keys.

# 111. Golden Path

El Golden Path cubre crear/cargar partida, operar tienda, comprar/recibir/reponer, atender, checkout, cerrar, guardar y recargar.

**Estado actual.** Existe como contrato interno, pendiente de StoreInitial final.

**Regla de aprobación.** Debe ejecutarse en la build Steam candidata exacta y en ambos idiomas declarados.

**Procedimiento operativo.**
1. Preparar precondiciones.
2. Grabar build ID.
3. Ejecutar sin cheats.
4. Revisar log y resultados.

**Evidencia mínima.** Caso firmado, vídeo/capturas y Player.log.

# 112. Campaña de siete días

La campaña detecta deriva de economía, saves, cola, stock y ciclo diario.

**Estado actual.** Está definida para H6, no completada sobre candidata Steam.

**Regla de aprobación.** Ejecutar sobre instalación Steam, con restart/update y ES/EN cuando sea razonable.

**Procedimiento operativo.**
1. Registrar métricas diarias.
2. Guardar/cargar.
3. Provocar errores recuperables.
4. Cerrar con conciliación.

**Evidencia mínima.** Daily logs, saves y resumen.

# 113. Save/load y migración

Una actualización no debe destruir progreso ni mezclar IDs de las dos capas de catálogo.

**Estado actual.** Los schemas son estables, pero existe deuda de unificación de productos y Cloud no está activo.

**Regla de aprobación.** No publicar update incompatible sin migración, aviso o decisión explícita. Probar saves de versiones anteriores.

**Procedimiento operativo.**
1. Crear corpus de saves.
2. Actualizar build.
3. Migrar/validar.
4. Probar rollback y backup.

**Evidencia mínima.** Compatibility matrix.

# 114. Instalación limpia

La instalación limpia valida que el depot contiene todo y no depende de archivos residuales.

**Estado actual.** No hay prueba Steam.

**Regla de aprobación.** Usar máquina/usuario limpio, desinstalar, borrar residuos permitidos y reinstalar desde branch.

**Procedimiento operativo.**
1. Verificar descarga.
2. Iniciar.
3. Crear save.
4. Revisar logs y paths.

**Evidencia mínima.** Install report y manifest.

# 115. Actualización

La actualización debe preservar saves/config y descargar solo lo necesario.

**Estado actual.** No hay SteamPipe delta test.

**Regla de aprobación.** Probar al menos previous-good → RC, RC → hotfix candidate y fallback si procede.

**Procedimiento operativo.**
1. Instalar build A.
2. Crear estado.
3. Promover B.
4. Verificar integridad.

**Evidencia mínima.** Update matrix y bytes/duration informativos.

# 116. Desinstalación y residuos

Desinstalar elimina archivos del depot, pero saves/config pueden persistir por diseño.

**Estado actual.** No hay política pública.

**Regla de aprobación.** Documentar qué se conserva, cómo borrar datos y no eliminar archivos ajenos.

**Procedimiento operativo.**
1. Desinstalar desde Steam.
2. Inspeccionar carpetas.
3. Reinstalar.
4. Probar reset manual.

**Evidencia mínima.** Uninstall policy/FAQ.

# 117. Player.log

El log es evidencia para warnings, excepciones y rutas. En Windows se espera bajo LocalLow/VRM Games/Cartridge & Cloud.

**Estado actual.** La Build Guide exige revisión para S16/S17/H6.

**Regla de aprobación.** La candidata Steam no se aprueba con excepciones no explicadas, missing assets/tables o spam recurrente.

**Procedimiento operativo.**
1. Recoger desde instalación Steam.
2. Revisar startup/Golden Path/shutdown.
3. Redactar datos personales si aplica.
4. Archivar con BuildID.

**Evidencia mínima.** Player.log hash y review checklist.

# 118. Crashes y diagnóstico

Los crashes requieren reproducibilidad, dumps/logs y soporte. Un servicio remoto añade privacidad y coste.

**Estado actual.** No existe crash reporting externo.

**Regla de aprobación.** H6 puede usar logs locales. Antes de lanzamiento, decidir si la tasa de crash se puede medir de forma responsable.

**Procedimiento operativo.**
1. Definir crash workflow.
2. Probar cierres forzados.
3. Preparar símbolos seguros.
4. Evaluar proveedor con legal/privacy.

**Evidencia mínima.** Crash runbook y test.

# 119. Rendimiento

La store page crea expectativas de rendimiento ligadas a requisitos.

**Estado actual.** No hay campaña post-StoreInitial ni hardware matrix.

**Regla de aprobación.** Medir build Release instalada por Steam en escenas representativas, no Editor. Registrar FPS, frame time, RAM, carga y stutter.

**Procedimiento operativo.**
1. Seleccionar hardware.
2. Definir escenarios.
3. Capturar métricas.
4. Ajustar requisitos/quality.

**Evidencia mínima.** Performance report.

# 120. Antivirus y falsos positivos

Builds nuevas o sin firma pueden generar avisos. No se debe instruir a desactivar seguridad como solución general.

**Estado actual.** No hay firma ni distribución.

**Regla de aprobación.** Probar en sistemas limpios, analizar binarios y mantener build reproducible. Evaluar firma si el riesgo/escala lo justifica.

**Procedimiento operativo.**
1. Escanear release.
2. Registrar hashes.
3. Investigar detecciones.
4. Comunicar con transparencia.

**Evidencia mínima.** Scan results y incident response.

# 121. QA Steam Deck/Proton

La compatibilidad requiere más que arrancar: input, texto, suspensión, teclado virtual y performance.

**Estado actual.** No probada.

**Regla de aprobación.** Ejecutar después de estabilizar Windows. No bloquear la primera release por Deck salvo decisión comercial, pero no hacer claim falso.

**Procedimiento operativo.**
1. Instalar build Steam.
2. Probar loop completo.
3. Revisar UI 1280×800.
4. Registrar problemas/review.

**Evidencia mínima.** Deck checklist y clips.

# 122. QA de localización de la store

Store copy y build deben coincidir en idiomas y terminología.

**Estado actual.** ES/EN no implementados.

**Regla de aprobación.** Revisar short description, About, screenshots con texto, requirements, credits, FAQ y announcements en cada idioma.

**Procedimiento operativo.**
1. Congelar source.
2. Traducir.
3. Preview.
4. LQA por build.

**Evidencia mínima.** Store LQA report.

# 123. Gate legal de candidata

La build no puede salir si audio, fuente, notices, marcas, créditos o privacidad están bloqueados.

**Estado actual.** El registro 23 marca 0 builds elegibles, 7 deudas S1 y 9 riesgos High/Critical.

**Regla de aprobación.** El gate legal es independiente del PASS técnico y requiere decisión manual.

**Procedimiento operativo.**
1. Cerrar registros.
2. Generar notices/créditos.
3. Verificar title/studio.
4. Firmar autorización por BuildID.

**Evidencia mínima.** Legal release certificate interno.

# 124. Release Candidate

Un RC es una build potencialmente publicable, congelada salvo defectos que bloqueen.

**Estado actual.** H6 es un gate interno y no equivale automáticamente a RC Steam.

**Regla de aprobación.** Crear RC solo tras StoreInitial, S17, H6 y onboarding. La misma build se somete a Valve salvo corrección posterior documentada.

**Procedimiento operativo.**
1. Congelar scope/content.
2. Ejecutar suites y manual.
3. Subir a `release_candidate`.
4. Completar legal/store review.

**Evidencia mínima.** RC manifest, BuildID, tests y decisión.

# 125. Review de store page por Valve

La presencia de tienda necesita revisión antes de publicación.

**Estado actual.** No se ha preparado ni enviado.

**Regla de aprobación.** Enviar con margen. La documentación actual indica 3–5 días laborables típicos y recomienda al menos 7 días laborables.

**Procedimiento operativo.**
1. Completar checklist.
2. Preview ES/EN.
3. Mark ready for review.
4. Corregir feedback y re-enviar.

**Evidencia mínima.** Estado de review y tickets.

# 126. Review de build por Valve

La build también se revisa para comprobar funcionamiento y configuración.

**Estado actual.** No existe AppID/build Steam.

**Regla de aprobación.** La store page debe enviarse primero según el flujo actual. Proporcionar instrucciones claras si el reviewer necesita pasos especiales.

**Procedimiento operativo.**
1. Subir RC.
2. Configurar launch/default depots.
3. Completar build checklist.
4. Responder feedback.

**Evidencia mínima.** Review status y build exacta.

# 127. Planificación de tiempos de review

El calendario debe contemplar revisión, correcciones y reenvío, no solo el tiempo típico.

**Estado actual.** No hay fecha.

**Regla de aprobación.** Reservar al menos siete días laborables por revisión y margen adicional para cambios; no enviar en el último momento.

**Procedimiento operativo.**
1. Crear deadline interno.
2. Añadir buffer.
3. No fijar vacaciones/ausencias críticas.
4. Definir decisión de retraso.

**Evidencia mínima.** Calendar with review windows.

# 128. Publicación de Coming Soon

Tras aprobación de store page, se publica Coming Soon y comienza el periodo mínimo.

**Estado actual.** No iniciado.

**Regla de aprobación.** Registrar exactamente qué assets/copy se hicieron públicos; publicar mueve contenido a servidores públicos y exige confidencialidad adecuada.

**Procedimiento operativo.**
1. Realizar preview final.
2. Publicar con permisos correctos.
3. Verificar URL e idiomas.
4. Registrar timestamp.

**Evidencia mínima.** Store snapshot y URL.

# 129. Readiness final

Readiness combina producto, plataforma, comercial, legal y soporte.

**Estado actual.** Actualmente bloqueado.

**Regla de aprobación.** No-go automático ante S0/S1, build review pendiente, store review pendiente, menos de dos semanas Coming Soon, legal bloqueado o rollback no probado.

**Procedimiento operativo.**
1. Reunir comité/owner.
2. Revisar checklist.
3. Firmar go/no-go.
4. Comunicar decisión.

**Evidencia mínima.** Acta de release readiness.

# 130. Día de lanzamiento

El lanzamiento es una operación controlada con ventanas, responsables y comprobaciones.

**Estado actual.** No hay calendario.

**Regla de aprobación.** No reconstruir el build ese día. Confirmar default BuildID, packages, price, idiomas, support y rollback antes de pulsar release.

**Procedimiento operativo.**
1. Preflight.
2. Release.
3. Compra/instalación smoke.
4. Monitorizar store, reviews, support y crashes.

**Evidencia mínima.** Launch log con timestamps.

# 131. Monitorización inicial

Las primeras horas/días requieren vigilancia técnica y de soporte, sin obsesionarse con métricas vanidosas.

**Estado actual.** No hay dashboard.

**Regla de aprobación.** Priorizar incapacidad de iniciar, pérdida de saves, checkout roto, crashes, problemas de compra/depots e idioma incorrecto.

**Procedimiento operativo.**
1. Definir canales.
2. Revisar soporte/reviews/logs.
3. Clasificar incidentes.
4. Decidir hotfix/rollback.

**Evidencia mínima.** Incident dashboard and shifts.

# 132. Reviews de usuarios

Las reviews reflejan producto, valor, prácticas y experiencia de comunidad.

**Estado actual.** No aplicable aún.

**Regla de aprobación.** No presionar, manipular ni discutir defensivamente. Analizar temas, responder a problemas verificables y mejorar el producto.

**Procedimiento operativo.**
1. Monitorizar tendencias.
2. Separar bugs/preferencias.
3. Responder con hechos.
4. Escalar anomalías a Valve si procede.

**Evidencia mínima.** Review theme log y acciones.

# 133. Soporte postlanzamiento

El soporte debe continuar después de la campaña inicial.

**Estado actual.** Capacidad no definida.

**Regla de aprobación.** Establecer horarios realistas, plantillas, prioridades y límites. Documentar soluciones en FAQ y parches.

**Procedimiento operativo.**
1. Triage diario inicial.
2. SLA interno.
3. Escalado técnico.
4. Cierre/comunicación.

**Evidencia mínima.** Support metrics y backlog.

# 134. Parches

Los parches siguen la misma disciplina de branch, QA, notices, localization y rollback.

**Estado actual.** No hay pipeline Steam.

**Regla de aprobación.** No convertir default en entorno de prueba. Publicar patch notes claras y probar upgrade desde la versión live.

**Procedimiento operativo.**
1. Crear candidate.
2. QA branch.
3. Legal/localization diff.
4. Promover y monitorizar.

**Evidencia mínima.** Patch build record and notes.

# 135. Comunicación de incidencias

Una incidencia importante necesita mensaje rápido, exacto y actualizado.

**Estado actual.** No hay plantillas.

**Regla de aprobación.** Reconocer impacto, ofrecer workaround seguro, evitar culpar al usuario y publicar resolución/postmortem cuando corresponda.

**Procedimiento operativo.**
1. Clasificar severidad.
2. Redactar mensaje inicial.
3. Actualizar hitos.
4. Cerrar con versión/BuildID.

**Evidencia mínima.** Incident communication log.

# 136. Rollback de producción

Rollback se ejecuta cuando el riesgo de mantener live supera el de volver al previous-good.

**Estado actual.** No está probado.

**Regla de aprobación.** Definir triggers: crash generalizado, save corruption, bloqueo de inicio/compra o defecto S0/S1 no mitigable. Considerar compatibilidad de saves.

**Procedimiento operativo.**
1. Declarar incidente.
2. Seleccionar BuildID anterior.
3. Promover.
4. Smoke y comunicar.

**Evidencia mínima.** Rollback record and customer note.

# 137. Métricas de publicación

Las métricas deben informar decisiones sin invadir privacidad: store visits, wishlists, conversion, refunds, support volume, crash rate, playtime and review themes.

**Estado actual.** No hay Steam data ni telemetría.

**Regla de aprobación.** Usar datos agregados de Steamworks y soporte. No añadir analytics in-game sin propósito, proveedor y política.

**Procedimiento operativo.**
1. Definir preguntas.
2. Seleccionar métricas.
3. Asignar owner/cadence.
4. Evitar métricas sin acción.

**Evidencia mínima.** Metric dictionary and reviews.

# 138. Retrospectiva

Después de Coming Soon, review, Playtest, launch y hotfix se registra qué funcionó y qué cambió.

**Estado actual.** No existe historial operativo Steam.

**Regla de aprobación.** Las retrospectivas generan acciones concretas, no solo narrativa. Actualizan este plan y el Post-Launch Plan.

**Procedimiento operativo.**
1. Recoger datos.
2. Identificar causas.
3. Asignar acciones.
4. Cerrar o aceptar deuda.

**Evidencia mínima.** Retrospective record.

# 139. Registro de riesgos de publicación

Los riesgos principales son título/estudio sin clearance, audio/font/notices incompletos, StoreInitial no integrado, ES/EN ausente, requisitos no medidos y falta de soporte/rollback.

**Estado actual.** Todos siguen abiertos en distinto grado.

**Regla de aprobación.** Mantener probabilidad, impacto, exposición, trigger, mitigación, owner y gate. No reducir severidad para cumplir una fecha.

**Procedimiento operativo.**
1. Sincronizar con registro 23.
2. Revisar por hito.
3. Añadir riesgos Steam actuales.
4. Escalar no-go.

**Evidencia mínima.** Risk register firmado.

| Riesgo | Estado | Gate |
|---|---|---|
| Título/VRM Games sin clearance | OPEN | Pre-Steam |
| Audio por clip | BLOCKING | H6/RC |
| Fuente de producción | BLOCKING | H6/RC |
| THIRD_PARTY_NOTICES | BLOCKING | RC |
| StoreInitial/runtime | BLOCKING | Sprint 16 |
| Localización ES/EN | BLOCKING | Sprint 17/H6 |
| Store assets/copy | NOT STARTED | Pre-Coming Soon |
| SteamPipe/rollback | NOT STARTED | RC |

# 140. Work packages de publicación

Los work packages separan onboarding, store, build, legal, marketing y soporte para evitar una tarea monolítica “publicar en Steam”.

**Estado actual.** No están abiertos formalmente.

**Regla de aprobación.** Cada paquete tiene owner, input, output, gate, dependencias, QA y rollback.

**Procedimiento operativo.**
1. Crear backlog tras H6.
2. Ordenar por dependencias.
3. No abrir features opcionales.
4. Vincular a evidencia.

**Evidencia mínima.** Backlog aprobado.

| ID | Paquete | Resultado |
|---|---|---|
| STM-WP-01 | Clearance de título/estudio | nombres aprobados |
| STM-WP-02 | Onboarding/Steam Direct | partner y AppID |
| STM-WP-03 | Store copy ES/EN | copy deck aprobado |
| STM-WP-04 | Cápsulas/library assets | exports validados |
| STM-WP-05 | Screenshots/tráiler | material real y legal |
| STM-WP-06 | SteamPipe/depot | upload a internal branch |
| STM-WP-07 | Branches/permissions | matriz operativa |
| STM-WP-08 | QA Steam client | install/update/save pass |
| STM-WP-09 | Cloud decision | Auto-Cloud/API o deferred |
| STM-WP-10 | Playtest/demo decision | ADR y app separada si aplica |
| STM-WP-11 | Legal/notices/credits | gate público cerrado |
| STM-WP-12 | Reviews Valve | store/build approved |
| STM-WP-13 | Coming Soon campaign | página pública y métricas |
| STM-WP-14 | RC/rollback | release-ready BuildID |
| STM-WP-15 | Launch/support | operación y incident response |

# 141. Impacto sobre Sprint 16

Sprint 16 debe cerrar StoreInitial y presentación representativa; no debe incorporar Steamworks.

**Estado actual.** S16 está en progreso con escena/build pendientes.

**Regla de aprobación.** Solo se permiten tareas de compatibilidad que eviten deuda futura: perfil Release limpio, capturas de evidencia, assets legales y exclusión de TestLab.

**Procedimiento operativo.**
1. Finalizar authoring.
2. Conectar runtime.
3. Build post-integración.
4. Registrar materiales candidatos sin publicarlos.

**Evidencia mínima.** S16 closure record.

# 142. Impacto sobre Sprint 17

Sprint 17 estabiliza input, localización, catálogos, UI, audio, QA y deuda necesaria para H6.

**Estado actual.** Está pendiente.

**Regla de aprobación.** No convertirlo en sprint de onboarding. Debe dejar el producto listo para evaluar publicación, no ejecutar campaña comercial completa.

**Procedimiento operativo.**
1. Cerrar click-through/foco.
2. Implementar ES/EN.
3. Unificar catálogo.
4. Cerrar audio/font/legal según H6.

**Evidencia mínima.** S17 acceptance.

# 143. Relación con H6

H6 demuestra que la vertical slice es representativa, estable y auditable.

**Estado actual.** H6 está pendiente.

**Regla de aprobación.** H6 es prerequisito para iniciar la fase Steam, pero no implica AppID, store review, precio ni lanzamiento.

**Procedimiento operativo.**
1. Ejecutar Golden Path/7 días.
2. Build externa y Player.log.
3. Cero S0/S1.
4. Decision PASS/FAIL.

**Evidencia mínima.** H6 certificate and evidence pack.

# 144. Gate Pre-Steam

Después de H6, un gate separado decide si el proyecto entra en Steamworks.

**Estado actual.** No existe todavía.

**Regla de aprobación.** Exige producto sostenible, título/estudio encaminados, presupuesto, owner, soporte, copy/materiales internos y calendario sin fecha pública.

**Procedimiento operativo.**
1. Revisar H6.
2. Revisar legal/budget.
3. Evaluar full/EA/demo/playtest.
4. Autorizar onboarding.

**Evidencia mínima.** Pre-Steam decision record.

# 145. Criterios no-go

Un no-go protege el producto y la reputación; no es fracaso administrativo.

**Estado actual.** Actualmente se cumplen múltiples no-go.

**Regla de aprobación.** No publicar o anunciar fecha con S0/S1, legal blocked, title unclear, build review/store review pendientes, menos de dos semanas Coming Soon, rollback no probado, soporte inexistente o claims falsos.

**Procedimiento operativo.**
1. Ejecutar checklist.
2. Documentar bloqueadores.
3. Replanificar.
4. Comunicar solo cuando sea necesario.

**Evidencia mínima.** No-go record with owners and next review.

# 146. Paquete de evidencia

Cada candidata Steam debe poder auditarse sin memoria oral.

**Estado actual.** La infraestructura interna ya usa hashes, logs y records; falta Steam metadata.

**Regla de aprobación.** El paquete debe contener source commit, version, internal BuildID, Steam BuildID, depot manifests, checksum, store snapshot, reviews, QA, legal, localization, known issues, rollback and approval.

**Procedimiento operativo.**
1. Crear carpeta por candidate.
2. Exportar portal evidence.
3. Hash files.
4. Archivar securely.

**Evidencia mínima.** Evidence index and retention policy.

```text
SteamReleaseEvidence/<ReleaseID>/
├── 00_Release_Manifest.md
├── 01_Source_Commit_and_Packages.txt
├── 02_Unity_Build_Report/
├── 03_SteamPipe_Scripts_and_Log/
├── 04_Depot_Manifest_IDs.txt
├── 05_QA_GoldenPath_SevenDays/
├── 06_PlayerLog_Crash_Performance/
├── 07_Store_Page_Snapshot_ES_EN/
├── 08_Valve_Review_Status/
├── 09_Legal_Credits_Notices/
├── 10_Known_Issues_and_Support/
├── 11_Rollback_Drill/
└── 12_Go_NoGo_Decision.md
```

# 147. Checklist maestro de publicación

El checklist maestro integra todas las disciplinas y se ejecuta por release ID.

**Estado actual.** Todas las áreas de Steam están pendientes.

**Regla de aprobación.** Un checkbox necesita evidencia, no una impresión. Los ítems no aplicables incluyen razón y aprobador.

**Procedimiento operativo.**
1. Completar identidad/onboarding.
2. Completar store/materiales.
3. Completar build/QA.
4. Completar legal/support/launch.

**Evidencia mínima.** Checklist firmado.

- [ ] Titular, impuestos, banco y seguridad completos.
- [ ] Título y developer/publisher cleared.
- [ ] AppID, fee y espera mínima registrados.
- [ ] Store page ES/EN aprobada por Valve.
- [ ] Coming Soon pública durante el mínimo vigente.
- [ ] Build RC exacta aprobada por Valve.
- [ ] StoreInitial activo; TestLab y NOT OPEN excluidos.
- [ ] Requisitos, input, idiomas y features veraces.
- [ ] Screenshots/tráiler/cápsulas legales y reales.
- [ ] SteamPipe, branches, manifests y rollback probados.
- [ ] Golden Path, siete días, clean install/update/uninstall PASS.
- [ ] Audio, fuente, créditos, notices y privacidad cerrados.
- [ ] Soporte, FAQ, incidentes y patch process preparados.
- [ ] Cero S0/S1 y decisión manual GO.

# 148. Glosario

Normaliza términos para producción y evita confundir build interna, Steam build, manifest o release.

**Estado actual.** Los documentos previos usan BuildID en varios sentidos.

**Regla de aprobación.** Usar prefijos y contexto explícito.

**Procedimiento operativo.**
1. Mantener glosario.
2. Actualizar templates.
3. No usar “publicado” para una branch privada.
4. Distinguir review de aprobación interna.

**Evidencia mínima.** Glosario en documentos y tickets.

| Término | Definición operativa |
|---|---|
| AppID | Identificador Steam de una aplicación |
| Depot | Conjunto de archivos distribuibles por plataforma/variante |
| ManifestID | Versión concreta del contenido de un depot |
| Steam BuildID | Agrupación de manifests subida a Steam |
| Internal Build ID | Identificador propio del proyecto |
| Branch | Canal que apunta a un BuildID |
| Package | Derecho de adquisición que incluye apps/depots |
| Coming Soon | Presencia pública previa al lanzamiento |
| RC | Build congelada candidata a publicación |
| H6 | Gate interno del vertical slice, no release Steam |
| Rollback | Reasignación a previous-good BuildID |
| Playtest | AppID separado para pruebas controladas |

# 149. Historial, fuentes y cierre

Cierra el documento con la genealogía, el inventario de fuentes y la regla de actualización.

**Estado actual.** Este plan se apoya en 177 fuentes históricas/operativas seleccionadas, 27 documentos consolidados y 24 fuentes oficiales Steamworks verificadas.

**Regla de aprobación.** La siguiente revisión obligatoria ocurre al cerrar H6, antes de pagar Steam Direct, antes de publicar Coming Soon, antes de enviar reviews y antes de release.

**Procedimiento operativo.**
1. Recalcular hashes.
2. Actualizar fuentes oficiales.
3. Registrar cambios de estado.
4. Repetir auditoría global al completar la jerarquía.

**Evidencia mínima.** Documento final, SHA-256 y lista de fuentes.

## 149.1. Historial de esta consolidación

| Versión | Fecha | Estado | Cambio |
|---|---|---|---|
| v0.3 | 2026-06-21/22 | HISTORICAL | gates de entrada, materiales, tags, branches y no-promesas |
| v0.4 | 2026-06-25 | HISTORICAL | estado tras Sprint 5 y pipeline futuro |
| v0.5 | 2026-07-01 | HISTORICAL | S0–15 PASS, S16 en curso, Steam diferido |
| 1.0 consolidado | 2026-07-01 | CURRENT PLAN | integración de estado real, legal, localización, Steamworks actual y operación |

## 149.2. Fuentes consolidadas actuales

| Documento | SHA-256 |
|---|---|
| `00_Enfoque_y_Alcance.md` | `63dd6f0f1b9c2c80be2aec55718d18d9d031ce4acb14f677769d6e69ceaad21f` |
| `01_Game_Design_Document.md` | `a9cd9e3203abc5269c25c59dc7f626b0771df4ec2d65e781109ff146bceb2177` |
| `02_Vertical_Slice_Specification.md` | `75893f130116e88a08b8da5590dd81876a234581081eafaa8d08374c1a60513f` |
| `02_Vertical_Slice_Specification.preview.md` | `75893f130116e88a08b8da5590dd81876a234581081eafaa8d08374c1a60513f` |
| `03_Technical_Design_Document.draft.md` | `66264703c5ff4959d03093690e9845a98ec0e5025e685b9a639ac8cbb616cd12` |
| `03_Technical_Design_Document.md` | `66264703c5ff4959d03093690e9845a98ec0e5025e685b9a639ac8cbb616cd12` |
| `04_Modelo_de_Datos.draft.md` | `d851a72b55742c2c704cc7c002929bc5894e7142511c6af843440bb193fb19d4` |
| `04_Modelo_de_Datos.md` | `d851a72b55742c2c704cc7c002929bc5894e7142511c6af843440bb193fb19d4` |
| `05_UX_Flow.md` | `e08da5ff67fb073a8b950babe325ae58ddb2e121513dc2f553acd4f2c14b867e` |
| `06_Production_Roadmap_y_Sprint_Plan.md` | `d00b156c0ad1c66069278119c55f76c738a577a3e2835f770208d1a5f2faadff` |
| `07_QA_Testing_Plan.md` | `bc9c05a3737e345546ef16e5390e034295ef35ad66c31647852aec81b4e7affe` |
| `08_QA_Testing_Matrix.xlsx` | `233e57f01d2468fe13c136c8fcd3ee75b39bd3de6530349626ea98d4ff254214` |
| `09_CSharp_Coding_Standards.md` | `3b734d6cd49f31c09c10bb4941625e143f6c634e3fb50c157f0720e73b1c2a68` |
| `10_Unity_Project_Setup_Guide.md` | `31430bb0544edd9577323aa0009d6ca3df8b9fbcf6c8a150fc33df69b9f963a4` |
| `11_Build_y_Versioning_Guide.md` | `38a16a0f97505c0b405509dd3c7e3d1b50e77b89fbe3f4cb1931d3e04f602215` |
| `12_Excel_Maestro_de_Produccion.xlsx` | `405376cb64f49b34f1f842f3840c12654e9f08e2ec4534b5c149fa811e87dd83` |
| `13_Trazabilidad_y_Control_de_Cambios.xlsx` | `f674e21d1a3a1f0970a3d339f26f2a2b51a19b5c257c24b9d3fa4118a5cf50eb` |
| `14_Project_Binder_Indice_Maestro.md` | `0c24dcd47e0d44793e75e764e3fc2c146e5b1fea04d887d43feb0df558711559` |
| `15_Guia_Maestra.md` | `879bc1925cccf77e10df6c104302f3afc72f71e8c58dd85ae4f06e84e6f8b624` |
| `16_Auditoria_Global_de_Coherencia.md` | `4a62376b8aee623be40957ae0511d2dcab2919947f3abb2cef3502b9bb60863e` |
| `17_Art_Bible.md` | `095c6cc42223a972c7faa68435595fc7c22f839fabc62b2038371a0ef6c14239` |
| `18_Audio_Bible.md` | `c2eabe93a7c26f70e1d4d77fb0ddcaa97e2cdd1e70247e637b9db5bd2f07ce92` |
| `19_UI_Style_Guide.md` | `b34ca4db1eba7a536466b6a9a105d8d92396ade2108aa7dd99fa7d5d4eaebc1a` |
| `20_Economy_and_Balance_Specification.md` | `908c70772ed35eecdb05c3036c6901523a4caf2c3e708ca8f049c2a160ef2aac` |
| `21_Initial_Content_Catalog.xlsx` | `9047299f60cfbd368fc4a2c8bee8d0ff4132dc5642363a9a67d1523559eb9ed3` |
| `22_Localization_Plan.md` | `184a53b2813bc1327a6913e6e88ea4c81142da1ee4ddeff88f75467c784f1ff0` |
| `23_Legal_Credits_and_Licenses_Register.xlsx` | `f543b3b5de8900f405910fac5d72697045f8c4969d98fce4c1cf4f12e532b2f1` |

## 149.3. Fuentes históricas y operativas seleccionadas

| Baseline | Tipo | Fuente | SHA-256 |
|---|---|---|---|
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/00_Project_Governance/Cartridge_And_Cloud_Package_Manifest_v0.1.pdf` | `4aed1b174d2405952bd79800726f58314e2bf96842da2b233cf005db84689f83` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/01_Design/Cartridge_And_Cloud_GDD_v0.4_PCSteam.pdf` | `9d07c1cbfde60aa02403129f5b0b2b36852cfb514123e836d8c8e81ee7fa837d` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/01_Design/Cartridge_And_Cloud_Vertical_Slice_Specification_v0.1.pdf` | `86b0a49dac832eda01fa751b5a9b720610868ba389093d2a8784222d01326ac3` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/02_Technical/Cartridge_And_Cloud_Build_Versioning_Guide_v0.3.pdf` | `81e38d9861f5caee0d4448c0aff214e0ffc922e82e2491f8b37c7f37719bd97a` |
| 0.3 | XLSX | `Documentation/00_Official_Baseline/v0.3/04_QA_Production/Cartridge_And_Cloud_QA_Testing_Matrix_v0.3.xlsx` | `f7d48e93b2ca3cc8e86b1dfc2164126d923af38ba0c76cb4dde566e6bc4da05d` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/04_QA_Production/Cartridge_And_Cloud_QA_Testing_Plan_v0.3.pdf` | `2d71d4dd720598429744bc6cf959162a8915301ad69171bc6816168e47f75709` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/05_Business_Release/Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.3.pdf` | `e358622a237b57cdcd65fb380460d70eafa0ef1085b43bf5e8fb7dff3ee734f0` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/05_Business_Release/Cartridge_And_Cloud_Localization_Plan_v0.3.pdf` | `461add7f8bf216439351ce4afe63b573de50e8bae3f9e270992917f616d82ad8` |
| 0.3 | PDF | `Documentation/00_Official_Baseline/v0.3/05_Business_Release/Cartridge_And_Cloud_Steam_Publishing_Plan_v0.3.pdf` | `10ff2852e8621c210bb578f63181382665ccbedeba3d62bfdce7eb4cb4e53472` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Build_Versioning_Guide_v0.3.md` | `fcc0988f428b45303c4cc2f5a681cbed9970475667bb6a37fa97a36529dc3705` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_GDD_v0.4_PCSteam.md` | `8939cf748467b18cee2e351b5eac309fe108b7fe02cd6da59a96230e9dd125fe` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.3.md` | `ffc5bc6af65b27ede021acfdef2dbc5f20f680560e0be1a872586ab146a2f101` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Localization_Plan_v0.3.md` | `f48dcd0d33e717b3e65734a9f87677d3b72edb9e67b54ff630720ecc46686349` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Package_Manifest_v0.1.md` | `58b7f326720277d2b490784a8091cff34f3182ac4fff57796e70d7817d9a4bc1` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_QA_Testing_Plan_v0.3.md` | `517a90d50868dd91010be4eaaca335326b63e8d370613c82a5b5664753ab3891` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Steam_Publishing_Plan_v0.3.md` | `1a004be75991bdcbbaf457a57da123e3297fce15a38c591e3f589e11e277dbd5` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Vertical_Slice_Specification_v0.1.md` | `69792ba4790022b23d631d437e043d777844f775b94d6552bc33bc682b876bb0` |
| 0.3 | MD | `Documentation/00_Official_Baseline/v0.3/PACKAGE_MANIFEST.md` | `7fb1bdbd0b11a372ccf4ad87efea6f88326188a21a8f4760680b0e49cd928783` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/00_Project_Governance/Cartridge_And_Cloud_Package_Manifest_v0.2.pdf` | `2fb3d34140d531bb28015e95ebed32e69d5e38f40fd2e591f99af99304035b5e` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/01_Design/Cartridge_And_Cloud_GDD_v0.4_PCSteam.pdf` | `daf3d2376783c00533ce932e3da58dddd77b65fab1efa2aa7a141241dc125069` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/01_Design/Cartridge_And_Cloud_Vertical_Slice_Specification_v0.2.pdf` | `d2505a3ba89c696248ddad9d0e3c4c7516eb0cf93b124111f2a4e3c6291bc46e` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/02_Technical/Cartridge_And_Cloud_Build_Versioning_Guide_v0.4.pdf` | `b26821c7646a0648c9bca240688bc7e3f82edd0c5ff853ac115e88ee3a742c4c` |
| 0.4 | XLSX | `Documentation/00_Official_Baseline/v0.4/04_QA_Production/Cartridge_And_Cloud_QA_Testing_Matrix_v0.4.xlsx` | `80d79fa25df8c4dfaf0e91ff3f0e519ffcd283d802cf9e418e06fa88fa01d4d4` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/04_QA_Production/Cartridge_And_Cloud_QA_Testing_Plan_v0.4.pdf` | `047a7bdd0a0b1ed6f9dec50aacc429bf1f18a2ba874fca0a28a270cc61bbfa45` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/05_Business_Release/Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.3.pdf` | `e4105813b235d24b75edf539eae505342f58f1f26a4b087e870dcbda9d63f558` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/05_Business_Release/Cartridge_And_Cloud_Localization_Plan_v0.3.pdf` | `fc6cb711af7107fecd82d8963f6461a6d2f00941e08d93ea535c16c8ce917c17` |
| 0.4 | PDF | `Documentation/00_Official_Baseline/v0.4/05_Business_Release/Cartridge_And_Cloud_Steam_Publishing_Plan_v0.3.pdf` | `b052a99f72f3b531b75b5950e5459fc6bd2a62c516be7dc1ad5e53d3a4684ec3` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/06_Development_Records/Governance/Project_Foundation_Release_Record.md` | `f789933c54973d329e078dd5a561b43f174333653a36aca00ce5254daaac0aab` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/06_Development_Records/Sprint_00/S0.10_Windows_Final_Build_Record.md` | `2bdddb6ad5bff23d564e9d179c0b31b5baa4dcf00624539571485725734231a2` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Build_Versioning_Guide_v0.4.md` | `6ac0c7072b5ac4b1844d82daffe1ec5f241dba369145e5c62152e290b8309acd` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_GDD_v0.4_PCSteam.md` | `88be82c3d3cfa4cb5965379e621b0aba522efa1217041cdc23a129ec4817485a` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.3.md` | `1a0e921f139b73179fe734eec452dcc3aa8c43860309390fe41ea1fb588fe81f` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Localization_Plan_v0.3.md` | `2158be2c46ec697e617419a9192bbaa8a0dd4435b5c650fca9295921b958cb3a` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Package_Manifest_v0.2.md` | `ccf1ff8da3ecfb69e1ee93a902e1d13d706d5e7ada9ac4a185515c8f93cd2b7a` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_QA_Testing_Plan_v0.4.md` | `623c83bd390d3fb1e7cb09e554f6a29f63db662c7ca3e296c65938c65969a724` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Steam_Publishing_Plan_v0.3.md` | `56979c2830951e4a82a7d01793cebe9a930df4a773245de2b7489d0e73cb0777` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Vertical_Slice_Specification_v0.2.md` | `884464e9599f0f1b66fdeff7a3c6422bf9d37b883817f480e0429ebdcd126b9d` |
| 0.4 | MD | `Documentation/00_Official_Baseline/v0.4/PACKAGE_MANIFEST.md` | `ccf1ff8da3ecfb69e1ee93a902e1d13d706d5e7ada9ac4a185515c8f93cd2b7a` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/00_Project_Governance/Cartridge_And_Cloud_Package_Manifest_v0.3.pdf` | `d74ae4fb1a2590813315e14d668ce2eca7a22081a7acdafbb5a55e07fb7a4f87` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/01_Design/Cartridge_And_Cloud_GDD_v0.5_PCSteam.pdf` | `7061f5ee8528d2fdffc2c219ff8a54f997705fe38cf3a713856caf519a997264` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/01_Design/Cartridge_And_Cloud_Vertical_Slice_Specification_v0.3.pdf` | `b256eb82b1c49fe2598797ec5ad74d4e4a22523ebbba62ef77ca27a777d231fc` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/02_Technical/Cartridge_And_Cloud_Build_Versioning_Guide_v0.5.pdf` | `34fc32adf9c6383d0bf5b79ed446cd4dd00c9518cbd67c6e9d7cd5623b236347` |
| 0.5 | XLSX | `Documentation/00_Official_Baseline/v0.5/04_QA_Production/Cartridge_And_Cloud_QA_Testing_Matrix_v0.5.xlsx` | `dbff9b69bb8edae19c2158fb4293aaf574a584a407cca672cf6f89dabae6690e` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/04_QA_Production/Cartridge_And_Cloud_QA_Testing_Plan_v0.5.pdf` | `0763d0d411746199eaa762acc995f591393eb39a1c8d88511c4dec488e56e74e` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/05_Business_Release/Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.4.pdf` | `d8fa9f63a454facc7e8d4e3fa8e50d1a95f0bb7a8f8bef06211e9d12accb5343` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/05_Business_Release/Cartridge_And_Cloud_Localization_Plan_v0.4.pdf` | `0194a14af0cb3121540d5ac73f3264c2d29037256953c15a74379690e1727e4c` |
| 0.5 | PDF | `Documentation/00_Official_Baseline/v0.5/05_Business_Release/Cartridge_And_Cloud_Steam_Publishing_Plan_v0.4.pdf` | `f0382da1359aa1339758e067dc58c2d921c46ca37aef870c74ecee0ef320a3b2` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Handoff/CURRENT_PROJECT_HANDOFF.md` | `9c251d0f3c14584f5da35a50ab3a5b62564e6f1f656d903fa2590eda74e2e396` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_01/QA/S1_Acceptance_Matrix.md` | `a5a4e0a192fe33559c582efd9ee4d53170e26af9e07db63aae1e22b1f5c66f49` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_02/QA/S2_Acceptance_Matrix.md` | `1c184fd5c44de6051e83614676a1571d4d49e7cd8b4d68c8ef85167b64e2692f` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_03/QA/S3.4_Build_Execution_Record.md` | `bdbcbe899c627a7abf1f302f7db42ce29de1f0809c8373a454e3bc070984ab7d` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_03/QA/S3_Acceptance_Matrix.md` | `dcd3a753ec03bf462ed61fee96f551716970f05a54702fcec5ebdc271bda15b7` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_04/QA/S4.4_Build_Execution_Record.md` | `6f662c5fe7ccef7a3080584798ccf51c720d4ade1dc50b1be2f8c4c107da529d` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_04/QA/S4_Acceptance_Matrix.md` | `30a86d51980351f806ef30aa6187ce90857bde474c163d47a6a3396b60522f89` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_05/QA/S5.4_Build_Execution_Record.md` | `83700192228af99fb4487bdf4cdd1332d13a194be7cd5fb197278013952f8fe1` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/06_Development_Records/Sprints/Sprint_05/QA/S5_Acceptance_Matrix.md` | `3c265e9509b133370788c1ea024e4de481f773b526a524f2ec880d60abe053b6` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Build_Versioning_Guide_v0.5.md` | `3df0fc5321a57c528f6386bcd922c2e930ba194736af94f064262c371bb63be6` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_GDD_v0.5_PCSteam.md` | `17284e1e8d67332d5a8e76e56d194b7e176225dfb49e2729caf6664a96c6aa09` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.4.md` | `18f78b69a56edee6f8783d32859d6e231d061422d24abd6feefdbf06fe68567b` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Localization_Plan_v0.4.md` | `01a3ddb286f975644985bc0c034eab75ef2ba4a283cd200f2e5dfd673732839f` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Package_Manifest_v0.3.md` | `7ae705ca39b1e39345748c208af4274cbbe9eaf887a249e5666bceefadf9d0d2` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_QA_Testing_Plan_v0.5.md` | `c19ab67f26aae4b44fd5c6d431fb163ddbfccefcbcc0ca68518826040b534f1c` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Steam_Publishing_Plan_v0.4.md` | `19664d4092ea89898c5bc37483a0b1aba6f473abbfa02c2794df8f2dd482058d` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Vertical_Slice_Specification_v0.3.md` | `17213417be3b7b2b95f8e3841d09a6679eee689bb62a35335e3f318135c15e46` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/CURRENT_PROJECT_HANDOFF.md` | `9c251d0f3c14584f5da35a50ab3a5b62564e6f1f656d903fa2590eda74e2e396` |
| 0.5 | MD | `Documentation/00_Official_Baseline/v0.5/PACKAGE_MANIFEST.md` | `8bb540cf6f6eb6e6f079cf55786ab3b53f4ec042bb1dd01147edb03222f306c5` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/00_Project_Governance/Cartridge_And_Cloud_Package_Manifest_v0.4.pdf` | `e2b87252fcc868ccf8c9bd6b7e9737260653ed2ae07d2eb86394cb503d64f48c` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/01_Design/Cartridge_And_Cloud_GDD_v0.6_PCSteam.pdf` | `f7002d60df83e5ed854264b4a802324de145e6389d3ec1de3b9f02ca4f619acd` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/01_Design/Cartridge_And_Cloud_Vertical_Slice_Specification_v0.4.pdf` | `326720d1a0d1302142034421279586f319992a9ecd3772c1922b6f5be085a692` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/02_Technical/Cartridge_And_Cloud_Build_Versioning_Guide_v0.6.pdf` | `758372f65f3e817c8cc192d557cac69f2ecb2b22d0705e8feeac9e5034cf0a02` |
| 0.6 | XLSX | `Documentation/00_Official_Baseline/v0.6/04_QA_Production/Cartridge_And_Cloud_QA_Testing_Matrix_v0.6.xlsx` | `96a6eb8272a2c9a42f718f71406a1b8d770e2d7cf0d96d2c3d0a0c418445d681` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/04_QA_Production/Cartridge_And_Cloud_QA_Testing_Plan_v0.6.pdf` | `ff451a919545374e6bd22a44efd7f8e65d3e9001741a2ddcac58b4f339052c04` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/05_Business_Release/Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.5.pdf` | `eee199263d089442dc5ea6228b10b562d7930163c707a7e90402e734bf94db51` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/05_Business_Release/Cartridge_And_Cloud_Localization_Plan_v0.5.pdf` | `0a7e40cfb0a39f46a303f93fd89ea65073e507955c15d89a67639ae1f445aaf8` |
| 0.6 | PDF | `Documentation/00_Official_Baseline/v0.6/05_Business_Release/Cartridge_And_Cloud_Steam_Publishing_Plan_v0.5.pdf` | `a21ea9abe67f1d825731d25b8608c8f7b67e5e1b437356e0e1ee3d97870d7728` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Governance/ADR-0035_StoreInitial_Manual_Scene_Authoring.md` | `0e69d8b664239477738f3dcce96eb5cfce2ff6e3db8817261f639ac4d64092b3` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Handoff/CURRENT_PROJECT_HANDOFF.md` | `7a297d84a36ecdd9e4296a631fc5acbff882218d125e63312850e8f28b46cd87` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Handoff/NEXT_CHAT_START_HERE.md` | `e0cca56b839dc2071363b310c06543178dbf978622990fc7f4be301e077fc754` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Historical/Sprint_16_Integration_Timeline.md` | `89fde821c460328fbb8700022883006d2eb8a1ab892c266972b7a1cc207cbcc0` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Sprints/Sprint_16/Documentation/StoreInitial_Authoring_Plan.md` | `f67d195bcda2bd1f861dcf5c53d0a299260c1f04c2f299250856a76616131eba` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Sprints/Sprint_16/Governance/Sprint_16_Current_Status.md` | `bc336c8bf852d942d1b0289cebfdde9db889088990fcc6151df5db559e94d1a9` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Sprints/Sprint_16/Governance/Sprint_16_Representative_Asset_Integration_Record.md` | `91133cf2823d732d58fcd23601d6816e610e00375cc38e39737929d88fc4ec54` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Sprints/Sprint_16/QA/S16_Acceptance_Matrix.md` | `4ad529aceeaf7e2ea87def0e0be2f8f3998d6218f6ac11a06b40f770e66c44fd` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Sprints/Sprint_16/QA/S16_QA_Execution_Record.md` | `40c6de3168db5dcf5c71afc166c0a3a2bf34f2dc3e71344c3ccea6176d573766` |
| 0.6 | CSV | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Sprints/Sprint_16/Traceability/S16_Traceability_Entries.csv` | `3263ee5adec14e81588139e9c291ae973115ac75bef79233e6a514ac3643fd67` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/06_Development_Records/Sprints/Sprint_17/Governance/Sprint_17_Opening_Brief.md` | `fb2c1a557e3b6cd765497315fa554eacc048c2fd21a2be9c7f5f28895a189ac1` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Build_Versioning_Guide_v0.6.md` | `8cd0f62e233cfbad2bed3a07122696e2ea81b4fd5280987cffb9a8a2a1e98d43` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_GDD_v0.6_PCSteam.md` | `333c7777584bcdee29c1caa8f90f4e43e3013e88cc339466a1a51f5bdd42c0d7` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.5.md` | `610fc2ca1dd8ea1859fb779039e5de841610646d1e5601d316b2fe34410c3c81` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Localization_Plan_v0.5.md` | `e8c63ec8a67f5d87f5dcec078c94ac08620b27a064a5a6866a20fa78de1becf9` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Package_Manifest_v0.4.md` | `89ae5a7d733d8d70f36b64cfe3653aed99149827155fcddec0cadf650fd38acb` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_QA_Testing_Plan_v0.6.md` | `7659a777a9aa78f083d8a6d577898bec89430cae108b079ee7137c769da7f069` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Steam_Publishing_Plan_v0.5.md` | `f7840ee791e07430dd750500421dc6e262d21d5119e634634919247ff13cf8d0` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Vertical_Slice_Specification_v0.4.md` | `efff0d3056515ad3e71a9c751f7ec86ce932d3098fb5870b246c04559695a3f5` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/CURRENT_PROJECT_HANDOFF.md` | `7a297d84a36ecdd9e4296a631fc5acbff882218d125e63312850e8f28b46cd87` |
| 0.6 | MD | `Documentation/00_Official_Baseline/v0.6/PACKAGE_MANIFEST.md` | `c25b2f638ce7814f8f2d9a153481efe6891a01b7e873f1a3251775ee51b922dd` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/00_Visual_Targets/ChatGPT Image 29 jun 2026, 21_37_54.png` | `cfdd490b742396ac2fff6ba0b1e6cea8d71fb2005305ddd9c82487ec7ea88b84` |
| working/current records | FILE | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/01_Architecture/.gitkeep` | `e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/02_Furniture/ChatGPT Image 29 jun 2026, 21_38_14 (10).png` | `42175ffa89a40d6bfb57217b38a5dfee04e337bc44b838ca8db01812e56454e5` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/02_Furniture/ChatGPT Image 29 jun 2026, 21_38_14 (7).png` | `6d5032c38dc50979430763f7ee18a84deee243325d3400affc2012551864ee1a` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/02_Furniture/ChatGPT Image 29 jun 2026, 21_38_14 (8).png` | `78f6c5d8ecc65077ee7191715e71f31b15a7ce22b59becf54e69dcca19bded48` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/02_Furniture/ChatGPT Image 29 jun 2026, 21_38_14 (9).png` | `16f940c84ce6c14a277a5ed236d1306df17d80419e118507f0f2032f0b4ad558` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/03_Products/ChatGPT Image 29 jun 2026, 21_38_14 (1).png` | `410d0bf38ab8848df5a496e250786d793d1d45bf8bbeab48c8f8ca83fa91d8fd` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/03_Products/ChatGPT Image 29 jun 2026, 21_38_14 (2).png` | `6107ce0119514adab534d271d224c0a3b9b2006eebe551cc43297299a9edc404` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/03_Products/ChatGPT Image 29 jun 2026, 21_38_14 (3).png` | `27639fb594bb9c24fc20403cbcf15a683b91af6b12250d3062fb2d0ec054528c` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/03_Products/ChatGPT Image 29 jun 2026, 21_38_14 (4).png` | `7ade04828ed3b7623c7e7dc271adde637cdcbf746c6a78566a3faa3cb5073e63` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/03_Products/ChatGPT Image 29 jun 2026, 21_38_14 (5).png` | `48be40e282d818092991e3f24d9e6b21e324025374a15ae762faf4fca8762779` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/03_Products/ChatGPT Image 29 jun 2026, 21_38_14 (6).png` | `56891223f619f891a523c01037823ca04fa386cc2b0963858c6115e9984df4bd` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/04_Characters/ChatGPT Image 29 jun 2026, 21_38_50 (1).png` | `fd439e31144c41f1eedd8d417911c1f78fbdbe02623b4859535db50082e8a491` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/04_Characters/ChatGPT Image 29 jun 2026, 21_38_50 (2).png` | `e5bca43f10a16a56d793260cf1bad77fa308db981207d5e6d37325159e6d8a11` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/04_Characters/ChatGPT Image 29 jun 2026, 21_38_50 (3).png` | `093b91e470d56a2609622789d9955d99f700014a6e9733fb4dc32da839d6d7e2` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/04_Characters/ChatGPT Image 29 jun 2026, 21_38_50 (4).png` | `546539231dbaf0ac0f0d3bd92bd4349e7ea7c13bee7dd1aa0768363529198c07` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/04_Characters/ChatGPT Image 29 jun 2026, 21_38_50 (5).png` | `1d02083cf9c0736568b4fbefea98d57e1104d84e1def585b1c8bf87fec8462d1` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/04_Characters/ChatGPT Image 29 jun 2026, 21_38_50 (6).png` | `6f0416232df549d46156a2f6ff92d039b785cf7aa44f82f17c840d05bc1927a6` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/04_Characters/ChatGPT Image 29 jun 2026, 21_38_50 (7).png` | `ebb549522b8d6ac4ca57d47c8bf1d8cd0e96608824dfb5f0f2d7bd51b6c2dd98` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/04_Characters/ChatGPT Image 29 jun 2026, 21_38_50 (8).png` | `3f1d37c6c690d4173bc0067a6460e29bc5bce2e8911c965d6a7b8aae05d296cf` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/05_Expansion_Concepts/ChatGPT Image 29 jun 2026, 21_39_17 (1).png` | `ce23b3b1e2bf6186ab627de7247b794ea9ed6f8b80bd8d4cb02df94619801f09` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/05_Expansion_Concepts/ChatGPT Image 29 jun 2026, 21_39_17 (2).png` | `79e9ccc8ba8126f7a63b5f38fdc4bafcbab5479b39e6882de1add15f66529e7f` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/05_Expansion_Concepts/ChatGPT Image 29 jun 2026, 21_39_17 (3).png` | `78cb05222f8155d5bd9e709e8e214f4517023444ed79538c3693bb0b09ea232f` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/05_Expansion_Concepts/ChatGPT Image 29 jun 2026, 21_39_17 (4).png` | `1074b34d1051c2e32fa23325f042158d04e7497b54d61c44f4cc1c3b4505b1f5` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/05_Expansion_Concepts/ChatGPT Image 29 jun 2026, 21_39_17 (5).png` | `686585e4a4b1054b23cdea08297e28ddd0df211d966dc0ab8a6116ce9c17ca25` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/05_Expansion_Concepts/ChatGPT Image 29 jun 2026, 21_39_17 (6).png` | `7c349ed7b9258e832c0e7c3f3529fb4fe8ea322f9d7c52ef004d98a741b21519` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/05_Expansion_Concepts/ChatGPT Image 29 jun 2026, 21_39_17 (7).png` | `cf9a87944e4aaff1730c4162d300a6204d248f4e6a62f8c0fe46ee7152819ff5` |
| working/current records | PNG | `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art/05_Expansion_Concepts/ChatGPT Image 29 jun 2026, 21_39_17 (8).png` | `cb6d28e87a016aa78cb5b6d1d84dbe42368fadc7b50f3ef8345214bf1e6a59dd` |
| working/current records | MD | `Documentation/10_Development_Records/Builds/S0.10_Windows_Final_Build_Record.md` | `2bdddb6ad5bff23d564e9d179c0b31b5baa4dcf00624539571485725734231a2` |
| working/current records | MD | `Documentation/10_Development_Records/Handoff/CURRENT_PROJECT_HANDOFF.md` | `ea48666ee43dbf978bef364ee645cd3ff32f31e4e6dfbd96b9edfab316546028` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_01/QA/S1_Acceptance_Matrix.md` | `a5a4e0a192fe33559c582efd9ee4d53170e26af9e07db63aae1e22b1f5c66f49` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_02/QA/S2_Acceptance_Matrix.md` | `1c184fd5c44de6051e83614676a1571d4d49e7cd8b4d68c8ef85167b64e2692f` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_03/QA/S3.4_Build_Execution_Record.md` | `bdbcbe899c627a7abf1f302f7db42ce29de1f0809c8373a454e3bc070984ab7d` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_03/QA/S3_Acceptance_Matrix.md` | `dcd3a753ec03bf462ed61fee96f551716970f05a54702fcec5ebdc271bda15b7` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_04/QA/S4.4_Build_Execution_Record.md` | `6f662c5fe7ccef7a3080584798ccf51c720d4ade1dc50b1be2f8c4c107da529d` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_04/QA/S4_Acceptance_Matrix.md` | `30a86d51980351f806ef30aa6187ce90857bde474c163d47a6a3396b60522f89` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_05/QA/S5.4_Build_Execution_Record.md` | `83700192228af99fb4487bdf4cdd1332d13a194be7cd5fb197278013952f8fe1` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_05/QA/S5_Acceptance_Matrix.md` | `3c265e9509b133370788c1ea024e4de481f773b526a524f2ec880d60abe053b6` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_06/QA/S6_Acceptance_Matrix.md` | `b55fe039c6404ff22749ff07f20dc741ee277ece3a892d9d1a6a526eb61dab8e` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_07/QA/S7_Acceptance_Matrix.md` | `e5517ab2f2b047aacb634c1ee7e42ea6f4b1b0f3bb6477cc7a0fb595439546b2` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_08/Governance/S8_Handoff_Update_Template.md` | `752c345e77607a3d096548520f2ce082a816d2eeb0d01ea0d29ddbe2bd32013a` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_08/QA/S8_Acceptance_Matrix.md` | `514b68f2839e76b1757df9d2d457dca9701cbe8f3de4169c6e38fe7e7c1e576c` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_08/QA/S8_Build_Execution_Record.md` | `975e35974c3ac193dedc354ad39f652dccd31356a4d69ec241dddcfdd84f29c1` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_09/Governance/S9_Handoff_Update_Template.md` | `e3d501b14918d549aab4d42cc2d289a8f1d83730600d1083109ea64ea409baae` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_09/QA/S9_Acceptance_Matrix.md` | `476e8342efea9323b73b46337890b795fe2b3bc500486fd08a9e9f5dbf6706ef` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_09/QA/S9_Build_Execution_Record.md` | `743edb2e5421984273fb169aab3fbb16c48d6b0ac2e177c5e83ba01b51099304` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_10/Governance/S10_Handoff_Update_Template.md` | `e7893c392feb7b35d3e270034b36f9da758d73673adb7e3f50cecea5763003a0` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_10/QA/S10_Acceptance_Matrix.md` | `8a3558314d7d868e3e2ce0dc33fe62e7b1b33eddf1e70b2d6eab43c9b3600d8c` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_10/QA/S10_Build_Execution_Record.md` | `adea9b7fcdab6e319f0ded3171260cc0200e2ae3ab424916a8068a7175872369` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_11/Governance/S11_Handoff_Update_Template.md` | `3da63a8600ab4da7f1afe4032690763029d3551297fad0d51afbfa841687dad5` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_11/QA/S11_Acceptance_Matrix.md` | `a6f876fb02398be1ee466d2f9dde9613ad9c0fa2fc0088778b791ab9a3d00e8d` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_11/QA/S11_Build_Execution_Record.md` | `6e5b3ee3b6b9a1a6d2f6c90fc07df567cfdd81126f6e2c06aa4c3307db206f24` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_12/Governance/S12_Handoff_Update_Template.md` | `683e99d27f7af84f319fad7c98230f6dec7f9b9eeddf8b4066f750b22c230bd0` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_12/QA/S12_Acceptance_Matrix.md` | `9b16fac6f71a8d05203d140a77a1b0c202a5f445556cc663208b04e5c45392b9` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_12/QA/S12_Build_Execution_Record.md` | `e3531d9bc28665f33ed38c176188246308d38bd480be42c1758f848e433f69b4` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_13/Governance/S13_Handoff_Update_Template.md` | `7d91ce81d3567cd4f392f6d9784c7415d628eec2b65e7468ea5e1b694af743f8` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_13/QA/S13_Acceptance_Matrix.md` | `e7f134005949b363e84fadb28d740ff268adb3c42de47070fbcc968862319f2b` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_13/QA/S13_Build_Execution_Record.md` | `887232c7346268536f197fc6069ad7f518735c2f51c302f0d27e6c2625cd005b` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_14/Governance/S14_Handoff_Update_Template.md` | `70ac9acc5473a29f44723f4e1499f027f5adbc772ae45635ebdb035f2374bd4f` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_14/QA/S14_Acceptance_Matrix.md` | `6abc7f9f6b1f199f797ede8e4610be0819c89c52ba763b09489f76e5b6d2b3e9` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_14/QA/S14_Build_Execution_Record.md` | `758b82a0301e5f3de699b76365cf74c577377bcb6d88333c4b65a26e6065af33` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_15/QA/S15_Acceptance_Matrix.md` | `ef5c3d5a5d788d7a52965e858620ca76d260711f96e5132bb7938c765b6762da` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_15/QA/S15_Build_Execution_Record.md` | `cb74c7605f9918b31b4f49ebbe0f63004b03886d5468bd258a3eb6f83ea9b61a` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Governance/Sprint_16_Two_Phase_Charter.md` | `b191b8b5d47654141df7860d6a79430552cc68e1bcdc786d1d4fef55583dc605` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Architecture/ADR-0067-Sprint16TwoPhaseDelivery.md` | `59f759e966b057f091473d52cf205d1a089a0917d2f8fff80342ca8ce2ca0a75` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Architecture/ADR-0068-PurePhase1Sidecar.md` | `5a4b767d6daf552a48dc2b4d256b38c698da130b41314243902c4b5fbab68459` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Architecture/ADR-0069-AutosaveCoordinatedCheckpoint.md` | `c4aaad9ce98b604d52894350668f7e59cc8c9d893ec3e96658a467bff0dbd44a` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Architecture/ADR-0070-PlacementCompatibilityBridge.md` | `66a5b5172267c6984edd5474ba03dc2fb0da28de72c310ced74feba060f6df17` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Architecture/ADR-0071-DecoupledPresentationFallbacks.md` | `f2e025be2f68b6de2388bd63a6b25549d4e6d111012bc685b034fa120ff9616c` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Architecture/ADR-0072-SharedMaterialAndPrefabIds.md` | `f32e2b4b274522d03df3c1c08084d86114a19dbe15fa2b3008c217943e2ff11a` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Architecture/ADR-0073-CompletedCheckoutSnapshotRecords.md` | `7d401e745e557c63167dae48cd0f4b05b1d08f5c58a2e652b4321925fc35caa7` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Documentation/S16_P1_Implementation_Record.md` | `3538c025e899624c8dfb10c740bcbc06387f5be41a78a3396a8f5937a5cfc147` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Documentation/S16_P1_PreImplementation_Record.md` | `0d6915434ed6282040553f42f849399d6a8e3380eb4e38a6082ea3d13fba0e9c` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Governance/S16_P1_Charter.md` | `4b5190df961a1180ce9fed6ced5fe194e88998f5f65df176bea9104c7bc825dc` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/Governance/S16_P1_Current_Status.md` | `6494de60296d252da3368dab5a5850938c4a5c219d414f80bde328379f1c7f3b` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/QA/S16_P1_Acceptance_Matrix.md` | `d6d2ca42287be0fced9c3d9da09df5db9729442157035a9a711e0ddbb9d91337` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/QA/S16_P1_QA_Execution_Record.md` | `a044ba4e7ebb4af622c74397e2a8f0ff6e59ad7e9c1bf89ddb62f5b06bcacec7` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/QA/S16_P1_Test_Plan.md` | `ff35438f414f45a152e04532664e348d0fed9bae380c9dc70bf520af5c7c04c8` |
| working/current records | MD | `Documentation/10_Development_Records/Sprint_16/Phase_1/QA/S16_P1_Validation_Checklist.md` | `ac85fc7fe9c1e905ca5ea2eb245aa016acf8c611b2a3fe1a10fb3d0740e0198b` |
| working/current records | CSV | `Documentation/10_Development_Records/Sprint_16/Phase_1/Traceability/S16_P1_Asset_Inventory.csv` | `c91f4676f6c8b07c075fda4bf3563530380a5e35770d9f68b5497b77cf99641b` |
| working/current records | CSV | `Documentation/10_Development_Records/Sprint_16/Phase_1/Traceability/S16_P1_Traceability.csv` | `d1e5ada71ed4d81b6a067aa05ea35626d5156967a1f8b212da829be32a4d318d` |

## 149.4. Dossier de onboarding Steamworks

El dossier de onboarding se prepara antes de iniciar el alta y permanece separado de los datos sensibles que se introduzcan en el portal. Su objetivo es impedir que la configuración se improvise o que el proyecto use nombres, responsables y cuentas inconsistentes. El dossier no contiene contraseñas, documentos de identidad, números bancarios, información fiscal completa ni códigos de recuperación. Solo registra qué información existe, quién la custodia y cuándo fue validada.

La ficha principal identifica la persona o entidad que firmará el acuerdo, la relación con VRM Games, el titular de los derechos de publicación, el país, el correo operativo, el owner de Steamworks y los usuarios que necesitan acceso. Debe distinguir nombre legal, nombre comercial, developer visible, publisher visible y titular de copyright. Si alguno cambia, se detiene el onboarding hasta actualizar legal, créditos, store copy, web y materiales.

La sección financiera registra el presupuesto de la tarifa Steam Direct, el método de pago autorizado, la fecha prevista, el responsable y la ubicación segura del comprobante. También incluye la persona que completará tax interview y bank information, pero no copia sus respuestas. El estado se expresa como `NOT STARTED`, `IN PROGRESS`, `VALIDATED` o `BLOCKED`. Una casilla marcada no sustituye la validación del portal.

La sección de seguridad contiene la matriz de roles, MFA, recuperación, gestor de contraseñas, revisión de dispositivos y procedimiento para retirar permisos. Cada usuario utiliza su propia cuenta. Las credenciales y tokens de SteamCMD se inyectan en ejecución y nunca se escriben en scripts, logs publicados, screenshots o paquetes de evidencia compartidos con terceros.

La sección de producto registra título provisional, candidatos alternativos, estado de clearance, versión, plataforma, modelo de lanzamiento y AppID cuando exista. La adquisición de un AppID no convierte el nombre en final ni obliga a publicar. La fecha de pago inicia un reloj mínimo, pero el calendario comercial continúa gobernado por store review, build review, Coming Soon, calidad y soporte.

**Checklist del dossier:**

- [ ] Titular legal y derechos de publicación confirmados.
- [ ] Developer/publisher visible definido de forma provisional o final.
- [ ] Clearance de `Cartridge & Cloud` y `VRM Games` con decisión documentada.
- [ ] Usuarios, roles, MFA y recuperación aprobados.
- [ ] Presupuesto, método de pago y custodia del comprobante.
- [ ] Datos fiscales y bancarios preparados fuera del repositorio.
- [ ] Correo de soporte y contacto operativo mantenibles.
- [ ] Modelo de lanzamiento decidido o explícitamente pendiente.
- [ ] Owner de store page, builds, legal, QA y release.
- [ ] Regla de no anunciar fecha antes de superar los gates.

## 149.5. Matriz de campos de la store page

Antes de editar Steamworks se crea una matriz offline que funciona como source of truth. Cada fila contiene el nombre del campo, locale, source copy, traducción, límite o restricción, owner, estado, fecha de revisión, build/claim relacionado y evidencia. Esto reduce errores al copiar contenido al portal y permite comparar cambios entre revisiones.

La matriz separa Basic Info, short description, About This Game, developer/publisher, copyright, enlaces, categorías, idiomas, requisitos, tags, content survey, assets, trailers, screenshots y noticias. Las traducciones ES/EN se mantienen en columnas diferentes, no en documentos sin IDs. Cualquier texto incrustado en imagen se registra como una variante por locale o se elimina para reducir mantenimiento.

Los claims se clasifican como `VERIFIED`, `CONDITIONAL`, `FUTURE/PROHIBITED` o `LEGAL REVIEW`. `VERIFIED` enlaza a una build exacta y una prueba. `CONDITIONAL` solo puede publicarse si la condición aparece de forma clara y no induce a error. `FUTURE/PROHIBITED` nunca pasa al portal. `LEGAL REVIEW` queda bloqueado hasta resolver título, marcas, privacidad o licencias.

La descripción corta no incluye fechas, disponibilidad, precio, descuentos ni calls-to-action. About This Game puede usar imágenes o GIFs, pero debe respetar los límites y las reglas vigentes; las imágenes con texto aumentan el coste de localización. Los enlaces se introducen exclusivamente en los campos destinados a ello. No se usan QR, URLs escritas en imágenes ni imitaciones de botones de Steam.

Los requisitos mínimos y recomendados se mantienen bloqueados hasta completar perfilado. Los idiomas se marcan según soporte in-game real. Las categorías de mando, Cloud, achievements, Workshop, Remote Play o Linux se mantienen sin marcar hasta disponer de implementación y QA. Los tags se ordenan tras Tag Wizard y revisión competitiva; la lista histórica solo sirve como semilla.

| Grupo | Campo | Owner | Gate | Evidencia requerida |
|---|---|---|---|---|
| Identity | Game name | Legal/Release | Pre-review | clearance y decisión |
| Basic Info | Developer/Publisher | Legal/Release | Onboarding | identidad consistente |
| Description | Short ES/EN | Marketing/Localization | Store review | copy review + claim matrix |
| Description | About ES/EN | Marketing/Design | Store review | preview + build references |
| Features | Languages | Localization/QA | H6/RC | Golden Path ES/EN |
| Features | Controller/Cloud/etc. | Engineering/QA | RC | implementación y tests |
| Assets | Screenshots | Art/QA | Store review | build/commit de origen |
| Assets | Capsules/Library | Art/Legal | Store review | templates y licenses |
| Video | Trailer | Art/Audio/Legal | Store review | master, cue sheet, claims |
| Technical | System requirements | Engineering/QA | RC | hardware matrix |
| Legal | Copyright/Privacy/EULA | Legal | Store review | textos aprobados |
| Discovery | Tags | Product/Marketing | Coming Soon | Tag Wizard y rationale |

## 149.6. Shot list de screenshots y captura de gameplay

La captura se realiza únicamente después de que StoreInitial sea la escena runtime aprobada y el perfil de candidata excluya TestLab, debug overlays, placeholders no exceptuados y contenido NOT OPEN. Cada sesión de captura comienza registrando versión, commit, Build ID, resolución, locale, quality preset, hardware y save utilizado. Las imágenes no se retocan para cambiar la función, la UI o el contenido; se permiten ajustes de compresión, recorte permitido y color global que no falseen la experiencia.

El conjunto mínimo operativo debe cubrir diversidad funcional y visual. Una captura muestra la tienda completa y su circulación. Otra muestra compra/pedido en Operations. Otra representa receiving y stock. Otra muestra productos en displays y clientes comprando. Otra muestra checkout y economía/HUD. Se pueden añadir construcción, cierre diario o un detalle de branding si la composición es clara. No se repite la misma cámara con variaciones mínimas.

La captura principal debe comunicar el género sin explicación. La UI visible debe ser legible, localizada y representativa. Si la store page se publica en ES y EN, se decide si se usan imágenes sin texto, variantes por locale o un conjunto que no dependa de labels. No se muestran IDs, rutas, números de test, mensajes de fase, botones internos, consola, profiler, FPS counter ni datos personales.

El shot list registra objetivo, escena, hora/día del juego, productos, clientes, paneles, focal point, riesgos y estado. Cada imagen pasa por revisión de gameplay truth, art, UI, localization, legal/IP y marketing. La imagen rechazada se conserva con motivo para evitar reintroducirla accidentalmente.

**Shot list propuesto:**

1. Vista general de StoreInitial con entrada, ventas, checkout y backroom legibles.
2. Operations/Shop con selección de productos y precios representativos.
3. Delivery/Receiving con pedido listo para recibir, sin debug.
4. Stock/Displays con reposición y productos visibles.
5. Cliente explorando y tomando una decisión de compra.
6. Cola y checkout con HUD y feedback económico.
7. Modo construcción con preview válido, solo si está cerrado y representativo.
8. Cierre diario/resumen, solo si la pantalla final está aprobada.

Cada export conserva PNG maestro, versión JPG/PNG para Steam, hash y documento de procedencia. Las dimensiones se obtienen de las plantillas actuales. Las screenshots se mantienen en 16:9 y mínimo vigente, pero la fuente master puede ser mayor para permitir recorte limpio.

## 149.7. Plantillas SteamPipe sin secretos

Los scripts SteamPipe se almacenan bajo control de versiones en una carpeta de tooling que no contiene credenciales. Los valores AppID y DepotID pueden ser parámetros o configuraciones versionadas; el usuario/password/token se pasan por entorno o prompt seguro. El directorio de contenido apunta al staging limpio generado por el pipeline de Unity.

Ejemplo conceptual —debe adaptarse con IDs reales y sintaxis oficial vigente—:

```text
Steamworks/
├── sdk/
│   └── tools/ContentBuilder/
├── scripts/
│   ├── app_build_<APPID>_internal.vdf
│   ├── app_build_<APPID>_candidate.vdf
│   └── depot_build_<DEPOTID>_windows.vdf
├── staging/
│   └── Windows_x64/
├── output/
└── logs/
```

```text
"AppBuild"
{
    "AppID"        "<APPID>"
    "Desc"         "<ReleaseID> <Commit> Windows x64"
    "BuildOutput"  "../output"
    "ContentRoot"  "../staging/Windows_x64"
    "SetLive"      "internal"
    "Depots"
    {
        "<DEPOTID>" "depot_build_<DEPOTID>_windows.vdf"
    }
}
```

```text
"DepotBuildConfig"
{
    "DepotID"       "<DEPOTID>"
    "ContentRoot"   "../staging/Windows_x64"
    "FileMapping"
    {
        "LocalPath" "*"
        "DepotPath" "."
        "recursive" "1"
    }
    "FileExclusion" "*.pdb"
    "FileExclusion" "*_BurstDebugInformation_DoNotShip*"
    "FileExclusion" "Player.log"
}
```

Las exclusiones reales se basan en el Build Report y en una allowlist revisada, no solo en tres patrones. Se debe comprobar que excluir símbolos no impide diagnóstico previsto y que no se suben saves, logs, documentación, source assets, Blender, conceptos, TestLab, archivos fiscales, credenciales o Steamworks SDK innecesario.

El upload se ejecuta primero con `SetLive` vacío o branch privada, según flujo aprobado. El log se redacta para ocultar secretos y se archiva con BuildID, Depot Manifest IDs, duración, tamaños y errores. Cualquier cambio de depot/package se prueba con una cuenta que represente el entitlement real.

## 149.8. Release Manifest de una candidata Steam

El Release Manifest es el índice humano y automatizable de una candidata. Identifica exactamente qué se revisó y qué se promoverá. Se genera antes del upload y se completa después con IDs de Steam.

```yaml
release_id: CAC_STEAM_RC_001
product_version: 0.0.x
unity_version: 6000.3.18f1
urp_version: 17.3.0
git_commit: <sha>
internal_build_id: <id>
app_id: <steam_appid>
steam_build_id: <after_upload>
branch: release_candidate
depots:
  - depot_id: <windows_depot>
    manifest_id: <after_upload>
platform: Windows x64
scenes:
  - Bootstrap
  - MainMenu
  - StoreInitial
excluded:
  - TestLab
  - Store
  - Expansion/NOT_OPEN assets
configuration:
  development_build: false
  script_debugging: false
  player_log: true
  locale_default: es-ES
  locales_supported: [es-ES, en-US]
checksums:
  package_sha256: <hash>
  manifest_sha256: <hash>
qa:
  automated: PASS
  golden_path_es: PASS
  golden_path_en: PASS
  seven_day_campaign: PASS
  clean_install: PASS
  update: PASS
  uninstall: PASS
  rollback_drill: PASS
legal:
  title_clearance: PASS
  audio: PASS
  font: PASS
  notices: PASS
  credits: PASS
  privacy: N/A_OR_PASS
store:
  store_review: APPROVED
  build_review: APPROVED
  coming_soon_since: <timestamp>
known_issues: []
previous_good_steam_build_id: <id>
decision: PENDING_MANUAL_GO
```

El manifest no contiene tokens, passwords ni datos personales. Las palabras `PASS` solo se usan con evidencia. Los estados `N/A` explican por qué no aplica y quién lo aprobó. Si se cambia un byte después de QA, se crea un nuevo release ID o una revisión explícita y se repiten las pruebas proporcionales.

La ficha enlaza a release notes, known issues, Player.log, build report, store snapshot, Valve review, legal register, localización, soporte y rollback. La candidata anterior se mantiene descargable en branch privada o identificada para restauración.

## 149.9. Paquete para review de Valve

La revisión de store page se prepara como una entrega coherente, no como un experimento. Antes de marcarla ready for review, se navega por todos los locales, se comprueban assets, enlaces, tags, requisitos y declaraciones. El título ya debe estar decidido debido al impacto de la revisión pre-release sobre cambios posteriores.

La build de review arranca desde Steam, ofrece una ruta clara al gameplay y no requiere cuentas externas, cheats secretos ni configuración manual no explicada. Si existe tutorial, guía al reviewer. Si no, las instrucciones de review describen cómo iniciar una partida y completar el flujo esencial sin revelar información innecesaria. La build no puede depender de una fecha, backend o servidor no disponible.

El paquete interno conserva:

- AppID, BuildID, branch y manifest IDs.
- Instrucciones para iniciar, idioma y controles.
- Duración estimada para alcanzar el bucle principal.
- Explicación de features declaradas.
- Known issues que no contradicen release.
- Contacto operativo y zona horaria.
- Store page preview ES/EN.
- Capturas de los checklists completados.
- Fecha de envío y fecha objetivo interna.

Al recibir feedback, cada observación se convierte en issue con severidad, owner y respuesta. No se discute una observación sin reproducirla. Si se corrige la build, se registra nuevo BuildID; si se corrige solo la página, se conserva diff. El calendario incluye al menos el margen recomendado y tiempo para un posible segundo envío.

## 149.10. Runbook de lanzamiento

El runbook se ensaya en una dry run. Todas las horas se expresan en Europe/Madrid y UTC para evitar confusiones. El release owner es la única persona que ejecuta la acción final; los otros roles verifican y registran.

**T−7 días laborables o antes:** confirmar reviews aprobadas, Coming Soon mínimo, precio, packages, fecha, store assets, locales, soporte, legal, previous-good y calendario. Congelar features. Publicar una comunicación solo si está aprobada.

**T−48 h:** instalar RC desde `release_candidate` en máquina limpia, completar smoke y compra simulada/entitlement según herramientas disponibles, revisar Store page final, FAQ, support inbox, notices y credits. Confirmar que no hay S0/S1, incidentes abiertos ni cambios de package/depots.

**T−4 h:** verificar que el BuildID aprobado sigue en la branch candidata, que default no cambió, que la cuenta release tiene acceso y MFA, y que previous-good está identificado. Revisar status de servicios y disponibilidad del owner. No realizar cambios cosméticos de última hora.

**T−30 min:** abrir el release manifest, comprobar AppID, package, precio, fecha, locales, store/build approval y el checklist. Iniciar launch log. Cerrar herramientas que puedan interferir y realizar una captura del estado previo.

**T0:** ejecutar release conforme al portal. Registrar hora y resultado. No asumir éxito por ver un botón completado: comprobar la página pública y la adquisición/instalación con una cuenta de prueba autorizada.

**T+15 min:** verificar store, precio, idiomas, cápsulas, trailer, screenshots, community, default branch y launch. Instalar y abrir. Comprobar que la versión visible y el save path son correctos.

**T+1 h:** revisar soporte, crashes, reviews, foros y problemas de entitlement. Clasificar incidentes. No responder impulsivamente a reviews; priorizar fallos de acceso, inicio, saves y corrupción.

**T+4 h y T+24 h:** publicar información solo si aporta valor. Decidir mantener, hotfix o rollback con criterios predefinidos. Actualizar el launch log y la lista de known issues.

## 149.11. Matriz de incidentes y decisiones

| Severidad | Ejemplos | Acción inmediata | Objetivo |
|---|---|---|---|
| S0 | malware/seguridad, pérdida masiva irreversible, cobro/acceso gravemente incorrecto | detener distribución si es posible, escalar, rollback y comunicación | contención inmediata |
| S1 | no inicia para población amplia, save corruption, Golden Path bloqueado, crash generalizado | declarar incidente, evaluar rollback/hotfix | decisión en ventana corta |
| S2 | feature importante degradada con workaround, localización rota en una pantalla crítica | triage prioritario, hotfix planificado | corrección rápida |
| S3 | defecto visual o UX no bloqueante | backlog/patch | siguiente parche razonable |
| S4 | mejora o inconsistencia menor | backlog | planificación normal |

El incidente registra BuildID, AppID, branch, alcance, primera detección, fuentes, reproducción, workaround, riesgo de save, decisión y owner. El canal de soporte recibe una respuesta coherente; el equipo no publica soluciones inseguras como desactivar antivirus, borrar carpetas indiscriminadamente o compartir credenciales.

Un rollback se favorece cuando el previous-good es compatible con saves y el defecto afecta al inicio, integridad o mayoría de usuarios. Un hotfix se favorece cuando la causa es pequeña, el cambio es aislado y se puede probar con rapidez sin aumentar el riesgo. Mantener live se acepta solo con impacto limitado, workaround seguro y comunicación suficiente.

## 149.12. Política de actualizaciones y patch notes

Las patch notes se escriben desde el diff real y las pruebas, no desde mensajes de commit aislados. Separan correcciones, cambios de balance, mejoras, contenido y known issues. No revelan datos personales, detalles explotables de seguridad ni promesas futuras no aprobadas. La versión, fecha y BuildID quedan registrados internamente aunque no todos se publiquen.

Cada actualización pasa por branch QA, install/update test, save compatibility, ES/EN, Player.log, legal diff y rollback. Los cambios de paquetes, audio, fuentes, nombres, servicios externos o datos requieren revisión legal adicional. Una actualización que añade feature de Steam actualiza store declarations y documentación.

La cadencia se adapta al riesgo. No se publica una cadena de microparches sin QA que genere descargas constantes. Tampoco se retrasa un S1 para mantener una cadencia estética. Los jugadores reciben información suficiente para entender qué cambia y si deben hacer algo.

## 149.13. Métricas y revisión postlanzamiento

La revisión inicial observa tendencias, no objetivos absolutos inventados. Steamworks proporciona store traffic, wishlists, sales, refunds, regional/language data y otras métricas de partner; el proyecto debe consultar la documentación y el portal vigente. El soporte añade volumen por categoría, tiempo de primera respuesta y defectos recurrentes. Las reviews se analizan por temas sin intentar manipular el score.

Las métricas técnicas mínimas son porcentaje de sesiones con crash si existe una fuente fiable, fallos de inicio, save issues, tiempo de carga, performance reports y distribución de versiones. No se añade telemetría propia solo para llenar un dashboard. Cualquier recogida adicional requiere propósito, minimización, proveedor, retención, privacidad y opt-out cuando corresponda.

A los 7, 30 y 90 días se realiza una revisión: calidad, soporte, balance, localización, marketing, precio, descuentos, plataforma y deuda. Las acciones se priorizan por impacto en jugadores y sostenibilidad. El resultado alimenta `25_Post_Launch_and_Live_Operations_Plan.md`, pero no reabre automáticamente sistemas NOT OPEN.

## 149.14. Definition of Ready y Definition of Done de la fase Steam

**Ready para onboarding:** H6 aprobado, titular y naming encaminados, presupuesto, security owner y decisión comercial. **Done:** partner account operativo, AppID registrado, fee y espera documentados, permisos y secretos gestionados.

**Ready para store production:** nombre aprobado, visual target estable, StoreInitial representativo, copy source, legal y localización activas. **Done:** store pack ES/EN, assets en templates, claims verificados, preview y review Valve aprobada.

**Ready para SteamPipe:** perfil Release, staging reproducible, TestLab/NOT OPEN excluidos, AppID/DepotID y scripts sin secretos. **Done:** upload a branch privada, manifests registrados, instalación Steam PASS y previous-good definido.

**Ready para RC:** Sprint 17/H6 cerrados, localización, audio, fuente, legal, QA y soporte sin bloqueos. **Done:** BuildID congelado, store/build reviews aprobadas, Coming Soon mínimo, rollback probado y go/no-go preparado.

**Ready para release:** no S0/S1, legal publicable, soporte disponible, precio/packages correctos, fecha y comunicaciones aprobadas. **Done:** default branch correcta, smoke post-release PASS, launch log completo y monitorización iniciada.


## 149.15. Protocolo de investigación de mercado y precio

La investigación comercial se ejecuta cerca de la decisión de precio porque catálogo, descuentos, valoraciones y posicionamiento cambian. No se seleccionan únicamente juegos visualmente parecidos: los comparables se agrupan por fantasía de gestión, profundidad sistémica, duración, nivel de producción, estado de lanzamiento, soporte de idiomas y tamaño aproximado del equipo. Los juegos en Early Access se separan de los lanzamientos completos y los descuentos temporales no se confunden con precio base.

La tabla de comparables registra nombre, fecha de consulta, precio base por mercado de referencia, discount observado, review status, tags principales, idiomas, features Steam, calidad de store assets, alcance jugable y motivo de comparabilidad. La información se utiliza para construir un rango, no para copiar un precio. También se anotan diferencias: contenido, profundidad, pulido, soporte, frecuencia de updates y reputación.

El memo de precio plantea al menos tres escenarios: conservador, objetivo y premium. Para cada uno analiza percepción de valor, volumen necesario, margen después de comisiones/impuestos aplicables, riesgo de refunds y capacidad de descuento futuro. La economía interna del juego no determina el precio real. Tampoco se usa el número de horas de desarrollo como argumento comercial.

La decisión final incluye fecha, moneda base, regiones excepcionadas, launch discount o ausencia de él, owner y condición de revisión. Se comprueba que el precio resultante no cae bajo mínimos regionales al aplicar descuentos. Cualquier cifra en este plan es interna hasta publicarse en Steamworks.

## 149.16. Matriz QA de assets de Steam

Cada asset gráfico pasa por una matriz independiente del gusto artístico. La revisión técnica confirma dimensiones, formato, perfil de color, peso, transparencia cuando corresponda y ausencia de corrupción. La revisión visual confirma logo legible, foco claro, contraste, recortes seguros y consistencia entre cápsulas. La revisión de reglas confirma que las cápsulas base no contienen textos prohibidos, precios, descuentos, fechas, awards o review scores fuera de los espacios admitidos.

La revisión legal verifica título, marcas ficticias, ausencia de logos ajenos, licencias de fuente, autoría y procedencia de ilustración. La revisión de localización comprueba que cualquier texto distinto del logo tenga variante apropiada o se elimine. La revisión de veracidad compara el material con la build y clasifica si es key art, screenshot, render o concepto; solo las screenshots se cargan en la sección correspondiente.

| Control | Header | Small | Main | Library | Screenshot | Trailer |
|---|---:|---:|---:|---:|---:|---:|
| Template vigente | obligatorio | obligatorio | obligatorio | obligatorio | obligatorio | n/a |
| Logo legible | sí | crítico | sí | sí | opcional | intro breve |
| Texto adicional | no | no | no | no | solo UI real | solo material aprobado |
| Build source | n/a | n/a | n/a | n/a | obligatorio | obligatorio |
| Licencia | obligatorio | obligatorio | obligatorio | obligatorio | obligatorio | audio/visual completo |
| Locale | logo o variante | logo o variante | logo o variante | logo o variante | estrategia definida | captions/texto revisado |
| Review final | Art + Legal | Art + Legal | Art + Legal | Art + Legal | Art + QA + Legal | Art + QA + Audio + Legal |

El paquete conserva el template original descargado, source editable, export, hash y preview. Al cambiar el título o logo se invalidan automáticamente los assets dependientes. Al cambiar visualmente el juego se revisa si las screenshots siguen representativas.

## 149.17. Árbol de decisión: Demo, Playtest o branch privada

Se usa branch privada cuando el grupo es pequeño, conocido y necesita validar instalación, updates, Cloud, depots o un RC concreto. Su ventaja es probar la aplicación base; su riesgo es gestionar passwords, keys y acceso. No es la herramienta ideal para reclutamiento público amplio.

Steam Playtest se usa cuando se necesita una cohorte mayor, control de acceso y recopilación de feedback sin entregar el producto base como lanzamiento. Debe tener objetivos, duración, build legal para test y proceso de cierre. El Playtest no se deja permanentemente activo por inercia ni se presenta como contenido final.

La demo se usa cuando existe una experiencia breve y mantenible que comunica valor, puede aparecer públicamente y justifica su coste de QA, store page y soporte. La demo necesita AppID relacionado, build propia, declarations verdaderas, política de saves y actualización. No se recorta una build arbitrariamente si el resultado tiene onboarding roto o concluye sin sentido.

Preguntas de decisión:

1. ¿El objetivo es QA técnico de una candidata exacta? Branch privada.
2. ¿El objetivo es feedback de una cohorte controlada antes del lanzamiento? Playtest.
3. ¿El objetivo es marketing público y prueba de producto sostenible? Evaluar demo.
4. ¿Existe capacidad para mantener dos builds/store surfaces? Si no, no crear demo.
5. ¿El contenido de prueba tiene licencias y privacidad adecuadas? Si no, bloquear.
6. ¿La decisión compromete H6 o el core product? Si sí, posponer.

## 149.18. Matriz de Steam Cloud y conflictos de save

Antes de activar Cloud se documentan todos los archivos persistentes: snapshots, backups, settings, locale, logs y temporales. Solo se sincroniza lo que el usuario espera recuperar. Logs, caches, diagnostic dumps y configuraciones específicas de máquina se excluyen. La preferencia de idioma puede ser global o local según la decisión de producto, pero debe ser consistente.

La prueba mínima cubre PC A online → PC B online, PC A offline → reconexión, cambios concurrentes, archivo corrupto, backup recovery, borrado intencionado, desactivación de Cloud por el usuario y actualización de schema. Se registra qué mecanismo de resolución muestra Steam y qué instrucciones ofrece el soporte. No se automatiza una elección destructiva sin backup.

| Caso | Precondición | Resultado esperado |
|---|---|---|
| Primera sincronización | save local válido | upload y descarga sin cambio de checksum lógico |
| Nuevo equipo | sin saves locales | descarga y carga correctas |
| Offline | Cloud no disponible | juego guarda local y reintenta después |
| Conflicto | dos saves divergentes | flujo comprensible, sin pérdida silenciosa |
| Corrupto | snapshot principal inválido | backup local/Cloud recuperable |
| Update schema | save antiguo | migración o error seguro |
| Cloud desactivado | opción de usuario | funcionamiento local completo |
| Rollback build | save de versión nueva | comportamiento documentado y no destructivo |

Cloud no se marca en la store page hasta pasar esta matriz en una branch Steam real. Si Auto-Cloud no permite controlar correctamente los archivos o conflictos, se evalúa API; esa decisión requiere arquitectura, tests y privacidad adicionales.

## 149.19. Biblioteca de respuestas de soporte

Las macros de soporte sirven como base, nunca como sustituto de leer el caso. Incluyen saludo, confirmación del problema, pasos seguros, datos mínimos y cierre. No piden contraseñas, códigos de Steam Guard, documentos personales ni acceso remoto no autorizado.

**No inicia:** confirmar versión, sistema operativo, ruta de instalación, mensaje exacto y Player.log. Proponer Verify Integrity, reinicio y actualización de drivers solo cuando sea pertinente. No recomendar borrar saves.

**Save no carga:** pedir copia del directorio de saves mediante canal seguro, versión y último momento funcional. Instruir backup antes de cualquier cambio. No prometer recuperación hasta inspeccionar schemas/checksum.

**Rendimiento:** solicitar resolución, quality preset, CPU/GPU/RAM, escenario y si el problema ocurre desde una versión concreta. Distinguir FPS bajo, stutter, carga y input lag.

**Idioma/texto:** registrar locale del juego y Steam, key/pantalla, resolución y captura. No pedir al jugador que cambie permanentemente de idioma como solución final.

**Compra/reembolso:** las transacciones y refunds se gestionan mediante Steam; el soporte del juego puede diagnosticar entitlement o contenido, pero no solicitar datos de pago.

**Bug conocido:** explicar impacto, workaround seguro, versión afectada y canal donde se anunciará la corrección. No dar una fecha si no está aprobada.

Todas las macros se localizan, versionan y revisan tras incidentes. Un caso repetido se convierte en FAQ o bug, no en una respuesta copiada indefinidamente.

## 149.20. Control de cambios en Steamworks

Steamworks es un sistema de producción. Los cambios en store page, precio, packages, depots, branches, release date, permissions y features se registran mediante change request. Cada solicitud indica AppID, área, valor anterior, valor nuevo, motivo, riesgo, owner, reviewer, fecha, rollback y screenshots antes/después.

Los cambios editoriales menores también necesitan revisión si afectan claims, idiomas o legal. Un typo puede corregirse con flujo ligero; cambiar nombre, plataforma, precio, fecha, packages o build exige aprobación reforzada. Los cambios urgentes durante un incidente se registran retroactivamente dentro de la ventana definida, pero nunca se omiten.

La regla de cuatro ojos se aplica cuando exista otra persona disponible; en producción individual se realiza una pausa deliberada y una segunda revisión contra checklist antes de confirmar. Los permisos de release no se utilizan para tareas rutinarias desde cuentas o dispositivos no controlados.

El change log permite reconstruir por qué una página o branch tuvo cierto estado en un momento. Se archiva junto a cada release y alimenta la retrospectiva. Los cambios de Steamworks que contradigan los documentos actuales abren una actualización documental o una excepción formal.

## 149.21. Plantilla formal de decisión Go / No-Go

La decisión de lanzamiento no se deduce automáticamente de que una build compile, supere la suite de tests o haya sido aceptada por la revisión técnica de Steam. Debe existir una reunión o revisión formal en la que se evalúen de manera conjunta producto, QA, operaciones, legal, comercial, soporte y capacidad de rollback. En un proyecto individual, estas funciones pueden recaer sobre la misma persona, pero deben revisarse como responsabilidades separadas para evitar que el entusiasmo por publicar sustituya a la evidencia.

La plantilla mínima de decisión debe registrar:

- App ID y nombre visible de la aplicación;
- versión, commit, checksum, depot manifest y branch candidata;
- fecha y hora de la decisión;
- estado de la revisión de la store page y de la build;
- resultado del smoke test de instalación limpia;
- resultado del Golden Path y de la campaña de siete días;
- defectos S0, S1 y S2 abiertos, con decisión explícita sobre cada uno;
- estado de audio, fuente, localización, créditos, notices, privacidad y clearance de marca;
- disponibilidad de una build anterior restaurable;
- disponibilidad de soporte durante la ventana de lanzamiento;
- decisión final `GO`, `NO-GO` o `GO WITH ACCEPTED RISKS`;
- firma o identificación del responsable y enlaces a todas las evidencias.

`GO WITH ACCEPTED RISKS` no debe utilizarse para eludir un bloqueo definido como obligatorio. Solo puede aceptar riesgos residuales que no afecten a integridad de datos, acceso al juego, obligaciones legales, información engañosa en la tienda, pérdida de partidas o imposibilidad de rollback. Toda excepción debe poseer owner, fecha de caducidad, impacto conocido y plan de corrección.

Un `NO-GO` no equivale a fracaso del proyecto. Es el funcionamiento correcto del gate cuando la evidencia no es suficiente. La nueva fecha debe fijarse después de identificar la causa y no como reacción inmediata para conservar un anuncio previo. Cuando una fecha pública esté comprometida, la comunicación debe ser veraz, breve y coordinada con la actualización de la store page y los canales comunitarios.

## 149.22. Retención, archivo y reproducibilidad de una publicación

Cada candidata y cada build publicada debe conservar un paquete de archivo suficiente para reconstruir qué se distribuyó y por qué fue aprobado. No basta con que Steam mantenga un manifest: el proyecto debe conservar su propia evidencia, protegida frente a pérdidas locales y separada de credenciales sensibles.

El paquete de archivo debe incluir:

1. código fuente o referencia inmutable al commit;
2. versión de Unity y paquetes;
3. configuración de build y lista de escenas;
4. scripts de ContentBuilder y VDF utilizados, sin secretos;
5. App ID, depot IDs, branch y manifest IDs;
6. checksum del paquete local y de cualquier instalador de verificación;
7. resultados de EditMode, PlayMode, smoke test, Golden Path y siete días;
8. `Player.log` y registros de errores relevantes;
9. capturas y vídeo de la build realmente publicada;
10. exportación de la store page y sus idiomas;
11. cápsulas, library assets y tráiler aprobados;
12. créditos, `THIRD_PARTY_NOTICES`, licencia y política de privacidad aplicables;
13. decisión Go/No-Go;
14. lista de defectos conocidos y riesgos aceptados;
15. instrucciones de rollback y build anterior restaurable.

Las credenciales de Steamworks, contraseñas, códigos de Steam Guard, claves privadas, datos fiscales y bancarios no forman parte del archivo del repositorio. Deben mantenerse en sistemas seguros con acceso mínimo. Los archivos VDF que contengan credenciales o tokens deben excluirse, sanearse o generarse localmente.

La retención recomendada es indefinida para releases públicas y para cualquier build que haya migrado datos de guardado. Las candidatas fallidas pueden conservarse según valor de diagnóstico, pero sus decisiones y defectos no deben desaparecer. Cuando un artifact sea sustituido, se marca como superseded y se conserva su hash y relación con el sucesor.

La prueba de reproducibilidad no exige que cada compilación futura produzca bytes idénticos si la cadena de herramientas no lo garantiza. Exige poder identificar con precisión las fuentes, dependencias, parámetros, entorno y decisiones que generaron el resultado. Ante un incidente, esta información debe permitir comparar la build afectada con la anterior, localizar cambios y preparar un hotfix o rollback sin improvisar.

## 149.23. Criterio de cierre del Plan de Publicación

Este plan se considera implementado, no solo redactado, cuando Steamworks está configurado con identidad legal verificada; el título ha superado clearance; la store page ES/EN utiliza materiales reales; la build candidata contiene `StoreInitial`, excluye TestLab y contenido `NOT OPEN`; SteamPipe, branches y rollback han sido ensayados; localización, audio, fuente, créditos y notices están autorizados; la revisión de Valve está superada; y existe una decisión manual de publicación respaldada por evidencia.

Mientras cualquiera de esos elementos permanezca abierto, el documento actúa como procedimiento y backlog de preparación. No debe utilizarse para afirmar que el juego está próximo a lanzamiento ni para anunciar una fecha. La autoridad final pertenece al registro de build, al gate legal, al QA de candidata y a la decisión Go/No-Go correspondiente.
