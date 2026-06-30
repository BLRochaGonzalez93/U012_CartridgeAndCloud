"""Generate modular props for future systems.
All generated items are conceptual and do not imply implemented gameplay.
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
from cc_config import CONFIG
from cc_common import *


def workstation(name,coll,mats,monitors=2):
    root=add_empty(name,collection=coll)
    desk=box(f"{name}_Desk",(1.6,0.75,0.74),(0,0,0.37),mats['wood'],coll);desk.parent=root
    for i in range(monitors):
        x=(i-(monitors-1)/2)*0.48
        mon=box(f"{name}_Monitor_{i+1}",(0.42,0.05,0.26),(x,-0.12,0.98),mats['dark'],coll);mon.parent=root
        screen=box(f"{name}_Screen_{i+1}",(0.37,0.008,0.21),(x,-0.148,0.98),mats['green_emissive'],coll,bevel=False);screen.parent=root
    return root


def build_logistics(coll,mats):
    # Packing table
    root=add_empty(f"{CONFIG.prefix}_Expansion_Logistics_PackingStation_LOD0",collection=coll)
    table=box(f"{root.name}_Table",(2.0,0.9,0.9),(0,0,0.45),mats['wood'],coll);table.parent=root
    printer=box(f"{root.name}_LabelPrinter",(0.28,0.24,0.18),(0.55,0,1.00),mats['dark'],coll);printer.parent=root
    scanner=box(f"{root.name}_Scanner",(0.16,0.12,0.10),(-0.50,0,0.98),mats['green'],coll);scanner.parent=root
    tag_asset(root,'Expansion','Warehouse Logistics',True)
    # Conveyor
    conv=add_empty(f"{CONFIG.prefix}_Expansion_Logistics_Conveyor_LOD0",collection=coll)
    base=box(f"{conv.name}_Base",(2.4,0.7,0.65),(0,0,0.325),mats['graphite'],coll);base.parent=conv
    for i in range(12):
        r=cylinder(f"{conv.name}_Roller_{i:02d}",0.03,0.62,(-1.1+i*0.2,0,0.68),16,mats['light'],coll,rotation=(math.radians(90),0,0));r.parent=conv
    tag_asset(conv,'Expansion','Warehouse Logistics',True)
    # Cart
    cart=add_empty(f"{CONFIG.prefix}_Expansion_Logistics_RollingCart_LOD0",collection=coll)
    platform=box(f"{cart.name}_Platform",(1.0,0.65,0.08),(0,0,0.16),mats['graphite'],coll);platform.parent=cart
    for x in (-0.42,0.42):
        for y in (-0.25,0.25):
            w=cylinder(f"{cart.name}_Wheel",0.06,0.04,(x,y,0.06),16,mats['dark'],coll,rotation=(math.radians(90),0,0));w.parent=cart
    handle=box(f"{cart.name}_Handle",(0.05,0.05,0.9),(0.45,0,0.68),mats['graphite'],coll);handle.parent=cart
    tag_asset(cart,'Expansion','Warehouse Logistics',True)


def build_staff_management(coll,mats):
    board=add_empty(f"{CONFIG.prefix}_Expansion_Staff_TaskBoard_LOD0",collection=coll)
    panel=box(f"{board.name}_Panel",(1.8,0.08,1.1),(0,0,0.55),mats['dark'],coll);panel.parent=board
    for i in range(12):
        x=-0.7+(i%4)*0.45;y=-0.05;z=0.25+(i//4)*0.3
        card=box(f"{board.name}_Card_{i:02d}",(0.30,0.01,0.18),(x,y,z),mats['green'] if i%3 else mats['amber'],coll,bevel=False);card.parent=board
    tag_asset(board,'Expansion','Staff Management',True)
    locker=add_empty(f"{CONFIG.prefix}_Expansion_Staff_Locker_LOD0",collection=coll)
    body=box(f"{locker.name}_Body",(0.55,0.50,1.90),(0,0,0.95),mats['graphite'],coll);body.parent=locker
    for z in (0.48,1.43):
        door=box(f"{locker.name}_Door",(0.49,0.03,0.86),(0,-0.265,z),mats['dark'],coll);door.parent=locker
    tag_asset(locker,'Expansion','Staff Management',True)


def build_online_commerce(coll,mats):
    workstation(f"{CONFIG.prefix}_Expansion_Ecommerce_OrderDesk_LOD0",coll,mats,2)
    sorter=add_empty(f"{CONFIG.prefix}_Expansion_Ecommerce_SortBins_LOD0",collection=coll)
    frame=box(f"{sorter.name}_Frame",(1.8,0.55,1.5),(0,0,0.75),mats['graphite'],coll);frame.parent=sorter
    for row in range(3):
        for col in range(4):
            bin=box(f"{sorter.name}_Bin_{row}_{col}",(0.38,0.45,0.32),(-0.63+col*0.42,0,0.30+row*0.42),mats['cardboard'],coll);bin.parent=sorter
    tag_asset(sorter,'Expansion','Ecommerce',True)


def build_publishing(coll,mats):
    table=add_empty(f"{CONFIG.prefix}_Expansion_Publishing_MeetingTable_LOD0",collection=coll)
    top=box(f"{table.name}_Top",(2.8,1.2,0.10),(0,0,0.76),mats['dark'],coll);top.parent=table
    for x in (-1.1,1.1):
        leg=box(f"{table.name}_Leg",(0.18,0.8,0.72),(x,0,0.36),mats['graphite'],coll);leg.parent=table
    tag_asset(table,'Expansion','Publishing',True)
    display=add_empty(f"{CONFIG.prefix}_Expansion_Publishing_LineupWall_LOD0",collection=coll)
    wall=box(f"{display.name}_Panel",(3.0,0.15,2.1),(0,0,1.05),mats['graphite'],coll);wall.parent=display
    for r in range(2):
        for c in range(6):
            case=box(f"{display.name}_Case_{r}_{c}",(0.22,0.035,0.31),(-1.15+c*0.46,-0.095,0.55+r*0.65),mats['cyan'] if (r+c)%2 else mats['amber'],coll);case.parent=display
    tag_asset(display,'Expansion','Publishing',True)


def build_internal_studio(coll,mats):
    workstation(f"{CONFIG.prefix}_Expansion_Studio_DevDesk_LOD0",coll,mats,3)
    art=workstation(f"{CONFIG.prefix}_Expansion_Studio_ArtDesk_LOD0",coll,mats,1)
    tablet=box(f"{art.name}_DrawingTablet",(0.58,0.38,0.035),(0,0,0.80),mats['dark'],coll);tablet.parent=art
    audio=add_empty(f"{CONFIG.prefix}_Expansion_Studio_AudioStation_LOD0",collection=coll)
    desk=box(f"{audio.name}_Desk",(1.5,0.7,0.72),(0,0,0.36),mats['wood'],coll);desk.parent=audio
    for x in (-0.45,0.45):
        sp=box(f"{audio.name}_Speaker",(0.24,0.22,0.36),(x,-0.05,0.95),mats['dark'],coll);sp.parent=audio
    mixer=box(f"{audio.name}_Mixer",(0.65,0.35,0.10),(0,-0.03,0.80),mats['graphite'],coll);mixer.parent=audio
    tag_asset(audio,'Expansion','Internal Studio',True)


def build_digital_platform(coll,mats):
    kiosk=add_empty(f"{CONFIG.prefix}_Expansion_Digital_Kiosk_LOD0",collection=coll)
    body=box(f"{kiosk.name}_Body",(0.55,0.38,1.45),(0,0,0.725),mats['graphite'],coll);body.parent=kiosk
    screen=box(f"{kiosk.name}_Screen",(0.43,0.02,0.55),(0,-0.20,1.05),mats['green_emissive'],coll);screen.parent=kiosk
    tag_asset(kiosk,'Expansion','Digital Platform',True)
    rack=add_empty(f"{CONFIG.prefix}_Expansion_Digital_CodeCardRack_LOD0",collection=coll)
    frame=box(f"{rack.name}_Frame",(1.2,0.45,1.5),(0,0,0.75),mats['graphite'],coll);frame.parent=rack
    for r in range(4):
        for c in range(4):
            card=box(f"{rack.name}_Card_{r}_{c}",(0.20,0.025,0.27),(-0.39+c*0.26,-0.24,0.30+r*0.30),mats['green'],coll);card.parent=rack
    tag_asset(rack,'Expansion','Digital Platform',True)


def build_servers(coll,mats):
    rack=add_empty(f"{CONFIG.prefix}_Expansion_Server_Rack_LOD0",collection=coll)
    frame=box(f"{rack.name}_Frame",(0.75,0.95,2.1),(0,0,1.05),mats['graphite'],coll);frame.parent=rack
    for i in range(12):
        unit=box(f"{rack.name}_Unit_{i:02d}",(0.64,0.80,0.105),(0,-0.02,0.16+i*0.15),mats['dark'],coll);unit.parent=rack
        led=box(f"{rack.name}_LED_{i:02d}",(0.18,0.012,0.012),(-0.15,-0.49,0.16+i*0.15),mats['green_emissive'],coll,bevel=False);led.parent=rack
    tag_asset(rack,'Expansion','Infrastructure Servers',True)
    ops=workstation(f"{CONFIG.prefix}_Expansion_Server_OperationsDesk_LOD0",coll,mats,3)
    tag_asset(ops,'Expansion','Infrastructure Servers',True)
    ups=add_empty(f"{CONFIG.prefix}_Expansion_Server_UPS_LOD0",collection=coll)
    for i in range(3):
        u=box(f"{ups.name}_{i}",(0.52,0.70,0.65),(-0.58+i*0.58,0,0.325),mats['graphite'],coll);u.parent=ups
        mark=box(f"{ups.name}_Mark_{i}",(0.18,0.01,0.22),(-0.58+i*0.58,-0.356,0.325),mats['green_emissive'],coll,bevel=False);mark.parent=ups
    tag_asset(ups,'Expansion','Infrastructure Servers',True)


def build_market(coll,mats):
    board=add_empty(f"{CONFIG.prefix}_Expansion_Market_AnalysisWall_LOD0",collection=coll)
    for i,x in enumerate((-1.5,0,1.5)):
        panel=box(f"{board.name}_Panel_{i}",(1.35,0.08,0.85),(x,0,0.425),mats['dark'],coll);panel.parent=board
        chart=box(f"{board.name}_Chart_{i}",(1.15,0.01,0.65),(x,-0.05,0.425),mats['green'] if i!=1 else mats['bluegray'],coll,bevel=False);chart.parent=board
    tag_asset(board,'Expansion','Market Analysis',True)
    planning=add_empty(f"{CONFIG.prefix}_Expansion_Market_PlanningTable_LOD0",collection=coll)
    top=box(f"{planning.name}_Top",(3.0,1.3,0.10),(0,0,0.76),mats['wood'],coll);top.parent=planning
    for x in (-1.2,1.2):
        leg=box(f"{planning.name}_Leg",(0.20,0.9,0.72),(x,0,0.36),mats['graphite'],coll);leg.parent=planning
    tag_asset(planning,'Expansion','Market Analysis',True)


def generate():
    scene_setup();mats=material_library();root=root_collection();coll=ensure_collection('CC_EXPANSIONS_CONCEPTUAL',root);clear_collection(coll.name)
    build_logistics(coll,mats);build_staff_management(coll,mats);build_online_commerce(coll,mats);build_publishing(coll,mats)
    build_internal_studio(coll,mats);build_digital_platform(coll,mats);build_servers(coll,mats);build_market(coll,mats)
    print('Cartridge & Cloud: conceptual expansion modules generated.')

if __name__=='__main__':generate()
