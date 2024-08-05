using Source.Notifications.Enum;
using Source.Notifications.UI;
using UnityEngine;

namespace Source.Notifications
{
    public class Notification : MonoBehaviour
    {
        public static Notification Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.Log($"{nameof(Notification)} is not initialized!");
                }

                return _instance;
            }
        }
        
        private static Notification _instance;
        
        [SerializeField]
        private NotificationToastUI _notificationPrefab;
        
        [SerializeField]
        private Transform _notificationParent;

        private void Awake()
        {
            _instance = this;
        }
        
        public static void Show(string context, NotificationType type)
        {
            Instance.ShowNotification(context, type);
        }
        
        private void ShowNotification(string context, NotificationType type)
        {
            var toast = Instantiate(_notificationPrefab, _notificationParent);
            toast.Initialize(context, type);
        }
    }
}