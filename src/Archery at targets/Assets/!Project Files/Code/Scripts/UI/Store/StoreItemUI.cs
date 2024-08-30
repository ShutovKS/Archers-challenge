using System;
using Data.Weapon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Store
{
    public class StoreItemUI : MonoBehaviour
    {
        public event Action<string> OnItemClicked;

        [SerializeField] private Button itemButton;
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text itemName;
        [SerializeField] private TMP_Text itemPrice;

        private string _key;

        public void Setup(WeaponData itemData)
        {
            gameObject.SetActive(true);
            
            _key = itemData.Key;
            
            itemName.text = itemData.Name;
            itemPrice.text = itemData.Price.ToString();
            itemImage.overrideSprite = itemData.Icon;
            itemButton.onClick.AddListener(() => OnItemClicked?.Invoke(_key));
        }

        private void OnDestroy()
        {
            itemButton.onClick.RemoveAllListeners();
        }
    }
}