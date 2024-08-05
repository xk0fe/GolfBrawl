using System;
using Fusion;
using Source.Network;
using Source.Notifications;
using Source.Notifications.Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Lobby.UI
{
    public class LobbyUI : MonoBehaviour
    {
        [Header("View")]
        [SerializeField]
        private Color _defaultNicknameColor;
        [SerializeField]
        private Color _missingNicknameColor;
        
        [Header("Settings")]
        [SerializeField]
        private TMP_InputField _nicknameInput;
        [SerializeField]
        private Image _nicknameInputBackground;
        
        [SerializeField]
        private TMP_InputField _roomInput;

        [SerializeField]
        private TextMeshProUGUI _connectText;
        [SerializeField]
        private Button _connectButton;
        
        [Header("Dependencies")]
        [SerializeField, Obsolete("Use NetworkService instead")]
        private FusionBootstrap _fusionBootstrap;

        [SerializeField]
        private NetworkService _networkService;
        
        [SerializeField]
        private bool _useFusionBootstrap;
        
        private void Awake()
        {
            _connectButton.onClick.AddListener(Connect);
            _nicknameInput.onValueChanged.AddListener(OnNicknameChanged);
            _nicknameInput.onSubmit.AddListener(OnNicknameChanged);
            
            _networkService.OnPlayerJoinedServer += OnPlayerJoinedServer;
            _networkService.OnPlayerLeftServer += OnPlayerLeftServer;
        }

        private void Start()
        {
            _nicknameInput.text = _networkService.LocalPlayerService.LocalPlayer.Name;
        }

        private void OnPlayerJoinedServer()
        {
            SetActive(false);
        }
        
        private void OnPlayerLeftServer()
        {
            SetActive(true);
            Reset();
        }
        
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        private void Connect()
        {
            if (string.IsNullOrEmpty(_nicknameInput.text))
            {
                _nicknameInputBackground.color = _missingNicknameColor;
                Notification.Show("Nickname is missing!", NotificationType.Error);
                return;
            }

            if (_useFusionBootstrap)
            {
                _fusionBootstrap.DefaultRoomName = _roomInput.text;
                _fusionBootstrap.StartSharedClient();
            }
            else
            {
                _networkService.JoinOrCreateSession(_roomInput.text);
            }
            
            _connectButton.interactable = false;
            _connectText.text = "Connecting...";
        }
        
        private void OnNicknameChanged(string nickname)
        {
            _nicknameInputBackground.color = string.IsNullOrEmpty(nickname) ? _missingNicknameColor : _defaultNicknameColor;
            _networkService.LocalPlayerService.SetNickname(nickname);
        }
        
        private void Reset()
        {
            _connectButton.interactable = true;
            _connectText.text = "Play!";
        }

        private void OnDestroy()
        {
            _connectButton.onClick.RemoveListener(Connect);
            _nicknameInput.onValueChanged.RemoveListener(OnNicknameChanged);
            _nicknameInput.onSubmit.RemoveListener(OnNicknameChanged);
            _networkService.OnPlayerJoinedServer -= OnPlayerJoinedServer;
            _networkService.OnPlayerLeftServer -= OnPlayerLeftServer;
        }
    }
}