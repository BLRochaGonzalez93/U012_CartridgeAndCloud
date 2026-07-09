# Sprint 17 Phase 6 — Build Profile Matrix

## Scope

This document records the implemented Windows build-profile differences for Sprint 17 Phase 6. The canonical Build and Versioning Guide remains scheduled for the global documentation pass in Phase 9.

## Shared Windows player baseline

- Target: Windows Standalone x64.
- Scripting backend: Mono.
- Default display: 1920×1080, borderless full screen, resizable window.
- Player log: enabled.
- Run in background: disabled.
- Global frame timing statistics: disabled.
- Company/product identity remains `VRM Games` / `Cartridge & Cloud`.

## Profile matrix

| Setting | Windows_Development | Windows_QA | Windows_H6_Candidate |
|---|---:|---:|---:|
| Development Build | Yes | Yes | No |
| Production scenes | Yes | Yes | Yes |
| TestLab | Yes | Yes | No |
| Autoconnect Profiler | No | No | No |
| Deep Profiling | No | No | No |
| Script Debugging | No | No | No |
| Wait for Managed Debugger | No | No | No |
| Copy PDB files | No | No | No |
| Create solution | No | No | No |
| Unity diagnostic data | Disabled | Disabled | Disabled |
| Compression | LZ4 | LZ4 | LZ4 |
| Profile define | `CC_BUILD_DEVELOPMENT` | `CC_BUILD_QA` | `CC_BUILD_H6_CANDIDATE` |

## Scene order

Development and QA:

1. `Bootstrap`
2. `MainMenu`
3. `StoreInitial`
4. `TestLab`

H6 Candidate:

1. `Bootstrap`
2. `MainMenu`
3. `StoreInitial`

## Decisions

- The formal QA profile does not inherit `CC_SPRINT17_PHASE3_QA`; the Phase 3 performance build remains a dedicated specialist build.
- TestLab is retained for Development and internal QA only.
- H6 Candidate is configured and validated in Phase 6, but the immutable candidate artifact, checksum and final manifest are produced in Phase 7.
- Player settings are shared globally because all three Windows profiles require the same player identity, save location, display baseline and Player.log behavior.
