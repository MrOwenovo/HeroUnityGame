using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileDetectionZone : MonoBehaviour
{
    public Collider2D detectedMissile;
    public float detectionRadius = 5f;
    public LayerMask missileLayerMask;

    void Update()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, detectionRadius, missileLayerMask);
        if (collider != null && collider.CompareTag("PlayerMissile"))
        {
            detectedMissile = collider;
        }
        else
        {
            detectedMissile = null;
        }
    }
}