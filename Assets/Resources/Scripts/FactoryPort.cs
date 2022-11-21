using System;
using UnityEngine;

[Serializable]
internal class FactoryPort<T> where T : Storage
{
    [SerializeField]
    internal ResourceType type;
    [SerializeField]
    internal int count;
    [SerializeField]
    protected T storage;
    [SerializeField]
    internal Transform endPoint;
    internal Factory factory;
}