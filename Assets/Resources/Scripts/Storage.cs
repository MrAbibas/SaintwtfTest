using System.Collections.Generic;
using UnityEngine;

internal class Storage : MonoBehaviour
{
    [SerializeField]
    private ResourceData resourceData;
    [SerializeField]
    private Transform floor;
    [SerializeField]
    private Vector3 spacing;
    private Vector3[] _resourcePositions;
    private Stack<Resource> _stack;

    internal ResourceType ResourceType => resourceData.ResourceType;
    internal int ResourceCount => _stack.Count;
    internal int maxResourceCount { get; private set; }

    private void Start()
    {
        _stack = new Stack<Resource>();
        SetResourcePositions();
    }
    private void SetResourcePositions()
    {
        Vector3 resourceSize = resourceData.Size + spacing;
        Vector3Int size = new Vector3Int
        {
            x = (int)(floor.localScale.x / resourceSize.x),
            y = (int)floor.localScale.y,
            z = (int)(floor.localScale.z / resourceSize.z)
        };
        maxResourceCount = size.x * size.z;
        _resourcePositions = new Vector3[maxResourceCount];
        Vector3 resourcePosOffset = floor.localScale / 2;
        resourcePosOffset.y = floor.position.y;
        for (int i = 0; i < maxResourceCount; i++)
            _resourcePositions[i] = new Vector3()
            {
                x = resourceSize.x / 2 + i / size.z * resourceSize.x,
                y = resourceSize.y / 2,
                z = resourceSize.z / 2 + i % size.z * resourceSize.z,
            } - resourcePosOffset;
    }
    internal void PushResource(Resource resource)
    {
        if (resource.ResourceData.ResourceType != resourceData.ResourceType)
            return;

        resource.transform.SetParent(transform, true);
        resource.transform.localPosition = _resourcePositions[_stack.Count];
        _stack.Push(resource);
    }
    internal Resource PopResource()
    {
        if (_stack.Count > 0)
            return _stack.Pop();
        else
            return null;
    }
}