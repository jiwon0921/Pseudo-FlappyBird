using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIReady : UIView
{
    public Button startButton;

    public GameObject namePanel;
    public TMP_InputField nameInputField;

    private void Start()
    {
        nameInputField.Select();

        nameInputField.onSubmit.AddListener(delegate { EnterName(); });
    }

    public void EnterName()
    {
        namePanel.SetActive(false);

        GameManager.Instance.playerName = nameInputField.text.ToString();
        PlayerData.Instance.SetName();

        OnStartButton();
    }

    public void OffStartButton()
    {
        startButton.interactable = false;
    }

    public void OnStartButton()
    {
        startButton.interactable = true;
    }
}
