# ADR-0042 — Strict FIFO checkout queue

**Estado:** Accepted

Solo la primera entrada puede ser llamada, procesada o completada. La posición
es 1-based y se recalcula después de completar o cancelar.
