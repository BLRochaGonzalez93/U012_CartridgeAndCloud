# ADR-0068 — Pure Phase 1 sidecar

**Estado:** Accepted

Furniture placement data not represented by the integrated schema is persisted
in a pure JSON sidecar. It contains no Unity references and is bound to the
active SessionId.
