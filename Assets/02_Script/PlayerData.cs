using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    public string DT_playerName;
    public int DT_playerBestScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void UpdateRanking()
    {
        Debug.Log("updateScore");

        int currentScore = GameManager.Instance.GetScore();
        string currentName = GameManager.Instance.GetPlayerName();

        if(currentScore < PlayerPrefs.GetInt("FirstScore"))
        {
            if (currentScore < PlayerPrefs.GetInt("SecondScore"))
            {
                if (currentScore < PlayerPrefs.GetInt("ThirdScore"))
                {
                    return;
                }

                // 3µî
                PlayerPrefs.SetInt("ThirdScore", currentScore);
                PlayerPrefs.SetString("ThirdName", currentName);
                return;
            }

            // 2µî
            PlayerPrefs.SetInt("ThirdScore", PlayerPrefs.GetInt("SecondScore"));
            PlayerPrefs.SetString("ThirdName", PlayerPrefs.GetString("SecondName"));
            PlayerPrefs.SetInt("SecondScore", currentScore);
            PlayerPrefs.SetString("SecondName", currentName);
            return;
        }

        if (currentScore == PlayerPrefs.GetInt("FirstScore")) return;

        // 1µî
        PlayerPrefs.SetInt("ThirdScore", PlayerPrefs.GetInt("SecondScore"));
        PlayerPrefs.SetString("ThirdName", PlayerPrefs.GetString("SecondName"));
        PlayerPrefs.SetInt("SecondScore", PlayerPrefs.GetInt("FirstScore"));
        PlayerPrefs.SetString("SecondName", PlayerPrefs.GetString("FirstName"));
        PlayerPrefs.SetInt("FirstScore", currentScore);
        PlayerPrefs.SetString("FirstName", currentName);
    }

    public void SetName()
    {
        DT_playerName = GameManager.Instance.GetPlayerName();
    }

    public void SetBestScore()
    {
        DT_playerBestScore = GameManager.Instance.GetBestScore();
    }
}
