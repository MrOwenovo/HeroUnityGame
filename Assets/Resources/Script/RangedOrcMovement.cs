using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Timeline;

public class RangedOrcMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    SpriteRenderer spriteRenderer;
    public float attackSpeed;
    public float rangedSpeed;
    

    RangedDetectionZone rangedDetectionZone;
    public float knockbackForce;
    public int attactPower;

    private float nextFireTime = 0f;
    public float fireRate = 1f;
    public LayerMask obstacleLayer;
    public float idealRange = 5f;
    public float tooCloseRange = 3f;
    private GameObject target; 
    
    private bool isCooldown = false; 
    public AudioSource audioSource;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rangedDetectionZone = GetComponent<RangedDetectionZone>();
        audioSource = GetComponent<AudioSource>(); 
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

            if (!isCooldown) 
            {
                OnFire();
                // damageable.OnHit(attactPower, force);
            }
        }
    }

    public void OnWalk()
    {
        animator.SetBool("isWalking", true);
    }

    public void OnWalkStop()
    {
        animator.SetBool("isWalking", false);
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

    void LaunchMissileTowards(Vector3 targetPosition)
    {
        if (GameDifficulty.CurrentDifficulty == GameDifficulty.Difficulty.Easy)
        {
            fireRate = 0.5f; 
            rangedSpeed = 2.5f; 
        }
        else
        {
            fireRate = 1f; 
            rangedSpeed = 5f; 
        }
        
        audioSource.clip = Resources.Load<AudioClip>("Sound/fire");
        audioSource.Play();
        
        Vector3 direction = targetPosition - transform.position;
        Transform missileTransform = Instantiate(GameAssets.Instance.Missile, transform.position, Quaternion.identity);
        MissileController missileController = missileTransform.GetComponent<MissileController>();
        missileController.Launch(direction);
    }
   
    private void MaintainDistanceFromTarget()
    {
        
        if (GameDifficulty.CurrentDifficulty == GameDifficulty.Difficulty.Easy)
        {
            return; // easy
        }
        if (target == null) return;

        float distanceToTarget = Vector2.Distance(transform.position, target.transform.position);
        Vector2 directionToTarget = (target.transform.position - transform.position).normalized;
        Vector2 moveDirection = Vector2.zero;

        if (distanceToTarget < tooCloseRange)
        {
            // Target is too close, move away
            moveDirection = -directionToTarget;
        }
        else if (distanceToTarget > idealRange)
        {
            // Target is too far, move closer
            moveDirection = directionToTarget;
        }

        // Check for obstacles in the path
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleLayer);
        if (hit.collider != null)
        {
            // There is an obstacle in the way, find a new direction to move
            moveDirection += hit.normal * rangedSpeed;
        }
        
        if (GetComponent<MissileDetectionZone>().detectedMissile != null)
        {
            Collider2D missileCollider = GetComponent<MissileDetectionZone>().detectedMissile;
            Vector2 dodgeDirection = Vector2.Perpendicular((Vector2)transform.position - (Vector2)missileCollider.transform.position).normalized;
        
            moveDirection = dodgeDirection;
        }

        MoveEnemy(moveDirection);
        
        
    }

    private void MoveEnemy(Vector2 direction)
    {
        
        float moveSpeed = rangedSpeed * Time.fixedDeltaTime;
        rb.velocity = direction.normalized * moveSpeed;

        UpdateAnimation(direction);
    }

    private void UpdateAnimation(Vector2 direction)
    {
        if (direction.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
            spriteRenderer.flipX = direction.x < 0;
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        target = player;

        if (player != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, player.transform.position);

            if (rangedDetectionZone.detectedObjs != null)
            {
                Vector2 direction = (rangedDetectionZone.detectedObjs.transform.position - transform.position);
                if (direction.magnitude <= rangedDetectionZone.viewRadius)
                {
                    rb.AddForce(direction.normalized * attackSpeed);
                    if (direction.x > 0)
                    {
                        spriteRenderer.flipX = false;
                        gameObject.BroadcastMessage("IsFacingRight", true);
                    }

                    if (direction.x < 0)
                    {
                        spriteRenderer.flipX = true;
                        gameObject.BroadcastMessage("IsFacingRight", false);
                    }

                    OnWalk();
                }
            }
            else
            {
                OnWalkStop();
                MaintainDistanceFromTarget();
                if (Time.time >= nextFireTime && distanceToTarget <= idealRange)
                {
                    LaunchMissileTowards(player.transform.position);
                    nextFireTime = Time.time + 1f / fireRate;
                }

            }
        }
    }

    // private void FixedUpdate()
    // {
    //     //近战
    //     if (rangedDetectionZone.detectedObjs != null)
    //     {
    //         Vector2 direction = (rangedDetectionZone.detectedObjs.transform.position - transform.position);
    //         if (direction.magnitude <= rangedDetectionZone.viewRadius)
    //         {
    //             rb.AddForce(direction.normalized * attackSpeed);
    //             if (direction.x > 0)
    //             {
    //                 spriteRenderer.flipX = false;
    //                 gameObject.BroadcastMessage("IsFacingRight", true);
    //             }
    //
    //             if (direction.x < 0)
    //             {
    //                 spriteRenderer.flipX = true;
    //                 gameObject.BroadcastMessage("IsFacingRight", false);
    //             }
    //
    //             OnWalk();
    //         }
    //         else
    //         {
    //             OnWalkStop();
    //         }
    //     }
    // }


    // Start is called before the first frame update
    void Start()
    {
    }
}