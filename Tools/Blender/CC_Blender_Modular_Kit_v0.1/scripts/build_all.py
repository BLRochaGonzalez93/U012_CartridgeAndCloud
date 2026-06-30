"""One-click builder for the non-character Cartridge & Cloud modular kit.
Open this file in Blender and press Run Script.
"""
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

MODULES_TO_RELOAD = [
    "cc_config", "cc_common", "generate_architecture", "generate_furniture",
    "generate_products", "generate_expansions", "generate_lods_colliders",
    "validate_scene", "export_unity_fbx"
]
for _module_name in MODULES_TO_RELOAD:
    sys.modules.pop(_module_name, None)

from cc_common import scene_setup, material_library
import generate_architecture
import generate_furniture
import generate_products
import generate_expansions
import generate_lods_colliders
import validate_scene


def build_all():
    scene_setup(); material_library()
    generate_architecture.generate()
    generate_furniture.generate()
    generate_products.generate()
    generate_expansions.generate()
    generate_lods_colliders.generate()
    validate_scene.validate()
    print(f'Cartridge & Cloud modular kit build complete. Scripts: {CC_SCRIPT_DIR}. Review geometry before export.')

if __name__ == '__main__':
    build_all()
