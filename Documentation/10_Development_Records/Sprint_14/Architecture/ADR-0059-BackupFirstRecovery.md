# ADR-0059 — Backup-first recovery

**Estado:** Accepted

Si el primario no es válido se intenta el backup. Un backup válido se devuelve,
repara el primario y marca la operación como RecoveredFromBackup.
