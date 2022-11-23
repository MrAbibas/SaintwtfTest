using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField]
    private ResourceData resourceData;
    internal Collider collider;
    internal ResourceData ResourceData => resourceData;
    private void Awake()
    {
        collider = GetComponent<Collider>();
    }
}
