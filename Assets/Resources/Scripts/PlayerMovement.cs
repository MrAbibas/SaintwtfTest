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
        if (joystick.Direction == Vector2.zero) return;

        Vector3 direction = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        transform.rotation = Quaternion.LookRotation(direction,transform.up);
        rigidbody.velocity = transform.forward * speed;
    }
}