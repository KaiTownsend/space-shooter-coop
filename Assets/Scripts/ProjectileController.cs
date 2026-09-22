using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _damage = 100f;

    private void OnEnable()
    {
        StartCoroutine(DisableAfterWait(2));
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
            gameObject.SetActive(false);
        }
    }

    private IEnumerator DisableAfterWait(int timeInSec)
    {
        yield return new WaitForSeconds(timeInSec);
        Debug.Log("wait done");
        gameObject.SetActive(false);
    }

}
