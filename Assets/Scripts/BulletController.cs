using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _damage = 100f;
    [HideInInspector] public float bulletSpread = 0f;
    [HideInInspector] public int bulletBounces = 0;
    private int yDirection = 1;
    private float uniqueBulletSpread;
    private int bulletBounceCounter;

    private void OnEnable()
    {
        uniqueBulletSpread = Random.Range(-bulletSpread, bulletSpread);
        // StartCoroutine(DisableAfterWait(1));
        bulletBounceCounter = bulletBounces;
    }

    private void Update()
    {
        // may need an if statement where after it bounces there's one without uniqueBulletSpread. will test later.
        transform.position = new Vector3(transform.position.x + uniqueBulletSpread * Time.deltaTime, transform.position.y + _speed * yDirection * Time.deltaTime, transform.position.z);
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
            yDirection = -yDirection;
            bulletBounceCounter--;
        }
    }

    // private IEnumerator DisableAfterWait(int timeInSec)
    // {
    //     yield return new WaitForSeconds(timeInSec);
    //     gameObject.SetActive(false);
    // }

}
