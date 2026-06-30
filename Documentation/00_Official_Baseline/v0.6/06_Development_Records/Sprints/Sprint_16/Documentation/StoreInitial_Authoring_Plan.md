# StoreInitial Authoring Plan

1. Crear `StoreInitial.unity` como copia de Store funcional.
2. Crear `StoreInitialEnvironment.prefab`.
3. Montar suelo y arquitectura manualmente.
4. Configurar puerta manualmente.
5. Colocar mobiliario inicial.
6. Añadir anchors y roots técnicos.
7. Crear `StoreInitialSceneContext`.
8. Conectar runtime.
9. Desactivar generación procedural del shell.
10. Corregir supresión de clic sobre UI.
11. Validar tests, Golden Path y build.

## Jerarquía recomendada

```text
StoreInitialEnvironment
├── Architecture
├── InitialFurniture
├── Lighting
├── Anchors
└── TechnicalColliders
```

No incluir EventSystem, HUD, cámara, managers globales, save services o ApplicationRoot.
