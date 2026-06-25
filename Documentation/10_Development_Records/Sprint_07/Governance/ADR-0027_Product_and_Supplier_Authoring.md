# ADR-0027 — Product and Supplier Authoring with ScriptableObjects

**Status:** Accepted for Sprint 7

## Context

Sprint 6 introduced pure C# product definitions and inventory behavior. Sprint
7 requires concrete products and supplier catalogs that a designer can inspect
and edit in Unity without coupling the domain model to `UnityEngine`.

## Decision

Add authoring assets in Infrastructure:

- `ProductDefinitionAsset`;
- `ProductCatalogAsset`;
- `SupplierDefinitionAsset`;
- `SupplierCatalogEntryAsset`;
- `SupplierCatalogAsset`.

Each asset validates and converts its serialized data into immutable domain
objects. Runtime inventory remains expressed through domain IDs and
quantities.

## Consequences

- Product and supplier data are editable from the Inspector.
- Domain and Application assemblies remain Unity-independent.
- Icons and prefabs can be linked later without becoming inventory authority.
- Invalid or duplicate authoring data fails during catalog construction.
- The package can supply technical content before representative art exists.
