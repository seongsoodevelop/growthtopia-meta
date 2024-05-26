using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using Unity.VisualScripting;

public class NetworkManager : MonoBehaviour {
    public static NetworkManager inst { get; private set; }

    WebSocket socket = null;

    bool isProduction = true;
    const string hostDevelop = "ws://localhost:7000";
    const string hostProduction = "wss://meta.growthtopia.net:7000";

    [SerializeField] MessageUIControl messageUIControl;

    [SerializeField] UnityEvent onConnectedEvent;

    private void Awake()
    {
        inst = this;
        messageUIControl.SetActive(true);
        messageUIControl.SetMessage("서버에 접속하는 중입니다");
    }

    IEnumerator Start () {
        socket = new WebSocket(new Uri(isProduction ? hostProduction : hostDevelop));
        yield return StartCoroutine(socket.Connect());

        Debug.Log("WEBSOCKET CONNECTED");
        messageUIControl.SetActive(false);
        messageUIControl.SetMessage("서버 접속에 성공했습니다");

        if (onConnectedEvent != null)
        {
            onConnectedEvent.Invoke();
        }

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
                Debug.Log($"WEBSOCKET ERROR: {socket.error}");
                break;
            }

            yield return 0;
        }

        if (socket != null)
        {
            OnQuit();
        }

        Debug.Log("WEBSOCKET CLOSED");
        messageUIControl.SetActive(true);
        messageUIControl.SetMessage("서버와의 연결이 끊어졌습니다");
    }

    public void Send(string message)
    {
        if (socket == null)
        {
            return;
        }

        socket.SendString(message);
    }

    private void OnApplicationQuit()
    {
        OnQuit();
    }

    public void OnQuit()
    {
        if (socket != null)
        {
            socket.Close();
            socket = null;
        }
    }
}
