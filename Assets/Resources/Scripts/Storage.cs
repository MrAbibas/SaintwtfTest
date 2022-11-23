using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

internal class Storage : MonoBehaviour
{
    [SerializeField]
    internal ResourceData resourceData;
    [SerializeField]
    private Transform floor;
    [SerializeField]
    private Vector3 spacing;
    private Vector3[] _resourcePositions;
    private Stack<Resource> _stack;
    internal UnityEvent onIncreaseCount;
    internal UnityEvent onDecreaseCount;

    internal ResourceType ResourceType => resourceData.ResourceType;
    internal int ResourceCount => _stack.Count;
    internal int MaxResourceCount { get; private set; }

    private void Awake()
    {
        _stack = new Stack<Resource>();
        SetResourcePositions();
        onIncreaseCount = new UnityEvent();
        onDecreaseCount = new UnityEvent();
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
        MaxResourceCount = size.x * size.z;
        _resourcePositions = new Vector3[MaxResourceCount];
        Vector3 resourcePosOffset = floor.localScale / 2;
        resourcePosOffset.y = floor.position.y;
        for (int i = 0; i < MaxResourceCount; i++)
            _resourcePositions[i] = new Vector3()
            {
                x = resourceSize.x / 2 + i / size.z * resourceSize.x,
                y = resourceSize.y / 2,
                z = resourceSize.z / 2 + i % size.z * resourceSize.z,
            } - resourcePosOffset;
    }
    internal bool TryGetFreeLocalPosition(out Vector3 position)
    {
        position = Vector3.zero;
        if (_stack.Count >= MaxResourceCount)
            return false;

        position = _resourcePositions[_stack.Count];
        return true;
    }
    internal bool PushResource(Resource resource)
    {
        if (resource.ResourceData.ResourceType != resourceData.ResourceType || _stack.Count >= MaxResourceCount)
            return false;

        resource.transform.SetParent(transform, true);
        _stack.Push(resource);
        CheckPositions();
        onIncreaseCount?.Invoke();
        return true;
    }
    internal Resource PopResource()
    {
        if (_stack.Count == 0)
            return null;

        Resource value = _stack.Pop();
        CheckPositions();
        onDecreaseCount?.Invoke();
        return value;
    }
    private void CheckPositions()
    {
        Resource[] resources = _stack.ToArray();
        for (int i = 0; i < resources.Length; i++)
            if (resources[i].transform.localPosition != _resourcePositions[i])
                resources[i].transform.localPosition = _resourcePositions[i];
    }
}