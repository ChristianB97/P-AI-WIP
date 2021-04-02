using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Walk : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 10;
    [SerializeField] private float sprintAddition = 5;
    private Rigidbody2D rb;
    [SerializeField]private BoolValue boolValue;
    [SerializeField] private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (anim != null)
        {
            anim = GetComponent<Animator>();
        }
        boolValue.OnValueChanged += StopWalking;
    }

    private void StopWalking(bool value)
    {
        if (!value)
        {
            rb.velocity = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        if (boolValue.RuntimeValue)
        {
            float currWalkSpeed = walkSpeed;
            if (Input.GetButton("Sprint"))
                currWalkSpeed += sprintAddition;
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * currWalkSpeed, Input.GetAxisRaw("Vertical") * currWalkSpeed);
            anim.SetFloat("xSpeed", Mathf.Max(Mathf.Abs(Input.GetAxisRaw("Horizontal")), Mathf.Abs(Input.GetAxisRaw("Vertical"))));
        }
    }
}
