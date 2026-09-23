using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _bulletContainerPrefab;
    [SerializeField] private Transform _enemySpawnTransform;
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _bulletSize = 3f;
    [SerializeField] private float _bulletSpread = 0.5f;
    [SerializeField] private int _bulletBounces = 0;
    [SerializeField] private float _shootCooldown = 0.25f;
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

        if (Input.GetKey(KeyCode.Space) && _reloadTimer >= _shootCooldown)
        {
            Shoot();
            _reloadTimer = 0f;
        }

        // TEST CODE TO BE REMOVED WHEN I HAVE BULLETS AUTO UPDATE!!!!!!!!!
        if (Input.GetKey(KeyCode.U))
        {
            UpdateBullets();
        }
    }

    private void CreateBullets()
    { 
        GameObject bulletContainerPrefab = Instantiate(_bulletContainerPrefab);

        for (int i = 0; i < _bulletList.Length; i++)
        {
            GameObject bulletPrefab = Instantiate(_bulletPrefab, bulletContainerPrefab.transform);
            _bulletList[i] = bulletPrefab;
        }

        UpdateBullets();
    }

    private void UpdateBullets()
    {
        foreach (GameObject bullet in _bulletList)
        {
            BulletController bulletController = bullet.GetComponent<BulletController>();

            bullet.transform.localScale = new Vector3(_bulletSize, _bulletSize, _bulletSize);
            bulletController.bulletSpread =  _bulletSpread;
            bulletController.bulletBounces = _bulletBounces;
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
