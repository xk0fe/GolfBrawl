using Source.Clubs;
using Source.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Source.UI
{
    public class HitDataUI : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField]
        private FloatEventChannel _onPowerChanged;
        [SerializeField]
        private ClubEventChannel _onClubChanged;

        [Header("View")]
        [SerializeField]
        private Color _minPower;
        [SerializeField]
        private Color _maxPower;
        
        [Header("Settings")]
        [SerializeField]
        private Image _powerMeter;
        [SerializeField]
        private RectTransform _powerMeterParent;
        [SerializeField]
        private RectTransform _groundedThreshold;

        private const int MAX_GROUNDED_THRESHOLD = 1;
        
        private void Awake()
        {
            _onPowerChanged.OnEventRaised += UpdatePower;
            _onClubChanged.OnEventRaised += UpdateGroundedThreshold;
        }

        public void UpdatePower(float fraction)
        {
            var color = Color.Lerp(_minPower, _maxPower, fraction);
            _powerMeter.color = color;
            _powerMeter.fillAmount = fraction;
        }
        
        private void UpdateGroundedThreshold(ClubScriptableObject club)
        {
            var width = _powerMeterParent.rect.width;
            var max = width / 2;
            var min = -max;
            var threshold = club.GroundedThreshold;

            var position = Mathf.Lerp(min, max, threshold);
            _groundedThreshold.anchoredPosition = new Vector2(position, 0);
            _groundedThreshold.gameObject.SetActive(threshold < MAX_GROUNDED_THRESHOLD);
        }

        private void OnDestroy()
        {
            _onPowerChanged.OnEventRaised -= UpdatePower;
            _onClubChanged.OnEventRaised -= UpdateGroundedThreshold;
        }
    }
}