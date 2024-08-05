using System;
using PrimeTween;
using Source.Notifications.Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Notifications.UI
{
    public class NotificationToastUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _context;

        [SerializeField]
        private Image _background;

        private const float DESTROY_OFFSET = 400;
        
        public void Initialize(string context, NotificationType type, float duration = 1)
        {
            _context.text = context;
            switch (type)
            {
                case NotificationType.Success:
                    _background.color = Color.green;
                    break;
                case NotificationType.Error:
                    _background.color = Color.red;
                    break;
                case NotificationType.Warning:
                    _background.color = Color.yellow;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
            Tween.Delay(duration).OnComplete(Destroy);
            Tween.StopAll(transform);
            Tween.PunchScale(transform, Vector3.one * .15f, .5f);
        }

        private void Destroy()
        {
            transform.SetParent(transform.parent.parent);
         
            Tween.StopAll(transform);
            var position = transform.position.x + DESTROY_OFFSET;
            Tween.PositionX(transform, position, .5f, Ease.Linear);
            Tween.Scale(transform, Vector3.zero, .65f).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
    }
}