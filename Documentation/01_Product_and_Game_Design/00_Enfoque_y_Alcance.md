---
title: "Cartridge & Cloud — Enfoque y Alcance"
author: "VRM Games / Blas Luis Rocha González"
date: "2026-07-07"
lang: "es-ES"
document_version: "1.0"
status: "Fuente vigente de enfoque y alcance"
project: "Cartridge & Cloud"
platform: "PC / Steam"
engine: "Unity 6.3 LTS 6000.3.18f1"
render_pipeline: "URP 17.3.0"
---

# Cartridge & Cloud — Enfoque y Alcance

## 0. Propósito del documento

Este documento establece la **dirección de producto**, la **fantasía del jugador**, los
**pilares de diseño**, los **límites de alcance**, la **progresión empresarial** y los
**criterios que deben gobernar cualquier decisión futura** de *Cartridge & Cloud*.

Su función es impedir que el proyecto evolucione por acumulación descontrolada de ideas,
por comodidad técnica o por imitación de otros simuladores. Toda propuesta de diseño,
arquitectura, contenido, producción, arte, interfaz, audio o monetización deberá poder
justificarse mediante este documento.

Este archivo es deliberadamente amplio. No pretende sustituir al GDD, al TDD, al modelo
de datos, al documento de UX, al roadmap, al plan de QA ni a las biblias de arte y audio.
Su objetivo es definir **qué juego se está construyendo, por qué se construye así y qué no
debe construirse todavía**.

Cuando otro documento entre en conflicto con este archivo:

1. prevalece este documento en materias de visión, fantasía, pilares y alcance;
2. el GDD podrá ampliar reglas de juego, pero no alterar los pilares sin un cambio aprobado;
3. el TDD podrá decidir cómo implementar, pero no redefinir la experiencia perseguida;
4. el roadmap podrá reordenar trabajo, pero no convertir contenido diferido en núcleo sin
   una revisión explícita de alcance;
5. los valores numéricos indicados como provisionales podrán balancearse sin modificar la
   intención del sistema;
6. cualquier cambio estructural deberá registrarse, motivarse y propagarse a los documentos
   afectados.

## 0.1. Jerarquía de autoridad utilizada para esta consolidación

Este documento se ha elaborado comparando las siguientes fuentes en este orden de autoridad:

1. `00_Official_Baseline/v0.6/00_Project_Governance/Enfoque_v0.8.md`
2. `00_Official_Baseline/v0.5/00_Project_Governance/Enfoque_v0.7.md`
3. `00_Official_Baseline/v0.4/99_Source_Markdown/Cartridge_And_Cloud_Enfoque_v0.6_PDF_Source.md`
4. `00_Official_Baseline/v0.4/00_Project_Governance/Enfoque_v0.6.md`

La versión v0.8 prevalece en dirección actual, filosofía técnica, estado y autoría explícita
del espacio. La versión v0.7 aporta el límite operativo del vertical slice y el estado de la
fundación jugable. La fuente editorial v0.6 aporta correcciones de contexto técnico. La
versión extensa v0.6 aporta la mayor parte de la visión sistémica, progresión, economía,
clientes, empleados y líneas empresariales tardías.

## 0.2. Estados de alcance empleados

| Estado | Significado |
|---|---|
| **NÚCLEO** | Capacidad necesaria para demostrar la fantasía base y cerrar el vertical slice. |
| **DIFERIDO CERCANO** | Expansión compatible con el núcleo, pero prohibida hasta estabilizarlo. |
| **VISIÓN DE MEDIO PLAZO** | Sistema empresarial posterior que reutiliza varios sistemas base. |
| **VISIÓN EMPRESARIAL TARDÍA** | Dirección estratégica de largo plazo, no compromiso inmediato. |
| **HIPÓTESIS CONFIGURABLE** | Valor o regla útil para prototipar, sujeto a balance y validación. |
| **FUERA DE ALCANCE** | Elemento que no debe entrar en la fase actual. |
| **SUSTITUIDO** | Decisión anterior que deja de ser normativa. |

## 0.3. Regla de interpretación

La existencia de una descripción detallada no implica que el sistema esté aprobado para
implementación inmediata. La documentación conserva profundidad para evitar perder la visión,
pero separa expresamente:

- lo necesario para el bucle base;
- lo diferido hasta que el bucle base sea estable;
- la visión empresarial futura;
- los valores provisionales pendientes de balance;
- las decisiones sustituidas por versiones de mayor autoridad.

---

# 1. Identidad del proyecto

## 1.1. Identificación

| Campo | Valor vigente |
|---|---|
| Proyecto | **Cartridge & Cloud** |
| Estudio / autoría | **VRM Games / Blas Luis Rocha González** |
| Plataforma inicial | **PC / Steam** |
| Motor | **Unity 6.3 LTS `6000.3.18f1`** |
| Render pipeline | **URP `17.3.0`** |
| Género principal | Simulador de gestión 3D con control directo del personaje |
| Perspectiva | Cámara elevada orbital, entre top-down e isométrica cenital |
| Modelo de interacción | Movimiento por clic, interacción física y UI contextual |
| Modelo de producción | Capas cerrables, verificables, trazables y demostrables |
| Estado del nombre | Nombre de trabajo oficial pendiente de validación comercial y legal |

## 1.2. Propuesta de valor breve

> **Cartridge & Cloud** es un simulador de gestión 3D en el que el jugador
> construye, organiza y opera físicamente una tienda de videojuegos, observa
> cómo sus decisiones afectan al espacio, al stock y a los clientes, y transforma
> gradualmente el comercio local en un ecosistema empresarial más amplio sin
> abandonar las actividades que hicieron posible su crecimiento.

## 1.3. Propuesta de valor desarrollada

El juego comienza en una escala íntima: un local, un presupuesto limitado, mobiliario
básico, mercancía por recibir y un propietario que debe realizar personalmente cada tarea.
El jugador no gobierna el negocio exclusivamente mediante hojas de cálculo o menús. Camina
por la tienda, decide dónde colocar los muebles, recibe cajas, traslada unidades, repone
expositores, abre el establecimiento, observa a los clientes, atiende la caja y comprueba
cómo la distribución física modifica la operación.

La progresión añade profundidad sin invalidar el trabajo anterior. Una tienda mejor organizada
permite manejar más productos y clientes; el conocimiento de la demanda permite ampliar
servicios; la reputación y el capital abren comercio online, publishing, desarrollo interno y
servicios digitales. Estas líneas avanzadas deberán apoyarse en datos, recursos, personal,
espacio y conocimiento procedentes de las etapas previas.

La promesa central no es “hacer números crecer”, sino **ver y comprender la transformación de
un negocio**. El espacio, los objetos, los agentes y los resultados deben comunicar esa
transformación de forma observable.

## 1.4. Fantasía central del jugador

El jugador empieza como propietario y trabajador de una pequeña tienda de videojuegos.

Durante la operación diaria debe:

- preparar el establecimiento;
- organizar mobiliario y zonas funcionales;
- recibir y almacenar mercancía;
- asignar productos a muebles compatibles;
- reponer exposición;
- fijar y revisar precios;
- abrir y cerrar la tienda;
- atender el mostrador;
- observar necesidades, recorridos y frustraciones de los clientes;
- cobrar ventas;
- resolver incidencias operativas;
- proteger la liquidez del negocio.

Fuera del horario comercial puede:

- analizar resultados;
- comprar inventario;
- reorganizar el local;
- preparar pedidos;
- ajustar catálogo y precios;
- planificar inversiones;
- decidir qué sistemas o servicios desbloquear;
- contratar y organizar personal cuando esa capa esté disponible;
- evaluar oportunidades empresariales posteriores.

La fantasía de progreso completa es:

> Empezar atendiendo personalmente una pequeña tienda de videojuegos y terminar
> dirigiendo una empresa capaz de combinar comercio físico, servicios para jugadores,
> comercio online, publishing, desarrollo propio, plataforma digital e infraestructura,
> manteniendo las etapas anteriores como partes activas del mismo ecosistema.

## 1.5. Público objetivo

El público principal está formado por:

- jugadores de simuladores de tiendas;
- jugadores de gestión y tycoon;
- personas interesadas en construir y optimizar espacios;
- jugadores que disfrutan observando agentes y cadenas logísticas;
- público atraído por la progresión empresarial a medio y largo plazo;
- jugadores que prefieren sistemas comprensibles con consecuencias visibles.

El público secundario incluye aficionados a:

- la cultura del videojuego;
- el comercio especializado;
- la personalización de establecimientos;
- la logística ligera;
- la gestión de catálogos y precios;
- experiencias tranquilas con profundidad sistémica;
- historias emergentes producidas por clientes, empleados y decisiones de negocio.

El juego no se concibe como una experiencia casual de partidas aisladas ni como una
simulación hiperrealista. Debe ser accesible en sus reglas iniciales y profundo por
acumulación de capas.

## 1.6. Duración y ritmo objetivo

Los siguientes valores son objetivos de ritmo, no promesas cerradas:

- sesión habitual: `30–90 minutos`;
- dominio del bucle inicial: varias sesiones;
- acceso aproximado al late game: `35–60 horas`;
- continuidad posterior: postgame abierto;
- velocidad de introducción de sistemas: gradual y condicionada por dominio operativo.

La duración final deberá validarse mediante telemetría, playtests y balance. Ningún sistema
debe alargarse artificialmente para cumplir una cifra de horas.

## 1.7. Tono

El tono será:

- profesional y cercano;
- comprensible sin infantilizar;
- optimista, pero no ingenuo;
- con humor ligero derivado de situaciones, clientes y productos ficticios;
- nostálgico durante las primeras etapas físicas;
- progresivamente empresarial y tecnológico sin perder humanidad.

El proyecto evitará:

- referencias dependientes de marcas reales;
- bromas que envejezcan rápidamente;
- cinismo constante;
- dramatización desproporcionada de tareas rutinarias;
- texto técnico innecesario para acciones sencillas.

---

# 2. Pilares de diseño

## 2.1. Gestión visible

Toda decisión importante debe producir una consecuencia observable en el mundo, en la UI o
en ambos:

- cajas que llegan;
- stock que cambia de ubicación;
- expositores que se vacían o se reponen;
- clientes que encuentran o no encuentran productos;
- colas que aparecen;
- dinero que entra o sale por causas identificables;
- mobiliario que mejora o empeora la circulación;
- zonas que se abren;
- servicios que ocupan espacio y consumen recursos.

Una estadística no sustituye a la representación cuando la representación es viable y aporta
comprensión.

## 2.2. Construcción funcional

La colocación no es decoración libre sin consecuencias. Debe afectar:

- capacidad;
- circulación;
- acceso;
- reposición;
- visibilidad;
- distancia de servicio;
- congestión;
- seguridad de entradas y salidas;
- rendimiento comercial.

La geometría visual no puede sustituir al modelo lógico de grid, huellas, ocupación, puntos de
interacción y conectividad.

## 2.3. Profundidad gradual

El juego comienza con una operación clara y añade complejidad por capas.

Cada nueva capa debe:

1. reutilizar sistemas existentes;
2. aportar una decisión nueva;
3. tener representación comprensible;
4. no exigir conocimiento de sistemas todavía bloqueados;
5. justificar su coste de producción;
6. poder probarse de forma aislada e integrada.

Empleados, investigación, puestos informáticos, comercio online, publishing, desarrollo
interno, plataforma digital e infraestructura permanecerán diferidos hasta que el bucle
comercial base esté cerrado y estable.

## 2.4. Claridad y control

El jugador debe comprender:

- qué acción intenta realizar;
- si es válida o inválida;
- por qué;
- cuánto cuesta;
- qué cambiará;
- qué riesgo asume;
- cómo puede corregir el resultado.

El feedback visual y sonoro debe ser inmediato y consistente. La UI y el mundo no deben
procesar el mismo clic cuando la UI lo consume. Ningún fallo de validación debe producir una
mutación parcial.

## 2.5. Autoría explícita del espacio

La arquitectura inicial aprobada es **contenido fijo autorado**.

Esto implica:

- la tienda no debe reconstruirse en runtime a partir de nombres, bounds, escalas o jerarquías
  accidentales de un FBX;
- las paredes, accesos, suelos, zonas funcionales y referencias importantes deben estar
  definidas de forma explícita;
- el runtime registra, valida y opera el entorno aprobado;
- los sistemas lógicos no deben depender de nombres visuales frágiles;
- los IDs y referencias funcionales deben permanecer estables aunque cambien los modelos;
- un placeholder puede sustituirse visualmente sin alterar la verdad lógica;
- la escena actúa como composición autorada, no como fuente implícita de reglas ocultas.

## 2.6. Progresión visible y acumulativa

Cada mejora debe transformar físicamente el negocio o ampliar su capacidad de operación.

Las nuevas etapas:

- no eliminan la tienda;
- no invalidan automáticamente los productos físicos;
- no convierten los sistemas previos en decorado;
- añaden nuevas relaciones entre catálogo, clientes, empleados, espacio y capital;
- deben mantener continuidad espacial o empresarial;
- deben permitir que el jugador reconozca el recorrido realizado.

## 2.7. Clientes observables

El mercado no existe únicamente como una curva de demanda. Los clientes:

- aparecen en el mundo;
- recorren la tienda;
- examinan productos;
- esperan;
- abandonan;
- compran;
- reaccionan;
- comunican problemas;
- producen información útil para el jugador.

Su comportamiento debe ser suficientemente expresivo para explicar resultados sin requerir
una pantalla de depuración.

## 2.8. Sistemas comprensibles

Las reglas deben poder explicarse con relaciones simples:

- el stock físico limita las ventas;
- el precio modifica el interés;
- la capacidad del mueble limita la exposición;
- la circulación condiciona el servicio;
- la cola modifica la satisfacción;
- el tiempo y los costes afectan a la rentabilidad;
- los sistemas avanzados dependen de recursos y capacidades previas;
- el componente limitante puede definir el tier de un puesto;
- una reserva lógica no equivale todavía a una venta.

La profundidad debe surgir de la interacción entre reglas claras, no de excepciones opacas.

---

# 3. Anti-pilares y límites de identidad

*Cartridge & Cloud* no debe convertirse en:

- un clon de *Game Dev Tycoon*;
- un simulador basado exclusivamente en menús;
- un juego donde la tienda sea un fondo decorativo;
- un constructor sin interacción directa;
- un simulador hiperrealista de mantenimiento;
- un juego de limpieza constante;
- un simulador técnico de reparación de ordenadores;
- un juego de supervivencia económica injusto;
- un sistema donde todo se automatiza demasiado pronto;
- un cibercafé independiente desconectado de la tienda;
- una colección de minijuegos sin relaciones sistémicas;
- un juego cuya única meta sea producir títulos AAA;
- un juego donde cada expansión sustituya o inutilice lo anterior;
- una simulación empresarial que oculte causas detrás de porcentajes incomprensibles;
- un proyecto cuya amplitud impida cerrar una experiencia jugable completa.

Cuando una propuesta acerque el proyecto a uno de estos anti-pilares, deberá rechazarse,
reducirse o reformularse.

---

# 4. Modelo de alcance

## 4.1. Núcleo actual

El núcleo debe demostrar una tienda física operable y comprensible:

1. control directo mediante clic;
2. cámara orbital y zoom;
3. arquitectura inicial autorada;
4. grid lógico, preview, rotación y ocupación;
5. mobiliario funcional;
6. catálogo de productos;
7. pedidos y recepción;
8. inventario y stock por ubicación;
9. asignación a expositores;
10. reposición;
11. precios;
12. clientes;
13. selección y reserva lógica de unidades;
14. cola;
15. caja y venta;
16. apertura y cierre;
17. ciclo diario;
18. economía;
19. persistencia;
20. feedback, UI, arte y audio representativos;
21. pruebas, aceptación, trazabilidad y build.

## 4.2. Diferido cercano

No debe entrar antes de cerrar el núcleo:

- empleados;
- investigación;
- puestos informáticos;
- reservas de clientes;
- devoluciones;
- productos usados y retro avanzados;
- robos;
- eventos de tienda;
- promociones complejas;
- ampliaciones funcionales;
- marketing avanzado;
- automatización operativa amplia.

## 4.3. Medio plazo

La primera expansión empresarial de gran alcance es:

- comercio online;
- picking y packing;
- embalaje;
- transportistas;
- capacidad logística;
- incidencias y devoluciones online;
- reputación online;
- integración del inventario físico y digital.

## 4.4. Visión tardía

Permanecen como visión, no como compromiso inmediato:

- publishing;
- desarrollo interno;
- plataforma digital;
- infraestructura y servicios online;
- mercado competitivo avanzado;
- competidores estratégicos;
- servicios tecnológicos para terceros;
- condición de victoria empresarial tardía.

## 4.5. Fuera del vertical slice

Quedan expresamente fuera:

- empleados complejos;
- investigación funcional;
- puestos informáticos;
- comercio online;
- publishing;
- desarrollo interno;
- plataforma digital;
- servidores;
- competidores;
- Steamworks;
- logros;
- Steam Cloud;
- demo pública;
- arte final;
- audio final;
- contenido masivo;
- balance definitivo.

---

# 5. Resoluciones de autoridad y decisiones sustituidas

| Materia | Decisión sustituida | Decisión vigente |
|---|---|---|
| Tienda inicial | `10 × 10` celdas, equivalentes a `5 × 5 m` | Espacio inicial representativo autorado de aproximadamente `10 × 15 m` |
| Arquitectura | Posible deducción a partir de geometría o placeholders | Autoría explícita; el runtime registra y opera, no reconstruye |
| Entrada de sistemas | Amplia definición conceptual podía interpretarse como backlog inmediato | Solo el núcleo entra antes del cierre del vertical slice |
| Escenas | Contenedores potenciales de lógica | Composición con referencias explícitas |
| Identidad funcional | Dependencia posible de nombres visuales | IDs estables independientes del nombre o modelo |
| Mutación | Validación y cambio podían mezclarse | Validación previa y mutación atómica |
| Prefabs | Posible encapsulado amplio | Unidades reutilizables; no managers globales |
| Contenido | Datos dispersos o embebidos | Catálogos y configuración mediante ScriptableObjects cuando corresponda |
| Calidad | “Funciona manualmente” | Compilación, invariantes, integración, severidades, build y trazabilidad |

Las dimensiones futuras del mapa global, las huellas de expansiones, los costes, los ritmos,
las capacidades y los porcentajes heredados se consideran hipótesis configurables hasta su
validación.

---

# 6. Mundo, espacio y control

## 6.1. Tienda inicial

La tienda inicial debe tratarse como un espacio aprobado y autorado, con una superficie
aproximada de `10 × 15 m`.

Debe contener o reservar de forma explícita:

- acceso principal;
- zona de entrada libre;
- recorrido inicial;
- mostrador y zona de atención;
- espacio de exposición;
- zona de recepción;
- almacenamiento;
- puntos funcionales necesarios para el vertical slice;
- límites de navegación;
- zonas no colocables;
- referencias de cámara y validación;
- espacio suficiente para comparar distribuciones.

La escena visual puede evolucionar, pero sus elementos funcionales no deben depender de
interpretaciones automáticas de la geometría.

## 6.2. Grid lógico

La cuadrícula lógica vigente utiliza celdas de `0,5 m`.

Reglas:

- los objetos colocables se ajustan al grid;
- las huellas se definen mediante datos;
- la rotación se realiza en incrementos de `90°`;
- no puede haber solapamiento físico no autorizado;
- entradas, salidas y puntos obligatorios deben permanecer accesibles;
- la ocupación lógica es la fuente de verdad;
- el preview debe anticipar el resultado real;
- una colocación inválida no debe modificar estado;
- retirar o mover un objeto debe liberar exactamente las celdas que ocupaba.

## 6.3. Movimiento

El jugador se desplaza mediante clic sobre una superficie transitable:

```text
Clic sobre mundo
→ raycast
→ validación de destino
→ cálculo de ruta
→ movimiento
→ llegada o fallback controlado
```

La navegación debe:

- evitar atravesar mobiliario;
- responder a cambios de ocupación;
- distinguir clic de UI y clic de gameplay;
- manejar destinos inaccesibles;
- impedir estados indefinidos;
- conservar una relación coherente entre velocidad y animación.

## 6.4. Cámara

La cámara:

- sigue al jugador;
- mantiene una perspectiva elevada;
- rota alrededor del personaje;
- ofrece zoom;
- preserva legibilidad de siluetas y mobiliario;
- no debe atravesar de forma molesta la arquitectura;
- no debe producir clics de mundo al operar UI;
- debe permitir inspeccionar pasillos, colas y puntos de interacción.

Los valores históricos de `5–18 m` de zoom se conservan como referencia inicial, no como
restricción definitiva.

## 6.5. Puertas y conexiones

Las puertas visibles deben comunicar acceso y autorización.

Soluciones previstas:

1. puerta automática deslizante para accesos principales;
2. puerta lateral automática para conexiones interiores;
3. paso técnico invisible únicamente cuando una puerta física no aporte valor.

La navegación, la detección y la animación deben permanecer desacopladas de nombres visuales
frágiles.

---

# 7. Vertical slice

## 7.1. Objetivo

Demostrar que la fantasía de construir y operar físicamente una tienda de videojuegos es
comprensible, repetible, estable y suficientemente atractiva antes de ampliar el proyecto.

Debe validar:

1. control y cámara;
2. autoría y lectura del espacio;
3. construcción funcional;
4. pedidos y recepción;
5. almacén, exposición y reposición;
6. clientes, selección, cola y cobro;
7. precios, stock, gastos y ventas;
8. apertura, cierre y progresión diaria;
9. guardado y carga;
10. UI, arte y audio representativos;
11. rendimiento, QA y build.

## 7.2. Contexto inicial

- tienda inicial autorada de aproximadamente `10 × 15 m`;
- capital estándar de referencia: `20.000 €`;
- local alquilado;
- mobiliario Tier E;
- proveedor general;
- sin empleados;
- el jugador realiza manualmente las tareas;
- catálogo reducido;
- complejidad suficiente para producir decisiones, no para simular toda la visión.

## 7.3. Contenido mínimo de referencia

| Elemento | Referencia inicial |
|---|---:|
| Escenario jugable | 1 |
| Tienda | 1 |
| Proveedor | 1 |
| Productos | 12 |
| Familias de producto | 6 |
| Arquetipos de cliente | 4 |
| Muebles funcionales | 5 |
| Días simulables | 7 o más |
| Clientes simultáneos | Hasta 8 |
| Idiomas | ES / EN |
| Slots | 3 |

Estas cifras podrán ajustarse si una validación demuestra que existe una forma más eficiente
de probar la misma fantasía sin reducir la cobertura sistémica.

## 7.4. Recorrido mínimo validable

```text
Iniciar o cargar partida
→ preparar tienda
→ pedir mercancía
→ recibir cajas
→ almacenar unidades
→ asignar productos
→ reponer exposición
→ fijar precios
→ abrir
→ recibir clientes
→ vender
→ cerrar
→ consultar resultados
→ guardar
→ cargar
→ comprobar equivalencia de estado
```

## 7.5. Criterios de aceptación

El vertical slice debe permitir completar al menos siete días y demostrar que:

- el jugador alcanza cualquier punto accesible;
- la cámara no bloquea la operación;
- la UI consume correctamente su input;
- el mobiliario no se solapa;
- no se bloquean accesos obligatorios;
- el preview coincide con la ocupación final;
- los pedidos se reciben una sola vez;
- las cantidades son correctas;
- el inventario conserva sus invariantes;
- una unidad no puede venderse dos veces;
- una reserva temporal no desaparece ni se duplica;
- el precio afecta al interés;
- los clientes pueden completar o abandonar su recorrido;
- las colas no quedan bloqueadas indefinidamente;
- cada venta produce un único movimiento económico;
- el cierre resuelve clientes y reservas con seguridad;
- el saldo cambia únicamente mediante movimientos registrados;
- el impuesto no se aplica a semanas sin beneficio positivo;
- guardar y cargar conserva el estado relevante;
- no existen excepciones recurrentes;
- no existen incidencias S0 o S1 abiertas;
- el rendimiento objetivo se sostiene en la configuración de prueba;
- el build exigido por el gate se genera y ejecuta.

## 7.6. Invariantes mínimas

### Inventario

```text
Stock total =
    recepción pendiente registrada
  + almacén
  + exposición
  + reservas temporales válidas
  + otras ubicaciones explícitas
```

Una unidad solo puede estar en una ubicación lógica a la vez.

### Venta

```text
Venta válida =
    cliente válido
  + unidad reservada
  + precio resuelto
  + caja disponible
  + confirmación atómica
```

La venta debe:

- retirar exactamente una unidad;
- registrar exactamente un ingreso;
- liberar la reserva;
- actualizar métricas;
- producir feedback;
- impedir una segunda confirmación.

### Colocación

```text
Colocación válida =
    huella dentro de límites
  + celdas libres
  + zona permitida
  + accesos preservados
  + referencias funcionales válidas
```

### Persistencia

Después de guardar, cerrar y cargar, deben conservarse como mínimo:

- dinero;
- día y hora;
- estado fiscal;
- inventario;
- ubicaciones de stock;
- precios;
- pedidos;
- mobiliario;
- ocupación;
- configuración necesaria;
- estado de progresión incluido en el slice.

## 7.7. Definition of Done del vertical slice

1. Todas las capacidades incluidas están implementadas.
2. Los criterios obligatorios están validados.
3. Se completa una partida de prueba de al menos siete días.
4. Guardado y carga conservan el estado esperado.
5. No existen S0/S1 abiertos.
6. Los defectos restantes están clasificados y documentados.
7. Las invariantes tienen cobertura.
8. La integración manual está registrada.
9. La suite completa permanece verde.
10. La documentación y trazabilidad están actualizadas.
11. El build interno exigido se genera.
12. Existe un informe de validación reproducible.

---

# 8. Filosofía técnica que protege el enfoque

La arquitectura debe servir a la experiencia, no redefinirla.

Principios vigentes:

- `Domain` y `Application` deterministas antes de `MonoBehaviour`;
- validación previa a la mutación;
- mutaciones atómicas;
- IDs estables;
- escenas como composición;
- referencias explícitas;
- `ScriptableObject` para catálogos y configuración;
- prefabs para unidades reutilizables;
- input centralizado y contextual;
- separación entre UI y gameplay;
- instaladores de Editor idempotentes cuando un cambio de escena sea complejo;
- `TestLab` para regresión aislada;
- `Store` / `StoreInitial` para integración real;
- evidencia suficiente para continuar sin conocimiento tácito.

Un sistema no debe considerarse terminado solo porque “parece funcionar” en una escena.

Debe:

- compilar limpio;
- respetar invariantes;
- fallar de forma controlada;
- exponer causas comprensibles;
- tener pruebas adecuadas;
- integrarse sin romper regresiones;
- actualizar documentación;
- producir trazabilidad;
- pasar build cuando corresponda.

---

# 9. Estado técnico de referencia en la consolidación

Este apartado es una fotografía fechada, no la parte estable de la visión.

A fecha de consolidación:

- la fundación Unity/URP y el repositorio existen;
- existen Bootstrap, MainMenu, Store y TestLab;
- existen ApplicationRoot y navegación;
- existen IDs, sesión y snapshot mínimo;
- existen contextos diferenciados de UI y gameplay;
- existe movimiento click-to-move;
- existe cámara orbital con zoom;
- existe grid lógico de `0,5 m`;
- existen preview, rotación y ocupación;
- la tienda inicial representativa es de aproximadamente `10 × 15 m`;
- existe reserva de entrada y validación de acceso;
- la construcción está integrada en Store;
- los Sprints 0–15 se consideran cerrados;
- Sprint 16 está `COMPLETED / PASS` tras aprobar la integración visual y funcional de `StoreInitial`, la build externa `0.0.21`, el Golden Path, la persistencia y la revisión de `Player.log`;
- Sprint 17 permanece `PENDING / READY TO OPEN` como gate de estabilización, balance, rendimiento, QA y build interna.

Este estado deberá migrarse a un documento específico de producción o handoff. No debe obligar
a modificar la visión cada vez que cambie un sprint.

---

# 10. Dirección visual y atmosférica

## 10.1. Identidad visual

La dirección aprobada combina:

- mundo 3D low poly;
- texturas handpainted/cartoon;
- bordes sombreados tipo tinta;
- mobiliario modular;
- iluminación interior cálida;
- productos coloridos;
- personajes estilizados;
- siluetas legibles desde cámara elevada;
- UI técnica, limpia y coherente;
- evolución visual desde comercio físico hasta tecnología digital.

## 10.2. Paleta de referencia

| Uso | Color |
|---|---|
| Fondo principal | `#0D1110` |
| Superficie | `#19201D` |
| Superficie elevada | `#242D29` |
| Verde principal | `#35D07F` |
| Verde secundario | `#1F8F5F` |
| Texto principal | `#E8EFEA` |
| Texto secundario | `#AAB7B0` |
| Advertencia | `#E8B44A` |
| Error | `#D95D5D` |

La paleta es una referencia de identidad, no una prohibición de usar otros colores en
productos, clientes, proveedores o contenido del mundo.

## 10.3. Reglas de legibilidad

- El estado funcional debe distinguirse sin depender solo del color.
- Los muebles deben leerse por silueta y uso.
- Los productos deben conservar variedad cromática.
- La iluminación no debe ocultar puntos de interacción.
- Los bordes no deben generar ruido excesivo.
- La densidad visual debe permitir reconocer pasillos y colas.
- Los elementos de VRM Games pueden usar coherencia negro/verde.
- Clientes, proveedores y marcas ficticias necesitan variedad propia.
- La UI debe distinguir información, confirmación, advertencia y error.

---

# 11. Progresión empresarial de alto nivel

La progresión es aditiva:

```text
Tienda física
→ ampliaciones y servicios
→ comercio online y logística
→ publishing
→ desarrollo interno
→ plataforma digital
→ infraestructura y servicios online
```

Cada etapa avanzada debe requerir:

- dominio operativo de la etapa anterior;
- inversión;
- reputación;
- capacidad;
- espacio o representación;
- personal cuando corresponda;
- riesgo comprensible;
- una razón jugable para seguir usando los sistemas previos.

La progresión no debe funcionar como una sucesión de pantallas independientes. El catálogo,
el conocimiento de clientes, la reputación, la logística y el capital deben conectar etapas.

---

# 12. Visión funcional detallada

Las secciones siguientes conservan la profundidad conceptual de la fuente extensa. Su
inclusión evita perder decisiones y relaciones sistémicas. El rótulo de alcance situado al
inicio de cada bloque prevalece sobre cualquier redacción histórica que pudiera interpretarse
como prioridad inmediata.

## 12.1. Bucle jugable diario

**Estado de alcance:** `NÚCLEO + ELEMENTOS DIFERIDOS`

> **Regla vigente:** La apertura, operación, cierre, resumen y avance diario pertenecen al núcleo. El director de eventos y los hitos empresariales avanzados quedan diferidos hasta que el bucle comercial base esté estabilizado.

#### Escala temporal

- Un día completo a velocidad `×1` dura una hora real.
- El jugador puede reducir la velocidad hasta `×0,5`.
- El jugador puede acelerarla hasta `×4`.
- El modificador afecta al paso del tiempo y a los temporizadores de simulación.
- La velocidad visual de movimiento del jugador, clientes y empleados no se multiplica.
- Los sistemas basados en duración deberán convertir correctamente el tiempo de simulación en tiempo real.

#### Antes de abrir

El jugador puede:

- Revisar inventario.
- Reponer estanterías y vitrinas.
- Colocar productos.
- Cambiar precios.
- Organizar mobiliario.
- Asignar tareas.
- Recibir o preparar pedidos.
- Revisar promociones.
- Comprobar el estado funcional del local.

La apertura será manual y estará disponible desde las `08:00`.

#### Durante el horario comercial

El jugador puede:

- Atender clientes.
- Cobrar compras y servicios.
- Reponer productos.
- Recibir mercancía.
- Transportar cajas.
- Organizar el almacén.
- Gestionar colas.
- Resolver bloqueos.
- Dirigir empleados.
- Supervisar servicios.
- Mantener productos disponibles.

El cierre voluntario será manual y estará disponible desde las `20:00`.

A las `22:00` se ejecutará un cierre obligatorio.

#### Resolución de clientes al cerrar

Al iniciar el cierre:

- Se bloquea el spawn de nuevos clientes.
- Las compras no pagadas se cancelan.
- Los productos reservados vuelven automáticamente al estado `Available`.
- Los clientes que solo compraban abandonan la tienda sin pagar.
- Los clientes que usan un puesto informático interrumpen el uso, liberan el puesto y pagan automáticamente el tiempo realmente consumido.
- Si un cliente tenía productos y servicio de ordenador, solo se cobra el servicio consumido; los productos se liberan.
- Las colas se disuelven.
- Todos los clientes se dirigen a la salida y ejecutan `Despawn`.

#### Después del cierre

El jugador puede:

- Revisar ingresos y ventas.
- Realizar pedidos.
- Analizar demanda.
- Comprar mobiliario.
- Preparar ampliaciones.
- Investigar mejoras.
- Gestionar empleados.
- Negociar con proveedores.
- Revisar propuestas de publicación.
- Planificar servicios.
- Trabajar en proyectos propios.

#### Resumen diario

Al pasar al día siguiente se muestra una pantalla superpuesta con:

- Ingresos por productos.
- Ingresos por servicios.
- Coste de mercancía.
- Salarios.
- Alquileres.
- Consumo eléctrico.
- Otros gastos.
- Beneficio o pérdida neta.
- Productos más vendidos.
- Servicios utilizados.
- Incidencias relevantes.

#### Condición principal de victoria y continuación

El juego utilizará un **final principal con continuación libre y varios hitos empresariales secundarios**.

La condición principal de victoria se alcanzará durante la etapa de plataforma digital al cumplir un conjunto de objetivos avanzados:

- Lanzar la plataforma.
- Alcanzar un volumen mínimo de usuarios.
- Mantener una reputación empresarial alta.
- Obtener rentabilidad durante varios periodos consecutivos.
- Disponer de la infraestructura necesaria para sostener los servicios.

Al alcanzarla se mostrará:

- Una pantalla o secuencia de cierre.
- Un resumen histórico de la empresa.
- Estadísticas acumuladas.
- Productos y proyectos más exitosos.
- Evolución de la tienda y del complejo.
- Hitos empresariales logrados.
- Una valoración o rango final.

Después, la partida continuará en modo postgame sin límite obligatorio.

Hitos secundarios previstos:

- Magnate del comercio.
- Maestro de la distribución.
- Publisher de referencia.
- Estudio reconocido.
- Líder digital.
- Proveedor tecnológico.
- Favorito de la comunidad.

Estos hitos serán acumulables y no excluirán otros estilos de juego.

#### Sistema de eventos de tienda

Se utilizará un modelo híbrido formado por:

1. **Sucesos dinámicos:** oportunidades o problemas contextuales seleccionados según la situación real de la empresa.
2. **Eventos comerciales programados:** actividades organizadas por el jugador mediante calendario, presupuesto, stock, espacio y personal.
3. **Hitos especiales:** acontecimientos únicos vinculados a la progresión.

Frecuencia objetivo:

| Tipo | Frecuencia recomendada |
|---|---:|
| Suceso menor | 2–3 por semana |
| Evento medio | 1 por semana |
| Evento mayor | 1 cada 2–4 semanas |
| Evento organizado | Máximo 1 por semana inicialmente |
| Tendencia de mercado | Una activa, durante 3–7 días |
| Hito | Una vez al cumplir su condición |

Protecciones:

- Días 1–3 sin eventos negativos aleatorios.
- Solo un evento mayor activo.
- Hasta dos sucesos menores simultáneos.
- Una única decisión urgente pendiente.
- Enfriamiento mínimo de 7 días tras un evento negativo de la misma categoría.
- Enfriamiento mínimo de 14 días tras un evento mayor.
- El mismo evento concreto no se repite durante 30 días.
- Los eventos importantes muestran coste, duración, efectos, riesgo y plazo antes de confirmar.
- Ningún suceso aleatorio podrá provocar una bancarrota inevitable a una empresa saludable.

Categorías previstas:

- Mercado y tendencias.
- Proveedores y logística.
- Clientes y comunidad.
- Operaciones de tienda.
- Nostalgia y cultura del videojuego.
- Oportunidades estratégicas avanzadas.

El director de eventos tendrá en cuenta etapa, reputación, dinero, stock, capacidad, empleados, historial y sistemas desbloqueados. No se generarán eventos incompatibles con el estado de la partida.

#### Bucle resumido

```text
Preparar tienda
→ abrir manualmente desde las 08:00
→ atender clientes
→ vender productos y servicios
→ cerrar manualmente desde las 20:00
→ cierre obligatorio a las 22:00
→ resolver clientes restantes
→ mostrar resumen diario
→ analizar resultados
→ comprar y planificar
→ iniciar el día siguiente
```

## 12.2. Tienda física, productos y economía

**Estado de alcance:** `NÚCLEO + EXTENSIONES DIFERIDAS`

> **Regla vigente:** Pedidos, recepción, inventario, exposición, reposición, precios, ventas y economía básica forman parte del núcleo. Reservas, devoluciones, retro avanzado, robos, promociones complejas y parte del catálogo quedan fuera del primer vertical slice.

#### Familias iniciales de productos

El catálogo inicial podrá incluir:

- Videojuegos físicos.
- Consolas.
- Ordenadores.
- Mandos.
- Teclados.
- Ratones.
- Auriculares.
- Monitores.
- Componentes.
- Merchandising.
- Tarjetas regalo.
- Accesorios.
- Juegos retro.
- Ediciones especiales.

Se incorporarán cuando correspondan:

- Productos publicados por la propia empresa.
- Componentes usados retirados de puestos informáticos.

Los productos concretos de cada familia se definirán como datos editables.

#### Proveedores provisionales

| Proveedor | Disponibilidad | Entrega | Coste orientativo respecto al precio recomendado |
|---|---|---:|---:|
| Distribuidor generalista | Inicio | 1 día | 70–85 % |
| Especialista en hardware | Nivel 2 | 2 días | 65–80 % |
| Mayorista de accesorios | Nivel 2 | 1–2 días | 55–70 % |
| Proveedor retro | Investigación | 2–4 días | 40–70 % |
| Distribuidor de coleccionismo | Reputación media | 3–5 días | 50–75 % |

Cada proveedor podrá tener catálogo, stock, pedido mínimo y fiabilidad propios.

#### Gestión de productos

El jugador podrá:

- Comprar a proveedores.
- Recibir mercancía.
- Trasladarla al almacén.
- Colocarla en muebles compatibles.
- Definir precios.
- Crear promociones.
- Reponer unidades.
- Retirar productos.
- Vender sobrantes.
- Vender componentes usados.
- Gestionar reservas.
- Preparar pedidos online en fases posteriores.

#### Sistema provisional de precios

Cada producto tendrá:

- Coste de compra.
- Precio recomendado.
- Precio establecido por el jugador.
- Margen.
- Atractivo.
- Sensibilidad al precio.

El precio podrá ajustarse inicialmente entre el `50 %` y el `150 %` del recomendado.

Regla provisional:

- Cada `1 %` por encima del precio recomendado reduce el interés en `1` punto.
- El descuento mejora el interés hasta un máximo equivalente al `15 %`.
- Los márgenes y penalizaciones serán editables.

#### Promociones

El jugador podrá activar descuentos del:

- `5 %`.
- `10 %`.
- `15 %`.
- `20 %`.

Duración inicial: entre `1` y `7` días.

Las promociones aumentarán el interés y la rotación, pero reducirán el margen.

#### Capacidad inicial por tier

| Tier | Estantería `4 × 2` | Vitrina `2 × 2` |
|---|---:|---:|
| E | 16 | 8 |
| D | 20 | 10 |
| C | 24 | 12 |
| B | 28 | 16 |
| A | 32 | 20 |
| S | 40 | 24 |

Los productos grandes podrán consumir más de una unidad de exposición mediante `displaySize`.

#### Representación del stock visible

Cada mueble podrá intercambiar su modelo o conjunto visual según el porcentaje de ocupación:

- `0 %`: vacío.
- `1–25 %`: casi vacío.
- `26–50 %`: medio-bajo.
- `51–75 %`: medio-alto.
- `76–100 %`: lleno.

El stock lógico será la fuente de verdad.

#### Reservas

Las reservas se desbloquearán mediante investigación:

1. Durante la noche llega una notificación con icono de correo.
2. El informe indica productos, cantidades, cliente, fecha y hora.
3. El jugador prepara el paquete.
4. El cliente paga al recogerlo.
5. Reserva preparada: `+15` de satisfacción.
6. Reserva no preparada: `-25` de satisfacción y `-2` de reputación.
7. La reserva fallida se cancela sin cobro.

#### Devoluciones

Sistema base provisional:

- Plazo: `3` días de juego.
- Solo productos no consumibles.
- Reembolso completo.
- El producto devuelto pasa a estado usado.
- Su precio base de reventa será el `50 %` del producto nuevo.
- La frecuencia y las causas se balancearán posteriormente.

#### Productos retro

- Se desbloquean mediante investigación.
- Proceden principalmente del proveedor retro.
- Se consideran usados.
- Pueden tener rareza y precio recomendado superiores.
- No requerirán reparación ni mantenimiento.
- Utilizarán la misma capa visual de desgaste que otros artículos usados.

#### Robos

- No formarán parte del vertical slice inicial.
- Se activarán posteriormente como sistema opcional.
- Probabilidad base provisional: `0,5 %` por cliente expuesto a un producto no vigilado.
- La vigilancia, el mostrador, los empleados y futuros sistemas de seguridad reducirán el riesgo.
- El robo retira stock y genera una incidencia en el resumen diario.

#### Inventario del cliente

```text
CustomerPurchaseList
├── Producto
├── Cantidad
├── Precio
└── Estado de reserva
```

El cliente:

1. Se aproxima al mueble.
2. Ejecuta una animación de examinar o coger.
3. Reserva lógicamente la unidad.
4. La añade a su lista.
5. Continúa recorriendo la tienda.
6. Paga en el mostrador.

La unidad solo se retira definitivamente del stock al completar el pago.

#### Economía inicial aprobada

##### Capital inicial

La dificultad Estándar comenzará con:

```text
Capital inicial: 20.000 €
Local inicial: alquilado y vacío
Empleados: ninguno
Préstamos: ninguno
Puestos informáticos: ninguno
```

La inversión esperada de apertura será de `13.000–15.000 €`, dejando aproximadamente `5.000–7.000 €` de tesorería.

##### Alquileres y costes fijos

Modelo híbrido aprobado:

- La tienda inicial paga `150 €/día` de alquiler.
- Cada ampliación exige un coste único de construcción.
- Cada ampliación añade un coste fijo diario propio.
- Electricidad, salarios, mercancía y transporte se calculan por separado.
- Todos los costes aparecen desglosados en el resumen diario.

Costes iniciales:

| Concepto | Coste estándar |
|---|---:|
| Alquiler de tienda | 150 €/día |
| Servicios básicos | 25 €/día |
| Seguro y licencias | 15 €/día |
| Electricidad inicial | 10–25 €/día |
| Total fijo esperado | 200–215 €/día |

##### Impuestos y gastos periódicos

- Impuesto ficticio y simplificado del `10 %` sobre el beneficio operativo positivo.
- Liquidación cada `7 días`.
- Las semanas con resultado cero o negativo no pagan impuestos.
- El resumen diario muestra una provisión estimada.
- Transporte normal: `50–100 €` por pedido.
- Entrega urgente: `200 €`.
- Mantenimiento semanal del complejo: `0,1 %` del valor construido.
- Seguro, licencias y servicios básicos no se cobran dos veces porque ya están incluidos en los costes diarios.

##### Márgenes objetivo

| Familia | Margen objetivo |
|---|---:|
| Consolas | 20 % |
| Ordenadores | 21–22 % |
| Videojuegos estándar | 30 % |
| Videojuegos premium | 30–31 % |
| Mandos | 30 % |
| Monitores | 29–30 % |
| Componentes | 30 % |
| Teclados | 33 % |
| Ediciones especiales | 33 % |
| Ratones | 40 % |
| Auriculares | 40 % |
| Productos retro | 43 % |
| Accesorios | 47 % |
| Productos usados | 45–50 % |
| Merchandising | 52 % |
| Tarjetas regalo | 8 % |

El margen bruto medio objetivo de la tienda será del `35 %`.

Con costes fijos iniciales cercanos a `210 €/día`, el punto de equilibrio aproximado será de `600 €` diarios en ventas. El objetivo saludable inicial será de `800–1.200 €` diarios.

##### Dificultades económicas

| Parámetro | Relajada | Estándar | Exigente |
|---|---:|---:|---:|
| Capital inicial | 28.000 € | 20.000 € | 16.000 € |
| Demanda | ×1,15 | ×1,00 | ×0,90 |
| Coste de mercancía | ×0,95 | ×1,00 | ×1,05 |
| Gastos fijos | ×0,85 | ×1,00 | ×1,15 |
| Impuestos | 5 % | 10 % | 15 % |
| Sensibilidad al precio | ×0,85 | ×1,00 | ×1,15 |
| Coste de investigación | ×0,85 | ×1,00 | ×1,15 |
| Tiempo de investigación | ×0,85 | ×1,00 | ×1,10 |
| Pérdida de reputación | ×0,75 | ×1,00 | ×1,25 |

Además existirá un modo Sandbox personalizable. La dificultad normal se elige al crear la partida y no se modifica posteriormente; las opciones de accesibilidad permanecen independientes.

## 12.3. Clientes y comportamiento de compra

**Estado de alcance:** `NÚCLEO`

> **Regla vigente:** El vertical slice debe demostrar clientes observables que buscan productos, reservan unidades lógicas, forman cola, pagan o abandonan la tienda con una causa comprensible.

#### Comportamiento general

Los clientes aparecen con una intención o combinación de intenciones:

- Solo comprar.
- Solo utilizar un puesto informático.
- Comprar y utilizar un puesto.
- Buscar un producto concreto.
- Explorar.
- Recoger una reserva.
- Realizar otras acciones futuras.

#### Estados básicos

```text
Spawn
Buscar servicio
Moverse por la tienda
Seleccionar productos
Buscar puesto informático
Ir al ordenador
Sentarse
Usar ordenador
Levantarse
Esperar disponibilidad
Ir al mostrador
Esperar en cola
Pagar
Salir
Alejarse de la tienda
Despawn
```

#### Modelo de perfiles aprobado: híbrido

Cada cliente recibe:

1. Un arquetipo base.
2. Valores aleatorios dentro de los rangos del arquetipo.
3. Una intención concreta.
4. Uno o dos modificadores menores.

Arquetipos iniciales:

- Jugador casual.
- Entusiasta.
- Coleccionista.
- Comprador económico.
- Cliente tecnológico.
- Aficionado retro.
- Comprador de regalos.

Ejemplo:

```text
Arquetipo: Coleccionista
Presupuesto: alto
Preferencia: ediciones especiales
Modificador: poca paciencia
Intención: buscar lanzamiento concreto
```

#### Atributos base

- Presupuesto.
- Tiempo máximo de espera.
- Preferencias.
- Intención de compra.
- Intención de usar servicios.
- Tiempo de uso del ordenador.
- Tolerancia a precios.
- Paciencia en cola.
- Satisfacción.

#### Fórmula de interés aprobada

```text
Interés =
    Afinidad con categoría
  + Afinidad con plataforma
  + Necesidad actual
  + Atractivo del producto
  + Efecto de promoción
  - Penalización por precio
  - Penalización por falta de stock
```

El cliente compra cuando el interés supera su umbral personal y dispone de presupuesto.

#### Satisfacción

La satisfacción comienza en `50/100`.

| Evento | Cambio provisional |
|---|---:|
| Encuentra el producto buscado | +10 |
| Precio por debajo de su expectativa | +5 |
| Puesto informático disponible | +5 |
| Espera breve en caja | -2 |
| Espera prolongada | -10 |
| Producto buscado sin stock | -15 |
| Puesto informático no disponible | -10 |
| Reserva preparada | +15 |
| Reserva no preparada | -25 |
| Cierre antes de completar la compra | -20 |

#### Consecuencias aprobadas

Al abandonar la tienda:

- Se muestra temporalmente un bocadillo visual con una reacción breve.
- La reacción utiliza claves localizables.
- La satisfacción modifica la reputación.

Fórmula provisional:

```text
CambioReputación = Clamp(
    Redondear((Satisfacción - 50) / 25),
    -2,
    +2
)
```

Ejemplos de bocadillos:

- “¡He encontrado justo lo que buscaba!”
- “Los precios son demasiado altos.”
- “No quedaban puestos libres.”
- “Mi reserva estaba preparada.”
- “He esperado demasiado.”

## 12.4. Servicio de puestos informáticos

**Estado de alcance:** `DIFERIDO CERCANO`

> **Regla vigente:** Es una expansión funcional de la tienda, pero no debe entrar antes de cerrar el bucle de venta física, inventario, clientes, caja, día, economía y persistencia.

#### Naturaleza del sistema

Será un servicio adicional integrado en la tienda, no un simulador completo de cibercafé.

No habrá:

- Mantenimiento periódico.
- Averías aleatorias.
- Limpieza obligatoria.
- Instalación individual de juegos.
- Gestión de licencias.
- Temperatura.
- Actualizaciones automáticas de hardware.
- Gestión detallada de la actividad del cliente.

#### Consumo eléctrico

```text
CosteDiario =
Σ(
    PotenciaActivaTier × HorasUso
  + PotenciaEsperaTier × HorasAbiertoSinUso
) × PrecioKWh
```

| Tier | Potencia activa | Potencia en espera |
|---|---:|---:|
| E | 0,10 kW | 0,02 kW |
| D | 0,15 kW | 0,025 kW |
| C | 0,22 kW | 0,03 kW |
| B | 0,30 kW | 0,04 kW |
| A | 0,42 kW | 0,05 kW |
| S | 0,60 kW | 0,07 kW |

Precio inicial editable: `0,25 €/kWh`.

#### Retirada, reutilización y venta

Los componentes no se degradan funcionalmente.

Un componente retirado podrá:

- Guardarse.
- Reutilizarse.
- Venderse como usado.

Los productos usados:

- Tendrán `isUsed = true`.
- Mostrarán el sufijo localizado “Used/Usado”.
- Tendrán un precio base del `50 %` del producto nuevo.
- Recibirán una capa adicional de material con suciedad, arañazos o desgaste.
- La intensidad del material podrá configurarse por prefab o categoría.

La capa de desgaste no sustituirá el material original; se combinará mediante shader, decal o material superpuesto según la solución artística elegida.

## 12.5. Desbloqueo de puestos informáticos

**Estado de alcance:** `DIFERIDO CERCANO`

> **Regla vigente:** Conserva la dirección conceptual; sus costes, requisitos y ritmo deberán revalidarse.

La primera tienda no permite instalar puestos.

#### Posición en investigación

El desbloqueo estará en la rama **Servicios al cliente**:

```text
Servicios al cliente I
→ Ampliación de tienda I
→ Servicio de puestos informáticos
→ Puestos premium
```

#### Requisitos iniciales

- `40` puntos de investigación.
- Nivel empresarial `2`.
- Reputación mínima `100`.
- Coste de investigación de `2.000 €`.
- Duración de `2` días.

#### Ampliación

- Coste inicial: `12.000 €`.
- Duración: `3` días.
- Zona provisional: `12 × 10` celdas al este de la tienda.

#### Flujo

```text
Tienda inicial
→ alcanzar nivel y reputación
→ desbloquear nodos previos
→ investigar servicio
→ construir ampliación
→ comprar componentes
→ instalar puestos
```

## 12.6. Componentes de los puestos

**Estado de alcance:** `DIFERIDO CERCANO`

> **Regla vigente:** El modelo debe continuar definido por datos y con reglas comprensibles.

Cada puesto requiere:

- Mesa.
- Silla.
- Ordenador.
- Monitor.
- Teclado.
- Ratón.

No existe un periférico adicional obligatorio.

#### Definición mediante datos

Un `ScriptableObject` de setup declarará:

- Slots obligatorios.
- Tipo aceptado en cada slot.
- Posición local fija.
- Rotación local fija.
- Huella total.
- Punto de acceso del cliente.
- Punto de asiento.
- Estado de validez.

El puesto estará completo cuando todos los slots obligatorios contengan un elemento válido.

La apariencia final será la composición visual de los modelos individuales.

#### Condiciones operativas

- Todos los slots completos.
- Silla en su posición.
- Acceso transitable.
- Zona compatible.
- No reservado.
- No ocupado.

## 12.7. Tiers de componentes

**Estado de alcance:** `DIFERIDO CERCANO`

> **Regla vigente:** La regla del componente limitante se conserva como principio de legibilidad.

Cada componente tiene un tier de calidad.

Orden de menor a mayor:

```text
E → D → C → B → A → S
```

El tier global del puesto se define por el componente de menor tier.

#### Fórmula

```text
TierPuesto = mínimo(
    TierMesa,
    TierSilla,
    TierOrdenador,
    TierMonitor,
    TierTeclado,
    TierRatón
)
```

#### Ejemplo A

```text
Mesa: B
Silla: B
Ordenador: B
Monitor: S
Teclado: B
Ratón: B

Tier final: B
```

#### Ejemplo B

```text
Mesa: B
Silla: D
Ordenador: B
Monitor: S
Teclado: A
Ratón: B

Tier final: D
```

El componente de peor calidad limita todo el setup.

#### Objetivo del sistema

- Hacer que las mejoras sean visibles.
- Evitar configuraciones desequilibradas.
- Crear progresión mediante sustitución de componentes.
- Dar valor a todo el mobiliario.
- Simplificar el cálculo de calidad.

## 12.8. Tarifa y duración de uso

**Estado de alcance:** `DIFERIDO CERCANO`

> **Regla vigente:** Las fórmulas se conservan como hipótesis de diseño, no como balance cerrado.

Cada tier tiene una tarifa automática por hora.

| Tier | Tarifa provisional |
|---|---:|
| E | 2,00 €/h |
| D | 3,00 €/h |
| C | 4,50 €/h |
| B | 6,00 €/h |
| A | 8,50 €/h |
| S | 12,00 €/h |

#### Tiempo de uso

Valores iniciales:

- `X = 15` minutos.
- `Y = 90` minutos.
- Resolución: múltiplos de `5` minutos.

Para favorecer duraciones intermedias se utilizará una distribución triangular:

```text
T = RedondearAMúltiplosDe5(
    X + ((Random01 + Random01) / 2) × (Y - X)
)
```

#### Cobro prorrateado

```text
Pago = TarifaPorHora × TiempoUtilizadoEnMinutos / 60
```

El resultado se redondeará a dos decimales.

## 12.9. Uso de los puestos por clientes

**Estado de alcance:** `DIFERIDO CERCANO`

> **Regla vigente:** La integración no debe comprometer navegación, colas, cierre ni reservas de inventario.

#### Flujo

```text
Spawn
→ buscar puesto
→ reservarlo
→ calcular ruta
→ caminar
→ sentarse
→ usarlo durante T
→ levantarse
→ liberar puesto
→ continuar comprando o ir a caja
→ pagar
→ salir
→ despawn
```

El puesto vuelve a `Available` al levantarse el cliente. El importe del servicio queda asociado al cliente hasta el pago.

#### Estados del puesto

```text
Invalid
Available
Reserved
Occupied
```

#### Camino bloqueado

- El cliente espera un segundo de simulación.
- Vuelve a solicitar una ruta.
- Cada fallo consume paciencia.
- Si agota la paciencia, abandona el objetivo.
- Si tiene productos reservados, va a caja.
- Si no tiene productos ni servicios pendientes, sale.

#### Reserva de productos

- La unidad permanece físicamente en el mueble.
- Su estado lógico pasa a `Reserved`.
- No puede seleccionarla otro cliente.
- Solo se descuenta del stock al pagar.
- Si el cliente renuncia o la tienda cierra, vuelve a `Available`.

#### Cierre de tienda

Al cerrar:

- El puesto se libera inmediatamente.
- El tiempo utilizado se calcula hasta el instante de cierre.
- El servicio se cobra automáticamente.
- Los productos reservados se liberan sin cobrarse.
- El cliente se dirige a la salida.
- La satisfacción recibe la penalización de cierre anticipado.

## 12.10. Cliente orientado exclusivamente al servicio

**Estado de alcance:** `DIFERIDO CERCANO`

> **Regla vigente:** Se conserva como variante futura del comportamiento de clientes.

#### Puesto disponible

```text
Spawn
→ reservar puesto
→ ir al ordenador
→ sentarse
→ usar durante T
→ levantarse
→ ir a caja
→ pagar
→ salir
→ despawn
```

#### Puesto no disponible

El cliente alterna entre:

- Caminar por zonas transitables de la tienda.
- Permanecer detenido brevemente.
- Reintentar cada segundo.

Cada intento fallido consume paciencia.

Al agotarla:

```text
Sin productos reservados:
    salir sin pagar

Con productos reservados por otro objetivo:
    ir a caja
    pagar productos
    salir
```

La falta de disponibilidad de puestos o productos reduce la satisfacción.

## 12.11. Cliente mixto: compra y servicio

**Estado de alcance:** `DIFERIDO CERCANO`

> **Regla vigente:** Debe reutilizar el núcleo de cliente, cola, pago y satisfacción.

El comportamiento previsto es:

1. Busca un puesto informático.
2. Si encuentra uno, lo utiliza.
3. Después recorre la tienda y selecciona productos.
4. Finalmente paga todo.

Si no hay puesto disponible:

1. Recorre la tienda.
2. Selecciona productos.
3. Vuelve a buscar un puesto.
4. Si encuentra uno, lo utiliza.
5. Si no encuentra ninguno, espera.
6. Si pierde la paciencia, paga solo los productos.
7. Abandona la tienda.

#### Flujo resumido

```text
Spawn
→ buscar puesto

Disponible:
    usar puesto
    → comprar
    → pagar servicio + productos
    → salir
    → despawn

No disponible:
    comprar
    → volver a buscar puesto

    Disponible:
        usar puesto
        → pagar servicio + productos
        → salir
        → despawn

    No disponible:
        esperar
        → perder paciencia
        → pagar solo productos
        → salir
        → despawn
```

## 12.12. Empleados

**Estado de alcance:** `DIFERIDO CERCANO`

> **Regla vigente:** Los empleados no deben ocultar ni automatizar prematuramente el bucle manual. Su incorporación exige que las tareas del jugador ya sean claras, estables y medibles.

La contratación se desbloqueará durante la **Etapa 2 — Tienda ampliada**.

Requisitos provisionales:

- Nivel empresarial `2`.
- Reputación mínima `50`.
- Al menos `7` días de actividad.
- Mostrador o área de trabajo válida.

#### 16.1. Tareas previstas

- Atender el mostrador y cobrar.
- Reponer estanterías y vitrinas.
- Transportar cajas.
- Recibir mercancía.
- Organizar el almacén.
- Preparar reservas y pedidos online.
- Recoger productos.
- Realizar tareas administrativas.

#### 16.2. Contratación y candidatos

Flujo:

```text
Abrir panel de contratación
→ elegir perfil
→ elegir canal
→ publicar oferta
→ esperar candidaturas
→ comparar
→ contratar o descartar
→ incorporación al día siguiente a las 08:00
```

Perfiles iniciales:

- Dependiente.
- Reponedor.
- Preparador de pedidos.
- Generalista.

Categorías profesionales:

| Categoría | Habilidades habituales | Salario orientativo |
|---|---|---:|
| Junior | Principal 1–3, resto 1–2 | 90–125 €/día |
| Cualificado | Principal 3–4, resto 1–3 | 125–170 €/día |
| Experimentado | Principal 4–5, resto 2–4 | 170–230 €/día |
| Especialista | Principal 5, secundaria 3–5 | 230–320 €/día |

Canales:

| Canal | Coste | Candidatos | Tiempo | Enfoque |
|---|---:|---:|---:|---|
| Tablón local | 0 € | 3 | 2 días | Principalmente junior |
| Portal especializado | 250 € | 4 | 1 día | Junior y cualificado |
| Agencia de selección | 750 € | 5 | 1 día | Cualificado y experimentado |
| Búsqueda ejecutiva | 2.500 € | 3 | 2–3 días | Experimentado y especialista |

Los candidatos mostrarán nombre, perfil, categoría, habilidades, velocidad, experiencia, salario solicitado, rasgo y disponibilidad. No habrá estadísticas ocultas ni entrevistas jugables en la primera implementación.

Cada grupo permanecerá disponible durante `5 días`. La primera publicación será gratuita y garantizará al menos un generalista junior con salario de `100–125 €/día`.

#### 16.3. Progresión profesional

Cada empleado tendrá:

- Nivel general `1–10`.
- Habilidades de Dependiente, Reposición y Preparación de pedidos en escala `1–5`.
- XP general.
- XP independiente por habilidad.
- Un punto de desarrollo por nivel.

Categorías por nivel:

| Nivel | Categoría |
|---:|---|
| 1–2 | Junior |
| 3–4 | Cualificado |
| 5–7 | Experimentado |
| 8–10 | Especialista |

XP acumulada objetivo:

| Nivel | XP acumulada |
|---:|---:|
| 1 | 0 |
| 2 | 100 |
| 3 | 250 |
| 4 | 450 |
| 5 | 700 |
| 6 | 1.000 |
| 7 | 1.400 |
| 8 | 1.900 |
| 9 | 2.500 |
| 10 | 3.200 |

XP por habilidad:

| Mejora | XP requerida |
|---|---:|
| 1 → 2 | 100 |
| 2 → 3 | 250 |
| 3 → 4 | 500 |
| 4 → 5 | 900 |

La mejora exige uso real de la habilidad y un punto de desarrollo. La XP normal de trabajo tendrá un límite de `40` por día para evitar explotación.

Especializaciones:

- Nivel 4: Atención al cliente, Logística, Pedidos o Generalista.
- Nivel 8: especialización avanzada o secundaria.

Formación:

| Curso | Coste | Duración | XP general | XP de habilidad |
|---|---:|---:|---:|---:|
| Básico | 500 € | 1 día | 25 | 50 |
| Especializado | 1.500 € | 2 días | 50 | 150 |
| Avanzado | 4.000 € | 3 días | 100 | 300 |

Durante la formación el empleado no trabaja y sigue cobrando.

#### 16.4. Cansancio y descansos

- Escala de cansancio: `0–100`.
- Generación base: `6` puntos por hora, modificada por intensidad.
- Desde `70`: `−10 %` a velocidad y eficiencia.
- Desde `90`: `−25 %`.
- A partir de `80`, el empleado deja de aceptar tareas nuevas y busca descanso cuando pueda terminar con seguridad su acción actual.

Descansos:

| Tipo | Duración | Recuperación |
|---|---:|---:|
| Pausa rápida | 5 minutos | 8 puntos |
| Descanso corto | 15 minutos | 25 puntos |
| Descanso completo | 30 minutos | 50 puntos |

Las zonas de descanso aplicarán multiplicadores de recuperación entre `×0,80` y `×1,30` según calidad. Los descansos forman parte del turno y se pagan normalmente.

Al cambiar de día:

```text
Cansancio siguiente = máximo(0, cansancio actual - 80)
```

Un día libre elimina todo el cansancio.

#### 16.5. Salarios y renegociación

Los salarios no subirán automáticamente. El empleado solicitará revisión en los niveles `3`, `5`, `8` y `10`, o cuando quede claramente por debajo de su valor de mercado.

Incrementos habituales:

| Hito | Subida solicitada |
|---:|---:|
| Nivel 3 | 8–12 % |
| Nivel 5 | 10–15 % |
| Nivel 8 | 12–18 % |
| Nivel 10 | 15–20 % |

Reglas:

- Intervalo mínimo entre revisiones: `14 días`.
- Petición activa: `5 días`.
- Aumento máximo por revisión: `25 %`.
- Cantidades redondeadas a múltiplos de `5 €`.

Opciones del jugador:

- Aceptar.
- Presentar contraoferta determinista dentro del rango válido.
- Pagar un bonus equivalente a `3 salarios diarios` y aplazar `7 días`.
- Aplazar una vez `3 días` sin bonus.
- Rechazar.

Estados salariales: Satisfecho, Conforme, Disconforme y Crítico. En estado crítico existirá un último plazo de `3 días` antes de una posible dimisión. No se permitirán reducciones salariales unilaterales.

#### 16.6. Despidos y reputación laboral

Modalidades iniciales:

| Modalidad | Condición | Compensación | Consecuencia |
|---|---|---:|---|
| Periodo de prueba | Primeros 3 días trabajados | 1 salario diario | Sin penalización |
| Despido planificado | 1 día de aviso | 3 salarios diarios | Sin penalización |
| Despido inmediato | Salida tras asegurar la tarea | 5 salarios diarios | −3 reputación laboral |

La reputación laboral utilizará escala `0–100`, con valor inicial `50`.

| Reputación | Consecuencia |
|---:|---|
| 70–100 | Los canales de pago pueden ofrecer un candidato adicional |
| 40–69 | Sin modificadores |
| 20–39 | Salarios solicitados +5 % |
| 0–19 | Salarios +10 % y menos especialistas |

Las liquidaciones impagadas se convierten en obligaciones salariales y bloquean nuevas contrataciones y formación hasta resolverse. Las tareas del empleado se devuelven siempre a un estado seguro.

#### 16.7. Cierre a las 22:00

A las `21:45` se avisa del cierre y no se asignan tareas largas.

A las `22:00`:

- Se bloquean tareas nuevas.
- Los empleados pasan a estado `Closing`.
- Terminan únicamente cobros o interacciones críticas ya iniciadas.
- Aseguran productos, paquetes y cajas.
- Pausan o devuelven tareas incompletas.
- Abandonan el establecimiento.

Existe un margen técnico hasta las `22:15`, incluido en el salario y sin horas extra. A esa hora todos deben estar fuera antes del cierre contable y el autoguardado.

Cada tarea declarará una política de cierre:

```text
FinishCurrentAction
MoveToSafeLocation
PauseAndSaveProgress
ReturnToQueue
Cancel
```

#### 16.8. Prioridad y automatización

Cada empleado tendrá:

```text
1 tarea activa
3 tareas reservadas
```

Estados de tarea:

```text
Pending
Reserved
InProgress
Paused
Completed
Cancelled
Failed
```

Prioridades base:

| Prioridad | Puntuación |
|---|---:|
| Crítica | 500 |
| Muy alta | 400 |
| Normal | 300 |
| Baja | 200 |
| Opcional | 100 |

La puntuación final combinará:

```text
Prioridad base
+ urgencia
+ adecuación al perfil
+ nivel de habilidad
+ antigüedad
- distancia
- cansancio
- coste de cambiar de tarea
```

Las órdenes manuales reciben `+1.000` y tienen prioridad absoluta cuando pueden ejecutarse con seguridad.

Modos:

- Automático.
- Prioridades personalizadas.
- Manual.

Categorías configurables:

- Preferida.
- Permitida.
- Emergencia.
- Prohibida.

Presets:

- Equilibrado.
- Clientes primero.
- Stock primero.
- Pedidos primero.
- Personalizado.

Las tareas duplicadas se agrupan, los recursos se reservan, las tareas inaccesibles generan una alerta después de varios intentos y las tareas de empleados ausentes regresan a la cola global.

#### 16.9. Fórmula base de eficiencia

```text
DuraciónRealTarea =
DuraciónBase / (1 + 0,15 × (Habilidad - 1))
```

## 12.13. Investigación

**Estado de alcance:** `DIFERIDO CERCANO`

> **Regla vigente:** La investigación desbloqueará profundidad y comodidad, pero no debe convertirse en un árbol independiente del mundo físico ni adelantar sistemas tardíos.

La investigación representa conocimiento empresarial, técnico y comercial.

#### Ramas previstas

1. **Operaciones de tienda**
   - Capacidad.
   - Logística.
   - Reservas.
   - Herramientas de gestión.

2. **Servicios al cliente**
   - Puestos informáticos.
   - Eventos.
   - Servicios premium.

3. **Expansión comercial**
   - Nuevos proveedores.
   - Venta online.
   - Distribución.
   - Edificios comerciales.

4. **Publishing y desarrollo**
   - Evaluación de proyectos.
   - Contratos.
   - Producción propia.

5. **Plataforma e infraestructura**
   - Distribución digital.
   - Servicios online.
   - Servidores.

Cada nodo podrá requerir:

- Puntos.
- Dinero.
- Nivel empresarial.
- Reputación.
- Nodos previos.
- Tiempo.

#### Modelo aprobado: híbrido

- Los hitos y descubrimientos conceden puntos.
- Los informes diarios pueden conceder una bonificación pequeña por variedad de información.
- Los nodos consumen puntos, dinero y tiempo.
- Las acciones repetidas no generan puntos indefinidamente.
- Algunos empleados podrán acelerar proyectos en fases posteriores.

Ejemplos de hitos:

- Vender por primera vez una categoría.
- Alcanzar cifras acumuladas.
- Atender perfiles diferentes.
- Completar reservas.
- Abrir servicios.
- Descubrir tendencias.
- Mantener satisfacción alta durante varios días.

#### Valores iniciales por categoría

| Categoría | Puntos | Coste | Tiempo |
|---|---:|---:|---:|
| Básico | 10–20 | 500–1.500 € | 1 día |
| Intermedio | 25–50 | 2.000–8.000 € | 2–4 días |
| Avanzado | 60–120 | 10.000–40.000 € | 5–10 días |
| Estratégico | 150+ | 50.000 € o más | 10+ días |

Todos los valores serán configurables mediante datos.

## 12.14. Publishing

**Estado de alcance:** `VISIÓN EMPRESARIAL TARDÍA`

> **Regla vigente:** Se conserva como dirección estratégica. No constituye alcance comprometido del vertical slice ni de la primera estabilización comercial.

El publishing será una línea de negocio avanzada centrada en descubrir, financiar, acompañar y comercializar proyectos creados por estudios externos. No será una variante pasiva del desarrollo interno ni una pantalla de inversión automática.

La fantasía será:

> **Detectar proyectos prometedores, negociar acuerdos, aportar financiación y servicios, supervisar su producción y convertirlos en lanzamientos comercialmente viables.**

#### Función dentro de la progresión

El publishing se desbloquea en la **Etapa 4**, después de estabilizar el comercio online y la logística. Aprovechará:

- Datos de ventas y preferencias de clientes.
- Comunidad y eventos de la tienda.
- Distribución física.
- Comercio online.
- Marketing.
- Relaciones con proveedores y estudios.
- La futura plataforma digital.

La diferencia fundamental respecto al desarrollo interno será:

| Publishing | Desarrollo interno |
|---|---|
| Proyecto de un estudio externo | Proyecto propiedad de la empresa |
| Riesgo compartido | Riesgo asumido internamente |
| Control creativo limitado por contrato | Control creativo directo |
| Inversión y servicios por hitos | Salarios, herramientas y producción propia |
| Participación en ingresos | Ingreso completo tras costes y comisiones |

#### Bucle principal

```text
Recibir propuestas
→ evaluar proyecto y estudio
→ realizar due diligence
→ negociar contrato
→ financiar por hitos
→ supervisar producción
→ resolver incidencias
→ preparar marketing y distribución
→ lanzar
→ liquidar ingresos
→ mantener relación y catálogo
```

#### Estudios externos

Cada estudio tendrá:

- Nombre e identidad ficticia.
- Tamaño y experiencia.
- Creatividad, capacidad técnica, producción y capacidad comercial.
- Fiabilidad contractual.
- Historial de lanzamientos.
- Situación financiera.
- Reputación.
- Relación con la empresa entre `-100` y `+100`.

Tamaños iniciales:

| Tipo | Miembros aproximados |
|---|---:|
| Microestudio | 1–3 |
| Estudio pequeño | 4–8 |
| Estudio consolidado | 9–20 |
| Estudio mediano | 21–50 |

Los equipos mayores aparecerán conforme aumente la reputación editorial.

#### Propuestas

Las propuestas llegarán con una frecuencia inicial de `2–4 por mes de juego` y podrán encontrarse en:

- Concepto.
- Prototipo.
- Producción.
- Fase cercana al lanzamiento.

Cada propuesta incluirá:

- Estudio.
- Nombre provisional.
- Género, tema, plataforma y público.
- Estado del proyecto.
- Presupuesto solicitado.
- Duración estimada.
- Alcance.
- Servicios requeridos.
- Ventana de lanzamiento.
- Riesgos declarados.
- Potencial comercial estimado.

#### Evaluación e incertidumbre

El jugador no conocerá de forma exacta la calidad futura. Verá estimaciones con un nivel de confianza.

Áreas evaluadas:

- Potencial creativo.
- Estado técnico.
- Viabilidad del alcance.
- Encaje de mercado.
- Capacidad del estudio.
- Presupuesto.
- Ventana de lanzamiento.
- Encaje con el catálogo.

Ejemplo:

```text
Potencial creativo: Alto
Estado técnico: Desconocido
Riesgo de alcance: Alto
Adecuación al mercado: Media
Confianza del informe: 48 %
```

#### Due diligence

| Evaluación | Coste provisional | Duración | Efecto |
|---|---:|---:|---|
| Revisión básica | 1.000 € | 1 día | Documentación, presupuesto e historial |
| Revisión técnica | 3.000 € | 2 días | Prototipo, rendimiento y riesgos técnicos |
| Estudio de mercado | 4.000 € | 2 días | Público, competencia y precio |
| Evaluación completa | 7.500 € | 4 días | Combina las anteriores con mayor confianza |

La información nunca será completamente perfecta.

#### Decisiones sobre una propuesta

El jugador podrá:

- Rechazar.
- Archivar.
- Solicitar cambios.
- Pedir un prototipo.
- Ofrecer distribución únicamente.
- Negociar financiación y servicios.
- Firmar.

Los cambios solicitados podrán afectar a plataformas, alcance, presupuesto, localización, calendario o público.

#### Tipos de contrato

| Contrato | Inversión orientativa | Participación editorial | Riesgo | Control |
|---|---:|---:|---|---|
| Distribución | 5.000–30.000 € | 10–20 % | Bajo | Bajo |
| Marketing y lanzamiento | 20.000–100.000 € | 15–30 % | Medio | Medio-bajo |
| Financiación parcial | 50.000–250.000 € | 25–40 % | Medio-alto | Medio |
| Financiación completa | 150.000–1.000.000 € | 40–60 % | Alto | Alto |
| Coproducción | Variable | 30–50 % | Compartido | Alto |

La primera implementación utilizará distribución, marketing y financiación parcial.

#### Elementos negociables

- Anticipo.
- Presupuesto.
- Pagos por hitos.
- Participación en ingresos.
- Recuperación prioritaria de inversión.
- Plataformas y territorios.
- Servicios incluidos.
- Control creativo.
- Propiedad intelectual.
- Bonificaciones y penalizaciones.
- Opciones sobre secuelas.

La propiedad del estudio será la condición habitual. La propiedad compartida o del publisher se reservará para acuerdos avanzados y podrá afectar a la reputación editorial.

#### Producción por hitos

Hitos orientativos:

1. Preproducción.
2. Vertical slice.
3. Alpha.
4. Beta.
5. Release Candidate.
6. Lanzamiento.

Cada hito tendrá:

- Fecha.
- Objetivos.
- Presupuesto.
- Entregables.
- Calidad prevista.
- Riesgos.
- Pago asociado.

Resultados de revisión:

- Aprobado.
- Aprobado con observaciones.
- Revisión requerida.
- Rechazado.

#### Capacidad editorial

Los proyectos consumirán capacidad:

| Complejidad | Capacidad |
|---|---:|
| Pequeño | 1 |
| Medio | 2 |
| Grande | 4 |
| Estratégico | 6 |

Capacidad inicial recomendada: `4 puntos`.

Se ampliará mediante personal, investigación, espacio, herramientas y reputación.

#### Intervención editorial

- **Mínima:** libertad alta y menor coste, pero mayor riesgo.
- **Colaborativa:** equilibrio recomendado.
- **Directiva:** mayor control, coste y riesgo de conflicto.

Los problemas de producción dependerán del alcance, presupuesto, capacidades, deuda, calendario y decisiones previas. Podrán incluir retrasos, bugs graves, cambios de personal, conflictos creativos o cambios de mercado.

#### Reputación editorial

```text
Reputación editorial: 0–100
Valor inicial: 40
```

Aumentará por:

- Cumplir pagos y compromisos.
- Tratar justamente a los estudios.
- Ejecutar buen marketing.
- Resolver problemas.
- Lanzar productos sólidos.

Disminuirá por:

- Contratos abusivos.
- Retrasos de pagos.
- Cancelaciones injustificadas.
- Sobrecarga de estudios.
- Mala gestión de lanzamientos.

La reputación controlará la calidad y cantidad de propuestas disponibles.

#### Marketing, distribución y lanzamiento

El jugador decidirá:

- Presupuesto y fases de marketing.
- Fecha de lanzamiento.
- Venta digital o física.
- Tirada física.
- Uso de tienda, eventos y comercio online.
- Distribución externa o futura plataforma propia.

Las ventas dependerán de:

```text
Calidad
+ adecuación al mercado
+ reputación del estudio
+ reputación editorial
+ marketing
+ precio
+ distribución
+ tendencias
- competencia
- problemas técnicos
- saturación
```

Calidad y potencial comercial serán variables separadas.

#### Recuperación de inversión y liquidación

```text
Ingresos netos
→ recuperación de costes recuperables
→ reparto de beneficios
```

Las liquidaciones serán semanales y mostrarán:

- Ventas.
- Comisiones.
- Fabricación.
- Marketing recuperable.
- Inversión pendiente.
- Beneficio repartible.
- Pago al estudio.
- Ingreso del publisher.

#### Catálogo y sinergias

Cada juego publicado conservará contrato, inversión, ventas, rentabilidad, recepción, plataformas, relación, propiedad y estado.

El publishing se conectará con:

- Datos de la tienda física.
- Playtests y eventos.
- Reservas y ediciones físicas.
- Comercio online.
- Desarrollo interno.
- Plataforma digital.

#### Publishing MVP

Incluirá:

- Estudios y propuestas.
- Evaluación básica y due diligence.
- Tres tipos de contrato.
- Negociación sencilla.
- Pagos por hitos.
- Problemas de producción.
- Marketing y lanzamiento.
- Ventas y liquidaciones.
- Reputación editorial.
- Relaciones.
- Catálogo persistente.
- Guardado y carga.

Quedan pospuestos:

- Propiedad intelectual compleja.
- Adquisiciones.
- Coproducción profunda.
- Exclusividades complejas.
- Secuelas y DLC avanzados.
- Disputas contractuales extensas.

#### Criterios de aceptación del MVP

El jugador debe poder recibir, evaluar, negociar, firmar, financiar, supervisar, lanzar y liquidar un proyecto externo sin duplicar pagos, ventas o estados.

## 12.15. Desarrollo interno de videojuegos

**Estado de alcance:** `VISIÓN EMPRESARIAL TARDÍA`

> **Regla vigente:** Debe aparecer como consecuencia del conocimiento y capital acumulados por el negocio, nunca como sustituto de la tienda ni como retorno al bucle clásico de un tycoon de desarrollo.

El desarrollo interno será una línea avanzada y diferenciada del modelo clásico de *Game Dev Tycoon*.

No se basará en:

- Buscar combinaciones ocultas de género, tema y plataforma.
- Repartir porcentajes abstractos entre diseño, tecnología y arte.
- Esperar a que se llenen barras de puntos.
- Recibir una nota producida casi exclusivamente por compatibilidades.

La fantasía será:

> **Descubrir oportunidades, formular una visión, validar prototipos, organizar equipos, controlar el alcance, producir por hitos, probar con usuarios reales y lanzar un producto coherente.**

#### Bucle principal

```text
Detectar oportunidad
→ crear brief
→ definir promesa y pilares
→ prototipar
→ realizar playtests
→ greenlight, posponer o cancelar
→ formar equipo
→ planificar backlog e hitos
→ desarrollar e integrar builds
→ probar, recortar o ampliar
→ alcanzar Release Candidate
→ lanzar
→ mantener y aprender
```

#### Insights y oportunidades

Las oportunidades procederán de:

- Ventas y búsquedas de la tienda.
- Preferencias de arquetipos de cliente.
- Comercio online.
- Publishing.
- Tendencias y saturación.
- Eventos.
- Propuestas del equipo.

Cada insight tendrá fuente, público, confianza, antigüedad y evidencias. No otorgará una bonificación automática; servirá para tomar decisiones informadas.

#### Brief y visión

El brief definirá:

- Nombre provisional.
- Fantasía principal.
- Público objetivo.
- Tipo de experiencia.
- Género o combinación.
- Perspectiva.
- Plataformas.
- Modelo comercial.
- Alcance.
- Duración.
- Ventana.
- Presupuesto.
- Insights utilizados.

Cada proyecto tendrá entre `2 y 4` pilares y una promesa de experiencia:

> “El jugador debe sentirse como…”

Las funcionalidades deberán apoyar al menos un pilar o justificar claramente su inclusión.

#### Prototipos

Tipos:

- Núcleo jugable.
- Técnico.
- Visual.
- Contenido.
- Comercial.

Cada prototipo responderá preguntas concretas. Un prototipo sin preguntas producirá información menos fiable.

Costes orientativos:

| Prototipo | Coste | Duración |
|---|---:|---:|
| Núcleo | 10.000–25.000 € | 2–4 días |
| Técnico | 15.000–40.000 € | 3–6 días |
| Visual | 8.000–20.000 € | 2–4 días |
| Contenido | 15.000–35.000 € | 3–5 días |
| Comercial | 5.000–15.000 € | 1–3 días |

#### Playtests de prototipo

Grupos:

- Equipo interno.
- Clientes de la tienda.
- Comunidad online.
- Panel externo.

Métricas:

- Comprensión.
- Interés.
- intención de continuar.
- estabilidad.
- accesibilidad.
- identidad.
- adecuación al público.
- intención de compra.
- alineación con pilares.

#### Greenlight

Áreas evaluadas:

- Confianza creativa.
- Viabilidad técnica.
- Encaje comercial.
- Viabilidad financiera.
- Preparación del equipo.

Resultados:

- Aprobado.
- Aprobado con condiciones.
- Pospuesto.
- Cancelado.
- Greenlight forzado con riesgos visibles.

Cancelar conservará aprendizajes, prototipos, tecnologías reutilizables y experiencia.

#### Tamaños de proyecto

| Tamaño | Alcance | Equipo | Duración | Presupuesto orientativo |
|---|---:|---:|---:|---:|
| Micro | 40–60 puntos | 2–4 | 5–9 días | 50.000–120.000 € |
| Pequeño | 80–120 | 4–7 | 8–15 días | 120.000–300.000 € |
| Mediano | 160–240 | 8–14 | 15–25 días | 350.000–900.000 € |
| Grande | 300–450 | 15–30 | 25–40 días | 1–3 M€ |
| Emblemático | 500+ | 30+ | 40+ días | 3 M€ o más |

El primer juego interno será micro o pequeño.

#### Equipo interno

Perfiles:

- Dirección de juego.
- Producción.
- Diseño.
- Programación.
- Arte.
- Audio.
- QA.
- Comunidad y marketing.

Las personas podrán cubrir varios roles en equipos pequeños. Las habilidades estarán en escala `1–5` y podrán incluir especialidades.

#### Backlog de funcionalidades

Cada funcionalidad tendrá:

- Complejidad.
- Valor.
- Pilar relacionado.
- Disciplinas.
- Dependencias.
- Riesgo.
- Estado.
- Calidad.
- Bugs.
- Coste y fecha.

Estados:

```text
Propuesta
→ Aprobada
→ Planificada
→ En desarrollo
→ En revisión
→ Integrada
→ Validada
```

También podrá quedar bloqueada, pausada, recortada, cancelada o pospuesta.

#### Dependencias y trabajo en curso

Comenzar sin dependencias completas aumentará retrabajo y errores. El límite inicial recomendado será:

```text
Máximo de tres funcionalidades activas
```

Superarlo generará cambios de contexto, integración tardía y deuda.

#### Hitos internos

- Concept Complete.
- Vertical Slice.
- Alpha.
- Beta.
- Release Candidate.
- Gold.

Cada revisión mostrará objetivos, presupuesto, retrasos, calidad, riesgos, bugs, feedback y carga del equipo.

#### Gestión del alcance

El proyecto distinguirá:

- Núcleo obligatorio.
- Funciones importantes.
- Contenido secundario.
- Mejoras opcionales.
- Ideas futuras.

Los cambios tardíos serán más caros y peligrosos. El *scope creep* será una consecuencia de decisiones, no un evento aleatorio aislado.

#### Deuda técnica y de diseño

La deuda técnica aumentará por prisas, falta de pruebas, tecnologías inadecuadas, cambios tardíos y exceso de trabajo simultáneo.

La deuda de diseño aumentará por sistemas contradictorios, funcionalidades sin propósito, cambios de visión y falta de validación.

Podrán reducirse mediante refactor, revisión, simplificación, eliminación y pruebas.

#### Builds e integración

Estados de build:

- Inestable.
- Jugable.
- Estable.
- Candidata.
- Aprobada.

Integrar revelará incompatibilidades, bugs, problemas de rendimiento o datos. Esperar demasiado para integrar aumentará el riesgo.

#### Playtests durante producción

Los grupos podrán ser internos, clientes, jugadores del género, público general o testers profesionales.

El feedback se clasificará por temas y el jugador decidirá aceptarlo, investigarlo, posponerlo o ignorarlo. Seguir todo el feedback será perjudicial; se valorará la coherencia con público, pilares, datos y alcance.

#### Calidad y mercado

Dimensiones de calidad:

- Núcleo jugable.
- Coherencia.
- Tecnología.
- Contenido.
- UX y accesibilidad.
- Presentación.
- Identidad.

```text
Calidad del producto ≠ potencial comercial
```

Un buen producto puede vender poco y uno comercial puede recibir malas críticas.

#### Presupuesto y contingencia

El presupuesto se dividirá entre salarios, outsourcing, herramientas, prototipos, producción, QA, localización, marketing, fabricación y contingencia.

Reserva recomendada:

```text
10–20 % del presupuesto
```

#### Intensidad de producción

- Normal.
- Elevada.
- Crisis.

La crisis aumentará capacidad temporal, cansancio, deuda, errores y riesgo laboral. No será óptima como estrategia permanente.

#### Lanzamiento y postlanzamiento

Antes del lanzamiento se aprobarán Release Candidate, QA, rendimiento, localización, precio, distribución, marketing y fecha.

Después se decidirá entre:

- Parches.
- Actualizaciones.
- Contenido.
- Nuevas plataformas.
- Edición física.
- Descuentos.
- Finalizar soporte.
- Preparar secuela.

Cada proyecto generará conocimiento persistente sobre géneros, tecnología, público, producción y marketing.

#### Sinergias

La tienda permitirá playtests, eventos, reservas, ediciones físicas y observación de clientes. Publishing aportará experiencia de producción y distribución. La plataforma aportará usuarios, analytics y distribución propia.

#### Desarrollo interno MVP

Incluirá:

- Un proyecto activo.
- Tamaños micro y pequeño.
- Brief, promesa y tres pilares.
- Prototipo de núcleo.
- Playtest y greenlight.
- Equipo básico.
- Backlog y dependencias.
- Vertical Slice, Alpha, Beta y Release Candidate.
- Builds y bugs.
- Gestión básica de alcance y deuda técnica.
- Presupuesto.
- Lanzamiento, reviews, ventas y un parche.
- Guardado y carga.

Quedan pospuestos proyectos simultáneos, secuelas complejas, franquicias, DLC profundo, multijugador avanzado, live service y equipos internacionales.

## 12.16. Comercio online y logística

**Estado de alcance:** `VISIÓN DE MEDIO PLAZO`

> **Regla vigente:** Es la primera gran extensión multicanal. Debe reutilizar catálogo, precios, inventario, pedidos, empleados y reputación en lugar de duplicarlos.

El comercio online será una extensión logística y comercial de la tienda física. No se confundirá con la plataforma digital.

```text
Comercio online = venta y envío de productos físicos
Plataforma digital = venta, licencia y distribución de videojuegos digitales
```

La fantasía será:

> **Convertir el inventario, la reputación y la logística de la tienda en un negocio a distancia capaz de preparar, enviar y atender pedidos físicos.**

#### Bucle principal

```text
Configurar catálogo
→ establecer precios y stock protegido
→ recibir pedidos
→ reservar inventario
→ recoger productos
→ empaquetar y etiquetar
→ trasladar a expedición
→ entregar al transportista
→ seguir el envío
→ resolver incidencias y devoluciones
→ analizar rentabilidad
```

#### Activación

La Etapa 3 requiere la zona logística `10 × 12`, investigación, nivel, reputación, inversión y el hito económico ya definidos.

Para abrir el canal se necesitará:

1. Estación de empaquetado.
2. Zona de pedidos pendientes.
3. Zona de paquetes terminados.
4. Catálogo mínimo.
5. Transportista.
6. Método de envío.
7. Capital operativo.
8. Jugador o empleado asignado.

Estados del canal:

- Cerrado.
- Abierto.
- Saturado.
- Suspendido.

#### Catálogo y precios

Cada producto configurará:

- Activo o inactivo.
- Precio online.
- Stock protegido para tienda.
- Stock máximo ofrecido.
- Límite por pedido.
- Métodos y regiones.
- Promociones.

El precio online podrá diferir del físico. La interfaz mostrará margen después de coste, embalaje, comisión, transporte subvencionado y devolución estimada.

#### Inventario compartido

Estados de una unidad:

```text
Available
ReservedInStore
ReservedOnline
Picking
Packed
Shipped
Returned
Damaged
```

Una unidad solo podrá estar en un estado operativo.

El stock protegido impedirá que el canal online consuma una cantidad mínima reservada para la tienda física.

#### Reserva y disponibilidad

Al confirmar un pedido:

1. Se valida disponibilidad.
2. Las unidades pasan a `ReservedOnline`.
3. Dejan de estar disponibles en ambos canales.
4. Se genera la tarea.
5. Comienza el plazo de procesamiento.

Preventas, pedidos bajo demanda y *backorders* quedan pospuestos.

#### Demanda online

Dependerá de:

```text
Visibilidad
+ reputación
+ precio
+ disponibilidad
+ variedad
+ velocidad de envío
+ valoraciones
+ promociones
+ tendencias
- competencia
- retrasos
- cancelaciones
- gastos de envío
```

Los clientes online se representarán mediante perfiles y métricas, no NPCs físicos.

#### Estados de pedido

```text
Confirmed
→ ReadyForPicking
→ Picking
→ ReadyForPacking
→ Packing
→ Packed
→ ReadyForDispatch
→ Dispatched
→ InTransit
→ Delivered
```

Estados alternativos:

- Cancelled.
- Delayed.
- Lost.
- Returned.
- Refunded.

Cada transición quedará registrada.

#### Pagos

El pago será abstracto. Comisión provisional:

```text
2 % del importe pagado
```

No se simularán bancos, tarjetas ni fraude en el MVP.

#### Picking y packing

El pedido generará tareas físicas:

```text
Recoger productos
→ llevar a estación
→ comprobar contenido
→ seleccionar embalaje
→ cerrar
→ etiquetar
→ mover a expedición
```

El origen preferente será el almacén. No se retirarán productos de exposición automáticamente salvo permiso del jugador.

#### Embalajes

| Tipo | Coste provisional |
|---|---:|
| Sobre protegido | 1 € |
| Caja pequeña | 2 € |
| Caja mediana | 4 € |
| Caja reforzada | 8 € |
| Protección adicional | 1–5 € |

Cada producto tendrá peso, volumen y fragilidad abstractos.

#### Transportistas

- Económico: 3–5 días.
- Estándar: 2–3 días.
- Exprés: 1 día.

El jugador decidirá cuánto paga el cliente y cuánto subvenciona la empresa.

Regla inicial recomendada:

```text
Envío estándar gratuito desde 100 €
```

#### Capacidad logística

Con una estación y un trabajador competente:

```text
8–12 pedidos por día
```

Objetivo de preparación:

```text
Dentro de 1 día
```

Pedidos confirmados antes de las `16:00` podrán salir el mismo día. Recogida provisional: `19:00`.

#### Satisfacción y reputación online

La satisfacción dependerá de producto, estado, precio, rapidez, embalaje, comunicación, errores, daños y coste de envío.

```text
Reputación online: 0–100
Valor inicial: 50
```

Las valoraciones utilizarán `1–5 estrellas`. Aproximadamente `20–35 %` de clientes valorarán, con mayor probabilidad en extremos de satisfacción.

#### Incidencias, cancelaciones y devoluciones

Incidencias iniciales:

- Retraso.
- Producto incorrecto.
- Daño.
- Cancelación.
- Devolución.
- Consulta de estado.

Política de devolución orientativa:

```text
3 días desde la entrega
```

Los productos devueltos podrán volver como nuevos, usados, dañados o no vendibles.

#### Integración con empleados y cierre

El sistema de tareas incluirá picking, packing, etiquetado, expedición e incidencias.

A las `22:00`:

- El canal podrá seguir aceptando pedidos.
- Los nuevos pedidos quedarán para el día siguiente.
- Las tareas activas se asegurarán.
- Los productos seguirán reservados.
- Los paquetes quedarán guardados.

#### Comercio online MVP

Incluirá:

- Canal activable.
- Catálogo y precios independientes.
- Stock compartido y protegido.
- Pedidos locales y nacionales.
- Reserva, picking y packing.
- Cuatro embalajes.
- Tres métodos de envío.
- Recogida y seguimiento básicos.
- Entrega, satisfacción y valoraciones.
- Reputación online.
- Cancelación antes del envío.
- Devolución sencilla.
- Métricas.
- Guardado y carga.

Quedan pospuestos venta internacional, preventas, fraude, múltiples almacenes, marketplace, suscripciones, multidivisa y logística propia.

## 12.17. Plataforma digital

**Estado de alcance:** `VISIÓN EMPRESARIAL TARDÍA`

> **Regla vigente:** Representa una culminación posible del crecimiento, no una obligación del alcance inicial.

La plataforma digital será la culminación empresarial de **Cartridge & Cloud** y el núcleo de la condición principal de victoria.

Representará un ecosistema capaz de:

- Gestionar cuentas y bibliotecas.
- Vender licencias digitales.
- Distribuir juegos y actualizaciones.
- Incorporar estudios externos.
- Organizar promociones y descubrimiento.
- Gestionar reviews y reembolsos.
- Operar servicios online.
- Generar datos y comunidad.

#### Bucle principal

```text
Definir posicionamiento
→ preparar infraestructura
→ incorporar catálogo
→ configurar políticas
→ beta cerrada
→ acceso anticipado
→ lanzamiento público
→ atraer y retener usuarios
→ ampliar catálogo y servicios
→ escalar capacidad
→ alcanzar sostenibilidad y victoria
```

#### Pilares

- Confianza.
- Catálogo.
- Descubrimiento.
- Fiabilidad.
- Equilibrio entre usuarios, estudios y empresa.

#### Identidad

La plataforma podrá orientarse como generalista, indie, especializada o conectada con la comunidad física. La recomendación inicial es un modelo híbrido con identidad surgida del catálogo y las decisiones.

#### Fases de lanzamiento

1. Entorno interno.
2. Beta cerrada de `500–2.000 usuarios`.
3. Acceso anticipado de `2.000–10.000 usuarios`.
4. Lanzamiento público.
5. Crecimiento.

Requisitos provisionales del lanzamiento público:

| Requisito | Valor |
|---|---:|
| Juegos disponibles | 20 |
| Juegos propios o publicados | 5 |
| Estudios externos activos | 5 |
| Usuarios de beta | 2.000 |
| Reputación de plataforma | 50/100 |
| Disponibilidad | 97 % |
| Incidencias críticas | 0 |
| Reserva operativa | 100.000 € |

#### Catálogo

Tipos:

- Juegos internos.
- Juegos publicados.
- Juegos de terceros.
- Contenido descargable sencillo.

Los estudios externos seguirán un flujo de solicitud, revisión técnica, creación de ficha, validación de build, configuración comercial y publicación.

Modelo recomendado: híbrido. Todos los juegos superan requisitos técnicos, pero la empresa no aprueba subjetivamente cada diseño.

#### Estados de publicación

```text
Draft
→ Submitted
→ UnderReview
→ TechnicalValidation
→ Approved
→ Scheduled
→ Published
```

Alternativas:

- ChangesRequired.
- Rejected.
- Suspended.
- Delisted.
- Archived.

#### Comisión

```text
Comisión estándar de plataforma: 20 %
Procesamiento de pago: 2 %
```

Acuerdos preferentes podrán reducir la comisión. Las condiciones afectarán a la confianza de desarrolladores.

```text
Confianza de desarrolladores: 0–100
Valor inicial: 40
```

#### Cuentas, bibliotecas y licencias

Cada usuario tendrá región, biblioteca, lista de deseos, historial, preferencias y actividad.

Estados de licencia:

```text
Owned
Gifted
RefundPending
Refunded
Revoked
```

La integridad debe impedir doble cobro, doble licencia, pérdida de licencia o acceso tras reembolso.

#### Descargas y actualizaciones

Las descargas utilizarán tamaño, capacidad, ancho de banda, región y concurrencia. Las builds pasarán por validación y podrán desplegarse como parche, actualización, contenido o hotfix.

#### Descubrimiento

Incluye:

- Búsqueda.
- Filtros.
- Listas de deseos.
- Tendencias.
- Más vendidos.
- Recomendaciones.
- Colecciones editoriales.
- Espacios promocionados identificados.

La visibilidad dependerá de ficha, conversión, reviews, ventas, actualizaciones, adecuación, campañas, reembolsos e incidencias; no solo de pagar publicidad.

#### Reviews y reembolsos

Solo podrán valorar usuarios con licencia. El MVP utilizará:

```text
Recomendado / No recomendado
```

Política ficticia:

```text
Hasta 7 días desde la compra
y menos de 2 horas de uso
```

Dentro del límite, aprobación automática; fuera, revisión manual.

#### Usuarios, retención y adquisición

Métricas:

- Registrados.
- Activos diarios, semanales y mensuales.
- Nuevos.
- Recurrentes.
- Perdidos.
- Conversión.
- CAC.
- Valor por usuario.

Captar un registro no equivaldrá a retener un usuario.

#### Reputación de plataforma

```text
Reputación de plataforma: 0–100
Valor inicial: 40
```

Aumentará con estabilidad, soporte, políticas justas, catálogo y transparencia. Disminuirá con caídas, cobros erróneos, pérdida de licencias, mala moderación o políticas abusivas.

#### Soporte y pagos a estudios

Se gestionarán incidencias de usuarios y desarrolladores. Las liquidaciones serán semanales y mostrarán ventas, reembolsos, impuestos, procesamiento, comisión e ingreso neto.

#### Rentabilidad

```text
Ingresos:
comisiones + promociones + servicios

Gastos:
infraestructura + soporte + desarrollo + adquisición + mantenimiento
```

Muchas ventas no garantizan rentabilidad.

#### Gobernanza

Políticas configurables:

- Comisiones.
- Reembolsos.
- Curación.
- Visibilidad.
- Moderación.
- Promociones.
- Pagos.
- Exclusividades futuras.

Periodo mínimo recomendado entre cambios importantes: `14 días`.

#### Condición principal de victoria

Se alcanzará al cumplir simultáneamente:

```text
Plataforma pública activa
100.000 usuarios registrados
25.000 usuarios activos mensuales
Reputación de plataforma ≥ 75
Confianza de desarrolladores ≥ 70
Disponibilidad ≥ 99 %
4 semanas consecutivas con beneficio neto positivo
50 juegos publicados
10 estudios externos activos
```

Después se mostrará el cierre de campaña y se habilitará el postgame abierto.

#### Plataforma MVP

Incluirá:

- Una región.
- Cuentas, bibliotecas y licencias.
- Catálogo propio, publicado y externo.
- Comisión configurable.
- Fichas y búsqueda.
- Listas de deseos.
- Compras y descargas abstractas.
- Actualizaciones.
- Reviews verificadas.
- Reembolsos.
- Promociones.
- Pagos semanales.
- Reputación y confianza.
- Soporte básico.
- Lanzamiento por fases.
- Condición de victoria.
- Guardado y carga.

Quedan pospuestos marketplace, suscripciones, chat, foros, workshop, streaming, cloud gaming, economía de objetos, mods, UGC y funciones sociales profundas.

## 12.18. Infraestructura y servicios online

**Estado de alcance:** `VISIÓN EMPRESARIAL TARDÍA`

> **Regla vigente:** Debe representarse con abstracción estratégica y presencia física legible, evitando convertir el juego en un simulador técnico de centros de datos.

La infraestructura tendrá una profundidad estratégica intermedia. No será una cifra pasiva, pero tampoco un simulador profesional de administración de sistemas.

La fantasía será:

> **Construir y operar la infraestructura que sostiene la plataforma, anticipar la demanda y equilibrar capacidad, coste, fiabilidad, seguridad y crecimiento.**

El jugador no configurará direcciones IP, sistemas operativos, cableado detallado ni componentes reales.

#### Tres capas

##### Capa física simplificada

- Racks.
- refrigeración.
- alimentación.
- almacenamiento.
- red abstracta.
- centro de operaciones.

##### Capa operativa

- Cómputo.
- almacenamiento.
- ancho de banda.
- base de datos.
- concurrencia.
- soporte.

##### Capa de servicios

- Cuentas.
- catálogo.
- licencias.
- pagos.
- descargas.
- actualizaciones.
- reviews.
- recomendaciones.
- analítica.
- servicios para juegos.

#### Pools de capacidad

La infraestructura se administrará como recursos agregados, no máquinas individuales.

Cada recurso mostrará capacidad, uso, pico, previsión, reserva, coste y estado.

| Uso | Estado |
|---:|---|
| 0–59 % | Saludable |
| 60–74 % | Elevado |
| 75–89 % | Riesgo |
| 90–99 % | Crítico |
| 100 %+ | Saturado |

Reserva recomendada:

```text
20–30 % de capacidad libre
```

#### Modelos de infraestructura

- Gestionada: rápida y flexible, pero cara y dependiente.
- Propia: inversión alta y control, con costes de energía y mantenimiento.
- Híbrida: base propia más capacidad externa para picos.

El modelo híbrido será la recomendación inicial.

#### Módulos físicos

- Rack de cómputo.
- Rack de almacenamiento.
- Red.
- Base de datos.
- Refrigeración.
- Alimentación redundante.
- Backups.
- Centro de operaciones.

Los tiers E–S modificarán capacidad, eficiencia, fiabilidad, coste y ocupación.

#### Espacio, temperatura y electricidad

Los módulos generarán carga térmica abstracta:

```text
Carga térmica total ≤ capacidad de refrigeración
```

Superarla aumentará consumo, reducirá rendimiento y elevará el riesgo.

La electricidad dependerá de carga base, utilización, refrigeración y redundancia.

#### Escalado

- Vertical: mejorar módulos.
- Horizontal: añadir módulos.
- Externo: contratar capacidad.
- Temporal: capacidad por 1, 3 o 7 días para eventos y lanzamientos.

Las mejoras requerirán tiempo y podrán reducir capacidad durante la instalación.

#### Prioridad de servicios

- Crítica: cuentas, licencias, compras, bibliotecas.
- Alta: catálogo, descargas, actualizaciones.
- Media: reviews, recomendaciones, analítica.
- Variable: servicios de juegos.

Niveles de servicio:

- Económico.
- Estándar.
- Prioritario.

En saturación, los servicios secundarios podrán degradarse para proteger los críticos.

#### Degradación controlada

Ejemplos:

- Recomendaciones menos precisas.
- Analítica retrasada.
- Descargas en cola.
- Reviews en espera.
- Imágenes con menor prioridad.

Esto evitará convertir cualquier pico en una caída total.

#### Picos y despliegues

Los lanzamientos, promociones, betas y actualizaciones generarán previsiones de tráfico.

El jugador podrá:

- Contratar capacidad.
- Escalonar el despliegue.
- Reducir servicios secundarios.
- Retrasar.
- Aceptar riesgo.

Actualizaciones:

- Simultáneas.
- Por oleadas.
- Regionales futuras.

#### Redundancia y backups

Redundancia:

- Ninguna.
- Básica.
- Alta.
- Regional futura.

Backups:

- Diarios.
- Cada seis horas.
- Continuos.

Se mostrarán tiempo de recuperación y pérdida potencial de datos de forma comprensible.

#### Incidencias

Categorías:

- Saturación.
- Fallo de hardware.
- Error de actualización.
- Fallo de proveedor.
- Problema de base de datos.
- Problema de red.
- Incidente de seguridad abstracto.
- Error humano.

Su probabilidad dependerá de utilización, mantenimiento, calidad, redundancia, deuda, cambios, personal y seguridad.

Respuestas:

- Reinicio abstracto.
- Revertir actualización.
- Restaurar backup.
- Activar capacidad externa.
- Redirigir usuarios.
- Desactivar servicios secundarios.
- Llamar al proveedor.
- Comunicar y compensar.

#### Operaciones y personal

El centro de operaciones gestionará alertas, cambios, capacidad, mantenimiento, backups, seguridad e incidencias.

Perfiles futuros:

- Operador de sistemas.
- Especialista de red.
- Administrador de datos.
- Seguridad.
- Responsable de operaciones.
- Ingeniero de plataforma.

La cobertura 24/7 se abstraerá mediante niveles de guardia y automatización.

#### Mantenimiento y seguridad

Mantenimiento:

- Reactivo.
- Programado.
- Preventivo.

La seguridad será abstracta y cubrirá cuentas, pagos, builds, datos y operaciones. Podrán realizarse auditorías internas, externas o de lanzamiento.

No se simularán técnicas ofensivas reales.

#### Servicios online para juegos

- Guardado online.
- Estadísticas.
- Rankings.
- Matchmaking.
- Servidores dedicados abstractos.
- Telemetría.

Podrán cobrarse como tarifa fija, por uso o paquetes. Los juegos propios también consumirán capacidad real.

#### Costes y eficiencia

Se distinguirán costes fijos y variables. Métricas:

```text
Coste operativo por usuario activo
Ingreso medio por usuario activo
Coste por descarga
Coste por servicio
Disponibilidad
```

La eficiencia mejorará mediante módulos, automatización, investigación, personal y optimización, con rendimientos decrecientes.

#### Infraestructura MVP

Incluirá:

- Una región.
- Infraestructura híbrida.
- Seis recursos agregados.
- Módulos físicos.
- Capacidad externa y temporal.
- Utilización y costes.
- Refrigeración y electricidad.
- Tres niveles de redundancia.
- Backups.
- Prioridades.
- Degradación.
- Cola de descargas.
- Despliegues por oleadas.
- Incidencias y mantenimiento.
- Centro de operaciones.
- Previsión y disponibilidad.
- Guardado y carga.

Quedan pospuestos cableado, redes individuales, componentes reales, sistemas operativos, scripting, múltiples regiones profundas, cloud gaming y administración técnica detallada.

## 12.19. Mercado y competidores

**Estado de alcance:** `VISIÓN EMPRESARIAL TARDÍA`

> **Regla vigente:** El mercado debe aportar contexto y reacción, sin exigir una simulación exhaustiva ni permitir ventajas injustas o bolas de nieve inevitables.

El mercado deberá sentirse vivo sin simular cada detalle físico de las empresas rivales.

La fantasía será:

> **Construir una empresa dentro de un sector dinámico, detectar oportunidades, responder a rivales y encontrar una posición propia mediante precio, servicio, especialización, comunidad, publishing, desarrollo y plataforma.**

Los competidores no tendrán información perfecta, dinero infinito ni capacidad para reaccionar instantáneamente a cada decisión del jugador.

#### Modelo híbrido

##### Competidores visibles

Entre `4 y 6` empresas relevantes con:

- Nombre e identidad.
- Tamaño.
- Especialización.
- Reputación.
- Estrategia.
- Recursos abstractos.
- Cuota de mercado.
- Catálogo.
- Fortalezas y debilidades.
- Relación con el jugador.

##### Mercado agregado

Representará tiendas, comercios, estudios y plataformas menores sin identidad individual.

##### Entorno externo

Representará tendencias, tecnología, estacionalidad, costes, crecimiento, contracción y preferencias.

#### Competencia por etapas

- Etapas 1–2: tiendas locales y cadenas.
- Etapa 3: comercios online y logística.
- Etapa 4: publishers y distribución editorial.
- Etapa 5: estudios rivales y talento.
- Etapa 6: plataformas y servicios digitales.

#### Arquetipos

- Competidor de precio.
- Especialista.
- Cadena generalista.
- Negocio premium.
- Innovador digital.
- Publisher agresivo.
- Publisher de prestigio.
- Plataforma abierta.
- Plataforma cerrada.

Cada empresa tendrá agresividad, prudencia, innovación, orientación a precio, calidad, tolerancia al riesgo, finanzas, operaciones, reputación y adaptabilidad en escala `0–100`.

#### Recursos abstractos y frecuencia

Los rivales tendrán capital, ingresos, costes, capacidad, reputación, catálogo, empleados abstractos, infraestructura y cuota.

Simulación:

- Diaria: ventas, gastos, stock y demanda.
- Semanal: precios, promociones, campañas, cuota y proyectos.
- Mensual: expansiones, entradas, cierres, fusiones futuras y cambios estratégicos.

No se simularán muebles, NPCs, cajas ni pedidos individuales de competidores.

#### Información disponible

El jugador verá:

- Precios públicos.
- Promociones.
- Catálogo.
- Lanzamientos.
- Reputación.
- Cuota estimada.
- Noticias.
- Tendencias.

Los datos internos aparecerán como rangos y estimaciones. La precisión mejorará con investigación y analítica.

#### Mercados por categoría

Cada familia tendrá tamaño, crecimiento, demanda, oferta, saturación, precio medio, margen, tendencias y líderes.

La demanda total será finita:

```text
Población interesada
× poder adquisitivo
× tendencia
× estacionalidad
× crecimiento
```

La cuota dependerá de precio, disponibilidad, reputación, variedad, servicio, promoción, especialización y fidelidad.

#### Competencia comercial

Los rivales revisarán precios normalmente cada `3–7 días`, no de forma instantánea.

Podrán surgir:

- Guerras de precios.
- Escasez.
- Competencia por stock.
- Exclusividades.
- Envío gratuito.
- Campañas.
- Entrada en nuevas categorías.

El jugador podrá responder mediante precio, servicio, surtido, lotes, especialización o retirada temporal.

#### Reacción al jugador

Solo reaccionarán ante acciones relevantes:

- Ganar cuota.
- Entrar en un segmento.
- Lanzar una gran promoción.
- Publicar un éxito.
- Abrir una plataforma.
- Reducir comisiones.
- Captar estudios importantes.

Las respuestas serán diferidas y dependerán de información, personalidad, recursos y estrategia.

#### Memoria y planes

Los rivales recordarán guerras de precios, categorías, campañas, acuerdos, conflictos y resultados.

Podrán mantener planes de varias semanas, como abrir comercio online o entrar en publishing. El jugador verá indicios, no el plan completo.

#### Competencia por estudios y talento

Los estudios podrán recibir varias ofertas editoriales y valorarán financiación, participación, reputación, servicios, libertad y confianza.

Los empleados avanzados podrán recibir ofertas si están mal pagados, existe escasez o la reputación laboral es baja. Siempre habrá aviso y opciones de respuesta.

#### Lanzamientos y saturación

Los juegos competirán por atención, marketing, presupuesto y fechas. La saturación reducirá visibilidad, pero no convertirá una categoría en inviable automáticamente.

Calidad, identidad y buen posicionamiento podrán superar un mercado saturado.

#### Plataformas

Los usuarios y estudios podrán utilizar varias plataformas. Captar un usuario o un estudio no implicará exclusividad permanente.

La competencia dependerá de:

- Catálogo.
- Comisiones.
- Exclusivos futuros.
- Estabilidad.
- reputación.
- descubrimiento.
- soporte.
- regiones.

#### Relaciones y colaboración

Cada competidor visible tendrá relación `-100 a +100`:

- Hostil.
- Rival.
- Neutral.
- Cordial.
- Socio.

Posibles colaboraciones:

- Compra conjunta.
- Eventos.
- Distribución.
- Copublishing.
- Infraestructura.
- Bundles.

No habrá sabotaje, hackeo, robo ni acciones criminales.

#### Noticias e informes

El boletín mostrará promociones, acuerdos, lanzamientos, cierres, expansiones y cambios de comisión.

El informe semanal resumirá:

- Cuota.
- Precios medios.
- Promociones.
- Tendencias.
- Lanzamientos.
- Movimientos rivales.
- Oportunidades y amenazas.

#### Entradas, salidas y evolución

Podrán entrar `0–2 competidores relevantes por año de juego` cuando un mercado sea atractivo.

Las empresas podrán cerrar, fusionarse, abandonar líneas o transformarse después de problemas acumulados, nunca por una única mala semana.

Cada `30 días` revisarán su estrategia y podrán reforzarla, diversificar, especializarse o abandonar un segmento.

#### IA estratégica

La decisión evaluará:

```text
Beneficio esperado
+ ajuste estratégico
+ oportunidad
+ capacidad
+ presión competitiva
- coste
- riesgo
- deuda
- incompatibilidad con identidad
```

La personalidad y una variación controlada evitarán rivales idénticos o perfectamente racionales.

Los competidores podrán cometer errores: sobreexpansión, malos precios, proyectos débiles, lanzamientos prematuros, saturación o pérdida de reputación.

#### Justicia y protección contra bola de nieve

Un líder tendrá costes crecientes, ineficiencias, saturación, escrutinio y dificultad de adaptación.

Dominar un mercado atraerá nuevos rivales y presión, pero el jugador pequeño podrá competir mediante nicho, servicio, comunidad, eficiencia e innovación.

La dificultad modificará agresividad, velocidad de reacción y disciplina, sin conceder información perfecta ni recursos infinitos.

#### Mercado competitivo MVP

Incluirá:

- Una región.
- Cinco competidores visibles.
- Mercado agregado.
- Minoristas, un rival online, un publisher y una plataforma tardía.
- Arquetipos y personalidad.
- Cuota por segmento.
- Precios, promociones y stock abstracto.
- Campañas.
- Reacción diferida.
- Noticias.
- Tendencias y estacionalidad.
- Competencia por estudios, talento y lanzamientos.
- Relaciones.
- Informes.
- Guardado y carga.

Quedan pospuestos fusiones complejas, adquisiciones del jugador, múltiples regiones profundas, mercados financieros, regulación avanzada y simulación completa de cadenas rivales.

## 12.20. Progresión empresarial acumulativa

**Estado de alcance:** `VISIÓN ESTRUCTURAL`

> **Regla vigente:** Las seis etapas se conservan como mapa de producto. Solo la primera etapa y el cierre del vertical slice constituyen compromiso inmediato.

Se mantienen seis etapas acumulativas. Cada etapa avanzada exige:

1. La etapa anterior activa.
2. Nivel empresarial.
3. Reputación comercial.
4. Investigación correspondiente.
5. Inversión de construcción.
6. Un hito operativo que demuestre dominio de la etapa anterior.

No se podrá iniciar una expansión con deudas salariales vencidas. El juego advertirá si la inversión deja menos de siete días de reserva operativa.

#### Etapa 1 — Pequeña tienda

- Tienda inicial representativa autorada con una superficie aproximada de `10 × 15 m`.
- Capital inicial Estándar: `20.000 €`.
- El jugador trabaja solo.
- Catálogo y almacén iniciales.
- Sin empleados ni servicios avanzados.

Ritmo objetivo: `0–5 horas`.

#### Etapa 2 — Tienda ampliada y servicios iniciales

- Ampliación básica y almacén mayor.
- Contratación de empleados.
- Investigación.
- Puestos informáticos.
- Reservas y eventos iniciales cuando correspondan.

Ritmo objetivo: `5–12 horas`.

#### Etapa 3 — Comercio online y logística

Zona: `10 × 12` celdas, al oeste.

| Requisito | Valor |
|---|---:|
| Nivel empresarial | 4 |
| Reputación | 300 |
| Investigación | Comercio online I |
| Puntos | 60 |
| Coste de investigación | 10.000 € |
| Tiempo de investigación | 4 días |
| Construcción | 60.000 € |
| Tiempo de construcción | 5 días |
| Coste fijo | 160 €/día |

Hito operativo:

- Una semana con beneficio operativo positivo.
- Satisfacción media mínima de `60/100` durante esa semana.
- Sin salarios ni impuestos vencidos.

Ritmo objetivo: `12–22 horas`.

#### Etapa 4 — Publishing

Zona: `12 × 12` celdas, al nordeste.

| Requisito | Valor |
|---|---:|
| Nivel empresarial | 5 |
| Reputación | 600 |
| Investigación | Publishing I |
| Puntos | 120 |
| Coste de investigación | 30.000 € |
| Tiempo de investigación | 7 días |
| Construcción | 150.000 € |
| Tiempo de construcción | 7 días |
| Coste fijo | 250 €/día |

Hito operativo:

- `100` pedidos online acumulados.
- Cumplimiento puntual mínimo del `80 %`.
- Beneficio positivo en dos de las últimas tres semanas.
- Sin obligaciones salariales vencidas.

Ritmo objetivo: `22–35 horas`.

#### Etapa 5 — Desarrollo interno

Zona: `14 × 12` celdas, al noroeste.

| Requisito | Valor |
|---|---:|
| Nivel empresarial | 6 |
| Reputación | 1.000 |
| Investigación | Desarrollo interno I |
| Puntos | 180 |
| Coste de investigación | 75.000 € |
| Tiempo de investigación | 10 días |
| Construcción | 300.000 € |
| Tiempo de construcción | 10 días |
| Coste fijo | 400 €/día |

Hito operativo:

- Lanzar al menos un proyecto externo como publisher.
- Completarlo sin incumplir el contrato principal.
- Beneficio positivo en tres de las últimas cuatro semanas.
- Sin deuda vencida.

Ritmo objetivo: `35–50 horas`.

#### Etapa 6 — Plataforma digital e infraestructura

Zona técnica: `12 × 12` celdas al sur, no transitable o parcialmente técnica.

| Requisito | Valor |
|---|---:|
| Nivel empresarial | 8 |
| Reputación | 2.000 |
| Investigación | Plataforma e infraestructura I |
| Puntos | 300 |
| Coste de investigación | 200.000 € |
| Tiempo de investigación | 14 días |
| Construcción | 650.000 € |
| Tiempo de construcción | 14 días |
| Coste fijo inicial | 700 €/día |

Hito operativo:

- Haber publicado al menos un proyecto externo.
- Haber lanzado al menos un juego interno.
- Tres semanas rentables dentro de las últimas cuatro.
- Sin salarios ni impuestos vencidos.
- Reputación laboral superior a `30/100`.

El lanzamiento de la plataforma inicia el tramo hacia la condición principal de victoria, pero no la concede de inmediato.

Ritmo objetivo: `50–60 horas`.

#### Tabla consolidada

| Etapa | Nivel | Reputación | Investigación + construcción | Coste fijo | Tiempo objetivo |
|---|---:|---:|---:|---:|---:|
| 3. Comercio online | 4 | 300 | 70.000 € | 160 €/día | 12–22 h |
| 4. Publishing | 5 | 600 | 180.000 € | 250 €/día | 22–35 h |
| 5. Desarrollo interno | 6 | 1.000 | 375.000 € | 400 €/día | 35–50 h |
| 6. Plataforma | 8 | 2.000 | 850.000 € | 700 €/día iniciales | 50–60 h |

Solo podrá existir una construcción empresarial principal activa. La zona en obras permanecerá inaccesible y no generará costes diarios hasta quedar operativa. Finalizar la construcción no activará automáticamente el servicio: el jugador deberá equiparlo, asignar personal y abrirlo.

Los requisitos estructurales no cambiarán con la dificultad. Los costes sí aplicarán sus multiplicadores económicos. Una etapa construida no se bloqueará por pérdidas posteriores, aunque sus servicios podrán cerrarse temporalmente.

# 13. Reglas para incorporar nuevos sistemas

Antes de aprobar un sistema nuevo debe responderse:

1. ¿Qué pilar refuerza?
2. ¿Qué problema o fantasía resuelve?
3. ¿Qué representación tendrá en el mundo?
4. ¿Qué decisiones añade?
5. ¿Qué sistemas existentes reutiliza?
6. ¿Qué complejidad técnica y de contenido introduce?
7. ¿Puede probarse de forma determinista?
8. ¿Puede cerrarse sin depender de tres sistemas todavía inexistentes?
9. ¿Es necesario para el vertical slice o pertenece a una etapa posterior?
10. ¿Qué se elimina o retrasa para compensar su coste?

Una propuesta debe rechazarse o aplazarse cuando:

- solo añade volumen de contenido;
- duplica una capacidad existente;
- depende de excepciones opacas;
- no produce una consecuencia visible;
- sustituye un sistema anterior sin una razón de diseño;
- introduce automatización antes de que la tarea manual sea significativa;
- exige una arquitectura desproporcionada;
- impide cerrar el gate actual;
- existe únicamente porque “sería realista”;
- acerca el proyecto a un anti-pilar.

# 14. Reglas de cambio de alcance

## 14.1. Cambio menor

Puede aprobarse como cambio menor:

- ajustar un valor de balance;
- modificar una cantidad de contenido;
- sustituir un asset visual;
- mejorar feedback;
- corregir una regla ambigua sin cambiar la fantasía;
- reorganizar una tarea sin añadir una capacidad nueva.

## 14.2. Cambio mayor

Requiere revisión formal:

- añadir un sistema al vertical slice;
- adelantar un sistema diferido;
- cambiar la fantasía central;
- eliminar representación física;
- sustituir la tienda como núcleo;
- alterar la progresión acumulativa;
- cambiar el modelo de control;
- modificar la arquitectura inicial de forma que afecte a gameplay;
- convertir una hipótesis tardía en compromiso de producción;
- introducir monetización o servicios no contemplados;
- redefinir público, plataforma o tono.

## 14.3. Evidencia mínima

Un cambio mayor debe incluir:

- motivación;
- problema observado;
- alternativas consideradas;
- impacto en pilares;
- impacto en alcance;
- impacto técnico;
- impacto en producción;
- impacto documental;
- criterios de aceptación;
- decisión y responsable;
- fecha;
- documentos que deben actualizarse.

# 15. Criterio de calidad general

Una capacidad está cerrada cuando:

- compila sin errores;
- las invariantes relevantes tienen cobertura;
- la validación ocurre antes de la mutación;
- el recorrido manual reproduce el resultado esperado;
- el feedback permite comprender éxito o fallo;
- la integración no rompe capacidades existentes;
- no hay S0/S1 abiertos;
- los defectos aceptados están documentados;
- el build pasa cuando el gate lo exige;
- documentación, aceptación y trazabilidad están actualizadas;
- otra persona puede continuar sin depender de conocimiento tácito.

La calidad visual no debe evaluarse únicamente por belleza estática. Debe comprobarse:

- legibilidad;
- coherencia con cámara;
- claridad de zonas;
- correspondencia entre visual y lógica;
- ausencia de colisiones engañosas;
- consistencia de escala;
- estabilidad de referencias;
- coste de mantenimiento.

# 16. Diferenciación y defensa de identidad

> **Cartridge & Cloud** es un simulador de gestión 3D en el que el jugador trabaja físicamente en una pequeña tienda de videojuegos, organiza el espacio, recibe y repone mercancía, fija precios, atiende clientes y convierte el comercio local en un ecosistema que integra servicios para jugadores, comercio online, publishing, desarrollo propio, plataforma digital e infraestructura tecnológica.

El nombre **Cartridge & Cloud** se utilizará como nombre provisional oficial en documentos y materiales internos. Los cinco finalistas conservados para una validación posterior son:

1. Cartridge & Cloud.
2. Shelf to Server.
3. Game District.
4. Pixels & Profits.
5. Save Point Market.

Antes de aprobar el nombre definitivo deberán comprobarse coincidencias relevantes en Steam, marcas, redes y dominios.

La nueva dirección se diferencia porque:

- El jugador controla directamente un personaje.
- Existe un establecimiento físico jugable.
- Los clientes son agentes visibles.
- Los productos ocupan espacio real.
- El inventario debe recibirse, almacenarse y colocarse.
- El negocio comienza en la venta, no en el desarrollo.
- El publishing aparece como expansión.
- Crear videojuegos no es la única fuente de progreso.
- La evolución amplía el modelo de negocio sin sustituir etapas anteriores.
- La tienda física y la plataforma digital forman parte de la misma trayectoria.
- La distribución espacial importa.
- La observación de los clientes aporta información del mercado.
- Los sistemas avanzados tienen presencia física o visual dentro del mundo.

## 16.1. Defensa adicional frente a la deriva

La tienda, los clientes, el inventario físico, la colocación y la operación directa son la defensa principal contra la deriva hacia un tycoon de desarrollo convencional. Publishing y desarrollo interno solo son válidos si continúan alimentados por el mercado, la reputación, la logística, el espacio y el conocimiento construidos desde la tienda.

# 17. Riesgos de producto y protecciones

| Riesgo | Protección |
|---|---|
| Crecimiento descontrolado | Gates de alcance y sistemas diferidos |
| Menús sustituyen al mundo | Prioridad de representación física |
| Tienda decorativa | Invariantes de stock, circulación y capacidad |
| Automatización temprana | Tareas manuales primero; empleados después |
| Arquitectura frágil | Autoría explícita, IDs estables y referencias |
| Balance injusto | Valores configurables, playtests y protecciones |
| Clientes opacos | Estados observables y feedback de causa |
| Contenido caro sin valor | Contenido mínimo representativo |
| Documentación contradictoria | Jerarquía de autoridad y control de cambios |
| Visión tardía consume el presente | Distinción entre núcleo, diferido y visión |
| Rehacer escenas por assets | Separación entre verdad lógica y representación |
| Regresión silenciosa | TestLab, suites y validación integrada |

# 18. Resumen normativo

1. La tienda física es el núcleo.
2. El jugador opera directamente el negocio.
3. El espacio importa funcionalmente.
4. La arquitectura inicial es autorada de forma explícita.
5. El runtime opera el entorno; no lo deduce de assets accidentales.
6. El grid lógico y la ocupación son fuentes de verdad para colocación.
7. Los clientes deben ser observables y comprensibles.
8. El stock debe existir en ubicaciones explícitas.
9. Las ventas y mutaciones deben ser atómicas.
10. La progresión es gradual y acumulativa.
11. Los sistemas avanzados permanecen diferidos hasta cerrar el núcleo.
12. La visión tardía no es backlog inmediato.
13. Las cifras heredadas son configurables salvo que se aprueben expresamente.
14. La calidad exige pruebas, integración, documentación, trazabilidad y build.
15. Cualquier cambio estructural debe justificarse contra los pilares.

# Anexo A. Anti-pilares heredados

El juego no debería convertirse en:

- Un clon de Game Dev Tycoon.
- Un simulador basado únicamente en menús.
- Un simulador hiperrealista de mantenimiento.
- Un juego de limpieza constante.
- Un simulador técnico de reparación de ordenadores.
- Un juego de supervivencia económica injusto.
- Un sistema donde todo se automatiza demasiado pronto.
- Un simulador de cibercafé independiente.
- Un constructor de oficinas sin interacción directa.
- Una colección de minijuegos desconectados.
- Un juego donde la única meta sea producir títulos AAA.
- Un juego donde las expansiones sustituyen o inutilizan los sistemas anteriores.

# Anexo B. Dirección visual heredada

La identidad aprobada es:

- Mundo 3D low poly.
- Texturas handpainted/cartoon.
- Shaders con bordes tipo tinta.
- Mobiliario modular.
- Iluminación cálida.
- Productos coloridos.
- UI técnica y limpia.
- Evolución visual del complejo.

#### Valores visuales provisionales

##### Paleta de interfaz

| Uso | Color provisional |
|---|---|
| Fondo principal | `#0D1110` |
| Superficie | `#19201D` |
| Superficie elevada | `#242D29` |
| Verde principal | `#35D07F` |
| Verde secundario | `#1F8F5F` |
| Texto principal | `#E8EFEA` |
| Texto secundario | `#AAB7B0` |
| Advertencia | `#E8B44A` |
| Error | `#D95D5D` |

##### Mundo

- Temperatura de iluminación interior cálida.
- Sombras suaves.
- Bordes oscuros moderados.
- Personajes estilizados y legibles desde cámara elevada.
- Siluetas diferenciadas.
- Materiales simples con variación pintada.
- Nivel de detalle medio-bajo para mantener claridad.

Estos valores son provisionales.

#### Identidad narrativa y atmosférica

- Tono profesional y cercano.
- Humor ligero basado en situaciones y personalidades.
- Componente nostálgico más visible durante la etapa de tienda física.
- Evolución gradual hacia una imagen empresarial y tecnológica.
- Coherencia entre la estética de cartuchos, cajas y estanterías y la posterior evolución hacia nube, plataforma e infraestructura.

# Anexo C. Trazabilidad de consolidación

| Fuente | SHA-256 del texto leído | Uso principal |
|---|---|---|
| `Enfoque_v0.8.md` | `7723b3a72d7b63ace8f1401c9eb2ed250a671e6a49e867f134583c4572ce524c` | Autoridad actual, filosofía técnica, estado y autoría explícita |
| `Enfoque_v0.7.md` | `dff2fd6d5ae6a688ae147c6eebc2e81b5667cf2433198204867ed51559cc11ea` | Fantasía condensada, límites del slice y fundación implementada |
| `Cartridge_And_Cloud_Enfoque_v0.6_PDF_Source.md` | `8e6ba5102d28b0aa36d1306b531407587510850d2a7ca50691389d53ce938548` | Contexto editorial y correcciones técnicas |
| `Enfoque_v0.6.md` | `6765207677c2adad0d124d20b9c27c82563eebc5eb3a0acb817dddbd148b3fbb` | Detalle conceptual, sistemas, progresión y parámetros iniciales |

## C.1. Principales decisiones de consolidación

- Se adopta la autoría explícita del espacio de v0.8.
- Se adopta el modelo técnico determinista y de mutación atómica de v0.8.
- Se conserva el límite del vertical slice expresado en v0.7.
- Se conserva la fantasía comercial y empresarial extensa de v0.6.
- Se reclasifican los sistemas avanzados como diferidos o visión tardía.
- Se sustituye la tienda conceptual de `10 × 10` celdas por la tienda inicial autorada de aproximadamente `10 × 15 m`.
- Se separa estado técnico fechado de visión estable.
- Se conservan cifras económicas como hipótesis configurables.
- Se evita interpretar la amplitud documental como una orden de implementación inmediata.

# Anexo D. Glosario de alcance

| Término | Definición |
|---|---|
| **Autoría explícita** | Definición intencional de arquitectura, referencias y zonas funcionales, sin inferencia frágil. |
| **Bucle base** | Preparar, abastecer, abrir, vender, cerrar, analizar y persistir. |
| **Capacidad** | Resultado observable y verificable que el jugador o el sistema puede producir. |
| **Contenido representativo** | Assets suficientes para demostrar calidad y lectura, sin alcanzar volumen final. |
| **Diferido** | Diseñado conceptualmente, pero bloqueado hasta superar un gate. |
| **Gate** | Conjunto de criterios obligatorios para avanzar de fase. |
| **Hipótesis configurable** | Valor inicial que debe vivir en datos y puede cambiar por balance. |
| **Invariante** | Regla que siempre debe cumplirse durante cualquier transición válida. |
| **Mutación atómica** | Cambio que se completa entero o no se aplica. |
| **Núcleo** | Conjunto mínimo de sistemas que demuestra la fantasía principal. |
| **Representación física** | Manifestación espacial o visual de una regla o recurso. |
| **Vertical slice** | Recorrido reducido pero integrado que demuestra experiencia, arquitectura y calidad objetivo. |
| **Visión tardía** | Dirección de futuro que orienta compatibilidad, pero no compromete producción actual. |

---

**Estado del documento:** fuente vigente de enfoque y alcance para la nueva carpeta `Documentacion/`. Cualquier sustitución deberá registrar explícitamente la versión, la motivación y las decisiones modificadas.

---

<!-- W0_S17_PHASE1_START -->

# Actualización de alcance W0 - Sprint17_Phase1

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

## Límites de alcance confirmados

- H6 usa un día de cinco minutos configurables y cuatro velocidades visibles; la pausa no es una quinta velocidad.
- El alcance H6 mantiene displays monoproducto, máximo de ocho clientes activos, historial detallado limitado y tributación semanal técnica.
- Guardado exacto a mitad de día, displays multiproducto y costes fijos recurrentes quedan Post-H6.
- La remediación no incorpora empleados, investigación, comercio online, publishing ni sistemas empresariales tardíos.
- La Vertical Slice solo podrá considerarse aceptada después de implementación, pruebas, build y signoff; la coherencia documental por sí sola no la cierra.

## Regla de cierre y no propagación

- El cierre de W0 no abre W1-W7 automáticamente; cada ola requiere su propia evidencia y control de cambios.
- No se declara Sprint17_Phase1 completada mientras W8 no haya ejecutado la regresión integral.
- No se propaga `PASS` de Sprint 16 a H6 ni a la Vertical Slice.
- No se introducen sistemas Post-H6 durante la remediación.
- Código, escenas, prefabs, builds y tests ejecutables no han sido modificados por esta actualización documental.

<!-- W0_S17_PHASE1_END -->
