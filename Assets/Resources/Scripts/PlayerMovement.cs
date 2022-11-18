using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Joystick joystick;
    [SerializeField]
    private float speed;
    private Rigidbody rigidbody;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector3()
        {
            x = joystick.Direction.x * speed,
            y = rigidbody.velocity.y,
            z = joystick.Direction.y * speed,
        };
    }
}
