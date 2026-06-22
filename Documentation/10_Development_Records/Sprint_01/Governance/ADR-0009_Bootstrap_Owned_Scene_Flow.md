# ADR-0009 — Bootstrap-Owned Scene Flow

**Status:** Proposed — accept only after local integration review  
**Sprint:** 1

## Context

Sprint 0 validated four scenes and a six-assembly dependency graph. Sprint 1 needs a real entry point and scene navigation without adding a seventh assembly or creating a general global service locator.

## Decision

- `Bootstrap` is the only production entry scene.
- `ApplicationRoot` lives in Infrastructure, persists through scene loads and implements `ISceneNavigator`.
- Scene-facing controllers live in Presentation and depend only on Application contracts.
- The composition root injects `ISceneNavigator` into `ISceneNavigationConsumer` components when a scene loads.
- Injection may scan the newly loaded scene only at the composition boundary.
- Scene loading uses `LoadSceneAsync` with `LoadSceneMode.Single`.
- Concurrent requests are rejected by a deterministic transition gate.
- `Loading` scene, dependency-injection framework and new asmdefs are deferred.

## Consequences

- Infrastructure and Presentation remain without direct references to each other.
- Bootstrap is no longer empty, so the Sprint 0 Bootstrap smoke test must be replaced.
- Direct play from MainMenu or Store is not the production contract for Sprint 1.
- Any future expansion of this composition pattern requires a new review rather than turning it into a generic service locator.
