using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private Transform _enemySpawnTransform;
    public float bulletSizeMultiplier = 1f;
    public float bulletSpreadMultiplier = 1f; // unused atm
    public float shootCooldown = 0.25f;
    private float _reloadTimer;
    private float _clampMin;
    private float _clampMax;
    
    

    private void Start()
    {
        _clampMin = _enemySpawnTransform.position.x;
        _clampMax = -_enemySpawnTransform.position.x;
    }

    private void Update()
    {
        float inputDirection = Input.GetAxisRaw("Horizontal");
        transform.position = new Vector3(Mathf.Clamp(transform.position.x + _movementSpeed * Time.deltaTime * inputDirection, _clampMin, _clampMax), transform.position.y, transform.position.z);

        _reloadTimer += Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && _reloadTimer >= shootCooldown)
        {
            Shoot();
            _reloadTimer = 0f;
        }
    }

    private void Shoot()
    {
        Vector3 shootPosition = new Vector3 (transform.position.x, transform.position.y + 0.3f, transform.position.z);
        GameObject bullet = Instantiate(_projectilePrefab, shootPosition, transform.rotation);

        bullet.transform.localScale *= bulletSizeMultiplier;
    }
}
