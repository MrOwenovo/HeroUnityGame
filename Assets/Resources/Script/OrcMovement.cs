using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Timeline;

public class OrcMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    SpriteRenderer spriteRenderer;
    public float speed;
    
    DetectionZone detectionZone;
    public float knockbackForce;
    public int attactPower;
    


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        detectionZone = GetComponent<DetectionZone>();
        
            // transform.Find("DetectZone").GetComponent<DetectionZone>();
        
        
        
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        IDamageable damageable = collider.GetComponent<IDamageable>();
        if (damageable != null && collider.tag == "Player")
        {
            Vector2 direction = collider.transform.position - transform.position;
            Vector2 force = direction.normalized * knockbackForce;

            damageable.OnHit(attactPower, force);

        }
    }

    public void OnWalk()
    {
        animator.SetBool("isWalking",true);
    }

    public void OnWalkStop()
    {
        animator.SetBool("isWalking",false);
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
        if (detectionZone.detectedObjs != null)
        {
            Vector2 direction = (detectionZone.detectedObjs.transform.position - transform.position);
            if (direction.magnitude <= detectionZone.viewRadius)
            {
                rb.AddForce(direction.normalized * speed);
                if (direction.x > 0)
                {
                    spriteRenderer.flipX = false;

                }

                if (direction.x <0)
                {
                    spriteRenderer.flipX = true;
                }
               
                OnWalk();
            }
            else
            {
                OnWalkStop();
            }
        }
        
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
