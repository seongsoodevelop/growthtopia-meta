using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UserManager : MonoBehaviour
{
    public static UserManager inst { get; private set; }

    private string ticketToken = "";
    private bool isAuthenticated = false;

    public int user_no { get; private set; } = -1;
    public string nickname { get; private set; } = "noname";

    [SerializeField] MessageUIControl connectionMessageUIControl;

    [SerializeField]
    PlayerControl playerPrefab;
    PlayerControl player = null;

    [SerializeField]
    NetworkPlayerControl networkPlayerPrefab;
    Dictionary<int, NetworkPlayerControl> networkPlayers = new Dictionary<int, NetworkPlayerControl>();

    [SerializeField]
    TMP_InputField chatInputField;
    [SerializeField]
    RectTransform chatContentPanel;
    [SerializeField]
    TMP_Text chatPrefab;

    private void Awake()
    {
        inst = this;
        connectionMessageUIControl.SetActive(true);

        chatInputField.onSubmit.AddListener((string x) => {
            if (chatInputField.text != "")
            {
                SendChat();
            }
            else
            {
                chatInputField.DeactivateInputField();
            }
        });
    }

    private void Update()
    {
        if (chatInputField.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                chatInputField.DeactivateInputField();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                chatInputField.ActivateInputField();
            }
        }
    }

    public void Authentication()
    {
        if (isAuthenticated)
        {
            return;
        }

        connectionMessageUIControl.SetMessage("로그인을 시도하는 중입니다");
        NetworkPacketSender.inst.SendAuthenticationPacket(ticketToken);
    }

    public void OnAuthenticated(int user_no, string nickname)
    {
        isAuthenticated = true;
        this.user_no = user_no;
        this.nickname = nickname;

        connectionMessageUIControl.SetActive(false); 
        connectionMessageUIControl.SetMessage("로그인에 성공했습니다");

        UserUI.inst.OnAuthenticated();
    }

    public void OnAuthenticationFailed()
    {
        connectionMessageUIControl.SetMessage("로그인에 실패했습니다\n다시 시도해 주세요");
    }

    public void SetTicketToken(string ticketToken)
    {
        this.ticketToken = ticketToken;
    }

    public void AddUser(int user_no, Vector3 position, string nickname)
    {
        if (user_no == this.user_no)
        {
            player = Instantiate(playerPrefab.gameObject, transform).GetComponent<PlayerControl>();
            player.Init(nickname);
            player.transform.name = $"{user_no} {nickname}";
            player.transform.position = position;
        }
        else
        {
            NetworkPlayerControl player = Instantiate(networkPlayerPrefab.gameObject, transform).GetComponent<NetworkPlayerControl>();
            player.Init(nickname);
            player.transform.name = $"{user_no} {nickname}";
            player.UpdateTransform(position);
            networkPlayers.Add(user_no, player);    
        }
    }

    public void UpdatePlayerData(int user_no, Vector3 position)
    {
        if (networkPlayers.TryGetValue(user_no, out NetworkPlayerControl networkPlayer))
        {
            networkPlayer.UpdateTransform(position);
        }
    }

    public void OnChat(int user_no, string message)
    {
        if (networkPlayers.TryGetValue(user_no, out NetworkPlayerControl networkPlayer))
        {
            TMP_Text chatInst = Instantiate(chatPrefab.gameObject, chatContentPanel).GetComponent<TMP_Text>();
            chatInst.text = $"{networkPlayer.nickname}: {message}";
            LayoutRebuilder.ForceRebuildLayoutImmediate(chatContentPanel);
        }
        else if (user_no == this.user_no)
        {
            TMP_Text chatInst = Instantiate(chatPrefab.gameObject, chatContentPanel).GetComponent<TMP_Text>();
            chatInst.text = $"{nickname}: {message}";
            LayoutRebuilder.ForceRebuildLayoutImmediate(chatContentPanel);
        }
    }


    public void RemoveUser(int user_no)
    {
        if (networkPlayers.TryGetValue(user_no, out NetworkPlayerControl networkPlayer))
        {
            networkPlayers.Remove(user_no);
            Destroy(networkPlayer.gameObject);
        }
    }

    public void OnRoomQuit()
    {
        if (player != null)
        {
            Destroy(player.gameObject);
            player = null;
        }

        foreach(KeyValuePair<int, NetworkPlayerControl> o in networkPlayers)
        {
            Destroy(o.Value.gameObject);
        }
        networkPlayers.Clear();
    }

    public void SendChat()
    {
        NetworkPacketSender.inst.SendRoomChatPacket(chatInputField.text);
        chatInputField.text = "";

        chatInputField.ActivateInputField();
    }
}
