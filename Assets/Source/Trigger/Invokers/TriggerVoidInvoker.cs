using Source.Events;
using Source.Trigger.Base;
using UnityEngine;

namespace Source.Trigger.Invokers
{
    public class TriggerVoidInvoker : TriggerInvoker
    {
        [SerializeField]
        private VoidEventChannel _onTriggerEnter;
        
        public override void Invoke(Collider other)
        {
            _onTriggerEnter.Invoke();
        }
    }
}