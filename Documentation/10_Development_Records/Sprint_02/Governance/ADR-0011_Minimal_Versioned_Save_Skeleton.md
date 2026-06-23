# ADR-0011 — Minimal Versioned Save Skeleton

**Status:** Accepted  
**Sprint:** 2  
**Accepted:** 2026-06-23  
**Technical commit:** `c55b5d04a150c2fc452e37ff0fc5be3f9633e80d`

## Decision

Sprint 2 establishes a versioned, JSON-backed save skeleton with:

- Pure Domain session and snapshot types.
- Stable identifiers represented by 32-character GUID values in N format.
- Three explicit save slots: 0, 1 and 2.
- Application-layer session service, repository and UTC clock contracts.
- Infrastructure JSON persistence below `Application.persistentDataPath/SaveGames`.
- Schema version 1.
- UTC timestamps stored as ticks.
- Temporary-file creation followed by atomic replacement of an existing slot file.
- One persistent `GameSessionService` composed by `ApplicationRoot`.

Bootstrap creates a new in-memory session in slot 0 and does not autosave it.

## Architectural boundaries

- Domain has no UnityEngine dependency.
- Application depends on Domain but not Infrastructure or Presentation.
- Infrastructure implements persistence and Unity-specific path/JSON concerns.
- Presentation receives session access only through `IGameSessionConsumer`.
- Scene objects are not serialized as save-state authorities.

## Rationale

This provides deterministic identifiers, explicit persistence boundaries and a
minimal restoration model without coupling future gameplay systems to scenes,
MonoBehaviours or Unity serialization.

## Validation

- Domain and Application dependency boundaries compiled successfully.
- Save slot range and stable identifier invariants passed automated tests.
- Snapshot restoration preserved session ID, slot, day and cash.
- JSON round-trip and delete lifecycle passed.
- Bootstrap session composition and scene-transition persistence passed.
- Full automated suite completed `29/29 PASS`.
- Windows x64 build and external execution passed.

## Resolved implementation incident

Package v001 used the namespace
`VRMGames.CartridgeAndCloud.Application.GameSession` alongside the Domain type
`VRMGames.CartridgeAndCloud.Domain.GameSession.GameSession`. Unity resolved the
short name ambiguously during compilation.

Hotfix v002 added explicit aliases for the Domain `GameSession` and snapshot types.
Compilation and the full validation sequence then passed. The incident is closed.

## Deferred

Autosave, backup rotation, save migration beyond schema v1, save/load UI,
cloud saves, encryption, compression and complete vertical-slice state remain
deferred to later sprints.
