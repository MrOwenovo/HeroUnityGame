using UnityEngine;
using UnityEngine.UI;

public class CooldownDisplay : MonoBehaviour
{
    public Image meleeCooldownImage; 
    public Image missileCooldownImage;
    public PlayerMovement player;

    void Update()
    {
        meleeCooldownImage.fillAmount = player.meleeCooldown / player.meleeCooldownTime;
        missileCooldownImage.fillAmount = player.missileCooldown / player.missileCooldownTime;
    }
}