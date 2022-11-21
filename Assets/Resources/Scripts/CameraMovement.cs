using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset;
    private Player player;
    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    private void Update()
    {
        transform.position = new Vector3()
        {
            x = player.transform.position.x,
            y = transform.position.y,
            z = player.transform.position.z
        } + offset;
    }
}