using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIRanking : UIView
{
    public TextMeshProUGUI firstName;
    public TextMeshProUGUI firstScore;

    public TextMeshProUGUI secondName;
    public TextMeshProUGUI secondScore;

    public TextMeshProUGUI thirdName;
    public TextMeshProUGUI thirdScore;

    public void LoadRankingData()
    {
        firstScore.text = PlayerPrefs.GetInt("FirstScore").ToString();
        secondScore.text = PlayerPrefs.GetInt("SecondScore").ToString();
        thirdScore.text = PlayerPrefs.GetInt("ThirdScore").ToString();

        firstName.text = PlayerPrefs.GetString("FirstName");
        secondName.text = PlayerPrefs.GetString("SecondName");
        thirdName.text = PlayerPrefs.GetString("ThirdName");
    }
}
