using Fusion;
using PrimeTween;
using Source.Events;
using Source.Level.SO;
using Source.Network;
using Source.Settings;
using UnityEngine;

namespace Source.Level
{
    public class Boot : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField]
        private NetworkService _networkService;
    
        [SerializeField]
        private VoidEventChannel _onBallHitEventChannel;
        
        [Header("Settings")]
        [SerializeField]
        private GameScriptableObject _settings;
        [SerializeField]
        private CampaignScriptableObject _campaignScriptableObject;

        private CampaignController _campaignController;
    
        private void Awake()
        {
            _networkService.OnNetworkRunnerStarted += OnNetworkRunnerStarted;
            _networkService.OnPlayerJoinedServer += OnPlayerJoinedServer;
            _networkService.OnPlayerLeftServer += OnPlayerLeftServer;
            _onBallHitEventChannel.OnEventRaised += OnBallHit;
        }

        private void OnBallHit()
        {
            Tween.Delay(2.5f, () =>
            {
                _campaignController.LoadNextLevel();
            });
        }

        private void LockCursor(bool isLocked)
        {
            Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isLocked;
        }

        private void OnPlayerJoinedServer()
        {
            LockCursor(true);
        }

        private void OnPlayerLeftServer()
        {
            LockCursor(false);
        }

        private void OnNetworkRunnerStarted(NetworkRunner runner)
        {
            var levelController = new LevelController(_settings, runner);
            _campaignController = new CampaignController(levelController);
        
            _campaignController.StartCampaign(_campaignScriptableObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.LeftAlt))
            {
                LockCursor(Cursor.visible);
            }
        }

        private void OnDestroy()
        {
            _networkService.OnNetworkRunnerStarted -= OnNetworkRunnerStarted;
        }
    }
}
