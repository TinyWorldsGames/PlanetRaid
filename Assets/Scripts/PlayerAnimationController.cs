using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    Vector2 movement;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }

    private void OnMove(InputValue value)
    {
        movement=value.Get<Vector2>();

        animator.SetFloat("SpeedX", movement.x);

        animator.SetFloat("SpeedZ", movement.y);

    }

}
