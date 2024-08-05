using System.Collections.Generic;
using Fusion.Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Source.Lobby.UI
{
    public class RegionSelectorUI : MonoBehaviour
    {
        [SerializeField]
        private PhotonAppSettings _appSettings;
        [SerializeField]
        private TMP_Dropdown _regionDropdown;
        
        private readonly List<string> _availableRegions = new() { "Best Region", "asia", "eu", "sa", "us" };
        
        private void Awake()
        {
            _regionDropdown.options.Clear();
            foreach (var region in _availableRegions)
            {
                _regionDropdown.options.Add(new TMP_Dropdown.OptionData(region));
            }
            
            _regionDropdown.onValueChanged.AddListener(OnRegionChanged);
        }
        
        private void OnRegionChanged(int regionIndex)
        {
            if (regionIndex < 0 || regionIndex >= _availableRegions.Count)
            {
                Debug.LogError($"Invalid region index {regionIndex}");
                return;
            }

            if (regionIndex == 0)
            {
                _appSettings.AppSettings.FixedRegion = string.Empty;
                Debug.Log("Region changed to Best Region");
                return;
            }

            var region = _availableRegions[regionIndex];
            _appSettings.AppSettings.FixedRegion = region;
            Debug.Log($"Region changed to {region}");
        }
        
        private void OnDestroy()
        {
            _regionDropdown.onValueChanged.RemoveListener(OnRegionChanged);
        }
    }
}