# Sprint 02 — Core Data & Save Skeleton

**State:** Ready for integration  
**Application target:** `0.0.3`

## SubSprints

### 2.1 — Stable IDs and minimal domain state
- StableId.
- SaveSlotId with slots 0–2.
- GameSession aggregate.
- Versioned GameSessionSnapshot.

### 2.2 — Session application layer
- IGameSessionService.
- GameSessionService.
- UTC clock abstraction.
- Save repository contract.
- Consumer injection contract.

### 2.3 — JSON slot persistence and validation
- JSON repository under Application.persistentDataPath/SaveGames.
- Atomic temporary-file write.
- Bootstrap creates one in-memory slot-0 session.
- EditMode and PlayMode coverage.
- Windows development build and external execution.

### 2.4 — Post-implementation documentation
Executed only after 2.1–2.3 pass:
- QA execution record.
- Acceptance closure.
- Traceability closure.
- Document impact update.
- Sprint closure record.

## Out of scope
- Save/load UI.
- Autosave.
- Backup rotation.
- Migration beyond schema v1.
- Inventory, customers, economy or store layout persistence.
- Encryption, compression or cloud saves.
