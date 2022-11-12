using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal enum ResourceType
{
    First,
    Second,
    Third
}
[CreateAssetMenu(fileName = "newResourceData", menuName = "Create resource data", order = 2)]
internal class ResourceData : ScriptableObject
{
    [SerializeField]
    private ResourceType resourceType;
}
