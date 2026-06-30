"""Generate Family A architecture and shell modules."""
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
import bpy
from cc_config import CONFIG
from cc_common import *


def build_floor(coll, mats):
    modules = []
    for s in (1.0, 2.0):
        o = box(f"{CONFIG.prefix}_Architecture_Floor_{int(s*100):03d}_LOD0", (s, s, 0.05), material=mats['dark'], collection=coll)
        tag_asset(o, 'Architecture', 'A01 Floor')
        modules.append(o)
    edge = box(f"{CONFIG.prefix}_Architecture_FloorEdge_100_LOD0", (1.0, 0.10, 0.06), material=mats['graphite'], collection=coll)
    transition = box(f"{CONFIG.prefix}_Architecture_FloorTransition_100_LOD0", (1.0, 0.35, 0.025), material=mats['light'], collection=coll)
    return modules + [edge, transition]


def build_walls(coll, mats):
    out = []
    for length in (1.0, 2.0, 4.0):
        root = add_empty(f"{CONFIG.prefix}_Architecture_Wall_{int(length*100):03d}_LOD0", collection=coll)
        body = box(f"{root.name}_Body", (length, CONFIG.wall_thickness, CONFIG.wall_height), (0,0,CONFIG.wall_height/2), mats['light'], coll)
        base = box(f"{root.name}_Baseboard", (length, CONFIG.wall_thickness+0.02, 0.18), (0,-0.01,0.09), mats['graphite'], coll)
        seam = box(f"{root.name}_TopTrim", (length, CONFIG.wall_thickness+0.01, 0.08), (0,0,CONFIG.wall_height-0.04), mats['graphite'], coll)
        for obj in (body, base, seam): obj.parent = root
        tag_asset(root, 'Architecture', 'A02 Wall')
        out.append(root)
    # Corner blocks and end cap
    for name, dims in [('CornerInner',(0.20,0.20,3.0)), ('CornerOuter',(0.24,0.24,3.0)), ('EndCap',(0.20,0.04,3.0))]:
        o = box(f"{CONFIG.prefix}_Architecture_{name}_LOD0", dims, (0,0,dims[2]/2), mats['light'], coll)
        tag_asset(o, 'Architecture', 'A02 Wall')
        out.append(o)
    return out


def build_facade(coll, mats):
    root = add_empty(f"{CONFIG.prefix}_Architecture_StorefrontFacade_LOD0", collection=coll)
    # 10m facade with 2m central opening
    left = box(f"{root.name}_Left", (4.0,0.20,3.0), (-3.0,0,1.5), mats['graphite'], coll)
    right = box(f"{root.name}_Right", (4.0,0.20,3.0), (3.0,0,1.5), mats['graphite'], coll)
    header = box(f"{root.name}_Header", (10.0,0.24,0.55), (0,0,2.725), mats['graphite'], coll)
    for x in (-3.0,3.0):
        glass = box(f"{root.name}_Glass_{'L' if x<0 else 'R'}", (3.0,0.025,1.86), (x,-0.12,1.25), mats['glass'], coll, bevel=False)
        glass.parent = root
    for obj in (left,right,header): obj.parent=root
    tag_asset(root, 'Architecture', 'A03 Facade')
    return root


def build_glass(coll, mats):
    out=[]
    for side in ('Left','Right'):
        g=box(f"{CONFIG.prefix}_Architecture_StoreGlass_{side}_LOD0", (3.0,0.02,1.86), (0,0,0.93), mats['glass'], coll, bevel=False)
        tag_asset(g,'Architecture','A04 Glass')
        out.append(g)
    return out


def build_door(coll, mats):
    root=add_empty(f"{CONFIG.prefix}_Architecture_AutoDoor_Root", collection=coll)
    for side,x in [('Left',-0.50),('Right',0.50)]:
        panel=add_empty(f"{CONFIG.prefix}_Architecture_AutoDoor_{side}", (x,0,0), coll)
        frame=box(f"{panel.name}_Frame", (0.96,0.08,2.35), (x,0,1.175), mats['graphite'], coll)
        glass=box(f"{panel.name}_Glass", (0.82,0.025,2.15), (x,-0.045,1.175), mats['glass'], coll, bevel=False)
        frame.parent=panel; glass.parent=panel; panel.parent=root
    rail=box(f"{root.name}_UpperRail", (2.2,0.18,0.18), (0,0,2.45), mats['graphite'], coll)
    sensor=box(f"{root.name}_Sensor", (0.32,0.12,0.08), (0,-0.12,2.55), mats['green_emissive'], coll)
    rail.parent=root; sensor.parent=root
    tag_asset(root,'Architecture','A05 Automatic Door')
    return root


def build_backroom_partition(coll,mats):
    root=add_empty(f"{CONFIG.prefix}_Architecture_BackroomPartition_LOD0",collection=coll)
    for x in (-3.1,3.1):
        seg=box(f"{root.name}_{'L' if x<0 else 'R'}", (3.8,0.20,3.0), (x,0,1.5), mats['light'], coll)
        guard=box(f"{seg.name}_Guard", (3.8,0.24,0.30), (x,-0.02,0.15), mats['graphite'], coll)
        seg.parent=root; guard.parent=root
    top=box(f"{root.name}_OpeningHeader",(2.4,0.20,0.35),(0,0,2.825),mats['graphite'],coll)
    top.parent=root
    tag_asset(root,'Architecture','A06 Backroom Partition')
    return root


def build_sign(coll,mats):
    root=add_empty(f"{CONFIG.prefix}_Architecture_StoreSign_LOD0",collection=coll)
    housing=box(f"{root.name}_Housing",(3.2,0.12,0.55),(0,0,0.275),mats['graphite'],coll)
    face=box(f"{root.name}_Face",(3.05,0.02,0.42),(0,-0.071,0.275),mats['green_emissive'],coll)
    housing.parent=root; face.parent=root
    tag_asset(root,'Architecture','A07 Sign')
    return root


def build_zone_markers(coll,mats):
    zones=[('Checkout',(3.0,2.5),mats['green']),('Receiving',(3.0,2.5),mats['amber']),('Backroom',(9.5,2.8),mats['bluegray'])]
    out=[]
    for name,(w,d),mat in zones:
        root=add_empty(f"{CONFIG.prefix}_Architecture_Zone{name}_LOD0",collection=coll)
        t=0.035; z=0.003
        for loc,size in [((0,-d/2,z),(w,t,0.006)),((0,d/2,z),(w,t,0.006)),((-w/2,0,z),(t,d,0.006)),((w/2,0,z),(t,d,0.006))]:
            q=box(f"{root.name}_Line",size,loc,mat,coll,bevel=False)
            q.parent=root
        tag_asset(root,'Architecture','A08 Zone Marker')
        out.append(root)
    return out


def build_lighting(coll,mats):
    out=[]
    for length in (1.0,2.0):
        rail=box(f"{CONFIG.prefix}_Architecture_LightRail_{int(length*100):03d}_LOD0",(length,0.055,0.055),(0,0,0),mats['graphite'],coll)
        tag_asset(rail,'Architecture','A09 Lighting')
        out.append(rail)
    spot_root=add_empty(f"{CONFIG.prefix}_Architecture_SpotLight_LOD0",collection=coll)
    stem=cylinder(f"{spot_root.name}_Stem",0.035,0.18,(0,0,-0.09),16,mats['graphite'],coll)
    head=cylinder(f"{spot_root.name}_Head",0.10,0.18,(0,0,-0.24),24,mats['graphite'],coll,rotation=(math.radians(90),0,0))
    lens=cylinder(f"{spot_root.name}_Lens",0.078,0.01,(0,-0.095,-0.24),24,mats['light'],coll,rotation=(math.radians(90),0,0))
    for o in (stem,head,lens):o.parent=spot_root
    out.append(spot_root)
    panel=box(f"{CONFIG.prefix}_Architecture_LinearPanel_LOD0",(1.2,0.25,0.06),(0,0,0),mats['light'],coll)
    out.append(panel)
    return out


def build_threshold(coll,mats):
    root=add_empty(f"{CONFIG.prefix}_Architecture_EntranceThreshold_LOD0",collection=coll)
    apron=box(f"{root.name}_Apron",(2.4,1.0,0.02),(0,0,0.01),mats['dark'],coll)
    profile=box(f"{root.name}_MetalProfile",(2.4,0.05,0.025),(0,0.45,0.0125),mats['graphite'],coll)
    mat=box(f"{root.name}_Mat",(1.8,0.65,0.012),(0,-0.10,0.016),mats['green'],coll)
    for o in (apron,profile,mat):o.parent=root
    tag_asset(root,'Architecture','A10 Threshold')
    return root


def generate():
    scene_setup(); mats=material_library()
    root=root_collection(); coll=ensure_collection('CC_ARCHITECTURE',root)
    clear_collection(coll.name)
    build_floor(coll,mats); build_walls(coll,mats); build_facade(coll,mats); build_glass(coll,mats)
    build_door(coll,mats); build_backroom_partition(coll,mats); build_sign(coll,mats)
    build_zone_markers(coll,mats); build_lighting(coll,mats); build_threshold(coll,mats)
    print('Cartridge & Cloud: architecture generated.')


if __name__ == '__main__':
    generate()
