using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _bulletContainerPrefab;
    [SerializeField] private Transform _enemySpawnTransform;
    
    [Header("Ship Sprites")]
    [SerializeField] private Sprite _maxHPSprite;
    [SerializeField] private Sprite _highHPSprite;
    [SerializeField] private Sprite _mediumHPSprite;
    [SerializeField] private Sprite _lowHPSprite;

    [Header("General Stats")]
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _maxHealth = 100f;

    [Header("Gun/Bullet Stats")]
    [SerializeField] private int _magSize = 1;
    [SerializeField] private float _bulletSize = 3f;
    [SerializeField] private float _bulletSpread = 0.3f;
    [SerializeField] private int _bulletBounces = 0;
    [SerializeField] private float _shootCooldown = 0.25f;

    public float health { get; private set; }
    private SpriteRenderer _spriteRenderer;
    private List<GameObject> _bulletList;
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

        health = _maxHealth;
        _uiManager.UpdateHealthText(_maxHealth);

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

        int availableBulletCount = 0;
        foreach (GameObject bullet in _bulletList)
        {
            if (!bullet.activeInHierarchy)
            {
                availableBulletCount++;
            }
            
        }
        _uiManager.UpdateMagText(availableBulletCount, _magSize);

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
        health -= damageAmt;
        Debug.Log(health);
        UpdateShipState();
        _uiManager.UpdateHealthText(_maxHealth);
    }

    private void UpdateShipState()
    {
        if (health > (0.80 * _maxHealth)) _spriteRenderer.sprite = _maxHPSprite;
        else if (health > (0.6 * _maxHealth)) _spriteRenderer.sprite = _highHPSprite;
        else if (health > (0.3 * _maxHealth)) _spriteRenderer.sprite = _mediumHPSprite;
        else if (health > (0 * _maxHealth)) _spriteRenderer.sprite = _lowHPSprite;
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
