# ADR-0063 — Closed-day autosave idempotency

**Estado:** Accepted

El autosave solo acepta DayCycle Closed y registra el último DayId guardado por
slot. Señales duplicadas no vuelven a escribir.
