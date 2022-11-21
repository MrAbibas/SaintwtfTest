using System;
using UnityEngine;

[Serializable]
internal class FactoryInputPort: FactoryPort<StorageInput>
{
    internal int curentCount { get; private set; }
    internal void ResetCurentCount() => curentCount = 0;
    internal void LoadResources(Action loadingComplete)
    {
        if (curentCount == count)
        {
            loadingComplete.Invoke();
            return;
        }
        storage.onIncreaseCount.RemoveAllListeners();
        Resource resource = storage.PopResource();
        if (resource != null)
        {
            resource.transform.SetParent(factory.transform, true);
            MoveAnimationInfo animationInfo = new MoveAnimationInfo()
            {
                resource = resource,
                endLocalPos = endPoint.transform.localPosition,
                endLocalRotation = Quaternion.identity,
                duration = factory.loadingAnimationLength,
                onCompleted = () => EndLoadingResource(resource, loadingComplete)
            };
            DoTweenMoveAnimation.Play(animationInfo);
        }
        else
        {
            storage.onIncreaseCount.AddListener(() => LoadResources(loadingComplete));
        }
    }
    private void EndLoadingResource(Resource resource, Action loadingComplete)
    {
        ResourcePool.Instance.Push(resource);
        curentCount++;
        LoadResources(loadingComplete);
    }
}