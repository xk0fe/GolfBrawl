using Source.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.GameMode.UI
{
    public class GameplayUI : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField]
        private NetworkService _networkService;

        [Header("Settings")]
        [SerializeField]
        private Button _disconnectButton;
        [SerializeField]
        private TextMeshProUGUI _nicknameText;
        [SerializeField]
        private TextMeshProUGUI _scoreText;

        private const string SCORE_KEY = "HITS: {0}\n\nSCR: {1}";
        
        private void Awake()
        {
            _disconnectButton.onClick.AddListener(OnDisconnectButtonClicked);
            
            SetScore(0, 0);
        }

        private void OnEnable()
        {
            if (_networkService == null)
            {
                return;
            }

            var localPlayerService = _networkService.LocalPlayerService;
            if (localPlayerService == null)
            {
                return;
            }

            var localPlayer = localPlayerService.LocalPlayer;
            if (localPlayer == null)
            {
                return;
            }
            
            _nicknameText.text = localPlayer.Name;
        }

        private void OnDisconnectButtonClicked()
        {
            _networkService.BreakSession();
        }

        private void SetScore(int hits, int score)
        {
            _scoreText.SetText(SCORE_KEY, hits, score);
        }
        
        private void OnDestroy()
        {
            _disconnectButton.onClick.RemoveListener(OnDisconnectButtonClicked);
        }
    }
}