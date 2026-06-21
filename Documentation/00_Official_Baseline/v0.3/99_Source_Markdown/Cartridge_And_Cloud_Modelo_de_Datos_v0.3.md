---
title: "Cartridge & Cloud - Modelo de Datos"
subtitle: "Entidades, estados, relaciones, IDs y persistencia"
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

# 1. Principios

- IDs estables y legibles.
- Contenido estático separado de estado de partida.
- Una única fuente de verdad para dinero, unidades y reservas.
- Estados explícitos en lugar de booleanos combinados.
- Datos de guardado sin referencias de escena.
- Versionado y migración desde la primera build compartida.
- Campos calculados no se duplican salvo necesidad de auditoría.

# 2. Capas de datos

| Capa | Ejemplos |
|---|---|
| Configuración | Reglas globales, dificultad, balance |
| Catálogo estático | Productos, muebles, perfiles, proveedores, eventos |
| Estado runtime | Tienda, inventario, clientes, empleados, economía |
| Historial | Transacciones, días, pedidos, lanzamientos |
| Persistencia | Snapshot versionado y migraciones |
| Analítica | Métricas derivadas y series temporales |

# 3. Convenciones de IDs

```text
product_game_nova_01
furniture_shelf_tier_e_01
customer_archetype_budget
supplier_general_01
event_market_holiday_demand
research_online_commerce_01
competitor_pixelhub
```

Los nombres traducidos nunca se utilizan como claves persistentes.

# 4. Entidades del vertical slice

| Entidad | Campos esenciales |
|---|---|
| CompanyState | id, name, money, difficultyId, reputation, currentDate |
| StoreState | gridSize, openState, areas, entrances, furnitureInstances |
| FurnitureDefinition | id, footprint, rotationRules, capacity, interactionPoints, price |
| FurnitureInstance | instanceId, definitionId, position, rotation, assignedProductId |
| ProductDefinition | id, family, cost, recommendedPrice, dimensions, weight, fragility |
| InventoryUnit/Stack | productId, quantity, condition, ownerContainerId, state |
| InventoryContainer | id, type, capacity, acceptedFamilies, content |
| SupplierDefinition | id, catalog, shippingRules, leadTime |
| PurchaseOrder | orderId, lines, total, state, orderedAt, deliveryAt |
| CustomerProfile | archetypeId, budget, patience, priceSensitivity, affinities |
| CustomerState | id, state, satisfaction, reservedItems, queuePosition |
| CheckoutTransaction | transactionId, customerId, lines, totals, state |
| DayState | day, time, speed, openingState, summaryState |
| LedgerEntry | entryId, timestamp, category, amount, referenceId |
| SaveGame | saveVersion, metadata, all context states |

# 5. Estados e invariantes

## 5.1 Inventario

```text
Available -> ReservedInStore -> InCustomerBasket -> Sold
Available -> ReservedOnline -> Picking -> Packed -> Shipped
Available -> Returned/Used/Damaged
```

Invariante: `almacén + exposición + reservas + tránsito + vendido histórico` debe conciliar con entradas, salidas y ajustes registrados.

## 5.2 Pedido a proveedor

`Draft`, `Confirmed`, `InTransit`, `Delivered`, `Cancelled`, `Failed`.

## 5.3 Cliente

`Entering`, `Browsing`, `Evaluating`, `Collecting`, `Queueing`, `Checkout`, `Leaving`, `Abandoned`.

## 5.4 Tienda

`Closed`, `Preparing`, `Open`, `Closing`, `Resolving`, `DayComplete`.

# 6. Datos de empleados

| Entidad | Campos |
|---|---|
| Candidate | profile, salaryRequest, skills, trait, expiry |
| Employee | id, contract, salary, level, xp, fatigue, satisfaction |
| EmployeeSkill | skillId, level, xp |
| EmployeeTask | taskId, category, priority, reservations, state |
| SalaryReview | trigger, requestedIncrease, deadline, resolution |
| LaborReputation | value, recentEvents |

# 7. Datos avanzados

## Publishing

Studio, Proposal, DueDiligenceReport, PublishingContract, Milestone, EditorialProject, MarketingCampaign, RoyaltySettlement y EditorialRelationship.

## Desarrollo interno

Insight, GameBrief, DesignPillar, Prototype, PlaytestReport, GreenlightDecision, Feature, Dependency, InternalMilestone, Build, Bug, Budget y InternalGameRelease.

## Comercio online

OnlineCatalogEntry, OnlineOrder, Package, Shipment, Carrier, ReturnRequest, Refund y OnlineCustomerSegment.

## Plataforma

PlatformUser, DigitalLicense, PlatformGame, StorePage, DeveloperAccount, DigitalPurchase, DigitalRefund, Review, WishlistEntry, PlatformPolicy y DeveloperSettlement.

## Infraestructura

CapacityPool, InfrastructureModule, Service, Incident, MaintenanceWindow, BackupPolicy, Region y ProviderContract.

## Mercado

Competitor, MarketSegment, StrategyPlan, MarketAction, Trend, MarketIndex y CompetitiveRelationship.

# 8. Relaciones críticas

```text
ProductDefinition 1---N InventoryStack
FurnitureInstance 1---0..1 AssignedProduct
CustomerState 1---N Reservation
CheckoutTransaction 1---N TransactionLine
LedgerEntry N---1 BusinessReference
PublishingContract 1---N Milestone
PlatformUser 1---N DigitalLicense
Service N---1 CapacityPool allocation
Competitor 1---N MarketAction
```

# 9. Snapshot de guardado

```json
{
  "saveVersion": 1,
  "metadata": {},
  "company": {},
  "time": {},
  "store": {},
  "inventory": {},
  "orders": [],
  "customers": [],
  "economy": {},
  "unlocks": {},
  "advancedSystems": {}
}
```

Los arrays avanzados pueden estar vacíos antes de desbloquear sus etapas.

# 10. Migración

Cada versión define migraciones incrementales. Un save más nuevo que el ejecutable debe rechazarse con mensaje claro. Las migraciones nunca inventan IDs silenciosamente; utilizan defaults documentados y generan informe de carga.

# 11. Validación de contenido

Herramientas de editor comprobarán IDs vacíos/duplicados, claves de localización, huellas inválidas, capacidades negativas, relaciones rotas, precios incoherentes y estados no alcanzables.

# 12. Datos conceptuales completos

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

## 7. Clientes

### Comportamiento general

Los clientes aparecen con una intención o combinación de intenciones:

- Solo comprar.
- Solo utilizar un puesto informático.
- Comprar y utilizar un puesto.
- Buscar un producto concreto.
- Explorar.
- Recoger una reserva.
- Realizar otras acciones futuras.

### Estados básicos

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

### Modelo de perfiles aprobado: híbrido

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

### Atributos base

- Presupuesto.
- Tiempo máximo de espera.
- Preferencias.
- Intención de compra.
- Intención de usar servicios.
- Tiempo de uso del ordenador.
- Tolerancia a precios.
- Paciencia en cola.
- Satisfacción.

### Fórmula de interés aprobada

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

### Satisfacción

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

### Consecuencias aprobadas

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

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> El modelo híbrido, la compra, la satisfacción, la reputación y el feedback visual están aprobados. Los rangos se balancearán con pruebas.

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

## 17. Investigación

La investigación representa conocimiento empresarial, técnico y comercial.

### Ramas previstas

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

### Modelo aprobado: híbrido

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

### Valores iniciales por categoría

| Categoría | Puntos | Coste | Tiempo |
|---|---:|---:|---:|
| Básico | 10–20 | 500–1.500 € | 1 día |
| Intermedio | 25–50 | 2.000–8.000 € | 2–4 días |
| Avanzado | 60–120 | 10.000–40.000 € | 5–10 días |
| Estratégico | 150+ | 50.000 € o más | 10+ días |

Todos los valores serán configurables mediante datos.

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> Las ramas, requisitos, generación de puntos y costes por categoría están definidos. El contenido exacto del árbol y su balance se producirán por fases.

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

# 13. Criterios de aceptación

- Las entidades del vertical slice están completamente definidas.
- Cada estado persistente tiene propietario y transición.
- El modelo soporta guardado sin referencias Unity.
- Las invariantes de dinero e inventario pueden automatizarse.
- Los sistemas avanzados pueden añadirse sin cambiar IDs base.
