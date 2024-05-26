using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserUI : MonoBehaviour
{
    public static UserUI inst { get; private set; }

    [SerializeField] GameObject uiPanel;
    [SerializeField] TMP_Text nicknameText;

    private void Awake()
    {
        inst = this;
    }

    public void OnAuthenticated()
    {
        nicknameText.text = $"{UserManager.inst.nickname}님 환영합니다 :D";
    }

    public void OnRoomJoin()
    {
        uiPanel.SetActive(false);
    }

    public void OnRoomQuit()
    {
        uiPanel.SetActive(true);
    }
}
