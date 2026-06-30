"""Generate Family C products and packaging as modular visual references."""
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
from cc_config import CONFIG
from cc_common import *


def retail_box(name,size,coll,mats,accent='green'):
    root=add_empty(name,collection=coll)
    body=box(f"{name}_Box",size,(0,0,size[2]/2),mats['cardboard'],coll)
    front=box(f"{name}_FrontPanel",(size[0]*0.86,0.008,size[2]*0.76),(0,-size[1]/2-0.005,size[2]*0.53),mats[accent],coll,bevel=False)
    body.parent=root;front.parent=root
    return root


def game_case(coll,mats):
    n=f"{CONFIG.prefix}_Product_NeonDrift_LOD0";root=add_empty(n,collection=coll)
    body=box(f"{n}_Case",(0.135,0.015,0.190),(0,0,0.095),mats['dark'],coll)
    cover=box(f"{n}_CoverSleeve",(0.125,0.006,0.178),(0,-0.011,0.095),mats['cyan'],coll,bevel=False)
    spine=box(f"{n}_Spine",(0.010,0.006,0.178),(-0.0675,-0.011,0.095),mats['green'],coll,bevel=False)
    for o in (body,cover,spine):o.parent=root
    tag_asset(root,'Product','P01 Neon Drift');return root


def premium_case(coll,mats):
    n=f"{CONFIG.prefix}_Product_CloudRunnerCase_LOD0";root=add_empty(n,collection=coll)
    shell=box(f"{n}_Shell",(0.17,0.035,0.11),(0,0,0.055),mats['bluegray'],coll)
    seam=torus(f"{n}_Seam",0.055,0.004,(0,-0.019,0.055),mats['amber'],coll,rotation=(math.radians(90),0,0))
    latch=box(f"{n}_Latch",(0.035,0.012,0.018),(0,-0.025,0.015),mats['light'],coll)
    for o in (shell,seam,latch):o.parent=root
    tag_asset(root,'Product','P02 Cloud Runner Case');return root


def console(coll,mats):
    n=f"{CONFIG.prefix}_Product_VertexOneConsole_LOD0";root=add_empty(n,collection=coll)
    body=box(f"{n}_Body",(0.30,0.23,0.08),(0,0,0.04),mats['graphite'],coll)
    panel=box(f"{n}_Panel",(0.25,0.18,0.035),(0.015,0.01,0.085),mats['dark'],coll)
    led=box(f"{n}_LED",(0.14,0.008,0.006),(0,-0.119,0.055),mats['cyan_emissive'],coll,bevel=False)
    for o in (body,panel,led):o.parent=root
    tag_asset(root,'Product','P03 Vertex One Console');
    retail_box(f"{CONFIG.prefix}_Product_VertexOne_Box_LOD0",(0.38,0.14,0.30),coll,mats,'green')
    return root


def controller(coll,mats):
    n=f"{CONFIG.prefix}_Product_OrbitPad_LOD0";root=add_empty(n,collection=coll)
    center=box(f"{n}_Center",(0.105,0.055,0.055),(0,0,0.055),mats['graphite'],coll)
    for side in (-1,1):
        grip=cylinder(f"{n}_Grip_{side}",0.028,0.09,(side*0.055,0,0.03),18,mats['dark'],coll,rotation=(math.radians(18),0,side*math.radians(15)))
        grip.parent=root
    for x in (-0.025,0.025):
        stick=cylinder(f"{n}_Stick_{x}",0.012,0.012,(x,-0.02,0.09),18,mats['dark'],coll)
        stick.parent=root
    ring=torus(f"{n}_OrbitRing",0.026,0.004,(0,-0.031,0.075),mats['green_emissive'],coll,rotation=(math.radians(90),0,0));ring.parent=root
    center.parent=root;tag_asset(root,'Product','P04 Orbit Pad')
    retail_box(f"{CONFIG.prefix}_Product_OrbitPad_Box_LOD0",(0.19,0.08,0.15),coll,mats,'green')
    return root


def headset(coll,mats):
    n=f"{CONFIG.prefix}_Product_SignalProHeadset_LOD0";root=add_empty(n,collection=coll)
    band=torus(f"{n}_Band",0.085,0.010,(0,0,0.10),mats['graphite'],coll,rotation=(math.radians(90),0,0));band.parent=root
    for x in (-0.075,0.075):
        cup=cylinder(f"{n}_Cup_{x}",0.035,0.04,(x,0,0.065),24,mats['dark'],coll,rotation=(0,math.radians(90),0));cup.parent=root
        accent=torus(f"{n}_Accent_{x}",0.024,0.003,(x,-0.022,0.065),mats['cyan_emissive'],coll,rotation=(math.radians(90),0,0));accent.parent=root
    mic=cylinder(f"{n}_Mic",0.004,0.075,(0.10,-0.02,0.045),12,mats['graphite'],coll,rotation=(0,math.radians(50),0));mic.parent=root
    tag_asset(root,'Product','P05 Signal Pro Headset')
    retail_box(f"{CONFIG.prefix}_Product_SignalPro_Box_LOD0",(0.23,0.12,0.25),coll,mats,'cyan')
    return root


def memory(coll,mats):
    n=f"{CONFIG.prefix}_Product_MemoryCore_LOD0";root=add_empty(n,collection=coll)
    dev=box(f"{n}_Device",(0.10,0.02,0.07),(0,0,0.035),mats['graphite'],coll)
    connector=box(f"{n}_Connector",(0.022,0.022,0.035),(0.055,0,0.025),mats['light'],coll)
    led=box(f"{n}_LED",(0.018,0.004,0.006),(-0.025,-0.012,0.052),mats['green_emissive'],coll,bevel=False)
    for o in (dev,connector,led):o.parent=root
    tag_asset(root,'Product','P06 Memory Core')
    retail_box(f"{CONFIG.prefix}_Product_MemoryCore_Box_LOD0",(0.12,0.03,0.18),coll,mats,'cyan')
    return root


def generate():
    scene_setup();mats=material_library();root=root_collection();coll=ensure_collection('CC_PRODUCTS',root);clear_collection(coll.name)
    game_case(coll,mats);premium_case(coll,mats);console(coll,mats);controller(coll,mats);headset(coll,mats);memory(coll,mats)
    print('Cartridge & Cloud: products generated.')

if __name__=='__main__':generate()
