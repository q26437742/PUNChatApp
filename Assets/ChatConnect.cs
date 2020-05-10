using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatConnect : MonoBehaviour
{
    public InputField chatName;
    // Start is called before the first frame update
    public void ConnectToChat()
    {
        ChatUI chatUI = FindObjectOfType<ChatUI>();
        chatUI.userName = this.chatName.text.Trim();
        if (!string.IsNullOrEmpty(chatUI.userName))
        {
            chatUI.Connect();
            enabled = false;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
