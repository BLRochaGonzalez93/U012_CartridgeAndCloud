using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRMGames.CartridgeAndCloud.Infrastructure.VerticalSlicePhase1;

namespace VRMGames.CartridgeAndCloud.Editor.ProjectOrganization
{
    public static class RepresentativeAssetIntegrationTool
    {
        private const string PreferredExportRelativePath =
            "Tools/Blender/" +
            "CC_Blender_Modular_Kit_v0.1/" +
            "Production/Exports";

        private const string FallbackExportRelativePath =
            "Tools/Blender/" +
            "CC_Blender_Modular_Kit_v0.1/" +
            "Production";

        private const string ModelRoot =
            "Assets/_Project/Art/Models";

        private const string PrefabRoot =
            "Assets/_Project/Prefabs";

        private const string CatalogPath =
            "Assets/_Project/Data/Catalogs/" +
            "RepresentativePrefabCatalog.asset";

        private const string ReportPath =
            "Assets/_Project/Reports/" +
            "RepresentativeAssetIntegrationReport.json";

        private const string FactorySourcePath =
            "Assets/_Project/Scripts/Infrastructure/" +
            "VerticalSlicePhase1/" +
            "Phase1BlockoutVisualFactory.cs";

        private const string StoreBuilderSourcePath =
            "Assets/_Project/Scripts/Runtime/" +
            "VerticalSlicePhase1/" +
            "Phase1StoreBlockoutBuilder.cs";

        private static readonly Dictionary<
            string,
            Material> Materials =
                new Dictionary<
                    string,
                    Material>(
                        StringComparer.Ordinal);

        [MenuItem(
            "Tools/Cartridge & Cloud/" +
            "Representative Assets/" +
            "Import FBX and Build Prefabs")]
        public static void Integrate()
        {
            if (EditorApplication
                .isPlayingOrWillChangePlaymode)
            {
                EditorUtility.DisplayDialog(
                    "Representative asset integration",
                    "Exit Play Mode before running the integration.",
                    "OK");
                return;
            }

            bool proceed =
                EditorUtility.DisplayDialog(
                    "Import FBX and build prefabs",
                    "This operation will:\n\n" +
                    "- Copy all FBX exports into Assets/_Project/Art/Models.\n" +
                    "- Configure Unity model importers and URP materials.\n" +
                    "- Build or update architecture, furniture, product and expansion prefabs.\n" +
                    "- Preserve the existing furniture and product prefab GUIDs.\n" +
                    "- Create a preloaded representative prefab catalog.\n" +
                    "- Patch the runtime to prefer the new prefabs while retaining procedural fallback.\n\n" +
                    "A clean Git commit should exist before continuing.",
                    "Integrate",
                    "Cancel");

            if (!proceed)
            {
                return;
            }

            Scene previewScene = default;
            List<GameObject> previewRoots =
                new List<GameObject>();

            try
            {
                string projectRoot =
                    ProjectRoot();
                string sourceRoot =
                    ResolveSourceRoot(
                        projectRoot);

                string[] sourceFiles =
                    Directory.GetFiles(
                        sourceRoot,
                        "*.fbx",
                        SearchOption.AllDirectories);

                if (sourceFiles.Length == 0)
                {
                    throw new FileNotFoundException(
                        "No FBX files were found under: " +
                        sourceRoot);
                }

                EnsureProjectFolders();
                CreateOrUpdateMaterials();

                List<string> importedPaths =
                    CopyAndImportModels(
                        projectRoot,
                        sourceRoot,
                        sourceFiles);

                previewScene =
                    EditorSceneManager
                        .NewPreviewScene();

                List<NodeRecord> nodes =
                    BuildNodeIndex(
                        importedPaths,
                        previewScene,
                        previewRoots);

                List<AssetSpec> specs =
                    CoreSpecs();

                specs.AddRange(
                    DiscoverExpansionSpecs(
                        nodes));

                ValidateRequiredSources(
                    specs,
                    nodes);

                List<BuiltRecord> built =
                    new List<BuiltRecord>();

                foreach (AssetSpec spec
                         in specs)
                {
                    NodeRecord source =
                        FindSourceNode(
                            nodes,
                            spec.SourceSuffix);

                    if (source == null)
                    {
                        if (spec.Required)
                        {
                            throw new InvalidOperationException(
                                "Required model root was not found: " +
                                spec.SourceSuffix);
                        }

                        continue;
                    }

                    BuiltRecord record =
                        BuildPrefab(
                            spec,
                            source,
                            nodes);

                    built.Add(record);
                }

                RepresentativePrefabCatalogAsset
                    catalog =
                        CreateOrUpdateCatalog(
                            built);

                AddPreloadedAsset(catalog);
                PatchRuntimeSources();
                WriteReport(
                    sourceRoot,
                    importedPaths,
                    built,
                    catalog);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(
                    ImportAssetOptions
                        .ForceSynchronousImport);

                Debug.Log(
                    "[Representative Assets] Integration completed. " +
                    "FBX: " +
                    importedPaths.Count +
                    ", prefabs: " +
                    built.Count +
                    ".");

                EditorUtility.DisplayDialog(
                    "Representative asset integration",
                    "Integration completed.\n\n" +
                    "Unity will recompile the patched runtime.\n\n" +
                    "After compilation run:\n" +
                    "1. Validate Representative Integration\n" +
                    "2. Phase1AssetAuthoringTests\n" +
                    "3. Full EditMode\n" +
                    "4. Phase 1 PlayMode (17 tests)\n" +
                    "5. Full PlayMode\n" +
                    "6. Manual Golden Path\n" +
                    "7. Windows build and external executable",
                    "OK");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);

                EditorUtility.DisplayDialog(
                    "Representative asset integration failed",
                    exception.Message +
                    "\n\nNo commit or push was performed.",
                    "OK");
            }
            finally
            {
                foreach (GameObject root
                         in previewRoots)
                {
                    if (root != null)
                    {
                        UnityEngine.Object
                            .DestroyImmediate(
                                root);
                    }
                }

                if (previewScene.IsValid())
                {
                    EditorSceneManager
                        .ClosePreviewScene(
                            previewScene);
                }
            }
        }

        [MenuItem(
            "Tools/Cartridge & Cloud/" +
            "Representative Assets/" +
            "Validate Representative Integration")]
        public static void ValidateIntegration()
        {
            List<string> errors =
                new List<string>();

            RepresentativePrefabCatalogAsset catalog =
                AssetDatabase.LoadAssetAtPath<
                    RepresentativePrefabCatalogAsset>(
                        CatalogPath);

            if (catalog == null)
            {
                errors.Add(
                    "Representative prefab catalog is missing.");
            }
            else
            {
                ValidateCatalogEntry(
                    catalog.FindFurniture(
                        "checkout-counter"),
                    "checkout-counter",
                    errors);
                ValidateCatalogEntry(
                    catalog.FindFurniture(
                        "central-shelf"),
                    "central-shelf",
                    errors);
                ValidateCatalogEntry(
                    catalog.FindProduct(
                        "game-neon-drift"),
                    "game-neon-drift",
                    errors);
                ValidateCatalogEntry(
                    catalog.FindArchitecture(
                        "architecture.floor.200"),
                    "architecture.floor.200",
                    errors);
                ValidateCatalogEntry(
                    catalog.FindArchitecture(
                        "architecture.automatic-door"),
                    "architecture.automatic-door",
                    errors);

                bool preloaded =
                    PlayerSettings
                        .GetPreloadedAssets()
                        .Contains(catalog);

                if (!preloaded)
                {
                    errors.Add(
                        "Representative prefab catalog is not preloaded.");
                }
            }

            ValidateSourcePatch(
                FactorySourcePath,
                "RepresentativePrefabFactory",
                errors);
            ValidateSourcePatch(
                StoreBuilderSourcePath,
                "RepresentativeStoreVisualBuilder",
                errors);

            if (errors.Count == 0)
            {
                Debug.Log(
                    "[Representative Assets] Validation PASS.");

                EditorUtility.DisplayDialog(
                    "Representative assets",
                    "Validation PASS.",
                    "OK");
                return;
            }

            string detail =
                string.Join(
                    "\n",
                    errors);

            Debug.LogError(
                "[Representative Assets] Validation failed:\n" +
                detail);

            EditorUtility.DisplayDialog(
                "Representative assets",
                "Validation failed:\n\n" +
                detail,
                "OK");
        }

        private static string ResolveSourceRoot(
            string projectRoot)
        {
            string preferred =
                Path.Combine(
                    projectRoot,
                    PreferredExportRelativePath
                        .Replace(
                            '/',
                            Path.DirectorySeparatorChar));

            if (Directory.Exists(preferred))
            {
                return preferred;
            }

            string fallback =
                Path.Combine(
                    projectRoot,
                    FallbackExportRelativePath
                        .Replace(
                            '/',
                            Path.DirectorySeparatorChar));

            if (Directory.Exists(fallback))
            {
                return fallback;
            }

            throw new DirectoryNotFoundException(
                "The Blender export folder was not found.\n" +
                "Checked:\n" +
                preferred +
                "\n" +
                fallback);
        }

        private static void EnsureProjectFolders()
        {
            string[] folders =
            {
                ModelRoot + "/Architecture",
                ModelRoot + "/Furniture",
                ModelRoot + "/Products",
                ModelRoot + "/Expansions",
                ModelRoot + "/Generated/LODs",
                ModelRoot + "/Generated/Colliders",
                ModelRoot + "/Misc",
                "Assets/_Project/Art/Materials/Representative",
                PrefabRoot + "/Architecture",
                PrefabRoot + "/Furniture",
                PrefabRoot + "/Products",
                PrefabRoot + "/Products/Packaging",
                PrefabRoot + "/Expansions",
                "Assets/_Project/Data/Catalogs",
                "Assets/_Project/Reports"
            };

            foreach (string folder in folders)
            {
                EnsureFolder(folder);
            }
        }

        private static List<string>
            CopyAndImportModels(
                string projectRoot,
                string sourceRoot,
                IEnumerable<string> sourceFiles)
        {
            List<string> imported =
                new List<string>();
            HashSet<string> destinations =
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

            foreach (string sourceFile
                     in sourceFiles
                         .OrderBy(
                             path =>
                                 path,
                             StringComparer.OrdinalIgnoreCase))
            {
                string folder =
                    ClassifyModelFolder(
                        sourceRoot,
                        sourceFile);

                string destinationAssetPath =
                    folder +
                    "/" +
                    Path.GetFileName(
                        sourceFile);

                if (!destinations.Add(
                        destinationAssetPath))
                {
                    throw new InvalidOperationException(
                        "Two source FBX files map to the same destination: " +
                        destinationAssetPath);
                }

                DeleteLegacyMisclassifiedCopy(
                    sourceFile,
                    destinationAssetPath);

                string destinationAbsolute =
                    Path.Combine(
                        projectRoot,
                        destinationAssetPath
                            .Replace(
                                '/',
                                Path.DirectorySeparatorChar));

                Directory.CreateDirectory(
                    Path.GetDirectoryName(
                        destinationAbsolute));

                File.Copy(
                    sourceFile,
                    destinationAbsolute,
                    true);

                AssetDatabase.ImportAsset(
                    destinationAssetPath,
                    ImportAssetOptions
                        .ForceSynchronousImport |
                    ImportAssetOptions
                        .ForceUpdate);

                ConfigureModelImporter(
                    destinationAssetPath);

                imported.Add(
                    destinationAssetPath);
            }

            return imported;
        }

        private static string ClassifyModelFolder(
            string sourceRoot,
            string sourceFile)
        {
            string relativePath =
                sourceFile.StartsWith(
                    sourceRoot,
                    StringComparison.OrdinalIgnoreCase)
                    ? sourceFile.Substring(
                        sourceRoot.Length)
                    : sourceFile;

            relativePath =
                relativePath
                    .Replace(
                        '\\',
                        '/')
                    .TrimStart('/');

            string upperPath =
                "/" +
                relativePath.ToUpperInvariant();

            string fileName =
                Path.GetFileNameWithoutExtension(
                    sourceFile);

            string upperFileName =
                fileName.ToUpperInvariant();

            bool explicitColliderExport =
                upperPath.Contains(
                    "/COLLIDERS/",
                    StringComparison.Ordinal) ||
                upperPath.Contains(
                    "/GENERATED_COLLIDERS/",
                    StringComparison.Ordinal) ||
                upperFileName.Contains(
                    "_COLLIDER_EXPORT",
                    StringComparison.Ordinal);

            bool explicitLodExport =
                upperPath.Contains(
                    "/LODS/",
                    StringComparison.Ordinal) ||
                upperPath.Contains(
                    "/GENERATED_LODS/",
                    StringComparison.Ordinal) ||
                upperFileName.Contains(
                    "_GENERATED_LODS",
                    StringComparison.Ordinal);

            if (explicitColliderExport)
            {
                return ModelRoot +
                    "/Generated/Colliders";
            }

            if (explicitLodExport)
            {
                return ModelRoot +
                    "/Generated/LODs";
            }

            if (upperPath.Contains(
                    "/ARCHITECTURE/",
                    StringComparison.Ordinal) ||
                upperFileName.Contains(
                    "_ARCHITECTURE_",
                    StringComparison.Ordinal))
            {
                return ModelRoot +
                    "/Architecture";
            }

            if (upperPath.Contains(
                    "/FURNITURE/",
                    StringComparison.Ordinal) ||
                upperFileName.Contains(
                    "_FURNITURE_",
                    StringComparison.Ordinal))
            {
                return ModelRoot +
                    "/Furniture";
            }

            if (upperPath.Contains(
                    "/PRODUCTS/",
                    StringComparison.Ordinal) ||
                upperPath.Contains(
                    "/PRODUCT/",
                    StringComparison.Ordinal) ||
                upperFileName.Contains(
                    "_PRODUCT_",
                    StringComparison.Ordinal))
            {
                return ModelRoot +
                    "/Products";
            }

            if (upperPath.Contains(
                    "/EXPANSIONS/",
                    StringComparison.Ordinal) ||
                upperPath.Contains(
                    "/EXPANSION/",
                    StringComparison.Ordinal) ||
                upperFileName.Contains(
                    "_EXPANSION_",
                    StringComparison.Ordinal))
            {
                return ModelRoot +
                    "/Expansions";
            }

            return ModelRoot +
                "/Misc";
        }

        private static void
            DeleteLegacyMisclassifiedCopy(
                string sourceFile,
                string correctDestination)
        {
            string previousWrongPath =
                ModelRoot +
                "/Generated/LODs/" +
                Path.GetFileName(
                    sourceFile);

            if (string.Equals(
                    previousWrongPath,
                    correctDestination,
                    StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            UnityEngine.Object previous =
                AssetDatabase.LoadMainAssetAtPath(
                    previousWrongPath);

            if (previous != null)
            {
                AssetDatabase.DeleteAsset(
                    previousWrongPath);

                Debug.Log(
                    "[Representative Assets] Removed " +
                    "legacy misclassified FBX: " +
                    previousWrongPath);
            }
        }

        private static void ConfigureModelImporter(
            string assetPath)
        {
            ModelImporter importer =
                AssetImporter.GetAtPath(
                    assetPath) as ModelImporter;

            if (importer == null)
            {
                throw new InvalidOperationException(
                    "ModelImporter was not available for " +
                    assetPath);
            }

            importer.globalScale = 1f;
            importer.useFileScale = true;
            importer.importBlendShapes = false;
            importer.importCameras = false;
            importer.importLights = false;
            importer.animationType =
                ModelImporterAnimationType.None;
            importer.importAnimation = false;
            importer.isReadable = false;
            importer.meshCompression =
                ModelImporterMeshCompression.Off;
            importer.addCollider = false;
            importer.generateSecondaryUV =
                assetPath.Contains(
                    "/Architecture/",
                    StringComparison.Ordinal) ||
                assetPath.Contains(
                    "/Furniture/",
                    StringComparison.Ordinal);
            importer.materialImportMode =
                ModelImporterMaterialImportMode
                    .ImportStandard;
            importer.importNormals =
                ModelImporterNormals.Import;
            importer.importTangents =
                ModelImporterTangents
                    .CalculateMikk;
            importer.preserveHierarchy = true;

            importer.SaveAndReimport();

            foreach (KeyValuePair<string, Material>
                     material in Materials)
            {
                importer.AddRemap(
                    new AssetImporter
                        .SourceAssetIdentifier(
                            typeof(Material),
                            material.Key),
                    material.Value);
            }

            importer.SaveAndReimport();
        }

        private static List<NodeRecord>
            BuildNodeIndex(
                IEnumerable<string> importedPaths,
                Scene previewScene,
                ICollection<GameObject> previewRoots)
        {
            List<NodeRecord> nodes =
                new List<NodeRecord>();

            foreach (string assetPath
                     in importedPaths)
            {
                GameObject model =
                    AssetDatabase.LoadAssetAtPath<
                        GameObject>(
                            assetPath);

                if (model == null)
                {
                    throw new InvalidOperationException(
                        "Imported model asset could not be loaded: " +
                        assetPath);
                }

                GameObject instance =
                    PrefabUtility.InstantiatePrefab(
                        model,
                        previewScene) as GameObject;

                if (instance == null)
                {
                    throw new InvalidOperationException(
                        "Imported model could not be instantiated: " +
                        assetPath);
                }

                previewRoots.Add(instance);

                bool generated =
                    assetPath.Contains(
                        "/Generated/",
                        StringComparison.Ordinal);

                foreach (Transform transform
                         in instance.GetComponentsInChildren<
                             Transform>(true))
                {
                    nodes.Add(
                        new NodeRecord(
                            transform,
                            assetPath,
                            generated));
                }
            }

            return nodes;
        }

        private static void
            ValidateRequiredSources(
                IEnumerable<AssetSpec> specs,
                IReadOnlyList<NodeRecord> nodes)
        {
            List<string> missing =
                new List<string>();

            foreach (AssetSpec spec in specs)
            {
                if (spec.Required &&
                    FindSourceNode(
                        nodes,
                        spec.SourceSuffix) ==
                    null)
                {
                    missing.Add(
                        spec.SourceSuffix);
                }
            }

            if (missing.Count > 0)
            {
                throw new InvalidOperationException(
                    "Required FBX model roots are missing:\n" +
                    string.Join(
                        "\n",
                        missing));
            }
        }

        private static BuiltRecord BuildPrefab(
            AssetSpec spec,
            NodeRecord source,
            IReadOnlyList<NodeRecord> nodes)
        {
            EnsureFolder(
                Path.GetDirectoryName(
                    spec.PrefabPath)
                    ?.Replace("\\", "/"));

            GameObject existing =
                AssetDatabase.LoadAssetAtPath<
                    GameObject>(
                        spec.PrefabPath);

            bool loadedContents =
                existing != null;

            GameObject root =
                loadedContents
                    ? PrefabUtility
                        .LoadPrefabContents(
                            spec.PrefabPath)
                    : new GameObject(
                        spec.PrefabName);

            try
            {
                root.name =
                    spec.PrefabName;

                ClearChildren(
                    root.transform);
                RemoveComponents<LODGroup>(root);
                RemoveComponents<Collider>(root);

                List<Renderer> lodZeroRenderers =
                    CloneRenderersForLod(
                        source,
                        nodes,
                        root.transform,
                        0);

                List<Renderer> lodOneRenderers =
                    CloneRenderersForLod(
                        source,
                        nodes,
                        root.transform,
                        1);

                List<Renderer> lodTwoRenderers =
                    CloneRenderersForLod(
                        source,
                        nodes,
                        root.transform,
                        2);

                BuildLodGroup(
                    root,
                    lodZeroRenderers,
                    lodOneRenderers,
                    lodTwoRenderers);

                BuildColliders(
                    source,
                    nodes,
                    root.transform);

                RepresentativePrefabInstance marker =
                    root.GetComponent<
                        RepresentativePrefabInstance>();

                if (marker == null)
                {
                    marker =
                        root.AddComponent<
                            RepresentativePrefabInstance>();
                }

                marker.Configure(
                    spec.Id,
                    spec.Family.ToString(),
                    spec.Conceptual);

                ConfigureFunctionalComponents(
                    root,
                    spec);

                ConfigureDoorParts(
                    root,
                    spec);

                ApplyStaticFlags(
                    root,
                    spec.Family ==
                        AssetFamily.Architecture ||
                    spec.Family ==
                        AssetFamily.Expansion);

                GameObject saved =
                    PrefabUtility.SaveAsPrefabAsset(
                        root,
                        spec.PrefabPath);

                if (saved == null)
                {
                    throw new InvalidOperationException(
                        "Prefab could not be saved: " +
                        spec.PrefabPath);
                }

                return new BuiltRecord(
                    spec,
                    saved);
            }
            finally
            {
                if (loadedContents)
                {
                    PrefabUtility
                        .UnloadPrefabContents(
                            root);
                }
                else
                {
                    UnityEngine.Object
                        .DestroyImmediate(
                            root);
                }
            }
        }

        private static List<Renderer>
            CloneRenderersForLod(
                NodeRecord source,
                IReadOnlyList<NodeRecord> nodes,
                Transform targetRoot,
                int level)
        {
            List<NodeRecord> candidates =
                FindInternalLodRenderers(
                    source,
                    nodes,
                    level);

            if (candidates.Count == 0 &&
                level > 0)
            {
                candidates =
                    FindExternalGeneratedLodRenderers(
                        source,
                        nodes,
                        level);
            }

            if (candidates.Count == 0)
            {
                return new List<Renderer>();
            }

            GameObject lodRoot =
                new GameObject(
                    "LOD" +
                    level);

            lodRoot.transform.SetParent(
                targetRoot,
                false);

            List<Renderer> renderers =
                new List<Renderer>();

            foreach (NodeRecord candidate
                     in candidates
                         .OrderBy(
                             node =>
                                 node.Transform.name,
                             StringComparer.Ordinal))
            {
                GameObject clone =
                    CloneRendererNode(
                        candidate.Transform,
                        source.Transform,
                        lodRoot.transform);

                if (clone == null)
                {
                    continue;
                }

                Renderer renderer =
                    clone.GetComponent<
                        Renderer>();

                if (renderer != null)
                {
                    renderers.Add(renderer);
                }
            }

            if (renderers.Count == 0)
            {
                UnityEngine.Object
                    .DestroyImmediate(
                        lodRoot);

                return renderers;
            }

            return renderers;
        }

        private static List<NodeRecord>
            FindInternalLodRenderers(
                NodeRecord source,
                IReadOnlyList<NodeRecord> nodes,
                int level)
        {
            HashSet<Transform> descendants =
                new HashSet<Transform>(
                    source.Transform
                        .GetComponentsInChildren<
                            Transform>(true));

            List<NodeRecord> renderers =
                nodes
                    .Where(
                        node =>
                            descendants.Contains(
                                node.Transform) &&
                            node.Transform
                                .GetComponent<
                                    Renderer>() !=
                            null &&
                            !IsColliderNode(
                                node.Transform) &&
                            ResolveLodLevel(
                                node.Transform,
                                source.Transform) ==
                            level)
                    .ToList();

            if (level == 0 &&
                renderers.Count == 0)
            {
                renderers =
                    nodes
                        .Where(
                            node =>
                                descendants.Contains(
                                    node.Transform) &&
                                node.Transform
                                    .GetComponent<
                                        Renderer>() !=
                                null &&
                                !IsColliderNode(
                                    node.Transform) &&
                                !HasExplicitLodLevel(
                                    node.Transform,
                                    source.Transform))
                        .ToList();
            }

            return renderers;
        }

        private static List<NodeRecord>
            FindExternalGeneratedLodRenderers(
                NodeRecord source,
                IReadOnlyList<NodeRecord> nodes,
                int level)
        {
            List<NodeRecord> result =
                new List<NodeRecord>();

            Renderer[] lodZeroRenderers =
                source.Transform
                    .GetComponentsInChildren<
                        Renderer>(true);

            foreach (Renderer renderer
                     in lodZeroRenderers)
            {
                if (IsColliderNode(
                        renderer.transform) ||
                    ResolveLodLevel(
                        renderer.transform,
                        source.Transform) != 0)
                {
                    continue;
                }

                string expected =
                    renderer.name +
                    "_LOD" +
                    level;

                NodeRecord match =
                    nodes.FirstOrDefault(
                        node =>
                            node.Generated &&
                            string.Equals(
                                node.Transform.name,
                                expected,
                                StringComparison.Ordinal));

                if (match != null &&
                    match.Transform.GetComponent<
                        Renderer>() !=
                    null)
                {
                    result.Add(match);
                }
            }

            return result;
        }

        private static int ResolveLodLevel(
            Transform transform,
            Transform stopAt)
        {
            Transform current =
                transform;

            while (current != null)
            {
                MatchCollection matches =
                    Regex.Matches(
                        current.name,
                        @"(?:^|_)LOD([012])(?=_|$)",
                        RegexOptions.CultureInvariant);

                if (matches.Count > 0)
                {
                    Match last =
                        matches[
                            matches.Count - 1];

                    return int.Parse(
                        last.Groups[1]
                            .Value);
                }

                if (current == stopAt)
                {
                    break;
                }

                current =
                    current.parent;
            }

            return 0;
        }

        private static bool HasExplicitLodLevel(
            Transform transform,
            Transform stopAt)
        {
            Transform current =
                transform;

            while (current != null)
            {
                if (Regex.IsMatch(
                        current.name,
                        @"(?:^|_)LOD[012](?=_|$)",
                        RegexOptions.CultureInvariant))
                {
                    return true;
                }

                if (current == stopAt)
                {
                    break;
                }

                current =
                    current.parent;
            }

            return false;
        }

        private static bool IsColliderNode(
            Transform transform)
        {
            Transform current =
                transform;

            while (current != null)
            {
                if (Regex.IsMatch(
                        current.name,
                        @"(?:^|_)COL(?=_|$)",
                        RegexOptions.CultureInvariant) ||
                    current.name.Contains(
                        "COLLIDER",
                        StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                current =
                    current.parent;
            }

            return false;
        }

        

        private static void BuildLodGroup(
            GameObject root,
            List<Renderer> lodZero,
            List<Renderer> lodOne,
            List<Renderer> lodTwo)
        {
            if (lodZero.Count == 0)
            {
                return;
            }

            List<LOD> lods =
                new List<LOD>
                {
                    new LOD(
                        0.60f,
                        lodZero.ToArray())
                };

            if (lodOne.Count > 0)
            {
                lods.Add(
                    new LOD(
                        0.30f,
                        lodOne.ToArray()));
            }

            if (lodTwo.Count > 0)
            {
                lods.Add(
                    new LOD(
                        0.10f,
                        lodTwo.ToArray()));
            }

            if (lods.Count == 1)
            {
                return;
            }

            LODGroup group =
                root.AddComponent<
                    LODGroup>();

            group.fadeMode =
                LODFadeMode.None;
            group.animateCrossFading =
                false;
            group.SetLODs(
                lods.ToArray());
            group.RecalculateBounds();
        }

        private static void BuildColliders(
            NodeRecord source,
            IReadOnlyList<NodeRecord> nodes,
            Transform targetRoot)
        {
            HashSet<Transform> descendants =
                new HashSet<Transform>(
                    source.Transform
                        .GetComponentsInChildren<
                            Transform>(true));

            List<NodeRecord> colliderNodes =
                nodes
                    .Where(
                        node =>
                            descendants.Contains(
                                node.Transform) &&
                            IsColliderNode(
                                node.Transform) &&
                            node.Transform
                                .GetComponent<
                                    MeshFilter>() !=
                            null)
                    .ToList();

            if (colliderNodes.Count == 0)
            {
                Renderer[] sourceRenderers =
                    source.Transform
                        .GetComponentsInChildren<
                            Renderer>(true);

                foreach (Renderer renderer
                         in sourceRenderers)
                {
                    string expected =
                        renderer.name +
                        "_COL";

                    NodeRecord match =
                        nodes.FirstOrDefault(
                            node =>
                                node.Generated &&
                                string.Equals(
                                    node.Transform.name,
                                    expected,
                                    StringComparison.Ordinal));

                    if (match != null)
                    {
                        colliderNodes.Add(
                            match);
                    }
                }
            }

            if (colliderNodes.Count == 0)
            {
                return;
            }

            GameObject colliderRoot =
                new GameObject(
                    "ColliderRoot");

            colliderRoot.transform.SetParent(
                targetRoot,
                false);

            int created = 0;

            foreach (NodeRecord colliderNode
                     in colliderNodes
                         .Distinct()
                         .OrderBy(
                             node =>
                                 node.Transform.name,
                             StringComparer.Ordinal))
            {
                MeshFilter filter =
                    colliderNode.Transform
                        .GetComponent<
                            MeshFilter>();

                if (filter == null ||
                    filter.sharedMesh == null)
                {
                    continue;
                }

                GameObject colliderObject =
                    new GameObject(
                        colliderNode.Transform.name +
                        "_Collider");

                colliderObject.transform.SetParent(
                    colliderRoot.transform,
                    false);

                Matrix4x4 relative =
                    source.Transform
                        .worldToLocalMatrix *
                    colliderNode.Transform
                        .localToWorldMatrix;

                ApplyMatrix(
                    colliderObject.transform,
                    relative);

                BoxCollider box =
                    colliderObject.AddComponent<
                        BoxCollider>();

                box.center =
                    filter.sharedMesh
                        .bounds.center;
                box.size =
                    filter.sharedMesh
                        .bounds.size;

                created++;
            }

            if (created == 0)
            {
                UnityEngine.Object
                    .DestroyImmediate(
                        colliderRoot);
            }
        }

        private static GameObject CloneRendererNode(
            Transform source,
            Transform assetRoot,
            Transform targetParent)
        {
            MeshFilter sourceFilter =
                source.GetComponent<
                    MeshFilter>();
            MeshRenderer sourceRenderer =
                source.GetComponent<
                    MeshRenderer>();

            if (sourceFilter == null ||
                sourceFilter.sharedMesh == null ||
                sourceRenderer == null)
            {
                return null;
            }

            GameObject clone =
                new GameObject(
                    source.name);

            clone.transform.SetParent(
                targetParent,
                false);

            Matrix4x4 relative =
                assetRoot.worldToLocalMatrix *
                source.localToWorldMatrix;

            ApplyMatrix(
                clone.transform,
                relative);

            MeshFilter targetFilter =
                clone.AddComponent<
                    MeshFilter>();

            targetFilter.sharedMesh =
                sourceFilter.sharedMesh;

            MeshRenderer targetRenderer =
                clone.AddComponent<
                    MeshRenderer>();

            targetRenderer.sharedMaterials =
                sourceRenderer.sharedMaterials;
            targetRenderer.shadowCastingMode =
                sourceRenderer.shadowCastingMode;
            targetRenderer.receiveShadows =
                sourceRenderer.receiveShadows;
            targetRenderer.lightProbeUsage =
                sourceRenderer.lightProbeUsage;
            targetRenderer.reflectionProbeUsage =
                sourceRenderer
                    .reflectionProbeUsage;

            return clone;
        }

        private static void ApplyMatrix(
            Transform target,
            Matrix4x4 matrix)
        {
            target.localPosition =
                matrix.GetColumn(3);
            target.localRotation =
                matrix.rotation;
            target.localScale =
                matrix.lossyScale;
        }

        private static void
            ConfigureFunctionalComponents(
                GameObject root,
                AssetSpec spec)
        {
            if (spec.Family ==
                AssetFamily.Furniture &&
                !string.IsNullOrWhiteSpace(
                    spec.DomainId))
            {
                Phase1FurniturePrefabAuthoring
                    authoring =
                        root.GetComponent<
                            Phase1FurniturePrefabAuthoring>();

                if (authoring == null)
                {
                    authoring =
                        root.AddComponent<
                            Phase1FurniturePrefabAuthoring>();
                }

                authoring.Configure(
                    spec.DomainId,
                    0.5f);
            }

            if (spec.Family ==
                AssetFamily.Product &&
                !string.IsNullOrWhiteSpace(
                    spec.DomainId))
            {
                Phase1ProductPrefabAuthoring
                    authoring =
                        root.GetComponent<
                            Phase1ProductPrefabAuthoring>();

                if (authoring == null)
                {
                    authoring =
                        root.AddComponent<
                            Phase1ProductPrefabAuthoring>();
                }

                authoring.Configure(
                    spec.DomainId);

                Phase1ProductVisualMarker
                    productMarker =
                        root.GetComponent<
                            Phase1ProductVisualMarker>();

                if (productMarker == null)
                {
                    productMarker =
                        root.AddComponent<
                            Phase1ProductVisualMarker>();
                }

                productMarker.Configure(
                    spec.DomainId);
            }
        }

        private static void ConfigureDoorParts(
            GameObject root,
            AssetSpec spec)
        {
            if (!string.Equals(
                    spec.Id,
                    "architecture.automatic-door",
                    StringComparison.Ordinal))
            {
                return;
            }

            Transform left =
                FindByNameContains(
                    root.transform,
                    "AutoDoor_Left");
            Transform right =
                FindByNameContains(
                    root.transform,
                    "AutoDoor_Right");

            if (left == null ||
                right == null)
            {
                throw new InvalidOperationException(
                    "Automatic door panel transforms were not found.");
            }

            RepresentativeAutomaticDoorParts
                parts =
                    root.GetComponent<
                        RepresentativeAutomaticDoorParts>();

            if (parts == null)
            {
                parts =
                    root.AddComponent<
                        RepresentativeAutomaticDoorParts>();
            }

            parts.Configure(
                left,
                right);
        }

        private static
            RepresentativePrefabCatalogAsset
            CreateOrUpdateCatalog(
                IEnumerable<BuiltRecord> built)
        {
            RepresentativePrefabCatalogAsset catalog =
                AssetDatabase.LoadAssetAtPath<
                    RepresentativePrefabCatalogAsset>(
                        CatalogPath);

            if (catalog == null)
            {
                catalog =
                    ScriptableObject.CreateInstance<
                        RepresentativePrefabCatalogAsset>();

                AssetDatabase.CreateAsset(
                    catalog,
                    CatalogPath);
            }

            List<BuiltRecord> records =
                built.ToList();

            catalog.Configure(
                Entries(
                    records,
                    AssetFamily.Architecture),
                Entries(
                    records,
                    AssetFamily.Furniture),
                Entries(
                    records,
                    AssetFamily.Product),
                Entries(
                    records,
                    AssetFamily.Expansion));

            EditorUtility.SetDirty(catalog);
            return catalog;
        }

        private static
            RepresentativePrefabCatalogAsset.Entry[]
            Entries(
                IEnumerable<BuiltRecord> records,
                AssetFamily family)
        {
            return records
                .Where(
                    record =>
                        record.Spec.Family ==
                        family)
                .OrderBy(
                    record =>
                        record.Spec.Id,
                    StringComparer.Ordinal)
                .Select(
                    record =>
                        new RepresentativePrefabCatalogAsset
                            .Entry
                        {
                            id =
                                record.Spec.Id,
                            prefab =
                                AssetDatabase
                                    .LoadAssetAtPath<
                                        GameObject>(
                                            record.Spec
                                                .PrefabPath)
                        })
                .ToArray();
        }

        private static void AddPreloadedAsset(
            RepresentativePrefabCatalogAsset catalog)
        {
            List<UnityEngine.Object> preloaded =
                PlayerSettings
                    .GetPreloadedAssets()
                    .Where(
                        asset =>
                            asset != null &&
                            !(asset is
                                RepresentativePrefabCatalogAsset))
                    .ToList();

            preloaded.Add(catalog);

            PlayerSettings.SetPreloadedAssets(
                preloaded.ToArray());
        }

        private static void PatchRuntimeSources()
        {
            PatchFactorySource();
            PatchStoreBuilderSource();
        }

        private static void PatchFactorySource()
        {
            string path =
                AbsolutePath(
                    FactorySourcePath);
            string source =
                File.ReadAllText(path);

            if (!source.Contains(
                    "RepresentativePrefabFactory" +
                    ".TryBuildFurniture",
                    StringComparison.Ordinal))
            {
                const string marker =
                    "            ClearGeneratedChildren(root.transform);";

                const string insertion =
                    "            if (RepresentativePrefabFactory\n" +
                    "                .TryBuildFurniture(\n" +
                    "                    root,\n" +
                    "                    definition.DefinitionId))\n" +
                    "            {\n" +
                    "                return;\n" +
                    "            }\n\n" +
                    marker;

                source =
                    ReplaceExactlyOnce(
                        source,
                        marker,
                        insertion,
                        "furniture prefab factory hook");
            }

            if (!source.Contains(
                    "RepresentativePrefabFactory" +
                    ".TryBuildProduct",
                    StringComparison.Ordinal))
            {
                const string marker =
                    "            PrimitiveType primitive =";

                const string insertion =
                    "            if (RepresentativePrefabFactory\n" +
                    "                .TryBuildProduct(\n" +
                    "                    parent,\n" +
                    "                    definition.ProductId,\n" +
                    "                    localPosition,\n" +
                    "                    out GameObject representative))\n" +
                    "            {\n" +
                    "                return representative;\n" +
                    "            }\n\n" +
                    marker;

                source =
                    ReplaceExactlyOnce(
                        source,
                        marker,
                        insertion,
                        "product prefab factory hook");
            }

            File.WriteAllText(
                path,
                source);
        }

        private static void PatchStoreBuilderSource()
        {
            string path =
                AbsolutePath(
                    StoreBuilderSourcePath);
            string source =
                File.ReadAllText(path);

            if (source.Contains(
                    "RepresentativeStoreVisualBuilder" +
                    ".TryApply",
                    StringComparison.Ordinal))
            {
                return;
            }

            const string marker =
                "            BuildLighting(\n" +
                "                center,\n" +
                "                width,\n" +
                "                depth);\n" +
                "            RegisterPlayer();";

            const string replacement =
                "            BuildLighting(\n" +
                "                center,\n" +
                "                width,\n" +
                "                depth);\n" +
                "            RepresentativeStoreVisualBuilder\n" +
                "                .TryApply(\n" +
                "                    _builtRoot,\n" +
                "                    _surface,\n" +
                "                    _shell,\n" +
                "                    EntranceAnchor,\n" +
                "                    CheckoutAnchor,\n" +
                "                    ReceivingAnchor,\n" +
                "                    BackroomAnchor,\n" +
                "                    Door);\n" +
                "            RegisterPlayer();";

            source =
                ReplaceExactlyOnce(
                    source,
                    marker,
                    replacement,
                    "representative store visual hook");

            File.WriteAllText(
                path,
                source);
        }

        private static string ReplaceExactlyOnce(
            string source,
            string oldValue,
            string newValue,
            string label)
        {
            int first =
                source.IndexOf(
                    oldValue,
                    StringComparison.Ordinal);

            if (first < 0)
            {
                throw new InvalidOperationException(
                    "Could not install " +
                    label +
                    ".");
            }

            int second =
                source.IndexOf(
                    oldValue,
                    first +
                    oldValue.Length,
                    StringComparison.Ordinal);

            if (second >= 0)
            {
                throw new InvalidOperationException(
                    "More than one source location matched " +
                    label +
                    ".");
            }

            return source.Substring(
                       0,
                       first) +
                   newValue +
                   source.Substring(
                       first +
                       oldValue.Length);
        }

        private static void CreateOrUpdateMaterials()
        {
            Materials.Clear();

            CreateMaterial(
                "CC_MAT_Graphite",
                "Graphite",
                new Color(
                    0.016f,
                    0.027f,
                    0.036f,
                    1f),
                0.35f,
                0.58f);
            CreateMaterial(
                "CC_MAT_DarkSurface",
                "DarkSurface",
                new Color(
                    0.045f,
                    0.070f,
                    0.090f,
                    1f),
                0.08f,
                0.45f);
            CreateMaterial(
                "CC_MAT_VRMGreen",
                "VRMGreen",
                new Color(
                    0.031f,
                    0.680f,
                    0.250f,
                    1f),
                0.05f,
                0.64f);
            CreateMaterial(
                "CC_MAT_VRMGreen_Emissive",
                "VRMGreenEmissive",
                new Color(
                    0.031f,
                    0.680f,
                    0.250f,
                    1f),
                0f,
                0.65f,
                true);
            CreateMaterial(
                "CC_MAT_Cyan",
                "Cyan",
                new Color(
                    0.085f,
                    0.575f,
                    0.800f,
                    1f),
                0.05f,
                0.64f);
            CreateMaterial(
                "CC_MAT_Cyan_Emissive",
                "CyanEmissive",
                new Color(
                    0.085f,
                    0.575f,
                    0.800f,
                    1f),
                0f,
                0.65f,
                true);
            CreateMaterial(
                "CC_MAT_WarmWood",
                "WarmWood",
                new Color(
                    0.320f,
                    0.160f,
                    0.070f,
                    1f),
                0f,
                0.52f);
            CreateMaterial(
                "CC_MAT_LightNeutral",
                "LightNeutral",
                new Color(
                    0.720f,
                    0.790f,
                    0.770f,
                    1f),
                0f,
                0.45f);
            CreateMaterial(
                "CC_MAT_Cardboard",
                "Cardboard",
                new Color(
                    0.420f,
                    0.235f,
                    0.095f,
                    1f),
                0f,
                0.18f);
            CreateMaterial(
                "CC_MAT_Amber",
                "Amber",
                new Color(
                    0.780f,
                    0.340f,
                    0.045f,
                    1f),
                0f,
                0.55f);
            CreateMaterial(
                "CC_MAT_Red",
                "Red",
                new Color(
                    0.700f,
                    0.060f,
                    0.055f,
                    1f),
                0f,
                0.55f);
            CreateMaterial(
                "CC_MAT_BlueGray",
                "BlueGray",
                new Color(
                    0.120f,
                    0.190f,
                    0.260f,
                    1f),
                0.08f,
                0.45f);
            CreateMaterial(
                "CC_MAT_Glass",
                "Glass",
                new Color(
                    0.150f,
                    0.280f,
                    0.300f,
                    0.25f),
                0f,
                0.88f,
                false,
                true);
        }

        private static void CreateMaterial(
            string blenderName,
            string assetName,
            Color color,
            float metallic,
            float smoothness,
            bool emission = false,
            bool transparent = false)
        {
            string path =
                "Assets/_Project/Art/Materials/" +
                "Representative/" +
                assetName +
                ".mat";

            Material material =
                AssetDatabase.LoadAssetAtPath<
                    Material>(
                        path);

            Shader shader =
                Shader.Find(
                    "Universal Render Pipeline/Lit") ??
                Shader.Find("Standard");

            if (material == null)
            {
                material =
                    new Material(shader);

                AssetDatabase.CreateAsset(
                    material,
                    path);
            }
            else
            {
                material.shader = shader;
            }

            if (material.HasProperty(
                    "_BaseColor"))
            {
                material.SetColor(
                    "_BaseColor",
                    color);
            }

            if (material.HasProperty(
                    "_Color"))
            {
                material.SetColor(
                    "_Color",
                    color);
            }

            if (material.HasProperty(
                    "_Metallic"))
            {
                material.SetFloat(
                    "_Metallic",
                    metallic);
            }

            if (material.HasProperty(
                    "_Smoothness"))
            {
                material.SetFloat(
                    "_Smoothness",
                    smoothness);
            }

            if (emission)
            {
                material.EnableKeyword(
                    "_EMISSION");

                if (material.HasProperty(
                        "_EmissionColor"))
                {
                    material.SetColor(
                        "_EmissionColor",
                        color * 2f);
                }
            }
            else
            {
                material.DisableKeyword(
                    "_EMISSION");
            }

            if (transparent)
            {
                material.SetFloat(
                    "_Surface",
                    1f);
                material.SetFloat(
                    "_ZWrite",
                    0f);
                material.renderQueue = 3000;
                material.EnableKeyword(
                    "_SURFACE_TYPE_TRANSPARENT");
            }
            else
            {
                material.SetFloat(
                    "_Surface",
                    0f);
                material.SetFloat(
                    "_ZWrite",
                    1f);
                material.renderQueue = -1;
                material.DisableKeyword(
                    "_SURFACE_TYPE_TRANSPARENT");
            }

            EditorUtility.SetDirty(
                material);
            Materials[blenderName] =
                material;
        }

        private static List<AssetSpec>
            CoreSpecs()
        {
            return new List<AssetSpec>
            {
                Architecture("Floor_100_LOD0", "architecture.floor.100", "Floor100"),
                Architecture("Floor_200_LOD0", "architecture.floor.200", "Floor200"),
                Architecture("FloorEdge_100_LOD0", "architecture.floor-edge.100", "FloorEdge100"),
                Architecture("FloorTransition_100_LOD0", "architecture.floor-transition.100", "FloorTransition100"),
                Architecture("Wall_100_LOD0", "architecture.wall.100", "Wall100"),
                Architecture("Wall_200_LOD0", "architecture.wall.200", "Wall200"),
                Architecture("Wall_400_LOD0", "architecture.wall.400", "Wall400"),
                Architecture("CornerInner_LOD0", "architecture.corner-inner", "CornerInner"),
                Architecture("CornerOuter_LOD0", "architecture.corner-outer", "CornerOuter"),
                Architecture("EndCap_LOD0", "architecture.end-cap", "EndCap"),
                Architecture("StorefrontFacade_LOD0", "architecture.storefront-facade", "StorefrontFacade"),
                Architecture("StoreGlass_Left_LOD0", "architecture.store-glass-left", "StoreGlassLeft"),
                Architecture("StoreGlass_Right_LOD0", "architecture.store-glass-right", "StoreGlassRight"),
                Architecture("AutoDoor_Root", "architecture.automatic-door", "AutomaticDoor"),
                Architecture("BackroomPartition_LOD0", "architecture.backroom-partition", "BackroomPartition"),
                Architecture("StoreSign_LOD0", "architecture.store-sign", "StoreSign"),
                Architecture("ZoneCheckout_LOD0", "architecture.zone-checkout", "ZoneCheckout"),
                Architecture("ZoneReceiving_LOD0", "architecture.zone-receiving", "ZoneReceiving"),
                Architecture("ZoneBackroom_LOD0", "architecture.zone-backroom", "ZoneBackroom"),
                Architecture("LightRail_100_LOD0", "architecture.light-rail.100", "LightRail100"),
                Architecture("LightRail_200_LOD0", "architecture.light-rail.200", "LightRail200"),
                Architecture("SpotLight_LOD0", "architecture.spot-light", "SpotLight"),
                Architecture("LinearPanel_LOD0", "architecture.linear-panel", "LinearPanel"),
                Architecture("EntranceThreshold_LOD0", "architecture.entrance-threshold", "EntranceThreshold"),

                Furniture("CheckoutCounter_LOD0", "checkout-counter", "CheckoutCounter"),
                Furniture("WallShelf_LOD0", "wall-shelf", "WallShelf"),
                Furniture("CentralShelf_LOD0", "central-shelf", "CentralShelf"),
                Furniture("LowDisplay_LOD0", "low-display", "LowDisplay"),
                Furniture("FeaturedDisplay_LOD0", "featured-display", "FeaturedDisplay"),
                Furniture("BackroomStorage_LOD0", "backroom-storage", "BackroomStorage"),
                Furniture("ReceivingCrate_LOD0", "receiving-crate", "ReceivingCrate"),
                Furniture("DecorativePlant_LOD0", "decoration-plant", "DecorationPlant"),

                Product("NeonDrift_LOD0", "game-neon-drift", "NeonDrift", "game-neon-drift"),
                Product("CloudRunnerCase_LOD0", "case-cloud-runner", "CloudRunnerCase", "case-cloud-runner"),
                Product("VertexOneConsole_LOD0", "console-vertex-one", "VertexOneConsole", "console-vertex-one"),
                Product("OrbitPad_LOD0", "controller-orbit-pad", "OrbitPadController", "controller-orbit-pad"),
                Product("SignalProHeadset_LOD0", "headset-signal-pro", "SignalProHeadset", "headset-signal-pro"),
                Product("MemoryCore_LOD0", "accessory-memory-core", "MemoryCoreAccessory", "accessory-memory-core"),

                Packaging("VertexOne_Box_LOD0", "product-packaging.vertex-one-box", "VertexOneBox"),
                Packaging("OrbitPad_Box_LOD0", "product-packaging.orbit-pad-box", "OrbitPadBox"),
                Packaging("SignalPro_Box_LOD0", "product-packaging.signal-pro-box", "SignalProBox"),
                Packaging("MemoryCore_Box_LOD0", "product-packaging.memory-core-box", "MemoryCoreBox")
            };
        }

        private static IEnumerable<AssetSpec>
            DiscoverExpansionSpecs(
                IReadOnlyList<NodeRecord> nodes)
        {
            HashSet<string> names =
                new HashSet<string>(
                    StringComparer.Ordinal);

            foreach (NodeRecord node in nodes)
            {
                string name =
                    node.Transform.name;

                if (node.Generated ||
                    !name.Contains(
                        "_Expansion_",
                        StringComparison.Ordinal) ||
                    !name.EndsWith(
                        "_LOD0",
                        StringComparison.Ordinal))
                {
                    continue;
                }

                int marker =
                    name.IndexOf(
                        "_Expansion_",
                        StringComparison.Ordinal);

                string raw =
                    name.Substring(
                        marker +
                        "_Expansion_".Length);

                raw =
                    raw.Substring(
                        0,
                        raw.Length -
                        "_LOD0".Length);

                if (!names.Add(raw))
                {
                    continue;
                }

                string[] parts =
                    raw.Split(
                        new[] { '_' },
                        2);

                string category =
                    parts[0];
                string item =
                    parts.Length > 1
                        ? parts[1]
                        : parts[0];

                yield return new AssetSpec(
                    "_Expansion_" +
                    raw +
                    "_LOD0",
                    AssetFamily.Expansion,
                    "expansion." +
                    Kebab(category) +
                    "." +
                    Kebab(item),
                    item.Replace(
                        "_",
                        string.Empty),
                    PrefabRoot +
                    "/Expansions/" +
                    category +
                    "/" +
                    item.Replace(
                        "_",
                        string.Empty) +
                    ".prefab",
                    string.Empty,
                    false,
                    true);
            }
        }

        private static AssetSpec Architecture(
            string suffix,
            string id,
            string prefabName)
        {
            return new AssetSpec(
                "_Architecture_" +
                suffix,
                AssetFamily.Architecture,
                id,
                prefabName,
                PrefabRoot +
                "/Architecture/" +
                prefabName +
                ".prefab",
                string.Empty,
                true,
                false);
        }

        private static AssetSpec Furniture(
            string suffix,
            string domainId,
            string prefabName)
        {
            return new AssetSpec(
                "_Furniture_" +
                suffix,
                AssetFamily.Furniture,
                domainId,
                prefabName,
                PrefabRoot +
                "/Furniture/" +
                prefabName +
                ".prefab",
                domainId,
                true,
                false);
        }

        private static AssetSpec Product(
            string suffix,
            string id,
            string prefabName,
            string domainId)
        {
            return new AssetSpec(
                "_Product_" +
                suffix,
                AssetFamily.Product,
                id,
                prefabName,
                PrefabRoot +
                "/Products/" +
                prefabName +
                ".prefab",
                domainId,
                true,
                false);
        }

        private static AssetSpec Packaging(
            string suffix,
            string id,
            string prefabName)
        {
            return new AssetSpec(
                "_Product_" +
                suffix,
                AssetFamily.Product,
                id,
                prefabName,
                PrefabRoot +
                "/Products/Packaging/" +
                prefabName +
                ".prefab",
                string.Empty,
                true,
                false);
        }

        private static NodeRecord FindSourceNode(
            IReadOnlyList<NodeRecord> nodes,
            string suffix)
        {
            List<NodeRecord> matches =
                nodes
                    .Where(
                        node =>
                            !node.Generated &&
                            node.Transform.name
                                .EndsWith(
                                    suffix,
                                    StringComparison.Ordinal))
                    .ToList();

            if (matches.Count == 0)
            {
                matches =
                    nodes
                        .Where(
                            node =>
                                !node.Generated &&
                                node.Transform.parent ==
                                null &&
                                Path
                                    .GetFileNameWithoutExtension(
                                        node.AssetPath)
                                    .EndsWith(
                                        suffix,
                                        StringComparison.Ordinal))
                        .ToList();
            }

            if (matches.Count > 1)
            {
                matches =
                    matches
                        .Where(
                            node =>
                                node.Transform.parent ==
                                null ||
                                !node.Transform.parent.name
                                    .Contains(
                                        "_LOD",
                                        StringComparison.Ordinal))
                        .ToList();
            }

            return matches.Count == 0
                ? null
                : matches[0];
        }

        private static NodeRecord FindNode(
            IEnumerable<NodeRecord> nodes,
            Transform transform)
        {
            return nodes.FirstOrDefault(
                node =>
                    node.Transform ==
                    transform);
        }

        private static Transform FindByNameContains(
            Transform root,
            string token)
        {
            foreach (Transform transform
                     in root.GetComponentsInChildren<
                         Transform>(true))
            {
                if (transform.name.Contains(
                        token,
                        StringComparison.Ordinal))
                {
                    return transform;
                }
            }

            return null;
        }

        private static void ApplyStaticFlags(
            GameObject root,
            bool isStatic)
        {
            if (!isStatic)
            {
                return;
            }

            StaticEditorFlags flags =
                StaticEditorFlags.BatchingStatic |
                StaticEditorFlags
                    .ContributeGI;

            foreach (Transform transform
                     in root.GetComponentsInChildren<
                         Transform>(true))
            {
                GameObjectUtility
                    .SetStaticEditorFlags(
                        transform.gameObject,
                        flags);
            }
        }

        private static void RemoveComponents<T>(
            GameObject root)
            where T : Component
        {
            T[] components =
                root.GetComponents<T>();

            foreach (T component
                     in components)
            {
                UnityEngine.Object
                    .DestroyImmediate(
                        component);
            }
        }

        private static void ClearChildren(
            Transform root)
        {
            for (int index =
                     root.childCount - 1;
                 index >= 0;
                 index--)
            {
                UnityEngine.Object
                    .DestroyImmediate(
                        root.GetChild(index)
                            .gameObject);
            }
        }

        private static void EnsureFolder(
            string folderPath)
        {
            if (string.IsNullOrWhiteSpace(
                    folderPath) ||
                AssetDatabase.IsValidFolder(
                    folderPath))
            {
                return;
            }

            string normalized =
                folderPath.Replace(
                    "\\",
                    "/");

            string[] segments =
                normalized.Split('/');

            string current =
                segments[0];

            for (int index = 1;
                 index < segments.Length;
                 index++)
            {
                string next =
                    current +
                    "/" +
                    segments[index];

                if (!AssetDatabase.IsValidFolder(
                        next))
                {
                    AssetDatabase.CreateFolder(
                        current,
                        segments[index]);
                }

                current = next;
            }
        }

        private static void WriteReport(
            string sourceRoot,
            IReadOnlyCollection<string> imported,
            IReadOnlyCollection<BuiltRecord> built,
            RepresentativePrefabCatalogAsset catalog)
        {
            StringBuilder json =
                new StringBuilder();

            json.AppendLine("{");
            json.AppendLine(
                "  \"sourceRoot\": \"" +
                EscapeJson(sourceRoot) +
                "\",");
            json.AppendLine(
                "  \"importedFbxCount\": " +
                imported.Count +
                ",");
            json.AppendLine(
                "  \"prefabCount\": " +
                built.Count +
                ",");
            json.AppendLine(
                "  \"architectureCount\": " +
                catalog.Architecture.Length +
                ",");
            json.AppendLine(
                "  \"furnitureCount\": " +
                catalog.Furniture.Length +
                ",");
            json.AppendLine(
                "  \"productCount\": " +
                catalog.Products.Length +
                ",");
            json.AppendLine(
                "  \"expansionCount\": " +
                catalog.Expansions.Length +
                ",");
            json.AppendLine(
                "  \"runtimeFallbackRetained\": true,");
            json.AppendLine(
                "  \"status\": \"PENDING_UNITY_TESTS\"");
            json.AppendLine("}");

            File.WriteAllText(
                AbsolutePath(
                    ReportPath),
                json.ToString());
        }

        private static string EscapeJson(
            string value)
        {
            return value
                .Replace(
                    "\\",
                    "\\\\")
                .Replace(
                    "\"",
                    "\\\"");
        }

        private static string Kebab(
            string value)
        {
            string separated =
                Regex.Replace(
                    value,
                    "([a-z0-9])([A-Z])",
                    "$1-$2");

            return separated
                .Replace(
                    "_",
                    "-")
                .ToLowerInvariant();
        }

        private static string ProjectRoot()
        {
            string root =
                Directory.GetParent(
                    UnityEngine.Application
                        .dataPath)
                    ?.FullName;

            if (string.IsNullOrWhiteSpace(
                    root))
            {
                throw new InvalidOperationException(
                    "Project root could not be resolved.");
            }

            return root;
        }

        private static string AbsolutePath(
            string assetPath)
        {
            return Path.Combine(
                ProjectRoot(),
                assetPath.Replace(
                    '/',
                    Path.DirectorySeparatorChar));
        }

        private static void ValidateCatalogEntry(
            GameObject prefab,
            string id,
            ICollection<string> errors)
        {
            if (prefab == null)
            {
                errors.Add(
                    "Catalog entry is missing: " +
                    id);
                return;
            }

            RepresentativePrefabInstance marker =
                prefab.GetComponent<
                    RepresentativePrefabInstance>();

            if (marker == null)
            {
                errors.Add(
                    "Representative marker is missing: " +
                    id);
            }
        }

        private static void ValidateSourcePatch(
            string assetPath,
            string token,
            ICollection<string> errors)
        {
            string path =
                AbsolutePath(assetPath);

            if (!File.Exists(path) ||
                !File.ReadAllText(path)
                    .Contains(
                        token,
                        StringComparison.Ordinal))
            {
                errors.Add(
                    "Runtime source patch is missing: " +
                    token);
            }
        }

        private sealed class NodeRecord
        {
            public NodeRecord(
                Transform transform,
                string assetPath,
                bool generated)
            {
                Transform = transform;
                AssetPath = assetPath;
                Generated = generated;
            }

            public Transform Transform {
                get;
            }

            public string AssetPath {
                get;
            }

            public bool Generated {
                get;
            }
        }

        private sealed class AssetSpec
        {
            public AssetSpec(
                string sourceSuffix,
                AssetFamily family,
                string id,
                string prefabName,
                string prefabPath,
                string domainId,
                bool required,
                bool conceptual)
            {
                SourceSuffix =
                    sourceSuffix;
                Family = family;
                Id = id;
                PrefabName =
                    prefabName;
                PrefabPath =
                    prefabPath;
                DomainId =
                    domainId;
                Required = required;
                Conceptual =
                    conceptual;
            }

            public string SourceSuffix {
                get;
            }

            public AssetFamily Family {
                get;
            }

            public string Id {
                get;
            }

            public string PrefabName {
                get;
            }

            public string PrefabPath {
                get;
            }

            public string DomainId {
                get;
            }

            public bool Required {
                get;
            }

            public bool Conceptual {
                get;
            }
        }

        private sealed class BuiltRecord
        {
            public BuiltRecord(
                AssetSpec spec,
                GameObject prefab)
            {
                Spec = spec;
                Prefab = prefab;
            }

            public AssetSpec Spec {
                get;
            }

            public GameObject Prefab {
                get;
            }
        }

        private enum AssetFamily
        {
            Architecture,
            Furniture,
            Product,
            Expansion
        }
    }
}
