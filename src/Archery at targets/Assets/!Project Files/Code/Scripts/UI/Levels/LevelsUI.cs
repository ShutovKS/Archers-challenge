#region

using System;
using System.Collections.Generic;
using Data.Level;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace UI.Levels
{
    public class LevelsUI : BaseUI
    {
        public event Action OnBackClicked;
        public event Action<string> OnItemClicked;

        [SerializeField] private Button backButton;
        [SerializeField] private Transform itemContainer;
        [SerializeField] private LevelItemUI itemUIPrefab;

        private readonly List<LevelItemUI> _items = new();

        private void Awake()
        {
            backButton.onClick.AddListener(() => OnBackClicked?.Invoke());
        }

        public void SetItems(IEnumerable<LevelData> weaponDatas)
        {
            ClearItems();

            foreach (var weaponData in weaponDatas)
            {
                AddItem(weaponData);
            }
        }

        private void AddItem(LevelData itemData)
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