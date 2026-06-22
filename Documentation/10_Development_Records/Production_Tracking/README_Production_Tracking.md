# Production Tracking — Operational Layer

## Versión vigente para S0.8

- `Cartridge_And_Cloud_Production_Tracking_Operational_v0.3.4.xlsx`
- `Cartridge_And_Cloud_QA_Operational_v0.3.4.xlsx`
- `Cartridge_And_Cloud_Traceability_Operational_v0.3.4.xlsx`

La versión v0.3.4 registra S0.7 como cerrado y S0.8 como `In Progress`.

Los libros v0.3.3 deben retirarse del árbol de trabajo. El historial Git conserva
su versión de cierre.

## Regla

S0.8 no se marcará PASS hasta ejecutar dos veces la suite completa con:

```text
EditMode 5/5
PlayMode 4/4
Console limpia
```
