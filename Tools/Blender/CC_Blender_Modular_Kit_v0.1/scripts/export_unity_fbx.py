"""Export each direct child collection of CC_MODULAR_KIT to a separate Unity-ready FBX."""
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

from pathlib import Path
import bpy
from cc_common import root_collection, ensure_export_dir


def export_collection(coll, output_path):
    bpy.ops.object.select_all(action='DESELECT')
    selected = []
    def walk(c):
        for obj in c.objects:
            if obj.type in {'MESH','EMPTY'}:
                obj.select_set(True); selected.append(obj)
        for ch in c.children:
            walk(ch)
    walk(coll)
    if not selected:
        return False
    bpy.context.view_layer.objects.active = next((o for o in selected if o.type == 'MESH'), selected[0])
    bpy.ops.export_scene.fbx(
        filepath=str(output_path), use_selection=True, object_types={'MESH','EMPTY'},
        apply_unit_scale=True, apply_scale_options='FBX_SCALE_UNITS',
        axis_forward='-Z', axis_up='Y', use_space_transform=True,
        bake_space_transform=True, add_leaf_bones=False, mesh_smooth_type='FACE',
        use_mesh_modifiers=True, use_triangles=True, bake_anim=False,
        path_mode='AUTO', embed_textures=False
    )
    return True


def export_all():
    root = root_collection(); out = Path(ensure_export_dir()); out.mkdir(parents=True, exist_ok=True)
    count=0
    for coll in root.children:
        if export_collection(coll, out / f'{coll.name}.fbx'):
            count += 1
    print(f'Cartridge & Cloud: exported {count} collection FBX files directly to Production: {out}')

if __name__ == '__main__':
    export_all()
