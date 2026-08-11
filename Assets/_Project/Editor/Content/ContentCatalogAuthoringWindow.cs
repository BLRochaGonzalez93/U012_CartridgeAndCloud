using UnityEditor;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Products;
using VRMGames.CartridgeAndCloud.Domain.Store;

namespace VRMGames.CartridgeAndCloud.Editor.Content
{
    public sealed class ContentCatalogAuthoringWindow : EditorWindow
    {
        private enum AuthoringMode
        {
            Product = 0,
            Fixture = 1
        }

        private AuthoringMode _mode;
        private Vector2 _scroll;
        private ContentCatalogAuthoringService.CatalogContext _context;
        private string _status = "Catalogs have not been loaded.";
        private MessageType _statusType = MessageType.Info;

        private readonly ContentCatalogAuthoringService.ProductDraft _product =
            new ContentCatalogAuthoringService.ProductDraft();

        private readonly ContentCatalogAuthoringService.FixtureDraft _fixture =
            new ContentCatalogAuthoringService.FixtureDraft();

        [MenuItem("Cartridge & Cloud/Content/Content Catalog Authoring")]
        public static void Open()
        {
            ContentCatalogAuthoringWindow window =
                GetWindow<ContentCatalogAuthoringWindow>();
            window.titleContent = new GUIContent("C&C Content");
            window.minSize = new Vector2(520f, 560f);
            window.Show();
        }

        private void OnEnable()
        {
            RefreshContext();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(8f);
            EditorGUILayout.LabelField(
                "Content Catalog Authoring",
                EditorStyles.boldLabel);
            EditorGUILayout.LabelField(
                "Adds new content definitions and their visual prefab mapping " +
                "in one explicit operation.",
                EditorStyles.wordWrappedLabel);

            EditorGUILayout.Space(6f);
            DrawCatalogStatus();

            using (new EditorGUI.DisabledScope(_context == null))
            {
                _mode = (AuthoringMode)GUILayout.Toolbar(
                    (int)_mode,
                    new[] { "Product", "Fixture" });

                EditorGUILayout.Space(6f);
                _scroll = EditorGUILayout.BeginScrollView(_scroll);

                if (_mode == AuthoringMode.Product)
                {
                    DrawProduct();
                }
                else
                {
                    DrawFixture();
                }

                EditorGUILayout.EndScrollView();
            }
        }

        private void DrawCatalogStatus()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.HelpBox(_status, _statusType);
                if (GUILayout.Button("Refresh", GUILayout.Width(90f), GUILayout.Height(38f)))
                {
                    RefreshContext();
                }
            }
        }

        private void DrawProduct()
        {
            EditorGUILayout.LabelField("Definition", EditorStyles.boldLabel);
            _product.ProductId = EditorGUILayout.TextField("Product ID", _product.ProductId);
            _product.DisplayName = EditorGUILayout.TextField("Display Name", _product.DisplayName);
            _product.Kind = (RetailProductKind)EditorGUILayout.EnumPopup("Kind", _product.Kind);

            EditorGUILayout.Space(5f);
            EditorGUILayout.LabelField("Economy", EditorStyles.boldLabel);
            _product.WholesalePriceCents = EditorGUILayout.LongField(
                "Wholesale (cents)",
                _product.WholesalePriceCents);
            _product.SalePriceCents = EditorGUILayout.LongField(
                "Sale (cents)",
                _product.SalePriceCents);
            _product.UnitsPerCase = EditorGUILayout.IntField(
                "Units Per Case",
                _product.UnitsPerCase);

            EditorGUILayout.Space(5f);
            EditorGUILayout.LabelField("Presentation metadata", EditorStyles.boldLabel);
            _product.MaterialVariantId = EditorGUILayout.TextField(
                "Material Variant ID",
                _product.MaterialVariantId);
            _product.LabelId = EditorGUILayout.TextField("Label ID", _product.LabelId);
            _product.IconResourcePath = EditorGUILayout.TextField(
                "Icon Resource Path",
                _product.IconResourcePath);
            _product.CoverResourcePath = EditorGUILayout.TextField(
                "Cover Resource Path",
                _product.CoverResourcePath);
            _product.PrefabResourcePath = EditorGUILayout.TextField(
                "Prefab Resource Path",
                _product.PrefabResourcePath);
            _product.Prefab = (GameObject)EditorGUILayout.ObjectField(
                "Representative Prefab",
                _product.Prefab,
                typeof(GameObject),
                false);

            EditorGUILayout.Space(12f);
            EditorGUILayout.HelpBox(
                "Existing IDs are never edited or renamed by this tool. " +
                "The operation is rejected before mutation if the ID or " +
                "visual mapping already exists.",
                MessageType.Info);

            if (GUILayout.Button("Add Product", GUILayout.Height(34f)))
            {
                if (ContentCatalogAuthoringService.TryAddProduct(
                        _context,
                        _product,
                        out string message))
                {
                    SetStatus(message, MessageType.Info);
                    ResetProductDraft();
                    RefreshContext(keepStatus: true);
                }
                else
                {
                    SetStatus(message, MessageType.Error);
                }
            }
        }

        private void DrawFixture()
        {
            EditorGUILayout.LabelField("Definition", EditorStyles.boldLabel);
            _fixture.DefinitionId = EditorGUILayout.TextField(
                "Definition ID",
                _fixture.DefinitionId);
            _fixture.DisplayName = EditorGUILayout.TextField(
                "Display Name",
                _fixture.DisplayName);
            _fixture.Kind = (StoreFixtureKind)EditorGUILayout.EnumPopup(
                "Kind",
                _fixture.Kind);

            EditorGUILayout.Space(5f);
            EditorGUILayout.LabelField("Physical footprint", EditorStyles.boldLabel);
            _fixture.WidthCells = EditorGUILayout.IntField("Width Cells", _fixture.WidthCells);
            _fixture.DepthCells = EditorGUILayout.IntField("Depth Cells", _fixture.DepthCells);
            _fixture.HeightMeters = EditorGUILayout.FloatField(
                "Height Meters",
                _fixture.HeightMeters);
            _fixture.Capacity = EditorGUILayout.IntField("Capacity", _fixture.Capacity);

            EditorGUILayout.Space(5f);
            EditorGUILayout.LabelField("Gameplay configuration", EditorStyles.boldLabel);
            _fixture.UnitCostCents = EditorGUILayout.LongField(
                "Unit Cost (cents)",
                _fixture.UnitCostCents);
            _fixture.IsInteractive = EditorGUILayout.Toggle(
                "Interactive",
                _fixture.IsInteractive);
            _fixture.IsPurchasable = EditorGUILayout.Toggle(
                "Purchasable",
                _fixture.IsPurchasable);
            _fixture.SupportsProducts = EditorGUILayout.Toggle(
                "Supports Products",
                _fixture.SupportsProducts);

            EditorGUILayout.Space(5f);
            EditorGUILayout.LabelField("Presentation metadata", EditorStyles.boldLabel);
            _fixture.MaterialVariantId = EditorGUILayout.TextField(
                "Material Variant ID",
                _fixture.MaterialVariantId);
            _fixture.PrefabResourcePath = EditorGUILayout.TextField(
                "Prefab Resource Path",
                _fixture.PrefabResourcePath);
            _fixture.Prefab = (GameObject)EditorGUILayout.ObjectField(
                "Representative Prefab",
                _fixture.Prefab,
                typeof(GameObject),
                false);

            EditorGUILayout.Space(12f);
            EditorGUILayout.HelpBox(
                "The selected prefab must already carry " +
                "StoreFixturePrefabAuthoring with the same definition ID. " +
                "The tool does not rewrite prefab identity.",
                MessageType.Info);

            if (GUILayout.Button("Add Fixture", GUILayout.Height(34f)))
            {
                if (ContentCatalogAuthoringService.TryAddFixture(
                        _context,
                        _fixture,
                        out string message))
                {
                    SetStatus(message, MessageType.Info);
                    ResetFixtureDraft();
                    RefreshContext(keepStatus: true);
                }
                else
                {
                    SetStatus(message, MessageType.Error);
                }
            }
        }

        private void RefreshContext(bool keepStatus = false)
        {
            if (ContentCatalogAuthoringService.TryLoadContext(
                    out _context,
                    out string message))
            {
                if (!keepStatus)
                {
                    SetStatus(message, MessageType.Info);
                }
            }
            else
            {
                SetStatus(message, MessageType.Error);
            }

            Repaint();
        }

        private void ResetProductDraft()
        {
            _product.ProductId = string.Empty;
            _product.DisplayName = string.Empty;
            _product.Kind = RetailProductKind.PhysicalGame;
            _product.WholesalePriceCents = 100;
            _product.SalePriceCents = 200;
            _product.UnitsPerCase = 1;
            _product.MaterialVariantId = string.Empty;
            _product.LabelId = string.Empty;
            _product.IconResourcePath = string.Empty;
            _product.CoverResourcePath = string.Empty;
            _product.PrefabResourcePath = string.Empty;
            _product.Prefab = null;
        }

        private void ResetFixtureDraft()
        {
            _fixture.DefinitionId = string.Empty;
            _fixture.DisplayName = string.Empty;
            _fixture.Kind = StoreFixtureKind.CentralShelf;
            _fixture.WidthCells = 1;
            _fixture.DepthCells = 1;
            _fixture.HeightMeters = 1f;
            _fixture.Capacity = 0;
            _fixture.UnitCostCents = 100;
            _fixture.IsInteractive = true;
            _fixture.IsPurchasable = true;
            _fixture.SupportsProducts = false;
            _fixture.MaterialVariantId = string.Empty;
            _fixture.PrefabResourcePath = string.Empty;
            _fixture.Prefab = null;
        }

        private void SetStatus(string message, MessageType type)
        {
            _status = message ?? string.Empty;
            _statusType = type;
        }
    }
}
