# U012 Reconstruction Package

Este directorio conserva la auditoría de entrada, la trazabilidad de cada bloque reconstruido y las evidencias de validación.

El validador reproducible está en `Tools/Validation/u012_static_validate.py`. Ejemplo:

```bash
python Tools/Validation/u012_static_validate.py . --json validation.json --markdown validation.md
```

La validación estática no sustituye la apertura, compilación, tests ni build dentro de Unity.
