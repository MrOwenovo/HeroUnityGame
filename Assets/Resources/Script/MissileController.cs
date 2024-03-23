using UnityEngine;

public class MissileController : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 moveDirection;

    public void Launch(Vector2 direction)
    {

        moveDirection = direction.normalized;
    }

    private void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null&&(collision.CompareTag("Player")||collision.CompareTag("Wreckable")) )
        {
            damageable.OnHit(1, moveDirection);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}