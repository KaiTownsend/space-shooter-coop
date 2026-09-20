using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _shootCooldown = 0.2f;
    private float timePassed;

    private void Update()
    {
        float inputDirection = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(_movementSpeed * Time.deltaTime * inputDirection, 0, 0);

        timePassed += Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && timePassed >= _shootCooldown)
        {
            Shoot();
            timePassed = 0f;
        }   
    }

    private void Shoot()
    {
        Vector3 shootPosition = new Vector3 (transform.position.x, transform.position.y + 0.2f, transform.position.z);
        Instantiate(_projectilePrefab, shootPosition, transform.rotation);
    }
}
