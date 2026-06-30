"""Generate Family B furniture prefabs, excluding characters."""
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
from cc_common import *


def shelf_frame(name,w,d,h,levels,double_sided,coll,mats,industrial=False):
    root=add_empty(name,collection=coll)
    metal=mats['graphite']; shelfmat=mats['dark'] if not industrial else mats['light']
    post=0.055
    for x in (-w/2+post/2,w/2-post/2):
        for y in ((-d/2+post/2,d/2-post/2) if double_sided else (-d/2+post/2,)):
            o=box(f"{name}_Post",(post,post,h),(x,y,h/2),metal,coll);o.parent=root
    for i in range(levels):
        z=0.18+i*(h-0.32)/(levels-1 if levels>1 else 1)
        dep=d*0.90
        o=box(f"{name}_Shelf_{i+1:02d}",(w*0.94,dep,0.045),(0,0,z),shelfmat,coll);o.parent=root
    back=box(f"{name}_Back",(w*0.95,0.035,h*0.80),(0,d/2-0.03,h*0.53),metal,coll)
    back.parent=root
    return root


def checkout(coll,mats):
    n=f"{CONFIG.prefix}_Furniture_CheckoutCounter_LOD0"; root=add_empty(n,collection=coll)
    body=box(f"{n}_Body",(2.0,1.0,0.96),(0,0,0.48),mats['graphite'],coll)
    top=box(f"{n}_Countertop",(2.04,1.04,0.10),(0,0,1.01),mats['dark'],coll)
    accent=box(f"{n}_Accent",(1.55,0.025,0.08),(0,-0.515,0.60),mats['green_emissive'],coll)
    terminal=box(f"{n}_POSTerminal",(0.34,0.18,0.32),(0.55,-0.15,1.22),mats['dark'],coll)
    scanner=box(f"{n}_Scanner",(0.18,0.12,0.08),(-0.45,-0.22,1.10),mats['green'],coll)
    reader=box(f"{n}_CardReader",(0.12,0.16,0.05),(0.15,-0.33,1.09),mats['dark'],coll)
    for o in (body,top,accent,terminal,scanner,reader):o.parent=root
    tag_asset(root,'Furniture','F01 Checkout Counter'); return root


def generate():
    scene_setup();mats=material_library();root=root_collection();coll=ensure_collection('CC_FURNITURE',root);clear_collection(coll.name)
    items=[]
    items.append(checkout(coll,mats))
    wall=shelf_frame(f"{CONFIG.prefix}_Furniture_WallShelf_LOD0",2.0,0.5,2.2,4,False,coll,mats);tag_asset(wall,'Furniture','F02 Wall Shelf');items.append(wall)
    central=shelf_frame(f"{CONFIG.prefix}_Furniture_CentralShelf_LOD0",2.0,1.0,1.6,3,True,coll,mats);tag_asset(central,'Furniture','F03 Central Shelf');items.append(central)
    low=add_empty(f"{CONFIG.prefix}_Furniture_LowDisplay_LOD0",collection=coll)
    for o in [box(f"{low.name}_Base",(1.5,1.0,0.78),(0,0,0.39),mats['graphite'],coll),box(f"{low.name}_Top",(1.52,1.02,0.12),(0,0,0.84),mats['dark'],coll),box(f"{low.name}_Accent",(1.2,0.02,0.04),(0,-0.51,0.58),mats['cyan_emissive'],coll)]:o.parent=low
    tag_asset(low,'Furniture','F04 Low Display');items.append(low)
    feat=add_empty(f"{CONFIG.prefix}_Furniture_FeaturedDisplay_LOD0",collection=coll)
    for o in [box(f"{feat.name}_Base",(1.0,1.0,0.95),(0,0,0.475),mats['graphite'],coll),box(f"{feat.name}_Top",(0.88,0.88,0.12),(0,0,1.01),mats['dark'],coll),box(f"{feat.name}_Accent",(0.92,0.92,0.035),(0,0,0.93),mats['cyan_emissive'],coll)]:o.parent=feat
    tag_asset(feat,'Furniture','F05 Featured Display');items.append(feat)
    storage=shelf_frame(f"{CONFIG.prefix}_Furniture_BackroomStorage_LOD0",2.5,1.0,2.4,5,True,coll,mats,True);tag_asset(storage,'Furniture','F06 Backroom Storage');items.append(storage)
    crate=add_empty(f"{CONFIG.prefix}_Furniture_ReceivingCrate_LOD0",collection=coll)
    for o in [box(f"{crate.name}_Body",(1.0,1.0,0.72),(0,0,0.36),mats['dark'],coll),box(f"{crate.name}_Lid",(1.02,1.02,0.10),(0,0,0.77),mats['graphite'],coll),box(f"{crate.name}_BandX",(1.03,0.08,0.18),(0,0,0.40),mats['amber'],coll),box(f"{crate.name}_BandY",(0.08,1.03,0.18),(0,0,0.40),mats['amber'],coll)]:o.parent=crate
    tag_asset(crate,'Furniture','F07 Receiving Crate');items.append(crate)
    # Stylized plant
    plant=add_empty(f"{CONFIG.prefix}_Furniture_DecorativePlant_LOD0",collection=coll)
    pot=cylinder(f"{plant.name}_Pot",0.20,0.34,(0,0,0.17),24,mats['graphite'],coll);pot.parent=plant
    soil=cylinder(f"{plant.name}_Soil",0.17,0.03,(0,0,0.35),24,mats['cardboard'],coll);soil.parent=plant
    for i,(x,y,rot) in enumerate([(-.08,0,25),(.08,0,-25),(0,.06,0),(0,-.06,45),(.04,.04,-45)]):
        leaf=box(f"{plant.name}_Leaf_{i:02d}",(0.17,0.035,0.72),(x,y,0.74),mats['green'],coll)
        leaf.rotation_euler[1]=rot*0.0174533;leaf.parent=plant
    tag_asset(plant,'Furniture','F08 Decorative Plant');items.append(plant)
    print('Cartridge & Cloud: furniture generated.')

if __name__=='__main__':generate()
