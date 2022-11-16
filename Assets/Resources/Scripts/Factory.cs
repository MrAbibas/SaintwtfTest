using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

internal class Factory : MonoBehaviour
{
    [SerializeField]
    private FactoryPort<StorageInput>[] inputParameters;
    private FactoryPort<StorageInput>[] inputLoadedParameters;
    [SerializeField]
    private FactoryPort<StorageOutput> outputParameter;

    [SerializeField]
    private float productionDuration;
    [SerializeField]
    private FactoryView factoryView;
    private void Start()
    {
        StartLoading();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) && inputParameters.Length > 0)
        {
            inputParameters[0].storage.PushResource(ResourcePool.Instance.Pop(inputParameters[0].type));
        }
        if (Input.GetKeyUp(KeyCode.Alpha1) && inputParameters.Length>1)
        {
            inputParameters[1].storage.PushResource(ResourcePool.Instance.Pop(inputParameters[1].type));
        }
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
            resources.Push(ResourcePool.Instance.Pop(outputParameter.type));
        UnloadingResources(resources);
    }
    private void UnloadingResources(Stack<Resource> resources)
    {
        outputParameter.storage.onDecreaseCount.RemoveAllListeners();
        if (resources.Count == 0)
        {
            StartLoading();
            return;
        }
        Resource resource = resources.Peek();
        if (outputParameter.storage.PushResource(resource) == false)
        {
            Debug.Log("Storage otput is full");
            outputParameter.storage.onDecreaseCount.AddListener(() => UnloadingResources(resources));
            return;
        }
        resources.Pop();
        UnloadingResources(resources);
    }
    private void StartLoading()
    {
        if (inputParameters == null || inputParameters.Length == 0)
        {
            StartCoroutine(Working());
            return;
        }
        inputLoadedParameters = new FactoryPort<StorageInput>[inputParameters.Length];

        for (int i = 0; i < inputParameters.Length; i++)
            inputLoadedParameters[i] = new FactoryPort<StorageInput>() { type = inputParameters[i].type, count = 0 };

        for (int i = 0; i < inputParameters.Length; i++)
            LoadResource(i, null);
    }
    private void EndLoading()
    {

        for (int i = 0; i < inputParameters.Length; i++)
            if (inputLoadedParameters[i].count != inputParameters[i].count)
            { Debug.Log($"End loading {inputLoadedParameters[i].type} {inputLoadedParameters[i].count}"); return; }

        StartCoroutine(Working());
    }
    private void LoadResource(int resourceId, UnityAction action)
    {
        if (action != null) inputParameters[resourceId].storage.onIncreaseCount.RemoveListener(action);
        if (inputLoadedParameters[resourceId].count == inputParameters[resourceId].count)
        {
            EndLoading();
            return;
        }
        if (inputParameters[resourceId].storage.ResourceCount > 0)
        {
            Resource resource = inputParameters[resourceId].storage.PopResource();
            ResourcePool.Instance.Push(resource);
            inputLoadedParameters[resourceId].count++;
            LoadResource(resourceId, action);
        }
        else
        {
            action = new UnityAction(() => LoadResource(resourceId, action));
            inputParameters[resourceId].storage.onIncreaseCount.AddListener(action);
        }
    }
}