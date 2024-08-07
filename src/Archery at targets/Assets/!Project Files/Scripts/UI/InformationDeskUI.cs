using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class InformationDeskUI : MonoBehaviour
    {
        [SerializeField] private GameObject informationPrefab;

        private readonly Dictionary<string, TMP_Text> _informationTexts = new();

        public void SetInformationText(string key, string value)
        {
            if (!_informationTexts.ContainsKey(key))
            {
                Debug.Log($"Creating new information text: {key}");
                var informationText = Instantiate(informationPrefab, transform);
                informationText.SetActive(true);
                informationText.name = value;
                _informationTexts.Add(key, informationText.GetComponent<TMP_Text>());
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