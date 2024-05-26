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

    public void SendRoomPlayerDataPacket(Vector3 position)
    {
        NetworkManager.inst.Send($"room:playerdata{Constants.PACKET_MESSAGE_DIVIDER}{position.x}{Constants.PACKET_MESSAGE_DIVIDER}{position.y}{Constants.PACKET_MESSAGE_DIVIDER}{position.z}");
    }

    public void SendRoomChatPacket(string message)
    {
        NetworkManager.inst.Send($"room:chat{Constants.PACKET_MESSAGE_DIVIDER}{message}");
    }

}
