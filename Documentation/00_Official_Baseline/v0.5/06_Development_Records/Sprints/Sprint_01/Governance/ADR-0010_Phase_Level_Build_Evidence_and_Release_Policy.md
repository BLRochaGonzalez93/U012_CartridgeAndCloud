# ADR-0010 — Phase-Level Build Evidence and Release Policy

**Status:** Accepted  
**Date:** 2026-06-23

## Context

Creating a checksum, tag, release package and full release evidence for every implementation sprint adds administrative overhead without improving the safety of ordinary incremental development. Sprint builds are primarily smoke and integration checks. The build that closes a phase has a different purpose: it is the reproducible milestone artifact used for archival, review and release evidence.

`Player.log` is useful when diagnosing a failure, crash or discrepancy, but reviewing it after every successful routine run creates unnecessary ceremony.

## Decision

### Ordinary sprint closure

Each sprint requires, when applicable:

- clean Unity compilation;
- automated tests relevant to the changed systems;
- acceptance criteria validation;
- a local Windows development build when Player-specific behavior changed;
- successful external execution;
- operational QA and traceability closure.

An ordinary sprint does **not** require:

- ZIP packaging of the build;
- SHA-256 generation;
- Git tag;
- GitHub release;
- formal release notes;
- mandatory `Player.log` review after an otherwise successful run.

### Conditional Player.log review

`Player.log` becomes mandatory when any of these conditions occurs:

- crash or forced termination;
- failed launch;
- unhandled exception;
- missing or failed scene load;
- behavior differs materially between Editor and Player;
- a defect cannot be explained from the Editor console;
- the sprint changes build initialization, native plugins, platform services or low-level persistence in a way that warrants explicit log inspection.

### Phase closure

The final build of a phase requires:

- final regression suite;
- external acceptance run;
- packaged milestone artifact;
- SHA-256 of the packaged artifact;
- phase build record;
- final `Player.log` review;
- phase tag and release record when the milestone is published;
- official documentation baseline publication.

### Documentation baseline cadence

Official baselines are published at phase boundaries. An interim baseline is allowed only after a material architecture, platform, tooling or product-direction change that makes the preceding baseline unsafe as the active reference.

Operational records remain the source of current sprint status between official baselines.

## Consequences

- Sprint implementation can advance with less administrative friction.
- Checksums remain meaningful because they identify milestone artifacts rather than disposable local builds.
- Tags and releases represent phase milestones instead of every sprint.
- Diagnostics are still mandatory when a Player failure occurs.
- Baseline v0.4 remains immutable; the next regular baseline is expected at the end of the current vertical-slice phase.
