# ADR-0031 — Display capacity and visible units

**Status:** Accepted for Sprint 8

Capacity is measured in total logical units. `VisibleUnitLimit` is separate and cannot exceed capacity. Visible units are derived as `min(stock, visible limit)`.

Inventory remains authoritative; presentation may pool or aggregate visuals later.
