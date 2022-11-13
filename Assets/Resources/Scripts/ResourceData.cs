using System;
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
    internal ResourceType ResourceType { get => resourceType; set => resourceType = value; }
    [SerializeField]
    private Material material;
    internal Material Material { get => material; set => material = value; }
    [SerializeField]
    private Vector3 size;
    internal Vector3 Size { get => size; set => size = value; }
}
