using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _damage = 100f;

    private void Start()
    {
        Destroy(gameObject, 2f);
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + _speed * Time.deltaTime, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        EnemyController enemyController = collider.gameObject.GetComponent<EnemyController>();

        if (enemyController != null)
        {
            enemyController.TakeDamage(_damage);
            Destroy(gameObject, 0f);
        }
    }

}
