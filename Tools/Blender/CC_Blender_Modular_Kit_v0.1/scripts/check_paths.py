"""Check the configured Blender kit paths without generating geometry."""
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

def check_paths():
    blend = Path(bpy.data.filepath).resolve() if bpy.data.filepath else None
    production = CONFIG.production_path()
    print("=== Cartridge & Cloud path check ===")
    print(f"Blend:      {blend or 'NOT SAVED'}")
    print(f"Scripts:    {CC_SCRIPT_DIR}")
    print(f"Production: {production}")
    print(f"Scripts OK: {(CC_SCRIPT_DIR / 'cc_common.py').is_file()}")
    print(f"Output OK:  {production.is_dir()}")
    return {"blend": str(blend) if blend else None, "scripts": str(CC_SCRIPT_DIR), "production": str(production)}

if __name__ == '__main__':
    check_paths()
