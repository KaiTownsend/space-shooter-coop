using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _damage = 100f;
    [HideInInspector] public float bulletSpread = 0f;
    [HideInInspector] public int bulletBounces = 0;
    private GameManager _gameManager;
    private int xDirection = 1;
    private int yDirection = 1;
    private float uniqueBulletSpread;
    private int bulletBounceCounter;

    private void Awake()
    {
        _gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        uniqueBulletSpread = Random.Range(-bulletSpread, bulletSpread);

        bulletBounceCounter = bulletBounces;

        xDirection = 1;
        yDirection = 1;
    }

    private void Update()
    {
        if (_gameManager.isGamePaused)
        {
            return;
        }

        transform.position = new Vector3(transform.position.x + uniqueBulletSpread * xDirection * Time.deltaTime, transform.position.y + _speed * yDirection * Time.deltaTime, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        EnemyController enemyController = collider.gameObject.GetComponent<EnemyController>();

        if (enemyController != null)
        {
            enemyController.TakeDamage(_damage);
        }

        if (bulletBounceCounter <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            if (enemyController != null)
            {
                if (Mathf.Abs(enemyController.transform.position.x - transform.position.x) > Mathf.Abs(enemyController.transform.position.y - transform.position.y))
                {
                    xDirection = -xDirection;
                }
                else
                {
                    yDirection = -yDirection;
                }
            }

            if (collider.gameObject.name == "Left Border" || collider.gameObject.name == "Right Border")
            {
                xDirection = -xDirection;
            }
            else if (collider.gameObject.name == "Top Border" || collider.gameObject.name == "Bottom Border")
            {
                yDirection = -yDirection;
            }

            if (collider.gameObject.GetComponent<PlayerController>() != null) // even if you still have bounces. if you catch the balls, they're essentially reloaded
            {
                gameObject.SetActive(false);
            }
            
            bulletBounceCounter--;
        }
    }
}
