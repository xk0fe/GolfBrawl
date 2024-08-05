using PrimeTween;
using UnityEngine;

namespace Source.Clubs
{
    public class ClubView : MonoBehaviour
    {
        public Vector3 HitPosition => _hitPositon;
        
        [SerializeField]
        private Vector3 _hitPositon;
        
        [Header("Debug")]
        [SerializeField]
        private float _hitRadius = 0.1f;

        private Tween _currentTween;

        private const float TWEEN_DURATION = 0.1f;
        
        public void SetRotation(Vector3 rotation)
        {
            if (_currentTween.isAlive)
            {
                _currentTween.Stop();
            }
            _currentTween = Tween.LocalRotation(transform, rotation, TWEEN_DURATION);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var position = transform.TransformPoint(_hitPositon);
            Gizmos.DrawSphere(position, _hitRadius);
        }
    }
}