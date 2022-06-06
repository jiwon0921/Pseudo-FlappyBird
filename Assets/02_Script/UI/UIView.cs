using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIView : MonoBehaviour
{
    public RectTransform rootRect;
    private Vector3 originalPos;

    private void Awake()
    {
        originalPos = rootRect.localPosition;
    }

    public virtual void Toggle(bool value)
    {
        if (value)
        {
            rootRect.localPosition = Vector3.zero;
        }
        else
        {
            rootRect.localPosition = originalPos;
        }

        rootRect.gameObject.SetActive(value);
    }
}
