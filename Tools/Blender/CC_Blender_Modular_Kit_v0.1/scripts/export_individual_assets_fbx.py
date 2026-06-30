"""Cartridge & Cloud — exportador FBX individual v0.1.3

Exporta cada asset raíz de las colecciones:
- CC_ARCHITECTURE
- CC_FURNITURE
- CC_PRODUCTS
- CC_EXPANSIONS_CONCEPTUAL

Cada asset se guarda como FBX independiente dentro de:
Production/Exports/Architecture
Production/Exports/Furniture
Production/Exports/Products
Production/Exports/Expansions

Compatible con Blender 3.x y 4.x.
"""

from __future__ import annotations

import re
import sys
from pathlib import Path
from typing import Iterable

import bpy


# ---------------------------------------------------------------------------
# CONFIGURACIÓN
# ---------------------------------------------------------------------------

KIT_ROOT = Path(
    r"D:\_PROGRAMAS\_ARCHIVOS\0_GameDevPortfolio"
    r"\U012_CartridgeAndCloud\U012_CartridgeAndCloud"
    r"\Tools\Blender\CC_Blender_Modular_Kit_v0.1"
)

PRODUCTION_ROOT = KIT_ROOT / "Production"
EXPORT_ROOT = PRODUCTION_ROOT / "Exports"

FAMILY_COLLECTIONS = {
    "CC_ARCHITECTURE": "Architecture",
    "CC_FURNITURE": "Furniture",
    "CC_PRODUCTS": "Products",
    "CC_EXPANSIONS_CONCEPTUAL": "Expansions",
}

EXPORT_OBJECT_TYPES = {"MESH", "EMPTY"}

# Si True, sustituye FBX existentes con el mismo nombre.
OVERWRITE_EXISTING = True

# Si True, intenta exportar colliders que sean descendientes del mismo root.
# Los colliders de colecciones globales separadas no se incluyen.
INCLUDE_CHILD_COLLIDERS = True


# ---------------------------------------------------------------------------
# UTILIDADES
# ---------------------------------------------------------------------------

def log(message: str) -> None:
    print(f"[CC FBX Export] {message}")


def validate_paths() -> None:
    if not KIT_ROOT.is_dir():
        raise FileNotFoundError(f"No existe KIT_ROOT:\n{KIT_ROOT}")

    if not PRODUCTION_ROOT.is_dir():
        raise FileNotFoundError(f"No existe la carpeta Production:\n{PRODUCTION_ROOT}")

    EXPORT_ROOT.mkdir(parents=True, exist_ok=True)


def sanitize_filename(name: str) -> str:
    """Convierte un nombre de Blender en un nombre de archivo seguro."""
    safe = re.sub(r'[<>:"/\\|?*\x00-\x1F]', "_", name)
    safe = safe.strip().rstrip(".")
    return safe or "UnnamedAsset"


def all_descendants(root: bpy.types.Object) -> list[bpy.types.Object]:
    """Devuelve el root y todos sus descendientes de forma recursiva."""
    result: list[bpy.types.Object] = []

    def walk(obj: bpy.types.Object) -> None:
        if obj.type in EXPORT_OBJECT_TYPES:
            result.append(obj)
        for child in obj.children:
            walk(child)

    walk(root)
    return result


def collection_top_level_roots(collection: bpy.types.Collection) -> list[bpy.types.Object]:
    """
    Detecta assets raíz:
    - objetos sin padre;
    - vinculados directamente o indirectamente a la colección de familia;
    - prioriza EMPTY como raíz;
    - también admite un MESH sin padre como módulo independiente.
    """
    objects = list(collection.all_objects)
    object_set = set(objects)

    roots = [
        obj
        for obj in objects
        if obj.parent is None or obj.parent not in object_set
    ]

    # Orden estable para exportaciones reproducibles.
    roots.sort(key=lambda obj: obj.name.lower())
    return roots


def deselect_all() -> None:
    if bpy.context.object and bpy.context.object.mode != "OBJECT":
        bpy.ops.object.mode_set(mode="OBJECT")
    bpy.ops.object.select_all(action="DESELECT")


def select_objects(objects: Iterable[bpy.types.Object]) -> list[bpy.types.Object]:
    deselect_all()
    selected: list[bpy.types.Object] = []

    for obj in objects:
        if obj.name not in bpy.context.view_layer.objects:
            continue
        obj.hide_set(False)
        obj.hide_viewport = False
        obj.select_set(True)
        selected.append(obj)

    active = next((obj for obj in selected if obj.type == "MESH"), None)
    if active is None and selected:
        active = selected[0]

    if active is not None:
        bpy.context.view_layer.objects.active = active

    return selected


def export_fbx(output_path: Path) -> None:
    """
    Exporta la selección con parámetros Unity.
    Incluye fallback para diferencias entre versiones de Blender.
    """
    base_kwargs = dict(
        filepath=str(output_path),
        use_selection=True,
        object_types={"MESH", "EMPTY"},
        axis_forward="-Z",
        axis_up="Y",
        apply_unit_scale=True,
        add_leaf_bones=False,
        use_mesh_modifiers=True,
        use_triangles=True,
        bake_anim=False,
        path_mode="AUTO",
        embed_textures=False,
    )

    attempts = [
        {
            **base_kwargs,
            "apply_scale_options": "FBX_SCALE_UNITS",
            "use_space_transform": True,
            "bake_space_transform": True,
            "mesh_smooth_type": "FACE",
        },
        {
            **base_kwargs,
            "apply_scale_options": "FBX_SCALE_UNITS",
            "use_space_transform": True,
            "mesh_smooth_type": "FACE",
        },
        base_kwargs,
    ]

    last_error: Exception | None = None

    for kwargs in attempts:
        try:
            result = bpy.ops.export_scene.fbx(**kwargs)
            if "FINISHED" not in result:
                raise RuntimeError(f"El operador FBX devolvió: {result}")
            return
        except (TypeError, ValueError, RuntimeError) as exc:
            last_error = exc

    raise RuntimeError(
        f"No se pudo exportar el FBX con esta versión de Blender.\n{last_error}"
    )


def export_asset(
    family_collection: bpy.types.Collection,
    family_folder: str,
    root: bpy.types.Object,
) -> bool:
    objects = all_descendants(root)

    if not INCLUDE_CHILD_COLLIDERS:
        objects = [
            obj for obj in objects
            if "collider" not in obj.name.lower()
        ]

    mesh_count = sum(obj.type == "MESH" for obj in objects)
    if mesh_count == 0:
        log(f"OMITIDO sin mallas: {root.name}")
        return False

    selected = select_objects(objects)
    if not selected:
        log(f"OMITIDO no seleccionable: {root.name}")
        return False

    family_dir = EXPORT_ROOT / family_folder
    family_dir.mkdir(parents=True, exist_ok=True)

    filename = sanitize_filename(root.name) + ".fbx"
    output_path = family_dir / filename

    if output_path.exists() and not OVERWRITE_EXISTING:
        log(f"OMITIDO ya existe: {output_path}")
        return False

    export_fbx(output_path)
    log(
        f"EXPORTADO [{family_collection.name}] "
        f"{root.name} -> {output_path}"
    )
    return True


# ---------------------------------------------------------------------------
# EJECUCIÓN
# ---------------------------------------------------------------------------

def export_all_assets() -> dict[str, int]:
    validate_paths()

    counts: dict[str, int] = {}
    total = 0

    log(f"Blend: {bpy.data.filepath or '(sin guardar)'}")
    log(f"Destino: {EXPORT_ROOT}")

    for collection_name, folder_name in FAMILY_COLLECTIONS.items():
        collection = bpy.data.collections.get(collection_name)

        if collection is None:
            log(f"AVISO: no existe la colección {collection_name}")
            counts[folder_name] = 0
            continue

        roots = collection_top_level_roots(collection)
        log(
            f"{collection_name}: "
            f"{len(roots)} posibles assets raíz detectados"
        )

        exported = 0
        for root in roots:
            try:
                if export_asset(collection, folder_name, root):
                    exported += 1
            except Exception as exc:
                log(f"ERROR exportando {root.name}: {exc}")

        counts[folder_name] = exported
        total += exported

    deselect_all()

    log("------------------------------------------------------------")
    for family, count in counts.items():
        log(f"{family}: {count} FBX")
    log(f"TOTAL: {total} FBX")
    log("Exportación finalizada.")

    return counts


if __name__ == "__main__":
    export_all_assets()
