using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _deathParticles;
    [SerializeField] private float _movementDist = 0.25f;
    [SerializeField] private float _movementCooldown = 3f;
    private Transform _enemyContainer;
    private float _healthPoints = 100f;
    private float _movementTimer;

    private void Start()
    {
        _enemyContainer = transform.parent.gameObject.transform;
    }

    private void Update()
    {
         _movementTimer += Time.deltaTime;

        // if (_movementTimer >= _movementCooldown)
        // {
        //     transform.position += new Vector3(0, -_movementDist, 0);
        //     _movementTimer = 0f;
        // }
        
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
            transform.position = spawnPosition;

            _deathParticles.transform.SetParent(gameObject.transform);
            _deathParticles.transform.localPosition = Vector3.zero;
            _deathParticles.transform.localScale = Vector3.one;
            _deathParticles.transform.localRotation = Quaternion.identity;
            _deathParticles.Stop();
        }
    }
}
