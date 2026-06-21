---
title: "Cartridge & Cloud - QA / Testing Plan"
subtitle: "Estrategia de calidad desde Sprint 0 hasta PC/Steam"
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

# 1. Objetivo

Integrar QA desde el primer commit, priorizando integridad de inventario, dinero, navegación, cierre diario y guardado. El plan asume proyecto sin código.

# 2. Riesgos P0

- Pérdida o duplicación de unidades.
- Cobros o reembolsos duplicados.
- Guardado corrupto.
- Cliente o empleado que bloquea cierre.
- Acceso/puerta permanentemente bloqueado.
- Progreso imposible de continuar.
- Desincronización entre mundo y datos.

# 3. Niveles de prueba

| Nivel | Uso |
|---|---|
| Unit | Fórmulas, estados, invariantes y validaciones |
| Integration | Casos de uso entre servicios |
| PlayMode | Grid, navegación, interacción y UI |
| Functional | Flujos completos en build |
| Regression | Suite del hito |
| Balance | Economía, demanda y progresión |
| Performance | FPS, CPU, memoria y tiempos de carga |
| UX | Comprensión, accesibilidad y fricción |

# 4. Severidad

S0 bloqueante, S1 crítico, S2 alto, S3 medio, S4 bajo. Ningún hito cierra con S0/S1. Los S2 necesitan corrección o excepción formal.

# 5. Definition of Done

- Criterios de aceptación satisfechos.
- Tests unit/integration relevantes.
- Prueba manual documentada.
- Sin errores rojos persistentes.
- Save/load validado si cambia estado.
- Datos validados.
- Documentación y trazabilidad actualizadas.
- Build fuera del editor cuando el sprint afecte al flujo.

# 6. Estrategia por fase

## Sprint 0-2

Setup, compilación, tests básicos, scene flow y persistencia mínima.

## Mundo y construcción

Grid, colisiones, rotaciones, accesos, NavMesh y recuperación de rutas.

## Inventario y logística

Invariantes, capacidades, transferencias, pedidos y cajas.

## Clientes y caja

Estados, reservas, colas, abandono y transacciones idempotentes.

## Día y economía

Apertura/cierre, gastos, impuestos, resumen y cambio de día.

## Vertical slice

Siete días, resolución de 1080p/720p/1440p, rendimiento, UX y regresión completa.

# 7. Casos de prueba mínimos

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

# 8. Guardado

Pruebas de slot nuevo, sobrescritura, autosave, backup, archivo truncado, versión antigua, ID desconocido, carga durante cierre y comparación de snapshot.

# 9. Balance

Simulaciones de 7, 30 y 100 días con diferentes precios, layouts y dificultades. Registrar insolvencia, tiempo de expansión, margen, satisfacción y roturas.

# 10. Rendimiento

Profiler en escena vacía, tienda equipada, 8 clientes y estrés progresivo. Vigilar allocations, pathfinding, UI, serialización y rebake de navegación.

# 11. Compatibilidad PC

Windows 10/11, 1280x720 a 2560x1440, ventana/borderless/fullscreen, distintas tasas de refresco, teclado/ratón y rutas de usuario con caracteres no ASCII.

# 12. QA externo

Primero pruebas observadas del vertical slice; luego sesiones cerradas y formularios. No abrir beta o demo hasta disponer de build representativa y política de feedback.

# 13. Criterios de salida

La build candidata debe tener test run completo, changelog, known issues, hashes, registro legal, validación de localización y comparación de rendimiento con la build anterior.
