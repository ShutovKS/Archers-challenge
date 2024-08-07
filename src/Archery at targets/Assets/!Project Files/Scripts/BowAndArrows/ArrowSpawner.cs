using UnityEngine;

namespace BowAndArrows
{
    /// <summary>
    /// Спавнит стрелы на луке и управляет их состоянием.
    /// </summary>
    public class ArrowSpawner : MonoBehaviour
    {
        [SerializeField] private Bow bow;
        [SerializeField] private Bowstring bowstring;

        [SerializeField] private GameObject arrowPrefab;
        [SerializeField] private Transform notchTransform;
        [SerializeField] private AudioSource audioSourceShot;

        private GameObject _currentArrow;

        private void OnEnable()
        {
            bow.Selected += TakeBow;
            bow.Selected += ReleaseBow;
            bowstring.PullReleased += HandleArrowRelease;
            bowstring.Selected += SelectedBowstring;
        }

        private void OnDisable()
        {
            bow.Selected -= TakeBow;
            bow.Selected -= ReleaseBow;
            bowstring.PullReleased -= HandleArrowRelease;
            bowstring.Selected -= SelectedBowstring;
        }

        private void TakeBow(bool isSelected)
        {
            if (!isSelected) return;
            
            bowstring.UnlockSelect();
        }

        private void ReleaseBow(bool isSelected)
        {
            if (isSelected) return;

            TryDestroyArrow();
            bowstring.LockSelect();
            bowstring.ReleaseBow();
        }

        private void SelectedBowstring(bool isSelected)
        {
            if (!isSelected) return;

            CreateArrow();
        }

        private void HandleArrowRelease(float pullAmount)
        {
            if (pullAmount > 0 && _currentArrow != null)
            {
                LaunchArrow(pullAmount);
            }

            _currentArrow = null;
        }

        private void CreateArrow()
        {
            _currentArrow = Instantiate(arrowPrefab, notchTransform);
            _currentArrow.transform.localPosition = Vector3.zero;
            _currentArrow.transform.localRotation = Quaternion.identity;
        }

        private void TryDestroyArrow()
        {
            if (_currentArrow != null)
            {
                Destroy(_currentArrow);
                _currentArrow = null;
            }
        }

        private void LaunchArrow(float pullAmount)
        {
            _currentArrow.GetComponent<Arrow>().Fire(pullAmount);

            audioSourceShot?.Play();
        }
    }
}