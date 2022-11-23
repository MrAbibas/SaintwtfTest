using System;
using TMPro;
using UnityEngine;

[Serializable]
internal class ResourceCountView:MonoBehaviour
{
    [SerializeField]
    internal ResourceType resourceType;
    [SerializeField]
    private TMP_Text textValue;
    private int count;
    internal int Count
    {
        get => count;
        set
        {
            count = value;
            if (count == 0)
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);
            
            textValue.text = value.ToString();
        }
    }
}