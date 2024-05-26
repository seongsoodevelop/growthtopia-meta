using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageUIControl : MonoBehaviour
{
    [SerializeField] GameObject uiPanel;
    [SerializeField] TMP_Text messageText;

    public void SetActive(bool active)
    {
        uiPanel.gameObject.SetActive(active);
    }

    public void SetMessage(string message)
    {
        messageText.text = message;
    }
}
