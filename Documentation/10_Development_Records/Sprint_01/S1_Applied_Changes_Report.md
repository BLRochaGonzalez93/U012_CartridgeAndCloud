# S1 — Applied Changes Report

**Applied UTC:** 2026-06-22 23:11:14  
**State:** Applied locally — pending Unity validation  
**Scene backup:** `Library/CartridgeAndCloud/Sprint1Backups/20260622_231113`

## Changes applied

- Bootstrap now owns one persistent ApplicationRoot.
- Bootstrap transitions automatically to MainMenu.
- MainMenu exposes Enter Store and Quit actions.
- Store exposes Return to Main Menu.
- Global scene order remains Bootstrap, MainMenu, Store, TestLab.
- Application version prepared as 0.0.2.

## Validation still required

- Unity compilation and Console review.
- EditMode target: 6/6 PASS.
- PlayMode target: 8/8 PASS.
- Windows x64 build and external execution.
- Player.log review.
- Documentation and traceability closure.
- Temporary Assets/_S1_TEMP folder removed before final validation.
