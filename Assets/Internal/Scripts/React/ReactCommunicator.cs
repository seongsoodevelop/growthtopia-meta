using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ReactCommunicator : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void GetTicketTokenFromReact();

    [SerializeField] string testTicketToken;

    private void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        GetTicketTokenFromReact();
#else
        GetTicketToken(testTicketToken);
#endif
    }

    public void GetTicketToken(string ticketToken)
    {
        UserManager.inst.SetTicketToken(ticketToken);
    }

    public void OnQuit()
    {
        NetworkManager.inst.OnQuit();

        RoomManager.inst.OnQuit();
        UserManager.inst.OnRoomQuit();
    }
}
