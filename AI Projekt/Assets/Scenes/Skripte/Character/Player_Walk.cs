using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Walk : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 10;
    [SerializeField] private float sprintAddition = 5;
    private Rigidbody2D rb;
    [SerializeField]private BoolValue boolValue;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.Log("Rigidbody disappeared");
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
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
        }
    }
}
