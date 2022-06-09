using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDataLoading : UIView
{
    [SerializeField]
    private TextMeshProUGUI text_info;
    private string str_info;

    public IEnumerator coMoveLoadingText;

    private void Start()
    {
        SetText("Loading");
    }
    public void SetText(string text)
    {
        text_info.text = text;
        str_info = text;
    }

    public void StartMoveLoadingText()
    {
        if (coMoveLoadingText != null) return;

        coMoveLoadingText = CoMoveLoadingText();
        StartCoroutine(coMoveLoadingText);

    }

    public void StopMoveLoadingText()
    {
        if (coMoveLoadingText != null)
        {
            StopCoroutine(coMoveLoadingText);
            coMoveLoadingText = null;
        }
    }
    private void OnEnable()
    {
        StartMoveLoadingText();
    }
    private void OnDisable()
    {
        StopMoveLoadingText();
    }

    private IEnumerator CoMoveLoadingText()
    {
        text_info.text = str_info;

        int dotCount = 0;
        string dot = string.Empty;
        WaitForSeconds waitForDelay = new WaitForSeconds(0.5f);

        while (true)
        {

            dot = dot.PadLeft(dotCount, '.');

            text_info.text = str_info + dot;

            dotCount += 1;
            dotCount %= 4;

            dot = string.Empty;
            yield return waitForDelay;
        }

    }
}
