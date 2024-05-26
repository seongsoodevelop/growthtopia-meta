using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using static NetworkPacketReceiver;

public class RoomManager : MonoBehaviour
{
    public static RoomManager inst { get; private set; }

    public static int roomId { get; private set; } = -1;
    public static bool onRoom { get; private set; } = false;


    private void Awake()
    {
        inst = this;
    }

    public void OnJoin(int roomId, List<PropertyElement> elements)
    {
        onRoom = true;
        RoomManager.roomId = roomId;

        UserUI.inst.OnRoomJoin();
        RoomElementManager.inst.Init(elements);
    }

    public void OnQuit()
    {
        onRoom = false;
        roomId = -1;

        UserUI.inst.OnRoomQuit();
        RoomElementManager.inst.OnRoomQuit();
    }
}
