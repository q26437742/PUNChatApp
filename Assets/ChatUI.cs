using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Chat;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class ChatUI : MonoBehaviour, IChatClientListener
{
    public string userName;
    public GameObject loginPanel;
    public ChatClient chatClient;
    public GameObject connectingLabel;
    protected internal AppSettings appSetting;

    public GameObject chatPanel;
    public Text messageText;
    public InputField messageInputField;
    public Text userList;
    public string[] channelsToAutoJoin;
    public int historyLength = 10;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        appSetting = PhotonNetwork.PhotonServerSettings.AppSettings;
        bool appIdPreset = !string.IsNullOrEmpty(this.appSetting.AppIdChat);
        loginPanel.gameObject.SetActive(appIdPreset);


        if (!appIdPreset)
        {
            Debug.LogError("Chat Id is missing in setting");
        }
        connectingLabel.SetActive(false);
        chatPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (chatClient != null)
        {
            chatClient.Service();
        }
    }

    public void Connect()
    {
        loginPanel.gameObject.SetActive(false);
        connectingLabel.gameObject.SetActive(true);

        chatClient = new ChatClient(this);
        chatClient.UseBackgroundWorkerForSending = true;

        //userName = 
        chatClient.Connect(appSetting.AppIdChat, "1.0", new Photon.Chat.AuthenticationValues(this.userName));

        Debug.Log("Connecting as " + userName);
    }
    public void SendChatMessage(string message)
    {
        chatClient.PublishMessage(this.channelsToAutoJoin[0], message);
        //Debug.Log(channelsToAutoJoin[0]);
        messageInputField.text = "";
        messageInputField.ActivateInputField();
        messageInputField.Select();
        //Debug.Log("Send " + message);
    }
    public void ChangeUserName(string na)
    {
        userName = na;
    }
    public void DebugReturn(DebugLevel level, string message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        //throw new System.NotImplementedException();
    }

    public void OnConnected()
    {
        Debug.Log("Connected to Server");
        connectingLabel.gameObject.SetActive(false);
        chatPanel.SetActive(true);
        chatClient.Subscribe(channelsToAutoJoin[0], 0, historyLength, creationOptions: new ChannelCreationOptions { PublishSubscribers = true });
    }

    public void OnDisconnected()
    {
        //throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        //Debug.Log("Get message.");
        ChatChannel channel = null;
        chatClient.TryGetChannel(channelsToAutoJoin[0], out channel);
        
        messageText.text = channel.ToStringMessages();
        //throw new System.NotImplementedException();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log("xxx");
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        //throw new System.NotImplementedException();
    }
    public void PrintSubscribeUsers(string channelName)
    {
        Debug.Log("print users");
        ChatChannel channel = null;
        chatClient.TryGetChannel(channelName, out channel);
        userList.text = "";
        if (!channel.Subscribers.Contains(this.chatClient.UserId))
            this.userList.text += this.chatClient.UserId + "\n";

        foreach (var u in channel.Subscribers)
        {
            userList.text += u + "\n";
        }
    }
    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("On Subscribed");
        chatClient.PublishMessage(channels[0], "has joined the chat");

        PrintSubscribeUsers(channels[0]);
    }

    public void OnUnsubscribed(string[] channels)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        
        Debug.Log("On User Subscribed");
        PrintSubscribeUsers(channel);
        //throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }


}
