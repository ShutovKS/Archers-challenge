using UnityEngine;

namespace BowAndArrow.BowScripts
{
    public class StretchSound : MonoBehaviour
    {
        [SerializeField] private AudioSource aSource;
        [SerializeField] private float minPitch = .25f;
        [SerializeField] private float maxPitch = .55f;

        [SerializeField] private float minVolume;
        [SerializeField] private float maxVolume = 1f;
        [SerializeField] private float volumeCurve = 0.001f;

        [SerializeField] private float stretchSpeed = .6f;

        [SerializeField, Range(0, 1)] private float amount;

        private float _lastPitch;
        private bool _isPaused;

        private void Start()
        {
            aSource.volume = 0;
            aSource.Play();
        }

        private void Update()
        {
            var newPitch = Mathf.Lerp(aSource.pitch, Mathf.Lerp(minPitch, maxPitch, amount), stretchSpeed);
            if (Mathf.Abs(newPitch - _lastPitch) < volumeCurve)
            {
                aSource.volume = Mathf.Lerp(maxVolume, minVolume, 1 - Mathf.Abs(newPitch - _lastPitch) / volumeCurve);

                _isPaused = true;
            }

            else
            {
                if (_isPaused)
                {
                    aSource.volume = maxVolume;
                    _isPaused = false;
                }

                aSource.pitch = newPitch;
            }

            _lastPitch = newPitch;
        }

        public void SetAmount(float setAmount)
        {
            amount = setAmount;
        }
    }
}