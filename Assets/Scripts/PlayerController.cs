using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _bulletContainerPrefab;
    [SerializeField] private Transform _enemySpawnTransform;
    [SerializeField] private Sprite _maxHPSprite;
    [SerializeField] private Sprite _highHPSprite;
    [SerializeField] private Sprite _mediumHPSprite;
    [SerializeField] private Sprite _lowHPSprite;
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private int _magSize = 1;
    [SerializeField] private float _bulletSize = 3f;
    [SerializeField] private float _bulletSpread = 0.3f;
    [SerializeField] private int _bulletBounces = 0;
    [SerializeField] private float _shootCooldown = 0.25f;

    private SpriteRenderer _spriteRenderer;
    private List<GameObject> _bulletList;
    private float _health;
    private float _reloadTimer;
    private float _clampMin;
    private float _clampMax;

    private void Awake()
    {
        _bulletList = new List<GameObject>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _clampMin = _enemySpawnTransform.position.x;
        _clampMax = -_enemySpawnTransform.position.x;
        _health = _maxHealth;

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

        // BOTH METHODS ARE TEST CODE TO BE REMOVED WHEN I HAVE BULLETS AUTO UPDATE!!!!!!!!!
        if (Input.GetKey(KeyCode.U)) // changing basically anything bullet-related except mag size
        {
            UpdateBullets();
        }

        if (Input.GetKey(KeyCode.I)) // for changing mag size
        {
            CreateBullets();
        }
    }

    public void TakeDamage(float damageAmt)
    {
        _health -= damageAmt;
        Debug.Log(_health);
        UpdateShipState();
    }

    private void UpdateShipState()
    {
        if (_health > (0.80 * _maxHealth)) _spriteRenderer.sprite = _maxHPSprite;
        else if (_health > (0.6 * _maxHealth)) _spriteRenderer.sprite = _highHPSprite;
        else if (_health > (0.3 * _maxHealth)) _spriteRenderer.sprite = _mediumHPSprite;
        else if (_health > (0 * _maxHealth)) _spriteRenderer.sprite = _lowHPSprite;
        else
        {
            _spriteRenderer.sprite = _lowHPSprite;
            Debug.Log("Game Over"); // replace with actual game over eventually or something.
        }
    }

    private void CreateBullets()
    {
        GameObject bulletContainerPrefab = Instantiate(_bulletContainerPrefab);

        for (int i = 0; i < (_magSize - _bulletList.Count); i++)
        {
            GameObject bulletPrefab = Instantiate(_bulletPrefab, bulletContainerPrefab.transform);
            _bulletList.Add(bulletPrefab);
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
