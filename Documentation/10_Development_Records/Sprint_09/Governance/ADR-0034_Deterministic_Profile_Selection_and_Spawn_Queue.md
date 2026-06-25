# ADR-0034 — Selección determinista y cola de spawn

**Estado:** Accepted

La selección usa pesos enteros y un `roll` explícito. La cola es FIFO, rechaza IDs de solicitud o instancia duplicados y solo se consume cuando el spawn se confirma. Esto permite tests reproducibles y evita aleatoriedad oculta.
