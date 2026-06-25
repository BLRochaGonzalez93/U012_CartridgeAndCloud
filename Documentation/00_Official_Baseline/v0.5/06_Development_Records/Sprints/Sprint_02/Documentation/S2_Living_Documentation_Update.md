# Sprint 02 — Living Documentation Update

## Technical design

The project now has an active Domain layer containing stable identifiers,
a minimal GameSession aggregate and a versioned snapshot. Application defines
use-case contracts and orchestration. Infrastructure owns Unity paths, JSON
serialization and physical slot files.

## Data model

Current minimal persisted fields:

| Field | Type | Rule |
|---|---|---|
| schemaVersion | int | Must equal 1 |
| sessionId | StableId | GUID in N format |
| slot | SaveSlotId | 0–2 |
| createdUtcTicks | long | UTC DateTime ticks |
| updatedUtcTicks | long | UTC and not earlier than creation |
| currentDay | int | At least 1 |
| cashCents | long | Minimal monetary placeholder |

## Save skeleton

- Storage root: `Application.persistentDataPath/SaveGames`.
- Slot files: `slot_0.json`, `slot_1.json`, `slot_2.json`.
- Existing slot writes use a temporary file and replacement.
- Bootstrap creates a memory-only slot-0 session.
- No autosave or startup write occurs.

## Deferred capabilities

The following remain intentionally absent:

- Save/load UI.
- Autosave.
- Rotating backups.
- Corruption recovery policy.
- Migration beyond schema v1.
- Cloud saves.
- Complete gameplay and world-state persistence.

## Baseline policy

Official baseline v0.4 remains immutable.
The next official baseline is deferred to the final closure of the current phase,
unless a material governance exception is approved.
