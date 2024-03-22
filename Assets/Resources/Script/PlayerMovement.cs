using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    SpriteRenderer spriteRenderer;
    public float moveSpeed;

    private Vector2 moveInput;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        
        
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        if (moveInput == Vector2.zero)
        {
            animator.SetBool("isWalking",false);
        }
        else
        {
            animator.SetBool("isWalking",true);
            if (moveInput.x > 0)
            {
                spriteRenderer.flipX = false;
                gameObject.BroadcastMessage("IsFacingRight",true);
            }
            else
            {
                spriteRenderer.flipX = true;
                gameObject.BroadcastMessage("IsFacingRight",false);
            }
        }
    }
    void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }
    public void OnDamage()
    {
        animator.SetTrigger("isDamage");
        
    }

    public void OnDamageOver()
    {
        animator.SetTrigger("isDamage");
    }

    public void OnDie()
    {
        animator.SetTrigger("isDead");
        
    }
    private void FixedUpdate()
    {
        rb.AddForce(moveInput*moveSpeed);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
