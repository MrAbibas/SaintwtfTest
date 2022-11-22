using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerInventory playerInventory;

    private void OnTriggerStay(Collider other)
    {
        if (playerInventory.isAnimationPlaying)
            return;

        StorageInput storageInput = other.GetComponent<StorageInput>();
        if (storageInput != null)
        {
            playerInventory.PullResource(storageInput);
            return;
        }
        StorageOutput storageOutput = other.GetComponent<StorageOutput>();
        if(storageOutput != null)
        {
            playerInventory.PushResource(storageOutput);
            return;
        }
    }
}