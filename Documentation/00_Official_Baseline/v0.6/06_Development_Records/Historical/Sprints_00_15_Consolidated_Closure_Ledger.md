# Sprints 00-15 - Consolidated Closure Ledger

**Baseline:** v0.6  
**Purpose:** provide enough historical implementation truth for a new development chat without consulting previous baselines or chat memory.

## Status rule

Sprints 0-15 are treated as `CLOSED / PASS`. This ledger summarizes their authoritative outcome. Detailed historical records remain useful archival evidence, but are not required to continue Sprint 16.

| Sprint | Name | Application version | Accepted result |
|---:|---|---:|---|
| 0 | Project Foundation | 0.0.1 | Unity/URP project, repository, assemblies, scene skeleton, test/build foundation |
| 1 | Bootstrap & Scene Flow | 0.0.2 | Persistent ApplicationRoot and Bootstrap/MainMenu/Store navigation |
| 2 | Core Data & Save Skeleton | 0.0.3 | Stable IDs, session, three slots and minimal versioned snapshot |
| 3 | Player Movement & Camera | 0.0.4 | Input contexts, click-to-move, orbit and zoom |
| 4 | Grid & Placement Foundation | 0.0.5 | 0.5 m grid, footprints, preview, rotation, occupancy and removal |
| 5 | Store Shell & Access Validation | 0.0.6 | 10x15 m Store, 20x30 grid, entrance reservation and route validation |
| 6 | Product & Inventory Core | 0.0.7 | Product definitions, quantities, containers and atomic transfers |
| 7 | Supplier Orders & Receiving | 0.0.8 | Suppliers, purchase orders, deliveries, boxes and receiving |
| 8 | Displays & Restocking | 0.0.9 | Display definitions, assignment, capacity, visible stock and restocking |
| 9 | Customer Profiles & Spawning | 0.0.10 | Profiles, deterministic spawning, navigation and patience |
| 10 | Shopping & Reservations | 0.0.11 | Shopping intent, reservations, cart and stock protection |
| 11 | Queue & Checkout | 0.0.12 | FIFO queue, station lifecycle and authoritative checkout |
| 12 | Day Cycle & Store Closure | 0.0.13 | BeforeOpen/Open/Closing/Closed states and deterministic logical time |
| 13 | Economy & Reports | 0.0.14 | Exact Money model, costs, sale prices, ledger and daily results |
| 14 | Save/Load Complete Slice | 0.0.15 | Integrated snapshot, backup, recovery, repair and schema evolution |
| 15 | UI/UX Integration | 0.0.16 | Three-slot flow, Store HUD, Operations panels, tutorial and accessibility |

## Known implementation commits for later sprints

| Sprint | Commit | Message / evidence role |
|---:|---|---|
| 6 | `dad686c0ebefbba01afbcca994fdc769011e65a6` | Sprint 6 implementation |
| 7 | `b997377eae720e960c02ee5dce53da8b37f66b19` | complete Sprint 7 supplier orders and receiving |
| 8 | `409b7fe8653aa12ece7d484eb414172e1ed38f70` | complete Sprint 8 displays and restocking |
| 9 | `b8ddf15356c73cd7d0c88a32805f0c6ee4058422` | complete Sprint 9 customer profiles and spawning |
| 10 | `f91c8fda6c1ff1ff9e811717b05fc8fa427707e6` | complete Sprint 10 shopping and reservations |
| 11 | `08ec92ba97f4070399f01023c32f49327a951712` | complete Sprint 11 queue and checkout |
| 12 | `2d6b846120dc86536c19c12d07e5b515f7baa8fd` | complete Sprint 12 day cycle and closure |
| 13 | `5a9b80cdce3d6d619d64932838d0b0d7d77bb66d` | complete Sprint 13 economy and daily results |
| 14 | `ebaf05d81a9ea18959413fd3bf25169a437c3435` | complete Sprint 14 save integration and recovery |
| 15 | `fb116945e359838c2175eed46ebb9c48aa6aecb3` | complete Sprint 15 UI/UX integration |

## Regression inheritance

The current accepted automated baseline is larger than the historical per-sprint totals and covers the closed systems together:

- EditMode: 1215 PASS.
- PlayMode: 70 PASS.
- Total: 1285 PASS.

This does not close Sprint 16 because its representative visual acceptance and post-migration build remain open.
