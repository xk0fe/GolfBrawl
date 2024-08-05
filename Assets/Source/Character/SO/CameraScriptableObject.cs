using UnityEngine;

namespace Source.Character.SO
{
    [CreateAssetMenu(fileName = "CameraScriptableObject", menuName = "SO/Game/CameraScriptableObject", order = 0)]
    public class CameraScriptableObject : ScriptableObject
    {
        [Header("Rotation")]
        public float RotationSpeed;
        public float PitchMin;
        public float PitchMax;
        
        [Header("Zoom")]
        public float ZoomSpeed;
        public float DistanceMin;
        public float DistanceMax;
    }
}