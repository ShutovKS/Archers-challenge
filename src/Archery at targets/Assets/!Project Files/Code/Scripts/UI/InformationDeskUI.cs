using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class InformationDeskUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text textPrefab;
        [SerializeField] private Transform parent;

        private readonly Dictionary<string, TMP_Text> _informationTexts = new();

        public void SetInformationText(string key, string value)
        {
            if (!_informationTexts.ContainsKey(key))
            {
                var text = Instantiate(textPrefab, parent);

                text.gameObject.SetActive(true);

                _informationTexts.Add(key, text);
            }

            _informationTexts[key].SetText(value);
        }

        public void RemoveInformationText(string key)
        {
            if (_informationTexts.ContainsKey(key))
            {
                Destroy(_informationTexts[key].gameObject);

                _informationTexts.Remove(key);
            }
        }
    }
}