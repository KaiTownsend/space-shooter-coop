using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _deathParticles;
    [SerializeField] private float _movementSpeed = 0.05f;
    private Transform _enemyContainer;
    private float _healthPoints = 100f;

    private void Start()
    {
        _enemyContainer = transform.parent.gameObject.transform;
    }

    private void Update()
    {
        transform.position += new Vector3(0, -_movementSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (_healthPoints <= 0)
        {
            _deathParticles.transform.SetParent(_enemyContainer);
            _deathParticles.Play();

            _healthPoints = 100f;

            gameObject.SetActive(false);
        }
    }

    public void TakeDamage(float damageAmt)
    {
        _healthPoints -= damageAmt;
    }

    public void Reactivate(Vector3 spawnPosition)
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);

            _deathParticles.transform.SetParent(gameObject.transform);
            _deathParticles.transform.localPosition = Vector3.zero;
            _deathParticles.transform.localScale = Vector3.one;
            _deathParticles.transform.localRotation = Quaternion.identity;
            _deathParticles.Stop();
        }
    }
}
