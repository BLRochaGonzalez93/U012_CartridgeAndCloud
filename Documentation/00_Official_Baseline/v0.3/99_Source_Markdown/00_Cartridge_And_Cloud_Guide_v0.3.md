---
title: "Cartridge & Cloud - Guía Maestra"
subtitle: "Producción, diseño, tecnología, QA y gestión documental desde preproducción"
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

# Resumen ejecutivo

**Cartridge & Cloud** es un simulador de gestión y evolución empresarial en el que el jugador comienza operando físicamente una pequeña tienda de videojuegos y tecnología. La experiencia crece desde la organización del local, inventario, clientes, empleados y logística hasta el comercio online, publishing, desarrollo interno, plataforma digital e infraestructura tecnológica.

El diseño busca diferenciarse de los tycoons clásicos de desarrollo de videojuegos mediante una progresión física y observable. El desarrollo de juegos no es el único bucle ni el punto de partida: aparece como una expansión empresarial avanzada apoyada por datos, comunidad, distribución y experiencia comercial acumulada.

## Premisas no negociables

- PC/Steam como plataforma inicial.
- Desarrollo viable para una persona, con producción modular y alcance controlado.
- Vista 3D estilizada con personaje controlable, construcción sobre cuadrícula y paneles de gestión.
- El primer objetivo de desarrollo será un vertical slice de la tienda física.
- Steam, demo pública, Steam Cloud, logros, plataforma propia y servicios futuros no se presentarán como implementados antes de existir.
- Todos los sistemas deben ser comprensibles, persistentes, testeables y ampliables sin rehacer el núcleo.

# 1. Propósito de la guía

Esta guía es el punto de entrada del proyecto. Define cómo leer la documentación, cómo convertir decisiones conceptuales en trabajo ejecutable y cómo evitar que el alcance avanzado desplace la validación del núcleo.

# 2. Estado oficial del proyecto

| Área | Estado oficial |
|---|---|
| Concepto | Cerrado mediante 26 decisiones consolidadas |
| Código | No iniciado a efectos de planificación |
| Proyecto Unity | Pendiente de creación/configuración formal |
| Vertical slice | Especificado, no implementado |
| Sprints | Reiniciados desde Sprint 0 |
| Identidad visual | Dirección aprobada; producción de assets pendiente |
| Steam | Planificado; no iniciado |
| Nombre comercial | Provisional, pendiente de validación legal y comercial |

# 3. Jerarquía documental

1. `Enfoque_v0.6.md`: fuente conceptual completa.
2. GDD: reglas de producto y gameplay.
3. Vertical Slice Specification: contrato del primer hito jugable.
4. TDD y Modelo de Datos: arquitectura prevista y persistencia.
5. UX/UI/Art/Audio: experiencia y presentación.
6. Roadmap, Excel Maestro y QA: ejecución y verificación.
7. Marketing, Steam, Legal y Localización: preparación comercial.
8. Project Binder: índice de versiones vigentes.

# 4. Principios de producción

- Primero demostrar el bucle físico de tienda; después expandir.
- Una feature no entra en sprint sin criterios de aceptación.
- La UI comunica; la simulación decide.
- El guardado forma parte de cada sistema, no de una fase final.
- Los datos configurables no deben quedar hardcodeados.
- Cada sprint termina con QA, documentación y trazabilidad.
- Los placeholders se identifican y nunca se confunden con arte final.

# 5. Fases maestras

| Fase | Resultado esperado |
|---|---|
| P0 - Preproducción | Documentación, alcance, riesgos y setup aprobados |
| P1 - Prototipo técnico | Movimiento, cámara, grid, interacción e inventario básicos |
| P2 - Prototipo jugable | Pedido, recepción, exposición, cliente, cola y venta |
| P3 - Vertical Slice | Una semana jugable estable, presentable y persistente |
| P4 - MVP tienda | Empleados, investigación, ordenadores y progresión inicial |
| P5 - Expansión | Online, publishing, desarrollo interno y mercado |
| P6 - Plataforma | Plataforma digital, infraestructura y condición de victoria |
| P7 - Alpha/Beta/RC | Contenido, balance, rendimiento, localización y publicación |

# 6. Gobernanza y control de cambios

Toda modificación importante debe registrar: decisión, documento afectado, tipo de cambio, impacto, validación y versión. La trazabilidad utiliza únicamente el catálogo cerrado aprobado.

# 7. Criterio para iniciar desarrollo

El desarrollo puede comenzar cuando estén aprobados: GDD, TDD preliminar, Modelo de Datos, Vertical Slice Specification, UX Flow, roadmap inicial, QA Matrix y setup de Unity. Los sistemas posteriores al vertical slice permanecerán documentados, pero no bloquearán el Sprint 0.

# 8. Dirección conceptual consolidada

## 2. Nueva fantasía principal del jugador

El jugador comienza como propietario y trabajador de una pequeña tienda de videojuegos.

Durante el horario comercial debe gestionar físicamente el establecimiento:

- Abrir la tienda.
- Recibir clientes.
- Reponer productos.
- Organizar estanterías y vitrinas.
- Atender el mostrador.
- Cobrar compras y servicios.
- Recibir pedidos.
- Gestionar el almacén.
- Colocar mobiliario.
- Ampliar el local.
- Contratar empleados.
- Observar las preferencias y comportamientos de los clientes.

Fuera del horario comercial puede realizar tareas estratégicas:

- Comprar inventario.
- Elegir qué productos vender.
- Ajustar precios.
- Contratar personal.
- Comprar mobiliario.
- Ampliar el establecimiento.
- Desbloquear nuevas líneas de negocio.
- Evaluar proyectos de otros desarrolladores.
- Publicar videojuegos.
- Desarrollar productos propios.
- Crear servicios digitales.

La fantasía de progreso es:

> Empezar atendiendo personalmente una pequeña tienda de videojuegos y terminar dirigiendo una empresa que combina comercio físico, servicios para jugadores, distribución, publishing, desarrollo propio y una plataforma digital, manteniendo activas las etapas previas como partes del mismo ecosistema.

> **Estado del apartado: COMPLETO.**  
> La fantasía general está definida. Sus detalles se concretarán mediante los sistemas y etapas posteriores.

---

## 3. Dirección general del proyecto

### Concepto principal

```text
Tienda de videojuegos
→ ampliaciones y servicios
→ comercio online
→ distribución
→ publishing
→ desarrollo interno
→ plataforma digital
→ servicios online y servidores
```

La progresión es **aditiva**. Cada nueva etapa amplía el negocio sin eliminar ni reemplazar las anteriores.

El jugador no comienza como desarrollador de videojuegos. El desarrollo aparece progresivamente como una nueva actividad empresarial.

### Modelo físico de expansión

- Existirá una única tienda principal sometida a ampliaciones modulares.
- Las líneas de negocio que necesiten espacios independientes aparecerán como edificios colindantes.
- Los edificios estarán conectados mediante puertas y formarán un único complejo empresarial.
- Cada edificio o zona tendrá una ubicación definida dentro del mapa global desde el inicio.
- La zona no será visible, accesible ni funcional hasta ser desbloqueada y construida.
- Las ampliaciones conservarán la actividad de la tienda inicial.

### Distribución provisional del complejo

Las siguientes huellas funcionan como base inicial editable:

| Zona | Huella provisional | Posición relativa | Etapa |
|---|---:|---|---:|
| Tienda inicial | `10 × 10` | Centro | 1 |
| Almacén ampliado | `8 × 10` | Norte | 2 |
| Zona de puestos informáticos | `12 × 10` | Este | 2 |
| Área logística y pedidos online | `10 × 12` | Oeste | 3 |
| Oficina de publishing | `12 × 12` | Nordeste | 4 |
| Estudio de desarrollo interno | `14 × 12` | Noroeste | 5 |
| Infraestructura de servidores | `12 × 12` | Sur, no transitable | 6 |

La disposición podrá ajustarse durante el prototipado, pero seguirá estas reglas:

- Las zonas forman un complejo continuo.
- Todas se conectan mediante accesos visibles o pasos técnicos.
- Ninguna expansión elimina las anteriores.
- Las ubicaciones se reservan desde el comienzo para impedir construcciones incompatibles.
- Los edificios avanzados pueden mostrarse exteriormente sin disponer de interior jugable.

### Requisitos provisionales de expansión

| Categoría | Nivel mínimo | Reputación | Coste orientativo |
|---|---:|---:|---:|
| Ampliación básica | 2 | 50 | 5.000–15.000 € |
| Servicio especializado | 3 | 150 | 15.000–35.000 € |
| Logística y comercio online | 4 | 300 | 40.000–80.000 € |
| Publishing | 5 | 600 | 100.000–200.000 € |
| Desarrollo interno | 6 | 1.000 | 200.000–400.000 € |
| Plataforma e infraestructura | 8 | 2.000 | 500.000 € o más |

Los valores se configurarán mediante datos y se balancearán con la economía real.

### Conocimiento del mercado

La tienda física es el origen del conocimiento comercial:

- Productos y géneros más demandados.
- Plataformas utilizadas.
- Sensibilidad al precio.
- Reservas.
- Tendencias.
- Públicos habituales.
- Servicios más demandados.
- Productos con poca rotación.

Ese conocimiento podrá utilizarse para decidir:

- Qué juegos publicar.
- Qué estudios financiar.
- Qué juegos desarrollar internamente.
- Qué productos promocionar.
- Qué líneas de negocio ampliar.
- Qué servicios incorporar a la plataforma digital.

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> El modelo de complejo empresarial, la distribución inicial y los requisitos orientativos están definidos. Las cifras se balancearán durante el desarrollo y no requieren una nueva decisión conceptual.

## 4. Tipo de juego

### Género principal

Simulador de gestión con control directo del personaje.

### Presentación prevista

- Mundo 3D.
- Estilo low poly.
- Texturas handpainted/cartoon.
- Shaders con bordes sombreados tipo tinta.
- Cámara fijada sobre el jugador.
- Perspectiva intermedia entre top-down e isométrica cenital.
- Movimiento libre y fluido.
- Cuadrícula lógica para construcción y colocación.
- Interacción directa con muebles, estanterías, vitrinas, cajas, puertas, puestos y clientes.
- Construcción modular.
- Interfaz complementaria para economía, pedidos, investigación y administración.

### Movimiento y navegación

El jugador se desplazará indicando un destino mediante clic izquierdo sobre una superficie transitable.

Flujo previsto:

```text
Clic sobre el suelo
→ Raycast obtiene el punto de destino
→ el sistema valida la superficie
→ se calcula una ruta
→ el personaje camina hasta el destino
```

La navegación utilizará **Unity AI Navigation / NavMesh** como solución inicial:

- `NavMeshSurface` para las superficies transitables.
- `NavMeshAgent` para jugador, clientes y empleados.
- `NavMeshObstacle` con carving para obstáculos colocables cuando resulte adecuado.
- Actualización de la navegación tras cambios estructurales de mobiliario.
- Fallback controlado si el destino no es alcanzable.

La velocidad base se sincronizará con la animación de caminar. Su valor numérico se balanceará posteriormente sin modificar el modelo de control.

### Control de cámara

- La cámara mantiene una inclinación y distancia base respecto al jugador.
- Manteniendo pulsado el botón derecho del ratón y arrastrando horizontalmente, rota 360 grados alrededor del personaje.
- La inclinación se mantiene durante la rotación.
- La rueda del ratón controla el zoom.
- Distancia mínima provisional: `5 m`.
- Distancia máxima provisional: `18 m`.
- Distancia inicial provisional: `10 m`.
- Los controles alternativos y las opciones de accesibilidad se implementarán en una fase posterior.

### Principio de representación física

La mayor parte de la gestión debe tener una representación física:

- Productos visibles.
- Pedidos que llegan físicamente.
- Clientes que recorren la tienda.
- Colas en el mostrador.
- Empleados que ejecutan tareas.
- Puestos informáticos que ocupan espacio.
- Almacenes con capacidad limitada.
- Distribución del local con consecuencias funcionales.

La interfaz se utilizará para información y planificación, no como único espacio jugable.

### Público objetivo

El público principal será:

- Jugadores de simuladores de gestión.
- Jugadores de simuladores de tiendas.
- Público de juegos tycoon y progresión empresarial.
- Jugadores interesados en construcción, distribución y optimización de espacios.
- Personas que buscan decisiones económicas y crecimiento a medio y largo plazo.

El público secundario incluye aficionados a la industria del videojuego, al comercio especializado, a la personalización de establecimientos y a experiencias tranquilas con profundidad sistémica.

El proyecto no se plantea como un juego casual de partidas rápidas. La experiencia está diseñada para desarrollarse durante múltiples sesiones.

### Duración objetivo

- Sesión habitual: `30–90 minutos`.
- Acceso aproximado al late game: `35–60 horas`.
- Acceso objetivo a plataforma e infraestructura: alrededor de `50–60 horas`.
- Continuación posterior: sin límite obligatorio.

Estas cifras son objetivos de ritmo y deberán validarse mediante pruebas de juego.

### Tono narrativo

El tono aprobado será **profesional y cercano, con humor ligero y un componente nostálgico durante las primeras etapas**.

- Los sistemas económicos y administrativos se explicarán con claridad.
- El humor surgirá de situaciones, clientes, proveedores y productos ficticios, no de una parodia constante.
- Las primeras etapas reforzarán la cercanía de una tienda local, los lanzamientos físicos, las cajas, los cartuchos y la cultura del comercio especializado.
- Las etapas avanzadas adoptarán un tono más empresarial sin perder humanidad.
- No se dependerá de referencias directas a empresas reales ni de bromas que envejezcan rápidamente.

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> El género, la presentación, el control, el público objetivo, la duración y el tono narrativo están definidos. Los valores exactos de ritmo, accesibilidad y movimiento se balancearán posteriormente.

## 5. Bucle jugable diario

### Escala temporal

- Un día completo a velocidad `×1` dura una hora real.
- El jugador puede reducir la velocidad hasta `×0,5`.
- El jugador puede acelerarla hasta `×4`.
- El modificador afecta al paso del tiempo y a los temporizadores de simulación.
- La velocidad visual de movimiento del jugador, clientes y empleados no se multiplica.
- Los sistemas basados en duración deberán convertir correctamente el tiempo de simulación en tiempo real.

### Antes de abrir

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

### Durante el horario comercial

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

### Resolución de clientes al cerrar

Al iniciar el cierre:

- Se bloquea el spawn de nuevos clientes.
- Las compras no pagadas se cancelan.
- Los productos reservados vuelven automáticamente al estado `Available`.
- Los clientes que solo compraban abandonan la tienda sin pagar.
- Los clientes que usan un puesto informático interrumpen el uso, liberan el puesto y pagan automáticamente el tiempo realmente consumido.
- Si un cliente tenía productos y servicio de ordenador, solo se cobra el servicio consumido; los productos se liberan.
- Las colas se disuelven.
- Todos los clientes se dirigen a la salida y ejecutan `Despawn`.

### Después del cierre

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

### Resumen diario

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

### Condición principal de victoria y continuación

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

### Sistema de eventos de tienda

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

### Bucle resumido

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

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> La escala temporal, la apertura, el cierre y la resolución de clientes están definidos. Los detalles numéricos de balance se validarán durante el prototipo.

## 24. Progresión empresarial consolidada

Se mantienen seis etapas acumulativas. Cada etapa avanzada exige:

1. La etapa anterior activa.
2. Nivel empresarial.
3. Reputación comercial.
4. Investigación correspondiente.
5. Inversión de construcción.
6. Un hito operativo que demuestre dominio de la etapa anterior.

No se podrá iniciar una expansión con deudas salariales vencidas. El juego advertirá si la inversión deja menos de siete días de reserva operativa.

### Etapa 1 — Pequeña tienda

- Tienda `10 × 10`.
- Capital inicial Estándar: `20.000 €`.
- El jugador trabaja solo.
- Catálogo y almacén iniciales.
- Sin empleados ni servicios avanzados.

Ritmo objetivo: `0–5 horas`.

### Etapa 2 — Tienda ampliada y servicios iniciales

- Ampliación básica y almacén mayor.
- Contratación de empleados.
- Investigación.
- Puestos informáticos.
- Reservas y eventos iniciales cuando correspondan.

Ritmo objetivo: `5–12 horas`.

### Etapa 3 — Comercio online y logística

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

### Etapa 4 — Publishing

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

### Etapa 5 — Desarrollo interno

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

### Etapa 6 — Plataforma digital e infraestructura

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

### Tabla consolidada

| Etapa | Nivel | Reputación | Investigación + construcción | Coste fijo | Tiempo objetivo |
|---|---:|---:|---:|---:|---:|
| 3. Comercio online | 4 | 300 | 70.000 € | 160 €/día | 12–22 h |
| 4. Publishing | 5 | 600 | 180.000 € | 250 €/día | 22–35 h |
| 5. Desarrollo interno | 6 | 1.000 | 375.000 € | 400 €/día | 35–50 h |
| 6. Plataforma | 8 | 2.000 | 850.000 € | 700 €/día iniciales | 50–60 h |

Solo podrá existir una construcción empresarial principal activa. La zona en obras permanecerá inaccesible y no generará costes diarios hasta quedar operativa. Finalizar la construcción no activará automáticamente el servicio: el jugador deberá equiparlo, asignar personal y abrirlo.

Los requisitos estructurales no cambiarán con la dificultad. Los costes sí aplicarán sus multiplicadores económicos. Una etapa construida no se bloqueará por pérdidas posteriores, aunque sus servicios podrán cerrarse temporalmente.

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> Las seis etapas, los requisitos de acceso, los hitos operativos y el ritmo de las etapas 3–6 están definidos. El balance final dependerá de pruebas de progresión.

## 25. Pilares provisionales del diseño

### Pilar 1 — Gestión física del negocio

La tienda debe sentirse como un espacio real que el jugador organiza y recorre.

### Pilar 2 — Progresión visible

Cada mejora debe cambiar físicamente el establecimiento o ampliar lo que el negocio puede hacer.

### Pilar 3 — Clientes observables

El mercado no se representa únicamente mediante estadísticas. Los clientes caminan, esperan, compran, utilizan servicios y abandonan la tienda.

### Pilar 4 — Crecimiento acumulativo

La progresión transforma el negocio sin sustituir el modelo anterior.

Cada nueva zona o sistema amplía lo previamente construido.

Las ubicaciones generales de futuras zonas deberán estar definidas desde el inicio de la partida, aunque permanezcan ocultas, inaccesibles o sin construir hasta su desbloqueo.

Al desbloquear y construir una zona:

- Pasa a ser visible.
- Se integra en el mundo.
- Mantiene la continuidad espacial del negocio.
- No elimina las zonas previas.

### Pilar 5 — Desarrollo de videojuegos como expansión

Crear o publicar videojuegos es una consecuencia del crecimiento del negocio, no la actividad inicial.

### Pilar 6 — Sistemas comprensibles

Las reglas deben ser fáciles de entender:

- El peor componente limita el tier.
- La tarifa depende del tier.
- Los clientes pagan según el tiempo.
- El stock físico limita las ventas.
- Las colas afectan al servicio.
- El espacio condiciona el crecimiento.

> **Estado del apartado: COMPLETO.**  
> Los pilares principales están definidos y sirven como filtro para las futuras decisiones de diseño.

---

## 26. Anti-pilares provisionales

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

> **Estado del apartado: COMPLETO.**  
> Los límites conceptuales están suficientemente definidos. Podrán ampliarse si aparecen nuevos riesgos de diseño.

---

## 29. Identidad visual

La identidad aprobada es:

- Mundo 3D low poly.
- Texturas handpainted/cartoon.
- Shaders con bordes tipo tinta.
- Mobiliario modular.
- Iluminación cálida.
- Productos coloridos.
- UI técnica y limpia.
- Evolución visual del complejo.

### Valores visuales provisionales

#### Paleta de interfaz

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

#### Mundo

- Temperatura de iluminación interior cálida.
- Sombras suaves.
- Bordes oscuros moderados.
- Personajes estilizados y legibles desde cámara elevada.
- Siluetas diferenciadas.
- Materiales simples con variación pintada.
- Nivel de detalle medio-bajo para mantener claridad.

Estos valores son provisionales.

### Identidad narrativa y atmosférica

- Tono profesional y cercano.
- Humor ligero basado en situaciones y personalidades.
- Componente nostálgico más visible durante la etapa de tienda física.
- Evolución gradual hacia una imagen empresarial y tecnológica.
- Coherencia entre la estética de cartuchos, cajas y estanterías y la posterior evolución hacia nube, plataforma e infraestructura.

> **Estado del apartado: PARCIALMENTE DEFINIDO.**  
> La dirección visual, cromática y narrativa está aprobada. Las referencias, personajes, materiales, iluminación definitiva y guía de arte se crearán durante la preproducción visual.

## 30. Propuesta de valor consolidada

> **Cartridge & Cloud** es un simulador de gestión 3D en el que el jugador trabaja físicamente en una pequeña tienda de videojuegos, organiza el espacio, recibe y repone mercancía, fija precios, atiende clientes y convierte el comercio local en un ecosistema que integra servicios para jugadores, comercio online, publishing, desarrollo propio, plataforma digital e infraestructura tecnológica.

El nombre **Cartridge & Cloud** se utilizará como nombre provisional oficial en documentos y materiales internos. Los cinco finalistas conservados para una validación posterior son:

1. Cartridge & Cloud.
2. Shelf to Server.
3. Game District.
4. Pixels & Profits.
5. Save Point Market.

Antes de aprobar el nombre definitivo deberán comprobarse coincidencias relevantes en Steam, marcas, redes y dominios.

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> El nombre de trabajo, la propuesta de valor, el tono y el público están definidos. Solo queda la validación comercial y legal del nombre antes de considerarlo definitivo.

## 31. Diferenciación respecto a Game Dev Tycoon

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

> **Estado del apartado: COMPLETO.**  
> La diferenciación conceptual principal está suficientemente definida.

---

## 33. Dirección consolidada actual

> **Cartridge & Cloud** es un simulador de gestión 3D para PC/Steam con control directo mediante clic, cámara orbital entre top-down e isométrica cenital y estética low poly handpainted/cartoon con bordes tipo tinta. El jugador comienza con `20.000 €`, un local alquilado y vacío y la responsabilidad de construir y operar físicamente una pequeña tienda de videojuegos. Recibe mercancía, gestiona almacén y exposición, fija precios, atiende clientes y controla una economía con costes diarios, impuestos semanales y márgenes diferenciados. La empresa crece de forma acumulativa hacia empleados, servicios informáticos, eventos, comercio online, publishing, desarrollo interno, plataforma digital e infraestructura. La experiencia está orientada a jugadores de simuladores de tiendas y tycoons, utiliza un tono profesional y cercano con humor ligero y nostalgia inicial, persigue una duración de decenas de horas y culmina en un final principal con continuación libre y varios hitos empresariales secundarios.

Principios consolidados:

- Nombre provisional oficial: **Cartridge & Cloud**.
- Público: simulación de gestión, tiendas y tycoon.
- Duración objetivo hasta late game: `35–60 horas`.
- Capital estándar: `20.000 €`.
- Margen bruto medio objetivo: `35 %`.
- Tres dificultades y Sandbox personalizable.
- Empleados con contratación, progresión, cansancio, salario, despido y automatización.
- Eventos dinámicos, programados e hitos.
- Seis etapas empresariales acumulativas.
- Victoria principal ligada a la plataforma, seguida de postgame abierto.
- Primer vertical slice limitado a la tienda física sin empleados ni sistemas avanzados.

> **Estado del apartado: COMPLETO.**  
> Esta es la visión consolidada vigente después de aprobar las decisiones 1–26. Solo deberá modificarse si cambia la fantasía principal, el modelo de progresión o los resultados de validación revelan un problema estructural.

## 34. Próximo objetivo conceptual y de preproducción

Las decisiones conceptuales 1–26 están cerradas. El primer vertical slice queda aprobado formalmente como objetivo de validación de la nueva dirección.

### Objetivo del vertical slice

Demostrar que la fantasía principal de la tienda física funciona antes de desarrollar empleados, comercio online, publishing, desarrollo interno o plataforma.

Debe validar:

1. Control y cámara.
2. Construcción física del local.
3. Pedido, recepción, almacén y reposición.
4. Clientes, selección de productos, colas y cobro.
5. Precios, stock, gastos y ventas.
6. Apertura, cierre y progresión diaria.
7. Guardado y carga íntegros.

### Alcance aprobado

#### Contexto inicial

- Tienda `10 × 10` dentro del mapa lógico de `100 × 100`.
- Capital: `20.000 €`.
- Local alquilado y vacío.
- Mobiliario Tier E.
- Un proveedor general.
- Sin empleados.
- El jugador realiza manualmente todas las tareas.

#### Sistemas incluidos

- Movimiento por clic y NavMesh.
- Cámara orbital de 360° y zoom `5–18 m`.
- Construcción sobre cuadrícula y rotación de 90°.
- Mostrador, estantería, vitrina, almacén y recepción.
- Catálogo mínimo de 12 productos y 6 familias.
- Proveedor, pedidos, transporte y entrega.
- Recepción, cajas, almacén, asignación y reposición.
- Precios entre 50–150 % del recomendado.
- Cuatro arquetipos de cliente.
- Máximo inicial de 8 clientes simultáneos.
- Cola y cobro manual por el jugador.
- Apertura desde las 08:00, cierre manual desde las 20:00 y obligatorio a las 22:00.
- Economía diaria y semanal.
- Resumen diario y semanal.
- Impuesto al séptimo día.
- Guardado manual con tienda cerrada.
- Autoguardado tras el resumen.
- Tres slots.
- Localización ES/EN.

#### Contenido mínimo

| Elemento | Mínimo |
|---|---:|
| Escenario | 1 |
| Tienda | 1 |
| Proveedor | 1 |
| Productos | 12 |
| Familias | 6 |
| Arquetipos de cliente | 4 |
| Muebles funcionales | 5 |
| Días simulables | 7 o más |
| Velocidades temporales | 5 |
| Clientes simultáneos | 8 |
| Idiomas | ES/EN |
| Slots | 3 |

#### Fuera del alcance

- Empleados.
- Investigación funcional.
- Ampliaciones del complejo.
- Puestos informáticos.
- Reservas.
- Eventos de tienda.
- Comercio online.
- Publishing.
- Desarrollo interno.
- Plataforma y servidores.
- Usados y retro.
- Devoluciones y robos.
- Competidores.
- Marketing avanzado.
- Steam, logros, Steam Cloud o demo pública.
- Arte y audio finales.

### Criterios de aceptación

El slice debe permitir completar al menos `7 días` y cumplir:

- El jugador llega a cualquier punto accesible sin atravesar obstáculos.
- La cámara no interfiere con UI ni colocación.
- No se puede solapar mobiliario ni bloquear accesos obligatorios.
- Los pedidos llegan una sola vez y con las cantidades correctas.
- El stock cumple siempre:

```text
Almacén + exposición + reservas temporales = stock total
```

- El precio afecta al interés de compra.
- Los clientes no duplican reservas ni quedan bloqueados indefinidamente.
- Cada venta se registra una sola vez.
- El cierre elimina clientes y reservas temporales de forma segura.
- El saldo solo cambia mediante movimientos registrados.
- El impuesto se aplica únicamente al beneficio semanal positivo.
- Guardar, cerrar y cargar conserva dinero, inventario, muebles, pedidos, precios y semana fiscal.
- No existen bugs bloqueantes ni excepciones recurrentes.
- Rendimiento objetivo: `60 FPS` a `1920 × 1080` con 8 clientes y tienda equipada.

### Definition of Done

```text
1. Sistemas incluidos implementados.
2. Criterios obligatorios validados.
3. Partida de siete días completada.
4. Guardado y carga sin diferencias de estado.
5. Sin bugs bloqueantes o críticos abiertos.
6. Bugs importantes restantes documentados.
7. Documentación técnica actualizada.
8. Trazabilidad registrada.
9. Build interna de Windows generada.
10. Informe de validación producido.
```

### Secuencia posterior aprobada

```text
Consolidar decisiones 1–26
→ actualizar GDD, TDD, modelo de datos, UX, arte, audio, QA, roadmap y planes afectados
→ auditar el código actual
→ decidir qué sistemas se reutilizan, refactorizan o sustituyen
→ diseñar la nueva arquitectura
→ crear el roadmap por sprints del vertical slice
→ implementar y validar el slice
→ desarrollar las etapas avanzadas según el orden de progresión aprobado
```

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> El alcance, las exclusiones, los criterios de aceptación y la Definition of Done del vertical slice están aprobados.

# 9. Checklist de arranque

- [ ] Validar nombre provisional y nomenclatura técnica.
- [ ] Crear repositorio y ramas.
- [ ] Congelar versión exacta de Unity.
- [ ] Crear proyecto URP 3D y paquetes mínimos.
- [ ] Importar estructura de carpetas y asmdefs.
- [ ] Crear escena Bootstrap, MainMenu, StorePrototype y TestLab.
- [ ] Crear backlog del Sprint 0 y Sprint 1.
- [ ] Configurar QA, versionado y changelog.
- [ ] Crear primer build vacío de smoke test.
