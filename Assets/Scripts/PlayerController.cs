using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _movementSpeed = 5f;

    private void Update()
    {
        float inputDirection = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(_movementSpeed * Time.deltaTime * inputDirection, 0, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
            Debug.Log("Shoot");
        }
    }

    private void Shoot()
    {
        Vector3 shootPosition = new Vector3 (transform.position.x, transform.position.y + 1, transform.position.z);
        Instantiate(_projectilePrefab, shootPosition, transform.rotation);
    }
}
