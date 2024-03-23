using UnityEngine;

public class MissileController : MonoBehaviour
{
    public float speed = 5f; // 导弹的速度
    private Vector2 moveDirection; // 导弹的移动方向

    // 设置导弹的移动方向
    public void Launch(Vector2 direction)
    {

        moveDirection = direction.normalized;
    }

    private void Update()
    {
        // 导弹的直线运动
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null&&(collision.CompareTag("Player")||collision.CompareTag("Wreckable")) )
        {
            // 对英雄造成伤害的逻辑
            damageable.OnHit(1, moveDirection); // 假设伤害值为1
            Destroy(gameObject); // 销毁导弹
        }
        else if (collision.CompareTag("Obstacle")) // 假设所有障碍物的 Tag 都设置为 "Obstacle"
        {
            Destroy(gameObject); // 如果是障碍物，也销毁导弹
        }
    }
}