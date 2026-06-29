# ADR-0073 — Completed checkout snapshot records

**Estado:** Accepted

A completed Phase 1 sale persists:

- customer Despawned;
- shopping session CheckedOut;
- reservation Consumed;
- transaction Completed;
- empty final queue;
- available station;
- checkout revenue ledger posting.

This satisfies integrated snapshot cross-reference validation and allows a
closed day.
