using UnityEngine;
using UnityEngine.Events;

namespace Source.Events
{
    [CreateAssetMenu(fileName = "VoidEventChannel", menuName = "SO/Events/VoidEventChannel", order = 0)]
    public class VoidEventChannel : ScriptableObject
    {
        public event UnityAction OnEventRaised;
        public void Invoke()
        {
            OnEventRaised?.Invoke();
        }
    }
}