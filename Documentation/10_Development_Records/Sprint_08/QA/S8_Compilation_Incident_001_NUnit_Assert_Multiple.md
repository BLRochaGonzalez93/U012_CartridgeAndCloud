# S8 Compilation Incident 001 — NUnit Assert.Multiple Compatibility

**Date:** 2026-06-25  
**Sprint:** 8 — Displays & Restocking  
**Severity:** Build-blocking test compilation issue  
**Production-code impact:** None  
**Package affected:** `Cartridge_And_Cloud_Sprint_08_Displays_Restocking_Manual_v1.0`  
**Correction:** package v1.1 / manual compilation fix  
**Final status:** CLOSED / PASS

## Observation

Unity entered Safe Mode during project import because the NUnit assembly
available to the project does not expose `Assert.Multiple`.

The compiler reported `CS0117` in seven Sprint 8 EditMode test files. Twenty
grouped assertion blocks were affected.

## Root cause

The generated Sprint 8 tests used an NUnit convenience API that is not
available in the NUnit framework version referenced by the existing Unity
EditMode test assembly.

The assertions themselves and the production implementation were not invalid.
The incompatibility occurred before Test Runner execution.

## Correction

All twenty `Assert.Multiple` wrappers were replaced with sequential
`Assert.That` statements.

Corrected files:

- `DisplayAuthoringAssetTests.cs`
- `DisplayInstanceRegistryTests.cs`
- `DisplayInstanceTests.cs`
- `DisplayRestockServiceTests.cs`
- `DisplayReturnStockServiceTests.cs`
- `DisplayRuntimeAuthoringTests.cs`
- `RestockTaskTests.cs`

No production source, asset, scene, asmdef, ProjectSettings or `.meta` file was
changed by this correction.

## Verification

- Static replacement check: PASS.
- Remaining `Assert.Multiple` references in Sprint 8 tests: `0`.
- Unity recompilation after correction: PASS.
- Focused Sprint 8 tests: `74/74 PASS`.
- Full EditMode: `333/333 PASS`.
- Full PlayMode: `41/41 PASS`.
- Full automated baseline: `374/374 PASS`.

## Closure decision

**CLOSED / PASS.** The incident was limited to test-framework compatibility,
was corrected without changing production behavior and introduced no reduction
in automated coverage.
