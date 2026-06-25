# S1 — Applied Changes Report

**State:** Closed  
**Result:** PASS  
**Closure date:** 2026-06-23

## Changes applied

- Bootstrap owns one persistent ApplicationRoot.
- Bootstrap transitions automatically to MainMenu.
- MainMenu exposes Enter Store and Quit actions.
- Store exposes Return to Main Menu.
- Scene identifiers and navigation contracts are centralized in Application.
- Unity scene loading is implemented in Infrastructure.
- Presentation controllers receive navigation through the composition boundary.
- Concurrent scene-transition requests are rejected.
- Global scene order remains Bootstrap, MainMenu, Store, TestLab.
- Application version is `0.0.2`.
- Automatic suite expanded from 9 to 14 tests.

## Final validation

- Unity compilation: PASS.
- EditMode: `6/6 PASS`.
- PlayMode: `8/8 PASS`.
- Windows x64 development build: PASS.
- External execution and clean Quit: PASS.
- Temporary `Assets/_S1_TEMP`: removed.
- No open blocking defect.

## Resolved implementation issues

- Corrected duplicate Input System default-action assignment in the temporary installer.
- Corrected the Player-only namespace collision by using `UnityEngine.Application.Quit()`.

## Evidence disposition

No per-sprint build checksum, tag or release is produced. Phase-final evidence follows ADR-0010.
