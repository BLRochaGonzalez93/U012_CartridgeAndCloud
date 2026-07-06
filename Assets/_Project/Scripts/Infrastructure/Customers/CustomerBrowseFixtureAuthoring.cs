using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Customers
{
    public sealed class CustomerBrowseFixtureAuthoring : MonoBehaviour
    {
        [SerializeField]
        private List<Transform> _points = new List<Transform>();

        [SerializeField]
        private string _assignedProductId = string.Empty;

        [SerializeField]
        [Min(0)]
        private int _productQuantity;

        public IReadOnlyList<Transform> Points => _points;

        public bool HasStock =>
            _productQuantity > 0 &&
            !string.IsNullOrWhiteSpace(_assignedProductId);

        public void ConfigurePoints(IEnumerable<Transform> points)
        {
            if (points == null)
            {
                throw new ArgumentNullException(nameof(points));
            }

            _points = new List<Transform>();
            foreach (Transform point in points)
            {
                if (point != null)
                {
                    _points.Add(point);
                }
            }
        }

        public void ConfigureStock(
            string assignedProductId,
            int productQuantity)
        {
            _assignedProductId = assignedProductId ?? string.Empty;
            _productQuantity = Mathf.Max(0, productQuantity);
        }
    }
}
