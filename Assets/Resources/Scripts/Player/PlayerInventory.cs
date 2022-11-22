using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private int maxHeight;
    [SerializeField]
    private float loadingAnimationLength;
    [SerializeField]
    private float unloadingAnimationLength;
    private List<Resource> resources;
    private float curentHeight;
    internal bool isAnimationPlaying;
    private void Start()
    {
        resources = new List<Resource>();
        curentHeight = 0;
    }
    internal void PushResource(StorageOutput storage)
    {
        if (curentHeight + storage.resourceData.Size.y > maxHeight)
        {
            return;
        }
        Resource resource = storage.PopResource();
        if (resource == null)
            return;

        resources.Add(resource);
        resource.collider.enabled = false;
        resource.transform.SetParent(transform, true);
        Vector3 endPos = (curentHeight + 0.5f * resource.ResourceData.Size.y) * Vector3.up;
        isAnimationPlaying = true;
        MoveAnimationInfo animationInfo = new MoveAnimationInfo()
        {
            resource = resource,
            endLocalPos = endPos,
            endLocalRotation = Quaternion.identity,
            duration = loadingAnimationLength,
            onCompleted = () => isAnimationPlaying = false
        };
        DoTweenMoveAnimation.Play(animationInfo);
        curentHeight += resource.ResourceData.Size.y;
    }
    internal void PullResource(StorageInput storageInput)
    {
        if (storageInput.TryGetFreeLocalPosition(out Vector3 localPosition) == false)
            return;

        for (int i = 0; i < resources.Count; i++)
        {
            if (resources[i].ResourceData.ResourceType == storageInput.ResourceType)
            {
                Resource resource = resources[i];
                isAnimationPlaying = true;
                resource.collider.enabled = true;
                resource.transform.SetParent(storageInput.transform, true);
                MoveAnimationInfo animationInfo = new MoveAnimationInfo()
                {
                    resource = resource,
                    endLocalPos = localPosition,
                    endLocalRotation = Quaternion.identity,
                    duration = unloadingAnimationLength,
                    onCompleted = () => PullResourceComplited(storageInput, resource)
                };
                DoTweenMoveAnimation.Play(animationInfo);
                resources.Remove(resource);
                UpdateResourcePositions();
                return;
            }
        }
    }
    private void PullResourceComplited(StorageInput storageInput, Resource resource)
    {
        storageInput.PushResource(resource);
        isAnimationPlaying = false;
    }
    private void UpdateResourcePositions()
    {
        curentHeight = 0;
        for (int i = 0; i < resources.Count; i++)
        {
            float resourceHeight = curentHeight + 0.5f * resources[i].ResourceData.Size.y;
            resources[i].transform.localPosition = resourceHeight * Vector3.up;
            curentHeight += resources[i].ResourceData.Size.y;
        }
    }
}