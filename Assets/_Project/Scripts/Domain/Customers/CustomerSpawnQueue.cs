using System;
using System.Collections.Generic;

namespace VRMGames.CartridgeAndCloud.Domain.Customers
{
    public sealed class CustomerSpawnQueue
    {
        private readonly Queue<CustomerSpawnRequest> _queue =
            new Queue<CustomerSpawnRequest>();
        private readonly HashSet<CustomerSpawnRequestId> _requestIds =
            new HashSet<CustomerSpawnRequestId>();
        private readonly HashSet<CustomerInstanceId> _instanceIds =
            new HashSet<CustomerInstanceId>();

        public int Count => _queue.Count;

        public bool TryEnqueue(CustomerSpawnRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (_requestIds.Contains(request.RequestId) ||
                _instanceIds.Contains(request.InstanceId))
            {
                return false;
            }

            _queue.Enqueue(request);
            _requestIds.Add(request.RequestId);
            _instanceIds.Add(request.InstanceId);
            return true;
        }

        public bool TryPeek(out CustomerSpawnRequest request)
        {
            if (_queue.Count == 0)
            {
                request = null;
                return false;
            }

            request = _queue.Peek();
            return true;
        }

        public bool TryDequeue(out CustomerSpawnRequest request)
        {
            if (_queue.Count == 0)
            {
                request = null;
                return false;
            }

            request = _queue.Dequeue();
            _requestIds.Remove(request.RequestId);
            _instanceIds.Remove(request.InstanceId);
            return true;
        }

        public void Clear()
        {
            _queue.Clear();
            _requestIds.Clear();
            _instanceIds.Clear();
        }
    }
}
