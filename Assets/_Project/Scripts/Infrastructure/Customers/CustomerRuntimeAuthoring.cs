using UnityEngine;
using VRMGames.CartridgeAndCloud.Domain.Customers;

namespace VRMGames.CartridgeAndCloud.Infrastructure.Customers
{
    public sealed class CustomerRuntimeAuthoring : MonoBehaviour
    {
        [SerializeField]
        private string _customerInstanceId;

        [SerializeField]
        private CustomerProfileAsset _profile;

        public string CustomerInstanceId => _customerInstanceId;

        public CustomerProfileAsset Profile => _profile;

        public CustomerInstance BuildInstance(
            CustomerNavigationPlan navigationPlan)
        {
            CustomerProfile profile = _profile.BuildProfile();
            return new CustomerInstance(
                new CustomerInstanceId(_customerInstanceId),
                profile.Id,
                navigationPlan,
                profile.PatienceSeconds);
        }

        public void Configure(
            string customerInstanceId,
            CustomerProfileAsset profile)
        {
            _customerInstanceId = customerInstanceId;
            _profile = profile;
        }
    }
}
