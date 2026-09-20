using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float _healthPoints = 100f;

    private void Update()
    {
        if (_healthPoints <= 0)
        {
            Destroy(gameObject, 0f);
        }
    }

    public void TakeDamage(float damageAmt)
    {
        _healthPoints -= damageAmt;
    }
}
