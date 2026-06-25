---
title: "Cartridge & Cloud - Modelo de Datos v0.4"
subtitle: "Baseline documental v0.5 - estado tras Sprint 5"
author: "VRM Games / Blas Luis Rocha González"
date: "25/06/2026"
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
  \fancyhead[L]{\small Cartridge \& Cloud - Baseline v0.5}
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
**Versión de aplicación:** `0.0.6`  
**Versión del documento:** v0.4  
**Estado:** Current / Approved  
**Baseline técnica:** `22d62967aaf9895db7bce75afd2ffa11f7858e0c`  
**Baseline documental:** `b650aa5968ecfc9ebe63fc308b0dc8ea2cafae57`


# 1. Implementado

## Sesión y guardado mínimo

- GameSession.
- Slot/snapshot mínimo versionado.
- Identificadores de sesión/slot según la base de Sprint 2.

## Grid y placement

- `GridCoordinate(X,Z)`.
- `GridSize(width,depth)`.
- `GridRotation`.
- `GridFootprint`.
- `PlacementInstanceId`.
- `PlacedObjectRecord`.
- `PlacementOccupancyRegistry`.

## Acceso

- `AccessAnchorId`.
- `AccessAnchor`.
- `StoreAccessLayout`.
- `AccessValidationResult`.

# 2. Modelo objetivo de Sprint 6

```text
ProductDefinition
  id
  displayNameKey
  categoryId
  unitCost
  suggestedPrice
  physicalSize
  tags

InventoryContainer
  id
  containerType
  capacityPolicy
  stacks[]

InventoryStack
  productId
  quantity

InventoryTransfer
  sourceId
  destinationId
  productId
  quantity
```

# 3. Invariantes futuras

- cantidad nunca negativa;
- producto inexistente no puede transferirse;
- una transferencia es atómica;
- no se excede capacidad;
- la suma de unidades se conserva entre origen y destino;
- los IDs son persistentes y no dependen del nombre visual;
- las definiciones son inmutables durante la sesión.

# 4. Modelos posteriores

## Pedidos

Supplier, SupplierCatalog, PurchaseOrder, PurchaseOrderLine, Delivery, ShipmentBox.

## Displays

DisplayDefinition, DisplayInstance, ProductAssignment, DisplayCapacity, RestockTask.

## Clientes

CustomerProfile, CustomerState, ShoppingIntent, Reservation, Cart, QueueTicket.

## Economía

LedgerEntry, SaleTransaction, Expense, TaxRule, DailyReport, WeeklyReport.

## Save completo

SaveRootV1 con sesión, Store, placement, inventarios, pedidos, clientes relevantes, economía y reloj.

# 5. Versionado

Cada snapshot persistente debe incluir `schemaVersion`. Las migraciones son explícitas, comprobables y no destructivas. La baseline actual no define aún SaveRootV1 completo.
