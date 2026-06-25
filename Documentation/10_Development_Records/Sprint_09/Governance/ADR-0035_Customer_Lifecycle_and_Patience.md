# ADR-0035 — Ciclo de vida y paciencia

**Estado:** Accepted

El estado mínimo es `WaitingToEnter → Entering → Browsing → Leaving → Despawned`. La paciencia se consume únicamente durante `Browsing`; al llegar a cero el cliente cambia a `Leaving` y su objetivo activo pasa a Exit. Sprint 9 no interpreta la paciencia como abandono de carrito porque el carrito pertenece a Sprint 10.
