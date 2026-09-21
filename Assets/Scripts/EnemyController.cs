using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _deathParticles;
    private float _healthPoints = 100f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (_healthPoints <= 0)
        {
            // _deathParticles.transform.rotation = transform.rotation;
            // _deathParticles.transform.position = transform.position;
            _deathParticles.transform.SetParent(null);
            _deathParticles.Play();
            gameObject.SetActive(false);
        }
    }

    public void TakeDamage(float damageAmt)
    {
        _healthPoints -= damageAmt;
    }
}
