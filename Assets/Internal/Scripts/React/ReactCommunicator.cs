using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ReactCommunicator : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void GetTicketTokenFromReact();

    
    private void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        GetTicketTokenFromReact();
#endif
    }

    public void GetTicketToken(string ticketToken)
    {
        Debug.Log(ticketToken);
        UserManager.inst.SetTicketToken(ticketToken);
    }
}
