using UnityEngine;
using System;

public class BulletController : MonoBehaviour
{
    public event Action OnDisableBulletEvent;

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _damage = 100f;
    [HideInInspector] public float bulletSpread = 0f;
    [HideInInspector] public int bulletBounces = 0;
    private GameManager _gameManager;
    private Rigidbody _rigidBody;
    private int _xDirection = 1;
    private int _yDirection = 1;
    private float _uniqueBulletSpread;
    private int _bulletBounceCounter;

    

    private void Awake()
    {
        _gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        float effectiveBulletSpread = bulletSpread;

        if (bulletSpread < 0)
        {
            effectiveBulletSpread = 0;
        }

        _uniqueBulletSpread = UnityEngine.Random.Range(-effectiveBulletSpread, effectiveBulletSpread);

        _bulletBounceCounter = bulletBounces;

        _xDirection = 1;
        _yDirection = 1;
    }

    private void FixedUpdate()
    {
        if (_gameManager.isGamePaused)
        {
            return;
        }

        Vector3 spreadDirection = new Vector3(_uniqueBulletSpread * _xDirection, _yDirection);
        Vector3 newPosition = _rigidBody.position + spreadDirection.normalized * _speed * Time.fixedDeltaTime;
        _rigidBody.MovePosition(newPosition);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Bullet")
        {
            return;
        }

        EnemyController enemyController = collider.gameObject.GetComponent<EnemyController>();

        if (enemyController != null)
        {
            enemyController.TakeDamage(_damage);
        }

        if (_bulletBounceCounter <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            if (enemyController != null)
            {
                if (Mathf.Abs(enemyController.transform.position.x - transform.position.x) > Mathf.Abs(enemyController.transform.position.y - transform.position.y))
                {
                    _xDirection = -_xDirection;
                }
                else
                {
                    _yDirection = -_yDirection;
                }
            }

            if (collider.gameObject.name == "Left Border" || collider.gameObject.name == "Right Border")
            {
                _xDirection = -_xDirection;
            }
            else if (collider.gameObject.name == "Top Border" || collider.gameObject.name == "Bottom Border")
            {
                _yDirection = -_yDirection;
            }

            if (collider.gameObject.GetComponent<PlayerController>() != null) // even if you still have bounces. if you catch the balls, they're essentially reloaded
            {
                gameObject.SetActive(false);
            }
            
            _bulletBounceCounter--;
        }
    }

    private void OnDisable()
    {
        OnDisableBulletEvent?.Invoke();
    }
}
