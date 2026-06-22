# Sprint 01 — Bootstrap & Scene Flow Charter

**Project:** Cartridge & Cloud  
**Owner:** VRM Games / Blas Luis Rocha González  
**Baseline authority:** `Documentation/00_Official_Baseline/v0.4`  
**Starting commit:** `9574dc217fd56ff8c867c04724ae2db23cf85f99`  
**Initial state:** Ready for integration — no implementation or validation claimed

## Objective

Deliver the first visible technical loop:

```text
Bootstrap -> MainMenu -> Store -> MainMenu -> Quit
```

## In scope

- One persistent application composition root.
- Centralized scene identifiers.
- Asynchronous single-scene transitions.
- MainMenu technical UI.
- Store technical shell and return action.
- Repeated-transition protection.
- Automated EditMode and PlayMode coverage.
- Windows x64 validation and operational documentation.

## Out of scope

- Player movement.
- Camera controls.
- NavMesh and world interaction.
- Grid and placement.
- GameSession, persistence or save slots.
- Representative art/audio.
- Final menu UX or localization pass.

## Closure gate

- EditMode `6/6 PASS`.
- PlayMode `8/8 PASS`.
- Windows x64 external run PASS.
- Player.log without crashes or unhandled exceptions.
- No open S0/S1 issues.
- QA, traceability, ADR and document-impact review updated.
- Build artifacts remain outside Git.
