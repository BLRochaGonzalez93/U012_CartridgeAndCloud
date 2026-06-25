using System;
using NUnit.Framework;
using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Customers;
using VRMGames.CartridgeAndCloud.Infrastructure.Customers;

namespace VRMGames.CartridgeAndCloud.Tests.EditMode.Customers
{
    public sealed class CustomerAuthoringAssetTests
    {
        [Test] public void ProfileAsset_BuildsConfiguredProfile() { CustomerProfileAsset a = CreateProfile("p"); try { CustomerProfile p = a.BuildProfile(); Assert.That(p.Id.Value, Is.EqualTo("p")); Assert.That(p.PatienceSeconds, Is.EqualTo(30)); } finally { UnityEngine.Object.DestroyImmediate(a); } }
        [Test] public void ProfileAsset_InvalidWeight_ThrowsOnBuild() { CustomerProfileAsset a = ScriptableObject.CreateInstance<CustomerProfileAsset>(); try { a.Configure("p", "name", Array.Empty<string>(), 0, 30, 1, 1f); Assert.Throws<ArgumentOutOfRangeException>(() => a.BuildProfile()); } finally { UnityEngine.Object.DestroyImmediate(a); } }
        [Test] public void Catalog_BuildsRegistry() { CustomerProfileAsset a = CreateProfile("p"); CustomerProfileCatalogAsset c = ScriptableObject.CreateInstance<CustomerProfileCatalogAsset>(); try { c.Configure(new[] { a }); Assert.That(c.BuildRegistry().Count, Is.EqualTo(1)); } finally { UnityEngine.Object.DestroyImmediate(c); UnityEngine.Object.DestroyImmediate(a); } }
        [Test] public void Catalog_MissingReference_Throws() { CustomerProfileCatalogAsset c = ScriptableObject.CreateInstance<CustomerProfileCatalogAsset>(); try { c.Configure(new CustomerProfileAsset[] { null }); Assert.Throws<InvalidOperationException>(() => c.BuildRegistry()); } finally { UnityEngine.Object.DestroyImmediate(c); } }
        [Test] public void Catalog_DuplicateId_Throws() { CustomerProfileAsset a = CreateProfile("p"); CustomerProfileAsset b = CreateProfile("p"); CustomerProfileCatalogAsset c = ScriptableObject.CreateInstance<CustomerProfileCatalogAsset>(); try { c.Configure(new[] { a, b }); Assert.Throws<ArgumentException>(() => c.BuildRegistry()); } finally { UnityEngine.Object.DestroyImmediate(c); UnityEngine.Object.DestroyImmediate(a); UnityEngine.Object.DestroyImmediate(b); } }
        [Test] public void SpawnSettings_BuildsPolicy() { CustomerSpawnSettingsAsset a = ScriptableObject.CreateInstance<CustomerSpawnSettingsAsset>(); try { a.Configure(6, 8, true); Assert.That(a.BuildPolicy().MaxActiveCustomers, Is.EqualTo(6)); } finally { UnityEngine.Object.DestroyImmediate(a); } }
        [Test] public void SpawnSettings_ZeroCap_ThrowsOnBuild() { CustomerSpawnSettingsAsset a = ScriptableObject.CreateInstance<CustomerSpawnSettingsAsset>(); try { a.Configure(0, 8, true); Assert.Throws<ArgumentOutOfRangeException>(() => a.BuildPolicy()); } finally { UnityEngine.Object.DestroyImmediate(a); } }
        [Test] public void RuntimeAuthoring_PreservesConfiguration() { GameObject go = new GameObject("customer"); CustomerProfileAsset p = CreateProfile("p"); try { CustomerRuntimeAuthoring a = go.AddComponent<CustomerRuntimeAuthoring>(); a.Configure("customer-1", p); Assert.That(a.CustomerInstanceId, Is.EqualTo("customer-1")); Assert.That(a.Profile, Is.SameAs(p)); } finally { UnityEngine.Object.DestroyImmediate(go); UnityEngine.Object.DestroyImmediate(p); } }

        private static CustomerProfileAsset CreateProfile(string id) { CustomerProfileAsset a = ScriptableObject.CreateInstance<CustomerProfileAsset>(); a.Configure(id, "name", new[] { "video-game" }, 1, 30, 2, 1.8f); return a; }
    }
}
