using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public static UserManager inst { get; private set; }

    private string ticketToken = "";
    private string authenticationMessage = "";
    private bool isAuthenticated = false;

    int user_no = -1;
    string nickname = "noname";

    [SerializeField] GameObject connectPanel;
    [SerializeField] TMP_Text authenticationMessageText;

    private void Awake()
    {
        inst = this;
        authenticationMessageText.text = authenticationMessage;
    }

    public void Authentication()
    {
        if (isAuthenticated)
        {
            return;
        }

        NetworkPacketSender.inst.SendAuthenticationPacket(ticketToken);
    }

    public void SetAuthenticationMessage(string message)
    {
        authenticationMessage = message;
        authenticationMessageText.text = authenticationMessage;
    }

    public void OnAuthenticated(int user_no, string nickname)
    {
        isAuthenticated = true;
        this.user_no = user_no;
        this.nickname = nickname;
        connectPanel.SetActive(false);
    }

    public void SetTicketToken(string ticketToken)
    {
        this.ticketToken = ticketToken;
        authenticationMessageText.text = ticketToken;
    }
}
