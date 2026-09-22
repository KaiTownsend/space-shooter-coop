using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _damage = 100f;
    [HideInInspector] public float bulletSpread = 0f;
    private float uniqueBulletSpread;

    private void OnEnable()
    {
        uniqueBulletSpread = Random.Range(-bulletSpread, bulletSpread);
        Debug.Log(uniqueBulletSpread);
        StartCoroutine(DisableAfterWait(1));
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x + uniqueBulletSpread * Time.deltaTime, transform.position.y + _speed * Time.deltaTime, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        EnemyController enemyController = collider.gameObject.GetComponent<EnemyController>();

        if (enemyController != null)
        {
            enemyController.TakeDamage(_damage);
            gameObject.SetActive(false);
        }
    }

    private IEnumerator DisableAfterWait(int timeInSec)
    {
        yield return new WaitForSeconds(timeInSec);
        gameObject.SetActive(false);
    }

}
