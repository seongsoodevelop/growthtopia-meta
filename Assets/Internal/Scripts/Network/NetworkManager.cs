using UnityEngine;
using System.Collections;
using System;

public class NetworkManager : MonoBehaviour {
    public static NetworkManager inst { get; private set; }

    WebSocket socket = null;

    bool isProduction = false;
    const string hostDevelop = "ws://localhost:7000";
    const string hostProduction = "ws://meta.growthtopia.net";

    private void Awake()
    {
        inst = this;
    }

    IEnumerator Start () {

        socket = new WebSocket(new Uri(isProduction ? hostProduction : hostDevelop));
        yield return StartCoroutine(socket.Connect());
        Debug.Log("WEBSOCKET CONNECTED");

        while (true)
        {
            string message = socket.RecvString();

            if (message != null)
            {
                string[] data = message.Split(Constants.PACKET_MESSAGE_DIVIDER);
                string header = data[0];
                string body = data[1];
                Debug.Log($"WEBSOCKET MESSAGE: \n{header}\n{body}");

                NetworkPacketReceiver.inst.EnqueuePacket(header, body);
            }

            if (socket.error != null)
            {
                Debug.LogError($"WEBSOCKET ERROR: {socket.error}");
                break;
            }

            yield return 0;
        }

        if (socket != null)
        {
            socket.Close();
            socket = null;
        }

        Debug.Log("WEBSOCKET CLOSED");
    }

    public void Send(string message)
    {
        if (socket == null)
        {
            return;
        }

        socket.SendString(message);
    }

    private void OnDestroy()
    {
        if (socket != null)
        {
            socket.Close();
            socket = null;
        }
    }
}
