using UnityEngine;
using UnityEngine.UI;

public class CooldownDisplay : MonoBehaviour
{
    public Image meleeCooldownImage; // 近战技能的冷却图标
    public Image missileCooldownImage; // 远程技能的冷却图标
    public PlayerMovement player; // 玩家控制脚本的引用

    void Update()
    {
        // 根据技能的冷却比例更新图标的填充量
        meleeCooldownImage.fillAmount = player.meleeCooldown / player.meleeCooldownTime;
        missileCooldownImage.fillAmount = player.missileCooldown / player.missileCooldownTime;
    }
}