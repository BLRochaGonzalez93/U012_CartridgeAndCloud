# ADR-0057 — Validated atomic write

**Estado:** Accepted

El payload se escribe en `.tmp`, se fuerza a disco, se vuelve a cargar y solo
después sustituye al primario. El primario anterior permanece como `.bak`.
