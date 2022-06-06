using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIInGame : UIView
{
    public TextMeshProUGUI score;
    public Animator flash;

    public void UpdateScore()
    {
        score.text = GameManager.Instance.score.ToString();
    }
}
