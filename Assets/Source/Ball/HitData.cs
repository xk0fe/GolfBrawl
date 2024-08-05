using UnityEngine;

namespace Source.Ball
{
    public struct HitData
    {
        public Vector3 HitPosition { get; set; }
        public Vector3 Direction { get; set; }
        public float Force { get; set; }
        public bool IsGrounded { get; set; }
    }
}