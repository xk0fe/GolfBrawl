using System.Collections.Generic;
using UnityEngine;

namespace Source.Trigger.Base
{
    public class TriggerBase : MonoBehaviour
    {
        private List<TriggerValidator> _validators = new();
        private List<TriggerInvoker> _invokers = new();

        private void Awake()
        {
            _validators.AddRange(GetComponents<TriggerValidator>());
            _invokers.AddRange(GetComponents<TriggerInvoker>());
        }

        private void OnTriggerEnter(Collider other)
        {
            foreach (var validator in _validators)
            {
                if (!validator.IsValid(other))
                {
                    return;
                }
            }

            foreach (var invoker in _invokers)
            {
                invoker.Invoke(other);
            }
        }

        private void OnValidate()
        {
            _invokers.Clear();
            _invokers.AddRange(GetComponents<TriggerInvoker>());
            
            if (_invokers.Count == 0)
            {
                Debug.LogWarning($"{nameof(TriggerBase)} is missing {nameof(TriggerInvoker)} component!");
            }
        }
    }
}