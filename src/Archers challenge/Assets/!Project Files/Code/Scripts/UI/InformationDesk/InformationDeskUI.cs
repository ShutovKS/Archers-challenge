#region

using TMPro;
using UnityEngine;

#endregion

namespace UI.InformationDesk
{
    public class InformationDeskUI : BaseUI
    {
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private TMP_Text scoreText;

        public void SetTimeText(string time) => timeText.text = time;
        public void SetScoreText(string score) => scoreText.text = score;
    }
}