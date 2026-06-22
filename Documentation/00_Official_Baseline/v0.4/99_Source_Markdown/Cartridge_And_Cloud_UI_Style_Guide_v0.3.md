---
title: "Cartridge & Cloud - UI Style Guide"
subtitle: "Sistema visual, componentes y accesibilidad para PC"
author: "VRM Games / Blas Luis Rocha González"
date: "22/06/2026"
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
**Motor validado:** Unity 6.3 LTS `6000.3.18f1` / C# / URP `17.3.0`  
**Versión del documento:** v0.3  
**Estado:** Revisado en la baseline documental v0.4; contenido funcional vigente  
**Fuente conceptual:** `Enfoque_v0.6.md`  

> **Regla de interpretación:** el proyecto se documenta como una preproducción nueva. Sprint 0 ha validado la fundación técnica (proyecto Unity, assemblies, escenas, tests y builds). Salvo cuando se cite evidencia de implementación, los sistemas de gameplay y contenido descritos en este documento continúan siendo especificación.

\newpage

# 1. Dirección

La UI combina una identidad tecnológica negra/verde con una capa comercial más cálida y nostálgica. Los paneles avanzados pueden sentirse técnicos; la tienda inicial debe resultar cercana y legible.

# 2. Paleta

| Token | HEX | Uso |
|---|---|---|
| VRM Black | #050505 | Fondo profundo |
| Deep Charcoal | #0B0F0C | Barras y overlays |
| Panel Graphite | #111812 | Paneles |
| Control Surface | #16201E | Inputs y cards |
| Border | #2F463D | Bordes y separadores |
| VRM Green | #00C853 | Acción principal |
| Neon Accent | #00FF66 | Foco excepcional |
| Soft Green | #7CFFB2 | Positivo suave |
| Data Cyan | #49E6FF | Métricas |
| Warning Amber | #F2C94C | Riesgo |
| Danger Red | #FF5C5C | Error crítico |
| Text Primary | #F2F5F3 | Texto principal |
| Text Muted | #A9BBB0 | Secundario |

# 3. Tipografía

Una sans-serif altamente legible para cuerpo y paneles; una familia display tecnológica moderada para títulos; fuente monoespaciada solo para cifras, IDs y logs. No se incluirán archivos de fuente sin licencia verificada.

# 4. Escala y layout

- Base 1920x1080, escalable a 1280x720 y 2560x1440.
- Márgenes seguros de 32-40 px.
- Grid de espaciado 4/8/12/16/24/32.
- Cuerpo 14-16 px; botones 14-16; títulos 24-32.
- Máximo de una acción primaria por panel.

# 5. HUD del vertical slice

- Dinero, día, hora y velocidad.
- Estado abierto/cerrado.
- Clientes presentes.
- Alertas de stock, cola y pedido.
- Accesos a construcción, inventario, proveedores y economía.

# 6. Componentes

Botón primario/secundario/peligro, input, dropdown, stepper numérico, tooltip, modal, toast, card, tabla, tabs, barra de progreso, estado, gráfico, timeline, árbol de investigación y panel contextual en mundo.

# 7. Estados

Normal, Hover, Focused, Pressed, Selected, Disabled, Loading, Success, Warning, Error. Color nunca es el único indicador; se acompaña de texto/icono.

# 8. Modo construcción

La previsualización usa verde para válido, rojo para inválido, ámbar para advertencia y contorno de huella. La UI muestra coste, rotación, requisitos, motivo de bloqueo y reembolso.

# 9. Tablas y dashboards

- Cabeceras fijas cuando haya scroll.
- Alineación derecha para cifras.
- Unidades visibles.
- Comparativas con periodo anterior.
- Filtros persistentes.
- No mostrar más de tres colores funcionales simultáneos.

# 10. Pantallas por etapa

## 18. Publishing

El publishing será una línea de negocio avanzada centrada en descubrir, financiar, acompañar y comercializar proyectos creados por estudios externos. No será una variante pasiva del desarrollo interno ni una pantalla de inversión automática.

La fantasía será:

> **Detectar proyectos prometedores, negociar acuerdos, aportar financiación y servicios, supervisar su producción y convertirlos en lanzamientos comercialmente viables.**

### Función dentro de la progresión

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

### Bucle principal

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

### Estudios externos

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

### Propuestas

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

### Evaluación e incertidumbre

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

### Due diligence

| Evaluación | Coste provisional | Duración | Efecto |
|---|---:|---:|---|
| Revisión básica | 1.000 € | 1 día | Documentación, presupuesto e historial |
| Revisión técnica | 3.000 € | 2 días | Prototipo, rendimiento y riesgos técnicos |
| Estudio de mercado | 4.000 € | 2 días | Público, competencia y precio |
| Evaluación completa | 7.500 € | 4 días | Combina las anteriores con mayor confianza |

La información nunca será completamente perfecta.

### Decisiones sobre una propuesta

El jugador podrá:

- Rechazar.
- Archivar.
- Solicitar cambios.
- Pedir un prototipo.
- Ofrecer distribución únicamente.
- Negociar financiación y servicios.
- Firmar.

Los cambios solicitados podrán afectar a plataformas, alcance, presupuesto, localización, calendario o público.

### Tipos de contrato

| Contrato | Inversión orientativa | Participación editorial | Riesgo | Control |
|---|---:|---:|---|---|
| Distribución | 5.000–30.000 € | 10–20 % | Bajo | Bajo |
| Marketing y lanzamiento | 20.000–100.000 € | 15–30 % | Medio | Medio-bajo |
| Financiación parcial | 50.000–250.000 € | 25–40 % | Medio-alto | Medio |
| Financiación completa | 150.000–1.000.000 € | 40–60 % | Alto | Alto |
| Coproducción | Variable | 30–50 % | Compartido | Alto |

La primera implementación utilizará distribución, marketing y financiación parcial.

### Elementos negociables

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

### Producción por hitos

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

### Capacidad editorial

Los proyectos consumirán capacidad:

| Complejidad | Capacidad |
|---|---:|
| Pequeño | 1 |
| Medio | 2 |
| Grande | 4 |
| Estratégico | 6 |

Capacidad inicial recomendada: `4 puntos`.

Se ampliará mediante personal, investigación, espacio, herramientas y reputación.

### Intervención editorial

- **Mínima:** libertad alta y menor coste, pero mayor riesgo.
- **Colaborativa:** equilibrio recomendado.
- **Directiva:** mayor control, coste y riesgo de conflicto.

Los problemas de producción dependerán del alcance, presupuesto, capacidades, deuda, calendario y decisiones previas. Podrán incluir retrasos, bugs graves, cambios de personal, conflictos creativos o cambios de mercado.

### Reputación editorial

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

### Marketing, distribución y lanzamiento

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

### Recuperación de inversión y liquidación

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

### Catálogo y sinergias

Cada juego publicado conservará contrato, inversión, ventas, rentabilidad, recepción, plataformas, relación, propiedad y estado.

El publishing se conectará con:

- Datos de la tienda física.
- Playtests y eventos.
- Reservas y ediciones físicas.
- Comercio online.
- Desarrollo interno.
- Plataforma digital.

### Publishing MVP

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

### Criterios de aceptación del MVP

El jugador debe poder recibir, evaluar, negociar, firmar, financiar, supervisar, lanzar y liquidar un proyecto externo sin duplicar pagos, ventas o estados.

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> El bucle, la evaluación, los contratos, los hitos, la reputación, las liquidaciones y el alcance del MVP están definidos.

---

## 19. Desarrollo interno

El desarrollo interno será una línea avanzada y diferenciada del modelo clásico de *Game Dev Tycoon*.

No se basará en:

- Buscar combinaciones ocultas de género, tema y plataforma.
- Repartir porcentajes abstractos entre diseño, tecnología y arte.
- Esperar a que se llenen barras de puntos.
- Recibir una nota producida casi exclusivamente por compatibilidades.

La fantasía será:

> **Descubrir oportunidades, formular una visión, validar prototipos, organizar equipos, controlar el alcance, producir por hitos, probar con usuarios reales y lanzar un producto coherente.**

### Bucle principal

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

### Insights y oportunidades

Las oportunidades procederán de:

- Ventas y búsquedas de la tienda.
- Preferencias de arquetipos de cliente.
- Comercio online.
- Publishing.
- Tendencias y saturación.
- Eventos.
- Propuestas del equipo.

Cada insight tendrá fuente, público, confianza, antigüedad y evidencias. No otorgará una bonificación automática; servirá para tomar decisiones informadas.

### Brief y visión

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

### Prototipos

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

### Playtests de prototipo

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

### Greenlight

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

### Tamaños de proyecto

| Tamaño | Alcance | Equipo | Duración | Presupuesto orientativo |
|---|---:|---:|---:|---:|
| Micro | 40–60 puntos | 2–4 | 5–9 días | 50.000–120.000 € |
| Pequeño | 80–120 | 4–7 | 8–15 días | 120.000–300.000 € |
| Mediano | 160–240 | 8–14 | 15–25 días | 350.000–900.000 € |
| Grande | 300–450 | 15–30 | 25–40 días | 1–3 M€ |
| Emblemático | 500+ | 30+ | 40+ días | 3 M€ o más |

El primer juego interno será micro o pequeño.

### Equipo interno

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

### Backlog de funcionalidades

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

### Dependencias y trabajo en curso

Comenzar sin dependencias completas aumentará retrabajo y errores. El límite inicial recomendado será:

```text
Máximo de tres funcionalidades activas
```

Superarlo generará cambios de contexto, integración tardía y deuda.

### Hitos internos

- Concept Complete.
- Vertical Slice.
- Alpha.
- Beta.
- Release Candidate.
- Gold.

Cada revisión mostrará objetivos, presupuesto, retrasos, calidad, riesgos, bugs, feedback y carga del equipo.

### Gestión del alcance

El proyecto distinguirá:

- Núcleo obligatorio.
- Funciones importantes.
- Contenido secundario.
- Mejoras opcionales.
- Ideas futuras.

Los cambios tardíos serán más caros y peligrosos. El *scope creep* será una consecuencia de decisiones, no un evento aleatorio aislado.

### Deuda técnica y de diseño

La deuda técnica aumentará por prisas, falta de pruebas, tecnologías inadecuadas, cambios tardíos y exceso de trabajo simultáneo.

La deuda de diseño aumentará por sistemas contradictorios, funcionalidades sin propósito, cambios de visión y falta de validación.

Podrán reducirse mediante refactor, revisión, simplificación, eliminación y pruebas.

### Builds e integración

Estados de build:

- Inestable.
- Jugable.
- Estable.
- Candidata.
- Aprobada.

Integrar revelará incompatibilidades, bugs, problemas de rendimiento o datos. Esperar demasiado para integrar aumentará el riesgo.

### Playtests durante producción

Los grupos podrán ser internos, clientes, jugadores del género, público general o testers profesionales.

El feedback se clasificará por temas y el jugador decidirá aceptarlo, investigarlo, posponerlo o ignorarlo. Seguir todo el feedback será perjudicial; se valorará la coherencia con público, pilares, datos y alcance.

### Calidad y mercado

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

### Presupuesto y contingencia

El presupuesto se dividirá entre salarios, outsourcing, herramientas, prototipos, producción, QA, localización, marketing, fabricación y contingencia.

Reserva recomendada:

```text
10–20 % del presupuesto
```

### Intensidad de producción

- Normal.
- Elevada.
- Crisis.

La crisis aumentará capacidad temporal, cansancio, deuda, errores y riesgo laboral. No será óptima como estrategia permanente.

### Lanzamiento y postlanzamiento

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

### Sinergias

La tienda permitirá playtests, eventos, reservas, ediciones físicas y observación de clientes. Publishing aportará experiencia de producción y distribución. La plataforma aportará usuarios, analytics y distribución propia.

### Desarrollo interno MVP

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

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> La visión, el prototipado, el greenlight, la producción por funcionalidades e hitos, la calidad y el MVP están definidos.

---

## 20. Comercio online

El comercio online será una extensión logística y comercial de la tienda física. No se confundirá con la plataforma digital.

```text
Comercio online = venta y envío de productos físicos
Plataforma digital = venta, licencia y distribución de videojuegos digitales
```

La fantasía será:

> **Convertir el inventario, la reputación y la logística de la tienda en un negocio a distancia capaz de preparar, enviar y atender pedidos físicos.**

### Bucle principal

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

### Activación

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

### Catálogo y precios

Cada producto configurará:

- Activo o inactivo.
- Precio online.
- Stock protegido para tienda.
- Stock máximo ofrecido.
- Límite por pedido.
- Métodos y regiones.
- Promociones.

El precio online podrá diferir del físico. La interfaz mostrará margen después de coste, embalaje, comisión, transporte subvencionado y devolución estimada.

### Inventario compartido

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

### Reserva y disponibilidad

Al confirmar un pedido:

1. Se valida disponibilidad.
2. Las unidades pasan a `ReservedOnline`.
3. Dejan de estar disponibles en ambos canales.
4. Se genera la tarea.
5. Comienza el plazo de procesamiento.

Preventas, pedidos bajo demanda y *backorders* quedan pospuestos.

### Demanda online

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

### Estados de pedido

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

### Pagos

El pago será abstracto. Comisión provisional:

```text
2 % del importe pagado
```

No se simularán bancos, tarjetas ni fraude en el MVP.

### Picking y packing

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

### Embalajes

| Tipo | Coste provisional |
|---|---:|
| Sobre protegido | 1 € |
| Caja pequeña | 2 € |
| Caja mediana | 4 € |
| Caja reforzada | 8 € |
| Protección adicional | 1–5 € |

Cada producto tendrá peso, volumen y fragilidad abstractos.

### Transportistas

- Económico: 3–5 días.
- Estándar: 2–3 días.
- Exprés: 1 día.

El jugador decidirá cuánto paga el cliente y cuánto subvenciona la empresa.

Regla inicial recomendada:

```text
Envío estándar gratuito desde 100 €
```

### Capacidad logística

Con una estación y un trabajador competente:

```text
8–12 pedidos por día
```

Objetivo de preparación:

```text
Dentro de 1 día
```

Pedidos confirmados antes de las `16:00` podrán salir el mismo día. Recogida provisional: `19:00`.

### Satisfacción y reputación online

La satisfacción dependerá de producto, estado, precio, rapidez, embalaje, comunicación, errores, daños y coste de envío.

```text
Reputación online: 0–100
Valor inicial: 50
```

Las valoraciones utilizarán `1–5 estrellas`. Aproximadamente `20–35 %` de clientes valorarán, con mayor probabilidad en extremos de satisfacción.

### Incidencias, cancelaciones y devoluciones

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

### Integración con empleados y cierre

El sistema de tareas incluirá picking, packing, etiquetado, expedición e incidencias.

A las `22:00`:

- El canal podrá seguir aceptando pedidos.
- Los nuevos pedidos quedarán para el día siguiente.
- Las tareas activas se asegurarán.
- Los productos seguirán reservados.
- Los paquetes quedarán guardados.

### Comercio online MVP

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

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> El catálogo, el stock multicanal, el flujo logístico, los envíos, la reputación y el MVP están definidos.

---

## 21. Plataforma digital

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

### Bucle principal

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

### Pilares

- Confianza.
- Catálogo.
- Descubrimiento.
- Fiabilidad.
- Equilibrio entre usuarios, estudios y empresa.

### Identidad

La plataforma podrá orientarse como generalista, indie, especializada o conectada con la comunidad física. La recomendación inicial es un modelo híbrido con identidad surgida del catálogo y las decisiones.

### Fases de lanzamiento

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

### Catálogo

Tipos:

- Juegos internos.
- Juegos publicados.
- Juegos de terceros.
- Contenido descargable sencillo.

Los estudios externos seguirán un flujo de solicitud, revisión técnica, creación de ficha, validación de build, configuración comercial y publicación.

Modelo recomendado: híbrido. Todos los juegos superan requisitos técnicos, pero la empresa no aprueba subjetivamente cada diseño.

### Estados de publicación

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

### Comisión

```text
Comisión estándar de plataforma: 20 %
Procesamiento de pago: 2 %
```

Acuerdos preferentes podrán reducir la comisión. Las condiciones afectarán a la confianza de desarrolladores.

```text
Confianza de desarrolladores: 0–100
Valor inicial: 40
```

### Cuentas, bibliotecas y licencias

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

### Descargas y actualizaciones

Las descargas utilizarán tamaño, capacidad, ancho de banda, región y concurrencia. Las builds pasarán por validación y podrán desplegarse como parche, actualización, contenido o hotfix.

### Descubrimiento

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

### Reviews y reembolsos

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

### Usuarios, retención y adquisición

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

### Reputación de plataforma

```text
Reputación de plataforma: 0–100
Valor inicial: 40
```

Aumentará con estabilidad, soporte, políticas justas, catálogo y transparencia. Disminuirá con caídas, cobros erróneos, pérdida de licencias, mala moderación o políticas abusivas.

### Soporte y pagos a estudios

Se gestionarán incidencias de usuarios y desarrolladores. Las liquidaciones serán semanales y mostrarán ventas, reembolsos, impuestos, procesamiento, comisión e ingreso neto.

### Rentabilidad

```text
Ingresos:
comisiones + promociones + servicios

Gastos:
infraestructura + soporte + desarrollo + adquisición + mantenimiento
```

Muchas ventas no garantizan rentabilidad.

### Gobernanza

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

### Condición principal de victoria

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

### Plataforma MVP

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

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> El ecosistema, el modelo comercial, las cuentas, el descubrimiento, la reputación, la victoria y el MVP están definidos.

---

## 22. Infraestructura y servicios online

La infraestructura tendrá una profundidad estratégica intermedia. No será una cifra pasiva, pero tampoco un simulador profesional de administración de sistemas.

La fantasía será:

> **Construir y operar la infraestructura que sostiene la plataforma, anticipar la demanda y equilibrar capacidad, coste, fiabilidad, seguridad y crecimiento.**

El jugador no configurará direcciones IP, sistemas operativos, cableado detallado ni componentes reales.

### Tres capas

#### Capa física simplificada

- Racks.
- refrigeración.
- alimentación.
- almacenamiento.
- red abstracta.
- centro de operaciones.

#### Capa operativa

- Cómputo.
- almacenamiento.
- ancho de banda.
- base de datos.
- concurrencia.
- soporte.

#### Capa de servicios

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

### Pools de capacidad

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

### Modelos de infraestructura

- Gestionada: rápida y flexible, pero cara y dependiente.
- Propia: inversión alta y control, con costes de energía y mantenimiento.
- Híbrida: base propia más capacidad externa para picos.

El modelo híbrido será la recomendación inicial.

### Módulos físicos

- Rack de cómputo.
- Rack de almacenamiento.
- Red.
- Base de datos.
- Refrigeración.
- Alimentación redundante.
- Backups.
- Centro de operaciones.

Los tiers E–S modificarán capacidad, eficiencia, fiabilidad, coste y ocupación.

### Espacio, temperatura y electricidad

Los módulos generarán carga térmica abstracta:

```text
Carga térmica total ≤ capacidad de refrigeración
```

Superarla aumentará consumo, reducirá rendimiento y elevará el riesgo.

La electricidad dependerá de carga base, utilización, refrigeración y redundancia.

### Escalado

- Vertical: mejorar módulos.
- Horizontal: añadir módulos.
- Externo: contratar capacidad.
- Temporal: capacidad por 1, 3 o 7 días para eventos y lanzamientos.

Las mejoras requerirán tiempo y podrán reducir capacidad durante la instalación.

### Prioridad de servicios

- Crítica: cuentas, licencias, compras, bibliotecas.
- Alta: catálogo, descargas, actualizaciones.
- Media: reviews, recomendaciones, analítica.
- Variable: servicios de juegos.

Niveles de servicio:

- Económico.
- Estándar.
- Prioritario.

En saturación, los servicios secundarios podrán degradarse para proteger los críticos.

### Degradación controlada

Ejemplos:

- Recomendaciones menos precisas.
- Analítica retrasada.
- Descargas en cola.
- Reviews en espera.
- Imágenes con menor prioridad.

Esto evitará convertir cualquier pico en una caída total.

### Picos y despliegues

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

### Redundancia y backups

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

### Incidencias

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

### Operaciones y personal

El centro de operaciones gestionará alertas, cambios, capacidad, mantenimiento, backups, seguridad e incidencias.

Perfiles futuros:

- Operador de sistemas.
- Especialista de red.
- Administrador de datos.
- Seguridad.
- Responsable de operaciones.
- Ingeniero de plataforma.

La cobertura 24/7 se abstraerá mediante niveles de guardia y automatización.

### Mantenimiento y seguridad

Mantenimiento:

- Reactivo.
- Programado.
- Preventivo.

La seguridad será abstracta y cubrirá cuentas, pagos, builds, datos y operaciones. Podrán realizarse auditorías internas, externas o de lanzamiento.

No se simularán técnicas ofensivas reales.

### Servicios online para juegos

- Guardado online.
- Estadísticas.
- Rankings.
- Matchmaking.
- Servidores dedicados abstractos.
- Telemetría.

Podrán cobrarse como tarifa fija, por uso o paquetes. Los juegos propios también consumirán capacidad real.

### Costes y eficiencia

Se distinguirán costes fijos y variables. Métricas:

```text
Coste operativo por usuario activo
Ingreso medio por usuario activo
Coste por descarga
Coste por servicio
Disponibilidad
```

La eficiencia mejorará mediante módulos, automatización, investigación, personal y optimización, con rendimientos decrecientes.

### Infraestructura MVP

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

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> La profundidad estratégica, los recursos, el escalado, la fiabilidad, las incidencias y el MVP están definidos.

---

## 23. Mercado y competidores

El mercado deberá sentirse vivo sin simular cada detalle físico de las empresas rivales.

La fantasía será:

> **Construir una empresa dentro de un sector dinámico, detectar oportunidades, responder a rivales y encontrar una posición propia mediante precio, servicio, especialización, comunidad, publishing, desarrollo y plataforma.**

Los competidores no tendrán información perfecta, dinero infinito ni capacidad para reaccionar instantáneamente a cada decisión del jugador.

### Modelo híbrido

#### Competidores visibles

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

#### Mercado agregado

Representará tiendas, comercios, estudios y plataformas menores sin identidad individual.

#### Entorno externo

Representará tendencias, tecnología, estacionalidad, costes, crecimiento, contracción y preferencias.

### Competencia por etapas

- Etapas 1–2: tiendas locales y cadenas.
- Etapa 3: comercios online y logística.
- Etapa 4: publishers y distribución editorial.
- Etapa 5: estudios rivales y talento.
- Etapa 6: plataformas y servicios digitales.

### Arquetipos

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

### Recursos abstractos y frecuencia

Los rivales tendrán capital, ingresos, costes, capacidad, reputación, catálogo, empleados abstractos, infraestructura y cuota.

Simulación:

- Diaria: ventas, gastos, stock y demanda.
- Semanal: precios, promociones, campañas, cuota y proyectos.
- Mensual: expansiones, entradas, cierres, fusiones futuras y cambios estratégicos.

No se simularán muebles, NPCs, cajas ni pedidos individuales de competidores.

### Información disponible

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

### Mercados por categoría

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

### Competencia comercial

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

### Reacción al jugador

Solo reaccionarán ante acciones relevantes:

- Ganar cuota.
- Entrar en un segmento.
- Lanzar una gran promoción.
- Publicar un éxito.
- Abrir una plataforma.
- Reducir comisiones.
- Captar estudios importantes.

Las respuestas serán diferidas y dependerán de información, personalidad, recursos y estrategia.

### Memoria y planes

Los rivales recordarán guerras de precios, categorías, campañas, acuerdos, conflictos y resultados.

Podrán mantener planes de varias semanas, como abrir comercio online o entrar en publishing. El jugador verá indicios, no el plan completo.

### Competencia por estudios y talento

Los estudios podrán recibir varias ofertas editoriales y valorarán financiación, participación, reputación, servicios, libertad y confianza.

Los empleados avanzados podrán recibir ofertas si están mal pagados, existe escasez o la reputación laboral es baja. Siempre habrá aviso y opciones de respuesta.

### Lanzamientos y saturación

Los juegos competirán por atención, marketing, presupuesto y fechas. La saturación reducirá visibilidad, pero no convertirá una categoría en inviable automáticamente.

Calidad, identidad y buen posicionamiento podrán superar un mercado saturado.

### Plataformas

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

### Relaciones y colaboración

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

### Noticias e informes

El boletín mostrará promociones, acuerdos, lanzamientos, cierres, expansiones y cambios de comisión.

El informe semanal resumirá:

- Cuota.
- Precios medios.
- Promociones.
- Tendencias.
- Lanzamientos.
- Movimientos rivales.
- Oportunidades y amenazas.

### Entradas, salidas y evolución

Podrán entrar `0–2 competidores relevantes por año de juego` cuando un mercado sea atractivo.

Las empresas podrán cerrar, fusionarse, abandonar líneas o transformarse después de problemas acumulados, nunca por una única mala semana.

Cada `30 días` revisarán su estrategia y podrán reforzarla, diversificar, especializarse o abandonar un segmento.

### IA estratégica

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

### Justicia y protección contra bola de nieve

Un líder tendrá costes crecientes, ineficiencias, saturación, escrutinio y dificultad de adaptación.

Dominar un mercado atraerá nuevos rivales y presión, pero el jugador pequeño podrá competir mediante nicho, servicio, comunidad, eficiencia e innovación.

La dificultad modificará agresividad, velocidad de reacción y disciplina, sin conceder información perfecta ni recursos infinitos.

### Mercado competitivo MVP

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

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> Los niveles de simulación, los arquetipos, la demanda, las reacciones, la competencia por recursos y el MVP están definidos.

---

# 11. Accesibilidad

- Contraste WCAG AA cuando sea aplicable.
- Escala de UI 80-140 %.
- Tamaño de texto y subtítulos.
- Navegación completa con teclado prevista.
- Reducción de animaciones y camera shake.
- Patrones/iconos además de color.
- Tooltips persistentes opcionales.

# 12. Implementación en Unity

Canvas Scaler `Scale With Screen Size`, TextMeshPro, prefabs de componentes, StyleTokens ScriptableObject, navegación explícita, pooling para listas largas y separación entre view y lógica.

# 13. Checklist

- [ ] Jerarquía clara en 3 segundos.
- [ ] Acción principal identificable.
- [ ] Sin texto cortado ES/EN.
- [ ] Estados de error y disabled explicados.
- [ ] Funciona a resoluciones mínimas.
- [ ] No depende solo del color.
- [ ] Navegación y cierre siempre disponibles.
