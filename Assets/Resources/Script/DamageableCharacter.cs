using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DamageableCharacter : MonoBehaviour ,IDamageable
{
    private Rigidbody2D rb;
    private Collider2D physicsCollider;
    public int health;

    bool targetable;
    

    public int Health
    {
        get { return health; }
        set
        {
            health = value;

            if (health <= 0)
            {
                gameObject.BroadcastMessage("OnDie");

            }
            else
            {
                gameObject.BroadcastMessage("OnDamage");
            }
        }
    }

    public bool Targetable
    {
        get { return targetable; }
        set
        {
            targetable = value;
            if (!targetable)
            {
                rb.simulated = false;
                

            }
        }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCollider = GetComponent<Collider2D>();
        
    }

    public void OnDying()
    {
        Targetable = false;
        
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        Health -= damage;
        rb.AddForce(knockback);
    }

    public void OnObjectDestroyed()
    {
        Destroy(gameObject);
    }
}