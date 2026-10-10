using Mirror;

public class PlayerStats : NetworkBehaviour
{
    [SyncVar] public float MovementSpeed = 5f;
    [SyncVar] public float MaxHealth = 100f;

    [SyncVar] public int MagSize = 1;
    [SyncVar] public float BulletSize = 3f;
    [SyncVar] public float BulletSpread = 0f;
    [SyncVar] public int BulletBounces = 0;
    [SyncVar] public float ShootCooldown = 0.25f;
}
