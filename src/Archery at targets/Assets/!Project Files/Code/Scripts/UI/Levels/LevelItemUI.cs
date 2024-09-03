#region

using System;
using Data.Level;
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
        [SerializeField] private TMP_Text itemStars;

        private string _key;

        public void Setup(LevelData itemData)
        {
            gameObject.SetActive(true);

            _key = itemData.Key;

            itemName.text = itemData.LevelName;
            itemImage.overrideSprite = itemData.Icon;
            itemStars.text = $"??? Stars";
            itemButton.onClick.AddListener(() => OnItemClicked?.Invoke(_key));
        }

        private void OnDestroy()
        {
            itemButton.onClick.RemoveAllListeners();
        }
    }
}