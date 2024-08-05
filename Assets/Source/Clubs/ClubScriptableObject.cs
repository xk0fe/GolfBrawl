using UnityEngine;

namespace Source.Clubs
{
    [CreateAssetMenu(fileName = "ClubScriptableObject", menuName = "SO/Game/ClubScriptableObject", order = 0)]
    public class ClubScriptableObject : ScriptableObject
    {
        public string Name;
        public float PowerIncreaseSpeed;
        public float MaxPower;
        public float HitRadius;

        public ClubView Prefab;
        
        [Range(0, 1f)]
        public float GroundedThreshold;
    }
}