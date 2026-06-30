"""Create simple LOD duplicates and box colliders for tagged kit assets."""
# --- Cartridge & Cloud portable path bootstrap ------------------------------
import sys
from pathlib import Path

CC_FIXED_SCRIPT_DIR = Path(r"D:\_PROGRAMAS\_ARCHIVOS\0_GameDevPortfolio\U012_CartridgeAndCloud\U012_CartridgeAndCloud\Tools\Blender\CC_Blender_Modular_Kit_v0.1\scripts")

def _cc_resolve_script_dir() -> Path:
    candidates = [CC_FIXED_SCRIPT_DIR]
    try:
        import bpy
        if bpy.data.filepath:
            blend_path = Path(bpy.data.filepath).resolve()
            candidates.append(blend_path.parent.parent / "scripts")
    except Exception:
        pass
    if "__file__" in globals():
        try:
            candidates.append(Path(__file__).resolve().parent)
        except Exception:
            pass
    candidates.append(Path.cwd())
    for candidate in candidates:
        if (candidate / "cc_common.py").is_file() and (candidate / "cc_config.py").is_file():
            resolved = candidate.resolve()
            if str(resolved) not in sys.path:
                sys.path.insert(0, str(resolved))
            return resolved
    searched = "\n".join(f"  - {p}" for p in candidates)
    raise ModuleNotFoundError(
        "No se encontró la carpeta de scripts de Cartridge & Cloud. Rutas comprobadas:\n" + searched
    )

CC_SCRIPT_DIR = _cc_resolve_script_dir()
# ---------------------------------------------------------------------------

import bpy
from cc_config import CONFIG
from cc_common import ensure_collection, root_collection, duplicate_lod, add_box_collider


def generate():
    root = root_collection()
    lod_coll = ensure_collection('CC_GENERATED_LODS', root)
    col_coll = ensure_collection('CC_GENERATED_COLLIDERS', root)
    created_lods = 0
    created_cols = 0
    for obj in list(bpy.data.objects):
        if obj.type != 'MESH' or '_LOD0' not in obj.name or obj.get('cc_collider'):
            continue
        if CONFIG.create_lods:
            duplicate_lod(obj, 'LOD1', decimate_ratio=0.60, collection=lod_coll)
            duplicate_lod(obj, 'LOD2', decimate_ratio=0.25, collection=lod_coll)
            created_lods += 2
        if CONFIG.create_colliders and not any(k in obj.name.lower() for k in ('glass','zone','light','decal','screen','led')):
            add_box_collider(obj, col_coll)
            created_cols += 1
    print(f'Cartridge & Cloud: generated {created_lods} LOD meshes and {created_cols} simple colliders.')

if __name__ == '__main__':
    generate()
