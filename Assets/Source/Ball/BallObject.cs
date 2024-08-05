using System.Collections.Generic;
using Fusion;
using PrimeTween;
using Source.Events;
using Source.Settings;
using UnityEngine;

namespace Source.Ball
{
    public class BallObject : NetworkBehaviour
    {
        [SerializeField]
        private GameObjectEventChannel _onHoleEnter;

        [SerializeField]
        private Collider _collider;
        [SerializeField]
        private Rigidbody _rigidbody;

        private Queue<HitData> _hitQueue = new();
        
        private void Awake()
        {
            _onHoleEnter.OnEventRaised += OnHoleEnter;
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_ApplyForce(Vector3 hitPosition, Vector3 direction, float force, bool isGrounded)
        {
            _hitQueue.Enqueue(new HitData
            {
                HitPosition = hitPosition,
                Direction = direction,
                Force = force,
                IsGrounded = isGrounded
            });
        }
        
        private void OnHoleEnter(GameObject hole)
        {
            _collider.enabled = false;
            _rigidbody.isKinematic = true;

            var delay = Random.Range(GameSettingsConstants.HOLE_IN_DELAY_MIN, GameSettingsConstants.HOLE_IN_DELAY_MAX);
            Sequence.Create()
                .Chain(Tween.Position(transform, hole.transform.position, .25f, Ease.InSine, startDelay: delay))
                .Chain(Tween.PositionY(transform, -1, .5f, Ease.Linear));
        }

        public override void FixedUpdateNetwork()
        {
            while (_hitQueue.Count > 0)
            {
                var data = _hitQueue.Dequeue();
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
                if (data.IsGrounded)
                {
                    _rigidbody.AddForceAtPosition(data.Direction * data.Force, data.HitPosition, ForceMode.Impulse);
                }
                else
                {
                    var jumpDirection = data.Direction + Vector3.up * 1.25f;
                    jumpDirection.Normalize();
                    _rigidbody.AddForceAtPosition(jumpDirection * (data.Force), data.HitPosition, ForceMode.Impulse);
                }
            }
        }

        private void OnDestroy()
        {
            _onHoleEnter.OnEventRaised -= OnHoleEnter;
        }
    }
}