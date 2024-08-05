using Fusion;
using Source.Settings;
using UnityEngine;

namespace Source.Level
{
    public class LevelController
    {
        private GameScriptableObject _settings;
        private NetworkRunner _runner;
        
        private LevelConstructor[] _levels;
        private int _currentLevelIndex = -1;
        private LevelConstructor _currentLevelConstructor;
        
        private const int FIRST_LEVEL_INDEX = 0;

        public LevelController(GameScriptableObject settings, NetworkRunner runner)
        {
            _settings = settings;
            _runner = runner;
        }
        
        public void Start()
        {
            if (_levels == null || _levels.Length == 0)
            {
                Debug.LogError("No levels assigned to LevelManager.");
                return;
            }
            
            LoadLevel(FIRST_LEVEL_INDEX);
        }
        
        public void SetLevels(LevelConstructor[] levels)
        {
            _levels = levels;
        }

        public void LoadLevel(int index)
        {
            if (index < 0 || index >= _levels.Length)
            {
                Debug.LogError("Level index out of range.");
                return;
            }

            UnloadCurrentLevel();
            
            _currentLevelIndex = index;
            _currentLevelConstructor = Object.Instantiate(_levels[_currentLevelIndex], Vector3.zero, Quaternion.identity);
            _currentLevelConstructor.Build(_runner, _settings.HolePrefab, _settings.BallPrefab);
        }

        public void LoadNextLevel()
        {
            var nextLevelIndex = (_currentLevelIndex + 1) % _levels.Length;
            LoadLevel(nextLevelIndex);
        }

        public void LoadPreviousLevel()
        {
            var previousLevelIndex = (_currentLevelIndex - 1 + _levels.Length) % _levels.Length;
            LoadLevel(previousLevelIndex);
        }

        private void UnloadCurrentLevel()
        {
            if (_currentLevelConstructor != null)
            {
                Object.Destroy(_currentLevelConstructor.gameObject);
            }
        }

        public void ReloadCurrentLevel()
        {
            LoadLevel(_currentLevelIndex);
        }
    }
}