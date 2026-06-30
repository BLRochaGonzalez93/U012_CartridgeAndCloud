---
title: "Cartridge & Cloud - Modelo de Datos v0.5"
subtitle: "Baseline documental v0.6 - Sprint 16 en curso"
author: "VRM Games / Blas Luis Rocha González"
date: "01/07/2026"
lang: es-ES
papersize: a4
fontsize: 10pt
geometry: margin=18mm
toc: true
toc-depth: 3
colorlinks: true
linkcolor: VRMGreen
urlcolor: VRMGreen
header-includes:
- |
  ```{=latex}
  \usepackage{xcolor}
  \definecolor{VRMGreen}{HTML}{008F46}
  \definecolor{VRMDark}{HTML}{101716}
  \definecolor{VRMGray}{HTML}{5B6660}
  \usepackage{sectsty}
  \sectionfont{\color{VRMGreen}}
  \subsectionfont{\color{VRMDark}}
  \subsubsectionfont{\color{VRMGray}}
  \usepackage{fancyhdr}
  \pagestyle{fancy}
  \fancyhf{}
  \fancyhead[L]{\small Cartridge \& Cloud - Baseline v0.6}
  \fancyhead[R]{\small VRM Games}
  \fancyfoot[C]{\thepage}
  \setlength{\headheight}{14pt}
  \usepackage{enumitem}
  \setlist{nosep}
  \usepackage{longtable}
  \usepackage{booktabs}
  \usepackage{array}
  \renewcommand{\arraystretch}{1.15}
  ```
---

**Proyecto:** Cartridge & Cloud  
**Plataforma:** PC / Steam  
**Motor:** Unity 6.3 LTS `6000.3.18f1` / URP `17.3.0`  
**Versión de aplicación:** `0.0.17`  
**Baseline documental:** `v0.6`  
**Último commit técnico validado:** `091090c43855b0b26b09abe9335d18b978ac7eab`  
**Último commit observado en main:** `d54316c771aab2143993e99b9fd58f2f88016568`  
**Estado global:** Sprints 0-15 CLOSED / PASS; Sprint 16 IN PROGRESS; Sprint 17 PENDING

# 1. Identidad y sesión

GameSession, slot, snapshot versionado, IntegratedGameStateSnapshot schema v2, backup y recovery.

# 2. Placement

GridCoordinate, GridSize, GridRotation, GridFootprint, PlacementInstanceId, PlacedObjectRecord, occupancy registry, access anchors y layout.

# 3. Productos e inventarios

ProductDefinitionId, ProductDefinition, InventoryContainerId, InventoryContainer, quantity, stock stacks y transferencias atómicas.

# 4. Pedidos

Supplier, SupplierCatalogEntry, PurchaseOrder, PurchaseOrderLine, Delivery y ShipmentBox.

# 5. Displays

DisplayDefinitionId, DisplayInstanceId, assignment de producto, capacidad, stock visible y RestockTask.

# 6. Clientes y shopping

CustomerProfileId, CustomerInstanceId, CustomerState, ShoppingIntentId, ReservationId, Cart, preferencias, paciencia y navegación.

# 7. Checkout

CheckoutQueueEntryId, CheckoutStationId, CheckoutTransactionId, estados FIFO y preflight de transacción.

# 8. Día y economía

StoreDayId, StoreDayPolicy, estados de día, tiempo entero, CurrencyCode, Money, catálogo de precios, quotes y resultados diarios.

# 9. Presentación representativa

RepresentativePrefabCatalogAsset enlaza IDs funcionales con prefabs visuales. Phase1RuntimeAssetRegistryAsset mantiene la referencia determinista. La escena fija usa StoreInitialSceneContext y no convierte GameObjects en estado de dominio.

# 10. Invariantes

- cantidades no negativas;
- conservación de unidades;
- transferencias atómicas;
- stock reservado no excede on-hand;
- checkout no muta si falla preflight;
- Money no usa float;
- IDs persistentes no dependen del nombre visual;
- snapshot declara schemaVersion;
- arquitectura fija no se guarda como mobiliario dinámico.
