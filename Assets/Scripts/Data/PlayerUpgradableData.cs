using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade Asset Menu", menuName = "Upgrade Scriptable Data")]
public class PlayerUpgradableData : ScriptableObject
{
    [Header("Upgrade Details")]
    public string UpgradeName;

    [Header("Player Stats")]
    public float MovementSpeedToAdd;
    public float MaxHealthToAdd;

    [Header("Gun/Bullet Stats")]
    public int MagSizeToAdd;
    public float BulletSizeToAdd;
    public float BulletSpreadToAdd;
    public int BulletBouncesToAdd;
    public float ShootCooldownToAdd;
}
