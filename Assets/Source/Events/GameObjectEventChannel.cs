using UnityEngine;

namespace Source.Events
{
    [CreateAssetMenu(fileName = "GameObjectEventChannel", menuName = "SO/Events/GameObjectEventChannel", order = 0)]
    public class GameObjectEventChannel : GenericEventChannel<GameObject>
    {
    }
}