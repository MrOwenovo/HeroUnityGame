using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IDamageable
{
    public void OnHit(int damage, Vector2 knockback);
    
}