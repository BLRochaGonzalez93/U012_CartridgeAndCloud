---
title: "Cartridge & Cloud - Economy & Balance Specification"
subtitle: "Economía inicial, costes, márgenes, dificultad y métricas de balance"
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
**Versión del documento:** v0.1  
**Estado:** Preproducción reiniciada desde cero  
**Fuente conceptual:** `Enfoque_v0.6.md`  

> **Regla de interpretación:** el proyecto se documenta como una preproducción nueva. No se presupone código, escenas, managers, datos, builds ni sprints implementados. Todo elemento técnico descrito es una especificación o recomendación hasta que exista evidencia de implementación y QA.

\newpage

# 1. Objetivo

Definir una economía transparente y ajustable que fuerce decisiones sobre inventario, espacio, precios y crecimiento sin convertir el inicio en una trampa irreversible.

# 2. Bucle económico

```text
Capital -> mobiliario y mercancía -> ventas -> margen bruto
-> costes fijos y variables -> beneficio operativo -> impuestos
-> reinversión y expansión
```

# 3. Valores aprobados

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

## 6. Tienda física

### Familias iniciales de productos

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

### Proveedores provisionales

| Proveedor | Disponibilidad | Entrega | Coste orientativo respecto al precio recomendado |
|---|---|---:|---:|
| Distribuidor generalista | Inicio | 1 día | 70–85 % |
| Especialista en hardware | Nivel 2 | 2 días | 65–80 % |
| Mayorista de accesorios | Nivel 2 | 1–2 días | 55–70 % |
| Proveedor retro | Investigación | 2–4 días | 40–70 % |
| Distribuidor de coleccionismo | Reputación media | 3–5 días | 50–75 % |

Cada proveedor podrá tener catálogo, stock, pedido mínimo y fiabilidad propios.

### Gestión de productos

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

### Sistema provisional de precios

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

### Promociones

El jugador podrá activar descuentos del:

- `5 %`.
- `10 %`.
- `15 %`.
- `20 %`.

Duración inicial: entre `1` y `7` días.

Las promociones aumentarán el interés y la rotación, pero reducirán el margen.

### Capacidad inicial por tier

| Tier | Estantería `4 × 2` | Vitrina `2 × 2` |
|---|---:|---:|
| E | 16 | 8 |
| D | 20 | 10 |
| C | 24 | 12 |
| B | 28 | 16 |
| A | 32 | 20 |
| S | 40 | 24 |

Los productos grandes podrán consumir más de una unidad de exposición mediante `displaySize`.

### Representación del stock visible

Cada mueble podrá intercambiar su modelo o conjunto visual según el porcentaje de ocupación:

- `0 %`: vacío.
- `1–25 %`: casi vacío.
- `26–50 %`: medio-bajo.
- `51–75 %`: medio-alto.
- `76–100 %`: lleno.

El stock lógico será la fuente de verdad.

### Reservas

Las reservas se desbloquearán mediante investigación:

1. Durante la noche llega una notificación con icono de correo.
2. El informe indica productos, cantidades, cliente, fecha y hora.
3. El jugador prepara el paquete.
4. El cliente paga al recogerlo.
5. Reserva preparada: `+15` de satisfacción.
6. Reserva no preparada: `-25` de satisfacción y `-2` de reputación.
7. La reserva fallida se cancela sin cobro.

### Devoluciones

Sistema base provisional:

- Plazo: `3` días de juego.
- Solo productos no consumibles.
- Reembolso completo.
- El producto devuelto pasa a estado usado.
- Su precio base de reventa será el `50 %` del producto nuevo.
- La frecuencia y las causas se balancearán posteriormente.

### Productos retro

- Se desbloquean mediante investigación.
- Proceden principalmente del proveedor retro.
- Se consideran usados.
- Pueden tener rareza y precio recomendado superiores.
- No requerirán reparación ni mantenimiento.
- Utilizarán la misma capa visual de desgaste que otros artículos usados.

### Robos

- No formarán parte del vertical slice inicial.
- Se activarán posteriormente como sistema opcional.
- Probabilidad base provisional: `0,5 %` por cliente expuesto a un producto no vigilado.
- La vigilancia, el mostrador, los empleados y futuros sistemas de seguridad reducirán el riesgo.
- El robo retira stock y genera una incidencia en el resumen diario.

### Inventario del cliente

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

### Economía inicial aprobada

#### Capital inicial

La dificultad Estándar comenzará con:

```text
Capital inicial: 20.000 €
Local inicial: alquilado y vacío
Empleados: ninguno
Préstamos: ninguno
Puestos informáticos: ninguno
```

La inversión esperada de apertura será de `13.000–15.000 €`, dejando aproximadamente `5.000–7.000 €` de tesorería.

#### Alquileres y costes fijos

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

#### Impuestos y gastos periódicos

- Impuesto ficticio y simplificado del `10 %` sobre el beneficio operativo positivo.
- Liquidación cada `7 días`.
- Las semanas con resultado cero o negativo no pagan impuestos.
- El resumen diario muestra una provisión estimada.
- Transporte normal: `50–100 €` por pedido.
- Entrega urgente: `200 €`.
- Mantenimiento semanal del complejo: `0,1 %` del valor construido.
- Seguro, licencias y servicios básicos no se cobran dos veces porque ya están incluidos en los costes diarios.

#### Márgenes objetivo

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

#### Dificultades económicas

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

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> Proveedores, precios, promociones, economía inicial, costes, impuestos, márgenes y dificultades disponen de una base aprobada. Sus cifras se validarán mediante balance sin reabrir su estructura conceptual salvo que las pruebas revelen un problema fundamental.

# 4. Empleados y coste laboral

## 16. Empleados

La contratación se desbloqueará durante la **Etapa 2 — Tienda ampliada**.

Requisitos provisionales:

- Nivel empresarial `2`.
- Reputación mínima `50`.
- Al menos `7` días de actividad.
- Mostrador o área de trabajo válida.

### 16.1. Tareas previstas

- Atender el mostrador y cobrar.
- Reponer estanterías y vitrinas.
- Transportar cajas.
- Recibir mercancía.
- Organizar el almacén.
- Preparar reservas y pedidos online.
- Recoger productos.
- Realizar tareas administrativas.

### 16.2. Contratación y candidatos

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

### 16.3. Progresión profesional

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

### 16.4. Cansancio y descansos

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

### 16.5. Salarios y renegociación

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

### 16.6. Despidos y reputación laboral

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

### 16.7. Cierre a las 22:00

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

### 16.8. Prioridad y automatización

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

### 16.9. Fórmula base de eficiencia

```text
DuraciónRealTarea =
DuraciónBase / (1 + 0,15 × (Habilidad - 1))
```

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> Contratación, candidatos, progresión, formación, cansancio, descansos, salarios, despidos, cierre y automatización están definidos. Los valores se validarán mediante balance y pruebas de comportamiento.

# 5. Expansiones empresariales

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

# 6. Reglas de cálculo

- El dinero solo cambia mediante transacciones registradas.
- El coste de mercancía vendida se reconoce al completar la venta.
- El stock no vendido permanece como activo operativo, no como beneficio.
- Los gastos de transporte se asignan al pedido o periodo correspondiente.
- Los impuestos solo se calculan sobre beneficio positivo.
- Los cambios de dificultad aplican multiplicadores declarados, no reglas ocultas.
- Los valores comerciales y los de simulación deben centralizarse en datos configurables.

# 7. Telemetría de balance

- Capital tras equipar el local.
- Días hasta beneficio operativo positivo.
- Ventas y margen por familia.
- Porcentaje de stock inmóvil.
- Coste por cliente atendido.
- Salarios sobre facturación.
- Tasa de devoluciones.
- Coste de oportunidad por rotura de stock.
- Tiempo hasta cada expansión.
- Probabilidad de insolvencia por dificultad.

# 8. Casos extremos

La QA debe probar precios mínimos/máximos, saldo cercano a cero, devolución después de cierre, impuesto en semana negativa, pedido que supera capacidad, despido con deuda salarial y expansión con reserva insuficiente.
