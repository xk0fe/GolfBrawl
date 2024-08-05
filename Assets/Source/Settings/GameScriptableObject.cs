using Fusion;
using Source.Ball;
using Source.Trigger.Base;
using UnityEngine;

namespace Source.Settings
{
    // rename to base game assets or something
    [CreateAssetMenu(fileName = "GameScriptableObject", menuName = "SO/Game/GameScriptableObject", order = 0)]
    public class GameScriptableObject : ScriptableObject
    {
        public NetworkObject CharacterPrefab;
        public TriggerBase HolePrefab;
        public BallObject BallPrefab;
    }
}