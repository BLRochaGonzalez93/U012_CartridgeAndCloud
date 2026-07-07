---
title: "Cartridge & Cloud — Art Bible"
subtitle: "Dirección artística, producción visual, StoreInitial y pipeline de assets"
author: "VRM Games / Blas Luis Rocha González"
date: "2026-07-07"
lang: es-ES
document_id: "CC-DOC-17"
document_version: "1.0"
status: "CONSOLIDATED / ACTIVE / NOT BASELINE-FROZEN"
project: "Cartridge & Cloud"
platform: "PC / Steam"
engine: "Unity 6.3 LTS 6000.3.18f1"
render_pipeline: "URP 17.3.0"
application_version: "0.0.21"
owner: "VRM Games"
---

# Cartridge & Cloud — Art Bible

**Archivo:** `17_Art_Bible.md`  
**Propósito:** establecer la autoridad visual y el procedimiento de producción artística de Cartridge & Cloud.  
**Estado del proyecto:** Sprints 0–15 `CLOSED / PASS`; Sprint 16 `COMPLETED / PASS`; Sprint 17 `PENDING / READY TO OPEN`; H6 `BLOCKED / NOT RUN`.  
**Escena objetivo:** `StoreInitial.unity`, aproximadamente `10 × 15 m`, grid lógico `20 × 30` a `0,5 m`.  
**Validación vigente:** compilación, EditMode, PlayMode, regresión manual y build externa `0.0.21` en PASS; StoreInitial cuenta con aprobación visual explícita de Sprint 16.  
**Regla de interpretación:** una imagen atractiva, un prefab que instancia o una suite verde no equivalen por sí solos a aprobación artística, integración funcional ni cierre de gate.

## Aprobación visual de Sprint 16 — 2026-07-06

La composición representativa de `StoreInitial` queda aprobada para el cierre de Sprint 16. La revisión confirma layout, escala, lectura de zonas, arquitectura, puerta, mobiliario, productos, personajes, colliders, navegación y relación entre pivotes/anchors sin defectos visuales bloqueantes. Esta aprobación se limita al gate de Sprint 16 y no sustituye la revisión artística, legal, de rendimiento o accesibilidad exigida por H6.

> Esta Art Bible consolida toda la genealogía disponible, incluidas las Art Bible v0.3, v0.4 y v0.5; la repetición de v0.3 en las baselines v0.3 y v0.4; los catálogos de contenido v0.1, v0.2 y v0.3; las UI Style Guide v0.3–v0.5; los registros legales v0.3–v0.5; los planes y registros de Sprint 16; el kit modular de Blender; los assets serializados de Unity; y las referencias conceptuales aprobadas. Las decisiones históricas no se eliminan: se clasifican como vigentes, sustituidas, diferidas o visión.

# 0. Control documental

Esta Art Bible ocupa la posición `17` dentro de la documentación consolidada. Su autoridad se limita a dirección visual, lenguaje de formas, materiales, iluminación, personajes, productos, arquitectura, pipeline de arte, integración visual y criterios de revisión. No redefine reglas económicas, persistencia, arquitectura de software ni aceptación de QA fuera de su impacto visual.

Los términos normativos se interpretan así:

- **DEBE**: requisito obligatorio para la fase o gate indicado.
- **NO DEBE**: práctica prohibida salvo excepción formal.
- **DEBERÍA**: recomendación fuerte que requiere justificación si se omite.
- **PUEDE**: opción permitida.
- **HISTÓRICO**: decisión preservada por trazabilidad, pero no necesariamente vigente.
- **OBJETIVO**: resultado deseado todavía no demostrado.
- **IMPLEMENTADO**: existe en assets o escena y ha sido observado.
- **VALIDADO**: existe evidencia técnica o visual suficiente.
- **VISION**: dirección futura no comprometida para el primer release.

El documento debe revisarse cuando cambie cualquiera de estos elementos: target visual, escala de StoreInitial, familia de materiales, shader principal, cámara objetivo, catálogo representativo, estrategia de personajes, pipeline DCC/Unity, presupuesto de rendimiento, regla legal de marcas o gate artístico de Sprint 16/H6.


# 1. Propósito y resultados esperados

La Art Bible convierte una colección heterogénea de conceptos, modelos, materiales, prefabs y decisiones históricas en un lenguaje visual ejecutable. Debe permitir que una persona pueda crear o revisar un asset sin depender de intuiciones no registradas, y que otra persona pueda distinguir entre un asset visualmente atractivo, un asset técnicamente integrable y un asset aprobado para una build.

Sus resultados esperados son:

1. fijar la identidad reconocible del juego;
2. conservar la calidez y nostalgia de la tienda física sin convertirla en un museo retro;
3. asegurar legibilidad desde cámara orbital/elevada;
4. impedir que la marca negro/verde vuelva monocromático el mundo;
5. definir qué partes de la visión histórica permanecen y cuáles requieren prueba;
6. separar arquitectura fija, mobiliario dinámico, datos y presentación;
7. alinear Blender, FBX, Unity, URP, prefabs, materiales, colliders, LOD y catálogos;
8. establecer criterios de Sprint 16 y H6;
9. proteger la propiedad intelectual mediante marcas ficticias y procedencia registrada;
10. mantener una evolución visual coherente desde la pequeña tienda hasta los sistemas futuros, sin convertir esa visión en compromiso inmediato.


# 2. Jerarquía de autoridad

Cuando exista contradicción, se aplicará la siguiente jerarquía:

1. `00_Enfoque_y_Alcance.md`: identidad, fantasía, límites y dirección general.
2. `02_Vertical_Slice_Specification.md`: contenido representativo y criterios obligatorios de H6.
3. `03_Technical_Design_Document.md` y `04_Modelo_de_Datos.md`: separación entre lógica y representación.
4. Esta Art Bible: reglas artísticas y pipeline visual.
5. `05_UX_Flow.md` y la futura `19_UI_Style_Guide.md`: legibilidad, jerarquía de información e iconografía.
6. `10_Unity_Project_Setup_Guide.md` y `11_Build_y_Versioning_Guide.md`: configuración técnica y evidencia de build.
7. `12_Excel_Maestro_de_Produccion.xlsx` y `13_Trazabilidad_y_Control_de_Cambios.xlsx`: estado operativo y relaciones.
8. Escenas, prefabs, catálogos, materiales y builds reales: evidencia de implementación, no autoridad automática sobre intención.
9. Referencias conceptuales: guía visual, nunca fuente de gameplay ni prueba de integración.

Una imagen conceptual no puede derogar un footprint funcional. Un FBX no puede definir por accidente la arquitectura de la tienda. Un nombre de GameObject no puede sustituir un ID estable. Una suite automatizada no puede aprobar composición, iluminación o lectura visual.


# 3. Alcance de la Art Bible

Incluye:

- dirección visual del mundo 3D;
- StoreInitial y sus zonas;
- arquitectura, mobiliario, props, productos y packaging;
- empleados, clientes y proveedores;
- materiales, color, iluminación, VFX y señalética;
- cámara como condición de lectura artística;
- pipeline de concepto, Blender, FBX, Unity, prefab y catálogo;
- LOD, occlusion, colliders y rendimiento en su dimensión visual;
- nomenclatura, rutas, IDs y variantes;
- procedencia, licencias y uso de IA generativa;
- revisión visual y técnica;
- evolución artística futura.

No incluye el diseño detallado de audio, que pertenece a `18_Audio_Bible.md`; el diseño detallado de UI, que pertenecerá a `19_UI_Style_Guide.md`; ni el catálogo de contenido completo, que pertenecerá a `21_Initial_Content_Catalog.xlsx`. Los menciona solo cuando condicionan la coherencia visual.


# 4. Estado ejecutivo de arte

A fecha de esta consolidación:

- la dirección visual general está aprobada;
- el target conceptual de StoreInitial Tier E está aprobado como referencia provisional;
- la superficie objetivo vigente es aproximadamente `10 × 15 m`, pendiente únicamente de ajustes derivados de blockout e interacción, no de volver al concepto histórico de `5 × 5 m`;
- existen 24 prefabs de arquitectura, 8 de mobiliario, 10 de productos/packaging, 3 prefabs de personaje y 19 prefabs conceptuales de expansión;
- existen 10 clips de audio, 11 clips de animación y una paleta de materiales URP;
- el catálogo funcional contiene 6 productos y 8 familias de mobiliario;
- los prefabs representativos de arquitectura, mobiliario y producto cargan, y buena parte incorpora LODGroup;
- los prefabs de personaje actuales son contenedores de ID/rol/material, sin modelo visual, Animator ni collider serializado;
- no hay texturas de producción bajo `Assets/_Project/Textures` y no hay assets de VFX bajo `Assets/_Project/VFX`;
- no existe shader de outline/toon demostrado en el paquete;
- `StoreInitial.unity` conserva la misma estructura técnica de `Store.unity`, sin instancias de los prefabs representativos;
- `StoreRuntimeSettings` sigue apuntando a `Store` y mantiene `_buildBlockoutOnLoad: 1`;
- muros, puerta y mobiliario de almacén fallaron revisión visual en la composición procedural;
- el build posterior a la migración de StoreInitial no se ha ejecutado.

Por tanto, el estado correcto es **dirección aprobada / assets parciales / escena representativa no cerrada**.


# 5. Genealogía documental completa

La dirección de arte no nació en v0.6. Se ha transformado en cuatro etapas documentales y varias capas auxiliares. Esta sección conserva todas ellas para evitar que una simplificación tardía borre decisiones útiles.

- **Baseline v0.3 — Art Bible v0.3:** visión extensa de preproducción. Fijó low poly, texturas hand-painted/cartoon, bordes tipo tinta, iluminación cálida, modularidad, productos ficticios, personajes estilizados, evolución por etapas y pipeline conceptual. Planteó una tienda de `5 × 5 m`, cinco muebles y doce productos.
- **Baseline v0.4 — Art Bible v0.3:** republicó la misma versión artística dentro de una baseline documental más madura. No debe tratarse como una versión visual nueva, pero sí como evidencia de que la dirección se mantuvo durante ese ciclo.
- **Baseline v0.5 — Art Bible v0.4:** sustituyó la preproducción pura por un estado técnico real tras Sprint 5. La tienda pasó a `10 × 15 m`, aparecieron primitivas y materiales técnicos, y se reconoció que el pase representativo todavía no existía.
- **Baseline v0.6 — Art Bible v0.5:** formalizó grafito, negro, verde, cyan, madera cálida, URP, ocho muebles, seis productos, packaging, arquitectura modular y separación de paleta entre empleados y resto del mundo. Declaró StoreInitial como target oficial de Sprint 16.
- **Documentos auxiliares históricos:** catálogos de contenido v0.1–v0.3; UI Style Guide v0.3–v0.5; registros legales v0.3–v0.5; Enfoque v0.6–v0.8; GDD/TDD/UX y planes de Sprint.
- **Evidencia actual:** Assets.zip, Tools.zip, StoreInitial, catálogos runtime, materiales, prefabs, registros de integración y conceptos visuales del 29 de junio de 2026.

La versión consolidada no selecciona arbitrariamente una etapa. Conserva la visión de v0.3, adopta la escala y evidencia técnica de v0.4/v0.5 y corrige cualquier afirmación que la implementación real no sostenga.


# 6. Decisiones históricas conservadas, sustituidas y diferidas

| Materia | Decisión histórica | Estado consolidado | Regla vigente |
| --- | --- | --- | --- |
| Escala de tienda | 5 × 5 m / 10 × 10 celdas | SUSTITUIDA | aprox. 10 × 15 m / grid 20 × 30 a 0,5 m |
| Estilo | low poly + hand-painted/cartoon + borde tinta | PARCIALMENTE CONSERVADA | low poly estilizado y color blocking vigentes; textura hand-painted y outline requieren spike y validación |
| Contenido mínimo | 5 muebles, 12 productos, 4 clientes | SUSTITUIDA/REINTERPRETADA | 8 familias de mobiliario, 6 productos funcionales; 4 arquetipos de cliente como referencia de variedad |
| Arquitectura | kit modular y expansión física | CONSERVADA | módulos de 1/2/4 m y autoría explícita de escena |
| Composición | builder procedural aceptable | SUSTITUIDA | StoreInitial y StoreInitialEnvironment se authoran manualmente |
| Paleta | negro/verde identitario | CONSERVADA CON LÍMITES | marca y empleados usan identidad; productos, clientes y proveedores mantienen variedad |
| Iluminación | cálida y sombras suaves | CONSERVADA | debe preservar navegación, interacción y contraste |
| Marketing art | distinguir captura, mockup, concepto y key art | CONSERVADA | obligatorio etiquetar y no presentar sistemas inexistentes como gameplay |
| Expansiones | logística, publishing, estudio, plataforma, servidores | VISION | lenguaje visual preservado, sin compromiso antes de gates posteriores |
| Personajes | animación económica y expresiva | CONSERVADA | postura, silueta y props antes que facial costoso |


# 7. Visión artística consolidada

Cartridge & Cloud debe sentirse como una tienda independiente de videojuegos que crece sin perder la cercanía humana. La imagen combina comercio físico, cultura del videojuego, orden operativo y una capa tecnológica progresiva. El jugador debe reconocer el lugar como un negocio real, pero no como una reproducción fotográfica ni como una parodia saturada de referencias.

La estética base es **3D estilizada, modular, limpia y cálida**, con proporciones ligeramente simplificadas, bordes legibles, materiales de respuesta controlada y suficiente detalle para comunicar uso. El juego debe resultar agradable en reposo y comprensible en movimiento. El entorno no existe para ser admirado desde una cámara libre de portfolio: existe para permitir comprar, mover, almacenar, exponer, vender, recorrer y leer estados.

La nostalgia aparece mediante formatos físicos, cajas, estanterías, carátulas ficticias, cartelería y rituales de tienda; no mediante copia de marcas reales. La tecnología aparece mediante grafito, acentos verdes/cyan, señalética luminosa y equipos progresivamente más sofisticados; no mediante convertir toda superficie en neón.


# 8. Pilares artísticos

1. **Legibilidad antes que microdetalle.** La función de una silueta debe entenderse a la distancia de cámara habitual.
2. **Calidez comercial.** Madera, luz cálida, cartón y pequeños elementos personales equilibran grafito y metal.
3. **Tecnología controlada.** Verde y cyan guían identidad y feedback, no bañan todo el espacio.
4. **Modularidad visible pero no repetitiva.** Las familias comparten construcción, mientras variantes, props y agrupación evitan clonación obvia.
5. **Color con propósito.** Producto, cliente y proveedor aportan variedad; la marca aporta cohesión.
6. **Escala creíble.** Puertas, mostradores, pasillos, estanterías y personajes deben compartir una métrica consistente.
7. **Evolución acumulativa.** Cada etapa futura parece una ampliación del mismo negocio, no otro juego.
8. **Separación entre lógica y arte.** IDs, footprints y reglas sobreviven al reemplazo visual.
9. **Producción viable.** El objetivo es un nivel representativo sostenible para un desarrollador principal, no un catálogo de cientos de assets únicos.
10. **Honestidad visual.** Concept art, mockup, captura y gameplay se identifican correctamente.


# 9. Experiencia visual objetivo

Al entrar en StoreInitial, el jugador debe percibir en pocos segundos: entrada, checkout, exposición principal, circulación, almacén y recepción. Después puede descubrir detalles de producto, packaging, cartelería, vegetación y utilería. Este orden importa: primero orientación, después fantasía y por último detalle.

La tienda debe sentirse modesta pero cuidada. Tier E no significa abandonada, rota ni sucia; significa recursos limitados, selección compacta, mobiliario funcional, acabados sencillos y una identidad todavía en formación. El crecimiento futuro puede mejorar materiales, iluminación, densidad y especialización, pero no debe hacer que el inicio parezca un placeholder deliberadamente feo.

La imagen conceptual principal muestra una combinación acertada: perímetro oscuro, paredes claras, suelo gris cálido, madera en caja, metal en estanterías, vidrio en vitrina, cartón en recepción, vegetación puntual y verde de marca. Esa composición es dirección, no plano literal: las posiciones finales deben someterse a footprints, navegación, cola, cámara e interacción.


# 10. Nivel de estilización

El nivel objetivo se sitúa entre low-poly de producción y estilización ilustrativa. Las formas principales se simplifican; los biseles y cambios de plano ayudan a capturar luz; el detalle pequeño se representa mediante color, relieve moderado, decal o textura cuando aporte lectura. Se evitarán tanto la geometría excesivamente facetada sin intención como el realismo PBR de alta frecuencia.

El documento histórico v0.3 proponía texturas hand-painted/cartoon y un borde sombreado tipo tinta. La implementación actual no contiene texturas de producción ni shader de outline. En consecuencia:

- el **color blocking estilizado**, los valores pintados y los bordes oscuros de diseño se mantienen como dirección;
- un **shader de outline global no es requisito cerrado** de Sprint 16;
- cualquier contorno debe probarse a distintas distancias, con mobiliario denso, transparencias, personajes y UI;
- si el outline introduce ruido, z-fighting, problemas de transparencia o coste injustificado, puede limitarse a selección, interacción o assets concretos;
- la ausencia de outline no permite abandonar la legibilidad de silueta.


# 11. Lenguaje de formas

La arquitectura utiliza masas rectangulares, marcos robustos, paneles y esquinas suavizadas mediante biseles discretos. El mobiliario combina cuerpos estables con capas funcionales: base, superficie, estante, marco, iluminación/acento y punto de interacción. Los productos usan siluetas simples y reconocibles: caja, consola, mando, headset o accesorio.

Reglas:

- base visual ligeramente pesada para transmitir estabilidad;
- esquinas no peligrosamente afiladas en elementos de contacto;
- marcos de vidrio y puertas suficientemente gruesos para leerse desde cámara media;
- piezas pequeñas agrupadas por valor o función, no dispersas;
- acentos de marca concentrados en frentes, señalética o perfiles;
- asimetría moderada para evitar aspecto de kit genérico;
- detalles funcionales exagerados lo justo: lector, scanner, asas, ruedas, baldas, marcos y bisagras;
- no modelar tornillos, costuras o conectores que desaparecen siempre a distancia objetivo, salvo que formen parte de una vista cercana o marketing art.


# 12. Silueta y lectura desde cámara

La cámara orbital genera tres distancias de lectura, visibles también en el target conceptual:

- **Cercana:** interacción, producto, checkout y detalle de uso.
- **Media:** equilibrio entre jugabilidad, cliente, mobiliario y contexto.
- **Lejana:** orientación, circulación, cola y zonificación.

Un asset obligatorio debe superar las tres pruebas. En cercana no debe revelar interpenetraciones graves ni materiales rotos. En media debe conservar función, orientación y punto de interacción. En lejana debe aportar una masa clara sin fundirse con suelo o pared.

La cámara elevada implica que las superficies superiores son importantes. Mostradores, vitrinas, estantes bajos, cajas y consolas necesitan una lectura cenital parcial. Los frentes verticales siguen siendo útiles para marca, pero no pueden contener toda la información. La oclusión de paredes debe coordinarse con `_hideOccludingWalls`; la solución artística no debe depender de que todas las paredes estén siempre visibles.


# 13. Paleta global

La paleta se organiza por capas, no por una única lista rígida:

1. **Base arquitectónica:** grafito, gris oscuro, neutro claro y gris cálido.
2. **Calidez:** madera, cartón, crema, luz cálida y vegetación.
3. **Marca:** verde principal y cyan secundario.
4. **Producto:** colores más amplios por familia, género y proveedor.
5. **Personajes:** diversidad cromática controlada por rol.
6. **Feedback:** verde válido, rojo inválido, ámbar advertencia y apoyos no cromáticos.

La tienda debe poder reconocerse en escala de grises por contraste y composición. El verde no sustituye estructura, icono, texto o animación. El uso simultáneo de verde de marca y verde de feedback debe separarse por contexto, intensidad, forma y temporalidad.


# 14. Paleta de interfaz heredada

| Uso | Color |
| --- | --- |
| Fondo principal | #0D1110 |
| Superficie | #19201D |
| Superficie elevada | #242D29 |
| Verde principal | #35D07F |
| Verde secundario | #1F8F5F |
| Texto principal | #E8EFEA |
| Texto secundario | #AAB7B0 |
| Advertencia | #E8B44A |
| Error | #D95D5D |

Esta paleta procede de la dirección histórica y permanece como referencia transversal. La futura UI Style Guide definirá tokens finales y estados. En el mundo 3D se utilizará como relación de identidad, no como obligación de reproducir exactamente los mismos valores en materiales sometidos a iluminación.


# 15. Paleta representativa de materiales Unity

| Material | Hex aproximado sRGB | Uso | Propiedades actuales |
| --- | --- | --- | --- |
| Graphite | #222E35 | estructura oscura, bases, marcos | metallic 0,35 / smoothness 0,58 |
| Dark Surface | #3C4B55 | superficies oscuras secundarias | metallic 0,08 / smoothness 0,45 |
| Light Neutral | #DDE6E3 | pared clara, texto físico y superficies de contraste | smoothness 0,45 |
| VRM Green | #31D789 | marca, confirmación, acento principal | metallic 0,05 / smoothness 0,64 |
| Cyan | #52C8E7 | acento tecnológico secundario | metallic 0,05 / smoothness 0,64 |
| Warm Wood | #996F4B | madera y calidez comercial | smoothness 0,52 |
| Cardboard | #AD8557 | cajas y logística | smoothness 0,18 |
| Amber | #E59E3C | advertencia y calor puntual | smoothness 0,55 |
| Red | #DA4542 | error y peligro | smoothness 0,55 |
| Blue Gray | #61798B | metal pintado y superficie técnica | metallic 0,08 / smoothness 0,45 |
| Glass | #6C9095 | vidrio tintado | transparente / smoothness 0,88 |

Los hex se derivan de los colores serializados de materiales actuales y sirven para reconocer la intención. No sustituyen una revisión dentro de Unity con exposición, tonemapping, luz, reflexión y transparencia reales. Los materiales `VRMGreenEmissive` y `CyanEmissive` comparten color base con sus variantes no emisivas, pero deben reservarse para señalética y acentos limitados.


# 16. Colores de feedback y accesibilidad

| Estado | Color aproximado | Uso |
| --- | --- | --- |
| Válido representativo | #50F3AD | acción válida, colocación aceptada |
| Inválido representativo | #F97669 | acción inválida, colisión o bloqueo |
| Advertencia representativa | #FFD159 | riesgo, atención o estado transitorio |
| Marcador checkout | #59D39E | zona funcional de caja |
| Marcador receiving | #F9C861 | zona de recepción |
| Marcador backroom | #86B8DD | zona de almacén |

Cada estado crítico debe tener al menos dos canales entre color, icono, texto, contorno, animación, sonido o cambio de forma. La colocación válida puede usar verde, huella estable y confirmación sonora; la inválida, rojo, patrón/contorno discontinuo, motivo textual y sonido de rechazo. El ámbar comunica advertencia o estado transitorio, no error irreversible.

Los marcadores técnicos de Sprint 5 no deben quedar visibles como arte final. Pueden mantenerse en TestLab, herramientas de debug o modo diagnóstico. Las zonas funcionales pueden sugerirse mediante mobiliario, suelo, señalética y luz, pero no necesitan pintar grandes rectángulos transparentes durante gameplay normal.


# 17. Identidad de Cartridge & Cloud y VRM Games

La marca visible al jugador es **Cartridge & Cloud**. VRM Games es el desarrollador y origen del sistema visual corporativo. El target conceptual utiliza logo de nube/mando, grafito, verde y tipografía compacta. La Art Bible conserva esa identidad sin exigir que toda superficie muestre el logo.

Aplicaciones prioritarias:

- rótulo de fachada;
- frontal del checkout;
- puerta o felpudo;
- credenciales y uniforme de empleados;
- bolsas, etiquetas o material propio de tienda;
- paneles de Operations dentro del mundo cuando proceda;
- packaging de marca propia solo si el producto realmente pertenece a la tienda.

No se colocará el logo en cada mueble, caja o producto de proveedor. La repetición indiscriminada reduce credibilidad y hace que el mundo parezca un showroom corporativo en vez de un comercio multimarca.


# 18. Paletas secundarias

Las paletas secundarias amplían la identidad:

- **Retail cálido:** madera media, crema, gris pizarra, verde vegetal y cartón.
- **Logística:** grafito, acero, cartón, amarillo/ámbar de seguridad y azul gris.
- **Proveedor:** cada empresa ficticia puede tener dos colores propios y un neutro, evitando copiar combinaciones distintivas de marcas reales.
- **Cliente:** ropa variada con un color dominante, un apoyo y un neutro; evitar que todos usen verde.
- **Producto:** familia reconocible por color/forma; ediciones premium pueden usar contraste más oscuro, foil simulado o acento metálico moderado.
- **Futuro tecnológico:** cyan, pantallas y luz fría aumentan gradualmente, sin borrar madera, personas y objetos físicos.

La diversidad debe planificarse. Aleatorizar colores sin armonía puede producir ruido equivalente a una paleta monocroma.


# 19. Materiales: principios generales

Los materiales deben comunicar categoría a distancia: pared mate, suelo resistente, madera cálida, metal estructural, plástico de producto, vidrio de vitrina y cartón de logística. La variación de roughness/smoothness es tan importante como el color.

Reglas:

- paredes y cartón: mate;
- madera: semimate con lectura de plano, no barniz espejo;
- metal: reflectancia controlada y valores no negros absolutos;
- plástico: smoothness media, bordes suavizados y acentos;
- vidrio: alta smoothness, tinte sutil, marco visible y fondo que permita leerlo;
- emisión: limitada a rótulos, LEDs, paneles y feedback;
- desgaste: configurable y localizado, nunca ruido uniforme en todo asset;
- material compartido: preferible cuando la variante se resuelve con `MaterialPropertyBlock` o datos;
- `renderer.material`: no debe usarse repetidamente si crea instancias accidentales.


# 20. Grafito, superficies oscuras y metal

El grafito define marcos, bases y tecnología. No debe convertirse en negro puro sin detalle. Se recomienda separar masas mediante roughness, biseles, paneles y pequeñas diferencias de valor. El metal visible puede combinar grafito con acero azul-gris, pero debe evitar el cromado intenso que compita con producto y UI.

En estanterías y marcos, el metal oscuro funciona como estructura que deja respirar los productos. En fachada, puede generar una silueta premium aunque la tienda sea Tier E, siempre que madera, pared clara, cartón y señalética mantengan cercanía. En infraestructura futura, el porcentaje de metal puede crecer, pero la lectura funcional debe seguir siendo inmediata.


# 21. Madera, cartón y materiales cálidos

La madera es el contrapeso humano a la tecnología. Se utiliza en mostrador, detalles de exposición, pequeños muebles o frentes, no necesariamente en toda estantería. El target conceptual muestra una madera media que se integra con grafito y verde. Debe evitarse una textura fotográfica de veta con escala incorrecta.

El cartón comunica stock, recepción y negocio en funcionamiento. Las cajas necesitan variación de tamaño, orientación, cinta, etiqueta y desgaste moderado. No deben formar masas visuales caóticas que bloqueen rutas o confundan colliders. La paleta actual `Cardboard` y los módulos de receiving permiten construir agrupaciones coherentes.


# 22. Vidrio y transparencias

El vidrio aparece en fachada, puerta y vitrinas. Debe ser visible sin parecer opaco. Se apoyará en marco, borde, reflexión, tinte y contraste con el fondo. El material actual `Glass` usa alta smoothness y transparencia; su resultado debe revisarse en build porque la transparencia puede variar según URP, sorting y postprocesado.

Reglas:

- no usar vidrio sin marco en ángulos donde desaparezca;
- evitar varias capas coplanares;
- no depender de una reflexión que solo existe en Editor;
- revisar orden de render con productos dentro de vitrina;
- separar collider de la hoja visual;
- la puerta automática necesita hojas, marco, rail y sensores con referencias explícitas;
- la legibilidad de entrada tiene prioridad sobre el realismo óptico.


# 23. Texturas, decals y densidad de detalle

El paquete actual no contiene texturas de producción en `Assets/_Project/Textures`. Por tanto, la textura hand-painted histórica es un **objetivo no implementado**, no una característica que pueda declararse cerrada.

La estrategia recomendada es incremental:

1. color y material compartido para validar composición;
2. decals o etiquetas para señalética y packaging;
3. texturas tiling ligeras para pared, suelo y madera si el color plano resulta insuficiente;
4. atlas o trimsheets para familias repetidas;
5. detalle único solo donde sea visible y tenga función narrativa.

La densidad debe seguir la cámara: producto o mostrador cercano puede recibir etiqueta y panel; pared lejana necesita masa, no microtexto. Todo texto pequeño que no pueda leerse debe funcionar como patrón gráfico, no como información crítica.


# 24. Bordes, sombreado y outline

La visión histórica pedía “bordes sombreados tipo tinta”. La dirección actual los interpreta como una combinación de:

- siluetas fuertes;
- biseles que capturan luz;
- ambient occlusion moderada;
- separación de valores;
- bordes pintados o marcos oscuros en diseño;
- outline selectivo cuando aporte feedback o identidad.

No existe shader de outline/toon probado en los assets observados. Antes de convertirlo en requisito global debe realizarse un spike con StoreInitial equipada, vidrio, personajes, productos y varias distancias. La decisión debe medir ruido, coste, transparencia, compatibilidad URP y accesibilidad. Hasta entonces, el aspecto ilustrativo se logra mediante modelado, color, luz y composición.


# 25. Iluminación: filosofía

La iluminación debe hacer tres trabajos simultáneos: orientar, vender producto y sostener atmósfera. La base es interior cálida, sombras suaves y contraste moderado. La tienda no debe parecer una cueva con neones ni un supermercado plano sin jerarquía.

Principios:

- entrada y caja deben localizarse con facilidad;
- exposición principal recibe luz suficiente para producto;
- almacén y recepción son más funcionales, pero no ilegibles;
- pasillos mantienen luminancia continua;
- acentos verdes/cyan se reservan a marca y tecnología;
- la luz cálida no debe teñir todos los colores hasta hacer indistinguibles los estados;
- evitar highlights especulares que borren iconos o labels;
- revisar luz con cámara y postprocesado reales;
- baked/mixed se usa donde compense estabilidad y coste, sin bloquear objetos dinámicos.


# 26. Iluminación comercial y de StoreInitial

StoreInitial necesita una jerarquía de tres niveles:

1. **Luz base:** cobertura general suave que garantiza navegación y lectura.
2. **Luz de producto:** rails, spots o paneles sobre displays y vitrina.
3. **Acentos:** rótulo, logo, checkout, puerta y puntos destacados.

El target conceptual usa lámparas cálidas puntuales y acentos verdes. Los módulos `LightRail100`, `LightRail200`, `SpotLight` y `LinearPanel` permiten construir esa jerarquía. Los módulos visuales deben alinearse con luces reales o con una solución baked coherente; una luminaria apagada que produce luz invisible, o una luz sin fuente visible, debe justificarse.

La exposición no debe quemar las carátulas. La vitrina necesita controlar reflejos. El checkout debe destacar sin parecer una alarma. La entrada debe recibir suficiente contraste para que puerta y umbral se entiendan.


# 27. Almacén, recepción y zonas técnicas

El almacén se diferencia mediante estructura más industrial, metal, cartón y luz ligeramente más neutra. No debe parecer una zona abandonada ni un segundo comercio. La recepción puede usar marcas de seguridad, pallet/carretilla conceptual y agrupaciones de cajas, pero el amarillo de advertencia debe reservarse a bordes, suelo o puntos de riesgo.

La zona de receiving debe leerse como transición entre proveedor e inventario. Cajas entregadas, crate, espacio de descarga y acceso al backroom necesitan una composición que no invada la circulación. La luz puede ser más uniforme y funcional que en exposición. Los marcadores `ZoneReceiving` y `ZoneBackroom` son ayudas técnicas; la versión final debe comunicar esas funciones mediante entorno y props.


# 28. Hora del día y evolución ambiental

El vertical slice no exige un sistema artístico completo de clima o ciclos solares, pero la escena debe tolerar el ciclo de jornada. La iluminación interior constituye la referencia estable; el exterior o ambientación puede cambiar de forma contenida para indicar apertura, tarde y cierre.

Requisitos mínimos:

- el cambio temporal no altera la lectura de feedback;
- el cierre puede apoyarse en descenso de actividad, luz exterior o señalética, sin oscurecer interacción;
- luces emisivas no deben saturar en horario oscuro;
- el Player no debe perder orientación por cambios bruscos;
- cualquier transición se valida con clientes, cola y UI activos.

Las variantes avanzadas de clima, temporada o decoración quedan fuera de H6 salvo que se incorporen como contenido no bloqueante y sin deuda técnica.


# 29. StoreInitial como target oficial

`StoreInitial.unity` es el objetivo visual oficial de Sprint 16. Debe ser una escena autorada, revisable y explícita. La arquitectura fija, puerta, zonas y mobiliario inicial no se deducen de bounds, nombres de FBX o jerarquías accidentales. `StoreInitialSceneContext` debe exponer referencias funcionales, y el runtime debe crear solo estado dinámico.

La escena actual es todavía una copia técnica de `Store`: ambas presentan 33 GameObjects, 17 MeshRenderers, 7 BoxColliders, 1 luz y 0 instancias de prefab. Esto no es un defecto de documentación, sino el punto de partida de migración. La Art Bible prohíbe interpretarlo como composición final.

Jerarquía objetivo:

```text
StoreInitialEnvironment
├── Architecture
├── InitialFurniture
├── Lighting
├── Anchors
└── TechnicalColliders
```

No deben incluirse dentro del prefab de entorno: EventSystem, HUD, cámara, ApplicationRoot, servicios de save, managers globales o lógica que sobreviva a la escena.


# 30. Dimensiones y grid de StoreInitial

La referencia vigente es aproximadamente `10 × 15 m`, equivalente a `20 × 30` celdas de `0,5 m`. “Aproximadamente” permite ajustar muros, espesor, fachada, umbral y nichos, pero no regresar a la tienda histórica de `5 × 5 m` ni cambiar la escala funcional sin registro.

La cuadrícula define footprints, ocupación y circulación. La geometría visual puede sobresalir ligeramente del footprint cuando no contradiga colisión, navegación o interacción. Las paredes pueden tener grosor exterior al área jugable. La fachada debe preservar un acceso claro y una reserva de entrada.

La escala humana de referencia se sitúa en torno a `1,65–1,85 m` para adultos estilizados. Puertas, mostradores y estanterías deben revisarse junto a personaje y cámara, no solo mediante medidas numéricas.


# 31. Fachada, entrada y puerta automática

La fachada es el primer elemento de identidad. Debe combinar rótulo, marco oscuro, vidrio, puerta automática y umbral. El target conceptual propone una entrada central con apertura clara; la escena real puede ajustar posición según cola, recorrido y StoreSceneContext.

La puerta necesita:

- hojas visuales izquierda/derecha;
- marco y rail superior;
- sensor o trigger funcional separado;
- collider coherente con estado abierto/cerrado;
- punto de entrada y salida;
- espacio de reserva sin muebles;
- audio de apertura/cierre;
- feedback ante bloqueo;
- comportamiento legible desde cámara media.

La composición procedural falló por orientación y transforms. La solución autorada no debe corregirse escalando arbitrariamente el root. Se colocarán piezas en escala `1,1,1` siempre que sea posible y se registrarán las excepciones.


# 32. Zona de exposición

La exposición debe mostrar variedad sin convertirse en pared de miniaturas ilegibles. Se combinarán:

- estanterías de pared para densidad y perímetro;
- central shelf para circulación y capacidad;
- low display para lectura cenital;
- featured display para una promoción o producto destacado;
- vitrina visual, cuando se integre, para productos de alto valor.

Los productos deben agruparse por familia, color o proveedor. El vacío también comunica: una balda parcialmente vacía debe verse como stock bajo, no como asset incompleto. La representación del llenado puede usar grupos, slots o variantes, pero debe corresponder con la lógica de inventario.

La composición debe preservar líneas de visión hacia entrada y checkout. Displays altos no deben crear pasillos ciegos ni ocultar clientes durante la cola.


# 33. Checkout y cola

El checkout es el foco operativo de la tienda. El mostrador actual tiene footprint `4 × 2` celdas (`2 × 1 m`) y altura `1,1 m`. Debe permitir leer terminal, scanner, superficie de entrega, posición de empleado y lado de cliente.

Reglas de arte:

- frontal de marca visible desde cámara media;
- superficie superior limpia para interacción;
- terminal y lector exagerados lo justo para reconocerse;
- punto de caja separado de decoración;
- espacio tras mostrador para empleado;
- cola con entrada, espera y salida legibles;
- props no deben bloquear animaciones o interacción;
- las luces de confirmación no sustituyen mensajes de transacción;
- el material de madera aporta calidez, el grafito sostiene identidad.

La cola debe comunicarse mediante disposición espacial antes que mediante una línea pintada permanente. Marcadores pueden aparecer solo cuando sean necesarios para onboarding o debug.


# 34. Almacén y recepción

`backroom-storage` ocupa `5 × 2` celdas (`2,5 × 1 m`) y alcanza `2,4 m`; por su altura, necesita comprobar oclusión y distancia a pared. `receiving-crate` ocupa `2 × 2` (`1 × 1 m`). El target conceptual y la imagen de almacén muestran metal oscuro, cajas de cartón, contenedores verdes y organización por niveles.

El error visual abierto sobre warehouse furniture obliga a una regla: el mueble de almacenamiento fijo se coloca manualmente bajo `InitialFurniture` o se integra como mueble funcional con footprint explícito, pero no se escala para “encajar” en bounds. Debe conservar acceso frontal, separación de pared y coherencia con inventario.

La recepción puede usar varias cajas visuales sobre un footprint lógico único, siempre que la interacción no engañe sobre unidades o capacidad.


# 35. Circulación y líneas de visión

La circulación es una propiedad artística y funcional. El layout debe ofrecer:

- ruta directa entrada → exposición → checkout → salida;
- ruta proveedor → receiving → backroom;
- acceso del jugador a puntos obligatorios;
- espacio para clientes que observan productos;
- cola sin bloquear puerta ni pasillos;
- margen de cámara para ver decisiones;
- ausencia de callejones visuales involuntarios.

El ancho mínimo histórico de una celda (`0,5 m`) es un límite lógico, no una recomendación estética para pasillos principales. En zonas con dos NPC, giro o cola, se necesitan anchos mayores. Se validará con cápsulas o agentes reales, no solo mirando el suelo.

Las líneas de visión priorizan entrada, caja y estado de la tienda. El mobiliario alto se coloca en perímetro o zonas donde no oculte la acción principal.


# 36. Arquitectura fija y mobiliario dinámico

La arquitectura fija incluye suelo, muros, fachada, vidrio, particiones, rótulo, railes de luz y elementos que no se compran ni recolocan durante el alcance actual. Se autora en escena o prefab de entorno.

El mobiliario dinámico incluye objetos comprables o reubicables, con definición en catálogo, footprint, coste, prefab y reglas de colocación. El mobiliario inicial puede aparecer ya colocado, pero debe conservar identidad funcional si el juego permite retirarlo o persistirlo.

No se serializa arquitectura fija como si fuera un mueble. No se deduce el layout leyendo nombres del FBX. No se convierte un collider técnico en decoración visible. La frontera de reemplazo se mantiene mediante IDs de catálogo, material variant IDs y Resources paths, conforme a ADR-0072.


# 37. Kit modular de arquitectura

| Módulo | Función artística/técnica |
| --- | --- |
| AutomaticDoor | entrada automática |
| BackroomPartition | separación de trastienda |
| CornerInner | esquina interior |
| CornerOuter | esquina exterior |
| EndCap | remate modular |
| EntranceThreshold | umbral |
| Floor100 | suelo 1 m |
| Floor200 | suelo 2 m |
| FloorEdge100 | borde de suelo |
| FloorTransition100 | transición de suelo |
| LightRail100 | raíl de luz 1 m |
| LightRail200 | raíl de luz 2 m |
| LinearPanel | panel lineal |
| SpotLight | spot visible |
| StorefrontFacade | fachada |
| StoreGlassLeft | vidrio izquierdo |
| StoreGlassRight | vidrio derecho |
| StoreSign | rótulo |
| Wall100 | muro 1 m |
| Wall200 | muro 2 m |
| Wall400 | muro 4 m |
| ZoneBackroom | visual técnico de backroom |
| ZoneCheckout | visual técnico de checkout |
| ZoneReceiving | visual técnico de receiving |

Los módulos de zona son herramientas de representación/diagnóstico y no deben convertirse automáticamente en arte final visible. Los módulos de iluminación representan luminarias, no garantizan que la luz esté configurada. Los módulos se combinan manualmente bajo Architecture y Lighting.


# 38. Principios de mobiliario

Cada familia de mueble debe tener:

- silueta reconocible;
- footprint estable;
- frente y orientación evidentes;
- punto de interacción o área de producto;
- altura compatible con cámara;
- variante material coherente;
- capacidad comunicable;
- collider o volumen técnico validado;
- LOD cuando el coste lo justifique;
- prefab independiente de reglas de negocio;
- ID estable en ContentCatalog y RepresentativePrefabCatalog.

Las baldas visuales no deben prometer más slots que la capacidad lógica sin una regla explícita de agrupación. Un mueble que no soporta productos no debe mostrar huecos que parezcan slots interactivos. Una decoración no debe tener feedback de selección si no puede interactuarse.


# 39. Catálogo de ocho familias de mobiliario

| ID | Nombre | Grid | Medida | Altura | Rol | Capacidad |
| --- | --- | --- | --- | --- | --- | --- |
| checkout-counter | Checkout Counter | 4 × 2 | 2,0 × 1,0 m | 1,1 m | checkout | 0 |
| wall-shelf | Wall Shelf | 4 × 1 | 2,0 × 0,5 m | 2,2 m | display de pared | 24 |
| central-shelf | Central Shelf | 4 × 2 | 2,0 × 1,0 m | 1,6 m | display central | 32 |
| low-display | Low Display | 3 × 2 | 1,5 × 1,0 m | 0,9 m | display bajo | 12 |
| featured-display | Featured Display | 2 × 2 | 1,0 × 1,0 m | 1,1 m | display destacado | 8 |
| backroom-storage | Backroom Storage | 5 × 2 | 2,5 × 1,0 m | 2,4 m | almacenamiento | 80 |
| receiving-crate | Receiving Crate | 2 × 2 | 1,0 × 1,0 m | 0,8 m | recepción | 24 |
| decoration-plant | Decorative Plant | 1 × 1 | 0,5 × 0,5 m | 1,2 m | decoración | 0 |

Estas dimensiones funcionales tienen prioridad sobre las medidas impresas en el concept target, que son aproximaciones visuales. Por ejemplo, el target ilustra un mostrador de `2,0 × 0,8 m`, mientras el catálogo actual usa `2,0 × 1,0 m`. La diferencia no es un error si el modelo final respeta footprint y circulación; debe registrarse en el asset sheet.


# 40. Props y decoración

Los props añaden historia y rompen repetición sin crear interacción falsa. Categorías permitidas:

- plantas y maceteros;
- bolsas, etiquetas, tickets y material de caja;
- cajas, cinta, hand truck o elementos de receiving;
- pósteres y paneles ficticios;
- pequeños objetos personales de empleados;
- elementos de mantenimiento;
- señalética de navegación;
- merchandising ficticio.

Reglas:

- la decoración no invade footprints ni cola;
- los objetos pequeños se agrupan para reducir draw calls y ruido;
- las plantas aportan verde orgánico distinto del verde de feedback;
- los pósteres evitan texto ofensivo, marcas reales y carátulas copiadas;
- una decoración que sugiera función no implementada debe evitarse o etiquetarse claramente como ambientación;
- el nivel de desorden de Tier E es “vivido y operativo”, no “sucio y negligente”.


# 41. Productos físicos y packaging

El producto es el principal portador de color. Debe reconocerse por categoría antes de leer su nombre. Las cajas de juego usan proporción de case; las consolas, volumen mayor y packaging de alto valor; los mandos y headsets, silueta específica; los accesorios, forma compacta y etiqueta clara.

El packaging debe incluir:

- marca ficticia;
- nombre de producto;
- familia o plataforma ficticia cuando proceda;
- jerarquía frontal simple;
- color de familia;
- iconos genéricos originales;
- edad o clasificación ficticia solo si no imita un sistema protegido;
- laterales y trasera simplificados para vistas cercanas;
- material de cartón/plástico coherente;
- versión de caja para recepción cuando el catálogo la requiera.

No es necesario que todo texto sea legible a cámara media. Sí es necesario que el patrón frontal diferencie SKU y no parezca una textura aleatoria.


# 42. Seis productos funcionales vigentes

| ID | Nombre | Categoría | Presentación | Unidades por caja |
| --- | --- | --- | --- | --- |
| game-neon-drift | Neon Drift | videojuego | caja de juego | 12 |
| case-cloud-runner | Cloud Runner Case | juego/case físico | caja física | 16 |
| console-vertex-one | Vertex One Console | consola | consola + packaging | 2 |
| controller-orbit-pad | Orbit Pad Controller | mando | mando + packaging | 6 |
| headset-signal-pro | Signal Pro Headset | auriculares | headset + packaging | 4 |
| accessory-memory-core | Memory Core Accessory | accesorio | accesorio + packaging | 10 |

Los nombres conceptuales Byte Frontier, Wildbyte, Cloudspire S-One, Cloudwave y Dual Dock pertenecen a referencias visuales. No sustituyen automáticamente los IDs funcionales `game-neon-drift`, `console-vertex-one`, `headset-signal-pro`, etc. Antes de adoptar un nombre conceptual debe comprobarse disponibilidad legal, coherencia de catálogo y trazabilidad.


# 43. Marcas ficticias y sistema de proveedores

Las marcas deben parecer parte de un mercado, no variaciones del logo Cartridge & Cloud. Cada proveedor puede definir:

- nombre y logotipo original;
- dos colores principales y un neutro;
- lenguaje de formas;
- tono de packaging;
- nivel de precio o especialidad;
- familia de productos;
- etiquetas y símbolos propios.

Se prohíbe copiar logotipos, nombres, tipografías distintivas, formas de consola protegidas, carátulas o trade dress reconocible. “Inspiración retro” debe expresarse mediante época, materiales y composición, no mediante réplica. Las empresas proveedoras de los personajes conceptuales usan azul/naranja como ejemplo de independencia cromática respecto a VRM.


# 44. Variedad de producto y rareza

La variedad puede construirse sin multiplicar meshes únicos:

- variantes de material y etiqueta;
- color de lomo;
- arte frontal modular;
- sticker de oferta o edición;
- tamaño de packaging;
- agrupación en balda;
- estado de stock/llenado;
- cajas master de recepción;
- accesorios compartidos.

Las rarezas o ediciones premium deben comunicarse con contraste, composición y detalle, no solo con dorado. El sistema de datos debe indicar categoría y valor; el arte no inventará una rareza que la lógica no reconoce. Las variantes no deben romper el ID funcional si solo son presentación.


# 45. Empleados

Los empleados representan la marca de la tienda. Las referencias aprobadas usan uniforme grafito/negro con verde, calzado práctico, credencial y accesorios funcionales. La silueta debe diferenciarse de clientes incluso cuando se vea de espaldas.

Elementos recomendados:

- chaqueta, polo o sudadera de trabajo;
- acento verde en vivo, cremallera, panel o logo;
- badge o lanyard;
- pantalón oscuro;
- calzado estable;
- móvil, scanner, caja o producto según tarea;
- variantes de pelo, piel, cuerpo y género sin perder uniforme.

No todos los empleados necesitan gorra. El logo debe mantenerse legible, pero no ocupar toda la ropa. El uniforme puede evolucionar por tier, conservando paleta y silueta profesional.


# 46. Clientes

Los clientes no siguen la paleta de marca. Deben aportar variedad de edad adulta/joven adulta, estilos, colores, siluetas y accesorios. Los conceptos muestran coleccionista, streetwear, gamer expresivo y comprador casual. Son arquetipos visuales, no estereotipos de comportamiento rígido.

Reglas:

- lectura de intención mediante postura, dirección de cabeza, manos y props;
- ropa con dos o tres masas cromáticas, evitando arcoíris accidental;
- proporciones estilizadas compatibles con puertas y muebles;
- diversidad sin convertir diferencias físicas en indicador de paciencia, riqueza o moral;
- accesorios que no atraviesen estantes o carrito;
- variantes suficientes para que cuatro clientes simultáneos no parezcan clones;
- a largo plazo, 4 arquetipos heredados son un mínimo de referencia, no el volumen final.


# 47. Proveedores

Los proveedores deben leerse como externos a Cartridge & Cloud. El representante comercial puede usar uniforme limpio, catálogo o muestras; el operario logístico, ropa resistente, guantes, gorra y carro. Ambos comparten identidad de su empresa ficticia, no necesariamente la misma silueta.

La paleta puede ser azul/naranja, rojo/gris u otra combinación original. El proveedor general del vertical slice necesita suficiente diferenciación respecto a cliente y empleado. Su presencia debe ayudar a entender receiving, cajas y flujo de entrega, no introducir una cinemática compleja.


# 48. Personajes: estado real y objetivo

El catálogo de presentación define `employee-main`, `customer-base` y `supplier-base`. Los prefabs actuales contienen ID, rol y material variant, pero no modelo visible, Animator ni Collider. Los clips de animación existen como assets separados. Por tanto:

- el concepto visual de personajes está aprobado;
- la representación actual de Fase 1 sigue siendo placeholder o generada por runtime;
- la integración de modelos, rig y animaciones no puede marcarse como completa;
- los prefabs deben evolucionar sin cambiar IDs funcionales;
- el primer objetivo es una silueta representativa y animación suficiente, no facial compleja.

La revisión de Sprint 16 debe comprobar que cliente, empleado y proveedor son distinguibles en StoreInitial, no solo en un render aislado.


# 49. Proporción, rig y animación

La proporción es estilizada pero compatible con objetos humanos. Cabeza y manos pueden ampliarse moderadamente para lectura; brazos y piernas deben permitir caminar, observar, coger producto, esperar, pagar y mover caja sin interpenetraciones extremas.

Clips presentes:

- Idle;
- Walk;
- Observe;
- PickProduct;
- QueueWait;
- Checkout;
- Satisfied;
- Frustrated;
- MoveCrate;
- PlayerPlace;
- PlayerRemove.

La animación económica prioriza root estable, poses claras y tiempos legibles. La emoción se comunica mediante postura y ritmo. Las animaciones deben probarse con el rig final; la existencia de `.anim` no garantiza compatibilidad. El personaje no debe deslizarse, flotar ni cambiar escala al reproducir.


# 50. Iconografía y señalética del mundo

La señalética física debe usar un sistema coherente con la UI sin copiarla literalmente. Funciones:

- identificar tienda y marca;
- orientar hacia checkout, salida, almacén o receiving;
- diferenciar zonas de personal;
- comunicar ofertas y productos destacados;
- apoyar tutorial o estado de cierre;
- etiquetar proveedores y cajas.

Los iconos deben mantener grosor y simplificación similares. El logo nube/mando puede ser el emblema principal. Las palabras deben prepararse para ES/EN o evitarse cuando un símbolo sea suficiente. Todo texto generado en concept art se considera placeholder hasta revisión ortográfica, legal y de localización.


# 51. Relación entre arte del mundo y UI

El mundo y la UI comparten grafito, verde, cyan, neutros y estados, pero cumplen funciones distintas. La UI necesita contraste constante y jerarquía; el mundo está sometido a luz y material. No se copiarán valores sin probar.

Un punto interactivo puede usar:

- forma o prop físico;
- highlight selectivo;
- prompt contextual;
- icono;
- sonido;
- cambio temporal de material.

No se colocarán paneles UI 3D permanentes sobre cada mueble. El HUD no debe tapar el checkout o la puerta. Los estados de construcción deben ser visibles sobre geometría sin producir un baño de color opaco que impida juzgar la forma.


# 52. VFX y feedback visual

El VFX actual se apoya principalmente en materiales de feedback y eventos; `Assets/_Project/VFX` no contiene assets de producción. La Art Bible define una dirección mínima:

- highlight o ghost de colocación;
- huella válida/inválida;
- confirmación discreta de compra, venta o reposición;
- indicador de stock o reserva cuando sea necesario;
- aviso de cierre;
- feedback de guardado;
- apertura/cierre de puerta;
- selección y hover.

Las partículas deben ser discretas, cortas y localizadas. No se usará confeti intenso para cada venta. El feedback debe respetar el tono de negocio y no ocultar cliente o producto. La futura Audio Bible coordinará sincronía y prioridad sonora.


# 53. Integración audiovisual

Aunque el diseño de audio se documentará por separado, el arte debe reservar fuentes y tiempos visuales. El catálogo actual contiene música de tienda, ambiente y diez clips de audio base para placement, UI, pedido, checkout, puerta y cierre.

Reglas de coordinación:

- una puerta visualmente abierta no puede reproducir cierre;
- el checkout confirma después del commit, no antes;
- el feedback de error usa color, forma y sonido coherentes;
- el ambiente no debe sugerir multitud cuando la tienda está vacía;
- una luminaria o monitor animado no necesita sonido constante;
- audio y VFX deben poder desactivarse o reducirse sin perder información crítica.


# 54. Unidades, escala y cuadrícula

- Unity: `1 unidad = 1 metro`.
- Grid lógico: `0,5 m` por celda.
- Rotación de placement: incrementos de `90°` salvo asset expresamente no colocable.
- Personajes: altura aproximada coherente con `1,65–1,85 m` estilizados.
- Altura visual de StoreInitial: alrededor de `3 m` como referencia, ajustable por cámara y módulos.
- Módulos principales: 1 m, 2 m y 4 m.

No se arreglará un asset mal exportado mediante escala arbitraria en la escena si puede corregirse en origen. Las escalas no uniformes deben ser excepción. La escala se valida con personaje, grid, puerta, mueble, collider y cámara.


# 55. Pivotes y orientación

Convención recomendada:

- eje Y hacia arriba en Unity;
- export FBX con forward `-Z`, up `Y`, unit scale aplicado;
- pivot de arquitectura en punto modular predecible, preferentemente esquina o centro de base según familia;
- pivot de mueble en suelo y centro de footprint, con frente documentado;
- pivot de producto en base/centro para display;
- puerta con root fijo y hojas independientes;
- elementos rotables sin offsets acumulados;
- colliders y anchors bajo transforms de escala uniforme.

El frente debe identificarse en prefab sheet y, cuando sea útil, mediante gizmo de Editor. No se deducirá orientación por nombre de mesh importado.


# 56. Blender Modular Kit v0.1

El kit de Blender es un punto de partida paramétrico para assets no personajes. Incluye arquitectura A01–A10, mobiliario F01–F08, productos P01–P06, módulos conceptuales de ocho áreas futuras, materiales de preview, generación básica de LOD/colliders, validación y export batch.

Su README advierte que la geometría generada es “production-oriented parametric starting point”, no arte pulido final. Por tanto:

- ejecutar `build_all.py` no aprueba visualmente los assets;
- revisar proporción, biseles, nombres y transforms antes de exportar;
- ejecutar validación;
- guardar `.blend` fuente;
- exportar solo tras revisión visual y funcional;
- los materiales de Blender son preview, no shaders URP finales;
- personajes, rig y animación están fuera del kit.


# 57. Exportación FBX

El exportador individual usa selección por asset root, familias Architecture/Furniture/Products/Expansions, eje forward `-Z`, up `Y`, unit scale, triangulación y sin animación. Puede incluir colliders descendientes si existen.

Convención actual de FBX:

```text
CC_S16_P2_<Familia>_<Nombre>_LOD0.fbx
```

El sufijo `_LOD0` del archivo no significa que todo mesh interno sea un único LOD; la integración actual puede generar jerarquías LOD0/LOD1/LOD2. Antes de importar:

- aplicar transforms;
- eliminar objetos ocultos accidentales;
- comprobar normales y tangentes;
- revisar nombres duplicados;
- comprobar origen;
- garantizar que la caja de bounds representa el asset, no el conjunto completo;
- separar mesh de collider y anchors.


# 58. Importación en Unity 6

La integración de Sprint 16 corrigió API de normals/tangents, clasificación de FBX `_LOD0`, transferencia de LODGroup y enlace a RuntimeAssetRegistry. Estas reparaciones son evidencia de pipeline, no aprobación visual.

Checklist de importación:

- escala 1;
- orientation correcta;
- Read/Write solo si es necesario;
- normals/tangents según shader;
- materials remapeados a assets compartidos;
- no duplicar materiales embebidos;
- LODGroup con renderers correctos;
- colliders convertidos o reemplazados por técnica explícita;
- prefab en ruta canónica;
- ID registrado;
- instancia de prueba en TestLab o escena de revisión;
- sin warnings recurrentes en Console.


# 59. Materiales URP

La baseline usa URP `17.3.0`. Los materiales actuales son principalmente URP Lit con color, metallic y smoothness. La Art Bible establece:

- usar materiales compartidos por familia;
- evitar materiales duplicados por prefab;
- usar variantes mediante MaterialPalette, propiedades o atlas;
- reservar emisión para señalética/acento;
- revisar transparencias en build;
- no crear instancias runtime innecesarias;
- mantener SRP Batcher compatible cuando sea posible;
- documentar shaders custom;
- no introducir un shader de outline global sin spike.

Los materiales técnicos de S4/S5 siguen disponibles para ghosts y markers. No deben reemplazar el acabado representativo salvo en modo construcción o debug.


# 60. Anatomía de prefab

Estructura recomendada:

```text
AssetRoot
├── Visual
│   ├── LOD0
│   ├── LOD1
│   └── LOD2
├── TechnicalColliders
├── InteractionAnchors
├── ProductSlots          # cuando aplique
└── Metadata / RepresentativePrefabInstance
```

No todos los assets necesitan tres LOD. Sí necesitan una estructura comprensible. El root debe estar a escala uniforme. Los componentes de lógica de negocio no deben depender de mesh names. Los anchors no deben ocultarse dentro de la geometría importada sin referencia explícita.

El prefab debe poder sustituirse manteniendo ID, footprint, anchors y material variant. Si un cambio visual requiere modificar reglas de capacidad, interacción o persistencia, ya no es un reemplazo puramente artístico y debe registrarse como cambio de diseño/datos.


# 61. Colliders, navegación y puntos funcionales

La documentación histórica afirma que se generaron colliders `_COL`, pero los prefabs serializados observados no contienen componentes Unity `BoxCollider`, `MeshCollider` o `CapsuleCollider`. Esto obliga a una clasificación prudente: **collider pipeline documentado, integración de collider no demostrada en prefab**.

Reglas:

- los colliders técnicos de Store/StoreInitial siguen siendo autoridad temporal;
- cada prefab interactivo debe definir o recibir un volumen validado;
- evitar MeshCollider complejo para mobiliario estático si bastan cajas compuestas;
- no usar renderer bounds como collider de gameplay;
- la puerta separa trigger, marco y hojas;
- las baldas no necesitan colisionar con cada producto si el volumen general es suficiente;
- navegación y collider deben revisarse juntos;
- el jugador y NPC no atraviesan geometría visible;
- el collider no bloquea un pasillo que visualmente está abierto.


# 62. LOD y distancia

El generador de Blender usa como punto de partida ratios aproximados de `60 %` para LOD1 y `25 %` para LOD2. La implementación observada contiene LODGroup en las 8 familias de mobiliario, los 10 prefabs de producto/packaging y los 19 prefabs conceptuales de expansión; parte de la arquitectura también tiene LODGroup.

Reglas:

- no crear LOD por obligación en un módulo trivial si el ahorro es nulo;
- mantener silueta, volumen y material principal;
- eliminar detalle pequeño antes que deformar la forma funcional;
- revisar popping desde cámara orbital;
- productos muy pequeños pueden usar un solo LOD o agruparse;
- personajes requieren estrategia propia tras integrar modelos;
- las transiciones se prueban con FOV y distancia reales;
- LOD no sustituye culling, batching, atlas o control de densidad.


# 63. Presupuestos artísticos iniciales

Estos valores son guardrails de producción, no contratos rígidos. Deben ajustarse por profiling:

| Categoría | LOD0 orientativo | Textura orientativa | Observación |
|---|---:|---:|---|
| Producto pequeño | 500–3.000 tris | 256–1024 | priorizar silueta y etiqueta |
| Packaging | 100–1.500 tris | 256–1024 | gran parte puede resolverse por material/decal |
| Mueble | 2.000–15.000 tris | 512–2048 | compartir materiales y módulos |
| Arquitectura modular | 500–10.000 tris/módulo | tiling/trim | evitar caras ocultas y duplicación |
| Prop decorativo | 200–5.000 tris | 256–1024 | agrupar cuando sea pequeño |
| Personaje estilizado | 15.000–35.000 tris | 1024–2048 | pendiente de rig y cámara |

La métrica importante es frame time, memoria, draw calls, overdraw y claridad. Un asset por debajo del presupuesto puede seguir siendo ineficiente si usa materiales únicos, transparencias o scripts costosos.


# 64. Occlusion, clipping y paredes

La cámara elevada puede quedar bloqueada por paredes. `StoreRuntimeSettings` mantiene `_hideOccludingWalls: 1`, por lo que la dirección debe tolerar ocultación o fade. Las paredes exteriores necesitan verse bien tanto presentes como retiradas.

Reglas:

- no colocar información crítica solo en la cara interior de una pared que se oculta;
- evitar props colgados que queden flotando cuando la pared se desvanece;
- agrupar pared y decoración para una ocultación coherente o definir comportamiento específico;
- revisar vidrio y fachada con cámara cercana/lejana;
- no usar techos completos si bloquean lectura, salvo sistema de corte;
- la oclusión debe evitar revelar huecos, backsides o meshes sin terminar.


# 65. Rendimiento visual objetivo

El objetivo heredado del vertical slice es `60 FPS a 1920 × 1080` con tienda equipada y hasta 8 clientes simultáneos, sujeto a hardware documentado. La Art Bible contribuye mediante:

- materiales compartidos;
- SRP Batcher;
- LOD donde aporta;
- control de transparencias y emisión;
- luces limitadas;
- baked/mixed cuando proceda;
- atlas/trims;
- agrupación de productos pequeños;
- ausencia de scripts de presentación por objeto cuando puede centralizarse;
- culling y densidad controlada.

El arte no se aprueba solo en Scene View. Debe revisarse en Game View y build, con Profiler cuando haya cambio significativo. El profiling final pertenece a Sprint 17.


# 66. Nomenclatura de assets

La nomenclatura histórica `ENV_`, `FUR_`, `PRD_`, `CHR_`, `VFX_`, `UI_` se conserva como taxonomía conceptual, pero el proyecto actual usa convenciones más específicas:

- referencia conceptual: `CC_ART_REF_<Subject>_<Purpose>_v###`;
- FBX de Sprint 16: `CC_S16_P2_<Family>_<Name>_LOD0.fbx`;
- prefab Unity: PascalCase (`CheckoutCounter.prefab`);
- material Unity: PascalCase por función (`FurnitureCheckout.mat`);
- ID funcional: kebab-case (`checkout-counter`);
- ID jerárquico de prefab futuro: dotted/kebab (`expansion.server.rack`);
- animación: verbo/estado (`Walk.anim`, `QueueWait.anim`);
- evidencia: incluir sprint, paquete y tipo de captura.

No renombrar IDs estables para “hacerlos bonitos”. Los nombres visibles pueden localizarse y cambiar; los IDs requieren migración.


# 67. Estructura de carpetas

Rutas canónicas principales:

```text
Assets/_Project/
├── Art/Materials/<Category>/
├── Audio/<Channel>/
├── Animations/Characters/
├── Data/Catalogs/
├── Prefabs/Architecture/
├── Prefabs/Furniture/
├── Prefabs/Products/
├── Prefabs/Characters/
├── Prefabs/Expansions/
├── Scenes/
├── Settings/Runtime/
└── VFX/
```

Las fuentes DCC viven bajo `Tools/Blender/CC_Blender_Modular_Kit_v0.1`. No se colocan `.blend` o exportaciones temporales dentro de rutas runtime sin intención. Los archivos de referencia conceptual viven en Documentation, no en Resources.


# 68. IDs, catálogos y reemplazabilidad

Los catálogos actuales son:

- `ContentCatalog.asset`: muebles y productos funcionales;
- `MaterialPalette.asset`: material variant IDs;
- `PresentationCatalog.asset`: personajes, animaciones y feedback;
- `AudioCatalog.asset`: eventos y clips;
- `RepresentativePrefabCatalog.asset`: prefabs de arquitectura, mobiliario, productos y expansiones;
- `RuntimeAssetRegistry.asset`: enlace de settings, catálogos, shell, paleta, presentación, audio y prefabs.

La frontera de reemplazo permite mejorar un mesh o material sin tocar reglas. Para conservarla:

- ID no depende de nombre de archivo;
- prefab no contiene estado persistente accidental;
- material variant ID es estable;
- Resources path solo se cambia con actualización de catálogo;
- tests verifican referencias;
- el save no serializa referencias directas a objetos visuales.


# 69. Variantes y modularidad

La reutilización debe ser visible como sistema, no como clonación. Técnicas:

- intercambiar material o label;
- rotar props decorativos;
- cambiar contenido de baldas;
- usar end caps y esquinas;
- combinar módulos de 1/2/4 m;
- variar plantas, cajas y posters;
- alterar estado de llenado;
- usar paletas de proveedor;
- cambiar accesorios de personaje;
- conservar base común de rig o mobiliario.

No se crearán veinte meshes casi iguales si una familia modular resuelve el problema. Tampoco se abusará de una única estantería en toda la tienda. Cada área importante necesita al menos una silueta o composición distintiva.


# 70. Flujo de creación de un asset

1. Registrar necesidad y rol.
2. Identificar ID funcional o crear propuesta.
3. Consultar footprint, capacidad y interacción.
4. Reunir referencias legales y de estilo.
5. Crear thumbnail/silueta.
6. Hacer blockout a escala.
7. Probar en cámara y grid.
8. Aprobar forma principal.
9. Modelar LOD0 y, cuando proceda, LOD1/LOD2.
10. Preparar UV, trims, decals o color blocking.
11. Crear materiales compartidos.
12. Definir pivot y frente.
13. Preparar collider/anchors.
14. Validar en Blender.
15. Exportar FBX.
16. Importar y remapear en Unity.
17. Crear prefab y registrar ID.
18. Instanciar en escena de revisión.
19. Ejecutar revisión visual y técnica.
20. Registrar procedencia, licencia y evidencia.

Ninguna fase debe saltarse porque el asset “se ve bien en Blender”.


# 71. Flujo de integración en StoreInitial

1. Duplicar o abrir StoreInitial desde baseline segura.
2. Crear/actualizar `StoreInitialEnvironment.prefab`.
3. Montar suelo y arquitectura manualmente.
4. Configurar puerta con referencias explícitas.
5. Colocar mobiliario inicial con footprints.
6. Añadir Lighting, Anchors y TechnicalColliders.
7. Crear/validar `StoreInitialSceneContext`.
8. Conectar RuntimeAssetRegistry y catálogos.
9. Desactivar builder procedural solo tras comparación.
10. Corregir input UI/mundo.
11. Ejecutar EditMode y PlayMode.
12. Ejecutar recorrido manual y Golden Path.
13. Generar build post-integración.
14. Revisar Player.log.
15. Capturar evidencia cercana, media y lejana.
16. Obtener aprobación visual manual.

Cada cambio grande debe poder revertirse sin destruir la escena técnica anterior.


# 72. Revisión visual

La revisión visual se realiza con un checklist y capturas comparables. Debe incluir:

- vista cercana, media y lejana;
- entrada y fachada;
- checkout y cola;
- exposición y pasillos;
- receiving y backroom;
- cámara en esquinas y oclusión;
- estado válido/inválido de placement;
- tienda vacía y con clientes;
- apertura y cierre;
- UI visible y oculta;
- resolución de referencia `1920 × 1080`;
- build externa cuando el gate lo exige.

Se evalúan escala, silueta, contraste, densidad, repetición, materiales, iluminación, interpenetraciones, floating meshes, clipping, texto, marcas y coherencia con función. Los comentarios deben señalar ubicación, gravedad y criterio, no “no me gusta”.


# 73. Revisión técnica

La revisión técnica verifica:

- ruta y nombre;
- GUID/meta estable;
- escala, pivot y orientación;
- material compartido;
- LODGroup y renderers;
- collider/technical volume;
- anchors;
- footprint;
- catalog ID y Resources path;
- ausencia de dependencias indebidas;
- carga en Editor y build;
- warnings/errors;
- draw calls, transparencias y luces;
- navegación;
- persistencia si el objeto es colocable;
- licencia y fuente.

Un asset puede obtener PASS visual y FAIL técnico, o al revés. Solo se considera aprobado cuando cumple ambos o existe excepción formal acotada.


# 74. Evidencia artística

Cada paquete de arte debe conservar como mínimo:

- ID del asset o escena;
- versión/commit;
- fuente DCC;
- captura de Blender cuando sea relevante;
- captura en Unity cercana/media/lejana;
- captura de wireframe o LOD si se revisa optimización;
- material y shader usados;
- collider/anchor visible en debug;
- prueba en Game View;
- resultado de importación;
- build ID cuando aplique;
- defectos conocidos;
- licencia/procedencia;
- decisión de aprobación y responsable.

Los nombres de evidencia deben permitir ordenar por sprint y work package. Una captura suelta sin commit ni contexto no es evidencia reproducible.


# 75. Criterios de aceptación de Sprint 16

Sprint 16 no cierra hasta obtener PASS en:

1. prefabs representativos cargan;
2. EditMode completo;
3. PlayMode completo;
4. arquitectura visual correcta;
5. puerta visual y funcional correcta;
6. mobiliario inicial colocado;
7. UI no propaga clicks al mundo;
8. Golden Path post-scene;
9. Windows build post-scene;
10. aprobación visual manual.

Los puntos 1–3 ya tienen evidencia previa. Los puntos 4–7 estaban abiertos y 8–10 pendientes. La Art Bible añade que la aprobación visual debe verificar las tres distancias de cámara, contraste de zonas, materiales, colliders visualmente coherentes, ausencia de placeholder no exceptuado y legibilidad de producto/personaje.


# 76. Criterios artísticos de H6

H6 no exige arte final completo ni volumen de lanzamiento. Exige un nivel representativo que permita juzgar la calidad objetivo. Debe cumplirse:

- StoreInitial autorada y aprobada;
- seis productos diferenciables;
- ocho familias de mobiliario funcionales o representadas según alcance;
- empleados/clientes/proveedor distinguibles;
- arquitectura, puerta y zonas coherentes;
- materiales y luz suficientes para identidad;
- feedback de interacción comprensible;
- no hay placeholders visibles no aprobados;
- no hay meshes flotantes/interpenetrados graves;
- colliders y visuales no se contradicen;
- rendimiento medido;
- build externa completa Golden Path;
- evidencia y known issues documentados.

Una deuda cosmética A3 puede aceptarse; una puerta que abre al lado equivocado, una pared mal escalada o un mueble que bloquea navegación no.


# 77. Elementos prohibidos o no aceptables

- marcas, consolas, logotipos o carátulas reales sin licencia;
- texto generado ilegible presentado como final;
- placeholders técnicos visibles sin excepción;
- meshes flotantes o penetraciones evidentes;
- escalas no uniformes usadas para corregir export defectuoso;
- colores de feedback dependientes solo del color;
- emisión excesiva en todas las superficies;
- vidrio invisible o sorting roto;
- colliders que bloquean huecos visibles;
- materiales duplicados por cada instancia;
- personajes clonados sin variación en una escena de evaluación;
- ruido visual que oculta pasillos, cola o interacción;
- assets futuros usados para prometer alcance no aprobado;
- capturas conceptuales etiquetadas como gameplay;
- props con marcas de terceros o contenido ofensivo accidental;
- nombres temporales convertidos en IDs de save.


# 78. Política de placeholders

Un placeholder puede permanecer cuando:

- está fuera del recorrido obligatorio;
- tiene excepción registrada;
- no aparece en captura o build candidata pública;
- no comunica una función equivocada;
- su reemplazo no bloquea la evaluación del sistema;
- está identificado en known issues.

Los placeholders de StoreInitial, puerta, checkout, productos o personajes del Golden Path son bloqueantes si impiden evaluar escala, silueta, circulación o identidad. Los marcadores técnicos pueden mantenerse en TestLab o modo debug. La transición se realiza por capas, no ocultando objetos rotos fuera de cámara.


# 79. Licencias, procedencia e IA generativa

Todo asset debe registrar autor, fuente, fecha, licencia, modificaciones, restricciones y uso previsto. Los conceptos generados con IA se consideran referencias de dirección, no necesariamente assets distribuibles ni diseños finales exclusivos. Antes de convertirlos en contenido comercial se deben rediseñar, revisar similitudes, corregir texto y registrar el proceso.

Reglas:

- no entrenar ni derivar intencionalmente de una marca o artista concreto sin permiso;
- no copiar carátulas o hardware reconocible;
- comprobar licencias de fuentes, iconos, texturas, audio y plugins;
- conservar créditos obligatorios;
- no distribuir archivos fuente con restricciones incompatibles;
- la aprobación artística no sustituye aprobación legal;
- todo material de marketing debe indicar si es concepto, mockup o captura real.


# 80. Registro de referencias conceptuales

| Grupo | Archivo | Uso y límite |
| --- | --- | --- |
| 00_Visual_Targets | ChatGPT Image 29 jun 2026, 21_37_54.png | Target de StoreInitial Tier E: tienda compacta, paleta, cámara, materiales, módulos y escala. |
| Objetos | ChatGPT Image 29 jun 2026, 21_38_14 (1).png | Caja de juego ficticio Byte Frontier; referencia no canónica de packaging. |
| Objetos | ChatGPT Image 29 jun 2026, 21_38_14 (2).png | Caja/edición ficticia Wildbyte; referencia de premium packaging. |
| Objetos | ChatGPT Image 29 jun 2026, 21_38_14 (3).png | Consola ficticia Cloudspire S-One; referencia no canónica. |
| Objetos | ChatGPT Image 29 jun 2026, 21_38_14 (4).png | Mando estilizado; referencia de silueta y materiales. |
| Objetos | ChatGPT Image 29 jun 2026, 21_38_14 (5).png | Headset ficticio Cloudwave; referencia de producto y packaging. |
| Objetos | ChatGPT Image 29 jun 2026, 21_38_14 (6).png | Dual Dock ficticio; referencia de accesorio. |
| Objetos | ChatGPT Image 29 jun 2026, 21_38_14 (7).png | Mostrador de checkout. |
| Objetos | ChatGPT Image 29 jun 2026, 21_38_14 (8).png | Estantería/expositor de pared. |
| Objetos | ChatGPT Image 29 jun 2026, 21_38_14 (9).png | Vitrina de cristal. |
| Objetos | ChatGPT Image 29 jun 2026, 21_38_14 (10).png | Estantería de almacén. |
| Personajes | ChatGPT Image 29 jun 2026, 21_38_50 (1).png | Empleado de tienda, variante masculina. |
| Personajes | ChatGPT Image 29 jun 2026, 21_38_50 (2).png | Empleada de tienda, variante femenina. |
| Personajes | ChatGPT Image 29 jun 2026, 21_38_50 (3).png | Cliente coleccionista o aficionado. |
| Personajes | ChatGPT Image 29 jun 2026, 21_38_50 (4).png | Cliente joven de streetwear colorido. |
| Personajes | ChatGPT Image 29 jun 2026, 21_38_50 (5).png | Cliente con estética gamer expresiva. |
| Personajes | ChatGPT Image 29 jun 2026, 21_38_50 (6).png | Cliente adulto casual. |
| Personajes | ChatGPT Image 29 jun 2026, 21_38_50 (7).png | Representante comercial de proveedor. |
| Personajes | ChatGPT Image 29 jun 2026, 21_38_50 (8).png | Operario logístico de proveedor. |
| Visión futura | ChatGPT Image 29 jun 2026, 21_39_17 (1).png | Almacén y logística avanzada. |
| Visión futura | ChatGPT Image 29 jun 2026, 21_39_17 (2).png | Tienda con público y mayor densidad. |
| Visión futura | ChatGPT Image 29 jun 2026, 21_39_17 (3).png | Tienda ampliada con backroom. |
| Visión futura | ChatGPT Image 29 jun 2026, 21_39_17 (4).png | Oficina de publishing. |
| Visión futura | ChatGPT Image 29 jun 2026, 21_39_17 (5).png | Estudio de desarrollo interno. |
| Visión futura | ChatGPT Image 29 jun 2026, 21_39_17 (6).png | Plataforma/espacio digital de marca. |
| Visión futura | ChatGPT Image 29 jun 2026, 21_39_17 (7).png | Infraestructura de servidores. |
| Visión futura | ChatGPT Image 29 jun 2026, 21_39_17 (8).png | Análisis de mercado y planificación. |

Las imágenes residen en `Documentation/01_Working_Documents/Sprint_16/Art_3D/Concept_Art`. El archivo principal incluye explícitamente “Conceptual — no representa gameplay real”. Esa etiqueta debe conservarse en cualquier uso externo hasta disponer de captura equivalente de build.


# 81. Lectura consolidada del target StoreInitial Tier E

El target principal aporta estas decisiones útiles:

- perímetro y marcos grafito;
- paredes claras y suelo gris cálido;
- madera localizada en checkout;
- estanterías metálicas oscuras;
- vitrina como foco central;
- recepción con cartón y señal de seguridad;
- puerta central y fachada legible;
- vegetación puntual;
- logos y acentos verdes;
- producto colorido;
- cámara cercana/media/lejana;
- relación de escala con cinco personajes;
- módulos de checkout, shelf, vitrina, storage y receiving.

No debe copiarse literalmente el layout compacto si contradice los `10 × 15 m`, la cola, los footprints o el backroom. Las medidas del target son referencia visual; ContentCatalog y escena aprobada mandan sobre footprint.


# 82. Inventario visual implementado

| Familia | Prefabs | LODGroup serializados | Collider components serializados | Animator serializados | Renderers |
| --- | --- | --- | --- | --- | --- |
| Arquitectura | 24 | 11 | 0 | 0 | 135 |
| Mobiliario | 8 | 8 | 0 | 0 | 144 |
| Productos y packaging | 10 | 10 | 0 | 0 | 96 |
| Personajes | 3 | 0 | 0 | 0 | 0 |
| Expansiones conceptuales | 19 | 19 | 0 | 0 | 459 |

Otros datos observados:

- materiales Unity: 52;
- clips de audio: 10;
- clips de animación: 11;
- texturas de producción encontradas: 0;
- assets en carpeta VFX encontrados: 0.

La cifra de collider components cero no prueba que ningún volumen técnico exista en escenas, pero sí impide considerar cerrada la colisión a nivel de prefab. Las expansiones conceptuales tienen LODGroup, pero no forman parte del alcance de H6.


# 83. Expansiones y evolución visual futura

La evolución histórica se conserva:

| Etapa | Lenguaje visual |
|---|---|
| Tienda pequeña | cálida, compacta, recursos limitados, física |
| Tienda ampliada | más orden, señalización y especialización |
| Logística online | industrial ligera, cartón, metal, flujo |
| Publishing | oficina editorial profesional, catálogo y reuniones |
| Desarrollo interno | estudio creativo, estaciones y prototipos |
| Plataforma digital | marca consolidada, pantallas y servicio |
| Infraestructura | sobria, modular, técnica y legible |
| Mercado/analítica | paneles, mapas, gráficos y planificación |

Los 19 prefabs conceptuales existentes son previsualización de esta evolución, no garantía de que se implementen. Deben permanecer fuera del catálogo jugable o de builds de H6 salvo uso explícito de presentación no interactiva.


# 84. Catálogo de prefabs conceptuales futuros

| Prefab conceptual | Área |
| --- | --- |
| Digital/CodeCardRack | Digital |
| Digital/Kiosk | Digital |
| Ecommerce/OrderDesk | Ecommerce |
| Ecommerce/SortBins | Ecommerce |
| Logistics/Conveyor | Logistics |
| Logistics/PackingStation | Logistics |
| Logistics/RollingCart | Logistics |
| Market/AnalysisWall | Market |
| Market/PlanningTable | Market |
| Publishing/LineupWall | Publishing |
| Publishing/MeetingTable | Publishing |
| Server/OperationsDesk | Server |
| Server/Rack | Server |
| Server/UPS | Server |
| Staff/Locker | Staff |
| Staff/TaskBoard | Staff |
| Studio/ArtDesk | Studio |
| Studio/AudioStation | Studio |
| Studio/DevDesk | Studio |

Cada bloque futuro deberá recibir su propia Art Bible amendment o sección aprobada antes de producción. No se reutilizará automáticamente el verde/negro de tienda en todas las oficinas: la identidad crece, pero cada espacio necesita función, ocupantes y materiales propios.


# 85. Marketing art y capturas

Todo material se clasifica como:

- **captura real de build**;
- **captura de Editor**;
- **mockup**;
- **arte conceptual**;
- **key art**;
- **diagrama de producción**.

La etiqueta debe acompañar archivo y publicación. Las cápsulas de Steam pueden ser aspiracionales, pero no mostrar sistemas inexistentes como gameplay. Una composición futura de servidores, publishing o estudio no debe aparecer como captura del juego actual. Las capturas de StoreInitial deben usar build y HUD reales cuando se presenten como gameplay.

Se conservará una cámara de captura reproducible, resolución, build ID, ajustes gráficos y fecha. No se retocará una captura hasta cambiar la lectura de producto o sistema sin indicarlo.


# 86. Deuda histórica resuelta y decisiones pendientes

El cierre de Sprint 16 resolvió la deuda representativa que afectaba a `StoreInitial`: composición, `StoreInitialEnvironment.prefab`, `StoreInitialSceneContext`, conexión runtime, retirada del blockout procedural productivo, muros/puerta/almacén, personajes visuales, colliders autorados y build post-integración fueron validados en `0.0.21`.

Continúan abiertas para Sprint 17/H6 o fases posteriores:

- ampliar o sustituir texturas que sigan siendo provisionales cuando la revisión artística lo requiera;
- incorporar VFX dedicados únicamente donde aporten feedback verificable;
- decidir y probar el shader de outline o una alternativa accesible;
- completar localización ES/EN de señalética y UI;
- validar resolución objetivo, AA y presupuestos visuales mediante profiling;
- mantener la estrategia de desgaste/material overlay como trabajo futuro;
- consolidar licencias y procedencia en el registro específico.

Estas deudas restantes no reabren Sprint 16, pero pueden bloquear Sprint 17 o H6 según su criterio asociado.


# 87. Protocolo de aprobación de un asset

Un asset recibe uno de estos estados:

| Estado | Significado |
|---|---|
| CONCEPT | dirección exploratoria |
| BLOCKOUT | escala y función en prueba |
| ART READY | forma/material listos para integración |
| TECH READY | prefab, ID, collider, LOD y rutas listos |
| INTEGRATED | usado por escena/runtime |
| VALIDATED | revisión visual/técnica y pruebas aprobadas |
| H6 APPROVED | incluido en candidata y evidencia H6 |
| DEPRECATED | sustituido, mantenido por compatibilidad |

`ART READY` no implica `TECH READY`. `INTEGRATED` no implica `VALIDATED`. Los estados se registrarán en producción y trazabilidad, no solo en nombre de carpeta.


# 88. Checklist para crear StoreInitial

- [ ] Escena parte de una baseline funcional segura.
- [ ] `StoreInitialEnvironment` existe.
- [ ] Architecture está autorada a escala.
- [ ] Floor y walls respetan 10 × 15 m aproximados.
- [ ] Fachada y puerta se leen.
- [ ] Checkout y cola funcionan.
- [ ] Displays preservan circulación.
- [ ] Backroom y receiving se distinguen.
- [ ] InitialFurniture usa footprints.
- [ ] Lighting tiene jerarquía base/producto/acento.
- [ ] Anchors y TechnicalColliders están separados.
- [ ] No hay managers dentro del prefab de entorno.
- [ ] SceneContext valida referencias.
- [ ] Procedural shell está aislado o desactivado.
- [ ] UI no mueve al jugador.
- [ ] EditMode y PlayMode PASS.
- [ ] Golden Path PASS.
- [ ] Build externa PASS.
- [ ] Player.log revisado.
- [ ] Aprobación visual registrada.


# 89. Checklist de asset 3D

- [ ] Rol y ID definidos.
- [ ] Referencia legal registrada.
- [ ] Escala y footprint comprobados.
- [ ] Silueta funciona en cámara media.
- [ ] Top surface funciona en cámara elevada.
- [ ] Pivot y frente correctos.
- [ ] Transforms aplicados.
- [ ] Normales/tangentes correctas.
- [ ] Material compartido.
- [ ] Emisión/transparencia justificadas.
- [ ] LOD adecuado o excepción.
- [ ] Collider o volumen técnico.
- [ ] Anchors/slots.
- [ ] Prefab en ruta canónica.
- [ ] Catalog ID enlazado.
- [ ] Sin warnings.
- [ ] Capturas cercana/media/lejana.
- [ ] Test en build cuando es bloqueante.


# 90. Checklist de personaje

- [ ] Rol distinguible.
- [ ] Proporción compatible con entorno.
- [ ] Silueta legible desde cámara.
- [ ] Paleta de rol correcta.
- [ ] Diversidad revisada.
- [ ] Rig estable.
- [ ] Clips requeridos compatibles.
- [ ] Props y manos no atraviesan.
- [ ] Root motion/locomoción coherentes.
- [ ] Collider y navegación.
- [ ] LOD o estrategia de distancia.
- [ ] Materiales compartidos.
- [ ] Expresión por postura suficiente.
- [ ] Prueba con varios personajes simultáneos.
- [ ] No usa marca real no licenciada.


# 91. Checklist de material e iluminación

- [ ] Material comunica categoría.
- [ ] Color funciona bajo luz real.
- [ ] Smoothness/metallic coherentes.
- [ ] No hay material duplicado accidental.
- [ ] Transparencia ordena correctamente.
- [ ] Emisión no satura.
- [ ] Texto/icono mantiene contraste.
- [ ] La luz no oculta interacción.
- [ ] Sombras no bloquean lectura.
- [ ] Se prueba en cercana/media/lejana.
- [ ] Se prueba con UI y feedback.
- [ ] Se prueba en build.
- [ ] Impacto en rendimiento medido cuando corresponde.


## 91.1. Playbook de revisión visual de StoreInitial por zonas

La revisión de `StoreInitial` no se realizará mediante una única captura heroica. La escena se evaluará por zonas y después como recorrido continuo, porque una composición puede resultar atractiva desde una cámara y fallar al jugar. Cada zona debe demostrar identidad, legibilidad, función y compatibilidad técnica.

**Entrada y fachada.** La entrada debe comunicar inmediatamente que se trata de una tienda de videojuegos y tecnología sin copiar una marca real. El rótulo de VRM Games, el marco, el cristal y la puerta automática deben formar una jerarquía clara. La puerta no puede parecer invertida, desplazada o más pequeña que el flujo de personajes previsto. Deben revisarse el plano exterior, la aproximación del jugador, el paso abierto y cerrado, las sombras sobre el umbral y la lectura desde cámara elevada. Una fachada muy oscura necesita contraste de marco, vidrio y señalización; la emisión verde no debe sustituir una iluminación funcional.

**Zona de exposición.** Los muebles centrales deben crear rutas reconocibles y conservar visión hacia checkout y puerta. Se observarán separación entre familias, densidad de producto, repetición de módulos, alturas y ritmo de siluetas. El objetivo no es llenar cada hueco: el Tier E representa un negocio modesto, organizado y en crecimiento. Los productos destacados pueden usar acento, luz o vacío alrededor; no deben depender de saturar todos los materiales. Las estanterías de pared deben reforzar el perímetro sin producir un túnel oscuro ni ocultar señalética.

**Checkout y cola.** El mostrador debe leerse como destino operativo, no como mesa decorativa. Se comprobarán frente del mueble, punto de interacción, espacio del empleado, entrada y salida de clientes, anchura de la cola, pantalla o terminal, feedback de venta y visibilidad del jugador. El lenguaje negro/verde de marca puede concentrarse aquí. El área debe seguir siendo legible cuando existan varios clientes y mensajes de UI.

**Recepción de mercancía.** Debe diferenciarse de la zona de venta mediante material, organización y utilería, no mediante suciedad excesiva. Cajas, mesa o crate, marcas de suelo y acceso al almacén deben explicar el flujo de entrega. La zona no puede confundirse con una exposición de producto. El color cardboard y el ámbar pueden utilizarse como acentos de logística.

**Almacén.** Debe parecer funcional, compacto y menos elaborado que la sala de venta, pero no abandonado. Se revisarán profundidad de estanterías, acceso, separación respecto a la circulación y compatibilidad con cajas y reposición. El `backroom-storage` tiene un footprint grande y debe colocarse respetando la lógica de acceso. La deuda visual conocida de su posición es bloqueante hasta que el mueble deje de contradecir la función de la sala.

**Iluminación.** Cada zona se revisa con iluminación base, acento y feedback. Ninguna función crítica puede desaparecer al desactivar emisión o al observar desde el ángulo de juego. La luz del almacén puede ser más fría y utilitaria; la sala de venta, más cálida y acogedora; checkout, ligeramente más contrastado. La transición no debe crear una frontera artificial.

**Recorrido integrado.** La última revisión reproduce la ruta: entrada, movimiento, exposición, compra, checkout, recepción, almacén, cierre y retorno. Se capturan vistas cercana, media y lejana en la build candidata. Se anotan oclusiones, colisiones visuales, repetición, orientación, puntos muertos y objetos que parecen interactivos sin serlo. La escena solo avanza a `VALIDATED` si la composición sobrevive al recorrido, no solo a la imagen de presentación.


## 91.2. Perfiles de revisión de las ocho familias de mobiliario

Cada familia de mobiliario debe tener una lectura funcional propia y una revisión coherente con su footprint de catálogo.

**Checkout Counter (`checkout-counter`).** Su frente debe estar orientado hacia la cola y su cara de trabajo hacia el empleado. El volumen de `2,0 × 1,0 m` debe aceptar terminal, superficie de apoyo y espacio de animación sin parecer desproporcionado. Se revisan pivote, orientación, altura de 1,1 m, ausencia de huecos imposibles y coherencia con el punto de interacción. El branding puede ser más intenso que en otros muebles, pero el logotipo no debe interferir con feedback de checkout.

**Wall Shelf (`wall-shelf`).** Debe apoyarse visualmente contra pared, no flotar ni atravesarla. Su altura de 2,2 m exige una base estable, divisiones claras y productos legibles desde cámara elevada. La capacidad 24 no obliga a mostrar 24 mallas independientes en todas las situaciones; el LOD y el pooling pueden representar densidad de forma eficiente. Se comprueba que los estantes no parezcan inaccesibles para empleados o clientes.

**Central Shelf (`central-shelf`).** Su silueta debe funcionar desde cuatro lados, conservar circulación y no bloquear la vista a checkout. El volumen de `2,0 × 1,0 m` y altura 1,6 m lo convierten en pieza de ritmo espacial. Las caras deben ser suficientemente diferentes para evitar que la tienda parezca una cuadrícula de duplicados. Los endcaps o cabeceras pueden crear focos de producto.

**Low Display (`low-display`).** Debe preservar visión y permitir presentar cajas, consolas o accesorios sin que el contenido parezca colocado en el suelo. Su altura 0,9 m es útil para cámara, pero exige cuidado con el clipping de manos y personajes. La base no debe ser tan oscura que desaparezca sobre el suelo.

**Featured Display (`featured-display`).** Es un foco de lanzamiento o producto de alto interés. Debe utilizar iluminación o composición especial con moderación. La capacidad 8 no justifica una acumulación visual. El mueble debe admitir sustitución de producto mediante slots estables y no incorporar una marca concreta en la geometría base.

**Backroom Storage (`backroom-storage`).** Debe comunicar capacidad y robustez, con altura 2,4 m y footprint `2,5 × 1,0 m`. Se revisan acceso frontal, profundidad, seguridad visual, relación con cajas y rutas. No puede situarse como escaparate principal ni bloquear puertas. El fallo histórico de colocación debe documentarse con captura antes/después.

**Receiving Crate (`receiving-crate`).** Funciona como contenedor/logística y no como display. Debe diferenciarse por material cardboard, madera o metal utilitario. Se revisan apertura, orientación, volumen interior aparente y compatibilidad con animaciones de mover caja. No debe usar una tapa o asa imposible para el tamaño.

**Decorative Plant (`decoration-plant`).** Es el único elemento puramente decorativo del catálogo inicial y debe usarse con disciplina. Su función es romper repetición, aportar vida y suavizar esquinas, no llenar espacio residual. Debe conservar footprint, no bloquear navegación, utilizar material económico y evitar parecer un objeto de interacción. La vegetación estilizada debe mantener el mismo nivel de simplificación que el resto del mundo.

Para todas las familias se exige: captura aislada, captura en contexto, escala con personaje, footprint visible, revisión de pivot/frente, material compartido, LOD cuando procede, collider o volumen técnico, slots, ID, ruta canónica y prueba en build. Una familia no se aprueba por tener un prefab serializado si el runtime no la coloca, si la escena la orienta mal o si la lectura funcional es ambigua.


## 91.3. Perfiles de revisión de producto y packaging

Los seis productos iniciales deben demostrar variedad de categoría sin romper la unidad del catálogo. Los nombres funcionales actuales son canónicos; los nombres aparecidos en imágenes conceptuales son referencias de estilo y no reemplazan IDs.

**Neon Drift (`game-neon-drift`).** Debe leerse como videojuego físico mediante caja, lomo y frente, incluso cuando la carátula se reduzca. La identidad puede usar velocidad, neón y formas diagonales, pero no copiar tipografías o composiciones reconocibles. En un estante, el lomo debe distinguirse sin depender de texto minúsculo.

**Cloud Runner Case (`case-cloud-runner`).** Representa un producto físico de caja/case y puede explorar color más claro o ilustración aventurera. Debe diferenciarse de Neon Drift por silueta gráfica y paleta, no por cambiar las dimensiones físicas arbitrariamente. Su categoría debe seguir siendo comprensible en display y en inventario.

**Vertex One Console (`console-vertex-one`).** La consola debe parecer una familia tecnológica propia, con lenguaje de volúmenes simple, ventilación estilizada y uno o dos acentos. No debe aproximarse demasiado a una consola comercial existente. Packaging y dispositivo comparten identidad, pero la caja debe comunicar contenido, orientación y generación. Se revisan proporción, puertos ficticios, base y lectura desde varios ángulos.

**Orbit Pad Controller (`controller-orbit-pad`).** Debe reconocer la categoría de mando sin replicar silueta patentada. Se pueden redistribuir masas, grips, panel central y botones. El código de color debe funcionar como producto y no confundirse con feedback de UI. La caja puede mostrar una ilustración simplificada del mando, pero no una captura engañosa.

**Signal Pro Headset (`headset-signal-pro`).** Debe conservar una silueta clara en tamaño pequeño: diadema, copas, micrófono o señal equivalente. Se evitará geometría demasiado fina que falle con LOD. La presentación puede utilizar cyan o un color de proveedor, con materiales de plástico, espuma y metal controlados.

**Memory Core Accessory (`accessory-memory-core`).** Debe comunicar accesorio o memoria sin reproducir una tarjeta o dispositivo real. Su packaging es especialmente importante porque el objeto puede ser pequeño. La forma visual debe permitir variantes futuras de capacidad o rareza sin crear nuevos prefabs completos.

El packaging ficticio seguirá una plantilla modular: banda de familia, marca ficticia, nombre de producto, símbolo de categoría, bloque de información y variante cromática. Las caras laterales deben mantener lectura en estantería. El texto legal puede ser abstracto o simulado, pero no ilegible de forma que parezca un error gráfico. Los códigos de barras, ratings o sellos no deben copiar organismos reales salvo que se decida una representación genérica claramente ficticia.

Se evaluará cada producto en cuatro contextos: hero shot, caja en estantería, conjunto repetido y vista de juego lejana. La carátula no puede ser la única fuente de identidad; la silueta, el color de familia y el packaging deben sobrevivir a la reducción. Se comprobará que el material no produzca moiré, que el texto no parpadee, que los LOD conserven la masa cromática y que el catálogo pueda sustituir visuales sin alterar reglas de inventario.


## 91.4. Perfiles de personajes, diversidad y lectura de roles

Los personajes deben ser estilizados, legibles y compatibles con una simulación de tienda. La referencia conceptual actual incluye empleados, clientes y proveedores, pero los prefabs de Unity observados son metadatos sin renderer, rig, animator o collider. Por tanto, esta sección define objetivo y aceptación futura, no estado implementado.

**Empleado de tienda.** La uniformidad de VRM Games se concentra en camiseta, chaqueta, polo, delantal o accesorios negros/grafito con verde. No todos los materiales deben ser idénticos: se permiten variantes de corte, manga, calzado y pequeños detalles para evitar clonación. El rol debe leerse desde cámara media por paleta y postura. Se evitará convertir al empleado en una mascota corporativa saturada de logotipos. La animación debe cubrir idle, caminar, observar, coger producto, mover caja, checkout, satisfacción/frustración cuando corresponda.

**Cliente.** La diversidad se evalúa en silueta, edad aparente adulta, altura, peinado, ropa, accesorios y paleta. No se usarán estereotipos ofensivos ni categorías de comportamiento asociadas a rasgos físicos. La ropa no tiene que obedecer al negro/verde; al contrario, debe ampliar la gama de color de la escena. El conjunto debe funcionar como grupo sin que un cliente parezca empleado o proveedor por accidente. Las variantes deben compartir rig cuando sea razonable, pero no limitar toda la población a la misma silueta.

**Proveedor comercial.** Puede utilizar una identidad corporativa propia, como azul/blanco u otra paleta. Debe leerse como visitante profesional y no como empleado de tienda. Portafolio, tablet, acreditación o chaqueta pueden comunicar función, siempre sin marcas reales. El diseño debe admitir proveedores futuros con lenguajes diferenciados.

**Trabajador logístico.** Puede usar azul/naranja, chaleco, guantes o prendas utilitarias. Debe ser compatible con animaciones de crate y receiving. El volumen de ropa no puede interferir con cajas, puertas o pasillos. El color de seguridad no debe confundirse con warning/error de UI; se controlará la saturación y el contexto.

**Escala y proporción.** Se conservará una escala humana coherente con puertas, mostradores y estanterías. La estilización puede ampliar manos, cabeza o calzado, pero no invalidar alcance, colisión o animación. Las manos deben poder acercarse a producto y terminal sin penetraciones graves. La cabeza debe mantenerse visible bajo cámara elevada y no desaparecer contra paredes oscuras.

**Materiales y LOD.** Se priorizarán materiales compartidos y variantes por parámetros. Ojos, cabello, ropa y piel deben conservar masa cromática a distancia. Se evitarán transparencias finas y accesorios que se conviertan en ruido. Los LOD deben preservar silueta y rol, incluso si eliminan detalles faciales.

**Revisión en población.** Ningún personaje se aprueba solo en T-pose o turntable. Se probará en grupos de cuatro o más, en cola, circulación, checkout y receiving. Se observarán interpenetraciones, repetición, contraste, lectura del rol y rendimiento. La aprobación exige registro legal/procedencia, rig y clips compatibles, collider/navegación, prefab, material variants, capturas y test en build.


## 91.5. Recetas de materiales, parámetros y fallos frecuentes

Las recetas siguientes son objetivos orientativos para URP y deben ajustarse tras observar la escena real. No son valores rígidos que sustituyan profiling o revisión de iluminación.

**Grafito estructural.** Base cercana a `#222E35`, metallic moderado y smoothness medio. Se usa en marcos, bases, checkout y estructura. Debe conservar separación respecto a `Dark Surface`. Fallos frecuentes: negro aplastado, reflejo plástico excesivo, pérdida de aristas y uso indiscriminado en todos los objetos. La corrección preferida es variar valor, roughness y luz, no añadir emisión a todo.

**Superficie oscura secundaria.** Base cercana a `#3C4B55`, metallic bajo y smoothness medio. Sirve para paneles, muebles y fondos. Debe ser más clara o menos saturada que el grafito principal. Fallos: parecer gris sin intención, mezclarse con paredes o crear una tienda monocroma. Se corrige introduciendo neutrales, madera y color de producto.

**VRM Green.** Aproximadamente `#31D789`. Es acento de marca, validez y foco. No se usa como relleno mayoritario. En emisión debe reducirse intensidad y área. Fallos: contaminar iluminación, competir con feedback válido, convertir todos los proveedores en VRM o causar fatiga. La corrección es concentrarlo en líneas, señalética, interfaces del mundo y uniformes.

**Cyan.** Aproximadamente `#52C8E7`. Puede apoyar tecnología, vidrio o señal secundaria. Debe diferenciarse del verde y no crear una segunda marca dominante. Fallos: aspecto de ciencia ficción genérica, exceso de emisión o confusión con elemento interactivo.

**Warm Wood.** Aproximadamente `#996F4B`. Introduce calidez y contrasta con tecnología. La estilización puede usar gradiente simple, variación pintada o normal sutil. Fallos: veta fotorrealista, escala incoherente, naranja saturado o uso en zonas donde debería existir metal/logística.

**Cardboard.** Aproximadamente `#AD8557`. Se utiliza en cajas y recepción. Debe admitir pequeñas variaciones de tono, cinta y etiqueta ficticia. Fallos: todas las cajas idénticas, ruido de textura, suciedad excesiva o contraste insuficiente con madera.

**Light Neutral.** Aproximadamente `#DDE6E3`. Sirve para paredes, paneles y contraste. No debe quemarse bajo luz ni parecer blanco puro clínico. Fallos: pérdida de texto, bloom, exposición desigual y contraste extremo con grafito.

**Amber, rojo y azul gris.** El ámbar apoya warnings/logística, el rojo invalidez/error y el azul gris zonas técnicas o proveedores. Su uso en mundo debe coordinarse con UI para evitar señales contradictorias. Un prop rojo no significa automáticamente error; el contexto y la intensidad importan.

**Vidrio.** Debe ser claro, con tintado leve y reflejo controlado. Se revisan orden de transparencia, doble cara, sombras, profundidad y legibilidad del producto. Fallos comunes: vidrio invisible, demasiado opaco, negro por reflexión, sorting incorrecto o coste excesivo. Se preferirá una solución simple y estable a refracción sofisticada.

**Plástico y metal.** El plástico debe tener metallic cero y smoothness según acabado; el metal necesita metallic alto con roughness suficiente. Fallos: consola de plástico que parece cromada, estantería metálica que parece goma o todos los productos con el mismo brillo. Los parámetros se documentan por variante compartida.

**Texturas.** La visión histórica contempla hand-painted/cartoon, pero actualmente no hay texturas de producción. Cuando se introduzcan, deberán reforzar forma, desgaste controlado y lectura de material, no ocultar geometría deficiente. Se evitarán fotografías sin estilizar, resoluciones desproporcionadas y detalles subpíxel. Toda textura nueva requiere fuente, licencia, tamaño, canal, compresión y revisión de memoria.


## 91.6. Protocolo de iluminación y capturas de validación

La iluminación se valida como sistema de lectura, no como filtro cinematográfico. El rig debe funcionar en Editor y build, mantener color de materiales, no ocultar interacción y permanecer dentro de presupuesto.

**Capa base.** Debe ofrecer visibilidad uniforme suficiente para navegar y leer muebles. La sala de venta puede ser cálida-neutra; almacén y recepción, algo más fríos. La diferencia debe percibirse sin crear dos juegos distintos. Se revisan exposición, sombras, zonas negras, banding y clipping.

**Capa de producto.** Spotlights o light rails pueden señalar displays destacados, checkout y fachada. Deben crear jerarquía sin iluminar cada estante por separado. La distribución modular de `LightRail100/200`, `LinearPanel` y `SpotLight` debe seguir líneas arquitectónicas y evitar patrones de luz repetitivos.

**Capa de señal y feedback.** Emisión y VFX indican validez, interacción o eventos. Deben permanecer visibles bajo la luz base sin dominar. Placement valid/invalid, checkout, puerta y warning necesitan pruebas con UI superpuesta y varios fondos.

**Sombras.** Se evalúan dureza, distancia, cascadas y estabilidad. Una sombra puede mejorar profundidad, pero no debe ocultar productos ni crear flicker al mover cámara. Los muebles altos y personajes requieren atención especial. La ausencia total de sombra puede hacer que los objetos floten; el exceso puede convertir el almacén en una masa negra.

**Capturas obligatorias.** Para cada revisión de escena se producen: fachada frontal, entrada desde exterior, vista general media, vista elevada del layout, checkout y cola, display de pared, display central, producto destacado, receiving, almacén, puerta abierta/cerrada, close-up de material, captura de feedback válido/inválido, captura con clientes y captura con UI. Cada imagen registra build ID o Editor, fecha, cámara y estado.

**Prueba de exposición.** Se comparan al menos tres escenarios: exposición nominal, una variante más oscura y otra más clara o distinta resolución. No se busca soportar cualquier ajuste arbitrario, sino detectar materiales que solo funcionan en una configuración accidental.

**Prueba de daltonismo y contraste.** El estado no puede depender únicamente de verde/rojo. Icono, forma, texto o animación acompañan el color. La Art Bible coordina esta regla con UI Style Guide y Accessibility. Las capturas deben incluir ejemplos donde la diferencia siga siendo legible en escala de grises.

**Prueba en movimiento.** La iluminación se observa durante órbita, zoom y desplazamiento. Se anotan popping, reflection changes, transparencias, sombras que cruzan UI del mundo y objetos que desaparecen. Una captura estática no detecta estos fallos.

**Cierre.** El rig solo se aprueba cuando las capturas son reproducibles, el material mantiene identidad, la escena no presenta puntos muertos y el profiling de build no revela una regresión inaceptable. Los cambios posteriores en URP, AA, resolución o postprocesado reabren la revisión.


## 91.7. Protocolo de interpretación de fuentes históricas

La genealogía documental contiene decisiones válidas, sustituciones y aspiraciones. Para evitar que una frase antigua se convierta en requisito actual por accidente, toda fuente histórica se interpreta mediante cinco preguntas.

1. **¿Qué versión y fecha tiene?** Una Art Bible v0.3 repetida en baseline v0.4 no equivale a una decisión nueva. Se conserva como evidencia de continuidad.
2. **¿Describe visión, objetivo de sprint o implementación observada?** “Hand-painted textures” en v0.3 es dirección; la ausencia de texturas actuales demuestra que no es implementación. “Store 10×15 m” en v0.4/v0.5 y en datos actuales sí es decisión vigente.
3. **¿Fue sustituida explícitamente?** La tienda inicial 5×5 m fue reemplazada por 10×15 m. El recuento de 12 productos/5 muebles fue reemplazado en el set representativo por 6 productos/8 muebles, sin impedir crecimiento futuro.
4. **¿Existe evidencia en Assets, escenas o configuración?** Los prefabs, materiales y catálogos confirman parte de la dirección. La ausencia de renderer en personajes, outline shader o texturas impide declararlos hechos.
5. **¿Qué documento vigente gobierna?** En contradicción se aplica `00`, Binder, documentos especializados, producción/trazabilidad y evidencia serializada según el tipo de dato.

Los conceptos visuales generados se registran como `VISUAL TARGET`, `CONCEPT SET` o `FUTURE VISION`. No son planos exactos, no fijan IDs y no sustituyen footprints. El board Tier E guía composición, paleta y calidad; las medidas del catálogo y la escena gobiernan integración. Los nombres Byte Frontier, Wildbyte, Cloudspire u otros aparecidos en láminas son marcas conceptuales. Los IDs `game-neon-drift`, `console-vertex-one` y demás siguen siendo canónicos mientras no exista un cambio aprobado.

Cuando una fuente antigua contiene una idea valiosa no implementada, se marca `DIFERIDA` en lugar de eliminarla. Ejemplos: contorno de tinta, texturas hand-painted de producción, desgaste avanzado, mayor variedad de personajes y espacios de expansión. Para promover una idea diferida se requiere cambio, impacto, prototipo, revisión y evidencia. No se introduce silenciosamente durante una corrección de asset.

Toda nueva edición de la Art Bible incluirá una tabla de decisiones heredadas con estado `VIGENTE`, `SUSTITUIDA`, `DIFERIDA`, `HISTÓRICA` o `DESCARTADA`. Así se protege la memoria del proyecto sin obligar a mantener objetivos obsoletos.


## 91.8. Rollback y recuperación de integración artística

La integración artística debe ser reversible. El objetivo no es conservar indefinidamente dos sistemas visuales, sino evitar que un reemplazo incompleto destruya una baseline funcional.

**Antes de integrar.** Se registra commit limpio, tests, capturas, escena funcional, catálogo, rutas y build previa cuando el gate lo exige. Los assets nuevos se introducen en carpetas canónicas sin sobrescribir archivos con IDs distintos. Los GUID se preservan cuando se reemplaza contenido compatible y se cambian solo mediante decisión consciente.

**Integración por capas.** Se recomienda: materiales compartidos, prefabs aislados, catálogo, escena de prueba, StoreInitial autorada, SceneContext, runtime, desactivación del fallback. Cada paso puede validarse y revertirse. No se mezclan en un único commit importación masiva, cambio de IDs, cambio de layout, refactor de runtime y actualización de build profile.

**Señales de rollback.** Se revierte o aísla cuando aparecen referencias perdidas, escena no cargable, duplicación de shell, colliders que bloquean Golden Path, materiales magenta, degradación grave de rendimiento, catálogos inconsistentes, pérdida de save, UI que mueve al jugador o una regresión que no puede diagnosticarse dentro del work package.

**Tipos de rollback.** El rollback visual puede restaurar un prefab o material. El rollback de escena puede volver a una copia funcional. El rollback de runtime reactiva temporalmente `Store` o blockout procedural. El rollback de catálogo restaura mappings. Ninguno debe borrar evidencia del intento; el cambio se registra como fallido, parcial o revertido.

**Store frente a StoreInitial.** Mientras StoreInitial no pase gate, `Store` es fallback técnico. No se presenta como dirección visual final. Cuando StoreInitial esté validada, el fallback se retira mediante cambio explícito y build. No se mantienen ambos como destinos indefinidos, porque aumentaría deuda y ambigüedad.

**Recuperación de importación.** Si un FBX presenta normales o jerarquía incorrectas, se vuelve al archivo fuente de Blender y al proceso de exportación. No se acumulan correcciones manuales opacas en Unity sin documentar. Si el LODGroup transfer falla, se compara jerarquía de origen y prefab antes de repetir. Los colliders generados por tooling deben convertirse en evidencia real de componentes o volúmenes técnicos.

**Cierre del rollback.** Se ejecutan pruebas dirigidas, suites completas según riesgo, captura comparativa y Player.log si hubo build. El registro explica causa, alcance, archivos revertidos, deuda restante y condición de reintento. Un rollback correcto protege el avance; no se interpreta automáticamente como fracaso de la dirección artística.


## 91.9. Convenciones de evidencia y nomenclatura de capturas

La evidencia artística debe permitir reconstruir qué se evaluó. Un nombre como `final.png` no es aceptable para gate.

Formato recomendado:

```text
CC_<AREA>_<ASSET-OR-SCENE>_<VIEW>_<STATE>_<VERSION>_<DATE>_<SEQ>.<ext>
```

Ejemplos:

```text
CC_S16_StoreInitial_Overview_ManualQA_v001_20260701_01.png
CC_S16_AutomaticDoor_Front_Open_v002_20260701_02.png
CC_FUR_BackroomStorage_Medium_FailedPlacement_v001_20260701_01.png
CC_H6_StoreInitial_Checkout_Build_0.0.18_20260705_01.png
```

Campos:

- `AREA`: S16, H6, FUR, PRD, CHR, MAT, ENV u otro catálogo controlado;
- `ASSET-OR-SCENE`: ID o nombre canónico sin espacios;
- `VIEW`: Front, Back, Side, Top, Close, Medium, Far, Overview, Gameplay;
- `STATE`: Concept, Blockout, ArtReady, Integrated, Pass, Fail, Before, After;
- `VERSION`: versión del asset o build;
- `DATE`: fecha ISO compacta;
- `SEQ`: orden de captura.

Cada set de revisión incluye un `README` o registro con: objetivo, fuente, build/commit, cámara, resolución, ajustes, resultado, hallazgos y enlaces a defectos. Las imágenes conceptuales incluyen `Concept` o `VisualTarget` en nombre y metadata. Las capturas reales incluyen build ID o `Editor`.

Las comparativas before/after conservan encuadre y exposición. No se cambian cámara, FOV y luz al mismo tiempo si el objetivo es demostrar una corrección de posición. Las anotaciones pueden usar flechas o recuadros, pero se conserva también una versión limpia.

Los videos de recorrido registran duración, ruta, build y propósito. No sustituyen capturas de detalle ni logs. Un GIF comprimido no es evidencia suficiente para evaluar materiales o performance.

La evidencia se enlaza desde producción, QA y trazabilidad. Cuando un asset se sustituye, las evidencias anteriores no se borran; se marcan superseded. Para H6 se archiva un paquete inmutable con capturas representativas, build, Player.log, tests, Golden Path, known issues y decisión.


## 91.10. Registro y tratamiento de deuda artística

La deuda artística es trabajo conocido que reduce calidad, claridad, rendimiento o mantenibilidad. No debe confundirse con una preferencia estética ni esconderse dentro de “polish”.

Categorías recomendadas:

- `ART-DIR`: desviación de dirección visual;
- `ART-MAT`: material, textura o shader;
- `ART-MOD`: modelado, proporción o silueta;
- `ART-RIG`: rig, skinning o animación;
- `ART-SCN`: composición, iluminación o escena;
- `ART-TECH`: prefab, pivot, LOD, collider o importación;
- `ART-CONT`: catálogo, variante o falta de contenido;
- `ART-LEGAL`: procedencia, licencia o marca;
- `ART-PERF`: coste, memoria, draw calls o overdraw;
- `ART-ACC`: contraste, legibilidad o accesibilidad.

Cada deuda registra ID, asset/escena, síntoma, evidencia, impacto, severidad, gate, workaround, resolución y criterio de cierre. Una deuda S1 puede bloquear Sprint 16/H6 si contradice arquitectura, puerta, navegación o lectura funcional. Una deuda S3 puede diferirse si es cosmética y no afecta calidad representativa.

Ejemplos actuales: muros mal colocados (`ART-SCN`), puerta orientada incorrectamente (`ART-SCN/TECH`), backroom storage desplazado (`ART-SCN`), ausencia de texturas de producción (`ART-MAT`, diferida), personajes sin visual integrado (`ART-CONT`, alcance pendiente), colliders de prefab no demostrados (`ART-TECH`), outline no implementado (`ART-MAT`, visión histórica) y falta de build post-integración (`ART-TECH/QA`).

La deuda se prioriza por consecuencia, no por facilidad. Primero se resuelven bloqueos de recorrido, interacción, identidad o gate; después consistencia y rendimiento; finalmente refinamiento. No se invierte tiempo en microdetalle de carátulas si la puerta sigue invertida.

Una deuda aceptada necesita decisión y fecha/gate de revisión. “No tenemos tiempo” no es resolución. Si la deuda afecta marketing, se prohíbe usar capturas engañosas. Si afecta licencia, se retira el asset de distribución hasta aclaración.

El cierre exige evidencia before/after, revisión visual/técnica, test dirigido y actualización de registros. Cuando la deuda revela una regla general —por ejemplo, pivots inconsistentes— se actualiza pipeline y checklist para impedir recurrencia.


## 91.11. Antipatrones de producción artística

Los siguientes antipatrones deben detectarse durante revisión:

**Aprobar por render aislado.** Un objeto puede verse bien fuera de Unity y fallar escala, material, pivot, LOD o función. La aprobación requiere contexto y build según gate.

**Usar el concept como plano exacto.** Las láminas sirven para intención, no para sustituir footprints, navegación o catálogos. Copiar literalmente puede romper el juego.

**Uniformidad corporativa total.** Convertir empleados, clientes, proveedores, productos y arquitectura en negro/verde reduce variedad y confunde roles. La marca se concentra; el mundo se diversifica.

**Detalle prematuro.** Pintar desgaste, textos o accesorios antes de fijar escala y layout desperdicia trabajo. La secuencia correcta es función, silueta, material, detalle.

**Material por objeto.** Duplicar materiales para pequeños cambios aumenta draw calls y deuda. Se prefieren materiales compartidos, variants e instancing cuando sea compatible.

**LOD por obligación ciega.** Un objeto muy simple puede no necesitar tres LOD; uno complejo sí. La decisión se basa en coste y distancia, no en checklist ceremonial.

**Collider derivado de render sin revisión.** Un collider automático puede bloquear pasillos o permitir atravesar mostradores. Se valida con grid y recorrido.

**Marcas reales disfrazadas.** Cambiar una letra o color no evita similitud. Se diseña una identidad ficticia propia desde forma, nombre y composición.

**Emission everywhere.** La emisión indiscriminada elimina jerarquía, contamina iluminación y vuelve genérica la estética tecnológica.

**Polish que altera gameplay.** Mover un display por composición puede invalidar footprint, reserva, interacción o navegación. Todo cambio de escena se valida con sistemas.

**Arreglar en prefab lo que pertenece a fuente.** Corregir geometría repetidamente en Unity sin actualizar Blender produce divergencia y futuras regresiones.

**Ocultar deuda con cámara.** Una captura que evita la puerta incorrecta no resuelve el problema. La evidencia incluye los ángulos relevantes.

**Nombrar todo final.** Sufijos `Final`, `Final2` o `New` destruyen trazabilidad. Se usan IDs, versiones y estados.

**Acumular placeholders invisibles.** Un placeholder sin etiqueta o criterio de retirada se convierte en deuda permanente. Cada placeholder tiene owner y gate.

**Mezclar visión futura con contenido H6.** Los prefabs de publishing, studio, server o ecommerce no entran en la candidata salvo decisión explícita. Su mera existencia en Assets no amplía el alcance.


## 91.12. Paquete mínimo de aprobación artística para Sprint 16 y H6

Para cerrar la parte artística de Sprint 16 se exige un paquete coherente, no una colección dispersa de capturas.

**Identidad del paquete.** Debe incluir versión, commit, Build ID, fecha, responsable, escena, ProjectSettings relevantes y lista de known issues. La versión de aplicación y el perfil de build deben corresponder a la evidencia.

**Escena.** Se archiva `StoreInitial.unity`, `StoreInitialEnvironment.prefab` cuando exista, SceneContext, catálogos y materiales usados. Se registra que la arquitectura fija está autorada y que el runtime no la reconstruye por nombres, bounds o jerarquía.

**Capturas.** Se incluyen las vistas obligatorias de zona, comparativas de los tres defectos visuales conocidos, materiales, personajes si entran en alcance, feedback y UI. Las imágenes se clasifican como build o Editor.

**Pruebas.** Se adjuntan resultados EditMode y PlayMode posteriores a integración. La baseline previa de `1215 + 70` es referencia, no sustituto. Se añaden tests dirigidos de escena, puerta, duplicación de shell, catálogos, input UI y referencias.

**Golden Path.** Se ejecuta desde Bootstrap/MainMenu hasta StoreInitial, compra, checkout, cierre, save/load y retorno según alcance. El recorrido debe utilizar la escena representativa y no un fallback oculto.

**Build.** Se genera Windows x64 fuera del Editor, se revisa `Player.log`, se comprueba lista de escenas, versión, identificador y ausencia de errores bloqueantes. Se conserva checksum y manifest si la build es candidata.

**Revisión manual.** La decisión artística registra PASS, PASS WITH ACCEPTED DEBT o FAIL. La aprobación identifica explícitamente muros, puerta, mobiliario, iluminación, composición, productos, legibilidad y rendimiento. Los defectos S0/S1 quedan a cero para el universo del gate definido.

**H6.** Para H6, además, se exige campaña estable, profiling, accesibilidad, localización aplicable, balance y documentación actualizada. La Art Bible no otorga H6 por sí sola; aporta el componente visual y de contenido al gate multidisciplinar.

**Criterio de archivo.** El paquete se almacena de forma inmutable y se enlaza desde QA, producción y trazabilidad. Si una corrección posterior cambia escena, material, catálogo o build, se crea una revisión nueva; no se sobrescribe silenciosamente la evidencia aprobada.


# 92. Glosario artístico

| Término | Definición |
| --- | --- |
| Asset representativo | Contenido suficiente para juzgar calidad objetivo, no necesariamente final de lanzamiento. |
| Autoría explícita | Colocación intencional de arquitectura y referencias sin inferencias frágiles. |
| Color blocking | Uso de masas de color/material para separar forma y función. |
| Footprint | Celdas lógicas ocupadas por un objeto colocable. |
| Kit modular | Familia de piezas compatibles que forman espacios y muebles. |
| LOD | Versión de menor coste usada a distancia. |
| Material variant ID | Identificador estable que enlaza lógica de presentación con material reemplazable. |
| Mockup | Imagen compuesta que no representa ejecución real. |
| Outline | Contorno visual por shader, geometría, postproceso o diseño; no necesariamente global. |
| Placeholder | Representación temporal aceptada con límites. |
| Tier E | Estado inicial modesto y funcional del negocio; no sinónimo de mala calidad. |
| Trim sheet | Textura reutilizable con bandas de materiales y bordes. |
| Visual target | Referencia de intención y calidad, no plano ni prueba de gameplay. |


# 93. Historial de revisión

| Versión | Baseline | Fecha | Cambio principal |
| --- | --- | --- | --- |
| v0.3 | v0.3 | 2026-06-21 | Visión extensa: low poly, hand-painted, tinta, tienda 5 × 5 m, 5 muebles y 12 productos. |
| v0.3 | v0.4 | 2026-06-23 aprox. | Reedición sin cambio artístico sustancial. |
| v0.4 | v0.5 | 2026-06-25 | Estado técnico tras Sprint 5; tienda 10 × 15 m y pase representativo futuro. |
| v0.5 | v0.6 | 2026-07-01 | Paleta grafito/verde/cyan, 8 muebles, 6 productos, StoreInitial target. |
| 1.0 | Consolidada | 2026-07-01 | Genealogía completa, assets reales, conceptos aprobados, pipeline y gates. |


# 94. Trazabilidad de fuentes históricas y operativas

| Fuente extraída | Bytes | SHA-256 |
| --- | --- | --- |
| Documentation__00_Official_Baseline__v0.3__99_Source_Markdown__Cartridge_And_Cloud_Art_Bible_v0.3.md | 13594 | de2f2c02aef523f9e01bee94c071c44903425c32f63eb0c6b8786ee792397bb3 |
| Documentation__00_Official_Baseline__v0.3__99_Source_Markdown__Cartridge_And_Cloud_Initial_Content_Catalog_v0.1.md | 23013 | 5b416cba7975a0ee42762eabb9ea236c30a19091fb3d3031285ac67ab0a83dcb |
| Documentation__00_Official_Baseline__v0.3__99_Source_Markdown__Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.3.md | 3972 | ffc5bc6af65b27ede021acfdef2dbc5f20f680560e0be1a872586ab146a2f101 |
| Documentation__00_Official_Baseline__v0.3__99_Source_Markdown__Cartridge_And_Cloud_UI_Style_Guide_v0.3.md | 47855 | d446fa1988af7343cacee8c3817afd3e3fd10f933fe1524d7e0b65f718842065 |
| Documentation__00_Official_Baseline__v0.4__99_Source_Markdown__Cartridge_And_Cloud_Art_Bible_v0.3.md | 13699 | 9d12adda06afbf7db50291337b88095e23005722431fdb4959ffb33afc1ad0c7 |
| Documentation__00_Official_Baseline__v0.4__99_Source_Markdown__Cartridge_And_Cloud_Initial_Content_Catalog_v0.1.md | 23118 | 2bb04a31e3a2661e17e00f23ac0becc61b2c198a36fce87cf9f7aa1c76f07103 |
| Documentation__00_Official_Baseline__v0.4__99_Source_Markdown__Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.3.md | 4077 | 1a0e921f139b73179fe734eec452dcc3aa8c43860309390fe41ea1fb588fe81f |
| Documentation__00_Official_Baseline__v0.4__99_Source_Markdown__Cartridge_And_Cloud_UI_Style_Guide_v0.3.md | 47960 | 8e0d241b40ecc46860cc6c51036f436d85ecfeca0cecfc721897b52396b7fca2 |
| Documentation__00_Official_Baseline__v0.5__99_Source_Markdown__Cartridge_And_Cloud_Art_Bible_v0.4.md | 2426 | 85241caca78cbac42e40a735b73a116ee47e5937605952cebb092272bb3a1979 |
| Documentation__00_Official_Baseline__v0.5__99_Source_Markdown__Cartridge_And_Cloud_Initial_Content_Catalog_v0.2.md | 2513 | 02439c64569896a24e292ec42717612412828e784bf7c1c701fbc477d57b0d51 |
| Documentation__00_Official_Baseline__v0.5__99_Source_Markdown__Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.4.md | 2001 | 18f78b69a56edee6f8783d32859d6e231d061422d24abd6feefdbf06fe68567b |
| Documentation__00_Official_Baseline__v0.5__99_Source_Markdown__Cartridge_And_Cloud_UI_Style_Guide_v0.4.md | 2262 | 8b3346d21414eb370a5e98eb7871f5083cf7bc8d4214ab5cff7ae87ff0b46a22 |
| Documentation__00_Official_Baseline__v0.6__06_Development_Records__Governance__ADR-0035_StoreInitial_Manual_Scene_Authoring.md | 847 | 0e69d8b664239477738f3dcce96eb5cfce2ff6e3db8817261f639ac4d64092b3 |
| Documentation__00_Official_Baseline__v0.6__06_Development_Records__Historical__Sprint_16_Integration_Timeline.md | 1783 | 89fde821c460328fbb8700022883006d2eb8a1ab892c266972b7a1cc207cbcc0 |
| Documentation__00_Official_Baseline__v0.6__06_Development_Records__Sprints__Sprint_16__Documentation__StoreInitial_Authoring_Plan.md | 741 | f67d195bcda2bd1f861dcf5c53d0a299260c1f04c2f299250856a76616131eba |
| Documentation__00_Official_Baseline__v0.6__06_Development_Records__Sprints__Sprint_16__Governance__Sprint_16_Current_Status.md | 665 | bc336c8bf852d942d1b0289cebfdde9db889088990fcc6151df5db559e94d1a9 |
| Documentation__00_Official_Baseline__v0.6__06_Development_Records__Sprints__Sprint_16__Governance__Sprint_16_Representative_Asset_Integration_Record.md | 554 | 91133cf2823d732d58fcd23601d6816e610e00375cc38e39737929d88fc4ec54 |
| Documentation__00_Official_Baseline__v0.6__06_Development_Records__Sprints__Sprint_16__QA__S16_Acceptance_Matrix.md | 572 | 4ad529aceeaf7e2ea87def0e0be2f8f3998d6218f6ac11a06b40f770e66c44fd |
| Documentation__00_Official_Baseline__v0.6__06_Development_Records__Sprints__Sprint_16__QA__S16_QA_Execution_Record.md | 328 | 40c6de3168db5dcf5c71afc166c0a3a2bf34f2dc3e71344c3ccea6176d573766 |
| Documentation__00_Official_Baseline__v0.6__99_Source_Markdown__Cartridge_And_Cloud_Art_Bible_v0.5.md | 2501 | 27d4fe8de7bd166329880e6975b1bf90883cd92ca7133b667f1036b09816ca42 |
| Documentation__00_Official_Baseline__v0.6__99_Source_Markdown__Cartridge_And_Cloud_Initial_Content_Catalog_v0.3.md | 2784 | 68f8841d7f5e83b1078bbf8c110e799165b74f5d75a823839d0acfd3086b333e |
| Documentation__00_Official_Baseline__v0.6__99_Source_Markdown__Cartridge_And_Cloud_Legal_Credits_Licenses_Register_v0.5.md | 1951 | 610fc2ca1dd8ea1859fb779039e5de841610646d1e5601d316b2fe34410c3c81 |
| Documentation__00_Official_Baseline__v0.6__99_Source_Markdown__Cartridge_And_Cloud_UI_Style_Guide_v0.5.md | 2181 | 857f070207a752796ff5c21968020d3230a8c357d84a140593c74c47c59ee645 |
| Documentation__10_Development_Records__Sprint_16__Governance__Sprint_16_Two_Phase_Charter.md | 640 | b191b8b5d47654141df7860d6a79430552cc68e1bcdc786d1d4fef55583dc605 |
| Documentation__10_Development_Records__Sprint_16__Phase_1__Architecture__ADR-0072-SharedMaterialAndPrefabIds.md | 223 | f32e2b4b274522d03df3c1c08084d86114a19dbe15fa2b3008c217943e2ff11a |
| Documentation__10_Development_Records__Sprint_16__Phase_1__Governance__S16_P1_Charter.md | 874 | 4b5190df961a1180ce9fed6ced5fe194e88998f5f65df176bea9104c7bc825dc |
| Documentation__10_Development_Records__Sprint_16__Phase_1__Governance__S16_P1_Current_Status.md | 569 | 6494de60296d252da3368dab5a5850938c4a5c219d414f80bde328379f1c7f3b |
| Documentation__10_Development_Records__Sprint_16__Phase_1__QA__S16_P1_Acceptance_Matrix.md | 1130 | d6d2ca42287be0fced9c3d9da09df5db9729442157035a9a711e0ddbb9d91337 |
| Documentation__10_Development_Records__Sprint_16__Phase_1__QA__S16_P1_QA_Execution_Record.md | 443 | a044ba4e7ebb4af622c74397e2a8f0ff6e59ad7e9c1bf89ddb62f5b06bcacec7 |
| Documentation__10_Development_Records__Sprint_16__Phase_1__Traceability__S16_P1_Asset_Inventory.csv | 6002 | c91f4676f6c8b07c075fda4bf3563530380a5e35770d9f68b5497b77cf99641b |

Además se han utilizado los documentos consolidados `00–16`, `Assets.zip`, `Documentation.zip`, `Tools.zip`, `ProjectSettings.zip` y la inspección de escenas, materiales, prefabs y catálogos serializados. Las fuentes históricas se conservan como evidencia; esta Art Bible es la autoridad artística consolidada mientras no sea sustituida formalmente.


# 95. Inventario y hash de documentos consolidados

| Documento | Bytes | SHA-256 |
| --- | --- | --- |
| 00_Enfoque_y_Alcance.md | 127401 | 63dd6f0f1b9c2c80be2aec55718d18d9d031ce4acb14f677769d6e69ceaad21f |
| 01_Game_Design_Document.md | 132822 | a9cd9e3203abc5269c25c59dc7f626b0771df4ec2d65e781109ff146bceb2177 |
| 02_Vertical_Slice_Specification.md | 64531 | 75893f130116e88a08b8da5590dd81876a234581081eafaa8d08374c1a60513f |
| 03_Technical_Design_Document.md | 101994 | 66264703c5ff4959d03093690e9845a98ec0e5025e685b9a639ac8cbb616cd12 |
| 04_Modelo_de_Datos.md | 102948 | d851a72b55742c2c704cc7c002929bc5894e7142511c6af843440bb193fb19d4 |
| 05_UX_Flow.md | 56805 | e08da5ff67fb073a8b950babe325ae58ddb2e121513dc2f553acd4f2c14b867e |
| 06_Production_Roadmap_y_Sprint_Plan.md | 61564 | d00b156c0ad1c66069278119c55f76c738a577a3e2835f770208d1a5f2faadff |
| 07_QA_Testing_Plan.md | 79435 | bc9c05a3737e345546ef16e5390e034295ef35ad66c31647852aec81b4e7affe |
| 08_QA_Testing_Matrix.xlsx | 185032 | 233e57f01d2468fe13c136c8fcd3ee75b39bd3de6530349626ea98d4ff254214 |
| 09_CSharp_Coding_Standards.md | 95321 | 3b734d6cd49f31c09c10bb4941625e143f6c634e3fb50c157f0720e73b1c2a68 |
| 10_Unity_Project_Setup_Guide.md | 78408 | 31430bb0544edd9577323aa0009d6ca3df8b9fbcf6c8a150fc33df69b9f963a4 |
| 11_Build_y_Versioning_Guide.md | 85324 | 38a16a0f97505c0b405509dd3c7e3d1b50e77b89fbe3f4cb1931d3e04f602215 |
| 12_Excel_Maestro_de_Produccion.xlsx | 158100 | 405376cb64f49b34f1f842f3840c12654e9f08e2ec4534b5c149fa811e87dd83 |
| 13_Trazabilidad_y_Control_de_Cambios.xlsx | 189424 | f674e21d1a3a1f0970a3d339f26f2a2b51a19b5c257c24b9d3fa4118a5cf50eb |
| 14_Project_Binder_Indice_Maestro.md | 92828 | 0c24dcd47e0d44793e75e764e3fc2c146e5b1fea04d887d43feb0df558711559 |
| 15_Guia_Maestra.md | 119920 | 879bc1925cccf77e10df6c104302f3afc72f71e8c58dd85ae4f06e84e6f8b624 |
| 16_Auditoria_Global_de_Coherencia.md | 87439 | 4a62376b8aee623be40957ae0511d2dcab2919947f3abb2cef3502b9bb60863e |


# 96. Matriz de autoridad por decisión

| Decisión | Fuente primaria | Evidencia secundaria | Estado |
| --- | --- | --- | --- |
| 10 × 15 m / grid 20 × 30 | 02_Vertical_Slice_Specification | ContentCatalog y Store shell técnico | VIGENTE |
| StoreInitial autorada | ADR-0035 / TDD | Authoring Plan | EN IMPLEMENTACIÓN |
| 6 productos y 8 muebles | 02 + ContentCatalog | Initial Content Catalog v0.3 | VIGENTE |
| Paleta grafito/verde/cyan/madera | Art Bible v0.5 + materiales | Concept target | VIGENTE |
| Hand-painted y outline | Art Bible v0.3 / Enfoque | sin shader/texturas actuales | OBJETIVO A VALIDAR |
| Personajes estilizados y diversos | Art Bible histórica + conceptos | prefabs de rol actuales | OBJETIVO PARCIAL |
| LOD y prefabs intercambiables | ADR-0072 / assets | integración Sprint 16 | IMPLEMENTADO PARCIAL |
| Colliders representativos | VS Spec / TDD | scene colliders; prefabs sin components | DEUDA |
| Expansiones visuales | Art Bible histórica / concepts | 19 prefabs conceptuales | VISION |
| Marketing honesto | Art Bible v0.3 + legal | labels de concept art | VIGENTE |


# 97. Criterios de aceptación de esta Art Bible

La Art Bible se considera apta para uso operativo cuando:

- incorpora todas las versiones históricas conocidas;
- distingue decisiones vigentes y sustituidas;
- refleja el estado real de Assets.zip;
- no afirma que StoreInitial está terminada;
- no afirma que personajes, texturas, VFX u outline están implementados;
- define paleta, materiales, iluminación, arquitectura, mobiliario, producto y personaje;
- contiene pipeline de Blender/FBX/Unity;
- define revisión visual y técnica;
- incluye criterios de Sprint 16 y H6;
- protege marcas y procedencia;
- conserva visión futura sin convertirla en compromiso;
- puede usarse para evaluar un asset sin consultar conversaciones informales.


# 98. Próximos documentos especializados

La Art Bible debe complementarse, no crecer indefinidamente. La secuencia recomendada es:

1. `18_Audio_Bible.md`;
2. `19_UI_Style_Guide.md`;
3. `20_Economy_and_Balance_Specification.md`;
4. `21_Initial_Content_Catalog.xlsx`;
5. `22_Localization_Plan.md`;
6. `23_Legal_Credits_and_Licenses_Register.xlsx`;
7. `24_Steam_Publishing_Plan.md`;
8. `25_Marketing_Plan.md`.

La auditoría global se repetirá cuando el conjunto especializado esté completo. Mientras tanto, los hallazgos de `16_Auditoria_Global_de_Coherencia.md` se conservan como backlog, pero no bloquean la generación documental.

---

<!-- W0_S17_PHASE1_START -->

# Actualización de autoría visual W0

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

## Pivote contractual y presentación

- Mobiliario, displays, checkout y props colocables usan base-centro como referencia de suelo.
- Los assets con pivote de origen diferente se encapsulan sin alterar el archivo fuente cuando ello preserve mejor la trazabilidad.
- `GroundAnchor` debe ser visible y verificable en prefab, con orientación y escala coherentes.
- La corrección no puede desplazar composiciones autoradas ni placements cargados desde `0.0.21`.
- Wall occlusion queda desactivada para H6; la composición, iluminación y materiales deben mantener legibilidad sin depender de ocultación dinámica de paredes.

## Regla de cierre y no propagación

- El cierre de W0 no abre W1-W7 automáticamente; cada ola requiere su propia evidencia y control de cambios.
- No se declara Sprint17_Phase1 completada mientras W8 no haya ejecutado la regresión integral.
- No se propaga `PASS` de Sprint 16 a H6 ni a la Vertical Slice.
- No se introducen sistemas Post-H6 durante la remediación.
- Código, escenas, prefabs, builds y tests ejecutables no han sido modificados por esta actualización documental.

<!-- W0_S17_PHASE1_END -->
