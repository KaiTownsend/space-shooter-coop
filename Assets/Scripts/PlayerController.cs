using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private Transform _enemySpawnTransform;
    public float bulletSizeMultiplier = 1f;
    public float bulletSpreadMultiplier = 1f; // unused atm
    public float shootCooldown = 0.25f;
    private GameObject[] _bulletList;
    private float _reloadTimer;
    private float _clampMin;
    private float _clampMax;
    
    private void Awake()
    {
        _bulletList = new GameObject[100];
    }

    private void Start()
    {
        _clampMin = _enemySpawnTransform.position.x;
        _clampMax = -_enemySpawnTransform.position.x;

        CreateBullets();
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

     private void CreateBullets()
    { 
        for (int i = 0; i < _bulletList.Length; i++)
        {
            Vector3 shootPosition = new Vector3 (transform.position.x, transform.position.y + 0.3f, transform.position.z);
            GameObject bulletPrefab = Instantiate(_bulletPrefab, shootPosition, transform.rotation);
            bulletPrefab.transform.localScale *= bulletSizeMultiplier;

            _bulletList[i] = bulletPrefab;
        }
    }

    private void Shoot()
    {
        foreach (GameObject bullet in _bulletList)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                bullet.transform.position = new Vector3 (transform.position.x, transform.position.y + 0.3f, transform.position.z);
                break;
            }
        }
    }
}
