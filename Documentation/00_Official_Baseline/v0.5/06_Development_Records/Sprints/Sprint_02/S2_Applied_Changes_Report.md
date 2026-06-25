# Sprint 02 — Applied Changes Report

**Date:** 2026-06-23  
**State:** Implemented and validated  
**Result:** PASS  
**Technical commit:** `c55b5d04a150c2fc452e37ff0fc5be3f9633e80d`

## 2.1 — Stable IDs and minimal Domain state

- Added `StableId`.
- Added `SaveSlotId` with the supported range 0–2.
- Added the minimal `GameSession` aggregate.
- Added schema-v1 `GameSessionSnapshot`.
- Added validation for UTC timestamps, day bounds and schema version.

## 2.2 — Application layer

- Added `IGameSessionService`.
- Added `GameSessionService`.
- Added `ISaveGameRepository`.
- Added `IUtcClock`.
- Added `IGameSessionConsumer`.
- Added typed operation results for success, missing session, empty slot and storage failure.

## 2.3 — Infrastructure, Bootstrap and QA

- Added `JsonSaveGameRepository`.
- Added `SystemUtcClock`.
- Added JSON files per slot below `SaveGames`.
- Added temporary-file and replacement handling.
- Extended `ApplicationRoot` to compose the session service.
- Bootstrap now creates one in-memory slot-0 session.
- Added 13 EditMode and 2 PlayMode test cases.
- Advanced application version to `0.0.3`.

## Resolved incident

The initial package v001 produced compilation errors because the Application
namespace `GameSession` collided with the Domain class named `GameSession`.

Hotfix v002 introduced explicit type aliases in:

- `IGameSessionService.cs`
- `GameSessionService.cs`

The project then compiled cleanly and all validation passed.

## Validation

- EditMode: `19/19 PASS`.
- PlayMode: `10/10 PASS`.
- Full suite: `29/29 PASS`.
- Editor scene flow: PASS.
- Windows x64 development build: PASS.
- External execution and Quit: PASS.
- Player.log review: not required by the phase evidence policy.

## Deferred scope

No save/load UI, autosave, migration pipeline, backup rotation, cloud storage,
encryption, compression or complete gameplay snapshot was introduced.
