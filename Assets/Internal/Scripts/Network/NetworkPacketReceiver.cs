using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPacketReceiver : MonoBehaviour
{
    public struct Packet
    {
        public string header;
        public string body;
        public Packet(string header, string body)
        {
            this.header = header;   
            this.body = body;   
        }
    }

    public class PacketAuthenticationAccepted
    {
        public int user_no;
        public string nickname;
    }

    [Serializable]
    public class PropertyElement
    {
        public int id;
        public int type;
        public int code;
        public float position_x;
        public float position_y;
        public float position_z;
    }

    [Serializable]
    public class PropertyData
    {
        public List<PropertyElement> elements;
        public List<float> spawnPoint;
        public int nextElementId;
    }

    [Serializable]
    public class PacketRoomJoin
    {
        public int roomId;
        public PropertyData data;
    }

    [Serializable]
    public class PacketRoomAddUser
    {
        public int user_no;
        public string nickname;
        public float position_x;
        public float position_y;
        public float position_z;
    }

    [Serializable]
    public class PacketRoomRemoveUser
    {
        public int user_no;
    }

    [Serializable]
    public class PacketRoomPlayerData
    {
        public int user_no;
        public float position_x;
        public float position_y;
        public float position_z;
    }

    [Serializable]
    public class PacketRoomChat
    {
        public int user_no;
        public string message;
    }

    public static NetworkPacketReceiver inst {  get; private set; }

    Queue<Packet> packets = new Queue<Packet>();

    private void Awake()
    {
        inst = this;
    }

    private void Update()
    {
        if (packets.Count == 0)
        {
            return;
        }
        Packet packet = packets.Dequeue();
        switch(packet.header)
        {
            case "welcome":
                {
                    Debug.Log("WELCOME");
                    break;
                }
            case "authentication:rejected":
                {
                    UserManager.inst.OnAuthenticationFailed();
                    break;
                }
            case "authentication:accepted":
                {
                    PacketAuthenticationAccepted bodyData = JsonUtility.FromJson<PacketAuthenticationAccepted>(packet.body);
                    UserManager.inst.OnAuthenticated(bodyData.user_no, bodyData.nickname);
                    break;
                }
            case "room:join":
                {
                    PacketRoomJoin bodyData = JsonUtility.FromJson<PacketRoomJoin>(packet.body);
                    RoomManager.inst.OnJoin(bodyData.roomId, bodyData.data.elements);
                    break;
                }
            case "room:quit":
                {
                    RoomManager.inst.OnQuit();
                    UserManager.inst.OnRoomQuit();
                    break;
                }
            case "room:adduser":
                {
                    PacketRoomAddUser bodyData = JsonUtility.FromJson<PacketRoomAddUser>(packet.body);
                    UserManager.inst.AddUser(bodyData.user_no, new Vector3(bodyData.position_x, bodyData.position_y, bodyData.position_z), bodyData.nickname);
                    break;
                }
            case "room:removeuser":
                {
                    PacketRoomRemoveUser bodyData = JsonUtility.FromJson<PacketRoomRemoveUser>(packet.body);
                    UserManager.inst.RemoveUser(bodyData.user_no);
                    break;
                }
            case "room:playerdata":
                {
                    PacketRoomPlayerData bodyData = JsonUtility.FromJson<PacketRoomPlayerData>(packet.body);
                    UserManager.inst.UpdatePlayerData(bodyData.user_no, new Vector3(bodyData.position_x, bodyData.position_y, bodyData.position_z));
                    break;
                }
            case "room:chat":
                {
                    PacketRoomChat bodyData = JsonUtility.FromJson<PacketRoomChat>(packet.body);
                    UserManager.inst.OnChat(bodyData.user_no, bodyData.message);
                    break;
                }
        }
    }

    public void EnqueuePacket(string header,string body)
    {
        packets.Enqueue(new Packet(header, body));
    }
}
