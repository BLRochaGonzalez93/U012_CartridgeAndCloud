"""Cartridge & Cloud modular kit configuration.
Paths are configured for the current repository layout.
"""
from dataclasses import dataclass, field
from pathlib import Path

@dataclass
class Palette:
    graphite: tuple = (0.016, 0.027, 0.036, 1.0)
    dark_surface: tuple = (0.045, 0.070, 0.090, 1.0)
    vrm_green: tuple = (0.031, 0.680, 0.250, 1.0)
    cyan: tuple = (0.085, 0.575, 0.800, 1.0)
    warm_wood: tuple = (0.320, 0.160, 0.070, 1.0)
    light_neutral: tuple = (0.720, 0.790, 0.770, 1.0)
    cardboard: tuple = (0.420, 0.235, 0.095, 1.0)
    glass_tint: tuple = (0.150, 0.280, 0.300, 0.25)
    amber: tuple = (0.780, 0.340, 0.045, 1.0)
    red: tuple = (0.700, 0.060, 0.055, 1.0)
    bluegray: tuple = (0.120, 0.190, 0.260, 1.0)

@dataclass
class KitConfig:
    version: str = "0.1.1"
    prefix: str = "CC_S16_P2"
    unit_scale: float = 1.0
    grid: float = 0.5
    store_width: float = 10.0
    store_depth: float = 15.0
    wall_height: float = 3.0
    wall_thickness: float = 0.20
    bevel_small: float = 0.008
    bevel_medium: float = 0.015
    create_colliders: bool = True
    create_lods: bool = True
    create_materials: bool = True
    collection_root: str = "CC_MODULAR_KIT"
    scripts_root: str = r"D:\_PROGRAMAS\_ARCHIVOS\0_GameDevPortfolio\U012_CartridgeAndCloud\U012_CartridgeAndCloud\Tools\Blender\CC_Blender_Modular_Kit_v0.1\scripts"
    production_root: str = r"D:\_PROGRAMAS\_ARCHIVOS\0_GameDevPortfolio\U012_CartridgeAndCloud\U012_CartridgeAndCloud\Tools\Blender\CC_Blender_Modular_Kit_v0.1\Production"
    export_root: str = r"D:\_PROGRAMAS\_ARCHIVOS\0_GameDevPortfolio\U012_CartridgeAndCloud\U012_CartridgeAndCloud\Tools\Blender\CC_Blender_Modular_Kit_v0.1\Production"
    palette: Palette = field(default_factory=Palette)
    warehouse_bay_width: float = 4.0
    warehouse_bay_depth: float = 6.0
    office_module_width: float = 4.0
    office_module_depth: float = 4.0
    server_room_width: float = 6.0
    server_room_depth: float = 5.0

    def scripts_path(self) -> Path:
        return Path(self.scripts_root)
    def production_path(self) -> Path:
        path = Path(self.production_root); path.mkdir(parents=True, exist_ok=True); return path
    def export_path(self) -> Path:
        path = Path(self.export_root); path.mkdir(parents=True, exist_ok=True); return path

CONFIG = KitConfig()
