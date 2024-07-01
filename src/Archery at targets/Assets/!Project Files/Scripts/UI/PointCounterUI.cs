using TMPro;
using UnityEngine;

namespace UI
{
    public class PointCounterUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text pointCounterText;

        public void SetPointCounter(string value)
        {
            pointCounterText.text = value;
        }
    }
}