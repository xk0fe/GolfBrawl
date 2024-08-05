using UnityEngine;
using UnityEngine.Events;

namespace Source.Events
{
    public class GenericEventChannel<T> : ScriptableObject
    {
        public event UnityAction<T> OnEventRaised;
        
        public void Invoke(T value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}