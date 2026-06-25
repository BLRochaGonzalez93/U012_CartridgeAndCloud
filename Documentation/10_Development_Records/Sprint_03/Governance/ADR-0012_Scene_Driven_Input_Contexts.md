# ADR-0012 — Scene-Driven Input Contexts

**Status:** Accepted  
**Sprint:** 3  
**SubSprint:** CC_S3.1  
**Accepted:** 2026-06-25  
**Technical commits:** `9acb74af4946c9a2f889d9e2b9180700ff3807d1`, `fe3f83ce9d52ded6944f4b57b3b0b6724ceeb7a6`

## Decision

The persistent ApplicationRoot owns one `IInputContextService`.

| Scene | Context |
|---|---|
| Bootstrap | None |
| MainMenu | UI |
| Store | Gameplay |
| TestLab | Gameplay |

Consumers receive the service through `IInputContextConsumer`.

## Rationale

Movement and camera inputs must not remain active while navigating menus.
Central context ownership prevents scene scripts and gameplay consumers from
independently enabling conflicting input behaviour.

## Validation

- InputContextService unit coverage passed.
- Scene-driven context coverage passed.
- ApplicationRoot persisted through scene changes.
- UI and Gameplay Input Action maps followed the active context exclusively.
- Full automated suite completed `66/66 PASS`.
- External Bootstrap → MainMenu → Store → MainMenu flow passed.

## Deferred

Rebinding UI, saved binding overrides and accessibility presets remain deferred.
