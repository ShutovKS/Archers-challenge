#region

using System;
using Data.Configurations.Level;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace UI.Levels
{
    public class LevelItemUI : MonoBehaviour
    {
        public event Action<string> OnItemClicked;

        [SerializeField] private Button itemButton;

        [SerializeField] private Image itemImage;

        [SerializeField] private TMP_Text itemName;
        [SerializeField] private TMP_Text gameplayMode;

        [SerializeField] private Transform itemStarsContainer;
        [SerializeField] private Image itemStars;

        public void Setup(
            string levelKey,
            string levelName,
            string levelGameplayMode,
            Sprite levelIcon,
            int stars
        )
        {
            gameObject.SetActive(true);

            itemName.text = levelName;

            gameplayMode.text = levelGameplayMode;

            itemImage.overrideSprite = levelIcon;

            CreateStars(stars);

            itemButton.onClick.AddListener(Clicked);

            return;

            void Clicked()
            {
                OnItemClicked?.Invoke(levelKey);
            }
        }

        private void CreateStars(int stars)
        {
            for (var i = 0; i < stars; i++)
            {
                var star = Instantiate(itemStars, itemStarsContainer);
                star.gameObject.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            itemButton.onClick.RemoveAllListeners();
        }
    }
}