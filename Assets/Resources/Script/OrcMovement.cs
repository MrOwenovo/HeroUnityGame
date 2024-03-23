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
    public LayerMask obstacleLayer;

    


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        detectionZone = GetComponent<DetectionZone>();
        
        
        
        
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        IDamageable damageable = collider.GetComponent<IDamageable>();
        if (damageable != null && collider.tag == "Player")
        {
            Vector2 direction = collider.transform.position - transform.position;
            Vector2 force = direction.normalized * knockbackForce;
            
            OnFire();
            // damageable.OnHit(attactPower, force);
    
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
    public void OnAttackOver()
    {
        animator.SetTrigger("swordAttack");
    }

    public void OnDie()
    {
        animator.SetTrigger("isDead");
        
    }
    
    private Vector2 GetMoveDirection()
    {
        if (detectionZone.detectedObjs != null)
        {
            Vector2 targetDirection = (detectionZone.detectedObjs.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, detectionZone.viewRadius, obstacleLayer);
            if (hit.collider != null)
            {
                Vector2 avoidanceDirection = Vector2.Perpendicular(targetDirection).normalized;
                return avoidanceDirection;
            }
            return targetDirection;
        }
        return Vector2.zero;
    }

    private void UpdateOrientation(Vector2 moveDirection)
    {
        if (moveDirection.x > 0)
        {
            spriteRenderer.flipX = false;
            gameObject.BroadcastMessage("IsFacingRight", true);
        }
        else if (moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
            gameObject.BroadcastMessage("IsFacingRight", false);
        }
    }

    
    private void FixedUpdate()
    {
        Vector2 moveDirection = GetMoveDirection();
        if (moveDirection != Vector2.zero)
        {
            rb.AddForce(moveDirection * speed);
            UpdateOrientation(moveDirection);
            OnWalk();
        }
        else
        {
            OnWalkStop();
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
