using UnityEngine;
using Mirror;

public class EnemyController : NetworkBehaviour
{
    [SerializeField] private ParticleSystem _spawnParticles;
    [SerializeField] private ParticleSystem _deathParticles;
    private AudioManager _audioManager;
    public float maxHealth = 100f;
    private float _healthPoints;

    private void Awake()
    {
        _healthPoints = maxHealth;
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
        _audioManager.PlayAudioOnHit();

        _healthPoints -= damageAmt;

        if (_healthPoints <= 0)
        {
            _deathParticles.Play();

            gameObject.SetActive(false);

            NetworkServer.UnSpawn(gameObject);
        }
    }

    [Server]
    public void Reactivate(Vector3 spawnPosition)
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
            transform.position = spawnPosition;

            NetworkServer.Spawn(gameObject);

            _spawnParticles.Play();

            _deathParticles.transform.SetParent(gameObject.transform);
            _deathParticles.transform.localPosition = Vector3.zero;
            _deathParticles.transform.localScale = Vector3.one;
            _deathParticles.transform.localRotation = Quaternion.identity;
            _deathParticles.Stop();
        }

        _healthPoints = maxHealth;
    }

    public void SetAudioManager(AudioManager audioManager)
    {
        _audioManager = audioManager;
    }
}
