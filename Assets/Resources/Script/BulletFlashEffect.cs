using UnityEngine;

public class BulletFlashEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Color flashColor = Color.yellow;
    public float flashDuration = 0.1f;
    private Color originalColor;
    private float flashTimer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void Update()
    {
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0)
            {
                spriteRenderer.color = originalColor;
            }
        }
        else
        {
            spriteRenderer.color = flashColor;
            flashTimer = flashDuration;
        }
    }
}