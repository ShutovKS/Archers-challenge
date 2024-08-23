// using System.Threading.Tasks;
// using Features.Projectile;
// using Features.Weapon;
// using UnityEngine;
// using Zenject;
//
// namespace Features
// {
//     /// <summary>
//     /// Создаёт стрелы на луке и управляет их состоянием.
//     /// </summary>
//     public class ArrowSpawner : MonoBehaviour
//     {
//         [SerializeField] private Bow bow;
//         [SerializeField] private Bowstring bowstring;
//
//         [SerializeField] private Transform notchTransform;
//         [SerializeField] private AudioSource audioSourceShot;
//
//         private GameObject _currentArrow;
//         private IProjectileFactory _projectileFactory;
//
//         [Inject]
//         public void Construct(IProjectileFactory projectileFactory)
//         {
//             _projectileFactory = projectileFactory;
//         }
//
//         private void OnEnable()
//         {
//             bow.OnSelected += HandleBowSelection;
//             bowstring.OnPullReleased += HandleArrowRelease;
//             bowstring.OnSelected += HandleBowstringSelection;
//         }
//
//         private void OnDisable()
//         {
//             bow.OnSelected -= HandleBowSelection;
//             bowstring.OnPullReleased -= HandleArrowRelease;
//             bowstring.OnSelected -= HandleBowstringSelection;
//         }
//
//         private void HandleBowSelection(bool isSelected)
//         {
//             if (isSelected)
//             {
//                 bowstring.UnlockSelect();
//             }
//             else
//             {
//                 ReleaseBow();
//             }
//         }
//
//         private async void HandleBowstringSelection(bool isSelected)
//         {
//             if (isSelected)
//             {
//                 await CreateArrow();
//             }
//         }
//
//         private void HandleArrowRelease(float pullAmount)
//         {
//             if (pullAmount > 0 && _currentArrow != null)
//             {
//                 LaunchArrow(pullAmount);
//             }
//         }
//
//         private async Task CreateArrow()
//         {
//             _currentArrow = await _projectileFactory.Instantiate(notchTransform.position, notchTransform.rotation);
//         }
//
//         private void ReleaseBow()
//         {
//             TryDestroyArrow();
//             bowstring.ReleaseBow();
//             bowstring.LockSelect();
//         }
//
//         private void TryDestroyArrow()
//         {
//             if (_currentArrow != null)
//             {
//                 Destroy(_currentArrow);
//                 _currentArrow = null;
//             }
//         }
//
//         private void LaunchArrow(float pullAmount)
//         {
//             if (_currentArrow == null) return;
//
//             var arrowScript = _currentArrow.GetComponent<Arrow>();
//             if (arrowScript != null)
//             {
//                 arrowScript.Fire(pullAmount);
//             }
//
//             audioSourceShot?.Play();
//             _currentArrow = null;
//         }
//     }
// }