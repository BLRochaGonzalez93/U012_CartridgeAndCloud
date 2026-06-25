# ADR-0036 — Límite de navegación técnica

**Estado:** Accepted

El dominio almacena un plan lógico de puntos. Los `Transform` y el movimiento técnico viven en Infrastructure. Se usa `Vector3.MoveTowards` para validar llegada, recorrido y salida sin comprometer todavía una solución definitiva de NavMesh, avoidance o animación.
