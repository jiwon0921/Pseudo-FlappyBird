using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIResult : UIView
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI bestScore;

    public void UpdateScore()
    {
        score.text = GameManager.Instance.score.ToString();
    }

    public void UpdateBestScore()
    {
        //
    }
}
