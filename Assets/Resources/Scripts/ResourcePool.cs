using System.Collections.Generic;
using UnityEngine;

public class ResourcePool : MonoBehaviour
{
    private static ResourcePool _instance;
    internal static ResourcePool Instance => _instance;
    [SerializeField]
    private List<Resource> resourcePrefabs;
    Dictionary<ResourceType, Stack<Resource>> resourcesStacs;
    private void Awake()
    {
        if (_instance)
            return;

        _instance = this;
        resourcesStacs = new Dictionary<ResourceType, Stack<Resource>>();
        for (int i = 0; i < resourcePrefabs.Count; i++)
            resourcesStacs.Add(resourcePrefabs[i].ResourceData.ResourceType, new Stack<Resource>());
    }
    private void Expand(ResourceType type)
    {
        for (int i = 0; i < resourcePrefabs.Count; i++)
            if (resourcePrefabs[i].ResourceData.ResourceType == type)
                resourcesStacs[type].Push(
                    Instantiate(resourcePrefabs[i],Vector3.zero,Quaternion.identity,transform));
    }
    internal void Push(Resource resource)
    {
        resource.transform.SetParent(transform);
        resource.transform.position = Vector3.zero;
        resource.transform.rotation = Quaternion.identity;
        resource.gameObject.SetActive(false);
        resourcesStacs[resource.ResourceData.ResourceType].Push(resource);
    }
    internal Resource Pop(ResourceType type)
    {
        if (resourcesStacs[type].Count==0)
            Expand(type);

        Resource resource = resourcesStacs[type].Pop();
        resource.gameObject.SetActive(true);
        return resource;
    }
}