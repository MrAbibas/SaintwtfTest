using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Factory : MonoBehaviour
{
    [SerializeField]
    private FactoryInputPort[] inputParameters;
    [SerializeField]
    private FactoryOutputPort outputParameter;
    [SerializeField]
    private float productionDuration;
    [SerializeField]
    internal float loadingAnimationLength;
    [SerializeField]
    internal float unloadingAnimationLength;
    [SerializeField]
    private FactoryView factoryView;

    private void Start()
    {
        outputParameter.factory = this;
        for (int i = 0; i < inputParameters.Length; i++)
        {
            FactoryInputPort inputParameter = inputParameters[i];
            inputParameter.factory = this;
            inputParameter.onResourceLoaded.AddListener((t, c) => 
                factoryView.SetResourceCount(t.ResourceData.ResourceType, inputParameter.count - c));
        }
        StartLoading();
    }
    private void StartLoading()
    {
        if (inputParameters == null || inputParameters.Length == 0)
        {
            StartCoroutine(Working());
            return;
        }
        for (int i = 0; i < inputParameters.Length; i++)
        {
            inputParameters[i].ResetCurentCount();
            inputParameters[i].LoadResources(EndLoadingResources);
            factoryView.SetResourceCount(inputParameters[i].type, inputParameters[i].count);
        }
    }
    private void EndLoadingResources()
    {
        for (int i = 0; i < inputParameters.Length; i++)
            if (inputParameters[i].curentCount != inputParameters[i].count)
                return;

        StartCoroutine(Working());
    }
    private IEnumerator Working()
    {
        float timeProduction = 0;
        while (timeProduction < productionDuration)
        {
            timeProduction += Time.deltaTime;
            factoryView.UpdateProgres(timeProduction / productionDuration);
            yield return null;
        }
        factoryView.UpdateProgres(0);
        StartUnloading();
    }
    private void StartUnloading()
    {
        Stack<Resource> resources = new Stack<Resource>();
        for (int i = 0; i < outputParameter.count; i++)
        {
            Resource resource = ResourcePool.Instance.Pop(outputParameter.type);
            resource.transform.SetParent(transform, true);
            resource.transform.position = outputParameter.endPoint.position;
            resources.Push(resource);
        }
        outputParameter.UnloadingResources(resources, StartLoading);
    }  
}