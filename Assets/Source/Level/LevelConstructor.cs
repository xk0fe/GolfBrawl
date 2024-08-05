using System.Collections.Generic;
using Fusion;
using Source.Ball;
using Source.Trigger.Base;
using UnityEngine;

namespace Source.Level
{
    public class LevelConstructor : MonoBehaviour
    {
        [SerializeField]
        private Vector3[] _playerSpawnPositions;
        [SerializeField]
        private Vector3[] _holePositions;
        [SerializeField]
        private Vector3 _ballSpawnPosition;

        private HashSet<Vector3> _takenSpawnPoints = new();
        
        public void Build(NetworkRunner runner, TriggerBase holeTrigger, BallObject ball)
        {
            foreach (var holePosition in _holePositions)
            {
                var holeInstance = Instantiate(holeTrigger, holePosition, Quaternion.identity);
                holeInstance.transform.SetParent(transform);
            }

            if (runner.IsSharedModeMasterClient)
            {
                runner.Spawn(ball, _ballSpawnPosition, Quaternion.identity, runner.LocalPlayer);
            }
        }

        public Vector3 TryTakeUniquePlayerSpawnPoint()
        {
            foreach (var position in _playerSpawnPositions)
            {
                if (!_takenSpawnPoints.Add(position))
                {
                    continue;
                }

                return position;
            }
            
            return _playerSpawnPositions[Random.Range(0, _playerSpawnPositions.Length)];
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            foreach (var position in _playerSpawnPositions)
            {
                Gizmos.DrawSphere(position, 0.5f);
            }
            
            Gizmos.color = Color.red;
            foreach (var position in _holePositions)
            {
                Gizmos.DrawSphere(position, 0.5f);
            }
            
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_ballSpawnPosition, 0.5f);
        }
    }
}