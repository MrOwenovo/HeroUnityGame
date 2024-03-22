using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<GameAssets>("GameAssets");
            }

            return instance;
        }
    }
    public Transform DamagePopup;
    public Transform NonDesObstacle;
    public Transform WreckableObstacle;
}
