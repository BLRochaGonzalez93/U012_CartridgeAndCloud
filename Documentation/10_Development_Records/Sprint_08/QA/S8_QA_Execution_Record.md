# Sprint 08 — QA Execution Record

## Environment

- Unity version: `6000.3.18f1`
- Operating system: Windows
- Application version: `0.0.9`
- Date: 2026-06-25
- Dependency baseline: Sprint 7 commit `b997377eae720e960c02ee5dce53da8b37f66b19`
- Commit under test: local Sprint 8 working tree; closure commit SHA to be recorded after publication
- Evidence source: developer execution in Unity Editor and external Player

## Compilation

- Initial import: FAIL because the project NUnit version did not expose `Assert.Multiple`.
- Correction: sequential compatible `Assert.That` statements in seven test files.
- Recompilation after correction: PASS.
- Blocking Console errors after correction: none reported.

## Automated execution

| Suite | Expected | Passed | Failed | Skipped | Result |
|---|---:|---:|---:|---:|---|
| Sprint 8 focused EditMode | 74 | 74 | 0 | 0 | PASS |
| Full EditMode | 333 | 333 | 0 | 0 | PASS |
| Full PlayMode | 41 | 41 | 0 | 0 | PASS |
| Full automated baseline | 374 | 374 | 0 | 0 | PASS |

## Manual authoring validation

- Single Shelf definition: PASS.
- Countertop Rack definition: PASS.
- Floor Stand definition: PASS.
- Technical display catalog: PASS.
- Missing scripts or broken serialized references: none reported.
- Runtime-authoring smoke validation: PASS.

## Manual regression

- Bootstrap: PASS.
- MainMenu: PASS.
- Store normal and direct entry: PASS.
- Movement: PASS.
- Camera: PASS.
- Placement: PASS.
- Access validation: PASS.
- TestLab: PASS.
- Return flow: PASS.

## Build

- Profile: Windows x64 Development.
- Build result: PASS.
- External launch: PASS.
- Application version: `0.0.9`.
- External smoke validation: PASS.

## Defects

- Open S0: 0.
- Open S1: 0.
- Other blocking defects: none reported.
- Resolved during sprint: NUnit `Assert.Multiple` compatibility incident.

## Final QA recommendation

**CLOSED / PASS.** Sprint 8 satisfies its acceptance gates and Sprint 9 may open
against the `374/374 PASS` automated baseline.
