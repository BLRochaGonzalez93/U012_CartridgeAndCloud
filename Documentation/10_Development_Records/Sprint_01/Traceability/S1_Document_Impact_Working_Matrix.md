# Sprint 01 — Document Impact Result

| Document | Impact | Closure action |
|---|---|---|
| Guide | Project state and visible flow changed | Operational delta recorded; baseline update deferred to phase close |
| Project Binder | Current implementation state changed | Operational delta recorded; baseline update deferred to phase close |
| TDD | Bootstrap composition root and scene navigation implemented | Implementation delta recorded below |
| Unity Setup Guide | Bootstrap/MainMenu/Store contents changed | Restoration delta recorded below |
| Build & Versioning Guide | Version 0.0.2 and evidence cadence changed | ADR-0010 accepted; baseline update deferred to phase close |
| Roadmap / Sprint Plan | Sprint 1 completed | Sprint 1 Closed/PASS in operational records |
| QA Testing Plan | Suite expanded to 14 tests | QA execution record created |
| QA Matrix | Sprint 1 acceptance completed | Acceptance matrix closed PASS |
| Production workbook | Sprint status changed | Consolidate at phase-close workbook update |
| Traceability workbook | Changes and ADR added | CSV operational trace closed; consolidate at phase close |
| Vertical Slice Specification | Entry conditions progressed | Record S1 complete; no scope change |
| GDD / Art / Audio / Business | No functional impact | Reviewed — No Change |

## Technical documentation delta

- `Bootstrap` is the sole production entry scene.
- `ApplicationRoot` persists across scene changes and owns the scene navigator.
- `SceneId`, `ISceneNavigator`, transition result and gate live in Application without Unity dependencies.
- Unity scene loading lives in Infrastructure.
- MainMenu and Store controllers live in Presentation.
- Normal flow is Bootstrap → MainMenu → Store → MainMenu → Quit.
- TestLab remains isolated from the normal player flow.
- Scene order remains indexes 0–3.
- Test baseline is now 6 EditMode + 8 PlayMode.
- Application version is `0.0.2`.

## Baseline decision

Official baseline v0.4 remains immutable. Under ADR-0010, the next regular official baseline is published at the end of the current phase. No v0.5 per-sprint baseline is required.
