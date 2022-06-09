using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using PFB.Database;
using System.Threading.Tasks;

public class UIReady : UIView
{
    public Button startButton;

    public GameObject namePanel;
    public TMP_InputField nameInputField;

    public TextMeshProUGUI text_info;


    public bool IsActiveStartButton { get; private set; }
    public bool IsAllowedName { get; private set; }

    public bool IsDataLoading { get; private set; }

    private void Start()
    {
        IsAllowedName = false;

        nameInputField.Select();

        nameInputField.onSubmit.AddListener(delegate { EnterName(); });
    }

    public override void Toggle(bool value)
    {
        base.Toggle(value);

        OnInputValueChanged();

        if (value == true)
        {
            text_info.text = "hello!";
            nameInputField.interactable = true;
            IsActiveStartButton = false;
        }
    }
    public void EnterName()
    {
        OnInputValueChanged();
        //조건에 안맞으면 나가리
        if (!IsAllowedName)
        {
            return;
        }

        //이미 스타트 버튼이 켜져있는 상태라면 아무것도 안함
        if (IsActiveStartButton)
        {
            return;
        }


        if (IsDataLoading)
        {
            return;
        }


        StartCoroutine(TryCreateUser(nameInputField.text));
    }


    private IEnumerator TryCreateUser(string userName)
    {

        IsDataLoading = true;


        if (GameManager.Instance.currentUserData.userName != userName)
        {
            UIManager.Instance.uiDataLoading.Toggle(true);

            nameInputField.interactable = false;

            //한 프레임 기다리기
            yield return null;

            Task request = PFBDBManager.Instance.request.GetUserDataByNameAsyncSafe(userName,
                (data) =>
                {
                    //유저 데이터 설정
                    GameManager.Instance.currentUserData = data;

                });

            yield return new WaitUntil(() => request.IsCompleted);

            UIManager.Instance.uiDataLoading.Toggle(false);

        }

        namePanel.SetActive(false);

       // GameManager.Instance.currentUserData.userName = nameInputField.text.ToString();
       // PlayerData.Instance.SetName();

        IsDataLoading = false;
        OnStartButton();
    }

    public void OffStartButton()
    {
        IsActiveStartButton = false;
        IsAllowedName = false;
        startButton.interactable = false;

        nameInputField.interactable = true;
    }

    public void OnStartButton()
    {
        IsActiveStartButton = true;
        startButton.interactable = true;
    }


    public void OnNameOkButtonClick()
    {
        EnterName();
    }


    public void OnInputValueChanged()
    {
        if (string.IsNullOrWhiteSpace(nameInputField.text) || string.IsNullOrEmpty(nameInputField.text) || nameInputField.text.Length < 1)
        {
            text_info.text = "omg...";
            IsAllowedName = false;
            return;
        }


        string pattern = @"^[a-zA-Z0-9]*$";
        //조건에 안맞으면 나가리
        if (!Regex.IsMatch(nameInputField.text, pattern))
        {
            text_info.text = "you can use only English alphabet.";
            IsAllowedName = false;
        }
        else
        {
            text_info.text = GetRandomGoodText();
            IsAllowedName = true;
        }
    }


    private readonly string[] randomGoodText = new string[] { "nice name!", "cool name!", "handsome name!", "amazing name!", "good name!" };
    private string GetRandomGoodText()
    {
        int random = UnityEngine.Random.Range(0, 5);

        return randomGoodText[random];
    }
}
