using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal struct MessageInfo
{
    internal string name;
    internal TMP_Text text;
}
internal class Log : MonoBehaviour
{
    private static Log _instance;
    internal static Log Instance => _instance;
    [SerializeField]
    private Transform content;
    [SerializeField]
    private TMP_Text messagePrefab;
    private List<MessageInfo> messages = new List<MessageInfo>();
    private void Awake()
    {
        if(_instance == null)
            _instance = this;
    }
    internal void SendMessage(string name, string value)
    {
        if(Contains(name,out MessageInfo messageInfo))
        {
            messageInfo.text.text = $"{name}: {value}";
            return;
        }
        MessageInfo info = new MessageInfo()
        {
            name = name,
            text = Instantiate(messagePrefab, content)
        };
        info.text.text = $"{name}: {value}";
        messages.Add(info);
    }
    internal bool Contains(string name, out MessageInfo messageInfo)
    {
        messageInfo = new MessageInfo();
        for (int i = 0; i < messages.Count; i++)
        {
            if (messages[i].name == name)
            {
                messageInfo = messages[i];
                return true;
            }
        }
        return false;
    }
    internal void RemoveMessage(string name)
    {
        if(Contains(name,out MessageInfo message))
        {
            Destroy(message.text.gameObject);
            messages.Remove(message);
        }
    }
}