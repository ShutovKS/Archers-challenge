using System;
using System.Collections.Generic;
using Data.Weapon;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Store
{
    public class StoreUI : MonoBehaviour
    {
        public event Action OnBackClicked;
        public event Action<string> OnItemClicked;

        [SerializeField] private Button backButton;
        [SerializeField] private Transform itemContainer;
        [SerializeField] private StoreItemUI itemUIPrefab;

        private readonly List<StoreItemUI> _items = new();

        private void Awake()
        {
            backButton.onClick.AddListener(() => OnBackClicked?.Invoke());
        }

        public void SetItems(IEnumerable<WeaponData> weaponDatas)
        {
            ClearItems();

            foreach (var weaponData in weaponDatas)
            {
                AddItem(weaponData);
            }
        }

        private void AddItem(WeaponData itemData)
        {
            var itemUI = Instantiate(itemUIPrefab, itemContainer);

            itemUI.Setup(itemData);

            itemUI.OnItemClicked += OnItemClicked;

            _items.Add(itemUI);
        }

        public void ClearItems()
        {
            foreach (var itemUI in _items)
            {
                itemUI.OnItemClicked -= OnItemClicked;
                
                Destroy(itemUI.gameObject);
            }

            _items.Clear();
        }
        
        private void OnDestroy()
        {
            backButton.onClick.RemoveAllListeners();
        }
    }
}