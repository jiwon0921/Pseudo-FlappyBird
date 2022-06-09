using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public UIInGame uiInGame;
    public UIReady uiReady;
    public UIResult uiResult;
    public UIRanking uiRanking;
    public UIDataLoading uiDataLoading;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }


        uiRanking.Init();
        //GameObject.DontDestroyOnLoad(this.gameObject);
    }
}
