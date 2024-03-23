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

    private float nextFireTime = 0f; // 下次发射时间
    public float fireRate = 1f; // 发射频率，每秒一次
    public LayerMask obstacleLayer; // 障碍物层
    public float idealRange = 5f;
    public float tooCloseRange = 3f;
    private GameObject target; // 目标（玩家）

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rangedDetectionZone = GetComponent<RangedDetectionZone>();

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

            OnFire();
            // damageable.OnHit(attactPower, force);
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
        // LaunchMissileTowards(transform.position);
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
        Vector3 direction = targetPosition - transform.position; // 计算指向玩家的方向
        Transform missileTransform = Instantiate(GameAssets.Instance.Missile, transform.position, Quaternion.identity);
        MissileController missileController = missileTransform.GetComponent<MissileController>();
        missileController.Launch(direction);
    }

    private void MaintainDistanceFromTarget()
    {
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

        MoveEnemy(moveDirection);
    }

    private void MoveEnemy(Vector2 direction)
    {
        
        float moveSpeed = rangedSpeed * Time.fixedDeltaTime; // 使用 fixedDeltaTime 保证帧率无关
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
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // 查找标记为"Player"的游戏对象
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