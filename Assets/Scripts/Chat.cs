using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Collections;

public class Chat : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    private void Start()
    {
        SubmitMessageRPC("Welcome to the chat!");
    }

    private void OnSend()
    {
        FixedString128Bytes message = new ("Hello!");
        SubmitMessageRPC(message);
    }

    [Rpc(SendTo.Server)]
    public void SubmitMessageRPC(FixedString128Bytes message)
    {
        UpdateMessageRPC(message);
    }

    [Rpc(SendTo.Everyone)]
    public void UpdateMessageRPC(FixedString128Bytes message)
    {
        text.text = message.ToString();
    }
}
