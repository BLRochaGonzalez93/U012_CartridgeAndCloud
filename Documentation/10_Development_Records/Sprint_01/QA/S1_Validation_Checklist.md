# Sprint 01 — Validation Checklist

## Integration

- [x] Integration content copied into the repository.
- [x] Unity imported the new scripts.
- [x] Prerequisite validation completed.
- [x] Bootstrap & Scene Flow application completed.
- [x] Temporary `Assets/_S1_TEMP` folder removed before final publication.

## Compilation and scenes

- [x] Console has zero blocking errors.
- [x] Bootstrap contains one ApplicationRoot.
- [x] MainMenu contains its technical scene-flow UI and EventSystem.
- [x] Store contains its technical scene-flow UI and EventSystem.
- [x] TestLab remains outside the normal player flow.
- [x] Scene order remains 0–3.
- [x] Application version is `0.0.2`.

## Automated QA

- [x] EditMode `6/6 PASS`.
- [x] PlayMode `8/8 PASS`.
- [x] Total suite `14/14 PASS` in the final implementation state.

## Manual and Player QA

- [x] Bootstrap opens MainMenu.
- [x] MainMenu opens Store.
- [x] Store returns to MainMenu.
- [x] Transition concurrency protection is covered by automated tests.
- [x] Windows x64 development build succeeded.
- [x] External Player run passed.
- [x] Quit closed the external Player cleanly.
- [x] No crash or blocking failure was observed.
- [x] `Player.log` review was not triggered under ADR-0010.

## Repository and documentation

- [x] Build output remains outside Git.
- [x] No temporary integration folder remains.
- [x] QA and traceability records updated.
- [x] Documentation impact reviewed.
- [x] No per-sprint checksum, tag or release required under ADR-0010.

## Result

- [x] PASS
- [ ] PASS WITH FORMAL EXCEPTION
- [ ] FAIL
