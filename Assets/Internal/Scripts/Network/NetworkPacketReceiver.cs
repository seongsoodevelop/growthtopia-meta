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
                    UserManager.inst.SetAuthenticationMessage("접속에 실패했습니다. 새로고침 후 다시 시도해 주세요.");
                    break;
                }
            case "authentication:accepted":
                {
                    UserManager.inst.SetAuthenticationMessage("접속에 성공했습니다!");
                    PacketAuthenticationAccepted data = JsonUtility.FromJson<PacketAuthenticationAccepted>(packet.body);
                    UserManager.inst.OnAuthenticated(data.user_no, data.nickname);
                    break;
                }
        }
    }

    public void EnqueuePacket(string header,string body)
    {
        packets.Enqueue(new Packet(header, body));
    }
}
