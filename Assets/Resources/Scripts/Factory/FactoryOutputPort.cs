using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal class FactoryOutputPort: FactoryPort<StorageOutput>
{
    internal void UnloadingResources(Stack<Resource> resources, Action unloadingComplete)
    {
       storage.onDecreaseCount.RemoveAllListeners();
        if (resources.Count == 0)
        {
            unloadingComplete.Invoke();
            return;
        }
        Resource resource = resources.Peek();
        if (storage.TryGetFreeLocalPosition(out Vector3 localPosition))
        {
            resource.transform.SetParent(storage.transform, true);
            MoveAnimationInfo animationInfo = new MoveAnimationInfo()
            {
                resource = resource,
                endLocalPos = localPosition,
                endLocalRotation = Quaternion.identity,
                duration = factory.unloadingAnimationLength,
                onCompleted = () => EndUnloadingResource(resources, unloadingComplete)
            };
            DoTweenMoveAnimation.Play(animationInfo);
        }
        else
        {
            Log.Instance.SendMessage(factory.name,"Storage is full");
            storage.onDecreaseCount.AddListener(() =>
            {
                Log.Instance.RemoveMessage(factory.name);
                UnloadingResources(resources, unloadingComplete);
            });
            return;
        }
    }
    internal void EndUnloadingResource(Stack<Resource> resources, Action unloadingComplete)
    {
        Resource resource = resources.Pop();
        storage.PushResource(resource);
        UnloadingResources(resources, unloadingComplete);
    }
}