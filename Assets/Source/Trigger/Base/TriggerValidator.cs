using UnityEngine;

namespace Source.Trigger.Base
{
    [RequireComponent(typeof(TriggerBase))]
    [AddComponentMenu(null)]
    public class TriggerValidator : MonoBehaviour
    {
        public virtual bool IsValid(Collider other)
        {
            return true;
        }
    }
}