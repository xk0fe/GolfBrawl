using UnityEngine;

namespace Source.Character.SO
{
    [CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "SO/Game/CharacterScriptableObject", order = 0)]
    public class CharacterScriptableObject : ScriptableObject
    {
        public float MoveSpeed;
        public float RotationSpeed;
        public float JumpHeight;
    }
}