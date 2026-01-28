using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreBoardUI : MonoBehaviour
{
    [Header("UI Slots (Top 8)")]
    public TextMeshProUGUI[] nameTexts;
    public TextMeshProUGUI[] scoreTexts;

    void OnEnable()
    {
        FireBaseManager.Instance.OnScoreboardUpdated += UpdateUI;
        FireBaseManager.Instance.StartListeningScoreboard();
    }

    void OnDisable()
    {
        FireBaseManager.Instance.OnScoreboardUpdated -= UpdateUI;
    }

    void UpdateUI(List<UserData> users)
    {
        for (int i = 0; i < nameTexts.Length; i++)
        {
            if (i < users.Count)
            {
                nameTexts[i].text = users[i].Name;
                scoreTexts[i].text = users[i].Score.ToString();
            }
            else
            {
                nameTexts[i].text = "-";
                scoreTexts[i].text = "-";
            }
        }
    }
}
