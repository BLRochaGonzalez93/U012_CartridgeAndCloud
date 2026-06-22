# Sprint 01 — Validation Checklist

## Before integration

- [ ] Current branch is `main`.
- [ ] HEAD is the approved Sprint 0 baseline or the deviation is documented.
- [ ] GitHub Desktop shows no unrelated local changes.
- [ ] Unity is closed.
- [ ] A local backup exists.

## Integration

- [ ] The contents of `ProjectOverlay` were copied into the repository root.
- [ ] Unity imported the new scripts.
- [ ] `VRM Games > Cartridge & Cloud > Sprint 1 > Validate prerequisites` passed.
- [ ] `Apply Bootstrap & Scene Flow` completed.
- [ ] Temporary `Assets/_S1_TEMP` folder was deleted from the Unity Project window before final tests and commit.

## Compilation and scenes

- [ ] Console has zero red errors.
- [ ] Bootstrap contains one ApplicationRoot.
- [ ] MainMenu contains camera, light, controller, Canvas and EventSystem.
- [ ] Store contains camera, light, controller, Canvas and EventSystem.
- [ ] TestLab remains unchanged.
- [ ] Scene order remains 0–3.
- [ ] Application version is `0.0.2`.

## Automated QA

- [ ] EditMode `6/6 PASS`.
- [ ] PlayMode `8/8 PASS`.
- [ ] Full suite repeated after closing and reopening Unity.
- [ ] Full suite repeated after the Windows build.

## Manual QA

- [ ] Ten MainMenu -> Store -> MainMenu loops.
- [ ] Rapid repeated clicks do not duplicate transitions.
- [ ] Exactly one ApplicationRoot exists after each loop.
- [ ] Quit exits the external Player cleanly.

## Build and repository

- [ ] Windows x64 development build succeeded.
- [ ] External Player run passed.
- [ ] Player.log reviewed.
- [ ] Build output remains outside Git.
- [ ] QA and traceability records updated.
- [ ] Documentation impact reviewed.

## Result

- [ ] PASS
- [ ] PASS WITH FORMAL EXCEPTION
- [ ] FAIL
