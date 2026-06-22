# ADR-0005 — Scene Baseline and Global Build Order

**Estado:** Accepted  
**Fecha:** 2026-06-21  
**Proyecto:** Cartridge & Cloud  
**Bloque:** S0.7

## Contexto

El proyecto necesita nombres de escena estables antes de introducir carga,
navegación, smoke tests y builds. La documentación histórica contiene nombres
provisionales como `StorePrototype` o `StoreSandbox`, pero la arquitectura actual
requiere una nomenclatura única.

## Decisión

Las escenas base son:

| Índice global | Nombre | Responsabilidad |
|---:|---|---|
| 0 | `Bootstrap` | Entrada técnica y futura composición |
| 1 | `MainMenu` | Menú principal |
| 2 | `Store` | Vertical slice de la tienda |
| 3 | `TestLab` | Pruebas técnicas aisladas |

Rutas:

```text
Assets/_Project/Scenes/Bootstrap.unity
Assets/_Project/Scenes/MainMenu.unity
Assets/_Project/Scenes/Store.unity
Assets/_Project/Scenes/TestLab.unity
```

`Bootstrap` se crea vacío. Las otras tres escenas se crean con la plantilla URP
básica y se reducen a `Main Camera` y `Directional Light`.

La lista global debe conservar el orden 0–3 anterior.

## Límites

S0.7 no introduce:

- scripts de carga;
- `GameManager`;
- `DontDestroyOnLoad`;
- Canvas o EventSystem;
- jugador;
- tienda;
- navegación;
- NavMesh;
- UI;
- gameplay.

## Consecuencias

- `Bootstrap` será la única entrada técnica de futuros builds.
- `TestLab` podrá estar en builds de desarrollo y se excluirá del perfil de
  distribución cuando exista.
- Los nombres quedan congelados.
- Cambiar un nombre o el orden requiere actualizar ADR, QA, trazabilidad y
  referencias de build.
- `StorePrototype` y `StoreSandbox` quedan descartados.
