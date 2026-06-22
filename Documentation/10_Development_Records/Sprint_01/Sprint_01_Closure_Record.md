# Sprint 01 — Closure Record

**Project:** Cartridge & Cloud  
**Sprint:** 01 — Bootstrap & Scene Flow  
**State:** Closed  
**Result:** PASS  
**Closure date:** 2026-06-23  
**Application version:** `0.0.2`

## Delivered scope

- Bootstrap-owned persistent `ApplicationRoot`.
- Automatic Bootstrap to MainMenu transition.
- Minimal MainMenu with Enter Store and Quit actions.
- Minimal Store scene with Return to MainMenu action.
- Central scene identifiers and navigation contract.
- Asynchronous scene navigation with concurrent-transition rejection.
- TestLab preserved outside the normal player flow.
- Scene indexes preserved at 0–3.

## Validation summary

| Check | Result |
|---|---|
| Unity compilation | PASS |
| EditMode | `6/6 PASS` |
| PlayMode | `8/8 PASS` |
| Full suite | `14/14 PASS` |
| Windows x64 development build | PASS |
| External scene-flow run | PASS |
| External Quit | PASS |
| Crash or blocking failure | None observed |
| Open S0/S1 defect | None known |

## Resolved incidents

1. Temporary installer Input System action assignment failure — corrected before final scene generation.
2. Player-only `Application.Quit()` namespace collision — corrected to `UnityEngine.Application.Quit()`; rebuild passed.

## Repository result

The implementation is published on `main` in the commit named:

```text
feat: implement Sprint 1 bootstrap scene flow
```

`Assets/_S1_TEMP` was removed before final validation. Build output remains outside Git.

## Administrative result

- No per-sprint build checksum.
- No Sprint 1 tag or release.
- No mandatory Player.log review because final external execution completed without a failure trigger.
- Official baseline v0.4 remains immutable.
- Next regular official baseline is deferred to phase closure under ADR-0010.

**Final decision:** Sprint 01 Closed / PASS.
