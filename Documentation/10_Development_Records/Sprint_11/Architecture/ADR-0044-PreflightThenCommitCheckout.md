# ADR-0044 — Preflight then commit checkout

**Estado:** Accepted

Todas las líneas se validan antes de mutar. El commit agrupa cantidades por
display y ejecuta stock, carrito, reservas, sesión, cola, estación y transacción
sin exponer estados intermedios.
