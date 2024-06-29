using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BowAndArrow.BowScripts
{
    public class StretchHaptic : MonoBehaviour
    {
        [SerializeField] private int clicksPerDraw = 10;

        [SerializeField] private float amplitude = 0.1f, duration = 0.01f;
        [SerializeField] private XRBaseController vibratingController1;
        [SerializeField] private XRBaseController vibratingController2;

        [SerializeField, Range(0, 1)] private float currentAmount;

        private float _previousAmount;

        private void Update()
        {
            if (currentAmount.Equals(_previousAmount))
            {
                return;
            }

            var betweenClicks = 1 / (clicksPerDraw - 1f);
            var currentClick = Mathf.Floor(currentAmount / betweenClicks);
            var previousClick = Mathf.Floor(_previousAmount / betweenClicks);
            if (!currentClick.Equals(previousClick))
            {
                vibratingController1.SendHapticImpulse(amplitude, duration * Mathf.Abs(currentClick - previousClick));
                vibratingController2.SendHapticImpulse(amplitude, duration * Mathf.Abs(currentClick - previousClick));
            }

            _previousAmount = currentAmount;
        }

        public void Pulse()
        {
            vibratingController1.SendHapticImpulse(0.3f, 0.01f);
            vibratingController2.SendHapticImpulse(0.3f, 0.01f);
        }

        public void SetAmount(float setAmount)
        {
            currentAmount = setAmount;
        }
    }
}