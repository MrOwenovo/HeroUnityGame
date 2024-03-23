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
    public EnemyType enemyType;
    
    private int totalAmount = 2;
    bool isGameWon = true;

    bool targetable;
    public event Action OnDeath;

    public enum EnemyType
    {
        Melee,
        Ranged,
        Obstacle
    }

    
    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            
            if (health <= 0)
            {
                gameObject.BroadcastMessage("OnDie");
                var mainUiController = FindObjectOfType<MainController>(); 
                if (mainUiController != null)
                {
                    int scoreToAdd = 0;
                    switch (enemyType)
                    {
                        case EnemyType.Melee:
                            scoreToAdd = 10;
                            totalAmount--;
                            break;
                        case EnemyType.Ranged:
                            scoreToAdd = 20;
                            totalAmount--;
                            break;
                        case EnemyType.Obstacle:
                            scoreToAdd = 0;
                            break;
                      
                    }
                    mainUiController.AddScore(scoreToAdd);
                }

                Debug.Log(totalAmount);

                
                if (totalAmount==0)
                {
                    // 假设这里已经有变量 isGameWon 表示游戏是否赢了，gameTime 表示游戏时间，score 表示得分
                    if (isGameWon)
                    {
                        mainUiController.SaveRank();
                        Debug.Log("成功保存前缀");

                    }
                }
            }
            else
            {
                gameObject.BroadcastMessage("OnDamage");
            }
            
            // 更新 HealthDisplay，仅当此 DamageableCharacter 是玩家时
            if (gameObject.CompareTag("Player"))
            {
                Debug.Log("damage:"+health);
                HealthDisplay.UpdateHealth(health);
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
        OnDeath?.Invoke();
        
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