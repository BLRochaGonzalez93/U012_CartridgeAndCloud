# ADR-0030 — Single-product display assignment

**Status:** Accepted for Sprint 8

Each `DisplayInstance` has zero or one assigned product. Assignment must resolve through the product registry and satisfy optional category restrictions. A different product can be selected only after all stock is returned and the previous assignment is cleared.

This keeps customer lookup deterministic and avoids premature mixed-slot merchandising.
