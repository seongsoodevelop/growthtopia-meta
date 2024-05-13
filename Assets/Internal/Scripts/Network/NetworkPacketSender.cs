using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPacketSender : MonoBehaviour
{
    public static NetworkPacketSender inst { get; private set; }

    private void Awake()
    {
        inst = this;
    }

    public void SendAuthenticationPacket(string ticketToken)
    {
        NetworkManager.inst.Send($"authentication{Constants.PACKET_MESSAGE_DIVIDER}{ticketToken}");
    }

}
