using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _spawnParticles;
    [SerializeField] private ParticleSystem _deathParticles;
    private Transform _enemyContainer;
    private float _healthPoints = 100f;

    private void Start()
    {
        _enemyContainer = transform.parent.gameObject.transform;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Bottom Border")
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().TakeDamage(5);
        }
    }

    public void TakeDamage(float damageAmt)
    {
        _healthPoints -= damageAmt;

        if (_healthPoints <= 0)
        {
            _deathParticles.transform.SetParent(_enemyContainer);
            _deathParticles.Play();

            _healthPoints = 100f;

            gameObject.SetActive(false);
        }
    }

    public void Reactivate(Vector3 spawnPosition)
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
            transform.position = spawnPosition;

            _spawnParticles.Play();

            _deathParticles.transform.SetParent(gameObject.transform);
            _deathParticles.transform.localPosition = Vector3.zero;
            _deathParticles.transform.localScale = Vector3.one;
            _deathParticles.transform.localRotation = Quaternion.identity;
            _deathParticles.Stop();
        }
    }
}
