"""Optional lightweight Blender add-on exposing build buttons in the 3D View sidebar."""
bl_info = {
    'name': 'Cartridge & Cloud Modular Kit',
    'author': 'VRM Games / generated assistance',
    'version': (0, 1, 1),
    'blender': (4, 0, 0),
    'location': 'View3D > Sidebar > C&C Kit',
    'description': 'Generate modular non-character assets for Cartridge & Cloud',
    'category': '3D View',
}
import bpy

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

class CC_OT_BuildAll(bpy.types.Operator):
    bl_idname='cc.build_all'; bl_label='Build Complete Kit'; bl_options={'REGISTER','UNDO'}
    def execute(self, context):
        import build_all; build_all.build_all(); return {'FINISHED'}

class CC_OT_Validate(bpy.types.Operator):
    bl_idname='cc.validate'; bl_label='Validate Kit'; bl_options={'REGISTER'}
    def execute(self, context):
        import validate_scene; validate_scene.validate(); return {'FINISHED'}

class CC_OT_Export(bpy.types.Operator):
    bl_idname='cc.export_fbx'; bl_label='Export Unity FBX'; bl_options={'REGISTER'}
    def execute(self, context):
        import export_unity_fbx; export_unity_fbx.export_all(); return {'FINISHED'}

class CC_PT_Panel(bpy.types.Panel):
    bl_label='Cartridge & Cloud Kit'; bl_idname='CC_PT_modular_kit'; bl_space_type='VIEW_3D'; bl_region_type='UI'; bl_category='C&C Kit'
    def draw(self, context):
        col=self.layout.column(align=True)
        col.operator('cc.build_all', icon='MOD_BUILD')
        col.operator('cc.validate', icon='CHECKMARK')
        col.operator('cc.export_fbx', icon='EXPORT')
        col.label(text='Future modules are conceptual.')

CLASSES=(CC_OT_BuildAll,CC_OT_Validate,CC_OT_Export,CC_PT_Panel)
def register():
    for c in CLASSES:bpy.utils.register_class(c)
def unregister():
    for c in reversed(CLASSES):bpy.utils.unregister_class(c)
if __name__=='__main__':register()
