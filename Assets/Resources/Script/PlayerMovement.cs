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
    public GameObject playerMissilePrefab; // 导弹预制体
    private PauseMenu pauseMenu;

    private Vector2 lastMoveDirection = Vector2.right; // 默认向右
    public float meleeCooldownTime = 2f;
    public float missileCooldownTime = 0.5f;
    public float meleeCooldown;
    public float missileCooldown;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // 找到暂停菜单的引用
        pauseMenu = FindObjectOfType<PauseMenu>();
        
        
    }
    private void Update()
    {
        if (meleeCooldown > 0) meleeCooldown -= Time.deltaTime;
        if (missileCooldown > 0) missileCooldown -= Time.deltaTime;

        // 其他 Update 逻辑...
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
        
        Vector2 input = value.Get<Vector2>();
        if (input != Vector2.zero)
        {
            lastMoveDirection = input; // 更新最后的移动方向
        }
    }
    void OnFire()
    {
        if ( meleeCooldown <= 0)
        {
            animator.SetTrigger("swordAttack");
            meleeCooldown = meleeCooldownTime; // 重置远程攻击的CD
        }
    }

    void OnFireOver()
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

    public void OnLaunchMissile(InputValue value)
    {
        if (value.isPressed && missileCooldown <= 0)
        {
            LaunchMissile(lastMoveDirection); // 使用最后的移动方向发射导弹
            missileCooldown = missileCooldownTime; // 重置远程攻击的CD
        }
    }
    private void LaunchMissile(Vector2 direction)
    {
        // 根据玩家的移动方向来发射导弹
        Vector3 launchDirection = new Vector3(direction.x, direction.y, 0).normalized;
        GameObject missile = Instantiate(playerMissilePrefab, transform.position + launchDirection * 0.5f, Quaternion.identity);
        missile.GetComponent<PlayerMissileController>().Launch(launchDirection);
    }

    public void OnPause(InputValue value)
    {
        if (value.isPressed)
        {
            // 暂停游戏
            pauseMenu.PauseGame();
        }
    }
}
