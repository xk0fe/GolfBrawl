using Source.Settings;
using UnityEngine;

namespace Source.Network
{
    public class LocalPlayerService
    {
        public LocalPlayerData LocalPlayer => _localPlayer;
        private LocalPlayerData _localPlayer = new();
        
        public LocalPlayerService()
        {
            Load();
        }
        
        public void SetNickname(string nickname)
        {
            _localPlayer.Name = nickname;
            _localPlayer.OnNicknameUpdate?.Invoke(nickname);
            Save();
        }
        
        public void SetLevel(int level)
        {
            _localPlayer.Level = level;
            _localPlayer.OnLevelUpdate?.Invoke(level);
            Save();
        }

        public void SetExperience(int experience)
        {
            _localPlayer.Experience = experience;
            _localPlayer.OnExperienceUpdate?.Invoke(experience);
            TryIncreaseLevelRecursively();
            Save();
        }

        public void AddExperience(int experience)
        {
            _localPlayer.Experience += experience;
            _localPlayer.OnExperienceUpdate?.Invoke(experience);
            TryIncreaseLevelRecursively();
            Save();
        }

        private void TryIncreaseLevelRecursively()
        {
            if (_localPlayer.Experience < GameSettingsConstants.MAX_EXPERIENCE)
            {
                return;
            }
            
            if (_localPlayer.Level >= GameSettingsConstants.MAX_LEVEL)
            {
                return;
            }

            _localPlayer.Level = Mathf.Min(_localPlayer.Level + 1, GameSettingsConstants.MAX_LEVEL);
            _localPlayer.Experience -= GameSettingsConstants.MAX_EXPERIENCE;
            _localPlayer.OnLevelUpdate?.Invoke(_localPlayer.Level);
            _localPlayer.OnExperienceUpdate?.Invoke(_localPlayer.Experience);
            TryIncreaseLevelRecursively();
        }

        private void Load()
        {
            KeyValueStorage.TryGetOrDefault<LocalPlayerData>(StorageAliases.PLAYER_SETTINGS_KEY, out var playerData);
            _localPlayer = playerData;
        }
        
        private void Save()
        {
            KeyValueStorage.Set(StorageAliases.PLAYER_SETTINGS_KEY, _localPlayer);
        }
        
        public static string GetLocalPlayerNickname()
        {
            var networkService = NetworkService.Instance();
            if (networkService == null)
            {
                return string.Empty;
            }
            
            var localPlayerService = networkService.LocalPlayerService;
            if (localPlayerService == null)
            {
                return string.Empty;
            }

            var localPlayer = localPlayerService.LocalPlayer;
            return localPlayer == null ? string.Empty : localPlayer.Name;
        }
    }
}