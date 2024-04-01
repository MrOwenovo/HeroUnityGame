using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class DamageableCharacter : MonoBehaviour ,IDamageable
{
    private Rigidbody2D rb;
    private Collider2D physicsCollider;
    public int health;
    public EnemyType enemyType;
    
    private int totalAmount = 2;
    bool isGameWon = true;
    public GameObject failureMenuUI;
    public TextMeshProUGUI healthText;
    
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
            
            if (gameObject.CompareTag("Wreckable") && GameDifficulty.CurrentDifficulty == GameDifficulty.Difficulty.Easy)
            {
                health = 1; //easy
            }
            
            if (health <= 0)
            {
                gameObject.BroadcastMessage("OnDie");
                if (gameObject.CompareTag("Player"))
                {
                    Debug.Log("player damage");
                    healthText.text = "Health: " + health.ToString();
                }
                OnDeath?.Invoke();
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
                    if (isGameWon)
                    {
                        mainUiController.SaveRank();
                        Debug.Log("成功保存前缀");

                    }
                }
                if (gameObject.CompareTag("Player"))
                {
                   Debug.Log("Failure");
                   FailureGame();
                }
            }
            else
            {
                gameObject.BroadcastMessage("OnDamage");
                if (gameObject.CompareTag("Player"))
                {
                    Debug.Log("player damage");
                    healthText.text = "Health: " + health.ToString();
                }
            }
            
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
    public void FailureGame()
    {
        failureMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCollider = GetComponent<Collider2D>();
        
        EnemyManager.OnRoundTwoStart += HandleRoundTwoStart;
        PauseMenu.resetHealth += HandleRoundTwoStart;
    }
    private void HandleRoundTwoStart()
    {
        if (gameObject.CompareTag("Player"))
        {
            Health = 30;
            healthText.text = "Health: " + health.ToString();
        }
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
    private void OnDestroy()
    {
        // 不要忘记在对象销毁时取消订阅，避免内存泄漏
        EnemyManager.OnRoundTwoStart -= HandleRoundTwoStart;
    }
}