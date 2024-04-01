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

    private bool isCooldown = false; 


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
            
            if (!isCooldown) 
            {
                OnFire();
                // damageable.OnHit(attactPower, force);
            }
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
        StartCoroutine(AttackCooldown());
    }
    IEnumerator AttackCooldown()
    {
        isCooldown = true; 
        yield return new WaitForSeconds(2); 
        isCooldown = false; 
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

        if (!(GameDifficulty.CurrentDifficulty == GameDifficulty.Difficulty.Easy))
        {
            // Check if you need to avoid missiles
            if (GetComponent<MissileDetectionZone>().detectedMissile != null)
            {
                Collider2D missileCollider = GetComponent<MissileDetectionZone>().detectedMissile;
                Vector2 dodgeDirection = Vector2.Perpendicular((Vector2)transform.position - (Vector2)missileCollider.transform.position).normalized;
        
                // Perform evasive actions, such as changing the current direction of movement
                DodgeMissile(dodgeDirection);
            }
        }
        
    }

    void DodgeMissile(Vector2 dodgeDirection)
    {
        float dodgeSpeed = 5f;
        rb.velocity = dodgeDirection * dodgeSpeed;
        StartCoroutine(ResetDodge());
    }
    
    IEnumerator ResetDodge()
    {
        yield return new WaitForSeconds(1f);
        rb.velocity = Vector2.zero; 
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
