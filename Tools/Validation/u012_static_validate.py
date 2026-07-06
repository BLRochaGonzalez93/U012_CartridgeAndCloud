#!/usr/bin/env python3
"""Read-only structural validator for the U012 Cartridge & Cloud reconstruction."""
from __future__ import annotations

import argparse
import hashlib
import json
import re
import sys
from dataclasses import asdict, dataclass
from pathlib import Path
from typing import Iterable

PHYSICAL_GUID = "c81a0fd5d1834d429c998b2d7fd8dbca"
FIXTURE_GUID = "7514d9de3e29bdf6eb683ce3ee627ecc"
NAV_AUTHORING_GUID = "7058912f93674060b72eb7867c853239"
NAV_SURFACE_GUID = "7a5ac11cc976e418e8d13136b07e1f52"
OBSTACLE_SYNC_GUID = "d03c3917e71e404da069d6eff78296c5"
ACTOR_AUTHORING_GUID = "ca6eeb096fa74a7795dc1ba945f75fa5"
CHARACTER_PRESENCE_GUID = "487c8929f4b01caf0d91e823c21e3d22"
LOCOMOTION_GUID = "b809cf83d90645899ae15d44ed78d66e"
DOOR_SENSOR_GUID = "08656c10d9094accab4c04d64538851a"
DELIVERY_VIEW_SCRIPT_GUID = "486c97590a2040c0bd3e859844dced98"
DELIVERY_VIEW_PREFAB_GUID = "f3d1fda2bade47b9a51a2c29c510c0a6"
DISPLAY_SPOT_GUID = "68a299dbfa5844a6a372ea2e2f5403b6"
BROWSE_FIXTURE_GUID = "bb3395558f1c47738b942664a3769b24"

EXPECTED_FURNITURE_ANCHORS = {
    "BackroomStorage.prefab": {"RestockSpot"},
    "CentralShelf.prefab": {"CustomerStandPoint_01", "CustomerStandPoint_02", "ProductDisplaySpots"},
    "CheckoutCounter.prefab": {"EmployeeStandPoint", "CustomerCheckoutStandPoint"},
    "DecorationPlant.prefab": {"InteractionSpot"},
    "FeaturedDisplay.prefab": {"CustomerStandPoint", "ProductDisplaySpots"},
    "LowDisplay.prefab": {"CustomerStandPoint_01", "CustomerStandPoint_02", "ProductDisplaySpots"},
    "ReceivingCrate.prefab": {"CarrySocket", "DropSocket"},
    "WallShelf.prefab": {"CustomerStandPoint", "ProductDisplaySpots"},
}
EXPECTED_PRODUCT_SPOTS = {
    "CentralShelf.prefab": 30,
    "FeaturedDisplay.prefab": 8,
    "LowDisplay.prefab": 12,
    "WallShelf.prefab": 24,
}
EXPECTED_ARCHITECTURE_ROLE_COUNTS = {0: 7, 1: 5, 2: 11, 3: 1}
ARCHIVED_TOOL_NAMES = {
    "ProjectAssetOrganizationMigration.cs",
    "ProjectAssetOrganizationPlayModeRepair.cs",
    "ProjectAssetOrganizationPrefabFixtureRepair.cs",
    "RepresentativeAssetIntegrationTool.cs",
    "RepresentativeRuntimeCatalogLinkRepair.cs",
    "StoreInitialSceneAuthoringTool.cs",
}
LEGACY_CHARACTER_PREFABS = {"Customer.prefab", "Employee.prefab", "Supplier.prefab"}
OLD_PRODUCTION_FILES = {
    "StoreVisualBuilder.cs",
    "StoreVisualPrefabFactory.cs",
    "StoreBlockoutBuilder.cs",
    "StoreBlockoutVisualFactory.cs",
    "CharacterPrefabLoader.cs",
    "CharacterPresentationCatalogAsset.cs",
    "CharacterPrefabAuthoring.cs",
}
OLD_SYMBOLS = {
    "StoreVisualBuilder",
    "StoreVisualPrefabFactory",
    "StoreBlockoutBuilder",
    "StoreBlockoutVisualFactory",
    "CharacterPrefabLoader",
    "CharacterPresentationCatalogAsset",
    "SpawnSupplierPlaceholder",
}


@dataclass
class Result:
    category: str
    check: str
    status: str
    detail: str


class Validator:
    def __init__(self, root: Path):
        self.root = root.resolve()
        self.results: list[Result] = []
        self.metrics: dict[str, object] = {}

    def add(self, category: str, check: str, ok: bool, detail: str) -> None:
        self.results.append(Result(category, check, "PASS" if ok else "FAIL", detail))

    def info(self, category: str, check: str, detail: str) -> None:
        self.results.append(Result(category, check, "INFO", detail))

    def text(self, rel: str) -> str:
        return (self.root / rel).read_text(encoding="utf-8")

    def files(self, rel: str, pattern: str) -> list[Path]:
        base = self.root / rel
        return sorted(base.rglob(pattern)) if base.exists() else []

    def run(self) -> None:
        self.validate_project_identity()
        self.validate_metadata_and_yaml()
        self.validate_prefab_roots()
        self.validate_physical_classification()
        self.validate_furniture()
        self.validate_environment_navigation()
        self.validate_automatic_door()
        self.validate_actors_and_catalogs()
        self.validate_delivery_idempotence()
        self.validate_runtime_cleanup()
        self.validate_archive_and_tests()
        self.validate_build_settings()
        self.validate_assemblies_and_csharp()

    def validate_project_identity(self) -> None:
        required = ["Assets", "Packages", "ProjectSettings"]
        self.add("baseline", "Unity project roots", all((self.root / x).exists() for x in required),
                 ", ".join(required))
        version = self.text("ProjectSettings/ProjectVersion.txt") if (self.root / "ProjectSettings/ProjectVersion.txt").exists() else ""
        self.add("baseline", "Unity version", "6000.3.18f1" in version, version.strip() or "missing")
        manifest_path = self.root / "Packages/manifest.json"
        manifest = json.loads(manifest_path.read_text()) if manifest_path.exists() else {}
        ai_nav = manifest.get("dependencies", {}).get("com.unity.ai.navigation")
        self.add("baseline", "AI Navigation package", ai_nav == "2.0.13", str(ai_nav))

        scripts = self.files("Assets/_Project", "*.cs")
        prefabs = self.files("Assets/_Project", "*.prefab")
        scenes = self.files("Assets/_Project", "*.unity")
        self.metrics.update(project_scripts=len(scripts), project_prefabs=len(prefabs), project_scenes=len(scenes))
        self.add("inventory", "Project script inventory", len(scripts) == 361, f"{len(scripts)} scripts")
        self.add("inventory", "Project prefab inventory", len(prefabs) == 70, f"{len(prefabs)} prefabs")
        self.add("inventory", "Project scene inventory", len(scenes) == 4, f"{len(scenes)} scenes")

    def validate_metadata_and_yaml(self) -> None:
        tracked_types = {".cs", ".prefab", ".unity", ".asset", ".asmdef", ".controller", ".anim", ".mat"}
        missing_meta = []
        for p in self.files("Assets/_Project", "*"):
            if p.is_file() and p.suffix in tracked_types and not Path(str(p) + ".meta").exists():
                missing_meta.append(str(p.relative_to(self.root)))
        self.add("integrity", "Unity file/meta pairs", not missing_meta,
                 "complete" if not missing_meta else "; ".join(missing_meta[:20]))

        guid_to_meta: dict[str, list[str]] = {}
        for p in self.files("Assets", "*.meta"):
            m = re.search(r"^guid:\s*([0-9a-f]{32})\s*$", p.read_text(errors="replace"), re.M)
            if m:
                guid_to_meta.setdefault(m.group(1), []).append(str(p.relative_to(self.root)))
        dupes = {g: paths for g, paths in guid_to_meta.items() if len(paths) > 1}
        self.add("integrity", "Unique Unity GUIDs inside Assets", not dupes,
                 "unique" if not dupes else json.dumps(dict(list(dupes.items())[:10]), ensure_ascii=False))

        # Archived one-shot scripts retain their old metas outside Assets. They must not be referenced by production YAML.
        archived_guids = set()
        archive_root = self.root / "Tools/ArchivedUnityEditorMigrations/U012"
        for p in archive_root.glob("*.meta") if archive_root.exists() else []:
            m = re.search(r"^guid:\s*([0-9a-f]{32})", p.read_text(), re.M)
            if m:
                archived_guids.add(m.group(1))
        stale_refs = []
        unresolved_project_refs = []
        asset_guid_set = set(guid_to_meta)
        for p in self.files("Assets/_Project", "*"):
            if not p.is_file() or p.suffix not in {".prefab", ".unity", ".asset", ".controller"}:
                continue
            txt = p.read_text(errors="replace")
            for guid in re.findall(r"m_Script:\s*\{[^}]*guid:\s*([0-9a-f]{32})", txt):
                if guid in archived_guids:
                    stale_refs.append(f"{p.relative_to(self.root)}:{guid}")
                if guid not in asset_guid_set and guid != NAV_SURFACE_GUID:
                    # Package scripts are external; flag only VRM-looking serialized records when identifiable.
                    if "VRMGames.CartridgeAndCloud" in txt:
                        unresolved_project_refs.append(f"{p.relative_to(self.root)}:{guid}")
        self.add("integrity", "No references to archived migration scripts", not stale_refs,
                 "none" if not stale_refs else "; ".join(stale_refs[:20]))
        # This is informational because Unity packages also serialize script GUIDs outside Assets.
        if unresolved_project_refs:
            self.info("integrity", "External/package script references",
                      f"{len(set(unresolved_project_refs))} unresolved through Assets metadata; package references are expected")

        local_ref_errors = []
        for p in self.files("Assets/_Project", "*.prefab") + self.files("Assets/_Project", "*.unity"):
            txt = p.read_text(errors="replace")
            doc_ids = {int(x) for x in re.findall(r"^--- !u!\d+ &(-?\d+)", txt, re.M)}
            for line_no, line in enumerate(txt.splitlines(), 1):
                if "{fileID:" not in line or "guid:" in line:
                    continue
                for raw in re.findall(r"\{fileID:\s*(-?\d+)\}", line):
                    fid = int(raw)
                    if fid and fid not in doc_ids:
                        local_ref_errors.append(f"{p.relative_to(self.root)}:{line_no}->{fid}")
        self.add("integrity", "Serialized local fileID references", not local_ref_errors,
                 "resolved" if not local_ref_errors else "; ".join(local_ref_errors[:25]))

    @staticmethod
    def yaml_documents(text: str) -> list[tuple[int, int, str]]:
        matches = list(re.finditer(r"^--- !u!(\d+) &(-?\d+)\n", text, re.M))
        docs = []
        for i, m in enumerate(matches):
            end = matches[i + 1].start() if i + 1 < len(matches) else len(text)
            docs.append((int(m.group(1)), int(m.group(2)), text[m.end():end]))
        return docs

    @staticmethod
    def gameobject_map(text: str) -> dict[int, dict[str, object]]:
        result: dict[int, dict[str, object]] = {}
        for class_id, file_id, body in Validator.yaml_documents(text):
            if class_id != 1:
                continue
            name = re.search(r"^  m_Name:\s*(.*)$", body, re.M)
            comp = [int(x) for x in re.findall(r"^  - component:\s*\{fileID:\s*(-?\d+)\}", body, re.M)]
            result[file_id] = {"name": name.group(1).strip() if name else "", "components": comp}
        return result

    def validate_prefab_roots(self) -> None:
        errors = []
        for p in self.files("Assets/_Project", "*.prefab"):
            txt = p.read_text(errors="replace")
            roots = []
            for class_id, file_id, body in self.yaml_documents(txt):
                if class_id not in {4, 224} or not re.search(r"^  m_Father:\s*\{fileID:\s*0\}", body, re.M):
                    continue
                pos = re.search(r"^  m_LocalPosition:\s*\{x:\s*([^,]+), y:\s*([^,]+), z:\s*([^}]+)\}", body, re.M)
                rot = re.search(r"^  m_LocalRotation:\s*\{x:\s*([^,]+), y:\s*([^,]+), z:\s*([^,]+), w:\s*([^}]+)\}", body, re.M)
                scale = re.search(r"^  m_LocalScale:\s*\{x:\s*([^,]+), y:\s*([^,]+), z:\s*([^}]+)\}", body, re.M)
                roots.append((file_id, pos, rot, scale))
            if len(roots) != 1:
                errors.append(f"{p.relative_to(self.root)} roots={len(roots)}")
                continue
            _, pos, rot, scale = roots[0]
            def nums(match): return tuple(float(v.strip()) for v in match.groups()) if match else ()
            if nums(pos) != (0.0, 0.0, 0.0) or nums(rot) != (0.0, 0.0, 0.0, 1.0) or nums(scale) != (1.0, 1.0, 1.0):
                errors.append(str(p.relative_to(self.root)))
        self.add("prefabs", "Identity root transforms", not errors,
                 "all project prefabs" if not errors else "; ".join(errors[:20]))

    @staticmethod
    def contract_role(text: str) -> int | None:
        pos = text.find(PHYSICAL_GUID)
        if pos < 0:
            return None
        m = re.search(r"\n  _role:\s*(\d+)", text[pos:])
        return int(m.group(1)) if m else None

    def validate_physical_classification(self) -> None:
        for folder, expected_count, required_role in [
            ("Architecture", 24, None), ("Products", 10, 4), ("Expansions", 19, 5), ("Furniture", 8, 3)
        ]:
            files = self.files(f"Assets/_Project/Prefabs/{folder}", "*.prefab")
            roles = [self.contract_role(p.read_text()) for p in files]
            ok = len(files) == expected_count and all(r is not None for r in roles)
            if required_role is not None:
                ok = ok and all(r == required_role for r in roles)
            detail = f"count={len(files)}, roles={dict((r, roles.count(r)) for r in sorted(set(roles), key=lambda x: -1 if x is None else x))}"
            self.add("prefabs", f"{folder} physical contracts", ok, detail)
        architecture = self.files("Assets/_Project/Prefabs/Architecture", "*.prefab")
        role_counts = {r: 0 for r in EXPECTED_ARCHITECTURE_ROLE_COUNTS}
        for p in architecture:
            role = self.contract_role(p.read_text())
            if role in role_counts:
                role_counts[role] += 1
        self.add("prefabs", "Architecture role classification", role_counts == EXPECTED_ARCHITECTURE_ROLE_COUNTS,
                 str(role_counts))

    def validate_furniture(self) -> None:
        files = self.files("Assets/_Project/Prefabs/Furniture", "*.prefab")
        errors = []
        for p in files:
            txt = p.read_text()
            names = set(re.findall(r"^  m_Name:\s*(.+)$", txt, re.M))
            required = {"Collision", "Anchors"} | EXPECTED_FURNITURE_ANCHORS.get(p.name, set())
            missing = sorted(required - names)
            if missing:
                errors.append(f"{p.name}: missing {missing}")
            tokens = ["BoxCollider:", "NavMeshObstacle:", OBSTACLE_SYNC_GUID, FIXTURE_GUID, PHYSICAL_GUID]
            absent = [token for token in tokens if token not in txt]
            if absent:
                errors.append(f"{p.name}: absent {absent}")
            expected_spots = EXPECTED_PRODUCT_SPOTS.get(p.name, 0)
            actual_spots = len(re.findall(r"^  m_Name:\s*ProductSpot_\d+", txt, re.M))
            if actual_spots != expected_spots:
                errors.append(f"{p.name}: product spots {actual_spots}/{expected_spots}")
            if expected_spots and DISPLAY_SPOT_GUID not in txt:
                errors.append(f"{p.name}: missing ProductDisplaySpotSet")
            if "CustomerStandPoint" in txt and BROWSE_FIXTURE_GUID not in txt and p.name != "CheckoutCounter.prefab":
                errors.append(f"{p.name}: missing customer browse authoring")
        self.add("furniture", "Collision, obstacle and functional anchors", not errors,
                 "8 complete prefabs" if not errors else "; ".join(errors[:25]))

    def validate_environment_navigation(self) -> None:
        rel = "Assets/_Project/Prefabs/Store/StoreInitialEnvironment.prefab"
        p = self.root / rel
        txt = p.read_text() if p.exists() else ""
        names = self.gameobject_map(txt)
        name_to_go = {str(v["name"]): k for k, v in names.items()}
        docs = {fid: (cid, body) for cid, fid, body in self.yaml_documents(txt)}
        missing = []
        for name in ["Collision", "Navigation", "Anchors", "WalkableBase_Interior", "WalkableBase_Exterior", "EntranceTransition"]:
            if name not in name_to_go:
                missing.append(name)
        nav_source_errors = []
        for name in ["WalkableBase_Interior", "WalkableBase_Exterior", "EntranceTransition"]:
            goid = name_to_go.get(name)
            if goid is None:
                continue
            comps = names[goid]["components"]
            if not any(docs.get(cid, (None, ""))[0] == 65 for cid in comps):
                nav_source_errors.append(name)
        checks = {
            "required hierarchy": not missing,
            "three collider-backed walkable sources": not nav_source_errors,
            "one NavMeshSurface": txt.count(NAV_SURFACE_GUID) == 1,
            "authored navigation component": NAV_AUTHORING_GUID in txt,
            "CollectObjects.Children": "m_CollectObjects: 2" in txt,
            "PhysicsColliders geometry": "m_UseGeometry: 1" in txt,
            "no legacy TechnicalColliders": "m_Name: TechnicalColliders" not in txt,
        }
        self.add("navigation", "Environment navigation authority", all(checks.values()),
                 "; ".join(f"{k}={v}" for k, v in checks.items()) + (f"; missing={missing}; noCollider={nav_source_errors}" if missing or nav_source_errors else ""))

        scene = self.text("Assets/_Project/Scenes/Production/StoreInitial.unity")
        scene_ok = ("m_Name: PlacementSurfaceGrid" in scene and "m_Name: EntranceApron" not in scene and
                    "m_Name: EntranceThreshold" not in scene and "m_Name: WalkableFloor" not in scene)
        self.add("navigation", "Scene navigation cleanup", scene_ok,
                 "PlacementSurfaceGrid retained; duplicate entry/walkable objects absent")

        controller = self.text("Assets/_Project/Scripts/Runtime/Navigation/DynamicStoreNavMeshController.cs")
        forbidden = ["AddComponent<NavMeshSurface>", "FindObjectsByType", "FindObjectsOfType", "CollectObjects.Volume", "StateChanged +="]
        ctrl_ok = all(x not in controller for x in forbidden) and "CollectObjects.Children" in controller and "PhysicsColliders" in controller
        self.add("navigation", "Single runtime navigation source", ctrl_ok,
                 "authored Navigation subtree only; no global scans or economy subscriptions")

    def validate_automatic_door(self) -> None:
        txt = self.text("Assets/_Project/Prefabs/Architecture/Modular/AutomaticDoor.prefab")
        required_names = {"Sensor", "Anchors", "ExteriorStandPoint", "InteriorStandPoint", "EntranceCenter"}
        names = set(re.findall(r"^  m_Name:\s*(.+)$", txt, re.M))
        prefab_ok = (required_names <= names and txt.count("BoxCollider:") >= 3 and "Rigidbody:" in txt and
                     DOOR_SENSOR_GUID in txt and self.contract_role(txt) == 3)
        self.add("door", "Self-contained automatic door prefab", prefab_ok,
                 f"colliders={txt.count('BoxCollider:')}, names complete={required_names <= names}")
        ctrl = self.text("Assets/_Project/Scripts/Presentation/Store/Doors/AutomaticSlidingDoorController.cs")
        no_fallback = "FindObjects" not in ctrl and "CharacterPresence" not in ctrl and "requires its authored sensor" in ctrl
        self.add("door", "No global sensor fallback", no_fallback, "trigger-owned sensor is mandatory")

    def validate_actors_and_catalogs(self) -> None:
        actor_files = self.files("Assets/_Project/Resources/Characters", "*.prefab")
        errors = []
        roles = []
        for p in actor_files:
            txt = p.read_text()
            role = self.contract_role(txt)  # actor uses another role field; parse after Actor GUID instead.
            pos = txt.find(ACTOR_AUTHORING_GUID)
            m = re.search(r"\n  _role:\s*(\d+)", txt[pos:]) if pos >= 0 else None
            roles.append(int(m.group(1)) if m else None)
            tokens = ["CapsuleCollider:", "NavMeshAgent:", ACTOR_AUTHORING_GUID, CHARACTER_PRESENCE_GUID, LOCOMOTION_GUID]
            absent = [x for x in tokens if x not in txt]
            if absent:
                errors.append(f"{p.name}: absent {absent}")
            cap = re.search(r"CapsuleCollider:.*?^  m_IsTrigger:\s*(\d+)", txt, re.S | re.M)
            if not cap or cap.group(1) != "1":
                errors.append(f"{p.name}: capsule must be trigger")
            if "Suppliers" in str(p) and "m_Name: CarrySocket" not in txt:
                errors.append(f"{p.name}: missing CarrySocket")
        expected_roles = {0: 2, 1: 2, 2: 2, 3: 1}
        actual_roles = {r: roles.count(r) for r in expected_roles}
        self.add("actors", "Seven complete actor wrapper prefabs", len(actor_files) == 7 and not errors and actual_roles == expected_roles,
                 f"count={len(actor_files)}, roles={actual_roles}" + (f"; {'; '.join(errors)}" if errors else ""))

        scene = self.text("Assets/_Project/Scenes/Production/StoreInitial.unity")
        technical_player_ok = ("m_Name: TechnicalPlayer" in scene and
                               "_characterId: player" in scene and
                               CHARACTER_PRESENCE_GUID in scene)
        self.add("actors", "Technical player presence is authored", technical_player_ok,
                 "scene-owned CharacterPresence; no runtime registration required")

        bootstrap = self.text("Assets/_Project/Scripts/Runtime/Characters/PlayerCharacterVisualBootstrap.cs")
        player_visual_ok = ("GetComponentsInChildren<NavMeshAgent>" in bootstrap and
                            "agent.enabled = false" in bootstrap and
                            "GetComponentsInChildren<Collider>" in bootstrap)
        self.add("actors", "Player visual physical components are disabled", player_visual_ok,
                 "visual collider and NavMeshAgent cannot duplicate TechnicalPlayer physics")

        legacy = [p for p in self.files("Assets/_Project/Prefabs/Characters", "*.prefab") if p.name in LEGACY_CHARACTER_PREFABS]
        self.add("actors", "Legacy generic character prefabs removed", not legacy,
                 "removed" if not legacy else "; ".join(str(p.relative_to(self.root)) for p in legacy))

        presentation = self.text("Assets/_Project/Data/Catalogs/PresentationCatalog.asset")
        actor_section = presentation.split("  _actors:", 1)[1].split("  _animations:", 1)[0] if "  _actors:" in presentation else ""
        actor_entries = re.findall(r"^  - id:\s*(\S+)", actor_section, re.M)
        catalog_ok = len(actor_entries) == 7 and DELIVERY_VIEW_PREFAB_GUID in presentation and "Characters/Customer" not in presentation
        self.add("catalogs", "Direct actor and delivery references", catalog_ok,
                 f"actors={actor_entries}, deliveryPrefab={DELIVERY_VIEW_PREFAB_GUID in presentation}")

        representative = self.text("Assets/_Project/Data/Catalogs/RepresentativePrefabCatalog.asset")
        self.add("catalogs", "Future expansions excluded from StoreInitial runtime", "_expansions: []" in representative,
                 "assets retained under Prefabs/Expansions; runtime list empty")

    def validate_delivery_idempotence(self) -> None:
        registry = self.text("Assets/_Project/Scripts/Runtime/Characters/DeliveryPresentationRegistry.cs")
        presenter = self.text("Assets/_Project/Scripts/Runtime/Characters/SupplierDeliveryPresenter.cs")
        facade = self.text("Assets/_Project/Scripts/Application/Store/StoreOperationsFacade.cs")
        root = self.text("Assets/_Project/Scripts/Runtime/Composition/StoreRuntimeCompositionRoot.cs")
        view = self.text("Assets/_Project/Prefabs/Characters/Suppliers/SupplierDeliveryView.prefab")
        registry_ok = all(x in registry for x in ["_active", "_completed", "TryBegin", "Complete", "HasSeen", "Normalize"])
        presenter_ok = all(x in presenter for x in ["DeliveryPresentationRegistry", "TryBegin", "SupplierDeliveryViewPrefab", "receiving-crate", "CharacterPrefabFactory", "StorePrefabFactory"]) and "CreatePrimitive" not in presenter and "GetHashCode" not in presenter
        correlation_ok = "correlationId: order.OrderId" in facade and "feedback.CorrelationId" in root and "OrderReceived" in root
        view_ok = DELIVERY_VIEW_SCRIPT_GUID in view and "m_Name: SupplierMount" in view and "m_Name: CrateMount" in view
        self.add("delivery", "Idempotent delivery presentation", registry_ok and presenter_ok and correlation_ok and view_ok,
                 f"registry={registry_ok}; presenter={presenter_ok}; correlation={correlation_ok}; authoredView={view_ok}")

    def validate_runtime_cleanup(self) -> None:
        production_cs = self.files("Assets/_Project/Scripts", "*.cs")
        present_names = {p.name for p in production_cs}
        old_files_present = sorted(OLD_PRODUCTION_FILES & present_names)
        symbol_hits = []
        primitive_hits = []
        for p in production_cs:
            txt = p.read_text(errors="replace")
            for symbol in OLD_SYMBOLS:
                if symbol in txt:
                    symbol_hits.append(f"{p.relative_to(self.root)}:{symbol}")
            if "GameObject.CreatePrimitive(" in txt:
                primitive_hits.append(str(p.relative_to(self.root)))
        self.add("cleanup", "Experimental builders/loaders removed", not old_files_present and not symbol_hits,
                 "removed" if not old_files_present and not symbol_hits else f"files={old_files_present}; symbols={symbol_hits[:20]}")
        self.add("cleanup", "No primitive fallback in production", not primitive_hits,
                 "none" if not primitive_hits else "; ".join(primitive_hits))

        binder = self.text("Assets/_Project/Scripts/Runtime/Store/AuthoredStoreRuntimeBinder.cs")
        factory = self.text("Assets/_Project/Scripts/Runtime/Store/StorePrefabFactory.cs")
        binder_ok = ("Bind(StoreInitialSceneContext" in binder and
                     "Build(" not in binder and
                     "CreatePrimitive" not in binder and
                     "AddComponent" not in binder and
                     "RegisterPlayer" not in binder)
        factory_ok = "ValidateFurniturePrefab" in factory and "CreatePrimitive" not in factory and "FindLoaded" in factory
        self.add("cleanup", "Authored binder and strict prefab factory", binder_ok and factory_ok,
                 f"binder={binder_ok}; factory={factory_ok}")

        settings = self.text("Assets/_Project/Scripts/Infrastructure/Store/StoreRuntimeSettingsAsset.cs")
        self.add("cleanup", "Legacy blockout settings removed", "Blockout" not in settings and "MaximumCustomers" in settings,
                 "StoreRuntimeSettings uses authored-store terminology")

    def validate_archive_and_tests(self) -> None:
        archive = self.root / "Tools/ArchivedUnityEditorMigrations/U012"
        archived = {p.name for p in archive.glob("*.cs")} if archive.exists() else set()
        assets_names = {p.name for p in self.files("Assets/_Project/Editor", "*.cs")}
        self.add("cleanup", "One-shot editor migrations archived", archived == ARCHIVED_TOOL_NAMES and not (ARCHIVED_TOOL_NAMES & assets_names),
                 f"archived={sorted(archived)}")
        installer = self.text("Assets/_Project/Editor/ProjectOrganization/AssetPipeline/SyntyCharacterIntegrationInstaller.cs")
        installer_ok = ("MenuItem(" in installer and
                        "InitializeOnLoad" not in installer and
                        "DidReloadScripts" not in installer and
                        "EnsureActorContract" in installer and
                        "ActorPrefabAuthoring" in installer and
                        "NavMeshAgent" in installer)
        self.add("cleanup", "Synty installer is explicit and contract-safe", installer_ok,
                 "manual execution only; regenerated wrappers keep collider, agent and authoring profile")

        runtime_scenarios = self.files("Assets/_Project/Scripts/Runtime/Development/Scenarios", "*.cs")
        support_scenarios = self.files("Assets/_Project/Tests/TestSupport/Scenarios", "*.cs")
        self.add("tests", "Scenario runners isolated to TestSupport", not runtime_scenarios and len(support_scenarios) == 7,
                 f"runtime={len(runtime_scenarios)}, testSupport={len(support_scenarios)}")
        structural_tests = self.root / "Assets/_Project/Tests/EditMode/Store/StoreStructuralContractTests.cs"
        validator = self.root / "Assets/_Project/Editor/ProjectOrganization/Validation/StoreStructuralValidationTool.cs"
        self.add("tests", "Read-only structural validation assets", structural_tests.exists() and validator.exists() and "SaveAssets" not in validator.read_text(),
                 "editor validator and EditMode contract suite present")

    def validate_build_settings(self) -> None:
        txt = self.text("ProjectSettings/EditorBuildSettings.asset")
        testlab = re.search(r"- enabled:\s*(\d+)\n\s+path:\s*Assets/_Project/Scenes/Test/TestLab.unity", txt)
        production_enabled = all(re.search(rf"- enabled:\s*1\n\s+path:\s*Assets/_Project/Scenes/Production/{name}\.unity", txt) for name in ["Bootstrap", "MainMenu", "StoreInitial"])
        self.add("build", "Production scene list", production_enabled and testlab is not None and testlab.group(1) == "0",
                 f"production enabled={production_enabled}; TestLab enabled={testlab.group(1) if testlab else 'missing'}")

    @staticmethod
    def strip_csharp_noncode(text: str) -> str:
        out = []
        i = 0
        state = "code"
        while i < len(text):
            c = text[i]
            n = text[i + 1] if i + 1 < len(text) else ""
            if state == "code":
                if c == "/" and n == "/": state = "line"; out.extend("  "); i += 2; continue
                if c == "/" and n == "*": state = "block"; out.extend("  "); i += 2; continue
                if c == '"': state = "string"; out.append(" "); i += 1; continue
                if c == "'": state = "char"; out.append(" "); i += 1; continue
                out.append(c); i += 1; continue
            if state == "line":
                if c == "\n": state = "code"; out.append("\n")
                else: out.append(" ")
                i += 1; continue
            if state == "block":
                if c == "*" and n == "/": state = "code"; out.extend("  "); i += 2
                else: out.append("\n" if c == "\n" else " "); i += 1
                continue
            if state in {"string", "char"}:
                if c == "\\": out.extend("  "); i += 2; continue
                if (state == "string" and c == '"') or (state == "char" and c == "'"):
                    state = "code"
                out.append("\n" if c == "\n" else " "); i += 1
        return "".join(out)

    def validate_assemblies_and_csharp(self) -> None:
        asmdefs = self.files("Assets/_Project", "*.asmdef")
        parsed = {}
        bad_json = []
        for p in asmdefs:
            try:
                data = json.loads(p.read_text())
                parsed[data["name"]] = data
            except Exception as exc:
                bad_json.append(f"{p.relative_to(self.root)}:{exc}")
        self.add("compile-static", "Assembly definition JSON", not bad_json,
                 f"{len(parsed)} assemblies" if not bad_json else "; ".join(bad_json))
        expected_refs = {
            "VRMGames.CartridgeAndCloud.Runtime": "Unity.AI.Navigation",
            "VRMGames.CartridgeAndCloud.Editor.ProjectOrganization": "Unity.AI.Navigation",
            "VRMGames.CartridgeAndCloud.Tests.EditMode": "VRMGames.CartridgeAndCloud.Tests.Support",
            "VRMGames.CartridgeAndCloud.Tests.PlayMode": "VRMGames.CartridgeAndCloud.Tests.Support",
        }
        ref_errors = []
        for asm, ref in expected_refs.items():
            if asm not in parsed or ref not in parsed[asm].get("references", []):
                ref_errors.append(f"{asm}->{ref}")
        self.add("compile-static", "Required assembly references", not ref_errors,
                 "present" if not ref_errors else "; ".join(ref_errors))

        balance_errors = []
        duplicate_types: dict[str, list[str]] = {}
        for p in self.files("Assets/_Project", "*.cs"):
            txt = p.read_text(errors="replace")
            code = self.strip_csharp_noncode(txt)
            pairs = [("{", "}"), ("(", ")"), ("[", "]")]
            for opening, closing in pairs:
                depth = 0
                for ch in code:
                    if ch == opening: depth += 1
                    elif ch == closing: depth -= 1
                    if depth < 0: break
                if depth != 0:
                    balance_errors.append(f"{p.relative_to(self.root)}:{opening}{closing}={depth}")
            ns = re.search(r"\bnamespace\s+([A-Za-z0-9_.]+)", code)
            declarations = list(re.finditer(
                r"\b(class|struct|interface|enum)\s+([A-Za-z_][A-Za-z0-9_]*)",
                code))
            if declarations:
                depths = [
                    code[:match.start()].count("{") - code[:match.start()].count("}")
                    for match in declarations
                ]
                top_level_depth = min(depths)
                for match, depth in zip(declarations, depths):
                    if depth != top_level_depth:
                        continue
                    name = match.group(2)
                    key = (ns.group(1) + "." if ns else "") + name
                    duplicate_types.setdefault(key, []).append(
                        str(p.relative_to(self.root)))
        duplicates = {k: v for k, v in duplicate_types.items() if len(v) > 1}
        self.add("compile-static", "C# lexical balance", not balance_errors,
                 "balanced" if not balance_errors else "; ".join(balance_errors[:25]))
        self.add("compile-static", "Unique declared type names", not duplicates,
                 "unique" if not duplicates else json.dumps(dict(list(duplicates.items())[:15]), ensure_ascii=False))

    @property
    def failed(self) -> list[Result]:
        return [r for r in self.results if r.status == "FAIL"]

    def payload(self) -> dict[str, object]:
        return {
            "root": str(self.root),
            "status": "PASS" if not self.failed else "FAIL",
            "summary": {
                "pass": sum(r.status == "PASS" for r in self.results),
                "fail": len(self.failed),
                "info": sum(r.status == "INFO" for r in self.results),
            },
            "metrics": self.metrics,
            "results": [asdict(r) for r in self.results],
        }

    def markdown(self) -> str:
        payload = self.payload()
        lines = [
            "# U012 — Resultado de validación estructural estática",
            "",
            f"**Estado:** {payload['status']}",
            f"**Raíz validada:** `{self.root}`",
            f"**Comprobaciones:** {payload['summary']['pass']} PASS · {payload['summary']['fail']} FAIL · {payload['summary']['info']} INFO",
            "",
            "| Categoría | Comprobación | Estado | Detalle |",
            "|---|---|---:|---|",
        ]
        for r in self.results:
            detail = r.detail.replace("|", "\\|").replace("\n", " ")
            lines.append(f"| {r.category} | {r.check} | **{r.status}** | {detail} |")
        lines.append("")
        lines.append("Esta validación comprueba estructura serializada, referencias, contratos de prefab y coherencia de código. No sustituye la importación, compilación, pruebas EditMode/PlayMode, bake de NavMesh ni build dentro de Unity.")
        return "\n".join(lines) + "\n"


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("root", nargs="?", default=".")
    parser.add_argument("--json", dest="json_path")
    parser.add_argument("--markdown", dest="markdown_path")
    args = parser.parse_args()
    validator = Validator(Path(args.root))
    validator.run()
    payload = validator.payload()
    if args.json_path:
        Path(args.json_path).write_text(json.dumps(payload, indent=2, ensure_ascii=False) + "\n")
    if args.markdown_path:
        Path(args.markdown_path).write_text(validator.markdown())
    print(json.dumps(payload["summary"], ensure_ascii=False))
    for result in validator.failed:
        print(f"FAIL [{result.category}] {result.check}: {result.detail}")
    return 1 if validator.failed else 0


if __name__ == "__main__":
    raise SystemExit(main())
