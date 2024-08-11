using UnityEngine;

namespace Fitches.BowAndArrows
{
    /// <summary>
    /// Создаёт стрелы на луке и управляет их состоянием.
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
            bow.Selected += HandleBowSelection;
            bowstring.PullReleased += HandleArrowRelease;
            bowstring.Selected += HandleBowstringSelection;
        }

        private void OnDisable()
        {
            bow.Selected -= HandleBowSelection;
            bowstring.PullReleased -= HandleArrowRelease;
            bowstring.Selected -= HandleBowstringSelection;
        }

        private void HandleBowSelection(bool isSelected)
        {
            if (isSelected)
            {
                bowstring.UnlockSelect();
            }
            else
            {
                ReleaseBow();
            }
        }

        private void HandleBowstringSelection(bool isSelected)
        {
            if (isSelected)
            {
                CreateArrow();
            }
        }

        private void HandleArrowRelease(float pullAmount)
        {
            if (pullAmount > 0 && _currentArrow != null)
            {
                LaunchArrow(pullAmount);
            }
        }

        private void CreateArrow()
        {
            _currentArrow = Instantiate(arrowPrefab, notchTransform);
            _currentArrow.transform.localPosition = Vector3.zero;
            _currentArrow.transform.localRotation = Quaternion.identity;
        }

        private void ReleaseBow()
        {
            TryDestroyArrow();
            bowstring.ReleaseBow();
            bowstring.LockSelect();
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
            if (_currentArrow == null) return;

            var arrowScript = _currentArrow.GetComponent<Arrow>();
            if (arrowScript != null)
            {
                arrowScript.Fire(pullAmount);
            }

            audioSourceShot?.Play();
            _currentArrow = null;
        }
    }
}