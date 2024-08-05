using Fusion;
using Source.Level;
using Source.Player;
using Source.Settings;
using UnityEngine;

namespace Source.Character
{
    public class CharacterSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {
        [SerializeField]
        private GameScriptableObject _settings;
        
        public void PlayerJoined(PlayerRef player)
        {
            // refactor
            var level = FindObjectOfType<LevelConstructor>();
            
            if (level == null)
            {
                Debug.LogError("No level found.");
                return;
            }

            var spawnPoint = level.TryTakeUniquePlayerSpawnPoint();

            if (player != Runner.LocalPlayer)
            {
                return;
            }

            var runner = Runner;
            var character = runner.Spawn(_settings.CharacterPrefab, spawnPoint, Quaternion.identity, player);
            
            if (!character.TryGetComponent<CharacterController>(out var characterController))
            { 
                return;   
            }

            characterController.enabled = true;
        }

        public void PlayerLeft(PlayerRef player)
        {
            var runner = Runner;
            var character = runner.GetPlayerObject(player);
            if (character != null)
            {
                runner.Despawn(character);
            }
        }
    }
}