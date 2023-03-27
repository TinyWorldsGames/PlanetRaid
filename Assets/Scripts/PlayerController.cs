using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 movement;

    private Rigidbody rb;

    public Camera cam;

    private Vector3 mousePos;

    public float turnSpeed = 10f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
        
    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(movement.x, 0f, movement.y);
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

        mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.transform.position.y));
        Vector3 lookDirection = mousePos - transform.position;
        lookDirection.y = 0;
        Quaternion newRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, turnSpeed * Time.deltaTime);


    }
}
