using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button buttonPrefab;
        [SerializeField] private Transform contentTransform;

        private readonly Dictionary<string, Button> _buttons = new();

        public void AddButton(string buttonName, Action onClick)
        {
            var button = Instantiate(buttonPrefab, contentTransform);
            button.GetComponentInChildren<TMP_Text>().text = buttonName;
            button.onClick.AddListener(() => onClick());
            button.gameObject.SetActive(true);

            _buttons.Add(buttonName, button);
        }

        public void RemoveButton(string buttonName)
        {
            if (_buttons.Remove(buttonName, out var button))
            {
                Destroy(button.gameObject);
            }
        }

        public void ClearButtons()
        {
            foreach (var button in _buttons.Values)
            {
                Destroy(button.gameObject);
            }

            _buttons.Clear();
        }
    }
}