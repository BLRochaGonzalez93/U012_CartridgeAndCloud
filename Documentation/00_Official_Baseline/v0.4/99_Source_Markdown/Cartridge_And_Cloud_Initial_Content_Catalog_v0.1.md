---
title: "Cartridge & Cloud - Initial Content & Catalog Specification"
subtitle: "Productos, mobiliario, clientes, proveedores y contenido inicial"
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
**Versión del documento:** v0.1  
**Estado:** Revisado en la baseline documental v0.4; contenido funcional vigente  
**Fuente conceptual:** `Enfoque_v0.6.md`  

> **Regla de interpretación:** el proyecto se documenta como una preproducción nueva. Sprint 0 ha validado la fundación técnica (proyecto Unity, assemblies, escenas, tests y builds). Salvo cuando se cite evidencia de implementación, los sistemas de gameplay y contenido descritos en este documento continúan siendo especificación.

\newpage

# 1. Objetivo

Definir el contenido mínimo editable que permite probar el bucle de tienda sin producir todavía el catálogo completo del juego.

# 2. Productos y servicios

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

## 8. Servicio de puestos informáticos

### Naturaleza del sistema

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

### Consumo eléctrico

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

### Retirada, reutilización y venta

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

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> El consumo, la reutilización, la venta y la representación de desgaste están definidos. Los valores eléctricos y el aspecto final se balancearán y pulirán durante producción.

## 9. Desbloqueo de los puestos informáticos

La primera tienda no permite instalar puestos.

### Posición en investigación

El desbloqueo estará en la rama **Servicios al cliente**:

```text
Servicios al cliente I
→ Ampliación de tienda I
→ Servicio de puestos informáticos
→ Puestos premium
```

### Requisitos iniciales

- `40` puntos de investigación.
- Nivel empresarial `2`.
- Reputación mínima `100`.
- Coste de investigación de `2.000 €`.
- Duración de `2` días.

### Ampliación

- Coste inicial: `12.000 €`.
- Duración: `3` días.
- Zona provisional: `12 × 10` celdas al este de la tienda.

### Flujo

```text
Tienda inicial
→ alcanzar nivel y reputación
→ desbloquear nodos previos
→ investigar servicio
→ construir ampliación
→ comprar componentes
→ instalar puestos
```

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> La posición del nodo, sus requisitos y la ampliación están definidos. El balance económico se realizará junto al resto de la progresión.

## 10. Componentes de un puesto informático

Cada puesto requiere:

- Mesa.
- Silla.
- Ordenador.
- Monitor.
- Teclado.
- Ratón.

No existe un periférico adicional obligatorio.

### Definición mediante datos

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

### Condiciones operativas

- Todos los slots completos.
- Silla en su posición.
- Acceso transitable.
- Zona compatible.
- No reservado.
- No ocupado.

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> La composición y validación están definidas. La clase concreta, los datos y la herramienta de montaje se diseñarán durante la implementación.

## 11. Tiers de componentes

Cada componente tiene un tier de calidad.

Orden de menor a mayor:

```text
E → D → C → B → A → S
```

El tier global del puesto se define por el componente de menor tier.

### Fórmula

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

### Ejemplo A

```text
Mesa: B
Silla: B
Ordenador: B
Monitor: S
Teclado: B
Ratón: B

Tier final: B
```

### Ejemplo B

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

### Objetivo del sistema

- Hacer que las mejoras sean visibles.
- Evitar configuraciones desequilibradas.
- Crear progresión mediante sustitución de componentes.
- Dar valor a todo el mobiliario.
- Simplificar el cálculo de calidad.

> **Estado del apartado: COMPLETO.**  
> La regla principal del tier está definida. Los valores económicos de cada tier se balancearán más adelante.

---

## 12. Tarifa y duración de uso de los puestos

Cada tier tiene una tarifa automática por hora.

| Tier | Tarifa provisional |
|---|---:|
| E | 2,00 €/h |
| D | 3,00 €/h |
| C | 4,50 €/h |
| B | 6,00 €/h |
| A | 8,50 €/h |
| S | 12,00 €/h |

### Tiempo de uso

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

### Cobro prorrateado

```text
Pago = TarifaPorHora × TiempoUtilizadoEnMinutos / 60
```

El resultado se redondeará a dos decimales.

> **Estado del apartado: PARCIALMENTE DEFINIDO.**  
> La fórmula, rango y distribución inicial están definidos. Las tarifas, duración media, inversión y rentabilidad se balancearán durante pruebas.

# 3. Clientes e interacción

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

## 13. Uso del puesto por los clientes

### Flujo

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

### Estados del puesto

```text
Invalid
Available
Reserved
Occupied
```

### Camino bloqueado

- El cliente espera un segundo de simulación.
- Vuelve a solicitar una ruta.
- Cada fallo consume paciencia.
- Si agota la paciencia, abandona el objetivo.
- Si tiene productos reservados, va a caja.
- Si no tiene productos ni servicios pendientes, sale.

### Reserva de productos

- La unidad permanece físicamente en el mueble.
- Su estado lógico pasa a `Reserved`.
- No puede seleccionarla otro cliente.
- Solo se descuenta del stock al pagar.
- Si el cliente renuncia o la tienda cierra, vuelve a `Available`.

### Cierre de tienda

Al cerrar:

- El puesto se libera inmediatamente.
- El tiempo utilizado se calcula hasta el instante de cierre.
- El servicio se cobra automáticamente.
- Los productos reservados se liberan sin cobrarse.
- El cliente se dirige a la salida.
- La satisfacción recibe la penalización de cierre anticipado.

> **Estado del apartado: COMPLETO A NIVEL CONCEPTUAL.**  
> Los estados, bloqueos, reservas, cobro y cierre están definidos. La paciencia se deriva del perfil híbrido del cliente.

## 14. Cliente que solo quiere usar un ordenador

### Puesto disponible

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

### Puesto no disponible

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

> **Estado del apartado: PARCIALMENTE DEFINIDO.**  
> El comportamiento está definido. Falta cerrar la paciencia inicial por perfil y la magnitud definitiva de las penalizaciones de satisfacción.

## 15. Cliente que quiere comprar y usar ordenador

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

### Flujo resumido

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

> **Estado del apartado: COMPLETO.**  
> El comportamiento principal queda definido. Los tiempos y valores de paciencia se balancearán posteriormente.

---

# 4. Catálogo mínimo del vertical slice

| ID de familia | Ejemplos | Nº inicial | Exposición |
|---|---|---:|---|
| games_standard | Juegos de precio medio | 4 | Estantería |
| games_premium | Lanzamientos premium | 2 | Vitrina/estantería |
| consoles | Consola ficticia | 1 | Vitrina |
| controllers | Mandos | 2 | Estantería/vitrina |
| headsets | Auriculares | 1 | Estantería |
| accessories | Cables, fundas y pequeños accesorios | 2 | Estantería |

# 5. Mobiliario mínimo

- Mostrador y caja.
- Estantería de productos.
- Vitrina protegida.
- Estantería de almacén.
- Punto de recepción.
- Decoración no funcional opcional.

# 6. Proveedor inicial

Un proveedor general con envío normal, umbral de envío gratuito, catálogo limitado y entrega al día siguiente. Las relaciones, descuentos y proveedores especializados se incorporan después del vertical slice.

# 7. Reglas de datos

Todo elemento tendrá ID estable, claves de localización, icono/asset provisional, coste, precio recomendado, volumen, peso, fragilidad, compatibilidad de mueble y reglas de desbloqueo. El contenido no debe contener lógica ejecutable.
