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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        //GameObject.DontDestroyOnLoad(this.gameObject);
    }
}
