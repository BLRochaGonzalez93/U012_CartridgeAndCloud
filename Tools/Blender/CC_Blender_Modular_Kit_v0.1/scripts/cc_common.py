"""Shared Blender helpers for Cartridge & Cloud modular kit.
Designed for Blender 4.x. Run from Blender's Scripting workspace.
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

import math
from pathlib import Path
import bpy
from mathutils import Vector
from cc_config import CONFIG


def ensure_collection(name, parent=None):
    coll = bpy.data.collections.get(name)
    if coll is None:
        coll = bpy.data.collections.new(name)
        if parent:
            parent.children.link(coll)
        else:
            bpy.context.scene.collection.children.link(coll)
    return coll


def root_collection():
    return ensure_collection(CONFIG.collection_root)


def clear_collection(name):
    coll = bpy.data.collections.get(name)
    if not coll:
        return
    for obj in list(coll.objects):
        bpy.data.objects.remove(obj, do_unlink=True)
    for child in list(coll.children):
        clear_collection(child.name)
        bpy.data.collections.remove(child)


def move_to_collection(obj, collection):
    for c in list(obj.users_collection):
        c.objects.unlink(obj)
    collection.objects.link(obj)


def apply_transform(obj, location=False, rotation=True, scale=True):
    bpy.context.view_layer.objects.active = obj
    obj.select_set(True)
    bpy.ops.object.transform_apply(location=location, rotation=rotation, scale=scale)
    obj.select_set(False)


def set_origin_bottom_center(obj):
    bpy.context.view_layer.objects.active = obj
    obj.select_set(True)
    world_bbox = [obj.matrix_world @ Vector(corner) for corner in obj.bound_box]
    center = sum(world_bbox, Vector()) / 8.0
    min_z = min(v.z for v in world_bbox)
    cursor_prev = bpy.context.scene.cursor.location.copy()
    bpy.context.scene.cursor.location = (center.x, center.y, min_z)
    bpy.ops.object.origin_set(type='ORIGIN_CURSOR', center='MEDIAN')
    bpy.context.scene.cursor.location = cursor_prev
    obj.select_set(False)


def add_bevel(obj, width=None, segments=2):
    width = CONFIG.bevel_small if width is None else width
    mod = obj.modifiers.new("CC_Bevel", 'BEVEL')
    mod.width = width
    mod.segments = segments
    mod.limit_method = 'ANGLE'
    return mod


def set_smooth_by_angle(obj, angle=math.radians(35)):
    if obj.type != 'MESH':
        return
    for poly in obj.data.polygons:
        poly.use_smooth = True
    obj.data.set_sharp_from_angle(angle=angle)


def create_material(name, base_color, metallic=0.0, roughness=0.5, emission=None, alpha=1.0):
    mat = bpy.data.materials.get(name) or bpy.data.materials.new(name)
    mat.use_nodes = True
    nt = mat.node_tree
    bsdf = nt.nodes.get("Principled BSDF")
    bsdf.inputs['Base Color'].default_value = base_color
    bsdf.inputs['Metallic'].default_value = metallic
    bsdf.inputs['Roughness'].default_value = roughness
    bsdf.inputs['Alpha'].default_value = alpha
    if emission:
        bsdf.inputs['Emission Color'].default_value = emission
        bsdf.inputs['Emission Strength'].default_value = 2.0
    if alpha < 1.0:
        mat.surface_render_method = 'DITHERED'
        mat.use_transparency_overlap = False
    return mat


def material_library():
    p = CONFIG.palette
    return {
        'graphite': create_material('CC_MAT_Graphite', p.graphite, metallic=0.35, roughness=0.42),
        'dark': create_material('CC_MAT_DarkSurface', p.dark_surface, metallic=0.08, roughness=0.55),
        'green': create_material('CC_MAT_VRMGreen', p.vrm_green, metallic=0.05, roughness=0.36),
        'green_emissive': create_material('CC_MAT_VRMGreen_Emissive', p.vrm_green, roughness=0.35, emission=p.vrm_green),
        'cyan': create_material('CC_MAT_Cyan', p.cyan, metallic=0.05, roughness=0.36),
        'cyan_emissive': create_material('CC_MAT_Cyan_Emissive', p.cyan, roughness=0.35, emission=p.cyan),
        'wood': create_material('CC_MAT_WarmWood', p.warm_wood, metallic=0.0, roughness=0.48),
        'light': create_material('CC_MAT_LightNeutral', p.light_neutral, metallic=0.0, roughness=0.55),
        'cardboard': create_material('CC_MAT_Cardboard', p.cardboard, metallic=0.0, roughness=0.82),
        'amber': create_material('CC_MAT_Amber', p.amber, metallic=0.0, roughness=0.45),
        'red': create_material('CC_MAT_Red', p.red, metallic=0.0, roughness=0.45),
        'bluegray': create_material('CC_MAT_BlueGray', p.bluegray, metallic=0.08, roughness=0.55),
        'glass': create_material('CC_MAT_Glass', p.glass_tint, metallic=0.0, roughness=0.12, alpha=p.glass_tint[3]),
    }


def assign_material(obj, mat):
    if obj.type == 'MESH':
        if not obj.data.materials:
            obj.data.materials.append(mat)
        else:
            obj.data.materials[0] = mat


def box(name, size, location=(0, 0, 0), material=None, collection=None, bevel=True, origin_bottom=True):
    bpy.ops.mesh.primitive_cube_add(location=location)
    obj = bpy.context.active_object
    obj.name = name
    obj.dimensions = size
    apply_transform(obj)
    if bevel:
        add_bevel(obj)
    if origin_bottom:
        set_origin_bottom_center(obj)
    if material:
        assign_material(obj, material)
    if collection:
        move_to_collection(obj, collection)
    return obj


def cylinder(name, radius, depth, location=(0, 0, 0), vertices=24, material=None, collection=None, rotation=(0, 0, 0)):
    bpy.ops.mesh.primitive_cylinder_add(vertices=vertices, radius=radius, depth=depth, location=location, rotation=rotation)
    obj = bpy.context.active_object
    obj.name = name
    apply_transform(obj)
    add_bevel(obj, width=min(CONFIG.bevel_small, radius * 0.08), segments=2)
    set_origin_bottom_center(obj)
    if material:
        assign_material(obj, material)
    if collection:
        move_to_collection(obj, collection)
    return obj


def torus(name, major_radius, minor_radius, location=(0, 0, 0), material=None, collection=None, rotation=(0, 0, 0)):
    bpy.ops.mesh.primitive_torus_add(major_radius=major_radius, minor_radius=minor_radius, major_segments=32,
                                    minor_segments=10, location=location, rotation=rotation)
    obj = bpy.context.active_object
    obj.name = name
    apply_transform(obj)
    if material:
        assign_material(obj, material)
    if collection:
        move_to_collection(obj, collection)
    return obj


def join_objects(objects, name, collection=None):
    objects = [o for o in objects if o]
    if not objects:
        return None
    bpy.ops.object.select_all(action='DESELECT')
    for o in objects:
        o.select_set(True)
    bpy.context.view_layer.objects.active = objects[0]
    bpy.ops.object.join()
    obj = bpy.context.active_object
    obj.name = name
    if collection:
        move_to_collection(obj, collection)
    set_origin_bottom_center(obj)
    return obj


def add_empty(name, location=(0, 0, 0), collection=None):
    obj = bpy.data.objects.new(name, None)
    obj.empty_display_type = 'PLAIN_AXES'
    obj.location = location
    (collection or root_collection()).objects.link(obj)
    return obj


def add_box_collider(source_obj, collection, name=None, margin=0.0):
    dims = tuple(d + margin * 2 for d in source_obj.dimensions)
    loc = source_obj.matrix_world.translation.copy()
    collider = box(name or f"{source_obj.name}_COL", dims, loc, material=None, collection=collection, bevel=False)
    collider.display_type = 'WIRE'
    collider.hide_render = True
    collider['cc_collider'] = True
    return collider


def duplicate_lod(source, suffix, scale_factor=1.0, decimate_ratio=0.55, collection=None):
    obj = source.copy()
    obj.data = source.data.copy()
    obj.name = f"{source.name}_{suffix}"
    (collection or source.users_collection[0]).objects.link(obj)
    if obj.type == 'MESH':
        dec = obj.modifiers.new(f"CC_Decimate_{suffix}", 'DECIMATE')
        dec.ratio = decimate_ratio
    obj.scale *= scale_factor
    return obj


def create_text_plate(name, text, size=(1.0, 0.3), depth=0.02, material=None, collection=None):
    # Plate only. Text remains editable as a child curve.
    plate = box(name, (size[0], depth, size[1]), material=material, collection=collection)
    bpy.ops.object.text_add(location=(0, -depth / 2 - 0.005, size[1] * 0.1), rotation=(math.radians(90), 0, 0))
    txt = bpy.context.active_object
    txt.name = f"{name}_Text"
    txt.data.body = text
    txt.data.align_x = 'CENTER'
    txt.data.align_y = 'CENTER'
    txt.data.size = min(size) * 0.35
    txt.data.extrude = 0.005
    if material:
        assign_material(txt, material)
    if collection:
        move_to_collection(txt, collection)
    txt.parent = plate
    return plate


def tag_asset(obj, family, element, conceptual=False):
    obj['cc_family'] = family
    obj['cc_element'] = element
    obj['cc_conceptual'] = conceptual
    obj['cc_grid'] = CONFIG.grid
    return obj


def _set_compatible_render_engine(scene):
    """Select the newest available Eevee engine, with safe fallbacks.

    Blender 4.2+ exposes ``BLENDER_EEVEE_NEXT`` while earlier supported
    versions expose ``BLENDER_EEVEE``. Workbench is used only as a final
    fallback so kit generation never fails solely because of the renderer.
    """
    available = {item.identifier for item in scene.bl_rna.properties['render'].fixed_type.properties['engine'].enum_items}

    for engine in ('BLENDER_EEVEE_NEXT', 'BLENDER_EEVEE', 'BLENDER_WORKBENCH'):
        if engine in available:
            scene.render.engine = engine
            print(f"[CC Kit] Render engine: {engine}")
            return engine

    # Extremely defensive fallback for unexpected Blender builds.
    current = scene.render.engine
    print(f"[CC Kit] Warning: no preferred render engine found; keeping {current}")
    return current


def scene_setup():
    scene = bpy.context.scene
    scene.unit_settings.system = 'METRIC'
    scene.unit_settings.scale_length = CONFIG.unit_scale
    scene.unit_settings.length_unit = 'METERS'
    _set_compatible_render_engine(scene)
    scene.render.resolution_x = 1920
    scene.render.resolution_y = 1080
    scene.render.resolution_percentage = 100
    return scene


def ensure_export_dir():
    root = bpy.path.abspath(CONFIG.export_root)
    Path(root).mkdir(parents=True, exist_ok=True)
    return root
