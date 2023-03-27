using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    Vector2 movement;
    enum DirectionStates  {Forward=1,Right=2,Backward=3,Left=4}

    [SerializeField] DirectionStates directionStates;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
    private void Update()
    {
        directionStates = (DirectionStates)Mathf.CeilToInt((((transform.rotation.eulerAngles.y + 45) % 360) / 90));
        animator.SetInteger("State", (int)directionStates);
    }

    private void OnMove(InputValue value)
    {
        movement=value.Get<Vector2>();

        animator.SetFloat("SpeedX", movement.x);

        animator.SetFloat("SpeedZ", movement.y);

    }

}
