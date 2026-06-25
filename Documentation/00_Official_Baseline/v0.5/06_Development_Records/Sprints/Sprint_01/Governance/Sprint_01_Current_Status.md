# Sprint 01 — Current Status

**State:** Closed  
**Result:** PASS  
**Closure date:** 2026-06-23  
**Application version:** `0.0.2`

## Validated result

- Unity compilation completed with zero blocking errors.
- Bootstrap owns one persistent `ApplicationRoot`.
- Bootstrap opens MainMenu automatically.
- MainMenu opens Store and Store returns to MainMenu.
- Quit closes the external Windows Player cleanly.
- Scene order remains Bootstrap, MainMenu, Store and TestLab at indexes 0–3.
- EditMode: `6/6 PASS`.
- PlayMode: `8/8 PASS`.
- Total automated suite: `14/14 PASS`.
- Windows x64 development build: PASS.
- External execution: PASS, without crash or blocking failure.
- Temporary `Assets/_S1_TEMP` integration content was removed before publication.
- Technical implementation is published on `main` in commit `feat: implement Sprint 1 bootstrap scene flow`.

## Resolved incidents

1. The first scene-installer attempt invoked Input System default-action assignment twice. The installer was corrected and the scenes were regenerated successfully.
2. The first Player build exposed a namespace collision in `Application.Quit()`. It was corrected to `UnityEngine.Application.Quit()` and the subsequent build passed.

Neither incident remains open.

## Evidence policy

- No build checksum, sprint tag or sprint release is required.
- Build SHA-256 and release evidence are reserved for the final build of the current phase.
- `Player.log` is reviewed during a sprint only when the Player crashes, fails, reports an exception or behaves differently from the Editor. No such condition occurred in the successful final run.

Sprint 01 is closed with no known S0 or S1 defect.
