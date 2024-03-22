using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObstacleMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    SpriteRenderer spriteRenderer;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        
        
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
  

}