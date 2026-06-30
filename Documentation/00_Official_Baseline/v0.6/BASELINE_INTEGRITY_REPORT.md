# Baseline Integrity Report - v0.6

**Date:** 2026-07-01  
**Status:** PASS

## Identity

- Project: Cartridge & Cloud.
- Documentation baseline: v0.6.
- Application version recorded: 0.0.17.
- Last validated technical commit: `091090c43855b0b26b09abe9335d18b978ac7eab`.
- Latest observed main commit: `d54316c771aab2143993e99b9fd58f2f88016568`.

## Integrity controls

- All final files are inventoried in `BASELINE_MANIFEST.csv` except the manifest and checksum file themselves.
- SHA-256 hashes are recorded in `CHECKSUMS_SHA256.txt`.
- PDF open/render validation: PASS.
- Spreadsheet structure and formula-error validation: PASS.
- No temporary render or preview files are stored inside the baseline folder.
- Previous baselines remain external and immutable.

## Project-state caveat

Documentation integrity is PASS. Sprint 16 remains IN PROGRESS because Store visual acceptance, StoreInitial migration, post-migration Golden Path and post-migration build are still open.
