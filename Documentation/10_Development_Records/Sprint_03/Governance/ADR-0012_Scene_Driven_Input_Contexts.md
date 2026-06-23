# ADR-0012 — Scene-Driven Input Contexts

**Status:** Proposed  
**Sprint:** 3  
**SubSprint:** CC_S3.1

## Decision

The persistent ApplicationRoot owns one IInputContextService.

Scene policy:

| Scene | Context |
|---|---|
| Bootstrap | None |
| MainMenu | UI |
| Store | Gameplay |
| TestLab | Gameplay |

Consumers receive the service through IInputContextConsumer.

## Rationale

Movement and camera inputs must not remain active while navigating menus.
Establishing the context boundary before creating actions prevents scene scripts
from enabling and disabling Input System maps independently.

## Deferred

Actual InputAction maps, bindings, pointer actions, movement and camera controls
remain deferred to CC_S3.2–CC_S3.4.
