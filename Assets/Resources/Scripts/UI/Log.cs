using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal struct MessageInfo
{
    internal string name;
    internal string value;
}
internal class Log : MonoBehaviour
{
    internal class MessageEntity
    {
        internal readonly int id;
        internal TMP_Text TMPText;
        internal MessageEntity(TMP_Text prefab,Transform parent, MessageInfo info)
        {
            TMPText = Instantiate(prefab, parent);
            id = TMPText.GetInstanceID();
            TMPText.text = $"{info.name}: {info.value}";
        }
    }
    private static Log _instance;
    internal static Log Instance => _instance;
    [SerializeField]
    private Transform content;
    [SerializeField]
    private TMP_Text messagePrefab;
    private List<MessageEntity> messages = new List<MessageEntity>();
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    internal void SendMessage(MessageInfo messageInfo, out int messageID)
    {
        MessageEntity entity = new MessageEntity(messagePrefab, content, messageInfo);
        messages.Add(entity);
        messageID = entity.id;
    }
    private bool Contains(int id, out MessageEntity entity)
    {
        entity = null;
        for (int i = 0; i < messages.Count; i++)
        {
            if (messages[i].id == id)
            {
                entity = messages[i];
                return true;
            }
        }
        return false;
    }
    internal void RemoveMessage(int id)
    {
        if (Contains(id, out MessageEntity entity))
        {
            Destroy(entity.TMPText.gameObject);
            messages.Remove(entity);
        }
    }
}