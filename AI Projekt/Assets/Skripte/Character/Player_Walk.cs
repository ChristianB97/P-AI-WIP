using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Walk : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 10;
    [SerializeField] private float sprintAddition = 5;
    private Rigidbody2D rb;
    [SerializeField]private BoolValue boolValue;
    [SerializeField]private Animator anim;
    private bool isFacingRight;
    private bool IsFacingRight
    {
        get
        {
            return isFacingRight;
        }
        set
        {
            if (isFacingRight != value)
            {
                isFacingRight = value;
                Flip();
            }
        }
    }

    private void Flip()
    {
        if (IsFacingRight)
            rb.transform.localRotation = Quaternion.Euler(0, 0, 0);
        else
            rb.transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

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
            float ySpeed = 0;
            float xSpeed = 0;
            if (Input.GetButton("Sprint"))
                currWalkSpeed += sprintAddition;
            if (Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") == 0)
            {
                IsFacingRight = Input.GetAxisRaw("Horizontal") > 0;
                rb.MovePosition(rb.position + new Vector2(Input.GetAxisRaw("Horizontal") * currWalkSpeed * Time.deltaTime, 0));
                xSpeed = Math.Abs(Input.GetAxisRaw("Horizontal"));
            }
            else if (Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Horizontal") == 0)
            {
                rb.MovePosition(rb.position + new Vector2(0, Input.GetAxisRaw("Vertical") * currWalkSpeed * Time.deltaTime));
                ySpeed = Input.GetAxisRaw("Vertical");
            }
                
            anim.SetFloat("absoluteXSpeed", xSpeed);
            anim.SetFloat("ySpeed", ySpeed);
            
        }
    }
}
