using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float _healthPoints = 100f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (_healthPoints <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void TakeDamage(float damageAmt)
    {
        _healthPoints -= damageAmt;
    }
}
