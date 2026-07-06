---
title: "Cartridge & Cloud — Audio Bible"
subtitle: "Identidad sonora, eventos, mezcla, implementación y validación"
author: "VRM Games / Blas Luis Rocha González"
date: "2026-07-06"
lang: es-ES
document_id: "CC-DOC-18"
document_version: "1.0"
status: "CONSOLIDATED / ACTIVE / NOT BASELINE-FROZEN"
project: "Cartridge & Cloud"
platform: "PC / Steam"
engine: "Unity 6.3 LTS 6000.3.18f1"
render_pipeline: "URP 17.3.0"
application_version: "0.0.21"
owner: "VRM Games"
---

# Cartridge & Cloud — Audio Bible

**Archivo:** `18_Audio_Bible.md`  
**Propósito:** establecer la autoridad sonora y el procedimiento de diseño, producción, integración, mezcla y validación de audio de Cartridge & Cloud.  
**Estado del proyecto:** Sprints 0–15 `CLOSED / PASS`; Sprint 16 `COMPLETED / PASS`; Sprint 17 `PENDING / READY TO OPEN`; H6 `BLOCKED / NOT RUN`.  
**Implementación observada:** 10 clips WAV, 23 event IDs, cuatro canales (`Music`, `Ambience`, `Ui`, `Effects`) y un router representativo basado en AudioSource compartidos.  
**Validación vigente:** compilación, EditMode, PlayMode, regresión manual y build externa `0.0.21` en PASS; el audio representativo de Sprint 16 no presenta regresiones bloqueantes.  
**Regla de interpretación:** que un clip exista, que el catálogo lo encuentre o que un test de null-safety pase no equivale a mezcla aprobada, espacialización validada, licencia cerrada ni audio listo para lanzamiento.

## Aceptación representativa de audio de Sprint 16 — 2026-07-06

El conjunto de feedback sonoro incluido en el Golden Path se acepta para el cierre de Sprint 16 tras la regresión manual y la ejecución externa de la build `0.0.21`. La aceptación confirma ausencia de errores bloqueantes y coherencia funcional suficiente para el gate representativo; no equivale a mezcla final, cobertura completa, legal clearance ni signoff H6.

> Esta Audio Bible consolida toda la genealogía disponible, incluidas las Audio Bible v0.3, v0.4 y v0.5; la reedición de v0.3 en las baselines v0.3 y v0.4; los catálogos de contenido v0.1–v0.3; las UI Style Guide y registros legales históricos; los planes, ADR, matrices y registros de Sprint 16; el código y los assets reales; y los documentos consolidados 00–17. Las decisiones históricas no se eliminan: se clasifican como vigentes, sustituidas, diferidas, objetivo o visión.
# 0. Control documental
Esta Audio Bible ocupa la posición `18` dentro de la documentación consolidada. Su autoridad se limita a identidad sonora, música, ambientes, efectos, feedback auditivo, mezcla, spatial audio, importación, catálogos, routing, accesibilidad auditiva, evidencia y criterios de aceptación. No redefine la lógica de gameplay, las transacciones, el modelo de datos ni la composición visual de StoreInitial; solo especifica cómo deben representarse sonoramente los resultados ya confirmados por esos sistemas.
- **DEBE**: requisito obligatorio para la fase o gate indicado.
- **NO DEBE**: práctica prohibida salvo excepción formal.
- **DEBERÍA**: recomendación fuerte cuya omisión requiere justificación.
- **PUEDE**: opción permitida.
- **HISTÓRICO**: decisión preservada por trazabilidad, no necesariamente vigente.
- **OBJETIVO**: resultado deseado todavía no demostrado.
- **IMPLEMENTADO**: existe en código o assets observados.
- **VALIDADO**: existe evidencia auditiva, técnica y de build suficiente.
- **PLACEHOLDER REPRESENTATIVO**: permite probar routing y feedback, pero no fija calidad final.
- **VISIÓN**: dirección futura no comprometida para el primer release.
Debe revisarse cuando cambie el catálogo de eventos, la arquitectura de buses, un AudioMixer, la política de spatial audio, las opciones del usuario, el pipeline de importación, la estrategia musical, los límites de voces, las reglas de licencias o los gates de Sprint 16, Sprint 17 y H6.
# 1. Propósito y resultados esperados
La Audio Bible convierte una colección de ideas históricas, clips breves, event IDs, feedback visual y decisiones técnicas en un lenguaje sonoro ejecutable. Debe permitir crear un clip, conectarlo a un evento, mezclarlo con el resto, probarlo en Editor y build, registrar su procedencia y decidir si está aprobado sin depender de preferencias implícitas.
1. fijar una identidad sonora reconocible y compatible con sesiones largas;
2. comunicar acciones importantes sin obligar a mirar permanentemente la UI;
3. separar el resultado de gameplay de su presentación auditiva;
4. coordinar sonido, texto, icono, animación y VFX;
5. preservar la baja fatiga y evitar cacofonía;
6. distinguir claramente audio implementado, placeholder, validado y futuro;
7. definir eventos, canales, prioridades, variación, cooldown y concurrencia;
8. alinear WAV, import settings, catálogos, router, opciones y builds;
9. establecer evidencia reproducible para Sprint 16, Sprint 17 y H6;
10. proteger licencias, créditos y trazabilidad.
# 2. Jerarquía de autoridad
Cuando exista contradicción, la interpretación seguirá esta jerarquía:
1. `00_Enfoque_y_Alcance.md`: fantasía, límites y tono general.
2. `02_Vertical_Slice_Specification.md`: acciones críticas y aceptación H6.
3. `03_Technical_Design_Document.md` y `04_Modelo_de_Datos.md`: contratos, eventos y separación lógica/presentación.
4. Esta Audio Bible: lenguaje sonoro, mezcla, pipeline y QA auditivo.
5. `05_UX_Flow.md`, `17_Art_Bible.md` y la futura `19_UI_Style_Guide.md`: redundancia visual, ritmo y legibilidad.
6. `07_QA_Testing_Plan.md`, `08_QA_Testing_Matrix.xlsx`: evidencia y gates.
7. `10_Unity_Project_Setup_Guide.md` y `11_Build_y_Versioning_Guide.md`: configuración e importación.
8. `12` y `13`: estado operativo y trazabilidad.
9. Clips, catálogos, scripts y builds reales: evidencia de implementación, no autoridad automática sobre intención.
10. Documentos históricos: fuente de evolución y decisiones no sustituidas.
Un clip no puede convertir una operación fallida en exitosa. Un sonido de checkout no puede dispararse antes del commit. Un evento audible no puede sustituir texto o forma cuando la información es crítica. Una build silenciosa por clip ausente no puede declararse correcta solo porque el gameplay continúa.
# 3. Alcance y exclusiones
Incluye música, ambientes, UI, efectos, personajes, notificaciones, spatial audio, mezcla, importación, runtime, opciones, accesibilidad, licencias, QA y visión futura. Excluye composición musical nota a nota, master final de lanzamiento, casting y dirección de voz completos, localización de diálogos y marketing sonoro externo; esas áreas podrán generar especificaciones o registros adicionales cuando entren en alcance.
La versión actual se centra en el vertical slice y StoreInitial. Publishing, desarrollo interno, ecommerce, plataforma digital e infraestructura aparecen solo como continuidad de identidad; no autorizan producción de audio antes de los gates correspondientes.
# 4. Estado ejecutivo de audio
A fecha de consolidación, el audio se encuentra en estado **base representativa implementada / mezcla y espacialización pendientes**. El paquete contiene diez WAV originales o internos de procedencia que debe quedar registrada, todos mono, 16-bit y 22.050 Hz. Dos clips de cuatro segundos actúan como música y ambiente; ocho clips cortos cubren UI, placement, pedido, puerta, checkout y cierre. El catálogo serializado contiene veintitrés event IDs que reutilizan esos diez clips.
| Área | Estado observado | Interpretación |
| --- | --- | --- |
| Clips | 10 WAV | Cobertura mínima, no biblioteca final |
| Event IDs | 23 | Routing funcional con reutilización intensa |
| Canales | Music, Ambience, Ui, Effects | Cuatro categorías runtime |
| AudioMixer | No existe asset `.mixer` | Buses históricos aún no implementados |
| Spatial audio | Todos los AudioSource se crean con `spatialBlend = 0` | Puerta/checkout todavía 2D |
| Volumen | PlayerPrefs por canal, default 0.8 | Persistencia técnica básica |
| UI de opciones | Ciclo 0 / 0.5 / 1 en Operations | No es menú final de accesibilidad |
| Variación | No implementada | Riesgo de repetición |
| Cooldown | No implementado | Riesgo de spam |
| Tests | catálogo, volumen y missing event seguro | No validan mezcla ni escucha |
| Build post-StoreInitial | PASS Sprint 16 | Validación externa `0.0.21`; mezcla final/H6 aún pendientes |

# 5. Genealogía documental completa
La dirección sonora atraviesa cuatro etapas. La baseline v0.3 definió una visión extensa de preproducción: tienda física, nostalgia tecnológica, baja fatiga, buses, spatial audio, variación, AudioMixer y prioridades. La baseline v0.4 republicó esa Audio Bible v0.3 con la fundación técnica de Sprint 0 ya validada. La baseline v0.5 produjo la Audio Bible v0.4, mucho más breve, que reconocía una dirección aprobada pero sin implementación y reservaba el pase para Sprint 16. La baseline v0.6 produjo la Audio Bible v0.5, que enumeró los diez eventos/clips de Fase 1 y dejó pendiente la validación de niveles, repetición, espacialización, transiciones, mute/volumen y build externa.
Los catálogos de contenido v0.1–v0.3, las UI Style Guide v0.3–v0.5, los registros legales v0.3–v0.5, los handoffs y el charter de Sprint 16 complementan esa genealogía. El código actual implementa parte de la visión: IDs, catálogo, router, cuatro canales, fuentes compartidas, preferencias por canal y fallbacks silenciosos. No implementa todavía AudioMixer, snapshots, buses históricos ampliados, 3D, reverb, variaciones, cooldowns ni una política final de carga.
# 6. Decisiones históricas conservadas, sustituidas y diferidas
| Decisión histórica | Estado consolidado | Aplicación |
| --- | --- | --- |
| Claridad funcional y baja fatiga | VIGENTE | Todo evento debe justificar su presencia |
| Nostalgia tecnológica y tienda cercana | VIGENTE | Timbres cálidos y electrónicos moderados |
| Buses Master/Music/Ambience/SFX/UI/Characters/Notifications/Voice | OBJETIVO EVOLUTIVO | Actual runtime solo tiene cuatro canales |
| AudioLibrary por IDs | IMPLEMENTADO PARCIAL | `AudioCatalog.asset` con 23 IDs |
| AudioService desacoplado | IMPLEMENTADO PARCIAL | Router no muta gameplay |
| Pooling de fuentes | IMPLEMENTADO BÁSICO | Una fuente compartida por canal |
| AudioMixer y snapshots | DIFERIDO | No existe `.mixer` |
| Spatial 3D para puerta/clientes/cajas | PENDIENTE | Todas las fuentes actuales son 2D |
| Varias muestras/pitch moderado | PENDIENTE | Pitch serializado en 1 |
| Cooldown de notificaciones | PENDIENTE | No existe control dedicado |
| Música lo-fi/electrónica suave | DIRECCIÓN VIGENTE | Clip actual es placeholder de cuatro segundos |
| Voice futuro | FUERA DE H6 | No producir sin diseño y localización |
| Audio no bloquea gameplay | VIGENTE | ADR-0071 y fallbacks no-op |
| Alternativa visual para alertas | VIGENTE | FeedbackPresenter muestra texto/VFX además de audio |

# 7. Visión sonora consolidada
Cartridge & Cloud debe sonar como una pequeña tienda de videojuegos activa, ordenada y humana, con una capa tecnológica contemporánea y una nostalgia física discreta. El audio debe sugerir materiales, espacio y actividad sin convertir la tienda en una feria de sonidos. La identidad nace de combinar gestos comerciales reconocibles —puerta, packaging, mostrador, caja, estantería— con confirmaciones digitales limpias y música electrónica suave.
El jugador debe poder comprender que una acción fue aceptada, rechazada, completada o aplazada sin depender únicamente del sonido. La mezcla debe dejar espacio mental para leer cifras, comparar precios y planificar. La música sostiene el ritmo, el ambiente evita sensación de vacío y los SFX comunican causalidad.
# 8. Pilares sonoros
1. **Información antes que decoración.** Cada sonido tiene una función o un valor ambiental controlado.
2. **Baja fatiga.** Repetición, agudos, transitorios y densidad se limitan para sesiones largas.
3. **Causalidad correcta.** El sonido ocurre después de un resultado confirmado.
4. **Mundo físico y capa digital.** Materiales y espacio conviven con feedback de UI.
5. **Jerarquía.** Error crítico, venta, cierre y ambiente no compiten al mismo nivel.
6. **Modularidad.** IDs y categorías permiten sustituir clips sin cambiar gameplay.
7. **Accesibilidad redundante.** Lo importante también se ve y se puede leer.
8. **Escalabilidad.** La solución actual puede evolucionar a mixer, snapshots y voces.
9. **Producción sostenible.** La biblioteca evita una variante única para cada microacción.
10. **Honestidad.** Placeholder, implementación y validación se etiquetan correctamente.
# 9. Experiencia auditiva objetivo
En StoreInitial, el fondo debe resultar estable y discreto. La puerta y las acciones cercanas destacan brevemente; las confirmaciones UI son cortas; la recepción de pedido, la venta y el cierre tienen firmas distintas. El jugador no debe oír una multitud inexistente ni una música triunfal constante. Cuando la tienda se llena, la sensación de actividad aumenta mediante capas y concurrencia limitada, no elevando indiscriminadamente el volumen.
El silencio relativo también comunica: una tienda cerrada, un menú pausado o un error de carga pueden reducir capas. La ausencia de un clip por fallo técnico no debe confundirse con silencio de diseño; se registra y se prueba.
# 10. Jerarquía de audibilidad
| Nivel | Clase | Ejemplos | Regla |
| --- | --- | --- | --- |
| A | Crítico | autosave failed, error bloqueante, cierre | Debe atravesar la mezcla y tener alternativa visual |
| B | Resultado principal | checkout, pedido recibido, placement commit | Breve, distintivo, no duplicado |
| C | Interacción | confirmar, seleccionar, abrir puerta | Legible pero subordinado |
| D | Estado ambiental | ambiente, música, clientes | Sostiene contexto; nunca oculta A–C |
| E | Detalle decorativo | foley menor, equipos, props | Se omite primero ante densidad o presupuesto |

La jerarquía se implementará con volumen relativo, espectro, duración, prioridad de voz, ducking o snapshots cuando exista mixer, y límites de frecuencia. No se resolverá solo haciendo más fuerte cada evento importante.
# 11. Estados semánticos del feedback
El sistema sonoro debe mantener familias semánticas coherentes: positivo, negativo, atención, progreso, economía, mundo y ambiente. Un sonido positivo no puede emplearse para una operación que todavía podría fallar. Un error recuperable debe ser menos agresivo que corrupción de save o bloqueo de progreso.
| Familia | Función | Timbre orientativo | Ejemplos |
| --- | --- | --- | --- |
| Confirmación | acción válida | transitorio claro, ascendente o estable | UI confirm, reserva |
| Rechazo | acción inválida | descendente, seco, breve | placement invalid, out of stock |
| Atención | evento temporal | dos fases o pulso moderado | closing warning |
| Recompensa | resultado económico | firma cálida y limpia | checkout, revenue |
| Operación física | material/objeto | foley estilizado | door, crate, shelf |
| Sistema | save/load/configuración | digital discreto | autosave succeeded/failed |
| Ambiente | espacio y actividad | continuo, ancho y poco intrusivo | store ambience |
| Música | ritmo emocional | loop largo y estable | store music |

# 12. Política de silencio
El silencio no es ausencia de trabajo; es una herramienta de contraste. La música puede reducirse en pantallas densas, eventos de cierre o mensajes críticos. El ambiente puede atenuarse al pausar o al abandonar la escena. No obstante, un canal silenciado por el usuario no debe impedir el feedback visual. La implementación debe distinguir entre silencio voluntario, ausencia de clip, canal a cero y AudioListener deshabilitado.
- no reproducir hover continuo en cada frame;
- no llenar la tienda vacía con crowd loop intenso;
- no mantener sonidos de maquinaria sin objeto visible;
- no usar silencio como única indicación de pausa o error;
- no reiniciar música de forma perceptible por cada apertura de panel.
# 13. Identidad acústica de la tienda
La identidad física se construye con puerta, cartón, plástico, madera, metal ligero, vidrio y superficies de checkout. La identidad digital utiliza tonos limpios, cortos y poco brillantes. La identidad de marca no necesita un jingle constante; puede emerger mediante intervalos, ritmos o timbres reutilizados en confirmaciones, apertura y stingers futuros.
El verde de VRM Games no tiene equivalente literal de frecuencia. La coherencia audiovisual debe basarse en timing, función y familia, no en asociaciones arbitrarias de color a nota.
# 14. Equilibrio entre mundo físico y capa digital
Una puerta tiene cuerpo físico y posición; una confirmación de catálogo pertenece a UI. Una venta combina escaneo/objeto, commit económico y resultado visual. La mezcla puede superponer capas, pero cada una debe tener causa y prioridad. En el vertical slice se preferirá una firma compacta por evento antes que secuencias complejas difíciles de mantener.
# 15. Taxonomía de eventos
| Prefijo objetivo | Dominio | Ejemplo | Observación |
| --- | --- | --- | --- |
| music. | música | music.store | loop |
| ambience. | ambiente | ambience.store | loop |
| ui. | interfaz | ui.confirm | objetivo futuro; actual usa feedback.* |
| world. | mundo físico | world.door.open | objetivo futuro |
| feedback. | resultado transversal | feedback.checkoutcompleted | implementado |
| character. | personajes | character.customer.frustrated | objetivo futuro |
| system. | sistema | system.save.failed | objetivo futuro |
| notification. | avisos | notification.closing.warning | objetivo futuro |

Los prefijos futuros no obligan a una migración inmediata. La tabla sirve para evitar que todos los eventos permanezcan indefinidamente bajo `feedback.*`. Cualquier migración conservará aliases o actualizará trazabilidad y tests.
# 16. Clases de prioridad y políticas de interrupción
Cada entrada futura debería registrar prioridad lógica, límite de concurrencia, cooldown y política de interrupción. Música y ambiente son loops exclusivos por canal; UI puede solaparse de forma limitada; Effects requiere límites por familia. Un error crítico puede duckear momentáneamente música, pero no cortar abruptamente el ambiente salvo transición de escena.
| Clase | Concurrencia sugerida | Cooldown inicial | Interrupción |
| --- | --- | --- | --- |
| Crítico | 1–2 | 0.25–1 s | Puede desplazar detalle decorativo |
| Resultado | 2–4 | 0.1–0.5 s | No duplicar el mismo transaction ID |
| UI | 2–6 | 50–150 ms | Evitar hover repetitivo |
| Foley cercano | 4–8 | según objeto | Prioridad por distancia |
| Ambiente puntual | 2–6 | variable | Virtualizable |
| Loop | 1 por subbus | n/a | Crossfade o stop controlado |

# 17. Canales actuales y buses históricos
El enum actual contiene `Music`, `Ambience`, `Ui` y `Effects`. La Audio Bible v0.3 proponía `Master`, `Music`, `Ambience`, `SFX`, `UI`, `Characters`, `Notifications` y `Voice`. La consolidación mantiene ambos niveles: cuatro canales son verdad implementada; el árbol ampliado es objetivo para una arquitectura con AudioMixer cuando la complejidad lo justifique.
```text
Master
├── Music
├── Ambience
├── SFX
│   ├── World
│   ├── Characters
│   └── Checkout
├── UI
├── Notifications
└── Voice  [future]
```
# 18. Arquitectura objetivo de mezcla
La arquitectura objetivo debe permitir volumen por categoría, mute, snapshots, ducking, diagnóstico y persistencia. No debe introducirse un mixer vacío solo por cumplir el documento: primero se definen necesidades y pruebas. Para H6 es suficiente una mezcla representativa controlable y estable; para lanzamiento se exige un AudioMixer o solución equivalente si la biblioteca supera las capacidades del router simple.
- `Master` controla salida general sin destruir balances internos.
- `Music` y `Ambience` tienen controles independientes.
- `UI` permanece 2D y de baja latencia.
- `SFX/World` admite spatial audio.
- `Notifications` puede tener prioridad y cooldown propios.
- `Voice` permanece fuera de alcance hasta existir guion, localización y subtítulos.
- Los snapshots futuros cubren gameplay, pause, menu, closing y critical alert.
# 19. Implementación runtime observada
`Sprint16Phase1RuntimeRoot` obtiene `AudioCatalog` del `RuntimeAssetRegistry`, crea un `Phase1AudioRouter`, lo conecta a `Phase1FeedbackPresenter` y reproduce `music.store` y `ambience.store` al iniciar la tienda. El router crea un GameObject hijo y un AudioSource por cada uno de los cuatro canales. Las fuentes tienen `playOnAwake = false` y `spatialBlend = 0`, por lo que todo el audio actual es 2D.
Los loops detienen y sustituyen el clip del canal si cambia. Los one-shots se reproducen sobre la fuente compartida del canal. La configuración usa PlayerPrefs `CC_S16_P1_Audio_<Channel>` con valor predeterminado `0.8`. El panel Operations permite ciclar cada canal entre 0 %, 50 % y 100 %. Esta interfaz demuestra control, pero no es un menú final de opciones.
# 20. Inventario de clips implementados
| Clip | Uso actual | Duración | Rol |
| --- | --- | --- | --- |
| StoreMusic.wav | music.store | 4.00 s | loop musical placeholder |
| StoreAmbience.wav | ambience.store | 4.00 s | loop ambiental placeholder |
| UiConfirm.wav | selección, hover, asignación, reserva, gasto, autosave success | 0.18 s | confirmación reutilizada |
| UiError.wav | out of stock, frustración, autosave fail | 0.25 s | error reutilizado |
| PlacementValid.wav | placement valid | 0.24 s | construcción positiva |
| PlacementInvalid.wav | placement invalid | 0.25 s | construcción negativa |
| OrderReceived.wav | restock y order received | 0.40 s | mercancía |
| Door.wav | door open y close | 0.45 s | puerta compartida |
| Checkout.wav | satisfacción, cola, checkout y revenue | 0.30 s | resultado comercial reutilizado |
| DayClosed.wav | closing warning y day closed | 0.65 s | ciclo diario compartido |

La reutilización es aceptable para una fase representativa, pero varias asociaciones son semánticamente demasiado amplias para una mezcla final. `Checkout.wav` no debería expresar simultáneamente entrada en cola, satisfacción, venta e ingreso sin variación o capas. `DayClosed.wav` no debería ser idéntico para warning y cierre definitivo. `Door.wav` necesita variantes o inversión temporal/tímbrica para abrir y cerrar.
# 21. Análisis técnico de los WAV actuales
| Archivo | Formato | Duración | RMS dBFS | Peak dBFS | Dominante aprox. | Observación |
| --- | --- | --- | --- | --- | --- | --- |
| Ambience/StoreAmbience.wav | 1ch / 22050 Hz / 16 bit | 4.00s | -34.2 | -28.7 | 55 Hz | placeholder; revisar escucha y loop |
| Music/StoreMusic.wav | 1ch / 22050 Hz / 16 bit | 4.00s | -25.5 | -19.0 | 110 Hz | placeholder; revisar escucha y loop |
| SFX/Checkout.wav | 1ch / 22050 Hz / 16 bit | 0.30s | -24.7 | -15.5 | 730 Hz | one-shot breve |
| SFX/DayClosed.wav | 1ch / 22050 Hz / 16 bit | 0.65s | -24.6 | -14.8 | 220 Hz | one-shot breve |
| SFX/Door.wav | 1ch / 22050 Hz / 16 bit | 0.45s | -29.8 | -24.9 | 96 Hz | one-shot breve |
| SFX/OrderReceived.wav | 1ch / 22050 Hz / 16 bit | 0.40s | -23.8 | -13.4 | 300 Hz | one-shot breve |
| SFX/PlacementInvalid.wav | 1ch / 22050 Hz / 16 bit | 0.25s | -23.5 | -15.0 | 120 Hz | one-shot breve |
| SFX/PlacementValid.wav | 1ch / 22050 Hz / 16 bit | 0.24s | -22.5 | -11.3 | 442 Hz | one-shot breve |
| UI/UiConfirm.wav | 1ch / 22050 Hz / 16 bit | 0.18s | -23.4 | -14.0 | 761 Hz | one-shot breve |
| UI/UiError.wav | 1ch / 22050 Hz / 16 bit | 0.25s | -24.0 | -15.0 | 184 Hz | one-shot breve |

Los niveles de archivo no muestran peaks cercanos a 0 dBFS, lo que deja headroom; no obstante, el nivel percibido depende del espectro, del importador, del volumen de entrada y del router. Los dos loops son de cuatro segundos, mono y 22.050 Hz. Esa duración es demasiado corta para música de sesión y puede revelar repetición aunque el punto de loop sea técnicamente continuo. La prueba definitiva debe hacerse en Unity y build, no solo mediante métricas offline.
# 22. Dirección musical
La música debe ser electrónica suave, lo-fi o híbrida, con textura tecnológica y calidez, tempo moderado y poca densidad melódica. Debe tolerar lectura de datos, planificación y repetición. Se evitarán loops épicos, drops frecuentes, agudos insistentes, subgraves dominantes y melodías de cuatro compases demasiado reconocibles.
- frases largas o capas que reduzcan la sensación de loop;
- pocos elementos simultáneos durante operación normal;
- variación sutil por hora, reputación o tier solo tras validar la base;
- stingers breves para apertura, cierre, hito o expansión;
- no sincronizar gameplay a beat salvo diseño explícito;
- pausa y menús no deben reiniciar la pista desde cero innecesariamente.
# 23. Estado y tratamiento de StoreMusic.wav
`StoreMusic.wav` es un WAV mono de 4,00 s a 22.050 Hz, importado con force-to-mono, normalize, preload y calidad serializada 0.7. El catálogo lo reproduce en loop a volumen de entrada 0.45. Es suficiente para demostrar una capa musical y el control por canal, pero no alcanza el objetivo histórico de una música que acompañe sesiones largas sin fatiga. Debe etiquetarse como placeholder representativo hasta superar escucha prolongada, loop, mezcla, licencia y build.
# 24. Evolución musical y transiciones
La evolución audible por etapas se conserva como visión. La primera release no necesita un score adaptativo complejo. Una progresión viable: pista base de tienda, variaciones de preparación/abierto/cierre, stingers de hitos y, más adelante, capas para ecommerce, publishing, estudio y plataforma. Toda transición debe ser musicalmente estable y no crear cortes al abrir UI.
| Estado | Tratamiento mínimo | Tratamiento futuro |
| --- | --- | --- |
| Preparación | música reducida o base | capa tranquila |
| Tienda abierta | pista principal | densidad modulada por actividad |
| Closing warning | duck + stinger corto | transición armónica |
| Día cerrado | stinger y fade | tema de resumen |
| Menú principal | pista separada o continuidad controlada | tema de marca |
| Crisis/hito | fuera de H6 | stingers específicos |

# 25. Dirección de ambientes
El ambiente comunica espacio y actividad, no sustituye objetos reales. Debe contener ruido de sala, HVAC suave, electricidad discreta, calle amortiguada y actividad compatible con la escena. Los ambientes se diseñan por capas para poder apagar lo que no corresponde. Una tienda vacía no reproduce murmullo de multitud; una puerta cerrada no deja pasar exterior al mismo nivel que abierta.
# 26. Estado y tratamiento de StoreAmbience.wav
`StoreAmbience.wav` es un WAV mono de 4,00 s a 22.050 Hz, loop a volumen 0.45. Su RMS es notablemente menor que el de música, lo que ayuda a no competir, pero la duración y mono limitan riqueza espacial. Se considera placeholder. El ambiente final debería tener un loop más largo, estéreo o multicapas si el coste lo permite, ausencia de clicks, contenido no identificable por repetición y relación clara con StoreInitial.
# 27. Tienda cerrada, preparación y apertura
La tienda cerrada usa menos actividad, puerta inactiva, equipos reducidos y música más discreta. La apertura puede añadir stinger, puerta, calle y capa comercial. No debe sonar como un interruptor abrupto de todos los loops. La transición debe seguir el reloj lógico y el estado de día, no un temporizador real independiente.
# 28. Exterior, entrada y puerta
El exterior se percibe principalmente desde la entrada. La puerta automática necesita eventos de apertura y cierre distintos o variantes coherentes, posición 3D, rango corto y protección contra spam cuando varios clientes cruzan. El estado visual de la puerta y el trigger son autoridad; el sonido reacciona después. Si la puerta oscila por bug, el audio debe tener cooldown para no amplificar el problema.
# 29. Almacén y recepción
El almacén debe sonar más seco y funcional que la zona comercial: cartón, cajas, ruedas, metal ligero y recepción. `OrderReceived.wav` cubre hoy pedido y reposición; el objetivo es separar llegada de mercancía, apertura de caja, transferencia a backroom y reposición en display. Cada paso sonoro solo ocurre si la operación correspondiente se confirma.
# 30. Clientes y actividad ambiental
La actividad de clientes se construirá con pasos, foley leve, presencia y reacciones puntuales, no con diálogos constantes. La Audio Bible v0.3 reservaba buses Characters y Voice. Para H6 no se exige voz. Los perfiles de cliente pueden diferenciarse por ritmo y selección de foley, pero la lógica de paciencia o compra no debe depender de audio.
# 31. Lenguaje sonoro de UI
UI usa sonidos breves, secos y consistentes. Debe evitarse que cada hover suene; se priorizan cambios de foco significativos, confirmación, cancelación, error y avisos. El canal UI es 2D, no se spatializa, y mantiene latencia baja. Los sonidos no deben parecer recompensas si solo se navega por un panel.
# 32. Hover, selección y navegación
Actualmente `ObjectHovered` y `ObjectSelected` reutilizan `UiConfirm.wav`. Para producción, hover debería ser más ligero o limitarse a elementos importantes. Se implementará debounce por elemento y no por frame. La navegación por tabs puede tener un sonido distinto de confirmación final. Mouse y teclado/controlador deben producir feedback equivalente sin multiplicar eventos.
# 33. Confirmación, cancelación y retorno
Confirmar significa que una intención aceptada ha cruzado el punto de commit o que una selección UI se ha aplicado. Cancelar o cerrar panel requiere una firma neutral o descendente, hoy ausente. Volver atrás no debe sonar como error. Las confirmaciones repetitivas deben mantener una familia y variación mínima.
# 34. Error, bloqueo y aviso
`UiError.wav` cubre out-of-stock, frustración y autosave fallido. Es demasiado amplio para final. Se separarán: rechazo local, aviso operativo, error crítico y fallo de persistencia. El error crítico debe llevar texto, icono y registro. Ningún error debe reproducirse por frame mientras una condición persiste.
# 35. Modales, tutorial y sistema
Abrir un modal no necesita sonido fuerte. Tutorial y guía contextual pueden usar un aviso suave solo en aparición inicial. Guardado y carga usan señales discretas; el éxito de autosave jamás se emite antes de que el repositorio confirme escritura. La falta de clip no puede ocultar un error de save: la UI textual sigue siendo obligatoria.
# 36. Construcción y placement
Placement válido e inválido ya disponen de clips distintos. El evento válido se reproduce después de confirmar posición, rotación, footprint, ocupación, coste y commit. El inválido se reproduce ante intento, no durante cada actualización del ghost. Rotación, selección y retirada pueden añadir sonidos futuros, pero no deben saturar el modo construcción.
| Acción | Audio mínimo | Regla |
| --- | --- | --- |
| Entrar en construcción | opcional discreto | no simular compra |
| Preview válido | preferible visual; audio solo al cambiar estado | evitar loop por frame |
| Preview inválido | preferible visual; audio al intento | cooldown |
| Colocar | PlacementValid | post-commit |
| Rotar | click mecánico suave | solo si cambia orientación |
| Retirar | foley neutro + confirmación | tras devolución a stock |
| Cancelar | UI cancel | sin sonido de error |

# 37. Inventario y transferencias
Las transferencias son atómicas. Audio se emite tras éxito y no por cada mutación interna. Recoger, transferir, recibir y reponer necesitan firmas compatibles con cartón/plástico/metal. Los fallos de capacidad o stock usan señal negativa y texto. Una retry idempotente no reproduce dos veces el mismo resultado.
# 38. Pedidos, proveedores y recepción
Pedido creado, pedido enviado, llegada y recepción son estados distintos. H6 solo necesita los incluidos en el Golden Path. `OrderReceived.wav` representa llegada/reposición; debe verificarse que no se dispare al ordenar antes de la entrega. Las marcas ficticias de proveedores pueden tener pequeñas firmas sonoras futuras, pero no son prioridad.
# 39. Displays y reposición
Asignar producto, reponer y quedar sin stock son eventos frecuentes. La mezcla debe evitar una recompensa exagerada por cada unidad. Reposición puede agrupar sonido por acción o lote. Out-of-stock debe ser comprensible, no punitivo, y contar con indicador visual.
# 40. Productos y packaging
Los seis productos funcionales comparten categorías físicas. El audio no necesita un clip exclusivo por SKU. Se diseñan familias de caja de juego, consola, mando, headset y accesorio, con variaciones de material y peso. Packaging ficticio evita marcas reales también en logos, jingles y sonidos de arranque reconocibles.
# 41. Puerta automática
El clip actual `Door.wav` sirve tanto para abrir como cerrar y se reproduce 2D. El objetivo de Sprint 16/H6 es una puerta visualmente coherente con navegación y sonido localizado. Debe registrar anchor, distancia mínima/máxima, cooldown, estado y prueba con varios clientes. Un sonido de cierre no se reproduce si la puerta permanece abierta por presencia.
# 42. Clientes: satisfacción, frustración y abandono
Satisfacción y frustración usan hoy `Checkout.wav` y `UiError.wav`. La reacción debe ser breve y no caricaturesca. No se requieren voces. Abandono, frustración y error de operación son conceptos distintos; pueden compartir familia, no necesariamente clip. La UI mantiene razón comprensible cuando afecte al jugador.
# 43. Reservas y carrito
Reservar usa confirmación actual. Preparación, recogida y fallo de reserva necesitarán eventos separados cuando entren en el recorrido validado. Un carrito lógico no necesita sonido por cada elemento si eso produce fatiga. Se prioriza cierre de reserva, cambio de estado y error.
# 44. Cola
`QueueEntered` reutiliza `Checkout.wav`, lo que puede hacer sonar una venta antes de ocurrir. Debe sustituirse por un evento discreto o quedar silencioso si la UI y movimiento bastan. La cola no genera un bip por cada cambio de posición. Los sonidos se reservan para entrada, llamada de caja o incidencia relevante.
# 45. Checkout
Checkout es un evento compuesto: preflight, commit, consumo, ingreso y presentación. El sonido principal se emite una vez después del commit idempotente. Puede incluir capa física de escaneo y firma de venta, pero debe evitar dobles reproducciones por eventos `CustomerSatisfied`, `CheckoutCompleted` y `Revenue` derivados de la misma transacción. El futuro router debería aceptar correlation/transaction ID o una política de agregación.
# 46. Economía y ledger
Ingreso y gasto deben diferenciarse, pero no convertir cada cambio de céntimo en sonido. `Revenue` usa Checkout y `Expense` usa UiConfirm. Los sonidos económicos son presentación de movimientos ya registrados. Resumen diario puede agrupar resultados. Saldo crítico requiere aviso visual y auditivo moderado con cooldown.
# 47. Ciclo diario
`ClosingWarning` y `DayClosed` comparten `DayClosed.wav`. Deben separarse: warning anticipa y permite actuar; cierre confirma transición. Apertura, 21:45, cierre y resumen fueron prioridades históricas. Las transiciones se sincronizan con el reloj lógico, guardado y UI. El cierre no debe solaparse con venta o error sin prioridad definida.
# 48. Guardado, carga y recuperación
Autosave success usa `UiConfirm`; autosave fail usa `UiError`. El éxito se mantiene discreto para no interrumpir. El fallo debe sobrevivir a mute mediante texto y estado. Backup-first recovery y restore en dos fases pueden tener feedback únicamente después de validar resultado. Carga no reproduce sonidos de duplicación por cada subsistema restaurado.
# 49. Estados negativos y recuperación
Se diferencian invalid input, operación rechazada, recurso insuficiente, fallo técnico recuperable y fallo crítico. Esa taxonomía evita que todo suene igual. El jugador debe poder identificar severidad sin sobresalto excesivo. El log y la UI son autoridad diagnóstica; el sonido solo ayuda a percibir.
# 50. Sincronización con VFX y animación
Audio, VFX y animación reaccionan al mismo evento confirmado, pero no dependen entre sí. La puerta sincroniza onset con movimiento; checkout con commit; placement con aparición final; recepción con caja/estado. Si una capa falta, las demás continúan. La sincronía se prueba a velocidad normal, pausa, transición y build.
# 51. Spatial audio
La visión histórica asignaba 3D a puerta, clientes, cajas y estaciones, y 2D a UI/notificaciones. La implementación actual es completamente 2D. La migración debe ser selectiva: no basta cambiar `spatialBlend = 1`. Se necesitan anchors, curvas de rolloff, distancias, doppler desactivado o controlado, prioridad y pruebas de cámara orbital.
# 52. Política 2D y 3D
| Tipo | Modo objetivo | Motivo |
| --- | --- | --- |
| Música | 2D | cobertura estable |
| Ambiente base de tienda | 2D o estéreo no posicional | cama global |
| Puerta | 3D | origen físico claro |
| Checkout scanner/terminal | 3D | localización funcional |
| Cajas/recepción | 3D | feedback de zona |
| Clientes/foley | 3D | presencia y distancia |
| UI | 2D | legibilidad |
| Avisos críticos | 2D | no perder por posición |
| Stingers | 2D | transición global |

# 53. Distancias, atenuación y doppler
Para StoreInitial de aproximadamente 10×15 m, los rangos deben ser cortos y funcionales. Puerta y checkout no se oyen a máximo nivel desde cualquier esquina. Se probarán curvas personalizadas con cámara cercana, media y lejana. Doppler se desactiva para objetos cotidianos salvo caso justificado. La atenuación no puede volver inaudible una alerta crítica porque esas alertas permanecen 2D.
# 54. Occlusion, reverb y zonas
No existe plugin spatializer ni AudioMixer observado. Occlusion y reverb son objetivos posteriores. Si se implementan, se evitará simular físicamente cada pared; bastan filtros o snapshots por zona cuando aporten claridad. Almacén puede ser más seco; área de venta más abierta; exterior amortiguado. La complejidad se justifica con escucha A/B y perfilado.
# 55. Variación
Las acciones repetitivas necesitan selección aleatoria controlada, pitch o volumen moderados. La variación no debe cambiar el significado. Se recomienda 2–5 variantes para UI frecuente, puerta, caja y foley; 1–2 para eventos raros. Pitch se mantiene normalmente dentro de ±2–5 %, con excepciones justificadas. La entrada actual tiene pitch 1 para todo.
# 56. Cooldown, debounce y anti-spam
Cada familia define cooldown. Hover se filtra por elemento; invalid placement se emite al intento; out-of-stock no repite mientras el estado no cambie; puerta agrupa cruces; warnings diarios son únicos; autosave success puede omitirse si demasiado frecuente. El cooldown no debe esconder eventos distintos ni impedir un error crítico posterior.
# 57. Concurrencia y límites de voces
ProjectSettings permite 32 voces reales y 512 virtuales, pero eso no es presupuesto de diseño. Para una tienda pequeña se fijan límites mucho menores por familia. Music y Ambience usan una voz principal cada uno; UI 4–6; Effects 12–20 según perfilado; personajes se agregan por distancia. Al superar límite, se descarta o virtualiza lo menos importante, no se eleva la configuración global sin medir.
# 58. Filosofía de mezcla
La mezcla debe preservar headroom, inteligibilidad y comodidad. El orden habitual es UI crítica, resultados, mundo cercano, ambiente y música. La música no compensa un ambiente pobre con volumen. Los sonidos agudos de confirmación se revisan en repetición. La mezcla se valida en auriculares, altavoces comunes y al menos un dispositivo de baja calidad.
# 59. Nivel, headroom y normalización
Los WAV actuales tienen peaks aproximados entre −11 y −29 dBFS. Ese margen no garantiza balance. Se evitará normalizar todos los archivos a máximo idéntico. El nivel objetivo se decide por rol y loudness percibido. Antes de H6 se registra una referencia interna, por ejemplo music/ambience a nivel cómodo y UI/resultados claramente audibles sin clipping. Para release se podrán adoptar métricas LUFS integradas por categoría y true peak, pero no se inventarán valores sin medición en mixer.
# 60. Ducking, snapshots y transiciones de mezcla
La versión actual no implementa ducking ni snapshots. El objetivo mínimo futuro: reducir música durante warning crítico, resumen diario o diálogo; atenuar ambiente en pause/menu; transicionar sin saltos. El ducking debe ser corto, con attack/release suaves, y no activarse por cada UI confirm. Se prueba que no quede un snapshot atascado tras cambiar escena.
# 61. Volúmenes configurables y persistencia
Los cuatro canales almacenan volumen en PlayerPrefs con default 0.8. Operations permite 0, 0.5 y 1. La opción final debería ofrecer Master y categorías, slider continuo o pasos accesibles, mute y restablecer valores. La preferencia es global, no save de partida. Se valida persistencia tras reinicio y ausencia de pops al cambiar valor.
El router actual asigna volumen de entrada y volumen de canal antes de `PlayOneShot`, y vuelve a pasar el mismo producto como `volumeScale`. En Unity, esta combinación puede multiplicar la atenuación dos veces; debe verificarse y corregirse si el resultado efectivo queda cuadrado. Al cambiar volumen durante un loop, `SetChannelVolume` asigna el valor de canal sin conservar explícitamente el volumen de entrada del clip; también requiere prueba.
# 62. Pausa, velocidad y foco
La Audio Bible histórica exigía audio correcto con pausa y velocidades. Debe decidirse qué se pausa: SFX de gameplay sí; música y UI pueden continuar o usar snapshot. `WaitForSecondsRealtime` del feedback visual ya ignora timeScale; el audio debe mantenerse coherente. Pérdida de foco, minimización y `runInBackground` desactivado se prueban en Player.
# 63. Transiciones de escena y ciclo de vida
Los AudioSource son hijos del runtime de Store y se destruyen con él. Música y ambiente pueden reiniciarse al recargar. Se debe decidir si un servicio persistente conserva música entre MainMenu y Store o si cada escena gestiona su propia capa. No pueden coexistir dos roots reproduciendo loops. Los tests deben detectar duplicación tras load/reload y volver al menú.
# 64. Formatos fuente y master
WAV es adecuado como fuente interna. Los masters deben conservar sample rate, bit depth y metadatos de procedencia. Unity puede comprimir al importar. No se distribuirán masters sin necesidad. Para música/ambiente largos se evalúa OGG/Vorbis de importación o Streaming; para UI crítica se prioriza baja latencia. No se convertirá repetidamente entre formatos con pérdida.
# 65. Sample rate, canales y compresión
Todos los clips observados son mono 22.050 Hz, mientras los `.meta` serializan sample rate setting 0, override 44.100, compression format 1, quality 0.7, force-to-mono y normalize. La revisión debe comprobar el resultado importado real en Unity, porque el override puede no aplicarse con setting automático. Música y ambiente finales deberían evaluarse en estéreo y 44.1/48 kHz si aportan calidad; SFX pueden permanecer mono cuando se spatialicen.
# 66. Carga, memoria y streaming
Los diez clips actuales usan loadType serializado 0, preload habilitado y carga en background deshabilitada. Por su tamaño pequeño, el coste es limitado. Esta política no escala a música larga. Clasificación objetivo: UI y one-shots cortos Decompress On Load; SFX medianos Compressed In Memory; música y ambiente largos Streaming tras medir. La decisión se registra por asset y plataforma.
# 67. Loop authoring
Un loop se valida mediante escucha repetida, forma de onda, continuidad de DC, cola y transición. Los loops de cuatro segundos pueden ocultar clicks pero revelar patrón. Se recomienda duración suficiente, crossfade o edición en zero crossings, y un test de 5–10 minutos. Unity no ofrece loop points arbitrarios por defecto en AudioClip; el archivo debe estar preparado o el sistema debe manejar segmentos.
# 68. Convenciones de nombres de archivos
El paquete actual usa PascalCase (`StoreMusic.wav`, `UiConfirm.wav`). Se mantiene para evitar churn inmediato. Para nuevos assets: nombre semántico, categoría, acción, variante y versión de fuente cuando proceda. Ejemplos: `Door_Open_01.wav`, `Checkout_Complete_02.wav`, `UI_Error_Critical_01.wav`. Los nombres de archivos no sustituyen event IDs estables.
# 69. Estructura de carpetas
```text
Assets/_Project/Audio/
├── Ambience/
├── Music/
├── SFX/
│   ├── World/
│   ├── Characters/
│   ├── Checkout/
│   └── System/
└── UI/
    ├── Navigation/
    ├── Confirm/
    └── Error/
```
La estructura actual de cuatro carpetas es válida. Las subcarpetas se añaden solo cuando el volumen de contenido lo requiere. No se duplican clips por escena. Los assets de mixer, snapshots o presets se ubican en una carpeta de configuración claramente gobernada.
# 70. Convenciones de event IDs
Los IDs actuales son lowercase y punto-separados. Deben ser estables, semánticos y ajenos al nombre del archivo. No incluir versión, escena o idioma salvo necesidad. Una entrada puede seleccionar varias variantes en el futuro. Los cambios de ID requieren migración de catálogo, tests y trazabilidad.
| Actual | Problema potencial | Objetivo |
| --- | --- | --- |
| feedback.queueentered | reutiliza sonido de venta | queue.entered o feedback específico |
| feedback.revenue | puede duplicar checkout | agregar por transaction |
| feedback.dooropened/closed | mismo clip | variantes o dos clips |
| feedback.closingwarning/dayclosed | mismo clip | separar warning y confirmación |
| feedback.objecthovered | frecuencia alta | debounce/cooldown |
| music.store | correcto | mantener |
| ambience.store | correcto | mantener o subdividir por estado |

# 71. AudioCatalog actual y objetivo
`AudioCatalog.asset` serializa 23 entradas con referencia directa a clips, canal, volumen, pitch y loop. Esto es preferible a rutas mágicas. `EnsureDefaults()` solo actúa si `_entries` está vacío y usa `Resources.Load("Audio/" + resourceName)`. Los nombres por defecto `MusicStore` y `AmbienceStore` no coinciden con `StoreMusic` y `StoreAmbience`, y los clips no están bajo una carpeta `Resources` observada. Por tanto, el fallback de catálogo vacío probablemente produciría clips nulos; debe repararse o eliminarse a favor de autoría explícita.
El catálogo objetivo puede añadir array de variantes, prioridad, cooldown, concurrency group, spatial mode, distancia, random pitch/volume y mixer group. No se añade complejidad hasta existir tests y casos reales.
# 72. Phase1AudioRouter actual y evolución
El router cumple tres objetivos: búsqueda por ID, fuentes compartidas y preferencias por canal. Tolera ID ausente sin excepción, coherente con ADR-0071. Sus límites: todo 2D, cuatro fuentes, sin mixer, sin stop/fade público, sin cooldown, sin variantes, sin prioridad, sin diagnóstico de missing clip y sin lifecycle explícito.
La evolución recomendada conserva `IPhase1AudioRouter` o introduce una interfaz más rica sin acoplar Domain a Unity. Los eventos se publican después del commit. Presentation decide si reproducir, agrupar o ignorar. El router registra warnings de desarrollo con rate limit, no lanza excepciones en gameplay por asset faltante.
# 73. Deuda técnica específica del audio
| ID | Hallazgo | Severidad operativa | Resolución |
| --- | --- | --- | --- |
| AUD-DEBT-001 | No AudioMixer | A2 | Diseñar buses/snapshots cuando se cierre mezcla |
| AUD-DEBT-002 | Todo audio 2D | A1 para puerta/checkout representativos | Añadir anchors y 3D selectivo |
| AUD-DEBT-003 | Loops de 4 s | A2 | Sustituir o validar con variación/capas |
| AUD-DEBT-004 | 23 eventos reutilizan 10 clips | A2 | Separar familias prioritarias |
| AUD-DEBT-005 | Sin cooldown/variación | A1/A2 | Implementar política por evento |
| AUD-DEBT-006 | Fallback Resources inconsistente | A2 | Reparar defaults y tests |
| AUD-DEBT-007 | Posible doble atenuación PlayOneShot | A1 | Test de volumen y corrección |
| AUD-DEBT-008 | SetChannelVolume puede alterar nivel relativo de loop | A2 | Conservar base gain |
| AUD-DEBT-009 | Sin stop/fade/pause API | A2 | Añadir lifecycle controlado |
| AUD-DEBT-010 | Licencia/procedencia no registrada por clip | A1 documental | Completar registro legal |
| AUD-DEBT-011 | Tests no escuchan mezcla ni duplicación | A1 | Añadir instrumentación/manual |
| AUD-DEBT-012 | Build post-StoreInitial | RESUELTA S16 | Player externo `0.0.21` ejecutado; repetir escucha formal para H6 |

# 74. Fallbacks y ausencia de clips
ADR-0071 establece que la ausencia de audio no invalida una operación de gameplay. Ese principio evita que un null clip bloquee una venta o guardado. No significa que missing audio sea aceptable para H6. El runtime puede no-op; QA registra el defecto. Los eventos críticos conservan texto, icono o estado. En desarrollo, el sistema debería emitir warning rate-limited con event ID y catálogo.
# 75. Pruebas automatizadas
Las pruebas observadas comprueban que el registro contiene AudioCatalog, que `music.store` tiene clip, que el router persiste volumen y que un event ID ausente no lanza excepción. Son útiles, pero no cubren toda la semántica.
| Prueba propuesta | Nivel | Criterio |
| --- | --- | --- |
| Todos los event IDs únicos | EditMode | sin duplicados/blank |
| Todos los clips serializados no null | EditMode | excepto entradas explícitamente opcionales |
| Canal válido y volume/pitch en rango | EditMode | sin valores inválidos |
| Loops solo en Music/Ambience salvo excepción | EditMode | política |
| FeedbackKind tiene mapping | EditMode | 21/21 |
| Missing clip no rompe gameplay | PlayMode | no excepción + warning |
| Volumen efectivo no se cuadra | PlayMode/instrumentación | gain esperado |
| Cambio de canal conserva base gain | PlayMode | loop y one-shot |
| Reload no duplica loops | PlayMode | una fuente activa |
| Transaction checkout reproduce una vez | PlayMode | correlation ID |
| Cooldown de invalid/hover | PlayMode | límite temporal |
| Pause/scene transition | PlayMode | estado coherente |

# 76. Pruebas manuales de escucha
La escucha manual usa un protocolo reproducible. Se registra build, escena, hardware, dispositivo, volumen del sistema, volúmenes internos, recorrido y resultado. No se aprueba con una escucha rápida en Editor.
1. iniciar en silencio y comprobar ruido/clicks al cargar;
2. escuchar música y ambiente durante al menos 10 minutos;
3. repetir placement válido/inválido 20 veces;
4. provocar pedido, reposición, out-of-stock y venta;
5. probar puerta con uno y varios clientes;
6. ejecutar warning y cierre;
7. cambiar cada canal 0/50/100 y reiniciar;
8. pausar, perder foco, cambiar escena y recargar slot;
9. comprobar auriculares y altavoces comunes;
10. registrar fatiga, clipping, repetición, duplicación y eventos ausentes.
# 77. Validación en Editor y build externa
Editor sirve para iterar; Player externo es obligatorio para H6. Se comprueba importación, AudioListener, pérdida de foco, latencia, streaming, logs, scene lifecycle y settings. `Player.log` se revisa por missing clips, errors, warnings repetidos y objetos duplicados. La build se archiva con versión, SHA y evidencia auditiva.
# 78. Profiling y rendimiento
Se usa Unity Profiler/Audio Profiler cuando esté disponible para observar voces reales/virtuales, CPU, memoria, streaming y clips cargados. El objetivo no es usar las 32 voces reales, sino mantener margen. Se prueba tienda equipada, hasta ocho clientes según objetivo heredado, puerta activa, checkout y UI. Un clip comprimido que ahorra memoria pero introduce latencia en UI no es aceptable.
# 79. Accesibilidad auditiva
El jugador puede reducir o silenciar categorías sin perder información. Los sonidos críticos tienen alternativa visual. Se evita depender de pitch puro para distinguir estados; también se usan ritmo, duración, texto e icono. Se considera sensibilidad a sonidos agudos y repetitivos. Las opciones se exponen con etiquetas claras y vista previa controlada.
# 80. Alternativas visuales, subtítulos e indicadores
El vertical slice no requiere subtitular cada bip. Sí requiere indicadores visuales para puerta relevante, errores, pedidos, ventas, cierre y save. Si se incorporan sonidos diegéticos que comunican información no visible —por ejemplo, anuncio o alarma— se añade caption o indicador direccional. Voz futura exige subtítulos, speaker y localización.
# 81. Licencias y procedencia
Todo clip debe registrar origen, autor, fecha, licencia, restricciones, ruta de master, transformaciones y crédito. El registro histórico exige revisar audio antes de Steam. Un archivo en el repositorio no demuestra derecho de distribución. Los clips actuales deben recibir entradas individuales o una declaración first-party verificable antes de baseline pública.
# 82. Audio original, adquirido o generado
| Origen | Requisitos |
| --- | --- |
| Original first-party | autor, fecha, archivos fuente, cesión interna si aplica |
| Librería adquirida | licencia, invoice/URL, restricciones de redistribución |
| Creative Commons | versión, atribución, compatibilidad comercial |
| IA generativa | herramienta, términos, prompt/proceso, revisión legal, no imitar artista |
| Síntesis/procedural | herramienta o código, autor, parámetros y licencia |
| Colaborador externo | contrato/cesión, créditos y masters |

No se usarán sonidos de consolas, sistemas operativos, juegos o marcas reconocibles sin autorización. La identidad ficticia también debe aplicarse a jingles y boot sounds.
# 83. Elementos prohibidos o no aceptables
- clipping audible o picos dolorosos;
- loops con click o patrón de cuatro segundos perceptible en producto final;
- sonido crítico sin alternativa visual;
- audio que se dispara antes del commit;
- doble reproducción por un mismo checkout o save;
- hover o error sonando cada frame;
- música épica constante o demasiado densa;
- multitud audible con tienda vacía;
- spatial audio que se pierde por cámara orbital;
- assets sin licencia/procedencia;
- copias reconocibles de marcas o juegos reales;
- silenciar un error técnico sin registro;
- declarar mezcla aprobada solo por tests automatizados.
# 84. Placeholders y deuda sonora
Los diez clips actuales se clasifican como placeholders representativos salvo evidencia posterior. Pueden permanecer en una build interna para validar routing, pero deben llevar estado en catálogo/producción. La deuda se registra por clip, evento o sistema, con criterio de cierre y gate. No se sustituyen todos a la vez sin rollback; se hace por familia y se compara A/B.
# 85. Criterios de aceptación de Sprint 16
1. Los 23 eventos obligatorios resuelven clip o fallback aprobado.
2. Música y ambiente arrancan una sola vez y no producen clicks bloqueantes.
3. Placement, pedido, checkout, puerta, cierre y error se oyen en contexto.
4. Los eventos críticos tienen feedback visual.
5. No hay spam o doble reproducción evidente.
6. Los cuatro canales cambian volumen y persisten.
7. Puerta y checkout tienen política de localización aprobada, aunque la implementación final pueda cerrarse en Sprint 17 si se documenta.
8. No hay null exceptions por audio.
9. La procedencia de los clips usados en candidata está registrada.
10. Golden Path y build post-StoreInitial incluyen revisión auditiva.
11. Los defectos S0/S1 auditivos están a cero para cierre.
12. La aprobación manual no se sustituye por 1215+70 tests verdes.
# 86. Criterios de Sprint 17
Sprint 17 estabiliza, no amplía indiscriminadamente la biblioteca. Debe resolver repetición, niveles relativos, cooldown, spatial audio selectivo, lifecycle, errores de router y evidencia. Puede sustituir placeholders prioritarios. No debe introducir voz, score adaptativo complejo ni sistemas futuros salvo decisión formal.
# 87. Criterios de H6
1. `VS-AUD-001` PASS: acciones críticas con feedback sonoro o alternativa aprobada.
2. Mezcla representativa en StoreInitial, no solo en Store/blockout.
3. Build Windows x64 externa con Player.log limpio de errores bloqueantes.
4. No duplicación tras load, scene transition o retry.
5. Volúmenes configurables y persistentes.
6. Accesibilidad visual para información crítica.
7. Licencias/procedencia completas para clips incluidos.
8. Revisión de fatiga y repetición en Golden Path y sesión extensa.
9. Profiling sin degradación grave.
10. Excepciones y deuda no bloqueante registradas con owner y fase.
# 88. Flujo de creación de un asset de audio
1. Definir event ID, causa, prioridad, canal y condición de disparo.
2. Determinar si necesita clip nuevo, variante o reutilización.
3. Crear brief con duración, timbre, material, distancia y referencia.
4. Producir master sin clipping y conservar fuente.
5. Editar onset, tail y loop; exportar WAV.
6. Registrar autoría/licencia y hash.
7. Importar con settings según categoría.
8. Añadir al catálogo con gain/pitch/loop.
9. Conectar al evento post-commit.
10. Añadir tests de catálogo y fallback.
11. Escuchar aislado y en mezcla.
12. Validar en build y archivar evidencia.
# 89. Flujo de integración de un evento
1. Identificar el evento de aplicación o presentación que representa el resultado.
2. Evitar disparar desde Domain o desde mutación parcial.
3. Asignar correlation/transaction ID cuando exista riesgo de retry.
4. Resolver entrada del catálogo y política de cooldown/concurrencia.
5. Seleccionar fuente 2D/3D y anchor.
6. Reproducir una vez tras commit.
7. Presentar texto/VFX complementario.
8. Probar success, failure, retry, pause, load y missing clip.
9. Medir y escuchar en contexto.
10. Actualizar trazabilidad y estado de producción.
# 90. Playbook de revisión auditiva por recorrido
La revisión se ejecuta siguiendo el Golden Path y no como una lista desconectada de clips. Cada paso registra evento esperado, clip real, número de reproducciones, canal, posición, alternativa visual y resultado. Los defectos se reproducen con pasos claros.
| Recorrido | Eventos mínimos | Riesgo principal |
| --- | --- | --- |
| Bootstrap/MainMenu | música/menu futuro, UI | duplicación al cambiar escena |
| New Game/Load | confirmaciones, save/load | afirmar éxito antes de tiempo |
| StoreInitial entrada | music.store, ambience.store, door | loops y 2D |
| Construcción | valid/invalid/select | spam |
| Pedido/recepción | orderreceived | estado incorrecto |
| Reposición | restocked/outofstock | reutilización excesiva |
| Cliente/cola | satisfied/frustrated/queue | sonar venta antes de venta |
| Checkout/economía | checkout/revenue/expense | doble evento |
| Cierre/autosave | warning/dayclosed/save | prioridad y claridad |
| Reload/menú | stop/restart | loops duplicados |

# 91. Perfiles de revisión de los 23 eventos actuales
| Event ID | Canal | Clip | Gain | Modo | Riesgo/validación |
| --- | --- | --- | --- | --- | --- |
| music.store | Music | Music/StoreMusic.wav | 0.45 | loop | Loop de 4 s; fatiga/repetición |
| ambience.store | Ambience | Ambience/StoreAmbience.wav | 0.45 | loop | Loop de 4 s; fatiga/repetición |
| feedback.placementvalid | Effects | SFX/PlacementValid.wav | 0.75 | one-shot | Reutilización / falta de variación |
| feedback.placementinvalid | Effects | SFX/PlacementInvalid.wav | 0.75 | one-shot | Reutilización / falta de variación |
| feedback.objectselected | Ui | UI/UiConfirm.wav | 0.75 | one-shot | Reutilización / falta de variación |
| feedback.objecthovered | Ui | UI/UiConfirm.wav | 0.75 | one-shot | Spam por frecuencia |
| feedback.productassigned | Ui | UI/UiConfirm.wav | 0.75 | one-shot | Reutilización / falta de variación |
| feedback.outofstock | Effects | UI/UiError.wav | 0.75 | one-shot | Reutilización / falta de variación |
| feedback.reserved | Effects | UI/UiConfirm.wav | 0.75 | one-shot | Reutilización / falta de variación |
| feedback.restocked | Effects | SFX/OrderReceived.wav | 0.75 | one-shot | Reutilización / falta de variación |
| feedback.orderreceived | Effects | SFX/OrderReceived.wav | 0.75 | one-shot | Reutilización / falta de variación |
| feedback.customersatisfied | Effects | SFX/Checkout.wav | 0.75 | one-shot | Duplicación semántica de venta |
| feedback.customerfrustrated | Effects | UI/UiError.wav | 0.75 | one-shot | Reutilización / falta de variación |
| feedback.queueentered | Effects | SFX/Checkout.wav | 0.75 | one-shot | Duplicación semántica de venta |
| feedback.checkoutcompleted | Effects | SFX/Checkout.wav | 0.75 | one-shot | Duplicación semántica de venta |
| feedback.revenue | Effects | SFX/Checkout.wav | 0.75 | one-shot | Duplicación semántica de venta |
| feedback.expense | Effects | UI/UiConfirm.wav | 0.75 | one-shot | Reutilización / falta de variación |
| feedback.closingwarning | Effects | SFX/DayClosed.wav | 0.75 | one-shot | Warning y cierre indistintos |
| feedback.dayclosed | Effects | SFX/DayClosed.wav | 0.75 | one-shot | Warning y cierre indistintos |
| feedback.autosavesucceeded | Ui | UI/UiConfirm.wav | 0.75 | one-shot | Sincronía con persistencia |
| feedback.autosavefailed | Ui | UI/UiError.wav | 0.75 | one-shot | Sincronía con persistencia |
| feedback.dooropened | Effects | SFX/Door.wav | 0.75 | one-shot | Mismo clip y audio 2D |
| feedback.doorclosed | Effects | SFX/Door.wav | 0.75 | one-shot | Mismo clip y audio 2D |

# 92. Checklist de música y ambiente
- loop sin click y sin patrón obvio;
- duración adecuada para sesión;
- nivel relativo a UI y SFX;
- sin frecuencias fatigantes;
- entrada y salida con fade;
- pause/menu/scene transition coherentes;
- memoria y streaming medidos;
- mono/estéreo decidido conscientemente;
- licencia y master archivados;
- prueba de 30–90 minutos antes de release.
# 93. Checklist de UI, SFX y spatial audio
- onset rápido para UI;
- duración corta y tail controlada;
- familia positiva/negativa consistente;
- cooldown y límite de concurrencia;
- event ID semántico;
- post-commit y una sola reproducción;
- anchor y distancia para 3D;
- doppler/rolloff revisados;
- alternativa visual;
- prueba con canal a cero;
- prueba con missing clip;
- prueba en build externa.
# 94. Paquete de evidencia
Cada campaña auditiva conserva: build ID, commit, versión, plataforma, Player.log, configuración de audio, dispositivo, capturas del mixer/router, tabla de eventos, grabación de referencia cuando proceda, defectos y decisión. Las grabaciones no sustituyen escuchar la build, pero permiten comparar regresiones.
```text
Evidence/Audio/<Campaign>/
├── Audio_Test_Charter.md
├── Event_Checklist.csv
├── Mixer_And_Settings.png
├── Device_And_Volumes.txt
├── Golden_Path_Audio_Record.md
├── Player.log
├── Captures_or_Reference_Recordings/
├── Defects.md
└── Approval.md
```
# 95. Inventario de fuentes históricas y operativas
| Fuente | Rol | Bytes | SHA-256 |
| --- | --- | --- | --- |
| 00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Audio_Bible_v0.3.md | Audio Bible histórica | 3927 | 8530be74a5a22c33b4000ae322d90249eeb7a4567ec123a402c7b5c76a7023c9 |
| 00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Initial_Content_Catalog_v0.1.md | Contenido inicial | 23013 | 5b416cba7975a0ee42762eabb9ea236c30a19091fb3d3031285ac67ab0a83dcb |
| 00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.3.md | Licencias y créditos | 3972 | ffc5bc6af65b27ede021acfdef2dbc5f20f680560e0be1a872586ab146a2f101 |
| 00_Official_Baseline/v0.3/99_Source_Markdown/Cartridge_And_Cloud_UI_Style_Guide_v0.3.md | Coordinación UI | 47855 | d446fa1988af7343cacee8c3817afd3e3fd10f933fe1524d7e0b65f718842065 |
| 00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Audio_Bible_v0.3.md | Audio Bible histórica | 4032 | 9de246aa116d50e54ce46f23ea6e683d337ce3d58f06023af66c068fa6803f4a |
| 00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Initial_Content_Catalog_v0.1.md | Contenido inicial | 23118 | 2bb04a31e3a2661e17e00f23ac0becc61b2c198a36fce87cf9f7aa1c76f07103 |
| 00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.3.md | Licencias y créditos | 4077 | 1a0e921f139b73179fe734eec452dcc3aa8c43860309390fe41ea1fb588fe81f |
| 00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_UI_Style_Guide_v0.3.md | Coordinación UI | 47960 | 8e0d241b40ecc46860cc6c51036f436d85ecfeca0cecfc721897b52396b7fca2 |
| 00_Official_Baseline/v0.5/06_Development_Records/Governance/Current_Project_Baseline_Record.md | Fuente histórica/operativa | 406 | 518e52784ae6c8737a360e221905ab542dd0a7c54d164409b71208556d26f793 |
| 00_Official_Baseline/v0.5/06_Development_Records/Governance/Sprint_History_Summary.md | Fuente histórica/operativa | 328 | d618312c388b749d21af7ddded9d201b6d7b6d0c4ea4f505c3140d70930c1cd1 |
| 00_Official_Baseline/v0.5/06_Development_Records/Handoff/CURRENT_PROJECT_HANDOFF.md | Fuente histórica/operativa | 2275 | 9c251d0f3c14584f5da35a50ab3a5b62564e6f1f656d903fa2590eda74e2e396 |
| 00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Audio_Bible_v0.4.md | Audio Bible histórica | 1955 | b63eff2e7197d01df8375a9d815ddf046d123a222fdaf70079a8356d58c9e092 |
| 00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Initial_Content_Catalog_v0.2.md | Contenido inicial | 2513 | 02439c64569896a24e292ec42717612412828e784bf7c1c701fbc477d57b0d51 |
| 00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.4.md | Licencias y créditos | 2001 | 18f78b69a56edee6f8783d32859d6e231d061422d24abd6feefdbf06fe68567b |
| 00_Official_Baseline/v0.5/99_Source_Markdown/Cartridge_And_Cloud_UI_Style_Guide_v0.4.md | Coordinación UI | 2262 | 8b3346d21414eb370a5e98eb7871f5083cf7bc8d4214ab5cff7ae87ff0b46a22 |
| 00_Official_Baseline/v0.5/CURRENT_PROJECT_HANDOFF.md | Fuente histórica/operativa | 2275 | 9c251d0f3c14584f5da35a50ab3a5b62564e6f1f656d903fa2590eda74e2e396 |
| 00_Official_Baseline/v0.6/06_Development_Records/Governance/Current_Project_Baseline_Record.md | Fuente histórica/operativa | 605 | 98e04d98696a645237fcf80cbca55467a16a09e6bd0d46df1a74637a222ca2e4 |
| 00_Official_Baseline/v0.6/06_Development_Records/Governance/Sprint_History_Summary.md | Fuente histórica/operativa | 858 | db3967d73cb90a9879c223328a922431bf3685b1a8e57afa190511403dc9d1eb |
| 00_Official_Baseline/v0.6/06_Development_Records/Handoff/CURRENT_PROJECT_HANDOFF.md | Fuente histórica/operativa | 1241 | 7a297d84a36ecdd9e4296a631fc5acbff882218d125e63312850e8f28b46cd87 |
| 00_Official_Baseline/v0.6/06_Development_Records/Historical/Sprint_16_Integration_Timeline.md | Sprint 16 / QA / trazabilidad | 1783 | 89fde821c460328fbb8700022883006d2eb8a1ab892c266972b7a1cc207cbcc0 |
| 00_Official_Baseline/v0.6/06_Development_Records/Sprints/Sprint_16/Governance/Sprint_16_Current_Status.md | Sprint 16 / QA / trazabilidad | 665 | bc336c8bf852d942d1b0289cebfdde9db889088990fcc6151df5db559e94d1a9 |
| 00_Official_Baseline/v0.6/06_Development_Records/Sprints/Sprint_16/Governance/Sprint_16_Representative_Asset_Integration_Record.md | Sprint 16 / QA / trazabilidad | 554 | 91133cf2823d732d58fcd23601d6816e610e00375cc38e39737929d88fc4ec54 |
| 00_Official_Baseline/v0.6/06_Development_Records/Sprints/Sprint_16/QA/S16_Acceptance_Matrix.md | Sprint 16 / QA / trazabilidad | 572 | 4ad529aceeaf7e2ea87def0e0be2f8f3998d6218f6ac11a06b40f770e66c44fd |
| 00_Official_Baseline/v0.6/06_Development_Records/Sprints/Sprint_16/QA/S16_QA_Execution_Record.md | Sprint 16 / QA / trazabilidad | 328 | 40c6de3168db5dcf5c71afc166c0a3a2bf34f2dc3e71344c3ccea6176d573766 |
| 00_Official_Baseline/v0.6/06_Development_Records/Sprints/Sprint_16/StoreInitial_Authoring_Plan.md | Sprint 16 / QA / trazabilidad | 741 | f67d195bcda2bd1f861dcf5c53d0a299260c1f04c2f299250856a76616131eba |
| 00_Official_Baseline/v0.6/06_Development_Records/Sprints/Sprint_16/Traceability/S16_Traceability_Entries.csv | Sprint 16 / QA / trazabilidad | 460 | 3263ee5adec14e81588139e9c291ae973115ac75bef79233e6a514ac3643fd67 |
| 00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Audio_Bible_v0.5.md | Audio Bible histórica | 2018 | 3edc3d0db88a36ac7726381883bbf4ffb5f5260a6cb5d972ecf659fbd5116aed |
| 00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Initial_Content_Catalog_v0.3.md | Contenido inicial | 2784 | 68f8841d7f5e83b1078bbf8c110e799165b74f5d75a823839d0acfd3086b333e |
| 00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.5.md | Licencias y créditos | 1951 | 610fc2ca1dd8ea1859fb779039e5de841610646d1e5601d316b2fe34410c3c81 |
| 00_Official_Baseline/v0.6/99_Source_Markdown/Cartridge_And_Cloud_UI_Style_Guide_v0.5.md | Coordinación UI | 2181 | 857f070207a752796ff5c21968020d3230a8c357d84a140593c74c47c59ee645 |
| 00_Official_Baseline/v0.6/CURRENT_PROJECT_HANDOFF.md | Fuente histórica/operativa | 1241 | 7a297d84a36ecdd9e4296a631fc5acbff882218d125e63312850e8f28b46cd87 |
| 10_Development_Records/Handoff/CURRENT_PROJECT_HANDOFF.md | Fuente histórica/operativa | 1168 | ea48666ee43dbf978bef364ee645cd3ff32f31e4e6dfbd96b9edfab316546028 |
| 10_Development_Records/Sprint_16/Governance/Sprint_16_Two_Phase_Charter.md | Sprint 16 / QA / trazabilidad | 640 | b191b8b5d47654141df7860d6a79430552cc68e1bcdc786d1d4fef55583dc605 |
| 10_Development_Records/Sprint_16/Phase_1/Architecture/ADR-0067-Sprint16TwoPhaseDelivery.md | Sprint 16 / QA / trazabilidad | 214 | 59f759e966b057f091473d52cf205d1a089a0917d2f8fff80342ca8ce2ca0a75 |
| 10_Development_Records/Sprint_16/Phase_1/Architecture/ADR-0071-DecoupledPresentationFallbacks.md | Sprint 16 / QA / trazabilidad | 223 | f2e025be2f68b6de2388bd63a6b25549d4e6d111012bc685b034fa120ff9616c |
| 10_Development_Records/Sprint_16/Phase_1/QA/S16_P1_Acceptance_Matrix.md | Sprint 16 / QA / trazabilidad | 1130 | d6d2ca42287be0fced9c3d9da09df5db9729442157035a9a711e0ddbb9d91337 |
| 10_Development_Records/Sprint_16/Phase_1/QA/S16_P1_QA_Execution_Record.md | Sprint 16 / QA / trazabilidad | 443 | a044ba4e7ebb4af622c74397e2a8f0ff6e59ad7e9c1bf89ddb62f5b06bcacec7 |
| 10_Development_Records/Sprint_16/Phase_1/Traceability/S16_P1_Traceability.csv | Sprint 16 / QA / trazabilidad | 1635 | d1e5ada71ed4d81b6a067aa05ea35626d5156967a1f8b212da829be32a4d318d |

# 96. Inventario y hash de documentos consolidados
| Documento | Bytes | SHA-256 |
| --- | --- | --- |
| 00_Enfoque_y_Alcance.md | 127401 | 63dd6f0f1b9c2c80be2aec55718d18d9d031ce4acb14f677769d6e69ceaad21f |
| 01_Game_Design_Document.md | 132822 | a9cd9e3203abc5269c25c59dc7f626b0771df4ec2d65e781109ff146bceb2177 |
| 02_Vertical_Slice_Specification.md | 64531 | 75893f130116e88a08b8da5590dd81876a234581081eafaa8d08374c1a60513f |
| 02_Vertical_Slice_Specification.preview.md | 64531 | 75893f130116e88a08b8da5590dd81876a234581081eafaa8d08374c1a60513f |
| 03_Technical_Design_Document.draft.md | 101994 | 66264703c5ff4959d03093690e9845a98ec0e5025e685b9a639ac8cbb616cd12 |
| 03_Technical_Design_Document.md | 101994 | 66264703c5ff4959d03093690e9845a98ec0e5025e685b9a639ac8cbb616cd12 |
| 04_Modelo_de_Datos.draft.md | 102948 | d851a72b55742c2c704cc7c002929bc5894e7142511c6af843440bb193fb19d4 |
| 04_Modelo_de_Datos.md | 102948 | d851a72b55742c2c704cc7c002929bc5894e7142511c6af843440bb193fb19d4 |
| 05_UX_Flow.md | 56805 | e08da5ff67fb073a8b950babe325ae58ddb2e121513dc2f553acd4f2c14b867e |
| 06_Production_Roadmap_y_Sprint_Plan.md | 61564 | d00b156c0ad1c66069278119c55f76c738a577a3e2835f770208d1a5f2faadff |
| 07_QA_Testing_Plan.md | 79435 | bc9c05a3737e345546ef16e5390e034295ef35ad66c31647852aec81b4e7affe |
| 08_QA_Testing_Matrix.xlsx | 185032 | 233e57f01d2468fe13c136c8fcd3ee75b39bd3de6530349626ea98d4ff254214 |
| 08_QA_Testing_Matrix_preview.png | 176822 | d0535417c725d7a9667f03ef1fe131af2d9f35b3b376e8749872d1e0c7d1cc01 |
| 09_CSharp_Coding_Standards.md | 95321 | 3b734d6cd49f31c09c10bb4941625e143f6c634e3fb50c157f0720e73b1c2a68 |
| 10_Unity_Project_Setup_Guide.md | 78408 | 31430bb0544edd9577323aa0009d6ca3df8b9fbcf6c8a150fc33df69b9f963a4 |
| 11_Build_y_Versioning_Guide.md | 85324 | 38a16a0f97505c0b405509dd3c7e3d1b50e77b89fbe3f4cb1931d3e04f602215 |
| 12_Excel_Maestro_de_Produccion.xlsx | 158100 | 405376cb64f49b34f1f842f3840c12654e9f08e2ec4534b5c149fa811e87dd83 |
| 12_Excel_Maestro_de_Produccion_preview.png | 230168 | 07de2e21fa1e568a006a7a48d21155dc18ef7d7b43dd2054361bc3efc9d3afb9 |
| 13_Trazabilidad_y_Control_de_Cambios.xlsx | 189424 | f674e21d1a3a1f0970a3d339f26f2a2b51a19b5c257c24b9d3fa4118a5cf50eb |
| 13_Trazabilidad_y_Control_de_Cambios_preview.png | 209222 | 0cce6c1f535328a566c2de4846c11e95028da3ebce62dfe8eb48fd315f3caad9 |
| 14_Project_Binder_Indice_Maestro.md | 92828 | 0c24dcd47e0d44793e75e764e3fc2c146e5b1fea04d887d43feb0df558711559 |
| 15_Guia_Maestra.md | 119920 | 879bc1925cccf77e10df6c104302f3afc72f71e8c58dd85ae4f06e84e6f8b624 |
| 16_Auditoria_Global_de_Coherencia.md | 87439 | 4a62376b8aee623be40957ae0511d2dcab2919947f3abb2cef3502b9bb60863e |
| 17_Art_Bible.md | 133913 | 095c6cc42223a972c7faa68435595fc7c22f839fabc62b2038371a0ef6c14239 |

# 97. Matriz de autoridad por decisión
| Decisión | Fuente primaria | Evidencia secundaria |
| --- | --- | --- |
| Acciones críticas de H6 | 02 Vertical Slice | 07/08 QA |
| Eventos y separación post-commit | 03 TDD / 15 Guía | Código runtime |
| Canales actuales | 04 Modelo de Datos / código | AudioCatalog |
| Identidad sonora | 18 Audio Bible | Audio Bible históricas |
| Coordinación visual | 17 Art Bible / 05 UX | FeedbackPresenter |
| Importación y ProjectSettings | 10 Setup Guide | meta y AudioManager.asset |
| Build y Player.log | 11 Build Guide | artefacto |
| Estado Sprint 16 | 12 Producción / registros S16 | handoff |
| Cambios y deuda | 13 Trazabilidad | ADRs |
| Licencias | registro legal futuro/23 | fuentes históricas v0.3–v0.5 |

# 98. Criterios de aceptación de esta Audio Bible
1. Incluye la genealogía v0.3, v0.4 y v0.5 y la reedición histórica de v0.3.
2. Distingue implementación observada de visión y objetivo.
3. Inventaría 10 WAV y 23 event IDs.
4. Documenta cuatro canales actuales y buses históricos.
5. Cubre música, ambiente, UI, mundo, gameplay, spatial, mezcla, importación, QA y licencias.
6. Registra deuda concreta del router y catálogo.
7. Fija criterios separados de Sprint 16, Sprint 17 y H6.
8. Incluye playbooks, checklists y evidencia.
9. No presenta los loops de cuatro segundos como audio final.
10. No permite que audio altere gameplay ni sea única señal crítica.
11. Conserva sistemas futuros como visión no comprometida.
12. Proporciona hashes de fuentes y documentos consolidados.
# 99. Próximos documentos especializados
La siguiente pieza es `19_UI_Style_Guide.md`, que definirá jerarquía visual, componentes, estados, tipografía, resolución, accesibilidad y relación exacta con los eventos sonoros. Después seguirán la especificación de economía/balance, el catálogo de contenido, localización, licencias, Steam y marketing. Esta Audio Bible debe actualizarse cuando esos documentos formalicen captions, opciones, créditos o catálogo final.
## Apéndice A — Matriz de eventos objetivo por sistema
| Sistema | Event ID objetivo | Bus | Causa | Notas |
| --- | --- | --- | --- | --- |
| Aplicación | system.bootstrap.ready | UI | inicio correcto | No requerido en H6; opcional |
| Slots | system.slot.created | UI | slot creado | después de persistencia |
| Slots | system.slot.loaded | UI | slot cargado | no duplicar al entrar |
| Save | system.save.succeeded | UI | guardado confirmado | discreto |
| Save | system.save.failed | Notifications | fallo de guardado | crítico + visual |
| Construcción | build.mode.enter | UI | entrar modo | opcional |
| Construcción | build.place.valid | Effects | colocación | implementado equivalente |
| Construcción | build.place.invalid | Effects | rechazo | implementado equivalente |
| Construcción | build.rotate | Effects | rotación | futuro |
| Construcción | build.remove | Effects | retirada | futuro |
| Inventario | inventory.transfer | Effects | transferencia | agrupar por acción |
| Pedido | order.placed | UI | pedido aceptado | no confundir con recibido |
| Pedido | order.received | Effects | llegada | implementado equivalente |
| Recepción | receiving.crate.open | World | caja abierta | futuro |
| Display | display.assigned | UI | asignación | implementado equivalente |
| Display | display.restocked | Effects | reposición | implementado equivalente |
| Display | display.out_of_stock | Notifications | stock agotado | visual + cooldown |
| Cliente | customer.enter | World | entrada | puerta/foley |
| Cliente | customer.satisfied | Characters | satisfacción | sin voz obligatoria |
| Cliente | customer.frustrated | Characters | frustración | no error técnico |
| Reserva | reservation.created | UI | reserva | implementado equivalente |
| Reserva | reservation.failed | Notifications | reserva fallida | visual |
| Cola | queue.entered | World | entrada en cola | no usar checkout |
| Checkout | checkout.scan | World | escaneo | capa física |
| Checkout | checkout.completed | Effects | venta completada | una vez por transaction |
| Economía | economy.revenue | UI | ingreso | agregar con venta |
| Economía | economy.expense | UI | gasto | discreto |
| Día | day.opened | Notifications | apertura | stinger |
| Día | day.closing_warning | Notifications | aviso | distinto de cierre |
| Día | day.closed | Notifications | cierre | confirmación |
| UI | ui.confirm | UI | confirmar | corto |
| UI | ui.cancel | UI | cancelar | neutral |
| UI | ui.error | UI | error local | no crítico |
| UI | ui.notification | UI | aviso | cooldown |
| Puerta | world.door.open | World | apertura | 3D |
| Puerta | world.door.close | World | cierre | 3D |
| Ambiente | ambience.store.closed | Ambience | preparación | loop/capa |
| Ambiente | ambience.store.open | Ambience | operación | loop/capa |
| Música | music.store | Music | tienda | implementado placeholder |
| Música | music.summary | Music | resumen | futuro |

## Apéndice B — Definition of Ready para audio
Un trabajo de audio está Ready cuando la causa de gameplay está definida, existe event ID estable, se conoce la condición de éxito/fallo, hay owner, canal, prioridad, alternativa visual, licencia prevista, criterio de escucha y build objetivo. No está Ready si la lógica aún cambia cada día, si no se sabe si el evento ocurre una o varias veces o si se pretende usar el sonido para decidir el estado.
## Apéndice C — Definition of Done para audio
Un trabajo de audio está Done cuando el clip/master y su procedencia están registrados; el importador está revisado; el catálogo contiene una entrada válida; el evento se dispara post-commit una vez; existe fallback; tests pasan; la mezcla se escucha en contexto; no hay spam, click, clipping ni repetición bloqueante; la build externa se valida; los defectos quedan cerrados o aceptados; producción y trazabilidad se actualizan.
## Apéndice D — Protocolo de comparación A/B
1. Congelar build y estado de tienda.
2. Igualar canales, dispositivo y volumen del sistema.
3. Capturar A y B con el mismo recorrido.
4. Comparar función, fatiga, claridad, timing y mezcla; no solo gusto.
5. Registrar preferencia y razones.
6. Mantener rollback del asset anterior hasta cerrar QA.
7. Actualizar catálogo sin cambiar event ID cuando la semántica no cambia.
## Apéndice E — Protocolo de rollback
Si un nuevo clip, mixer o router introduce fallo, se revierte la presentación sin revertir gameplay. Se restaura el catálogo/asset anterior, se preservan event IDs, se ejecutan tests y Golden Path, y se registra la causa. No se eliminan masters o licencias durante la prueba. Los fallbacks procedurales o silenciosos se retiran solo después de una build representativa aprobada.
## Apéndice F — Antipatrones específicos
| Antipatrón | Consecuencia | Corrección |
| --- | --- | --- |
| Un sonido por microacción | fatiga y mantenimiento | agrupar por resultado |
| Todos los éxitos usan el mismo bip | ambigüedad | familias semánticas |
| Todo error es alarma | estrés | severidad gradual |
| Más volumen = más importancia | mezcla plana | prioridad/espectro/ducking |
| SpatialBlend 1 para todo | pérdida de UI | política 2D/3D |
| Audio desde Domain | acoplamiento | eventos de presentación |
| Sonido antes del commit | mentira causal | post-commit |
| Reproducir por Update | spam | edge/click/correlation |
| Duplicar clip por escena | descontrol | catálogo central |
| No-op sin warning en desarrollo | deuda invisible | diagnóstico rate-limited |
| Mixer complejo sin necesidad | coste y confusión | evolución incremental |
| Usar clip comercial reconocible | riesgo legal | original/licenciado |

## Apéndice G — Campaña de 90 minutos
La dirección histórica exige sesiones largas sin fatiga. Antes de release se ejecuta una campaña de al menos 90 minutos con música, ambiente, ventas, pedidos y construcción. Se registran momentos de irritación, sonidos demasiado frecuentes, loops reconocibles, balance y necesidad de bajar canales. Para H6 puede ejecutarse una sesión más corta si el audio sigue siendo representativo, pero la campaña completa permanece gate de preparación de lanzamiento.
## Apéndice H — Registro mínimo de defecto auditivo
| Campo | Contenido |
| --- | --- |
| Defect ID | AUD-BUG-### |
| Build/commit | identidad exacta |
| Escena/estado | StoreInitial y slot |
| Event ID | ID esperado |
| Clip/canal | asset y bus |
| Pasos | reproducción |
| Resultado esperado/actual | incluye número de reproducciones |
| Dispositivo/volúmenes | contexto de escucha |
| Severidad | S0–S4 según impacto |
| Evidencia | grabación/log/captura |
| Resolución | clip, routing, mix o lógica de presentación |

## Apéndice I — Severidad auditiva orientativa
| Severidad | Ejemplo |
| --- | --- |
| S0 | audio causa crash, corrupción o imposibilidad de arrancar |
| S1 | error crítico inaudible y sin alternativa, loop ensordecedor, duplicación masiva |
| S2 | evento importante ausente, mezcla bloqueante, puerta/checkout incoherentes |
| S3 | variación insuficiente, balance menor, click ocasional no bloqueante |
| S4 | preferencia estética, refinamiento o contenido futuro |

## Apéndice J — Reglas para sistemas futuros
Ecommerce puede añadir notificaciones de pedido y logística; publishing, stingers editoriales; desarrollo interno, ambiente de estudio; plataforma, UI digital; infraestructura, alertas y hums. Ninguna de estas capas se produce antes de autorizar su fase. Deben heredar claridad, baja fatiga, buses, accesibilidad y licencias, sin invadir StoreInitial.
## Apéndice K — Perfiles operativos detallados de cada event ID actual
### music.store — Música base de tienda
**Estado actual.** El catálogo dirige este evento al clip `Music/StoreMusic.wav`, canal `Music`, gain de entrada `0.45`, pitch `1.00` y modo `loop`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Debe iniciar una vez al entrar en el runtime válido y permanecer subordinada a UI y resultados. Se valida loop, restart, fade y persistencia del canal. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** No debe reiniciarse al abrir Operations ni duplicarse tras load. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### ambience.store — Cama ambiental de tienda
**Estado actual.** El catálogo dirige este evento al clip `Ambience/StoreAmbience.wav`, canal `Ambience`, gain de entrada `0.45`, pitch `1.00` y modo `loop`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Debe sostener espacio y actividad sin sugerir clientes u objetos inexistentes. Se prueba con tienda vacía, abierta y cerrada. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** No debe enmascarar puerta, checkout o aviso de cierre. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.placementvalid — Colocación confirmada
**Estado actual.** El catálogo dirige este evento al clip `SFX/PlacementValid.wav`, canal `Effects`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Se dispara después de validar footprint, coste, ocupación y commit. Debe sentirse positiva y física, no como recompensa económica. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** No se dispara al mover el preview ni antes del commit. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.placementinvalid — Intento de colocación rechazado
**Estado actual.** El catálogo dirige este evento al clip `SFX/PlacementInvalid.wav`, canal `Effects`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Se dispara por intento consciente o transición de estado, con cooldown. Debe complementar el motivo visual. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** No se reproduce cada frame mientras el ghost permanezca inválido. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.objectselected — Selección de objeto
**Estado actual.** El catálogo dirige este evento al clip `UI/UiConfirm.wav`, canal `Ui`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Confirma cambio de selección, no compra ni placement. Debe ser más ligero que una confirmación final. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** No debe sonar al seleccionar repetidamente el mismo objeto. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.objecthovered — Hover significativo
**Estado actual.** El catálogo dirige este evento al clip `UI/UiConfirm.wav`, canal `Ui`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Solo se usa cuando aporta foco real y con debounce. En listas densas puede omitirse. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** Es el evento con mayor riesgo de spam y fatiga. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.productassigned — Asignación de producto a display
**Estado actual.** El catálogo dirige este evento al clip `UI/UiConfirm.wav`, canal `Ui`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Confirma que la asignación quedó aplicada y compatible. Puede compartir familia UI, pero no debe fingir reposición. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** Se emite una vez tras actualizar la asignación. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.outofstock — Falta de stock
**Estado actual.** El catálogo dirige este evento al clip `UI/UiError.wav`, canal `Effects`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Advierte de una condición operativa y conserva explicación visual. Debe ser menos grave que save failed. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** No se repite mientras el mismo estado permanezca sin cambio. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.reserved — Unidad reservada
**Estado actual.** El catálogo dirige este evento al clip `UI/UiConfirm.wav`, canal `Effects`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Confirma una reserva lógica válida. Debe evitar confundirse con venta o cobro. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** No se emite si la reserva falla o se revierte antes de commit. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.restocked — Reposición completada
**Estado actual.** El catálogo dirige este evento al clip `SFX/OrderReceived.wav`, canal `Effects`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Representa transferencia efectiva a display. Puede usar foley de packaging y confirmación suave. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** Debe agruparse por acción, no por cada unidad si se reponen muchas. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.orderreceived — Pedido llegado
**Estado actual.** El catálogo dirige este evento al clip `SFX/OrderReceived.wav`, canal `Effects`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Marca cambio de pedido a recibido y disponibilidad de mercancía. Debe distinguirse de pedido creado. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** No se dispara al pagar o enviar el pedido. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.customersatisfied — Cliente satisfecho
**Estado actual.** El catálogo dirige este evento al clip `SFX/Checkout.wav`, canal `Effects`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Reacción breve, no voz obligatoria. Debe ser menos dominante que checkout. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** No debe duplicar la firma de venta cuando ambos eventos nacen de la misma transacción. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.customerfrustrated — Cliente frustrado
**Estado actual.** El catálogo dirige este evento al clip `UI/UiError.wav`, canal `Effects`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Señal humana moderada, distinta de error técnico. La causa permanece en UI si afecta a decisiones. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** Evitar caricatura, alarma o repetición por cada tick de paciencia. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.queueentered — Entrada en cola
**Estado actual.** El catálogo dirige este evento al clip `SFX/Checkout.wav`, canal `Effects`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Puede ser silencioso o un foley discreto; no debe usar firma de checkout final. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** No se reproduce por cada avance de posición en la cola. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.checkoutcompleted — Checkout completado
**Estado actual.** El catálogo dirige este evento al clip `SFX/Checkout.wav`, canal `Effects`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Es la firma principal de venta y ocurre una vez tras commit idempotente. Puede agrupar scanner y confirmación. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** Se verifica con retry, doble input y reload para evitar duplicación. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.revenue — Ingreso registrado
**Estado actual.** El catálogo dirige este evento al clip `SFX/Checkout.wav`, canal `Effects`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Presenta un movimiento ya escrito en ledger. Puede agregarse a checkout para no sonar dos veces. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** No se emite por proyección o preview de precio. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.expense — Gasto registrado
**Estado actual.** El catálogo dirige este evento al clip `UI/UiConfirm.wav`, canal `Effects`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Debe ser informativo y menos celebratorio. Pedidos y costes grandes pueden compartir familia, no necesariamente clip. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** No se reproduce si el pedido es rechazado y el saldo no cambia. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.closingwarning — Aviso previo al cierre
**Estado actual.** El catálogo dirige este evento al clip `SFX/DayClosed.wav`, canal `Effects`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Debe captar atención sin sonar a cierre definitivo. Se emite una vez por día o ventana. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** Requiere alternativa visual y prioridad sobre ambiente. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.dayclosed — Día cerrado
**Estado actual.** El catálogo dirige este evento al clip `SFX/DayClosed.wav`, canal `Effects`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Confirma transición, resumen y bloqueo de nuevas acciones. Debe diferenciarse del warning. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** No se dispara antes de completar transacciones o guardado según diseño. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.autosavesucceeded — Autosave correcto
**Estado actual.** El catálogo dirige este evento al clip `UI/UiConfirm.wav`, canal `Ui`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Confirmación muy discreta; puede ser opcional si resulta frecuente. Siempre posterior a escritura validada. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** No debe interrumpir ni sonar durante cada cambio de snapshot. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.autosavefailed — Autosave fallido
**Estado actual.** El catálogo dirige este evento al clip `UI/UiError.wav`, canal `Ui`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Evento crítico con texto persistente y log. Debe distinguirse de invalid input. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** El canal a cero no puede ocultar el estado de error. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.dooropened — Puerta abierta
**Estado actual.** El catálogo dirige este evento al clip `SFX/Door.wav`, canal `Effects`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Foley 3D sincronizado con inicio o punto mecánico de apertura. Requiere anchor y cooldown. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** No se dispara si el controlador no cambia de estado. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
### feedback.doorclosed — Puerta cerrada
**Estado actual.** El catálogo dirige este evento al clip `SFX/Door.wav`, canal `Effects`, gain de entrada `0.75`, pitch `1.00` y modo `one-shot`. Esta asociación demuestra routing, pero no implica que el clip sea definitivo ni que la mezcla esté aprobada.
**Intención y timing.** Foley 3D distinto o variante coherente con cierre. Debe respetar presencia y navegación. La autoridad del resultado pertenece al sistema de aplicación o al commit correspondiente; Presentation solo reacciona. El evento se observa junto con texto, icono, VFX o cambio de estado para confirmar que ninguna capa contradice a otra.
**Protecciones.** No se reproduce mientras la puerta permanece abierta o bloqueada. Debe registrarse número de reproducciones, contexto, canal, dispositivo y build. Cuando exista riesgo de retry o de varios eventos derivados, se usa correlation ID, agregación o una política explícita para que la presentación siga siendo idempotente.
**Prueba mínima.** Ejecutar success, failure o no-op según corresponda; repetir la acción de forma rápida; cambiar volumen a 0/50/100; recargar escena o slot cuando aplique; retirar temporalmente el clip; revisar Player.log; y escuchar en la mezcla completa. El resultado se marca PASS solo si el evento es audible cuando debe, silencioso cuando no debe, no se duplica y conserva alternativa visual para información crítica.
## Apéndice L — Perfiles técnicos y creativos de los diez clips actuales
### StoreAmbience.wav — ambiente
**Datos observados.** Duración `4.000 s`, `1` canal, `22050 Hz`, `16` bit, RMS aproximado `-34.16 dBFS`, peak `-28.66 dBFS`, frecuencia dominante aproximada `55.0 Hz`, tamaño `176444` bytes y SHA-256 `2faead25215be67735257d0cbdf6912b852da904f069d9298703e3c1baf88b1f`. Estas métricas describen el archivo, no sustituyen la escucha perceptual.
**Uso y dirección.** Cama global que demuestra Ambience. Debe evolucionar hacia un ambiente más largo, contextual y posiblemente estéreo, sin multitud ficticia ni detalles demasiado reconocibles. El importador serializado fuerza mono y normaliza, usa calidad 0.7, preload y load type 0. La decisión final de compresión y canales se tomará según categoría, longitud, latencia y perfilado, no copiando settings de forma indiscriminada.
**Criterios de revisión.** Comprobar onset, tail, ruido, click, espectro, dinámica, relación con la acción, nivel relativo, repetición, acceso al master y licencia. Para loops se escucha durante varios minutos y se observa la transición; para one-shots se repite en ráfaga y dentro del Golden Path. Un reemplazo mantiene el event ID para preservar contratos.
**Estado consolidado.** Placeholder representativo implementado. Puede recibir PASS técnico de importación y routing mientras permanezca PENDING en calidad/mezcla. La retirada solo ocurre después de que el reemplazo pase pruebas y exista rollback.
### StoreMusic.wav — música
**Datos observados.** Duración `4.000 s`, `1` canal, `22050 Hz`, `16` bit, RMS aproximado `-25.46 dBFS`, peak `-19.03 dBFS`, frecuencia dominante aproximada `110.0 Hz`, tamaño `176444` bytes y SHA-256 `db0e76e00a881fb36c8865510db3aeb52e8ab574c185eff0d7cb58febafcd9bc`. Estas métricas describen el archivo, no sustituyen la escucha perceptual.
**Uso y dirección.** Loop base que demuestra el canal Music. Debe sustituirse por una pieza más larga o un sistema de capas si la repetición se percibe. La revisión escucha fraseo, ruido de borde, balance con ambiente y reinicio tras escenas. El importador serializado fuerza mono y normaliza, usa calidad 0.7, preload y load type 0. La decisión final de compresión y canales se tomará según categoría, longitud, latencia y perfilado, no copiando settings de forma indiscriminada.
**Criterios de revisión.** Comprobar onset, tail, ruido, click, espectro, dinámica, relación con la acción, nivel relativo, repetición, acceso al master y licencia. Para loops se escucha durante varios minutos y se observa la transición; para one-shots se repite en ráfaga y dentro del Golden Path. Un reemplazo mantiene el event ID para preservar contratos.
**Estado consolidado.** Placeholder representativo implementado. Puede recibir PASS técnico de importación y routing mientras permanezca PENDING en calidad/mezcla. La retirada solo ocurre después de que el reemplazo pase pruebas y exista rollback.
### Checkout.wav — resultado comercial
**Datos observados.** Duración `0.300 s`, `1` canal, `22050 Hz`, `16` bit, RMS aproximado `-24.70 dBFS`, peak `-15.47 dBFS`, frecuencia dominante aproximada `730.0 Hz`, tamaño `13274` bytes y SHA-256 `007c220b2d74180a975910331e738ab7911356ac8f01a8bc51b1e6790a807114`. Estas métricas describen el archivo, no sustituyen la escucha perceptual.
**Uso y dirección.** Se usa en cuatro eventos distintos. Debe quedar reservado a checkout o dividirse en scanner, venta y satisfacción para evitar dobles significados. El importador serializado fuerza mono y normaliza, usa calidad 0.7, preload y load type 0. La decisión final de compresión y canales se tomará según categoría, longitud, latencia y perfilado, no copiando settings de forma indiscriminada.
**Criterios de revisión.** Comprobar onset, tail, ruido, click, espectro, dinámica, relación con la acción, nivel relativo, repetición, acceso al master y licencia. Para loops se escucha durante varios minutos y se observa la transición; para one-shots se repite en ráfaga y dentro del Golden Path. Un reemplazo mantiene el event ID para preservar contratos.
**Estado consolidado.** Placeholder representativo implementado. Puede recibir PASS técnico de importación y routing mientras permanezca PENDING en calidad/mezcla. La retirada solo ocurre después de que el reemplazo pase pruebas y exista rollback.
### DayClosed.wav — ciclo diario
**Datos observados.** Duración `0.650 s`, `1` canal, `22050 Hz`, `16` bit, RMS aproximado `-24.58 dBFS`, peak `-14.79 dBFS`, frecuencia dominante aproximada `220.0 Hz`, tamaño `28708` bytes y SHA-256 `31705ae11eda7f71a53ddfb9a2af66fa120c1e19f0c86a9c98d5e1c490dcfdf1`. Estas métricas describen el archivo, no sustituyen la escucha perceptual.
**Uso y dirección.** Se usa para warning y cierre. Debe separarse en anticipación y confirmación, con prioridades y transiciones musicales coherentes. El importador serializado fuerza mono y normaliza, usa calidad 0.7, preload y load type 0. La decisión final de compresión y canales se tomará según categoría, longitud, latencia y perfilado, no copiando settings de forma indiscriminada.
**Criterios de revisión.** Comprobar onset, tail, ruido, click, espectro, dinámica, relación con la acción, nivel relativo, repetición, acceso al master y licencia. Para loops se escucha durante varios minutos y se observa la transición; para one-shots se repite en ráfaga y dentro del Golden Path. Un reemplazo mantiene el event ID para preservar contratos.
**Estado consolidado.** Placeholder representativo implementado. Puede recibir PASS técnico de importación y routing mientras permanezca PENDING en calidad/mezcla. La retirada solo ocurre después de que el reemplazo pase pruebas y exista rollback.
### Door.wav — mundo físico
**Datos observados.** Duración `0.450 s`, `1` canal, `22050 Hz`, `16` bit, RMS aproximado `-29.78 dBFS`, peak `-24.85 dBFS`, frecuencia dominante aproximada `95.6 Hz`, tamaño `19888` bytes y SHA-256 `6c93c93ae3aaa6c97ea5eb372b762c330521534ff1c31f742caf1de7c7477e6e`. Estas métricas describen el archivo, no sustituyen la escucha perceptual.
**Uso y dirección.** Clip compartido de apertura/cierre. Debe convertirse en dos variantes o un diseño reversible convincente y pasar a 3D con anchor de puerta. El importador serializado fuerza mono y normaliza, usa calidad 0.7, preload y load type 0. La decisión final de compresión y canales se tomará según categoría, longitud, latencia y perfilado, no copiando settings de forma indiscriminada.
**Criterios de revisión.** Comprobar onset, tail, ruido, click, espectro, dinámica, relación con la acción, nivel relativo, repetición, acceso al master y licencia. Para loops se escucha durante varios minutos y se observa la transición; para one-shots se repite en ráfaga y dentro del Golden Path. Un reemplazo mantiene el event ID para preservar contratos.
**Estado consolidado.** Placeholder representativo implementado. Puede recibir PASS técnico de importación y routing mientras permanezca PENDING en calidad/mezcla. La retirada solo ocurre después de que el reemplazo pase pruebas y exista rollback.
### OrderReceived.wav — mercancía
**Datos observados.** Duración `0.400 s`, `1` canal, `22050 Hz`, `16` bit, RMS aproximado `-23.75 dBFS`, peak `-13.41 dBFS`, frecuencia dominante aproximada `300.0 Hz`, tamaño `17684` bytes y SHA-256 `2f7ea1961f60dfb3656d86656b1249116c3b71461a9756777622de77baf0517b`. Estas métricas describen el archivo, no sustituyen la escucha perceptual.
**Uso y dirección.** Se usa en recepción y reposición. Puede conservarse como capa digital, pero el foley físico de caja o stock debería diferenciar estados. El importador serializado fuerza mono y normaliza, usa calidad 0.7, preload y load type 0. La decisión final de compresión y canales se tomará según categoría, longitud, latencia y perfilado, no copiando settings de forma indiscriminada.
**Criterios de revisión.** Comprobar onset, tail, ruido, click, espectro, dinámica, relación con la acción, nivel relativo, repetición, acceso al master y licencia. Para loops se escucha durante varios minutos y se observa la transición; para one-shots se repite en ráfaga y dentro del Golden Path. Un reemplazo mantiene el event ID para preservar contratos.
**Estado consolidado.** Placeholder representativo implementado. Puede recibir PASS técnico de importación y routing mientras permanezca PENDING en calidad/mezcla. La retirada solo ocurre después de que el reemplazo pase pruebas y exista rollback.
### PlacementInvalid.wav — construcción negativa
**Datos observados.** Duración `0.250 s`, `1` canal, `22050 Hz`, `16` bit, RMS aproximado `-23.48 dBFS`, peak `-15.02 dBFS`, frecuencia dominante aproximada `120.0 Hz`, tamaño `11068` bytes y SHA-256 `93fea3a564062cd9488922b3f079c9d18fb6bba2d0200bef2b4e5c28610a9b58`. Estas métricas describen el archivo, no sustituyen la escucha perceptual.
**Uso y dirección.** Rechazo corto. Requiere cooldown y espectro no agresivo. Debe seguir siendo legible a volumen bajo sin castigar al jugador. El importador serializado fuerza mono y normaliza, usa calidad 0.7, preload y load type 0. La decisión final de compresión y canales se tomará según categoría, longitud, latencia y perfilado, no copiando settings de forma indiscriminada.
**Criterios de revisión.** Comprobar onset, tail, ruido, click, espectro, dinámica, relación con la acción, nivel relativo, repetición, acceso al master y licencia. Para loops se escucha durante varios minutos y se observa la transición; para one-shots se repite en ráfaga y dentro del Golden Path. Un reemplazo mantiene el event ID para preservar contratos.
**Estado consolidado.** Placeholder representativo implementado. Puede recibir PASS técnico de importación y routing mientras permanezca PENDING en calidad/mezcla. La retirada solo ocurre después de que el reemplazo pase pruebas y exista rollback.
### PlacementValid.wav — construcción positiva
**Datos observados.** Duración `0.240 s`, `1` canal, `22050 Hz`, `16` bit, RMS aproximado `-22.51 dBFS`, peak `-11.28 dBFS`, frecuencia dominante aproximada `441.7 Hz`, tamaño `10628` bytes y SHA-256 `65e6ea5eb176a8f2953abb4f37c2b472899471f404a51407dfd0a7fa4f68224b`. Estas métricas describen el archivo, no sustituyen la escucha perceptual.
**Uso y dirección.** Transitorio de colocación. Se evalúa junto a material del mueble, VFX y commit. Puede recibir variantes moderadas si el jugador coloca muchos objetos. El importador serializado fuerza mono y normaliza, usa calidad 0.7, preload y load type 0. La decisión final de compresión y canales se tomará según categoría, longitud, latencia y perfilado, no copiando settings de forma indiscriminada.
**Criterios de revisión.** Comprobar onset, tail, ruido, click, espectro, dinámica, relación con la acción, nivel relativo, repetición, acceso al master y licencia. Para loops se escucha durante varios minutos y se observa la transición; para one-shots se repite en ráfaga y dentro del Golden Path. Un reemplazo mantiene el event ID para preservar contratos.
**Estado consolidado.** Placeholder representativo implementado. Puede recibir PASS técnico de importación y routing mientras permanezca PENDING en calidad/mezcla. La retirada solo ocurre después de que el reemplazo pase pruebas y exista rollback.
### UiConfirm.wav — UI positiva
**Datos observados.** Duración `0.180 s`, `1` canal, `22050 Hz`, `16` bit, RMS aproximado `-23.40 dBFS`, peak `-14.02 dBFS`, frecuencia dominante aproximada `761.1 Hz`, tamaño `7982` bytes y SHA-256 `7ffea9f36d9c6b6f76941ac5ba8196b8a51b71cc0175d1b888e82e0305296c03`. Estas métricas describen el archivo, no sustituyen la escucha perceptual.
**Uso y dirección.** Clip más reutilizado. Su uso en hover, selección, asignación, reserva, gasto y autosave success exige separar semánticas o reducir frecuencia. Debe tolerar repetición sin fatiga. El importador serializado fuerza mono y normaliza, usa calidad 0.7, preload y load type 0. La decisión final de compresión y canales se tomará según categoría, longitud, latencia y perfilado, no copiando settings de forma indiscriminada.
**Criterios de revisión.** Comprobar onset, tail, ruido, click, espectro, dinámica, relación con la acción, nivel relativo, repetición, acceso al master y licencia. Para loops se escucha durante varios minutos y se observa la transición; para one-shots se repite en ráfaga y dentro del Golden Path. Un reemplazo mantiene el event ID para preservar contratos.
**Estado consolidado.** Placeholder representativo implementado. Puede recibir PASS técnico de importación y routing mientras permanezca PENDING en calidad/mezcla. La retirada solo ocurre después de que el reemplazo pase pruebas y exista rollback.
### UiError.wav — UI negativa
**Datos observados.** Duración `0.250 s`, `1` canal, `22050 Hz`, `16` bit, RMS aproximado `-24.01 dBFS`, peak `-14.99 dBFS`, frecuencia dominante aproximada `184.0 Hz`, tamaño `11068` bytes y SHA-256 `e11141e228dc3d95422b0609d0e271cf562eb43a465d6bf4680d69e7ecb3ba96`. Estas métricas describen el archivo, no sustituyen la escucha perceptual.
**Uso y dirección.** Representa out-of-stock, frustración y autosave fail. Debe dividirse por severidad antes de lanzamiento para que un fallo crítico no suene igual que un rechazo local. El importador serializado fuerza mono y normaliza, usa calidad 0.7, preload y load type 0. La decisión final de compresión y canales se tomará según categoría, longitud, latencia y perfilado, no copiando settings de forma indiscriminada.
**Criterios de revisión.** Comprobar onset, tail, ruido, click, espectro, dinámica, relación con la acción, nivel relativo, repetición, acceso al master y licencia. Para loops se escucha durante varios minutos y se observa la transición; para one-shots se repite en ráfaga y dentro del Golden Path. Un reemplazo mantiene el event ID para preservar contratos.
**Estado consolidado.** Placeholder representativo implementado. Puede recibir PASS técnico de importación y routing mientras permanezca PENDING en calidad/mezcla. La retirada solo ocurre después de que el reemplazo pase pruebas y exista rollback.
## Apéndice M — Matriz de dispositivos, entornos y configuraciones de escucha
| Entorno | Objetivo | Criterio |
| --- | --- | --- |
| Auriculares cerrados | detectar clicks, estéreo, fatiga y subgrave | volumen moderado; no mezclar solo aquí |
| Auriculares abiertos | espacio y balance natural | revisar fuga de ambiente |
| Altavoces de escritorio | uso PC común | distancia y volumen realistas |
| Altavoz de portátil | claridad de UI y medios | comprobar que no desaparezcan eventos |
| TV/monitor con altavoz | escenario casual | latencia y graves limitados |
| Un solo canal/mono | accesibilidad y compatibilidad | no perder información por fase |
| Volumen bajo | jerarquía y ruido | UI crítica aún legible |
| Canales internos al 50 % | balance por defecto | referencia de QA |
| Music/Ambience a 0 | redundancia | gameplay y feedback siguen comprensibles |
| Effects/UI a 0 | accesibilidad | texto/VFX cubren estados |
| Editor | iteración y profiler | no aprobar como evidencia única |
| Windows Player externo | gate real | Player.log y lifecycle |

La campaña no pretende certificar todos los dispositivos comerciales. Busca detectar dependencias de un único sistema de escucha y asegurar que la mezcla mantiene su jerarquía en condiciones plausibles de PC. El volumen físico se documenta de manera aproximada y no se cambia entre comparaciones A/B.
## Apéndice N — Plan de madurez sonora por fases
| Fase | Contenido | Estado |
| --- | --- | --- |
| Fase 0 — Fundación | Event IDs, cuatro canales, clips placeholder, no-op seguro | IMPLEMENTADO |
| Fase 1 — Representativa | routing completo, controles, eventos críticos, build | PARCIAL / Sprint 16 |
| Fase 2 — Estabilización | cooldown, variación, 3D selectivo, lifecycle, mezcla | Sprint 17 |
| Fase 3 — H6 | Golden Path, evidencia, licencias, no S0/S1 | PENDING |
| Fase 4 — Producción | biblioteca ampliada, mixer/snapshots si procede | POST-H6 |
| Fase 5 — Release | masters finales, 90 min, dispositivos, créditos | FUTURO |
| Fase 6 — Expansiones | ecommerce/publishing/studio/platform | VISIÓN |

Cada fase reemplaza riesgos concretos, no añade contenido por inercia. La transición requiere criterios y evidencia. La Audio Bible puede aceptar una solución simple en H6 si es clara, estable, accesible y mantenible; no exige implementar anticipadamente toda la arquitectura de lanzamiento.
## Apéndice O — Work packages recomendados
| WP | Objetivo | Salida | Ventana |
| --- | --- | --- | --- |
| AUD-WP-01 | Auditar gain efectivo del router | test instrumental y corrección | Sprint 16 |
| AUD-WP-02 | Añadir diagnóstico rate-limited | missing ID/clip visible en desarrollo | Sprint 16 |
| AUD-WP-03 | Separar queue/checkout/revenue | una firma por transacción | Sprint 17 |
| AUD-WP-04 | Separar warning/day closed | dos firmas y prioridad | Sprint 17 |
| AUD-WP-05 | Puerta 3D | anchor, dos variantes, cooldown | Sprint 17 |
| AUD-WP-06 | Sustituir loops de 4 s | música/ambiente representativos | Sprint 17/H6 |
| AUD-WP-07 | Variación y concurrency | política por familia | Sprint 17 |
| AUD-WP-08 | Opciones de audio final | Master/categorías/mute/persistencia | H6/post-H6 |
| AUD-WP-09 | Licencias de clips | registro individual y créditos | antes de candidata |
| AUD-WP-10 | Campaña externa | build, Golden Path, Player.log | H6 |

### AUD-WP-01 — Auditar gain efectivo del router
**Alcance.** Este paquete produce `test instrumental y corrección` y se planifica para `Sprint 16`. Debe limitarse al problema descrito, conservar event IDs cuando sea posible y evitar cambios de gameplay. Antes de comenzar se registra baseline de audio, build y clips afectados.
**Ejecución.** Crear criterio de aceptación, preparar test o recorrido, implementar en Presentation/Infrastructure, actualizar catálogo, ejecutar EditMode/PlayMode, escuchar en contexto y validar en Player cuando afecte a lifecycle o mezcla. Cualquier excepción se documenta con riesgo y rollback.
**Cierre.** El paquete se cierra con código/assets, evidencia, defectos resueltos, procedencia actualizada y relación en producción/trazabilidad. Un clip que suena mejor pero rompe prioridad, accesibilidad o licencias no obtiene PASS.
### AUD-WP-02 — Añadir diagnóstico rate-limited
**Alcance.** Este paquete produce `missing ID/clip visible en desarrollo` y se planifica para `Sprint 16`. Debe limitarse al problema descrito, conservar event IDs cuando sea posible y evitar cambios de gameplay. Antes de comenzar se registra baseline de audio, build y clips afectados.
**Ejecución.** Crear criterio de aceptación, preparar test o recorrido, implementar en Presentation/Infrastructure, actualizar catálogo, ejecutar EditMode/PlayMode, escuchar en contexto y validar en Player cuando afecte a lifecycle o mezcla. Cualquier excepción se documenta con riesgo y rollback.
**Cierre.** El paquete se cierra con código/assets, evidencia, defectos resueltos, procedencia actualizada y relación en producción/trazabilidad. Un clip que suena mejor pero rompe prioridad, accesibilidad o licencias no obtiene PASS.
### AUD-WP-03 — Separar queue/checkout/revenue
**Alcance.** Este paquete produce `una firma por transacción` y se planifica para `Sprint 17`. Debe limitarse al problema descrito, conservar event IDs cuando sea posible y evitar cambios de gameplay. Antes de comenzar se registra baseline de audio, build y clips afectados.
**Ejecución.** Crear criterio de aceptación, preparar test o recorrido, implementar en Presentation/Infrastructure, actualizar catálogo, ejecutar EditMode/PlayMode, escuchar en contexto y validar en Player cuando afecte a lifecycle o mezcla. Cualquier excepción se documenta con riesgo y rollback.
**Cierre.** El paquete se cierra con código/assets, evidencia, defectos resueltos, procedencia actualizada y relación en producción/trazabilidad. Un clip que suena mejor pero rompe prioridad, accesibilidad o licencias no obtiene PASS.
### AUD-WP-04 — Separar warning/day closed
**Alcance.** Este paquete produce `dos firmas y prioridad` y se planifica para `Sprint 17`. Debe limitarse al problema descrito, conservar event IDs cuando sea posible y evitar cambios de gameplay. Antes de comenzar se registra baseline de audio, build y clips afectados.
**Ejecución.** Crear criterio de aceptación, preparar test o recorrido, implementar en Presentation/Infrastructure, actualizar catálogo, ejecutar EditMode/PlayMode, escuchar en contexto y validar en Player cuando afecte a lifecycle o mezcla. Cualquier excepción se documenta con riesgo y rollback.
**Cierre.** El paquete se cierra con código/assets, evidencia, defectos resueltos, procedencia actualizada y relación en producción/trazabilidad. Un clip que suena mejor pero rompe prioridad, accesibilidad o licencias no obtiene PASS.
### AUD-WP-05 — Puerta 3D
**Alcance.** Este paquete produce `anchor, dos variantes, cooldown` y se planifica para `Sprint 17`. Debe limitarse al problema descrito, conservar event IDs cuando sea posible y evitar cambios de gameplay. Antes de comenzar se registra baseline de audio, build y clips afectados.
**Ejecución.** Crear criterio de aceptación, preparar test o recorrido, implementar en Presentation/Infrastructure, actualizar catálogo, ejecutar EditMode/PlayMode, escuchar en contexto y validar en Player cuando afecte a lifecycle o mezcla. Cualquier excepción se documenta con riesgo y rollback.
**Cierre.** El paquete se cierra con código/assets, evidencia, defectos resueltos, procedencia actualizada y relación en producción/trazabilidad. Un clip que suena mejor pero rompe prioridad, accesibilidad o licencias no obtiene PASS.
### AUD-WP-06 — Sustituir loops de 4 s
**Alcance.** Este paquete produce `música/ambiente representativos` y se planifica para `Sprint 17/H6`. Debe limitarse al problema descrito, conservar event IDs cuando sea posible y evitar cambios de gameplay. Antes de comenzar se registra baseline de audio, build y clips afectados.
**Ejecución.** Crear criterio de aceptación, preparar test o recorrido, implementar en Presentation/Infrastructure, actualizar catálogo, ejecutar EditMode/PlayMode, escuchar en contexto y validar en Player cuando afecte a lifecycle o mezcla. Cualquier excepción se documenta con riesgo y rollback.
**Cierre.** El paquete se cierra con código/assets, evidencia, defectos resueltos, procedencia actualizada y relación en producción/trazabilidad. Un clip que suena mejor pero rompe prioridad, accesibilidad o licencias no obtiene PASS.
### AUD-WP-07 — Variación y concurrency
**Alcance.** Este paquete produce `política por familia` y se planifica para `Sprint 17`. Debe limitarse al problema descrito, conservar event IDs cuando sea posible y evitar cambios de gameplay. Antes de comenzar se registra baseline de audio, build y clips afectados.
**Ejecución.** Crear criterio de aceptación, preparar test o recorrido, implementar en Presentation/Infrastructure, actualizar catálogo, ejecutar EditMode/PlayMode, escuchar en contexto y validar en Player cuando afecte a lifecycle o mezcla. Cualquier excepción se documenta con riesgo y rollback.
**Cierre.** El paquete se cierra con código/assets, evidencia, defectos resueltos, procedencia actualizada y relación en producción/trazabilidad. Un clip que suena mejor pero rompe prioridad, accesibilidad o licencias no obtiene PASS.
### AUD-WP-08 — Opciones de audio final
**Alcance.** Este paquete produce `Master/categorías/mute/persistencia` y se planifica para `H6/post-H6`. Debe limitarse al problema descrito, conservar event IDs cuando sea posible y evitar cambios de gameplay. Antes de comenzar se registra baseline de audio, build y clips afectados.
**Ejecución.** Crear criterio de aceptación, preparar test o recorrido, implementar en Presentation/Infrastructure, actualizar catálogo, ejecutar EditMode/PlayMode, escuchar en contexto y validar en Player cuando afecte a lifecycle o mezcla. Cualquier excepción se documenta con riesgo y rollback.
**Cierre.** El paquete se cierra con código/assets, evidencia, defectos resueltos, procedencia actualizada y relación en producción/trazabilidad. Un clip que suena mejor pero rompe prioridad, accesibilidad o licencias no obtiene PASS.
### AUD-WP-09 — Licencias de clips
**Alcance.** Este paquete produce `registro individual y créditos` y se planifica para `antes de candidata`. Debe limitarse al problema descrito, conservar event IDs cuando sea posible y evitar cambios de gameplay. Antes de comenzar se registra baseline de audio, build y clips afectados.
**Ejecución.** Crear criterio de aceptación, preparar test o recorrido, implementar en Presentation/Infrastructure, actualizar catálogo, ejecutar EditMode/PlayMode, escuchar en contexto y validar en Player cuando afecte a lifecycle o mezcla. Cualquier excepción se documenta con riesgo y rollback.
**Cierre.** El paquete se cierra con código/assets, evidencia, defectos resueltos, procedencia actualizada y relación en producción/trazabilidad. Un clip que suena mejor pero rompe prioridad, accesibilidad o licencias no obtiene PASS.
### AUD-WP-10 — Campaña externa
**Alcance.** Este paquete produce `build, Golden Path, Player.log` y se planifica para `H6`. Debe limitarse al problema descrito, conservar event IDs cuando sea posible y evitar cambios de gameplay. Antes de comenzar se registra baseline de audio, build y clips afectados.
**Ejecución.** Crear criterio de aceptación, preparar test o recorrido, implementar en Presentation/Infrastructure, actualizar catálogo, ejecutar EditMode/PlayMode, escuchar en contexto y validar en Player cuando afecte a lifecycle o mezcla. Cualquier excepción se documenta con riesgo y rollback.
**Cierre.** El paquete se cierra con código/assets, evidencia, defectos resueltos, procedencia actualizada y relación en producción/trazabilidad. Un clip que suena mejor pero rompe prioridad, accesibilidad o licencias no obtiene PASS.
## Apéndice P — Guía de brief para producción sonora
Cada brief describe: event ID; sistema y momento exacto; estado anterior/posterior; emoción funcional; material o fuente; duración máxima; 2D/3D; prioridad; frecuencia de uso; variantes; rango de pitch; referencia no infractora; canal; nivel relativo; alternativa visual; formato de entrega; nombre; licencia; test y gate. Debe evitar referencias del tipo “que suene como una consola concreta” y usar cualidades acústicas generales.
## Apéndice Q — Guía de revisión de una mezcla candidata
1. Restaurar volúmenes de referencia y confirmar que PlayerPrefs no arrastran valores anómalos.
2. Entrar en StoreInitial y escuchar 60 segundos sin acciones.
3. Abrir/cerrar UI y comprobar que música no reinicia.
4. Ejecutar diez placements y cinco rechazos.
5. Recibir pedido, reponer y forzar out-of-stock.
6. Observar varios clientes, puerta y cola.
7. Completar checkout y comparar número de firmas con transacciones.
8. Ejecutar warning, cierre y autosave.
9. Recargar slot y volver a menú.
10. Revisar profiler y Player.log.
11. Repetir con Music/Ambience en cero.
12. Repetir en altavoz de portátil o dispositivo equivalente.
13. Registrar PASS/FAIL por criterio, no una impresión global.
## Apéndice R — Riesgos y mitigaciones
| Riesgo | Impacto | Mitigación |
| --- | --- | --- |
| Fatiga por clips cortos | Alta | loops largos, variación, campaña prolongada |
| Doble checkout | Alta | correlation ID y agregación |
| Audio 2D incoherente | Media/Alta | 3D selectivo y anchors |
| Opciones alteran balance | Media | base gain separado de channel gain |
| Missing clip silencioso | Media | warning rate-limited + QA |
| Licencia desconocida | Alta | registro por clip antes de candidata |
| Streaming causa latencia | Media | perfilado por categoría |
| Mixer sobrediseñado | Media | implementación incremental |
| Puerta genera spam | Alta | state edge + cooldown |
| UI hover irritante | Alta | debounce y opcional |
| Música se reinicia | Media | lifecycle y servicio persistente |
| Eventos futuros invaden H6 | Media | scope gate y roadmap |
