"""Validate scale, naming, transforms, dimensions and common Unity handoff issues."""
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

import json
from pathlib import Path
import bpy
from cc_config import CONFIG
from cc_common import ensure_export_dir


def validate():
    issues = []
    stats = {'objects': 0, 'meshes': 0, 'conceptual': 0}
    for obj in bpy.data.objects:
        if not obj.name.startswith('CC_'):
            continue
        stats['objects'] += 1
        if obj.type == 'MESH':
            stats['meshes'] += 1
            if len(obj.data.polygons) == 0:
                issues.append({'severity':'error','object':obj.name,'message':'Mesh has no polygons.'})
        if obj.get('cc_conceptual'):
            stats['conceptual'] += 1
        if any(abs(s - 1.0) > 1e-4 for s in obj.scale):
            issues.append({'severity':'warning','object':obj.name,'message':f'Unapplied scale {tuple(round(v,4) for v in obj.scale)}'})
        if any(abs(r) > 1e-4 for r in obj.rotation_euler) and obj.type == 'MESH':
            issues.append({'severity':'info','object':obj.name,'message':'Mesh has non-zero rotation. Apply before FBX if intentional.'})
        if obj.type == 'MESH' and min(obj.dimensions) <= 0:
            issues.append({'severity':'error','object':obj.name,'message':'Invalid dimensions.'})
        if obj.type == 'MESH' and len(obj.data.materials) == 0 and not obj.get('cc_collider'):
            issues.append({'severity':'warning','object':obj.name,'message':'No material assigned.'})
    report = {'kit_version':'0.1','grid_m':CONFIG.grid,'stats':stats,'issues':issues}
    path = Path(ensure_export_dir()) / 'CC_Validation_Report.json'
    path.write_text(json.dumps(report, indent=2), encoding='utf-8')
    print(f'Validation complete: {len(issues)} issue(s). Report: {path}')
    return report

if __name__ == '__main__':
    validate()
