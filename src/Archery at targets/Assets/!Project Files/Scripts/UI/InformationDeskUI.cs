using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class InformationDeskUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text informationPrefab;

        private readonly Dictionary<string, TMP_Text> _informationTexts = new();

        public void SetInformationText(string key, string value)
        {
            if (!_informationTexts.ContainsKey(key))
            {
                var informationText = Instantiate(informationPrefab, transform);

                informationText.gameObject.SetActive(true);

                _informationTexts.Add(key, informationText);
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

        public void ClearInformationTexts()
        {
            foreach (var informationText in _informationTexts)
            {
                Destroy(informationText.Value.gameObject);
            }

            _informationTexts.Clear();
        }
    }
}