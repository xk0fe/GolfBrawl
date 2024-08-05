using UnityEngine;

namespace Source.Trigger.Base
{
    [RequireComponent(typeof(TriggerBase))]
    [AddComponentMenu(null)]
    public abstract class TriggerInvoker : MonoBehaviour
    {

        public virtual void Invoke(Collider other)
        {
            
        }
    }
}