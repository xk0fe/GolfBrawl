using Source.Events;
using Source.Trigger.Base;
using UnityEngine;

namespace Source.Trigger.Invokers
{
    public class TriggerGameObjectThisInvoker : TriggerInvoker
    {
        [SerializeField]
        private GameObjectEventChannel _onTriggerEnter;

        public override void Invoke(Collider other)
        {
            _onTriggerEnter.Invoke(gameObject);
        }
    }
}